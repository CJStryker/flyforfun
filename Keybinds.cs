using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HiddenUniverse_WebClient
{
    internal class Keybinds
    {
        internal static Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();
        internal static int actionKey { get; set; } = ((int)Keys.C);
        internal static int taskbar1 { get; set; } = ((int)Keys.F1);
        internal static int taskbar2 { get; set; } = ((int)Keys.F2);
        internal static int taskbar3 { get; set; } = ((int)Keys.F3);
        internal static int slot0 { get; set; } = ((int)Keys.D0);
        internal static int slot1 { get; set; } = ((int)Keys.D1);
        internal static int slot2 { get; set; } = ((int)Keys.D2);
        internal static int slot3 { get; set; } = ((int)Keys.D3);
        internal static int slot4 { get; set; } = ((int)Keys.D4);
        internal static int slot5 { get; set; } = ((int)Keys.D5);
        internal static int slot6 { get; set; } = ((int)Keys.D6);
        internal static int slot7 { get; set; } = ((int)Keys.D7);
        internal static int slot8 { get; set; } = ((int)Keys.D8);
        internal static int slot9 { get; set; } = ((int)Keys.D9);
        internal static int follow { get; set; } = ((int)Keys.Z);
        internal const int ESC = ((int)Keys.Escape);

        static Keybinds()
        {
            InitDictionary();
        }

        public Keybinds()
        {
        }

        private static void InitDictionary()
        {
            if (keyValuePairs.Count > 0) { return; }
            keyValuePairs.Add("TAB", 0x09);
            keyValuePairs.Add("SHIFT", 0x10);
            keyValuePairs.Add("CTRL", 0x11);
            keyValuePairs.Add("ALT", 0x12);
            keyValuePairs.Add("CAPSLOCK", 0x14);
            keyValuePairs.Add("ESC", 0x1B);
            keyValuePairs.Add("SPACE", 0x20);
            keyValuePairs.Add("ENTER", 0x0D);
            keyValuePairs.Add("BACKSPACE", 0x08);
            keyValuePairs.Add("PAGEUP", 0x21);
            keyValuePairs.Add("PAGEDOWN", 0x22);
            keyValuePairs.Add("END", 0x23);
            keyValuePairs.Add("HOME", 0x24);
            keyValuePairs.Add("LEFT", 0x25);
            keyValuePairs.Add("UP", 0x26);
            keyValuePairs.Add("RIGHT", 0x27);
            keyValuePairs.Add("DOWN", 0x28);
            keyValuePairs.Add("SELECT", 0x29);
            keyValuePairs.Add("PRINT", 0x2A);
            keyValuePairs.Add("INSERT", 0x2D);
            keyValuePairs.Add("0", 0x30);
            keyValuePairs.Add("1", 0x31);
            keyValuePairs.Add("2", 0x32);
            keyValuePairs.Add("3", 0x33);
            keyValuePairs.Add("4", 0x34);
            keyValuePairs.Add("5", 0x35);
            keyValuePairs.Add("6", 0x36);
            keyValuePairs.Add("7", 0x37);
            keyValuePairs.Add("8", 0x38);
            keyValuePairs.Add("9", 0x39);
            keyValuePairs.Add("A", 0x41);
            keyValuePairs.Add("B", 0x42);
            keyValuePairs.Add("C", 0x43);
            keyValuePairs.Add("D", 0x44);
            keyValuePairs.Add("E", 0x45);
            keyValuePairs.Add("F", 0x46);
            keyValuePairs.Add("G", 0x47);
            keyValuePairs.Add("H", 0x48);
            keyValuePairs.Add("I", 0x49);
            keyValuePairs.Add("J", 0x4A);
            keyValuePairs.Add("K", 0x4B);
            keyValuePairs.Add("L", 0x4C);
            keyValuePairs.Add("M", 0x4D);
            keyValuePairs.Add("N", 0x4E);
            keyValuePairs.Add("O", 0x4F);
            keyValuePairs.Add("P", 0x50);
            keyValuePairs.Add("Q", 0x51);
            keyValuePairs.Add("R", 0x52);
            keyValuePairs.Add("S", 0x53);
            keyValuePairs.Add("T", 0x54);
            keyValuePairs.Add("U", 0x55);
            keyValuePairs.Add("V", 0x56);
            keyValuePairs.Add("W", 0x57);
            keyValuePairs.Add("X", 0x58);
            keyValuePairs.Add("Y", 0x59);
            keyValuePairs.Add("Z", 0x5A);
            keyValuePairs.Add("NUM0", 0x60);
            keyValuePairs.Add("NUM1", 0x61);
            keyValuePairs.Add("NUM2", 0x62);
            keyValuePairs.Add("NUM3", 0x63);
            keyValuePairs.Add("NUM4", 0x64);
            keyValuePairs.Add("NUM5", 0x65);
            keyValuePairs.Add("NUM6", 0x66);
            keyValuePairs.Add("NUM7", 0x67);
            keyValuePairs.Add("NUM8", 0x68);
            keyValuePairs.Add("NUM9", 0x69);
            keyValuePairs.Add("F1", 0x70);
            keyValuePairs.Add("F2", 0x71);
            keyValuePairs.Add("F3", 0x72);
            keyValuePairs.Add("F4", 0x73);
            keyValuePairs.Add("F5", 0x74);
            keyValuePairs.Add("F6", 0x75);
            keyValuePairs.Add("F7", 0x76);
            keyValuePairs.Add("F8", 0x77);
            keyValuePairs.Add("F9", 0x78);
            keyValuePairs.Add("F10", 0x79);
            keyValuePairs.Add("F11", 0x7A);
            keyValuePairs.Add("F12", 0x7B);
            keyValuePairs.Add("SCRL", 0x91);
            keyValuePairs.Add("LSHIFT", 0xA0);
            keyValuePairs.Add("RSHIFT", 0xA1);
            keyValuePairs.Add("LCTRL", 0xA2);
            keyValuePairs.Add("RCTRL", 0xA3);
            keyValuePairs.Add("LALT", 0xA4);
            keyValuePairs.Add("RALT", 0xA5);
        }
        internal static int[] GetTaskbars()
        {
            return new int[] { taskbar1, taskbar2, taskbar3 };
        }
        internal static int[] GetSlots()
        {
            return new int[] { slot0, slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9 };
        }
        internal static int[] GetKeybinds()
        {
            return new int[] { actionKey, taskbar1, taskbar2, taskbar3, slot0, slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9, follow };
        }
        internal static void SetKeyBinds (List<int> keycodes)
        {
            actionKey = keycodes[0];
            taskbar1 = keycodes[1];
            taskbar2 = keycodes[2];
            taskbar3 = keycodes[3];
            slot0 = keycodes[4];
            slot1 = keycodes[5];
            slot2 = keycodes[6];
            slot3 = keycodes[7];
            slot4 = keycodes[8];
            slot5 = keycodes[9];
            slot6 = keycodes[10];
            slot7 = keycodes[11];
            slot8 = keycodes[12];
            slot9 = keycodes[13];
            follow = keycodes[14];
        }
        internal string GetKeyName(int i)
        {
            return keyValuePairs.FirstOrDefault(x => x.Value == i).Key;
        }
        internal int GetKeyID(string str)
        {
            return keyValuePairs.FirstOrDefault(x => x.Key == str).Value;
        }
    }
}
