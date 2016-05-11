using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HoppoAlpha.DataLibrary;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest
{
    static class SortieReportDataBase
    {
        public static bool IsInited { get; set; }
        //直近のデータ
        public static SortieReportCollection Reports { get; private set; }
        public static string LastSavedFileName { get; set; }
        public static string LastSavedDirectory { get; set; }
        //バッファー
        public static SortieReportBuffer Buffer { get; private set; }

        //内部クラス
        #region 内部クラス
        //出撃報告書を作るのにバッファリングするクラス
        public class SortieReportBuffer
        {
            public SortieReportMapHash MapHash { get; set; }
            public SortieReportHash DeckHash { get; set; }

            public bool IsBossApproached { get; set; }
            public bool IsBossDefeated { get; set; }
            public bool IsBossSWin { get; set; }

            public bool OnReturnAccepted { get; set; }//帰投したときの処理を受け入れるかどうか
            public bool SortieCompleted { get; set; }//帰投が完了したかどうか

            public int FlagshipExp { get; set; }
            public int TotalExp { get; set; }
            public int PlayerExp {get; set;}
            public int CombinedFlagshipExp { get; set; }
            public int CombinedTotalExp { get; set; }

            public int[] MVPCount { get; set; }
            public int[] CombinedMVPCount { get; set; }

            public List<MaterialBuffer> Material { get; set; }

            //public List<int> ShipIdBuffer { get; set; }//出撃中の編成に含まれる船のID
            public static Dictionary<int, SortieReportItem> ShipIdPublicBuffer { get; set; }//広域なIDバッファー（遅延補給用）
            public static MaterialRecord DelaySupplyBuffer { get; set; }//遅延補給用の資源バッファー 

            public DateTime BeforeSortie { get; set; }
            public DateTime AfterSortie { get; set; }

            public static Dictionary<int, string> MissionNameBuffer { get; set; }//遠征名のキャッシュ

            public SortieReportBuffer()
            {
                this.MVPCount = new int[6];
                this.CombinedMVPCount = new int[6];

                this.Material = new List<MaterialBuffer>();
            }

            static SortieReportBuffer()
            {
                MissionNameBuffer = new Dictionary<int, string>();
                ShipIdPublicBuffer = new Dictionary<int, SortieReportItem>();
            }

            //資源レコードの追加
            public bool AddMaterialRecord(MaterialUsageType usage, bool isAfter, int firstSupplyShipId = -1)
            {
                //出撃が完了してなかったら補給と入渠は追加しない
                if(usage == MaterialUsageType.Charge || usage == MaterialUsageType.Repair)
                {
                    if (!SortieCompleted) return false;
                }
                //補給の通常補給と遅延補給の分岐
                if (usage == MaterialUsageType.Charge)
                {
                    //補給対象の場合
                    if(ShipIdPublicBuffer.ContainsKey(firstSupplyShipId))
                    {
                        var supplyTarget = ShipIdPublicBuffer[firstSupplyShipId];
                        //遅延補給
                        if(supplyTarget != null)
                        {
                            //Beforeの場合
                            if (!isAfter)
                            {
                                DelaySupplyBuffer = MaterialRecord.Load();
                                return true;
                            }
                            //Afterで正常に処理できる場合
                            else if (DelaySupplyBuffer != null)
                            {
                                var after = MaterialRecord.Load();
                                foreach(var i in Enumerable.Range(0,8))
                                {
                                    supplyTarget.MaterialSupply[i] += after[i] - DelaySupplyBuffer[i];
                                }
                                ShipIdPublicBuffer.Remove(firstSupplyShipId);
                                return true;
                            }
                            //おかしい場合
                            else
                            {
                                ShipIdPublicBuffer.Remove(firstSupplyShipId);
                                return false;
                            }
                        }
                        //
                        //通常補給対象
                        // （下に流れる）
                    }
                    //補給対象ではなかった場合
                    else
                    {
                        return false;
                    }
                }

                //通常補給
                //Beforeに追加する場合
                if(!isAfter)
                {
                    //前のレコードが閉じているか
                    if(Material.Count > 0)
                    {
                        var last = Material[Material.Count - 1];
                        if(last.Before != null && last.After == null)
                        {
                            //前のレコードが閉じていない場合変化なしとして閉じる
                            last.After = (MaterialRecord)last.Before.Select(x => x).ToList();
                            Material[Material.Count - 1] = last;
                        }
                    }
                    //新規に追加できる場合
                    var buf = new MaterialBuffer();
                    buf.Usage = usage;
                    buf.Before = MaterialRecord.Load();
                    this.Material.Add(buf);
                }
                //Afterに書き込む場合
                else
                {
                    //Beforeのレコードがない場合
                    if(Material.Count == 0)
                    {
                        var buf = new MaterialBuffer();
                        buf.Usage = usage;
                        buf.Before = MaterialRecord.Load();
                        buf.After = MaterialRecord.Load();
                        this.Material.Add(buf);
                    }
                    else
                    {
                        var last = Material[Material.Count - 1];
                        //レコードが1件以上あるが前のレコードとTypeが違う場合
                        if(last.Usage != usage)
                        {
                            //異なるレコードが閉じていない場合、閉じる
                            if(last.After == null && last.Before != null)
                            {
                                last.After = (MaterialRecord)last.Before.Select(x => x).ToList();
                                Material[Material.Count - 1] = last;
                            }
                            //新規にレコード追加
                            var buf = new MaterialBuffer();
                            buf.Usage = usage;
                            buf.Before = MaterialRecord.Load();
                            buf.After = MaterialRecord.Load();
                            this.Material.Add(buf);
                        }
                        //これが正常なパターン
                        else
                        {
                            last.After = MaterialRecord.Load();
                            this.Material[Material.Count - 1] = last;
                        }
                    }
                    //バッファーからIDを消す
                    ShipIdPublicBuffer.Remove(firstSupplyShipId);
                }
                return true;
            }

            //戦闘結果のセット
            public bool SetBattleResult
                (bool isBoss, string winrank, 
                List<int> getShipExp, int mvp, int playerExp, List<int> shipLv,
                List<int> getShipExpCombined = null, int mvpCombined = -1, List<int> shipLvCombined = null)
            {
                //出撃後なら追加しない
                if (SortieCompleted) return false;
                //フラグ関連
                if(isBoss)
                {
                    IsBossApproached = true;
                    IsBossDefeated = winrank == "S" || winrank == "A" || winrank == "B";
                    IsBossSWin = winrank == "S";
                }
                //キャラの経験値
                if(getShipExp != null && shipLv != null)
                {
                    for (int i = 1; i < Math.Min(Math.Min(getShipExp.Count, shipLv.Count), 7); i++)
                    {
                        if(shipLv[i] != 99 && shipLv[i] !=150)
                        {
                            //旗艦経験値
                            if (i == 1) FlagshipExp += getShipExp[i];
                            //合計経験値
                            TotalExp += getShipExp[i];
                        }
                    }
                }
                //連合艦隊の経験値
                if(getShipExpCombined != null && shipLvCombined != null)
                {
                    for (int i = 1; i < Math.Min(Math.Min(getShipExpCombined.Count, shipLvCombined.Count), 7); i++)
                    {
                        if(shipLvCombined[i] != 99 && shipLvCombined[i] != 150)
                        {
                            //旗艦経験値
                            if (i == 1) CombinedFlagshipExp += getShipExpCombined[i];
                            //合計経験値
                            CombinedTotalExp += getShipExpCombined[i];
                        }
                    }
                }
                //提督経験値
                PlayerExp += playerExp;
                //MVP
                if (mvp >= 1 && mvp <= 6) MVPCount[mvp - 1]++;
                if (mvpCombined >= 1 && mvpCombined <= 6) CombinedMVPCount[mvpCombined - 1]++;

                return true;
            }

            //出撃時にセット
            public bool OnStartSortie
                (int mapAreaId, int mapInfoId, bool isCombined, int sallyFleetNumber)
            {
                //帰投後ならセットしない
                if (SortieCompleted) return false;

                //ハッシュ
                MapHash = new SortieReportMapHash()
                {
                    MapAreaID = mapAreaId,
                    MapInfoID = mapInfoId,
                };
                DeckHash = SortieReportHash.CreateInstance(APIPort.DeckPorts, APIPort.ShipsDictionary,
                    APIGetMember.SlotItemsDictionary, isCombined, sallyFleetNumber);
                //資源
                AddMaterialRecord(MaterialUsageType.MapStartEnd, false);
                //時間
                BeforeSortie = DateTime.Now;

                //出撃した船のIDを登録
                if (sallyFleetNumber - 1 < APIPort.DeckPorts.Count)
                {
                    foreach (var s in APIPort.DeckPorts[sallyFleetNumber - 1].api_ship) ShipIdPublicBuffer[s] = null;
                    //ShipIdBuffer.AddRange(APIPort.DeckPorts[sallyFleetNumber - 1].api_ship);
                }
                if (isCombined && APIPort.DeckPorts.Count >= 2)
                {
                    foreach (var s in APIPort.DeckPorts[1].api_ship) ShipIdPublicBuffer[s] = null;
                    //ShipIdBuffer.AddRange(APIPort.DeckPorts[1].api_ship);
                }

                //遠征で支援任務があれば入れる
                foreach(var f in APIPort.DeckPorts)
                {
                    if(f.api_mission.Count >= 2)
                    {
                        int missionid = (int)f.api_mission[1];//mission[1]が遠征ID
                        //遠征名の取得
                        string missionname;
                        //名前がバッファリングされている場合は取得
                        if(!MissionNameBuffer.TryGetValue(missionid, out missionname))
                        {
                            var omission = APIMaster.MstMissions.Where(x => x.api_id == missionid).FirstOrDefault();
                            if (omission != null)
                            {
                                missionname = omission.api_name;
                                MissionNameBuffer[missionid] = missionname;
                            }
                        }
                        if(missionname != null)
                        {
                            //支援艦隊だったらIDを追加
                            if(missionname == "前衛支援任務" | missionname == "艦隊決戦支援任務")
                            {
                                foreach (var s in f.api_ship) ShipIdPublicBuffer[s] = null;
                                //ShipIdBuffer.AddRange(f.api_ship);
                            }
                        }
                    }
                }

                //帰投処理を1回のみ受け入れる
                OnReturnAccepted = true;

                return true;
            }

            //帰投時にセット
            public bool OnReturningPort()
            {
                if (!OnReturnAccepted) return false;
                //資源
                AddMaterialRecord(MaterialUsageType.MapStartEnd, true);
                //時間
                AfterSortie = DateTime.Now;

                //帰投処理を無効にする
                OnReturnAccepted = false;
                //帰投完了
                SortieCompleted = true;

                return true;
            }
        }

        public class MaterialRecord : List<int>
        {
            public MaterialRecord()
                : base()
            {
                this.AddRange(Enumerable.Repeat(0, 8));
            }

            public static MaterialRecord Load()
            {
                var result = new MaterialRecord();

                foreach(int i in Enumerable.Range(0, Math.Min(APIPort.Materials.Count, 8)))
                {
                    result[i] = APIPort.Materials[i].api_value;
                }

                return result;
            }
        }

        public class MaterialBuffer
        {
            public MaterialUsageType Usage {get; set;}
            public MaterialRecord Before {get; set;}
            public MaterialRecord After {get; set;}
        }

        //資源がどの要因で増えたか
        public enum MaterialUsageType
        {
            MapStartEnd,//出撃帰投時に取得
            Repair,//修理により取得
            Charge,//補給により取得
        }
        #endregion

        //ファイル関連
        #region ファイル関連
        public static string GetDirectory(DateTime date)
        {
            if (APIPort.Basic == null) return null;

            DateTime delay = date.AddHours(-5);//デイリーにあわせる
            return @"user/" + APIPort.Basic.api_member_id + @"/sortie/" + delay.ToString("yyyy") + @"/";
        }

        public static string GetFileName(DateTime date)
        {
            string dir = GetDirectory(date);
            return dir + string.Format("sortie{0}_{1}.dat",
                date.AddHours(-5).Year, SortieReportCollection.Helper.GetNowWeeklyIndex(date));
        }

        public static void Init()
        {
            if (IsInited) return;
            //初期化
            DateTime now = DateTime.Now;
            InitReport(now);
            //読み込み
            string dir = GetDirectory(now);
            string file = GetFileName(now);

            var loadResult = Files.TryLoad(file, DataType.SortieReport);
            LogSystem.AddLogMessage(DataType.SortieReport, loadResult, false);
            
            if(loadResult.IsSuccess) Reports = (SortieReportCollection)loadResult.Instance;

            //ファイル名
            LastSavedDirectory = dir;
            LastSavedFileName = file;
            //初期化フラグ
            IsInited = true;
        }

        //報告書の初期化
        private static void InitReport(DateTime now)
        {
            Reports = new SortieReportCollection(now);

            Buffer = new SortieReportBuffer();

            SortieReportBuffer.ShipIdPublicBuffer = new Dictionary<int, SortieReportItem>();
        }

        public static void Save(bool onclosing)
        {
            if (!IsInited || Reports == null) return;
            //週またぎチェック
            DateTime now = DateTime.Now;
            string dir = GetDirectory(now);
            string file = GetFileName(now);
            if(file != LastSavedFileName)
            {
                SwitchFile();
            }
            else
            {
                SaveAs(dir, file, onclosing, false);
            }
        }

        private static void SaveAs(string dir, string filename, bool enforeceFinalize, bool onFlieSwitched)
        {
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            //バッファーのファイナライズを強制するか
            if (enforeceFinalize && !onFlieSwitched) FinalizeBuffer();

            var saveResult = Files.Save(filename, DataType.SortieReport, Reports);
            LogSystem.AddLogMessage(DataType.SortieReport, saveResult, true);

            LastSavedDirectory = dir;
            LastSavedFileName = filename;
        }

        //ファイルの切り替え処理
        public static void SwitchFile()
        {
            //既存ログの保存
            SaveAs(LastSavedDirectory, LastSavedFileName, true, true);
            //初期化
            DateTime now = DateTime.Now;
            InitReport(now);
            //次のファイル名に
            LastSavedDirectory = GetDirectory(now);
            LastSavedFileName = GetFileName(now);
        }
        #endregion

        //バッファーのファイナライズ（コレクションに追加）
        public static void FinalizeBuffer()
        {
            if(Buffer.SortieCompleted)
            {
                SortieReport maptarget = null;
                SortieReportItem itemtarget = null;
                if(Reports.Collection.TryGetValue(Buffer.MapHash, out maptarget))
                {
                    maptarget.Sorties.TryGetValue(Buffer.DeckHash, out itemtarget);
                }
                if(maptarget == null)
                {
                    maptarget = new SortieReport(true);
                    maptarget.Map = Buffer.MapHash;
                }
                if(itemtarget == null)
                {
                    itemtarget = new SortieReportItem(true);
                    itemtarget.Deck = Buffer.DeckHash;
                }

                //--バッファーの値を書き込み
                //回数
                itemtarget.NumSortie++;
                if (Buffer.IsBossApproached) itemtarget.NumBossApproached++;
                if (Buffer.IsBossDefeated) itemtarget.NumBossDefeated++;
                if (Buffer.IsBossSWin) itemtarget.NumBossSWin++;

                //経験値
                itemtarget.FlagshipExp += Buffer.FlagshipExp;
                itemtarget.TotalExp += Buffer.TotalExp;
                itemtarget.PlayerExp += Buffer.PlayerExp;
                itemtarget.CombinedFlagshipExp += Buffer.CombinedFlagshipExp;
                itemtarget.CombinedTotalExp += Buffer.CombinedTotalExp;

                //MVP回数
                for(int i=0; i<6; i++)
                {
                    itemtarget.MVPCount[i] += Buffer.MVPCount[i];
                    itemtarget.CombinedMVPCount[i] += Buffer.CombinedMVPCount[i];
                }

                //資源
                foreach(var b in Buffer.Material)
                {
                    switch(b.Usage)
                    {
                        case MaterialUsageType.Charge:
                            foreach(int i in Enumerable.Range(0,8))
                                itemtarget.MaterialSupply[i] += b.After[i] - b.Before[i];
                            break;
                        case MaterialUsageType.MapStartEnd:
                            foreach (int i in Enumerable.Range(0, 8))
                                itemtarget.MaterialEarn[i] += b.After[i] - b.Before[i];
                            break;
                        case MaterialUsageType.Repair:
                            foreach (int i in Enumerable.Range(0, 8))
                                itemtarget.MaterialRepair[i] += b.After[i] - b.Before[i];
                            break;
                    }
                }

                //時間
                TimeSpan span = Buffer.AfterSortie - Buffer.BeforeSortie;
                if (span.TotalMinutes >= 20.0) span = new TimeSpan(0, 20, 0);//20分以上は頭打ちさせる温情

                itemtarget.TimeSortie.Add(span);
                if (Buffer.IsBossApproached) itemtarget.TimeBossApproached.Add(span);
                if (Buffer.IsBossDefeated) itemtarget.TimeBossDefeated.Add(span);
                if (Buffer.IsBossSWin) itemtarget.TimeBossSWin.Add(span);

                //遅延補給用にIDバッファーを待機させる
                var keys = SortieReportBuffer.ShipIdPublicBuffer.Keys.Select(x => x).ToList();
                foreach(var k in keys)
                {
                    if (SortieReportBuffer.ShipIdPublicBuffer[k] == null) SortieReportBuffer.ShipIdPublicBuffer[k] = itemtarget;
                }

                //ファイルまたぎチェック
                DateTime now = DateTime.Now;
                string file = GetFileName(now);
                if(file != LastSavedFileName)
                {
                    SwitchFile();
                }
                //---レポートに書き込み
                maptarget.Sorties[itemtarget.Deck] = itemtarget;
                Reports.Collection[maptarget.Map] = maptarget;
            }
        }

        //出撃開始
        public static void SetStartSortie()
        {
            //以前のバッファーがある場合はファイナライズ
            if (Buffer.SortieCompleted) FinalizeBuffer();
            //バッファーの初期化
            Buffer = new SortieReportBuffer();
            //パラメーターの取得
            int areaid = APIBattle.BattleView.AreaID;
            int mapid = APIBattle.BattleView.MapID;
            int sallyfleet = APIReqMap.SallyDeckPort;
            bool iscombined = APIPort.CombinedFlag != 0 && sallyfleet == 1;
            //出撃プロセス
            bool debug = Buffer.OnStartSortie(areaid, mapid, iscombined, sallyfleet);
        }

        //戦闘結果
        public static void SetBattleResult(string winrank, 
                List<int> getShipExp, int mvp, int playerExp, List<int> shipLv,
                List<int> getShipExpCombined = null, int mvpCombined = -1, List<int> shipLvCombined = null)
        {
            //ボスかどうか
            bool isBoss = APIBattle.BattleView.BossFlag == 2;
            //戦闘結果プロセス
            bool debug = Buffer.SetBattleResult(isBoss, winrank, getShipExp, mvp, playerExp, shipLv, getShipExpCombined, mvpCombined, shipLvCombined);
        }

        //帰投
        public static void SetEndSortie()
        {
            //帰投プロセス
            bool debug = Buffer.OnReturningPort();
        }

        //補給(isAfterの値を変えて2回読むこと)
        public static void SetSupply(bool isAfter, int firstSupplyShipId)
        {
            //補給
            bool debug = Buffer.AddMaterialRecord(MaterialUsageType.Charge, isAfter, firstSupplyShipId);
        }

        //入渠(isAfterの値を変えて2回読むこと)
        public static void SetRepair(bool isAfter)
        {
            //入渠
            bool debug = Buffer.AddMaterialRecord(MaterialUsageType.Repair, isAfter);
        }
    }
}
