using DADataManager.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TickNetClient.Core;

namespace TickNetClient.Controls
{
    public partial class SymbolItem : UserControl
    {

        #region EVENTS

        public delegate void ItemStopCollectingClickHandler(int itemIndex);

        public event ItemStopCollectingClickHandler ItemStopCollectingClick;

        protected virtual void OnItemStopCollectingClick(int itemIndex)
        {
            ItemStopCollectingClickHandler handler = ItemStopCollectingClick;
            if (handler != null) handler(itemIndex);
        }

        #endregion

        private Color _mainColor = Color.Green;
        private DateTime _itemDatetime;

        private string _description;
        private int _depth;

        #region Constractors

		public SymbolItem()
        {
            InitializeComponent();
            panelEx_left.BackColor = Color.LightGreen;
            labelX_title.ForeColor = Color.Black;
            Dock = DockStyle.Top;
            _description = "";
            ItemStateChangingEnabled = true;

        }

        public SymbolItem(string text, int index,int depth, string description)
        {
            InitializeComponent();
            Dock = DockStyle.Top;
            panelEx_left.BackColor = Color.LightGreen;
            labelX_title.ForeColor = Color.Black;

            ItemDepth = depth;
            ItemText = text;
            ItemIndex = index;
            ItemDescription = description;
            ItemStateChangingEnabled = true;

            ItemDateTime = DateTime.Now;
        }
         
	    #endregion

        #region PROPS


        public int ItemIndex { get; set; }

        public string ItemText
        {
            get { return labelX_title.Text; }
            set { labelX_title.Text = value; }
        }


        public int ItemDepth { get { return _depth; } set { _depth = value;
            labelX_depth.Text = "<"+value+">"; } }

        public string ItemDescription
        {
            get { return _description; }
            set
            {
                _description = value;
                labelX_state.Text = _description;                
            }
        }

        public DateTime ItemDateTime
        {
            get { return _itemDatetime; }
            set
            {
                _itemDatetime = value;
                labelX_time1.Text = value.ToShortDateString() + "  " + value.ToShortTimeString();
            }
        }

        public bool ItemStateChangingEnabled { get; set; }
        

        #endregion
        
        private void panelEx_back_MouseMove(object sender, MouseEventArgs e)
        {
            panelEx_back.Style.BackColor1.Color = Color.FromArgb(150,220,240,220);                  
        }

        private void panelEx_back_MouseLeave(object sender, System.EventArgs e)
        {
            panelEx_back.Style.BackColor1.Color = Color.White;

        }

        private void editGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnItemStopCollectingClick(ItemIndex);
        }

        private void labelX_settings_MouseMove(object sender, MouseEventArgs e)
        {
            panelEx_back.Style.BackColor1.Color = Color.FromArgb(150,220, 240, 220);
            labelX_settings.SymbolColor = Color.Green;
        }

        private void labelX_settings_MouseLeave(object sender, EventArgs e)
        {
            labelX_settings.SymbolColor = Color.LightGreen;
            panelEx_back.Style.BackColor1.Color = Color.White;
        }


        
    }
}
