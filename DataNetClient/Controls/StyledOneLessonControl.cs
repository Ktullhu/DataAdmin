using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;

namespace RozkladCommon.Controls
{
    public partial class StyledOneLessonControl : UserControl
    {
        public StyledOneLessonControl()
        {
            InitializeComponent();
        }

        public string NoTitle
        {
            get { return labelX_no.Text; }
            set { labelX_no.Text = value; }
        }

        public string Predmet
        {
            get { return labelX_predmet.Text; }
            set { labelX_predmet.Text = value; }
        }

        public string Prepod
        {
            get { return labelX_prepod.Text; }
            set { labelX_prepod.Text = value; }
        }
        public string Auditoria
        {
            get { return labelX_aud.Text; }
            set { labelX_aud.Text = value; }
        }

        private void labelX_no_MouseMove(object sender, MouseEventArgs e)
        {
            var lbl = (sender as LabelX);
            if (lbl != null)
            {
                lbl.Font = new Font(labelX4.Font, FontStyle.Bold);
            }
        }

        private void labelX_no_MouseLeave(object sender, System.EventArgs e)
        {
            var lbl = (sender as LabelX);
            if (lbl != null)
            {
                lbl.Font = labelX4.Font;                
            }
        }

        private LabelX _lastLabel;
        private TextBoxX _lastTbx;

        private void labelX_predmet_DoubleClick(object sender, System.EventArgs e)
        {
            var lbl = (sender as LabelX);
            if (lbl != null)
            {
                _lastLabel = lbl;
                _lastTbx = new TextBoxX { Size = new Size(lbl.Size.Width - 5, lbl.Size.Height), Location = new Point(lbl.Location.X+4, lbl.Location.Y + 4), Text = lbl.Text, BackColor = Color.FromArgb(255,65,66,66)};
                _lastTbx.LostFocus += tbx_LostFocus;
                _lastTbx.KeyDown += _lastTbx_KeyDown;
                Controls.Add(_lastTbx);
                _lastTbx.BringToFront();
                _lastTbx.Focus();
            }
        }

        void _lastTbx_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                var tbx = sender as TextBoxX;
                if (tbx != null)
                {
                    
                    _lastLabel.Text = tbx.Text;

                    tbx.Dispose();

                    OnTextChanged(e);
                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                var tbx = sender as TextBoxX;
                if (tbx != null)
                {
                    //invers
                    tbx.Text = _lastLabel.Text;

                    tbx.Dispose();

                    OnTextChanged(e);
                }
            }
        }

        void tbx_LostFocus(object sender, System.EventArgs e)
        {
            var tbx = sender as TextBoxX;
            if (tbx != null)
            {
                _lastLabel.Text = tbx.Text;
                
                tbx.Dispose();

                OnTextChanged(e);
            }
        }

        private void labelX_predmet_Click(object sender, System.EventArgs e)
        {
            if (_lastTbx != null)
            {
                _lastTbx.Dispose();
                _lastLabel = null;
            }
        }
    }
}
