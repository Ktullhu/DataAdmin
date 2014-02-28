using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DataExport.Core;
using DataExport.Core.CustomFormula;
using DevComponents.DotNetBar;
using NCalc;
using DADataManager.ExportModels;
using DADataManager;

namespace DataExport.Forms
{
    public partial class CustomFormulaControl : DevComponents.DotNetBar.Controls.SlidePanel
    {
        public bool IfSnapShot
        {
            get { return _isSnapShot; }
        }

        private readonly bool _isSnapShot;
        private readonly List<SimpleFormulaModel> _formulas = new List<SimpleFormulaModel>();

        #region HANDLES

        public List<SimpleFormulaModel> Formulas
        {
            get { return _formulas.Where(a => a.Formula != "").ToList(); }
        }

        public CustomFormulaControl(MetroBillCommands commands, int userId, bool isSnapShot,
                                    IEnumerable<SimpleFormulaModel> formulas,IEnumerable<string> checkedColumns)
        {
            InitializeComponent();
            Commands = commands;
            _userId = userId;
            _isSnapShot = isSnapShot;
            _formulas = new List<SimpleFormulaModel>();
            foreach (var simpleFormulaModel in formulas)
            {
                var nModel = new SimpleFormulaModel
                                 {
                                     Name = simpleFormulaModel.Name,
                                     Elements = simpleFormulaModel.Elements.ToList(),
                                     Formula = simpleFormulaModel.Formula,
                                     FormulaId = simpleFormulaModel.FormulaId,
                                     FormulaType = simpleFormulaModel.FormulaType,
                                     IsSnapShot = simpleFormulaModel.IsSnapShot,
                                     UsedColumns = simpleFormulaModel.UsedColumns.ToList(),
                                     UserId = simpleFormulaModel.UserId
                                 };
                AddFormula(nModel);
            }
            ui_comboBoxEx_useColumn.Items.Clear();
            foreach (var checkedColumn in checkedColumns)
            {
                ui_comboBoxEx_useColumn.Items.Add(checkedColumn);
            }
        }

        private MetroBillCommands _commands;
        private readonly int _userId;

        /// <summary>
        /// Gets or sets the commands associated with the control.
        /// </summary>
        private MetroBillCommands Commands
        {
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
                //ui_buttonX_save.Command = newValue.CustomFormulaControlCommands.Save;
                ui_buttonX_cancel.Command = newValue.CustomFormulaControlCommands.Cancel;
            }
            else
            {
                // ui_buttonX_save.Command = null;
                ui_buttonX_cancel.Command = null;
            }
        }

        private void StartControlLoad(object sender, EventArgs e)
        {
            labelX1.ForeColor = Color.CadetBlue;
            ui_comboBoxEx_useColumn.SelectedIndex = 0;
            elementContainerControl1.Repaint();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ui_buttonX_cancel.Command.Execute();
        }

        #endregion

        #region CUSTOM Formula

