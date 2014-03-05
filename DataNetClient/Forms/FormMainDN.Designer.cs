using DataAdminCommonLib;
using Hik.Communication.ScsServices.Client;

namespace DataNetClient.Forms
{
    partial class FormMainDN
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("10/12/2013", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("13/12/12", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("14/12/12", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("10/12/2013", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("13/12/12", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("14/12/12", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup7 = new System.Windows.Forms.ListViewGroup("10/12/2013", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup8 = new System.Windows.Forms.ListViewGroup("13/12/12", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup9 = new System.Windows.Forms.ListViewGroup("14/12/12", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMainDN));
            this.metroStatusBar1 = new DevComponents.DotNetBar.Metro.MetroStatusBar();
            this.ui__status_labelItem_status = new DevComponents.DotNetBar.LabelItem();
            this.labelItem_collecting = new DevComponents.DotNetBar.LabelItem();
            this.ui_status_labelItemStatusSB = new DevComponents.DotNetBar.LabelItem();
            this.labelItemStatusCQG = new DevComponents.DotNetBar.LabelItem();
            this.progressBarItemCollecting = new DevComponents.DotNetBar.ProgressBarItem();
            this.labelItemUserName = new DevComponents.DotNetBar.LabelItem();
            this.labelItem_server = new DevComponents.DotNetBar.LabelItem();
            this.metroShell1 = new DevComponents.DotNetBar.Metro.MetroShell();
            this.metroTabPanel3 = new DevComponents.DotNetBar.Metro.MetroTabPanel();
            this.tableLayoutPanel_missingBar = new System.Windows.Forms.TableLayoutPanel();
            this.listViewResult = new System.Windows.Forms.ListView();
            this.columnHeaderDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStartDay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStart = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderEndDay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderEnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.metroTilePanel1 = new DevComponents.DotNetBar.Metro.MetroTilePanel();
            this.ui_metroTileItem_missingBar = new DevComponents.DotNetBar.Metro.MetroTileItem();
            this.ui_missingbar_panelEx_symbolsBack = new DevComponents.DotNetBar.PanelEx();
            this.ui_missingbars_panelEx_list_back = new DevComponents.DotNetBar.PanelEx();
            this.ui_listBox_symbolsForMissing = new System.Windows.Forms.ListBox();
            this.contextMenuStripTables = new System.Windows.Forms.ContextMenuStrip();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.labelX17 = new DevComponents.DotNetBar.LabelX();
            this.metroTabPanel5 = new DevComponents.DotNetBar.Metro.MetroTabPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelEx11 = new DevComponents.DotNetBar.PanelEx();
            this.panelEx12 = new DevComponents.DotNetBar.PanelEx();
            this.buttonX_daily_updateValues = new DevComponents.DotNetBar.ButtonX();
            this.buttonX_daily_getValues = new DevComponents.DotNetBar.ButtonX();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelEx9 = new DevComponents.DotNetBar.PanelEx();
            this.panelEx10 = new DevComponents.DotNetBar.PanelEx();
            this.listBox_daily_symbols = new System.Windows.Forms.ListBox();
            this.labelX20 = new DevComponents.DotNetBar.LabelX();
            this.metroTabPanel2 = new DevComponents.DotNetBar.Metro.MetroTabPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelEx3 = new DevComponents.DotNetBar.PanelEx();
            this.linkLabel_sort_time = new System.Windows.Forms.LinkLabel();
            this.linkLabel_sort_tf = new System.Windows.Forms.LinkLabel();
            this.linkLabel_sort_name = new System.Windows.Forms.LinkLabel();
            this.linkLabel_selectNone = new System.Windows.Forms.LinkLabel();
            this.linkLabel_selectAll = new System.Windows.Forms.LinkLabel();
            this.panelEx6 = new DevComponents.DotNetBar.PanelEx();
            this.styledListControl1 = new DataNetClient.Controls.StyledListControl();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.panelEx4 = new DevComponents.DotNetBar.PanelEx();
            this.panelEx7 = new DevComponents.DotNetBar.PanelEx();
            this.label1 = new System.Windows.Forms.Label();
            this.switchButton_changeMode = new DevComponents.DotNetBar.Controls.SwitchButton();
            this.buttonX_StartCollectGroups = new DevComponents.DotNetBar.ButtonX();
            this.buttonX_StartCollectSymbols = new DevComponents.DotNetBar.ButtonX();
            this.buttonX_stopCollecting = new DevComponents.DotNetBar.ButtonX();
            this.labelX15 = new DevComponents.DotNetBar.LabelX();
            this.ui_checkBoxAuto_CheckForMissedBars = new DevComponents.DotNetBar.Controls.SwitchButton();
            this.panelExBARS = new DevComponents.DotNetBar.PanelEx();
            this.cmbContinuationType = new System.Windows.Forms.ComboBox();
            this.cmbHistoricalPeriod = new System.Windows.Forms.ComboBox();
            this.labelX14 = new DevComponents.DotNetBar.LabelX();
            this.labelX13 = new DevComponents.DotNetBar.LabelX();
            this.nudStartBar = new System.Windows.Forms.NumericUpDown();
            this.nudEndBar = new System.Windows.Forms.NumericUpDown();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.labelX12 = new DevComponents.DotNetBar.LabelX();
            this.rdb31 = new System.Windows.Forms.RadioButton();
            this.rdb1 = new System.Windows.Forms.RadioButton();
            this.panelExTimeInterval = new DevComponents.DotNetBar.PanelEx();
            this.dateTimeInputStart = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.dateTimeInputEnd = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.grbDataCollectType = new DevComponents.DotNetBar.PanelEx();
            this.radioButtonTick = new System.Windows.Forms.RadioButton();
            this.radioButBars = new System.Windows.Forms.RadioButton();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel_edit = new System.Windows.Forms.LinkLabel();
            this.panelEx8 = new DevComponents.DotNetBar.PanelEx();
            this.ui_listBox_symbols = new System.Windows.Forms.ListBox();
            this.contextMenuStripSymbols = new System.Windows.Forms.ContextMenuStrip();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unselectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.ui_ToolStripMenuItem_EditSymbols = new System.Windows.Forms.ToolStripMenuItem();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.metroTabPanel4 = new DevComponents.DotNetBar.Metro.MetroTabPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.listViewLogger = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.metroTabPanel1 = new DevComponents.DotNetBar.Metro.MetroTabPanel();
            this.panelEx5 = new DevComponents.DotNetBar.PanelEx();
            this.ui_LabelX_sharedAvaliable = new DevComponents.DotNetBar.LabelX();
            this.ui_buttonX_shareConnect = new DevComponents.DotNetBar.ButtonX();
            this.labelX16 = new DevComponents.DotNetBar.LabelX();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.labelX19 = new DevComponents.DotNetBar.LabelX();
            this.ui_home_textBoxX_db_historical = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX18 = new DevComponents.DotNetBar.LabelX();
            this.ui_home_textBoxX_db_bar = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.ui_LabelX_localAvaliable = new DevComponents.DotNetBar.LabelX();
            this.checkBoxX1 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.ui_buttonX_localConnect = new DevComponents.DotNetBar.ButtonX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.ui_home_textBoxX_pwd = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.ui_home_textBoxX_uid = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.ui_home_textBoxX_db = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.ui_home_textBoxX_host = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.metroAppButton1 = new DevComponents.DotNetBar.Metro.MetroAppButton();
            this.metroTabItem1 = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.metroTabItem2 = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.metroTabItem3 = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.metroTabItem5 = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.metroTabItem4 = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.contextMenuStripGroups = new System.Windows.Forms.ContextMenuStrip();
            this.toolStripMenuItem6_selectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_unselectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.editListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.metroShell1.SuspendLayout();
            this.metroTabPanel3.SuspendLayout();
            this.tableLayoutPanel_missingBar.SuspendLayout();
            this.ui_missingbar_panelEx_symbolsBack.SuspendLayout();
            this.ui_missingbars_panelEx_list_back.SuspendLayout();
            this.contextMenuStripTables.SuspendLayout();
            this.metroTabPanel5.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panelEx11.SuspendLayout();
            this.panelEx12.SuspendLayout();
            this.panelEx9.SuspendLayout();
            this.panelEx10.SuspendLayout();
            this.metroTabPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelEx3.SuspendLayout();
            this.panelEx6.SuspendLayout();
            this.panelEx4.SuspendLayout();
            this.panelEx7.SuspendLayout();
            this.panelExBARS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEndBar)).BeginInit();
            this.panelExTimeInterval.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeInputStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeInputEnd)).BeginInit();
            this.grbDataCollectType.SuspendLayout();
            this.panelEx2.SuspendLayout();
            this.panelEx8.SuspendLayout();
            this.contextMenuStripSymbols.SuspendLayout();
            this.metroTabPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.metroTabPanel1.SuspendLayout();
            this.panelEx5.SuspendLayout();
            this.panelEx1.SuspendLayout();
            this.contextMenuStripGroups.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroStatusBar1
            // 
            this.metroStatusBar1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.metroStatusBar1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroStatusBar1.ContainerControlProcessDialogKey = true;
            this.metroStatusBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.metroStatusBar1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metroStatusBar1.ForeColor = System.Drawing.Color.Black;
            this.metroStatusBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.ui__status_labelItem_status,
            this.labelItem_collecting,
            this.ui_status_labelItemStatusSB,
            this.labelItemStatusCQG,
            this.progressBarItemCollecting,
            this.labelItemUserName,
            this.labelItem_server});
            this.metroStatusBar1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.metroStatusBar1.Location = new System.Drawing.Point(0, 478);
            this.metroStatusBar1.Name = "metroStatusBar1";
            this.metroStatusBar1.Size = new System.Drawing.Size(799, 21);
            this.metroStatusBar1.TabIndex = 1;
            this.metroStatusBar1.Text = "metroStatusBar1";
            // 
            // ui__status_labelItem_status
            // 
            this.ui__status_labelItem_status.Name = "ui__status_labelItem_status";
            this.ui__status_labelItem_status.Text = "READY";
            this.ui__status_labelItem_status.TextChanged += new System.EventHandler(this.ui__status_labelItem_status_TextChanged);
            // 
            // labelItem_collecting
            // 
            this.labelItem_collecting.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.labelItem_collecting.Name = "labelItem_collecting";
            this.labelItem_collecting.PaddingLeft = 5;
            this.labelItem_collecting.Text = "Stoped";
            this.labelItem_collecting.TextAlignment = System.Drawing.StringAlignment.Far;
            this.labelItem_collecting.Tooltip = "Collecting process";
            // 
            // ui_status_labelItemStatusSB
            // 
            this.ui_status_labelItemStatusSB.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.ui_status_labelItemStatusSB.Name = "ui_status_labelItemStatusSB";
            this.ui_status_labelItemStatusSB.PaddingLeft = 5;
            this.ui_status_labelItemStatusSB.Text = "Not connected";
            this.ui_status_labelItemStatusSB.TextAlignment = System.Drawing.StringAlignment.Far;
            this.ui_status_labelItemStatusSB.Tooltip = "DB connection status";
            // 
            // labelItemStatusCQG
            // 
            this.labelItemStatusCQG.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.labelItemStatusCQG.Name = "labelItemStatusCQG";
            this.labelItemStatusCQG.PaddingLeft = 5;
            this.labelItemStatusCQG.PaddingRight = 5;
            this.labelItemStatusCQG.Text = "CQG not started";
            this.labelItemStatusCQG.Tooltip = "CQG status";
            // 
            // progressBarItemCollecting
            // 
            // 
            // 
            // 
            this.progressBarItemCollecting.BackStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.progressBarItemCollecting.ChunkGradientAngle = 0F;
            this.progressBarItemCollecting.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.progressBarItemCollecting.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.progressBarItemCollecting.Name = "progressBarItemCollecting";
            this.progressBarItemCollecting.RecentlyUsed = false;
            this.progressBarItemCollecting.Value = 100;
            this.progressBarItemCollecting.ValueChanged += new System.EventHandler(this.progressBarItemCollecting_ValueChanged);
            // 
            // labelItemUserName
            // 
            this.labelItemUserName.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.labelItemUserName.Name = "labelItemUserName";
            this.labelItemUserName.PaddingLeft = 5;
            this.labelItemUserName.PaddingRight = 5;
            this.labelItemUserName.Text = "<user>";
            this.labelItemUserName.Tooltip = "User name";
            // 
            // labelItem_server
            // 
            this.labelItem_server.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.labelItem_server.Name = "labelItem_server";
            this.labelItem_server.PaddingLeft = 5;
            this.labelItem_server.Text = "Master";
            this.labelItem_server.TextAlignment = System.Drawing.StringAlignment.Far;
            this.labelItem_server.Tooltip = "Collecting process";
            // 
            // metroShell1
            // 
            this.metroShell1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.metroShell1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroShell1.CanCustomize = false;
            this.metroShell1.CaptionVisible = true;
            this.metroShell1.Controls.Add(this.metroTabPanel2);
            this.metroShell1.Controls.Add(this.metroTabPanel3);
            this.metroShell1.Controls.Add(this.metroTabPanel5);
            this.metroShell1.Controls.Add(this.metroTabPanel4);
            this.metroShell1.Controls.Add(this.metroTabPanel1);
            this.metroShell1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroShell1.ForeColor = System.Drawing.Color.Black;
            this.metroShell1.HelpButtonText = "LOGOUT";
            this.metroShell1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.metroAppButton1,
            this.metroTabItem1,
            this.metroTabItem2,
            this.metroTabItem3,
            this.metroTabItem5,
            this.metroTabItem4});
            this.metroShell1.KeyTipsFont = new System.Drawing.Font("Tahoma", 7F);
            this.metroShell1.Location = new System.Drawing.Point(0, 1);
            this.metroShell1.Name = "metroShell1";
            this.metroShell1.Size = new System.Drawing.Size(799, 498);
            this.metroShell1.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
            this.metroShell1.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
            this.metroShell1.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
            this.metroShell1.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
            this.metroShell1.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
            this.metroShell1.SystemText.QatDialogAddButton = "&Add >>";
            this.metroShell1.SystemText.QatDialogCancelButton = "Cancel";
            this.metroShell1.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
            this.metroShell1.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
            this.metroShell1.SystemText.QatDialogOkButton = "OK";
            this.metroShell1.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShell1.SystemText.QatDialogRemoveButton = "&Remove";
            this.metroShell1.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
            this.metroShell1.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShell1.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
            this.metroShell1.TabIndex = 0;
            this.metroShell1.TabStripFont = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metroShell1.Text = "Shell";
            this.metroShell1.SettingsButtonClick += new System.EventHandler(this.metroShell1_SettingsButtonClick);
            this.metroShell1.HelpButtonClick += new System.EventHandler(this.metroShell1_LogOutButtonClick);
            // 
            // metroTabPanel3
            // 
            this.metroTabPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.metroTabPanel3.Controls.Add(this.tableLayoutPanel_missingBar);
            this.metroTabPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabPanel3.Location = new System.Drawing.Point(0, 51);
            this.metroTabPanel3.Name = "metroTabPanel3";
            this.metroTabPanel3.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.metroTabPanel3.Size = new System.Drawing.Size(799, 447);
            // 
            // 
            // 
            this.metroTabPanel3.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel3.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel3.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTabPanel3.TabIndex = 3;
            this.metroTabPanel3.Visible = false;
            // 
            // tableLayoutPanel_missingBar
            // 
            this.tableLayoutPanel_missingBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel_missingBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.tableLayoutPanel_missingBar.ColumnCount = 2;
            this.tableLayoutPanel_missingBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel_missingBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_missingBar.Controls.Add(this.listViewResult, 1, 0);
            this.tableLayoutPanel_missingBar.Controls.Add(this.metroTilePanel1, 0, 1);
            this.tableLayoutPanel_missingBar.Controls.Add(this.ui_missingbar_panelEx_symbolsBack, 0, 0);
            this.tableLayoutPanel_missingBar.ForeColor = System.Drawing.Color.Black;
            this.tableLayoutPanel_missingBar.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel_missingBar.Name = "tableLayoutPanel_missingBar";
            this.tableLayoutPanel_missingBar.RowCount = 2;
            this.tableLayoutPanel_missingBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_missingBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 155F));
            this.tableLayoutPanel_missingBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_missingBar.Size = new System.Drawing.Size(793, 423);
            this.tableLayoutPanel_missingBar.TabIndex = 0;
            // 
            // listViewResult
            // 
            this.listViewResult.BackColor = System.Drawing.Color.White;
            this.listViewResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderDate,
            this.columnHeaderState,
            this.columnHeaderStartDay,
            this.columnHeaderStart,
            this.columnHeaderEndDay,
            this.columnHeaderEnd});
            this.listViewResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewResult.ForeColor = System.Drawing.Color.Black;
            this.listViewResult.FullRowSelect = true;
            listViewGroup1.Header = "10/12/2013";
            listViewGroup1.Name = "listViewGroup1";
            listViewGroup2.Header = "13/12/12";
            listViewGroup2.Name = "listViewGroup2";
            listViewGroup3.Header = "14/12/12";
            listViewGroup3.Name = "listViewGroup3";
            this.listViewResult.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.listViewResult.Location = new System.Drawing.Point(253, 3);
            this.listViewResult.Name = "listViewResult";
            this.tableLayoutPanel_missingBar.SetRowSpan(this.listViewResult, 2);
            this.listViewResult.Size = new System.Drawing.Size(537, 417);
            this.listViewResult.TabIndex = 2;
            this.listViewResult.UseCompatibleStateImageBehavior = false;
            this.listViewResult.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderDate
            // 
            this.columnHeaderDate.Text = "Date";
            this.columnHeaderDate.Width = 96;
            // 
            // columnHeaderState
            // 
            this.columnHeaderState.Text = "State";
            this.columnHeaderState.Width = 95;
            // 
            // columnHeaderStartDay
            // 
            this.columnHeaderStartDay.Text = "Start Day";
            this.columnHeaderStartDay.Width = 85;
            // 
            // columnHeaderStart
            // 
            this.columnHeaderStart.Text = "Start Time";
            this.columnHeaderStart.Width = 88;
            // 
            // columnHeaderEndDay
            // 
            this.columnHeaderEndDay.Text = "End Day";
            this.columnHeaderEndDay.Width = 85;
            // 
            // columnHeaderEnd
            // 
            this.columnHeaderEnd.Text = "End Time";
            this.columnHeaderEnd.Width = 83;
            // 
            // metroTilePanel1
            // 
            this.metroTilePanel1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.metroTilePanel1.BackgroundStyle.Class = "MetroTilePanel";
            this.metroTilePanel1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTilePanel1.ContainerControlProcessDialogKey = true;
            this.metroTilePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTilePanel1.ForeColor = System.Drawing.Color.Black;
            this.metroTilePanel1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.ui_metroTileItem_missingBar});
            this.metroTilePanel1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.metroTilePanel1.Location = new System.Drawing.Point(4, 272);
            this.metroTilePanel1.Margin = new System.Windows.Forms.Padding(4);
            this.metroTilePanel1.Name = "metroTilePanel1";
            this.metroTilePanel1.Size = new System.Drawing.Size(242, 147);
            this.metroTilePanel1.TabIndex = 33;
            this.metroTilePanel1.Text = "s";
            // 
            // ui_metroTileItem_missingBar
            // 
            this.ui_metroTileItem_missingBar.Image = global::DataNetClient.Properties.Resources.Charts;
            this.ui_metroTileItem_missingBar.Name = "ui_metroTileItem_missingBar";
            this.ui_metroTileItem_missingBar.SymbolColor = System.Drawing.Color.Empty;
            this.ui_metroTileItem_missingBar.Text = "<font size=\"+4\">Start analyzing<br/>symbol data</font>";
            this.ui_metroTileItem_missingBar.TileColor = DevComponents.DotNetBar.Metro.eMetroTileColor.Default;
            // 
            // 
            // 
            this.ui_metroTileItem_missingBar.TileStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(132)))));
            this.ui_metroTileItem_missingBar.TileStyle.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(65)))), ((int)(((byte)(66)))));
            this.ui_metroTileItem_missingBar.TileStyle.BackColorGradientAngle = 45;
            this.ui_metroTileItem_missingBar.TileStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_metroTileItem_missingBar.TileStyle.PaddingBottom = 4;
            this.ui_metroTileItem_missingBar.TileStyle.PaddingLeft = 4;
            this.ui_metroTileItem_missingBar.TileStyle.PaddingRight = 4;
            this.ui_metroTileItem_missingBar.TileStyle.PaddingTop = 4;
            this.ui_metroTileItem_missingBar.TileStyle.TextColor = System.Drawing.Color.White;
            this.ui_metroTileItem_missingBar.TitleText = "Analyzing";
            this.ui_metroTileItem_missingBar.Click += new System.EventHandler(this.metroTileItemMissingBar_Click);
            // 
            // ui_missingbar_panelEx_symbolsBack
            // 
            this.ui_missingbar_panelEx_symbolsBack.CanvasColor = System.Drawing.SystemColors.Control;
            this.ui_missingbar_panelEx_symbolsBack.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_missingbar_panelEx_symbolsBack.Controls.Add(this.ui_missingbars_panelEx_list_back);
            this.ui_missingbar_panelEx_symbolsBack.Controls.Add(this.labelX17);
            this.ui_missingbar_panelEx_symbolsBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_missingbar_panelEx_symbolsBack.Location = new System.Drawing.Point(3, 3);
            this.ui_missingbar_panelEx_symbolsBack.Name = "ui_missingbar_panelEx_symbolsBack";
            this.ui_missingbar_panelEx_symbolsBack.Padding = new System.Windows.Forms.Padding(1);
            this.ui_missingbar_panelEx_symbolsBack.Size = new System.Drawing.Size(244, 262);
            this.ui_missingbar_panelEx_symbolsBack.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.ui_missingbar_panelEx_symbolsBack.Style.BackColor1.Color = System.Drawing.Color.White;
            this.ui_missingbar_panelEx_symbolsBack.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.ui_missingbar_panelEx_symbolsBack.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.ui_missingbar_panelEx_symbolsBack.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.ui_missingbar_panelEx_symbolsBack.Style.GradientAngle = 90;
            this.ui_missingbar_panelEx_symbolsBack.TabIndex = 34;
            // 
            // ui_missingbars_panelEx_list_back
            // 
            this.ui_missingbars_panelEx_list_back.CanvasColor = System.Drawing.SystemColors.Control;
            this.ui_missingbars_panelEx_list_back.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_missingbars_panelEx_list_back.Controls.Add(this.ui_listBox_symbolsForMissing);
            this.ui_missingbars_panelEx_list_back.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_missingbars_panelEx_list_back.Location = new System.Drawing.Point(1, 33);
            this.ui_missingbars_panelEx_list_back.Name = "ui_missingbars_panelEx_list_back";
            this.ui_missingbars_panelEx_list_back.Padding = new System.Windows.Forms.Padding(1);
            this.ui_missingbars_panelEx_list_back.Size = new System.Drawing.Size(242, 228);
            this.ui_missingbars_panelEx_list_back.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.ui_missingbars_panelEx_list_back.Style.BackColor1.Color = System.Drawing.Color.White;
            this.ui_missingbars_panelEx_list_back.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.ui_missingbars_panelEx_list_back.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.ui_missingbars_panelEx_list_back.Style.BorderWidth = 0;
            this.ui_missingbars_panelEx_list_back.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.ui_missingbars_panelEx_list_back.Style.GradientAngle = 90;
            this.ui_missingbars_panelEx_list_back.TabIndex = 39;
            this.ui_missingbars_panelEx_list_back.Text = "panelEx7";
            // 
            // ui_listBox_symbolsForMissing
            // 
            this.ui_listBox_symbolsForMissing.BackColor = System.Drawing.Color.White;
            this.ui_listBox_symbolsForMissing.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ui_listBox_symbolsForMissing.ContextMenuStrip = this.contextMenuStripTables;
            this.ui_listBox_symbolsForMissing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_listBox_symbolsForMissing.ForeColor = System.Drawing.Color.Black;
            this.ui_listBox_symbolsForMissing.FormattingEnabled = true;
            this.ui_listBox_symbolsForMissing.Location = new System.Drawing.Point(1, 1);
            this.ui_listBox_symbolsForMissing.Name = "ui_listBox_symbolsForMissing";
            this.ui_listBox_symbolsForMissing.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ui_listBox_symbolsForMissing.Size = new System.Drawing.Size(240, 226);
            this.ui_listBox_symbolsForMissing.TabIndex = 37;
            // 
            // contextMenuStripTables
            // 
            this.contextMenuStripTables.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            this.contextMenuStripTables.Name = "contextMenuStrip1";
            this.contextMenuStripTables.Size = new System.Drawing.Size(135, 48);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(134, 22);
            this.toolStripMenuItem2.Text = "Select all";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(134, 22);
            this.toolStripMenuItem3.Text = "Unselect all";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // labelX17
            // 
            // 
            // 
            // 
            this.labelX17.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX17.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelX17.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX17.ForeColor = System.Drawing.Color.Black;
            this.labelX17.Location = new System.Drawing.Point(1, 1);
            this.labelX17.Name = "labelX17";
            this.labelX17.PaddingLeft = 6;
            this.labelX17.Size = new System.Drawing.Size(242, 32);
            this.labelX17.TabIndex = 38;
            this.labelX17.Text = "SYMBOLS";
            // 
            // metroTabPanel5
            // 
            this.metroTabPanel5.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.metroTabPanel5.Controls.Add(this.tableLayoutPanel2);
            this.metroTabPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabPanel5.Location = new System.Drawing.Point(0, 51);
            this.metroTabPanel5.Name = "metroTabPanel5";
            this.metroTabPanel5.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.metroTabPanel5.Size = new System.Drawing.Size(799, 447);
            // 
            // 
            // 
            this.metroTabPanel5.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel5.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel5.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTabPanel5.TabIndex = 5;
            this.metroTabPanel5.Visible = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.listView2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.panelEx11, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.listView1, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.panelEx9, 0, 0);
            this.tableLayoutPanel2.ForeColor = System.Drawing.Color.Black;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(793, 420);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // listView2
            // 
            this.listView2.BackColor = System.Drawing.Color.White;
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.listView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView2.ForeColor = System.Drawing.Color.Black;
            this.listView2.FullRowSelect = true;
            listViewGroup4.Header = "10/12/2013";
            listViewGroup4.Name = "listViewGroup1";
            listViewGroup5.Header = "13/12/12";
            listViewGroup5.Name = "listViewGroup2";
            listViewGroup6.Header = "14/12/12";
            listViewGroup6.Name = "listViewGroup3";
            this.listView2.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup4,
            listViewGroup5,
            listViewGroup6});
            this.listView2.Location = new System.Drawing.Point(253, 250);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(537, 167);
            this.listView2.TabIndex = 36;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Id";
            this.columnHeader1.Width = 25;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Symbol";
            this.columnHeader2.Width = 85;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Tick Size";
            this.columnHeader6.Width = 90;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Currency";
            this.columnHeader7.Width = 110;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Expiration";
            this.columnHeader8.Width = 80;
            // 
            // panelEx11
            // 
            this.panelEx11.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx11.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx11.Controls.Add(this.panelEx12);
            this.panelEx11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx11.Location = new System.Drawing.Point(253, 3);
            this.panelEx11.Name = "panelEx11";
            this.panelEx11.Padding = new System.Windows.Forms.Padding(1);
            this.panelEx11.Size = new System.Drawing.Size(537, 69);
            this.panelEx11.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx11.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelEx11.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx11.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx11.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx11.Style.GradientAngle = 90;
            this.panelEx11.TabIndex = 35;
            // 
            // panelEx12
            // 
            this.panelEx12.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx12.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx12.Controls.Add(this.buttonX_daily_updateValues);
            this.panelEx12.Controls.Add(this.buttonX_daily_getValues);
            this.panelEx12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx12.Location = new System.Drawing.Point(1, 1);
            this.panelEx12.Name = "panelEx12";
            this.panelEx12.Padding = new System.Windows.Forms.Padding(1);
            this.panelEx12.Size = new System.Drawing.Size(535, 67);
            this.panelEx12.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx12.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelEx12.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx12.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx12.Style.BorderWidth = 0;
            this.panelEx12.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx12.Style.GradientAngle = 90;
            this.panelEx12.TabIndex = 39;
            // 
            // buttonX_daily_updateValues
            // 
            this.buttonX_daily_updateValues.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX_daily_updateValues.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX_daily_updateValues.Location = new System.Drawing.Point(304, 10);
            this.buttonX_daily_updateValues.Name = "buttonX_daily_updateValues";
            this.buttonX_daily_updateValues.Size = new System.Drawing.Size(201, 46);
            this.buttonX_daily_updateValues.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX_daily_updateValues.TabIndex = 54;
            this.buttonX_daily_updateValues.Text = "Update values for selected symbols";
            this.toolTip1.SetToolTip(this.buttonX_daily_updateValues, "From CQG");
            this.buttonX_daily_updateValues.Click += new System.EventHandler(this.buttonX_daily_updateValues_Click);
            // 
            // buttonX_daily_getValues
            // 
            this.buttonX_daily_getValues.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX_daily_getValues.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX_daily_getValues.Location = new System.Drawing.Point(17, 10);
            this.buttonX_daily_getValues.Name = "buttonX_daily_getValues";
            this.buttonX_daily_getValues.Size = new System.Drawing.Size(201, 46);
            this.buttonX_daily_getValues.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX_daily_getValues.TabIndex = 53;
            this.buttonX_daily_getValues.Text = "Get values for selected symbols";
            this.toolTip1.SetToolTip(this.buttonX_daily_getValues, "From DB");
            this.buttonX_daily_getValues.Click += new System.EventHandler(this.buttonX_daily_getValues_Click);
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.Color.White;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15,
            this.columnHeader16});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.ForeColor = System.Drawing.Color.Black;
            this.listView1.FullRowSelect = true;
            listViewGroup7.Header = "10/12/2013";
            listViewGroup7.Name = "listViewGroup1";
            listViewGroup8.Header = "13/12/12";
            listViewGroup8.Name = "listViewGroup2";
            listViewGroup9.Header = "14/12/12";
            listViewGroup9.Name = "listViewGroup3";
            this.listView1.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup7,
            listViewGroup8,
            listViewGroup9});
            this.listView1.Location = new System.Drawing.Point(253, 78);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(537, 166);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Id";
            this.columnHeader10.Width = 25;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Symbol";
            this.columnHeader11.Width = 65;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Date time";
            this.columnHeader12.Width = 90;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Indicative Open";
            this.columnHeader13.Width = 110;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Settlement";
            this.columnHeader14.Width = 80;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "Marker";
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "Today Marker";
            this.columnHeader16.Width = 90;
            // 
            // panelEx9
            // 
            this.panelEx9.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx9.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx9.Controls.Add(this.panelEx10);
            this.panelEx9.Controls.Add(this.labelX20);
            this.panelEx9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx9.Location = new System.Drawing.Point(3, 3);
            this.panelEx9.Name = "panelEx9";
            this.panelEx9.Padding = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel2.SetRowSpan(this.panelEx9, 3);
            this.panelEx9.Size = new System.Drawing.Size(244, 414);
            this.panelEx9.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx9.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelEx9.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx9.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx9.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx9.Style.GradientAngle = 90;
            this.panelEx9.TabIndex = 34;
            // 
            // panelEx10
            // 
            this.panelEx10.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx10.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx10.Controls.Add(this.listBox_daily_symbols);
            this.panelEx10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx10.Location = new System.Drawing.Point(1, 33);
            this.panelEx10.Name = "panelEx10";
            this.panelEx10.Padding = new System.Windows.Forms.Padding(1);
            this.panelEx10.Size = new System.Drawing.Size(242, 380);
            this.panelEx10.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx10.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelEx10.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx10.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx10.Style.BorderWidth = 0;
            this.panelEx10.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx10.Style.GradientAngle = 90;
            this.panelEx10.TabIndex = 39;
            this.panelEx10.Text = "panelEx7";
            // 
            // listBox_daily_symbols
            // 
            this.listBox_daily_symbols.BackColor = System.Drawing.Color.White;
            this.listBox_daily_symbols.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox_daily_symbols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_daily_symbols.ForeColor = System.Drawing.Color.Black;
            this.listBox_daily_symbols.FormattingEnabled = true;
            this.listBox_daily_symbols.Location = new System.Drawing.Point(1, 1);
            this.listBox_daily_symbols.Name = "listBox_daily_symbols";
            this.listBox_daily_symbols.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox_daily_symbols.Size = new System.Drawing.Size(240, 378);
            this.listBox_daily_symbols.TabIndex = 37;
            // 
            // labelX20
            // 
            // 
            // 
            // 
            this.labelX20.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX20.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelX20.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX20.ForeColor = System.Drawing.Color.Black;
            this.labelX20.Location = new System.Drawing.Point(1, 1);
            this.labelX20.Name = "labelX20";
            this.labelX20.PaddingLeft = 6;
            this.labelX20.Size = new System.Drawing.Size(242, 32);
            this.labelX20.TabIndex = 38;
            this.labelX20.Text = "SYMBOLS";
            // 
            // metroTabPanel2
            // 
            this.metroTabPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.metroTabPanel2.Controls.Add(this.tableLayoutPanel1);
            this.metroTabPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabPanel2.Location = new System.Drawing.Point(0, 51);
            this.metroTabPanel2.Name = "metroTabPanel2";
            this.metroTabPanel2.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.metroTabPanel2.Size = new System.Drawing.Size(799, 447);
            // 
            // 
            // 
            this.metroTabPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTabPanel2.Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.metroTabPanel2.Style.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            // 
            // 
            // 
            this.metroTabPanel2.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel2.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTabPanel2.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.99712F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 241F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.00288F));
            this.tableLayoutPanel1.Controls.Add(this.panelEx3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelEx4, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelEx2, 0, 0);
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(793, 423);
            this.tableLayoutPanel1.TabIndex = 23;
            // 
            // panelEx3
            // 
            this.panelEx3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx3.Controls.Add(this.linkLabel_sort_time);
            this.panelEx3.Controls.Add(this.linkLabel_sort_tf);
            this.panelEx3.Controls.Add(this.linkLabel_sort_name);
            this.panelEx3.Controls.Add(this.linkLabel_selectNone);
            this.panelEx3.Controls.Add(this.linkLabel_selectAll);
            this.panelEx3.Controls.Add(this.panelEx6);
            this.panelEx3.Controls.Add(this.labelX7);
            this.panelEx3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx3.Location = new System.Drawing.Point(442, 3);
            this.panelEx3.Name = "panelEx3";
            this.panelEx3.Padding = new System.Windows.Forms.Padding(1);
            this.panelEx3.Size = new System.Drawing.Size(348, 417);
            this.panelEx3.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx3.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelEx3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx3.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx3.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx3.Style.GradientAngle = 90;
            this.panelEx3.TabIndex = 5;
            // 
            // linkLabel_sort_time
            // 
            this.linkLabel_sort_time.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkLabel_sort_time.AutoSize = true;
            this.linkLabel_sort_time.ForeColor = System.Drawing.Color.Black;
            this.linkLabel_sort_time.LinkColor = System.Drawing.Color.DimGray;
            this.linkLabel_sort_time.Location = new System.Drawing.Point(179, 13);
            this.linkLabel_sort_time.Name = "linkLabel_sort_time";
            this.linkLabel_sort_time.Size = new System.Drawing.Size(30, 13);
            this.linkLabel_sort_time.TabIndex = 34;
            this.linkLabel_sort_time.TabStop = true;
            this.linkLabel_sort_time.Tag = "3";
            this.linkLabel_sort_time.Text = "Time";
            this.toolTip1.SetToolTip(this.linkLabel_sort_time, "Sort by time of last collecting");
            this.linkLabel_sort_time.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_sort_name_LinkClicked);
            // 
            // linkLabel_sort_tf
            // 
            this.linkLabel_sort_tf.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkLabel_sort_tf.AutoSize = true;
            this.linkLabel_sort_tf.ForeColor = System.Drawing.Color.Black;
            this.linkLabel_sort_tf.LinkColor = System.Drawing.Color.DimGray;
            this.linkLabel_sort_tf.Location = new System.Drawing.Point(155, 13);
            this.linkLabel_sort_tf.Name = "linkLabel_sort_tf";
            this.linkLabel_sort_tf.Size = new System.Drawing.Size(18, 13);
            this.linkLabel_sort_tf.TabIndex = 33;
            this.linkLabel_sort_tf.TabStop = true;
            this.linkLabel_sort_tf.Tag = "2";
            this.linkLabel_sort_tf.Text = "TF";
            this.toolTip1.SetToolTip(this.linkLabel_sort_tf, "Sort by time frame");
            this.linkLabel_sort_tf.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_sort_name_LinkClicked);
            // 
            // linkLabel_sort_name
            // 
            this.linkLabel_sort_name.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkLabel_sort_name.AutoSize = true;
            this.linkLabel_sort_name.ForeColor = System.Drawing.Color.Black;
            this.linkLabel_sort_name.LinkColor = System.Drawing.Color.DimGray;
            this.linkLabel_sort_name.Location = new System.Drawing.Point(113, 13);
            this.linkLabel_sort_name.Name = "linkLabel_sort_name";
            this.linkLabel_sort_name.Size = new System.Drawing.Size(36, 13);
            this.linkLabel_sort_name.TabIndex = 32;
            this.linkLabel_sort_name.TabStop = true;
            this.linkLabel_sort_name.Tag = "1";
            this.linkLabel_sort_name.Text = "Name";
            this.toolTip1.SetToolTip(this.linkLabel_sort_name, "Sort by name");
            this.linkLabel_sort_name.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_sort_name_LinkClicked);
            // 
            // linkLabel_selectNone
            // 
            this.linkLabel_selectNone.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkLabel_selectNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel_selectNone.AutoSize = true;
            this.linkLabel_selectNone.ForeColor = System.Drawing.Color.Black;
            this.linkLabel_selectNone.LinkColor = System.Drawing.Color.DimGray;
            this.linkLabel_selectNone.Location = new System.Drawing.Point(307, 13);
            this.linkLabel_selectNone.Name = "linkLabel_selectNone";
            this.linkLabel_selectNone.Size = new System.Drawing.Size(35, 13);
            this.linkLabel_selectNone.TabIndex = 31;
            this.linkLabel_selectNone.TabStop = true;
            this.linkLabel_selectNone.Text = "None";
            this.toolTip1.SetToolTip(this.linkLabel_selectNone, "Unselect");
            this.linkLabel_selectNone.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_selectNone_LinkClicked);
            // 
            // linkLabel_selectAll
            // 
            this.linkLabel_selectAll.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkLabel_selectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel_selectAll.AutoSize = true;
            this.linkLabel_selectAll.ForeColor = System.Drawing.Color.Black;
            this.linkLabel_selectAll.LinkColor = System.Drawing.Color.DimGray;
            this.linkLabel_selectAll.Location = new System.Drawing.Point(286, 13);
            this.linkLabel_selectAll.Name = "linkLabel_selectAll";
            this.linkLabel_selectAll.Size = new System.Drawing.Size(20, 13);
            this.linkLabel_selectAll.TabIndex = 30;
            this.linkLabel_selectAll.TabStop = true;
            this.linkLabel_selectAll.Text = "All";
            this.toolTip1.SetToolTip(this.linkLabel_selectAll, "Select all");
            this.linkLabel_selectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_selectAll_LinkClicked);
            // 
            // panelEx6
            // 
            this.panelEx6.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx6.Controls.Add(this.styledListControl1);
            this.panelEx6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx6.Location = new System.Drawing.Point(1, 33);
            this.panelEx6.Name = "panelEx6";
            this.panelEx6.Padding = new System.Windows.Forms.Padding(1);
            this.panelEx6.Size = new System.Drawing.Size(346, 383);
            this.panelEx6.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx6.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelEx6.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx6.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx6.Style.GradientAngle = 90;
            this.panelEx6.TabIndex = 24;
            // 
            // styledListControl1
            // 
            this.styledListControl1.BackColor = System.Drawing.Color.White;
            this.styledListControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.styledListControl1.ForeColor = System.Drawing.Color.Black;
            this.styledListControl1.Location = new System.Drawing.Point(1, 1);
            this.styledListControl1.Name = "styledListControl1";
            this.styledListControl1.SelectedItem = -1;
            this.styledListControl1.Size = new System.Drawing.Size(344, 381);
            this.styledListControl1.StateChangingEnabled = false;
            this.styledListControl1.TabIndex = 23;
            // 
            // labelX7
            // 
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelX7.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX7.ForeColor = System.Drawing.Color.Black;
            this.labelX7.Location = new System.Drawing.Point(1, 1);
            this.labelX7.Name = "labelX7";
            this.labelX7.PaddingLeft = 6;
            this.labelX7.Size = new System.Drawing.Size(346, 32);
            this.labelX7.TabIndex = 20;
            this.labelX7.Text = "GROUPS";
            // 
            // panelEx4
            // 
            this.panelEx4.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx4.Controls.Add(this.panelEx7);
            this.panelEx4.Controls.Add(this.buttonX_StartCollectGroups);
            this.panelEx4.Controls.Add(this.buttonX_StartCollectSymbols);
            this.panelEx4.Controls.Add(this.buttonX_stopCollecting);
            this.panelEx4.Controls.Add(this.labelX15);
            this.panelEx4.Controls.Add(this.ui_checkBoxAuto_CheckForMissedBars);
            this.panelEx4.Controls.Add(this.panelExBARS);
            this.panelEx4.Controls.Add(this.panelExTimeInterval);
            this.panelEx4.Controls.Add(this.grbDataCollectType);
            this.panelEx4.Controls.Add(this.labelX8);
            this.panelEx4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx4.Location = new System.Drawing.Point(201, 3);
            this.panelEx4.Name = "panelEx4";
            this.panelEx4.Padding = new System.Windows.Forms.Padding(1);
            this.panelEx4.Size = new System.Drawing.Size(235, 417);
            this.panelEx4.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx4.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelEx4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx4.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx4.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx4.Style.GradientAngle = 90;
            this.panelEx4.TabIndex = 4;
            // 
            // panelEx7
            // 
            this.panelEx7.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx7.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx7.Controls.Add(this.label1);
            this.panelEx7.Controls.Add(this.switchButton_changeMode);
            this.panelEx7.Location = new System.Drawing.Point(4, 33);
            this.panelEx7.Name = "panelEx7";
            this.panelEx7.Size = new System.Drawing.Size(224, 29);
            this.panelEx7.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx7.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelEx7.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx7.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx7.Style.BorderSide = DevComponents.DotNetBar.eBorderSide.Bottom;
            this.panelEx7.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx7.Style.GradientAngle = 90;
            this.panelEx7.TabIndex = 65;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(5, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 20);
            this.label1.TabIndex = 51;
            this.label1.Text = "Auto Collect";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // switchButton_changeMode
            // 
            // 
            // 
            // 
            this.switchButton_changeMode.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.switchButton_changeMode.ForeColor = System.Drawing.Color.Black;
            this.switchButton_changeMode.Location = new System.Drawing.Point(118, 3);
            this.switchButton_changeMode.Name = "switchButton_changeMode";
            this.switchButton_changeMode.Size = new System.Drawing.Size(100, 20);
            this.switchButton_changeMode.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.switchButton_changeMode.SwitchBackColor = System.Drawing.Color.DimGray;
            this.switchButton_changeMode.SwitchWidth = 20;
            this.switchButton_changeMode.TabIndex = 2;
            this.switchButton_changeMode.ValueChanged += new System.EventHandler(this.switchButton_changeMode_ValueChanged);
            // 
            // buttonX_StartCollectGroups
            // 
            this.buttonX_StartCollectGroups.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX_StartCollectGroups.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX_StartCollectGroups.Location = new System.Drawing.Point(122, 330);
            this.buttonX_StartCollectGroups.Name = "buttonX_StartCollectGroups";
            this.buttonX_StartCollectGroups.Size = new System.Drawing.Size(109, 46);
            this.buttonX_StartCollectGroups.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX_StartCollectGroups.TabIndex = 53;
            this.buttonX_StartCollectGroups.Text = "Start collecting\r\nGroups";
            this.buttonX_StartCollectGroups.Click += new System.EventHandler(this.buttonX_StartCollectGroups_Click);
            // 
            // buttonX_StartCollectSymbols
            // 
            this.buttonX_StartCollectSymbols.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX_StartCollectSymbols.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX_StartCollectSymbols.Location = new System.Drawing.Point(4, 330);
            this.buttonX_StartCollectSymbols.Name = "buttonX_StartCollectSymbols";
            this.buttonX_StartCollectSymbols.Size = new System.Drawing.Size(111, 46);
            this.buttonX_StartCollectSymbols.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX_StartCollectSymbols.TabIndex = 52;
            this.buttonX_StartCollectSymbols.Text = "Start collect \r\nsymbols";
            this.buttonX_StartCollectSymbols.Click += new System.EventHandler(this.buttonX_StartCollectSymbols_Click);
            // 
            // buttonX_stopCollecting
            // 
            this.buttonX_stopCollecting.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX_stopCollecting.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX_stopCollecting.Location = new System.Drawing.Point(165, 300);
            this.buttonX_stopCollecting.Name = "buttonX_stopCollecting";
            this.buttonX_stopCollecting.Size = new System.Drawing.Size(65, 25);
            this.buttonX_stopCollecting.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX_stopCollecting.TabIndex = 3;
            this.buttonX_stopCollecting.Text = "Stop";
            this.buttonX_stopCollecting.Click += new System.EventHandler(this.buttonX_stopCollecting_Click);
            // 
            // labelX15
            // 
            this.labelX15.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX15.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX15.ForeColor = System.Drawing.Color.Black;
            this.labelX15.Location = new System.Drawing.Point(4, 382);
            this.labelX15.Name = "labelX15";
            this.labelX15.Size = new System.Drawing.Size(139, 23);
            this.labelX15.TabIndex = 2;
            this.labelX15.Text = "Auto check for missing bars";
            // 
            // ui_checkBoxAuto_CheckForMissedBars
            // 
            this.ui_checkBoxAuto_CheckForMissedBars.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_checkBoxAuto_CheckForMissedBars.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_checkBoxAuto_CheckForMissedBars.ForeColor = System.Drawing.Color.Black;
            this.ui_checkBoxAuto_CheckForMissedBars.Location = new System.Drawing.Point(165, 383);
            this.ui_checkBoxAuto_CheckForMissedBars.Name = "ui_checkBoxAuto_CheckForMissedBars";
            this.ui_checkBoxAuto_CheckForMissedBars.Size = new System.Drawing.Size(66, 22);
            this.ui_checkBoxAuto_CheckForMissedBars.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_checkBoxAuto_CheckForMissedBars.SwitchBackColor = System.Drawing.Color.DimGray;
            this.ui_checkBoxAuto_CheckForMissedBars.SwitchWidth = 20;
            this.ui_checkBoxAuto_CheckForMissedBars.TabIndex = 1;
            // 
            // panelExBARS
            // 
            this.panelExBARS.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelExBARS.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelExBARS.Controls.Add(this.cmbContinuationType);
            this.panelExBARS.Controls.Add(this.cmbHistoricalPeriod);
            this.panelExBARS.Controls.Add(this.labelX14);
            this.panelExBARS.Controls.Add(this.labelX13);
            this.panelExBARS.Controls.Add(this.nudStartBar);
            this.panelExBARS.Controls.Add(this.nudEndBar);
            this.panelExBARS.Controls.Add(this.labelX11);
            this.panelExBARS.Controls.Add(this.labelX12);
            this.panelExBARS.Controls.Add(this.rdb31);
            this.panelExBARS.Controls.Add(this.rdb1);
            this.panelExBARS.Location = new System.Drawing.Point(3, 154);
            this.panelExBARS.Name = "panelExBARS";
            this.panelExBARS.Size = new System.Drawing.Size(227, 129);
            this.panelExBARS.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelExBARS.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelExBARS.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelExBARS.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelExBARS.Style.BorderSide = DevComponents.DotNetBar.eBorderSide.Bottom;
            this.panelExBARS.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelExBARS.Style.GradientAngle = 90;
            this.panelExBARS.TabIndex = 51;
            // 
            // cmbContinuationType
            // 
            this.cmbContinuationType.BackColor = System.Drawing.Color.White;
            this.cmbContinuationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbContinuationType.ForeColor = System.Drawing.Color.Black;
            this.cmbContinuationType.FormattingEnabled = true;
            this.cmbContinuationType.Location = new System.Drawing.Point(10, 95);
            this.cmbContinuationType.Name = "cmbContinuationType";
            this.cmbContinuationType.Size = new System.Drawing.Size(129, 21);
            this.cmbContinuationType.TabIndex = 60;
            // 
            // cmbHistoricalPeriod
            // 
            this.cmbHistoricalPeriod.BackColor = System.Drawing.Color.White;
            this.cmbHistoricalPeriod.DropDownHeight = 250;
            this.cmbHistoricalPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHistoricalPeriod.ForeColor = System.Drawing.Color.Black;
            this.cmbHistoricalPeriod.FormattingEnabled = true;
            this.cmbHistoricalPeriod.IntegralHeight = false;
            this.cmbHistoricalPeriod.Items.AddRange(new object[] {
            "1 minute",
            "2 minutes",
            "3 minutes",
            "5 minutes",
            "10 minutes",
            "15 minutes",
            "30 minutes",
            "60 minutes",
            "240 minutes",
            "Daily",
            "Weekly",
            "Monthly",
            "Quarterly",
            "Semiannual",
            "Yearly"});
            this.cmbHistoricalPeriod.Location = new System.Drawing.Point(10, 45);
            this.cmbHistoricalPeriod.Name = "cmbHistoricalPeriod";
            this.cmbHistoricalPeriod.Size = new System.Drawing.Size(129, 21);
            this.cmbHistoricalPeriod.TabIndex = 59;
            // 
            // labelX14
            // 
            // 
            // 
            // 
            this.labelX14.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX14.ForeColor = System.Drawing.Color.Black;
            this.labelX14.Location = new System.Drawing.Point(161, 70);
            this.labelX14.Name = "labelX14";
            this.labelX14.Size = new System.Drawing.Size(58, 23);
            this.labelX14.TabIndex = 65;
            this.labelX14.Text = "Finish";
            // 
            // labelX13
            // 
            // 
            // 
            // 
            this.labelX13.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX13.ForeColor = System.Drawing.Color.Black;
            this.labelX13.Location = new System.Drawing.Point(161, 18);
            this.labelX13.Name = "labelX13";
            this.labelX13.Size = new System.Drawing.Size(58, 23);
            this.labelX13.TabIndex = 64;
            this.labelX13.Text = "Start";
            // 
            // nudStartBar
            // 
            this.nudStartBar.BackColor = System.Drawing.Color.White;
            this.nudStartBar.ForeColor = System.Drawing.Color.Black;
            this.nudStartBar.Location = new System.Drawing.Point(161, 42);
            this.nudStartBar.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudStartBar.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.nudStartBar.Name = "nudStartBar";
            this.nudStartBar.Size = new System.Drawing.Size(58, 22);
            this.nudStartBar.TabIndex = 62;
            // 
            // nudEndBar
            // 
            this.nudEndBar.BackColor = System.Drawing.Color.White;
            this.nudEndBar.ForeColor = System.Drawing.Color.Black;
            this.nudEndBar.Location = new System.Drawing.Point(161, 94);
            this.nudEndBar.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudEndBar.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.nudEndBar.Name = "nudEndBar";
            this.nudEndBar.Size = new System.Drawing.Size(58, 22);
            this.nudEndBar.TabIndex = 63;
            this.nudEndBar.Value = new decimal(new int[] {
            3000,
            0,
            0,
            -2147483648});
            // 
            // labelX11
            // 
            // 
            // 
            // 
            this.labelX11.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX11.ForeColor = System.Drawing.Color.Black;
            this.labelX11.Location = new System.Drawing.Point(10, 69);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(127, 23);
            this.labelX11.TabIndex = 61;
            this.labelX11.Text = "Continuation Types:";
            // 
            // labelX12
            // 
            // 
            // 
            // 
            this.labelX12.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX12.ForeColor = System.Drawing.Color.Black;
            this.labelX12.Location = new System.Drawing.Point(10, 18);
            this.labelX12.Name = "labelX12";
            this.labelX12.Size = new System.Drawing.Size(142, 23);
            this.labelX12.TabIndex = 58;
            this.labelX12.Text = "Historical Periods:";
            // 
            // rdb31
            // 
            this.rdb31.AutoSize = true;
            this.rdb31.Checked = true;
            this.rdb31.ForeColor = System.Drawing.Color.Black;
            this.rdb31.Location = new System.Drawing.Point(119, 1);
            this.rdb31.Name = "rdb31";
            this.rdb31.Size = new System.Drawing.Size(37, 17);
            this.rdb31.TabIndex = 57;
            this.rdb31.TabStop = true;
            this.rdb31.Text = "31";
            this.rdb31.UseVisualStyleBackColor = true;
            // 
            // rdb1
            // 
            this.rdb1.AutoSize = true;
            this.rdb1.ForeColor = System.Drawing.Color.Black;
            this.rdb1.Location = new System.Drawing.Point(46, 1);
            this.rdb1.Name = "rdb1";
            this.rdb1.Size = new System.Drawing.Size(31, 17);
            this.rdb1.TabIndex = 56;
            this.rdb1.Text = "1";
            this.rdb1.UseVisualStyleBackColor = true;
            // 
            // panelExTimeInterval
            // 
            this.panelExTimeInterval.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelExTimeInterval.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelExTimeInterval.Controls.Add(this.dateTimeInputStart);
            this.panelExTimeInterval.Controls.Add(this.dateTimeInputEnd);
            this.panelExTimeInterval.Controls.Add(this.labelX10);
            this.panelExTimeInterval.Controls.Add(this.labelX9);
            this.panelExTimeInterval.Enabled = false;
            this.panelExTimeInterval.Location = new System.Drawing.Point(4, 97);
            this.panelExTimeInterval.Name = "panelExTimeInterval";
            this.panelExTimeInterval.Size = new System.Drawing.Size(227, 51);
            this.panelExTimeInterval.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelExTimeInterval.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelExTimeInterval.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelExTimeInterval.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelExTimeInterval.Style.BorderSide = DevComponents.DotNetBar.eBorderSide.Bottom;
            this.panelExTimeInterval.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelExTimeInterval.Style.GradientAngle = 90;
            this.panelExTimeInterval.TabIndex = 50;
            // 
            // dateTimeInputStart
            // 
            // 
            // 
            // 
            this.dateTimeInputStart.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dateTimeInputStart.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputStart.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this.dateTimeInputStart.ButtonDropDown.Visible = true;
            this.dateTimeInputStart.ForeColor = System.Drawing.Color.Black;
            this.dateTimeInputStart.IsPopupCalendarOpen = false;
            this.dateTimeInputStart.Location = new System.Drawing.Point(10, 22);
            // 
            // 
            // 
            this.dateTimeInputStart.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateTimeInputStart.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputStart.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
            this.dateTimeInputStart.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dateTimeInputStart.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dateTimeInputStart.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimeInputStart.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dateTimeInputStart.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dateTimeInputStart.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dateTimeInputStart.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dateTimeInputStart.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputStart.MonthCalendar.DisplayMonth = new System.DateTime(2013, 2, 1, 0, 0, 0, 0);
            this.dateTimeInputStart.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday;
            this.dateTimeInputStart.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.dateTimeInputStart.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateTimeInputStart.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dateTimeInputStart.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimeInputStart.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dateTimeInputStart.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputStart.MonthCalendar.TodayButtonVisible = true;
            this.dateTimeInputStart.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
            this.dateTimeInputStart.Name = "dateTimeInputStart";
            this.dateTimeInputStart.Size = new System.Drawing.Size(96, 22);
            this.dateTimeInputStart.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.dateTimeInputStart.TabIndex = 33;
            this.dateTimeInputStart.Value = new System.DateTime(2013, 2, 27, 17, 13, 49, 0);
            // 
            // dateTimeInputEnd
            // 
            // 
            // 
            // 
            this.dateTimeInputEnd.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dateTimeInputEnd.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputEnd.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this.dateTimeInputEnd.ButtonDropDown.Visible = true;
            this.dateTimeInputEnd.ForeColor = System.Drawing.Color.Black;
            this.dateTimeInputEnd.IsPopupCalendarOpen = false;
            this.dateTimeInputEnd.Location = new System.Drawing.Point(118, 22);
            // 
            // 
            // 
            this.dateTimeInputEnd.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateTimeInputEnd.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputEnd.MonthCalendar.CalendarDimensions = new System.Drawing.Size(1, 1);
            this.dateTimeInputEnd.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dateTimeInputEnd.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dateTimeInputEnd.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimeInputEnd.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dateTimeInputEnd.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dateTimeInputEnd.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dateTimeInputEnd.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dateTimeInputEnd.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputEnd.MonthCalendar.DisplayMonth = new System.DateTime(2013, 2, 1, 0, 0, 0, 0);
            this.dateTimeInputEnd.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday;
            this.dateTimeInputEnd.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.dateTimeInputEnd.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dateTimeInputEnd.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dateTimeInputEnd.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dateTimeInputEnd.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dateTimeInputEnd.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dateTimeInputEnd.MonthCalendar.TodayButtonVisible = true;
            this.dateTimeInputEnd.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
            this.dateTimeInputEnd.Name = "dateTimeInputEnd";
            this.dateTimeInputEnd.Size = new System.Drawing.Size(96, 22);
            this.dateTimeInputEnd.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.dateTimeInputEnd.TabIndex = 34;
            this.dateTimeInputEnd.Value = new System.DateTime(2013, 2, 27, 17, 13, 49, 0);
            // 
            // labelX10
            // 
            // 
            // 
            // 
            this.labelX10.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX10.ForeColor = System.Drawing.Color.Black;
            this.labelX10.Location = new System.Drawing.Point(118, 1);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(96, 23);
            this.labelX10.TabIndex = 36;
            this.labelX10.Text = "End time point";
            // 
            // labelX9
            // 
            // 
            // 
            // 
            this.labelX9.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX9.ForeColor = System.Drawing.Color.Black;
            this.labelX9.Location = new System.Drawing.Point(10, 1);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(96, 23);
            this.labelX9.TabIndex = 35;
            this.labelX9.Text = "Start time point";
            // 
            // grbDataCollectType
            // 
            this.grbDataCollectType.CanvasColor = System.Drawing.SystemColors.Control;
            this.grbDataCollectType.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.grbDataCollectType.Controls.Add(this.radioButtonTick);
            this.grbDataCollectType.Controls.Add(this.radioButBars);
            this.grbDataCollectType.Location = new System.Drawing.Point(4, 62);
            this.grbDataCollectType.Name = "grbDataCollectType";
            this.grbDataCollectType.Size = new System.Drawing.Size(227, 29);
            this.grbDataCollectType.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.grbDataCollectType.Style.BackColor1.Color = System.Drawing.Color.White;
            this.grbDataCollectType.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.grbDataCollectType.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grbDataCollectType.Style.BorderSide = DevComponents.DotNetBar.eBorderSide.Bottom;
            this.grbDataCollectType.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.grbDataCollectType.Style.GradientAngle = 90;
            this.grbDataCollectType.TabIndex = 45;
            // 
            // radioButtonTick
            // 
            this.radioButtonTick.AutoSize = true;
            this.radioButtonTick.ForeColor = System.Drawing.Color.Black;
            this.radioButtonTick.Location = new System.Drawing.Point(118, 7);
            this.radioButtonTick.Name = "radioButtonTick";
            this.radioButtonTick.Size = new System.Drawing.Size(49, 17);
            this.radioButtonTick.TabIndex = 41;
            this.radioButtonTick.Text = "Ticks";
            this.radioButtonTick.UseVisualStyleBackColor = true;
            this.radioButtonTick.Click += new System.EventHandler(this.radioButBars_Click);
            // 
            // radioButBars
            // 
            this.radioButBars.AutoSize = true;
            this.radioButBars.Checked = true;
            this.radioButBars.ForeColor = System.Drawing.Color.Black;
            this.radioButBars.Location = new System.Drawing.Point(45, 7);
            this.radioButBars.Name = "radioButBars";
            this.radioButBars.Size = new System.Drawing.Size(47, 17);
            this.radioButBars.TabIndex = 40;
            this.radioButBars.TabStop = true;
            this.radioButBars.Text = "Bars";
            this.radioButBars.UseVisualStyleBackColor = true;
            this.radioButBars.Click += new System.EventHandler(this.radioButBars_Click);
            // 
            // labelX8
            // 
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX8.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelX8.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX8.ForeColor = System.Drawing.Color.Black;
            this.labelX8.Location = new System.Drawing.Point(1, 1);
            this.labelX8.Name = "labelX8";
            this.labelX8.PaddingLeft = 6;
            this.labelX8.Size = new System.Drawing.Size(233, 32);
            this.labelX8.TabIndex = 20;
            this.labelX8.Text = "INPUTS";
            // 
            // panelEx2
            // 
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx2.Controls.Add(this.linkLabel1);
            this.panelEx2.Controls.Add(this.linkLabel_edit);
            this.panelEx2.Controls.Add(this.panelEx8);
            this.panelEx2.Controls.Add(this.labelX6);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx2.Location = new System.Drawing.Point(3, 3);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Padding = new System.Windows.Forms.Padding(1);
            this.panelEx2.Size = new System.Drawing.Size(192, 417);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 3;
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.White;
            this.linkLabel1.ForeColor = System.Drawing.Color.Black;
            this.linkLabel1.LinkColor = System.Drawing.Color.DimGray;
            this.linkLabel1.Location = new System.Drawing.Point(136, 13);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(17, 13);
            this.linkLabel1.TabIndex = 30;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "↑↓";
            this.toolTip1.SetToolTip(this.linkLabel1, "Change sorting mode");
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel_edit
            // 
            this.linkLabel_edit.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkLabel_edit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel_edit.AutoSize = true;
            this.linkLabel_edit.ForeColor = System.Drawing.Color.Black;
            this.linkLabel_edit.LinkColor = System.Drawing.Color.DimGray;
            this.linkLabel_edit.Location = new System.Drawing.Point(159, 13);
            this.linkLabel_edit.Name = "linkLabel_edit";
            this.linkLabel_edit.Size = new System.Drawing.Size(27, 13);
            this.linkLabel_edit.TabIndex = 29;
            this.linkLabel_edit.TabStop = true;
            this.linkLabel_edit.Text = "Edit";
            this.toolTip1.SetToolTip(this.linkLabel_edit, "EDIT SYMBOLS");
            this.linkLabel_edit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_edit_LinkClicked);
            // 
            // panelEx8
            // 
            this.panelEx8.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx8.Controls.Add(this.ui_listBox_symbols);
            this.panelEx8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx8.Location = new System.Drawing.Point(1, 33);
            this.panelEx8.Name = "panelEx8";
            this.panelEx8.Padding = new System.Windows.Forms.Padding(1);
            this.panelEx8.Size = new System.Drawing.Size(190, 383);
            this.panelEx8.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx8.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelEx8.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx8.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx8.Style.GradientAngle = 90;
            this.panelEx8.TabIndex = 22;
            // 
            // ui_listBox_symbols
            // 
            this.ui_listBox_symbols.BackColor = System.Drawing.Color.White;
            this.ui_listBox_symbols.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ui_listBox_symbols.ContextMenuStrip = this.contextMenuStripSymbols;
            this.ui_listBox_symbols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_listBox_symbols.ForeColor = System.Drawing.Color.Black;
            this.ui_listBox_symbols.FormattingEnabled = true;
            this.ui_listBox_symbols.Location = new System.Drawing.Point(1, 1);
            this.ui_listBox_symbols.Name = "ui_listBox_symbols";
            this.ui_listBox_symbols.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ui_listBox_symbols.Size = new System.Drawing.Size(188, 381);
            this.ui_listBox_symbols.TabIndex = 21;
            this.ui_listBox_symbols.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxSymbols_MouseDown);
            this.ui_listBox_symbols.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listBoxSymbols_MouseMove);
            // 
            // contextMenuStripSymbols
            // 
            this.contextMenuStripSymbols.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.unselectAllToolStripMenuItem,
            this.toolStripMenuItem7,
            this.ui_ToolStripMenuItem_EditSymbols});
            this.contextMenuStripSymbols.Name = "contextMenuStrip1";
            this.contextMenuStripSymbols.Size = new System.Drawing.Size(142, 76);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.selectAllToolStripMenuItem.Text = "Select all";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // unselectAllToolStripMenuItem
            // 
            this.unselectAllToolStripMenuItem.Name = "unselectAllToolStripMenuItem";
            this.unselectAllToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.unselectAllToolStripMenuItem.Text = "Unselect all";
            this.unselectAllToolStripMenuItem.Click += new System.EventHandler(this.unselectAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(138, 6);
            // 
            // ui_ToolStripMenuItem_EditSymbols
            // 
            this.ui_ToolStripMenuItem_EditSymbols.Name = "ui_ToolStripMenuItem_EditSymbols";
            this.ui_ToolStripMenuItem_EditSymbols.Size = new System.Drawing.Size(141, 22);
            this.ui_ToolStripMenuItem_EditSymbols.Text = "Edit symbols";
            this.ui_ToolStripMenuItem_EditSymbols.Click += new System.EventHandler(this.ui_ToolStripMenuItem_EditSymbols_Click);
            // 
            // labelX6
            // 
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelX6.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX6.ForeColor = System.Drawing.Color.Black;
            this.labelX6.Location = new System.Drawing.Point(1, 1);
            this.labelX6.Name = "labelX6";
            this.labelX6.PaddingLeft = 6;
            this.labelX6.Size = new System.Drawing.Size(190, 32);
            this.labelX6.TabIndex = 20;
            this.labelX6.Text = "SYMBOLS";
            // 
            // metroTabPanel4
            // 
            this.metroTabPanel4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.metroTabPanel4.Controls.Add(this.tableLayoutPanel3);
            this.metroTabPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabPanel4.Location = new System.Drawing.Point(0, 51);
            this.metroTabPanel4.Name = "metroTabPanel4";
            this.metroTabPanel4.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.metroTabPanel4.Size = new System.Drawing.Size(799, 447);
            // 
            // 
            // 
            this.metroTabPanel4.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel4.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel4.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTabPanel4.TabIndex = 4;
            this.metroTabPanel4.Visible = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 873F));
            this.tableLayoutPanel3.Controls.Add(this.listViewLogger, 0, 0);
            this.tableLayoutPanel3.ForeColor = System.Drawing.Color.Black;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(793, 423);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // listViewLogger
            // 
            this.listViewLogger.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewLogger.BackColor = System.Drawing.Color.White;
            this.listViewLogger.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listViewLogger.ForeColor = System.Drawing.Color.Black;
            this.listViewLogger.Location = new System.Drawing.Point(3, 3);
            this.listViewLogger.Name = "listViewLogger";
            this.listViewLogger.ShowItemToolTips = true;
            this.listViewLogger.Size = new System.Drawing.Size(867, 417);
            this.listViewLogger.TabIndex = 0;
            this.listViewLogger.UseCompatibleStateImageBehavior = false;
            this.listViewLogger.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Message";
            this.columnHeader3.Width = 507;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Date";
            this.columnHeader4.Width = 146;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Category";
            this.columnHeader5.Width = 127;
            // 
            // metroTabPanel1
            // 
            this.metroTabPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.metroTabPanel1.Controls.Add(this.panelEx5);
            this.metroTabPanel1.Controls.Add(this.panelEx1);
            this.metroTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabPanel1.Location = new System.Drawing.Point(0, 0);
            this.metroTabPanel1.Name = "metroTabPanel1";
            this.metroTabPanel1.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.metroTabPanel1.Size = new System.Drawing.Size(799, 498);
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
            this.metroTabPanel1.StyleMouseOver.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.metroTabPanel1.TabIndex = 0;
            this.metroTabPanel1.Visible = false;
            // 
            // panelEx5
            // 
            this.panelEx5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelEx5.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx5.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx5.Controls.Add(this.ui_LabelX_sharedAvaliable);
            this.panelEx5.Controls.Add(this.ui_buttonX_shareConnect);
            this.panelEx5.Controls.Add(this.labelX16);
            this.panelEx5.Location = new System.Drawing.Point(113, 58);
            this.panelEx5.Name = "panelEx5";
            this.panelEx5.Size = new System.Drawing.Size(272, 313);
            this.panelEx5.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx5.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelEx5.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx5.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx5.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx5.Style.GradientAngle = 90;
            this.panelEx5.TabIndex = 17;
            // 
            // ui_LabelX_sharedAvaliable
            // 
            // 
            // 
            // 
            this.ui_LabelX_sharedAvaliable.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_LabelX_sharedAvaliable.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ui_LabelX_sharedAvaliable.ForeColor = System.Drawing.Color.Black;
            this.ui_LabelX_sharedAvaliable.Location = new System.Drawing.Point(113, 34);
            this.ui_LabelX_sharedAvaliable.Name = "ui_LabelX_sharedAvaliable";
            this.ui_LabelX_sharedAvaliable.PaddingRight = 6;
            this.ui_LabelX_sharedAvaliable.Size = new System.Drawing.Size(156, 32);
            this.ui_LabelX_sharedAvaliable.TabIndex = 23;
            this.ui_LabelX_sharedAvaliable.Text = "avaliable";
            this.ui_LabelX_sharedAvaliable.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // ui_buttonX_shareConnect
            // 
            this.ui_buttonX_shareConnect.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_shareConnect.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_shareConnect.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ui_buttonX_shareConnect.Location = new System.Drawing.Point(49, 270);
            this.ui_buttonX_shareConnect.Name = "ui_buttonX_shareConnect";
            this.ui_buttonX_shareConnect.Size = new System.Drawing.Size(144, 29);
            this.ui_buttonX_shareConnect.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_shareConnect.TabIndex = 21;
            this.ui_buttonX_shareConnect.Text = "Connect to Share DB";
            this.ui_buttonX_shareConnect.Click += new System.EventHandler(this.Ui_ButtonX_ShareConnect_Click);
            // 
            // labelX16
            // 
            // 
            // 
            // 
            this.labelX16.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX16.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelX16.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX16.ForeColor = System.Drawing.Color.Black;
            this.labelX16.Location = new System.Drawing.Point(0, 0);
            this.labelX16.Name = "labelX16";
            this.labelX16.PaddingLeft = 6;
            this.labelX16.Size = new System.Drawing.Size(272, 32);
            this.labelX16.TabIndex = 20;
            this.labelX16.Text = "SHARED DB";
            // 
            // panelEx1
            // 
            this.panelEx1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.labelX19);
            this.panelEx1.Controls.Add(this.ui_home_textBoxX_db_historical);
            this.panelEx1.Controls.Add(this.labelX18);
            this.panelEx1.Controls.Add(this.ui_home_textBoxX_db_bar);
            this.panelEx1.Controls.Add(this.ui_LabelX_localAvaliable);
            this.panelEx1.Controls.Add(this.checkBoxX1);
            this.panelEx1.Controls.Add(this.ui_buttonX_localConnect);
            this.panelEx1.Controls.Add(this.labelX5);
            this.panelEx1.Controls.Add(this.labelX4);
            this.panelEx1.Controls.Add(this.ui_home_textBoxX_pwd);
            this.panelEx1.Controls.Add(this.labelX3);
            this.panelEx1.Controls.Add(this.ui_home_textBoxX_uid);
            this.panelEx1.Controls.Add(this.labelX2);
            this.panelEx1.Controls.Add(this.ui_home_textBoxX_db);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Controls.Add(this.ui_home_textBoxX_host);
            this.panelEx1.Location = new System.Drawing.Point(391, 58);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(272, 313);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // labelX19
            // 
            // 
            // 
            // 
            this.labelX19.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX19.ForeColor = System.Drawing.Color.Black;
            this.labelX19.Location = new System.Drawing.Point(3, 153);
            this.labelX19.Name = "labelX19";
            this.labelX19.Size = new System.Drawing.Size(107, 20);
            this.labelX19.TabIndex = 28;
            this.labelX19.Text = "historical tick DB";
            this.labelX19.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // ui_home_textBoxX_db_historical
            // 
            this.ui_home_textBoxX_db_historical.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_home_textBoxX_db_historical.Border.BorderLeftColor = System.Drawing.Color.Green;
            this.ui_home_textBoxX_db_historical.Border.BorderLeftWidth = 3;
            this.ui_home_textBoxX_db_historical.Border.Class = "TextBoxBorder";
            this.ui_home_textBoxX_db_historical.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_home_textBoxX_db_historical.ForeColor = System.Drawing.Color.Black;
            this.ui_home_textBoxX_db_historical.Location = new System.Drawing.Point(116, 153);
            this.ui_home_textBoxX_db_historical.Name = "ui_home_textBoxX_db_historical";
            this.ui_home_textBoxX_db_historical.Size = new System.Drawing.Size(128, 22);
            this.ui_home_textBoxX_db_historical.TabIndex = 27;
            // 
            // labelX18
            // 
            // 
            // 
            // 
            this.labelX18.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX18.ForeColor = System.Drawing.Color.Black;
            this.labelX18.Location = new System.Drawing.Point(35, 126);
            this.labelX18.Name = "labelX18";
            this.labelX18.Size = new System.Drawing.Size(75, 20);
            this.labelX18.TabIndex = 26;
            this.labelX18.Text = "bar DB";
            this.labelX18.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // ui_home_textBoxX_db_bar
            // 
            this.ui_home_textBoxX_db_bar.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_home_textBoxX_db_bar.Border.BorderLeftColor = System.Drawing.Color.Green;
            this.ui_home_textBoxX_db_bar.Border.BorderLeftWidth = 3;
            this.ui_home_textBoxX_db_bar.Border.Class = "TextBoxBorder";
            this.ui_home_textBoxX_db_bar.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_home_textBoxX_db_bar.ForeColor = System.Drawing.Color.Black;
            this.ui_home_textBoxX_db_bar.Location = new System.Drawing.Point(116, 126);
            this.ui_home_textBoxX_db_bar.Name = "ui_home_textBoxX_db_bar";
            this.ui_home_textBoxX_db_bar.Size = new System.Drawing.Size(128, 22);
            this.ui_home_textBoxX_db_bar.TabIndex = 25;
            // 
            // ui_LabelX_localAvaliable
            // 
            // 
            // 
            // 
            this.ui_LabelX_localAvaliable.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_LabelX_localAvaliable.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ui_LabelX_localAvaliable.ForeColor = System.Drawing.Color.Black;
            this.ui_LabelX_localAvaliable.Location = new System.Drawing.Point(116, 34);
            this.ui_LabelX_localAvaliable.Name = "ui_LabelX_localAvaliable";
            this.ui_LabelX_localAvaliable.PaddingRight = 6;
            this.ui_LabelX_localAvaliable.Size = new System.Drawing.Size(153, 32);
            this.ui_LabelX_localAvaliable.TabIndex = 24;
            this.ui_LabelX_localAvaliable.Text = "avaliable";
            this.ui_LabelX_localAvaliable.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // checkBoxX1
            // 
            // 
            // 
            // 
            this.checkBoxX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxX1.Checked = true;
            this.checkBoxX1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxX1.CheckValue = "Y";
            this.checkBoxX1.ForeColor = System.Drawing.Color.Black;
            this.checkBoxX1.Location = new System.Drawing.Point(116, 235);
            this.checkBoxX1.Name = "checkBoxX1";
            this.checkBoxX1.Size = new System.Drawing.Size(100, 23);
            this.checkBoxX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBoxX1.TabIndex = 4;
            this.checkBoxX1.Text = "Save me";
            // 
            // ui_buttonX_localConnect
            // 
            this.ui_buttonX_localConnect.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_localConnect.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_localConnect.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ui_buttonX_localConnect.Location = new System.Drawing.Point(60, 270);
            this.ui_buttonX_localConnect.Name = "ui_buttonX_localConnect";
            this.ui_buttonX_localConnect.Size = new System.Drawing.Size(144, 29);
            this.ui_buttonX_localConnect.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_localConnect.TabIndex = 5;
            this.ui_buttonX_localConnect.Text = "Connect to Local DB";
            this.ui_buttonX_localConnect.Click += new System.EventHandler(this.Ui_ButtonX_LocalConnect_Click);
            // 
            // labelX5
            // 
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelX5.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX5.ForeColor = System.Drawing.Color.Black;
            this.labelX5.Location = new System.Drawing.Point(0, 0);
            this.labelX5.Name = "labelX5";
            this.labelX5.PaddingLeft = 6;
            this.labelX5.Size = new System.Drawing.Size(272, 32);
            this.labelX5.TabIndex = 19;
            this.labelX5.Text = "LOCAL DB SETTINGS";
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.ForeColor = System.Drawing.Color.Black;
            this.labelX4.Location = new System.Drawing.Point(35, 207);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(75, 20);
            this.labelX4.TabIndex = 18;
            this.labelX4.Text = "password";
            this.labelX4.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // ui_home_textBoxX_pwd
            // 
            this.ui_home_textBoxX_pwd.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_home_textBoxX_pwd.Border.BorderLeftColor = System.Drawing.Color.Green;
            this.ui_home_textBoxX_pwd.Border.BorderLeftWidth = 3;
            this.ui_home_textBoxX_pwd.Border.Class = "TextBoxBorder";
            this.ui_home_textBoxX_pwd.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_home_textBoxX_pwd.ForeColor = System.Drawing.Color.Black;
            this.ui_home_textBoxX_pwd.Location = new System.Drawing.Point(116, 207);
            this.ui_home_textBoxX_pwd.Name = "ui_home_textBoxX_pwd";
            this.ui_home_textBoxX_pwd.PasswordChar = '*';
            this.ui_home_textBoxX_pwd.Size = new System.Drawing.Size(128, 22);
            this.ui_home_textBoxX_pwd.TabIndex = 3;
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.ForeColor = System.Drawing.Color.Black;
            this.labelX3.Location = new System.Drawing.Point(35, 181);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 20);
            this.labelX3.TabIndex = 16;
            this.labelX3.Text = "user";
            this.labelX3.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // ui_home_textBoxX_uid
            // 
            this.ui_home_textBoxX_uid.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_home_textBoxX_uid.Border.BorderLeftColor = System.Drawing.Color.Green;
            this.ui_home_textBoxX_uid.Border.BorderLeftWidth = 3;
            this.ui_home_textBoxX_uid.Border.Class = "TextBoxBorder";
            this.ui_home_textBoxX_uid.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_home_textBoxX_uid.ForeColor = System.Drawing.Color.Black;
            this.ui_home_textBoxX_uid.Location = new System.Drawing.Point(116, 181);
            this.ui_home_textBoxX_uid.Name = "ui_home_textBoxX_uid";
            this.ui_home_textBoxX_uid.Size = new System.Drawing.Size(128, 22);
            this.ui_home_textBoxX_uid.TabIndex = 2;
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.ForeColor = System.Drawing.Color.Black;
            this.labelX2.Location = new System.Drawing.Point(35, 100);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 20);
            this.labelX2.TabIndex = 14;
            this.labelX2.Text = "system DB";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // ui_home_textBoxX_db
            // 
            this.ui_home_textBoxX_db.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_home_textBoxX_db.Border.BorderLeftColor = System.Drawing.Color.Green;
            this.ui_home_textBoxX_db.Border.BorderLeftWidth = 3;
            this.ui_home_textBoxX_db.Border.Class = "TextBoxBorder";
            this.ui_home_textBoxX_db.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_home_textBoxX_db.ForeColor = System.Drawing.Color.Black;
            this.ui_home_textBoxX_db.Location = new System.Drawing.Point(116, 100);
            this.ui_home_textBoxX_db.Name = "ui_home_textBoxX_db";
            this.ui_home_textBoxX_db.Size = new System.Drawing.Size(128, 22);
            this.ui_home_textBoxX_db.TabIndex = 1;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Location = new System.Drawing.Point(35, 74);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 20);
            this.labelX1.TabIndex = 12;
            this.labelX1.Text = "host";
            this.labelX1.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // ui_home_textBoxX_host
            // 
            this.ui_home_textBoxX_host.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_home_textBoxX_host.Border.BorderLeftColor = System.Drawing.Color.Green;
            this.ui_home_textBoxX_host.Border.BorderLeftWidth = 3;
            this.ui_home_textBoxX_host.Border.Class = "TextBoxBorder";
            this.ui_home_textBoxX_host.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_home_textBoxX_host.ForeColor = System.Drawing.Color.Black;
            this.ui_home_textBoxX_host.Location = new System.Drawing.Point(116, 74);
            this.ui_home_textBoxX_host.Name = "ui_home_textBoxX_host";
            this.ui_home_textBoxX_host.Size = new System.Drawing.Size(128, 22);
            this.ui_home_textBoxX_host.TabIndex = 0;
            // 
            // metroAppButton1
            // 
            this.metroAppButton1.AutoExpandOnClick = true;
            this.metroAppButton1.CanCustomize = false;
            this.metroAppButton1.ImageFixedSize = new System.Drawing.Size(16, 16);
            this.metroAppButton1.ImagePaddingHorizontal = 0;
            this.metroAppButton1.ImagePaddingVertical = 0;
            this.metroAppButton1.Name = "metroAppButton1";
            this.metroAppButton1.ShowSubItems = false;
            this.metroAppButton1.Text = "&File";
            this.metroAppButton1.Visible = false;
            // 
            // metroTabItem1
            // 
            this.metroTabItem1.Name = "metroTabItem1";
            this.metroTabItem1.Panel = this.metroTabPanel1;
            this.metroTabItem1.Text = "&HOME";
            // 
            // metroTabItem2
            // 
            this.metroTabItem2.Checked = true;
            this.metroTabItem2.Name = "metroTabItem2";
            this.metroTabItem2.Panel = this.metroTabPanel2;
            this.metroTabItem2.Text = "&COLLECT DATA";
            this.metroTabItem2.Visible = false;
            // 
            // metroTabItem3
            // 
            this.metroTabItem3.Name = "metroTabItem3";
            this.metroTabItem3.Panel = this.metroTabPanel3;
            this.metroTabItem3.Text = "&MISSING BARS";
            this.metroTabItem3.Visible = false;
            // 
            // metroTabItem5
            // 
            this.metroTabItem5.Name = "metroTabItem5";
            this.metroTabItem5.Panel = this.metroTabPanel5;
            this.metroTabItem5.Text = "ADDITIONAL INFO";
            this.metroTabItem5.Visible = false;
            // 
            // metroTabItem4
            // 
            this.metroTabItem4.Name = "metroTabItem4";
            this.metroTabItem4.Panel = this.metroTabPanel4;
            this.metroTabItem4.Text = "&LOG";
            // 
            // contextMenuStripGroups
            // 
            this.contextMenuStripGroups.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem6_selectAll,
            this.toolStripMenuItem_unselectAll});
            this.contextMenuStripGroups.Name = "contextMenuStrip1";
            this.contextMenuStripGroups.Size = new System.Drawing.Size(135, 48);
            // 
            // toolStripMenuItem6_selectAll
            // 
            this.toolStripMenuItem6_selectAll.Name = "toolStripMenuItem6_selectAll";
            this.toolStripMenuItem6_selectAll.Size = new System.Drawing.Size(134, 22);
            this.toolStripMenuItem6_selectAll.Text = "Select all";
            // 
            // toolStripMenuItem_unselectAll
            // 
            this.toolStripMenuItem_unselectAll.Name = "toolStripMenuItem_unselectAll";
            this.toolStripMenuItem_unselectAll.Size = new System.Drawing.Size(134, 22);
            this.toolStripMenuItem_unselectAll.Text = "Unselect all";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 6);
            // 
            // editListToolStripMenuItem
            // 
            this.editListToolStripMenuItem.Name = "editListToolStripMenuItem";
            this.editListToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // deleteListToolStripMenuItem
            // 
            this.deleteListToolStripMenuItem.Name = "deleteListToolStripMenuItem";
            this.deleteListToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerColorTint = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Metro;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.Green);
            // 
            // FormMainDN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.metroStatusBar1);
            this.Controls.Add(this.metroShell1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "FormMainDN";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Net";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.metroShell1.ResumeLayout(false);
            this.metroShell1.PerformLayout();
            this.metroTabPanel3.ResumeLayout(false);
            this.tableLayoutPanel_missingBar.ResumeLayout(false);
            this.ui_missingbar_panelEx_symbolsBack.ResumeLayout(false);
            this.ui_missingbars_panelEx_list_back.ResumeLayout(false);
            this.contextMenuStripTables.ResumeLayout(false);
            this.metroTabPanel5.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panelEx11.ResumeLayout(false);
            this.panelEx12.ResumeLayout(false);
            this.panelEx9.ResumeLayout(false);
            this.panelEx10.ResumeLayout(false);
            this.metroTabPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelEx3.ResumeLayout(false);
            this.panelEx3.PerformLayout();
            this.panelEx6.ResumeLayout(false);
            this.panelEx4.ResumeLayout(false);
            this.panelEx7.ResumeLayout(false);
            this.panelExBARS.ResumeLayout(false);
            this.panelExBARS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEndBar)).EndInit();
            this.panelExTimeInterval.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeInputStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeInputEnd)).EndInit();
            this.grbDataCollectType.ResumeLayout(false);
            this.grbDataCollectType.PerformLayout();
            this.panelEx2.ResumeLayout(false);
            this.panelEx2.PerformLayout();
            this.panelEx8.ResumeLayout(false);
            this.contextMenuStripSymbols.ResumeLayout(false);
            this.metroTabPanel4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.metroTabPanel1.ResumeLayout(false);
            this.panelEx5.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.contextMenuStripGroups.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Metro.MetroShell metroShell1;
        private DevComponents.DotNetBar.Metro.MetroTabPanel metroTabPanel1;
        private DevComponents.DotNetBar.Metro.MetroTabPanel metroTabPanel2;
        private DevComponents.DotNetBar.Metro.MetroAppButton metroAppButton1;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem1;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem2;
        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.Metro.MetroStatusBar metroStatusBar1;
        private DevComponents.DotNetBar.LabelItem ui__status_labelItem_status;
        private DevComponents.DotNetBar.Metro.MetroTabPanel metroTabPanel3;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem3;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX1;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_localConnect;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.TextBoxX ui_home_textBoxX_pwd;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX ui_home_textBoxX_uid;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX ui_home_textBoxX_db;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX ui_home_textBoxX_host;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSymbols;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ListView listViewResult;
        private System.Windows.Forms.ColumnHeader columnHeaderState;
        private System.Windows.Forms.ColumnHeader columnHeaderStart;
        private System.Windows.Forms.ColumnHeader columnHeaderEnd;
        //private System.Windows.Forms.ContextMenuStrip contextMenuStripLists;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem deleteListToolStripMenuItem;
        private DevComponents.DotNetBar.LabelItem labelItemUserName;
        private System.Windows.Forms.ToolStripMenuItem unselectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem editListToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripGroups;
        private DevComponents.DotNetBar.LabelItem labelItemStatusCQG;
        private System.Windows.Forms.ToolTip toolTip1;
        private DevComponents.DotNetBar.Metro.MetroTilePanel metroTilePanel1;
        private DevComponents.DotNetBar.Metro.MetroTileItem ui_metroTileItem_missingBar;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTables;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private DevComponents.DotNetBar.Metro.MetroTabPanel metroTabPanel4;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem4;
        private System.Windows.Forms.ListView listViewLogger;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevComponents.DotNetBar.PanelEx panelEx3;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.PanelEx panelEx4;
        private DevComponents.DotNetBar.PanelEx grbDataCollectType;
        private System.Windows.Forms.RadioButton radioButtonTick;
        private System.Windows.Forms.RadioButton radioButBars;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private System.Windows.Forms.ListBox ui_listBox_symbols;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelItem ui_status_labelItemStatusSB;
        private DevComponents.DotNetBar.ProgressBarItem progressBarItemCollecting;
        private DevComponents.DotNetBar.PanelEx panelExTimeInterval;
        private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dateTimeInputEnd;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dateTimeInputStart;
        private DevComponents.DotNetBar.PanelEx panelExBARS;
        private DevComponents.DotNetBar.LabelX labelX14;
        private DevComponents.DotNetBar.LabelX labelX13;
        private System.Windows.Forms.NumericUpDown nudStartBar;
        private System.Windows.Forms.NumericUpDown nudEndBar;
        private DevComponents.DotNetBar.LabelX labelX11;
        internal System.Windows.Forms.ComboBox cmbContinuationType;
        internal System.Windows.Forms.ComboBox cmbHistoricalPeriod;
        private DevComponents.DotNetBar.LabelX labelX12;
        private System.Windows.Forms.RadioButton rdb31;
        private System.Windows.Forms.RadioButton rdb1;
        private System.Windows.Forms.ColumnHeader columnHeaderDate;
        private System.Windows.Forms.ColumnHeader columnHeaderStartDay;
        private System.Windows.Forms.ColumnHeader columnHeaderEndDay;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem ui_ToolStripMenuItem_EditSymbols;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_missingBar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private DevComponents.DotNetBar.PanelEx panelEx5;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_shareConnect;
        private DevComponents.DotNetBar.LabelX labelX16;
        private DevComponents.DotNetBar.LabelX ui_LabelX_sharedAvaliable;
        private DevComponents.DotNetBar.LabelX ui_LabelX_localAvaliable;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6_selectAll;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_unselectAll;
        private DevComponents.DotNetBar.PanelEx ui_missingbar_panelEx_symbolsBack;
        private System.Windows.Forms.ListBox ui_listBox_symbolsForMissing;
        private DevComponents.DotNetBar.PanelEx ui_missingbars_panelEx_list_back;
        private DevComponents.DotNetBar.LabelX labelX17;
        private DevComponents.DotNetBar.PanelEx panelEx8;
        private DevComponents.DotNetBar.LabelX labelX18;
        private DevComponents.DotNetBar.Controls.TextBoxX ui_home_textBoxX_db_bar;
        private DevComponents.DotNetBar.LabelX labelX19;
        private DevComponents.DotNetBar.Controls.TextBoxX ui_home_textBoxX_db_historical;
        private Controls.StyledListControl styledListControl1;
        private DevComponents.DotNetBar.PanelEx panelEx6;
        private DevComponents.DotNetBar.Controls.SwitchButton switchButton_changeMode;
        private DevComponents.DotNetBar.ButtonX buttonX_stopCollecting;
        private DevComponents.DotNetBar.LabelX labelX15;
        private DevComponents.DotNetBar.Controls.SwitchButton ui_checkBoxAuto_CheckForMissedBars;
               
        private DevComponents.DotNetBar.ButtonX buttonX_StartCollectGroups;
        private DevComponents.DotNetBar.ButtonX buttonX_StartCollectSymbols;
        private DevComponents.DotNetBar.LabelItem labelItem_collecting;
        private System.Windows.Forms.LinkLabel linkLabel_edit;
        private System.Windows.Forms.LinkLabel linkLabel_selectNone;
        private System.Windows.Forms.LinkLabel linkLabel_selectAll;
        private DevComponents.DotNetBar.PanelEx panelEx7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private DevComponents.DotNetBar.LabelItem labelItem_server;
        private System.Windows.Forms.LinkLabel linkLabel_sort_tf;
        private System.Windows.Forms.LinkLabel linkLabel_sort_name;
        private System.Windows.Forms.LinkLabel linkLabel_sort_time;
        private DevComponents.DotNetBar.Metro.MetroTabPanel metroTabPanel5;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ListView listView1;
        private DevComponents.DotNetBar.PanelEx panelEx9;
        private DevComponents.DotNetBar.PanelEx panelEx10;
        private System.Windows.Forms.ListBox listBox_daily_symbols;
        private DevComponents.DotNetBar.LabelX labelX20;
        private DevComponents.DotNetBar.PanelEx panelEx11;
        private DevComponents.DotNetBar.PanelEx panelEx12;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private DevComponents.DotNetBar.ButtonX buttonX_daily_getValues;
        private DevComponents.DotNetBar.ButtonX buttonX_daily_updateValues;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;        
    }
}

