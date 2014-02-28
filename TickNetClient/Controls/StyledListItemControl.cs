using DADataManager.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TickNetClient.Core;

namespace TickNetClient.Controls
{
    public partial class StyledListItemControl : UserControl
    {

        #region EVENTS

        public delegate void ItemSelectedChangedHandler(int itemIndex, GroupState state);

        public event ItemSelectedChangedHandler ItemSelectedChanged;

        protected virtual void OnItemSelectedChanged(int itemIndex, GroupState state)
        {
            ItemSelectedChangedHandler handler = ItemSelectedChanged;
            if (handler != null) handler(itemIndex, ItemState);
        }

        public delegate void ItemEditGroupClickHandler(int itemIndex);

        public event ItemEditGroupClickHandler ItemEditGroupClick;

        protected virtual void OnItemEditGroupClick(int itemIndex)
        {
            ItemEditGroupClickHandler handler = ItemEditGroupClick;
            if (handler != null) handler(itemIndex);
        }

        #endregion

        private Color _mainColor = Color.Green;
        private DateTime _itemDatetime;
        private List<string> _symbols;
        private DateTime _itemStartDateTime;
        private GroupState _state;
        private int _depth;
        private List<SessionModel> _sessions;
        private bool _isAutoCollect;

        #region Constractors

		public StyledListItemControl()
        {
            InitializeComponent();
            Dock = DockStyle.Top;
            labelX_count.ForeColor = Color.Black;
            _state = GroupState.NotInQueue;
            ItemStateChangingEnabled = true;

        }

        public StyledListItemControl(string text, int index,int depth, GroupState state, DateTime datetime, string count, bool isAutoCollect)
        {
            InitializeComponent();
            Dock = DockStyle.Top;
            labelX_count.ForeColor = Color.Black;

            ItemIsAutoCollect = isAutoCollect;
            ItemDepth = depth;
            ItemText = text;
            ItemIndex = index;
            ItemState = state;
            ItemDateTime = datetime;
            ItemCount = count;
            ItemStateChangingEnabled = true;
        }
         
	    #endregion

        #region PROPS

        public List<string> Symbols
        {
            get { return _symbols; }
            set
            {
                _symbols = value;
                symbolsToolStripMenuItem.DropDownItems.Clear();
                foreach (string s in value)
                {
                    symbolsToolStripMenuItem.DropDownItems.Add(s);
                }

            }
        }

        public List<SessionModel> Sessions
        {
            get { return _sessions; }
            set
            {
                _sessions = value;
                sessionsToolStripMenuItem.DropDownItems.Clear();

                foreach (var s in _sessions)
                {
                    sessionsToolStripMenuItem.DropDownItems.Add(s.Name+" ["+s.TimeStart.ToShortTimeString()+" - "+s.TimeEnd.ToShortTimeString()+"]"+(s.IsStartYesterday?" StartYesterday":""));
                }

            }
        }

        public bool ItemIsAutoCollect
        {
            get { return _isAutoCollect; }
            set
            {
                _isAutoCollect = value;
                labelX_isAutoCollectEnabled.Visible = value;
            }
        }

        public int ItemIndex { get; set; }

        public string ItemText
        {
            get { return labelX_title.Text; }
            set { labelX_title.Text = value; }
        }

        public string ItemCount
        {
            get { return labelX_count.Text; }
            set { labelX_count.Text = value; }
        }

        public int ItemDepth { get { return _depth; } set { _depth = value;
            labelX_depth.Text = "<"+value+">"; } }

        public GroupState ItemState
        {
            get { return _state; }
            set
            {
                _state = value;
                labelX_state.Text = _state.ToString();
                Redraw();
            }
        }

        public DateTime ItemStartDateTime
        {
            get { return _itemStartDateTime; }
            set
            {
                _itemStartDateTime = value;

                labelX_time1.Text =
                    value.ToShortDateString() + "  " + value.ToShortTimeString();
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



        private void panelEx_back_Click(object sender, EventArgs e)
        {
            if (!ItemStateChangingEnabled) return;
            if (ItemState == GroupState.InProgress) return;

            if (ItemState == GroupState.NotInQueue)
                ItemState = GroupState.InQueue;
            else if (ItemState == GroupState.InQueue || ItemState == GroupState.Finished)
                ItemState = GroupState.NotInQueue;                
            

            OnItemSelectedChanged(ItemIndex, ItemState);            
        }

        private void Redraw()
        {
            if (_state != GroupState.NotInQueue)
            {
                panelEx_left.BackColor =
                labelX_title.ForeColor = _mainColor;
            }
            else
            {
                panelEx_left.BackColor = Color.White;
                labelX_title.ForeColor = Color.Black;
            }
        }
        
        private void panelEx_back_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (ItemState != GroupState.NotInQueue)
            {
                panelEx_left.BackColor =
                labelX_title.ForeColor = _mainColor;                
                             
            }
            else
            {
                panelEx_left.BackColor = Color.LightGreen; 
                labelX_title.ForeColor = Color.Black;
            }
               

        }

        private void panelEx_back_MouseLeave(object sender, System.EventArgs e)
        {            
            labelX_count.ForeColor = Color.Black;
            panelEx_left.BackColor = ItemState!= GroupState.NotInQueue ? _mainColor :
            Color.White;
        }

        private void editGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnItemEditGroupClick(ItemIndex);
        }

        private void labelX_settings_MouseMove(object sender, MouseEventArgs e)
        {
            labelX_settings.SymbolColor = Color.Green;
        }

        private void labelX_settings_MouseLeave(object sender, EventArgs e)
        {
            labelX_settings.SymbolColor = Color.LightGreen;
        }


        
    }
}
