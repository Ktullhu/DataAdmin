using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DataNetClient.Controls
{
    public partial class StyledTextBox : UserControl
    {
        public StyledTextBox()
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

        public List<string> AutoCompleteCustomSource1
        {
            get
            {
                return (from object re in textBoxX1.AutoCompleteCustomSource select re.ToString()).ToList();
            }
            set
            {
                textBoxX1.AutoCompleteCustomSource.Clear();

                foreach (var item in value)
                {
                    textBoxX1.AutoCompleteCustomSource.Add(item);    
                }
                
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

        }

        private void textBoxX1_TextChanged(object sender, System.EventArgs e)
        {
            OnTextChanged(e);
        }

        
    }
}
