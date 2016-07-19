using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.DataObject;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using HoppoAlpha.DataLibrary.RawApi.ApiReqRanking;
using HoppoAlpha.DataLibrary.Const;
using Excel = Microsoft.Office.Interop.Excel;

namespace VisualFormTest
{
    public class LogConvertLogic
    {
        //ターゲットのファイル
        public ExportFile[] Files { get; set; }
        //出力ディレクトリ
        public string OutputDirectory { get; set; }
        //フルモードかどうか
        public bool FullMode { get; set; }

        //内部クラス
        #region 内部クラス
        //ファイルモード
        public enum ExportMode
        {
            None, Experience, Material, Ranking, Senka, FleetDB, DropDB,
        }

        //ファイルのデータ
        public class ExportFile
        {
            public string InputFileName { get; set; }
            public ExportMode Mode { get; set; }
        }

        //編成をマップごとに集計するクラス
        public class FleetRecordCrossSection
        {
            public List<EnemyFleetRecord> Records { get; set; }
            public int AreaID { get; set; }//5-4の5
            public int MapID { get; set; }//5-4の4
            public string DisplaySheetName { get; set; }//これをキーにする

            public FleetRecordCrossSection(int areaid, int mapid, EnemyFleetRecord record)
            {
                this.AreaID = areaid;
                this.MapID = mapid;
                this.DisplaySheetName = GetDisplayName(areaid, mapid);

                this.Records = new List<EnemyFleetRecord>();
                if (record != null) this.Records.Add(record);
            }

            public static string GetDisplayName(int areaid, int mapid)
            {
                if (areaid == -1 && mapid == -1) return "ALL";
                else if (areaid <= 0 || mapid <= 0) return "others";
                return string.Format("{0}-{1}", areaid, mapid);
            }
        }

        //マスターShipと装備を組み合わせるクラス
        public class MasterShipAndESlotCombine
        {
            public ExMasterShip MstShip { get; set; }
            public Status UnitStatus { get; set; }
            public Status IncludingStatus { get; set; }
            public Status SlotStatus { get; set; }

            public class Status
            {
                public int HP { get; set; }
                public int Fire { get; set; }
                public int Defense { get; set; }
                public int Torpedo { get; set; }
                public int? Evasion { get; set; }//5
                public int AntiAir { get; set; }
                public int EqSum { get; set; }
                public int? AntiSub { get; set; }
                public string Speed { get; set; }
                public int? Search { get; set; }//10
                public string Range { get; set; }
                public int Luck { get; set; }
                public int AirSup { get; set; }//13

                //SlotStatus用
                public Status()
                {
                    //装備だけのステータスを呼ぶ用なのでNull許容型を初期化
                    this.Evasion = 0;
                    this.AntiSub = 0;
                    this.Search = 0;
                    this.Speed = "低速";
                    this.Range = "なし";
                }

                //int?をそのまま
                public Status(bool flag)
                {
                    this.Speed = "低速";
                    this.Range = "なし";
                }

                //UnitStatus用
                public Status(ExMasterShip dship)
                {
                    this.HP = dship.api_taik != null ? dship.api_taik[0] : 0;//ここあとで修正
                    this.Fire = dship.api_houg != null ? dship.api_houg[0] : 0;
                    this.Defense = dship.api_souk != null ? dship.api_souk[0] : 0;
                    this.Torpedo = dship.api_raig != null ? dship.api_raig[0] : 0;
                    if (dship.Evation != null) this.Evasion = dship.Evation[0];
                    this.AntiAir = dship.api_tyku != null ? dship.api_tyku[0] : 0;
                    if(dship.api_maxeq != null) this.EqSum = dship.api_maxeq.Where(x => x > 0).Sum();
                    if (dship.AntiSub != null) this.AntiSub = dship.AntiSub[0];
                    this.Speed = Helper.MstShipSpeedToString(dship.api_soku);
                    if (dship.Search != null) this.Search = dship.Search[0];
                    this.Range = Helper.MstSlotitemLengthToString(dship.api_leng);
                    this.Luck = dship.api_luck != null ? dship.api_luck[0] : 0;
                }

                public dynamic GetValue(StatusEnum senum)
                {
                    switch(senum)
                    {
                        case StatusEnum.HP: return this.HP;
                        case StatusEnum.Fire: return this.Fire;
                        case StatusEnum.Defense: return this.Defense;
                        case StatusEnum.Torpedo: return this.Torpedo;
                        case StatusEnum.Evasion: return this.Evasion;//?//5
                        case StatusEnum.AntiAir: return this.AntiAir;
                        case StatusEnum.EqSum: return this.EqSum;
                        case StatusEnum.AntiSub: return this.AntiSub;//?
                        case StatusEnum.Speed: return this.Speed;
                        case StatusEnum.Search: return this.Search;//?//10
                        case StatusEnum.Range: return this.Range;
                        case StatusEnum.Luck: return this.Luck;
                        case StatusEnum.AirSup: return this.AirSup;//13
                        default: throw new ArgumentException();
                    }
                }

