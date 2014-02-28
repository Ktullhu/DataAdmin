using System;
using System.Windows.Forms;

namespace DataExport.Controls
{
    public partial class TitleControl : UserControl
    {
        public TitleControl()
        {
            InitializeComponent();
        }

        public char Character
        {
            get { return labelChar.Text[0]; }
            set { labelChar.Text = Convert.ToString(value); }
        }
        public string LabelText1
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }
        public string LabelText2
        {
            get { return label2.Text; }
            set { label2.Text = value; }
        }
    }
}
