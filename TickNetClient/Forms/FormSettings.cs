using System;
using System.Windows.Forms;

namespace TickNetClient.Forms
{
    public partial class FormSettings : DevComponents.DotNetBar.Metro.MetroForm
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            
        }

        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {            
            Properties.Settings.Default.Save();
        }
    }
}