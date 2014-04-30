using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using DataNetClient.Core;
using DevComponents.DotNetBar;
using DADataManager;
using DADataManager.Models;

namespace DataNetClient.Forms
{
    public partial class NewSymbolsEditControl : UserControl
    {
        public delegate void UpdateSymbolsEventHandler();
        public event UpdateSymbolsEventHandler UpdateSymbolsEvent;

        public delegate void UpdateGroupsEventHandler();
        public event UpdateGroupsEventHandler UpdateGroupsEvent;

        private void OnUpdateSymbolsEvent()
        {
            UpdateSymbolsEventHandler handler = UpdateSymbolsEvent;
            if (handler != null) handler();
        }

        private void OnUpdateGroupsEvent()
        {
            UpdateGroupsEventHandler handler = UpdateGroupsEvent;
            if (handler != null) handler();
        }

        public NewSymbolsEditControl(int userID)
        {
            _userID = userID;
            InitializeComponent();
        }

        private readonly int _userID ;
        private List<SymbolModel> _symbols;
        private List<SymbolModel> _allSymbols; 
        private List<GroupModel> _groups;
        private List<GroupModel> _allGroups;

        private MetroBillCommands _commands;        

        /// <summary>
        /// Gets or sets the commands associated with the control.
        /// </summary>
        public MetroBillCommands Commands
        {
            get { return _commands; }
            set
            {
                if (value != _commands)
                {
                    MetroBillCommands oldValue = _commands;
                    _commands = value;
                    OnCommandsChanged(oldValue, value);
                }
            }
        }
        /// <summary>
        /// Called when Commands property has changed.
        /// </summary>
        /// <param name="oldValue">Old property value</param>
        /// <param name="newValue">New property value</param>
        protected virtual void OnCommandsChanged(MetroBillCommands oldValue, MetroBillCommands newValue)
        {
            if (newValue != null)
            {
                //saveButton.Command = newValue.NewSymbolCommands.Add;
                ui_buttonX_newGroup.Command = newValue.NewSymbolCommands.NewGroup;
                ui_buttonX_editGroup.Command = newValue.NewSymbolCommands.EditGroup;
                ui_ButtonX_cancel.Command = newValue.NewSymbolCommands.Cancel;                
            }
            else
            {
                ui_ButtonX_add.Command = null;
                ui_ButtonX_cancel.Command = null;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ui_ButtonX_cancel.Command.Execute();
        }

        private void SymbolsEditControl_Load(object sender, EventArgs e)
        {
            new Thread((ThreadStart) (()=>
                                          {
                                              Thread.Sleep(200);
                                              RefreshSymbols();
                                              RefreshGroups();
                                          })).Start();
            
        }

        private void RefreshSymbols()
        {
            ui_listBox_symbols.Invoke((Action)(() => ui_listBox_symbols.Items.Clear()));

            _symbols = DatabaseManager.GetSymbols(_userID,false);
            _allSymbols = DatabaseManager.GetAllSymbols();
            foreach (var item in _symbols)
            {
                SymbolModel item1 = item;
                ui_listBox_symbols.Invoke((Action)(() =>ui_listBox_symbols.Items.Add(item1.SymbolName)));
            }
        }
        
        private void RefreshGroups()
        {
            _groups = DatabaseManager.GetGroups(_userID, ApplicationType.DataNet);
            ui_listBox_groups.Invoke((Action)(() => ui_listBox_groups.Items.Clear()));
            if (DatabaseManager.CurrentDbIsShared) _allGroups = DatabaseManager.GetAllGroups(ApplicationType.DataNet);
            foreach (var item in _groups)
            {
                var item1 = item;
                ui_listBox_groups.Invoke((Action)(() => ui_listBox_groups.Items.Add(item1.GroupName)));
            }            
        }

        private void ui_ButtonX_add_Click(object sender, EventArgs e)
        {
            if (ui_textBoxXSymbolName.Text == "")
            {
                ToastNotification.Show(this, "Please, enter new symbol name");
                return;
            }
            if(_symbols.Exists(a=>a.SymbolName==ui_textBoxXSymbolName.Text))
            {
                ToastNotification.Show(this, "The name of symbol already exists");
                return;
            }
            var newSymbol = ui_textBoxXSymbolName.Text;
            if (!DatabaseManager.CurrentDbIsShared) DatabaseManager.AddNewSymbol(newSymbol);
            if (DatabaseManager.CurrentDbIsShared)
            {
                if (!_allSymbols.Exists(a => a.SymbolName == newSymbol)) DatabaseManager.AddNewSymbol(newSymbol);
                DatabaseManager.Commit();
                var symbolId = DatabaseManager.GetAllSymbols().Find(a => a.SymbolName == newSymbol).SymbolId;
                DatabaseManager.AddSymbolForUser(_userID, symbolId, ApplicationType.DataNet);
            }
            RefreshSymbols();
            ToastNotification.Show(this, "Symbol '" + newSymbol + "' added");
            OnUpdateSymbolsEvent();
        }

        private void ui_buttonX_del_Click(object sender, EventArgs e)
        {
            var listSymbols = new List<string>();
            var list = "";
            foreach (var item in ui_listBox_symbols.SelectedItems)
            {
                listSymbols.Add(item.ToString());
                list += item + ", ";
            }
            list = list.Substring(0, list.Length - 2);

            if (listSymbols.Count == 0)
            {
                ToastNotification.Show(this, "Please, select symbol");
                return;
            }


            if (MessageBox.Show("Do you wish to delete " + listSymbols.Count + " symbol(s) [" + list + "] ?", "Deleting symbols", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {

                foreach (var item in listSymbols)
                {
                    if (!DatabaseManager.CurrentDbIsShared)
                        DatabaseManager.DeleteSymbol(_symbols.Find(a => a.SymbolName == item).SymbolId);
                    if (DatabaseManager.CurrentDbIsShared)
                    {
                        var symbolId = _allSymbols.Find(a => a.SymbolName == item).SymbolId;
                        if (DatabaseManager.IsSymbolOnlyForThisUser(symbolId))
                        {
                            DatabaseManager.DeleteSymbol(symbolId);
                        }
                        DatabaseManager.DeleteSymbolForUser(_userID, symbolId, ApplicationType.DataNet);
                    }
                }


                RefreshSymbols();
                ToastNotification.Show(this, listSymbols.Count + " symbol(s) [" + list + "] deleted");
                OnUpdateSymbolsEvent();
            }
        }

        private void ui_buttonX_replace_Click(object sender, EventArgs e)
        {
            if (ui_listBox_symbols.SelectedItem == null)
            {
                ToastNotification.Show(this, "Please, select symbol");
                return;
            }
            if (ui_textBoxXSymbolName.Text == "")
            {
                ToastNotification.Show(this, "Please, enter new symbol name");
                return;
            }
            if (_symbols.Exists(symbol => symbol.SymbolName == ui_textBoxXSymbolName.Text) || (!DatabaseManager.CurrentDbIsShared && _allSymbols.Exists(a => a.SymbolName == ui_textBoxXSymbolName.Text)))
            {
                ToastNotification.Show(this, "Symbol with this name already exists");
                return;
            }
            var ind = ui_listBox_symbols.SelectedIndex;
            var oldName = ui_listBox_symbols.SelectedItem.ToString();
            var newName = ui_textBoxXSymbolName.Text;

            if (
                MessageBox.Show("Do you wish to replace symbol " + oldName + " by " + newName + " ?", "Replacing symbol", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {


                DatabaseManager.EditSymbol(oldName, newName, _userID, ApplicationType.DataNet);
                RefreshSymbols();
                ui_listBox_symbols.SelectedIndex = ind;
                ToastNotification.Show(this, "The symbol '" + oldName + "' replaaced by '" + newName + "'");
                OnUpdateSymbolsEvent();
            }

        }

        private void ui_buttonX_join_Click(object sender, EventArgs e)
        {
            var message = "";
            var symbCount = ui_listBox_symbols.SelectedItems.Count;
            var groupCount = ui_listBox_groups.SelectedItems.Count;

            if (symbCount==0)
            {
                ToastNotification.Show(this, "Please, select symbols");
                return;
            }
            if(groupCount==0)
            {
                ToastNotification.Show(this, "Please, select groups");
                return;
            }

            for (int i = 0; i < symbCount; i++)
            {
                var currSmb = ui_listBox_symbols.SelectedItems[i].ToString();
                var currSmbId = _symbols.Find(a => a.SymbolName == currSmb).SymbolId;

                for (int j = 0; j < groupCount; j++)
                {                    
                    var currGroupName = ui_listBox_groups.SelectedItems[j].ToString();
                    var currGrp = _groups.Find(a => a.GroupName == currGroupName);
                    var currGroupId = currGrp.GroupId;
                    var currGroupSymbols = DatabaseManager.GetSymbolsInGroup(currGroupId);
                    if (!currGroupSymbols.Exists(a => a.SymbolName == currSmb))
                    {
                        var sModel = new SymbolModel { SymbolId = currSmbId, SymbolName = currSmb };

                        if (GetCntTypeOfSymbol(currSmb) == currGrp.CntType)
                            DatabaseManager.AddSymbolIntoGroup(currGroupId, sModel);
                        else
                            message += " Symbol: '" + currSmb + "' can't be added to '" + currGroupName + "' group";   
                    }
                }

                /*ui_listBox_symbolsInGroup.Items.Clear();
                var currentGroup = _groups.Find(a => a.GroupName == ui_listBox_groups.SelectedItems[0].ToString());
                if (ui_listBox_groups.SelectedItems.Count == 1 && currentGroup.Privilege != GroupPrivilege.UseGroup)
                {
                    var symbolsForCurrGroup = DatabaseManager.GetSymbolsInGroup(currentGroup.GroupId);

                    foreach (var symbolModel in symbolsForCurrGroup)
                    {
                        ui_listBox_symbolsInGroup.Items.Add(symbolModel.SymbolName);
                    }
                }*/
            }
            ToastNotification.Show(this, "Joined.");
            if (!String.IsNullOrEmpty(message))
                ToastNotification.Show(this, message);
            OnUpdateGroupsEvent();
        }


        private string GetCntTypeOfSymbol(string currSmb)
        {
            var isNoCont = false;
            var lastIndex = currSmb.Length - 1;
            var month = new List<char> { 'F', 'G', 'H', 'J', 'K', 'M', 'N', 'Q', 'U', 'V', 'X', 'Z' };

            if (Char.IsDigit(currSmb[lastIndex]) && char.IsDigit(currSmb[lastIndex - 1]) && month.Contains(currSmb.ToUpper()[lastIndex - 2]))
                isNoCont = true;
            return isNoCont ? "tsctNoContinuation" : "tsctStandard";
        }



        private void ui_buttonX_delGroup_Click(object sender, EventArgs e)
        {
            var listSymbols = new List<string>();
            var list = "";
            foreach (var item in ui_listBox_groups.SelectedItems)
            {
                listSymbols.Add(item.ToString());
                list += item + ", ";
            }
            list = list.Substring(0, list.Length - 2);

            if (listSymbols.Count == 0)
            {
                ToastNotification.Show(this, "Please, select group");
                return;
            }


            if (MessageBox.Show("Do you wish to delete " + listSymbols.Count + " group(s) [" + list + "] ?", "Deleting groups", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                foreach (var item in listSymbols)
                {
                    if (!DatabaseManager.CurrentDbIsShared)
                        DatabaseManager.DeleteGroupForUser(_userID,
                        _groups.Find(a => a.GroupName == item).GroupId, ApplicationType.DataNet.ToString());

                    if (DatabaseManager.CurrentDbIsShared)
                    {
                        var groupId = _allGroups.Find(a => a.GroupName == item).GroupId;
                        if (DatabaseManager.IsGroupOnlyForThisUser(groupId))
                        {
                            DatabaseManager.DeleteGroupOfSymbols(groupId);
                        }
                        DatabaseManager.DeleteGroupForUser(_userID, groupId, ApplicationType.DataNet.ToString());
                    }
                }

                RefreshGroups();
                ToastNotification.Show(this, listSymbols.Count + " group(s) [" + list + "] deleted");
                OnUpdateGroupsEvent();
            }
        }

        private void ui_listBox_groups_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ui_listBox_symbolsInGroup.Items.Clear();
            ui_buttonX_delGroup.Enabled = true;
            ui_buttonX_editGroup.Enabled = true;
            ui_buttonX_join.Enabled = true;
            if (ui_listBox_groups.SelectedItems.Count <= 0) return;

            var privilege = new GroupPrivilege();
            int groupId = 0;

            for (int i = 0; i < ui_listBox_groups.SelectedItems.Count; i++)
            {
                var groupName = ui_listBox_groups.SelectedItems[i].ToString();

                groupId = _groups.Find(a => a.GroupName == groupName).GroupId;
                privilege = DatabaseManager.GetUserPrivilegeForGroup(groupId, _userID, ApplicationType.DataNet.ToString());

                if (privilege != GroupPrivilege.Creator)
                {
                    ui_buttonX_delGroup.Enabled = false;
                    ui_buttonX_editGroup.Enabled = false;
                    ui_buttonX_join.Enabled = false;
                }
            }
            /*
            if (ui_listBox_groups.SelectedItems.Count == 1 && privilege != GroupPrivilege.UseGroup)
            {
                var symbolsForCurrGroup = DatabaseManager.GetSymbolsInGroup(groupId);

                foreach (var symbolModel in symbolsForCurrGroup)
                {
                    ui_listBox_symbolsInGroup.Items.Add(symbolModel.SymbolName);
                }
            }*/
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DatabaseManager.SortingModeIsAsc = !DatabaseManager.SortingModeIsAsc;
            RefreshSymbols();
            RefreshGroups();
        }


    }
}
