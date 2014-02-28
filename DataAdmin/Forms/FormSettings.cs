using DataAdmin.Properties;

namespace DataAdmin.Forms
{
    public partial class FormSettings : DevComponents.DotNetBar.Metro.MetroForm
    {
        public FormSettings()
        {
            InitializeComponent();
            numericUpDown_maxTick.Value = Settings.Default.MaxHistoryLooksBackDays;
        }

        private void FormSettings_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Settings.Default.MaxHistoryLooksBackDays = (int)numericUpDown_maxTick.Value;
        }
    }
}