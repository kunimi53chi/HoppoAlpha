using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest.UserControls
{
    public partial class AirBaseCorps_PlaneLookup : Form
    {
        public AirBaseCorps_PlaneLookup()
        {
            InitializeComponent();
        }

        //基地航空隊に配置可能な装備種別一覧
        private int[] dispatchable = new int[]
        {
            6,//艦戦
            7,//艦爆
            8,//艦攻
            9,//艦偵
            10,//水偵
            11,//水爆
            41,//大型飛行艇
            45,//水上戦闘機
            47,//陸上攻撃機
            48,//局地戦闘機
        };

        //艦種別のスロット数
        private int GetNumSlot(int equipType)
        {
            switch(equipType)
            {
                case 9://艦偵
                case 10://水偵
                case 41://大型飛行艇
                    return 4;
                default:
                    return 12;
            }
        }

        private void AirBaseCorps_PlaneLookup_Load(object sender, EventArgs e)
        {
            if (APIMaster.MstSlotitems == null || APIMaster.MstSlotitemEquiptypes == null) return;

            var equips = APIMaster.MstSlotitems.Values
                .Where(x => x.api_type != null && !x.IsEnemySlotitem && dispatchable.Any(k => k == x.EquipType));

            listView1.SuspendLayout();
            foreach(var dslot in equips)
            {
                var dtype = APIMaster.MstSlotitemEquiptypes.Where(x => x.api_id == dslot.EquipType).FirstOrDefault();
                if (dtype == null) continue;

                var item = new ListViewItem(dslot.api_id.ToString());
                item.SubItems.Add(dslot.api_name);
                item.SubItems.Add(dtype.api_name.ToString());
                item.SubItems.Add(dslot.api_distance.ToString());
                item.SubItems.Add(dslot.api_cost.ToString());

                var numslot = GetNumSlot(dslot.EquipType);
                var airsup_max = AirSupResult.SingleSlotitemAirSup(dslot, (double)numslot, 7);
                var airsup_min = AirSupResult.SingleSlotitemAirSup(dslot, (double)numslot, 0);
                item.SubItems.Add((dslot.api_cost * numslot).ToString());
                item.SubItems.Add(airsup_max.AirSupValueMax.ToString());
                item.SubItems.Add(airsup_min.AirSupValueMin.ToString());
                item.SubItems.Add(dslot.api_tyku.ToString());
                item.SubItems.Add(dslot.Geigeki.ToString());

                item.SubItems.Add(dslot.TaiBaku.ToString());
                item.SubItems.Add(dslot.api_houm.ToString());
                item.SubItems.Add(dslot.api_raig.ToString());
                item.SubItems.Add(dslot.api_baku.ToString());
                item.SubItems.Add(dslot.api_saku.ToString());

                item.SubItems.Add(dslot.api_houk.ToString());
                item.SubItems.Add(dslot.api_houg.ToString());
                item.SubItems.Add(dslot.api_tais.ToString());

                listView1.Items.Add(item);
            }

            var sorter = new Helper.ListViewItemComparer();
            sorter.ColumnModes = new Helper.ListViewItemComparer.ComparerMode[]
            {
                Helper.ListViewItemComparer.ComparerMode.Integer, Helper.ListViewItemComparer.ComparerMode.String, Helper.ListViewItemComparer.ComparerMode.String, 
                Helper.ListViewItemComparer.ComparerMode.Integer, Helper.ListViewItemComparer.ComparerMode.Integer, Helper.ListViewItemComparer.ComparerMode.Integer, 
                Helper.ListViewItemComparer.ComparerMode.Integer, Helper.ListViewItemComparer.ComparerMode.Integer, Helper.ListViewItemComparer.ComparerMode.Integer, 
                Helper.ListViewItemComparer.ComparerMode.Integer, Helper.ListViewItemComparer.ComparerMode.Integer, Helper.ListViewItemComparer.ComparerMode.Integer, 
                Helper.ListViewItemComparer.ComparerMode.Integer, Helper.ListViewItemComparer.ComparerMode.Integer, Helper.ListViewItemComparer.ComparerMode.Integer, 
                Helper.ListViewItemComparer.ComparerMode.Integer, Helper.ListViewItemComparer.ComparerMode.Integer, Helper.ListViewItemComparer.ComparerMode.Integer, 
            };
            listView1.ListViewItemSorter = sorter;

            listView1.ResumeLayout();
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var sorter = listView1.ListViewItemSorter as Helper.ListViewItemComparer;
            if(sorter != null)
            {
                listView1.SuspendLayout();
                sorter.Column = e.Column;
                listView1.Sort();
                listView1.ResumeLayout();
            }
        }
    }
}
