using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using HoppoAlpha.DataLibrary.DataObject;

namespace HoppoAlpha.DataLibrary.RawApi.ApiGetMember
{
    public class SlotItem
    {
        public int api_id { get; set; }
        public int api_slotitem_id { get; set; }
        public int api_locked { get; set; }
        public int api_level { get; set; }
        public int api_alv { get; set; }

        //マスターデータ
        /// <summary>
        /// 装備のマスターデータ
        /// </summary>
        public static ExMasterSlotitemCollection MasterSlotitems { get; set; }
        /// <summary>
        /// 装備の種類のマスターデータ
        /// </summary>
        public static List<ApiMstSlotitemEquiptype> MasterSlotitemEquipTypes { get; set; }

        //マスターデータを必要とするプロパティ
        #region マスターデータから取得するプロパティ
        //マスターデータのオブジェクト
        private ExMasterSlotitem _exMasterSlotitem = null;

        public ExMasterSlotitem DSlotitem
        {
            get
            {
                //キャッシュされていない場合
                if (_exMasterSlotitem == null)
                {
                    //マスターデータが登録されていない場合
                    if (MasterSlotitems == null) throw new NullReferenceException("MasterSlotitemsがNullです");
                    //マスターデータのキャッシュ
                    MasterSlotitems.TryGetValue(this.api_slotitem_id, out _exMasterSlotitem);
                }
                return _exMasterSlotitem;
            }
        }
        #endregion
    }
}
