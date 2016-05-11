using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using HoppoAlpha.DataLibrary.RawApi.ApiReqQuest;
using WeifenLuo.WinFormsUI.Docking;

namespace VisualFormTest
{
    public partial class Form1 : Form
    {
        //タイマー
        System.Windows.Forms.Timer toolStripStatusLabel1Timer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer balloonTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer labelStatusTimer = new Timer();
        System.Windows.Forms.Timer labelStatusTimer2 = new Timer();

        //ドッキングパネル
        DockPanel dockPanel1;
        //ウィンドウ一覧
        public DockingWindows.DockWindowTabCollection dwPageCollection;
        //ドッキングの幅や高さ
        public int DocumentX { get; set; }
        public int DocumentY { get; set; }
        //ドッキングでパネルがあるか
        public bool DockContainsTop { get; set; }
        public bool DockContainsBottom { get; set; }
        public bool DockContainsLeft { get; set; }
        public bool DockContainsRight { get; set; }
        public bool DockContainsDocument { get; set; }
        public ToolStripDropDownButton ToolStrip
        {
            get { return this.toolStripDropDownButton2; }
        }

        //最後にAPIを取得したタイミング
        public DateTime LastAPIReceivedTime { get; set; }
        public string LastAPIReceivedPath { get; set; }
        //チェックマーク
        static string CheckString = Convert.ToChar(Convert.ToInt32("2713", 16)).ToString();
        static string LabelStatusDefaultString = "(:3[____]";

        public Form1()
        {
            InitializeComponent();
        }

        void FiddlerApplication_BeforeRequest(Fiddler.Session oSession)
        {
            //言語設定
            oSession.oRequest["Accept-Language"] = "jp";

            //艦これAPI以外は無視する
            var path = oSession.PathAndQuery;
            if(!path.StartsWith("/kcsapi/api_"))
            {
                oSession.Ignore();//Fiddlerでバッファリングをしないというだけでセッションを握りつぶりているのではない
                return;
            }

            //ローカルプロキシ
            if (Config.ProxyAddress != "") oSession["X-OverrideGateway"] = Config.ProxyAddress;//127.0.0.1:8080等の値が代入されます
        }

        static System.Threading.SemaphoreSlim _fiddler_semaphore = new System.Threading.SemaphoreSlim(1, 1);
        async void FiddlerApplication_AfterSessionComplete(Fiddler.Session oSession)
        {
            await _fiddler_semaphore.WaitAsync();
            try
            {
                //検証DB
                if (KancolleVerifyDb.KCVDBMain.IsCapture)
                {
                    await KancolleVerifyDb.KCVDBMain.PostServerAsync(oSession);
                }

                APIResolve(oSession);

                //統計DB
                if (KancolleDb.KancolleDbPost.IsCapture)
                {
                    KancolleDb.KancolleDbPost.FiddlerApplication_AfterSessionComplete(oSession);
                }
            }
            finally
            {
                _fiddler_semaphore.Release();
            }
        }

        //全角文字の変換
        #region デリゲート
        //MatchEvaluatorデリゲート
        private string JisCodeToStringDM(System.Text.RegularExpressions.Match m)
        {
            int char16 = Convert.ToInt32(m.Groups[1].Value, 16);//16進数→数値
            char c = Convert.ToChar(char16);//数値→文字
            return c.ToString();
        }
        #endregion

