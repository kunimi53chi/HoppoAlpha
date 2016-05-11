using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualFormTest
{
    public partial class BrowserHelper : Form
    {
        private WebBrowser webBrowser;
        private UserControls.HoppoBrowser hoppoBrowser;
        private int defaultOffsetX, defaultOffsetY;

        public BrowserHelper()
        {
            InitializeComponent();
        }

        public void SetWebBrowser(WebBrowser web, UserControls.HoppoBrowser browser,
            int defaultoffset_x, int default_offset_y, 
            int saved_value_x, int saved_value_y)
        {
            this.webBrowser = web;
            this.hoppoBrowser = browser;
            this.defaultOffsetX = defaultoffset_x - saved_value_x;
            this.defaultOffsetY = default_offset_y - saved_value_y;
            this.numericUpDown1.Value = saved_value_x; this.numericUpDown2.Value = saved_value_y;

            this.comboBox1.Items.AddRange(Enumerable.Range(0, Config.BrowserUrlFavorite.Length)
                .Select(x => string.Format("[{0}] {1}", x, Config.BrowserUrlFavorite[x])).ToArray());
            this.textBox1.Text = web.Url.AbsoluteUri;
        }

        private void button_scrollreset_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
        }

        private void Adjust()
        {
            //移動する距離
            int x = defaultOffsetX + (int)numericUpDown1.Value;
            int y = defaultOffsetY + (int)numericUpDown2.Value;
            //ブラウザの位置の移動なら
            webBrowser.Document.Window.ScrollTo(x, y);
        }

        delegate void SetWebBrowserScrollBarsCallBack(bool flag);
        private void SetWebBrowserScrollBars(bool flag)
        {
            if (webBrowser == null) return;
            if(webBrowser.InvokeRequired)
            {
                SetWebBrowserScrollBarsCallBack d = new SetWebBrowserScrollBarsCallBack(SetWebBrowserScrollBars);
                webBrowser.Invoke(d, new object[] { flag });
            }
            else
            {
                webBrowser.ScrollBarsEnabled = flag;
            }
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Adjust();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            Adjust();
        }

        private void button_scrollapply_Click(object sender, EventArgs e)
        {
            Config.BrowserOffsetDiff = new Point((int)numericUpDown1.Value, (int)numericUpDown2.Value);
        }

        private void button_urlfavorite_Click(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            if (index == -1) return;
            if(string.IsNullOrWhiteSpace(textBox1.Text)) return;
            //Config側の書き換え
            Config.BrowserUrlFavorite[index] = textBox1.Text;
            //ComboBox側
            comboBox1.Items[index] = string.Format("[{0}] {1}", index, textBox1.Text);
         }

        private async void button_urlnavigate_Click(object sender, EventArgs e)
        {
            button_urlnavigate.Enabled = false;
            if (webBrowser != null)
            {
                MethodInvoker mi = delegate() { webBrowser.Navigate(textBox1.Text); };
                await Task.Factory.StartNew(() =>
                {
                    if (!webBrowser.IsHandleCreated || webBrowser.InvokeRequired)
                    {
                        webBrowser.Invoke(mi);
                    }
                    else
                    {
                        mi.Invoke();
                    }
                });
            }
            button_urlnavigate.Enabled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            if (index == -1) return;
            //Config側から
            textBox1.Text = Config.BrowserUrlFavorite[index];
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            Config.BrowserOffsetDiff = new Point((int)numericUpDown1.Value, (int)numericUpDown2.Value);
            if (checkBox1.Checked) SetWebBrowserScrollBars(false);
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked) SetWebBrowserScrollBars(false);            
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SetWebBrowserScrollBars(checkBox1.Checked);
        }

        private void button_style_Click(object sender, EventArgs e)
        {
            hoppoBrowser.ApplyStyleSheet();
        }
    }
}
