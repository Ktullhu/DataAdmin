using DADataManager.ExportModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DataExport.Forms
{
    public partial class FormExistingsFormulas : Form
    {
        private readonly List<SimpleFormulaModel> _alllExistingF;

        public SimpleFormulaModel SelectedFormula
        {
            get { return _alllExistingF[elementContainerControl1.SelectedIndex]; }
        }

        public FormExistingsFormulas(List<SimpleFormulaModel> alllExistingF)
        {
            InitializeComponent();            
            _alllExistingF = alllExistingF;
            foreach (var fmodel in alllExistingF)
            {
                elementContainerControl1.AddElement(fmodel.Name);    
            }
            
        }

        private void FormSymbolAdd_Shown(object sender, EventArgs e)
        {
            ui_buttonX_save.Focus();
        }


        private void ui_buttonX_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ui_buttonX_save_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;            
        }

        private void FormExistingsFormulas_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;   
            }
        }

        private void elementContainerControl1_SelectedIndexChanged(object sender, Controls.ElementEventArgs e)
        {
            textBoxX_formulaName.Text = _alllExistingF[elementContainerControl1.SelectedIndex].Formula;
        }

        


    }
}
