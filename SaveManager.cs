using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace HiddenUniverse_WebClient
{
    class SaveManager
    {
        private static SaveManager _instance;
        FlyffWCForm mainForm = FlyffWCForm.Instance;

        private SaveManager()
        {

        }
        public static SaveManager Instance { get { if (_instance == null) { _instance = new SaveManager(); } return _instance; } }
        public void SaveAssistfsConfig()
        {
            int autoHealSelectedIndex = mainForm.autoHealSelectedIndex;
            List<string> buffTree = mainForm.selectedBuffSlots;
            List<string> config = new List<string>();
            int configItems = buffTree.Count + 1;
            config.Add(autoHealSelectedIndex.ToString());
            foreach (string b in buffTree) { config.Add(b); }
            File.WriteAllLines(ArgumentManager.assistfsConfigPath, config);
        }

        public void LoadAssistfsConfig()
        {
            if (File.Exists(ArgumentManager.assistfsConfigPath))
            {
                string[] config = File.ReadAllLines(ArgumentManager.assistfsConfigPath);
                int autoHealSelectedIndex = Int32.Parse(config[0]);
                mainForm.autoHealSelectedIndex = autoHealSelectedIndex;
                mainForm.autoBuffTreeCheckItem(config); 

            }
        }
        public void LoadKeybindsConfig()
        {
            if (File.Exists(ArgumentManager.keybindsConfigPath))
            {
                string[] config = File.ReadAllLines(ArgumentManager.keybindsConfigPath);
                List<int> keycodes = new List<int>();
                foreach (string str in config)
                {
                    int p;
                    if (Int32.TryParse(str, out p))
                    {
                        keycodes.Add(p);
                    }
                }
                if (keycodes.Count == Keybinds.GetKeybinds().Length)
                {
                    Keybinds.SetKeyBinds(keycodes);
                }
            }
        }
        public void SaveKeybindsConfig()
        {
            var kb = Keybinds.GetKeybinds();
            List<string> config = new List<string>();
            foreach (var k in kb) { config.Add(k.ToString()); }
            File.WriteAllLines(ArgumentManager.keybindsConfigPath, config);
        }
    }
}
