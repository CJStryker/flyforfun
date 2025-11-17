using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiddenUniverse_WebClient
{
    internal class AutoFollowTimer
    {
        private System.Windows.Forms.Timer timer { get; set; }
        public System.Windows.Forms.Timer Timer { get { return timer; } }
        public void InitTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = 5000; // in miliseconds
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            FlyffWCForm.Instance.sendKeyCodeToBrowser(Keybinds.follow) ;
        }
    }
}
