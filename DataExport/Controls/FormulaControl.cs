using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DataExport.Core.CustomFormula;
using DevComponents.DotNetBar;
using DADataManager.ExportModels;

namespace DataExport.Controls
{
    public partial class FormulaControl : UserControl
    {
        #region VARIABLES

        private ButtonX _currentElement;
        private readonly List<ElementStructure> _elements =new List<ElementStructure>();        

        #endregion 
        
        #region EVENTS

        public delegate void ChangeHandler();
        public event ChangeHandler FormulaChanged;


        private void OnFormulaChanged()
        {
            ChangeHandler handler = FormulaChanged;
            if (handler != null) handler();
        }

        #endregion


        public FormulaControl()
        {
            InitializeComponent();
        }

        #region public Functions

        public void AddElement(string value, DADataManager.ExportModels.ElementType type, bool update=true)
        {
            var locX = GetNextLocation();

            var element = new ButtonX
            {
                Text = value,
                Size = new Size(GetStringWidth(value), 23),
                Location = new Point(locX, 1),
                Parent = ui_panelEx_formula,
                ContextMenuStrip = contextMenuStrip_element,
                Tag = type
            };
            element.Click += Element_Click;
            element.MouseDown += Element_MouseDown;

            ui_panelEx_formula.Controls.Add(element);            
                SetCurrentElement(element, update);
            ShiftElements(_currentElement, GetStringWidth(value) + 1);

            if (type == ElementType.Equal)
            {
                element.ContextMenuStrip = contextMenuStrip_equal;
            }

            var listCnt = (from object control in ui_panelEx_formula.Controls select (control as ButtonX)).Cast<Control>().ToList().OrderBy(x => x.Location.X).ToList();

            _elements.Clear();
            for (var i = 0; i < listCnt.Count; i++)
            {
                listCnt[i].TabIndex = i;
                _elements.Add(new ElementStructure{Type = (ElementType)listCnt[i].Tag, Value = listCnt[i].Text});
            }

           
           // _elements.Add(new ElementStructure{Type = type, Value = value});
            if (update)
            {
                ui_labelX_arrow.Visible = true;
                OnFormulaChanged();
            }

        }

        public void ReplaceElement(string value, ElementType type)
        {
            if (_currentElement == null)
            {
                return;
            }
            var oldWidth = GetStringWidth(_currentElement.Text);
            var newWidth = GetStringWidth(value);
            var offset = (newWidth - oldWidth);

            var currIndex = GetCurrentIndex();

            var item = _elements[currIndex];
                item.Type = type;
                item.Value = value;
            _elements[currIndex] = item;

            _currentElement.Text = value;
            _currentElement.Tag = type;
            _currentElement.Width = newWidth;

            ShiftElements(_currentElement, offset);

            OnFormulaChanged();
        }

        private int GetCurrentIndex()
        {
            var listCnt = (from object control in ui_panelEx_formula.Controls select (control as ButtonX)).Cast<Control>().ToList().OrderBy(x => x.Location.X).ToList();
            for (int i = 0; i < listCnt.Count; i++)
            {
                if(listCnt[i]==_currentElement)
                {
                    return i;                    
                }
            }
            return -1;
        }

        public void Clear()
        {
            ui_labelX_arrow.Visible = false;
            _currentElement = null;
            ui_panelEx_formula.Controls.Clear();
            ui_labelX_arrow.Location= new Point(11,56);
            _elements.Clear();
            
        }

        #endregion

        #region CUSTOM Formula

        private void DeleteCurentElement()
        {
            if (_currentElement == null) return;
            var currWidth = GetStringWidth(_currentElement.Text) + 1;
            var currLeft = _currentElement.Location.X;

            if (ui_panelEx_formula.Controls.Contains(_currentElement))
            {
                var currIndex = GetCurrentIndex();
                var item = _elements[currIndex];
                _elements.Remove(item);

                ui_panelEx_formula.Controls.Remove(_currentElement);
            }

            if (ui_panelEx_formula.Controls.Count == 0)
            {
                _currentElement = null;
                ChangeArrowLocation(null);
            }
            else
            {
                ShiftElements(null, -currWidth, currLeft);
                SetNextElement(currLeft);
            }

            OnFormulaChanged();
        }

