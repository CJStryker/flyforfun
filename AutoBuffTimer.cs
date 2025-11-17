using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiddenUniverse_WebClient
{
    internal class AutoBuffTimer
    {
        private System.Windows.Forms.Timer timer { get; set; }
        public System.Windows.Forms.Timer Timer { get { return timer; } }
        private System.Windows.Forms.Timer delaybbTimer { get; set; }
        public System.Windows.Forms.Timer DelaybbTimer { get { return delaybbTimer; } }
        private System.Windows.Forms.Timer cdTimer { get; set; }
        public System.Windows.Forms.Timer CDTimer { get { return cdTimer; } }

        public int autoBuffInterval = 0;
        public int currentBuffIndex = 0;
        private string[] selectedBuffsSlots;
        public static int delaybb = 1750;
        public DateTime cdStart = default(DateTime);
        public DateTime abStart = default(DateTime);
        int defaultFIndex;

        public void InitTimer()
        {
            if (autoBuffInterval > 0)
            {
                if (abStart == default(DateTime)) { abStart = DateTime.Now; }
                timer = new System.Windows.Forms.Timer();
                timer.Tick += new EventHandler(Timer_Tick);
                timer.Interval = 200; // in miliseconds
                timer.Start();
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (abStart == default(DateTime)) { abStart = DateTime.Now; }
            DateTime dt = DateTime.Now;
            TimeSpan ts = dt.Subtract(abStart);
            if (ts.TotalMilliseconds >= autoBuffInterval) { FlyffWCForm.Instance.initiateBuff(); abStart = DateTime.Now; }
        }
        public void InitDelaybbTimer()
        {
            delaybbTimer = new System.Windows.Forms.Timer();
            delaybbTimer.Tick += new EventHandler(DelaybbTimer_Tick);
            delaybbTimer.Interval = delaybb;
            delaybbTimer.Start();
        }
        private async void DelaybbTimer_Tick(object sender, EventArgs e)
        {
            if (FlyffWCForm.Instance == null)
            {
                delaybbTimer.Stop();
                return;
            }

            if (FlyffWCForm.Instance.selectedBuffSlots == null || FlyffWCForm.Instance.selectedBuffSlots.Count == 0)
            {
                ResetBuffRotation();
                return;
            }

            if (selectedBuffsSlots == null || selectedBuffsSlots.Length == 0)
            {
                selectedBuffsSlots = FlyffWCForm.Instance.selectedBuffSlots.ToArray();
                currentBuffIndex = 0;
            }

            if (currentBuffIndex >= selectedBuffsSlots.Length)
            {
                await FinishBuffRotationAsync().ConfigureAwait(true);
                return;
            }

            delaybbTimer.Stop();
            if (currentBuffIndex == 0)
            {
                int f, n;
                FlyffWCForm.Instance.AutoBuffStringConvert(selectedBuffsSlots[currentBuffIndex], out f, out n);
                defaultFIndex = f;
            }

            int fKeyIndex, nKeyIndex;
            FlyffWCForm.Instance.AutoBuffStringConvert(selectedBuffsSlots[currentBuffIndex], out fKeyIndex, out nKeyIndex);
            FlyffWCForm.Instance.sendKeyCodeToBrowser(Keybinds.GetTaskbars()[fKeyIndex]);
            await Task.Delay(75).ConfigureAwait(true); // delay between switching hotbar & sending a buff command
            FlyffWCForm.Instance.sendKeyCodeToBrowser(Keybinds.GetSlots()[nKeyIndex]);
            currentBuffIndex++;
            delaybbTimer.Start();
        }

        private async Task FinishBuffRotationAsync()
        {
            delaybbTimer.Stop();
            await Task.Delay(1000).ConfigureAwait(true);
            if (FlyffWCForm.Instance.healWasEnabled)
            {
                FlyffWCForm.Instance.autoHealBox.Checked = true;
            }
            if (selectedBuffsSlots != null && selectedBuffsSlots.Length > 0)
            {
                FlyffWCForm.Instance.sendKeyCodeToBrowser(Keybinds.GetTaskbars()[defaultFIndex]);
            }
            ResetBuffRotation();
        }

        private void ResetBuffRotation()
        {
            defaultFIndex = default(int);
            currentBuffIndex = 0;
            selectedBuffsSlots = null;
            if (delaybbTimer != null)
            {
                delaybbTimer.Stop();
            }
        }
        public void InitCDTimer()
        {
            if (cdStart == default(DateTime)) { cdStart = DateTime.Now; }
            cdTimer = new System.Windows.Forms.Timer();
            cdTimer.Tick += new EventHandler(CDTimer_Tick);
            cdTimer.Interval = 100; // in miliseconds
            cdTimer.Start();
        }
        private void CDTimer_Tick(object sender, EventArgs e)
        {
            if (cdStart == default(DateTime)) { cdStart = DateTime.Now; }
            DateTime dt = DateTime.Now;
            TimeSpan ts = dt.Subtract(cdStart);
            var cdsec = (autoBuffInterval - ts.TotalMilliseconds + delaybb) / 1000;
            FlyffWCForm.Instance.autoBuffCD.Text = cdsec.ToString("0.00") + " Seconds";
            if (ts.TotalMilliseconds - delaybb >= autoBuffInterval) { abStart = cdStart = DateTime.Now; }
        }
    }
}
