using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CQG;
using TickNetClient.Core;
using DADataManager.Models;
using DADataManager;


namespace TickNetClient.Forms
{
    public partial class EditListControl : DevComponents.DotNetBar.Controls.SlidePanel
    {
        public EditListControl(int groupId, GroupModel groupModel)
        {
            GroupId = groupId;
            AGroupModel = groupModel;            

            InitializeComponent();
        }

        public bool OpenSymbolControl;
        private MetroBillCommands _commands;
        public int GroupId { get; set; }

        public GroupModel AGroupModel { get; set; }               
        public string OldGroupName { get; private set; } 
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
                saveButton.Command = newValue.EditListCommands.Save;
                cancelButton.Command = newValue.EditListCommands.Cancel;                
            }
            else
            {
                saveButton.Command = null;
                cancelButton.Command = null;
            }
        }


        private void btnRemov_Click(object sender, EventArgs e)
        {
            var asd = lbSelList.SelectedItems;
            for (int index = 0; index < asd.Count; index++)
            {
                var item = asd[index];
                lbSelList.Items.Remove(item);
            }

            if (lbSelList.Items.Count > 0)
            {
                btnRemov.Enabled = true;               
                lbSelList.SetSelected(0, true);
            }
            else
            {
                btnRemov.Enabled = false;
                
            }
           
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            cancelButton.Command.Execute();
        }  


        private void EditListControl_Load(object sender, EventArgs e)
        {            
            OldGroupName = textBoxXListName.Text;
            ui_nudDOMDepth.Value = AGroupModel.Depth;
            checkBox_AutoCollec.Checked = AGroupModel.IsAutoModeEnabled;

            LoadSymbols();

            LoadSessions();
            
        }

        private void LoadSessions()
        {
            var sessionsList = DatabaseManager.GetSessionsInGroup(GroupId);
            foreach (var sessions in sessionsList)
            {
                var res = listViewEx_times.Items.Add(listViewEx_times.Items.Count.ToString());
                res.SubItems.Add(sessions.Name);
                res.SubItems.Add(sessions.TimeStart.ToShortTimeString());
                res.SubItems.Add(sessions.TimeEnd.ToShortTimeString());

                res.SubItems.Add(sessions.IsStartYesterday.ToString());
                res.SubItems.Add(sessions.Days);
            }
        }

        private void LoadSymbols()
        {

            var symbolsList = DatabaseManager.GetSymbolsInGroup(GroupId);
            foreach (var symbol in symbolsList)
            {
                var exist = false;
                foreach (var item in lbSelList.Items)
                {
                    if (item.ToString() == symbol.SymbolName) exist = true;
                }
                if (!exist) lbSelList.Items.Add(symbol.SymbolName);
            }
               
        }      
 

        private void buttonX_add_Click(object sender, EventArgs e)
        {
            var res = listViewEx_times.Items.Add(listViewEx_times.Items.Count.ToString());
            res.SubItems.Add(textBoxX_sessionsName.Text); 
            res.SubItems.Add(dateTimeInput1.Value.ToShortTimeString());
            res.SubItems.Add(dateTimeInput2.Value.ToShortTimeString());
            
            res.SubItems.Add(checkBox_sy.Checked.ToString());
            res.SubItems.Add(GetDaysStr());

            var sess = new SessionModel {
                Id= -1,
                Name = textBoxX_sessionsName.Text,
                IsStartYesterday = checkBox_sy.Checked,
                Days = GetDaysStr(),
                TimeStart = dateTimeInput1.Value,
                TimeEnd = dateTimeInput2.Value,
            };

            DatabaseManager.AddSessionForGroup(AGroupModel.GroupId, sess);

        }

        private string GetDaysStr()
        {
            string str="";
            str += checkBox_sun.Checked ? "S" : "_";
            str += checkBox_mon.Checked ? "M" : "_";
            str += checkBox_tue.Checked ? "T" : "_";
            str += checkBox_wed.Checked ? "W" : "_";
            str += checkBox_thu.Checked ? "T" : "_";
            str += checkBox_fri.Checked ? "F" : "_";
            str += checkBox_sat.Checked ? "S" : "_";
            return str;
        }


        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewEx_times.SelectedItems.Count <= 0) return;
            var index = listViewEx_times.SelectedIndices[0];
            var name = listViewEx_times.Items[index].SubItems[1].Text;

            listViewEx_times.Items.RemoveAt(index);
            

            var id =DatabaseManager.GetSessionsInGroup(GroupId).Find(oo=>oo.Name.ToUpper() == name.ToUpper()).Id;

            DatabaseManager.RemoveSession(GroupId, id);
        } 

        public int GetDepth()
        {
            return (int) ui_nudDOMDepth.Value;
        }

        private void comboBox_AutoCollec_CheckedChanged(object sender, EventArgs e)
        {
            panelEx1.Enabled = checkBox_AutoCollec.Checked;
        }

        public bool GetIsAutoModeEnabled()
        {
            return checkBox_AutoCollec.Checked;
        }
    }
}
