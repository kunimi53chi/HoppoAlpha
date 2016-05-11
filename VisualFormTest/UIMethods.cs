using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

namespace VisualFormTest
{
    public class UIMethods
    {
        #region NonStatic
        public Action ActionDelegate { get; set; }

        public UIMethods(Action action)
        {
            this.ActionDelegate = action;
        }

        public void Invoke()
        {
            if (this.ActionDelegate != null) ActionDelegate();
        }
        #endregion


        #region Static
        public static Queue<UIMethods> UIQueue { get; set; }

        private static Timer timer;
        private static Form1 _form1;


        static UIMethods()
        {
            //キューの初期化
            UIQueue = new Queue<UIMethods>();
        }

        //タイマーの初期化＋開始
        public static void InitTimer(Form1 form1)
        {
            //UIタイマーの開始
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTimer);
            timer.Interval = Config.UIRefreshTimerInterval;
            timer.SynchronizingObject = form1;
            _form1 = form1;
            timer.Start();
        }

        static void OnElapsedTimer (object sender, EventArgs e)
        {
            if(UIQueue.Count > 0)
            {
                if (_form1.IsDisposed) return;
                UIQueue.Dequeue().Invoke();
            }
        }

        public static void SetIntervalTimer()
        {
            timer.Interval = Config.UIRefreshTimerInterval;
        }
        #endregion
    }
}
