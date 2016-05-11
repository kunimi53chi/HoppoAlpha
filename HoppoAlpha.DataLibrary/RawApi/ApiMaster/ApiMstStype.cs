using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace HoppoAlpha.DataLibrary.RawApi.ApiMaster
{
    public class ApiMstStype
    {
        public int api_id { get; set; }
        public int api_sortno { get; set; }
        public string api_name { get; set; }
        public int api_scnt { get; set; }
        public int api_kcnt { get; set; }
        public BigInteger api_equip_type { get; set; } //多倍長変数のビットフラグに変える

        private bool? _isAirCombatable = null;

        public bool IsAirCombatable
        {
            /*
                    case 6:
                    case 7:
                    case 8:
                    case 11:
                        return true;
             * */
            get
            {
                //キャッシュされていない場合
                if(_isAirCombatable == null)
                {
                    int[] flagarray = new int[] { 1 << 5, 1 << 6, 1 << 7, 1 << 10 };//api_type[2]が6,7,8,11のフラグ
                    bool f = false;
                    foreach(var target in flagarray)
                    {
                        f = f || (api_equip_type & target) == target;
                        if (f) break;
                    }
                    _isAirCombatable = f;
                }
                return (bool)_isAirCombatable;
            }
        }

        public BigInteger SetBitflag(string json)
        {
            /*
             * {"1":0,"2":0,"3":0,"4":0,"5":0,"6":0,"7":0,"8":0,"9":0,"10":0,
             * "11":0,"12":0,"13":0,"14":0,"15":0,"16":0,"17":0,"18":0,"19":0,
             * "20":0,"21":0,"22":0,"23":0,"24":0,"25":0,"26":0,"27":0,"28":0,
             * "29":0,"30":0,"31":0,"32":0,"33":0,"34":0,"35":0}
             * →　これをビットフラグに変える
             *
             */
            string equipstr = json.Replace("\"", "-");
            char[] reg_equiptype = System.Text.RegularExpressions.Regex.Replace(equipstr,
                @"}|(?:{?,?-[0-9]{1,2}-:)", "").ToCharArray();
            Array.Reverse(reg_equiptype);
            
            //2進数→BigIntegerに変換
            BigInteger x = 0;
            foreach(char c in reg_equiptype)
            {
                x = ((c == '1' ? 1 : 0) | x) << 1;
            }
            x = x >> 1;

            //修正
            this.api_equip_type = x; _isAirCombatable = null;
            return x;
        }
    }
}
