using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest
{
    static class HistoricalData
    {
        //資材のデータ
        public static List<MaterialRecord> LogMaterial { get; set; }
        //経験値のデータ
        public static List<ExpRecord> LogExperience { get; set; }
        //直近の個人戦果
        public static List<SenkaRecord> LogSenka { get; set; }

        //資材グラフの詳細
        public static GraphInfo GraphInfoMaterial { get; set; }
        //提督経験値グラフの詳細
        public static GraphInfo GraphInfoExperience { get; set; }

        //初期化されたかどうか
        public static bool IsInited { get; set; }
        //直前にセーブした名前
        public static Dictionary<string, string> LastSavedFileName { get; set; }
        

        //ディレクトリの取得
        public static string GetDirectory(string mode)
        {
            return @"user/" +  APIPort.Basic.api_member_id + @"/" + mode + @"/";
        }

        //ファイル名の取得
        public static string GetFileName(string mode, DateTime date, int version)
        {
            string month = date.ToString("yyyyMM");//月まで
            switch(mode)
            {
                case "material":
                    return string.Format("{0}material_{1}.dat", GetDirectory(mode), month);
                case "experience":
                    return string.Format("{0}experience_{1}.dat", GetDirectory(mode), month);
                case "senka":
                    return string.Format("{0}senka_{1}.dat", GetDirectory(mode), month);
                default:
                    throw new KeyNotFoundException();
            }
        }

        //戦果レコードのインスタンス作成
        private static SenkaRecord CreateSenkaInstance(DateTime datetime)
        {
            SenkaRecord lastSenka = null;
            if(LogSenka.Count > 0) lastSenka = LogSenka[LogSenka.Count - 1];
            return new SenkaRecord(datetime, APIPort.Basic.api_experience, lastSenka);
        }

        //初期化用
        public static void Init()
        {
            if (IsInited) return;
            //初期化
            LogMaterial = new List<MaterialRecord>();
            LogExperience = new List<ExpRecord>();
            LogSenka = new List<SenkaRecord>();
            //--デシリアライズ
            DateTime now = DateTime.Now;
            //素材
            string material_file = GetFileName("material", now, -1);
            string exp_file = GetFileName("experience", now, -1);
            string senka_file = GetFileName("senka", now, -1);
            HoppoAlpha.DataLibrary.Files.FileOperationResult loadresult;
            //資源
            loadresult = HoppoAlpha.DataLibrary.Files.TryLoad(material_file, HoppoAlpha.DataLibrary.DataType.Material);
            if (loadresult.IsSuccess) LogMaterial = (List<MaterialRecord>)loadresult.Instance;
            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.Material, loadresult, false);
            //経験値
            loadresult = HoppoAlpha.DataLibrary.Files.TryLoad(exp_file, HoppoAlpha.DataLibrary.DataType.Experience);
            if (loadresult.IsSuccess) LogExperience = (List<ExpRecord>)loadresult.Instance;
            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.Experience, loadresult, false);
            //個人戦果
            loadresult = HoppoAlpha.DataLibrary.Files.TryLoad(senka_file, HoppoAlpha.DataLibrary.DataType.Senka);
            if (loadresult.IsSuccess) LogSenka = (List<SenkaRecord>)loadresult.Instance;
            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.Senka, loadresult, false);

            if (LogSenka.Count == 0) LogSenka.Add(CreateSenkaInstance(DateTime.Now));//戦果だけない場合追加する
            //---ファイル名の記録
            LastSavedFileName = new Dictionary<string, string>();
            LastSavedFileName["material"] = material_file;
            LastSavedFileName["experience"] = exp_file;
            LastSavedFileName["senka"] = senka_file;
            //--グラフの詳細
            GraphInfoMaterial = new GraphInfo();
            GraphInfoExperience = new GraphInfo();
            //---フラグの更新
            IsInited = true;
        }

        //全て保存
        public static void SaveAll()
        {
            if (!IsInited) return;
            //素材と経験値と戦果
            string[] modes = new string[] { "material", "experience", "senka" };
            foreach(string m in modes)
            {
                string dir = GetDirectory(m);
                string file = GetFileName(m, DateTime.Now, -1);
                if (file != LastSavedFileName[m]) SwitchFile(m);//月またぎをする場合
                else SaveAs(dir, file, m);
            }
        }

        //ファイル名を指定して保存
        private static void SaveAs(string dir, string filename, string mode)
        {
            //ディレクトリの作成
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var saveResult = new HoppoAlpha.DataLibrary.Files.FileOperationResult();
            switch(mode)
            {
                case "material":
                    saveResult = HoppoAlpha.DataLibrary.Files.Save(filename, HoppoAlpha.DataLibrary.DataType.Material, LogMaterial);
                    LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.Material, saveResult, true);
                    break;
                case "experience":
                    saveResult = HoppoAlpha.DataLibrary.Files.Save(filename, HoppoAlpha.DataLibrary.DataType.Experience, LogExperience);
                    LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.Experience, saveResult, true);
                    break;
                case "senka":
                    saveResult = HoppoAlpha.DataLibrary.Files.Save(filename, HoppoAlpha.DataLibrary.DataType.Senka, LogSenka);
                    LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.Senka, saveResult, true);
                    break;
            }
            LastSavedFileName[mode] = filename;
        }

        //ファイルが違う場合の処理（月またぎや日またぎ）
        private static void SwitchFile(string mode)
        {
            //既存ログの保存
            SaveAs(GetDirectory(mode), LastSavedFileName[mode], mode);
            switch (mode)
            {
                case "material":
                    //資源ログのクリア
                    LogMaterial = new List<MaterialRecord>();
                    break;
                case "experience":
                    //経験値ログのクリア
                    LogExperience = new List<ExpRecord>();
                    break;
                case "senka":
                    //戦果ログのクリア
                    LogSenka = new List<SenkaRecord>();
                    LogSenka.Add(CreateSenkaInstance(DateTime.Now));
                    break;
            }
            //次の月のファイル名に
            LastSavedFileName[mode] = GetFileName(mode, DateTime.Now, -1);
        }

        //資材データの追加
        public static void AddLogMaterial()
        {
            //ファイルが違ったら
            string material_file = GetFileName("material", DateTime.Now, -1);
            if(material_file != LastSavedFileName["material"])
            {
                //月またぎ処理
                SwitchFile("material");
            }
            //資材
            Dictionary<string, int> val = new Dictionary<string, int>();
            for(int i=0; i<APIPort.Materials.Count; i++)
            {
                val[MaterialRecord.Keys[i]] = APIPort.Materials[i].api_value;
            }
            //オブジェクト
            MaterialRecord item = new MaterialRecord() { Date = APIPort.LastUpdate, Value = val };
            //変わってなかったら追加しない
            if(LogMaterial.Count > 0)
            {
                if (LogMaterial[LogMaterial.Count - 1].IsEquials(item)) return;
                //if (MaterialRecord.IsEquials(LogMaterial[LogMaterial.Count - 1], item)) return;
            }
            LogMaterial.Add(item);
        }

        //提督経験値データの追加
        public static void AddExperience()
        {
            //月またぎチェック
            string exp_file = GetFileName("experience", DateTime.Now, -1);
            if(exp_file != LastSavedFileName["experience"])
            {
                SwitchFile("experience");
            }
            //提督経験値（取れるものだけ）
            DateTime now = DateTime.Now;
            ExpRecord item = new ExpRecord()
            {
                Date = now,
                Value = APIPort.Basic.api_experience,
                Before6H =  null, Before12H = null, Before24H = null,
            };
            //--○時間前のログを探す
            //逆順リストの作成 逆順から探索したほうが早い
            List<ExpRecord> rev = new List<ExpRecord>();
            rev.AddRange(LogExperience);
            rev.Reverse();
            //探索終了フラグ
            bool fin6h, fin12h, fin24h;
            fin6h = fin12h = fin24h = false;
            //リストになにもない場合
            if(LogExperience.Count == 0)
            {
                fin6h = fin12h = fin24h = true;
            }
            //最後まで行っても該当日時前のデータがない場合
            else
            {
                ExpRecord last = LogExperience[0];
                if (last.Date > now.AddHours(-6)) fin6h = true;
                if (last.Date > now.AddHours(-12)) fin12h = true;
                if (last.Date > now.AddHours(-24)) fin24h = true;
            }
            //順次探索
            DateTime prev = now.AddHours(1);
            foreach(ExpRecord x in rev)
            {
                //全て終わったら離脱
                if(fin6h && fin12h && fin24h) break;
                int sign_now, sign_prev;
                //6時間前
                if(!fin6h)
                {
                    sign_now = Math.Sign((x.Date - now.AddHours(-6)).TotalMinutes);
                    sign_prev = Math.Sign((prev - now.AddHours(-6)).TotalMinutes);
                    if(sign_now * sign_prev <= 0)
                    {
                        fin6h = true;
                        item.Before6H = x;
                    }
                }
                //12時間前
                if(!fin12h)
                {
                    sign_now = Math.Sign((x.Date - now.AddHours(-12)).TotalMinutes);
                    sign_prev = Math.Sign((prev - now.AddHours(-12)).TotalMinutes);
                    if(sign_now * sign_prev <= 0)
                    {
                        fin12h = true;
                        item.Before12H = x;
                    }
                }
                //24時間前
                if(!fin24h)
                {
                    sign_now = Math.Sign((x.Date - now.AddHours(-24)).TotalMinutes);
                    sign_prev = Math.Sign((prev - now.AddHours(-24)).TotalMinutes);
                    if(sign_now * sign_prev <= 0)
                    {
                        fin24h = true;
                        item.Before24H = x;
                    }
                }
                //prevに追加
                prev = x.Date;
            }
            //アイテムの追加
            LogExperience.Add(item);
        }

        //戦果関連の操作
        #region 戦果関連
        //スイッチチェック（必ずこれを読む）
        public static void SetSenkaValue()
        {
            //月またぎチェック
            string senka_file = GetFileName("senka", DateTime.Now, -1);
            if (senka_file != LastSavedFileName["senka"])
            {
                SwitchFile("senka");
            }
            //スイッチするかどうかのチェック
            SenkaRecord last = LogSenka[LogSenka.Count - 1];
            if(last.SectionSwitchRequired)
            {
                SenkaRecord newitem = CreateSenkaInstance(DateTime.Now);
                //追加
                LogSenka.Add(newitem);
            }
        }

        //経験値の追加
        public static void SetSenkaValue(DateTime date, int exp, int special, int title)
        {
            //ベース
            SetSenkaValue();
            //ターゲット
            SenkaRecord target = LogSenka[LogSenka.Count - 1];
            //値の置き換え
            target.EndTime = date;
            target.EndExp = exp;
            target.SpecialSenka += special;
            target.CalcEstimateMySenka();
            target.Title = title;
        }

        //戦果の画面
        public static void SetSenkaValue(int startSenka, int startIndex, int endIndex, 
            int[] rankingApiRate, string[] rankingName)
        {
            //ベース
            SetSenkaValue();
            SenkaRecord target = LogSenka[LogSenka.Count - 1];
            //非配列部分
            if (startSenka >= 0)
            {
                target.StartSenka = startSenka;
                target.CalcEstimateMySenka();
            }
            //配列部分
            if (endIndex >= SenkaRecord.MaxArraySize) return;
            for(int i=startIndex; i<=endIndex; i++)
            {
                int validx = i - startIndex;
                target.SetPlayerApiRate(i, rankingApiRate[validx]);
                target.TopName[i] = rankingName[validx];
            }
        }
        #endregion
    }

    //グラフの情報
    #region Graphinfo関係
    public class GraphInfo
    {
        public int Mode { get; set; }
        public GraphInfoTerm Term { get; set; }
        public bool IsDiff { get; set; }
        public GraphExceptSeries ExceptSeries { get; set; }
    }

    public enum GraphInfoTerm
    {
        None, All, Week, Day,
    }

    [Flags]
    public enum GraphExceptSeries
    {
        Series1 = 1,
        Series2 = 2,
        Series3 = 4,
        Series4 = 8,
        Series5 = 16,
        Series6 = 32,
        Series7 = 64,
        Series8 = 128,
        Series9 = 256,
        Series10 = 512,
    }

    public static class GraphInfoTermExt
    {
        public static DateTime GetMinDate(this GraphInfoTerm g)
        {
            switch(g)
            {
                case GraphInfoTerm.All: return new DateTime();
                case GraphInfoTerm.Day: return DateTime.Now - new TimeSpan(1, 0, 0, 0);
                case GraphInfoTerm.Week: return DateTime.Now - new TimeSpan(7, 0, 0, 0);
                default: return DateTime.Today;
            }
        }

        public static bool ContainsExceptSeries(this GraphExceptSeries graphExceptSeries, int series)
        {
            if (series <= 0 || series > 10) throw new ArgumentOutOfRangeException();

            int val = (int)Math.Pow(2, series - 1);
            var flag = (GraphExceptSeries)val;

            return graphExceptSeries.HasFlag(flag);
        }
    }
    #endregion


}
