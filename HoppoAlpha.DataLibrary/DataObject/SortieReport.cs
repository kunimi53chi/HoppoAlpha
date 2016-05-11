using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using ProtoBuf;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// 出撃報告書を表すクラス
    /// </summary>
    [ProtoContract]
    public class SortieReport
    {
        /// <summary>
        /// マップのハッシュキー
        /// </summary>
        [ProtoMember(1)]
        public SortieReportMapHash Map { get; set; }//キー
        /// <summary>
        /// マップ別出撃一覧
        /// </summary>
        [ProtoMember(2)]
        public Dictionary<SortieReportHash, SortieReportItem> Sorties { get; set; }


        //コンストラクタ
        internal SortieReport()
        {
        }

        /// <summary>
        /// 変数の初期化を行うコンストラクタ
        /// </summary>
        /// <param name="flag">変数の初期化を行うかどうかのフラグ</param>
        public SortieReport(bool flag)
        {
            if(flag)
            {
                this.Sorties = new Dictionary<SortieReportHash, SortieReportItem>();
            }
        }

        /// <summary>
        /// ２つのアイテムを統合する
        /// </summary>
        /// <param name="item">統合する</param>
        /// <returns>統合されたアイテム></returns>
        public SortieReport Integrate(SortieReport item)
        {
            var result = new SortieReport(true);

            //キーは自分自身のを流用
            result.Map = this.Map;
            //出撃は統合する（ディープコピー）
            result.Sorties = new Dictionary<SortieReportHash,SortieReportItem>();
            foreach(var s in this.Sorties)
            {
                //自分自身
                result.Sorties[s.Key] = s.Value.DeepCopy();
            }
            foreach(var s in item.Sorties)
            {
                //ターゲットの統合
                SortieReportItem reportItem;
                if(result.Sorties.TryGetValue(s.Key, out reportItem))
                {
                    reportItem = reportItem.Integrate(s.Value);
                }
                else
                {
                    reportItem = s.Value.DeepCopy();
                }

                result.Sorties[s.Key] = reportItem;
            }

            return result;
        }

        /// <summary>
        /// インスタンスをディープコピーします
        /// </summary>
        /// <returns>複製されたインスタンス</returns>
        public SortieReport DeepCopy()
        {
            SortieReport item = new SortieReport();

            item.Map = this.Map;
            item.Sorties = new Dictionary<SortieReportHash, SortieReportItem>();
            foreach(var x in this.Sorties)
            {
                item.Sorties[x.Key] = x.Value.DeepCopy();
            }

            return item;
        }
    }

    [ProtoContract]
    public class SortieReportItem
    {
        #region プロパティ
        /// <summary>
        /// 艦隊編成のキー
        /// </summary>
        [ProtoMember(1)]
        public SortieReportHash Deck { get; set; }//キー

        /// <summary>
        /// 出撃回数
        /// </summary>
        [ProtoMember(2)]
        public int NumSortie { get; set; }
        /// <summary>
        /// ボス到達回数
        /// </summary>
        [ProtoMember(3)]
        public int NumBossApproached { get; set; }
        /// <summary>
        /// ボス勝利回数
        /// </summary>
        [ProtoMember(4)]
        public int NumBossDefeated { get; set; }
        /// <summary>
        /// ボスS勝利回数
        /// </summary>
        [ProtoMember(5)]
        public int NumBossSWin { get; set; }

        /// <summary>
        /// 旗艦獲得経験値
        /// </summary>
        [ProtoMember(6)]
        public int FlagshipExp { get; set; }
        /// <summary>
        /// 艦隊獲得経験値
        /// </summary>
        [ProtoMember(7)]
        public int TotalExp { get; set; }
        /// <summary>
        /// 獲得提督経験値
        /// </summary>
        [ProtoMember(8)]
        public int PlayerExp { get; set; }
        /// <summary>
        /// 連合艦隊：旗艦獲得経験値
        /// </summary>
        [ProtoMember(9)]
        public int CombinedFlagshipExp { get; set; }
        /// <summary>
        /// 連合艦隊：艦隊獲得経験値
        /// </summary>
        [ProtoMember(10)]
        public int CombinedTotalExp { get; set; }
        
        /// <summary>
        /// MVP取得回数
        /// </summary>
        [ProtoMember(11)]
        public int[] MVPCount { get; set; }
        /// <summary>
        /// 連合艦隊MVP取得回数
        /// </summary>
        [ProtoMember(12)]
        public int[] CombinedMVPCount { get; set; }

        /// <summary>
        /// 資源：マップでの獲得
        /// </summary>
        [ProtoMember(13)]
        public int[] MaterialEarn { get; set; }
        /// <summary>
        /// 資源：補給での消費
        /// </summary>
        [ProtoMember(14)]
        public int[] MaterialSupply { get; set; }
        /// <summary>
        /// 資源：修理での消費
        /// </summary>
        [ProtoMember(15)]
        public int[] MaterialRepair { get; set; }

        /// <summary>
        /// 出撃から母港帰投までの時間
        /// </summary>
        [ProtoMember(16)]
        public List<TimeSpan> TimeSortie { get; set; }
        /// <summary>
        /// 出撃時間：ボス到達時のみ
        /// </summary>
        [ProtoMember(17)]
        public List<TimeSpan> TimeBossApproached { get; set; }
        /// <summary>
        /// 出撃時間：ボス勝利時のみ
        /// </summary>
        [ProtoMember(18)]
        public List<TimeSpan> TimeBossDefeated { get; set; }
        /// <summary>
        /// 出撃時間：ボスSで勝利時のみ
        /// </summary>
        [ProtoMember(19)]
        public List<TimeSpan> TimeBossSWin { get; set; }
        #endregion

        //コンストラクタ
        internal SortieReportItem()
        {
            if (TimeSortie == null) TimeSortie = new List<TimeSpan>();
            if (TimeBossApproached == null) TimeBossApproached = new List<TimeSpan>();
            if (TimeBossDefeated == null) TimeBossDefeated = new List<TimeSpan>();
            if (TimeBossSWin == null) TimeBossSWin = new List<TimeSpan>();
        }

        /// <summary>
        /// 変数の初期化を行うコンストラクタ
        /// </summary>
        /// <param name="flag">変数の初期化を行うかどうかのフラグ</param>
        public SortieReportItem(bool flag)
        {
            if (flag)
            {
                this.MVPCount = new int[6];
                this.CombinedMVPCount = new int[6];

                this.MaterialEarn = new int[8];
                this.MaterialRepair = new int[8];
                this.MaterialSupply = new int[8];

                this.TimeSortie = new List<TimeSpan>();
                this.TimeBossApproached = new List<TimeSpan>();
                this.TimeBossDefeated = new List<TimeSpan>();
                this.TimeBossSWin = new List<TimeSpan>();
            }
        }

        /// <summary>
        /// 2つのアイテムを統合する。集計用
        /// </summary>
        /// <param name="item">統合するアイテム</param>
        /// <returns>統合されたアイテム</returns>
        public SortieReportItem Integrate(SortieReportItem item)
        {
            var result = new SortieReportItem(true);

            //キーは自分のを流用
            result.Deck = this.Deck;
            //回数は加算
            result.NumSortie = this.NumSortie + item.NumSortie;
            result.NumBossApproached = this.NumBossApproached + item.NumBossApproached;
            result.NumBossDefeated = this.NumBossDefeated + item.NumBossDefeated;
            result.NumBossSWin = this.NumBossSWin + item.NumBossSWin;
            //獲得経験値も加算
            result.FlagshipExp = this.FlagshipExp + item.FlagshipExp;
            result.TotalExp = this.TotalExp + item.TotalExp;
            result.PlayerExp = this.PlayerExp + item.PlayerExp;
            result.CombinedFlagshipExp = this.CombinedFlagshipExp + item.CombinedFlagshipExp;
            result.CombinedTotalExp = this.CombinedTotalExp + item.CombinedTotalExp;
            //MVP獲得回数
            foreach(int i in Enumerable.Range(0, 6))
            {
                result.MVPCount[i] = this.MVPCount[i] + item.MVPCount[i];
                result.CombinedMVPCount[i] = this.CombinedMVPCount[i] + item.CombinedMVPCount[i];
            }
            //資源
            foreach(int i in Enumerable.Range(0, 8))
            {
                result.MaterialEarn[i] = this.MaterialEarn[i] + item.MaterialEarn[i];
                result.MaterialRepair[i] = this.MaterialRepair[i] + item.MaterialRepair[i];
                result.MaterialSupply[i] = this.MaterialSupply[i] + item.MaterialSupply[i];
            }
            //時間
            result.TimeSortie.AddRange(this.TimeSortie); result.TimeSortie.AddRange(item.TimeSortie);
            result.TimeBossApproached.AddRange(this.TimeBossApproached); result.TimeBossApproached.AddRange(item.TimeBossApproached);
            result.TimeBossDefeated.AddRange(this.TimeBossDefeated); result.TimeBossDefeated.AddRange(item.TimeBossDefeated);
            result.TimeBossSWin.AddRange(this.TimeBossSWin); result.TimeBossSWin.AddRange(item.TimeBossSWin);

            return result;
        }

        /// <summary>
        /// インスタンスをディープコピーをします
        /// </summary>
        /// <returns>複製されたインスタンス</returns>
        public SortieReportItem DeepCopy()
        {
            SortieReportItem item = new SortieReportItem(true);

            item.Deck = this.Deck;

            item.NumSortie = this.NumSortie;
            item.NumBossApproached = this.NumBossApproached;
            item.NumBossDefeated = this.NumBossDefeated;
            item.NumBossSWin = this.NumBossSWin;

            item.FlagshipExp = this.FlagshipExp;
            item.TotalExp = this.TotalExp;
            item.PlayerExp = this.PlayerExp;
            item.CombinedFlagshipExp = this.CombinedFlagshipExp;
            item.CombinedTotalExp = this.CombinedTotalExp;

            item.MVPCount = this.MVPCount.Select(x => x).ToArray();//int[]
            item.CombinedMVPCount = this.CombinedMVPCount.Select(x => x).ToArray();//int[]

            item.MaterialEarn = this.MaterialEarn.Select(x => x).ToArray();//int[]
            item.MaterialSupply = this.MaterialSupply.Select(x => x).ToArray();//int[]
            item.MaterialRepair = this.MaterialRepair.Select(x => x).ToArray();//int[]

            item.TimeSortie = this.TimeSortie.Select(x => x).ToList();//List<TimeSpan>以下同様
            item.TimeBossApproached = this.TimeBossApproached.Select(x => x).ToList();
            item.TimeBossDefeated = this.TimeBossDefeated.Select(x => x).ToList();
            item.TimeBossSWin = this.TimeBossSWin.Select(x => x).ToList();

            return item;
        }

        //UI用
        #region UI用
        /// <summary>
        /// UI用のテキスト表示メソッド
        /// </summary>
        /// <param name="viewmode">データを統合するモード</param>
        /// <param name="mstship">艦船マスターデータ</param>
        /// <param name="mststype">艦種マスターデータ</param>
        /// <param name="mstslotitem">装備マスターデータ</param>
        /// <param name="maphash">マップハッシュ</param>
        /// <returns>UI用の表示テキスト</returns>
        public string Display(SortieReportShipHashIntegrateMode viewmode, ExMasterShipCollection mstship, Dictionary<int, ApiMstStype> mststype, ExMasterSlotitemCollection mstslotitem, SortieReportMapHash maphash)
        {
            //連合艦隊フラグ
            bool isCombined = this.Deck.FleetCombined.Ship1.MasterShipTypeID != 0;

            StringBuilder sb = new StringBuilder();
            //--マップ
            sb.AppendFormat("【{0}】", maphash.Display()).AppendLine();
            //--編成
            sb.AppendLine("☆編成");
            //通常艦隊
            string fleet = DisplayFleet(viewmode, Deck.Fleet, mstship, mststype, mstslotitem);
            if (fleet != null) sb.AppendLine(fleet);
            //連合艦隊
            if(isCombined)
            {
                fleet = DisplayFleet(viewmode, Deck.FleetCombined, mstship, mststype, mstslotitem);
                if(fleet != null)
                {
                    sb.AppendLine(fleet);
                }
            }
            sb.AppendLine();
            //--カウンター
            sb.AppendLine("☆カウンター");
            sb.AppendFormat("出撃回数 : {0}", NumSortie).AppendLine();
            double bossapproach = (double)NumBossApproached / (double)NumSortie;
            double bosswin = (double)NumBossDefeated / (double)NumSortie;
            double boss_s = (double)NumBossSWin / (double)NumSortie;
            sb.AppendFormat("ボス到達 : {0}({1})\tボス勝利 : {2}({3})", NumBossApproached, bossapproach.ToString("P1"), NumBossDefeated, bosswin.ToString("P1")).AppendLine();
            sb.AppendFormat("ボスS勝利 : {0}({1})", NumBossSWin, boss_s.ToString("P1")).AppendLine();
            sb.AppendLine();
            //--経験値
            sb.AppendLine("☆経験値");
            sb.AppendLine("旗艦Exp合計 : " + FlagshipExp);
            sb.AppendLine("艦隊Exp合計 : " + TotalExp);
            sb.AppendLine("提督Exp合計 : " + PlayerExp);
            if(isCombined)
            {
                sb.AppendLine("連合旗艦Exp合計 : " + CombinedFlagshipExp);
                sb.AppendLine("連合艦隊Exp合計 : " + CombinedTotalExp);
            }
            sb.AppendLine();
            //--MVP回数
            sb.AppendLine("☆MVP回数");
            sb.AppendLine(DisplayMVP(MVPCount));
            if(isCombined) sb.AppendLine(DisplayMVP(CombinedMVPCount));
            sb.AppendLine();
            //--資源
            //合計資材
            int[] materialTotal = new int[8];
            foreach (int i in Enumerable.Range(0, 8)) materialTotal[i] = MaterialEarn[i] + MaterialRepair[i] + MaterialSupply[i];
            sb.AppendLine("☆資材");
            sb.Append(DisplayMaterial(MaterialEarn, NumSortie, "獲得資材"));
            sb.Append(DisplayMaterial(MaterialSupply, NumSortie, "補給資材"));
            sb.Append(DisplayMaterial(MaterialRepair, NumSortie, "入渠資材"));
            sb.Append(DisplayMaterial(materialTotal, NumSortie, "合計"));
            sb.AppendLine();
            //--時間
            sb.AppendLine("☆時間");
            sb.AppendLine("【総合】 " + DisplayTime(TimeSortie));
            sb.AppendLine("【ボス到達時】 " + DisplayTime(TimeBossApproached));
            sb.AppendLine("【ボス勝利時】 " + DisplayTime(TimeBossDefeated));
            sb.AppendLine("【ボスS勝利時】" + DisplayTime(TimeBossSWin));
            sb.AppendLine("※[最小値, 25%タイル, 中央値, 75%タイル, 最大値 | 平均値]");

            return sb.ToString();
        }

        //集計用のDictionaryの加算用
        private static void AppendDictionary(Dictionary<int, int> dict, int key)
        {
            int x;
            dict.TryGetValue(key, out x);
            dict[key] = x + 1;
        }

        //MVPの表示用
        private static string DisplayMVP(int[] mvparray)
        {
            double mvptotal = mvparray.Sum();
            var percents = mvparray.Select(x => ((double)x / mvptotal).ToString("P0"));

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join("-", mvparray));
            if (mvptotal != 0)
            {
                sb.Append("[" + string.Join("-", percents) + "]");
            }
            else
            {
                sb.Append("[" + string.Join("-", Enumerable.Repeat("0%", 6)) + "]");
            }

            return sb.ToString();
        }

        //資源の表示用
        private static string[] materialNames = new string[] { "燃", "弾", "鋼", "ボ", "建", "修", "開", "改" };
        private static string DisplayMaterial(int[] materialArray, int numSortie, string title)
        {
            List<string> total = new List<string>();
            List<string> average = new List<string>();
            foreach(int i in Enumerable.Range(0, materialArray.Length))
            {
                if(materialArray[i] != 0)
                {
                    total.Add(materialNames[i] + " " +materialArray[i]);
                    average.Add(materialNames[i] +" " + ((double)materialArray[i] / (double)numSortie).ToString("F1"));
                }
            }

            StringBuilder sb = new StringBuilder();
            if (total.Count > 0) sb.AppendLine(title + " : " + string.Join(", ", total));
            if (average.Count > 0) sb.AppendLine("－1周あたり平均 : " + string.Join(", ", average));

            return sb.ToString();
        }

        private static string DisplayTime(List<TimeSpan> times)
        {
            if (times == null || times.Count == 0) return "データなし";
            //降順ソート
            var sorted = times.OrderByDescending(x => x).ToList();
            //25%
            var p25 = sorted[times.Count * 3 / 4];
            //中央値
            var median = sorted[times.Count / 2];
            //75%
            var p75 = sorted[times.Count / 4];
            //平均値
            var average = TimeSpan.FromMilliseconds(times.Average(x => x.TotalMilliseconds));
            //最小値
            var min = sorted[sorted.Count - 1];
            //最大値
            var max = sorted[0];

            return string.Format("[{0}, {1}, {2}, {3}, {4} | {5}]",
                min.ToString(@"mm\:ss"), p25.ToString(@"mm\:ss"), median.ToString(@"mm\:ss"),
                p75.ToString(@"mm\:ss"), max.ToString(@"mm\:ss"), average.ToString(@"mm\:ss"));
        }

        //編成一覧の作成
        private static string DisplayFleet(SortieReportShipHashIntegrateMode viewmode, SortieReportFleetHash fleet, 
            ExMasterShipCollection mstship, Dictionary<int, ApiMstStype> mststype, ExMasterSlotitemCollection mstslotitem)
        {
            //艦種で表示する場合
            if(viewmode.HasFlag(SortieReportShipHashIntegrateMode._ShipType))
            {
                //艦種で集計
                Dictionary<int, int> num_stype = new Dictionary<int, int>();
                AppendDictionary(num_stype, fleet.Ship1.MasterShipTypeID);
                AppendDictionary(num_stype, fleet.Ship2.MasterShipTypeID);
                AppendDictionary(num_stype, fleet.Ship3.MasterShipTypeID);
                AppendDictionary(num_stype, fleet.Ship4.MasterShipTypeID);
                AppendDictionary(num_stype, fleet.Ship5.MasterShipTypeID);
                AppendDictionary(num_stype, fleet.Ship6.MasterShipTypeID);
                //艦種IDを降順でソートして文字列化したクエリ
                List<string> query = new List<string>();
                foreach(var val in num_stype.OrderByDescending(x => x.Key))
                {
                    ApiMstStype dstype;
                    if (val.Key != 0 && mststype.TryGetValue(val.Key, out dstype))
                    {
                        query.Add(dstype.api_name + val.Value);//例：潜水空母4
                    }
                }

                //返り値
                string result = string.Join("　", query);
                if (viewmode.HasFlag(SortieReportShipHashIntegrateMode._Descending)) result = result + "　(※順不同)";
                return result;
            }
            //艦で表示する場合
            else if(viewmode.HasFlag(SortieReportShipHashIntegrateMode._ShipId))
            {
                int[] mstshipid = new int[]
                {
                    fleet.Ship1.MasterShipID, fleet.Ship2.MasterShipID, fleet.Ship3.MasterShipID, fleet.Ship4.MasterShipID, fleet.Ship5.MasterShipID, fleet.Ship6.MasterShipID
                };
                
                List<string> shipnames = new List<string>();
                foreach(var x in mstshipid)
                {
                    ExMasterShip dship;
                    if (mstship.TryGetValue(x, out dship)) shipnames.Add(dship.api_name);
                }

                //返り値
                string result = string.Join("　", shipnames);
                if (viewmode.HasFlag(SortieReportShipHashIntegrateMode._Descending)) result = result + "　(※順不同)";
                return result;
            }
            //装備込みで表示する場合
            else if(viewmode.HasFlag(SortieReportShipHashIntegrateMode._Slotitem))
            {
                SortieReportShipHash[] shiphash = new SortieReportShipHash[]
                {
                    fleet.Ship1, fleet.Ship2, fleet.Ship3, fleet.Ship4, fleet.Ship5, fleet.Ship6
                };

                StringBuilder sb = new StringBuilder();
                foreach(var ship in shiphash)
                {
                    if(ship.MasterShipID == 0) continue;
                    //装備リスト
                    List<string> slots = new List<string>();
                    ExMasterSlotitem dslot;
                    if (ship.SlotitemMasterID1 != 0 && mstslotitem.TryGetValue(ship.SlotitemMasterID1, out dslot)) slots.Add(dslot.api_name);
                    if (ship.SlotitemMasterID2 != 0 && mstslotitem.TryGetValue(ship.SlotitemMasterID2, out dslot)) slots.Add(dslot.api_name);
                    if (ship.SlotitemMasterID3 != 0 && mstslotitem.TryGetValue(ship.SlotitemMasterID3, out dslot)) slots.Add(dslot.api_name);
                    if (ship.SlotitemMasterID4 != 0 && mstslotitem.TryGetValue(ship.SlotitemMasterID4, out dslot)) slots.Add(dslot.api_name);
                    if (ship.SlotitemMasterID5 != 0 && mstslotitem.TryGetValue(ship.SlotitemMasterID5, out dslot)) slots.Add(dslot.api_name);
                    if (ship.SlotitemMasterIDEx > 0 && mstslotitem.TryGetValue(ship.SlotitemMasterIDEx, out dslot)) slots.Add(dslot.api_name);
                    //キャラ名
                    string charaname = "ID" + ship.MasterShipID;
                    ExMasterShip dship;
                    if(mstship.TryGetValue(ship.MasterShipID, out dship)) charaname = dship.api_name;
                    //キャラ名　装備 / 装備 / …
                    string slotstr;
                    if (slots.Count == 0) slotstr = "なし";
                    else slotstr = string.Join(" / ", slots);

                    sb.AppendLine(charaname + "　" + slotstr);
                }

                if (viewmode.HasFlag(SortieReportShipHashIntegrateMode._Descending)) sb.AppendLine("(※順不同)");

                //返り値
                return sb.ToString();
            }
            //どれでもない場合
            return null;
        }
        #endregion

    }
}
