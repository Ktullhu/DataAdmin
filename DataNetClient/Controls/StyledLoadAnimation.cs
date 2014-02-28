using System;
using System.Drawing;
using System.Windows.Forms;

namespace DataNetClient.Controls
{
    public partial class StyledLoadAnimation : UserControl
    {
        public StyledLoadAnimation()
        {
            InitializeComponent();
        }

        private int _newX;
        private Color _baseColor = Color.DodgerBlue;

        public Color BackColor1
        {
            get { return panelEx1.Style.BackColor1.Color; }
            set { panelEx1.Style.BackColor1.Color = value; }
        }

        public Color BaseColor
        {
            get { return _baseColor; }
            set { _baseColor = value; }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panelEx1.Invalidate();
            _newX += 20;
            if (_newX > Width + 200)
            {

                _newX = -2;
                _animationShow=
                timer1.Enabled = false;
                panelEx1.Invalidate();
            }
        }

        private bool _animationShow;        

        public void StartAnimation()
        {
            _newX = -2;
            _animationShow=
            timer1.Enabled = true;
        }

        private void panelEx1_Paint(object sender, PaintEventArgs e)
        {
            
            if (_animationShow)
            {
                var xLoc = Math.Min(_newX, panelEx1.Width);
                e.Graphics.DrawLine(new Pen(Color.FromArgb(70, _baseColor), 3), new Point(xLoc - 60, 1),
                    new Point(xLoc + 2, 1));
                e.Graphics.DrawLine(new Pen(Color.FromArgb(60, _baseColor), 4), new Point(xLoc - 40, 1),
                    new Point(xLoc, 1));
                e.Graphics.DrawLine(new Pen(Color.FromArgb(50, _baseColor), 5), new Point(xLoc - 30, 1),
                    new Point(xLoc, 1));
                e.Graphics.DrawLine(new Pen(Color.FromArgb(40, _baseColor), 6), new Point(xLoc - 25, 1),
                    new Point(xLoc + 2, 1));
                e.Graphics.DrawLine(new Pen(Color.FromArgb(255, _baseColor), 2), new Point(0, 1), new Point(_newX, 1));
            }
        }
    }
}
