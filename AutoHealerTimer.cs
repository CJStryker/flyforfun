using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiddenUniverse_WebClient
{
    internal class AutoHealTimer
    {
        private System.Windows.Forms.Timer timer { get; set; }
        public System.Windows.Forms.Timer Timer { get { return timer; } }
        public int autoHealInterval = 0;
        public void InitTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = autoHealInterval * 1000; // in miliseconds
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            FlyffWCForm.Instance.sendKeyCodeToBrowser(Keybinds.actionKey);
        }
    }
}
