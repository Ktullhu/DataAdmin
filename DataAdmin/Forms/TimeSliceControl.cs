using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DataAdmin.Forms
{
    public partial class TimeSliceControl : UserControl
    {

        #region HANDLES

        public struct StrDate
        {
            public DateTime Start;
            public DateTime End;
        }

        public TimeSliceControl()
        {
            InitializeComponent();
            _startDate = DateTime.Now;
            LineColor1 = Color.Tomato;
            LineColor2 = Color.Goldenrod;
            LineColor3 = Color.Teal;
        }


        #endregion

        #region VARIABLES

        private List<StrDate> _loginsList1 = new List<StrDate>();
        private List<StrDate> _loginsList2 = new List<StrDate>();
        private List<StrDate> _loginsList3 = new List<StrDate>();
        private string _lastToolTip = "";
        private DateTime _startDate;
        private DateTime _endDate;
        private int _lineCount = 3;
        private int _maxDaysLooksBack = 7;

        #endregion

        #region PUBLICS

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                UpdateUI();
            }
        }

        public int MaxDaysLooksBack
        {
            get { return _maxDaysLooksBack; }
            set { _maxDaysLooksBack = value; UpdateUI(); }
        }
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                UpdateUI();
            }
        }

        public string TitleText
        {
            get { return labelXTitle.Text; }
            set { labelXTitle.Text = value; }
        }

        public Color LineColor1 { get; set; }
        public Color LineColor2 { get; set; }
        public Color LineColor3 { get; set; }

        public void SetList1(List<StrDate> listLogin)
        {
            _loginsList1 = listLogin;
            UpdateUI();
        }

        public void SetList2(List<StrDate> listLogin)
        {
            _loginsList2 = listLogin;
            UpdateUI();
        }

        public void SetList3(List<StrDate> listLogin)
        {
            _loginsList3 = listLogin;
            UpdateUI();
        }

        public int LineCount
        {
            get { return _lineCount; }
            set { _lineCount = Math.Max(2,Math.Min(3,value)); UpdateUI(); }
        }

        #endregion

        #region LOGIC

        private void UpdateUI()
        {
            //labelX_date.Text = "";
            panelEx_time.Refresh();
        }

        private void panelEx_time_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            var y1 = 1;
            var y2 = (float)panelEx_time.Height / 3;
            var y3 = (float)panelEx_time.Height * 2 / 3;

            var h1 = panelEx_time.Height / 3 - 2;
            var h2 = panelEx_time.Height / 3 - 1;
            var h3 = panelEx_time.Height / 3 - 1;            

            if (LineCount == 2)
            {
                y1 = 1;
                y2 = (float)panelEx_time.Height / 2;

                h1 = panelEx_time.Height/2 - 2;
                h2 = panelEx_time.Height/2 - 1;                
            }
            var totalM = (EndDate - StartDate).TotalMinutes;
            var wid = panelEx_time.Width;
            var div = (double)wid / (double)totalM;

            foreach (var item in _loginsList1)
            {
                var ths = (item.Start - StartDate).TotalMinutes;
                var the = (item.End - StartDate).TotalMinutes;
                var wdt = Math.Max(1, (the - ths) * div);

                var x = ths * div;

                e.Graphics.FillRectangle(new SolidBrush(LineColor1), (float)x, y1, (float)wdt, h1);
            }
            if (LineCount >= 2)
            {
                foreach (var item in _loginsList2)
                {
                    var ths = (item.Start - StartDate).TotalMinutes;
                    var the = (item.End - StartDate).TotalMinutes;
                    var wdt = Math.Max(1, (the - ths)*div);

                    var x = ths*div;

                    e.Graphics.FillRectangle(new SolidBrush(LineColor2), (float) x, y2,
                                             (float) wdt, h2);
                }
            }
            if (LineCount >= 3)
            {
                foreach (var item in _loginsList3)
                {
                    var ths = (item.Start - StartDate).TotalMinutes;
                    var the = (item.End - StartDate).TotalMinutes;
                    var wdt = Math.Max(1, (the - ths)*div);

                    var x = ths*div;

                    e.Graphics.FillRectangle(new SolidBrush(LineColor3), (float) x, y3,
                                             (float) wdt, h3);
                }
            }
            // day lines 
            for (int i = 1; i < MaxDaysLooksBack; i++)
            {
                var x = (StartDate.AddDays(i) - StartDate).TotalMinutes * div;
                e.Graphics.DrawLine(new Pen(Color.DarkBlue, 2), (float)x, (float)panelEx_time.Height / 2, (float)x, (panelEx_time.Height - 1));
            }
        }


        private void panelEx_time_MouseMove(object sender, MouseEventArgs e)
        {
            var totalH = (EndDate - StartDate).TotalMinutes;
            var wid = panelEx_time.Width;
            var div = wid / totalH;

            var date = StartDate.AddMinutes((int)(e.X / div));


            var newToolTip = date.ToString(CultureInfo.InvariantCulture);

            if (_loginsList1.Exists(a => a.Start < date && a.End > date))
            {
                var start = _loginsList1.Find(a => a.Start < date && a.End > date).Start;
                var end = _loginsList1.Find(a => a.Start < date && a.End > date).End;
                newToolTip += "\nDN [ " + start + " - " + end + " ]";
            }

            if (_loginsList2.Exists(a => a.Start < date && a.End > date))
            {
                var start = _loginsList2.Find(a => a.Start < date && a.End > date).Start;
                var end = _loginsList2.Find(a => a.Start < date && a.End > date).End;
                newToolTip += "\nTN [ " + start + " - " + end + " ]";
            }
            if (LineCount == 3)
            {
                if (_loginsList3.Exists(a => a.Start < date && a.End > date))
                {
                    var start = _loginsList3.Find(a => a.Start < date && a.End > date).Start;
                    var end = _loginsList3.Find(a => a.Start < date && a.End > date).End;
                    newToolTip += "\nDE [ " + start + " - " + end + " ]";
                }
            }

            if (newToolTip != _lastToolTip)
            {
                _lastToolTip = newToolTip;
                toolTip1.SetToolTip(panelEx_time, _lastToolTip);
            }

        }

        private void panelEx_back_MouseMove(object sender, MouseEventArgs e)
        {
            //labelX_date.Text = "";//StartDate.ToString(CultureInfo.InvariantCulture);
        }
        
        #endregion
    

    }

 
}
