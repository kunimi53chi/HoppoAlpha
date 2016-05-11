using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.DataObject;
using HoppoAlpha.DataLibrary.SearchModel;

namespace VisualFormTest
{
    public static class Config
    {

        //ゲームの設定
        //編成表示の設定
        //名前 Lv(cond) 燃10弾薬10　
        public static bool[] FleetShowFull = new bool[] { true, true, false, false };
        //倍率
        public static int Ratio = 100;

        //デフォルトの値
        //public static System.Drawing.Point BrowserOffset = new System.Drawing.Point(-100, -77);//-75
        public static System.Drawing.Size GameScreenSize = new System.Drawing.Size(800, 480);
        //public static System.Drawing.Size BrowserSize = new System.Drawing.Size(1000, 1000);//875
        //デフォルトのフォームサイズ
        public static System.Drawing.Size DefaultFormSize = new System.Drawing.Size(1024, 776);
        //フォームサイズ
        public static System.Drawing.Size FormSize = new System.Drawing.Size(1024, 776);
        //フォームのロケーション
        public static System.Drawing.Point FormLocation = new System.Drawing.Point(0, 0);

        //画面位置の微調整用
        public static System.Drawing.Point BrowserOffsetDiff = new System.Drawing.Point(0, 0);

        //下に装備数を表示するか
        public static bool StatusStripShowShipNum = false;
        public static int StatusStripShowShipNumThreshold = 700;
        public static bool TabShowAutoScroll = false;
        public static int TabAutoScrollWidthThreshold = 950;

        //サブウィンドウで艦娘リストを表示するか
        public static bool ShowUnitList = false;
        //サブウィンドウの艦娘リストで表示するクエリ番号
        public static int SubWindowUnitListQueryNo = 0;
        //装備検索で表示したインデックス
        public static int SlotitemSearchMainIndex = 0;
        public static int SlotitemSearchSubIndex = 0;
        //サブウィンドウでグラフを表示するか
        public static bool ShowSubWindowGraph = false;
        //艦娘タブを自動更新を自動更新しないか
        public static bool TabKanmusuAutoRefreshDisable = false;
        //艦娘タブで使用しているクエリ（起動時と終了時のみ操作）
        public static List<int> TabKanmusuQueryNumber = new List<int>();
        //マップ情報でクリア済みのマップを表示する
        public static bool MapInfoShowtCleard = false;
        //終了時の確認をしない
        public static bool OnClosingNotifyDisable = false;

        //S艦隊タブで表示する艦隊インデックス
        public static int TabFleetShortFleetIndex = 0;
        //HPのボーダーライン
        public static double BucketHPRatio = 0.35;
        //使用中の索敵モデル
        public static Models SearchUsingModel = Models.Model33;
        //謎のおまじない
        public static string NazonoOmajonai = "謎のおまじない";

        //--出力するアイテム
        //ファイル名
        private static string Directory = @"config/";
        private static string Filename = @"config/config.dat";
        //JSONを表示するか
        public static bool ShowJson = false;
        //JSONを表示している際にJSONデータを保存しないか
        public static bool LoggingDisableOnShowJson = false;
        //ネタバレ防止
        public static bool ShowBattleInfo = true;
        //最前面表示
        public static bool ShowTopMost = false;
        //ポート設定
        public static int PortNumber = 0;
        //スクリーンショットの保存フォルダ
        public static string ScreenshotDirectory = System.Environment.CurrentDirectory + @"\screenshot\";
        //プロキシアドレス
        public static string ProxyAddress = "";
        //受信専用モード
        public static bool ListeningMode = false;
        //高画質モード
        public static bool HighQualityMode = false;
        //戦闘詳細で表示を保持するか
        public static bool BattleDetailViewKeeping = false;
        //UI更新タイマーのインターバル
        public static int UIRefreshTimerInterval = 120;

