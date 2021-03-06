using System.Drawing;
using System.Windows.Forms;
using DataNetClient.Core;
using DataNetClient.Properties;

namespace DataNetClient.Forms
{
    public partial class StartControl : DevComponents.DotNetBar.Controls.SlidePanel
    {
        public StartControl()
        {
            InitializeComponent();                   
        }


        private MetroBillCommands _commands;
        /// <summary>
        /// Gets or sets the commands associated with the control.
        /// </summary>
        public MetroBillCommands Commands
        {
            get { return _commands; }
            set
            {
                if (value != _commands)
                {
                    MetroBillCommands oldValue = _commands;
                    _commands = value;
                    OnCommandsChanged(oldValue, value);
                }
            }
        }
        /// <summary>
        /// Called when Commands property has changed.
        /// </summary>
        /// <param name="oldValue">Old property value</param>
        /// <param name="newValue">New property value</param>
        protected virtual void OnCommandsChanged(MetroBillCommands oldValue, MetroBillCommands newValue)
        {
            if (newValue != null)
            {
                ui_buttonX_logon.Command = newValue.StartControlCommands.Logon;
                ui_buttonX_exit.Command = newValue.StartControlCommands.Exit;
            }
            else
            {
                ui_buttonX_logon.Command = null;
                ui_buttonX_exit.Command = null;
            }
        }

        private void StartControlLoad(object sender, System.EventArgs e)
        {            
            labelX1.ForeColor = Color.Green;
            ui_textBox_ip.Focus();
            ui_textBoxX_login.Text = Settings.Default.scUser1;
            ui_textBoxX_password.Text = Settings.Default.scPassword;
            ui_textBox_ip.Text = Settings.Default.scHost;
            ui_textBox_ip_slave.Text = Settings.Default.scHostSlave;   
        }

        private void Ui_TextBoxX_Login_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData== Keys.Enter)
            {
                if (ui_buttonX_logon.Enabled)
                    ui_buttonX_logon.Command.Execute();
            }
        }

        private void ui_buttonX_logon_Click(object sender, System.EventArgs e)
        {

        }


    }
}
