using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HoppoAlpha.DataLibrary.RawApi.ApiReqQuest;

namespace VisualFormTest
{
    static class APIReqQuest
    {
        public static SortedDictionary<int, ApiQuest> Quests { get; set; }
        public static DateTime LastUpdated { get; set; }

        public static bool IsInited { get; set; }
        public static string SaveDirectory { get; set; }
        public static string SaveFilename { get; set; }

        //questlistはget_member側
        //clearitem
        public static void ReadClearitem(string requestbody)
        {
            //クエストIDの抽出
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fquest%5Fid=([0-9]+)");
            int quest_id = Convert.ToInt32(match.Groups[1].Value);
            //ハッシュから消去
            Quests.Remove(quest_id);
        }

        //初期化
        public static void Init()
        {
            if (IsInited) return;
            LastUpdated = DateTime.Today;
            Quests = new SortedDictionary<int, ApiQuest>();

            //読み込み
            SaveDirectory = @"user/" + APIPort.Basic.api_member_id + @"/general/";
            SaveFilename = SaveDirectory + "quest.dat";

            var loadResult = HoppoAlpha.DataLibrary.Files.TryLoad(SaveFilename, HoppoAlpha.DataLibrary.DataType.Quest);
            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.Quest, loadResult, false);
            var savevals = (HoppoAlpha.DataLibrary.DataObject.QuestRecord)loadResult.Instance;
            if (savevals.LastCheckDate != new DateTime())
            {
                Quests = savevals.Records;
                LastUpdated = savevals.LastCheckDate;
            }
            //日またぎチェック
            CheckDaysOver();

            IsInited = true;
        }

        //5時またぎチェック
        public static void CheckDaysOver()
        {
            DateTime lastdays = LastUpdated - new TimeSpan(5, 0, 0);
            DateTime nowdays = DateTime.Now - new TimeSpan(5, 0, 0);
            if(nowdays.Day != lastdays.Day)
            {
                Quests = new SortedDictionary<int, ApiQuest>();
                LastUpdated = DateTime.Now;
            }
        }

        //保存
        public static void Save()
        {
            if (!IsInited || Quests == null) return;
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
            var savevals = new HoppoAlpha.DataLibrary.DataObject.QuestRecord()
            {
                Records = Quests,
                LastCheckDate = LastUpdated,
            };
            //保存
            var saveResult = HoppoAlpha.DataLibrary.Files.Save(SaveFilename, HoppoAlpha.DataLibrary.DataType.Quest, savevals);
            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.Quest, saveResult, true);
        }
    }

}
