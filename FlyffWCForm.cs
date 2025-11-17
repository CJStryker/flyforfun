using System;
using System.Drawing;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using AutoUpdaterDotNET;
using System.Threading.Tasks;

namespace HiddenUniverse_WebClient
{
    public partial class FlyffWCForm : Form 
    {
        // References
        private static FlyffWCForm _instance;
        public ChromiumWebBrowser chromeBrowser;
        public bool healWasEnabled;
        private System.Windows.Forms.Timer waitForGameExitTimer;
        private AutoHealTimer autoHealerTimer = new AutoHealTimer();
        private AutoFollowTimer autoFollowTimer = new AutoFollowTimer();
        private AutoBuffTimer autoBuffTimer = new AutoBuffTimer();
        private AutoUseTimer autoUseTimerA = new AutoUseTimer();
        private AutoUseTimer autoUseTimerB = new AutoUseTimer();
        private AutoUseTimer autoUseTimerC = new AutoUseTimer();

        // Configuration Variables
        bool assistMode = false;
        public int autoHealSelectedIndex = -1;      
        public int delaybb = 1500;

        // Auto Use Configuration
        internal Point autoUsePosA, autoUsePosB, autoUsePosC;
        internal int autoUseIntervalA = 300000;
        internal int autoUseIntervalB = 300000;
        internal int autoUseIntervalC = 300000;
        public List<string> selectedBuffSlots { get; set; }

