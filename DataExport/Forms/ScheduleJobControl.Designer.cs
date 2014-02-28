using System.Windows.Forms;

namespace DataExport.Forms
{
    partial class ScheduleJobControl 
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ui_buttonX_cancel = new DevComponents.DotNetBar.ButtonX();
            this.buttonXDelete = new DevComponents.DotNetBar.ButtonX();
            this.buttonXAdd = new DevComponents.DotNetBar.ButtonX();
            this.changeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelExAddNew = new DevComponents.DotNetBar.PanelEx();
            this.daysSelectorControl1 = new DataExport.Controls.DaysSelectorControl();
            this.checkBoxX_repeatDaily = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.dateTimeInputTime = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.textBoxX_jobName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.comboItem4 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem9 = new DevComponents.Editors.ComboItem();
            this.comboItem8 = new DevComponents.Editors.ComboItem();
            this.comboItem7 = new DevComponents.Editors.ComboItem();
            this.comboItem6 = new DevComponents.Editors.ComboItem();
            this.comboItem5 = new DevComponents.Editors.ComboItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.ui_buttonX_save = new DevComponents.DotNetBar.ButtonX();
            this.elementContainerControl1 = new DataExport.Controls.ElementContainerControl();
            this.panelExAddNew.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeInputTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ui_buttonX_cancel
            // 
            this.ui_buttonX_cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_buttonX_cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_cancel.Location = new System.Drawing.Point(724, 385);
            this.ui_buttonX_cancel.Name = "ui_buttonX_cancel";
            this.ui_buttonX_cancel.Size = new System.Drawing.Size(104, 40);
            this.ui_buttonX_cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_cancel.TabIndex = 9;
            this.ui_buttonX_cancel.Text = "Close";
            // 
            // buttonXDelete
            // 
            this.buttonXDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonXDelete.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXDelete.Location = new System.Drawing.Point(535, 73);
            this.buttonXDelete.Name = "buttonXDelete";
            this.buttonXDelete.Size = new System.Drawing.Size(54, 23);
            this.buttonXDelete.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXDelete.TabIndex = 28;
            this.buttonXDelete.Text = "DELETE";
            this.buttonXDelete.Click += new System.EventHandler(this.buttonXDelete_Click);
            // 
            // buttonXAdd
            // 
            this.buttonXAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonXAdd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXAdd.Location = new System.Drawing.Point(481, 73);
            this.buttonXAdd.Name = "buttonXAdd";
            this.buttonXAdd.Size = new System.Drawing.Size(50, 23);
            this.buttonXAdd.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXAdd.TabIndex = 29;
            this.buttonXAdd.Text = "ADD";
            this.buttonXAdd.Click += new System.EventHandler(this.buttonXAdd_Click);
            // 
            // changeToolStripMenuItem
            // 
            this.changeToolStripMenuItem.Name = "changeToolStripMenuItem";
            this.changeToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.changeToolStripMenuItem.Text = "Change";
            // 
            // panelExAddNew
            // 
            this.panelExAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelExAddNew.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelExAddNew.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelExAddNew.Controls.Add(this.daysSelectorControl1);
            this.panelExAddNew.Controls.Add(this.checkBoxX_repeatDaily);
            this.panelExAddNew.Controls.Add(this.dateTimeInputTime);
            this.panelExAddNew.Controls.Add(this.labelX3);
            this.panelExAddNew.Enabled = false;
            this.panelExAddNew.Location = new System.Drawing.Point(601, 126);
            this.panelExAddNew.Name = "panelExAddNew";
            this.panelExAddNew.Size = new System.Drawing.Size(227, 253);
            this.panelExAddNew.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelExAddNew.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelExAddNew.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelExAddNew.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelExAddNew.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelExAddNew.Style.GradientAngle = 90;
            this.panelExAddNew.TabIndex = 59;
            // 
            // daysSelectorControl1
            // 
            this.daysSelectorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.daysSelectorControl1.BackColor = System.Drawing.Color.CadetBlue;
            this.daysSelectorControl1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.daysSelectorControl1.Location = new System.Drawing.Point(14, 61);
            this.daysSelectorControl1.Name = "daysSelectorControl1";
            this.daysSelectorControl1.Padding = new System.Windows.Forms.Padding(2);
            this.daysSelectorControl1.Size = new System.Drawing.Size(197, 184);
            this.daysSelectorControl1.TabIndex = 56;
            this.daysSelectorControl1.CheckedStateChanged += new DataExport.Controls.DaysSelectorControl.CheckedStateChangedHandler(this.DaysSelectorControl1_CheckedStateChanged);
            // 
            // checkBoxX_repeatDaily
            // 
            // 
            // 
            // 
            this.checkBoxX_repeatDaily.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxX_repeatDaily.Location = new System.Drawing.Point(14, 32);
            this.checkBoxX_repeatDaily.Name = "checkBoxX_repeatDaily";
            this.checkBoxX_repeatDaily.Size = new System.Drawing.Size(106, 23);
            this.checkBoxX_repeatDaily.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBoxX_repeatDaily.TabIndex = 55;
            this.checkBoxX_repeatDaily.Text = "Repeat daily";
            this.checkBoxX_repeatDaily.CheckedChanged += new System.EventHandler(this.checkBoxX_repeatDaily_CheckedChanged);
            // 
            // dateTimeInputTime
            // 
            // 
            // 
            // 
            this.dateTimeInputTime.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dateTimeInputTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputTime.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this.dateTimeInputTime.ButtonDropDown.Visible = true;
            this.dateTimeInputTime.Format = DevComponents.Editors.eDateTimePickerFormat.LongTime;
            this.dateTimeInputTime.IsPopupCalendarOpen = false;
            this.dateTimeInputTime.Location = new System.Drawing.Point(67, 4);
            // 
            // 
            // 
            this.dateTimeInputTime.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateTimeInputTime.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputTime.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
            this.dateTimeInputTime.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dateTimeInputTime.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dateTimeInputTime.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimeInputTime.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dateTimeInputTime.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dateTimeInputTime.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dateTimeInputTime.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dateTimeInputTime.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputTime.MonthCalendar.DisplayMonth = new System.DateTime(2013, 8, 1, 0, 0, 0, 0);
            this.dateTimeInputTime.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday;
            this.dateTimeInputTime.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.dateTimeInputTime.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateTimeInputTime.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dateTimeInputTime.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimeInputTime.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dateTimeInputTime.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputTime.MonthCalendar.TodayButtonVisible = true;
            this.dateTimeInputTime.MonthCalendar.Visible = false;
            this.dateTimeInputTime.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
            this.dateTimeInputTime.Name = "dateTimeInputTime";
            this.dateTimeInputTime.Size = new System.Drawing.Size(144, 22);
            this.dateTimeInputTime.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.dateTimeInputTime.TabIndex = 54;
            this.dateTimeInputTime.Value = new System.DateTime(2013, 8, 23, 0, 0, 0, 0);
            this.dateTimeInputTime.ValueChanged += new System.EventHandler(this.dateTimeInput2_ValueChanged);
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX3.ForeColor = System.Drawing.Color.DimGray;
            this.labelX3.Location = new System.Drawing.Point(14, 3);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(47, 23);
            this.labelX3.TabIndex = 52;
            this.labelX3.Text = "time";
            // 
            // textBoxX_jobName
            // 
            this.textBoxX_jobName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxX_jobName.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textBoxX_jobName.Border.Class = "TextBoxBorder";
            this.textBoxX_jobName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX_jobName.ButtonCustom.Text = "Save";
            this.textBoxX_jobName.ForeColor = System.Drawing.Color.Black;
            this.textBoxX_jobName.Location = new System.Drawing.Point(601, 98);
            this.textBoxX_jobName.Name = "textBoxX_jobName";
            this.textBoxX_jobName.ReadOnly = true;
            this.textBoxX_jobName.Size = new System.Drawing.Size(227, 22);
            this.textBoxX_jobName.TabIndex = 14;
            this.textBoxX_jobName.ButtonCustomClick += new System.EventHandler(this.textBoxX_formulaName_ButtonCustomClick);
            this.textBoxX_jobName.TextChanged += new System.EventHandler(this.textBoxX_jobName_TextChanged);
            // 
            // labelX2
            // 
            this.labelX2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX2.ForeColor = System.Drawing.Color.DimGray;
            this.labelX2.Location = new System.Drawing.Point(601, 72);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(227, 23);
            this.labelX2.TabIndex = 15;
            this.labelX2.Text = "current job name:";
            // 
            // comboItem4
            // 
            this.comboItem4.Text = "Close";
            // 
            // comboItem3
            // 
            this.comboItem3.Text = "Low";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "High";
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "Open";
            // 
            // comboItem9
            // 
            this.comboItem9.Text = "Time";
            // 
            // comboItem8
            // 
            this.comboItem8.Text = "Close";
            // 
            // comboItem7
            // 
            this.comboItem7.Text = "Low";
            // 
            // comboItem6
            // 
            this.comboItem6.Text = "High";
            // 
            // comboItem5
            // 
            this.comboItem5.Text = "Open";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DataExport.Properties.Resources.backbutton1;
            this.pictureBox1.Location = new System.Drawing.Point(20, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(44, 44);
            this.pictureBox1.TabIndex = 62;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelX1.ForeColor = System.Drawing.Color.CadetBlue;
            this.labelX1.Location = new System.Drawing.Point(70, 9);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(142, 36);
            this.labelX1.TabIndex = 61;
            this.labelX1.Text = "Schedule Job";
            // 
            // ui_buttonX_save
            // 
            this.ui_buttonX_save.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_buttonX_save.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.ui_buttonX_save.Location = new System.Drawing.Point(601, 385);
            this.ui_buttonX_save.Name = "ui_buttonX_save";
            this.ui_buttonX_save.Size = new System.Drawing.Size(97, 40);
            this.ui_buttonX_save.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_save.Symbol = "";
            this.ui_buttonX_save.TabIndex = 63;
            this.ui_buttonX_save.Text = "SAVE";
            // 
            // elementContainerControl1
            // 
            this.elementContainerControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.elementContainerControl1.AutoScroll = true;
            this.elementContainerControl1.BackColor = System.Drawing.Color.White;
            this.elementContainerControl1.ElementHeight = 32;
            this.elementContainerControl1.ElementsColor = System.Drawing.Color.CadetBlue;
            this.elementContainerControl1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.elementContainerControl1.Location = new System.Drawing.Point(31, 70);
            this.elementContainerControl1.MinimumSize = new System.Drawing.Size(170, 2);
            this.elementContainerControl1.Name = "elementContainerControl1";
            this.elementContainerControl1.Size = new System.Drawing.Size(561, 309);
            this.elementContainerControl1.TabIndex = 60;
            this.elementContainerControl1.Title = "Existing Jobs";
            this.elementContainerControl1.SelectedIndexChanged += new DataExport.Controls.ElementContainerControl.SelectedIndexChangedHandler(this.elementContainerControl1_SelectedIndexChanged);
            // 
            // ScheduleJobControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ui_buttonX_save);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.textBoxX_jobName);
            this.Controls.Add(this.panelExAddNew);
            this.Controls.Add(this.ui_buttonX_cancel);
            this.Controls.Add(this.buttonXAdd);
            this.Controls.Add(this.buttonXDelete);
            this.Controls.Add(this.elementContainerControl1);
            this.Controls.Add(this.labelX2);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "ScheduleJobControl";
            this.Size = new System.Drawing.Size(850, 470);
            this.SlideOutButtonVisible = false;
            this.Load += new System.EventHandler(this.StartControlLoad);
            this.panelExAddNew.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeInputTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX ui_buttonX_cancel;
        private DevComponents.DotNetBar.ButtonX buttonXDelete;
        private DevComponents.DotNetBar.ButtonX buttonXAdd;
        private System.Windows.Forms.ToolStripMenuItem changeToolStripMenuItem;
        private DevComponents.DotNetBar.PanelEx panelExAddNew;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX_jobName;
        private DevComponents.Editors.ComboItem comboItem4;
        private DevComponents.Editors.ComboItem comboItem3;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem1;
        private Controls.ElementContainerControl elementContainerControl1;
        private DevComponents.Editors.ComboItem comboItem9;
        private DevComponents.Editors.ComboItem comboItem8;
        private DevComponents.Editors.ComboItem comboItem7;
        private DevComponents.Editors.ComboItem comboItem6;
        private DevComponents.Editors.ComboItem comboItem5;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dateTimeInputTime;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX_repeatDaily;
        internal PictureBox pictureBox1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_save;
        private Controls.DaysSelectorControl daysSelectorControl1;
    }
}
