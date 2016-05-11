using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codeplex.Data;

namespace VisualFormTest
{
    static class APIReqMember
    {
        //get_practice_enemyinfo
        public static void ReadGetPracticeEnemyInfo(string json)
        {
            BattleView view = new BattleView();
            //JSONオブジェクトに
            var ojson = DynamicJson.Parse(json).api_data;
            //形勢オブジェクト
            view.EnemyPracticeID = (int)ojson.api_member_id;
            APIBattle.BattleView = view;
        }
    }
}