                public static dynamic GetMstSlotItemValue(ApiMstSlotitem dslot, StatusEnum senum)
                {
                    switch(senum)
                    {
                        case StatusEnum.HP: return dslot.api_taik;
                        case StatusEnum.Fire: return dslot.api_houg;
                        case StatusEnum.Defense: return dslot.api_souk;
                        case StatusEnum.Torpedo: return dslot.api_raig;
                        case StatusEnum.Evasion: return dslot.api_houk;//5
                        case StatusEnum.AntiAir: return dslot.api_tyku;
                        case StatusEnum.EqSum: return 0;
                        case StatusEnum.AntiSub: return dslot.api_tais;//?
                        case StatusEnum.Speed: return Helper.MstShipSpeedToString(dslot.api_soku);
                        case StatusEnum.Search: return dslot.api_saku;//?//10
                        case StatusEnum.Range: return Helper.MstSlotitemLengthToString(dslot.api_leng);
                        case StatusEnum.Luck: return dslot.api_luck;
                        case StatusEnum.AirSup: return 0;//13
                        default: throw new ArgumentException();
                    }
                }

                public void ApeendValue(StatusEnum senum, dynamic val)
                {
                    //型チェック
                    dynamic original = GetValue(senum);
                    if (original == null || val == null) return;
                    if (original is int && !(val is int)) throw new ArgumentException();
                    if (original is string && !(val is string)) throw new ArgumentException();
                    //追加
                    switch(senum)
                    {
                        case StatusEnum.HP: this.HP += val; break;
                        case StatusEnum.Fire: this.Fire += val; break;
                        case StatusEnum.Defense: this.Defense += val; break;
                        case StatusEnum.Torpedo: this.Torpedo += val; break;
                        case StatusEnum.Evasion: if(this.Evasion != null) this.Evasion += val; break;//?//5
                        case StatusEnum.AntiAir: this.AntiAir += val; break;
                        case StatusEnum.EqSum: this.EqSum += val; break;
                        case StatusEnum.AntiSub: if(this.AntiSub != null) this.AntiSub += val; break;//?
                        case StatusEnum.Speed: if (this.Speed == "高速" || val == "高速") this.Speed = "高速"; break;
                        case StatusEnum.Search: if (this.Search != null) this.Search += val; break;//10
                        case StatusEnum.Range:
                            string[] ranges = new string[] { "なし", "短", "中", "長", "超長", "超超長" };
                            if (this.Range == null) break;
                            this.Range = Helper.MstSlotitemLengthToString(Math.Max(Array.IndexOf(ranges, this.Range), Array.IndexOf(ranges, val)));
                            break;
                        case StatusEnum.Luck: this.Luck += val; break;
                        case StatusEnum.AirSup: break;//制空値はなにもしない 13
                        default: throw new ArgumentException();
                    }
                }

                public void SetValue(StatusEnum senum, dynamic val)
                {
                    //型チェック
                    dynamic original = GetValue(senum);
                    //if (val == null) throw new ArgumentException();
                    if (original is int && !(val is int)) throw new ArgumentException();
                    if (original is string && !(val is string)) throw new ArgumentException();
                    //追加
                    switch (senum)
                    {
                        case StatusEnum.HP: this.HP = val; break;
                        case StatusEnum.Fire: this.Fire = val; break;
                        case StatusEnum.Defense: this.Defense = val; break;
                        case StatusEnum.Torpedo: this.Torpedo = val; break;
                        case StatusEnum.Evasion: this.Evasion = val; break;//?//5
                        case StatusEnum.AntiAir: this.AntiAir = val; break;
                        case StatusEnum.EqSum: this.EqSum = val; break;
                        case StatusEnum.AntiSub: this.AntiSub = val; break;//?
                        case StatusEnum.Speed: this.Speed = val; break;
                        case StatusEnum.Search: this.Search = val; break;//10
                        case StatusEnum.Range: this.Range = val; break;
                        case StatusEnum.Luck: this.Luck = val; break;
                        case StatusEnum.AirSup: break;//制空値はなにもしない 13
                        default: throw new ArgumentException();
                    }
                }
            }

            public enum StatusEnum
            {
                HP, Fire, Defense, Torpedo, Evasion,
                AntiAir, EqSum, AntiSub, Speed, Search,
                Range, Luck, AirSup,
            };

