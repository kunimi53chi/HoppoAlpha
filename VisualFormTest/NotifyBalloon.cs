using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;

namespace VisualFormTest
{
    static class NotifyBalloon
    {
        //初期化したか
        public static bool IsNdockInited { get; set; }
        public static bool IsMissionInited { get; set; }
        //バルーンのデータ
        public static SortedDictionary<DateTime, BalloonItem> BalloonData { get; set; }

        //内部クラス
        #region 内部クラス
        //バルーン用のクラス
        public class BalloonItem
        {
            public BalloonInfoType Type { get; set; }
            public string Message { get; set; }
            public DateTime ExecuteTime { get; set; }
            public int NdockID { get; set; }
        }

        //バルーンの情報
        public enum BalloonInfoType
        {
            None, Mission, Ndock,
        }
        #endregion

        static NotifyBalloon()
        {
            BalloonData = new SortedDictionary<DateTime, BalloonItem>();
        }

        //入渠ドックバルーンの初期化
        public static void InitNdock()
        {
            foreach(ApiNdock ndock in APIPort.Ndocks)
            {
                if (ndock.api_ship_id == 0) continue;
                BalloonItem item = GetInstanceNdock(ndock.api_ship_id, ndock.api_complete_time, ndock.api_id);
                BalloonData[item.ExecuteTime] = item;
            }
            IsNdockInited = true;
        }

        //入渠ドックバルーンの追加
        public static void AddNdock(int shipid, int ndockid)
        {
            BalloonItem item = GetInstanceNdock(shipid, ndockid);
            BalloonData[item.ExecuteTime] = item;
        }

        //入渠ドックのバルーンインスタンスを生成（ちょっと精度落ちるかも）
        public static BalloonItem GetInstanceNdock(int shipid, int ndockid)
        {
            ApiShip ship = APIPort.ShipsDictionary[shipid];
            string name = ship.ShipName;
            int milsec = ship.api_ndock_time;
            DateTime endtime = DateTime.Now.AddMilliseconds(milsec);//入渠完了時間
            //バルーンアイテム
            BalloonItem b = new BalloonItem()
            {
                Type = BalloonInfoType.Ndock,
                Message = string.Format("{0}(Lv{1})の修理が完了しました", name, ship.api_lv),
                ExecuteTime = endtime - new TimeSpan(0, 1, 0),
                NdockID = ndockid,
            };
            return b;
        }

        //入渠ドックのバルーンインスタンス（精度良い）
        public static BalloonItem GetInstanceNdock(int shipid, long epochs, int ndockid)
        {
            ApiShip ship = APIPort.ShipsDictionary[shipid];
            string name = ship.ShipName;
            DateTime endtime = KancolleInfo.EpochmsToDate(epochs);//入渠完了時間
            //バルーンアイテム
            BalloonItem b = new BalloonItem()
            {
                Type = BalloonInfoType.Ndock,
                Message = string.Format("{0}(Lv{1})の修理が完了しました", name, ship.api_lv),
                ExecuteTime = endtime - new TimeSpan(0, 1, 0),
                NdockID = ndockid,
            };
            return b;
        }

        //削除チェック
        public static void RemoveNdock(int ndockid)
        {
            BalloonItem item = (from x in BalloonData
                                where x.Value.NdockID == ndockid
                                select x.Value).First();
            if (item == null) return;
            BalloonData.Remove(item.ExecuteTime);
        }

        //遠征の初期化
        public static void InitMissions()
        {
            foreach(ApiDeckPort deck in APIPort.DeckPorts)
            {
                long completetime = deck.api_mission[2];
                if(completetime == 0) continue;
                if (KancolleInfo.EpochmsToDate(completetime) <= DateTime.Now) continue;
                BalloonItem item = GetInstanceMission(deck.api_id, completetime);
                BalloonData[item.ExecuteTime] = item;
            }
            IsMissionInited = true;
        }

        //遠征の追加
        public static void AddMission(int fleetid, long completetime)
        {
            BalloonItem item = GetInstanceMission(fleetid, completetime);
            BalloonData[item.ExecuteTime] = item;
        }

        //遠征の変更
        public static void ModifyMission(int fleetid, long oldCompletetime, long newCompletetime)
        {
            DateTime oldtime = KancolleInfo.EpochmsToDate(oldCompletetime) - new TimeSpan(0, 1, 0);
            if (!BalloonData.ContainsKey(oldtime)) return;
            BalloonItem newitem = GetInstanceMission(fleetid, newCompletetime);
            //削除
            BalloonData.Remove(oldtime);
            //追加
            BalloonData[newitem.ExecuteTime] = newitem;
        }

        //遠征バルーンのインスタンス
        public static BalloonItem GetInstanceMission(int fleetid, long completetime)
        {
            DateTime endtime = KancolleInfo.EpochmsToDate(completetime);
            //バルーンアイテム
            BalloonItem b = new BalloonItem()
            {
                Type = BalloonInfoType.Mission,
                Message = string.Format("第{0}艦隊が遠征から帰投します", fleetid),
                ExecuteTime = endtime - new TimeSpan(0, 1, 0),
            };
            return b;
        }
    }
}
