using System.Drawing;
using System.Windows.Forms;

namespace RozkladCommon.Controls
{
    public partial class StyledPasswordTextBox : UserControl
    {
        public StyledPasswordTextBox()
        {
            InitializeComponent();
        }

        public string InputText
        {
            get
            {
                return textBoxX1.Text; 
                
            }
            set
            {
                textBoxX1.Text = value;
            }
        }

        public string WatermarkText
        {
            get
            {
                return textBoxX1.WatermarkText;

            }
            set
            {
                textBoxX1.WatermarkText = value;
            }
        }


        private void StyledTextBox_Paint(object sender, PaintEventArgs e)
        {
            textBoxX1.BackColor = Color.FromArgb(255, 65, 66, 66);
            /*
            var renderer = GlobalManager.Renderer as Office2007Renderer;
            if (renderer == null) return;
            var table = renderer.ColorTable;

            var style = (ElementStyle)table.StyleClasses[ElementStyleClassKeys.TextBoxBorderKey];
            style.BorderColor =
            style.BackColor = Color.FromArgb(255,65,66,66);

            Office2007ComboBoxColorTable comboColors = table.ComboBox;
            comboColors.DefaultStandalone.Background = Color.FromArgb(255,65,66,66);*/
        }

        private void textBoxX1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OnKeyDown(e);
            }
        }
    }
}
