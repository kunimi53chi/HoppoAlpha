using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using HoppoAlpha.DataLibrary.DataObject;
using Codeplex.Data;

namespace VisualFormTest
{
    static class APIMaster
    {
        //APIデータの保存
        public static ExMasterShipCollection MstShips { get; private set; }
        //public static List<ApiMstShipgraph> MstShipgraphs { get; set; }
        public static List<ApiMstSlotitemEquiptype> MstSlotitemEquiptypes { get; set; }
        public static Dictionary<int, ApiMstStype> MstStypesDictionary { get; private set; }
        public static ExMasterSlotitemCollection MstSlotitems { get; private set; }//ここまで
        //public static List<ApiMstSlotitemgraph> MstSlotitemgraphs { get; set; }
        public static List<ApiMstFurniture> MstFurnitures { get; set; }
        //public static List<ApiMstFurnituregraph> MstFurnituregraphs { get; set; }
        public static List<ApiMstUseitem> MstUseitems { get; set; }
        //public static List<ApiMstPayitem> MstPayitems { get; set; }
        //public static List<ApiMstItemShop> MstItemShops { get; set; }
        public static List<ApiMstMaparea> MstMapareas { get; set; }
        public static List<ApiMstMapinfo> MstMapinfos { get; set; }
        //public static List<ApiMstMapbgm> MstMapbgms { get; set; }
        public static List<ApiMstMapcell> MstMapcells { get; set; }
        public static List<ApiMstMission> MstMissions { get; set; }
        public static ApiMstConst MstConst { get; set; }
        //public static List<ApiMstShipupgrade> MstShipupgrades { get; set; }
        //public static List<ApiMstBgm> MstBgms { get; set; }


        //api_start2からのアップデート
        public static void ReadApiStart2(string json)
        {
            //JSONオブジェクトの作成
            var ojson = DynamicJson.Parse(json);
            //ApiMstShipの読み込み
            List<ApiMstShip> master_ship = ojson.api_data.api_mst_ship.Deserialize<List<ApiMstShip>>();
            //ExMasterShipsロード
            if (MstShips == null)
            {
                var loadmstships = HoppoAlpha.DataLibrary.Files.TryLoad(
                    HoppoAlpha.DataLibrary.DataObject.ExMasterShipCollection.FilePath, HoppoAlpha.DataLibrary.DataType.ExMasterShip);
                LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.ExMasterShip, loadmstships, false);

                //ロードが失敗した場合
                if (!loadmstships.IsSuccess && loadmstships.ErrorReason != "File not found.")
                {
                    while (true)
                    {
                        var dialog = System.Windows.Forms.MessageBox.Show("船のマスターデータの読み込み失敗しました\n" + loadmstships.ErrorReason, "読込",
                                    System.Windows.Forms.MessageBoxButtons.RetryCancel, System.Windows.Forms.MessageBoxIcon.Exclamation);
                        //再試行
                        if (dialog == System.Windows.Forms.DialogResult.Retry)
                        {
                            loadmstships = HoppoAlpha.DataLibrary.Files.TryLoad(
                                HoppoAlpha.DataLibrary.DataObject.ExMasterShipCollection.FilePath, HoppoAlpha.DataLibrary.DataType.ExMasterShip);
                            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.ExMasterShip, loadmstships, false);
                            if (loadmstships.IsSuccess || loadmstships.ErrorReason == "File not found.") break;
                        }
                        //キャンセル
                        else if(dialog == System.Windows.Forms.DialogResult.Cancel)
                        {
                            Environment.Exit(-1);//強制終了
                        }
                    }
                }

                MstShips = (ExMasterShipCollection)loadmstships.Instance;
            }
            MstShips.MergeMasterData(master_ship);
            //ApiMstShipgraphの読み込み
            //MstShipgraphs = ojson.api_data.api_mst_shipgraph.Deserialize<List<ApiMstShipgraph>>();
            //ApiMstSlotitemEquiptypeの読み込み
            MstSlotitemEquiptypes = ojson.api_data.api_mst_slotitem_equiptype.Deserialize<List<ApiMstSlotitemEquiptype>>();

            //ApiMstStypeの読み込み
            List<ApiMstStype> master_stype = ojson.api_data.api_mst_stype.Deserialize<List<ApiMstStype>>();
            //api_mst_stype.api_equip_typeの修正
            int cnt = 0;
            foreach (var x in ojson.api_data.api_mst_stype)
            {
                //ビットフラグ化して修正
                master_stype[cnt].SetBitflag(x.api_equip_type.ToString());
                cnt++;
            }
            //↑のDic化
            MstStypesDictionary = new Dictionary<int, ApiMstStype>();
            foreach(ApiMstStype s in master_stype)
            {
                MstStypesDictionary[s.api_id] = s;
            }

