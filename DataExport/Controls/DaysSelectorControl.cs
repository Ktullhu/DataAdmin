using System;
using System.Drawing;
using System.Windows.Forms;

namespace DataExport.Controls
{
    public partial class DaysSelectorControl : UserControl
    {

        public delegate void CheckedStateChangedHandler();

        public event CheckedStateChangedHandler CheckedStateChanged;

        public void OnCheckedStateChanged()
        {
            CheckedStateChangedHandler handler = CheckedStateChanged;
            if (handler != null) handler();
        }

        public DaysSelectorControl()
        {
            InitializeComponent();
        }
        
        private void DaysSelectorControl_Load(object sender, EventArgs e)
        {
            Repaint();
        }

        public void Repaint()
        {
            labelTop.BackColor =
                panelTop.BackColor = BackColor;
            labelTop.ForeColor = Color.White;            
            labelMiddle.ForeColor = Color.DimGray;
        }

        public bool GetCheckedState(int index)
        {
            if (index>6) throw new IndexOutOfRangeException();

            switch (index)
            {
                case 0:
                    return checkBoxX1.CheckState == CheckState.Checked;
                case 1:
                    return checkBoxX2.CheckState == CheckState.Checked;
                case 2:
                    return checkBoxX3.CheckState == CheckState.Checked;
                case 3:
                    return checkBoxX4.CheckState == CheckState.Checked;
                case 4:
                    return checkBoxX5.CheckState == CheckState.Checked;
                case 5:
                    return checkBoxX6.CheckState == CheckState.Checked;
                case 6:
                    return checkBoxX7.CheckState == CheckState.Checked;
            }
            return false;
        }

        public void SetCheckedState(int index, bool newState)
        {
            if (index > 6) throw new IndexOutOfRangeException();

            switch (index)
            {
                case 0:
                    checkBoxX1.CheckValue = newState;
                    break;
                case 1:
                    checkBoxX2.CheckValue = newState;
                    break;
                case 2:
                    checkBoxX3.CheckValue = newState;
                    break;
                case 3:
                    checkBoxX4.CheckValue = newState;
                    break;
                case 4:
                    checkBoxX5.CheckValue = newState;
                    break;
                case 5:
                    checkBoxX6.CheckValue = newState;
                    break;
                case 6:
                    checkBoxX7.CheckValue = newState;
                    break;
            }            
        }


        private void checkBoxX1_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckedStateChanged();
        }

       
    }
}
