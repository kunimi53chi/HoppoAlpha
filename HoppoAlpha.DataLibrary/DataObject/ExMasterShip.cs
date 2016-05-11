using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using HoppoAlpha.DataLibrary.Const;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// 船のマスターデータの拡張クラス（初期装備との統合）
    /// </summary>
    public class ExMasterShip : ApiMstShip
    {
        //アプデで外れたパラメーター
        /// <summary>
        /// 回避を表します
        /// </summary>
        public List<int> Evation { get; set; }
        /// <summary>
        /// 対潜を表します
        /// </summary>
        public List<int> AntiSub { get; set; }
        /// <summary>
        /// 索敵を表します
        /// </summary>
        public List<int> Search { get; set; }
        /// <summary>
        /// 初期装備を表します
        /// </summary>
        public List<int> DefaultSlotItem { get; set; }

        //便宜上入れておくパラメーター
        /// <summary>
        /// 制空値を表します
        /// </summary>
        public int AirSuperiority { get; set; }
        /// <summary>
        /// 加重対空値を表します
        /// </summary>
        public int WeightedAntiAir { get; set; }

        //計算用に使うプロパティ
        /// <summary>
        /// 空母かどうかを表します
        /// </summary>
        public bool IsCarrier
        {
            get
            {
                switch(this.api_stype)
                {
                    case 7://正規空母
                    case 11://軽空母
                    case 18://装甲空母
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// 補給艦かどうかを表します
        /// </summary>
        public bool IsSupplier
        {
            get
            {
                return this.api_stype == 15;//補給艦
            }
        }

        public bool IsEnemyShip
        {
            get
            {
                switch(this.api_yomi)
                {
                    case "":
                    case "-":
                    case "elite":
                    case "flagship":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// CSV←→パラメーターを相互運用するための列挙体
        /// </summary>
        public enum ExMasterShipParameter
        {
            Id, SortNo, Name, Yomi, Stype,
            AfterLv, AfterShipId, Taik, Souk, Houg,
            Raig, Tyku, Luck, Soku, Leng,
            SlotNum, MaxEq, BuildTime, Broken,PowerUp,
            Backs, GetMes, AfterFuel, AfterBull, FuelMax, 
            BullMax, Voicef, _Evation, _AntiSub, _Search,
            _DefaultSlotItem, _AirSuperiority, _WeightedAntiAir,
        }

        /// <summary>
        /// CSVのマスターデータのヘッダー
        /// </summary>
        public static string[] ExMasterShipHeader { get; private set; }
        /// <summary>
        /// パラメーターの型
        /// </summary>
        public static ExCommon.ValueType[] ExMasterShipValueType { get; private set; }
        /// <summary>
        /// 配列のパラメーターの要素数
        /// </summary>
        public static int[] ExMasterShipArrayLength { get; private set; }

        static ExMasterShip()
        {
            ExMasterShipHeader = new string[]
            {
                "艦船ID", "図鑑番号", "艦名", "読み", "艦種",
                "改装Lv", "改装後", "耐久", "装甲", "火力",
                "雷撃", "対空", "運", "速力", "射程",
                "スロット数", "搭載数", "建造時間", "解体資源", "近代化改修",
                "レア", "ドロップ文章", "改装弾薬", "改装鋼材", "搭載燃料", 
                "搭載弾薬", "ボイス", "回避", "対潜", "索敵",
                "初期装備", "制空値", "加重対空値"
            };
            ExMasterShipValueType = new ExCommon.ValueType[]
            {
                ExCommon.ValueType.Int, ExCommon.ValueType.Int, ExCommon.ValueType.String, ExCommon.ValueType.String, ExCommon.ValueType.Int,
                ExCommon.ValueType.Int, ExCommon.ValueType.String, ExCommon.ValueType.ListInt, ExCommon.ValueType.ListInt, ExCommon.ValueType.ListInt,
                ExCommon.ValueType.ListInt, ExCommon.ValueType.ListInt, ExCommon.ValueType.ListInt, ExCommon.ValueType.Int, ExCommon.ValueType.Int,
                ExCommon.ValueType.Int, ExCommon.ValueType.ListInt, ExCommon.ValueType.Int, ExCommon.ValueType.ListInt, ExCommon.ValueType.ListInt,
                ExCommon.ValueType.Int, ExCommon.ValueType.String, ExCommon.ValueType.Int, ExCommon.ValueType.Int, ExCommon.ValueType.Int,
                ExCommon.ValueType.Int, ExCommon.ValueType.Int, ExCommon.ValueType.ListInt, ExCommon.ValueType.ListInt, ExCommon.ValueType.ListInt,
                ExCommon.ValueType.ListInt, ExCommon.ValueType.Int, ExCommon.ValueType.Int,
            };
            ExMasterShipArrayLength = new int[]
            {
                0, 0, 0, 0, 0,
                0, 0, 2, 2, 2,
                2, 2, 2, 0, 0,
                0, 5, 0, 4, 4,
                0, 0, 0, 0, 0,
                0, 0, 2, 2, 2,
                5, 0, 0,
            };
        }

        public ExMasterShip() { }

        public ExMasterShip(ApiMstShip mstShip)
        {
            this.api_id = mstShip.api_id;
            this.api_sortno = mstShip.api_sortno;
            this.api_name = ExCommon.SafeStringImport(mstShip.api_name);
            this.api_yomi = ExCommon.SafeStringImport(mstShip.api_yomi);
            this.api_stype = mstShip.api_stype;
            this.api_afterlv = mstShip.api_afterlv;
            this.api_aftershipid = ExCommon.SafeStringImport(mstShip.api_aftershipid);
            this.api_taik = mstShip.api_taik;
            this.api_souk = mstShip.api_souk;
            this.api_houg = mstShip.api_houg;
            this.api_raig = mstShip.api_raig;
            this.api_tyku = mstShip.api_tyku;
            this.api_luck = mstShip.api_luck;
            this.api_soku = mstShip.api_soku;
            this.api_leng = mstShip.api_leng;
            this.api_slot_num = mstShip.api_slot_num;
            this.api_maxeq = mstShip.api_maxeq;
            this.api_buildtime = mstShip.api_buildtime;
            this.api_broken = mstShip.api_broken;
            this.api_powup = mstShip.api_powup;
            this.api_backs = mstShip.api_backs;
            this.api_getmes = ExCommon.SafeStringImport(mstShip.api_getmes);
            this.api_afterfuel = mstShip.api_afterfuel;
            this.api_afterbull = mstShip.api_afterbull;
            this.api_fuel_max = mstShip.api_fuel_max;
            this.api_bull_max = mstShip.api_bull_max;
            this.api_voicef = mstShip.api_voicef;
        }

        //インポート部分
        #region インポート部分
        /// <summary>
        /// CSV→パラメーターへのセット
        /// </summary>
        /// <param name="header">CSVに書き込まれた値と同じ列のヘッダー</param>
        /// <param name="text">CSVに書き込まれた値</param>
        public void SetValueFromCsv(string header, string text)
        {
            //列番号の検索
            int column = Array.IndexOf(ExMasterShipHeader, header);
            if (column <= -1 || column >= ExMasterShipHeader.Length) return;
            //列挙体
            ExMasterShipParameter param = (ExMasterShipParameter)column;
            //テキストをキャスト
            int defaultvalue;
            switch(param)
            {
                case ExMasterShipParameter._DefaultSlotItem:
                    defaultvalue = -1;
                    break;
                default:
                    defaultvalue = 0;
                    break;
            }
            dynamic val = ExCommon.ValueConverter(text, ExMasterShipValueType[column], ExMasterShipArrayLength[column], defaultvalue);

            //値をセット
            switch(param)
            {
                case ExMasterShipParameter._AirSuperiority: this.AirSuperiority = val; break;
                case ExMasterShipParameter._AntiSub: this.AntiSub = val; break;
                case ExMasterShipParameter._DefaultSlotItem: this.DefaultSlotItem = val; break;
                case ExMasterShipParameter._Evation: this.Evation = val; break;
                case ExMasterShipParameter._Search: this.Search = val; break;
                case ExMasterShipParameter._WeightedAntiAir: this.WeightedAntiAir = val; break;

                case ExMasterShipParameter.AfterBull: this.api_afterbull = val; break;
                case ExMasterShipParameter.AfterFuel: this.api_afterfuel = val; break;
                case ExMasterShipParameter.AfterLv: this.api_afterlv = val; break;
                case ExMasterShipParameter.AfterShipId: this.api_aftershipid = val; break;
                case ExMasterShipParameter.Backs: this.api_backs = val; break;

                case ExMasterShipParameter.Broken: this.api_broken = val; break;
                case ExMasterShipParameter.BuildTime: this.api_buildtime = val; break;
                case ExMasterShipParameter.BullMax: this.api_bull_max = val; break;
                case ExMasterShipParameter.FuelMax: this.api_fuel_max = val; break;
                case ExMasterShipParameter.GetMes: this.api_getmes = val; break;

                case ExMasterShipParameter.Houg: this.api_houg = val; break;
                case ExMasterShipParameter.Id: this.api_id = val; break;
                case ExMasterShipParameter.Leng: this.api_leng = val; break;
                case ExMasterShipParameter.Luck: this.api_luck = val; break;
                case ExMasterShipParameter.MaxEq: this.api_maxeq = val; break;

                case ExMasterShipParameter.Name: this.api_name = val; break;
                case ExMasterShipParameter.PowerUp: this.api_powup = val; break;
                case ExMasterShipParameter.Raig: this.api_raig = val; break;
                case ExMasterShipParameter.SlotNum: this.api_slot_num = val; break;
                case ExMasterShipParameter.Soku: this.api_soku = val; break;

                case ExMasterShipParameter.SortNo: this.api_sortno = val; break;
                case ExMasterShipParameter.Souk: this.api_souk = val; break;
                case ExMasterShipParameter.Stype: this.api_stype = val; break;
                case ExMasterShipParameter.Taik: this.api_taik = val; break;
                case ExMasterShipParameter.Tyku: this.api_tyku = val; break;

                case ExMasterShipParameter.Voicef: this.api_voicef = val; break;
                case ExMasterShipParameter.Yomi: this.api_yomi = val; break;
            }
        }


        /// <summary>
        /// マスターデータ→パラメーターのセット
        /// </summary>
        /// <param name="master">マスターデータ</param>
        public void MergeValueFromMasterData(ApiMstShip master)
        {
            this.api_id = master.api_id;
            this.api_sortno = master.api_sortno;
            if (master.api_name != null) this.api_name = ExCommon.SafeStringImport(master.api_name);
            if (master.api_yomi != null) this.api_yomi = ExCommon.SafeStringImport(master.api_yomi);
            this.api_stype = master.api_stype;

            this.api_afterlv = master.api_afterlv;
            if (master.api_aftershipid != null) this.api_aftershipid = ExCommon.SafeStringImport(master.api_aftershipid);
            if (master.api_taik != null) this.api_taik = master.api_taik;
            if (master.api_souk != null) this.api_souk = master.api_souk;
            if (master.api_houg != null) this.api_houg = master.api_houg;

            if (master.api_raig != null) this.api_raig = master.api_raig;
            if (master.api_tyku != null) this.api_tyku = master.api_tyku;
            if (master.api_luck != null) this.api_luck = master.api_luck;
            this.api_soku = master.api_soku;
            this.api_leng = master.api_leng;

            this.api_slot_num = master.api_slot_num;
            if (master.api_maxeq != null) this.api_maxeq = master.api_maxeq;
            this.api_buildtime = master.api_buildtime;
            if (master.api_broken != null) this.api_broken = master.api_broken;
            if (master.api_powup != null) this.api_powup = master.api_powup;

            this.api_backs = master.api_backs;
            if (master.api_getmes != null) this.api_getmes = ExCommon.SafeStringImport(master.api_getmes);
            this.api_afterfuel = master.api_afterfuel;
            this.api_afterbull = master.api_afterbull;
            this.api_fuel_max = master.api_fuel_max;

            this.api_bull_max = master.api_bull_max;
            this.api_voicef = master.api_voicef;
        }

        /// <summary>
        /// 初期装備・初期スロットの状態の制空値を計算します。敵の制空はこの値を参照します
        /// </summary>
        /// <param name="slotdata">装備のマスターデータ</param>
        public void CalcDefaultAirSuperiority(ExMasterSlotitemCollection slotdata)
        {
            if(this.api_maxeq != null && this.DefaultSlotItem != null)
            {
                int airsup = 0;

                foreach (var i in Enumerable.Range(0, Math.Min(this.DefaultSlotItem.Count, this.api_maxeq.Count)))
                {
                    ExMasterSlotitem dslotitem;
                    if (slotdata.TryGetValue(this.DefaultSlotItem[i], out dslotitem))
                    {
                        if (dslotitem.api_type != null && dslotitem.IsAirCombatable)
                        {
                            double slotas = (double)dslotitem.api_tyku * Math.Sqrt((double)this.api_maxeq[i]);
                            airsup += (int)slotas;
                        }
                    }
                }

                this.AirSuperiority = airsup;
            }
        }

        /// <summary>
        /// 初期装備・初期スロットの状態の加重対空値を計算します。敵艦が参照します
        /// </summary>
        /// <param name="slotdata">装備のマスターデータ</param>
        public void CalcDefaultWeightedAntiAir(ExMasterSlotitemCollection slotdata)
        {
            if(this.api_maxeq != null && this.DefaultSlotItem != null && this.api_tyku != null)
            {
                double antiair = (int)(2.0 * Math.Sqrt(this.api_tyku[0]));//敵艦のみ加重対空値は2√素対空で

                foreach(var i in Enumerable.Range(0, Math.Min(this.DefaultSlotItem.Count, this.api_maxeq.Count)))
                {
                    ExMasterSlotitem dslotitem;
                    if(slotdata.TryGetValue(this.DefaultSlotItem[i], out dslotitem))
                    {
                        antiair += dslotitem.WeightedAntiAirEquipmentRatio * dslotitem.api_tyku;
                    }
                }

                this.WeightedAntiAir = (int)antiair;
            }
        }
        #endregion

        //エクスポート部分
        #region エクスポート部分
        /// <summary>
        /// CSVの行を作成
        /// </summary>
        /// <returns>CSVの行テキスト</returns>
        public CsvList<string> ExportCSV()
        {
            var csv = new CsvList<string>();

            //アイテムの追加
            csv.Add(this.api_id.ToString());
            csv.Add(this.api_sortno.ToString());
            csv.Add(this.api_name ?? ExCommon.NullString);
            csv.Add(this.api_yomi ?? ExCommon.NullString);
            csv.Add(this.api_stype.ToString());

            csv.Add(this.api_afterlv.ToString());
            csv.Add(this.api_aftershipid ?? ExCommon.NullString);
            csv.Add(ExCommon.ListToCsvString(this.api_taik));
            csv.Add(ExCommon.ListToCsvString(this.api_souk));
            csv.Add(ExCommon.ListToCsvString(this.api_houg));

            csv.Add(ExCommon.ListToCsvString(this.api_raig));
            csv.Add(ExCommon.ListToCsvString(this.api_tyku));
            csv.Add(ExCommon.ListToCsvString(this.api_luck));
            csv.Add(this.api_soku.ToString());
            csv.Add(this.api_leng.ToString());

            csv.Add(this.api_slot_num.ToString());
            csv.Add(ExCommon.ListToCsvString(this.api_maxeq));
            csv.Add(this.api_buildtime.ToString());
            csv.Add(ExCommon.ListToCsvString(this.api_broken));
            csv.Add(ExCommon.ListToCsvString(this.api_powup));

            csv.Add(this.api_backs.ToString());
            csv.Add(this.api_getmes ?? ExCommon.NullString);
            csv.Add(this.api_afterfuel.ToString());
            csv.Add(this.api_afterbull.ToString());
            csv.Add(this.api_fuel_max.ToString());

            csv.Add(this.api_bull_max.ToString());
            csv.Add(this.api_voicef.ToString());
            csv.Add(ExCommon.ListToCsvString(this.Evation));
            csv.Add(ExCommon.ListToCsvString(this.AntiSub));
            csv.Add(ExCommon.ListToCsvString(this.Search));

            csv.Add(ExCommon.ListToCsvString(this.DefaultSlotItem));
            csv.Add(this.AirSuperiority.ToString());
            csv.Add(this.WeightedAntiAir.ToString());

            return csv;
        }


        /// <summary>
        /// CSVのヘッダーを作成
        /// </summary>
        /// <returns>CSVのヘッダーテキスト</returns>
        public static CsvList<string> ExportCSVHeader()
        {
            var csv = new CsvList<string>();
            csv.AddRange(ExMasterShipHeader);
            return csv;
        }
        #endregion

    }
}
