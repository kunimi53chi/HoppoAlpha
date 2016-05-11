using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using KCVDB.Client;

namespace VisualFormTest.KancolleVerifyDb
{
    public static class KCVDBMain
    {
        //送信用クライアント
        static IKCVDBClient client;
        //提督IDが取れるまでのキュー
        static Queue<QueueData> queueInitial = new Queue<QueueData>();

        #region キュー用のクラス
        public class QueueData
        {
            public Uri RequestUri{get; set;}
            public int StatusCode {get; set;}
            public string RequestBody {get; set;}
            public string ResponseBody {get; set;}
            public string DateHeaderValue {get; set;}

            public QueueData(Uri requestUri, int statusCode, string requestBody, string responseBody, string dateHeaderValue)
            {
                this.RequestUri = requestUri;
                this.StatusCode = statusCode;
                this.RequestBody = requestBody;
                this.ResponseBody = responseBody;
                this.DateHeaderValue = dateHeaderValue;
            }
        }
        #endregion

        private static void Init()
        {
            //クライアント初期化
            var agentId = string.Format("ほっぽアルファ_{0}_{1}", APIPort.Basic.api_member_id, APIPort.Basic.api_nickname);
            var sessionId = Guid.NewGuid();
            client = KCVDBClientService.Instance.CreateClient(agentId, sessionId.ToString());
            KCVDBObjects.InitClient(sessionId, agentId);

            //イベントハンドラ
            client.FatalError += (_, e) =>
            {
                KCVDBObjects.SentDoneAPI(KCVDBApiItem.CreateInstance(e));
            };

            client.SendingError += (_, e) =>
            {
                KCVDBObjects.SentDoneAPI(KCVDBApiItem.CreateInstance(e));
            };

            client.InternalError += (_, e) =>
            {
                KCVDBObjects.SentDoneAPI(KCVDBApiItem.CreateInstance(e));
            };

            client.ApiDataSent += (_, data) =>
            {
                KCVDBObjects.SentDoneAPI(KCVDBApiItem.CreateInstance(data));
            };
            client.ApiDataSending += (_, data) =>
            {
                KCVDBObjects.SendingAPI(KCVDBApiItem.CreateInstance(data));
            };
        }

        public static bool IsCapture
        {
            get
            {
                //検証DBがオープンしてなければ
                if (!Config.KancolleVerifyDbActivate) return false;
                //送信フラグがオンでなければ→false
                if (!Config.KancolleVerifyPostEnable) return false;
                //他条件

                //全てを満たしたとき→true
                return true;
            }
        }

        public static bool IsNotifyDialogShow
        {
            get
            {
                //検証DBがオープンしてなければ→false
                if (!Config.KancolleVerifyDbActivate) return false;
                //ダイアログを表示しなければ→false
                if (Config.KancolleVerifyNotifyDialogNotShow) return false;
                //送信フラグがオンになってれば→false
                if (Config.KancolleVerifyPostEnable) return false;

                return true;
            }
        }

        public static async Task PostServerAsync(Fiddler.Session oSession)
        {
            if (oSession.PathAndQuery.StartsWith("/kcsapi") &&
                oSession.oResponse.MIMEType.Equals("text/plain"))
            {
                try
                {
                    var postUri = new Uri(oSession.fullUrl);
                    string requestBody = HttpUtility.HtmlDecode(oSession.GetRequestBodyAsString());
                    var postRequestBody = Regex.Replace(requestBody, @"&api(_|%5F)token=[0-9a-f]+|api(_|%5F)token=[0-9a-f]+&?", "");
                    var postResponseBody = oSession.GetResponseBodyAsString();
                    var postStatusCode = oSession.responseCode;

                    string httpDate = "";
                    if (oSession.oResponse.headers == null) return;
                    foreach(var h in oSession.oResponse.headers)
                    {
                        if(h.Name == "Date")
                        {
                            httpDate = h.Value;
                            break;
                        }
                    }

                    //提督IDが取得できていない場合
                    if(APIPort.Basic == null || APIPort.Basic.api_nickname == null)
                    {
                        //キューに追加
                        var queueitem = new QueueData(postUri, postStatusCode, postRequestBody, postResponseBody, httpDate);
                        queueInitial.Enqueue(queueitem);
                        KCVDBObjects.QueuingAPI(new KCVDBApiItem(queueitem));
                    }
                    else
                    {
                        //クライアントの初期化
                        if(client == null) Init();

                        //キューにデータがある場合
                        while(queueInitial.Count > 0)
                        {
                            //キューから取り出す
                            var dequeue = queueInitial.Dequeue();
                            //送信
                            await client.SendRequestDataAsync(dequeue.RequestUri, dequeue.StatusCode, dequeue.RequestBody, dequeue.ResponseBody, dequeue.DateHeaderValue);
                        }

                        //もらったAPIを送信
                        await client.SendRequestDataAsync(postUri, postStatusCode, postRequestBody, postResponseBody, httpDate);
                    }
                }
                catch(Exception)
                {
                }
            }
        }
    }
}
