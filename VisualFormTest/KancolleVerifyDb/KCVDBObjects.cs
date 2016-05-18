using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Threading.Tasks;

namespace VisualFormTest.KancolleVerifyDb
{
    public class KCVDBObjects
    {
        //--ログファイル
        public static StringBuilder LogText { get; private set; }
        //--ログ画面
        //再更新が必要かどうか
        public static bool ScreenRefreshRequired { get; set; }
        //概況
        public static Guid SessionID { get; private set; }
        public static string AgentID { get; private set; }
        public static DateTime SessionStarted { get; set; }
        public static int SendApis { get; private set; }
        public static int SendSuccess { get; private set; }
        public static int SendFailure { get; private set; }
        public static long TotalKBytes { get; private set; }
        public static int[] PerHourKBytes { get; private set; }

        private static DateTime LastApiSendDate { get; set; }
        //API履歴
        public static Queue<KCVDBApiItem> ApiHistory { get; private set; }
        const int ApiHistoryLength = 7;

        //閉塞
        private static AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
        //キューに入っている個数
        private static int numOfApiQueue = 0;

        //コンストラクタ
        static KCVDBObjects()
        {
            //ログファイル
            LogText = new StringBuilder();
            //ログ画面
            PerHourKBytes = new int[60];
            ApiHistory = new Queue<KCVDBApiItem>();
        }

        //クライアント初期化
        public static void InitClient(Guid sessionId, string agentId)
        {
            _autoResetEvent.WaitOne();

            //概況
            SessionID = sessionId;
            AgentID = agentId;
            SessionStarted = DateTime.Now;
            SendApis = 0; SendSuccess = 0; SendFailure = 0;
            TotalKBytes = 0;
            PerHourKBytes = new int[60];
            //履歴
            ApiHistory = new Queue<KCVDBApiItem>();
            foreach (int i in Enumerable.Range(0, ApiHistoryLength)) ApiHistory.Enqueue(new KCVDBApiItem());
            //ログ
            AppendLog("Initialize Client!(AgentID = " + agentId + ", SessionID = " + sessionId.ToString() + ")");

            numOfApiQueue = 0;

            //再描画
            ScreenRefreshRequired = true;

            _autoResetEvent.Set();
        }

        //API追加
        public static void SendingAPI(IEnumerable<KCVDBApiItem> items)
        {
            _autoResetEvent.WaitOne();

            foreach (var item in items)
            {
                //概況
                SendApis++;
                //履歴
                KCVDBApiItem queue = null;
                if (numOfApiQueue > 0)
                {
                    foreach (var h in ApiHistory)
                    {
                        if (h.Url == item.Url) queue = h;
                    }
                }
                if (queue != null)
                {
                    //キューが消化された場合
                    queue.SendStatus = KCVDBSendStatus.Sending;
                    numOfApiQueue--;
                }
                else
                {
                    ApiHistory.Enqueue(item);
                    ApiHistory.Dequeue();
                }
            }
            //再描画
            ScreenRefreshRequired = true;

            _autoResetEvent.Set();
        }

        //Queueに追加
        public static void QueuingAPI(KCVDBApiItem item)
        {
            _autoResetEvent.WaitOne();

            //履歴
            ApiHistory.Enqueue(item);
            ApiHistory.Dequeue();
            numOfApiQueue++;
            
            //再描画
            ScreenRefreshRequired = true;

            _autoResetEvent.Set();
        }

        //送信完了
        public static void SentDoneAPI(IEnumerable<KCVDBApiItem> items)
        {
            _autoResetEvent.WaitOne();

            foreach (var item in items)
            {
                //--概況
                //成功失敗
                switch (item.SendStatus)
                {
                    case KCVDBSendStatus.Success:
                        SendSuccess++;
                        break;
                    case KCVDBSendStatus.FatalError:
                    case KCVDBSendStatus.InternalError:
                    case KCVDBSendStatus.SendingError:
                        SendFailure++;
                        break;
                }
                //合計容量
                TotalKBytes += item.Size;
                //時間あたり容量
                var lastIndex = GetRingBufferIndex(LastApiSendDate);
                var nowIndex = GetRingBufferIndex(item.Date);
                var stripItemDate = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, item.Date.Hour, item.Date.Minute, 0);
                var indexElapsed = Math.Abs((int)Math.Min((stripItemDate - LastApiSendDate).TotalMinutes, PerHourKBytes.Length));
                foreach(var i in Enumerable.Range(lastIndex+1, indexElapsed))
                {
                    //インデックスが移動した分の初期化
                    PerHourKBytes[i % PerHourKBytes.Length] = 0;
                }
                PerHourKBytes[nowIndex] += item.Size;
                LastApiSendDate = stripItemDate;

                //--履歴
                foreach (var h in ApiHistory)
                {
                    if (h.TrackingId == item.TrackingId)
                    {
                        h.SendStatus = item.SendStatus;
                        h.ErrorMessage = item.ErrorMessage;
                        h.Size = item.Size;

                        break;
                    }
                }

                //--ログに追加
                LogText.Append(item.ToLogText());
            }

            //再描画
            ScreenRefreshRequired = true;

            _autoResetEvent.Set();
        }

        //ビュー側からの閉塞操作
        public static void AutoResetEventViewSwitcher(bool isend)
        {
            if (isend) _autoResetEvent.Set();
            else _autoResetEvent.WaitOne();
        }

        //ログにメッセージを追加
        private static void AppendLog(string str)
        {
            string message = DateTime.Now.ToString() + "\t" + str;
            LogText.AppendLine(message);
        }

        //リングバッファのインデックスの計算
        private static int GetRingBufferIndex(DateTime date)
        {
            return Math.Max(Math.Min(date.Minute, 59), 0);
        }

        //ログの保存
        public static void SaveDbErrorLog()
        {
            if (LogText.Length == 0) return;
            if (APIPort.Basic == null) return;

            _autoResetEvent.WaitOne();

            string dir = "user\\" + APIPort.Basic.api_member_id + "\\kcvdb";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            //kcvdblog20160326.txt
            string filename = dir + "\\kcvdblog" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            Encoding enc = Encoding.GetEncoding("shift_jis");

            if (!File.Exists(filename))
            {
                var sb = new StringBuilder();
                sb.Append(string.Join("\t", KCVDBApiItem.Header)).AppendLine();
                var text = LogText.ToString();
                if (!text.Contains("Initialize Client"))
                {
                    sb.AppendFormat("\t\tInitialize Client!(from past day)(AgentID = {0}, SessionID = {1})", AgentID, SessionID.ToString()).AppendLine();
                }
                sb.Append(text);

                File.WriteAllText(filename, sb.ToString(), enc);
            }
            else
            {
                File.AppendAllText(filename, LogText.ToString(), enc);
            }

            LogText = new StringBuilder();

            _autoResetEvent.Set();
        }
    }
}
