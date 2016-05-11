using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiReqPractice;
using Codeplex.Data;

namespace VisualFormTest
{
    static class APIReqPractice
    {
        //演習リスト
        public static List<ApiPractice> Practice { get; set; }
        //最終更新
        public static DateTime LastUpdateTime { get; set; }

        static APIReqPractice()
        {
            Practice = new List<ApiPractice>();
            for (int i = 0; i < 5; i++) Practice.Add(new ApiPractice());
        }

        //api_req_practice/battleを読む
        public static void ReadPracticeBattle(string json)
        {
            //通常戦闘と違うのはSituationだけ
            APIBattle.BattleView.Situation = BattleSituation.Practice;
            //通常戦闘へ
            APIBattle.ReadSortieBattle(json);
        }

        //api_req_practice/battle_resultを読む
        public static void ReadPracticeBattleResult(string json)
        {
            var ojson = DynamicJson.Parse(json).api_data;
            string rank = ojson.api_win_rank;
            //対象の演習リストを探す
            ApiPractice prac = (from p in Practice
                               where p.api_enemy_id == APIBattle.BattleView.EnemyPracticeID
                               select p).First();
            //Stateの修正
            prac.api_state = ApiPractice.GetResultState(rank);
            //あとは通常の戦闘終了処理へ
            APIBattle.ReadBattleResult(json, null);
        }

        //演習夜戦
        public static void ReadPacticeNightBattle(string json)
        {
            APIBattle.ReadBattleMidnight(json);
        }

        
    }
}
