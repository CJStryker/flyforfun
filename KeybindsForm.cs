using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HiddenUniverse_WebClient
{
    public partial class KeybindsForm : Form
    {
        Keybinds keybinds = new Keybinds();
        string def = @"Click on a keybind to alter (please match game settings)";
        Button alterSource = null;
        public KeybindsForm()
        {
            InitializeComponent();
            InitSettings();
        }
        private void InitSettings()
        {
            dscText.Text = def;
            KeyPreview = true;
        }

        private void actionButt_Click(object sender, EventArgs e)
        {
            string str = "Current Action Key is: " + keybinds.GetKeyName(Keybinds.actionKey) + " . Press a valid key to alter (ESC to cancel).";
            dscText.Text = str;
            alterSource = actionButt;
        }

        private void followButt_Click(object sender, EventArgs e)
        {
            string str = "Current Follow Key is: " + keybinds.GetKeyName(Keybinds.follow) + " . Press a valid key to alter (ESC to cancel).";
            dscText.Text = str;
            alterSource = followButt;
        }

        private void s0Butt_Click(object sender, EventArgs e)
        {
            string str = "Current Slot 0 Key is: " + keybinds.GetKeyName(Keybinds.slot0) + " . Press a valid key to alter (ESC to cancel).";
            dscText.Text = str;
            alterSource = s0Butt;
        }

        private void s1Butt_Click(object sender, EventArgs e)
        {
            string str = "Current Slot 1 Key is: " + keybinds.GetKeyName(Keybinds.slot1) + " . Press a valid key to alter (ESC to cancel).";
            dscText.Text = str;
            alterSource = s1Butt;
        }

        private void s2Butt_Click(object sender, EventArgs e)
        {
            string str = "Current Slot 2 Key is: " + keybinds.GetKeyName(Keybinds.slot2) + " . Press a valid key to alter (ESC to cancel).";
            dscText.Text = str;
            alterSource = s2Butt;
        }

        private void s3Butt_Click(object sender, EventArgs e)
        {
            string str = "Current Slot 3 Key is: " + keybinds.GetKeyName(Keybinds.slot3) + " . Press a valid key to alter (ESC to cancel).";
            dscText.Text = str;
            alterSource = s3Butt;
        }

        private void s4Butt_Click(object sender, EventArgs e)
        {
            string str = "Current Slot 4 Key is: " + keybinds.GetKeyName(Keybinds.slot4) + " . Press a valid key to alter (ESC to cancel).";
            dscText.Text = str;
            alterSource = s4Butt;
        }

        private void s5Butt_Click(object sender, EventArgs e)
        {
            string str = "Current Slot 5 Key is: " + keybinds.GetKeyName(Keybinds.slot5) + " . Press a valid key to alter (ESC to cancel).";
            dscText.Text = str;
            alterSource = s5Butt;
        }

        private void s6Butt_Click(object sender, EventArgs e)
        {
            string str = "Current Slot 6 Key is: " + keybinds.GetKeyName(Keybinds.slot6) + " . Press a valid key to alter (ESC to cancel).";
            dscText.Text = str;
            alterSource = s6Butt;
        }

        private void s7Butt_Click(object sender, EventArgs e)
        {
            string str = "Current Slot 7 Key is: " + keybinds.GetKeyName(Keybinds.slot7) + " . Press a valid key to alter (ESC to cancel).";
            dscText.Text = str;
            alterSource = s7Butt;
        }

        private void s8Butt_Click(object sender, EventArgs e)
        {
            string str = "Current Slot 8 Key is: " + keybinds.GetKeyName(Keybinds.slot8) + " . Press a valid key to alter (ESC to cancel).";
            dscText.Text = str;
            alterSource = s8Butt;
        }

        private void s9Butt_Click(object sender, EventArgs e)
        {
            string str = "Current Slot 9 Key is: " + keybinds.GetKeyName(Keybinds.slot9) + " . Press a valid key to alter (ESC to cancel).";
            dscText.Text = str;
            alterSource = s9Butt;
        }

        private void tb1Butt_Click(object sender, EventArgs e)
        {
            string str = "Current Taskbar 1 Key is: " + keybinds.GetKeyName(Keybinds.taskbar1) + " . Press a valid key to alter (ESC to cancel).";
            dscText.Text = str;
            alterSource = tb1Butt;
        }

        private void tb2Butt_Click(object sender, EventArgs e)
        {
            string str = "Current Taskbar 2 Key is: " + keybinds.GetKeyName(Keybinds.taskbar2) + " . Press a valid key to alter (ESC to cancel).";
            dscText.Text = str;
            alterSource = tb2Butt;
        }

        private void tb3Butt_Click(object sender, EventArgs e)
        {
            string str = "Current Taskbar 3 Key is: " + keybinds.GetKeyName(Keybinds.taskbar3) + " . Press a valid key to alter (ESC to cancel).";
            dscText.Text = str;
            alterSource = tb3Butt;
        }
        private void Settings_KeyDown(object sender, KeyEventArgs e)
        {
            if (alterSource != null)
            {
                var kv = e.KeyValue;
                var kn = keybinds.GetKeyName(kv);
                if (kv == Keybinds.ESC)
                {
                    dscText.Text = def;
                    alterSource = null;
                    return;
                }
                else if (kn != null)
                {
                    SetKeyBind(alterSource, kv);
                    dscText.Text = alterSource.Text + " keybind has been set to " + kn;
                    alterSource = null;
                }
            }
        }

        private void SetKeyBind(Button button, int keycode)
        {
            if (button == actionButt) { Keybinds.actionKey = keycode; }
            else if (button == followButt ) { Keybinds.follow = keycode; }
            else if (button == tb1Butt) { Keybinds.taskbar1 = keycode; }
            else if (button == tb2Butt) { Keybinds.taskbar2 = keycode; }
            else if (button == tb3Butt) { Keybinds.taskbar3 = keycode; }
            else if (button == s0Butt) { Keybinds.slot0 = keycode; }
            else if (button == s1Butt) { Keybinds.slot1 = keycode; }
            else if (button == s2Butt) { Keybinds.slot2 = keycode; }
            else if (button == s3Butt) { Keybinds.slot3 = keycode; }
            else if (button == s4Butt) { Keybinds.slot4 = keycode; }
            else if (button == s5Butt) { Keybinds.slot5 = keycode; }
            else if (button == s6Butt) { Keybinds.slot6 = keycode; }
            else if (button == s7Butt) { Keybinds.slot7 = keycode; }
            else if (button == s8Butt) { Keybinds.slot8 = keycode; }
            else if (button == s9Butt) { Keybinds.slot9 = keycode; }
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveManager.Instance.SaveKeybindsConfig();
        }
    }
}
