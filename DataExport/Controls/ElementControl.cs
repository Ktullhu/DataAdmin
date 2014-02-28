using System;
using System.Drawing;
using System.Windows.Forms;

namespace DataExport.Controls
{
    public partial class ElementControl : UserControl
    {
        private  int _index;
        private Color _backColor = Color.Gray;
        private bool _isSelected;
        private bool _isIncorrect;

        public ElementControl()
        {
            InitializeComponent();
            _index = 0;
            ElementColor = Color.SteelBlue;
            _backColor = Color.Gray;
            _isSelected = false;
            _isIncorrect = false;
        }

        #region Properties
        
        public string LabeledText
        {
            get { return labelElementText.Text; }
            set { labelElementText.Text = value; }
        }
        public int Index 
        { 
            get { return _index; } 
            set { _index = value; } 
        }

        private Color _elementColor;
        public Color ElementColor
        {
            get { return _elementColor; }  
            set { 
                _elementColor = value;
                ChnageColor();
            }
        }

        private void ChnageColor()
        {
            panelRightBorder.BackColor = _elementColor;
            labelXRight.BackColor = _elementColor;
        }
        

        #endregion

        #region Events

        public delegate void BtnClick(object sender, ElementEventArgs e);
        public event BtnClick ButtonClick;
        void OnButtonClick(object sender, ElementEventArgs e)
        {
            if(ButtonClick!=null)
            {
                ButtonClick(sender, e);
            }
        }
        

        #endregion

        #region Buttons Click

        private void buttonClick(object sender, EventArgs e)
        {
            var ee = new ElementEventArgs { Index = _index};
            OnButtonClick(this, ee);
        }

        #endregion        
        
        public void SetSelected(bool isSelected)
        {
            _isSelected = isSelected;

            Repaint();
        }

        public void SetIncorrect(bool isIncorrect)
        {
            _isIncorrect = isIncorrect;

            Repaint();
        }

        private void ElementControl_MouseLeave(object sender, EventArgs e)
        {
            panelMain.BackColor =
                labelElementText.BackColor = 
                _backColor;
        }

        private void ElementControl_MouseMove(object sender, MouseEventArgs e)
        {
            //panelLeftBorder.BackColor = _backColor;
            panelRightBorder.BackColor =
                labelXRight.BackColor =
                ElementColor;

            panelMain.BackColor =                
                labelElementText.BackColor =
                ElementColor;

            if (_isIncorrect)
            {
                panelRightBorder.BackColor=
                panelLeftBorder.BackColor =
                labelXRight.BackColor = Color.Red;
            }   
        }

        public void Repaint()
        {
            if (_isSelected)
            {
                _backColor = labelElementText.BackColor = Color.DimGray;
                panelMain.BackColor = ElementColor;
                    //

                panelLeftBorder.BackColor = ElementColor;

                panelRightBorder.Width = 35;
            }
            else
            {
                panelRightBorder.Width = 45;

                _backColor = panelMain.BackColor =
                    panelLeftBorder.BackColor =
                    labelElementText.BackColor = Color.Gray;   
            }         

            panelRightBorder.BackColor =
                    labelXRight.BackColor = ElementColor;
            panelMain.ForeColor = 
                labelElementText.ForeColor = Color.White;

            if (_isIncorrect)
            {
                panelRightBorder.BackColor =
                panelLeftBorder.BackColor =
                labelXRight.BackColor = Color.Red;
            }   
        }
    }

    public class ElementEventArgs : EventArgs
    {
        public int Index;
    }


}