            //ApiMstSlotitemの読み込み
            List<ApiMstSlotitem> master_slotitem = ojson.api_data.api_mst_slotitem.Deserialize<List<ApiMstSlotitem>>();
            //ExMasterSlotitemのロード
            if (MstSlotitems == null)
            {
                var loadslotitem = HoppoAlpha.DataLibrary.Files.TryLoad(
                    HoppoAlpha.DataLibrary.DataObject.ExMasterSlotitemCollection.FilePath, HoppoAlpha.DataLibrary.DataType.ExMasterSlotitem);
                LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.ExMasterSlotitem, loadslotitem, false);

                //ロードが失敗した場合
                if (!loadslotitem.IsSuccess && loadslotitem.ErrorReason != "File not found.")
                {
                    while (true)
                    {
                        var dialog = System.Windows.Forms.MessageBox.Show("装備のマスターデータの読み込み失敗しました\n" + loadslotitem.ErrorReason, "読込",
                                    System.Windows.Forms.MessageBoxButtons.RetryCancel, System.Windows.Forms.MessageBoxIcon.Exclamation);
                        //再試行
                        if (dialog == System.Windows.Forms.DialogResult.Retry)
                        {
                            loadslotitem = HoppoAlpha.DataLibrary.Files.TryLoad(
                                HoppoAlpha.DataLibrary.DataObject.ExMasterSlotitemCollection.FilePath, HoppoAlpha.DataLibrary.DataType.ExMasterSlotitem);
                            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.ExMasterSlotitem, loadslotitem, false);
                            if (loadslotitem.IsSuccess || loadslotitem.ErrorReason == "File not found.") break;
                        }
                        //キャンセル
                        else if (dialog == System.Windows.Forms.DialogResult.Cancel)
                        {
                            Environment.Exit(-1);//強制終了
                        }
                    }
                }

                MstSlotitems = (ExMasterSlotitemCollection)loadslotitem.Instance;
            }
            MstSlotitems.MergeMasterData(master_slotitem);
            //ApiMstSlotitemをApiMstShipsにマージして制空値を計算（強制再計算はOFF）
            MstShips.CalcMasterAirSuperiorityAndWeightedAntiAir(MstSlotitems, false);
            //ApiMstSlotItemGraphの読み込み
           // MstSlotitemgraphs = ojson.api_data.api_mst_slotitemgraph.Deserialize<List<ApiMstSlotitemgraph>>();

            //ApiMstFurnitureの読み込み
            MstFurnitures = ojson.api_data.api_mst_furniture.Deserialize<List<ApiMstFurniture>>();
            //ApiMstFurnituregraphの読み込み
            //MstFurnituregraphs = ojson.api_data.api_mst_furnituregraph.Deserialize<List<ApiMstFurnituregraph>>();

            //ApiMstUseitemの読み込み
            MstUseitems = ojson.api_data.api_mst_useitem.Deserialize<List<ApiMstUseitem>>();
            //ApiMstPayitemの読み込み
            //MstPayitems = ojson.api_data.api_mst_payitem.Deserialize<List<ApiMstPayitem>>();
            //ApiMstItemShopの読み込み
            //MstItemShops = ojson.api_data.api_mst_furniture.Deserialize<List<ApiMstItemShop>>();

            //ApiMstMapareaの読み込み
            MstMapareas = ojson.api_data.api_mst_maparea.Deserialize<List<ApiMstMaparea>>();
            //ApiMstMapinfoの読み込み
            MstMapinfos = ojson.api_data.api_mst_mapinfo.Deserialize<List<ApiMstMapinfo>>();
            //ApiMstMapbgmの読み込み
            //MstMapbgms = ojson.api_data.api_mst_mapbgm.Deserialize<List<ApiMstMapbgm>>();
            //ApiMstMapcellの読み込み
            MstMapcells = ojson.api_data.api_mst_mapcell.Deserialize<List<ApiMstMapcell>>();

            //ApiMstMissionの読み込み
            MstMissions = ojson.api_data.api_mst_mission.Deserialize<List<ApiMstMission>>();

            //ApiMstConstの読み込み
            MstConst = ojson.api_data.api_mst_const.Deserialize<ApiMstConst>();
            //ApiMstShipupgradeの読み込み
            //MstShipupgrades = ojson.api_data.api_mst_shipupgrade.Deserialize<List<ApiMstShipupgrade>>();
            //ApiMstBgmの読み込み
            //MstBgms = ojson.api_data.api_mst_bgm.Deserialize<List<ApiMstBgm>>();

            //マスターデータのセット
            ExCommon.SetMasterData(MstShips, MstStypesDictionary, MstSlotitems, MstSlotitemEquiptypes);
        }

        //遠征の名前の読み込み
        public static string GetMissionName(int id)
        {
            var query = from x in MstMissions
                        where x.api_id == id
                        select x;
            return query.First().api_name;
        }
    }

}
