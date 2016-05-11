using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;

namespace VisualFormTest
{
    public static class HelperDockWindow
    {
        //タブ込みのサイズの取得
        public static Size GetIMyDockWindowSizeWithTabs(this DockingWindows.IMyDockingWindow window)
        {
            DockContent content = window as DockContent;
            //タブ抜きのサイズ
            int x = window.RealWidth;
            int y = window.RealHeight;
            //タブ込みのサイズ
            int contentcount = content.Pane.Contents.Where(k => k.DockHandler.DockState != DockState.Unknown && k.DockHandler.DockState != DockState.Hidden).Count();
            if(contentcount > 1)
            {
                y += Config.DocumentWindowTabHeight;
            }
            return new Size(x, y);
        }
    }
}
