using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using Fiddler;

namespace VisualFormTest
{
    public static class JsonLogger
    {
        public static bool IsLogging
        {
            get
            {
                //JSONを表示しない場合　→　false
                if (!Config.ShowJson) return false;
                //JSONを表示時に記録しない場合　→　false
                if (Config.LoggingDisableOnShowJson) return false;
                //それ以外 →　true
                return true;
            }
        }

        public class SaveApiData
        {
            public string api_name { get; set; }
            public string fullurl { get; set; }
            public int httpcode { get; set; }
            public string request_body { get; set; }
            public string response_body_unescaped { get; set; }
            public List<SaveHttpHeaderItem> request_headers { get; set; }
            public List<SaveHttpHeaderItem> response_headers { get; set; }
            public string response_body { get; set; }

            public string GetJson()
            {
                //Dynamic JSON の挙動がもどかしいので自作
                var sb = new StringBuilder();

                sb.Append("{\"api_name\":");
                sb.Append("\"").Append(api_name).Append("\",");

                sb.Append("\"fullurl\":");
                sb.Append("\"").Append(fullurl).Append("\",");

                sb.Append("\"httpcode\":");
                sb.Append(httpcode).Append(",");

                sb.Append("\"request_body\":");
                sb.Append("\"").Append(request_body).Append("\",");

                sb.Append("\"response_body_unescaped\":");
                sb.Append(response_body_unescaped).Append(",");

                sb.Append("\"request_headers\":");
                sb.Append("[");
                sb.Append(string.Join(",", request_headers.Select(x => x.GetJson())));
                sb.Append("],");

                sb.Append("\"response_headers\":");
                sb.Append("[");
                sb.Append(string.Join(",", response_headers.Select(x => x.GetJson())));
                sb.Append("],");

                sb.Append("\"response_body\":");
                sb.Append("\"").Append(response_body.Replace("\\", "\\\\").Replace("\"", "\\\"")).Append("\"");

                sb.Append("}");
                //
                return sb.ToString();
            }
        }

        public class SaveHttpHeaderItem
        {
            public string name { get; set; }
            public string value { get; set; }

            public string GetJson()
            {
                var sb = new StringBuilder();
                sb.Append("{\"name\":");
                sb.Append("\"").Append(name).Append("\",");

                sb.Append("\"value\":");
                sb.Append("\"").Append(value).Append("\"}");

                return sb.ToString();
            }
        }

        public static void SaveJsonAsync(string requestBody, string decordedResponseBody, Fiddler.Session oSession)
        {
            Task.Factory.StartNew(() =>
                {
                    SaveJson(requestBody, decordedResponseBody, oSession);
                });
        }

        private static void SaveJson(string requestBody, string decordedResponseBody, Fiddler.Session oSession)
        {
            //JSONのオブジェクト作成
            var ojson = new SaveApiData();
            //API名
            var apiname = Regex.Match(oSession.fullUrl, "kcsapi[a-zA-Z0-9_/]+").Value.Replace("kcsapi/", "");
            ojson.api_name = apiname;
            //フルURL
            ojson.fullurl = oSession.fullUrl;
            //HTTP code
            ojson.httpcode = oSession.responseCode;
            //Request Body
            ojson.request_body = requestBody;
            //Responce Body(デコード)
            ojson.response_body_unescaped = decordedResponseBody.Replace("svdata=", "");
            //Request header
            ojson.request_headers = new List<SaveHttpHeaderItem>();
            if (oSession.oRequest.headers != null)
            {
                foreach (var h in oSession.oRequest.headers)
                {
                    var hitem = new SaveHttpHeaderItem();
                    hitem.name = h.Name;
                    hitem.value = h.Value;
                    ojson.request_headers.Add(hitem);
                }
            }
            //Response header
            ojson.response_headers = new List<SaveHttpHeaderItem>();
            if (oSession.oResponse.headers != null)
            {
                foreach (var h in oSession.oResponse.headers)
                {
                    var hitem = new SaveHttpHeaderItem();
                    hitem.name = h.Name;
                    hitem.value = h.Value;
                    ojson.response_headers.Add(hitem);
                }
            }
            //Responce Body(生)
            ojson.response_body = oSession.GetResponseBodyAsString();

            //JSONを取得
            var jsonstr = ojson.GetJson();

            //ファイルに保存
            var date = DateTime.Now;
            var filename = string.Format("{0}_{1}.json", date.ToString("yyyyMMdd_HHmmss_ffff"), apiname.Replace("/", "."));
            var dir = string.Format("user\\json\\{0}\\{1}", date.ToString("yyyyMM"), date.ToString("yyyyMMdd"));

            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.WriteAllText(dir + "\\" + filename, jsonstr, Encoding.GetEncoding("utf-8"));
        }
    }
}