        //このフォーム関係
        #region このフォーム関係
        private async void Form1_Load(object sender, EventArgs e)
        {
            //UIタイマーの開始
            UIMethods.InitTimer(this);

            //位置の変更
            this.Location = Config.FormLocation;

            //UA変更
            //UserAgent.SetUserAgent();

            //クライアントへリクエストを送る前に呼ばれるイベント
            Fiddler.FiddlerApplication.BeforeRequest
                += new Fiddler.SessionStateHandler(FiddlerApplication_BeforeRequest);

            //クライアントへレスポンスを返した後に呼ばれるイベント
            Fiddler.FiddlerApplication.AfterSessionComplete
                        += new Fiddler.SessionStateHandler(FiddlerApplication_AfterSessionComplete);

            //Fiddlerを開始。ポートは自動選択
            Fiddler.FiddlerApplication.Startup(Config.PortNumber, Fiddler.FiddlerCoreStartupFlags.ChainToUpstreamGateway);

            //当該プロセスのプロキシを設定する。WebBroweserコントロールはこの設定を参照
            Fiddler.URLMonInterop.SetProxyInProcess(string.Format("127.0.0.1:{0}",
                        Fiddler.FiddlerApplication.oProxy.ListenPort), "<local>");

            //---コントロールの追加
            ControlInit();
            //ブラウザのスタート
            if (!Config.ListeningMode)
            {
                MethodInvoker mi = delegate() 
                {
                    //dwPageCollection.Browser.extraWebBrowser1.Navigate(@"http://www.ugtop.com/spill.shtml");
                    dwPageCollection.Browser.extraWebBrowser1.Navigate(@"http://www.dmm.com/netgame/social/-/gadgets/=/app_id=854854/"); 
                };
                IntPtr browserhandle = dwPageCollection.Browser.Handle;//ハンドルを関連付ける

                await Task.Factory.StartNew(() =>
                    {
                        if(!dwPageCollection.Browser.IsHandleCreated || dwPageCollection.Browser.InvokeRequired)
                        {
                            dwPageCollection.Browser.Invoke(mi);
                        }
                        else
                        {
                            mi.Invoke();
                        }
                    });
            }

            //ホーム画面の初期化
            HomeButtonInit();

            //SEのサーバー起動
            Sounds.ConnectToServer();

            //タイマー
            toolStripStatusLabel1Timer.Tick += new EventHandler(toolStripStatusLabel1Timer_Tick);
            toolStripStatusLabel1Timer.Interval = 1000;
            toolStripStatusLabel1Timer.Start();
            balloonTimer.Tick += new EventHandler(balloonTimer_Tick);
            balloonTimer.Interval = 2000;
            balloonTimer.Start();
            timerIdletime.Start();

            //検証DBに送信有無のダイアログ
            if(KancolleVerifyDb.KCVDBMain.IsNotifyDialogShow)
            {
                var verifyNotify = new KancolleVerifyDb.NotifyDialog(dwPageCollection);
                verifyNotify.ShowDialog(this);
            }

            //エイプリルフールジャック
            if(AprilFool.IsAprilFool)
            {
                this.Text = "かもかもアルファ";
                AprilFool.DoAprilSound();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon1.Dispose();
            //サーバーの終了
            Sounds.DisconnectToServer();
            //プロキシ設定を外す
            Fiddler.URLMonInterop.ResetProxyInProcessToDefault();
            //Fiddlerを終了させる
            Fiddler.FiddlerApplication.Shutdown();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //ここにキャンセルの処理を入れる
            if (!Config.OnClosingNotifyDisable)
            {
                if (MessageBox.Show("ほっぽアルファを終了しますか？", "終了確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != System.Windows.Forms.DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            //レイアウトの保存
            foreach (var x in dwPageCollection.UnitPages)
            {
                if (x.DockState == DockState.Hidden) x.DockPanel = null;//Hiddenの艦娘タブは解放する
            }
            foreach (var y in dwPageCollection.Pages)
            {
                var x = y as DockContent;
                if (x.DockState == DockState.Hidden) x.DockPanel = null;//Hiddenの艦娘タブは解放する
            }
            foreach (var x in dwPageCollection.TabPages)
            {
                if (x.DockState == DockState.Hidden) x.DockPanel = null;//Hiddenの艦娘タブは解放する
            }
            dwPageCollection.UnitPageFactory.Clean();
            if (!Directory.Exists(Environment.CurrentDirectory + @"/config")) Directory.CreateDirectory(Environment.CurrentDirectory + @"/config");
            dockPanel1.SaveAsXml(@"config/layout.xml");

            //Config類の保存
            Config.FormLocation = this.Location;
            LogSystem.AddLogMessage("ログを保存します");
            DataLibraryCenter.BackupSerials(true);
            LogSystem.SaveSystemLog();
        }


        //サイズ変更イベント
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            /*if (dwTabPages != null) dwTabPages[0].Width = Math.Max(0, this.Width - 22);
            //ユニット数を表示するか
            bool old_flag = Config.StatusStripShowShipNum;
            bool new_flag = this.Height <= Config.StatusStripShowShipNumThreshold;
            if (old_flag != new_flag)
            {
                Config.StatusStripShowShipNum = new_flag;
                dwShipSlotitemNum.shipSlotitemNum1.SetValue();
            }*/
            //Config側にも記録
            Config.FormSize = this.Size;
            Config.FormLocation = this.Location;
            //dockPanel1をDockFillをNoneにしたのでその関係
            if (dockPanel1 != null)
            {
                dockPanel1.Size = new Size(Math.Max(this.ClientSize.Width, 0), Math.Max(this.ClientSize.Height - statusStrip1.Height, 0));
                //DockPanelの区切り
                int x = dockPanel1.ClientRectangle.Width;
                int y = dockPanel1.ClientRectangle.Height;
                //どのドッキングが固定されているか
                bool leftmoveflag = dockPanel1.DockLeftPortion <= 1;
                bool rightmoveflag = dockPanel1.DockRightPortion <= 1;
                bool topmoveflag = dockPanel1.DockTopPortion <= 1;
                bool bottommoveflag = dockPanel1.DockBottomPortion <= 1;
                //ブラウザがDockmentかどうか
                if (dwPageCollection.GetDockContent(DockingWindows.NonTabPageType.Browser).DockState != DockState.Document)
                {
                    //ブラウザがDocumentでない場合の横処理
                    if (leftmoveflag && !rightmoveflag)
                    {
                        dockPanel1.DockLeftPortion = Math.Max((x - 10 - dockPanel1.DockRightPortion) / x, 0.001);
                    }
                    if (!leftmoveflag && rightmoveflag)
                    {
                        dockPanel1.DockRightPortion = Math.Max((x - 10 - dockPanel1.DockLeftPortion) / x, 0.001);
                    }
                    if (leftmoveflag && rightmoveflag)
                    {
                        dockPanel1.DockLeftPortion = Math.Max((double)(x - 10) / 2 / x, 0.001);
                        dockPanel1.DockRightPortion = Math.Max((double)(x - 10) / 2 / x, 0.001);
                    }
                    //ブラウザがDocumentでない場合の縦処理
                    if (topmoveflag && !bottommoveflag)
                    {
                        dockPanel1.DockTopPortion = Math.Max((y - 10 - dockPanel1.DockBottomPortion) / y, 0.001);
                    }
                    if (!topmoveflag && bottommoveflag)
                    {
                        dockPanel1.DockBottomPortion = Math.Max((y - 10 - dockPanel1.DockTopPortion) / y, 0.001);
                    }
                    if (topmoveflag && bottommoveflag)
                    {
                        dockPanel1.DockTopPortion = Math.Max((double)(y - 10) / y, 0.001);
                        dockPanel1.DockBottomPortion = Math.Max((double)(y - 10) / 2 / y, 0.001);
                    }
                    DockParameterAutoSet(false);
                    return;
                }
                //ドッキングが格納されていないリストを作成
                List<DockStateCalculator> calc = new List<DockStateCalculator>();
                calc.Add(new DockStateCalculator() { State = DockState.Document, Destination = DockStateCalculatorDestination.Vertical });
                calc.Add(new DockStateCalculator() { State = DockState.Document, Destination = DockStateCalculatorDestination.Horizonal });
                if (!DockContainsTop) calc.Add(new DockStateCalculator() { State = DockState.DockTop, Destination = DockStateCalculatorDestination.Vertical });
                if (!DockContainsBottom) calc.Add(new DockStateCalculator() { State = DockState.DockBottom, Destination = DockStateCalculatorDestination.Vertical });
                if (!DockContainsLeft) calc.Add(new DockStateCalculator() { State = DockState.DockLeft, Destination = DockStateCalculatorDestination.Horizonal });
                if (!DockContainsRight) calc.Add(new DockStateCalculator() { State = DockState.DockRight, Destination = DockStateCalculatorDestination.Horizonal });
                //縦方向、横方向に分類
                var fixed_vertical = from c in calc
                                     where c.Destination == DockStateCalculatorDestination.Vertical
                                     select c;
                var fixed_horizonal = from c in calc
                                      where c.Destination == DockStateCalculatorDestination.Horizonal
                                      select c;
                //--横方向の処理
                //Dxだけを固定する場合
                if (fixed_horizonal.Count() == 1)
                {
                    if (leftmoveflag && !rightmoveflag)
                    {
                        dockPanel1.DockLeftPortion = Math.Max((x - DocumentX - dockPanel1.DockRightPortion) / x, 0.001);
                    }
                    if (!leftmoveflag && rightmoveflag)
                    {
                        dockPanel1.DockRightPortion = Math.Max((x - DocumentX - dockPanel1.DockLeftPortion) / x, 0.001);
                    }
                    if (leftmoveflag && rightmoveflag)
                    {
                        dockPanel1.DockLeftPortion = Math.Max((double)(x - DocumentX) / 2 / x, 0.001);
                        dockPanel1.DockRightPortion = Math.Max((double)(x - DocumentX) / 2 / x, 0.001);
                    }
                }
                //Dx＋何かを固定する場合
                else if (fixed_horizonal.Count() == 2)
                {
                    if (leftmoveflag)
                    {
                        dockPanel1.DockLeftPortion = Math.Max((double)(x - DocumentX) / x, 0.001);
                    }
                    if (rightmoveflag)
                    {
                        dockPanel1.DockRightPortion = Math.Max((double)(x - DocumentX) / x, 0.001);
                    }
                }
                //--縦方向の固定
                if (fixed_vertical.Count() == 1)
                {
                    if (topmoveflag && !bottommoveflag)
                    {
                        dockPanel1.DockTopPortion = Math.Max((y - DocumentY - dockPanel1.DockBottomPortion) / y, 0.001);
                    }
                    if (!topmoveflag && bottommoveflag)
                    {
                        dockPanel1.DockBottomPortion = Math.Max((y - DocumentY - dockPanel1.DockTopPortion) / y, 0.001);
                    }
                    if (topmoveflag && bottommoveflag)
                    {
                        dockPanel1.DockTopPortion = Math.Max((double)(y - DocumentY) / y, 0.001);
                        dockPanel1.DockBottomPortion = Math.Max((double)(y - DocumentY) / 2 / y, 0.001);
                    }
                }
                else if (fixed_vertical.Count() == 2)
                {
                    if (topmoveflag)
                    {
                        dockPanel1.DockTopPortion = Math.Max((double)(y - DocumentY) / y, 0.001);
                    }
                    if (bottommoveflag)
                    {
                        dockPanel1.DockBottomPortion = Math.Max((double)(y - DocumentY) / y, 0.001);
                    }
                }
                //オートセット
                DockParameterAutoSet(false);
            }
        }

        //ドッキングの固定パラメータの再設定
        public void DockParameterAutoSet(bool recall)
        {
            //DockmentのX,Yの設定
            int x = 0; int y = 0;
            var visibleleft = dockPanel1.DockWindows.Where(k => k.DockState == DockState.DockLeft).First()
                                .VisibleNestedPanes.Select(k => k.Contents.First())
                                .Where(k => k.DockHandler.Content is DockingWindows.IMyDockingWindow && k.DockHandler.DockState == DockState.DockLeft)
                                .Select(k => k.DockHandler.Content).Cast<DockingWindows.IMyDockingWindow>();
            foreach(var c in visibleleft)
            {
                Size size = c.GetIMyDockWindowSizeWithTabs();
                x += size.Width;
                y += size.Height;
            }

            int last_docx = DocumentX;
            int last_docy = DocumentY;
            var dwHoppoBrowser = dwPageCollection.GetDockContent(DockingWindows.NonTabPageType.Browser) as DockingWindows.DockWindowHoppoBrowser;
            DocumentX = Math.Max(dwHoppoBrowser.RealWidth, x);
            DocumentY = Math.Max(dwHoppoBrowser.RealHeight, y);
            //ネストドッキングのPortionの設定
            //if(last_docy != DocumentY)
            //{
            //Left, Rightに格納されているContent一覧
            /*DockContentHandler[] lefts = (from c in dockPanel1.Contents
                                            where c.DockHandler.DockState == DockState.DockLeft
                                            select c.DockHandler).ToArray();*/ //←ダメな例（ドッキングの順序が変更されたときに反映されない）

            DockContentHandler[] lefts = dockPanel1.DockWindows.Where(k => k.DockState == DockState.DockLeft).First()
                                            .VisibleNestedPanes.Select(k => k.Contents.First())
                                            .Where(k => k.DockHandler.Content is DockingWindows.IMyDockingWindow && k.DockHandler.DockState == DockState.DockLeft)
                                            .Select(k => k.DockHandler)
                                            .ToArray();//OKな例

            DockContentHandler[] rights = dockPanel1.DockWindows.Where(k => k.DockState == DockState.DockRight).First()
                                            .VisibleNestedPanes.Select(k => k.Contents.First())
                                            .Where(k => k.DockHandler.Content is DockingWindows.IMyDockingWindow && k.DockHandler.DockState == DockState.DockRight)
                                            .Select(k => k.DockHandler)
                                            .ToArray();

            DockContentHandler[][] contents = new DockContentHandler[][] { lefts, rights };

            //ネストPortionの再設定
            foreach(DockContentHandler[] c in contents)
            {
                if (c == null || c.Length == 0) continue;
                double[] portion = new double[c.Length + 1];

                int height;
                if (c == lefts) height = (from d in dockPanel1.DockWindows
                                            where d.DockState == DockState.DockLeft
                                            select d).First().DisplayingRectangle.Height;
                else height = (from d in dockPanel1.DockWindows
                                where d.DockState == DockState.DockRight
                                select d).First().DisplayingRectangle.Height;
                //タブがある場合プラス

                //余りピクセル
//                int sumall = c.Select(k => k.Content).Cast<DockingWindows.IMyDockingWindow>().

                int sumall = (from k in c
                                select (k.Content as DockingWindows.IMyDockingWindow).GetIMyDockWindowSizeWithTabs().Height).Sum();
                int remain = Math.Max(height - sumall, 0);//DocumentY + dockdiff

                //1-portionの再計算
                foreach (int i in Enumerable.Range(0, c.Length))
                {
                    int pixels = (from k in c
                                    where Array.IndexOf(c, k) >= i
                                    select (k.Content as DockingWindows.IMyDockingWindow).GetIMyDockWindowSizeWithTabs().Height).Sum();
                    portion[i + 1] =
                        Math.Max((double)((c[i].Content as DockingWindows.IMyDockingWindow).GetIMyDockWindowSizeWithTabs().Height) / (double)(pixels + remain), 0.001);
                }
                //再設定
                foreach(int i in Enumerable.Range(0, c.Length))
                {
                    c[i].Pane.SetNestedDockingProportion(1 - portion[i]);
                }
            }
            //}
            //FloatWindowの調整
            foreach(DockContent c in dockPanel1.Contents)
            {
                if(c.DockState == DockState.Float)
                {
                    DockingWindows.IMyDockingWindow mydock = c as DockingWindows.IMyDockingWindow;
                    mydock.Stretch();
                }
            }

            //エイプリルフール
            if(recall)
            {
                if (AprilFool.IsAprilFool) AprilFool.DoAprilFoolWindow(dockPanel1);
            }

            //再表示
            if(recall) Form1_SizeChanged(this, new EventArgs());
        }
        #endregion

        //API解析
        #region API解析
        private void APIResolve(Fiddler.Session oSession)
        {
            //取り敢えずログを吐く
            string res = string.Format("Session {0}({3}):HTTP {1} for {2} ({4})",
                    oSession.id, oSession.responseCode, oSession.fullUrl, oSession.oResponse.MIMEType, DateTime.Now.ToString());
            //セッションのBody
            //全角文字の修正
            string body = System.Text.RegularExpressions.Regex.Replace(
                oSession.GetResponseBodyAsString(), @"\\u([0-9a-f]{4})",
                new System.Text.RegularExpressions.MatchEvaluator(JisCodeToStringDM));
            var path = oSession.PathAndQuery;
            string reqb = oSession.GetRequestBodyAsString();

            if (Config.ShowJson)
            {
                UpdateJson(res, string.Format("【Request】{0}{1}{2}", reqb, Environment.NewLine, body));

                if (JsonLogger.IsLogging) JsonLogger.SaveJsonAsync(reqb, body, oSession);
            }

            //--重複APIのチェック（簡易版）
            //同一APIを3秒以内に受信したらスルー→port限定
            DateTime now = DateTime.Now;
            if (LastAPIReceivedPath == path && path.Contains("api_port") &&
                (now - LastAPIReceivedTime).TotalSeconds <= 3.0) return;            
            //----
            string json = body.Replace("svdata=", "");
            //失敗した場合
            if (!json.Contains("\"api_result_msg\":\"成功\"")) return;
            //直近に記録したAPIの記録
            LastAPIReceivedTime = now;
            LastAPIReceivedPath = path;
            //APIのパスの分割
            string[] paths = path.Split('/');
            string firstAddress = ""; string secondAddress = "";
            if (paths.Length < 3) return;
            firstAddress = paths[2];
            if (paths.Length >= 4) secondAddress = paths[3];
            //APIの処理
            switch(firstAddress)
            {
                //api_start2の場合
                case "api_start2":
                    APIMaster.ReadApiStart2(json);
                    ZoomEnableSwitch(true);
                    CallBacks.SetToolStripMenuItemEnabled(toolStripMenuItem25, statusStrip1, true);//コンバート
                    LogSystem.AddLogMessage("接続に成功しました");
                    return;
                //api_get_memberの場合
                case "api_get_member":
                    switch(secondAddress)
                    {
                        //api_get_member/reqiure_infoの場合（追加でまとめられた）
                        case "require_info":
                            APIGetMember.ReadReuireInfo(json);
                            
                            //--Basicにあった処理
                            //記録の初期化
                            //if (!HistoricalData.IsInited) HistoricalData.Init();
                            if (!APIReqQuest.IsInited)
                            {
                                APIReqQuest.Init();
                                //任務の更新
                                UpdateQuest();
                            }
                            if (!APIReqRanking.IsInited)
                            {
                                APIReqRanking.Init();
                                dwPageCollection.RankingViewer.Init();
                            }
                            
                            //--slotitemにあった処理→なし

                            //--unsetslotにあった処理
                            //艦娘数のアップデート : 2015/7のメンテでport→slotitem→unsetslotの順になったので変更
                            UpdateShipSlotitemNum();

                            //--kdockにあった処理→なし

                            //--useitemにあった処理→なし

                            //--furnitureにあった処理→なし
                            return;

                        //api_get_member/basicの場合
                        case "basic":
                            APIGetMember.ReadBasic(json);
                            //記録の初期化
                            if (!HistoricalData.IsInited) HistoricalData.Init();
                            if (!APIReqQuest.IsInited)
                            {
                                APIReqQuest.Init();
                                //任務の更新
                                UpdateQuest();
                            }
                            if (!APIReqRanking.IsInited)
                            {
                                APIReqRanking.Init();
                                dwPageCollection.RankingViewer.Init();
                            }
                            return;
                        //api_get_member/furnitureの場合
                        case "furniture":
                            APIGetMember.ReadFurniture(json);
                            return;
                        //api_get_member/slot_itemの場合
                        case "slot_item":
                            APIGetMember.ReadSlotItem(json);
                            return;
                        //api_get_member/useitemの場合
                        case "useitem":
                            APIGetMember.ReadUseitem(json);
                            return;
                        //api_get_member/kdockの場合
                        case "kdock":
                            APIGetMember.ReadKdock(json);
                            return;
                        //api_get_member/unsetslotの場合
                        case "unsetslot":
                            APIGetMember.ReadUnsetslot(json);
                            //艦娘数のアップデート : 2015/7のメンテでport→slotitem→unsetslotの順になったので変更
                            UpdateShipSlotitemNum();
                            return;
                        //api_get_member/shipdeck
                        case "ship_deck":
                            APIGetMember.ReadShipDeck(json);
                            //艦隊情報のアップデート
                            FleetInfoUpdate();
                            //艦娘数のアップデート
                            UpdateShipSlotitemNum();
                            //大破警告の更新
                            UpdateSankWarning(true);
                            //合計経験値の更新
                            if (KancolleInfoUnitList.unitListViewer != null && !KancolleInfoUnitList.unitListViewer.IsDisposed) KancolleInfoUnitList.unitListViewer.UpdateTotalExp(true);
                            return;
                        //api_get_member/deckの場合
                        case "deck":
                            APIGetMember.ReadDeck(json);
                            //艦隊情報のアップデート
                            FleetInfoUpdate();
                            //遠征情報のセット
                            UpdateMission();
                            //下の遠征情報のアップデート
                            toolStripStatusLabel2Update();
                            //艦娘数のアップデート
                            UpdateShipSlotitemNum();
                            return;
                        //api_get_member/ndockの場合
                        case "ndock":
                            APIGetMember.ReadNdock(json);
                            //入渠情報のアップデート
                            UpdateNdock();
                            //艦隊情報の更新
                            FleetInfoUpdate();
                            return;
                        //api_get_member/ship2の場合　→　2015/5/18のメンテで一部shit_deckに置き換えされた模様
                        case "ship2":
                            APIGetMember.ReadShip2(json);
                            //艦隊情報のアップデート
                            FleetInfoUpdate();
                            //艦娘数のアップデート
                            UpdateShipSlotitemNum();
                            //大破警告の更新
                            UpdateSankWarning(false);
                            return;
                        //api_get_member/ship3の場合
                        case "ship3":
                            APIGetMember.ReadShip3(json);
                            //艦隊情報のアップデート
                            FleetInfoUpdate();
                            //艦娘数のアップデート
                            UpdateShipSlotitemNum();
                            return;
                        //api_get_member/questlistの場合
                        case "questlist":
                            APIGetMember.ReadQuestlist(json);
                            //任務の更新
                            UpdateQuest();
                            return;
                        //api_get_member/practiceの場合
                        case "practice":
                            APIGetMember.ReadPractice(json);
                            //演習の更新
                            UpdatePractice();
                            return;
                        //api_get_member/materialの場合
                        case "material":
                            APIGetMember.ReadMaterial(json);
                            //資源表示の更新
                            UpdateMaterial();
                            return;
                        //api_get_member/mapinfoの場合
                        case "mapinfo":
                            APIGetMember.ReadMapInfo(json);
                            //マップ情報の更新
                            UpdateMapInfo();
                            return;
                        //api_get_member/preset_deckの場合
                        case "preset_deck":
                            APIGetMember.ReadPresetDeck(json);
                            dwPageCollection.PresetDeckViewer.CheckDuplicate();
                            return;
                    }
                    break;
                //api_portの場合
                case "api_port":
                    if (!dwPageCollection.IsInit2Done)
                    {
                        dwPageCollection.Init2All();
                    }
                    //出撃報告書の初期化
                    if (!SortieReportDataBase.IsInited)
                    {
                        SortieReportDataBase.Init();
                        dwPageCollection.SotieReportViewer.Init();
                    }
                    //api_port/portの読み込み（暗黙的に）
                    APIPort.ReadPort(json);

                    //basicからのお引越し
                    if (!HistoricalData.IsInited)
                    {
                        HistoricalData.Init();
                    }

                    //艦隊情報のアップデート
                    FleetInfoUpdate();
                    //遠征情報のセット
                    UpdateMission();
                    //下の遠征情報のアップデート
                    toolStripStatusLabel2Update();
                    //入渠情報のアップデート
                    UpdateNdock();
                    //左上の戦闘ビューの更新
                    UpdateBattleView();
                    //大破警告のリセット
                    UpdateSankWarning(false);
                    //艦娘数のアップデート
                    if(!APIPort.OnSortie) UpdateShipSlotitemNum();//帰投直後じゃない場合のみ
                    //資源表示の更新
                    //UpdateMaterial();
                    UpdateMaterialAwaitable().ContinueWith(_ =>
                    {
                        //資源の記録
                        HistoricalData.AddLogMaterial();
                        //資源グラフの更新
                        UpdateMaterialGraph();
                        //個人戦果の記録
                        HistoricalData.AddExperience();
                        //個人戦果のラベルの更新
                        UpdateSenka();
                        //個人戦果のグラフの更新
                        UpdateSenkaGraph();
                        //バルーンのセット
                        if (!NotifyBalloon.IsNdockInited) NotifyBalloon.InitNdock();
                        if (!NotifyBalloon.IsMissionInited) NotifyBalloon.InitMissions();
                        //クエリの読み込み
                        if (!dwPageCollection.Unit.IsQueryLoaded) dwPageCollection.UnitPage_AllReadInitialQuery();
                    });
                    return;
                //api_req_memberの場合
                case "api_req_member":
                    switch(secondAddress)
                    {
                        //api_req_member/get_practice_enemyinfoの場合
                        case "get_practice_enemyinfo":
                            APIReqMember.ReadGetPracticeEnemyInfo(json);
                            return;
                    }
                    break;
                //api_req_practiceの場合
                case "api_req_practice":
                    switch(secondAddress)
                    {
                        //midnight_battleの場合
                        case "midnight_battle":
                            APIReqPractice.ReadPacticeNightBattle(json);
                            //左上の戦闘ビューの更新
                            UpdateBattleView();
                            return;
                        //battleの場合
                        case "battle":
                            APIReqPractice.ReadPracticeBattle(json);
                            //左上の戦闘ビューの更新
                            UpdateBattleView();
                            return;
                        //battle_resultの場合
                        case "battle_result":
                            APIReqPractice.ReadPracticeBattleResult(json);
                            //左上の戦闘ビューの更新
                            UpdateBattleView();
                            //演習の更新
                            UpdatePractice();
                            //個人戦果のラベルの更新
                            UpdateSenka();
                            return;
                    }
                    break;
                //api_req_kousyouの場合
                case "api_req_kousyou":
                    switch(secondAddress)
                    {
                        //createitemの場合　装備開発
                        case "createitem":
                            APIReqKousyou.ReadCreateitem(json);
                            UpdateShipSlotitemNum();
                            //資源表示の更新
                            UpdateMaterial();
                            return;
                        //destroyitem2の場合　装備廃棄
                        case "destroyitem2":
                            APIReqKousyou.ReadDestroyItem2(reqb, json);
                            UpdateShipSlotitemNum();
                            //資源表示の更新
                            UpdateMaterial();
                            return;
                        //getshipの場合
                        case "getship":
                            APIReqKousyou.ReadGetShip(json);
                            UpdateShipSlotitemNum();
                            return;
                        //destroyshipの場合
                        case "destroyship":
                            APIReqKousyou.ReadDestroyship(reqb, json);
                            UpdateShipSlotitemNum();
                            FleetInfoUpdate();
                            //資源表示の更新
                            UpdateMaterial();
                            return;
                        //remodel_slotの場合
                        case "remodel_slot":
                            APIReqKousyou.ReadRemodelSlot(json);
                            UpdateShipSlotitemNum();
                            //資源表示の更新
                            UpdateMaterial();
                            return;
                    }
                    break;
                //api_req_kaisouの場合
                case "api_req_kaisou":
                    switch(secondAddress)
                    {
                        case "powerup":
                            APIReqKaisou.ReadPowerup(json, reqb);
                            UpdateShipSlotitemNum();
                            FleetInfoUpdate();
                            return;
                        case "lock":
                            APIReqKaisou.ReadLock(reqb, json);
                            return;
                        case "slot_exchange_index":
                            APIReqKaisou.ReadSlotExchangeIndex(reqb, json);
                            FleetInfoUpdate();
                            return;
                        case "remodeling":
                            APIReqKaisou.ReadRemodeling(reqb);
                            return;
                    }
                    break;
                //api_req_henseiの場合
                case "api_req_hensei":
                    switch(secondAddress)
                    {
                        case "change":
                            APIReqHensei.ReadChange(reqb);
                            FleetInfoUpdate();
                            return;
                        case "combined":
                            APIReqHensei.Combined(json);
                            FleetInfoUpdate();
                            return;
                        case "preset_select":
                            APIReqHensei.PresetSelect(reqb, json);
                            FleetInfoUpdate();
                            UpdateMission();//キラキラとかも表示されるように
                            dwPageCollection.PresetDeckViewer.CheckDuplicate();
                            return;
                        case "preset_delete":
                            APIReqHensei.PresetDelete(reqb);
                            dwPageCollection.PresetDeckViewer.CheckDuplicate();
                            return;
                        case "preset_register":
                            APIReqHensei.PresetRegister(json);
                            dwPageCollection.PresetDeckViewer.CheckDuplicate();
                            return;
                    }
                    break;
                //api_req_hokyuの場合
                case "api_req_hokyu":
                    switch(secondAddress)
                    {
                        case "charge":
                            APIReqHokyu.ReadCharge(json);
                            FleetInfoUpdate();
                            //資源表示の更新
                            UpdateMaterial();
                            return;
                    }
                    break;
                //api_req_nyukyoの場合
                case "api_req_nyukyo":
                    switch(secondAddress)
                    {
                        //startの場合
                        case "start":
                            APIReqNyukyo.ReadStart(reqb);
                            //資源表示の更新
                            UpdateMaterial();
                            FleetInfoUpdate();
                            return;
                        //speedchangeの場合
                        case "speedchange":
                            APIReqNyukyo.ReadSpeedchange(reqb);
                            FleetInfoUpdate();
                            //入渠情報のアップデート
                            UpdateNdock();
                            return;
                    }
                    break;
                //--戦闘関係
                //api_req_sortie 通常戦闘
                case "api_req_sortie":
                    switch(secondAddress)
                    {
                        //api_req_sortie/battle 昼戦
                        case "battle":
                            APIBattle.ReadSortieBattle(json);
                            //左上の戦闘ビューの更新
                            UpdateBattleView();
                            return;
                        //api_req_sortie/battleresult 戦闘結果
                        case "battleresult":
                            APIBattle.ReadBattleResult(json, dwPageCollection.Counter);
                            //艦隊情報のアップデート
                            FleetInfoUpdate();
                            //個人戦果のラベルの更新
                            UpdateSenka();
                            //左上の戦闘ビューの更新
                            UpdateBattleView();
                            //大破警告の更新
                            UpdateSankWarning(false);
                            return;
                        //api_req_sortie/airbattle　航空戦
                        case "airbattle":
                            APIBattle.ReadAirbattle(json);
                            //左上の戦闘ビューの更新
                            UpdateBattleView();
                            return;
                        //api_req_sortie/ld_airbattle　航空戦（地上基地）
                        case "ld_airbattle":
                            APIBattle.ReadLdAirBattle(json);
                            UpdateBattleView();
                            return;
                    }
                    break;
                //api_req_battle_midnight : 夜戦
                case "api_req_battle_midnight":
                    switch(secondAddress)
                    {
                        //api_req_battle_midnight/battle　追撃夜戦
                        case "battle":
                            //艦隊情報のアップデート
                            //await FleetInfoUpdateAwaitable();
                            FleetInfoUpdate();
                            APIBattle.ReadBattleMidnight(json);
                            //左上の戦闘ビューの更新
                            UpdateBattleView();
                            return;
                        //api_req_battle_midnight/sp_midnight 開幕夜戦
                        case "sp_midnight":
                            APIBattle.ReadSpMidnight(json);
                            //左上の戦闘ビューの更新
                            UpdateBattleView();
                            return;
                    }
                    break;
                //api_req_combined_battle : 連合艦隊
                case "api_req_combined_battle":
                    switch(secondAddress)
                    {
                        //api_req_combined_battle/battle_water 水上打撃昼戦
                        case "battle_water":
                            APIBattle.ReadCombinedBattleWater(json);
                            //左上の戦闘ビューの更新
                            UpdateBattleView();
                            return;
                        //api_req_combined_battle/battle 機動部隊昼
                        case "battle":
                            APIBattle.ReadCombinedBattle(json);
                            //左上の戦闘ビューの更新
                            UpdateBattleView();
                            return;
                        //api_req_combined_battle/airbattle 機動部隊航空戦
                        case "airbattle":
                            APIBattle.ReadCombinedAirbattle(json);
                            //左上の戦闘ビューの更新
                            UpdateBattleView();
                            return;
                        //api_req_combined_battle/ld_airbattle 陸上航空戦
                        case "ld_airbattle":
                            //goto case "airbattle";
                            APIBattle.ReadCombinedLdAirbattle(json);
                            UpdateBattleView();
                            return;
                        //api_req_combined_battle/midnight_battle 連合艦隊追撃夜戦
                        case "midnight_battle":
                            //艦隊情報のアップデート
                            //await FleetInfoUpdateAwaitable();
                            FleetInfoUpdate();
                            APIBattle.ReadCombinedMidnightBattle(json);
                            //左上の戦闘ビューの更新
                            UpdateBattleView();
                            return;
                        //api_req_combined_battle/sp_midnight : 連合艦隊開幕夜戦
                        case "sp_midnight":
                            //艦隊情報のアップデート
                            //await FleetInfoUpdateAwaitable();
                            FleetInfoUpdate();
                            APIBattle.ReadCombinedMidnightBattleSP(json);
                            //左上の戦闘ビューの更新
                            UpdateBattleView();
                            return;
                        //api_req_combined_battle/battleresult
                        case "battleresult":
                            APIBattle.ReadCombinedBattleResult(json, dwPageCollection.Counter);
                            //艦隊情報のアップデート
                            FleetInfoUpdate();
                            //個人戦果のラベルの更新
                            UpdateSenka();
                            //左上の戦闘ビューの更新
                            UpdateBattleView();
                            //大破警告の更新
                            UpdateSankWarning(false);
                            return;
                        //api_req_combined_battle/goback_port
                        case "goback_port":
                            APIBattle.ReadCombinedGobackPort();
                            //艦隊情報のアップデート
                            FleetInfoUpdate();
                            //大破警告の更新
                            UpdateSankWarning(false);
                            return;
                    }
                    break;
                //マップ関係
                case "api_req_map":
                    switch(secondAddress)
                    {
                        //select_eventmap_rank
                        case "select_eventmap_rank":
                            APIReqMap.ReadSelectEventMapRank(reqb);
                            //マップ情報の更新
                            UpdateMapInfo();
                            return;
                        //startのみ
                        case "start":
                            APIReqMap.ReadStart(reqb, json, dwPageCollection.Counter);
                            goto case "next";
                        //start or next
                        case "next":
                            APIReqMap.ReadNext(json);
                            //左上の戦闘ビューの更新
                            UpdateBattleView();
                            //大破警告の更新
                            UpdateSankWarning(true);
                            return;
                    }
                    break;
                //任務クリア
                case "api_req_quest":
                    switch(secondAddress)
                    {
                        case "clearitemget":
                            APIReqQuest.ReadClearitem(reqb);
                            //任務の更新
                            UpdateQuest();
                            return;
                    }
                    break;
                //api_req_mission
                case "api_req_mission":
                    switch(secondAddress)
                    {
                        //result 遠征クリア
                        case "result":
                            APIReqMission.ReadResult(json);
                            //個人戦果のラベルの更新
                            UpdateSenka();
                            return;
                        //start 遠征スタート
                        case "start":
                            APIReqMission.ReadStart(reqb, json);
                            return;
                        //return_instruction 遠征中止
                        case "return_instruction":
                            APIReqMission.ReadReturnInstruction(reqb, json);
                            //遠征情報のセット
                            UpdateMission();
                            //下の遠征情報のアップデート
                            toolStripStatusLabel2Update();
                            return;
                    }
                    break;
                //戦果ランキング
                case "api_req_ranking":
                    switch(secondAddress)
                    {
                        case "getlist":
                            APIReqRanking.ReadGetlist(json);
                            //個人戦果のラベルの更新
                            UpdateSenka();
                            return;
                    }
                    break;
            }
        }
        #endregion

        //タイマーイベント
        #region タイマーイベント
        //時刻表示のタイマーイベント
        private void toolStripStatusLabel1Timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            toolStripStatusLabel1.Text = now.ToString("yyyy年M月d日(ddd) HH:mm:ss");
        }

        //バルーンのタイマー
        private void balloonTimer_Tick(object sender, EventArgs e)
        {
            if(NotifyBalloon.BalloonData.Count == 0)
            {
                return;
            }
            var data_parent = NotifyBalloon.BalloonData.First();
            NotifyBalloon.BalloonItem data = data_parent.Value;
            //実行
            bool showflag = false;
            if(data.ExecuteTime <= DateTime.Now)
            {
                switch (data.Type)
                {
                    case NotifyBalloon.BalloonInfoType.Mission:
                        notifyIcon1.BalloonTipTitle = "遠征帰投";
                        if (!Config.NotShowNotifyBalloonMission) showflag = true;
                        if (!Config.SoundMissionDisableFlag && !Config.OyasumiMode) Sounds.PlaySounds(Config.SoundMissionFileName);//音
                        break;
                    case NotifyBalloon.BalloonInfoType.Ndock:
                        notifyIcon1.BalloonTipTitle = "入渠完了";
                        if (!Config.NotShowNotifyBalloonNdock) showflag = true;
                        if (!Config.SoundNdockDisableFlag && !Config.OyasumiMode) Sounds.PlaySounds(Config.SoundNdockFileName);//音
                        break;
                    default:
                        notifyIcon1.BalloonTipTitle = "";
                        break;
                }
                notifyIcon1.BalloonTipText = data.Message;
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                //データの削除
                NotifyBalloon.BalloonData.Remove(data_parent.Key);
                //おやすみモード
                if (Config.OyasumiMode) showflag = false;
                //バルーンの表示
                if(showflag) notifyIcon1.ShowBalloonTip(10000);
            }
        }

        //自動保存
        private async void timerIdletime_Tick(object sender, EventArgs e)
        {
            //AFKしてたら保存しない
            if ((DateTime.Now - LastAPIReceivedTime).TotalMilliseconds <= timerIdletime.Interval)
            {
                LogSystem.AddLogMessage("自動バックアップを開始します");
                await DataLibraryCenter.BackupSerialsAsync(false);
                LogSystem.AddLogMessage("自動バックアップを終了します");
            }
        }
        #endregion

        //ドッキングウィンドウのコントロール
        #region ドッキングウィンドウのコントロール
        //コントロールの初期化
        private void ControlInit()
        {
            //ドッキングウィンドウ
            dockPanel1 = new DockPanel()
            {
                Location = new Point(0,0),
                Size = new Size(this.ClientSize.Width, this.ClientSize.Height-statusStrip1.Height),
                Theme = vS2012LightTheme1,
                DocumentStyle = DocumentStyle.DockingWindow,
                ShowDocumentIcon = false,
                //Margin = new Padding(3, 3, 3, 100),
            };
            this.Controls.Add(dockPanel1);
            //インスタンスの初期化
            dwPageCollection = new DockingWindows.DockWindowTabCollection(this);
            //ToolStripのハンドラーの初期化
            dwPageCollection.ToolStripHandler = new DockingWindows.DockWindowTabCollection.ToolStripMenuItemHandler()
            {
                TFleet = toolStripMenuItem_wh_fleet,
                TGeneral = toolStripMenuItem_wh_general,
                TMaterial = toolStripMenuItem_wh_material,
                TSenka = toolStripMenuItem_wh_senka,
                TUnit = toolStripMenuItem_wh_unit,
                TEquipSearch = toolStripMenuItem_wh_equipsearch,
                TCounter = toolStripMenuItem_wh_counter,
                TJson = toolStripMenuItem_wh_json,
                TSystemLog = toolStripMenuItem_wh_systemlog,

                TSFleet = toolStripMenuItem_ws_sfleet,
                TSMaterial = toolStripMenuItem_ws_smaterial,
                TSSenka = toolStripMenuItem_ws_ssenka,

                TBrowser = toolStripMenuItem_wo_browser,
                TSankWarning = toolStripMenuItem_wo_sankwarning,
                TBattleState = toolStripMenuItem_wo_battlestate,
                TTimerViewer = toolStripMenuItem_wo_timerviewer,
                TSlotitemNum = toolStripMenuItem_wo_slotitemnum,
                TBattleDetail = toolStripMenuItem_wo_battledetail,
                TBattleDetailSquare = toolStripMenuItem_wo_battledetailsquare,
                TBattleDetailSquare2 = toolStripMenuItem_wo_battledetailsquare2,
                TCompactScreen = toolStripMenuItem_wo_compactscreen,
                TCompactScreenVertical = toolStripMenuItem_wo_compactscreenvertical,
                TToolBox = toolStripMenuItem_wo_toolbox,
                TMapInfo = toolStripMenuItem_wo_mapinfo,
                TQuestViewer = toolStripMenuItem_wo_questviewer,

                TDropAnalyzer = toolStripMenuItem_wa_dropanalyze,
                TSortieReportViewer = toolStripMenuItem_wa_sortiereport,
                TRankingViewer = toolStripMenuItem_wa_ranking,
                TPresetDeckViewer = toolStripMenuItem_wa_preset,

                TKCVDBLog = toolStripMenuItem_wv_kcvdb,
            };
            //ToolStrip←→DockStateChangedのハンドラー
            foreach (DockContent x in dwPageCollection.TabPages)
            {
                if (x.Text == "艦娘") continue;
                x.DockStateChanged += new EventHandler(IMyDockingWindow_DockStateChanged2);
            }
            foreach (var x in dwPageCollection.Pages)
            {
                var y = x as DockContent;
                y.DockStateChanged += new EventHandler(IMyDockingWindow_DockStateChanged2);
            }
            //ToolStipMenuItemのハンドラー
            toolStripMenuItem_wh_fleet.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wh_general.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wh_material.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wh_senka.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wh_equipsearch.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wh_counter.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wh_json.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wh_systemlog.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);

            toolStripMenuItem_wh_unit.Click += new EventHandler(toolStripMenuItem_Window_Multiple_Click);

            toolStripMenuItem_ws_sfleet.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_ws_smaterial.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_ws_ssenka.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);

            toolStripMenuItem_wo_browser.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wo_sankwarning.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wo_battlestate.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wo_timerviewer.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wo_slotitemnum.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wo_battledetail.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wo_battledetailsquare.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wo_battledetailsquare2.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wo_compactscreen.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wo_compactscreenvertical.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wo_toolbox.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wo_mapinfo.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wo_questviewer.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);

