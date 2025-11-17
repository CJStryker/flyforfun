using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Text;

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
            config.Add(autoHealSelectedIndex.ToString());
            config.Add(mainForm.autoCombatSelectedIndex.ToString());
            config.Add(mainForm.autoPathSelectedIndex.ToString());
            config.Add(Convert.ToBase64String(Encoding.UTF8.GetBytes(mainForm.autoCombatRotationConfig ?? string.Empty)));
            config.Add(Convert.ToBase64String(Encoding.UTF8.GetBytes(mainForm.autoPathWaypointConfig ?? string.Empty)));
            config.Add("__BUFFS__");
            foreach (string b in buffTree) { config.Add(b); }
            File.WriteAllLines(ArgumentManager.assistfsConfigPath, config);
        }

        public void LoadAssistfsConfig()
        {
            if (File.Exists(ArgumentManager.assistfsConfigPath))
            {
                string[] config = File.ReadAllLines(ArgumentManager.assistfsConfigPath);
                if (config.Length > 0)
                {
                    int autoHealSelectedIndex = Int32.Parse(config[0]);
                    mainForm.autoHealSelectedIndex = autoHealSelectedIndex;
                }

                int configIndex = 1;
                int buffMarkerIndex = Array.IndexOf(config, "__BUFFS__");
                if (buffMarkerIndex != -1)
                {
                    if (configIndex < buffMarkerIndex)
                    {
                        int combatIndex;
                        if (Int32.TryParse(config[configIndex], out combatIndex)) { mainForm.autoCombatSelectedIndex = combatIndex; }
                        else { mainForm.autoCombatSelectedIndex = -1; }
                        configIndex++;
                    }
                    if (configIndex < buffMarkerIndex)
                    {
                        int pathIndex;
                        if (Int32.TryParse(config[configIndex], out pathIndex)) { mainForm.autoPathSelectedIndex = pathIndex; }
                        else { mainForm.autoPathSelectedIndex = -1; }
                        configIndex++;
                    }
                    if (configIndex < buffMarkerIndex)
                    {
                        mainForm.autoCombatRotationConfig = DecodeConfigString(config[configIndex]);
                        configIndex++;
                    }
                    if (configIndex < buffMarkerIndex)
                    {
                        mainForm.autoPathWaypointConfig = DecodeConfigString(config[configIndex]);
                        configIndex++;
                    }
                    configIndex = buffMarkerIndex + 1;
                }
                else
                {
                    configIndex = 1;
                }
                if (config.Length > configIndex)
                {
                    string[] buffConfig = new string[config.Length - configIndex];
                    Array.Copy(config, configIndex, buffConfig, 0, buffConfig.Length);
                    mainForm.autoBuffTreeCheckItem(buffConfig);
                }
                mainForm.ApplyAutomationConfigFromSave();

            }
        }

        private string DecodeConfigString(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) { return string.Empty; }
            try
            {
                var data = Convert.FromBase64String(value);
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
                return value;
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
