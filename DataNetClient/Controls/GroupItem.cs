using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CQG;
using DADataManager;
using DADataManager.Models;
using DataNetClient.Core;
using DevComponents.DotNetBar.Controls;

namespace DataNetClient.Controls
{
    public partial class GroupItem : UserControl
    {
        public GroupItem()
        {
            InitializeComponent();
            Dock = DockStyle.Top;
            _isSelected = false;
        }

        //var symbols = ClientDatabaseManager.GetSymbols(_client.UserID, false);
        private bool _isAutoCollect;
        public bool ItemIsAutoCollect
        {
            get { return _isAutoCollect; }
            set
            {
                _isAutoCollect = value;
                if (_isAutoCollect) labelX_Autocollect.Text = "ON";
                else labelX_Autocollect.Text = "OFF";
            }
        }

        /*public GroupItem(string name, string timeframe, DateTime time, string conttype,Color color)
        {
            InitializeComponent();
            Dock = DockStyle.Top;
            _isSelected = false;
            ItemName = name;
            TimeFrame = timeframe;
            Time = time;
            ContType = conttype;
            ItemColor = color;
        }*/

        public Color ItemColor
        {
            get { return panelEx_Main.Style.BackColor1.Color; }
            set { panelEx_Main.Style.BackColor1.Color = value;
                expandablePanel1.Style.BackColor1.Color = value;}
        }

        public string ItemName
        {
            get { return labelX_Name.Text; }
            set { labelX_Name.Text = value; }
        }

        public string TimeFrame
        {
            get { return labelX_TimeFrame.Text; }
            set { labelX_TimeFrame.Text = value; }
        }

        public void SetCount(int col, int all)
        {
            labelX_Status_Count.Text=col+"/"+all;
        }

        private DateTime _time ;
        public DateTime Time
        {
            set { _time = value; UpdateTime(); }
            get { return _time; }
        }

        public string ContType
        {
            get { return labelX_ContType.Text; }
            set { labelX_ContType.Text = value; }
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

        private void panelEx_Main_Click(object sender, EventArgs e)
        {
            ItemIsSelected = !ItemIsSelected;
            Focus();
        }

        private void expandablePanel1_ExpandedChanged(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            if(expandablePanel1.Expanded) Height += expandablePanel1.Size.Height-26;
            
            
        }

        private List<string> _symbols;    
        public List<string> Symbols
        {
            get { return _symbols; }
            set
            {
                _symbols = value;
                listViewEx1.Controls.Clear();
                foreach (var symbolModel in _symbols)
                {
                    listViewEx1.Items.Add(symbolModel);
                }
            }
        }


        public GroupItemModel SymbolsList
        {
            set;
            get;
        }


        private void expandablePanel1_ExpandedChanging(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            if (expandablePanel1.Expanded) Height -= expandablePanel1.Size.Height - 26;
        }

        //public delegate void ItemColorChangedHandler(Status status);

        //public event ItemColorChangedHandler ItemColorChanged;

        private void StatusChanged(GroupState state)
        {
            //if (state == GroupState.Unsuccessful) ItemColor = Color.Red;
            if (state == GroupState.Finished) ItemColor = Color.Green;
            if (state == GroupState.InProgress) ItemColor = Color.Yellow;
            if (state == GroupState.InQueue) ItemColor = Color.Blue;
            if (state == GroupState.NotInQueue) ItemColor = Color.White;

            //ItemColorChangedHandler handler = ItemColorChanged;
            //if (handler != null) handler(status);
        }

        private GroupState _state;
        public GroupState ItemState
        {
            get { return _state; }
            set
            {
                _state = value;
                labelX_Status_Name.Text = _state.ToString();
                StatusChanged(_state);
            }
        }

        public void AddDay()
        {
            panelEx_Day.Controls.Add(new Day());
        }

        public void RemoveDay(int index)
        {
            if (panelEx_Day.Controls.Count - 1 >= index)
            panelEx_Day.Controls.RemoveAt(index);
        }

        public void RemoveSelectedDay()
        {
            foreach (var day in panelEx_Day.Controls.Cast<object>().OfType<Day>().Where(day => day.ItemIsSelected))
            {
                panelEx_Day.Controls.Remove(day);
            }
        }

        private void buttonX_AddDay_Click(object sender, EventArgs e)
        {
            AddDay();
        }

        private void buttonX_RemoveDay_Click(object sender, EventArgs e)
        {
            RemoveSelectedDay();
        }

        private void UpdateTime()
        {
            if (DateTime.Now.AddMinutes(-2) < Time)
                labelX_Time.Text = "just now";
            else
            {
                int totalLocalMinutes = Convert.ToInt32(Math.Floor((DateTime.Now - Time).TotalMinutes));
                int totalLocalHours = Convert.ToInt32(Math.Floor((DateTime.Now - Time).TotalHours));
                int totalLocalDays = Convert.ToInt32(Math.Floor((DateTime.Now - Time).TotalDays));

                labelX_Time.Text =
                    totalLocalMinutes < 60
                        ? totalLocalMinutes + " minutes ago"
                        : totalLocalHours < 24
                            ? totalLocalHours + " hours ago"
                            : totalLocalDays < 30
                                ? totalLocalDays + " days ago"
                                : totalLocalDays > 365 ? "-" : Time.ToShortDateString();
            }
        }

        private void timer_update_Tick(object sender, EventArgs e)
        {
            UpdateTime();
        }

    }
}

