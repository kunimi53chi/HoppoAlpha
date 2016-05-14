using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace VisualFormTest.DockingWindows
{
    public class DockWindowTabCollection
    {
        public List<DockWindowTabPage> TabPages { get; private set; }
        public List<IMyDockingWindow> Pages { get; private set; }
        public ToolStripMenuItemHandler ToolStripHandler { get; set; }
        public TabUnitPageFactory UnitPageFactory { get; private set; }
        public Form1 MainScreen { get; private set; }
        public bool IsInit2Done { get; set; }

        //各ページのコントロールのアクセサ
        #region ユーザーコントロールのプロパティ
        //タブページ
        #region タブページプロパティ
        public UserControls.TabFleet Fleet
        {
            get
            {
                return TabPages[(int)TabPageType.Fleet].MyControl as UserControls.TabFleet;
            }
        }

        public UserControls.TabGeneral General
        {
            get
            {
                return TabPages[(int)TabPageType.General].MyControl as UserControls.TabGeneral;
            }
        }

        public UserControls.TabMaterial Material
        {
            get
            {
                return TabPages[(int)TabPageType.Material].MyControl as UserControls.TabMaterial;
            }
        }

        public UserControls.TabSenka Senka
        {
            get
            {
                return TabPages[(int)TabPageType.Senka].MyControl as UserControls.TabSenka;
            }
        }

        public UserControls.TabUnit Unit
        {
            get
            {
                return TabPages[(int)TabPageType.Unit].MyControl as UserControls.TabUnit;
            }
        }

        public UserControls.TabEquipSearch EquipSearch
        {
            get
            {
                return TabPages[(int)TabPageType.EquipSearch].MyControl as UserControls.TabEquipSearch;
            }
        }

        public UserControls.TabCounter Counter
        {
            get
            {
                return TabPages[(int)TabPageType.Counter].MyControl as UserControls.TabCounter;
            }
        }

        public UserControls.TabJson Json
        {
            get
            {
                return TabPages[(int)TabPageType.Json].MyControl as UserControls.TabJson;
            }
        }
        public UserControls.TabSystemLog SystemLog
        {
            get
            {
                return TabPages[(int)TabPageType.SystemLog].MyControl as UserControls.TabSystemLog;
            }
        }
        #endregion


        //非タブページ
        #region 非タブページ
        public UserControls.HoppoBrowser Browser
        {
            get
            {
                return (Pages[(int)NonTabPageType.Browser] as DockingWindows.DockWindowHoppoBrowser).hoppoBrowser1;
            }
        }

        public UserControls.SankWarning SankWarning
        {
            get
            {
                return (Pages[(int)NonTabPageType.SankWarning] as DockingWindows.DockWindowSankWarning).sankWarning1;
            }
        }

        public UserControls.BattleState BattleState
        {
            get
            {
                return (Pages[(int)NonTabPageType.BattleState] as DockingWindows.DockWindowBattleState).battleState1;
            }
        }

        public UserControls.TimerViewer TimerViewer
        {
            get
            {
                return (Pages[(int)NonTabPageType.TimerViewer] as DockingWindows.DockWindowTimerViewer).timerViewer1;
            }
        }

        public UserControls.ShipSlotitemNum ShipSlotitemNum
        {
            get
            {
                return (Pages[(int)NonTabPageType.SlotitemNum] as DockingWindows.DockWindowShipSlotitemNum).shipSlotitemNum1;
            }
        }

        public UserControls.BattleDetail BattleDetail
        {
            get
            {
                return (Pages[(int)NonTabPageType.BattleDetail] as DockingWindows.DockWindowBattleDetail).battleDetail1;
            }
        }

        public UserControls.BattleDetailSquare BattleDetailSquare
        {
            get
            {
                return (Pages[(int)NonTabPageType.BattleDetailSquare] as DockingWindows.DockWindowBattleDetailSquare).battleDetailSquare1;
            }
        }

        public UserControls.BattleDetailSquare BattleDetailSquare2
        {
            get
            {
                return (Pages[(int)NonTabPageType.BattleDetailSquare2] as DockingWindows.DockWindowBattleDetailSquare2).battleDetailSquare1;
            }
        }

        public UserControls.CompactScreen CompactScreen
        {
            get
            {
                return (Pages[(int)NonTabPageType.CompactScreen] as DockingWindows.DockWindowCompactScreen).compactScreen1;
            }
        }

        public UserControls.CompactScreenVertical CompactScreenVertical
        {
            get
            {
                return (Pages[(int)NonTabPageType.CompactScreenVertical] as DockingWindows.DockWindowCompactScreenVertical).compactScreenVertical1;
            }
        }

        public UserControls.DropAnalyzer DropAnalyzer
        {
            get
            {
                return (Pages[(int)NonTabPageType.DropAnalyzer] as DockingWindows.DockWindowDropAnalyzer).dropAnalyzer1;
            }
        }

        public UserControls.ToolBox ToolBox
        {
            get
            {
                return (Pages[(int)NonTabPageType.ToolBox] as DockingWindows.DockWindowToolBox).toolBox1;
            }
        }

        public UserControls.MapInfo MapInfo
        {
            get
            {
                return (Pages[(int)NonTabPageType.MapInfo] as DockingWindows.DockWindowMapInfo).mapInfo1;
            }
        }

        public UserControls.QuestViewer QuestViewer
        {
            get
            {
                return (Pages[(int)NonTabPageType.QuestViewer] as DockingWindows.DockWindowQuestViewer).questViewer1;
            }
        }

        public UserControls.TabFleetShort TabFleetShort
        {
            get
            {
                return (Pages[(int)NonTabPageType.SFleet] as DockingWindows.DockWindowShortTabFleet).tabFleetShort1;
            }
        }

        public UserControls.TabMaterialShort TabMaterialShort
        {
            get
            {
                return (Pages[(int)NonTabPageType.SMaterial] as DockingWindows.DockWindowShortTabMaterial).tabMaterialShort1;
            }
        }

        public UserControls.TabSenkaShort TabSenkaShort
        {
            get
            {
                return (Pages[(int)NonTabPageType.SSenka] as DockingWindows.DockWindowShortTabSenka).tabSenkaShort1;
            }
        }

        public UserControls.SortieReportViewer SotieReportViewer
        {
            get
            {
                return (Pages[(int)NonTabPageType.SortieReportViewer] as DockingWindows.DockWindowSortieReportViewer).sortieReportViewer1;
            }
        }

        public UserControls.RankingViewer RankingViewer
        {
            get
            {
                return (Pages[(int)NonTabPageType.RankingViewer] as DockingWindows.DockWindowRankingViewer).rankingViewer1;
            }
        }

        public UserControls.KCVDBLog KCVDBLog
        {
            get
            {
                return (Pages[(int)NonTabPageType.KCVDBLog] as DockingWindows.DockWindowKCVDBLog).kcvdbLog1;
            }
        }

        public UserControls.PresetDeckViewer PresetDeckViewer
        {
            get
            {
                return (Pages[(int)NonTabPageType.PresetDeck] as DockingWindows.DockWindowPresetDeckViewer).presetDeckViewer1;
            }
        }

        public UserControls.AirBaseCorps AirBaseCorps
        {
            get
            {
                return (Pages[(int)NonTabPageType.AirBaseCorps] as DockingWindows.DockWindowAirBaseCorps).airBaseCorps1; 
            }
        }
        #endregion
        #endregion

        //ユニットページのコレクション
        public List<DockWindowTabPage> UnitPages
        {
            get { return this.UnitPageFactory.UnitPages; }
        }

        //内部クラス
        #region 内部クラス
        public class ToolStripMenuItemHandler
        {
            //タブページ
            public ToolStripMenuItem TFleet { get; set; }
            public ToolStripMenuItem TGeneral { get; set; }
            public ToolStripMenuItem TMaterial { get; set; }
            public ToolStripMenuItem TSenka { get; set; }
            public ToolStripMenuItem TUnit { get; set; }
            public ToolStripMenuItem TEquipSearch { get; set; }
            public ToolStripMenuItem TCounter { get; set; }
            public ToolStripMenuItem TJson { get; set; }
            public ToolStripMenuItem TSystemLog { get; set; }

            //非タブページ
            public ToolStripMenuItem TBrowser { get; set; }
            public ToolStripMenuItem TSankWarning { get; set; }
            public ToolStripMenuItem TBattleState { get; set; }
            public ToolStripMenuItem TTimerViewer { get; set; }
            public ToolStripMenuItem TSlotitemNum { get; set; }
            public ToolStripMenuItem TBattleDetail { get; set; }
            public ToolStripMenuItem TBattleDetailSquare { get; set; }
            public ToolStripMenuItem TBattleDetailSquare2 { get; set; }
            public ToolStripMenuItem TCompactScreen { get; set; }
            public ToolStripMenuItem TCompactScreenVertical { get; set; }
            public ToolStripMenuItem TDropAnalyzer { get; set; }
            public ToolStripMenuItem TToolBox { get; set; }
            public ToolStripMenuItem TMapInfo { get; set; }
            public ToolStripMenuItem TQuestViewer { get; set; }
            public ToolStripMenuItem TSFleet { get; set; }
            public ToolStripMenuItem TSMaterial { get; set; }
            public ToolStripMenuItem TSSenka { get; set; }
            public ToolStripMenuItem TSortieReportViewer { get; set; }
            public ToolStripMenuItem TRankingViewer { get; set; }
            public ToolStripMenuItem TKCVDBLog { get; set; }
            public ToolStripMenuItem TPresetDeckViewer { get; set; }
            public ToolStripMenuItem TAirBaseCorps { get; set; }
        }

        public class TabUnitPageFactory
        {
            public List<DockWindowTabPage> UnitPages { get; private set; }
            
            private Form1 _parent;
            private DockWindowTabCollection _collection;

            public TabUnitPageFactory(Form1 parent, DockWindowTabCollection collection)
            {
                _parent = parent;
                _collection = collection;

                this.UnitPages = new List<DockWindowTabPage>();
            }

            //再利用ありでページを作る
            public DockWindowTabPage Create()
            {
                //再利用できるウィンドウがある場合
                foreach(var x in UnitPages)
                {
                    if(x.DockState == DockState.Hidden)
                    {
                        return x;
                    }
                }
                //新規に作る場合
                DockWindowTabPage page = new DockWindowTabPage(TabPageType.Unit, _parent, _collection);
                this.UnitPages.Add(page);
                return page;
            }

            public DockWindowTabPage Create(EventHandler dockStateChangedEventHandler)
            {
                var page = this.Create();
                page.DockStateChanged += new EventHandler(dockStateChangedEventHandler);
                return page;
            }

            //再利用なしでページを作る
            public DockWindowTabPage CreateNew(EventHandler dockStateChangedEventHandler)
            {
                DockWindowTabPage page = new DockWindowTabPage(TabPageType.Unit, _parent, _collection);
                page.DockStateChanged += new EventHandler(dockStateChangedEventHandler);
                this.UnitPages.Add(page);
                return page;
            }

            //解放する
            public void Clean()
            {
                List<DockWindowTabPage> cleanpage = new List<DockWindowTabPage>();
                foreach(var x in UnitPages)
                {
                    if(x.DockPanel == null)
                    {
                        cleanpage.Add(x);
                    }
                }
                foreach (var x in cleanpage) UnitPages.Remove(x);
                //クエリ番号の記録
                List<int> queryid = new List<int>();
                foreach(var x in UnitPages)
                {
                    var tabunit = x.MyControl as UserControls.TabUnit;
                    if(tabunit != null && tabunit.UsingQuery != null) queryid.Add(tabunit.UsingQuery.ID);
                }
                Config.TabKanmusuQueryNumber = queryid;
            }
        }
        #endregion

        public DockWindowTabCollection(Form1 parent)
        {
            TabPages = new List<DockWindowTabPage>();
            TabPageType[] types = (TabPageType[])Enum.GetValues(typeof(TabPageType));
            foreach (var x in types)
            {
                DockWindowTabPage form = new DockWindowTabPage(x, parent, this);
                TabPages.Add(form);
            }
            //PageのUnitの部分をコピー
            UnitPageFactory = new TabUnitPageFactory(parent, this);
            UnitPageFactory.UnitPages.Add(TabPages[(int)TabPageType.Unit]);
            //親スクリーンの設定
            this.MainScreen = parent;

            //非タブページ
            Pages = new List<IMyDockingWindow>();
            NonTabPageType[] ntype = (NonTabPageType[])Enum.GetValues(typeof(NonTabPageType));
            foreach(var x in ntype)
            {
                IMyDockingWindow dw = x.CreateWindowInstance(parent, this);
                Pages.Add(dw);
            }
        }

        #region Get系メソッド
        //コレクションの取得
        public static DockWindowTabCollection GetCollection(Control c)
        {
            DockWindowTabPage page = c.FindForm() as DockWindowTabPage;
            DockWindowShortTabFleet shorts = c.FindForm() as DockWindowShortTabFleet;
            if (page != null) return page.Collection;
            if (shorts != null) return shorts.Collection;
            throw new NullReferenceException();
        }

        //DockContentの取得
        public DockContent GetDockContent(TabPageType tabtype)
        {
            return TabPages[(int)tabtype];
        }
        public DockContent GetDockContent(NonTabPageType pagetype)
        {
            return Pages[(int)pagetype] as DockContent;
        }

        //DockContent　→　ToolStripMenuItem
        public ToolStripMenuItem GetToolStripMenuItemFromDockContent(DockContent content)
        {
            ToolStripMenuItem tool = null;
            if (content is DockingWindows.DockWindowTabPage)
            {
                switch ((content as DockingWindows.DockWindowTabPage).PageType)
                {
                    case DockingWindows.TabPageType.Fleet:
                        tool = this.ToolStripHandler.TFleet;
                        break;
                    case DockingWindows.TabPageType.General:
                        tool = this.ToolStripHandler.TGeneral;
                        break;
                    case DockingWindows.TabPageType.Material:
                        tool = this.ToolStripHandler.TMaterial;
                        break;
                    case DockingWindows.TabPageType.Senka:
                        tool = this.ToolStripHandler.TSenka;
                        break;
                    case DockingWindows.TabPageType.Unit:
                        tool = this.ToolStripHandler.TUnit;
                        break;
                    case DockingWindows.TabPageType.EquipSearch:
                        tool = this.ToolStripHandler.TEquipSearch;
                        break;
                    case DockingWindows.TabPageType.Counter:
                        tool = this.ToolStripHandler.TCounter;
                        break;
                    case DockingWindows.TabPageType.Json:
                        tool = this.ToolStripHandler.TJson;
                        break;
                    case DockingWindows.TabPageType.SystemLog:
                        tool = this.ToolStripHandler.TSystemLog;
                        break;
                }
            }
            else if (content is DockingWindows.DockWindowHoppoBrowser) tool = this.ToolStripHandler.TBrowser;
            else if (content is DockingWindows.DockWindowSankWarning) tool = this.ToolStripHandler.TSankWarning;
            else if (content is DockingWindows.DockWindowBattleState) tool = this.ToolStripHandler.TBattleState;
            else if (content is DockingWindows.DockWindowTimerViewer) tool = this.ToolStripHandler.TTimerViewer;
            else if (content is DockingWindows.DockWindowShipSlotitemNum) tool = this.ToolStripHandler.TSlotitemNum;
            else if (content is DockingWindows.DockWindowBattleDetail) tool = this.ToolStripHandler.TBattleDetail;
            else if (content is DockingWindows.DockWindowBattleDetailSquare) tool = this.ToolStripHandler.TBattleDetailSquare;
            else if (content is DockingWindows.DockWindowBattleDetailSquare2) tool = this.ToolStripHandler.TBattleDetailSquare2;
            else if (content is DockingWindows.DockWindowCompactScreen) tool = this.ToolStripHandler.TCompactScreen;
            else if (content is DockingWindows.DockWindowCompactScreenVertical) tool = this.ToolStripHandler.TCompactScreenVertical;
            else if (content is DockingWindows.DockWindowDropAnalyzer) tool = this.ToolStripHandler.TDropAnalyzer;
            else if (content is DockingWindows.DockWindowToolBox) tool = this.ToolStripHandler.TToolBox;
            else if (content is DockingWindows.DockWindowMapInfo) tool = this.ToolStripHandler.TMapInfo;
            else if (content is DockingWindows.DockWindowQuestViewer) tool = this.ToolStripHandler.TQuestViewer;
            else if (content is DockingWindows.DockWindowShortTabFleet) tool = this.ToolStripHandler.TSFleet;
            else if (content is DockingWindows.DockWindowShortTabMaterial) tool = this.ToolStripHandler.TSMaterial;
            else if (content is DockingWindows.DockWindowShortTabSenka) tool = this.ToolStripHandler.TSSenka;
            else if (content is DockingWindows.DockWindowSortieReportViewer) tool = this.ToolStripHandler.TSortieReportViewer;
            else if (content is DockingWindows.DockWindowRankingViewer) tool = this.ToolStripHandler.TRankingViewer;
            else if (content is DockingWindows.DockWindowKCVDBLog) tool = this.ToolStripHandler.TKCVDBLog;
            else if (content is DockingWindows.DockWindowPresetDeckViewer) tool = this.ToolStripHandler.TPresetDeckViewer;
            else if (content is DockingWindows.DockWindowAirBaseCorps) tool = this.ToolStripHandler.TAirBaseCorps;

            if (tool == null) throw new NullReferenceException();
            else return tool;
        }

        //ToolStripMenuItem → DockContent
        public DockContent GetDockContentFromToolStipMenuItem(ToolStripMenuItem item)
        {
            if (item == this.ToolStripHandler.TFleet) return GetDockContent(TabPageType.Fleet);
            if (item == this.ToolStripHandler.TGeneral) return GetDockContent(TabPageType.General);
            if (item == this.ToolStripHandler.TMaterial) return GetDockContent(TabPageType.Material);
            if (item == this.ToolStripHandler.TSenka) return GetDockContent(TabPageType.Senka);
            if (item == this.ToolStripHandler.TUnit) return GetDockContent(TabPageType.Unit);
            if (item == this.ToolStripHandler.TEquipSearch) return GetDockContent(TabPageType.EquipSearch);
            if (item == this.ToolStripHandler.TCounter) return GetDockContent(TabPageType.Counter);
            if (item == this.ToolStripHandler.TJson) return GetDockContent(TabPageType.Json);
            if (item == this.ToolStripHandler.TSystemLog) return GetDockContent(TabPageType.SystemLog);

            if (item == this.ToolStripHandler.TBrowser) return GetDockContent(NonTabPageType.Browser);
            if (item == this.ToolStripHandler.TSankWarning) return GetDockContent(NonTabPageType.SankWarning);
            if (item == this.ToolStripHandler.TBattleState) return GetDockContent(NonTabPageType.BattleState);
            if (item == this.ToolStripHandler.TTimerViewer) return GetDockContent(NonTabPageType.TimerViewer);
            if (item == this.ToolStripHandler.TSlotitemNum) return GetDockContent(NonTabPageType.SlotitemNum);
            if (item == this.ToolStripHandler.TBattleDetail) return GetDockContent(NonTabPageType.BattleDetail);
            if (item == this.ToolStripHandler.TBattleDetailSquare) return GetDockContent(NonTabPageType.BattleDetailSquare);
            if (item == this.ToolStripHandler.TBattleDetailSquare2) return GetDockContent(NonTabPageType.BattleDetailSquare2);
            if (item == this.ToolStripHandler.TCompactScreen) return GetDockContent(NonTabPageType.CompactScreen);
            if (item == this.ToolStripHandler.TCompactScreenVertical) return GetDockContent(NonTabPageType.CompactScreenVertical);
            if (item == this.ToolStripHandler.TDropAnalyzer) return GetDockContent(NonTabPageType.DropAnalyzer);
            if (item == this.ToolStripHandler.TToolBox) return GetDockContent(NonTabPageType.ToolBox);
            if (item == this.ToolStripHandler.TMapInfo) return GetDockContent(NonTabPageType.MapInfo);
            if (item == this.ToolStripHandler.TQuestViewer) return GetDockContent(NonTabPageType.QuestViewer);
            if (item == this.ToolStripHandler.TSFleet) return GetDockContent(NonTabPageType.SFleet);
            if (item == this.ToolStripHandler.TSMaterial) return GetDockContent(NonTabPageType.SMaterial);
            if (item == this.ToolStripHandler.TSSenka) return GetDockContent(NonTabPageType.SSenka);
            if (item == this.ToolStripHandler.TSortieReportViewer) return GetDockContent(NonTabPageType.SortieReportViewer);
            if (item == this.ToolStripHandler.TRankingViewer) return GetDockContent(NonTabPageType.RankingViewer);
            if (item == this.ToolStripHandler.TKCVDBLog) return GetDockContent(NonTabPageType.KCVDBLog);
            if (item == this.ToolStripHandler.TPresetDeckViewer) return GetDockContent(NonTabPageType.PresetDeck);
            if (item == this.ToolStripHandler.TAirBaseCorps) return GetDockContent(NonTabPageType.AirBaseCorps);

            throw new NullReferenceException();
        }
        #endregion


        //Init2（JSONに依存するものの初期化を一斉に叩く）
        public void Init2All()
        {
            if (IsInit2Done) return;
            foreach(DockWindowTabPage page in TabPages)
            {
                page.MyControl.Init2();
            }
            foreach(DockWindowTabPage page in UnitPages)
            {
                if (!page.MyControl.Init2Finished) page.MyControl.Init2();
            }
            IsInit2Done = true;
        }

        //全て表示
        public void ShowAll(DockPanel dockPanel, DockState dockState)
        {
            foreach (DockWindowTabPage x in TabPages) x.Show(dockPanel, dockState);
            Activate(dockPanel, TabPageType.Fleet);
        }

        //フォーカスの変更
        public void Activate(DockPanel dockPanel, TabPageType tabtype)
        {
            foreach (IDockContent x in dockPanel.Contents)
            {
                if (x.DockHandler.Form == TabPages[(int)tabtype])
                {
                    x.DockHandler.Activate();
                    break;
                }
            }
        }

        //ユニットページのクエリ名を更新
        public void UnitPage_RefreshAllQueryName()
        {
            foreach(DockWindowTabPage x in UnitPages)
            {
                UserControls.TabUnit tab = x.MyControl as UserControls.TabUnit;
                tab.StripNameRefresh();
            }
        }

        //ユニットページの自動更新
        public void UnitPage_AllAutoRefreshCheck_Q()
        {
            foreach (DockWindowTabPage x in UnitPages)
            {
                UserControls.TabUnit tab = x.MyControl as UserControls.TabUnit;
                if (tab.AutoRefresh) tab.TabUnit_ListViewUpdate_Q();
            }
        }

        public void UnitPage_AllReadInitialQuery()
        {
            foreach(int i in Enumerable.Range(0, UnitPages.Count))
            {
                UserControls.TabUnit tab = UnitPages[i].MyControl as UserControls.TabUnit;
                int queryid;
                if (i < Config.TabKanmusuQueryNumber.Count) queryid = Config.TabKanmusuQueryNumber[i];
                else queryid = 0;
                tab.ReadQueryByNumber(queryid);
            }
        }

        //戦闘詳細の表示保持切り替え
        public void BattleDetail_ViewKeepingChangeCallBack()
        {
            BattleDetail.ViewKeepingChange(false);
            BattleDetailSquare.ViewKeepingChange(false);
            BattleDetailSquare2.ViewKeepingChange(false);
        }
    }
}
