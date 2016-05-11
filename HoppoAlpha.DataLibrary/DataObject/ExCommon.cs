using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// マスターデータを拡張する際に共通の処理
    /// </summary>
    public static class ExCommon
    {
        /// <summary>
        /// 値の種類
        /// </summary>
        public enum ValueType
        {
            Int, ListInt, String,
        }
        /// <summary>
        /// NULLのときの代入文字列
        /// </summary>
        public static readonly string NullString = "null";

        /// <summary>
        /// テキスト→それぞれのタイプにコンバート
        /// </summary>
        /// <param name="value">CSVのコンバーター</param>
        /// <param name="type">タイプ指定</param>
        /// <param name="arraylength">配列の要素数</param>
        /// <param name="defaultvalue">初期値（デフォルトで0）</param>
        /// <returns></returns>
        public static dynamic ValueConverter(string value, ValueType type, int arraylength, int defaultvalue = 0)
        {

            switch (type)
            {
                case ValueType.Int:
                    int intval;
                    if (int.TryParse(value, out intval)) return intval;
                    else return 0;
                case ValueType.ListInt:
                    if (value == NullString) return null;

                    string[] separate = value.Replace("[", "").Replace("]", "").Split(',');
                    List<int> list = new List<int>();

                    int cnt = 0;
                    foreach (var x in separate)
                    {
                        int y;
                        cnt++;
                        if (int.TryParse(x, out y)) list.Add(y);
                    }

                    //何も入っていない場合
                    if (cnt == 0) return null;

                    //部分的には入っている場合
                    for (int i = cnt; i < arraylength; i++)
                    {
                        list.Add(defaultvalue);//初期値で埋める
                    }
                    return list;
                case ValueType.String:
                    if (value == NullString) return null;
                    return SafeStringImport(value);
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// 半角ダブルクオーテーションが入っている用対策
        /// </summary>
        /// <param name="original">オリジナルの文字列</param>
        /// <returns>半角ダブルクオーテーションを全角に変換した文字列</returns>
        public static string SafeStringImport(string original)
        {
            if (original == null) return null;
            return original.Replace("\"", "”");//半角ダブルクオーテーションを全角に
        }

        /// <summary>
        /// リスト→文字列への変換
        /// </summary>
        /// <param name="list">リスト</param>
        /// <returns>文字列</returns>
        public static string ListToCsvString(List<int> list)
        {
            if (list == null) return ExCommon.NullString;

            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(string.Join(",", list));
            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// マスターデータを静的変数にセットします
        /// </summary>
        /// <param name="exMasterShipCollection"></param>
        /// <param name="masterStypes"></param>
        /// <param name="exMasterSlotitems"></param>
        /// <param name="masterSlotitemEquiptypes"></param>
        public static void SetMasterData(ExMasterShipCollection exMasterShipCollection, Dictionary<int, ApiMstStype> masterStypes,
            ExMasterSlotitemCollection exMasterSlotitems, List<ApiMstSlotitemEquiptype> masterSlotitemEquiptypes)
        {
            HoppoAlpha.DataLibrary.RawApi.ApiPort.ApiShip.MasterShips = exMasterShipCollection;
            HoppoAlpha.DataLibrary.RawApi.ApiPort.ApiShip.MasterSTypes = masterStypes;
            HoppoAlpha.DataLibrary.RawApi.ApiPort.ApiShip.MasterSlotitems = exMasterSlotitems;
            HoppoAlpha.DataLibrary.RawApi.ApiPort.ApiShip.MasterSlotitemEquipTypes = masterSlotitemEquiptypes;

            HoppoAlpha.DataLibrary.RawApi.ApiGetMember.SlotItem.MasterSlotitems = exMasterSlotitems;
            HoppoAlpha.DataLibrary.RawApi.ApiGetMember.SlotItem.MasterSlotitemEquipTypes = masterSlotitemEquiptypes;
        }

    }
}
