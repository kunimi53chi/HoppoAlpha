using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HoppoAlpha.DataLibrary.DataObject;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;
using HoppoAlpha.DataLibrary.Const;

namespace VisualFormTest
{
    public static class DropDataBase
    {
        //データコレクション
        public static DropRecordCollection Collection { get; set; }
        //追加をブロック
        public static bool AddingBlock { get; set; }
        //初期化したかどうか
        public static bool IsInited { get; set; }
        //ファイル名（旧版）
        public static readonly string OutputOldFileName = "droprecord.dat";
        public static readonly string OutputOldDirectory = "config";
        public static string OutputOldFullPath
        {
            get
            {
                return OutputOldDirectory + @"\" + OutputOldFileName;
            }
        }
        public static readonly string OutputDirectory = @"user\drop";
        public static string GetOutputFileName(DateTime date)
        {
            return string.Format("droprecord{0}.dat", date.Year);
        }
        public static string GetOutputFullPath(DateTime date)
        {
            return OutputDirectory + @"\" + GetOutputFileName(date);
        }


        #region 内部クラス
        //集計用クラス
        public class DropDataSummarize
        {
            public string[] Headers { get; set; }//リストビューの列
            public List<DropDataSummarizeRow> Rows { get; set; }
            public DropDataSummarizeMode Mode { get; set; }
        }

        //集計用モード
        public enum DropDataSummarizeMode
        {
            None, ByMap, ByShip,
        }

        //集計用の行
        public class DropDataSummarizeRow
        {
            public string Item { get; set; }
            public int NumTotal { get; set; }
            public int NumS { get; set; }
            public int NumA { get; set; }
            public int NumB { get; set; }
            public int NumNone { get; set; }
            public double Percentage { get; set; }
            public List<DropRecord> CorrespondingRecord { get; set; }

            public object GetCell(int column)
            {
                switch(column)
                {
                    case 0:
                        return this.Item;
                    case 1:
                        return this.NumTotal;
                    case 2:
                        return this.NumS;
                    case 3:
                        return this.NumA;
                    case 4:
                        return this.NumB;
                    case 5:
                        return this.NumNone;
                    case 6:
                        return this.Percentage;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            public string[] MakeListViewRow()
            {
                return new string[]{this.Item, this.NumTotal.ToString(), 
                    this.NumS.ToString(), this.NumA.ToString(), this.NumB.ToString(), this.NumNone.ToString(), this.Percentage.ToString("P1")};
            }
        }

        public class SummarizeByWinRank
        {
            public int CountS { get; set; }
            public int CountA { get; set; }
            public int CountB { get; set; }
            public int CountNone { get; set; }
            public int Total { get; set; }
            public List<DropRecord> Records { get; set; }

            public void AddValue(string winrank, bool dropnothing)
            {
                this.Total++;

                if (dropnothing) this.CountNone++;
                else
                {
                    switch(winrank)
                    {
                        case "S": this.CountS++; break;
                        case "A": this.CountA++; break;
                        case "B": this.CountB++; break;
                    }
                }
            }
        }

        [Flags]
        public enum WinRank
        {
            None = 0,
            S = 1,
            A = 2,
            B = 4,
            C = 8,
            D = 16,
            E = 32
        }
        #endregion


