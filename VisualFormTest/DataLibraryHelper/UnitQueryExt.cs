using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.DataObject;
using Codeplex.Data;

namespace VisualFormTest
{
    public static class UnitQueryExt
    {
        //デフォルトではAND検索でチェック
        public static bool CheckAll(this UnitQuery query, ApiShip oship)
        {
            if (!query.IsOr)
            {
                //AND検索
                foreach (UnitQueryItem x in query.Query)
                {
                    if (!x.Check(oship)) return false;
                }
                return true;
            }
            else
            {
                //OR検索
                foreach (UnitQueryItem x in query.Query)
                {
                    if (x.Check(oship)) return true;
                }
                return false;
            }
        }

        //JSONに変換
        public static string ToJson(this UnitQuery query)
        {
            return DynamicJson.Serialize(query);
        }
    }

    public static class UnitQueryItemExt
    {
        //これをJSONに
        public static string ToJson(this UnitQueryItem item)
        {
            string str = DynamicJson.Serialize(item);
            return str;
        }
        //SeachをJSONに
        public static string ToSearchesJson(this UnitQueryItem item)
        {
            string str = DynamicJson.Serialize(item.Searches);
            return str;
        }
        //JSONをSearchesに
        public static List<UnitQueryItemSearchBase> FromJsonToSearches(string json)
        {
            var ojson = DynamicJson.Parse(json);
            return ojson.Deserialize<List<UnitQueryItemSearchBase>>();
        }


        //Targetのインスタンスを取得
        public static object GetInstance(this UnitQueryItem item, ApiShip oship)
        {
            object instance = null;
            //値の取得
            switch (item.TargetEnum)
            {
                case UnitQueryMode.ID:
                    instance = oship.api_id;
                    break;
                case UnitQueryMode.Name:
                    instance = oship.ShipName;
                    break;
                case UnitQueryMode.Lv:
                    instance = oship.api_lv;
                    break;
                case UnitQueryMode.ShipType:
                    instance = oship.DShip.api_stype;
                    break;
                case UnitQueryMode.ShipTypeName://5
                    instance = oship.ShipTypeName;
                    break;
                case UnitQueryMode.Cond:
                    instance = oship.api_cond;
                    break;
                case UnitQueryMode.NdockTime:
                    instance = oship.api_ndock_time;
                    break;
                case UnitQueryMode.HP:
                    instance = oship.api_nowhp;
                    break;
                case UnitQueryMode.MaxHP:
                    instance = oship.api_maxhp;
                    break;
                case UnitQueryMode.HPRatio://10
                    instance = (double)oship.api_nowhp / (double)oship.api_maxhp;
                    break;
                case UnitQueryMode.IsLatestRemodeling:
                    instance = Convert.ToInt32(oship.DShip.api_aftershipid == "0");
                    break;
                case UnitQueryMode.EXP:
                    instance = oship.api_exp[0];
                    break;
                case UnitQueryMode.Fire:
                    instance = oship.api_karyoku[0];
                    break;
                case UnitQueryMode.Torpedo:
                    instance = oship.api_raisou[0];
                    break;
                case UnitQueryMode.NightFire://15
                    instance = oship.api_karyoku[0] + oship.api_raisou[0];
                    break;
                case UnitQueryMode.AirSup:
                    instance = oship.GetAirSupValue(APIGetMember.SlotItemsDictionary, APIPort.IsWithdrawn, APIPort.DeckPorts).AirSupValueMax;
                    break;
                case UnitQueryMode.CarrierFire:
                    instance = KancolleInfoFleet.CalcCarrierFire(oship);
                    break;
                case UnitQueryMode.AntiSubmarine:
                    instance = oship.api_taisen[0];
                    break;
                case UnitQueryMode.AntiAir:
                    instance = oship.api_taiku[0];
                    break;
                case UnitQueryMode.Defense://20
                    instance = oship.api_soukou[0];
                    break;
                case UnitQueryMode.Evasion:
                    instance = oship.api_kaihi[0];
                    break;
                case UnitQueryMode.AircraftNum:
                    instance = oship.api_onslot.Sum();
                    break;
                case UnitQueryMode.Range:
                    instance = oship.api_leng;
                    break;
                case UnitQueryMode.Search:
                    instance = oship.api_sakuteki[0];
                    break;
                case UnitQueryMode.Luck://25
                    instance = oship.api_lucky[0];
                    break;
                case UnitQueryMode.IsLocked:
                    instance = oship.api_locked;
                    break;
                case UnitQueryMode.FuelNow:
                    instance = oship.api_fuel;
                    break;
                case UnitQueryMode.FuelMax:
                    instance = oship.DShip.api_fuel_max;
                    break;
                case UnitQueryMode.FuelRatio:
                    instance = (double)oship.api_fuel / (double)oship.DShip.api_fuel_max;
                    break;
                case UnitQueryMode.BullNow://30
                    instance = oship.api_bull;
                    break;
                case UnitQueryMode.BullMax:
                    instance = oship.DShip.api_bull_max;
                    break;
                case UnitQueryMode.BullRatio:
                    instance = (double)oship.api_bull / (double)oship.DShip.api_bull_max;
                    break;
                case UnitQueryMode.IsFleetAssign:
                    var fleets = APIPort.DeckPorts.SelectMany(x => x.api_ship).Where(x => x != -1);
                    instance = fleets.Contains(oship.api_id) ? 1 : 0;
                    break;
                case UnitQueryMode.IsBath:
                    var ndock = APIPort.Ndocks.Select(x => x.api_ship_id).Where(x => x != 0);
                    instance = ndock.Contains(oship.api_id) ? 1 : 0;
                    break;
                case UnitQueryMode.SallyArea:
                    instance = oship.api_sally_area;
                    break;
            }
            return instance;
        }

