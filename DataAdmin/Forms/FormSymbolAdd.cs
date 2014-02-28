using System;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DataAdmin.Forms
{
    public partial class FormSymbolAdd : Form
    {
        public FormSymbolAdd()
        {
            InitializeComponent();
        }

        private void buttonX_data_archive_start_Click(object sender, EventArgs e)
        {
            if (ui_textBoxX_SymbolName.Text != "")
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                ToastNotification.Show(this, "Please, enter name of contract.", 1000, eToastPosition.TopCenter);
            }          
        }

        private void ui_buttonX_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void FormSymbolAdd_Shown(object sender, EventArgs e)
        {
            ui_textBoxX_SymbolName.Focus();
            ui_textBoxX_SymbolName.Text = "";
        }

        private void ui_textBoxX_SymbolName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                DialogResult = DialogResult.Cancel;
            if (e.KeyCode == Keys.Enter)
            {
                if (ui_textBoxX_SymbolName.Text != "")
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    ToastNotification.Show(this, "Please, enter name of contract.", 1000, eToastPosition.TopCenter);
                }
            }
        }

    }
}
