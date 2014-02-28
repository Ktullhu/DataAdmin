namespace DataExport.Forms
{
    partial class FormNewProfile
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
            this.ui_textBoxX_ProfileName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.ui_LabelX_Profile = new DevComponents.DotNetBar.LabelX();
            this.panelExBack = new DevComponents.DotNetBar.PanelEx();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.buttonX2 = new DevComponents.DotNetBar.ButtonX();
            this.panelExBack.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_textBoxX_ProfileName
            // 
            this.ui_textBoxX_ProfileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_textBoxX_ProfileName.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_textBoxX_ProfileName.Border.Class = "TextBoxBorder";
            this.ui_textBoxX_ProfileName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_textBoxX_ProfileName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.ui_textBoxX_ProfileName.ForeColor = System.Drawing.Color.Black;
            this.ui_textBoxX_ProfileName.Location = new System.Drawing.Point(14, 40);
            this.ui_textBoxX_ProfileName.MaxLength = 50;
            this.ui_textBoxX_ProfileName.Name = "ui_textBoxX_ProfileName";
            this.ui_textBoxX_ProfileName.Size = new System.Drawing.Size(249, 22);
            this.ui_textBoxX_ProfileName.TabIndex = 194;
            this.ui_textBoxX_ProfileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ui_textBoxX_ProfileName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ui_textBoxX_SymbolName_KeyDown);
            // 
            // ui_LabelX_Profile
            // 
            this.ui_LabelX_Profile.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.ui_LabelX_Profile.BackgroundStyle.BackColor = System.Drawing.Color.Transparent;
            this.ui_LabelX_Profile.BackgroundStyle.BackColor2 = System.Drawing.Color.Transparent;
            this.ui_LabelX_Profile.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_LabelX_Profile.Font = new System.Drawing.Font("Segoe UI Symbol", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ui_LabelX_Profile.ForeColor = System.Drawing.Color.CadetBlue;
            this.ui_LabelX_Profile.Location = new System.Drawing.Point(14, 12);
            this.ui_LabelX_Profile.Name = "ui_LabelX_Profile";
            this.ui_LabelX_Profile.Size = new System.Drawing.Size(100, 23);
            this.ui_LabelX_Profile.TabIndex = 193;
            this.ui_LabelX_Profile.Text = "Profile";
            // 
            // panelExBack
            // 
            this.panelExBack.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelExBack.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelExBack.Controls.Add(this.buttonX1);
            this.panelExBack.Controls.Add(this.ui_textBoxX_ProfileName);
            this.panelExBack.Controls.Add(this.ui_LabelX_Profile);
            this.panelExBack.Controls.Add(this.buttonX2);
            this.panelExBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelExBack.Location = new System.Drawing.Point(0, 0);
            this.panelExBack.Name = "panelExBack";
            this.panelExBack.Size = new System.Drawing.Size(274, 128);
            this.panelExBack.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelExBack.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelExBack.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelExBack.Style.BorderColor.Color = System.Drawing.Color.CadetBlue;
            this.panelExBack.Style.BorderWidth = 2;
            this.panelExBack.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelExBack.Style.GradientAngle = 90;
            this.panelExBack.TabIndex = 195;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.buttonX1.Location = new System.Drawing.Point(12, 77);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(134, 40);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.Symbol = "";
            this.buttonX1.TabIndex = 62;
            this.buttonX1.TabStop = false;
            this.buttonX1.Text = "Ok";
            this.buttonX1.Click += new System.EventHandler(this.buttonX_data_archive_start_Click);
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonX2.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX2.Location = new System.Drawing.Point(166, 77);
            this.buttonX2.Margin = new System.Windows.Forms.Padding(2);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(97, 40);
            this.buttonX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX2.TabIndex = 31;
            this.buttonX2.TabStop = false;
            this.buttonX2.Text = "Cancel";
            this.buttonX2.Click += new System.EventHandler(this.ui_buttonX_Cancel_Click);
            // 
            // FormNewProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(274, 128);
            this.Controls.Add(this.panelExBack);
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormNewProfile";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Add Contract";
            this.Shown += new System.EventHandler(this.FormSymbolAdd_Shown);
            this.panelExBack.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX ui_LabelX_Profile;
        public DevComponents.DotNetBar.Controls.TextBoxX ui_textBoxX_ProfileName;
        private DevComponents.DotNetBar.PanelEx panelExBack;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.ButtonX buttonX2;
    }
}