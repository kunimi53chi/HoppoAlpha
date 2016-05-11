using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary.RawApi.ApiReqPractice
{
    public class ApiPractice
    {
        public int api_member_id { get; set; }
        public int api_id { get; set; }
        public int api_enemy_id { get; set; }
        public string api_enemy_name { get; set; }
        public string api_enemy_name_id { get; set; }
        public int api_enemy_level { get; set; }
        public string api_enemy_rank { get; set; }
        public int api_enemy_flag { get; set; }
        public int api_enemy_flag_ship { get; set; }
        public string api_enemy_comment { get; set; }
        public string api_enemy_comment_id { get; set; }
        public int api_state { get; set; }

        public string StateString
        {
            get
            {
                switch (this.api_state)
                {
                    case 0: return "未プレイ";
                    case 1: return "E敗北";
                    case 2: return "D敗北";
                    case 3: return "C敗北";
                    case 4: return "B勝利";
                    case 5: return "A勝利";
                    case 6: return "S勝利";
                    default: return "";
                }
            }
        }

        //表示用
        public string Display()
        {
            if (this.api_id == 0) return "未取得";
            //取得
            return string.Format("{0} / Lv{1} / {2}", this.api_enemy_name, this.api_enemy_level, this.StateString);
        }


        //Stateを返す
        public static int GetResultState(string rank)
        {
            switch (rank)
            {
                case "S": return 6;
                case "A": return 5;
                case "B": return 4;
                case "C": return 3;
                case "D": return 2;
                case "E": return 1;
                default: return 0;
            }
        }
    }
}
