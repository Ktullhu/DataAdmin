namespace DataExport.Forms
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
            this.uiServerStatus = new DevComponents.DotNetBar.LabelX();
            this.uiServerOnlineFakeSymbol = new DevComponents.DotNetBar.LabelX();
            this.uiOfflineFakeSymbol = new DevComponents.DotNetBar.LabelX();
            this.ui_textBoxX_login = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.ui_textBox_ip = new DevComponents.DotNetBar.Controls.TextBoxX();
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
            this.ui_textBoxX_password.Border.BorderLeftColor = System.Drawing.Color.CadetBlue;
            this.ui_textBoxX_password.Border.BorderLeftWidth = 3;
            this.ui_textBoxX_password.Border.Class = "TextBoxBorder";
            this.ui_textBoxX_password.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_textBoxX_password.ForeColor = System.Drawing.Color.Black;
            this.ui_textBoxX_password.Location = new System.Drawing.Point(325, 286);
            this.ui_textBoxX_password.Name = "ui_textBoxX_password";
            this.ui_textBoxX_password.PasswordChar = '*';
            this.ui_textBoxX_password.Size = new System.Drawing.Size(198, 22);
            this.ui_textBoxX_password.TabIndex = 2;
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
            this.ui_buttonX_logon.BackColor = System.Drawing.Color.CadetBlue;
            this.ui_buttonX_logon.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_logon.Enabled = false;
            this.ui_buttonX_logon.Location = new System.Drawing.Point(251, 333);
            this.ui_buttonX_logon.Name = "ui_buttonX_logon";
            this.ui_buttonX_logon.Size = new System.Drawing.Size(89, 44);
            this.ui_buttonX_logon.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_logon.TabIndex = 3;
            this.ui_buttonX_logon.Text = "LogOn";
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
            this.ui_buttonX_exit.TabIndex = 4;
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
            this.labelX1.ForeColor = System.Drawing.Color.CadetBlue;
            this.labelX1.Location = new System.Drawing.Point(23, 9);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(126, 36);
            this.labelX1.TabIndex = 7;
            this.labelX1.Text = "Data Export";
            // 
            // labelX5
            // 
            this.labelX5.Anchor = System.Windows.Forms.AnchorStyles.None;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Location = new System.Drawing.Point(223, 211);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(96, 23);
            this.labelX5.TabIndex = 14;
            this.labelX5.Text = "IP address";
            this.labelX5.TextAlignment = System.Drawing.StringAlignment.Far;
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
            this.uiServerStatus.Location = new System.Drawing.Point(529, 214);
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
            this.ui_textBoxX_login.Border.BorderLeftColor = System.Drawing.Color.CadetBlue;
            this.ui_textBoxX_login.Border.BorderLeftWidth = 3;
            this.ui_textBoxX_login.Border.Class = "TextBoxBorder";
            this.ui_textBoxX_login.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_textBoxX_login.ForeColor = System.Drawing.Color.Black;
            this.ui_textBoxX_login.Location = new System.Drawing.Point(325, 250);
            this.ui_textBoxX_login.Name = "ui_textBoxX_login";
            this.ui_textBoxX_login.Size = new System.Drawing.Size(198, 22);
            this.ui_textBoxX_login.TabIndex = 1;
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
            this.ui_textBox_ip.Border.BorderLeftColor = System.Drawing.Color.CadetBlue;
            this.ui_textBox_ip.Border.BorderLeftWidth = 3;
            this.ui_textBox_ip.Border.Class = "TextBoxBorder";
            this.ui_textBox_ip.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_textBox_ip.ForeColor = System.Drawing.Color.Black;
            this.ui_textBox_ip.Location = new System.Drawing.Point(325, 214);
            this.ui_textBox_ip.Name = "ui_textBox_ip";
            this.ui_textBox_ip.Size = new System.Drawing.Size(198, 22);
            this.ui_textBox_ip.TabIndex = 0;
            // 
            // StartControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ui_textBox_ip);
            this.Controls.Add(this.uiOfflineFakeSymbol);
            this.Controls.Add(this.uiServerOnlineFakeSymbol);
            this.Controls.Add(this.uiServerStatus);
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
        internal DevComponents.DotNetBar.ButtonX ui_buttonX_logon;
        public DevComponents.DotNetBar.LabelX uiServerStatus;
        public DevComponents.DotNetBar.LabelX uiServerOnlineFakeSymbol;
        public DevComponents.DotNetBar.LabelX uiOfflineFakeSymbol;
        internal DevComponents.DotNetBar.Controls.TextBoxX ui_textBoxX_login;
        internal DevComponents.DotNetBar.Controls.TextBoxX ui_textBox_ip;
    }
}
