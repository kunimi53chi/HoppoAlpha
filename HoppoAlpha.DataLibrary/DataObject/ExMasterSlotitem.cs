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
    /// 装備のマスターデータの拡張クラス
    /// </summary>
    /// 拡張できるように継承で実装しておく
    public class ExMasterSlotitem : ApiMstSlotitem
    {
        /// <summary>
        /// 装備の種類のID
        /// </summary>
        public int EquipType
        {
            get
            {
                if (this.api_type == null)
                {
                    Console.WriteLine(Environment.StackTrace);
                    throw new NullReferenceException("api_typeがNULLです");
                }
                return this.api_type[2];
            }
        }

        /// <summary>
        /// 該当の装備が制空争いに参加できるかどうかを表します
        /// </summary>
        public bool IsAirCombatable
        {
            get
            {
                switch(EquipType)
                {
                    //制空争いに参加する装備：艦戦(6), 艦爆(7), 艦攻(8), 水上攻撃機(11), 水上戦闘機(45)
                    case 6:
                    case 7:
                    case 8:
                    case 11:
                    case 45:
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// 敵の装備かどうかを表します
        /// </summary>
        public bool IsEnemySlotitem
        {
            get
            {
                return this.api_id >= 500;
            }
        }

        /// <summary>
        /// 加重対空値の装備倍率を表します
        /// </summary>
        public int WeightedAntiAirEquipmentRatio
        {
            get
            {
                //EquipmentTypeと異なりapi_type[3]で判定するので注意
                switch(this.api_type[3])
                {
                    case 11://対空電探
                        return 3;
                    case 15://機銃
                        return 6;
                    case 16://高角砲
                    case 30://高射装置
                        return 4;
                    default:
                        return 0;
                }
            }
        }

        /// <summary>
        /// CSV←→パラメーターを相互運用するための列挙体
        /// </summary>
        public enum ExMasterSlotitemParameter
        {
            Id, SortNo, Name, Type, Taik,
            Souk, Houg, Raig, Soku, Baku,
            Tyku, Tais, Atap, Houm, Raim,
            Houk, Raik, Bakk, Saku, Sakb,
            Luck, Leng, Rare, Broken, Info,
            Usebull,
        }

        /// <summary>
        /// CSVのマスターデータのヘッダー
        /// </summary>
        public static string[] ExMasterSlotitemHeader { get; private set; }
        /// <summary>
        /// パラメーターの型
        /// </summary>
        public static ExCommon.ValueType[] ExMasterSlotitemValueType { get; private set; }
        /// <summary>
        /// 配列のパラメーターの要素数
        /// </summary>
        public static int[] ExMasterSlotitemArrayLength { get; private set; }


        static ExMasterSlotitem()
        {
            ExMasterSlotitemHeader = new string[]
            {
                "装備ID", "図鑑番号", "装備名", "種類", "耐久",
                "装甲", "火力", "雷撃", "速力", "爆装",
                "対空", "対潜", "api_atap", "命中", "雷撃命中",
                "回避", "雷撃回避", "爆装回避", "索敵", "索敵妨害",
                "運", "射程", "レアリティ", "破棄時資材", "説明",
                "弾薬消費",
            };
            ExMasterSlotitemValueType = new ExCommon.ValueType[]
            {
                ExCommon.ValueType.Int, ExCommon.ValueType.Int, ExCommon.ValueType.String, ExCommon.ValueType.ListInt, ExCommon.ValueType.Int,
                ExCommon.ValueType.Int, ExCommon.ValueType.Int, ExCommon.ValueType.Int, ExCommon.ValueType.Int, ExCommon.ValueType.Int,
                ExCommon.ValueType.Int, ExCommon.ValueType.Int, ExCommon.ValueType.Int, ExCommon.ValueType.Int, ExCommon.ValueType.Int,
                ExCommon.ValueType.Int, ExCommon.ValueType.Int, ExCommon.ValueType.Int, ExCommon.ValueType.Int, ExCommon.ValueType.Int,
                ExCommon.ValueType.Int, ExCommon.ValueType.Int, ExCommon.ValueType.Int, ExCommon.ValueType.ListInt, ExCommon.ValueType.String,
                ExCommon.ValueType.String,
            };
            ExMasterSlotitemArrayLength = new int[]
            {
                0,0,0,4,0,
                0,0,0,0,0,
                0,0,0,0,0,
                0,0,0,0,0,
                0,0,0,4,0,
                0,
            };
        }

        public ExMasterSlotitem() { }

        public ExMasterSlotitem(ApiMstSlotitem mstSlotitem)
        {
            this.api_id = mstSlotitem.api_id;
            this.api_sortno = mstSlotitem.api_sortno;
            this.api_name = ExCommon.SafeStringImport(mstSlotitem.api_name);
            this.api_type = mstSlotitem.api_type;
            this.api_taik = mstSlotitem.api_taik;
            this.api_souk = mstSlotitem.api_soku;
            this.api_houg = mstSlotitem.api_houg;
            this.api_raig = mstSlotitem.api_raig;
            this.api_soku = mstSlotitem.api_soku;
            this.api_baku = mstSlotitem.api_baku;
            this.api_tyku = mstSlotitem.api_tyku;
            this.api_tais = mstSlotitem.api_tais;
            this.api_atap = mstSlotitem.api_atap;
            this.api_houm = mstSlotitem.api_houm;
            this.api_raim = mstSlotitem.api_raim;
            this.api_houk = mstSlotitem.api_houk;
            this.api_raik = mstSlotitem.api_raik;
            this.api_bakk = mstSlotitem.api_bakk;
            this.api_saku = mstSlotitem.api_saku;
            this.api_sakb = mstSlotitem.api_sakb;
            this.api_luck = mstSlotitem.api_luck;
            this.api_leng = mstSlotitem.api_leng;
            this.api_rare = mstSlotitem.api_rare;
            this.api_broken = mstSlotitem.api_broken;
            this.api_info = ExCommon.SafeStringImport(mstSlotitem.api_info);
            this.api_usebull = ExCommon.SafeStringImport(mstSlotitem.api_usebull);
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
            int column = Array.IndexOf(ExMasterSlotitemHeader, header);
            if (column <= -1 || column >= ExMasterSlotitemHeader.Length) return;
            //列挙体
            ExMasterSlotitemParameter param = (ExMasterSlotitemParameter)column;
            //テキストをキャスト
            dynamic val = ExCommon.ValueConverter(text, ExMasterSlotitemValueType[column], ExMasterSlotitemArrayLength[column]);//初期値は全て0でOK

            //値をセット
            switch(param)
            {
                case ExMasterSlotitemParameter.Atap: this.api_atap = val; break;
                case ExMasterSlotitemParameter.Bakk: this.api_bakk = val; break;
                case ExMasterSlotitemParameter.Baku: this.api_baku = val; break;
                case ExMasterSlotitemParameter.Broken: this.api_broken = val; break;
                case ExMasterSlotitemParameter.Houg: this.api_houg = val; break;

                case ExMasterSlotitemParameter.Houk: this.api_houk = val; break;
                case ExMasterSlotitemParameter.Houm: this.api_houm = val; break;
                case ExMasterSlotitemParameter.Id: this.api_id = val; break;
                case ExMasterSlotitemParameter.Info: this.api_info = val; break;
                case ExMasterSlotitemParameter.Leng: this.api_leng = val; break;

                case ExMasterSlotitemParameter.Luck: this.api_luck = val; break;
                case ExMasterSlotitemParameter.Name: this.api_name = val; break;
                case ExMasterSlotitemParameter.Raig: this.api_raig = val; break;
                case ExMasterSlotitemParameter.Raik: this.api_raik = val; break;
                case ExMasterSlotitemParameter.Raim: this.api_raim = val; break;

                case ExMasterSlotitemParameter.Rare: this.api_rare = val; break;
                case ExMasterSlotitemParameter.Sakb: this.api_sakb = val; break;
                case ExMasterSlotitemParameter.Saku: this.api_saku = val; break;
                case ExMasterSlotitemParameter.Soku: this.api_soku = val; break;
                case ExMasterSlotitemParameter.SortNo: this.api_sortno = val; break;

                case ExMasterSlotitemParameter.Souk: this.api_souk = val; break;
                case ExMasterSlotitemParameter.Taik: this.api_taik = val; break;
                case ExMasterSlotitemParameter.Tais: this.api_tais = val; break;
                case ExMasterSlotitemParameter.Tyku: this.api_tyku = val; break;
                case ExMasterSlotitemParameter.Type: this.api_type = val; break;

                case ExMasterSlotitemParameter.Usebull: this.api_usebull = val; break;
            }
        }

        /// <summary>
        /// マスターデータ→パラメーターのセット
        /// </summary>
        /// <param name="master">マスターデータ</param>
        public void MergeValueFromMasterData(ApiMstSlotitem master)
        {
            this.api_id = master.api_id;
            this.api_sortno = master.api_sortno;
            if(master.api_name != null) this.api_name = ExCommon.SafeStringImport(master.api_name);
            if(master.api_type != null) this.api_type = master.api_type;
            this.api_taik = master.api_taik;
            this.api_souk = master.api_soku;
            this.api_houg = master.api_houg;
            this.api_raig = master.api_raig;
            this.api_soku = master.api_soku;
            this.api_baku = master.api_baku;
            this.api_tyku = master.api_tyku;
            this.api_tais = master.api_tais;
            this.api_atap = master.api_atap;
            this.api_houm = master.api_houm;
            this.api_raim = master.api_raim;
            this.api_houk = master.api_houk;
            this.api_raik = master.api_raik;
            this.api_bakk = master.api_bakk;
            this.api_saku = master.api_saku;
            this.api_sakb = master.api_sakb;
            this.api_luck = master.api_luck;
            this.api_leng = master.api_leng;
            this.api_rare = master.api_rare;
            if(master.api_broken != null) this.api_broken = master.api_broken;
            if(master.api_info != null) this.api_info = ExCommon.SafeStringImport(master.api_info);
            if(master.api_usebull != null) this.api_usebull = ExCommon.SafeStringImport(master.api_usebull);
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

            csv.Add(this.api_id.ToString());
            csv.Add(this.api_sortno.ToString());
            csv.Add(this.api_name ?? ExCommon.NullString);
            csv.Add(ExCommon.ListToCsvString(this.api_type));
            csv.Add(this.api_taik.ToString());

            csv.Add(this.api_souk.ToString());
            csv.Add(this.api_houg.ToString());
            csv.Add(this.api_raig.ToString());
            csv.Add(this.api_soku.ToString());
            csv.Add(this.api_baku.ToString());

            csv.Add(this.api_tyku.ToString());
            csv.Add(this.api_tais.ToString());
            csv.Add(this.api_atap.ToString());
            csv.Add(this.api_houm.ToString());
            csv.Add(this.api_raim.ToString());

            csv.Add(this.api_houk.ToString());
            csv.Add(this.api_raik.ToString());
            csv.Add(this.api_bakk.ToString());
            csv.Add(this.api_saku.ToString());
            csv.Add(this.api_sakb.ToString());

            csv.Add(this.api_luck.ToString());
            csv.Add(this.api_leng.ToString());
            csv.Add(this.api_rare.ToString());
            csv.Add(ExCommon.ListToCsvString(this.api_broken));
            csv.Add(this.api_info ?? ExCommon.NullString);

            csv.Add(this.api_usebull ?? ExCommon.NullString);

            return csv;
        }

        /// <summary>
        /// CSVのヘッダーを作成
        /// </summary>
        /// <returns>CSVのヘッダーテキスト</returns>
        public static CsvList<string> ExportCSVHeader()
        {
            var csv = new CsvList<string>();
            csv.AddRange(ExMasterSlotitemHeader);
            return csv;
        }
        #endregion

    }
}
