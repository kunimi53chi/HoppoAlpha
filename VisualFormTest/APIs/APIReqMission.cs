using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using Codeplex.Data;

namespace VisualFormTest
{
    static class APIReqMission
    {
        //api_req_mission/result/
        public static void ReadResult(string json)
        {
            var ojson = DynamicJson.Parse(json).api_data;
            //提督経験値
            int exp = (int)ojson.api_member_exp;
            APIPort.SetAdmiralExp(exp);
        }

        //api_req_mission/start
        public static void ReadStart(string requestbody, string json)
        {
            //艦隊の番号
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fdeck%5Fid=([0-9])");
            int fleetid = Convert.ToInt32(match.Groups[1].Value);
            //遠征のID
            match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fmission%5Fid=([0-9]+)");
            int mission_id = Convert.ToInt32(match.Groups[1].Value);
            ApiMstMission mission = (from m in APIMaster.MstMissions
                                     where m.api_id == mission_id
                                     select m).First();
            //帰投時間
            var ojson = DynamicJson.Parse(json).api_data;
            long completetime = (long)ojson.api_complatetime;
            //支援艦隊だったら離脱
            if (mission.api_name == "前衛支援任務" || mission.api_name == "艦隊決戦支援任務") return;
            //バルーンに追加
            NotifyBalloon.AddMission(fleetid, completetime);
        }

        //api_req_mission/return_instruction
        public static void ReadReturnInstruction(string requestbody, string json)
        {
            //艦隊の番号
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fdeck%5Fid=([0-9])");
            int fleetid = Convert.ToInt32(match.Groups[1].Value);
            //ミッションのデータの書き換え
            var ojson = DynamicJson.Parse(json).api_data;
            List<long> api_mission = ojson.api_mission.Deserialize<List<long>>();
            //コピーの作成
            List<ApiDeckPort> decks = APIPort.DeckPorts;
            //書き換え
            long oldctime = decks[fleetid-1].api_mission[2];
            decks[fleetid-1].api_mission = api_mission;
            //バルーンの修正
            NotifyBalloon.ModifyMission(fleetid, oldctime, api_mission[2]);
        }
    }
}
