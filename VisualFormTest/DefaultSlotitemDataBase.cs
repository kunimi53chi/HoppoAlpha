using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HoppoAlpha.DataLibrary.DataObject;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using HoppoAlpha.DataLibrary.Const;

namespace VisualFormTest
{
    public static class DefaultSlotitemDataBase
    {
        //出撃前のバッファー
        static List<ShipBuffer> BeforeSortie { get; set; }
        //出撃後のバッファー
        static List<ShipBuffer> AfterReturn { get; set; }

        static RemodelBuffer Remodeling { get; set; }

        //内部クラス
        #region 内部クラス
        public class ShipBuffer
        {
            public int ID { get; set; }
            public int ShipID { get; set; }
            public List<int> MstSlotItemID { get; set; }

            public ShipBuffer(ApiShip oship)
            {
                this.ID = oship.api_id;
                this.ShipID = oship.api_ship_id;
                this.MstSlotItemID = oship.api_slot.Select(delegate(int x)
                {
                    SlotItem oslot;
                    if (APIGetMember.SlotItemsDictionary.TryGetValue(x, out oslot)) return oslot.api_slotitem_id;
                    else return -1;
                }).ToList();
            }
        }

        //比較するクラス
        public class ShipBufferComparer : EqualityComparer<ShipBuffer>
        {
            public override bool Equals(ShipBuffer x, ShipBuffer y)
            {
                if (x == null || y == null) return false;

                if (x.ID != y.ID) return false;
                if (x.ShipID != y.ShipID) return false;
                if (x.MstSlotItemID.Count != y.MstSlotItemID.Count) return false;
                foreach(int i in Enumerable.Range(0, x.MstSlotItemID.Count))
                {
                    if (x.MstSlotItemID[i] != y.MstSlotItemID[i]) return false;
                }

                return true;
            }

            public override int GetHashCode(ShipBuffer obj)
            {
                return obj.ID.GetHashCode();
            }
        }

        //改造用のバッファー
        public class RemodelBuffer
        {
            public int ShipId { get; set; }
            public bool IsShip3Loaded { get; set; }
            public bool IsSlotitemLoaded { get; set; }
        }
        #endregion


        //記録する
        public static void AddCollection(int masterShipId, List<int> mstSlotitemId)
        {            
            //Masterから取れない場合
            ExMasterShip dship;
            if(!APIMaster.MstShips.TryGetValue(masterShipId, out dship)) return;

            //初期装備のデータがない場合
            if (dship.DefaultSlotItem == null) dship.DefaultSlotItem = mstSlotitemId;
        }

        //出撃前の処理
        public static void BeforeSortieProcess()
        {
            //出撃前のLv1の艦のバッファーを作る
            BeforeSortie = APIPort.ShipsDictionary.Values.Where(x => x.api_lv == 1).Select(x => new ShipBuffer(x)).ToList();
            AfterReturn = null;
        }

        //出撃後の処理
        public static void AfterReturnProcess()
        {
            //出撃前のバッファーがない場合
            if (BeforeSortie == null) return;

            //帰投時のLv1の艦のバッファーを作る
            AfterReturn = APIPort.ShipsDictionary.Values.Where(x => x.api_lv == 1).Select(x => new ShipBuffer(x)).ToList();
            //差分を取得
            var diff = AfterReturn.Except(BeforeSortie, new ShipBufferComparer());//Comparerが重要

            //差分の追加
            foreach(var x in diff)
            {
                AddCollection(x.ShipID, x.MstSlotItemID);
            }

            //バッファーをクリアする
            BeforeSortie = null;
            AfterReturn = null;
        }

        //改造3セット1つ目
        public static void StartRemodeling(int shipid)
        {
            Remodeling = new RemodelBuffer();

            Remodeling.ShipId = shipid;
        }

        //改造3セット2つ目
        public static void MiddleRemodeling_Ship3()
        {
            //1つ目が呼ばれた場合のみ処理する
            if (Remodeling == null || Remodeling.ShipId <= 0) return;

            Remodeling.IsShip3Loaded = true;
        }

        //改造3セット3つ目
        public static void EndRemodeling_Slotitem()
        {
            //1つ目2つ目が呼ばれた場合のみ処理する
            if (Remodeling == null || Remodeling.ShipId <= 0 || !Remodeling.IsShip3Loaded) return;

            Remodeling.IsSlotitemLoaded = true;//いらないかも

            //変更済みの該当艦を呼ぶ
            ApiShip oship;
            if (!APIPort.ShipsDictionary.TryGetValue(Remodeling.ShipId, out oship)) return;

            //装備一覧
            List<int> slotitems = new List<int>();
            foreach(var sid in oship.api_slot)
            {
                SlotItem oslot;
                if(APIGetMember.SlotItemsDictionary.TryGetValue(sid, out oslot))
                {
                    slotitems.Add(oslot.api_slotitem_id);
                }
                else
                {
                    slotitems.Add(-1);
                }
            }

            //コレクションに登録
            AddCollection(oship.api_ship_id, slotitems);

            //終了処理
            Remodeling = null;
        }
    }
}