        //通知設定
        //バルーン
        public static bool NotShowNotifyBalloonMission = false;
        public static bool NotShowNotifyBalloonNdock = false;
        //効果音（遠征）
        public static bool SoundMissionDisableFlag = false;
        public static string SoundMissionFileName = System.Environment.CurrentDirectory + @"\sounds\arpeggio-f01.mp3";
        //効果音（入渠）
        public static bool SoundNdockDisableFlag = false;
        public static string SoundNdockFileName = System.Environment.CurrentDirectory + @"\sounds\one08.mp3";
        //効果音（大破警告）
        public static bool SoundDamageDisableFlag = false;
        public static string SoundDamageFileName = System.Environment.CurrentDirectory + @"\sounds\warning03.mp3";
        //効果音（大破進撃警告）
        public static bool SoundDamageSortieDisableFlag = false;
        public static string SoundDamageSortieFileName = System.Environment.CurrentDirectory + @"\sounds\warning03.mp3";
        //音量
        public static int SoundVolume = 20;
        public static bool SoundIsMuted = false;

        //ログエクスポートのディレクトリ
        public static string LogExportOutputDirectory = System.Environment.CurrentDirectory;
        //戦果予測のディレクトリ
        public static string SenkaPredictOutputDirectory = System.Environment.CurrentDirectory;
        //ブラウザのお気に入り
        public static string[] BrowserUrlFavorite = Enumerable.Repeat("", 10).ToArray();

        //プリセット情報の重複チェックを旗艦に適用するか
        public static bool PresetDuplicateCheckAppliesFlagship = false;
        
        //--出撃報告書
        //統合モード
        public static SortieReportShipHashIntegrateMode SortieReportIntegrateMode = SortieReportShipHashIntegrateMode.MaskShipTypeDescending;
        //表示色（前景色）
        public static string SortieReportViewForeColor = "Black";
        //表示色（背景色）
        public static string SortieReportViewBackColor = "Linen";
        //期間統合モード
        public static SortieReportTermIntegrateMode SortieReportTermMode = SortieReportTermIntegrateMode.Week;

        //---ドッキング
        //ツールウィンドウのタブサイズ
        public static int ToolWindowTabHeight = 21;
        public static int ToolWindowWidth = 170;
        //ブラウザのタブサイズ
        public static int DocumentWindowTabHeight = 24;
        //ドッキングの固定
        public static bool DockFixing = false;

        //----AlphaDashのパラメーター
        //最小スレッド数
        public static int ThreadPoolMin = 50;

        //おやすみモード
        public static bool OyasumiMode = false;
        //任務をタイトルバーに表示しない
        public static bool QuestNotDisplayToForm = false;
        //ドロップログに追加しない
        public static bool DropRecordAddDisable = false;

        //--外部サイトとの連携
        //艦これDBのユーザートークン
        public static string KancolleDbUserToken = null;
        //艦これDBへの送信を停止する（デフォルトで送信する）
        public static bool KancolleDbPostDisable = false;
        //艦これDBへの送信をプロキシモード時も行う（デフォルトでは行わない）
        public static bool KancolleDbPostOnProxyMode = false;

        //検証DBを活性化するかどうかのフラグ
        public static bool KancolleVerifyDbActivate = true;
        //艦これ検証DBでへの送信を有効にする（デフォルトで送信しない）
        public static bool KancolleVerifyPostEnable = false;
        //艦これDBのダイアログ表示を行わない（デフォルトでは行う）
        public static bool KancolleVerifyNotifyDialogNotShow = false;
        //KCVDBの画面の更新頻度
        public static int KancolleVerifyScreenRefreshTimer = 500;

        //--戦果分析関連
        //潜水マンリスト
        public static Dictionary<int, string> RankingSubmarinerList = new Dictionary<int, string>();
        //潜水マンのEOハンデ
        public static int RankingSubmarinerEOHandicap = 200;
        //表示するボーダーの順位
        public static int[] TabSenkaBorderDisplay = new int[] { 1, 2, 3, 5, 20, 100, 500 };

