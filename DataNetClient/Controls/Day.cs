using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DataNetClient.Controls
{
    public partial class Day : UserControl
    {
        public Day()
        {
            InitializeComponent();
            Dock = DockStyle.Top;
            _isSelected = false;
        }

        private void labelX1_Click(object sender, EventArgs e)
        {
            labelX1.ForeColor = labelX1.ForeColor == Color.Silver ? Color.Black : Color.Silver;
        }

        private void labelX2_Click(object sender, EventArgs e)
        {
            labelX2.ForeColor = labelX2.ForeColor == Color.Silver ? Color.Black : Color.Silver;
        }

        private void labelX3_Click(object sender, EventArgs e)
        {
            labelX3.ForeColor = labelX3.ForeColor == Color.Silver ? Color.Black : Color.Silver;
        }

        private void labelX4_Click(object sender, EventArgs e)
        {
            labelX4.ForeColor = labelX4.ForeColor == Color.Silver ? Color.Black : Color.Silver;
        }

        private void labelX5_Click(object sender, EventArgs e)
        {
            labelX5.ForeColor = labelX5.ForeColor == Color.Silver ? Color.Black : Color.Silver;
        }

        private void labelX6_Click(object sender, EventArgs e)
        {
            labelX6.ForeColor = labelX6.ForeColor == Color.Silver ? Color.Black : Color.Silver;
        }

        private void labelX7_Click(object sender, EventArgs e)
        {
            labelX7.ForeColor = labelX7.ForeColor == Color.Silver ? Color.Black : Color.Silver;
        }

        /*public string Time
        {
            get { return Convert.ToDateTime(dateTimeInput1.Text).ToShortTimeString(); }
        }*/
        public DateTime Time
        {
            get { return Convert.ToDateTime(dateTimeInput1.Text); }
        }

        public delegate void ItemSelectedChangedHandler(bool isSelected, int itemIndex);

        public event ItemSelectedChangedHandler ItemSelectedChanged;

        protected virtual void OnItemSelectedChanged(bool isSelected, int itemIndex)
        {
            panelEx_isSelected.Visible = isSelected;

            ItemSelectedChangedHandler handler = ItemSelectedChanged;
            if (handler != null) handler(isSelected, itemIndex);
        }

        private int ItemIndex { get; set; }
        private bool _isSelected;
        public bool ItemIsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnItemSelectedChanged(value, ItemIndex);
            }
        }


        private void Day_Click(object sender, EventArgs e)
        {
            ItemIsSelected = !ItemIsSelected;
            Focus();
        }

        

    }
}
