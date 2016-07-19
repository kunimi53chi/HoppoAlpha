using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace HoppoAlpha.DataLibrary.RawApi.ApiReqQuest
{
    /// <summary>
    /// 任務を記録するクラス
    /// </summary>
    [ProtoContract]
    public class ApiQuest
    {
        /// <summary>
        /// 任務IDを表します
        /// </summary>
        [ProtoMember(1)]
        public int api_no { get; set; }
        /// <summary>
        /// 任務のカテゴリーを表します
        /// </summary>
        [ProtoMember(2)]
        public int api_category { get; set; }
        [ProtoMember(3)]
        public int api_type { get; set; }
        /// <summary>
        /// 任務の受注状況を表します
        /// </summary>
        [ProtoMember(4)]
        public int api_state { get; set; }
        /// <summary>
        /// 任務名を表します
        /// </summary>
        [ProtoMember(5)]
        public string api_title { get; set; }
        /// <summary>
        /// 任務の説明を表します
        /// </summary>
        [ProtoMember(6)]
        public string api_detail { get; set; }
        /// <summary>
        /// 任務の獲得資源を表します
        /// </summary>
        [ProtoMember(7)]
        public List<int> api_get_material { get; set; }
        /// <summary>
        /// 任務で追加獲得アイテムがあるかどうかを表します
        /// </summary>
        [ProtoMember(8)]
        public int api_bonus_flag { get; set; }
        /// <summary>
        /// 任務の進捗度を表します
        /// </summary>
        [ProtoMember(9)]
        public int api_progress_flag { get; set; }
        [ProtoMember(10)]
        public int api_invalid_flag { get; set; }

        /// <summary>
        /// 任務と進捗度を表示します
        /// </summary>
        /// <returns>任務名(進捗度)</returns>
        public string Display()
        {
            //受注と進捗
            string prog = "";
            if (this.api_state == 3) prog = "完了";
            else if (this.api_state == 2)
            {
                switch (this.api_progress_flag)
                {
                    case 0:
                        prog = "0%"; break;
                    case 1:
                        prog = "50%"; break;
                    case 2:
                        prog = "80%"; break;
                }
            }
            //出現タイプ
            switch(this.api_type)
            {
                case 1://デイリー
                    prog = prog + ":D";
                    break;
                case 2://ウィークリー
                    prog = prog + ":W";
                    break;
                case 3://マンスリー
                    prog = prog + ":M";
                    break;
                case 5://クォタリー
                    prog = prog + ":Q";
                    break;
            }

            return string.Format("{0}({1})", this.api_title, prog);
        }

        /// <summary>
        /// 任務名を短縮して進捗度と表示します(タイトルバー用)
        /// </summary>
        /// <returns>任務名[12文字](進捗度)</returns>
        public string DisplayShort()
        {
            //受注と進捗
            string prog = "";
            if (this.api_state == 3) prog = "完了";
            else if (this.api_state == 2)
            {
                switch (this.api_progress_flag)
                {
                    case 0:
                        prog = "0%"; break;
                    case 1:
                        prog = "50%"; break;
                    case 2:
                        prog = "80%"; break;
                }
            }
            //出現タイプ
            switch (this.api_type)
            {
                case 1://デイリー
                    prog = prog + ":D";
                    break;
                case 2://ウィークリー
                    prog = prog + ":W";
                    break;
                case 3://マンスリー
                    prog = prog + ":M";
                    break;
                case 5://クォタリー
                    prog = prog + ":Q";
                    break;
            }

            //タイトルを短くする
            string title;
            if (this.api_title.Length >= 13)
            {
                title = this.api_title.Substring(0, 12);
                title += "…";
            }
            else
            {
                title = this.api_title;
            }
            return string.Format("{0}({1})", title, prog);
        }
    }
}