        //コンストラクタ
        static Config()
        {
            if(File.Exists(Filename))
            {
                var loadResult = HoppoAlpha.DataLibrary.Files.TryLoad(Filename, HoppoAlpha.DataLibrary.DataType.Config);
                ConfigSerializeItem item = (ConfigSerializeItem)loadResult.Instance;
                LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.Config, loadResult, false);

                //置き換え
                ShowJson = item.ShowJson; ShowBattleInfo = item.ShowBattleInfo;
                ShowTopMost = item.ShowTopMost; //ShowSodeuraDisable = item.ShowSodeuraDisable;
                PortNumber = item.PortNumber; if(item.ScreenshotDirectory != null && item.ScreenshotDirectory != "") ScreenshotDirectory = item.ScreenshotDirectory;
                if(item.ProxyAddress != null) ProxyAddress = item.ProxyAddress;
                if (item.Ratio != 0) Ratio = item.Ratio; if (item.FormSizeX >= 10 && item.FormSizeY >= 10) FormSize = new System.Drawing.Size(item.FormSizeX, item.FormSizeY);
                //11-
                ListeningMode = item.ListeningMode; TabKanmusuAutoRefreshDisable = item.TabKanmusuAutoRefreshDisable;
                DockFixing = item.DockFixing; if (item.LogExportOutputDirectory != null) LogExportOutputDirectory = item.LogExportOutputDirectory;
                if (item.BrowserUrlFavorite != null) BrowserUrlFavorite = item.BrowserUrlFavorite;
                FormLocation = new System.Drawing.Point(item.FormLocationX, item.FormLocationY);
                QuestNotDisplayToForm = item.QuestNotDisplayToForm;
                //21-
                DropRecordAddDisable = item.DropRecordAddDisable; SlotitemSearchMainIndex = item.SlotitemSearchMainIndex;
                SlotitemSearchSubIndex = item.SlotitemSearchSubIndex; if (item.TabKanmusuQueryNumber != null) TabKanmusuQueryNumber = item.TabKanmusuQueryNumber;
                TabFleetShortFleetIndex = item.TabFleetShortFleetIndex; if (item.SortieReportIntegrateMode != 0) SortieReportIntegrateMode = (SortieReportShipHashIntegrateMode)item.SortieReportIntegrateMode;
                if (item.SortieReportViewBackColor != null) SortieReportViewBackColor = item.SortieReportViewBackColor; if (item.SortieReportViewForeColor != null) SortieReportViewForeColor = item.SortieReportViewForeColor;
                BattleDetailViewKeeping = item.BattleDetailViewKeeping; LoggingDisableOnShowJson = item.LoggingDisableOnShowJson;
                //31-
                if (item.NazonoOmajonai != null) NazonoOmajonai = item.NazonoOmajonai; if (item.UIRefresTimerhInterval >= 10 && item.UIRefresTimerhInterval <= 1000) UIRefreshTimerInterval = item.UIRefresTimerhInterval;
                if (item.SortieReportTermIntegrateMode != 0) SortieReportTermMode = (SortieReportTermIntegrateMode)item.SortieReportTermIntegrateMode; OnClosingNotifyDisable = item.OnClosingNotifyDisable;
                PresetDuplicateCheckAppliesFlagship = item.PresetDuplicateCheckAppliesFlagship;
                //50--
                //DisableMultiThreading = item.DisableMultiThreading; 
                //if (item.ThreadPoolMin != 0) ThreadPoolMin = item.ThreadPoolMin;
                HighQualityMode = item.HighQualityMode; MapInfoShowtCleard = item.MapInfoShowCleared;
                if (item.SenkaPredictOutputDirectory != null) SenkaPredictOutputDirectory = item.SenkaPredictOutputDirectory;
                if (item.BucketHPRatio > 0.0 && item.BucketHPRatio < 1.0) BucketHPRatio = item.BucketHPRatio; SearchUsingModel = item.SearchUsingModel;
                //ユニットリスト
                //通知
                //200-
                NotShowNotifyBalloonMission = item.NotShowNotifyBalloonMission; NotShowNotifyBalloonNdock = item.NotShowNotifyBalloonNdock;
                SoundMissionDisableFlag = item.SoundMissionDisableFlag;
                if (item.SoundMissionFileName != null) SoundMissionFileName = item.SoundMissionFileName;
                //if (item.SoundMissionMode != 0) SoundMissionMode = item.SoundMissionMode;
                SoundNdockDisableFlag = item.SoundNdockDisableFlag;
                if (item.SoundNdockFileName != null) SoundNdockFileName = item.SoundNdockFileName;
                //if (item.SoundNdockMode != 0) SoundNdockMode = item.SoundNdockMode;
                SoundDamageDisableFlag = item.SoundDamageDisableFlag;
                if (item.SoundDamageFileName != null) SoundDamageFileName = item.SoundDamageFileName;
                //211-
                if (item.SoundVolume != 0) SoundVolume = item.SoundVolume; SoundIsMuted = item.SoundIsMuted;
                SoundDamageSortieDisableFlag = item.SoundDamageSortieDisableFlag;
                if (item.SoundDamageSortieFileName != null) SoundDamageSortieFileName = item.SoundDamageSortieFileName;
                //300-
                if (!string.IsNullOrEmpty(item.KancolleDbUserToken)) KancolleDbUserToken = item.KancolleDbUserToken;
                KancolleDbPostDisable = item.KancolleDbPostDisable; KancolleDbPostOnProxyMode = item.KancolleDbPostOnProxyMode;
                //350-
                KancolleVerifyPostEnable = item.KancolleVerifyPostEnable; KancolleVerifyNotifyDialogNotShow = item.KancolleVerifyNotifyDialogNotShow;
                if (item.KancolleVerifyScreenRefreshTimerInterval >= 10 && item.KancolleVerifyScreenRefreshTimerInterval <= 1000) KancolleVerifyScreenRefreshTimer = item.KancolleVerifyScreenRefreshTimerInterval;
                //400-
                if (item.RankingSubmarinerList != null) RankingSubmarinerList = item.RankingSubmarinerList; if (item.RankingSubmarinerEOHandicap != 0) RankingSubmarinerEOHandicap = item.RankingSubmarinerEOHandicap;
                if (item.TabSenkaBorderDisplay != null && item.TabSenkaBorderDisplay.Length == 7) TabSenkaBorderDisplay = item.TabSenkaBorderDisplay;
            }
            //DPI調整
            var dpi = HelperScreen.GetSystemDpi();
            ToolWindowTabHeight = (int)((double)ToolWindowTabHeight * dpi.ScaleX);
            ToolWindowWidth = (int)((double)ToolWindowWidth * dpi.ScaleX);
            //ブラウザのタブサイズ
            DocumentWindowTabHeight = (int)((double)DocumentWindowTabHeight * dpi.ScaleX);
        }

