namespace DataExport.Forms
{
    partial class FormExistingsFormulas
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
            this.elementContainerControl1 = new DataExport.Controls.ElementContainerControl();
            this.ui_buttonX_save = new DevComponents.DotNetBar.ButtonX();
            this.panelExBack = new DevComponents.DotNetBar.PanelEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.textBoxX_formulaName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.panelExBack.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_buttonX_Cancel
            // 
            this.ui_buttonX_Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_buttonX_Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_Cancel.Location = new System.Drawing.Point(323, 215);
            this.ui_buttonX_Cancel.Margin = new System.Windows.Forms.Padding(2);
            this.ui_buttonX_Cancel.Name = "ui_buttonX_Cancel";
            this.ui_buttonX_Cancel.Size = new System.Drawing.Size(97, 40);
            this.ui_buttonX_Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_Cancel.TabIndex = 31;
            this.ui_buttonX_Cancel.TabStop = false;
            this.ui_buttonX_Cancel.Text = "Cancel";
            this.ui_buttonX_Cancel.Click += new System.EventHandler(this.ui_buttonX_Cancel_Click);
            this.ui_buttonX_Cancel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormExistingsFormulas_KeyDown);
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
            this.elementContainerControl1.Location = new System.Drawing.Point(12, 40);
            this.elementContainerControl1.MinimumSize = new System.Drawing.Size(170, 2);
            this.elementContainerControl1.Name = "elementContainerControl1";
            this.elementContainerControl1.Size = new System.Drawing.Size(407, 169);
            this.elementContainerControl1.TabIndex = 61;
            this.elementContainerControl1.TabStop = false;
            this.elementContainerControl1.Title = "All Existing Formulas";
            this.elementContainerControl1.SelectedIndexChanged += new DataExport.Controls.ElementContainerControl.SelectedIndexChangedHandler(this.elementContainerControl1_SelectedIndexChanged);
            this.elementContainerControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormExistingsFormulas_KeyDown);
            // 
            // ui_buttonX_save
            // 
            this.ui_buttonX_save.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_buttonX_save.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.ui_buttonX_save.Location = new System.Drawing.Point(169, 215);
            this.ui_buttonX_save.Name = "ui_buttonX_save";
            this.ui_buttonX_save.Size = new System.Drawing.Size(134, 40);
            this.ui_buttonX_save.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_save.Symbol = "";
            this.ui_buttonX_save.TabIndex = 62;
            this.ui_buttonX_save.TabStop = false;
            this.ui_buttonX_save.Text = "Add selected";
            this.ui_buttonX_save.Click += new System.EventHandler(this.ui_buttonX_save_Click);
            this.ui_buttonX_save.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormExistingsFormulas_KeyDown);
            // 
            // panelExBack
            // 
            this.panelExBack.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelExBack.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelExBack.Controls.Add(this.labelX2);
            this.panelExBack.Controls.Add(this.textBoxX_formulaName);
            this.panelExBack.Controls.Add(this.elementContainerControl1);
            this.panelExBack.Controls.Add(this.ui_buttonX_save);
            this.panelExBack.Controls.Add(this.ui_buttonX_Cancel);
            this.panelExBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelExBack.Location = new System.Drawing.Point(0, 0);
            this.panelExBack.Name = "panelExBack";
            this.panelExBack.Size = new System.Drawing.Size(431, 266);
            this.panelExBack.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelExBack.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelExBack.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelExBack.Style.BorderColor.Color = System.Drawing.Color.CadetBlue;
            this.panelExBack.Style.BorderWidth = 2;
            this.panelExBack.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelExBack.Style.GradientAngle = 90;
            this.panelExBack.TabIndex = 63;
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX2.ForeColor = System.Drawing.Color.DimGray;
            this.labelX2.Location = new System.Drawing.Point(12, 11);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(160, 23);
            this.labelX2.TabIndex = 64;
            this.labelX2.Text = "selected formula:";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // textBoxX_formulaName
            // 
            this.textBoxX_formulaName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxX_formulaName.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textBoxX_formulaName.Border.Class = "TextBoxBorder";
            this.textBoxX_formulaName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX_formulaName.ButtonCustom.Shortcut = DevComponents.DotNetBar.eShortcut.CtrlS;
            this.textBoxX_formulaName.ButtonCustom.Text = "Save";
            this.textBoxX_formulaName.ForeColor = System.Drawing.Color.Black;
            this.textBoxX_formulaName.Location = new System.Drawing.Point(178, 12);
            this.textBoxX_formulaName.Name = "textBoxX_formulaName";
            this.textBoxX_formulaName.ReadOnly = true;
            this.textBoxX_formulaName.Size = new System.Drawing.Size(241, 22);
            this.textBoxX_formulaName.TabIndex = 63;
            // 
            // FormExistingsFormulas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(431, 266);
            this.Controls.Add(this.panelExBack);
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormExistingsFormulas";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Add Contract";
            this.Shown += new System.EventHandler(this.FormSymbolAdd_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormExistingsFormulas_KeyDown);
            this.panelExBack.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX ui_buttonX_Cancel;
        private Controls.ElementContainerControl elementContainerControl1;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_save;
        private DevComponents.DotNetBar.PanelEx panelExBack;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX_formulaName;
    }
}