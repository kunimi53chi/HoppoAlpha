using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KCVDB.Client;

namespace VisualFormTest.KancolleVerifyDb
{
    public class KCVDBApiItem
    {
        public DateTime Date { get; set; }
        public Uri Url { get; set; }
        public int HttpStatusCode { get; set; }
        public KCVDBSendStatus SendStatus { get; set; }
        public int Size { get; set; }
        public Guid TrackingId { get; set; }
        public string ErrorMessage { get; set; }

        public static string[] Header { get; private set; }

        #region コンストラクタ
        static KCVDBApiItem()
        {
            Header = new string[]
            {
                "Date", "URL", "HTTPCode", "SendStatus", "Size(KB)", "TrackingID"
            };
        }

        public KCVDBApiItem() { }

        public KCVDBApiItem(KCVDBMain.QueueData queueData)
        {
            this.Date = DateConverter(queueData.DateHeaderValue);
            this.Url = queueData.RequestUri;
            this.HttpStatusCode = queueData.StatusCode;
            this.SendStatus = KCVDBSendStatus.Queuing;
        }
        #endregion

        #region インスタンスの作成
        public static IEnumerable<KCVDBApiItem> CreateInstance(FatalErrorEventArgs fatalArgs)
        {
            var item = new KCVDBApiItem();
            item.Date = DateTime.Now;
            item.SendStatus = KCVDBSendStatus.FatalError;
            item.ErrorMessage = fatalArgs.Message;

            yield return item;
        }

        public static IEnumerable<KCVDBApiItem> CreateInstance(SendingErrorEventArgs sendingErrorArgs)
        {
            if (sendingErrorArgs.ApiData == null || sendingErrorArgs.TrackngIds == null) yield break;

            foreach (var i in Enumerable.Range(0, Math.Min(sendingErrorArgs.ApiData.Length, sendingErrorArgs.TrackngIds.Length)))
            {
                var item = new KCVDBApiItem();

                if (sendingErrorArgs.ApiData[i] != null)
                {
                    item.Date = DateConverter(sendingErrorArgs.ApiData[i].HttpDateHeaderValue);
                    item.Url = sendingErrorArgs.ApiData[i].RequestUri;
                    item.HttpStatusCode = sendingErrorArgs.ApiData[i].StatusCode;
                }
                item.SendStatus = KCVDBSendStatus.SendingError;
                item.TrackingId = sendingErrorArgs.TrackngIds[i];
                item.ErrorMessage = sendingErrorArgs.Message;

                yield return item;
            }
        }

        public static IEnumerable<KCVDBApiItem> CreateInstance(InternalErrorEventArgs internalErrorArgs)
        {
            if (internalErrorArgs.ApiData == null || internalErrorArgs.TrackingIds == null) yield break;

            foreach (var i in Enumerable.Range(0, Math.Min(internalErrorArgs.ApiData.Length, internalErrorArgs.TrackingIds.Length)))
            {
                var item = new KCVDBApiItem();

                if (internalErrorArgs.ApiData[i] != null)
                {
                    item.Date = DateConverter(internalErrorArgs.ApiData[i].HttpDateHeaderValue);
                    item.Url = internalErrorArgs.ApiData[i].RequestUri;
                    item.HttpStatusCode = internalErrorArgs.ApiData[i].StatusCode;
                }
                item.SendStatus = KCVDBSendStatus.InternalError;
                item.TrackingId = internalErrorArgs.TrackingIds[i];
                item.ErrorMessage = internalErrorArgs.Message;

                yield return item;
            }
        }

        public static IEnumerable<KCVDBApiItem> CreateInstance(ApiDataSentEventArgs sentArgs)
        {
            if (sentArgs.ApiData == null || sentArgs.TrackingIds == null) yield break;

            var n = Math.Min(sentArgs.ApiData.Length, sentArgs.TrackingIds.Length);
            foreach (var i in Enumerable.Range(0, n))
            {
                var item = new KCVDBApiItem();

                if (sentArgs.ApiData[i] != null)
                {
                    item.Date = DateConverter(sentArgs.ApiData[i].HttpDateHeaderValue);
                    item.Url = sentArgs.ApiData[i].RequestUri;
                    item.HttpStatusCode = sentArgs.ApiData[i].StatusCode;
                }
                item.SendStatus = KCVDBSendStatus.Success;
                item.TrackingId = sentArgs.TrackingIds[i];
                if (i == n - 1)
                {
                    item.Size = sentArgs.SentApiData.PayloadByteCount / 1024;//最後のやつだけ値を与える、それ以外は0KBとみなす
                }

                yield return item;
            }
        }

        public static IEnumerable<KCVDBApiItem> CreateInstance(ApiDataSendingEventArgs sendingArgs)
        {
            if (sendingArgs.ApiData == null || sendingArgs.TrackingIds == null) yield break;

            foreach (var i in Enumerable.Range(0, Math.Min(sendingArgs.ApiData.Length, sendingArgs.TrackingIds.Length)))
            {
                var item = new KCVDBApiItem();

                if (sendingArgs.ApiData[i] != null)
                {
                    item.Date = DateConverter(sendingArgs.ApiData[i].HttpDateHeaderValue);
                    item.Url = sendingArgs.ApiData[i].RequestUri;
                    item.HttpStatusCode = sendingArgs.ApiData[i].StatusCode;
                }
                item.SendStatus = KCVDBSendStatus.Sending;
                item.TrackingId = sendingArgs.TrackingIds[i];

                yield return item;
            }
        }
        #endregion

        public string ToLogText()
        {
            var sb = new StringBuilder();
            //FatalErrorの場合のみ
            if(this.SendStatus == KCVDBSendStatus.FatalError)
            {
                sb.AppendLine(this.ErrorMessage);
                return sb.ToString();
            }

            //共通行
            sb.Append(Date.ToString()).Append("\t");
            if(Url != null) sb.Append(Url.AbsoluteUri).Append("\t");
            sb.Append(HttpStatusCode).Append("\t");
            sb.Append(SendStatus.ToString()).Append("\t");
            sb.Append(Size.ToString("N0")).Append("\t");
            sb.AppendLine(TrackingId.ToString());
            
            //エラーの場合はスタックトレース
            switch(this.SendStatus)
            {
                case KCVDBSendStatus.InternalError:
                case KCVDBSendStatus.SendingError:
                    sb.AppendLine(this.ErrorMessage);
                    break;
            }

            return sb.ToString();
        }

        private static DateTime DateConverter(string rfc1123Date)
        {
            DateTime date;
            //パースできればサーバー時間
            if (DateTime.TryParse(rfc1123Date, out date)) return date;
            //不可能ならクライアント時間
            else return DateTime.Now;
        }
    }

    public enum KCVDBSendStatus
    {
        None, Queuing, Sending, Success, FatalError, SendingError, InternalError,
    }
}
