using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HiddenUniverse_WebClient
{
    public partial class AutoUseForm : Form
    {
        public AutoUseForm()
        {
            InitializeComponent();
            InitSelection();
        }

        private void InitSelection()
        {
            int intervalA = FlyffWCForm.Instance.autoUseIntervalA;
            int intervalB = FlyffWCForm.Instance.autoUseIntervalB;
            int intervalC = FlyffWCForm.Instance.autoUseIntervalC;
            var items = autoUseATime.Items;
            for (int i = 0; i < items.Count; i++)
            {
                GroupCollection gc = RegexCheck.Test(autoUseATime.GetItemText(items[i]), "Every ([0-9]{1,2}) seconds");
                if (gc != null)
                {
                    var interval = Int32.Parse(gc[1].Value) * 1000;
                    if (interval == intervalA) { autoUseATime.SelectedIndex = i; }
                    if (interval == intervalB) { autoUseBTime.SelectedIndex = i; }
                    if (interval == intervalC) { autoUseCTime.SelectedIndex = i; }
                }
                else
                {
                    gc = RegexCheck.Test(autoUseATime.GetItemText(items[i]), "Every ([0-9]{1,2}) minutes");
                    if (gc != null)
                    {
                        var interval = Int32.Parse(gc[1].Value) * 60000;
                        if (interval == intervalA) { autoUseATime.SelectedIndex = i; }
                        if (interval == intervalB) { autoUseBTime.SelectedIndex = i; }
                        if (interval == intervalC) { autoUseCTime.SelectedIndex = i; }
                    }
                }
            }
        }

        private void autoUseATime_SelectedIndexChanged(object sender, EventArgs e)
        {
            GroupCollection gc = RegexCheck.Test(autoUseATime.GetItemText(autoUseATime.SelectedItem), "Every ([0-9]{1,2}) seconds");
            if (gc != null)
            {
                var interval = Int32.Parse(gc[1].Value);
                FlyffWCForm.Instance.SetAutoUseA(interval * 1000);
            }
            else
            {
                gc = RegexCheck.Test(autoUseATime.GetItemText(autoUseATime.SelectedItem), "Every ([0-9]{1,2}) minutes");
                if (gc != null)
                {
                    var interval = Int32.Parse(gc[1].Value);
                    FlyffWCForm.Instance.SetAutoUseA(interval * 60000);
                }
            }
        }

        private void autoUseBTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            GroupCollection gc = RegexCheck.Test(autoUseBTime.GetItemText(autoUseBTime.SelectedItem), "Every ([0-9]{1,2}) seconds");
            if (gc != null)
            {
                var interval = Int32.Parse(gc[1].Value);
                FlyffWCForm.Instance.SetAutoUseB(interval * 1000);
            }
            else
            {
                gc = RegexCheck.Test(autoUseBTime.GetItemText(autoUseBTime.SelectedItem), "Every ([0-9]{1,2}) minutes");
                if (gc != null)
                {
                    var interval = Int32.Parse(gc[1].Value);
                    FlyffWCForm.Instance.SetAutoUseB(interval * 60000);
                }
            }
        }

        private void autoUseCTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            GroupCollection gc = RegexCheck.Test(autoUseCTime.GetItemText(autoUseCTime.SelectedItem), "Every ([0-9]{1,2}) seconds");
            if (gc != null)
            {
                var interval = Int32.Parse(gc[1].Value);
                FlyffWCForm.Instance.SetAutoUseC(interval * 1000);
            }
            else
            {
                gc = RegexCheck.Test(autoUseCTime.GetItemText(autoUseCTime.SelectedItem), "Every ([0-9]{1,2}) minutes");
                if (gc != null)
                {
                    var interval = Int32.Parse(gc[1].Value);
                    FlyffWCForm.Instance.SetAutoUseC(interval * 60000);
                }
            }
        }
    }
}
