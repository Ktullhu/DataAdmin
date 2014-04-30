using DataNetClient.Properties;
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
            numericUpDown_days.Value = Properties.Settings.Default.DaysToExpiration;
            textBox1.Text = Settings.Default.AdditionalText;
            checkBox_add_text.Checked = Settings.Default.IsAdditionalTextReuired;
        }

        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.valFinish = (int)(nudEndBar.Value);
            Properties.Settings.Default.MaxTickDays = (int) numericUpDown_maxTick.Value;

            Properties.Settings.Default.MaxTimeOutMinutes = (int)numericUpDown1.Value;
            Properties.Settings.Default.MaxTimeOutMinutesStandard = (int)numericUpDown2.Value;
            Properties.Settings.Default.Emails= textBox_emails.Text;            
            Properties.Settings.Default.DaysToExpiration = (int)numericUpDown_days.Value;
            Settings.Default.AdditionalText = textBox1.Text;
            Settings.Default.IsAdditionalTextReuired = checkBox_add_text.Checked;

            Properties.Settings.Default.Save();
        }
    }
}