        //保存
        public static void Save()
        {
            ConfigSerializeItem item = new ConfigSerializeItem();
            item.ShowJson = ShowJson; item.ShowBattleInfo = ShowBattleInfo;
            item.ShowTopMost = ShowTopMost; //item.ShowSodeuraDisable = ShowSodeuraDisable;
            item.PortNumber = PortNumber; item.ScreenshotDirectory = ScreenshotDirectory;
            item.ProxyAddress = ProxyAddress;
            item.Ratio = Ratio; item.FormSizeX = FormSize.Width; item.FormSizeY = FormSize.Height;
            //11-
            item.ListeningMode = ListeningMode; item.TabKanmusuAutoRefreshDisable = TabKanmusuAutoRefreshDisable;
            item.DockFixing = DockFixing; item.LogExportOutputDirectory = LogExportOutputDirectory;
            item.BrowserUrlFavorite = BrowserUrlFavorite; item.FormLocationX = FormLocation.X;
            item.FormLocationY = FormLocation.Y; item.QuestNotDisplayToForm = QuestNotDisplayToForm;
            //21-
            item.DropRecordAddDisable = DropRecordAddDisable; item.SlotitemSearchMainIndex = SlotitemSearchMainIndex;
            item.SlotitemSearchSubIndex = SlotitemSearchSubIndex; item.TabKanmusuQueryNumber = TabKanmusuQueryNumber;
            item.TabFleetShortFleetIndex = TabFleetShortFleetIndex; item.SortieReportIntegrateMode = (int)SortieReportIntegrateMode;
            item.SortieReportViewBackColor = SortieReportViewBackColor; item.SortieReportViewForeColor = SortieReportViewForeColor;
            item.BattleDetailViewKeeping = BattleDetailViewKeeping; item.LoggingDisableOnShowJson = LoggingDisableOnShowJson;
            //31-
            item.NazonoOmajonai = NazonoOmajonai; item.UIRefresTimerhInterval = UIRefreshTimerInterval;
            item.SortieReportTermIntegrateMode = (int)SortieReportTermMode; item.OnClosingNotifyDisable = OnClosingNotifyDisable;
            item.PresetDuplicateCheckAppliesFlagship = PresetDuplicateCheckAppliesFlagship;
            //50-
            //item.DisableMultiThreading = DisableMultiThreading; 
            //item.ThreadPoolMin = ThreadPoolMin;
            item.HighQualityMode = HighQualityMode; item.MapInfoShowCleared = MapInfoShowtCleard;
            item.SenkaPredictOutputDirectory = SenkaPredictOutputDirectory;
            item.BucketHPRatio = BucketHPRatio; item.SearchUsingModel = SearchUsingModel;
            //---
            item.NotShowNotifyBalloonMission = NotShowNotifyBalloonMission; item.NotShowNotifyBalloonNdock = NotShowNotifyBalloonNdock;
            item.SoundMissionDisableFlag = SoundMissionDisableFlag;
            item.SoundMissionFileName = SoundMissionFileName; //item.SoundMissionMode = SoundMissionMode;
            item.SoundNdockDisableFlag = SoundNdockDisableFlag;
            item.SoundNdockFileName = SoundNdockFileName; //item.SoundNdockMode = SoundNdockMode;
            item.SoundDamageDisableFlag = SoundDamageDisableFlag;
            item.SoundDamageFileName = SoundDamageFileName;
            item.SoundVolume = SoundVolume; item.SoundIsMuted = SoundIsMuted;
            //---300
            item.KancolleDbUserToken = KancolleDbUserToken;
            item.KancolleDbPostDisable = KancolleDbPostDisable; item.KancolleDbPostOnProxyMode = KancolleDbPostOnProxyMode;
            //---350
            item.KancolleVerifyPostEnable = KancolleVerifyPostEnable; item.KancolleVerifyNotifyDialogNotShow = KancolleVerifyNotifyDialogNotShow;
            item.KancolleVerifyScreenRefreshTimerInterval = KancolleVerifyScreenRefreshTimer;
            //--400
            item.RankingSubmarinerEOHandicap = RankingSubmarinerEOHandicap; item.RankingSubmarinerList = RankingSubmarinerList;
            item.TabSenkaBorderDisplay = TabSenkaBorderDisplay;

            if(!System.IO.Directory.Exists(Directory))
            {
                System.IO.Directory.CreateDirectory(Directory);
            }

            var saveResult = HoppoAlpha.DataLibrary.Files.Save(Filename, HoppoAlpha.DataLibrary.DataType.Config, item);
            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.Config, saveResult, true);
        }
    }
}
