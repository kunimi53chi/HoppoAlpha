using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.DataObject;

namespace HoppoAlpha.DataLibrary.RawApi.ApiGetMember
{
    public class BaseAirCorp
    {
        public int api_rid { get; set; }
        public string api_name { get; set; }
        public int api_distance { get; set; }
        public int api_action_kind { get; set; }
        public List<ApiPlaneInfo> api_plane_info { get; set; }

        public static readonly int NumOfSquadron = 4;
        public static readonly int NumOfAirBase = 3;

        /// <summary>
        /// 基地航空隊の制空値を計算します
        /// </summary>
        /// <param name="slotData">装備データ</param>
        /// <returns>基地航空隊の制空値</returns>
        public AirSupResult GetBaseAirSup(Dictionary<int, SlotItem> slotData)
        {
            var result = new AirSupResult();
            if (api_plane_info == null) return result;

            foreach(var plane in api_plane_info)
            {
                result = result.Merge(plane.GetPlaneAirSup(slotData));
            }
            return result;
        }

        #region ビュー
        public class ViewStatus
        {
            public ApiPlaneInfo.ViewStatus[] SquadronView { get; set; }
            public int TotalNum { get; set; }
            public int TotalCost { get; set; }
            public int TotalRedius { get; set; }
            public int TotalDispatch { get; set; }
            public AirSupResult TotalAirSup { get; set; }

            public ViewStatus()
            {
                SquadronView = new ApiPlaneInfo.ViewStatus[NumOfSquadron];


                TotalAirSup = new AirSupResult();
            }
        }

        /// <summary>
        /// ステータス : 基地航空隊のビュー用のデータを取得します
        /// </summary>
        /// <param name="slotData"></param>
        /// <returns></returns>
        public ViewStatus GetViewStatus(Dictionary<int, SlotItem> slotData)
        {
            if (api_plane_info == null) return null;

            var result = new ViewStatus();

            foreach(var i in Enumerable.Range(0, NumOfSquadron))
            {
                var sq = api_plane_info[i].GetStatusViewData(slotData);

                result.SquadronView[i] = sq;

                result.TotalNum += api_plane_info[i].api_count;
                if (sq != null)
                {
                    result.TotalCost += sq.Cost;

                    result.TotalRedius = Math.Max(result.TotalRedius, sq.Radius);

                    result.TotalDispatch += sq.Dispatch;
                    result.TotalAirSup = result.TotalAirSup.Merge(sq.AirSup);
                }
            }

            return result;
        }
        #endregion

    }


    public class ApiPlaneInfo
    {
        public int api_squadron_id { get; set; }
        public int api_state { get; set; }
        public int api_slotid
        {
            get { return _api_slot_id; }
            set { _api_slot_id = value; oslot = null; mstSlot = null; _airsup = null; }
        }
        public int api_count
        {
            get { return _api_count; }
            set { _api_count = value; _airsup = null; }
        }
        public int api_max_count { get; set; }
        public int api_cond { get; set; }

        private int _api_slot_id, _api_count;
        SlotItem oslot = null;
        ExMasterSlotitem mstSlot = null;
        AirSupResult _airsup = null;

        /// <summary>
        /// 中隊（航空機）あたりの制空値を計算します
        /// </summary>
        /// <param name="slotData">装備データ</param>
        /// <returns>中隊あたりの制空値</returns>
        public AirSupResult GetPlaneAirSup(Dictionary<int, SlotItem> slotData)
        {
            if (_airsup == null)
            {
                if (slotData == null)
                {
                    throw new NullReferenceException("GetPlaneAirSup で slotDataがNullです");
                }

                if (mstSlot == null)
                {
                    if (oslot == null)
                    {
                        if (!slotData.TryGetValue(_api_slot_id, out oslot)) throw new KeyNotFoundException("GetPlaneAirSup で 対象の装備データを取得できません");
                    }
                    mstSlot = oslot.DSlotitem;
                    if (mstSlot == null)
                    {
                        throw new KeyNotFoundException("GetPlaneAirSup で 対象のマスターデータを取得できません");
                    }
                }

                _airsup = AirSupResult.SingleSlotitemAirSup(mstSlot, (double)_api_count, oslot.api_alv);
            }

            return _airsup;
        }

        #region ビュー
        public class ViewStatus
        {
            public string PlaneName { get; set; }
            public string Training { get; set; }
            public int Cost { get; set; }
            public int Radius { get; set; }
            public int Dispatch { get; set; }
            public AirSupResult AirSup { get; set; }

            public ViewStatus()
            {
                AirSup = new AirSupResult();
            }
        }

        /// <summary>
        /// ステータス　：　中隊ベースのビュー用のデータを取得します
        /// </summary>
        /// <param name="slotData">装備データ</param>
        /// <returns>ビュー用のデータ</returns>
        public ViewStatus GetStatusViewData(Dictionary<int, SlotItem> slotData)
        {
            if(slotData == null) return null;

            if(oslot == null)
            {
                if (!slotData.TryGetValue(_api_slot_id, out oslot)) return null;
            }
            if(mstSlot == null)
            {
                mstSlot = oslot.DSlotitem;
                if(mstSlot == null) return null;
            }

            var airsup = GetPlaneAirSup(slotData);

            var result = new ViewStatus()
            {
                PlaneName = mstSlot.api_name,
                Training = "◆" + oslot.api_alv.ToString(),
                Cost = mstSlot.api_cost,
                Radius = mstSlot.api_distance,
                Dispatch = mstSlot.api_cost * this.api_max_count,
                AirSup = airsup,
            };

            return result;
        }
        #endregion
    }
}
