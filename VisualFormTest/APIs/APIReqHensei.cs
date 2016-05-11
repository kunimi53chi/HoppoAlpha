using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;
using Codeplex.Data;

namespace VisualFormTest
{
    static class APIReqHensei
    {
        //changeの読み込み
        public static void ReadChange(string requestbody)
        {
            //api_ship_idx ：　入れ替えるインデックス　※-1で僚艦解除
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fship%5Fidx=((?:%2D)?[0-9]+)");
            string str_idx = match.Groups[1].Value.Replace("%2D", "-");
            int idx = Convert.ToInt32(str_idx);
            //api_id　：　編成を替える艦隊　：　1＝第1艦隊…
            match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fid=([0-9]+)");
            int fleetid = Convert.ToInt32(match.Groups[1].Value) - 1;//1引いてインデックス化
            //api_ship_id　：　入れ替える相手の艦のID　※-1で解除　-2で僚艦全て解除
            match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fship%5Fid=((?:%2D)?[0-9]+)");
            string str_shipid = match.Groups[1].Value.Replace("%2D", "-");
            int shipid = Convert.ToInt32(str_shipid);

            //艦隊の複製の作成
            List<ApiDeckPort> decks = APIPort.DeckPorts;

            //随伴艦一斉解除の場合
            if(idx == -1)
            {
                List<int> ships = new List<int>();
                ships.Add(decks[fleetid].api_ship[0]);//旗艦のIDの追加
                ships.AddRange(new int[] { -1, -1, -1, -1, -1 });//空の艦×5
                //変更をdeckへ
                decks[fleetid].api_ship = ships;
            }
            //通常の入れ替えの場合
            else
            {
                //該当艦を外す場合
                if(shipid == -1)
                {
                    //該当艦を削除して上へシフト
                    decks[fleetid].api_ship.RemoveAt(idx);
                    //下に-1を追加
                    decks[fleetid].api_ship.Add(-1);
                }
                //入れ替えの場合
                else
                {
                    //艦隊所属の艦どうしの入れ替えかどうかの判定
                    bool switchflag = false;
                    int target_f = -1; int target_i = -1;
                    for (int i = 0; i < decks.Count; i++ )
                    {
                        if (switchflag) break;
                        for (int j = 0; j < decks[i].api_ship.Count; j++ )
                        {
                            //一致した場合
                            if(shipid == decks[i].api_ship[j])
                            {
                                switchflag = true;
                                target_f = i; target_i = j;
                                break;
                            }
                        }
                    }
                    //艦隊所属の艦どうしの入れ替えの場合
                    if (switchflag)
                    {
                        int tmpid = decks[target_f].api_ship[target_i];
                        decks[target_f].api_ship[target_i] = decks[fleetid].api_ship[idx];
                        decks[fleetid].api_ship[idx] = tmpid;
                    }
                    //入れ替え相手がいずれの艦隊にいない場合
                    else
                    {
                        decks[fleetid].api_ship[idx] = shipid;
                    }
                }
            }
        }

        //combined 連合艦隊結成・解除
        public static void Combined(string json)
        {
            var ojson = DynamicJson.Parse(json).api_data;
            APIPort.CombinedFlag = (int)ojson.api_combined;
        }

        //preset_select : プリセット・展開
        public static void PresetSelect(string requestbody, string json)
        {
            //艦隊IDの抽出
            var match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fdeck%5Fid=([0-9]+)");
            int fleetid = Convert.ToInt32(match.Groups[1].Value) - 1;//1引いてインデックス化
            //プリセットの番号の抽出
            match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fpreset%5Fno=([0-9]+)");
            APIGetMember.SelectedPresetNo = Convert.ToInt32(match.Groups[1].Value);
            //JSONをApiDeckPortとしてシリアライズ
            var ojson = DynamicJson.Parse(json).api_data;
            ApiDeckPort preset_deck = ojson.Deserialize<ApiDeckPort>();
            //登録
            if(fleetid >= 0 && fleetid < APIPort.DeckPorts.Count)
            {
                APIPort.DeckPorts[fleetid] = preset_deck;
            }
        }

        //preset_register : プリセット登録
        public static void PresetRegister(string json)
        {
            var ojson = DynamicJson.Parse(json);
            PresetDeck preset = ojson.api_data.Deserialize<PresetDeck>();

            int id = (int)preset.api_preset_no;
            if (preset == null || APIGetMember.Preset == null || APIGetMember.Preset.Count < id) return;

            APIGetMember.Preset[id - 1] = preset;
        }

        //preset_delete : プリセット削除
        public static void PresetDelete(string requestbody)
        {
            var match = System.Text.RegularExpressions.Regex.Match(requestbody, @"api%5Fpreset%5Fno=([0-9]+)");
            int presetid = Convert.ToInt32(match.Groups[1].Value);

            if (APIGetMember.Preset == null || APIGetMember.Preset.Count < presetid) return;

            APIGetMember.Preset[presetid - 1] = new PresetDeck();

            if (APIGetMember.SelectedPresetNo == presetid) APIGetMember.SelectedPresetNo = 0;
        }
    }
}
