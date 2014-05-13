using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DADataManager.Models;

namespace DataNetClient.Controls
{
    public partial class GroupList : UserControl
    {
        public GroupList()
        {
            InitializeComponent();
        }

        private List<string> asl = new List<string>();
        public GroupItemModel ListForGroup
        {
            set;
            get;
        }

        private List<GroupModel> _groupModels=new List<GroupModel>();
        private List<GroupItemModel> _groupItems = new List<GroupItemModel>();

        public int Count
        {
            //set { _groupModels.Count = value; }
            get { return _groupModels.Count;}            
        }



        public void AddItem(GroupModel groupModel, GroupItemModel groupItems)
        {
            //_groupModels.Add();
            _groupModels.Add(groupModel);
            _groupItems.Add(groupItems);
            //panelEx1.Controls.Add(groupItem);
            //groupItem.AddDay();
            Redraw();
        }

        public delegate void ItemStateChangedHandler(int index, GroupState state);

        public event ItemStateChangedHandler ItemStateChanged;

        protected virtual void OnItemStateChanged(int index, GroupState state)
        {
            ItemStateChangedHandler handler = ItemStateChanged;
            if (handler != null) handler(index, state);
        }

        private void Redraw()
        {
            SetItemsCount(Count);
            //for (int index = 0; index < _groupModels.Count; index++)
            //{
                var item = _groupModels[Count-1];
                var gi = (GroupItem)(panelEx1.Controls[Count - 1]);
                if (gi != null)
                {
                    gi.ItemName = item.GroupName;
                    gi.TimeFrame = item.TimeFrame;
                    gi.Time = item.End;
                    gi.ContType = item.CntType;
                    gi.ItemIsSelected = false;
                    gi.ItemState = GroupState.NotInQueue;
                    gi.Symbols = _groupItems[Count - 1].AllSymbols;
                    gi.SetCount(_groupItems[Count - 1].CollectedSymbols.Count, _groupItems[Count - 1].AllSymbols.Count);
                    gi.ItemIsAutoCollect = false;
                    gi.ItemSelectedChanged += gi_ItemSelectedChanged;
                }
           // }
        }

        public void ChangeState(int index, GroupState state)
        {
            var groupItem = panelEx1.Controls[index] as GroupItem;
            if (groupItem != null)
                groupItem.ItemState = state;
        }

        public void ChangeCollectedCount(int index, int count, int totalCount)
        {
            var groupItem = panelEx1.Controls[index] as GroupItem;
            if (groupItem != null)
                groupItem.SetCount(count,totalCount);
        }

        void gi_ItemSelectedChanged(bool isSelected, int itemIndex)
        {
           
        }

        internal void SelectedNone()
        {
            for (int i = 0; i < panelEx1.Controls.Count; i++)
            {

                var groupItem = panelEx1.Controls[i] as GroupItem;
                if (groupItem != null)
                {
                    if (groupItem.ItemState != GroupState.InProgress)
                    {
                        groupItem.ItemState = GroupState.NotInQueue;
                        groupItem.Refresh();
                        OnItemStateChanged(i, GroupState.NotInQueue);
                    }
                }
            }
        }

        internal void SelectedAll()
        {
            for (int i = 0; i < panelEx1.Controls.Count; i++)
            {

                var groupItem = panelEx1.Controls[i] as GroupItem;
                if (groupItem != null)
                {
                    if (groupItem.ItemState != GroupState.InProgress)
                    {
                        groupItem.ItemState = GroupState.InQueue;
                        groupItem.Refresh();
                        OnItemStateChanged(i, GroupState.InQueue);
                    }
                }
            }
        }

        public void ClearAllItems()
        {
            panelEx1.Controls.Clear();
        }

        public void RemoveItem(int index)
        {
            if(panelEx1.Controls.Count-1>=index)
            panelEx1.Controls.RemoveAt(index);
        }

        public void RemoveSelectedItems()
        {
            foreach (var groupItem in panelEx1.Controls.Cast<object>().OfType<GroupItem>().Where(groupItem => groupItem.ItemIsSelected))
            {
                panelEx1.Controls.Remove(groupItem);
            }
        }
    
        public void SetItemsCount(int count)
        {
            while (panelEx1.Controls.Count > count)
                panelEx1.Controls.RemoveAt(panelEx1.Controls.Count - 1);

            while (panelEx1.Controls.Count < count)
                 panelEx1.Controls.Add(new GroupItem());
        }

        public List<GroupItem> GetSelectedItems()
        {
            var gr = panelEx1.Controls.Cast<object>().OfType<GroupItem>().Where(groupItem => groupItem.ItemIsSelected).ToList();
            return gr;
        }

    }
}
