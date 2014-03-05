using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TickNetClient.Core;
using System.Linq;
using DADataManager.Models;

namespace TickNetClient.Controls
{
    public partial class SymbolList : UserControl
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


        private int _selectedItem;
        private bool _stateChangingEnabled;

    
        public int SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value < panelEx_container.Controls.Count)
                _selectedItem = value;
                
            }
        }



        public SymbolList()
        {
            InitializeComponent();
            SelectedItem = -1;
        }


        public void AddItem(string text, int depth, string description)
        {
            var ind = panelEx_container.Controls.Count;
            var cntrl = new SymbolItem(text, ind, depth, description);            
            cntrl.ItemStopCollectingClick += cntrl_ItemStopCollectingClick;
            panelEx_container.Controls.Add(cntrl);
            //if(SelectedItem==-1) SelectFirstItem();

        }

        void cntrl_ItemStopCollectingClick(int itemIndex)
        {
            OnItemStopCollectingClick(itemIndex);
        }

 
        public void RemoveItem(int index)
        {
            SelectedItem = -1;
            bool removed=false;
            for (int i = 0; i < panelEx_container.Controls.Count; i++)
            {
                var item = panelEx_container.Controls[i];
                var styledListItemControl = item as SymbolItem;
                if (styledListItemControl != null)
                {
                    if (removed)
                    {
                        styledListItemControl.ItemIndex--;
                    }
                    else if (styledListItemControl.ItemIndex == index)
                    {
                        panelEx_container.Controls.Remove(styledListItemControl);
                        removed = true;
                        i--;
                    }
                }
            }            
        }

        public void RenameSelectedItem(string p)
        {
            var styledListItemControl = panelEx_container.Controls[SelectedItem] as SymbolItem;
            if (styledListItemControl != null)
                styledListItemControl.ItemText = p;
        }

        internal void ClearItems()
        {
            panelEx_container.Controls.Clear();
        }


        public bool StateChangingEnabled
        {
            get { return _stateChangingEnabled; }
            set
            {
                _stateChangingEnabled = value;
                for (int i = 0; i < panelEx_container.Controls.Count; i++)
                {
                    var item = panelEx_container.Controls[i];
                    var styledListItemControl = item as SymbolItem;
                    if (styledListItemControl != null)
                    {
                        styledListItemControl.ItemStateChangingEnabled = value;
                    }
                }     
            }
        }

        public List<string> GetGroups()
        {
            var res = new List<string>();

            foreach (var item in panelEx_container.Controls.OfType<SymbolItem>())
            {
                res.Add(item.ItemText);
            }

            return res;
        }


        public void SetItem(int ind, string text, int depth, string description)//, DateTime dateTime)
        {
            if (ind < panelEx_container.Controls.Count)
            {
                var cntrl = panelEx_container.Controls[ind] as SymbolItem;

                cntrl.ItemText = text;
                cntrl.ItemDescription = description;
                //cntrl.ItemDateTime = dateTime;

                cntrl.ItemDepth = depth;
            }
            else
            {
                AddItem(text, depth, description);
            }
        }

        internal void SetItemsCount(int count)
        {
            for (int i = count; i < panelEx_container.Controls.Count; i++)
            {
                panelEx_container.Controls.RemoveAt(i);
                i--;
            }
        }

        internal string GetText(int ind)
        {
            if (ind < panelEx_container.Controls.Count)
            {
                var cntrl = panelEx_container.Controls[ind] as SymbolItem;

                return cntrl.ItemText;
            }
            return "";
        }
    }
}
