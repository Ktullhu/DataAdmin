namespace TickNetClient.Forms
{
    partial class EditListControl
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
            this.components = new System.ComponentModel.Container();
            this.btnRemov = new DevComponents.DotNetBar.ButtonX();
            this.saveButton = new DevComponents.DotNetBar.ButtonX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.lbSelList = new System.Windows.Forms.ListBox();
            this.cancelButton = new DevComponents.DotNetBar.ButtonX();
            this.labelXTitle = new DevComponents.DotNetBar.LabelX();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox_sat = new System.Windows.Forms.CheckBox();
            this.checkBox_fri = new System.Windows.Forms.CheckBox();
            this.checkBox_thu = new System.Windows.Forms.CheckBox();
            this.checkBox_wed = new System.Windows.Forms.CheckBox();
            this.checkBox_tue = new System.Windows.Forms.CheckBox();
            this.checkBox_mon = new System.Windows.Forms.CheckBox();
            this.checkBox_sun = new System.Windows.Forms.CheckBox();
            this.textBoxXListName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.listViewEx_times = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.columnHeader_No = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Name1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_TS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_TE = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_SY = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Days = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxX_sessionsName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.checkBox_sy = new System.Windows.Forms.CheckBox();
            this.buttonX_add = new DevComponents.DotNetBar.ButtonX();
            this.dateTimeInput2 = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.dateTimeInput1 = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.ui_nudDOMDepth = new System.Windows.Forms.NumericUpDown();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.checkBox_AutoCollec = new System.Windows.Forms.CheckBox();
            this.labelX_back = new DevComponents.DotNetBar.LabelX();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.panelEx3 = new DevComponents.DotNetBar.PanelEx();
            this.panelEx4 = new DevComponents.DotNetBar.PanelEx();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.comboBoxEx_existigsSessions = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.panelEx1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeInput2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeInput1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_nudDOMDepth)).BeginInit();
            this.panelEx2.SuspendLayout();
            this.panelEx3.SuspendLayout();
            this.panelEx4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRemov
            // 
            this.btnRemov.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRemov.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnRemov.Location = new System.Drawing.Point(111, 38);
            this.btnRemov.Name = "btnRemov";
            this.btnRemov.Size = new System.Drawing.Size(59, 24);
            this.btnRemov.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnRemov.TabIndex = 73;
            this.btnRemov.Text = "<";
            this.btnRemov.Click += new System.EventHandler(this.btnRemov_Click);
            // 
            // saveButton
            // 
            this.saveButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.saveButton.Location = new System.Drawing.Point(542, 404);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(105, 31);
            this.saveButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.saveButton.TabIndex = 70;
            this.saveButton.Text = "Save";
            // 
            // labelX5
            // 
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelX5.Location = new System.Drawing.Point(8, 38);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(97, 21);
            this.labelX5.TabIndex = 68;
            this.labelX5.Text = "Selected symbols:";
            // 
            // lbSelList
            // 
            this.lbSelList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbSelList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbSelList.FormattingEnabled = true;
            this.lbSelList.ItemHeight = 15;
            this.lbSelList.Location = new System.Drawing.Point(8, 74);
            this.lbSelList.Name = "lbSelList";
            this.lbSelList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbSelList.Size = new System.Drawing.Size(162, 229);
            this.lbSelList.TabIndex = 58;
            // 
            // cancelButton
            // 
            this.cancelButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.cancelButton.Location = new System.Drawing.Point(666, 404);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(105, 31);
            this.cancelButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cancelButton.TabIndex = 21;
            this.cancelButton.Text = "Cancel";
            this.toolTip1.SetToolTip(this.cancelButton, "Return without saving");
            // 
            // labelXTitle
            // 
            // 
            // 
            // 
            this.labelXTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelXTitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelXTitle.Location = new System.Drawing.Point(80, 0);
            this.labelXTitle.Name = "labelXTitle";
            this.labelXTitle.Size = new System.Drawing.Size(239, 34);
            this.labelXTitle.TabIndex = 19;
            this.labelXTitle.Text = "EDIT GROUP";
            // 
            // checkBox_sat
            // 
            this.checkBox_sat.AutoSize = true;
            this.checkBox_sat.ForeColor = System.Drawing.Color.Black;
            this.checkBox_sat.Location = new System.Drawing.Point(458, 8);
            this.checkBox_sat.Name = "checkBox_sat";
            this.checkBox_sat.Size = new System.Drawing.Size(32, 19);
            this.checkBox_sat.TabIndex = 110;
            this.checkBox_sat.Text = "S";
            this.toolTip1.SetToolTip(this.checkBox_sat, "Saturday");
            this.checkBox_sat.UseVisualStyleBackColor = true;
            // 
            // checkBox_fri
            // 
            this.checkBox_fri.AutoSize = true;
            this.checkBox_fri.ForeColor = System.Drawing.Color.Black;
            this.checkBox_fri.Location = new System.Drawing.Point(420, 8);
            this.checkBox_fri.Name = "checkBox_fri";
            this.checkBox_fri.Size = new System.Drawing.Size(32, 19);
            this.checkBox_fri.TabIndex = 109;
            this.checkBox_fri.Text = "F";
            this.toolTip1.SetToolTip(this.checkBox_fri, "Friday");
            this.checkBox_fri.UseVisualStyleBackColor = true;
            // 
            // checkBox_thu
            // 
            this.checkBox_thu.AutoSize = true;
            this.checkBox_thu.ForeColor = System.Drawing.Color.Black;
            this.checkBox_thu.Location = new System.Drawing.Point(381, 8);
            this.checkBox_thu.Name = "checkBox_thu";
            this.checkBox_thu.Size = new System.Drawing.Size(33, 19);
            this.checkBox_thu.TabIndex = 108;
            this.checkBox_thu.Text = "T";
            this.toolTip1.SetToolTip(this.checkBox_thu, "Thursday");
            this.checkBox_thu.UseVisualStyleBackColor = true;
            // 
            // checkBox_wed
            // 
            this.checkBox_wed.AutoSize = true;
            this.checkBox_wed.ForeColor = System.Drawing.Color.Black;
            this.checkBox_wed.Location = new System.Drawing.Point(335, 8);
            this.checkBox_wed.Name = "checkBox_wed";
            this.checkBox_wed.Size = new System.Drawing.Size(37, 19);
            this.checkBox_wed.TabIndex = 107;
            this.checkBox_wed.Text = "W";
            this.toolTip1.SetToolTip(this.checkBox_wed, "Wednesday");
            this.checkBox_wed.UseVisualStyleBackColor = true;
            // 
            // checkBox_tue
            // 
            this.checkBox_tue.AutoSize = true;
            this.checkBox_tue.ForeColor = System.Drawing.Color.Black;
            this.checkBox_tue.Location = new System.Drawing.Point(296, 8);
            this.checkBox_tue.Name = "checkBox_tue";
            this.checkBox_tue.Size = new System.Drawing.Size(33, 19);
            this.checkBox_tue.TabIndex = 106;
            this.checkBox_tue.Text = "T";
            this.toolTip1.SetToolTip(this.checkBox_tue, "Tuesday");
            this.checkBox_tue.UseVisualStyleBackColor = true;
            // 
            // checkBox_mon
            // 
            this.checkBox_mon.AutoSize = true;
            this.checkBox_mon.ForeColor = System.Drawing.Color.Black;
            this.checkBox_mon.Location = new System.Drawing.Point(253, 8);
            this.checkBox_mon.Name = "checkBox_mon";
            this.checkBox_mon.Size = new System.Drawing.Size(37, 19);
            this.checkBox_mon.TabIndex = 105;
            this.checkBox_mon.Text = "M";
            this.toolTip1.SetToolTip(this.checkBox_mon, "Monday");
            this.checkBox_mon.UseVisualStyleBackColor = true;
            // 
            // checkBox_sun
            // 
            this.checkBox_sun.AutoSize = true;
            this.checkBox_sun.ForeColor = System.Drawing.Color.Black;
            this.checkBox_sun.Location = new System.Drawing.Point(215, 8);
            this.checkBox_sun.Name = "checkBox_sun";
            this.checkBox_sun.Size = new System.Drawing.Size(32, 19);
            this.checkBox_sun.TabIndex = 104;
            this.checkBox_sun.Text = "S";
            this.toolTip1.SetToolTip(this.checkBox_sun, "Sunday");
            this.checkBox_sun.UseVisualStyleBackColor = true;
            // 
            // textBoxXListName
            // 
            this.textBoxXListName.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textBoxXListName.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.textBoxXListName.Border.BorderLeftColor = System.Drawing.Color.Green;
            this.textBoxXListName.Border.BorderLeftWidth = 3;
            this.textBoxXListName.Border.Class = "TextBoxBorder";
            this.textBoxXListName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxXListName.ForeColor = System.Drawing.Color.Black;
            this.textBoxXListName.Location = new System.Drawing.Point(200, 59);
            this.textBoxXListName.Name = "textBoxXListName";
            this.textBoxXListName.Size = new System.Drawing.Size(277, 23);
            this.textBoxXListName.TabIndex = 83;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelX1.Location = new System.Drawing.Point(116, 58);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 21);
            this.labelX1.TabIndex = 84;
            this.labelX1.Text = "List Name:";
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.Color.LightGreen;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.listViewEx_times);
            this.panelEx1.Enabled = false;
            this.panelEx1.Location = new System.Drawing.Point(200, 88);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(571, 158);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(250)))), ((int)(((byte)(220)))));
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.Color = System.Drawing.Color.Green;
            this.panelEx1.Style.BorderSide = DevComponents.DotNetBar.eBorderSide.Left;
            this.panelEx1.Style.BorderWidth = 3;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 90;
            // 
            // listViewEx_times
            // 
            this.listViewEx_times.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.listViewEx_times.Border.Class = "ListViewBorder";
            this.listViewEx_times.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.listViewEx_times.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_No,
            this.columnHeader_Name1,
            this.columnHeader_TS,
            this.columnHeader_TE,
            this.columnHeader_SY,
            this.columnHeader_Days});
            this.listViewEx_times.ContextMenuStrip = this.contextMenuStrip1;
            this.listViewEx_times.ForeColor = System.Drawing.Color.Black;
            this.listViewEx_times.FullRowSelect = true;
            this.listViewEx_times.Location = new System.Drawing.Point(7, 5);
            this.listViewEx_times.MultiSelect = false;
            this.listViewEx_times.Name = "listViewEx_times";
            this.listViewEx_times.Size = new System.Drawing.Size(560, 149);
            this.listViewEx_times.TabIndex = 94;
            this.listViewEx_times.UseCompatibleStateImageBehavior = false;
            this.listViewEx_times.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader_No
            // 
            this.columnHeader_No.Text = "No";
            this.columnHeader_No.Width = 35;
            // 
            // columnHeader_Name1
            // 
            this.columnHeader_Name1.Text = "Name";
            this.columnHeader_Name1.Width = 120;
            // 
            // columnHeader_TS
            // 
            this.columnHeader_TS.Text = "Time Start";
            this.columnHeader_TS.Width = 75;
            // 
            // columnHeader_TE
            // 
            this.columnHeader_TE.Text = "Time End";
            this.columnHeader_TE.Width = 75;
            // 
            // columnHeader_SY
            // 
            this.columnHeader_SY.Text = "Start Yesterday";
            this.columnHeader_SY.Width = 105;
            // 
            // columnHeader_Days
            // 
            this.columnHeader_Days.Text = "Days";
            this.columnHeader_Days.Width = 80;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // textBoxX_sessionsName
            // 
            this.textBoxX_sessionsName.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textBoxX_sessionsName.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.textBoxX_sessionsName.Border.BorderLeftColor = System.Drawing.Color.Green;
            this.textBoxX_sessionsName.Border.BorderLeftWidth = 3;
            this.textBoxX_sessionsName.Border.Class = "TextBoxBorder";
            this.textBoxX_sessionsName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX_sessionsName.ForeColor = System.Drawing.Color.Black;
            this.textBoxX_sessionsName.Location = new System.Drawing.Point(14, 7);
            this.textBoxX_sessionsName.MaxLength = 100;
            this.textBoxX_sessionsName.Name = "textBoxX_sessionsName";
            this.textBoxX_sessionsName.Size = new System.Drawing.Size(183, 23);
            this.textBoxX_sessionsName.TabIndex = 103;
            // 
            // checkBox_sy
            // 
            this.checkBox_sy.AutoSize = true;
            this.checkBox_sy.ForeColor = System.Drawing.Color.Black;
            this.checkBox_sy.Location = new System.Drawing.Point(296, 36);
            this.checkBox_sy.Name = "checkBox_sy";
            this.checkBox_sy.Size = new System.Drawing.Size(104, 19);
            this.checkBox_sy.TabIndex = 102;
            this.checkBox_sy.Text = "Start Yesterday";
            this.checkBox_sy.UseVisualStyleBackColor = true;
            // 
            // buttonX_add
            // 
            this.buttonX_add.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX_add.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX_add.Location = new System.Drawing.Point(496, 8);
            this.buttonX_add.Name = "buttonX_add";
            this.buttonX_add.Size = new System.Drawing.Size(71, 65);
            this.buttonX_add.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX_add.TabIndex = 100;
            this.buttonX_add.Text = "Add";
            this.buttonX_add.Click += new System.EventHandler(this.buttonX_add_Click);
            // 
            // dateTimeInput2
            // 
            // 
            // 
            // 
            this.dateTimeInput2.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dateTimeInput2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInput2.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this.dateTimeInput2.ButtonDropDown.Visible = true;
            this.dateTimeInput2.Format = DevComponents.Editors.eDateTimePickerFormat.ShortTime;
            this.dateTimeInput2.IsPopupCalendarOpen = false;
            this.dateTimeInput2.Location = new System.Drawing.Point(114, 36);
            // 
            // 
            // 
            this.dateTimeInput2.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateTimeInput2.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInput2.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
            this.dateTimeInput2.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dateTimeInput2.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dateTimeInput2.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimeInput2.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dateTimeInput2.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dateTimeInput2.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dateTimeInput2.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dateTimeInput2.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInput2.MonthCalendar.DisplayMonth = new System.DateTime(2013, 12, 1, 0, 0, 0, 0);
            this.dateTimeInput2.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday;
            this.dateTimeInput2.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.dateTimeInput2.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateTimeInput2.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dateTimeInput2.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimeInput2.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dateTimeInput2.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInput2.MonthCalendar.TodayButtonVisible = true;
            this.dateTimeInput2.MonthCalendar.Visible = false;
            this.dateTimeInput2.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
            this.dateTimeInput2.Name = "dateTimeInput2";
            this.dateTimeInput2.Size = new System.Drawing.Size(83, 23);
            this.dateTimeInput2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.dateTimeInput2.TabIndex = 99;
            this.dateTimeInput2.Value = new System.DateTime(2013, 12, 17, 0, 0, 0, 0);
            // 
            // dateTimeInput1
            // 
            // 
            // 
            // 
            this.dateTimeInput1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dateTimeInput1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInput1.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this.dateTimeInput1.ButtonDropDown.Visible = true;
            this.dateTimeInput1.Format = DevComponents.Editors.eDateTimePickerFormat.ShortTime;
            this.dateTimeInput1.IsPopupCalendarOpen = false;
            this.dateTimeInput1.Location = new System.Drawing.Point(14, 36);
            // 
            // 
            // 
            this.dateTimeInput1.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateTimeInput1.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInput1.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
            this.dateTimeInput1.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dateTimeInput1.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dateTimeInput1.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimeInput1.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dateTimeInput1.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dateTimeInput1.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dateTimeInput1.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dateTimeInput1.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInput1.MonthCalendar.DisplayMonth = new System.DateTime(2013, 12, 1, 0, 0, 0, 0);
            this.dateTimeInput1.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday;
            this.dateTimeInput1.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.dateTimeInput1.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateTimeInput1.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dateTimeInput1.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimeInput1.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dateTimeInput1.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInput1.MonthCalendar.TodayButtonVisible = true;
            this.dateTimeInput1.MonthCalendar.Visible = false;
            this.dateTimeInput1.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
            this.dateTimeInput1.Name = "dateTimeInput1";
            this.dateTimeInput1.Size = new System.Drawing.Size(83, 23);
            this.dateTimeInput1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.dateTimeInput1.TabIndex = 98;
            this.dateTimeInput1.Value = new System.DateTime(2013, 12, 17, 0, 0, 0, 0);
            // 
            // ui_nudDOMDepth
            // 
            this.ui_nudDOMDepth.BackColor = System.Drawing.Color.White;
            this.ui_nudDOMDepth.ForeColor = System.Drawing.Color.Black;
            this.ui_nudDOMDepth.Location = new System.Drawing.Point(111, 9);
            this.ui_nudDOMDepth.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.ui_nudDOMDepth.Name = "ui_nudDOMDepth";
            this.ui_nudDOMDepth.Size = new System.Drawing.Size(59, 23);
            this.ui_nudDOMDepth.TabIndex = 92;
            this.ui_nudDOMDepth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelX6
            // 
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelX6.Location = new System.Drawing.Point(49, 9);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(56, 21);
            this.labelX6.TabIndex = 93;
            this.labelX6.Text = "Depth";
            this.labelX6.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // checkBox_AutoCollec
            // 
            this.checkBox_AutoCollec.AutoSize = true;
            this.checkBox_AutoCollec.ForeColor = System.Drawing.Color.Black;
            this.checkBox_AutoCollec.Location = new System.Drawing.Point(619, 58);
            this.checkBox_AutoCollec.Name = "checkBox_AutoCollec";
            this.checkBox_AutoCollec.Size = new System.Drawing.Size(152, 19);
            this.checkBox_AutoCollec.TabIndex = 101;
            this.checkBox_AutoCollec.Text = "Auto collecting enabled";
            this.checkBox_AutoCollec.UseVisualStyleBackColor = true;
            this.checkBox_AutoCollec.CheckedChanged += new System.EventHandler(this.comboBox_AutoCollec_CheckedChanged);
            // 
            // labelX_back
            // 
            // 
            // 
            // 
            this.labelX_back.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX_back.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX_back.ForeColor = System.Drawing.Color.Black;
            this.labelX_back.Location = new System.Drawing.Point(6, 3);
            this.labelX_back.Name = "labelX_back";
            this.labelX_back.PaddingLeft = 6;
            this.labelX_back.Size = new System.Drawing.Size(68, 64);
            this.labelX_back.Symbol = "";
            this.labelX_back.SymbolColor = System.Drawing.Color.Green;
            this.labelX_back.SymbolSize = 50F;
            this.labelX_back.TabIndex = 102;
            this.labelX_back.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.Color.LightGreen;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx2.Controls.Add(this.btnRemov);
            this.panelEx2.Controls.Add(this.lbSelList);
            this.panelEx2.Controls.Add(this.labelX6);
            this.panelEx2.Controls.Add(this.labelX5);
            this.panelEx2.Controls.Add(this.ui_nudDOMDepth);
            this.panelEx2.Location = new System.Drawing.Point(16, 88);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(177, 310);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(250)))), ((int)(((byte)(220)))));
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.Color = System.Drawing.Color.Green;
            this.panelEx2.Style.BorderSide = DevComponents.DotNetBar.eBorderSide.Left;
            this.panelEx2.Style.BorderWidth = 3;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 103;
            // 
            // panelEx3
            // 
            this.panelEx3.CanvasColor = System.Drawing.Color.LightGreen;
            this.panelEx3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx3.Controls.Add(this.checkBox_sat);
            this.panelEx3.Controls.Add(this.textBoxX_sessionsName);
            this.panelEx3.Controls.Add(this.checkBox_fri);
            this.panelEx3.Controls.Add(this.dateTimeInput1);
            this.panelEx3.Controls.Add(this.checkBox_thu);
            this.panelEx3.Controls.Add(this.buttonX_add);
            this.panelEx3.Controls.Add(this.checkBox_sy);
            this.panelEx3.Controls.Add(this.checkBox_wed);
            this.panelEx3.Controls.Add(this.dateTimeInput2);
            this.panelEx3.Controls.Add(this.checkBox_tue);
            this.panelEx3.Controls.Add(this.checkBox_sun);
            this.panelEx3.Controls.Add(this.checkBox_mon);
            this.panelEx3.Enabled = false;
            this.panelEx3.Location = new System.Drawing.Point(200, 252);
            this.panelEx3.Name = "panelEx3";
            this.panelEx3.Size = new System.Drawing.Size(572, 76);
            this.panelEx3.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx3.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(250)))), ((int)(((byte)(220)))));
            this.panelEx3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx3.Style.BorderColor.Color = System.Drawing.Color.Green;
            this.panelEx3.Style.BorderSide = DevComponents.DotNetBar.eBorderSide.Left;
            this.panelEx3.Style.BorderWidth = 3;
            this.panelEx3.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx3.Style.GradientAngle = 90;
            this.panelEx3.TabIndex = 111;
            // 
            // panelEx4
            // 
            this.panelEx4.CanvasColor = System.Drawing.Color.LightGreen;
            this.panelEx4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx4.Controls.Add(this.buttonX1);
            this.panelEx4.Controls.Add(this.comboBoxEx_existigsSessions);
            this.panelEx4.Enabled = false;
            this.panelEx4.Location = new System.Drawing.Point(200, 334);
            this.panelEx4.Name = "panelEx4";
            this.panelEx4.Size = new System.Drawing.Size(572, 64);
            this.panelEx4.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx4.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(250)))), ((int)(((byte)(220)))));
            this.panelEx4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx4.Style.BorderColor.Color = System.Drawing.Color.Green;
            this.panelEx4.Style.BorderSide = DevComponents.DotNetBar.eBorderSide.Left;
            this.panelEx4.Style.BorderWidth = 3;
            this.panelEx4.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx4.Style.GradientAngle = 90;
            this.panelEx4.TabIndex = 112;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(496, 3);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(71, 54);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 101;
            this.buttonX1.Text = "Add";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // comboBoxEx_existigsSessions
            // 
            this.comboBoxEx_existigsSessions.DisplayMember = "Text";
            this.comboBoxEx_existigsSessions.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxEx_existigsSessions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEx_existigsSessions.FormattingEnabled = true;
            this.comboBoxEx_existigsSessions.ItemHeight = 17;
            this.comboBoxEx_existigsSessions.Location = new System.Drawing.Point(51, 20);
            this.comboBoxEx_existigsSessions.Name = "comboBoxEx_existigsSessions";
            this.comboBoxEx_existigsSessions.Size = new System.Drawing.Size(363, 23);
            this.comboBoxEx_existigsSessions.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.comboBoxEx_existigsSessions.TabIndex = 82;
            // 
            // EditListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.checkBox_AutoCollec);
            this.Controls.Add(this.panelEx4);
            this.Controls.Add(this.panelEx3);
            this.Controls.Add(this.panelEx2);
            this.Controls.Add(this.labelX_back);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.textBoxXListName);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.labelXTitle);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "EditListControl";
            this.Size = new System.Drawing.Size(800, 482);
            this.Load += new System.EventHandler(this.EditListControl_Load);
            this.panelEx1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeInput2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeInput1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_nudDOMDepth)).EndInit();
            this.panelEx2.ResumeLayout(false);
            this.panelEx3.ResumeLayout(false);
            this.panelEx3.PerformLayout();
            this.panelEx4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal DevComponents.DotNetBar.ButtonX cancelButton;
        internal DevComponents.DotNetBar.LabelX labelXTitle;
        internal System.Windows.Forms.ListBox lbSelList;
        internal DevComponents.DotNetBar.LabelX labelX5;
        internal DevComponents.DotNetBar.ButtonX saveButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private DevComponents.DotNetBar.ButtonX btnRemov;
        internal DevComponents.DotNetBar.Controls.TextBoxX textBoxXListName;
        internal DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.ColumnHeader columnHeader_No;
        private System.Windows.Forms.ColumnHeader columnHeader_Name1;
        private DevComponents.DotNetBar.ButtonX buttonX_add;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dateTimeInput2;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dateTimeInput1;
        internal DevComponents.DotNetBar.Controls.ListViewEx listViewEx_times;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.NumericUpDown ui_nudDOMDepth;
        internal DevComponents.DotNetBar.LabelX labelX6;
        private System.Windows.Forms.ColumnHeader columnHeader_TS;
        private System.Windows.Forms.ColumnHeader columnHeader_SY;
        private System.Windows.Forms.ColumnHeader columnHeader_TE;
        private System.Windows.Forms.ColumnHeader columnHeader_Days;
        internal DevComponents.DotNetBar.Controls.TextBoxX textBoxX_sessionsName;
        private System.Windows.Forms.CheckBox checkBox_sy;
        private System.Windows.Forms.CheckBox checkBox_sun;
        private System.Windows.Forms.CheckBox checkBox_sat;
        private System.Windows.Forms.CheckBox checkBox_fri;
        private System.Windows.Forms.CheckBox checkBox_thu;
        private System.Windows.Forms.CheckBox checkBox_wed;
        private System.Windows.Forms.CheckBox checkBox_tue;
        private System.Windows.Forms.CheckBox checkBox_mon;
        private DevComponents.DotNetBar.LabelX labelX_back;
        public System.Windows.Forms.CheckBox checkBox_AutoCollec;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.PanelEx panelEx3;
        private DevComponents.DotNetBar.PanelEx panelEx4;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        internal DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxEx_existigsSessions;

    }
}
