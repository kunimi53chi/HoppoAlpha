using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualFormTest.UserControls
{
    public partial class KisuTetyo : UserControl
    {
        public bool IsShown { get; set; }

        Uri uri = new Uri("http://kcvdb.jp/KISU_2");

        public KisuTetyo()
        {
            InitializeComponent();
        }

        public void Navigate()
        {
            if (extraWebBrowser1.Url != uri) extraWebBrowser1.Navigate(uri);
            else extraWebBrowser1.Refresh();
        }

        public void BrowserRefresh()
        {
            extraWebBrowser1.Refresh();
        }

        private void toolStripMenuItem_refresh_Click(object sender, EventArgs e)
        {
            BrowserRefresh();
        }
    }
}