        //SearchBase単体でのチェック
        public static bool Check(this UnitQueryItem item, ApiShip oship)
        {
            object instance = item.GetInstance(oship);
            //デフォルトでは全OR検索
            if (!item.IsAnd)
            {
                foreach (UnitQueryItemSearchBase x in item.Searches)
                {
                    if (x.Checker(oship, instance)) return (true ^ item.IsNot);
                }
                return (false ^ item.IsNot);
            }
            //AND検索
            else
            {
                foreach (UnitQueryItemSearchBase x in item.Searches)
                {
                    if (!x.Checker(oship, instance)) return (false ^ item.IsNot);
                }
                return (true ^ item.IsNot);
            }
        }
    }

    public static class UnitQueryItemSearchBaseExt
    {
        //JSONから変換
        public static List<UnitQueryItemSearchBase> FromJson(string json)
        {
            var ojson = DynamicJson.Parse(json);
            return ojson.Deserialize<List<UnitQueryItemSearchBase>>();
        }
    }

    public static class UnitQueryModeExt
    {
        //ToolTipのメッセージ
        public static string GetToolTipMessage(this UnitQueryMode u)
        {
            StringBuilder sb;
            switch (u)
            {
                case UnitQueryMode.ID: return "艦娘ごとに割り当てられている固有のIDで判定します。\nIDは艦娘リストに表示されているIDと一致します。";
                case UnitQueryMode.Name: return "艦娘の名前で判定します。\n例：吹雪、伊58、長門改…。";
                case UnitQueryMode.Lv: return "艦娘のレベルで判定します。";
                case UnitQueryMode.ShipType:
                    sb = new StringBuilder();
                    sb.AppendLine("艦種をマスターデータで管理されているIDで判定します。");
                    sb.AppendLine("この値は、艦種ごとに異なる整数を取ります。");
                    sb.AppendLine("--値一覧 : --");
                    foreach (var x in APIMaster.MstStypesDictionary)
                    {
                        sb.AppendFormat("{0}={1} ", x.Key, x.Value.api_name);
                        if (x.Key % 5 == 4) sb.AppendLine();
                    }
                    return sb.ToString();
                case UnitQueryMode.ShipTypeName:
                    return "艦種を文字列で判定します。\n文字列の検索モードが完全一致の場合、\nマスターデータで定義されている艦種名と同じでなければいけません。\n例：完全一致＋雷巡→×, 完全一致＋重雷装巡洋艦→○, 前方一致＋重雷装→○";//5
                case UnitQueryMode.Cond: return "Cond値で判定します";
                case UnitQueryMode.NdockTime: return "入渠時間をミリ秒単位で判定します。\n最大値は 2147483647 です。";
                case UnitQueryMode.HP: return "現在の耐久(HP)で判定します。";
                case UnitQueryMode.MaxHP: return "耐久(HP)最大値で判定します。";
                case UnitQueryMode.HPRatio:
                    return "現在の耐久(HP)÷耐久(HP)最大値 で判定します。\n小破、中破などの損傷条件で判定したい場合は範囲で指定してください\n・大破：x ≦0.25\n・中破 : 0.25 ＜ x ≦ 0.5\n・小破 : 0.5 ＜ x ≦ 0.75\n・無傷 : x ＞ 0.75\n[Tips]「aより大きくbより小さい」といった判定を用いたい場合は、\nこのクエリアイテムのANDモードをONにしてください。";//10
                case UnitQueryMode.IsLatestRemodeling: return "最終改造であるかどうかを判定します。\nこれ以上改造不可能ならば1を、改造可能ならば0を返します";
                case UnitQueryMode.EXP: return "Lv1からの累積経験値で判定します。\nLv99でカンストしている場合はこの値は1000000(100万)です";
                case UnitQueryMode.Fire: return "現在の火力で判定します。\nこれは編成で表示される値で、キャップ補正・消費弾薬補正等は含まれません。";
                case UnitQueryMode.Torpedo: return "現在の雷装で判定します。\nこれは編成で表示される値で、キャップ補正・消費弾薬補正等は含まれません。";
                case UnitQueryMode.NightFire: return "夜戦火力(火力+雷装)で判定します。\nキャップ補正・消費弾薬補正等は含まれません。";//15
                case UnitQueryMode.AirSup: return "現在の制空値で判定します。";
                case UnitQueryMode.CarrierFire: return "空母の昼戦の砲雷撃戦時の実質的な火力で判定します。この値は、(火力+雷装)×1.5+爆装×2+55 で計算されます。\nキャップ補正・消費弾薬補正等は含まれません。";
                case UnitQueryMode.AntiSubmarine: return "現在の対潜で判定します。\nこれは編成で表示される値で、キャップ補正・消費弾薬補正等は含まれません。";
                case UnitQueryMode.AntiAir: return "現在の対空で判定します。\nこれは編成で表示される値です。";
                case UnitQueryMode.Defense: return "現在の装甲で判定します。";//20
                case UnitQueryMode.Evasion: return "現在の回避で判定します。\nこれは編成で表示される値で、疲労補正・消費燃料補正等は含まれません。";
                case UnitQueryMode.AircraftNum: return "現在の搭載数で判定します。\n艦載機を装備していないスロットの搭載数も合計して計算されます。";
                case UnitQueryMode.Range: return "艦娘の射程を数値で判定します。\n例 : 0 = なし, 1 = 短, 2 = 中, …, 5 = 超超長";
                case UnitQueryMode.Search: return "現在の索敵で判定します。\nこれは編成で表示される値で、2-5式等のルート制御の計算結果とは異なります。";
                case UnitQueryMode.Luck: return "現在の運で判定します。";//25
                case UnitQueryMode.IsLocked: return "艦娘がロックされているかを判定します。\nロックされている場合は1を、ロックされていなければ0を返します";
                case UnitQueryMode.FuelNow: return "現在の燃料残量で判定します。\nこの値は残目盛り数とは異なり、\nケッコンしていない場合、燃料最大値からこの値を引くと補給に必要な燃料と一致します。\nケッコン済の場合は、引き算の結果に燃費向上補正をかけたものと一致します。";
                case UnitQueryMode.FuelMax: return "燃料搭載の最大値で判定します。\nこの値は残目盛り数とは異なります。";
                case UnitQueryMode.FuelRatio: return "燃料の残量÷燃料最大値で判定します。\nこの値はおよそ残目盛り数と一致します。";
                case UnitQueryMode.BullNow:
                    return "現在の弾薬残量で判定します。\nこの値は残目盛り数とは異なり、\nケッコンしていない場合、弾薬最大値からこの値を引くと補給に必要な弾薬と一致します。\nケッコン済の場合は、引き算の結果に燃費向上補正をかけたものと一致します。";//30
                case UnitQueryMode.BullMax: return "弾薬搭載の最大値で判定します。\nこの値は残目盛り数とは異なります。";
                case UnitQueryMode.BullRatio: return "弾薬の残量÷弾薬最大値で判定します。\nこの値はおよそ残目盛り数と一致します。";
                case UnitQueryMode.IsFleetAssign: return "いずれかの艦隊に配属されているかで判定します。\n配属されてなければ0、配属されていれば1を返します。";
                case UnitQueryMode.IsBath: return "入渠しているを判定します。\n入渠していなければ0、入渠していれば1を返します。";
                case UnitQueryMode.SallyArea: return "出撃マップの制限を判定します。\nイベント期間中以外は常に0となります。";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
