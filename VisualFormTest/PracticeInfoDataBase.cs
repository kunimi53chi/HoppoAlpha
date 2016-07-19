using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HoppoAlpha.DataLibrary.DataObject;
using HoppoAlpha.DataLibrary.RawApi.ApiReqMember;

namespace VisualFormTest
{
    public static class PracticeInfoDataBase
    {
        //データのコレクション
        public static PracticeInfoCollection Collection { get; set; }
        //追加のブロック
        public static bool AddingBlock { get; set; }
        //初期化したかどうか
        public static bool IsInited { get; set; }

        public static readonly string OutputDirectory = @"user\practice";
        public static readonly string OutputFilePath = OutputDirectory + @"\practiceinfo.dat";

        //コンストラクタ
        static PracticeInfoDataBase()
        {
            //出力フォルダがない場合
            if(!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }

            //ファイルがなければ
            if(!File.Exists(OutputFilePath))
            {
                Collection = new PracticeInfoCollection();
            }
            else
            {
                var loadResult = HoppoAlpha.DataLibrary.Files.TryLoad(OutputFilePath, HoppoAlpha.DataLibrary.DataType.PracticeInfo);
                LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.PracticeInfo, loadResult, false);
                Collection = (PracticeInfoCollection)loadResult.Instance;
            }

            IsInited = true;
        }

        //保存
        public static void Save()
        {
            if (!IsInited || Collection == null) return;

            AddingBlock = true;
            if(!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }

            var saveResult = HoppoAlpha.DataLibrary.Files.Save(OutputFilePath, HoppoAlpha.DataLibrary.DataType.PracticeInfo, Collection);
            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.PracticeInfo, saveResult, true);

            AddingBlock = false;
        }

        //追加
        public static void AddDataBase(GetPracticeEnemyinfo enemyInfo)
        {
            if (AddingBlock) return;

            //日付のセット
            enemyInfo.GetDatetime = DateTime.Now;
            //艦名のセット
            enemyInfo.SetShipNames(APIMaster.MstShips);

            //メンバー単位のデータに追加
            PracticeInfoMemberData mdata;
            if(!Collection.AllData.TryGetValue(enemyInfo.api_member_id, out mdata)) mdata = new PracticeInfoMemberData();

            mdata.MemberDataByDate[enemyInfo.GetDatetime] = enemyInfo;

            //全体データに追加
            Collection.AllData[enemyInfo.api_member_id] = mdata;
        }
    }
}
