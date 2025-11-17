using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace HiddenUniverse_WebClient
{
    internal class AutoUseTimer
    {
       private System.Windows.Forms.Timer timer { get; set; }
        public System.Windows.Forms.Timer Timer { get { return timer; } }
        internal int interval;
        internal string owner = null;
        public void InitTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = interval; // in miliseconds
            timer.Start();
        }
        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (owner != null && FlyffWCForm.Instance != null)
            {
                await FlyffWCForm.Instance.InitAutoUseAsync(owner).ConfigureAwait(true);
            }
        }
    }
}