        private void ui_textBoxX_constant_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',') e.KeyChar = '.';
            if (e.KeyChar == '@' || e.KeyChar == '^') e.Handled = true;
        }

        private void textBoxX_formulaName_TextChanged(object sender, EventArgs e)
        {
            var index = elementContainerControl1.SelectedIndex;
            if (index < 0) return;

            if (_formulas[index].Name != textBoxX_formulaName.Text && !string.IsNullOrEmpty(textBoxX_formulaName.Text))
            {
                textBoxX_formulaName.ButtonCustom.Visible = true;
                textBoxX_formulaName.Modified = true;
            }
            else
            {
                textBoxX_formulaName.ButtonCustom.Visible = false;
                textBoxX_formulaName.Modified = false;
            }
        }

        private void textBoxX_formulaName_ButtonCustomClick(object sender, EventArgs e)
        {
            textBoxX_formulaName.Modified = false;
            textBoxX_formulaName.ButtonCustom.Visible = false;
            if (elementContainerControl1.SelectedIndex != -1)
            {

                var index = elementContainerControl1.SelectedIndex;
                var oldName = elementContainerControl1.GetText(index);
                var newName = textBoxX_formulaName.Text;

                if (oldName != newName)
                {
                    if (!_formulas.Exists(a => a.Name == newName))
                    {
                        _formulas[index].Name = newName;
                        elementContainerControl1.SetText(index, newName);
                    }
                    else
                    {
                        var i = 1;
                        while (_formulas.Exists(a => a.Name == newName + " " + i))
                        {
                            i++;
                        }
                        _formulas[index].Name = newName + " " + i;
                        elementContainerControl1.SetText(index, newName + " " + i);
                        textBoxX_formulaName.Text = newName + " " + i;
                    }
                }
                _formulas[index].Formula = formulaControl1.GetFormula();
                _formulas[index].Elements = formulaControl1.GetElements();
                _formulas[index].UsedColumns = formulaControl1.GetUsedColumns();
            }

        }

        private void formulaControl1_FormulaChanged()
        {
            var index = elementContainerControl1.SelectedIndex;
            if (index < 0) return;
            _formulas[index].Formula = formulaControl1.GetFormula();
            _formulas[index].Elements = formulaControl1.GetElements();
            _formulas[index].UsedColumns = formulaControl1.GetUsedColumns();
        }

        private void ui_buttonX_useColumn_Click(object sender, EventArgs e)
        {
            if (ui_comboBoxEx_useColumn.SelectedItem == null)
            {
                ToastNotification.Show(this, "Please select column.", eToastPosition.BottomCenter);
                return;
            }

            var text = ui_comboBoxEx_useColumn.SelectedItem.ToString();

            formulaControl1.AddElement(text, ElementType.Column);
        }

        private void ui_buttonX_replaceColumn_Click(object sender, EventArgs e)
        {
            if (ui_comboBoxEx_useColumn.SelectedItem == null)
            {
                ToastNotification.Show(this, "Please select column.", eToastPosition.BottomCenter);
                return;
            }


            var text = ui_comboBoxEx_useColumn.SelectedItem.ToString();

            formulaControl1.ReplaceElement(text, ElementType.Column);
        }

        private void ui_buttonX_constant_add_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ui_textBoxX_constant.Text))
            {
                ToastNotification.Show(this, "Please type constant.", eToastPosition.BottomCenter);
                return;
            }
            var text = ui_textBoxX_constant.Text;

            formulaControl1.AddElement(text, ElementType.Constant);

        }

        private void ui_buttonX_replaceConstant_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ui_textBoxX_constant.Text))
            {
                ToastNotification.Show(this, "Please type constant.", eToastPosition.BottomCenter);
                return;
            }

            var text = ui_textBoxX_constant.Text;

            formulaControl1.ReplaceElement(text, ElementType.Constant);
        }

        private void ui_buttonX_operation_Click(object sender, EventArgs e)
        {
            var text = "";
            var buttonX = sender as ButtonX;
            if (buttonX != null)
            {
                text = buttonX.Text;
            }
            formulaControl1.AddElement(text, ElementType.Operation);
        }


        private void ui_buttonX_operation_openBraket_Click(object sender, EventArgs e)
        {

            formulaControl1.AddElement("(", ElementType.OpenBracket);
        }

        private void ui_buttonX_operation_closeBraket_Click(object sender, EventArgs e)
        {

            formulaControl1.AddElement(")", ElementType.CloseBracket);
        }


        #endregion

        #region Add Edit Delete

        private void buttonXAddExisting_Click(object sender, EventArgs e)
        {
            
            var alllExistingF = DataExportClientDataManager.GetFormulaForUser(_userId);

            if (alllExistingF.Count == 0)
            {
                ToastNotification.Show(elementContainerControl1, "There are no one frmula.", eToastPosition.TopCenter);
                return;
            }

            var frm = new FormExistingsFormulas(alllExistingF)
                          {
                              Location =
                                  PointToScreen(new Point(elementContainerControl1.Location.X + 10,
                                                          elementContainerControl1.Location.Y + 41)),
                              Size =
                                  new Size(elementContainerControl1.Size.Width - 20,
                                           elementContainerControl1.Size.Height - 51)

                          };
            if (frm.ShowDialog() == DialogResult.OK)
            {
                const string fname = "new formula ";
                var i = 1;
                while (_formulas.Exists(a => a.Name == fname + i))
                {
                    i++;
                }

                var model = frm.SelectedFormula;
                model.Name = fname + i;
                model.IsSnapShot = _isSnapShot;
                AddFormula(model);
            }
        }




        private void buttonXDelete_Click(object sender, EventArgs e)
        {
            if (elementContainerControl1.SelectedIndex != -1)
            {
                var index = elementContainerControl1.SelectedIndex;
                _formulas.Remove(_formulas[index]);
                elementContainerControl1.RemoveElement(index);

            }
        }

        private void buttonXAdd_Click(object sender, EventArgs e)
        {
            const string fname = "new formula ";
            var i = 1;
            while (_formulas.Exists(a => a.Name== fname + i))
            {
                i++;
            }

            AddFormula(fname + i);
        }


        private void AddFormula(string fName)
        {
            AddFormula(new SimpleFormulaModel
                           {
                               Name = fName,
                               FormulaType = FormulaType.Simply,
                               Elements = new List<ElementStructure>(),
                               IsSnapShot = _isSnapShot,
                               UsedColumns = new List<string>(),
                               Formula = "",
                               UserId = _userId

                           });
        }

        private void AddFormula(SimpleFormulaModel fmodel)
        {
            _formulas.Add(fmodel);
            elementContainerControl1.AddElement(fmodel.Name);

        }

        private void elementContainerControl1_SelectedIndexChanged(object sender, Controls.ElementEventArgs e)
        {
            formulaControl1.Clear();

            if (elementContainerControl1.SelectedIndex != -1)
            {
                textBoxX_formulaName.Text = elementContainerControl1.GetText(elementContainerControl1.SelectedIndex);
                panelExAddNew.Enabled = true;
                textBoxX_formulaName.ReadOnly = false;

                for (int index = 0; index < _formulas[elementContainerControl1.SelectedIndex].Elements.Count; index++)
                {
                    var item = _formulas[elementContainerControl1.SelectedIndex].Elements[index];
                    formulaControl1.AddElement(item.Value, item.Type,
                                               index ==
                                               _formulas[elementContainerControl1.SelectedIndex].Elements.Count - 1);
                }
            }
            else
            {
                textBoxX_formulaName.Text = "";
                panelExAddNew.Enabled = false;
                textBoxX_formulaName.ReadOnly = true;
            }

        }

        #endregion

        #region Operators

        private void buttonX_max_Click(object sender, EventArgs e)
        {
            var btn = (sender as ButtonX);
            if (btn != null)
            {
                var text = btn.Text;
                formulaControl1.AddElement(text, ElementType.Operation);
                formulaControl1.AddElement("(", ElementType.OpenBracket);
                formulaControl1.AddElement("5", ElementType.Constant);
                formulaControl1.AddElement(",", ElementType.IfSeparete);
                formulaControl1.AddElement("3", ElementType.Constant);
                formulaControl1.AddElement(")", ElementType.CloseBracket);
            }
        }

        private void ui_buttonX_ifAdd_Click(object sender, EventArgs e)
        {

            formulaControl1.AddElement("if", ElementType.If);
            formulaControl1.AddElement("(", ElementType.OpenBracket);
            formulaControl1.AddElement("5", ElementType.Constant);
            formulaControl1.AddElement("<>", ElementType.Equal);
            formulaControl1.AddElement("3", ElementType.Constant);
            formulaControl1.AddElement(",", ElementType.IfSeparete);
            formulaControl1.AddElement("5", ElementType.Constant);
            formulaControl1.AddElement(",", ElementType.IfSeparete);
            formulaControl1.AddElement("3", ElementType.Constant);
            formulaControl1.AddElement(")", ElementType.CloseBracket);

        }

        private void buttonX_Abs_Click(object sender, EventArgs e)
        {
            var btn = (sender as ButtonX);
            if (btn != null)
            {
                var text = btn.Text;
                formulaControl1.AddElement(text, ElementType.Operation);
            }
        }

        #endregion

        private void ui_buttonX_save_Click(object sender, EventArgs e)
        {
            var incorrectFormulas = new List<string>();

            for (int index = 0; index < _formulas.Count; index++)
            {
                var simpleFormulaModel = _formulas[index];
                if (string.IsNullOrEmpty(simpleFormulaModel.Formula)) continue;

                var expression = new Expression(simpleFormulaModel.Formula);
                foreach (var column in simpleFormulaModel.UsedColumns)
                {
                    expression.Parameters[column] = 1.2;
                }

                try
                {
                    var item = (double) expression.Evaluate();
                    elementContainerControl1.SetIncorrect(index, false);
                }
                catch (Exception)
                {
                    try
                    {
                        var item = (int) expression.Evaluate();
                        elementContainerControl1.SetIncorrect(index, false);
                    }
                    catch (Exception)
                    {
                        incorrectFormulas.Add(simpleFormulaModel.Name);
                        elementContainerControl1.SetIncorrect(index, true);
                    }
                }
            }
            if (incorrectFormulas.Count > 0)
            {
                elementContainerControl1.Repaint();

                ToastNotification.Show(elementContainerControl1, "There are incorrect formulas!");
                return;
            }
            _commands.CustomFormulaControlCommands.Save.Execute();

            

        }
    }
}
