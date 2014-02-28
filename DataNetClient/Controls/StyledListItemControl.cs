using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DataNetClient.Core.CQGDataCollector;
using DADataManager.Models;

namespace DataNetClient.Controls
{
    public partial class StyledListItemControl : UserControl
    {
        #region EVENTS
        public delegate void ItemEditGroupClickHandler(int itemIndex);

        public event ItemEditGroupClickHandler ItemEditGroupClick;

        protected virtual void OnItemEditGroupClick(int itemIndex)
        {
            ItemEditGroupClickHandler handler = ItemEditGroupClick;
            if (handler != null) handler(itemIndex);
        }

        #endregion
        private Color _mainColor = Color.Green;
        private bool _isAutoCollect;
        private List<SessionModel> _sessions;


        public StyledListItemControl()
        {
            InitializeComponent();
            Dock = DockStyle.Top;
            labelX_count.ForeColor = Color.Black;
            ItemState = GroupState.NotInQueue;
            ItemStateChangingEnabled = true;

        }

        public StyledListItemControl(string text, int index,GroupState state, DateTime datetime, string count, string timeFrame, bool isAutoCollect)
        {
            InitializeComponent();
            Dock = DockStyle.Top;
            labelX_count.ForeColor = Color.Black;

            ItemIsAutoCollect = isAutoCollect;
            ItemText = text;
            ItemIndex = index;
            ItemState = state;
            ItemTimeframe = timeFrame;
            ItemDateTime = datetime;
            ItemCount = count;
            ItemStateChangingEnabled = true;
        }

        public delegate void ItemSelectedChangedHandler(int itemIndex, GroupState state);

        public event ItemSelectedChangedHandler ItemSelectedChanged;

        protected virtual void OnItemSelectedChanged(int itemIndex, GroupState state)
        {
            if (state != GroupState.NotInQueue)
            {
                panelEx_left.BackColor =
                labelX_title.ForeColor = _mainColor;

                
            }
            else 
            {
                panelEx_left.BackColor = Color.White;
                labelX_title.ForeColor = Color.Black;                
            }

            ItemSelectedChangedHandler handler = ItemSelectedChanged;
            if (handler != null) handler( itemIndex, ItemState);
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

        void Redraw()
        {
            if (ItemState != GroupState.NotInQueue)
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

        private void panelEx_back_MouseLeave(object sender, System.EventArgs e)
        {            
            labelX_count.ForeColor = Color.Black;
            panelEx_left.BackColor = ItemState!= GroupState.NotInQueue ? _mainColor :
            Color.White;
        }

        private void panelEx_back_Click(object sender, EventArgs e)
        {
            if (!ItemStateChangingEnabled) return;

            if (_state == GroupState.NotInQueue)
            {
                ItemState = GroupState.InQueue;
                OnItemSelectedChanged(ItemIndex, ItemState);

                return;
            }
            
            if (_state == GroupState.InQueue)
            {
                ItemState =GroupState.NotInQueue;
                OnItemSelectedChanged(ItemIndex, ItemState);

                return;
            }            
            if (_state == GroupState.Finished)
            {
                ItemState = GroupState.NotInQueue;
                OnItemSelectedChanged(ItemIndex, ItemState);

                return;
            }

            
        }

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

        public int ItemIndex { get; set; }
        private string _timeFrame;
        public string ItemTimeframe
        {
            get { return _timeFrame; }
            set
            {
                _timeFrame = value;
                labelX_tf.Text = value.ToString();
            }
        }

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

        private GroupState _state;
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

        private DateTime _itemDatetime;
        private List<string> _symbols;        

        public DateTime ItemDateTime
        {
            get { return _itemDatetime; }
            set
            {
                _itemDatetime = value;
                UpdateTime();
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

        public List<SessionModel> Sessions
        {
            get { return _sessions; }
            set
            {
                _sessions = value;
                sessionsToolStripMenuItem.DropDownItems.Clear();

                foreach (var s in _sessions)
                {
                    sessionsToolStripMenuItem.DropDownItems.Add(s.Name + " [" + s.TimeStart.ToShortTimeString() + "]");
                }

            }
        }

        public bool ItemStateChangingEnabled { get; set; }

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

        private void timer_update_Tick(object sender, EventArgs e)
        {
            UpdateTime();
        }

        void UpdateTime()
        {            
            toolTip1.SetToolTip(labelX_datetime, ItemDateTime.ToString());

            if (DateTime.Now.AddMinutes(-2) < ItemDateTime)
                labelX_datetime.Text = "just now";
            else
            {
                int totalLocalMinutes = Convert.ToInt32(Math.Floor((DateTime.Now - ItemDateTime).TotalMinutes));
                int totalLocalHours = Convert.ToInt32(Math.Floor((DateTime.Now - ItemDateTime).TotalHours));
                int totalLocalDays = Convert.ToInt32(Math.Floor((DateTime.Now - ItemDateTime).TotalDays));

                labelX_datetime.Text =
                    totalLocalMinutes < 60 ? totalLocalMinutes + " minutes ago" :
                    totalLocalHours < 24 ? totalLocalHours + " hours ago" :
                    totalLocalDays < 30 ? totalLocalDays + " days ago" :
                    totalLocalDays > 365 ? "-" : ItemDateTime.ToShortDateString();
            }           
        }
    }
}