        // initiailization
        public FlyffWCForm()
        {
            if (_instance == null) { _instance = this; }
            InitializeComponent();
            CheckForUpdates();
            SetArguments();
        }
        public static FlyffWCForm Instance { get { return _instance; } }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Shown += new EventHandler(Form1_Shown);
        }
        private void SetArguments()
        {
            ArgumentManager.Instance.InitializeArguments();
            if (assistMode) { selectedBuffSlots = new List<string>(); SaveManager.Instance.LoadAssistfsConfig(); }
            InitializeChromium();
        }
        public void EnableAssistMode()
        {
            assistMode = true;
            autoHealBox.Enabled = true;
            autoHealBox.Visible = true;
            autoBuffBox.Visible = true;
            autoBuffTime.Visible = true;
            autoBuffTree.Visible = true;
            autoBuffTree.Enabled = true;
            EnableAutoFollow();
        }
        public void EnableAutoFollow()
        {
            autoFollowBox.Visible = true;
            autoFollowBox.Enabled = true;
            keybindsButt.Visible = keybindsButt.Enabled = true;
            SaveManager.Instance.LoadKeybindsConfig();
        }
        public void EnableAutoUse()
        {
            autoUseTB.Visible = autoUseTB.Enabled = autoUseA.Visible = autoUseA.Enabled = autoUseB.Visible = autoUseB.Enabled = autoUseC.Visible = autoUseC.Enabled = autoUseButt.Visible = autoUseButt.Enabled = true;
            autoUseHeaderLabel.Visible = true;
        }
        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            Cef.EnableHighDPISupport();
            settings.CachePath = ArgumentManager.profilePath;
            settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 /CefSharp Browser" + Cef.CefSharpVersion;
            Cef.Initialize(settings);
            chromeBrowser = new ChromiumWebBrowser("https://universe.flyff.com/play");
            browserHostPanel.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
            if (autoUseTB.Enabled) {
                chromeBrowser.JavascriptMessageReceived += chromeBrowser_SetMousePos;
                chromeBrowser.FrameLoadEnd += chromeBrowser_GetMousePosOnClick;
            }
        }
        private void Form1_Shown(Object sender, EventArgs e)
        {
            InitWaitForGameExitTimer();
        }

        // Auto Heal Methods
        private void autoHealBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (autoHealBox.CheckState == CheckState.Checked) { 
                autoHealBox.BackColor = Color.PeachPuff;
                autoHealTime.Enabled = true;
                autoHealTime.Visible = true;
                autoHealTime.BackColor = Color.PeachPuff;
                if (autoHealSelectedIndex == -1) { autoHealTime.SelectedIndex = 2; } else { autoHealTime.SelectedIndex = autoHealSelectedIndex; }
                if (autoHealerTimer.Timer == null) { autoHealerTimer.InitTimer(); }
                else if (autoHealerTimer.Timer != null && !autoHealerTimer.Timer.Enabled) { autoHealerTimer.Timer.Interval = autoHealerTimer.autoHealInterval * 1000; autoHealerTimer.Timer.Start(); }
            }
            else { autoHealBox.BackColor = Color.Gray;
                autoHealTime.Enabled = false;
                autoHealTime.Visible = false;
                autoHealTime.BackColor = Color.Gray;
                autoHealerTimer.Timer.Stop();
            }
        }
        private void autoHealTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            GroupCollection gc = RegexCheck.Test(autoHealTime.GetItemText(autoHealTime.SelectedItem), "Every ([0-9]{1,2}) seconds");
            if (gc != null)
            {
                var interval = Int32.Parse(gc[1].Value);
                autoHealSelectedIndex = autoHealTime.SelectedIndex;
                autoHealerTimer.autoHealInterval = interval;
                if (autoHealerTimer.Timer != null && autoHealerTimer.Timer.Enabled)
                {
                    autoHealerTimer.Timer.Interval = autoHealerTimer.autoHealInterval * 1000; // in miliseconds
                }
                else if (autoHealerTimer.Timer != null && !autoHealerTimer.Timer.Enabled)
                {
                    autoHealerTimer.Timer.Interval = autoHealerTimer.autoHealInterval * 1000; // in miliseconds
                    autoHealerTimer.Timer.Start();
                }
            }
        }
        private void autoHealTime_DropDown(object sender, EventArgs e)
        {
            if (autoHealerTimer.Timer != null && autoHealerTimer.Timer.Enabled) { autoHealerTimer.Timer.Stop(); }
        }
        private void autoHealTime_DropDownClosed(object sender, EventArgs e)
        {
            if (autoHealerTimer.Timer != null && !autoHealerTimer.Timer.Enabled) { autoHealerTimer.Timer.Start(); }
        }
        public void CheckHealBox()//used to enable it after buff ends
        {
            autoHealBox.CheckState = CheckState.Checked;
        }

        // Auto Buff Methods
        private void autoBuffBox_Click(object sender, EventArgs e)
        {
            initiateBuff();
        }
        private void autoBuffTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count > 0 ) // If this is a parent node being checked
            {
                foreach (TreeNode childNode in e.Node.Nodes) // check/uncheck children nodes if there are any
                {
                    if (childNode.Checked != childNode.Parent.Checked) // Making sure no duplication of entries upon config load
                    {
                        childNode.Checked = childNode.Parent.Checked;
                    }
                }
            }
            else if (e.Node.Parent.Checked) // This is a child node and it's parent is checked
            {
                bool parentCheck = false;
                foreach (TreeNode child in e.Node.Parent.Nodes) // verify if parent needs to be unchecked (if all childrens are unchecked)
                {
                    if (child.Checked) { parentCheck = true; break; }
                }
                if (!parentCheck) { e.Node.Parent.Checked = false; }
            }
            else // a child node and it's parent is not checked
            {
                bool parentCheck = true;
                foreach (TreeNode child in e.Node.Parent.Nodes) // verify if parent needs to be checked (if all childrens are checked)
                {
                    if (!child.Checked) { parentCheck = false; break; }
                }
                if (parentCheck) { e.Node.Parent.Checked = true; }
            }
            if (e.Node.Checked && e.Node.Parent != null) // Add to list if checked
            {
                string cn = e.Node.Parent.Index.ToString() + "x" + e.Node.Index;
                selectedBuffSlots.Add(cn);                
            }
            else if (!e.Node.Checked && e.Node.Parent != null) // Remove from list if not checked
            {
                string cn = e.Node.Parent.Index.ToString() + "x" + e.Node.Index;
                selectedBuffSlots.Remove(cn);
            }
            selectedBuffSlots.Sort(); // Sorts the slots by placement and not by user selection order.
            if (selectedBuffSlots.Count > 0 && !autoBuffBox.Enabled) // Enable Buffbox
            {
                autoBuffBox.Enabled = true;
                autoBuffBox.BackColor = Color.PeachPuff;
                autoBuffTime.Enabled = true;
                autoBuffTime.SelectedIndex = 0;
            }
            else if (selectedBuffSlots.Count <= 0 && autoBuffBox.Enabled) // Disable Boffbox
            {
                autoBuffBox.Enabled = false;
                autoBuffBox.BackColor = Color.Gray;
                autoBuffTime.Enabled = false;
                autoBuffTime.SelectedIndex = 0;
            }
        }
        public void initiateBuff()
        {
            if (autoHealerTimer.Timer != null && autoHealerTimer.Timer.Enabled)
            {
                autoHealerTimer.Timer.Stop();
                autoHealBox.Checked = false;
                healWasEnabled = true;
            }
            else { healWasEnabled = false; }
            if (autoBuffTimer.DelaybbTimer == null) { autoBuffTimer.InitDelaybbTimer(); }
            else if (autoBuffTimer.DelaybbTimer != null && autoBuffTimer.DelaybbTimer.Enabled) { autoBuffTimer.DelaybbTimer.Stop(); autoBuffTimer.currentBuffIndex = 0; }
            else if (autoBuffTimer.DelaybbTimer != null && !autoBuffTimer.DelaybbTimer.Enabled) { autoBuffTimer.currentBuffIndex = 0; autoBuffTimer.DelaybbTimer.Start(); }
        }
        private void autoBuffTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            GroupCollection gc = RegexCheck.Test(autoBuffTime.GetItemText(autoBuffTime.SelectedItem), "Every ([0-9]{1,2}) minutes");
            if (gc != null)
            {
                var interval = Int32.Parse(gc[1].Value);
                autoBuffTimer.cdStart = autoBuffTimer.abStart = DateTime.Now;
                if (autoBuffTimer.Timer == null) { autoBuffTimer.autoBuffInterval = interval * 60000;
                    autoBuffTimer.InitTimer();
                    autoBuffTimer.InitCDTimer();
                }
                else if (autoBuffTimer.Timer != null && autoBuffTimer.Timer.Enabled) { 
                    autoBuffTimer.autoBuffInterval = interval * 60000; }
                else if (autoBuffTimer.Timer != null && !autoBuffTimer.Timer.Enabled) { 
                    autoBuffTimer.autoBuffInterval = interval * 60000; 
                    autoBuffTimer.Timer.Start();
                    autoBuffTimer.CDTimer.Start();
                }
                autoBuffCD.Visible = true;
            }
            else
            {
                if (autoBuffTimer.Timer != null && autoBuffTimer.Timer.Enabled) { autoBuffTimer.Timer.Stop(); autoBuffTimer.CDTimer.Stop();  }
                autoBuffCD.Visible = false;
            }
        }
        public void autoBuffTreeCheckItem (string[] config)
        {
            for (int i = 1; i < config.Length; i++) // check all saved items
            {
                int fKeyIndex, nKeyIndex;
                AutoBuffStringConvert(config[i], out fKeyIndex, out nKeyIndex);
                autoBuffTree.Nodes[fKeyIndex].Nodes[nKeyIndex].Checked = true;
            }
            for (int i = 0; i < autoBuffTree.Nodes.Count; i++)
            {
                bool parentCheck = true;
                foreach (TreeNode child in autoBuffTree.Nodes[i].Nodes)
                {
                    if (!child.Checked) { parentCheck = false; break; }
                }
                if (parentCheck) { autoBuffTree.Nodes[i].Checked = true; }
            }
        }
        public void AutoBuffStringConvert(string str, out int fKeyIndex, out int nKeyIndex)
        {
            var split = str.Split("x".ToCharArray());
            fKeyIndex = Int32.Parse(split[0]);
            nKeyIndex = Int32.Parse(split[1]);
        }
        private void autoBuffTime_DropDown(object sender, EventArgs e)
        {
            if (autoHealerTimer.Timer != null && autoHealerTimer.Timer.Enabled) { autoHealerTimer.Timer.Stop(); }
        }
        private void autoBuffTime_DropDownClosed(object sender, EventArgs e)
        {
            if (autoHealerTimer.Timer != null && !autoHealerTimer.Timer.Enabled && autoHealTime.Enabled) { autoHealerTimer.Timer.Start(); }
        }

        // Auto Follow Methods
        private void autoFollowBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (autoFollowBox.Checked)
            {
                autoFollowBox.BackColor = Color.PeachPuff;
                if (autoFollowTimer.Timer == null)
                {
                    autoFollowTimer.InitTimer();
                }
                else { autoFollowTimer.Timer.Start(); }
            }
            else if (!autoFollowBox.Checked)
            {
                autoFollowBox.BackColor = Color.Gray;
                if (autoFollowTimer.Timer != null) { autoFollowTimer.Timer.Stop(); }
            }
        }

        // Auto Use Methods
        private void autoUseButt_Click(object sender, EventArgs e) // Auto Use Settings Form
        {
            var set = new AutoUseForm();
            set.StartPosition = FormStartPosition.CenterParent;
            set.ShowDialog(this);
        }
        private void chromeBrowser_GetMousePosOnClick(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                chromeBrowser.ExecuteScriptAsync(@"
                    document.addEventListener('click', function(e) {
                        var parent = e.target.parentElement;
                        CefSharp.PostMessage(''+e.pageX+','+e.pageY);
                    }, false);
                ");
            }
        }
        private void chromeBrowser_SetMousePos(object sender, JavascriptMessageReceivedEventArgs e)
        {
            if (e.Message != null)
            {
                var msg = Convert.ToString(e.Message).Split(',');
                if (autoUseA.Checked && autoUsePosA == default(Point))
                {
                    autoUsePosA.X = Int32.Parse(msg[0]);
                    autoUsePosA.Y = Int32.Parse(msg[1]);
                }
                else if (autoUseB.Checked && autoUsePosB == default(Point))
                {
                    autoUsePosB.X = Int32.Parse(msg[0]);
                    autoUsePosB.Y = Int32.Parse(msg[1]);
                }
                else if (autoUseC.Checked && autoUsePosC == default(Point))
                {
                    autoUsePosC.X = Int32.Parse(msg[0]);
                    autoUsePosC.Y = Int32.Parse(msg[1]);
                }
            }
        }
        public void InitAutoUse(string owner)
        {
            if (owner == "A")
            {
                if (autoUseA.Checked && autoUsePosA != default(Point))
                {
                    chromeBrowser.GetBrowser().GetHost().SendMouseClickEvent(autoUsePosA.X, autoUsePosA.Y, MouseButtonType.Left, false, 1, CefEventFlags.None);
                    Task.Delay(15);
                    chromeBrowser.GetBrowser().GetHost().SendMouseClickEvent(autoUsePosA.X, autoUsePosA.Y, MouseButtonType.Left, true, 1, CefEventFlags.None);
                }
            }
            else if (owner == "B")
            {
                if (autoUseB.Checked && autoUsePosB != default(Point))
                {
                    chromeBrowser.GetBrowser().GetHost().SendMouseClickEvent(autoUsePosB.X, autoUsePosB.Y, MouseButtonType.Left, false, 1, CefEventFlags.None);
                    Task.Delay(15);
                    chromeBrowser.GetBrowser().GetHost().SendMouseClickEvent(autoUsePosB.X, autoUsePosB.Y, MouseButtonType.Left, true, 1, CefEventFlags.None);
                }
            }
            else if (owner == "C")
            {
                if (autoUseC.Checked && autoUsePosC != default(Point))
                {
                    chromeBrowser.GetBrowser().GetHost().SendMouseClickEvent(autoUsePosC.X, autoUsePosC.Y, MouseButtonType.Left, false, 1, CefEventFlags.None);
                    Task.Delay(15);
                    chromeBrowser.GetBrowser().GetHost().SendMouseClickEvent(autoUsePosC.X, autoUsePosC.Y, MouseButtonType.Left, true, 1, CefEventFlags.None);
                }
            }
        }
        private void autoUseA_CheckStateChanged(object sender, EventArgs e)
        {
            if (autoUseA.CheckState == CheckState.Checked)
            {
                autoUseA.BackColor = Color.PeachPuff;
                if (autoUseTimerA.Timer == null)
                {
                    autoUseTimerA.owner = "A";
                    autoUseTimerA.interval = autoUseIntervalA;
                    autoUseTimerA.InitTimer();
                }
                else if (autoUseTimerA.Timer != null && autoUseTimerA.Timer.Enabled)
                {
                    autoUseTimerA.interval = autoUseTimerA.Timer.Interval = autoUseIntervalA;
                }
                else if (autoUseTimerA.Timer != null && !autoUseTimerA.Timer.Enabled)
                {
                    autoUseTimerA.interval = autoUseTimerA.Timer.Interval = autoUseIntervalA;
                    autoUseTimerA.Timer.Start();
                }
            }
            else
            {
                autoUseA.BackColor = Color.Gray;
                autoUsePosA = default(Point);
                if (autoUseTimerA.Timer != null && autoUseTimerA.Timer.Enabled) { autoUseTimerA.Timer.Stop(); }
            }
        }
        private void autoUseB_CheckStateChanged(object sender, EventArgs e)
        {
            if (autoUseB.CheckState == CheckState.Checked)
            {
                autoUseB.BackColor = Color.PeachPuff;
                if (autoUseTimerB.Timer == null)
                {
                    autoUseTimerB.owner = "B";
                    autoUseTimerB.interval = autoUseIntervalB;
                    autoUseTimerB.InitTimer();
                }
                else if (autoUseTimerB.Timer != null && autoUseTimerB.Timer.Enabled)
                {
                    autoUseTimerB.interval = autoUseTimerB.Timer.Interval = autoUseIntervalB;
                }
                else if (autoUseTimerB.Timer != null && !autoUseTimerB.Timer.Enabled)
                {
                    autoUseTimerB.interval = autoUseTimerB.Timer.Interval = autoUseIntervalB;
                    autoUseTimerB.Timer.Start();
                }
            }
            else
            {
                autoUseB.BackColor = Color.Gray;
                autoUsePosB = default(Point);
                if (autoUseTimerB.Timer != null && autoUseTimerB.Timer.Enabled) { autoUseTimerB.Timer.Stop(); }
            }
        }
        private void autoUseC_CheckStateChanged(object sender, EventArgs e)
        {
            if (autoUseC.CheckState == CheckState.Checked)
            {
                autoUseC.BackColor = Color.PeachPuff;
                if (autoUseTimerC.Timer == null)
                {
                    autoUseTimerC.owner = "C";
                    autoUseTimerC.interval = autoUseIntervalC;
                    autoUseTimerC.InitTimer();
                }
                else if (autoUseTimerC.Timer != null && autoUseTimerC.Timer.Enabled)
                {
                    autoUseTimerC.interval = autoUseTimerC.Timer.Interval = autoUseIntervalC;
                }
                else if (autoUseTimerC.Timer != null && !autoUseTimerC.Timer.Enabled)
                {
                    autoUseTimerC.interval = autoUseTimerC.Timer.Interval = autoUseIntervalC;
                    autoUseTimerC.Timer.Start();
                }
            }
            else
            {
                autoUseC.BackColor = Color.Gray;
                autoUsePosC = default(Point);
                if (autoUseTimerC.Timer != null && autoUseTimerC.Timer.Enabled) { autoUseTimerC.Timer.Stop(); }
            }
        }
        internal void SetAutoUseA(int interval)
        {
            if (autoUseTimerA.Timer == null)
            {
                autoUseIntervalA = autoUseTimerA.interval = interval;
            }
            else if (autoUseTimerA.Timer != null)
            {
                autoUseIntervalA = autoUseTimerA.interval = autoUseTimerA.Timer.Interval = interval;
            }
        }
        internal void SetAutoUseB(int interval)
        {
            if (autoUseTimerB.Timer == null)
            {
                autoUseIntervalB = autoUseTimerB.interval = interval;
            }
            else if (autoUseTimerB.Timer != null)
            {
                autoUseIntervalB = autoUseTimerB.interval = autoUseTimerB.Timer.Interval = interval;
            }
        }
        internal void SetAutoUseC(int interval)
        {
            if (autoUseTimerC.Timer == null)
            {
                autoUseIntervalC = autoUseTimerC.interval = interval;
            }
            else if (autoUseTimerC.Timer != null)
            {
                autoUseIntervalC = autoUseTimerC.interval = autoUseTimerC.Timer.Interval = interval;
            }
        }

        // Send Keyboard Keystroke
        public void sendKeyCodeToBrowser(int keyCodeHex)
        {
            KeyEvent k = new KeyEvent();
            k.Modifiers = CefEventFlags.CapsLockOn; // added to allow sending keyboard commands even if selected language is not English
            k.WindowsKeyCode = keyCodeHex;
            k.FocusOnEditableField = false;
            k.IsSystemKey = false;
            k.Type = KeyEventType.KeyDown;
            chromeBrowser.GetBrowser().GetHost().SendKeyEvent(k);
            k.Type = KeyEventType.KeyUp;
            chromeBrowser.GetBrowser().GetHost().SendKeyEvent(k);
        }

        // Updates
        private void CheckForUpdates()
        {
            AutoUpdater.Synchronous = true;
            AutoUpdater.Mandatory = true;
            AutoUpdater.AppTitle = "Hidden Universe WebClient";
            AutoUpdater.Start("https://raw.githubusercontent.com/HiddenUniverse/Hidden-Universe/main/version.xml");
        }
        
        // Keybinds Form
        private void keybindsButt_Click(object sender, EventArgs e)
        {
            var set = new KeybindsForm();
            set.StartPosition = FormStartPosition.CenterParent;
            set.ShowDialog(this);
        }

        // Game Exit
        public void InitWaitForGameExitTimer()
        {
            waitForGameExitTimer = new System.Windows.Forms.Timer();
            waitForGameExitTimer.Tick += new EventHandler(waitForGameExitTimer_Tick);
            waitForGameExitTimer.Interval = 1000; // in miliseconds
            waitForGameExitTimer.Start();
        }
        private void waitForGameExitTimer_Tick(object sender, EventArgs e)
        {
            Thread t = new Thread(WaitForGameExit);
            t.Start();
        }
        public void WaitForGameExit()
        {
            string pat = "Restart the Game";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            if (chromeBrowser.IsBrowserInitialized)
            {
                var task = chromeBrowser.GetTextAsync();
                task.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        Match m = r.Match(response);
                        if (m.Success) { if (assistMode) { SaveManager.Instance.SaveAssistfsConfig(); ; } Application.Exit(); }
                    }
                });
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }
    }
}
