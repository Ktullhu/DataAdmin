using System.Drawing;
using DataAdmin.Core;

namespace DataAdmin.Forms
{
    public partial class AddUserControl : DevComponents.DotNetBar.Controls.SlidePanel
    {
        public AddUserControl()
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
                ui_buttonX_add.Command = newValue.AddUserControlCommands.Add;
                ui_buttonX_cancel.Command = newValue.AddUserControlCommands.Cancel;
            }
            else
            {
                ui_buttonX_add.Command = null;
                ui_buttonX_cancel.Command = null;
            }
        }

        private void StartControl_Load(object sender, System.EventArgs e)
        {
            labelX1.ForeColor = Color.SteelBlue;
            ui_textBoxX_name.Clear();
            ui_textBoxX_login.Clear();
            ui_textBoxX_password.Clear();
            ui_textBoxX_repassword.Clear();
            ui_textBoxX_phone.Clear();
            ui_textBoxX_email.Clear();
            ui_textBoxX_ip.Clear();
            ui_switchButton_any_Ip.Value = true;
            ui_switchButton_share.Value = true;
            ui_switchButton_allowCollecting.Value = true;
            ui_switchButton_allowUser.Value = true;
            ui_switchButton_allwoMissingBar.Value = true;
            ui_switchButton_enableDataNet.Value = true;
            ui_switchButton_enableTickNet.Value = true;
            ui_switchButton_local.Value = true;            

            ui_textBoxX_name.Focus();
        }

        private void ui_pictureBox_backButton_Click(object sender, System.EventArgs e)
        {
            Commands.AddUserControlCommands.Cancel.Execute();
        }

    }
}
