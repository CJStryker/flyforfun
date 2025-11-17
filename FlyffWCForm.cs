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
using System.Globalization;

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
        private AutoCombatTimer autoCombatTimer = new AutoCombatTimer();
        private AutoPathTimer autoPathTimer = new AutoPathTimer();
        private AutoUseTimer autoUseTimerA = new AutoUseTimer();
        private AutoUseTimer autoUseTimerB = new AutoUseTimer();
        private AutoUseTimer autoUseTimerC = new AutoUseTimer();
        private const string FlyffGameUrl = "https://universe.flyff.com/play";

        // Configuration Variables
        bool assistMode = false;
        public int autoHealSelectedIndex = -1;
        public int delaybb = 1500;
        public int autoCombatSelectedIndex = -1;
        public int autoPathSelectedIndex = -1;
        public string autoCombatRotationConfig = string.Empty;
        public string autoPathWaypointConfig = string.Empty;
        private readonly List<int> autoCombatKeySequence = new List<int>();
        private readonly List<int> autoPathKeySequence = new List<int>();
        private int autoCombatKeyIndex = 0;
        private int autoPathKeyIndex = 0;
        private bool automationBridgeAvailable = false;
        private bool playerInCombat = false;
        private PointF playerPosition = PointF.Empty;

        // Auto Use Configuration
        internal Point autoUsePosA, autoUsePosB, autoUsePosC;
        internal int autoUseIntervalA = 300000;
        internal int autoUseIntervalB = 300000;
        internal int autoUseIntervalC = 300000;
        public List<string> selectedBuffSlots { get; set; }

        public bool AutomationBridgeAvailable { get { return automationBridgeAvailable; } }
        public bool PlayerInCombat { get { return playerInCombat; } }

        // initiailization
        public FlyffWCForm()
        {
            if (_instance == null) { _instance = this; }
            InitializeComponent();
            PrimeCommandSelectors();
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
        private void PrimeCommandSelectors()
        {
            if (inventoryTaskbarSelector.Items.Count > 0) { inventoryTaskbarSelector.SelectedIndex = 0; }
            if (inventorySlotSelector.Items.Count > 0) { inventorySlotSelector.SelectedIndex = 0; }
            if (attackTaskbarSelector.Items.Count > 0) { attackTaskbarSelector.SelectedIndex = 0; }
            if (attackSlotSelector.Items.Count > 0) { attackSlotSelector.SelectedIndex = 0; }
            if (inventoryQuickList.Items.Count > 0)
            {
                inventoryQuickList.Items[0].Tag = Tuple.Create(0, 1);
            }
            if (inventoryQuickList.Items.Count > 1)
            {
                inventoryQuickList.Items[1].Tag = Tuple.Create(1, 4);
            }
        }
        public void EnableAssistMode()
        {
            assistMode = true;
            assistGroupBox.Enabled = true;
            buffGroupBox.Enabled = true;
            assistGroupBox.Visible = true;
            buffGroupBox.Visible = true;
            autoHealBox.Enabled = true;
            autoHealBox.Visible = true;
            autoBuffBox.Visible = true;
            autoBuffTime.Visible = true;
            autoBuffTree.Visible = true;
            autoBuffTree.Enabled = true;
            autoCombatBox.Visible = true;
            autoCombatBox.Enabled = true;
            autoCombatIntervalBox.Visible = true;
            autoCombatIntervalBox.Enabled = true;
            autoCombatSkills.Visible = true;
            autoCombatSkills.Enabled = true;
            if (autoCombatIntervalBox.SelectedIndex == -1) { autoCombatIntervalBox.SelectedIndex = 1; }
            autoPathBox.Visible = true;
            autoPathBox.Enabled = true;
            autoPathIntervalBox.Visible = true;
            autoPathIntervalBox.Enabled = true;
            autoPathWaypoints.Visible = true;
            autoPathWaypoints.Enabled = true;
            if (autoPathIntervalBox.SelectedIndex == -1) { autoPathIntervalBox.SelectedIndex = 0; }
            EnableAutoFollow();
            UpdateAutomationStatus("Assist mode ready");
        }
        public void ApplyAutomationConfigFromSave()
        {
            if (autoCombatSelectedIndex >= 0 && autoCombatSelectedIndex < autoCombatIntervalBox.Items.Count)
            {
                autoCombatIntervalBox.SelectedIndex = autoCombatSelectedIndex;
            }
            autoCombatSkills.Text = autoCombatRotationConfig ?? string.Empty;
            if (autoPathSelectedIndex >= 0 && autoPathSelectedIndex < autoPathIntervalBox.Items.Count)
            {
                autoPathIntervalBox.SelectedIndex = autoPathSelectedIndex;
            }
            autoPathWaypoints.Text = autoPathWaypointConfig ?? string.Empty;
        }
        public void EnableAutoFollow()
        {
            followGroupBox.Enabled = true;
            followGroupBox.Visible = true;
            autoFollowBox.Visible = true;
            autoFollowBox.Enabled = true;
            keybindsButt.Visible = keybindsButt.Enabled = true;
            SaveManager.Instance.LoadKeybindsConfig();
            UpdateAutomationStatus("Auto follow ready");
        }
        public void EnableAutoUse()
        {
            autoUseGroupBox.Visible = true;
            autoUseGroupBox.Enabled = true;
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
            chromeBrowser.JavascriptMessageReceived += chromeBrowser_HandleJavascriptMessage;
            chromeBrowser.FrameLoadEnd += chromeBrowser_InjectAutomationBridge;
            chromeBrowser.BringToFront();
            if (autoUseTB.Enabled) {
                chromeBrowser.FrameLoadEnd += chromeBrowser_GetMousePosOnClick;
            }
            UpdateAutomationStatus("Client ready");
        }
        private void reloadGameButton_Click(object sender, EventArgs e)
        {
            if (chromeBrowser != null && chromeBrowser.IsBrowserInitialized)
            {
                chromeBrowser.Load(FlyffGameUrl);
                UpdateAutomationStatus("Reloading client...");
            }
            else
            {
                InitializeChromium();
                UpdateAutomationStatus("Launching client...");
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
                UpdateAutomationStatus("Auto Heal running");
            }
            else { autoHealBox.BackColor = Color.Gray;
                autoHealTime.Enabled = false;
                autoHealTime.Visible = false;
                autoHealTime.BackColor = Color.Gray;
                autoHealerTimer.Timer.Stop();
                UpdateAutomationStatus("Auto Heal paused");
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
                UpdateAutomationStatus($"Auto Heal interval set to {interval}s");
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
            UpdateAutomationStatus("Auto Buff rotation initiated");
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
                UpdateAutomationStatus($"Auto Buff timer set to every {interval} minutes");
            }
            else
            {
                if (autoBuffTimer.Timer != null && autoBuffTimer.Timer.Enabled) { autoBuffTimer.Timer.Stop(); autoBuffTimer.CDTimer.Stop();  }
                autoBuffCD.Visible = false;
                UpdateAutomationStatus("Manual buff mode active");
            }
        }
        public void autoBuffTreeCheckItem (string[] config)
        {
            for (int i = 0; i < config.Length; i++) // check all saved items
            {
                if (string.IsNullOrWhiteSpace(config[i]) || !config[i].Contains("x")) { continue; }
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

        // Auto Combat Methods
        private void autoCombatBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (autoCombatBox.Checked)
            {
                autoCombatBox.BackColor = Color.PeachPuff;
                if (autoCombatIntervalBox.SelectedIndex == -1) { autoCombatIntervalBox.SelectedIndex = 1; }
                autoCombatKeyIndex = 0;
                if (autoCombatTimer.Timer == null)
                {
                    autoCombatTimer.InitTimer();
                }
                else
                {
                    autoCombatTimer.Timer.Interval = autoCombatTimer.Interval;
                    autoCombatTimer.Timer.Start();
                }
            }
            else
            {
                autoCombatBox.BackColor = Color.Gray;
                if (autoCombatTimer.Timer != null) { autoCombatTimer.Timer.Stop(); }
            }
        }
        private void autoCombatIntervalBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            autoCombatSelectedIndex = autoCombatIntervalBox.SelectedIndex;
            int interval = ParseIntervalFromText(autoCombatIntervalBox.GetItemText(autoCombatIntervalBox.SelectedItem));
            autoCombatTimer.Interval = interval;
            if (autoCombatTimer.Timer != null)
            {
                autoCombatTimer.Timer.Interval = interval;
                if (autoCombatBox.Checked && !autoCombatTimer.Timer.Enabled)
                {
                    autoCombatTimer.Timer.Start();
                }
            }
            else if (autoCombatBox.Checked)
            {
                autoCombatTimer.InitTimer();
            }
        }
        private void autoCombatSkills_TextChanged(object sender, EventArgs e)
        {
            autoCombatRotationConfig = autoCombatSkills.Text;
            autoCombatKeySequence.Clear();
            autoCombatKeySequence.AddRange(BuildKeySequence(autoCombatRotationConfig));
            autoCombatKeyIndex = 0;
        }
        public void ExecuteCombatRotation()
        {
            if (!autoCombatBox.Checked || autoCombatKeySequence.Count == 0) { return; }
            if (autoCombatKeyIndex >= autoCombatKeySequence.Count) { autoCombatKeyIndex = 0; }
            sendKeyCodeToBrowser(autoCombatKeySequence[autoCombatKeyIndex]);
            autoCombatKeyIndex = (autoCombatKeyIndex + 1) % autoCombatKeySequence.Count;
        }

        // Auto Path Methods
        private void autoPathBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (autoPathBox.Checked)
            {
                autoPathBox.BackColor = Color.PeachPuff;
                if (autoPathIntervalBox.SelectedIndex == -1) { autoPathIntervalBox.SelectedIndex = 0; }
                autoPathKeyIndex = 0;
                autoPathTimer.LastPosition = PointF.Empty;
                if (autoPathTimer.Timer == null)
                {
                    autoPathTimer.InitTimer();
                }
                else
                {
                    autoPathTimer.Timer.Interval = autoPathTimer.Interval;
                    autoPathTimer.Timer.Start();
                }
            }
            else
            {
                autoPathBox.BackColor = Color.Gray;
                autoPathTimer.LastPosition = PointF.Empty;
                if (autoPathTimer.Timer != null) { autoPathTimer.Timer.Stop(); }
            }
        }
        private void autoPathIntervalBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            autoPathSelectedIndex = autoPathIntervalBox.SelectedIndex;
            int interval = ParseIntervalFromText(autoPathIntervalBox.GetItemText(autoPathIntervalBox.SelectedItem));
            autoPathTimer.Interval = interval;
            if (autoPathTimer.Timer != null)
            {
                autoPathTimer.Timer.Interval = interval;
                if (autoPathBox.Checked && !autoPathTimer.Timer.Enabled)
                {
                    autoPathTimer.Timer.Start();
                }
            }
            else if (autoPathBox.Checked)
            {
                autoPathTimer.InitTimer();
            }
        }
        private void autoPathWaypoints_TextChanged(object sender, EventArgs e)
        {
            autoPathWaypointConfig = autoPathWaypoints.Text;
            autoPathKeySequence.Clear();
            autoPathKeySequence.AddRange(BuildKeySequence(autoPathWaypointConfig));
            autoPathKeyIndex = 0;
        }
        public void ExecutePathingStep(AutoPathTimer pathTimer)
        {
            if (!autoPathBox.Checked || autoPathKeySequence.Count == 0) { return; }
            if (autoPathKeyIndex >= autoPathKeySequence.Count) { autoPathKeyIndex = 0; }
            bool usePosition = pathTimer != null && pathTimer.UsePositionState && automationBridgeAvailable;
            if (usePosition)
            {
                if (playerPosition == PointF.Empty) { return; }
                if (pathTimer.LastPosition != PointF.Empty)
                {
                    if (Distance(playerPosition, pathTimer.LastPosition) > 2f)
                    {
                        pathTimer.LastPosition = playerPosition;
                        return;
                    }
                }
            }
            sendKeyCodeToBrowser(autoPathKeySequence[autoPathKeyIndex]);
            autoPathKeyIndex = (autoPathKeyIndex + 1) % autoPathKeySequence.Count;
            if (usePosition)
            {
                pathTimer.LastPosition = playerPosition;
            }
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
                UpdateAutomationStatus("Auto Follow active");
            }
            else if (!autoFollowBox.Checked)
            {
                autoFollowBox.BackColor = Color.Gray;
                if (autoFollowTimer.Timer != null) { autoFollowTimer.Timer.Stop(); }
                UpdateAutomationStatus("Auto Follow paused");
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

        private void chromeBrowser_InjectAutomationBridge(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                chromeBrowser.ExecuteScriptAsync(@"
                    (function(){
                        if(window.__huAutomationBridge){return;}
                        window.__huAutomationBridge = true;
                        const reportState = function(){
                            try {
                                var player = (window.gameClient && window.gameClient.player) ? window.gameClient.player : null;
                                if(player && player.position){
                                    CefSharp.PostMessage('position:' + player.position.x + ',' + player.position.y);
                                }
                                if(player && typeof player.isInCombat !== 'undefined'){
                                    CefSharp.PostMessage('combat:' + player.isInCombat);
                                }
                            } catch (err) {}
                        };
                        setInterval(reportState, 1000);
                    })();
                ");
            }
        }
        private void chromeBrowser_HandleJavascriptMessage(object sender, JavascriptMessageReceivedEventArgs e)
        {
            if (e.Message == null) { return; }
            var payload = Convert.ToString(e.Message);
            if (string.IsNullOrWhiteSpace(payload)) { return; }
            if (payload.StartsWith("position:", StringComparison.OrdinalIgnoreCase))
            {
                HandlePositionPayload(payload.Substring("position:".Length));
                return;
            }
            if (payload.StartsWith("combat:", StringComparison.OrdinalIgnoreCase))
            {
                HandleCombatPayload(payload.Substring("combat:".Length));
                return;
            }
            if (payload.Contains(","))
            {
                var msg = payload.Split(',');
                if (msg.Length < 2) { return; }
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

        private void HandlePositionPayload(string payload)
        {
            var msg = payload.Split(',');
            if (msg.Length < 2) { return; }
            float x, y;
            if (float.TryParse(msg[0], NumberStyles.Float, CultureInfo.InvariantCulture, out x) &&
                float.TryParse(msg[1], NumberStyles.Float, CultureInfo.InvariantCulture, out y))
            {
                playerPosition = new PointF(x, y);
                automationBridgeAvailable = true;
            }
        }

        private void HandleCombatPayload(string payload)
        {
            bool combatState;
            if (bool.TryParse(payload, out combatState))
            {
                playerInCombat = combatState;
                automationBridgeAvailable = true;
            }
        }
        public async Task InitAutoUseAsync(string owner)
        {
            var host = chromeBrowser?.GetBrowser()?.GetHost();
            if (host == null)
            {
                return;
            }

            if (!TryGetAutoUseTarget(owner, out var target))
            {
                return;
            }

            host.SendMouseClickEvent(target.X, target.Y, MouseButtonType.Left, false, 1, CefEventFlags.None);
            await Task.Delay(15).ConfigureAwait(true);
            host.SendMouseClickEvent(target.X, target.Y, MouseButtonType.Left, true, 1, CefEventFlags.None);
        }

        private bool TryGetAutoUseTarget(string owner, out Point target)
        {
            target = default(Point);
            if (string.Equals(owner, "A", StringComparison.OrdinalIgnoreCase))
            {
                if (autoUseA.Checked && autoUsePosA != default(Point))
                {
                    target = autoUsePosA;
                    return true;
                }
            }
            else if (string.Equals(owner, "B", StringComparison.OrdinalIgnoreCase))
            {
                if (autoUseB.Checked && autoUsePosB != default(Point))
                {
                    target = autoUsePosB;
                    return true;
                }
            }
            else if (string.Equals(owner, "C", StringComparison.OrdinalIgnoreCase))
            {
                if (autoUseC.Checked && autoUsePosC != default(Point))
                {
                    target = autoUsePosC;
                    return true;
                }
            }
            return false;
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
                UpdateAutomationStatus("Auto Use A active");
            }
            else
            {
                autoUseA.BackColor = Color.Gray;
                autoUsePosA = default(Point);
                if (autoUseTimerA.Timer != null && autoUseTimerA.Timer.Enabled) { autoUseTimerA.Timer.Stop(); }
                UpdateAutomationStatus("Auto Use A paused");
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
                UpdateAutomationStatus("Auto Use B active");
            }
            else
            {
                autoUseB.BackColor = Color.Gray;
                autoUsePosB = default(Point);
                if (autoUseTimerB.Timer != null && autoUseTimerB.Timer.Enabled) { autoUseTimerB.Timer.Stop(); }
                UpdateAutomationStatus("Auto Use B paused");
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
                UpdateAutomationStatus("Auto Use C active");
            }
            else
            {
                autoUseC.BackColor = Color.Gray;
                autoUsePosC = default(Point);
                if (autoUseTimerC.Timer != null && autoUseTimerC.Timer.Enabled) { autoUseTimerC.Timer.Stop(); }
                UpdateAutomationStatus("Auto Use C paused");
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

        private void useSelectedItemButton_Click(object sender, EventArgs e)
        {
            ActivateSelectedSlot(inventoryTaskbarSelector, inventorySlotSelector);
        }

        private void triggerAttackSlotButton_Click(object sender, EventArgs e)
        {
            ActivateSelectedSlot(attackTaskbarSelector, attackSlotSelector);
        }

        private void basicAttackButton_Click(object sender, EventArgs e)
        {
            sendKeyCodeToBrowser(Keybinds.actionKey);
        }

        private async void burstAttackButton_Click(object sender, EventArgs e)
        {
            await ExecuteAttackBurstAsync();
        }

        private void inventoryQuickList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (inventoryQuickList.SelectedItems.Count == 0) { return; }
            if (inventoryQuickList.SelectedItems[0].Tag is Tuple<int, int> mapping)
            {
                inventoryTaskbarSelector.SelectedIndex = mapping.Item1;
                inventorySlotSelector.SelectedIndex = mapping.Item2;
            }
        }

        private void ActivateSelectedSlot(ComboBox taskbarSelector, ComboBox slotSelector)
        {
            if (taskbarSelector.SelectedIndex < 0 || slotSelector.SelectedIndex < 0) { return; }
            ActivateActionSlot(taskbarSelector.SelectedIndex, slotSelector.SelectedIndex);
        }

        private void ActivateActionSlot(int taskbarIndex, int slotIndex)
        {
            var taskbars = Keybinds.GetTaskbars();
            var slots = Keybinds.GetSlots();
            if (taskbarIndex < 0 || taskbarIndex >= taskbars.Length) { return; }
            if (slotIndex < 0 || slotIndex >= slots.Length) { return; }
            sendKeyCodeToBrowser(taskbars[taskbarIndex]);
            sendKeyCodeToBrowser(slots[slotIndex]);
        }

        private async Task ExecuteAttackBurstAsync()
        {
            var combo = new (int taskbar, int slot)[] { (0, 0), (0, 1), (0, 2) };
            foreach (var step in combo)
            {
                ActivateActionSlot(step.taskbar, step.slot);
                await Task.Delay(150);
            }
        }

        // Send Keyboard Keystroke
        public void sendKeyCodeToBrowser(int keyCodeHex)
        {
            if (chromeBrowser?.IsBrowserInitialized != true)
            {
                return;
            }

            var host = chromeBrowser.GetBrowser()?.GetHost();
            if (host == null)
            {
                return;
            }

            KeyEvent k = new KeyEvent();
            k.Modifiers = CefEventFlags.CapsLockOn; // added to allow sending keyboard commands even if selected language is not English
            k.WindowsKeyCode = keyCodeHex;
            k.FocusOnEditableField = false;
            k.IsSystemKey = false;
            k.Type = KeyEventType.KeyDown;
            host.SendKeyEvent(k);
            k.Type = KeyEventType.KeyUp;
            host.SendKeyEvent(k);
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
        private void UpdateAutomationStatus(string statusMessage)
        {
            if (automationStatusLabel == null) { return; }
            Action updateAction = () => automationStatusLabel.Text = $"Automation status: {statusMessage}";
            if (automationStatusLabel.InvokeRequired)
            {
                automationStatusLabel.Invoke(updateAction);
            }
            else
            {
                updateAction();
            }
        }

        private List<int> BuildKeySequence(string config)
        {
            var sequence = new List<int>();
            if (string.IsNullOrWhiteSpace(config)) { return sequence; }
            var tokens = config.Split(new[] { ',', '\n', '\r', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var token in tokens)
            {
                if (TryResolveKeyCode(token.Trim(), out var keyCode))
                {
                    sequence.Add(keyCode);
                }
            }
            return sequence;
        }

        private bool TryResolveKeyCode(string token, out int keyCode)
        {
            keyCode = 0;
            if (string.IsNullOrWhiteSpace(token)) { return false; }
            var normalized = token.Trim().ToUpperInvariant();

            if (normalized.Length == 1 && char.IsDigit(normalized[0]))
            {
                var slots = Keybinds.GetSlots();
                int index = normalized[0] - '0';
                if (index >= 0 && index < slots.Length)
                {
                    keyCode = slots[index];
                    return true;
                }
            }

            if (normalized.StartsWith("SLOT") && int.TryParse(normalized.Substring(4), out var slotIndex))
            {
                var slots = Keybinds.GetSlots();
                if (slotIndex >= 0 && slotIndex < slots.Length)
                {
                    keyCode = slots[slotIndex];
                    return true;
                }
            }

            if (normalized.StartsWith("TASKBAR") && int.TryParse(normalized.Substring(7), out var taskbarIndex))
            {
                var taskbars = Keybinds.GetTaskbars();
                if (taskbarIndex > 0 && taskbarIndex <= taskbars.Length)
                {
                    keyCode = taskbars[taskbarIndex - 1];
                    return true;
                }
            }

            if (normalized == "ACTION" || normalized == "ACTIONKEY")
            {
                keyCode = Keybinds.actionKey;
                return true;
            }

            if (normalized == "FOLLOW")
            {
                keyCode = Keybinds.follow;
                return true;
            }

            if (Keybinds.keyValuePairs.TryGetValue(normalized, out var mappedValue))
            {
                keyCode = mappedValue;
                return true;
            }

            if (Enum.TryParse(token, true, out Keys parsedKey))
            {
                keyCode = (int)parsedKey;
                return true;
            }
            return false;
        }

        private int ParseIntervalFromText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) { return 1000; }
            var match = Regex.Match(text, "([0-9]+(?:\\.[0-9]+)?)", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return 1000;
            }

            double value;
            if (!double.TryParse(match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                return 1000;
            }

            int multiplier = 1000;
            if (text.IndexOf("minute", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                multiplier = 60000;
            }
            else if (text.IndexOf("millisecond", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("ms", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                multiplier = 1;
            }

            var interval = (int)Math.Max(1, Math.Round(value * multiplier));
            return interval;
        }

        private static float Distance(PointF a, PointF b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return (float)Math.Sqrt((dx * dx) + (dy * dy));
        }
    }
}