            #region コンストラクタ
            //なにもなくて、編成レコードから読む場合
            public MasterShipAndESlotCombine(ExMasterShip mstship)
            {
                this.MstShip = mstship;

                //装備なしのステータス
                this.UnitStatus = new Status(mstship);
                //装備のみのステータス
                this.SlotStatus = new Status();
                //装備の反映
                StatusEnum[] enums = (StatusEnum[])Enum.GetValues(typeof(StatusEnum));
                if (mstship.DefaultSlotItem != null)
                {
                    foreach (int s in mstship.DefaultSlotItem)
                    {
                        ExMasterSlotitem slot;
                        if (!APIMaster.MstSlotitems.TryGetValue(s, out slot)) continue;

                        foreach (var x in enums)
                        {
                            //装備ステータス
                            dynamic st = Status.GetMstSlotItemValue(slot, x);
                            if (st is int && st == 0) continue;
                            //装備ステの反映
                            this.SlotStatus.ApeendValue(x, st);
                        }
                    }
                }
            }
            #endregion


            //ファイナライズして計算する
            public void FinalizeCalc()
            {
                //装備込みのステータスの計算
                this.IncludingStatus = new Status(false);
                StatusEnum[] enums = (StatusEnum[])Enum.GetValues(typeof(StatusEnum));
                //素のステータス＋装備ステータス
                foreach(var x in enums)
                {
                    if (x == StatusEnum.AirSup) continue;
                    this.IncludingStatus.SetValue(x, this.UnitStatus.GetValue(x));
                    this.IncludingStatus.ApeendValue(x, this.SlotStatus.GetValue(x));
                }
                //制空値の再計算
                this.IncludingStatus.AirSup = this.MstShip.AirSuperiority;//再計算してない
            }

            public string Display(StatusEnum senum)
            {
                dynamic inc = this.IncludingStatus.GetValue(senum);
                dynamic slot = this.SlotStatus.GetValue(senum);
                //int?の場合
                if(inc is int?)
                {
                    return string.Format("{0}{1}", inc ?? "null", (slot != null && slot > 0) ? string.Format("(+{0})", slot) : "");
                }
                //intの場合
                else if(inc is int)
                {
                    return string.Format("{0}{1}", inc, slot > 0 ? string.Format("(+{0})", slot) : "");
                }
                //stringの場合
                else if(inc is string)
                {
                    return (string)inc;
                }
                //nullの場合
                else if(inc == null)
                {
                    if (slot is int? || slot is int) return string.Format("NULL{0}", slot > 0 ? string.Format("(+{0})", slot) : "");
                    else if (slot is string) return (string)slot;
                    else if (slot == null) return "NULL";
                    else throw new FormatException();
                }
                //エラー
                else
                {
                    throw new FormatException();
                }
            }

        }
        #endregion

        //コンストラクタ
        public LogConvertLogic(string[] srcFiles, string destDirectory, bool isFullMode)
        {
            //Filesの設定
            this.Files = srcFiles.Select(delegate(string x)
            {
                //ファイル名
                ExportFile item = new ExportFile();
                string name = Path.GetFileName(x);
                item.InputFileName = x;
                //拡張子のチェック
                string extension = Path.GetExtension(x);
                if (extension != ".dat") throw new FormatException();
                //モードの設定
                if (name.Contains("experience")) item.Mode = ExportMode.Experience;
                else if (name.Contains("material")) item.Mode = ExportMode.Material;
                else if (name.Contains("ranking")) item.Mode = ExportMode.Ranking;
                else if (name.Contains("senka")) item.Mode = ExportMode.Senka;
                else if (name.Contains("enemyfleet")) item.Mode = ExportMode.FleetDB;
                else if (name.Contains("droprecord")) item.Mode = ExportMode.DropDB;
                return item;
            }).ToArray();
            //出力ディレクトリの設定
            this.OutputDirectory = destDirectory;
            //FullMode
            this.FullMode = isFullMode;
        }

        //実行
        public async Task Execute(IProgress<string> progress, System.Threading.CancellationTokenSource token)
        {
            int totalfiles = Files.Length;
            int success = 0;
            int failure = 0;
            //出力ディレクトリのチェック
            if (!Directory.Exists(this.OutputDirectory))
            {
                Directory.CreateDirectory(this.OutputDirectory);
            }
            //実行
            await Task.Factory.StartNew(() =>
                {
                    int flag = -1;
                    //出力
                    foreach (ExportFile f in this.Files)
                    {
                        if (token.IsCancellationRequested)
                        {
                            throw new OperationCanceledException();
                        }

                        switch (f.Mode)
                        {
                            case ExportMode.Experience:
                                flag = ExportExpRecord(f.InputFileName);
                                break;
                            case ExportMode.Material:
                                flag = ExportMaterialRecord(f.InputFileName);
                                break;
                            case ExportMode.Ranking:
                                flag = ExportRanking(f.InputFileName);
                                break;
                            case ExportMode.Senka:
                                flag = ExportSenkaRecord(f.InputFileName);
                                break;
                            case ExportMode.FleetDB:
                                flag = ExportFleetDB(f.InputFileName);
                                break;
                            case ExportMode.DropDB:
                                flag = ExportDropDB(f.InputFileName);
                                break;
                        }
                        //チェック
                        if (flag == 0) success++;
                        else failure++;
                        //ログの出力
                        progress.Report(string.Format("出力中:{0}個 -> 成功{1} / 失敗{2}", totalfiles, success, failure));
                    }
                });
        }

