using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using mshtml;

namespace VisualFormTest.UserControls
{
    public partial class HoppoBrowser : UserControl
    {

        public bool IsHelperShown { get; set; }
        private int browserRatio = 100;
        private Dpi _dpi;
        public ToolStripDropDownButton ToolStrip { get; set; }
        private static object lockObject = new object();

        public HoppoBrowser()
        {
            InitializeComponent();
            this._dpi = HelperScreen.GetSystemDpi();
            this.ClientSize = extraWebBrowser1.Size;

        }

        private void extraWebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            lock (lockObject)
            {
                //CSSの書き換え
                ApplyStyleSheet();

                //サイズの変更
                DockingWindows.DockWindowHoppoBrowser page = this.FindForm() as DockingWindows.DockWindowHoppoBrowser;
                page.MainForm.BrowserZoom(Config.Ratio);
            }
        }

        public void ApplyStyleSheet()
        {
            try
            {
                //CSSの書き換え
                var document = extraWebBrowser1.Document;
                if (document == null) return;

                if (document.Url.AbsoluteUri.Contains(".swf?"))
                {
                    document.Body.SetAttribute("width", "100%");
                    document.Body.SetAttribute("height", "100%");
                }
                else
                {
                    var swf = getFrameElementById(document, "externalswf");
                    if (swf == null) return;

                    // InvokeScriptは関数しか呼べないようなので、スクリプトをevalで渡す
                    document.InvokeScript("eval", new object[] { Properties.Resources.PageScript });
                    swf.Document.InvokeScript("eval", new object[] { Properties.Resources.FrameScript });
                }
            }
            catch(Exception)
            {
                LogSystem.AddLogMessage("スタイルシートの適用に失敗しました");
                return;
            }
        }

        public void BrowserZoom(int ratio)
        {
            //ズーム倍率が変わっていないか
            if (browserRatio == ratio) return;
            //ズームのサイズ
            double zoomFactor = (double)ratio / 100.0;
            double baseX = (double)Config.GameScreenSize.Width * _dpi.ScaleX;
            double baseY = (double)Config.GameScreenSize.Height * _dpi.ScaleY;
            Size newSize = new System.Drawing.Size((int)(baseX * zoomFactor), (int)(baseY * zoomFactor));
            //ズーム
            if(Config.HighQualityMode)
            {
                var wb = extraWebBrowser1.ActiveXInstance as SHDocVw.IWebBrowser2;
                if (wb == null || wb.ReadyState == SHDocVw.tagREADYSTATE.READYSTATE_UNINITIALIZED || wb.Busy) return;

                object pout = null;
                object pin = ratio;

                wb.ExecWB(SHDocVw.OLECMDID.OLECMDID_OPTICAL_ZOOM, SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref pin, ref pout);
            }
            extraWebBrowser1.Size = newSize;
            //パネルの微補正
            this.ClientSize = extraWebBrowser1.Size;
            //変更倍率の記録
            browserRatio = ratio;
            Config.Ratio = ratio;
        }

        //微調整
        public void ScreenAdjust(Form1 owner)
        {
            if (IsHelperShown) return;
            if (extraWebBrowser1.Document == null) return;

            BrowserHelper sc = new BrowserHelper();
            int scleft, sctop;

            //Documentの解析
            mshtml.IHTMLDocument3 doc = (mshtml.IHTMLDocument3)extraWebBrowser1.Document.DomDocument;
            mshtml.IHTMLElement2 elm = (mshtml.IHTMLElement2)doc.documentElement;
            scleft = elm.scrollLeft;
            sctop = elm.scrollTop;

            sc.SetWebBrowser(extraWebBrowser1, this, scleft, sctop,
                Config.BrowserOffsetDiff.X, Config.BrowserOffsetDiff.Y);
            sc.FormClosed += sc_FormClosed;
            sc.Owner = owner;

            IsHelperShown = true;
            sc.Show();
        }

        //画面位置調整のClose
        void sc_FormClosed(object sender, FormClosedEventArgs e)
        {
            BrowserZoom(Config.Ratio);
            IsHelperShown = false;
            if (extraWebBrowser1.ScrollBarsEnabled) extraWebBrowser1.ScrollBarsEnabled = false;
        }

        // ショートカットキーが反映されない問題の対策
        void Browser_ReplacedKeyDown(object sender, KeyEventArgs e)
        {
            if (ToolStrip == null) return;

            foreach (var item in ToolStrip.DropDownItems)
            {

                ToolStripMenuItem menu = item as ToolStripMenuItem;

                if (menu != null)
                {
                    if (e.KeyData == menu.ShortcutKeys)
                    {
                        menu.PerformClick();
                        e.Handled = true;
                    }
                }
            }
        }
        #region 呪文
        // 中のフレームからidにマッチする要素を返す
        private static HtmlElement getFrameElementById(HtmlDocument document, String id)
        {
            foreach (HtmlWindow frame in document.Window.Frames)
            {

                // frameが別ドメインだとセキュリティ上の問題（クロスフレームスクリプティング）
                // からアクセスができないのでアクセスできるドキュメントに変換する
                IServiceProvider provider = (IServiceProvider)frame.DomWindow;
                object ppvobj;
                provider.QueryService(typeof(SHDocVw.IWebBrowserApp).GUID, typeof(SHDocVw.IWebBrowser2).GUID, out ppvobj);
                var htmlDocument = WrapHTMLDocument((IHTMLDocument2)((SHDocVw.IWebBrowser2)ppvobj).Document);
                var htmlElement = htmlDocument.GetElementById(id);
                if (htmlElement == null)
                    continue;

                return htmlElement;
            }

            return null;
        }

        // ラッパークラスに戻す
        private static HtmlDocument WrapHTMLDocument(IHTMLDocument2 document)
        {
            ConstructorInfo[] constructor = typeof(HtmlDocument).GetConstructors(
                BindingFlags.NonPublic | BindingFlags.Instance);
            return (HtmlDocument)constructor[0].Invoke(new object[] { null, document });
        }

        [ComImport, Guid("6d5140c1-7436-11ce-8034-00aa006009fa"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(false)]
        internal interface IServiceProvider
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int QueryService(ref Guid guidService, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);
        }

        #endregion

    }

    //ショートカットを無効にするブラウザ
    internal class ExtraWebBrowser : WebBrowser
    {

        public event KeyEventHandler ReplacedKeyDown = delegate { };


        public ExtraWebBrowser()
            : base() { }

        public override bool PreProcessMessage(ref Message msg)
        {

            if (msg.Msg == 0x100)
            {		//WM_KEYDOWN

                var e = new KeyEventArgs((Keys)msg.WParam | ModifierKeys);
                ReplacedKeyDown(this, e);

                if (e.Handled)
                    return true;
            }

            return base.PreProcessMessage(ref msg);
        }
    }

}
