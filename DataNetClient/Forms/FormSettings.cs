using System;
using System.Windows.Forms;

namespace DataNetClient.Forms
{
    public partial class FormSettings : DevComponents.DotNetBar.Metro.MetroForm
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            numericUpDown_maxTick.Value = Properties.Settings.Default.MaxTickDays;
            nudEndBar.Value = Properties.Settings.Default.valFinish;
            numericUpDown1.Value = Properties.Settings.Default.MaxTimeOutMinutes;
            numericUpDown2.Value = Properties.Settings.Default.MaxTimeOutMinutesStandard;
            textBox_emails.Text = Properties.Settings.Default.Emails;
        }

        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.valFinish = (int)(nudEndBar.Value);
            Properties.Settings.Default.MaxTickDays = (int) numericUpDown_maxTick.Value;

            Properties.Settings.Default.MaxTimeOutMinutes = (int)numericUpDown1.Value;
            Properties.Settings.Default.MaxTimeOutMinutesStandard = (int)numericUpDown2.Value;
            Properties.Settings.Default.Emails= textBox_emails.Text;

            Properties.Settings.Default.Save();
        }
    }
}