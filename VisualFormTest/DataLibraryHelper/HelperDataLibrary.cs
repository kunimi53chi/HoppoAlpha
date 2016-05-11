using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest
{
    //DataLibararyで実装できなかったものヘルパー
    public static class HelperDataLibrary
    {
        #region HoppoAlpha.DataLibary.DataObject
        //敵編成のデータを記録するクラスのヘルパー
        public static class EnemyFleetRecordHelper
        {
            public static EnemyFleetRecord CreateInstance(BattleInfo info, BattleView view)
            {
                EnemyFleetRecord item = new EnemyFleetRecord();

                item.MapAreaID = view.AreaID; item.MapInfoNo = view.MapID; item.CellNo = view.CellID;
                //item.ID = view.EnemyID; 2015/7/17のメンテで廃止
                item.LocalID = view.EnemyLocalID; item.LocalShortID = view.EnemyLocalShortID;
                item.ShipKe = info.api_ship_ke.ToList(); item.ShipLv = info.api_ship_lv.ToList();
                item.ESlot = new Dictionary<int, List<int>>();
                for (int i = 0; i < info.api_eSlot.Count; i++) item.ESlot[i] = info.api_eSlot[i];//リストをネストするとエラーを吐くため
                item.EParam = new Dictionary<int, List<int>>();
                for (int i = 0; i < info.api_eParam.Count; i++) item.EParam[i] = info.api_eParam[i];
                if (info.Formation != null) item.Formation = info.Formation[1];

                return item;
            }
        }
        #endregion
    }
}