        //コンストラクタ
        static DropDataBase()
        {
            //新しいディレクトリが作られていない場合
            if(!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }

            //コンバート処理（古いディレクトリにファイルがあり、新しいディレクトリのファイルがない場合）
            DateTime now = DateTime.Now;
            string filePath = GetOutputFullPath(now);
            if(Directory.GetFiles(OutputDirectory).Length == 0 && File.Exists(OutputOldFullPath))
            {
                //新しいディレクトリにコピーする
                File.Copy(OutputOldFullPath, filePath);
            }

            //新しいファイルがなければ
            if (!File.Exists(filePath))
            {
                Collection = new DropRecordCollection();
            }
            else
            {
                var loadResult = HoppoAlpha.DataLibrary.Files.TryLoad(filePath, HoppoAlpha.DataLibrary.DataType.DropRecord);
                LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.DropRecord, loadResult, false);
                Collection = (DropRecordCollection)loadResult.Instance;

                /*Ver4.0でコンバート処理を切る
                //--2015/7/17前のデータの変換
                //マスターヘッダー
                foreach(var a in Collection.MasterMapHeader.ChildrenAreas)
                {
                    foreach(var b in a.Value.ChildrenMaps)
                    {
                        foreach(var c in b.Value.ChildrenCells)
                        {
                            //敵編成IDの変換
                            c.Value.ConvertOldData(EnemyFleetDataBase.DataBase);
                        }
                    }
                }
                //ドロップデータ
                foreach(var x in Collection.DataBase)
                {
                    x.ConvertOldData(Collection.MasterMapHeader);
                }*/

            }
            IsInited = true;
        }

        //保存
        public static void Save()
        {
            if (!IsInited || Collection == null) return;

            AddingBlock = true;
            if (!System.IO.Directory.Exists(OutputDirectory))
            {
                System.IO.Directory.CreateDirectory(OutputDirectory);
            }

            var saveResult = HoppoAlpha.DataLibrary.Files.Save(GetOutputFullPath(DateTime.Now), HoppoAlpha.DataLibrary.DataType.DropRecord, Collection);
            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.DropRecord, saveResult, true);

            AddingBlock = false;
        }

