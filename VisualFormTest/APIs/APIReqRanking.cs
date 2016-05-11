using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Codeplex.Data;
using HoppoAlpha.DataLibrary.RawApi.ApiReqRanking;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest
{
    static class APIReqRanking
    {
        public static bool IsInited { get; set; }
        //ランキング
        public static SortedDictionary<int, ApiRanking.ApiList> Rankings { get; set; }
        public static string LastSavedFileName {get; set;}
        public static string LastSavedDirectory { get; set; }

        public static string GetDirectory(DateTime date)
        {
            DateTime latedate = date.AddHours(-3);
            return @"user/" + APIPort.Basic.api_member_id + @"/ranking/" + latedate.ToString("yyyyMM") + @"/";
        }

        public static string GetFileName(DateTime date)
        {
            string dir = GetDirectory(date);
            int section;
            //ファイル名側は月初・月末関係なく処理する
            if (date.Hour < 3) section = 2;
            else if (date.Hour < 15) section = 1;
            else section = 2;

            return dir + string.Format("ranking{0}_{1}.dat", (date.AddHours(-3)).ToString("yyyyMMdd"), section);
        }
        public static string GetFileName(DateTime date, int section)
        {
            string dir = GetDirectory(date);
            return dir + string.Format("ranking{0}_{1}.dat", (date.AddHours(-3)).ToString("yyyyMMdd"), section);
        }


        public static void Init()
        {
            if (IsInited) return;
            //初期化
            Rankings = new SortedDictionary<int, ApiRanking.ApiList>();
            //読み込み
            DateTime now = DateTime.Now;
            string dir = GetDirectory(now);
            string file = GetFileName(now);

            var loadResult = HoppoAlpha.DataLibrary.Files.TryLoad(file, HoppoAlpha.DataLibrary.DataType.Ranking);
            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.Ranking, loadResult, false);
            Rankings = (SortedDictionary<int, ApiRanking.ApiList>)loadResult.Instance;

            //ファイル名
            LastSavedDirectory = dir;
            LastSavedFileName = file;
            //初期化フラグ
            IsInited = true;
        }

        public static void Save()
        {
            if(!IsInited || Rankings == null) return;
            //セクションまたぎ
            DateTime now = DateTime.Now;
            string dir = GetDirectory(now);
            string file = GetFileName(now);
            if(file != LastSavedFileName)
            {
                SwitchFile();
            }
            else
            {
                SaveAs(dir, file);
            }
        }

        private static void SaveAs(string dir, string filename)
        {
            //ディレクトリの作成
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var saveResult = HoppoAlpha.DataLibrary.Files.Save(filename, HoppoAlpha.DataLibrary.DataType.Ranking, Rankings);
            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.Ranking, saveResult, true);

            LastSavedDirectory = dir;
            LastSavedFileName = filename;
        }

        public static void SwitchFile()
        {
            //既存ログの保存
            SaveAs(LastSavedDirectory, LastSavedFileName);
            //初期化
            Rankings = new SortedDictionary<int, ApiRanking.ApiList>();
            //次のファイル名に
            DateTime now = DateTime.Now;
            LastSavedDirectory = GetDirectory(now);
            LastSavedFileName = GetFileName(now);
        }

        //ランキングの追加
        public static void AddRanking(List<ApiRanking.ApiList> rankingdata)
        {
            //ファイルまたぎチェック
            DateTime now = DateTime.Now;
            string file = GetFileName(now);
            if(file != LastSavedFileName)
            {
                SwitchFile();
            }
            //追加
            foreach(var x in rankingdata)
            {
                Rankings[x.api_no] = x;
            }
        }


        //api_req_ranking/getlist
        public static void ReadGetlist(string json)
        {
            var ojson = DynamicJson.Parse(json).api_data;
            //ランキングデータ
            ApiRanking ranking = ojson.Deserialize<ApiRanking>();
            //ランキングの生データの追加
            AddRanking(ranking.api_list);
            //セクション切り替わり時間中なら表示を更新しない
            DateTime now = DateTime.Now;
            if(SenkaRecord.GetSection(now) == 3)
            {
                return;
            }
            if((now >= DateTime.Today.AddHours(2) && now < DateTime.Today.AddHours(3)) ||
                now >= DateTime.Today.AddHours(14) && now < DateTime.Today.AddHours(15))
            {
                return;
            }
            //--HistoricalData.LogSenka側
            int startidx = (ranking.api_disp_page - 1) * 10;
            int endidx = ranking.api_disp_page * 10 - 1;
            int[] array_id = ranking.api_list.Select(x => x.api_member_id).ToArray();
            int[] array_exp = ranking.api_list.Select(x => x.api_experience).ToArray();
            int[] array_senka = ranking.api_list.Select(x => x.api_rate).ToArray();
            string[] array_name = ranking.api_list.Select(x => x.api_nickname).ToArray();
            //自分の戦果
            int mysenka;
            int mysenkaidx = Array.IndexOf(array_id, Convert.ToInt32(APIPort.Basic.api_member_id));
            if(mysenkaidx != -1)//自分のデータがある場合
            {
                mysenka = ranking.api_list[mysenkaidx].api_rate;
            }
            else
            {
                mysenka = -1;
            }
            HistoricalData.SetSenkaValue(mysenka, startidx, endidx, array_senka, array_id, array_exp, array_name);
        }
    }

}
