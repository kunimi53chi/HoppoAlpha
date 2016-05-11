using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;

namespace VisualFormTest.KancolleDb
{
    public static class KancolleDbPost
    {
        public enum UrlType
        {
            PORT,
            SHIP2,
            SHIP3,
            SLOT_ITEM,
            KDOCK,
            MAPINFO,
            CHANGE,
            CREATESHIP,
            GETSHIP,
            CREATEITEM,
            START,
            NEXT,
            SELECT_EVENTMAP_RANK,
            BATTLE,
            BATTLE_MIDNIGHT,
            BATTLE_SP_MIDNIGHT,
            BATTLE_NIGHT_TO_DAY,
            BATTLERESULT,
            COMBINED_BATTLE,
            COMBINED_BATTLE_AIR,
            COMBINED_BATTLE_MIDNIGHT,
            COMBINED_BATTLE_RESULT,
            AIRBATTLE,
            COMBINED_BATTLE_WATER,
            COMBINED_BATTLE_SP_MIDNIGHT,
            //MASTER,
        };

        private static Dictionary<UrlType, string> urls = new Dictionary<UrlType, string>()
        {
            { UrlType.PORT,                     "api_port/port"                       },
            { UrlType.SHIP2,                    "api_get_member/ship2"                },
            { UrlType.SHIP3,                    "api_get_member/ship3"                },
            { UrlType.SLOT_ITEM,                "api_get_member/slot_item"            },
            { UrlType.KDOCK,                    "api_get_member/kdock"                },
            { UrlType.MAPINFO,                  "api_get_member/mapinfo"              },
            { UrlType.CHANGE,                   "api_req_hensei/change"               },
            { UrlType.CREATESHIP,               "api_req_kousyou/createship"          },
            { UrlType.GETSHIP,                  "api_req_kousyou/getship"             },
            { UrlType.CREATEITEM,               "api_req_kousyou/createitem"          },
            { UrlType.START,                    "api_req_map/start"                   },
            { UrlType.NEXT,                     "api_req_map/next"                    },
			{ UrlType.SELECT_EVENTMAP_RANK,     "api_req_map/select_eventmap_rank"    }, 
            { UrlType.BATTLE,                   "api_req_sortie/battle"               },
            { UrlType.BATTLE_MIDNIGHT,          "api_req_battle_midnight/battle"      },
            { UrlType.BATTLE_SP_MIDNIGHT,       "api_req_battle_midnight/sp_midnight" },
            { UrlType.BATTLE_NIGHT_TO_DAY,      "api_req_sortie/night_to_day"         },
            { UrlType.BATTLERESULT,             "api_req_sortie/battleresult"         },
            { UrlType.COMBINED_BATTLE,          "api_req_combined_battle/battle"      },
            { UrlType.COMBINED_BATTLE_AIR,      "api_req_combined_battle/airbattle"   },
            { UrlType.COMBINED_BATTLE_MIDNIGHT, "api_req_combined_battle/midnight_battle"},
            { UrlType.COMBINED_BATTLE_RESULT,   "api_req_combined_battle/battleresult"},
            { UrlType.AIRBATTLE,                "api_req_sortie/airbattle"            },
            { UrlType.COMBINED_BATTLE_WATER,    "api_req_combined_battle/battle_water"},
            { UrlType.COMBINED_BATTLE_SP_MIDNIGHT,"api_req_combined_battle/sp_midnight"},
			//{ UrlType.MASTER,                   "api_start2"                          },
        };

        public static bool IsCapture
        {
            get
            {
                //キャプチャーのフラグがFlase→false
                if (Config.KancolleDbPostDisable) return false;
                //プロキシモードで送信しないがON かつ プロキシモード→false
                if (!Config.KancolleDbPostOnProxyMode && Config.ListeningMode) return false;
                //ユーザートークンがNullorEmpty → false
                if (string.IsNullOrEmpty(Config.KancolleDbUserToken)) return false;
                //全てを満たしたときに → true
                return true;
            }
        }

        //private bool isCapture = false;

        public static void FiddlerApplication_AfterSessionComplete(Fiddler.Session oSession)
        {
            if (oSession.PathAndQuery.StartsWith("/kcsapi") &&
                oSession.oResponse.MIMEType.Equals("text/plain"))
            {
                Task.Factory.StartNew(() =>
                {
                    string url = oSession.fullUrl;
                    foreach (KeyValuePair<UrlType, string> kvp in urls)
                    {
                        if (url.IndexOf(kvp.Value) > 0)
                        {
                            //string responseBody = oSession.GetResponseBodyAsString();
                            //responseBody.Replace("svdata=", "");

                            //string str = "Post server from " + url + "\n";
                            //AppendText(str);

                            string res = PostServer(oSession);
                            //str = "Post response : " + res + "\n";
                            //AppendText(str);

                            //システムログに記載（エラー時のみ）
                            if (res != "200: ") LogSystem.AddLogMessage(res);

                            return;
                        }
                    }
                });
            }
        }

        private static string PostServer(Fiddler.Session oSession)
        {
            string token = Config.KancolleDbUserToken;                   // TODO: ユーザー毎のトークンを設定
            string agent = ApiKeys.ApiKeys.KancolleDbAgentId;          // TODO: アプリ毎のトークンを設定
            string url = oSession.fullUrl;
            string requestBody = HttpUtility.HtmlDecode(oSession.GetRequestBodyAsString());
            requestBody = Regex.Replace(requestBody, @"&api(_|%5F)token=[0-9a-f]+|api(_|%5F)token=[0-9a-f]+&?", "");	// api_tokenを送信しないように削除
            string responseBody = oSession.GetResponseBodyAsString();
            responseBody.Replace("svdata=", "");

            try
            {
                WebRequest req = WebRequest.Create("http://api.kancolle-db.net/2/");
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                string postdata =
                      "token=" + HttpUtility.UrlEncode(token) + "&"
                    + "agent=" + HttpUtility.UrlEncode(agent) + "&"
                    + "url=" + HttpUtility.UrlEncode(url) + "&"
                    + "requestbody=" + HttpUtility.UrlEncode(requestBody) + "&"
                    + "responsebody=" + HttpUtility.UrlEncode(responseBody);
                byte[] postDataBytes = System.Text.Encoding.ASCII.GetBytes(postdata);
                req.ContentLength = postDataBytes.Length;

                Stream reqStream = req.GetRequestStream();
                reqStream.Write(postDataBytes, 0, postDataBytes.Length);
                reqStream.Close();

                WebResponse res = req.GetResponse();
                HttpWebResponse httpRes = (HttpWebResponse)res;
                Stream resStream = res.GetResponseStream();
                StreamReader sr = new StreamReader(resStream, enc);
                string response = sr.ReadToEnd();
                sr.Close();
                return oSession.responseCode + ": " + response;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse error = (HttpWebResponse)ex.Response;
                    return error.ResponseUri + " " + oSession.responseCode + ": " + error.StatusDescription;
                }
                return ex.Message;
            }
        }
    }
}