        //追加
        public static void AddDataBase(int dropshipid, int dropitemid, string mapname, string enemyfleetname)
        {
            if (AddingBlock) return;
            //戦闘状態のチェック
            BattleView view = APIBattle.BattleView;
            if (view == null) return;
            if (view.Situation != BattleSituation.EndBattle && view.Situation == BattleSituation.EndCombinedBattle) return;
            //追加を拒否している状態をチェック
            if (Config.DropRecordAddDisable) return;
            //infoチェック
            //名前とレベルゲット用の関数を作成
            Func<int, string> getShipName = delegate(int shipid)
            {
                if(shipid == -1) return "";
                if(!APIPort.ShipsDictionary.ContainsKey(shipid)) return "";
                ApiShip oship = APIPort.ShipsDictionary[shipid];
                if (oship.ShipName == null) return "";
                return oship.ShipName;
            };
            Func<int, int> getShipLevel = delegate(int shipid)
            {
                if(shipid == -1) return -1;
                if(!APIPort.ShipsDictionary.ContainsKey(shipid)) return -1;
                return APIPort.ShipsDictionary[shipid].api_lv;
            };
            //マップの難易度
            int difficulty = 0;
            if(APIGetMember.MapInfo != null)
            {
                var map_query = APIGetMember.MapInfo.Where(x => x.api_id == view.AreaID * 10 + view.MapID);
                if(map_query.Count() != 0)
                {
                    var target_map = map_query.First();
                    if (target_map.api_eventmap != null) difficulty = target_map.api_eventmap.api_selected_rank;
                }
            }
            //改造艦を含めて存在する個数
            int shipexists_remodeling = 0;
            if (dropshipid != -1)
            {
                //調査する船のID
                List<int> checkid = new List<int>();
                checkid.Add(dropshipid);
                //該当艦のノードを取る
                if (!KanmusuRemodelingTable.IsInited) KanmusuRemodelingTable.Init();
                KanmusuRemodelingTable.KanmusuRemodelingNode node;
                if(KanmusuRemodelingTable.Nodes.TryGetValue(dropshipid, out node))
                {
                    var prev = node.Previous;
                    while(prev != null)
                    {
                        if (prev.Data == null) break;
                        checkid.Add(prev.Data.api_id);
                        prev = prev.Previous;
                    }
                    var next = node.Next;
                    while(next != null)
                    {
                        if (next.Data == null) break;//ループ改装対策の対策用
                        checkid.Add(next.Data.api_id);
                        next = next.Next;
                    }
                }
                //存在する個数
                foreach(var check in checkid)
                {
                    foreach(var x in APIPort.ShipsDictionary.Values)
                    {
                        if (x.api_ship_id == check)
                        {
                            shipexists_remodeling++;
                        }
                    }
                }
            }
            //ドロップアイテムが既に存在するか
            bool dropitemexsits = false;
            Useitem dropuseitemobj = null;
            if(dropitemid != -1 && dropitemid >= 0)
            {
                var query = APIGetMember.Useitems.Where(x => x.api_id == dropitemid);
                if(query.Count() >= 1)
                {
                    dropuseitemobj = query.First();
                    dropitemexsits = dropuseitemobj.api_count >= 1;
                }
            }
            //DBアイテムの作成
            DropRecord drop = new DropRecord()
            {
                //マップ情報
                MapAreaID = view.AreaID,
                MapInfoID = view.MapID,
                MapCellID = view.CellID,
                //ボス戦かどうか
                BossFlag = view.BossFlag == 2 ? true : false,//2がボス戦で1が雑魚戦
                WinRank = view.WinRank,
                //マップ関係
                HQLevel = APIPort.Basic.api_level,
                MapDifficulty = difficulty,
                //ドロップ（船）
                DropShipFlag = dropshipid != -1,
                DropShipID = dropshipid,
                DropShipAlreadyExists = (dropshipid != -1) ? APIPort.ShipsDictionary.Values.Select(x => x.api_ship_id).Contains(dropshipid) : false,
                DropShipAlreadyExistsContainsRemodeling = shipexists_remodeling > 0,//2016/2追加
                DropShipNumOfExistsContainsRemodeling = shipexists_remodeling,//2016/2追加
                //アイテム
                DropItemFlag = dropitemid != -1,
                DropItemID = dropitemid,
                DropItemAlreadyExists = dropitemexsits,//2015/10/10修正
                //艦隊所属艦
                FleetShipName = APIPort.DeckPorts[APIReqMap.SallyDeckPort -1 ].api_ship.Select(getShipName).Where(x => x != "").ToArray(),
                FleetShipLevel =APIPort.DeckPorts[APIReqMap.SallyDeckPort -1 ].api_ship.Select(getShipLevel).Where(x => x!= -1).ToArray(),
                //連合艦隊所属艦
                FleetCombinedShipName = !view.IsCombined ? null : 
                    APIPort.DeckPorts[Math.Min(APIReqMap.SallyDeckPort, APIPort.DeckPorts.Count -1)].api_ship.Select(getShipName).Where(x => x != "").ToArray(),
                FleetCombinedShipLevel = !view.IsCombined ? null :
                    APIPort.DeckPorts[Math.Min(APIReqMap.SallyDeckPort, APIPort.DeckPorts.Count -1)].api_ship.Select(getShipLevel).Where(x => x!= -1).ToArray(),
                //日時
                DropDate = DateTime.Now,
                //敵のID
                //EnemyFleetID = view.EnemyID, 2015/7/17で廃止
                EnemyFleetLocalID = view.EnemyLocalID,
                EnemyFleetLocalShortID = view.EnemyLocalShortID,
                //ドロップを封じているかどうか
                DropDisabled = !((APIPort.ShipsDictionary.Count < APIPort.Basic.api_max_chara) && //キャラの条件
                                 (APIPort.Basic.api_max_slotitem 
                                    - APIGetMember.GetSlotitemNumOnSortie()//装備空きが3より大きい場合落ちる（maxslot + 3 で最大装備数）
                                    > 0)),
            };
            //DBに追加
            Collection.DataBase.Add(drop);
            //--マスターデータに船の名前を追加
            if(dropshipid != -1) Collection.MasterDropShipHeader[dropshipid] = APIMaster.MstShips[dropshipid].api_name;
            //--マスターデータにアイテムの名前を追加
            if (dropuseitemobj != null) Collection.MasterDropItemHeader[dropitemid] = dropuseitemobj.api_name;
            //--マスターデータのヘッダーに追加しインデックスの作成
            DropRecordCollection.MasterHeader mstheader = Collection.MasterMapHeader;
            //エリアの取得
            DropRecordCollection.MasterHeaderArea mstarea;
            if(!mstheader.ChildrenAreas.ContainsKey(view.AreaID))
            {
                mstarea = new DropRecordCollection.MasterHeaderArea();
                mstarea.ParentMaster = mstheader;
                mstheader.ChildrenAreas[view.AreaID] = mstarea;
            }
            else
            {
                mstarea = mstheader.ChildrenAreas[view.AreaID];
            }
            //マップの取得
            DropRecordCollection.MasterHeaderMap mstmap;
            if(!mstarea.ChildrenMaps.ContainsKey(view.MapID))
            {
                mstmap = new DropRecordCollection.MasterHeaderMap();
                mstmap.ParentArea = mstarea;
                mstmap.MapName = mapname;
                mstarea.ChildrenMaps[view.MapID] = mstmap;
            }
            else
            {
                mstmap = mstarea.ChildrenMaps[view.MapID];
            }
            //セルの取得
            DropRecordCollection.MasterHeaderCell mstcell;
            if(!mstmap.ChildrenCells.ContainsKey(view.CellID))
            {
                mstcell = new DropRecordCollection.MasterHeaderCell();
                mstcell.ParentMap = mstmap;
                mstcell.EnemyFleetName = enemyfleetname;
                mstcell.IsBoss = view.BossFlag == 2;
                mstmap.ChildrenCells[view.CellID] = mstcell;
            }
            else
            {
                mstcell = mstmap.ChildrenCells[view.CellID];
            }
            //敵編成ID取得の書き込み（もう少し丁寧に変更）
            var idcontains = mstcell.ChildrenLocalFleets.Where(x => x.LocalID == view.EnemyLocalID).ToList();
            //変わっている場合に限り変更
            if(idcontains.Count == 0)
            {
                var item = new DropRecordCollection.EnemyFleetTable()
                {
                    LocalID = view.EnemyLocalID,
                    LocalShortID = view.EnemyLocalShortID,
                    OldID = 0,//悲しいけど
                };
                mstcell.ChildrenLocalFleets.Add(item);
            }
        }

