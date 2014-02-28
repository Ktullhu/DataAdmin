using System;
using CQG;
using DataNetClient.Core;

namespace DataNetClient.Forms
{
    public partial class AddListControl : DevComponents.DotNetBar.Controls.SlidePanel
    {
        public AddListControl()
        {
            InitializeComponent();
        }

        public bool OpenSymbolControl;
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
                saveButton.Command = newValue.NewListCommands.Add;
                cancelButton.Command = newValue.NewListCommands.Cancel;                
            }
            else
            {
                saveButton.Command = null;
                cancelButton.Command = null;
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            cancelButton.Command.Execute();
        }

        private void EditListControl_Load(object sender, EventArgs e)
        {
            cmbHistoricalPeriod.SelectedIndex = 0;
            cmbContinuationType.Items.Clear();
            cmbContinuationType.Items.Add(eTimeSeriesContinuationType.tsctNoContinuation);
            cmbContinuationType.Items.Add(eTimeSeriesContinuationType.tsctStandard);
            //cmbContinuationType.Items.Add(eTimeSeriesContinuationType.tsctStandardByMonth);
            //cmbContinuationType.Items.Add(eTimeSeriesContinuationType.tsctActive);
            //cmbContinuationType.Items.Add(eTimeSeriesContinuationType.tsctActiveByMonth);
            //cmbContinuationType.Items.Add(eTimeSeriesContinuationType.tsctAdjusted);
            //cmbContinuationType.Items.Add(eTimeSeriesContinuationType.tsctAdjustedByMonth);
            cmbContinuationType.SelectedIndex = 1;
            cmbHistoricalPeriod.SelectedIndex = 1;
        }
    }
}
