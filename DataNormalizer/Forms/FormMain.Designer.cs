namespace DataNormalizer.Forms
{
    partial class FormMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.metroShellMain = new DevComponents.DotNetBar.Metro.MetroShell();
            this.metroTabPanelUsers = new DevComponents.DotNetBar.Metro.MetroTabPanel();
            this.ui_tableLayoutPanel_Symbols = new System.Windows.Forms.TableLayoutPanel();
            this.ui_panelEx_Symbols = new DevComponents.DotNetBar.PanelEx();
            this.ui_dGridViewX_Symbols = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.dGridViewColumn_Symbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uiCollectPriorityGrid = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.metroTabPanel1 = new DevComponents.DotNetBar.Metro.MetroTabPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ui_logs_dGridX_Logs = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.ui_logColumn_Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_logColumn_UserLogin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_logColumn_MsgType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_logColumn_Symbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_logColumn_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uiRefreshLogBtn = new DevComponents.DotNetBar.ButtonX();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.metroTabItem_users = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.metroTabItem1 = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.metroTabItem_symbols = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.metroTabItem_GROUPS = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.metroTabItem_logs = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.timerLogon = new System.Windows.Forms.Timer(this.components);
            this.metroShellMain.SuspendLayout();
            this.metroTabPanelUsers.SuspendLayout();
            this.ui_tableLayoutPanel_Symbols.SuspendLayout();
            this.ui_panelEx_Symbols.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_dGridViewX_Symbols)).BeginInit();
            this.metroTabPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_logs_dGridX_Logs)).BeginInit();
            this.panelEx2.SuspendLayout();
            this.SuspendLayout();
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerColorTint = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Metro;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.SteelBlue);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.NotifyIcon1Click);
            // 
            // metroShellMain
            // 
            this.metroShellMain.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.metroShellMain.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroShellMain.CanCustomize = false;
            this.metroShellMain.CaptionVisible = true;
            this.metroShellMain.Controls.Add(this.metroTabPanelUsers);
            this.metroShellMain.Controls.Add(this.metroTabPanel1);
            this.metroShellMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroShellMain.ForeColor = System.Drawing.Color.Black;
            this.metroShellMain.HelpButtonText = "LOGOUT";
            this.metroShellMain.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.metroTabItem_users,
            this.metroTabItem1});
            this.metroShellMain.KeyTipsEnabled = false;
            this.metroShellMain.KeyTipsFont = new System.Drawing.Font("Tahoma", 7F);
            this.metroShellMain.Location = new System.Drawing.Point(0, 1);
            this.metroShellMain.Margin = new System.Windows.Forms.Padding(2);
            this.metroShellMain.MouseWheelTabScrollEnabled = false;
            this.metroShellMain.Name = "metroShellMain";
            this.metroShellMain.Size = new System.Drawing.Size(588, 509);
            this.metroShellMain.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
            this.metroShellMain.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
            this.metroShellMain.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
            this.metroShellMain.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
            this.metroShellMain.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
            this.metroShellMain.SystemText.QatDialogAddButton = "&Add >>";
            this.metroShellMain.SystemText.QatDialogCancelButton = "Cancel";
            this.metroShellMain.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
            this.metroShellMain.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
            this.metroShellMain.SystemText.QatDialogOkButton = "OK";
            this.metroShellMain.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShellMain.SystemText.QatDialogRemoveButton = "&Remove";
            this.metroShellMain.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
            this.metroShellMain.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShellMain.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
            this.metroShellMain.TabIndex = 1;
            this.metroShellMain.TabStripFont = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metroShellMain.TitleText = "Data Normalizer";
            this.metroShellMain.HelpButtonClick += new System.EventHandler(this.metroShellMain_HelpButtonClick);
            this.metroShellMain.Resize += new System.EventHandler(this.MetroShellMainResize);
            // 
            // metroTabPanelUsers
            // 
            this.metroTabPanelUsers.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.metroTabPanelUsers.Controls.Add(this.ui_tableLayoutPanel_Symbols);
            this.metroTabPanelUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabPanelUsers.Location = new System.Drawing.Point(0, 51);
            this.metroTabPanelUsers.Margin = new System.Windows.Forms.Padding(2);
            this.metroTabPanelUsers.Name = "metroTabPanelUsers";
            this.metroTabPanelUsers.Padding = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this.metroTabPanelUsers.Size = new System.Drawing.Size(588, 458);
            // 
            // 
            // 
            this.metroTabPanelUsers.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanelUsers.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanelUsers.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTabPanelUsers.TabIndex = 3;
            // 
            // ui_tableLayoutPanel_Symbols
            // 
            this.ui_tableLayoutPanel_Symbols.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.ui_tableLayoutPanel_Symbols.ColumnCount = 2;
            this.ui_tableLayoutPanel_Symbols.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ui_tableLayoutPanel_Symbols.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ui_tableLayoutPanel_Symbols.Controls.Add(this.ui_panelEx_Symbols, 0, 0);
            this.ui_tableLayoutPanel_Symbols.Controls.Add(this.uiCollectPriorityGrid, 1, 0);
            this.ui_tableLayoutPanel_Symbols.Controls.Add(this.buttonX1, 1, 1);
            this.ui_tableLayoutPanel_Symbols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_tableLayoutPanel_Symbols.ForeColor = System.Drawing.Color.Black;
            this.ui_tableLayoutPanel_Symbols.Location = new System.Drawing.Point(2, 0);
            this.ui_tableLayoutPanel_Symbols.Margin = new System.Windows.Forms.Padding(2);
            this.ui_tableLayoutPanel_Symbols.Name = "ui_tableLayoutPanel_Symbols";
            this.ui_tableLayoutPanel_Symbols.RowCount = 2;
            this.ui_tableLayoutPanel_Symbols.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ui_tableLayoutPanel_Symbols.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.ui_tableLayoutPanel_Symbols.Size = new System.Drawing.Size(584, 456);
            this.ui_tableLayoutPanel_Symbols.TabIndex = 4;
            // 
            // ui_panelEx_Symbols
            // 
            this.ui_panelEx_Symbols.CanvasColor = System.Drawing.SystemColors.Control;
            this.ui_panelEx_Symbols.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_panelEx_Symbols.Controls.Add(this.ui_dGridViewX_Symbols);
            this.ui_panelEx_Symbols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_panelEx_Symbols.Location = new System.Drawing.Point(2, 2);
            this.ui_panelEx_Symbols.Margin = new System.Windows.Forms.Padding(2);
            this.ui_panelEx_Symbols.Name = "ui_panelEx_Symbols";
            this.ui_tableLayoutPanel_Symbols.SetRowSpan(this.ui_panelEx_Symbols, 2);
            this.ui_panelEx_Symbols.Size = new System.Drawing.Size(288, 452);
            this.ui_panelEx_Symbols.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.ui_panelEx_Symbols.Style.BackColor1.Color = System.Drawing.Color.White;
            this.ui_panelEx_Symbols.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.ui_panelEx_Symbols.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.ui_panelEx_Symbols.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.ui_panelEx_Symbols.Style.GradientAngle = 90;
            this.ui_panelEx_Symbols.TabIndex = 0;
            // 
            // ui_dGridViewX_Symbols
            // 
            this.ui_dGridViewX_Symbols.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ui_dGridViewX_Symbols.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ui_dGridViewX_Symbols.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_dGridViewX_Symbols.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dGridViewColumn_Symbol});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ui_dGridViewX_Symbols.DefaultCellStyle = dataGridViewCellStyle2;
            this.ui_dGridViewX_Symbols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_dGridViewX_Symbols.EnableHeadersVisualStyles = false;
            this.ui_dGridViewX_Symbols.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.ui_dGridViewX_Symbols.Location = new System.Drawing.Point(0, 0);
            this.ui_dGridViewX_Symbols.Margin = new System.Windows.Forms.Padding(2);
            this.ui_dGridViewX_Symbols.Name = "ui_dGridViewX_Symbols";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ui_dGridViewX_Symbols.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.ui_dGridViewX_Symbols.RowHeadersVisible = false;
            this.ui_dGridViewX_Symbols.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ui_dGridViewX_Symbols.RowTemplate.Height = 24;
            this.ui_dGridViewX_Symbols.Size = new System.Drawing.Size(288, 452);
            this.ui_dGridViewX_Symbols.TabIndex = 0;
            // 
            // dGridViewColumn_Symbol
            // 
            this.dGridViewColumn_Symbol.HeaderText = "Symbol";
            this.dGridViewColumn_Symbol.Name = "dGridViewColumn_Symbol";
            this.dGridViewColumn_Symbol.Width = 358;
            // 
            // uiCollectPriorityGrid
            // 
            this.uiCollectPriorityGrid.BackColor = System.Drawing.Color.White;
            this.uiCollectPriorityGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiCollectPriorityGrid.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.uiCollectPriorityGrid.ForeColor = System.Drawing.Color.Black;
            this.uiCollectPriorityGrid.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.uiCollectPriorityGrid.Location = new System.Drawing.Point(295, 3);
            this.uiCollectPriorityGrid.Name = "uiCollectPriorityGrid";
            this.uiCollectPriorityGrid.PrimaryGrid.RowHeaderWidth = 0;
            this.uiCollectPriorityGrid.Size = new System.Drawing.Size(286, 415);
            this.uiCollectPriorityGrid.TabIndex = 1;
            this.uiCollectPriorityGrid.Text = "superGridControl1";
            this.uiCollectPriorityGrid.DataBindingComplete += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridDataBindingCompleteEventArgs>(this.uiCollectPriorityGridDataBindingComplete);
            this.uiCollectPriorityGrid.AfterExpand += new System.EventHandler<DevComponents.DotNetBar.SuperGrid.GridAfterExpandEventArgs>(this.uiCollectPriorityGridAfterExpand);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonX1.Location = new System.Drawing.Point(295, 424);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(286, 29);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 2;
            this.buttonX1.Text = "REFRESH";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // metroTabPanel1
            // 
            this.metroTabPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.metroTabPanel1.Controls.Add(this.tableLayoutPanel1);
            this.metroTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabPanel1.Location = new System.Drawing.Point(0, 51);
            this.metroTabPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.metroTabPanel1.Name = "metroTabPanel1";
            this.metroTabPanel1.Padding = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this.metroTabPanel1.Size = new System.Drawing.Size(588, 458);
            // 
            // 
            // 
            this.metroTabPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTabPanel1.TabIndex = 4;
            this.metroTabPanel1.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.ui_logs_dGridX_Logs, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.uiRefreshLogBtn, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panelEx1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelEx2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.64103F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.35897F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 179F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 456);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ui_logs_dGridX_Logs
            // 
            this.ui_logs_dGridX_Logs.AllowUserToAddRows = false;
            this.ui_logs_dGridX_Logs.AllowUserToDeleteRows = false;
            this.ui_logs_dGridX_Logs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ui_logs_dGridX_Logs.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ui_logs_dGridX_Logs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.ui_logs_dGridX_Logs.ColumnHeadersHeight = 20;
            this.ui_logs_dGridX_Logs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.ui_logs_dGridX_Logs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_logColumn_Date,
            this.ui_logColumn_UserLogin,
            this.ui_logColumn_MsgType,
            this.ui_logColumn_Symbol,
            this.ui_logColumn_Status});
            this.tableLayoutPanel1.SetColumnSpan(this.ui_logs_dGridX_Logs, 2);
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ui_logs_dGridX_Logs.DefaultCellStyle = dataGridViewCellStyle5;
            this.ui_logs_dGridX_Logs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_logs_dGridX_Logs.EnableHeadersVisualStyles = false;
            this.ui_logs_dGridX_Logs.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.ui_logs_dGridX_Logs.Location = new System.Drawing.Point(3, 249);
            this.ui_logs_dGridX_Logs.Name = "ui_logs_dGridX_Logs";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ui_logs_dGridX_Logs.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.ui_logs_dGridX_Logs.RowHeadersVisible = false;
            this.ui_logs_dGridX_Logs.RowTemplate.Height = 24;
            this.ui_logs_dGridX_Logs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_logs_dGridX_Logs.Size = new System.Drawing.Size(578, 173);
            this.ui_logs_dGridX_Logs.TabIndex = 1;
            // 
            // ui_logColumn_Date
            // 
            this.ui_logColumn_Date.HeaderText = "Date";
            this.ui_logColumn_Date.Name = "ui_logColumn_Date";
            this.ui_logColumn_Date.ReadOnly = true;
            this.ui_logColumn_Date.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ui_logColumn_UserLogin
            // 
            this.ui_logColumn_UserLogin.FillWeight = 70F;
            this.ui_logColumn_UserLogin.HeaderText = "User";
            this.ui_logColumn_UserLogin.Name = "ui_logColumn_UserLogin";
            this.ui_logColumn_UserLogin.ReadOnly = true;
            // 
            // ui_logColumn_MsgType
            // 
            this.ui_logColumn_MsgType.FillWeight = 48.44585F;
            this.ui_logColumn_MsgType.HeaderText = "Event";
            this.ui_logColumn_MsgType.Name = "ui_logColumn_MsgType";
            this.ui_logColumn_MsgType.ReadOnly = true;
            // 
            // ui_logColumn_Symbol
            // 
            this.ui_logColumn_Symbol.FillWeight = 62.19339F;
            this.ui_logColumn_Symbol.HeaderText = "Symbol";
            this.ui_logColumn_Symbol.Name = "ui_logColumn_Symbol";
            this.ui_logColumn_Symbol.ReadOnly = true;
            // 
            // ui_logColumn_Status
            // 
            this.ui_logColumn_Status.FillWeight = 60F;
            this.ui_logColumn_Status.HeaderText = "Status";
            this.ui_logColumn_Status.Name = "ui_logColumn_Status";
            this.ui_logColumn_Status.ReadOnly = true;
            // 
            // uiRefreshLogBtn
            // 
            this.uiRefreshLogBtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.uiRefreshLogBtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.tableLayoutPanel1.SetColumnSpan(this.uiRefreshLogBtn, 2);
            this.uiRefreshLogBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiRefreshLogBtn.Location = new System.Drawing.Point(3, 428);
            this.uiRefreshLogBtn.Name = "uiRefreshLogBtn";
            this.uiRefreshLogBtn.Size = new System.Drawing.Size(578, 25);
            this.uiRefreshLogBtn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.uiRefreshLogBtn.TabIndex = 2;
            this.uiRefreshLogBtn.Text = "REFRESH";
            this.uiRefreshLogBtn.Click += new System.EventHandler(this.uiRefreshLogBtn_Click);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.tableLayoutPanel1.SetColumnSpan(this.panelEx1, 2);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(3, 214);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(578, 29);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 3;
            this.panelEx1.Text = "GENERAL LOGS";
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.tableLayoutPanel1.SetColumnSpan(this.panelEx2, 2);
            this.panelEx2.Controls.Add(this.labelX1);
            this.panelEx2.Controls.Add(this.listBox1);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx2.Location = new System.Drawing.Point(3, 3);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(578, 205);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 4;
            this.panelEx2.Text = "panelEx2";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Location = new System.Drawing.Point(237, 4);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(82, 16);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "ALERT LOGS";
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.Color.White;
            this.listBox1.ForeColor = System.Drawing.Color.Black;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 26);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(572, 173);
            this.listBox1.TabIndex = 0;
            // 
            // metroTabItem_users
            // 
            this.metroTabItem_users.Checked = true;
            this.metroTabItem_users.Name = "metroTabItem_users";
            this.metroTabItem_users.Panel = this.metroTabPanelUsers;
            this.metroTabItem_users.Text = "SYMBOLS";
            // 
            // metroTabItem1
            // 
            this.metroTabItem1.Name = "metroTabItem1";
            this.metroTabItem1.Panel = this.metroTabPanel1;
            this.metroTabItem1.Text = "LOGS";
            // 
            // metroTabItem_symbols
            // 
            this.metroTabItem_symbols.Checked = true;
            this.metroTabItem_symbols.Name = "metroTabItem_symbols";
            this.metroTabItem_symbols.Text = "SYMBOLS";
            // 
            // metroTabItem_GROUPS
            // 
            this.metroTabItem_GROUPS.Checked = true;
            this.metroTabItem_GROUPS.Name = "metroTabItem_GROUPS";
            this.metroTabItem_GROUPS.Text = "GROUPS";
            // 
            // metroTabItem_logs
            // 
            this.metroTabItem_logs.Checked = true;
            this.metroTabItem_logs.Name = "metroTabItem_logs";
            this.metroTabItem_logs.Text = "LOGS";
            // 
            // timerLogon
            // 
            this.timerLogon.Interval = 200;
            this.timerLogon.Tick += new System.EventHandler(this.timerLogon_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 511);
            this.Controls.Add(this.metroShellMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximumSize = new System.Drawing.Size(589, 511);
            this.MinimumSize = new System.Drawing.Size(589, 511);
            this.Name = "FormMain";
            this.Text = "`";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.metroShellMain.ResumeLayout(false);
            this.metroShellMain.PerformLayout();
            this.metroTabPanelUsers.ResumeLayout(false);
            this.ui_tableLayoutPanel_Symbols.ResumeLayout(false);
            this.ui_panelEx_Symbols.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_dGridViewX_Symbols)).EndInit();
            this.metroTabPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_logs_dGridX_Logs)).EndInit();
            this.panelEx2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.StyleManager styleManager1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private DevComponents.DotNetBar.Metro.MetroShell metroShellMain;
        private DevComponents.DotNetBar.Metro.MetroTabPanel metroTabPanelUsers;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem_users;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem_symbols;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem_GROUPS;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem_logs;
        private System.Windows.Forms.TableLayoutPanel ui_tableLayoutPanel_Symbols;
        private DevComponents.DotNetBar.PanelEx ui_panelEx_Symbols;
        private DevComponents.DotNetBar.Controls.DataGridViewX ui_dGridViewX_Symbols;
        private DevComponents.DotNetBar.Metro.MetroTabPanel metroTabPanel1;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevComponents.DotNetBar.SuperGrid.SuperGridControl uiCollectPriorityGrid;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dGridViewColumn_Symbol;
        private DevComponents.DotNetBar.Controls.DataGridViewX ui_logs_dGridX_Logs;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_logColumn_Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_logColumn_UserLogin;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_logColumn_MsgType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_logColumn_Symbol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_logColumn_Status;
        private DevComponents.DotNetBar.ButtonX uiRefreshLogBtn;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private System.Windows.Forms.ListBox listBox1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.Timer timerLogon;
    }
}

