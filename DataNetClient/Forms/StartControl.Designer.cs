namespace DataNetClient.Forms
{
    partial class StartControl
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
            this.ui_textBoxX_password = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.ui_buttonX_logon = new DevComponents.DotNetBar.ButtonX();
            this.ui_buttonX_exit = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.panelEx7 = new DevComponents.DotNetBar.PanelEx();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ui_labelX_CQGstatus = new DevComponents.DotNetBar.LabelX();
            this.uiServerStatus = new DevComponents.DotNetBar.LabelX();
            this.uiServerOnlineFakeSymbol = new DevComponents.DotNetBar.LabelX();
            this.uiOfflineFakeSymbol = new DevComponents.DotNetBar.LabelX();
            this.ui_textBoxX_login = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.ui_textBox_ip = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.ui_textBox_ip_slave = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.uiServerStatus2 = new DevComponents.DotNetBar.LabelX();
            this.panelEx7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ui_textBoxX_password
            // 
            this.ui_textBoxX_password.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ui_textBoxX_password.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_textBoxX_password.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.ui_textBoxX_password.Border.BorderLeftColor = System.Drawing.Color.Green;
            this.ui_textBoxX_password.Border.BorderLeftWidth = 3;
            this.ui_textBoxX_password.Border.Class = "TextBoxBorder";
            this.ui_textBoxX_password.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_textBoxX_password.ForeColor = System.Drawing.Color.Black;
            this.ui_textBoxX_password.Location = new System.Drawing.Point(325, 286);
            this.ui_textBoxX_password.Name = "ui_textBoxX_password";
            this.ui_textBoxX_password.PasswordChar = '*';
            this.ui_textBoxX_password.Size = new System.Drawing.Size(198, 22);
            this.ui_textBoxX_password.TabIndex = 3;
            this.ui_textBoxX_password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Ui_TextBoxX_Login_KeyDown);
            // 
            // labelX2
            // 
            this.labelX2.Anchor = System.Windows.Forms.AnchorStyles.None;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(223, 247);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(96, 23);
            this.labelX2.TabIndex = 5;
            this.labelX2.Text = "login";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // labelX3
            // 
            this.labelX3.Anchor = System.Windows.Forms.AnchorStyles.None;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(223, 283);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(96, 23);
            this.labelX3.TabIndex = 6;
            this.labelX3.Text = "password";
            this.labelX3.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // ui_buttonX_logon
            // 
            this.ui_buttonX_logon.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_logon.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ui_buttonX_logon.BackColor = System.Drawing.Color.SteelBlue;
            this.ui_buttonX_logon.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_logon.Enabled = false;
            this.ui_buttonX_logon.Location = new System.Drawing.Point(251, 333);
            this.ui_buttonX_logon.Name = "ui_buttonX_logon";
            this.ui_buttonX_logon.Size = new System.Drawing.Size(89, 44);
            this.ui_buttonX_logon.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_logon.TabIndex = 4;
            this.ui_buttonX_logon.Text = "LogOn";
            this.ui_buttonX_logon.Click += new System.EventHandler(this.ui_buttonX_logon_Click);
            // 
            // ui_buttonX_exit
            // 
            this.ui_buttonX_exit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_exit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ui_buttonX_exit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_exit.Location = new System.Drawing.Point(434, 333);
            this.ui_buttonX_exit.Name = "ui_buttonX_exit";
            this.ui_buttonX_exit.Size = new System.Drawing.Size(89, 44);
            this.ui_buttonX_exit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_exit.TabIndex = 5;
            this.ui_buttonX_exit.Text = "Exit";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelX1.ForeColor = System.Drawing.Color.Green;
            this.labelX1.Location = new System.Drawing.Point(23, 9);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(97, 36);
            this.labelX1.TabIndex = 7;
            this.labelX1.Text = "Data Net";
            // 
            // labelX5
            // 
            this.labelX5.Anchor = System.Windows.Forms.AnchorStyles.None;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Location = new System.Drawing.Point(223, 176);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(96, 23);
            this.labelX5.TabIndex = 14;
            this.labelX5.Text = "IP address master";
            this.labelX5.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // panelEx7
            // 
            this.panelEx7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelEx7.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx7.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx7.Controls.Add(this.pictureBox1);
            this.panelEx7.Controls.Add(this.ui_labelX_CQGstatus);
            this.panelEx7.Location = new System.Drawing.Point(251, 54);
            this.panelEx7.Name = "panelEx7";
            this.panelEx7.Size = new System.Drawing.Size(272, 105);
            this.panelEx7.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx7.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelEx7.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx7.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx7.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx7.Style.GradientAngle = 90;
            this.panelEx7.TabIndex = 17;
            // 
            // pictureBox1
            // 
            this.pictureBox1.ForeColor = System.Drawing.Color.Black;
            this.pictureBox1.Image = global::DataNetClient.Properties.Resources.cqg_logo_color_gray_500x120;
            this.pictureBox1.Location = new System.Drawing.Point(63, 15);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(145, 43);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            // 
            // ui_labelX_CQGstatus
            // 
            // 
            // 
            // 
            this.ui_labelX_CQGstatus.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_labelX_CQGstatus.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_labelX_CQGstatus.ForeColor = System.Drawing.Color.Black;
            this.ui_labelX_CQGstatus.Location = new System.Drawing.Point(16, 71);
            this.ui_labelX_CQGstatus.Name = "ui_labelX_CQGstatus";
            this.ui_labelX_CQGstatus.Size = new System.Drawing.Size(240, 20);
            this.ui_labelX_CQGstatus.TabIndex = 20;
            this.ui_labelX_CQGstatus.Text = "CQG not started";
            this.ui_labelX_CQGstatus.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // uiServerStatus
            // 
            this.uiServerStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
            // 
            // 
            // 
            this.uiServerStatus.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.uiServerStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiServerStatus.ForeColor = System.Drawing.Color.Black;
            this.uiServerStatus.Location = new System.Drawing.Point(529, 172);
            this.uiServerStatus.Name = "uiServerStatus";
            this.uiServerStatus.Size = new System.Drawing.Size(217, 26);
            this.uiServerStatus.Symbol = "";
            this.uiServerStatus.SymbolColor = System.Drawing.Color.Crimson;
            this.uiServerStatus.TabIndex = 18;
            this.uiServerStatus.Text = "Server is offline";
            this.uiServerStatus.Visible = false;
            // 
            // uiServerOnlineFakeSymbol
            // 
            // 
            // 
            // 
            this.uiServerOnlineFakeSymbol.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.uiServerOnlineFakeSymbol.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiServerOnlineFakeSymbol.ForeColor = System.Drawing.Color.Black;
            this.uiServerOnlineFakeSymbol.Location = new System.Drawing.Point(251, 441);
            this.uiServerOnlineFakeSymbol.Name = "uiServerOnlineFakeSymbol";
            this.uiServerOnlineFakeSymbol.Size = new System.Drawing.Size(30, 26);
            this.uiServerOnlineFakeSymbol.Symbol = "";
            this.uiServerOnlineFakeSymbol.SymbolColor = System.Drawing.Color.Green;
            this.uiServerOnlineFakeSymbol.TabIndex = 19;
            this.uiServerOnlineFakeSymbol.Visible = false;
            // 
            // uiOfflineFakeSymbol
            // 
            // 
            // 
            // 
            this.uiOfflineFakeSymbol.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.uiOfflineFakeSymbol.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiOfflineFakeSymbol.ForeColor = System.Drawing.Color.Black;
            this.uiOfflineFakeSymbol.Location = new System.Drawing.Point(287, 441);
            this.uiOfflineFakeSymbol.Name = "uiOfflineFakeSymbol";
            this.uiOfflineFakeSymbol.Size = new System.Drawing.Size(32, 26);
            this.uiOfflineFakeSymbol.Symbol = "";
            this.uiOfflineFakeSymbol.SymbolColor = System.Drawing.Color.Crimson;
            this.uiOfflineFakeSymbol.TabIndex = 20;
            this.uiOfflineFakeSymbol.Visible = false;
            // 
            // ui_textBoxX_login
            // 
            this.ui_textBoxX_login.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ui_textBoxX_login.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_textBoxX_login.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.ui_textBoxX_login.Border.BorderLeftColor = System.Drawing.Color.Green;
            this.ui_textBoxX_login.Border.BorderLeftWidth = 3;
            this.ui_textBoxX_login.Border.Class = "TextBoxBorder";
            this.ui_textBoxX_login.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_textBoxX_login.ForeColor = System.Drawing.Color.Black;
            this.ui_textBoxX_login.Location = new System.Drawing.Point(325, 250);
            this.ui_textBoxX_login.Name = "ui_textBoxX_login";
            this.ui_textBoxX_login.Size = new System.Drawing.Size(198, 22);
            this.ui_textBoxX_login.TabIndex = 2;
            this.ui_textBoxX_login.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Ui_TextBoxX_Login_KeyDown);
            // 
            // ui_textBox_ip
            // 
            this.ui_textBox_ip.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ui_textBox_ip.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_textBox_ip.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.ui_textBox_ip.Border.BorderLeftColor = System.Drawing.Color.Green;
            this.ui_textBox_ip.Border.BorderLeftWidth = 3;
            this.ui_textBox_ip.Border.Class = "TextBoxBorder";
            this.ui_textBox_ip.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_textBox_ip.ForeColor = System.Drawing.Color.Black;
            this.ui_textBox_ip.Location = new System.Drawing.Point(325, 176);
            this.ui_textBox_ip.Name = "ui_textBox_ip";
            this.ui_textBox_ip.Size = new System.Drawing.Size(198, 22);
            this.ui_textBox_ip.TabIndex = 0;
            // 
            // ui_textBox_ip_slave
            // 
            this.ui_textBox_ip_slave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ui_textBox_ip_slave.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_textBox_ip_slave.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.ui_textBox_ip_slave.Border.BorderLeftColor = System.Drawing.Color.Green;
            this.ui_textBox_ip_slave.Border.BorderLeftWidth = 3;
            this.ui_textBox_ip_slave.Border.Class = "TextBoxBorder";
            this.ui_textBox_ip_slave.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_textBox_ip_slave.ForeColor = System.Drawing.Color.Black;
            this.ui_textBox_ip_slave.Location = new System.Drawing.Point(325, 215);
            this.ui_textBox_ip_slave.Name = "ui_textBox_ip_slave";
            this.ui_textBox_ip_slave.Size = new System.Drawing.Size(198, 22);
            this.ui_textBox_ip_slave.TabIndex = 1;
            // 
            // labelX4
            // 
            this.labelX4.Anchor = System.Windows.Forms.AnchorStyles.None;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(223, 212);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(96, 23);
            this.labelX4.TabIndex = 22;
            this.labelX4.Text = "IP address slave";
            this.labelX4.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // uiServerStatus2
            // 
            this.uiServerStatus2.Anchor = System.Windows.Forms.AnchorStyles.None;
            // 
            // 
            // 
            this.uiServerStatus2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.uiServerStatus2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiServerStatus2.ForeColor = System.Drawing.Color.Black;
            this.uiServerStatus2.Location = new System.Drawing.Point(529, 209);
            this.uiServerStatus2.Name = "uiServerStatus2";
            this.uiServerStatus2.Size = new System.Drawing.Size(217, 26);
            this.uiServerStatus2.Symbol = "";
            this.uiServerStatus2.SymbolColor = System.Drawing.Color.Crimson;
            this.uiServerStatus2.TabIndex = 23;
            this.uiServerStatus2.Text = "Server is offline";
            this.uiServerStatus2.Visible = false;
            // 
            // StartControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.uiServerStatus2);
            this.Controls.Add(this.ui_textBox_ip_slave);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.ui_textBox_ip);
            this.Controls.Add(this.uiOfflineFakeSymbol);
            this.Controls.Add(this.uiServerOnlineFakeSymbol);
            this.Controls.Add(this.uiServerStatus);
            this.Controls.Add(this.panelEx7);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.ui_buttonX_exit);
            this.Controls.Add(this.ui_buttonX_logon);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.ui_textBoxX_password);
            this.Controls.Add(this.ui_textBoxX_login);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "StartControl";
            this.Size = new System.Drawing.Size(781, 467);
            this.SlideOutButtonVisible = false;
            this.Load += new System.EventHandler(this.StartControlLoad);
            this.panelEx7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_exit;
        internal DevComponents.DotNetBar.Controls.TextBoxX ui_textBoxX_password;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.PanelEx panelEx7;
        private System.Windows.Forms.PictureBox pictureBox1;
        internal DevComponents.DotNetBar.LabelX ui_labelX_CQGstatus;
        internal DevComponents.DotNetBar.ButtonX ui_buttonX_logon;
        public DevComponents.DotNetBar.LabelX uiServerStatus;
        public DevComponents.DotNetBar.LabelX uiServerOnlineFakeSymbol;
        public DevComponents.DotNetBar.LabelX uiOfflineFakeSymbol;
        internal DevComponents.DotNetBar.Controls.TextBoxX ui_textBoxX_login;
        internal DevComponents.DotNetBar.Controls.TextBoxX ui_textBox_ip;
        internal DevComponents.DotNetBar.Controls.TextBoxX ui_textBox_ip_slave;
        private DevComponents.DotNetBar.LabelX labelX4;
        public DevComponents.DotNetBar.LabelX uiServerStatus2;
    }
}