        private void SetNextElement(int currLeft)
        {
            var min = 9999;
            ButtonX cbtn = null;

            foreach (var cntrl in ui_panelEx_formula.Controls)
            {
                var btn = (cntrl as ButtonX);

                if (btn != null && btn.Left >= currLeft && btn.Left < min)
                {
                    min = btn.Left;
                    cbtn = btn;
                }
            }                        
            
            if (cbtn != null)
                SetCurrentElement(cbtn);
            else
            {
                min = 0;
                foreach (var cntrl in ui_panelEx_formula.Controls)
                {
                    var btn = (cntrl as ButtonX);

                    if (btn != null && btn.Left < currLeft && btn.Left > min)
                    {
                        min = btn.Left;
                        cbtn = btn;
                    }
                }

                if (cbtn != null)
                    SetCurrentElement(cbtn);
            }

        }

        private int GetStringWidth(string str)
        {
            var g = ui_panelEx_formula.CreateGraphics();
            var width = (int)g.MeasureString(str, ui_panelEx_formula.Font).Width;
            return width + 6;
        }

        private int GetNextLocation()
        {
            if (_currentElement == null) return 1;
            return _currentElement.Location.X + _currentElement.Size.Width + 1;
        }

        private void Element_Click(object sender, EventArgs e)
        {
            SetCurrentElement(sender as ButtonX);
        }

        private void Element_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SetCurrentElement(sender as ButtonX);
                //_currentElement = sender as ButtonX;
            }
        }

        private void SetCurrentElement(ButtonX sender, bool update=true)
        {
            foreach (var item in ui_panelEx_formula.Controls)
            {
                if (item == sender)
                {
                    var buttonX = item as ButtonX;
                    if (buttonX != null)
                    {
                        if(update)
                            ChangeArrowLocation(buttonX);
                        _currentElement = buttonX;
                    }
                }
            }
        }

        private void ShiftElements(ButtonX button, int offset, int from = 0)
        {
            for (int i = ui_panelEx_formula.Controls.Count - 1; i >= 0; i--)
            {
                if (button == null && ui_panelEx_formula.Controls[i].Location.X < from)
                    continue;
                if (button != null && ui_panelEx_formula.Controls[i].Location.X < button.Location.X)
                    continue;
                if (ui_panelEx_formula.Controls[i] == button)
                    continue;
                ui_panelEx_formula.Controls[i].Location = new Point(ui_panelEx_formula.Controls[i].Location.X + offset,
                                                                    ui_panelEx_formula.Controls[i].Location.Y);
            }
            //ChangeArrowLocation(button);
        }

        private void ChangeArrowLocation(ButtonX buttonX)
        {
            if (buttonX == null)
            {
                ui_labelX_arrow.Visible = false;
                ui_labelX_arrow.Location = new Point(11, ui_labelX_arrow.Location.Y);
                return;
            }
            ui_labelX_arrow.Location = new Point(buttonX.Location.X + 5 + buttonX.Size.Width / 2, ui_labelX_arrow.Location.Y);

        }
         
        private void ui_panelEx_formula_Click(object sender, EventArgs e)
        {
            SetLastElementCurrent();
        }

        private void SetLastElementCurrent()
        {
            int[] max = { 0 };
            ButtonX maxButton = null;
            foreach (var button in ui_panelEx_formula.Controls.OfType<ButtonX>().Where(button => button.Left > max[0]))
            {
                max[0] = button.Left;
                maxButton = button;
            }

            if (maxButton != null)
                SetCurrentElement(maxButton);
        }

        private void deleteElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteCurentElement();

        }

        private void changeToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentElement == null) return;
            var menuItem = sender as ToolStripDropDownItem;
            if (menuItem != null) _currentElement.Text = menuItem.Text;

            var currIndex = GetCurrentIndex();

            var item = _elements[currIndex];
            if (menuItem != null) item.Value = menuItem.Text;
            _elements[currIndex] = item;

            
            OnFormulaChanged();
        }

        private void toolStripMenuItemClearAll_Click(object sender, EventArgs e)
        {
            Clear();
            OnFormulaChanged();
        }

        #endregion        

        public string GetFormula()
        {
            var listCnt = (from object control in ui_panelEx_formula.Controls select (control as ButtonX)).Cast<Control>().ToList().OrderBy(x => x.Location.X).ToList();
            return listCnt.Aggregate("", (current, control) => current + control.Text);
        }

        public List<ElementStructure> GetElements()
        {
            return _elements.ToList();
        }

        public List<string> GetUsedColumns()
        {
            var res = new List<string>();
            foreach (var item in _elements.Where(item => item.Type == ElementType.Column && !res.Exists(a=>a == item.Value)))
            {
                res.Add(item.Value);
            }
            return res;            
        }
    }
}