        //CSVの保存
        private bool CsvSave(string body, string filepath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filepath, false, Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write(body);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        //経験値ログの出力
        private int ExportExpRecord(string filepath)
        {
            //デシリアライズ
            List<ExpRecord> val;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    var result = HoppoAlpha.DataLibrary.Files.TryLoad(filepath, HoppoAlpha.DataLibrary.DataType.Experience);

                    if (result.IsSuccess) val = (List<ExpRecord>)result.Instance;
                    else return -1;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
            //ヘッダー
            StringBuilder sb = new StringBuilder();
            CsvList<string> header = new CsvList<string>();
            header.Add("日付");
            header.Add("提督経験値");
            header.Add("6時間前");
            header.Add("6時間前の経験値");
            header.Add("12時間前");
            header.Add("12時間前の経験値");
            header.Add("24時間前");
            header.Add("24時間前の経験値");
            sb.AppendLine(string.Join(",", header.ToArray()));
            //個々のデータ
            foreach (var x in val)
            {
                CsvList<string> row = new CsvList<string>();
                //時間
                row.Add(x.Date.ToString());
                //提督経験値
                row.Add(x.Value.ToString());
                //6時間前
                row.Add(x.Before6H == null ? "" : x.Before6H.Date.ToString());
                //6時間前の経験値
                row.Add(x.Before6H == null ? "" : x.Before6H.Value.ToString());
                //12時間前
                row.Add(x.Before12H == null ? "" : x.Before12H.Date.ToString());
                //12時間前の経験値
                row.Add(x.Before12H == null ? "" : x.Before12H.Value.ToString());
                //24時間前
                row.Add(x.Before24H == null ? "" : x.Before24H.Date.ToString());
                //24時間前の経験値
                row.Add(x.Before24H == null ? "" : x.Before24H.Value.ToString());
                //行の追加
                sb.AppendLine(string.Join(",", row.ToArray()));
            }
            //出力ファイル名
            string outputfilename = this.OutputDirectory + @"/" + Path.GetFileNameWithoutExtension(filepath) + ".csv";
            //書き込み
            bool wflag = CsvSave(sb.ToString(), outputfilename);
            //終了
            if (wflag) return 0;
            else return -1;
        }

        //資源レコードの出力
        private int ExportMaterialRecord(string filepath)
        {
            //デシリアライズ
            List<MaterialRecord> val;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    var result = HoppoAlpha.DataLibrary.Files.TryLoad(filepath, HoppoAlpha.DataLibrary.DataType.Material);

                    if (result.IsSuccess) val = (List<MaterialRecord>)result.Instance;
                    else return -1;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
            //ヘッダー
            StringBuilder sb = new StringBuilder();
            string[] material_key = new string[]
            {
                "日付",
                "燃料", "弾薬", "鋼材", "ボーキサイト", 
                "高速建造材", "高速修復剤", "開発資材", "改修資材",
            };
            CsvList<string> header = new CsvList<string>();
            foreach (string x in material_key)
            {
                header.Add(x);
            }
            sb.AppendLine(string.Join(",", header.ToArray()));
            //個々のデータ
            foreach (var x in val)
            {
                CsvList<string> row = new CsvList<string>();
                //時間
                row.Add(x.Date.ToString());
                //各資材
                foreach (string key in MaterialRecord.Keys)
                {
                    if (x.Value.ContainsKey(key))
                    {
                        row.Add(x.Value[key].ToString());
                    }
                    else
                    {
                        row.Add("");
                    }
                }
                //行の追加
                sb.AppendLine(string.Join(",", row.ToArray()));
            }
            //出力ファイル名
            string outputfilename = this.OutputDirectory + @"/" + Path.GetFileNameWithoutExtension(filepath) + ".csv";
            //書き込み
            bool wflag = CsvSave(sb.ToString(), outputfilename);
            //終了
            if (wflag) return 0;
            else return -1;
        }

        //ランキングの出力
        private int ExportRanking(string filepath)
        {
            //デシリアライズ
            SortedDictionary<int, ApiRanking.ApiList> val;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    var result = HoppoAlpha.DataLibrary.Files.TryLoad(filepath, HoppoAlpha.DataLibrary.DataType.Ranking);

                    if (result.IsSuccess) val = (SortedDictionary<int, ApiRanking.ApiList>)result.Instance;
                    else return -1;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
            //ヘッダー
            StringBuilder sb = new StringBuilder();
            string[] header_text = new string[]
            {
                "順位", "提督ID", "司令部レベル", "階級", "名前",
                "提督経験値", "コメント", "戦果", "api_flag", "名前ID",
                "コメントID", "甲種勲章",
            };
            CsvList<string> header = new CsvList<string>();
            foreach (string x in header_text)
            {
                header.Add(x);
            }
            sb.AppendLine(string.Join(",", header.ToArray()));
            //個々のデータ
            foreach (var x in val)
            {
                ApiRanking.ApiList item = x.Value;
                var row = item.ExportToCsvRow(Helper.RankToString(item.api_rank));
                //行の追加
                sb.AppendLine(string.Join(",", row.ToArray()));
            }
            //出力ファイル名
            string outputfilename = this.OutputDirectory + @"/" + Path.GetFileNameWithoutExtension(filepath) + ".csv";
            //書き込み
            bool wflag = CsvSave(sb.ToString(), outputfilename);
            //終了
            if (wflag) return 0;
            else return -1;
        }

        //戦果の出力
        private int ExportSenkaRecord(string filepath)
        {
            //デシリアライズ
            List<SenkaRecord> val;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    var result = HoppoAlpha.DataLibrary.Files.TryLoad(filepath, HoppoAlpha.DataLibrary.DataType.Senka);

                    if (result.IsSuccess) val = (List<SenkaRecord>)result.Instance;
                    else return -1;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
            //ヘッダー
            StringBuilder sb = new StringBuilder();
            //--簡易モード
            //ヘッダー
            string[] header_simple = new string[]
            {
                "セクション", "開始日時", "終了日時", "開始提督経験値", "終了提督経験値", 
                "開始戦果", "終了戦果の推定値", "特別戦果", "順位", "階級",
                "1位戦果", "2位戦果", "3位戦果", "5位戦果", "20位戦果", "100位戦果", "500位戦果",
                "1位ID", "2位ID", "3位ID", "5位ID", "20位ID", "100位ID", "500位ID",
                "1位経験値", "2位経験値", "3位経験値", "5位経験値", "20位経験値", "100位経験値", "500位経験値",
                "1位名前", "2位名前", "3位名前", "5位名前", "20位名前", "100位名前", "500位名前",
            };
            CsvList<string> header = new CsvList<string>();
            foreach (string x in header_simple)
            {
                header.Add(x);
            }
            sb.AppendLine(string.Join(",", header.ToArray()));
            //個々のデータ
            foreach (var x in val)
            {
                CsvList<string> row = new CsvList<string>();
                //1行目
                row.Add(x.Section.ToString());
                row.Add(x.StartTime.ToString());
                row.Add(x.EndTime == new DateTime() ? "" : x.EndTime.ToString());
                row.Add(x.StartExp.ToString());
                row.Add(x.EndExp < 0 ? "" : x.EndExp.ToString());
                //2行目
                row.Add(x.StartSenka < 0 ? "" : x.StartSenka.ToString());
                row.Add(x.EndSenkaEst < 0 ? "" : x.EndSenkaEst.ToString("F2"));
                row.Add(x.SpecialSenka.ToString());
                row.Add(x.Rank < 0 ? "" : x.Rank.ToString());
                row.Add(x.Title < 0 ? "" : x.Title.ToString());
                //3行目（ランカー戦果）
                int[] ranker = new int[] { 0, 1, 2, 4, 19, 99, 499 };
                foreach (int i in ranker)
                {
                    var rate = x.GetPlayerViewSenka(i);
                    row.Add(rate < 0 ? "" : rate.ToString());
                }
                //4行目（ランカーID）
                foreach (int i in ranker) row.Add(x.TopID[i] < 0 ? "" : x.TopID[i].ToString());
                //5行目（ランカー経験値）
                foreach (int i in ranker) row.Add(x.TopExp[i] < 0 ? "" : x.TopExp[i].ToString());
                //6行目（名前）
                foreach (int i in ranker) row.Add(x.TopName[i]);
                //行の追加
                sb.AppendLine(string.Join(",", row.ToArray()));
            }
            //出力ファイル名
            string outputfilename_simple = this.OutputDirectory + @"/" + Path.GetFileNameWithoutExtension(filepath) + ".csv";
            //書き込み
            bool wflag = CsvSave(sb.ToString(), outputfilename_simple);
            if (!wflag) return -1;
            //---フルモード
            if (this.FullMode)
            {
                for (int mode = 0; mode < 4; mode++)
                {
                    //ヘッダー
                    sb = new StringBuilder();
                    CsvList<string> header_full = new CsvList<string>();
                    switch (mode)
                    {
                        case 0:
                            header_full.Add("戦果/セクション");
                            break;
                        case 1:
                            header_full.Add("ID/セクション");
                            break;
                        case 2:
                            header_full.Add("経験値/セクション");
                            break;
                        case 3:
                            header_full.Add("名前/セクション");
                            break;
                    }
                    //ヘッダー共通部分
                    header_full.Add("開始日時");
                    for (int i = 0; i < SenkaRecord.MaxArraySize; i++)
                    {
                        header_full.Add(string.Format("{0}位", i + 1));
                    }
                    sb.AppendLine(string.Join(",", header_full.ToArray()));
                    //個々のデータ
                    foreach (var x in val)
                    {
                        //選択するデータ
                        object target = new object();
                        switch (mode)
                        {
                            case 0:
                                target = Enumerable.Range(0, SenkaRecord.MaxArraySize).Select(k => x.GetPlayerViewSenka(k)).ToArray();
                                break;
                            case 1:
                                target = x.TopID;
                                break;
                            case 2:
                                target = x.TopExp;
                                break;
                            case 3:
                                target = x.TopName;
                                break;
                        }
                        CsvList<string> row = new CsvList<string>();
                        //セクション
                        row.Add(x.Section.ToString());
                        //開始日時
                        row.Add(x.StartTime.ToString());
                        //順位データ
                        if (target is int[])
                        {
                            foreach (int y in (int[])target)
                            {
                                row.Add(y < 0 ? "" : y.ToString());
                            }
                        }
                        if (target is string[])
                        {
                            foreach (string y in (string[])target)
                            {
                                row.Add(y);
                            }
                        }
                        //行の追加
                        sb.AppendLine(string.Join(",", row.ToArray()));
                    }
                    //出力ファイル名
                    string mode_str = "";
                    switch (mode)
                    {
                        case 0:
                            mode_str = "a";
                            break;
                        case 1:
                            mode_str = "b";
                            break;
                        case 2:
                            mode_str = "c";
                            break;
                        case 3:
                            mode_str = "d";
                            break;
                    }
                    string outputfilename_full = this.OutputDirectory + @"/" + Path.GetFileNameWithoutExtension(filepath) + mode_str + ".csv";
                    //書き込み
                    wflag = CsvSave(sb.ToString(), outputfilename_full);
                }
            }
            //終了処理
            if (wflag) return 0;
            else return -1;
        }

        //敵編成データの出力
        private int ExportFleetDB(string filepath)
        {
            //データオブジェクトの取得
            Dictionary<int, EnemyFleetRecord> rawdata = null;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    var loadresult = HoppoAlpha.DataLibrary.Files.TryLoad(filepath, HoppoAlpha.DataLibrary.DataType.EnemyFleet);

                    if (loadresult.IsSuccess) rawdata = (Dictionary<int, EnemyFleetRecord>)loadresult.Instance;
                    else return -1;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
            //編成IDでソート
            var sortdata = rawdata.Values.OrderBy(x => x.MapAreaID).ThenBy(x => x.MapInfoNo).ThenBy(x => x.CellNo).ThenBy(x => x.LocalID);
            //マップごとに集計
            Dictionary<string, FleetRecordCrossSection> collecion = new Dictionary<string, FleetRecordCrossSection>();
            string allsheetname = FleetRecordCrossSection.GetDisplayName(-1, -1);
            collecion.Add(allsheetname, new FleetRecordCrossSection(-1, -1, null));////ALLに対応するインスタンスの追加
            foreach(var x in sortdata)
            {
                //ALLシートにADD
                collecion[allsheetname].Records.Add(x);
                //個別マップのシートにADD
                string keyname = FleetRecordCrossSection.GetDisplayName(x.MapAreaID, x.MapInfoNo);//対応するシート
                if(!collecion.ContainsKey(keyname))
                {
                    //シートがない場合
                    collecion[keyname] = new FleetRecordCrossSection(x.MapAreaID, x.MapInfoNo, x);
                }
                else
                {
                    collecion[keyname].Records.Add(x);
                }
            }
            //--その1 - エクセルに書き出し
            #region エクセルに書き出し
            Excel.Application oExcelApp = null;
            Excel.Workbook oExcelWBook = null;
            try
            {
                oExcelApp = new Excel.Application();
                oExcelApp.DisplayAlerts = false;//Excel確認ダイアログのオフ
                oExcelApp.Visible = false;//Excelを非表示に
                //Excelをオープン
                oExcelWBook = (Excel.Workbook)oExcelApp.Workbooks.Add();
                List<int> edummy = Enumerable.Repeat(-1, 5).ToList();//eSlotのダミー用
                //ワークシートごとにデータの追加
                foreach(var k in collecion.Keys.OrderBy(x => x))
                {
                    var data = collecion[k];
                    //レコードのソート
                    List<EnemyFleetRecord> sortedrecord;
                    if (k == allsheetname) sortedrecord = data.Records.OrderBy(x => x.MapAreaID).ThenBy(x => x.MapInfoNo).ThenBy(x => x.CellNo).ThenBy(x => x.LocalID).ToList();
                    else sortedrecord = data.Records.OrderBy(x => x.CellNo).ThenBy(x => x.LocalID).ToList();

                    //先にセルテキストの配列を作る
                    string[][] celltext = Enumerable.Range(0, sortedrecord.Count + 1).Select(delegate(int i)
                    {
                        if (i == 0) return EnemyFleetRecord.ExcelHeader;
                        var rec = sortedrecord[i - 1];

                        //shipke
                        int[] safe_shipke = rec.ShipKe.Skip(1).Concat(Enumerable.Repeat(-1, 6)).Take(6).ToArray();//これで必ず6個の要素を持つ配列になる
                        //eSlot
                        List<List<int>> safe_eslot = rec.ESlot.Values.Concat(Enumerable.Repeat(edummy, 6)).Take(6).ToList();//これで必ず6個のリストを持つリストになる
                        //艦名
                        string[] shipname = safe_shipke.Select(delegate(int shipid)
                        {
                            if (shipid == -1) return "";
                            var dship = APIMaster.MstShips[shipid];
                            return dship.api_name + dship.api_yomi.Replace("-", "") ;
                        }).ToArray();
                        //敵制空基本値
                        int enemyairsup = Enumerable.Range(0, shipname.Length).Select(x => safe_shipke[x] == -1 ? 0 : KancolleInfoFleet.CalcEnemyAirSup(safe_shipke[x], safe_eslot[x]).AirSupValueMax).Sum();
                        //敵陣形
                        string enemyformation = Helper.BattleFormationToString(rec.Formation);

                        //データに格納
                        return rec.ConvertToExcelText(shipname, safe_shipke, enemyairsup, enemyformation);
                    }).ToArray();
                    //ワークシートの取得
                    Excel.Worksheet oExcelWSheet = (Excel.Worksheet)oExcelWBook.Worksheets.Add(oExcelWBook.Worksheets[1]);
                    oExcelWSheet.Name = k;
                    //書き込み
                    foreach(var i in Enumerable.Range(0, celltext.Length))
                    {
                        foreach(var j in Enumerable.Range(0, celltext[i].Length))
                        {
                            oExcelWSheet.Cells[i+1, j+1] = celltext[i][j];
                        }
                    }
                }
                //保存名
                string destExcelFilename = this.OutputDirectory + @"/" + Path.GetFileNameWithoutExtension(filepath) + ".xls";
                //保存
                oExcelWBook.CheckCompatibility = false;//互換性チェックのオフ
                oExcelWBook.SaveAs(destExcelFilename, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8);//.xlsとして保存
            }
            catch(Exception)
            {
                return -1;
            }
            finally
            {
                oExcelWBook.Close(false);
                oExcelApp.Quit();
                GC.Collect();
            }
            #endregion
            //--その2 - HTMLに書き出し
            //マスターデータの集計
            var ships = new Dictionary<int, MasterShipAndESlotCombine>();
            foreach(var s in APIMaster.MstShips.Values)
            {
                if(s.IsEnemyShip) ships[s.api_id] = new MasterShipAndESlotCombine(s);
            }

            //装備込みのステータスの計算
            foreach (var x in ships) x.Value.FinalizeCalc();
            //IDごとにソート
            var sortedships = ships.OrderBy(x => x.Key).Select(x => x.Value);
            //HTML化
            StringBuilder sb = new StringBuilder();
            sb.Append(Properties.Resources.FleetDBHtmlConvertHeader);//<div id = "main">まで
            //--目次
            sb.AppendLine("<p>");
            sb.AppendLine("敵艦一覧<br/>");
            sb.AppendLine("<ui>");
            foreach (var x in sortedships)
                sb.AppendFormat("  <li><a href=\"#{0}\">{1} : {2}</a></li>", x.MstShip.api_id, x.MstShip.api_id, x.MstShip.api_name + x.MstShip.api_yomi.Replace("-", "")).AppendLine();
            sb.AppendLine("</ui>");
            sb.AppendLine("</p>");
            sb.AppendLine();
            //--個別
            foreach(var x in sortedships)
            {
                sb.AppendFormat("<p id=\"{0}\">", x.MstShip.api_id).AppendLine();//文章内リンク
                sb.AppendLine("<table>");
                sb.AppendFormat("  <tr><th colspan=\"4\">{0} : {1}{2}</th></tr>", x.MstShip.api_id, x.MstShip.api_name, x.MstShip.api_yomi.Replace("-", ""));//ヘッダー

                if (x.MstShip.api_maxeq == null || x.MstShip.DefaultSlotItem == null)
                {
                    foreach (var i in Enumerable.Range(0, 5))
                        sb.AppendFormat("  <tr><th>装備{0}</th><td>[{1}]{2}</td></tr>", i + 1, "?", "").AppendLine();
                }
                else
                {
                    foreach (var i in Enumerable.Range(0, 5))
                    {
                        string slotname;
                        if (i < x.MstShip.DefaultSlotItem.Count)
                        {
                            int slotid = x.MstShip.DefaultSlotItem[i];
                            ExMasterSlotitem dslot;
                            if (APIMaster.MstSlotitems.TryGetValue(slotid, out dslot)) slotname = dslot.api_name;
                            else slotname = "";
                        }
                        else slotname = "";

                        int eq;
                        if (i < x.MstShip.api_maxeq.Count) eq = x.MstShip.api_maxeq[i];
                        else eq = 0;

                        sb.AppendFormat("  <tr><th>装備{0}</th><td>[{1}]{2}</td></tr>", i + 1, eq, slotname).AppendLine();//装備
                    }
                }
                sb.AppendFormat("  <tr><th>耐久</th><td>{0}</td><th>火力</th><td>{1}</td></tr>",
                    x.Display(MasterShipAndESlotCombine.StatusEnum.HP), x.Display(MasterShipAndESlotCombine.StatusEnum.Fire)).AppendLine();
                sb.AppendFormat("  <tr><th>装甲</th><td>{0}</td><th>雷装</th><td>{1}</td></tr>",
                    x.Display(MasterShipAndESlotCombine.StatusEnum.Defense), x.Display(MasterShipAndESlotCombine.StatusEnum.Torpedo)).AppendLine();
                sb.AppendFormat("  <tr><th>回避</th><td>{0}</td><th>対空</th><td>{1}</td></tr>",
                    x.Display(MasterShipAndESlotCombine.StatusEnum.Evasion), x.Display(MasterShipAndESlotCombine.StatusEnum.AntiAir)).AppendLine();
                sb.AppendFormat("  <tr><th>搭載</th><td>{0}</td><th>対潜</th><td>{1}</td></tr>",
                    x.Display(MasterShipAndESlotCombine.StatusEnum.EqSum), x.Display(MasterShipAndESlotCombine.StatusEnum.AntiSub)).AppendLine();
                sb.AppendFormat("  <tr><th>速力</th><td>{0}</td><th>索敵</th><td>{1}</td></tr>",
                    x.Display(MasterShipAndESlotCombine.StatusEnum.Speed), x.Display(MasterShipAndESlotCombine.StatusEnum.Search)).AppendLine();
                sb.AppendFormat("  <tr><th>射程</th><td>{0}</td><th>運</th><td>{1}</td></tr>",
                    x.Display(MasterShipAndESlotCombine.StatusEnum.Range), x.Display(MasterShipAndESlotCombine.StatusEnum.Luck)).AppendLine();
                sb.AppendFormat("  <tr><th>制空値</th><td>{0}</td><th></th><td></td></tr>",
                    x.Display(MasterShipAndESlotCombine.StatusEnum.AirSup)).AppendLine();
                sb.AppendLine("</table>");
                sb.AppendLine("</p>");
                sb.AppendLine();
            }
            //--フッター
            sb.AppendLine("</div>");
            sb.AppendLine();
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            //--保存
            try
            {
                string destHtmlFilename = this.OutputDirectory + @"/" + Path.GetFileNameWithoutExtension(filepath) + "_master.html";
                using (FileStream fs = new FileStream(destHtmlFilename, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("shift-jis")))
                        sw.WriteLine(sb.ToString());
                }
            }
            catch(Exception)
            {
                return -1;
            }
            //--正常終了
            return 0;
        }

        //ドロップログの出力
        private int ExportDropDB(string filepath)
        {
            //デシリアライズ
            DropRecordCollection val;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    var result = HoppoAlpha.DataLibrary.Files.TryLoad(filepath, HoppoAlpha.DataLibrary.DataType.DropRecord);

                    if (result.IsSuccess) val = (DropRecordCollection)result.Instance;
                    else return -1;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
            //コンバート用のテキスト
            string text = DropDataBase.MakeConvertText(val.DataBase, val.MasterDropShipHeader, val.MasterDropItemHeader);
            //出力ファイル名
            string outputfilename = this.OutputDirectory + @"/" + Path.GetFileNameWithoutExtension(filepath) + ".csv";
            //書き込み
            bool wflag = CsvSave(text, outputfilename);
            //終了
            if (wflag) return 0;
            else return -1;
        }
    }
}
