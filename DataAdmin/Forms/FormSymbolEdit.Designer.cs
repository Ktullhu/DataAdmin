namespace DataAdmin.Forms
{
    partial class FormSymbolEdit
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
            this.ui_buttonX_Cancel = new DevComponents.DotNetBar.ButtonX();
            this.ui_buttonX_Save = new DevComponents.DotNetBar.ButtonX();
            this.ui_textBoxX_SymbolName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.ui_labelX_Symbol = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // ui_buttonX_Cancel
            // 
            this.ui_buttonX_Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_buttonX_Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_Cancel.Location = new System.Drawing.Point(161, 83);
            this.ui_buttonX_Cancel.Margin = new System.Windows.Forms.Padding(2);
            this.ui_buttonX_Cancel.Name = "ui_buttonX_Cancel";
            this.ui_buttonX_Cancel.Size = new System.Drawing.Size(61, 35);
            this.ui_buttonX_Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_Cancel.TabIndex = 31;
            this.ui_buttonX_Cancel.TabStop = false;
            this.ui_buttonX_Cancel.Text = "Cancel";
            this.ui_buttonX_Cancel.Click += new System.EventHandler(this.ui_buttonX_Cancel_Click);
            // 
            // ui_buttonX_Save
            // 
            this.ui_buttonX_Save.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_buttonX_Save.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_Save.Location = new System.Drawing.Point(20, 83);
            this.ui_buttonX_Save.Margin = new System.Windows.Forms.Padding(2);
            this.ui_buttonX_Save.Name = "ui_buttonX_Save";
            this.ui_buttonX_Save.Size = new System.Drawing.Size(61, 35);
            this.ui_buttonX_Save.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_Save.TabIndex = 30;
            this.ui_buttonX_Save.TabStop = false;
            this.ui_buttonX_Save.Text = "Save";
            this.ui_buttonX_Save.Click += new System.EventHandler(this.ui_buttonX_Save_Click);
            // 
            // ui_textBoxX_SymbolName
            // 
            this.ui_textBoxX_SymbolName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_textBoxX_SymbolName.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_textBoxX_SymbolName.Border.Class = "TextBoxBorder";
            this.ui_textBoxX_SymbolName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_textBoxX_SymbolName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.ui_textBoxX_SymbolName.ForeColor = System.Drawing.Color.Black;
            this.ui_textBoxX_SymbolName.Location = new System.Drawing.Point(20, 45);
            this.ui_textBoxX_SymbolName.MaxLength = 50;
            this.ui_textBoxX_SymbolName.Name = "ui_textBoxX_SymbolName";
            this.ui_textBoxX_SymbolName.Size = new System.Drawing.Size(202, 26);
            this.ui_textBoxX_SymbolName.TabIndex = 194;
            this.ui_textBoxX_SymbolName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ui_textBoxX_SymbolName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ui_textBoxX_SymbolName_KeyDown);
            // 
            // ui_labelX_Symbol
            // 
            this.ui_labelX_Symbol.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.ui_labelX_Symbol.BackgroundStyle.BackColor = System.Drawing.Color.Transparent;
            this.ui_labelX_Symbol.BackgroundStyle.BackColor2 = System.Drawing.Color.Transparent;
            this.ui_labelX_Symbol.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_labelX_Symbol.Font = new System.Drawing.Font("Segoe UI Symbol", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ui_labelX_Symbol.ForeColor = System.Drawing.Color.White;
            this.ui_labelX_Symbol.Location = new System.Drawing.Point(20, 12);
            this.ui_labelX_Symbol.Name = "ui_labelX_Symbol";
            this.ui_labelX_Symbol.Size = new System.Drawing.Size(100, 23);
            this.ui_labelX_Symbol.TabIndex = 193;
            this.ui_labelX_Symbol.Text = "Contract";
            // 
            // FormSymbolEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(244, 130);
            this.Controls.Add(this.ui_textBoxX_SymbolName);
            this.Controls.Add(this.ui_labelX_Symbol);
            this.Controls.Add(this.ui_buttonX_Cancel);
            this.Controls.Add(this.ui_buttonX_Save);
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSymbolEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Edit Contract";
            this.Shown += new System.EventHandler(this.FormSymbolEdit_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX ui_buttonX_Cancel;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_Save;
        private DevComponents.DotNetBar.LabelX ui_labelX_Symbol;
        public DevComponents.DotNetBar.Controls.TextBoxX ui_textBoxX_SymbolName;
    }
}