        //難易度の変換
        internal static WinRank ConverToWinRank(string s)
        {
            switch(s)
            {
                case "S": return WinRank.S;
                case "A": return WinRank.A;
                case "B": return WinRank.B;
                case "C": return WinRank.C;
                case "D": return WinRank.D;
                case "E": return WinRank.E;
                default: return WinRank.None;
            }
        }

        //マップで集計して船のクロスセクションを出力
        public static DropDataSummarize SummarizeByMap(int areaId, int mapId, int cellId, int enemyFleetLocalId, bool exceptDropDisabled, int difficulty, WinRank winrank, DateTime startDate, DateTime endDate)
        {
            //フィルタリング
            List<DropRecord> filter = new List<DropRecord>();
            DateTime newDate = new DateTime();
            foreach(var x in Collection.DataBase)
            {
                if (areaId != -1 && x.MapAreaID != areaId) continue;
                if (mapId != -1 && x.MapInfoID != mapId) continue;
                if (cellId != -1 && x.MapCellID != cellId) continue;
                //if (enemyFleetlId != -1 && x.EnemyFleetID != enemyFleetId) continue;
                if (enemyFleetLocalId != -1 && x.EnemyFleetLocalID != enemyFleetLocalId) continue;
                if(exceptDropDisabled && x.DropDisabled) continue;
                if (difficulty != -1 && x.MapDifficulty != difficulty) continue;
                if(!winrank.HasFlag(ConverToWinRank(x.WinRank))) continue;
                if (startDate != newDate && x.DropDate < startDate) continue;
                if (endDate != newDate && x.DropDate > endDate) continue;
                filter.Add(x);
            }
            Dictionary<int, SummarizeByWinRank> table = new Dictionary<int, SummarizeByWinRank>();
            //勝利ランクごとに集計
            foreach(var x in filter)
            {
                //ドロップ艦
                if(!table.ContainsKey(x.DropShipID))
                {
                    table[x.DropShipID] = new SummarizeByWinRank();
                    table[x.DropShipID].Records = new List<DropRecord>();
                }
                table[x.DropShipID].AddValue(x.WinRank, false);//なしはなしとして集計するためここではfalse
                table[x.DropShipID].Records.Add(x);
                //ドロップアイテム
                if(x.DropItemFlag)
                {
                    int convertItemId = x.DropItemID * (-1);
                    if(!table.ContainsKey(convertItemId))
                    {
                        table[convertItemId] = new SummarizeByWinRank();
                        table[convertItemId].Records = new List<DropRecord>();
                    }
                    table[convertItemId].AddValue(x.WinRank, false);
                    table[convertItemId].Records.Add(x);
                }
            }
            //サンプル数合計
            var cnttotal = table.Values.Select(x => x.Total).Sum();
            //サマリーの出力
            var summary = table.Select(delegate(KeyValuePair<int, SummarizeByWinRank> pair)
            {
                //アイテム・艦名を取得
                string keyname = "不明";
                //アイテムの場合
                if(pair.Key < -1)
                {
                    if(!Collection.MasterDropItemHeader.TryGetValue(pair.Key * -1, out keyname)) keyname = "不明";
                }
                else if(pair.Key == -1)
                {
                    keyname = "なし";
                }
                //艦の場合
                else if(pair.Key > 0)
                {
                    if (!Collection.MasterDropShipHeader.TryGetValue(pair.Key, out keyname)) keyname = "不明";
                }

                return new DropDataSummarizeRow()
                {
                    CorrespondingRecord = pair.Value.Records,
                    Item = keyname,
                    NumS = pair.Value.CountS,
                    NumA = pair.Value.CountA,
                    NumB = pair.Value.CountB,
                    NumNone = pair.Value.CountNone,
                    NumTotal = pair.Value.Total,
                    Percentage = (double)(pair.Value.Total - pair.Value.CountNone) / (double)cnttotal,
                };
            });
            //返り値
            return new DropDataSummarize()
            {
                Headers = new string[] { "艦名・アイテム名", "件数", "S", "A", "B", "なし", "%" },
                Mode = DropDataSummarizeMode.ByMap,
                Rows = summary.OrderByDescending(x => x.NumTotal).ToList(),
            };
        }

