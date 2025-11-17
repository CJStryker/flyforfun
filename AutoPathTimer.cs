using System;
using System.Drawing;

namespace HiddenUniverse_WebClient
{
    internal class AutoPathTimer
    {
        private System.Windows.Forms.Timer timer { get; set; }
        public System.Windows.Forms.Timer Timer { get { return timer; } }
        public int Interval { get; set; } = 3000;
        public bool UsePositionState { get; set; } = true;
        public PointF LastPosition { get; set; } = PointF.Empty;

        public void InitTimer()
        {
            if (timer == null)
            {
                timer = new System.Windows.Forms.Timer();
                timer.Tick += new EventHandler(Timer_Tick);
            }
            timer.Interval = Interval;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            FlyffWCForm.Instance.ExecutePathingStep(this);
        }
    }
}
