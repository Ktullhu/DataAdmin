using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DataExport.Core;
using DataExport.Core.ProfileManagement;
using DADataManager.ExportModels;

namespace DataExport.Forms
{
    public partial class ScheduleJobControl : DevComponents.DotNetBar.Controls.SlidePanel
    {        
        private readonly List<SheduleJobModel> _schedulas = new List<SheduleJobModel>();
        private MetroBillCommands _commands;
        private bool _stopUpdate;

        public List<SheduleJobModel> Schedulas
        {
            get { return _schedulas.ToList(); }

        }

        #region HANDLES



        public ScheduleJobControl(MetroBillCommands commands)
        {
            InitializeComponent();    
            Commands = commands;            
 
            var schedulas = ProfilesManager.CurrentProfile.GetSheduleTimes();

            foreach (var item in schedulas)
            {
                AddSchedule(item);
            }                         
        }



                
        /// <summary>
        /// Gets or sets the commands associated with the control.
        /// </summary>
        private MetroBillCommands Commands
        {
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
                ui_buttonX_save.Command = newValue.ScheduleJobControlCommands.Save;
                ui_buttonX_cancel.Command = newValue.ScheduleJobControlCommands.Cancel;
            }
            else
            {
                ui_buttonX_save.Command = null;
                ui_buttonX_cancel.Command = null;
            }
        }

        private void StartControlLoad(object sender, EventArgs e)
        {
            labelX1.ForeColor = Color.CadetBlue;            
            elementContainerControl1.Repaint();
            daysSelectorControl1.BackColor = Color.CadetBlue;
            daysSelectorControl1.Repaint();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ui_buttonX_cancel.Command.Execute();
        }

        #endregion

        #region ScheduleJob

        private void textBoxX_jobName_TextChanged(object sender, EventArgs e)
        {
            var index = elementContainerControl1.SelectedIndex;
            if (index < 0) return;

            if (_schedulas[index].Name != textBoxX_jobName.Text && (!string.IsNullOrEmpty(textBoxX_jobName.Text)))
            {
                textBoxX_jobName.ButtonCustom.Visible = true;
                textBoxX_jobName.Modified = true;
            }
            else
            {
                textBoxX_jobName.ButtonCustom.Visible = false;
                textBoxX_jobName.Modified = false;
            }
        }

        private void textBoxX_formulaName_ButtonCustomClick(object sender, EventArgs e)
        {
            textBoxX_jobName.Modified = false;
            textBoxX_jobName.ButtonCustom.Visible = false;
            if (elementContainerControl1.SelectedIndex != -1)
            {                
                
                var index = elementContainerControl1.SelectedIndex;
                var oldName = elementContainerControl1.GetText(index);
                var newName = textBoxX_jobName.Text;

                if (oldName != newName)
                {
                    
                    if(!_schedulas.Exists(a => a.Name == newName))
                    {
                        var item = _schedulas[index];
                        item.Name = newName;
                        _schedulas[index] = item;
                        elementContainerControl1.SetText(index, newName);
                    }
                    else
                    {
                        var i = 1;
                        while (_schedulas.Exists(a => a.Name == newName + " " + i))
                        {
                            i++;
                        }
                        var item = _schedulas[index];
                        item.Name = newName + " " + i;
                        _schedulas[index] = item;                        
                        elementContainerControl1.SetText(index, newName + " " + i);
                        textBoxX_jobName.Text = newName + " " + i;
                    }                         
                }
               
            }
            
        } 
        
        #endregion

        #region Add Edit Delete

        private void buttonXDelete_Click(object sender, EventArgs e)
        {
            if(elementContainerControl1.SelectedIndex != -1)
            {                
                var index = elementContainerControl1.SelectedIndex;
                _schedulas.Remove(_schedulas[index]);
                elementContainerControl1.RemoveElement(index);                
            }
        }

        private void buttonXAdd_Click(object sender, EventArgs e)
        {
            const string fname = "new schedule ";
            var i = 1;
            while (_schedulas.Exists(a=>a.Name == fname+i))
            {
                i++;
            }

            AddSchedule(fname+i);
        }

        private void AddSchedule(string fname)
        {
            var item = new SheduleJobModel
            {                
                Name = fname,
                IsDaily = false,
                ProfileId = ProfilesManager.CurrentProfile.Parameters.ProfileId,
                Date = DateTime.Now,
                SelectedDays = new List<int>()
            };
            AddSchedule(item);
        }

        private void AddSchedule(SheduleJobModel schdueModel)
        {
            _schedulas.Add(schdueModel);
            elementContainerControl1.AddElement(schdueModel.Name);
        }

        private void elementContainerControl1_SelectedIndexChanged(object sender, Controls.ElementEventArgs e)
        {
            if (elementContainerControl1.SelectedIndex != -1)
            {
                textBoxX_jobName.Text = elementContainerControl1.GetText(elementContainerControl1.SelectedIndex);
                panelExAddNew.Enabled = true;
                textBoxX_jobName.ReadOnly = false;
                var index = e.Index;

                _stopUpdate = true;                
                textBoxX_jobName.Text = _schedulas[index].Name;
                checkBoxX_repeatDaily.CheckValue = _schedulas[index].IsDaily;
                dateTimeInputTime.Value = _schedulas[index].Date;
                SetCheckedDays(_schedulas[index].SelectedDays);           
                _stopUpdate = false;
            }
            else
            {
                panelExAddNew.Enabled = false;
                textBoxX_jobName.ReadOnly = true;
                textBoxX_jobName.Text = "";           
            }

        }

        #endregion

        void DaysSelectorControl1_CheckedStateChanged()
        {
            if (_stopUpdate) return;

            var index = elementContainerControl1.SelectedIndex;
            var item = _schedulas[index];

            item.SelectedDays = GetCheckedDays();           

            _schedulas[index] = item;
        }

        private void SetCheckedDays(List<int> selectedDays)
        {
            if (daysSelectorControl1==null) return;

            for (int i = 0; i < 7; i++)
            {
                daysSelectorControl1.SetCheckedState(i, selectedDays.Contains(i));
            }
        }

        private List<int> GetCheckedDays()
        {
            var result = new List<int>();

            for (int i = 0; i < 7; i++)
            {
                if(daysSelectorControl1.GetCheckedState(i))
                {
                    result.Add(i);
                }
            }

            return result;
        }

        private void dateTimeInput2_ValueChanged(object sender, EventArgs e)
        {
            if(_stopUpdate) return;
            var index = elementContainerControl1.SelectedIndex;

            var item = _schedulas[index];
            
            item.Date = dateTimeInputTime.Value;
            
            _schedulas[index] = item;
        }

        private void checkBoxX_repeatDaily_CheckedChanged(object sender, EventArgs e)
        {
            var isChecked = checkBoxX_repeatDaily.CheckState == CheckState.Checked;

            daysSelectorControl1.Enabled = !isChecked;

            if (_stopUpdate) return;

            var index = elementContainerControl1.SelectedIndex;            
            var item = _schedulas[index];

            item.Date = dateTimeInputTime.Value;

            item.IsDaily = isChecked;

            _schedulas[index] = item;
        }

    }
}