        //船ごとに集計してマップごとにクロスセクション
        public static DropDataSummarize SummarizeByShip(int shipid, int itemid, bool mergeByCell, bool exceptDropDisabled, int difficulty, WinRank winrank, DateTime startDate, DateTime endDate)
        {
            //日付けや勝利条件でまずフィルタリング
            List<DropRecord> filter = new List<DropRecord>();
            DateTime newDate = new DateTime();
            foreach(var x in Collection.DataBase)
            {
                if (difficulty != -1 && x.MapDifficulty != difficulty) continue;
                if (!winrank.HasFlag(ConverToWinRank(x.WinRank))) continue;
                if (startDate != newDate && x.DropDate < startDate) continue;
                if (endDate != newDate && x.DropDate > endDate) continue;
                filter.Add(x);
            }
            //編成をセル単位でマージした場合のハッシュ
            Func<int, int, int, int> maphash = (int areaid, int mapid, int cellid) =>
                {
                    return (cellid + 1000 * mapid + 100000 * areaid);
                };
            //該当艦が含まれるハッシュを抽出する
            HashSet<int> hashset = new HashSet<int>();
            foreach(var x in filter)
            {
                if((shipid != -1 && x.DropShipID == shipid) || (itemid != -1 && x.DropItemID == itemid))
                {
                    if (mergeByCell) hashset.Add(maphash(x.MapAreaID, x.MapInfoID, x.MapCellID));
                    else hashset.Add(x.EnemyFleetLocalShortID);
                }
            }
            //ハッシュが含まれるセル・IDでフィルタリング
            var query = filter
                .Where(x => hashset.Any(h => h == (mergeByCell ? maphash(x.MapAreaID, x.MapInfoID, x.MapCellID) : x.EnemyFleetLocalShortID)));
            //勝利ごとに集計
            Dictionary<string, SummarizeByWinRank> table = new Dictionary<string, SummarizeByWinRank>();
            foreach(var x in query)
            {
                string key = mergeByCell ? string.Format("{0}-{1}-{2}", x.MapAreaID, x.MapInfoID, x.MapCellID) : x.EnemyFleetLocalShortID.ToString();
                bool dropnothing = false;
                if (shipid != -1) dropnothing = x.DropShipID != shipid;//該当艦以外はドロップなしとみなす
                if (itemid != -1) dropnothing = x.DropItemID != itemid;
                if(!table.ContainsKey(key))
                {
                    table[key] = new SummarizeByWinRank();
                    table[key].Records = new List<DropRecord>();
                }
                table[key].AddValue(x.WinRank, dropnothing);
                table[key].Records.Add(x);
            }
            //サンプル数合計
            var cnttotal = table.Values.Select(x => x.Total).Sum();
            //サマリーの出力
            var summary = table.Values.Select(delegate(SummarizeByWinRank row)
            {
                DropRecord first = row.Records[0];
                string enemyfleetname = Collection.MasterMapHeader.ChildrenAreas[first.MapAreaID].ChildrenMaps[first.MapInfoID].ChildrenCells[first.MapCellID].EnemyFleetName;
                return new DropDataSummarizeRow()
                {
                    CorrespondingRecord = row.Records,
                    Item = mergeByCell ?
                        string.Format("{0}{1}-{2}-{3}:{4}", first.BossFlag ? "★" : "", first.MapAreaID, first.MapInfoID, first.MapCellID, enemyfleetname) :
                        string.Format("{0}({1}{2}-{3}-{4}:{5})", first.EnemyFleetLocalShortID, first.BossFlag ? "★" : "", first.MapAreaID, first.MapInfoID, first.MapCellID, enemyfleetname),
                    NumS = row.CountS,
                    NumA = row.CountA,
                    NumB = row.CountB,
                    NumNone = row.CountNone,
                    NumTotal = row.Total,
                    Percentage = (double)(row.Total - row.CountNone) / (double)row.Total,
                };
            });
            //返り値
            return new DropDataSummarize()
            {
                Headers = new string[] { "海域", "件数", "S", "A", "B", "なし", "%" },
                Mode = DropDataSummarizeMode.ByShip,
                Rows = summary.OrderByDescending(x => x.NumTotal).ToList(),
            };
        }

        //コンバート用のテキストの作成
        public static string MakeConvertText(IEnumerable<DropRecord> records,  Dictionary<int, string> dropshipheader, Dictionary<int, string> dropitemheader)
        {
            StringBuilder sb = new StringBuilder();
            CsvList<string> header = new CsvList<string>();
            foreach (string x in DropRecord.LogConvertHeader)
            {
                header.Add(x);
            }
            sb.AppendLine(string.Join(",", header.ToArray()));
            //個々のデータ
            foreach (var x in records)
            {
                //--行の追加
                sb.AppendLine(x.MakeLogConvertText(dropshipheader, dropitemheader));
            }
            //帰り値
            return sb.ToString();
        }
    }
}
