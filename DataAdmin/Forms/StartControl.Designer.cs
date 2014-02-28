namespace DataAdmin.Forms
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
            this.ui_textBoxX_login = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.ui_textBoxX_password = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.ui_buttonX_logon = new DevComponents.DotNetBar.ButtonX();
            this.ui_buttonX_exit = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.ui_textBoxX_db = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.ui_textBoxX_host = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.ui_checkBoxX_autoLogin = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.textBoxX_db_live = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.textBoxX_db_bar = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.textBoxX_db_hbar = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.SuspendLayout();
            // 
            // ui_textBoxX_login
            // 
            this.ui_textBoxX_login.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ui_textBoxX_login.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_textBoxX_login.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.ui_textBoxX_login.Border.BorderLeftColor = System.Drawing.Color.SteelBlue;
            this.ui_textBoxX_login.Border.BorderLeftWidth = 3;
            this.ui_textBoxX_login.Border.Class = "TextBoxBorder";
            this.ui_textBoxX_login.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_textBoxX_login.ForeColor = System.Drawing.Color.Black;
            this.ui_textBoxX_login.Location = new System.Drawing.Point(300, 94);
            this.ui_textBoxX_login.Name = "ui_textBoxX_login";
            this.ui_textBoxX_login.Size = new System.Drawing.Size(198, 22);
            this.ui_textBoxX_login.TabIndex = 0;
            // 
            // ui_textBoxX_password
            // 
            this.ui_textBoxX_password.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ui_textBoxX_password.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_textBoxX_password.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.ui_textBoxX_password.Border.BorderLeftColor = System.Drawing.Color.SteelBlue;
            this.ui_textBoxX_password.Border.BorderLeftWidth = 3;
            this.ui_textBoxX_password.Border.Class = "TextBoxBorder";
            this.ui_textBoxX_password.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_textBoxX_password.ForeColor = System.Drawing.Color.Black;
            this.ui_textBoxX_password.Location = new System.Drawing.Point(300, 131);
            this.ui_textBoxX_password.Name = "ui_textBoxX_password";
            this.ui_textBoxX_password.PasswordChar = '*';
            this.ui_textBoxX_password.Size = new System.Drawing.Size(198, 22);
            this.ui_textBoxX_password.TabIndex = 1;
            // 
            // labelX2
            // 
            this.labelX2.Anchor = System.Windows.Forms.AnchorStyles.None;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(219, 91);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 5;
            this.labelX2.Text = "user";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // labelX3
            // 
            this.labelX3.Anchor = System.Windows.Forms.AnchorStyles.None;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(219, 128);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
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
            this.ui_buttonX_logon.Location = new System.Drawing.Point(300, 378);
            this.ui_buttonX_logon.Name = "ui_buttonX_logon";
            this.ui_buttonX_logon.Size = new System.Drawing.Size(89, 44);
            this.ui_buttonX_logon.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_logon.TabIndex = 8;
            this.ui_buttonX_logon.Text = "LogOn";
            this.ui_buttonX_logon.Click += new System.EventHandler(this.ui_buttonX_logon_Click);
            // 
            // ui_buttonX_exit
            // 
            this.ui_buttonX_exit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_exit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ui_buttonX_exit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_exit.Location = new System.Drawing.Point(409, 378);
            this.ui_buttonX_exit.Name = "ui_buttonX_exit";
            this.ui_buttonX_exit.Size = new System.Drawing.Size(89, 44);
            this.ui_buttonX_exit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_exit.TabIndex = 9;
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
            this.labelX1.ForeColor = System.Drawing.Color.SteelBlue;
            this.labelX1.Location = new System.Drawing.Point(23, 9);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(148, 36);
            this.labelX1.TabIndex = 7;
            this.labelX1.Text = "DATA ADMIN";
            // 
            // labelX4
            // 
            this.labelX4.Anchor = System.Windows.Forms.AnchorStyles.None;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(163, 202);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(131, 23);
            this.labelX4.TabIndex = 15;
            this.labelX4.Text = "system DB";
            this.labelX4.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // labelX5
            // 
            this.labelX5.Anchor = System.Windows.Forms.AnchorStyles.None;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Location = new System.Drawing.Point(219, 165);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(75, 23);
            this.labelX5.TabIndex = 14;
            this.labelX5.Text = "host";
            this.labelX5.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // ui_textBoxX_db
            // 
            this.ui_textBoxX_db.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ui_textBoxX_db.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_textBoxX_db.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.ui_textBoxX_db.Border.BorderLeftColor = System.Drawing.Color.SteelBlue;
            this.ui_textBoxX_db.Border.BorderLeftWidth = 3;
            this.ui_textBoxX_db.Border.Class = "TextBoxBorder";
            this.ui_textBoxX_db.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_textBoxX_db.ForeColor = System.Drawing.Color.Black;
            this.ui_textBoxX_db.Location = new System.Drawing.Point(300, 205);
            this.ui_textBoxX_db.Name = "ui_textBoxX_db";
            this.ui_textBoxX_db.Size = new System.Drawing.Size(198, 22);
            this.ui_textBoxX_db.TabIndex = 3;
            // 
            // ui_textBoxX_host
            // 
            this.ui_textBoxX_host.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ui_textBoxX_host.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_textBoxX_host.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.ui_textBoxX_host.Border.BorderLeftColor = System.Drawing.Color.SteelBlue;
            this.ui_textBoxX_host.Border.BorderLeftWidth = 3;
            this.ui_textBoxX_host.Border.Class = "TextBoxBorder";
            this.ui_textBoxX_host.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_textBoxX_host.ForeColor = System.Drawing.Color.Black;
            this.ui_textBoxX_host.Location = new System.Drawing.Point(300, 168);
            this.ui_textBoxX_host.Name = "ui_textBoxX_host";
            this.ui_textBoxX_host.Size = new System.Drawing.Size(198, 22);
            this.ui_textBoxX_host.TabIndex = 2;
            // 
            // ui_checkBoxX_autoLogin
            // 
            this.ui_checkBoxX_autoLogin.Anchor = System.Windows.Forms.AnchorStyles.None;
            // 
            // 
            // 
            this.ui_checkBoxX_autoLogin.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_checkBoxX_autoLogin.Checked = true;
            this.ui_checkBoxX_autoLogin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ui_checkBoxX_autoLogin.CheckValue = "Y";
            this.ui_checkBoxX_autoLogin.ForeColor = System.Drawing.Color.Black;
            this.ui_checkBoxX_autoLogin.Location = new System.Drawing.Point(300, 344);
            this.ui_checkBoxX_autoLogin.Name = "ui_checkBoxX_autoLogin";
            this.ui_checkBoxX_autoLogin.Size = new System.Drawing.Size(100, 23);
            this.ui_checkBoxX_autoLogin.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_checkBoxX_autoLogin.TabIndex = 7;
            this.ui_checkBoxX_autoLogin.Text = "Auto login";
            // 
            // labelX6
            // 
            this.labelX6.Anchor = System.Windows.Forms.AnchorStyles.None;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Location = new System.Drawing.Point(163, 239);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(131, 23);
            this.labelX6.TabIndex = 17;
            this.labelX6.Text = "live DB";
            this.labelX6.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // textBoxX_db_live
            // 
            this.textBoxX_db_live.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxX_db_live.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textBoxX_db_live.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.textBoxX_db_live.Border.BorderLeftColor = System.Drawing.Color.SteelBlue;
            this.textBoxX_db_live.Border.BorderLeftWidth = 3;
            this.textBoxX_db_live.Border.Class = "TextBoxBorder";
            this.textBoxX_db_live.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX_db_live.ForeColor = System.Drawing.Color.Black;
            this.textBoxX_db_live.Location = new System.Drawing.Point(300, 242);
            this.textBoxX_db_live.Name = "textBoxX_db_live";
            this.textBoxX_db_live.Size = new System.Drawing.Size(198, 22);
            this.textBoxX_db_live.TabIndex = 4;
            // 
            // labelX7
            // 
            this.labelX7.Anchor = System.Windows.Forms.AnchorStyles.None;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Location = new System.Drawing.Point(163, 276);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(131, 23);
            this.labelX7.TabIndex = 19;
            this.labelX7.Text = "bar DB";
            this.labelX7.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // textBoxX_db_bar
            // 
            this.textBoxX_db_bar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxX_db_bar.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textBoxX_db_bar.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.textBoxX_db_bar.Border.BorderLeftColor = System.Drawing.Color.SteelBlue;
            this.textBoxX_db_bar.Border.BorderLeftWidth = 3;
            this.textBoxX_db_bar.Border.Class = "TextBoxBorder";
            this.textBoxX_db_bar.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX_db_bar.ForeColor = System.Drawing.Color.Black;
            this.textBoxX_db_bar.Location = new System.Drawing.Point(300, 279);
            this.textBoxX_db_bar.Name = "textBoxX_db_bar";
            this.textBoxX_db_bar.Size = new System.Drawing.Size(198, 22);
            this.textBoxX_db_bar.TabIndex = 5;
            // 
            // labelX8
            // 
            this.labelX8.Anchor = System.Windows.Forms.AnchorStyles.None;
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX8.Location = new System.Drawing.Point(163, 313);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(131, 23);
            this.labelX8.TabIndex = 21;
            this.labelX8.Text = "historical tick DB";
            this.labelX8.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // textBoxX_db_hbar
            // 
            this.textBoxX_db_hbar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxX_db_hbar.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textBoxX_db_hbar.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.textBoxX_db_hbar.Border.BorderLeftColor = System.Drawing.Color.SteelBlue;
            this.textBoxX_db_hbar.Border.BorderLeftWidth = 3;
            this.textBoxX_db_hbar.Border.Class = "TextBoxBorder";
            this.textBoxX_db_hbar.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX_db_hbar.ForeColor = System.Drawing.Color.Black;
            this.textBoxX_db_hbar.Location = new System.Drawing.Point(300, 316);
            this.textBoxX_db_hbar.Name = "textBoxX_db_hbar";
            this.textBoxX_db_hbar.Size = new System.Drawing.Size(198, 22);
            this.textBoxX_db_hbar.TabIndex = 6;
            // 
            // StartControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.labelX8);
            this.Controls.Add(this.textBoxX_db_hbar);
            this.Controls.Add(this.labelX7);
            this.Controls.Add(this.textBoxX_db_bar);
            this.Controls.Add(this.labelX6);
            this.Controls.Add(this.textBoxX_db_live);
            this.Controls.Add(this.ui_checkBoxX_autoLogin);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.ui_textBoxX_db);
            this.Controls.Add(this.ui_textBoxX_host);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.ui_buttonX_exit);
            this.Controls.Add(this.ui_buttonX_logon);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.ui_textBoxX_password);
            this.Controls.Add(this.ui_textBoxX_login);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "StartControl";
            this.Size = new System.Drawing.Size(781, 467);
            this.SlideOutButtonVisible = false;
            this.Load += new System.EventHandler(this.StartControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_logon;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_exit;
        internal DevComponents.DotNetBar.Controls.TextBoxX ui_textBoxX_login;
        internal DevComponents.DotNetBar.Controls.TextBoxX ui_textBoxX_password;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX5;
        internal DevComponents.DotNetBar.Controls.TextBoxX ui_textBoxX_db;
        internal DevComponents.DotNetBar.Controls.TextBoxX ui_textBoxX_host;
        public DevComponents.DotNetBar.Controls.CheckBoxX ui_checkBoxX_autoLogin;
        private DevComponents.DotNetBar.LabelX labelX6;
        internal DevComponents.DotNetBar.Controls.TextBoxX textBoxX_db_live;
        private DevComponents.DotNetBar.LabelX labelX7;
        internal DevComponents.DotNetBar.Controls.TextBoxX textBoxX_db_bar;
        private DevComponents.DotNetBar.LabelX labelX8;
        internal DevComponents.DotNetBar.Controls.TextBoxX textBoxX_db_hbar;
    }
}