            toolStripMenuItem_wa_dropanalyze.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wa_sortiereport.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wa_ranking.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            toolStripMenuItem_wa_preset.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);

            toolStripMenuItem_wv_kcvdb.Click += new EventHandler(toolStripMenuItem_Window_Single_Click);
            //読み込み
            string userlayout_filename = @"config\layout.xml";
            if (File.Exists(userlayout_filename))
            {
                using (FileStream fs = new FileStream(userlayout_filename, FileMode.Open, FileAccess.Read))
                {
                    dockPanel1.LoadFromXml(userlayout_filename, GetDockContentFromPersistString);
                    foreach (var x in dockPanel1.Contents.OfType<DockingWindows.DockWindowTabPage>()) x.MyControl.Init();
                    CallBacks.SetToolStripMenuItemChecked(toolStripMenuItem27_99, statusStrip1, true);
                }
            }
            else
            {
                using (Stream stream = new MemoryStream(Encoding.Unicode.GetBytes(Properties.Resources.basic)))
                {
                    dockPanel1.LoadFromXml(stream, GetDockContentFromPersistString);
                    foreach (var x in dockPanel1.Contents.OfType<DockingWindows.DockWindowTabPage>()) x.MyControl.Init();
                    CallBacks.SetToolStripMenuItemChecked(toolStripMenuItem27_1, statusStrip1, true);
                }
            }
            //レイアウトロック
            if (Config.DockFixing) ToggleLockLayout(true);
            //DockStateChangedイベント
            foreach (DockContent x in dwPageCollection.TabPages)
            {
                x.DockStateChanged += new EventHandler(IMyDockingWindow_DockStateChanged);
            }
            foreach(var x in dwPageCollection.Pages)
            {
                var y = x as DockContent;
                y.DockStateChanged += new EventHandler(IMyDockingWindow_DockStateChanged);
            }
            //フォームサイズ
            this.Size = Config.FormSize;
            //自動調節
            IMyDockingWindow_DockStateChanged(null, new EventArgs());
            DockParameterAutoSet(true);
        }

        //レイアウトのロック
        private void ToggleLockLayout(bool flag)
        {
            dockPanel1.AllowEndUserDocking = !flag;
            CallBacks.SetToolStripMenuItemChecked(toolStripMenuItem20, statusStrip1, flag);
            CallBacks.SetToolStripMenuItemEnabled(toolStripMenuItem27, statusStrip1, !flag);
            CallBacks.SetToolStripMenuItemEnabled(toolStripMenuItem12, statusStrip1, !flag);
            Config.DockFixing = flag;

            DockAreas notfloat = (DockAreas)(DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom | DockAreas.Document);
            DockAreas flotable = (DockAreas)(DockAreas.Float | notfloat);

            foreach(DockContent x in dockPanel1.Contents)
            {
                if (flag && x.DockState != DockState.Float)
                {
                    x.DockAreas = notfloat;
                    x.CloseButtonVisible = false;
                }
                else
                {
                    x.DockAreas = flotable;
                    x.CloseButtonVisible = true;
                }
            }
        }

        //ドッキングを復元するデリゲート
        IDockContent GetDockContentFromPersistString( string persistString )
        {
            string[] elements = persistString.Split('.');
            
            if(elements[1] == "DockWindowTabPage")
            {
                DockingWindows.TabPageType pagetype = (DockingWindows.TabPageType)Enum.Parse(typeof(DockingWindows.TabPageType), elements[2]);
                //Unitの場合
                if(pagetype == DockingWindows.TabPageType.Unit)
                {
                    int unitpageid = int.Parse(elements[3]);
                    if(unitpageid == 0)
                    {
                        return dwPageCollection.TabPages[(int)pagetype];
                    }
                    else
                    {
                        while(dwPageCollection.UnitPages.Count < unitpageid + 1)
                        {
                            dwPageCollection.UnitPageFactory.CreateNew(IMyDockingWindow_DockStateChanged);
                        }
                        return dwPageCollection.UnitPages[unitpageid];
                    }
                }
                else
                {
                    return dwPageCollection.TabPages[(int)pagetype];
                }
            }
            else if(elements[1].StartsWith("DockWindow"))
            {
                return dwPageCollection.GetDockContent(DockingWindows.NonTabPageExt.GetEnumFromString(elements[1]));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        //レイアウトの切り替え
        public void ChangeLayout(string layoutname)
        {
            string readfile = null;
            //ユーザーの場合ファイルが存在するか
            if(layoutname == "user")
            {
                if (!File.Exists(@"config\layout.xml")) return;
            }
            //ファイルから読み込む場合
            else if(layoutname.StartsWith("file"))
            {
                readfile = layoutname.Split('\n')[1];
                if (!File.Exists(readfile)) return;
                if(Path.GetExtension(readfile) != ".xml")
                {
                    MessageBox.Show("読み込みファイルの形式が不正です", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            //コントロールの解放
            List<DockContent> content = new List<DockContent>();
            foreach(DockContent x in dockPanel1.Contents)
            {
                content.Add(x);
            }
            foreach(DockContent x in content)
            {
                x.DockPanel = null;
            }
            //チェックを外す
            CallBacks.SetToolStripMenuItemChecked(toolStripMenuItem27_1, statusStrip1, false);
            CallBacks.SetToolStripMenuItemChecked(toolStripMenuItem27_2, statusStrip1, false);
            CallBacks.SetToolStripMenuItemChecked(toolStripMenuItem27_3, statusStrip1, false);
            CallBacks.SetToolStripMenuItemChecked(toolStripMenuItem27_4, statusStrip1, false);
            CallBacks.SetToolStripMenuItemChecked(toolStripMenuItem27_99, statusStrip1, false);
            //レイアウトの書き換え
            string laydata = null;
            switch(layoutname)
            {
                case "basic":
                    laydata = Properties.Resources.basic;
                    CallBacks.SetToolStripMenuItemChecked(toolStripMenuItem27_1, statusStrip1, true);
                    break;
                case "row2":
                    laydata = Properties.Resources.row2;
                    CallBacks.SetToolStripMenuItemChecked(toolStripMenuItem27_2, statusStrip1, true);
                    break;
                case "kanbura":
                    laydata = Properties.Resources.kanbura;
                    CallBacks.SetToolStripMenuItemChecked(toolStripMenuItem27_3, statusStrip1, true);
                    break;
                case "compact":
                    laydata = Properties.Resources.compact;
                    CallBacks.SetToolStripMenuItemChecked(toolStripMenuItem27_4, statusStrip1, true);
                    break;
                case "user":
                    //ここだけ特別
                    dockPanel1.LoadFromXml(@"config\layout.xml", GetDockContentFromPersistString);
                    foreach (var x in dockPanel1.Contents.OfType<DockingWindows.DockWindowTabPage>())
                    {
                        if(!x.MyControl.InitFinished) x.MyControl.Init();
                        if (dwPageCollection.IsInit2Done && !x.MyControl.Init2Finished) x.MyControl.Init2();
                    }
                    CallBacks.SetToolStripMenuItemChecked(toolStripMenuItem27_99, statusStrip1, true);
                    DockParameterAutoSet(true);
                    return;
                case "file":
                    dockPanel1.LoadFromXml(readfile, GetDockContentFromPersistString);
                    DockParameterAutoSet(true);
                    return;
                default:
                    //ユーザーのファイルを読み込む場合
                    if(layoutname.StartsWith("file"))
                    {
                        goto case "file";
                    }
                    throw new ArgumentException();
            }
            using (Stream stream = new MemoryStream(Encoding.Unicode.GetBytes(laydata)))
            {
                dockPanel1.LoadFromXml(stream, GetDockContentFromPersistString);
            }
            foreach (var x in dockPanel1.Contents.OfType<DockingWindows.DockWindowTabPage>())
            {
                if (!x.MyControl.InitFinished) x.MyControl.Init();
                if (dwPageCollection.IsInit2Done && !x.MyControl.Init2Finished) x.MyControl.Init2();            }
            DockParameterAutoSet(true);
        }

        //DockStateChanged
        public void IMyDockingWindow_DockStateChanged(object sender, EventArgs e)
        {
            foreach(DockContent x in dockPanel1.Contents)
            {
                if(x.DockState == DockState.Float)
                {
                    if(x.FloatPane != null)
                    {
                        DockingWindows.IMyDockingWindow idock = x as DockingWindows.IMyDockingWindow;
                        x.FloatPane.FloatWindow.ClientSize = new Size(idock.RealWidth, idock.RealHeight - Config.ToolWindowTabHeight);
                    }
                }
            }
            //Dockの状態のセット
            DockContainsTop = false; DockContainsBottom = false; DockContainsLeft = false; DockContainsRight = false; DockContainsDocument = false;
            foreach (DockContent x in dockPanel1.Contents)
            {
                switch(x.DockState)
                {
                    case DockState.DockTop:
                        DockContainsTop = true;
                        break;
                    case DockState.DockBottom:
                        DockContainsBottom = true;
                        break;
                    case DockState.DockRight:
                        DockContainsRight = true;
                        break;
                    case DockState.DockLeft:
                        DockContainsLeft = true;
                        break;
                    case DockState.Document:
                        DockContainsDocument = true;
                        break;
                }
            }
            //
            DockParameterAutoSet(true);
        }

        //toolstripmenuitemの連携
        private void IMyDockingWindow_DockStateChanged2(object sender, EventArgs e)
        {
            //該当するToolStripMenuItemの選択
            DockContent con = sender as DockContent;
            ToolStripMenuItem tool = dwPageCollection.GetToolStripMenuItemFromDockContent(con);
        
            //選択されていた場合
            bool isHidden = con.DockState == DockState.Hidden || con.DockState == DockState.Unknown;
            if(tool.Checked && isHidden)
            {
                CallBacks.SetToolStripMenuItemChecked(tool, statusStrip1, false);
            }
            else if(!tool.Checked && !isHidden)
            {
                CallBacks.SetToolStripMenuItemChecked(tool, statusStrip1, true);
            }
        }

        //ToolStripMenuItem（ウィンドウの起動終了）
        private void toolStripMenuItem_Window_Single_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            DockContent con = dwPageCollection.GetDockContentFromToolStipMenuItem(item);

            bool action = !item.Checked;//true→表示、false→閉じる
            if (action)
            {
                if (con.DockPanel == null)
                {
                    con.Show(dockPanel1, DockState.Float);
                    var myControls = con.Controls.OfType<UserControl>().Where(x => x is UserControls.ITabControl).Cast<UserControls.ITabControl>();
                    if (myControls != null && myControls.Count() != 0)
                    {
                        var myControl = myControls.First();
                        if (!myControl.InitFinished) myControl.Init();
                        if (dwPageCollection.IsInit2Done && !myControl.Init2Finished) myControl.Init2();
                    }
                }
                else con.DockState = DockState.Float;
            }
            else con.DockState = DockState.Hidden;
            CallBacks.SetToolStripMenuItemChecked(item, statusStrip1, action);//こっちを後で読ませる
        }

        //ToolStripMenuItem（複数起動できるウィンドウ）
        private void toolStripMenuItem_Window_Multiple_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            var con = dwPageCollection.GetDockContentFromToolStipMenuItem(item) as DockingWindows.DockWindowTabPage;
            if (con.PageType != DockingWindows.TabPageType.Unit) throw new InvalidOperationException();//艦娘ページ以外は例外を出しておく

            var window = dwPageCollection.UnitPageFactory.Create(IMyDockingWindow_DockStateChanged);
            window.Show(dockPanel1, DockState.Float);
            if (!window.MyControl.InitFinished) window.MyControl.Init();
            if (dwPageCollection.IsInit2Done && !window.MyControl.Init2Finished) window.MyControl.Init2();
        }

        //ドッキング状態を記録するためのクラス
        public class DockStateCalculator
        {
            public DockState State { get; set; }
            public DockStateCalculatorDestination Destination { get; set; }
        }

        //計算する方向
        public enum DockStateCalculatorDestination
        {
            Vertical, Horizonal,
        }
        #endregion

        //アップデート
        #region アップデート処理
        //艦隊情報のアップデート
        private void FleetInfoUpdate()
        {
            //dwPageCollection.Fleet.TabUpdate();//非同期
            dwPageCollection.Fleet.TabUpdate_Q();
            //dwPageCollection.TabFleetShort.TabUpdate();//非同期
            dwPageCollection.TabFleetShort.TabUpdate_Q();//非同期
            dwPageCollection.CompactScreen.FleetUpdate_Q();//非同期
            dwPageCollection.CompactScreenVertical.FleetUpdate_Q();
            dwPageCollection.UnitPage_AllAutoRefreshCheck_Q();//タブの自動更新、非同期
        }

        //艦隊情報のアップデート（待機可能）
        /*
        private Task FleetInfoUpdateAwaitable()
        {
            dwPageCollection.UnitPage_AllAutoRefreshCheck();//タブの自動更新、非同期（投げっぱなし）
            dwPageCollection.CompactScreen.FleetUpdate();//非同期（投げっぱなし）
            dwPageCollection.CompactScreenVertical.FleetUpdate();
            dwPageCollection.TabFleetShort.TabUpdate(); ;//非同期（投げっぱなし）
            return dwPageCollection.Fleet.TabUpdate();//非同期
        }*/
        //演習
        public void UpdatePractice()
        {
            dwPageCollection.General.TabGeneral_PracticeUpdate_Q();//非同期
        }
        //任務
        public void UpdateQuest()
        {
            dwPageCollection.General.TabGeneral_QuestUpdate_Q();//非同期
            KancolleInfoGeneral.ShowQuestsForm(this);//非同期
            dwPageCollection.QuestViewer.ControlUpdate_Q();//非同期
        }
        //資材
        public void UpdateMaterial()
        {
            dwPageCollection.CompactScreen.MaterialUpdate_Q();
            dwPageCollection.CompactScreenVertical.MaterialUpdate_Q();
            dwPageCollection.TabMaterialShort.TabMaterialUpdate_Q();//非同期
            dwPageCollection.Material.TabMaterialUpdate_Q();//非同期
        }

        //資材（待機可能）これだけ残しておく
        private async Task UpdateMaterialAwaitable()
        {
            dwPageCollection.CompactScreen.MaterialUpdate_Q();
            dwPageCollection.CompactScreenVertical.MaterialUpdate_Q();
            await dwPageCollection.TabMaterialShort.TabMaterialUpdate();
            await dwPageCollection.Material.TabMaterialUpdate();
        }

        //資材グラフ
        public void UpdateMaterialGraph()
        {
            dwPageCollection.Material.TabMaterial_GraphUpdate_Q(null, new EventArgs());//非同期
        }
        //戦果
        public void UpdateSenka()
        {
            dwPageCollection.Senka.TabSenkaUpdate();//非同期
            dwPageCollection.TabSenkaShort.TabSenkaUpdate();//非同期
            dwPageCollection.General.TabGeneral_AdmiralExpUpdate_Q();//非同期
            dwPageCollection.CompactScreen.SenkaUpdate_Q();
            dwPageCollection.CompactScreenVertical.SenkaUpdate_Q();
        }
        //戦果グラフ
        public void UpdateSenkaGraph()
        {
            dwPageCollection.Senka.TabSenka_GraphUpdate(null, new EventArgs());//非同期
        }
        //艦娘タブの実行
        public void UnitList_DoIt()
        {
            dwPageCollection.Unit.TabUnit_ListViewUpdate_Q();//非同期
        }
        //JSON
        public void UpdateJson(string response, string body)
        {
            dwPageCollection.Json.UpdateJson_Q(response, body);//非同期
        }
        //遠征
        public void UpdateMission()
        {
            dwPageCollection.TimerViewer.SetMissionTime_Q();//非同期
            dwPageCollection.General.TabGeneral_MissionUpdate_Q();//非同期
            dwPageCollection.CompactScreen.MissionUpdate_Q();
            dwPageCollection.CompactScreenVertical.MissionUpdate_Q();
        }
        //入渠
        public void UpdateNdock()
        {
            dwPageCollection.TimerViewer.SetNdockTime_Q();//非同期
            dwPageCollection.General.TabGeneral_NdockUpdate_Q();//非同期
            dwPageCollection.CompactScreen.NdockUpdate_Q();
            dwPageCollection.CompactScreenVertical.NdockUpdate_Q();
        }
        //艦娘数
        public void UpdateShipSlotitemNum()
        {
            dwPageCollection.ShipSlotitemNum.SetValue_Q();//非同期
            dwPageCollection.General.TabGeneral_ShipSlotitemNumUpdate_Q();//非同期
            dwPageCollection.CompactScreen.NumUpdate_Q();
            dwPageCollection.CompactScreenVertical.NumUpdate_Q();
        }
        //大破チェッカー
        public void UpdateSankWarning(bool onMapSortie)
        {
            if(APIReqMap.SallyDeckPort != 0 && APIBattle.BattleView.BossFlag < 2)
            {
                var warnstate = KancolleBattle.GetWarnState();
                if ((int)warnstate >= (int)HoppoAlpha.DataLibrary.DataObject.WarnState.ShipUnlockedAndEquipsLockedDamaged 
                    && !Config.SoundDamageDisableFlag)
                {
                    //進撃時
                    if(onMapSortie)
                    {
                        Sounds.PlaySounds(Config.SoundDamageSortieFileName);
                    }
                    //戦闘終了時
                    else
                    {
                        Sounds.PlaySounds(Config.SoundDamageFileName);
                    }
                }
            }
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    KancolleBattle.SetSankWarning(dwPageCollection.SankWarning.textBox_sankwarning);
                }));
            dwPageCollection.General.TabGeneral_TaihaCheckerUpdate_Q();//非同期
        }
        //戦況ビュー
        public void UpdateBattleView()
        {
            UIMethods.UIQueue.Enqueue(new UIMethods(() =>
                {
                    try
                    {
                        KancolleBattle.SetBattleView(dwPageCollection.BattleState);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "エラーが発生しました", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }));
            dwPageCollection.General.TabGeneral_BattleViewUpdate_Q();//非同期
            dwPageCollection.BattleDetail.ControlUpdate_Q();//非同期
            dwPageCollection.BattleDetailSquare.ControlUpdate_Q();
            dwPageCollection.BattleDetailSquare2.ControlUpdate_Q();
            dwPageCollection.CompactScreen.BattleUpdate_Q();
            dwPageCollection.CompactScreenVertical.BattleUpdate_Q();
        }

        //マップ情報
        public void UpdateMapInfo()
        {
            dwPageCollection.MapInfo.TextUpdate_Q();//非同期
        }
        #endregion


        //倍率の変更
        #region ズーム関係
        private void toolStripDropDownButton1_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //倍率
            int after_ratio = Convert.ToInt32(e.ClickedItem.Tag);
            //ズーム
            BrowserZoom(after_ratio);
        }

        //ズームを有効にするか
        private void ZoomEnableSwitch(bool flag)
        {
            CallBacks.SetToolStripDropDownButtonEnabled(toolStripDropDownButton1, statusStrip1, flag);
        }

        public void BrowserZoom(int ratio)
        {
            dwPageCollection.Browser.BrowserZoom(ratio);
            //ToolStrip側のTextの変更
            foreach (ToolStripItem item in toolStripDropDownButton1.DropDownItems)
            {
                CallBacks.SetToolStripMenuItemText(item as ToolStripMenuItem, statusStrip1, (string)item.Tag + "%");
            }
            //該当するToolStripMenuItem
            ToolStripMenuItem menuitem = (from x in toolStripDropDownButton1.DropDownItems.OfType<ToolStripMenuItem>()
                                          where (string)x.Tag == ratio.ToString()
                                          select x).First();
            //チェックマーク
            CallBacks.SetToolStripMenuItemText(menuitem, statusStrip1, CheckString + menuitem.Text);
            //微調整の初期化
            //Config.BrowserOffsetDiff = new Point(0, 0);
            //パラメータの再設定
            IMyDockingWindow_DockStateChanged(null, new EventArgs());
        }
        #endregion

        //StatusStrip関連
        #region StatusStrip関連
        //消音
        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {
        }
        private void toolStripDropDownButton4_Click(object sender, EventArgs e)
        {
            Volume ovol = Volume.GetInstance();
            toolStripDropDownButton4.Text = ovol.IsMute != true ? CheckString + "消音" : "消音";
            ovol.ToggleMute();
        }

        //遠征情報のラベルのセット
        private void toolStripStatusLabel2Update()
        {
            toolStripStatusLabel2.Text = KancolleInfo.GetToolStripMissionStatus();
        }

        //ホームの画面
        private void toolStripDropDownButton2_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //更新ボタン
            if(e.ClickedItem == toolStripMenuItem13)
            {
                //データの更新
                //BackupSerials();
                dwPageCollection.Browser.extraWebBrowser1.Refresh();
                ZoomEnableSwitch(false);
            }
            //画面位置調整
            else if(e.ClickedItem == toolStripMenuItem14)
            {
                dwPageCollection.Browser.ScreenAdjust(this);
            }
            //艦隊晒しのパーサー
            else if(e.ClickedItem == toolStripMenuItem16)
            {
                if (APIPort.ShipsDictionary == null) return;
                KanmusuList kl = new KanmusuList();
                kl.TopMost = Config.ShowTopMost;
                kl.Owner = this;
                this.TopMost = false;
                kl.FormClosed += new FormClosedEventHandler(ModalDialog_FormClosed);
                kl.ShowDialog();
            }
            //ネタバレ回避
            else if(e.ClickedItem == toolStripMenuItem15)
            {
                Config.ShowBattleInfo = !(Config.ShowBattleInfo);
                toolStripMenuItem15.Checked = !(toolStripMenuItem15.Checked);
            }
            //JSONを表示
            else if(e.ClickedItem == toolStripMenuItem17)
            {
                Config.ShowJson = !(Config.ShowJson);
                toolStripMenuItem17.Checked = !(toolStripMenuItem17.Checked);
            }
            //JSONのクリア
            else if(e.ClickedItem == toolStripMenuItem34)
            {
                dwPageCollection.Json.ClearJson();
            }
            //常に最前面
            else if(e.ClickedItem == toolStripMenuItem18)
            {
                Config.ShowTopMost = !(Config.ShowTopMost);
                toolStripMenuItem18.Checked = !(toolStripMenuItem18.Checked);
                this.TopMost = !(this.TopMost);
            }
            //グラフ別窓
            else if(e.ClickedItem == toolStripMenuItem19)
            {
                KancolleInfo.ShowChartViewer(0, this);
            }
            //任務リセット
            else if(e.ClickedItem == toolStripMenuItem22)
            {
                if (!APIReqQuest.IsInited) return;
                APIReqQuest.Quests = new SortedDictionary<int, ApiQuest>();
                UpdateQuest();
            }
            //ポート設定
            else if(e.ClickedItem == toolStripMenuItem23)
            {
                PortConfig pc = new PortConfig();
                pc.TopMost = Config.ShowTopMost;
                pc.Owner = this;
                this.TopMost = false;
                pc.FormClosed += new FormClosedEventHandler(ModalDialog_FormClosed);
                pc.ShowDialog();
            }
            //艦娘リストの別窓表示
            else if(e.ClickedItem == toolStripMenuItem24)
            {
                if (KancolleInfoUnitList.Queries == null) return;
                KancolleInfoUnitList.SubWindow_UnitList_Switch(this);
            }

            //本体設定
            else if(e.ClickedItem == toolStripMenuItem26)
            {
                SettingsViewer view = new SettingsViewer(dockPanel1);
                view.TopMost = Config.ShowTopMost;
                this.TopMost = false;
                view.FormClosed += new FormClosedEventHandler(ModalDialog_FormClosed);
                view.FormClosed += new FormClosedEventHandler(SettingsViewer_FormClosed);
                view.ShowDialog();
            }
            //ボーダー表示
            else if(e.ClickedItem == toolStripMenuItem28)
            {
                ShowSenkaPredict();
            }
            //ブラウザのサイズをデフォルトに
            else if(e.ClickedItem == toolStripMenuItem29)
            {
                BrowserZoom(100);
                this.Size = Config.DefaultFormSize;
            }
            //レイアウト
            else if(e.ClickedItem == toolStripMenuItem27)
            {
                //ここはなし
            }
            //ドッキングの固定
            else if(e.ClickedItem == toolStripMenuItem20)
            {
                ToggleLockLayout(!Config.DockFixing);
            }
            //ログのコンバート
            else if(e.ClickedItem == toolStripMenuItem25)
            {
                LogConvert lg = new LogConvert();
                lg.TopMost = Config.ShowTopMost;
                this.TopMost = false;
                lg.FormClosed += new FormClosedEventHandler(ModalDialog_FormClosed);
                lg.FormClosed += new FormClosedEventHandler(SettingsViewer_FormClosed);
                lg.ShowDialog();
            }
            //おやすみモード
            else if(e.ClickedItem == toolStripMenuItem30)
            {
                toolStripMenuItem30.Checked = !toolStripMenuItem30.Checked;
                Config.OyasumiMode = !Config.OyasumiMode;
            }
            //ゲーム画面の再抽出
            else if(e.ClickedItem == toolStripMenuItem21)
            {
                dwPageCollection.Browser.ApplyStyleSheet();
            }
            //ウィンドウの緊急回収
            else if(e.ClickedItem == toolStripMenuItem38)
            {
                if (MessageBox.Show("画面外に飛んでいってしまったウィンドウを回収します。\n現在のレイアウトは保存されません。よろしいですか？",
                    "ウィンドウの緊急回収", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                    == System.Windows.Forms.DialogResult.Yes)
                {
                    var windows = dockPanel1.Contents
                        .Where(x => x.DockHandler.DockState == DockState.Float && x.DockHandler.Content is DockingWindows.IMyDockingWindow)
                        .Select(x => x.DockHandler.FloatPane);
                    int cnt = 1;
                    foreach (var w in windows)
                    {
                        w.FloatWindow.Location = new Point(this.Location.X + 30 * cnt, this.Location.Y + 30 * cnt);
                        cnt++;
                    }
                }
            }
        }

        //レイアウトの復元
        private void toolStripMenuItem27_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //基本
            if(e.ClickedItem == toolStripMenuItem27_1)
            {
                ChangeLayout("basic");
            }
            //基本2
            else if(e.ClickedItem == toolStripMenuItem27_2)
            {
                ChangeLayout("row2");
            }
            //艦ぶら
            else if(e.ClickedItem == toolStripMenuItem27_3)
            {
                ChangeLayout("kanbura");
            }
            //省スペース
            else if(e.ClickedItem == toolStripMenuItem27_4)
            {
                ChangeLayout("compact");
            }
            //ユーザー
            else if(e.ClickedItem == toolStripMenuItem27_99)
            {
                ChangeLayout("user");
            }
            //---
        }

        //レイアウトのファイル読み込み
        private void toolStripMenuItem27_100_Click(object sedner, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "layout.xml";
            ofd.InitialDirectory = Environment.CurrentDirectory + @"\config\";
            ofd.Filter =
                "XMLファイル(*.xml)|*.xml|すべてのファイル(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Title = "開くファイルを選択してください";
            ofd.RestoreDirectory = true;
            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ChangeLayout("file\n" + ofd.FileName);
            }
        }

        //レイアウトのファイルの保存
        private void toolStripMenuItem27_101_Click(object sedner, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "savelayout.xml";
            sfd.InitialDirectory = Environment.CurrentDirectory + @"\config\";
            sfd.Filter =
                "XMLファイル(*.xml)|*.xml|すべてのファイル(*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.Title = "保存するファイルを選択してください";
            sfd.RestoreDirectory = true;
            //ダイアログを表示する
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                dockPanel1.SaveAsXml(sfd.FileName);
            }
        }

        //ホーム画面の初期化
        private void HomeButtonInit()
        {
            //ネタバレ15 JSON17
            //戦闘ネタバレ回避ボタン
            toolStripMenuItem15.Checked = !Config.ShowBattleInfo;
            //JSONを表示
            toolStripMenuItem17.Checked = Config.ShowJson;
            //最前面
            toolStripMenuItem18.Checked = Config.ShowTopMost;
            this.TopMost = Config.ShowTopMost;
            //左側のコントロール
            LeftControlPanelVisibleSwitch();
        }

        private void toolStripDropDownButton3_Click(object sender, EventArgs e)
        {
            ScreenShot();
        }

        //艦娘数、タイマーの表示のアップデート
        private void LeftControlPanelVisibleSwitch()
        {/*
            panel2.Visible = !Config.DoNotShowShipSlotitemNum;
            panel3.Visible = !Config.DoNotShowMissionNdockTimer;*/
        }

        //本体設定の終了イベント
        private void SettingsViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            LeftControlPanelVisibleSwitch();
            if (sender is SettingsViewer) FleetInfoUpdate();
        }

        //スクリーンショット
        private void ScreenShot()
        {
            //スクショの処理
            HelperScreen.ScreenShot(dwPageCollection.Browser.extraWebBrowser1, "");

            //メッセージ
            SetTextToLabelStatusTimer("保存完了", 5000);
            //よりわかりやすく
            toolStripDropDownButton3.ForeColor = SystemColors.Window;
            toolStripDropDownButton3.BackColor = SystemColors.ControlText;
            labelStatusTimer2 = new Timer();
            labelStatusTimer2.Interval = 500;
            labelStatusTimer2.Tick += new EventHandler(labelStatusTimer2_Tick);
            labelStatusTimer2.Start();
        }

        //右下のLabelStatusに一定時間表示する
        public void SetTextToLabelStatusTimer(string text, int interval_ms)
        {
            toolStripStatusLabel5.Text = text;
            labelStatusTimer.Enabled = false;//カウントリセット
            labelStatusTimer.Interval = interval_ms;
            labelStatusTimer.Tick += new EventHandler(labelStatusTimer_Tick);
            labelStatusTimer.Enabled = true;
            labelStatusTimer.Start();
        }
        private void labelStatusTimer_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel5.Text = LabelStatusDefaultString;
            labelStatusTimer.Stop();
        }

        private void labelStatusTimer2_Tick(object sender, EventArgs e)
        {
            toolStripDropDownButton3.ForeColor = SystemColors.ControlText;
            toolStripDropDownButton3.BackColor = SystemColors.ControlLight;
            labelStatusTimer2.Stop();
        }

        //戦果予測
        public void ShowSenkaPredict()
        {
            if (!HistoricalData.IsInited) return;
            BorderPredict bp = new BorderPredict();
            bp.TopMost = Config.ShowTopMost;
            this.TopMost = false;
            bp.FormClosed += new FormClosedEventHandler(ModalDialog_FormClosed);
            bp.ShowDialog();
        }

        private void ModalDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.TopMost = Config.ShowTopMost;
        }
        #endregion

        
        //ショートカットキー
        #region ショートカットキー
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch(keyData)
            {
                //スクリーンショット
                case Keys.F12:
                    ScreenShot();
                    break;
                //タブの切り替え
                case Keys.F1:
                    if (dwPageCollection != null) dwPageCollection.Activate(dockPanel1, DockingWindows.TabPageType.Fleet);
                    break;
                case Keys.F2:
                    if (dwPageCollection != null) dwPageCollection.Activate(dockPanel1, DockingWindows.TabPageType.General);
                    break;
                case Keys.F3:
                    if (dwPageCollection != null) dwPageCollection.Activate(dockPanel1, DockingWindows.TabPageType.Material);
                    break;
                case Keys.F4:
                    if (dwPageCollection != null) dwPageCollection.Activate(dockPanel1, DockingWindows.TabPageType.Senka);
                    break;
                case Keys.F5:
                    if (dwPageCollection != null) dwPageCollection.Activate(dockPanel1, DockingWindows.TabPageType.Unit);
                    break;
                case Keys.F6:
                    if (dwPageCollection != null) dwPageCollection.Activate(dockPanel1, DockingWindows.TabPageType.EquipSearch);
                    break;
                case Keys.F7:
                    if (dwPageCollection != null) dwPageCollection.Activate(dockPanel1, DockingWindows.TabPageType.Counter);
                    break;
                case Keys.F8:
                    if (dwPageCollection != null) dwPageCollection.Activate(dockPanel1, DockingWindows.TabPageType.Json);
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion


        

    }
}
