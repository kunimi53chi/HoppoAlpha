using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace VisualFormTest
{
    //ElectricObserverからのお借り
    public static class HelperScreen
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr ptr);
        [DllImport("user32.dll")]
        static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern uint GetDeviceCaps(
            IntPtr hdc, // handle to DC
            int nIndex // index of capability
        );
        [DllImport("user32.dll")]
        static extern bool SetProcessDPIAware();

        const int LOGPIXELSX = 88;
        const int LOGPIXELSY = 90;

        static Dpi _dpi;
        public static Dpi GetSystemDpi()
        {
            if (_dpi == null)
            {
                //SetProcessDPIAware(); //重要
                IntPtr screenDC = GetDC(IntPtr.Zero);
                uint dpi_x = GetDeviceCaps(screenDC, LOGPIXELSX);
                uint dpi_y = GetDeviceCaps(screenDC, LOGPIXELSY);
                _dpi = new Dpi(dpi_x, dpi_y);
                ReleaseDC(IntPtr.Zero, screenDC);
            }
            return _dpi;
        }

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private extern static bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        /// <summary>
        /// スクリーンショットを行う
        /// </summary>
        /// <param name="ctrl">キャプチャするコントロール</param>
        /// <param name="appendMessage">ファイル名に追加する文字列</param>
        /// <param name="startX">トリミング開始X座標</param>
        /// <param name="startY">トリミング開始Y座標</param>
        /// <returns>取得できたイメージ</returns>
        public static void ScreenShot(Control ctrl, string appendMessage, int startX = 0, int startY = 0)
        {
            //ディレクトリの作成
            string dir = Config.ScreenshotDirectory;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            //ファイル名
            string filename;
            if (string.IsNullOrEmpty(appendMessage)) filename = DateTime.Now.ToString("yyyyMMddHHmmssff") + ".png";
            else filename = DateTime.Now.ToString("yyyyMMddHHmmssff") + "_" + appendMessage + ".png";
            //コントロールのキャプチャ
            try
            {
                using (Bitmap img = new Bitmap(ctrl.Width, ctrl.Height))
                {
                    using (Graphics memg = Graphics.FromImage(img))
                    {
                        IntPtr dc = memg.GetHdc();
                        PrintWindow(ctrl.Handle, dc, 0);
                        memg.ReleaseHdc(dc);
                    }
                    //トリミング
                    if (startX != 0 && startY != 0 && startX < ctrl.Width && startY < ctrl.Height)
                    {
                        Rectangle rect = new Rectangle(startX, startY, img.Width - startX, img.Height - startY);
                        using (Bitmap trimmed = img.Clone(rect, img.PixelFormat))
                        {
                            trimmed.Save(dir + filename, System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }
                    //トリミングしない場合
                    else
                    {
                        img.Save(dir + filename, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("スクリーンショットの保存に失敗しました\n" + ex, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

    }

    public class Dpi
    {
        public static readonly Dpi Default = new Dpi(96, 96);

        public uint X { get; private set; }
        public uint Y { get; private set; }

        public Dpi(uint x, uint y)
        {
            this.X = x;
            this.Y = y;
        }

        public double ScaleX
        {
            get { return this.X / (double)Default.X; }
        }

        public double ScaleY
        {
            get { return this.Y / (double)Default.Y; }
        }
    }
}
