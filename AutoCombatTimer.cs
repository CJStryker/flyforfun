using System;

namespace HiddenUniverse_WebClient
{
    internal class AutoCombatTimer
    {
        private System.Windows.Forms.Timer timer { get; set; }
        public System.Windows.Forms.Timer Timer { get { return timer; } }
        public int Interval { get; set; } = 2000;
        public bool SmartModeEnabled { get; set; } = true;

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
            if (SmartModeEnabled && FlyffWCForm.Instance.AutomationBridgeAvailable && !FlyffWCForm.Instance.PlayerInCombat)
            {
                return;
            }
            FlyffWCForm.Instance.ExecuteCombatRotation();
        }
    }
}
