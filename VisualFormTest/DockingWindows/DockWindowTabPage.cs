using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace VisualFormTest.DockingWindows
{
    public partial class DockWindowTabPage : DockContent, IMyDockingWindow
    {
        public UserControls.ITabControl MyControl { get; private set; }
        public Form1 MainScreen { get; private set; }
        public DockWindowTabCollection Collection { get; private set; }
        public TabPageType PageType { get; private set; }

        public DockWindowTabPage(TabPageType tabtype, Form1 parent, DockWindowTabCollection parenttab)
        {
            InitializeComponent();

            //タイトルバー
            this.Text = tabtype.TitleName();
            this.Icon = tabtype.GetIcon();
            this.PageType = tabtype;

            this.HideOnClose = true;

            //コントロール
            this.MainScreen = parent;
            MyControl = tabtype.CreateInstance();
            //MyControl.Init();
            UserControl myctrl = MyControl as UserControl;
            myctrl.Location = new Point(0, 0);
            myctrl.Padding = new Padding(0);
            //myctrl.Dock = DockStyle.Fill;
            this.ClientSize = myctrl.Size;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Controls.Add(myctrl);
            this.Collection = parenttab;
        }

        protected override string GetPersistString()
        {
            List<string> elements = new List<string>();
            elements.Add("VisualFormTest");
            elements.Add("DockWindowTabPage");
            elements.Add(this.PageType.ToString());
            //Unitのタブのみ
            if(this.PageType == TabPageType.Unit)
            {
                elements.Add(this.Collection.UnitPages.IndexOf(this).ToString());
            }
            return string.Join(".", elements);
        }

        public int RealHeight
        {
            get
            {
                return Config.ToolWindowTabHeight + (MyControl as UserControl).ClientSize.Height;
            }
        }
        public int RealWidth
        {
            get
            {
                return (MyControl as UserControl).ClientSize.Width;
            }
        }
        public void Stretch()
        {
            this.ClientSize = (MyControl as UserControl).Size;
        }

    }

    //タブの内容
    public enum TabPageType
    {
        Fleet, General, Material, Senka, Unit,
        EquipSearch, Counter, Json, SystemLog,
    }

    public static class TabPageTypeExt
    {
        //タイトルバーの名前
        public static string TitleName(this TabPageType tabtype)
        {
            switch(tabtype)
            {
                case TabPageType.Fleet: return "艦隊";
                case TabPageType.General: return "一般";
                case TabPageType.Material: return "資材";
                case TabPageType.Senka: return "戦果";
                case TabPageType.Unit: return "艦娘";
                case TabPageType.EquipSearch: return "装備検索";
                case TabPageType.Counter: return "カウンター";
                case TabPageType.Json: return "JSON";
                case TabPageType.SystemLog: return "システムログ";
                default: throw new NotImplementedException();
            }
        }

        //アイコン
        public static System.Drawing.Icon GetIcon(this TabPageType tabtype)
        {
            switch(tabtype)
            {
                case TabPageType.Fleet: return Properties.Resources.speech_balloon_orange_f;
                case TabPageType.General: return Properties.Resources.speech_balloon_orange_g;
                case TabPageType.Material: return Properties.Resources.speech_balloon_orange_m;
                case TabPageType.Senka: return Properties.Resources.speech_balloon_orange_a;
                case TabPageType.Unit: return Properties.Resources.speech_balloon_orange_u;
                case TabPageType.EquipSearch: return Properties.Resources.speech_balloon_orange_s;
                case TabPageType.Counter: return Properties.Resources.speech_balloon_orange_c;
                case TabPageType.Json: return Properties.Resources.speech_balloon_orange_j;
                case TabPageType.SystemLog: return Properties.Resources.speech_balloon_orange_l;
                default: throw new NotImplementedException();
            }
        }

        //インスタンス
        public static UserControls.ITabControl CreateInstance(this TabPageType tabtype)
        {
            switch(tabtype)
            {
                case TabPageType.Fleet: return new UserControls.TabFleet();
                case TabPageType.General: return new UserControls.TabGeneral();
                case TabPageType.Material: return new UserControls.TabMaterial();
                case TabPageType.Senka: return new UserControls.TabSenka();
                case TabPageType.Unit: return new UserControls.TabUnit();
                case TabPageType.EquipSearch: return new UserControls.TabEquipSearch();
                case TabPageType.Counter: return new UserControls.TabCounter();
                case TabPageType.Json: return new UserControls.TabJson();
                case TabPageType.SystemLog: return new UserControls.TabSystemLog();
                default: throw new NotImplementedException();
            }
        }
    }
}
