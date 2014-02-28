using System.Drawing;
using DataAdmin.Core;

namespace DataAdmin.Forms
{
    public partial class EditUserControl : DevComponents.DotNetBar.Controls.SlidePanel
    {
        public EditUserControl()
        {
            InitializeComponent();
        }

        public string OldUserLogin { get; private set; }
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

        

        protected virtual void OnCommandsChanged(MetroBillCommands oldValue, MetroBillCommands newValue)
        {
            if (newValue != null)
            {
                ui_buttonX_Save.Command = newValue.EditUserControlCommands.SaveChanges;
                ui_buttonX_cancel.Command = newValue.EditUserControlCommands.Cancel;
            }
            else
            {
                ui_buttonX_Save.Command = null;
                ui_buttonX_cancel.Command = null;
            }
        }

        private void EditUserControl_Load(object sender, System.EventArgs e)
        {
            labelX1.ForeColor = Color.SteelBlue;
            OldUserLogin = ui_textBoxX_login.Text; 
            ui_textBoxX_name.Focus();
        }

        private void ui_pictureBox_backButton_Click(object sender, System.EventArgs e)
        {
            Commands.EditUserControlCommands.Cancel.Execute();
        }
    }
}
