using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codeplex.Data;

namespace VisualFormTest
{
    static class APIReqMap
    {
        //出撃している艦隊の番号
        public static int SallyDeckPort { get; set; }
        //何戦目か
        public static int BattleCount { get; set; }

        //startの場合
        public static void ReadStart(string requestbody, string json, UserControls.TabCounter counter)
        {
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fdeck%5Fid=([0-9])");
            string str_idx = match.Groups[1].Value;
            SallyDeckPort = Convert.ToInt32(str_idx);
            //出撃時のカウンターのチェック
            BattleView view = new BattleView();
            //JSONオブジェクトに
            var ojson = DynamicJson.Parse(json).api_data;
            //マップ情報の取得
            view.AreaID = (int)ojson.api_maparea_id;
            view.MapID = (int)ojson.api_mapinfo_no;
            view.CellID = (int)ojson.api_no;
            APIBattle.BattleView = view;
            //出撃中にチェックを入れる
            APIPort.OnSortie = true;
            //出撃前の母港のバッファリング
            DefaultSlotitemDataBase.BeforeSortieProcess();
            //出撃報告書にセット
            SortieReportDataBase.SetStartSortie();
            //カウンターのチェック
            KancolleInfoCounter.TrialCheckAllAndCount(counter, APIBattle.BattleView);
        }

        //nextまたはstartだった場合
        public static void ReadNext(string json)
        {
            BattleView view = new BattleView();
            //JSONオブジェクトに
            var ojson = DynamicJson.Parse(json).api_data;
            //マップ情報の取得
            view.AreaID = (int)ojson.api_maparea_id;
            view.MapID = (int)ojson.api_mapinfo_no;
            view.CellID = (int)ojson.api_no;
            //戦闘マスかどうか
            bool isBattle = ojson.api_event_id == 4 || ojson.api_event_id == 5;
            //次マスが戦闘マスじゃない場合
            if (!isBattle)
            {
                APIBattle.BattleView = view;
                //1-6のボスマス
                if (ojson.api_event_id == 8)
                {
                    //特別戦果
                    if(ojson.IsDefined("api_get_eo_rate"))
                    {
                        int exrate = 0;
                        if (ojson.api_get_eo_rate is double) exrate = (int)ojson.api_get_eo_rate;
                        else if (ojson.api_get_eo_rate is string) exrate = Convert.ToInt32(ojson.api_get_eo_rate);
                        if (exrate > 0 && HistoricalData.LogSenka != null)
                        {
                            int exp = HistoricalData.LogSenka[HistoricalData.LogSenka.Count - 1].EndExp;
                            HistoricalData.SetSenkaValue(DateTime.Now, exp, exrate, APIPort.Basic.api_rank);
                        }
                    }
                }
                return;
            }
            //ボスかどうか
            view.BossFlag = (ojson.api_event_id == 5) ? 2 : 1;
            //形勢オブジェクト
            view.Situation = BattleSituation.BeforeBattle;
            APIBattle.BattleView = view;
            //○戦目カウンターの追加
            BattleCount++;
        }

        //難易度変更
        public static void ReadSelectEventMapRank(string requestbody)
        {
            //マップNo
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fmap%5Fno=([0-9]+)");
            int map_no = Convert.ToInt32(match.Groups[1].Value);
            //ランク
            match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Frank=([0-9]+)");
            int rank = Convert.ToInt32(match.Groups[1].Value);
            //エリアID
            match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fmaparea%5Fid=([0-9]+)");
            int area = Convert.ToInt32(match.Groups[1].Value);
            //データの変更
            var query = APIGetMember.MapInfo.Where(x => x.api_id == area * 10 + map_no);
            if (query.Count() == 0) throw new IndexOutOfRangeException();
            var target = query.First();
            //イベントデータ
            if (target.api_eventmap == null) return;
            target.api_eventmap.api_selected_rank = rank;
        }
    }
}
