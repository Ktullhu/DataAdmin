namespace DataExport.Controls
{
    partial class FormulaControl
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
            this.ui_labelX_arrow = new DevComponents.DotNetBar.LabelX();
            this.ui_panelEx_formula = new DevComponents.DotNetBar.PanelEx();
            this.contextMenuStrip_mainPanel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.panelExMain = new DevComponents.DotNetBar.PanelEx();
            this.ui_buttonX_save = new DevComponents.DotNetBar.ButtonX();
            this.labelXTitle = new DevComponents.DotNetBar.LabelX();
            this.contextMenuStrip_element = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteElementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip_equal = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.changeToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.changeToToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.changeToToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip_mainPanel.SuspendLayout();
            this.panelExMain.SuspendLayout();
            this.contextMenuStrip_element.SuspendLayout();
            this.contextMenuStrip_equal.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_labelX_arrow
            // 
            // 
            // 
            // 
            this.ui_labelX_arrow.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_labelX_arrow.Font = new System.Drawing.Font("Segoe UI", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ui_labelX_arrow.Location = new System.Drawing.Point(11, 56);
            this.ui_labelX_arrow.Name = "ui_labelX_arrow";
            this.ui_labelX_arrow.Size = new System.Drawing.Size(23, 25);
            this.ui_labelX_arrow.Symbol = "";
            this.ui_labelX_arrow.SymbolColor = System.Drawing.Color.CadetBlue;
            this.ui_labelX_arrow.TabIndex = 58;
            this.ui_labelX_arrow.Visible = false;
            // 
            // ui_panelEx_formula
            // 
            this.ui_panelEx_formula.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_panelEx_formula.CanvasColor = System.Drawing.SystemColors.Control;
            this.ui_panelEx_formula.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_panelEx_formula.ContextMenuStrip = this.contextMenuStrip_mainPanel;
            this.ui_panelEx_formula.Location = new System.Drawing.Point(16, 28);
            this.ui_panelEx_formula.Name = "ui_panelEx_formula";
            this.ui_panelEx_formula.Size = new System.Drawing.Size(266, 25);
            this.ui_panelEx_formula.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.ui_panelEx_formula.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.ui_panelEx_formula.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.ui_panelEx_formula.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.ui_panelEx_formula.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.ui_panelEx_formula.Style.GradientAngle = 90;
            this.ui_panelEx_formula.TabIndex = 57;
            this.ui_panelEx_formula.Click += new System.EventHandler(this.ui_panelEx_formula_Click);
            // 
            // contextMenuStrip_mainPanel
            // 
            this.contextMenuStrip_mainPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemClearAll});
            this.contextMenuStrip_mainPanel.Name = "contextMenuStrip_operation";
            this.contextMenuStrip_mainPanel.Size = new System.Drawing.Size(117, 26);
            // 
            // toolStripMenuItemClearAll
            // 
            this.toolStripMenuItemClearAll.Name = "toolStripMenuItemClearAll";
            this.toolStripMenuItemClearAll.Size = new System.Drawing.Size(116, 22);
            this.toolStripMenuItemClearAll.Text = "Clear all";
            this.toolStripMenuItemClearAll.Click += new System.EventHandler(this.toolStripMenuItemClearAll_Click);
            // 
            // panelExMain
            // 
            this.panelExMain.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelExMain.Controls.Add(this.ui_buttonX_save);
            this.panelExMain.Controls.Add(this.ui_panelEx_formula);
            this.panelExMain.Controls.Add(this.ui_labelX_arrow);
            this.panelExMain.Controls.Add(this.labelXTitle);
            this.panelExMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelExMain.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelExMain.Location = new System.Drawing.Point(0, 0);
            this.panelExMain.Name = "panelExMain";
            this.panelExMain.Size = new System.Drawing.Size(331, 83);
            this.panelExMain.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelExMain.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelExMain.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelExMain.Style.BorderColor.Color = System.Drawing.Color.Silver;
            this.panelExMain.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelExMain.Style.GradientAngle = 90;
            this.panelExMain.TabIndex = 59;
            // 
            // ui_buttonX_save
            // 
            this.ui_buttonX_save.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_buttonX_save.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.ui_buttonX_save.Location = new System.Drawing.Point(288, 27);
            this.ui_buttonX_save.Name = "ui_buttonX_save";
            this.ui_buttonX_save.Size = new System.Drawing.Size(32, 27);
            this.ui_buttonX_save.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_save.Symbol = "";
            this.ui_buttonX_save.SymbolSize = 14F;
            this.ui_buttonX_save.TabIndex = 60;
            this.ui_buttonX_save.Tooltip = "Clear all";
            this.ui_buttonX_save.Click += new System.EventHandler(this.toolStripMenuItemClearAll_Click);
            // 
            // labelXTitle
            // 
            this.labelXTitle.AutoSize = true;
            // 
            // 
            // 
            this.labelXTitle.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.labelXTitle.BackgroundStyle.BorderTopColor = System.Drawing.Color.CadetBlue;
            this.labelXTitle.BackgroundStyle.BorderTopWidth = 3;
            this.labelXTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelXTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelXTitle.Location = new System.Drawing.Point(21, 0);
            this.labelXTitle.Name = "labelXTitle";
            this.labelXTitle.Size = new System.Drawing.Size(110, 22);
            this.labelXTitle.TabIndex = 1;
            this.labelXTitle.Text = "Current Formula";
            // 
            // contextMenuStrip_element
            // 
            this.contextMenuStrip_element.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteElementToolStripMenuItem});
            this.contextMenuStrip_element.Name = "contextMenuStrip_operation";
            this.contextMenuStrip_element.Size = new System.Drawing.Size(154, 26);
            // 
            // deleteElementToolStripMenuItem
            // 
            this.deleteElementToolStripMenuItem.Name = "deleteElementToolStripMenuItem";
            this.deleteElementToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.deleteElementToolStripMenuItem.Text = "Delete element";
            this.deleteElementToolStripMenuItem.Click += new System.EventHandler(this.deleteElementToolStripMenuItem_Click);
            // 
            // contextMenuStrip_equal
            // 
            this.contextMenuStrip_equal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.changeToToolStripMenuItem,
            this.toolStripMenuItem6,
            this.changeToToolStripMenuItem1,
            this.changeToToolStripMenuItem2,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5});
            this.contextMenuStrip_equal.Name = "contextMenuStrip_operation";
            this.contextMenuStrip_equal.Size = new System.Drawing.Size(154, 164);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(153, 22);
            this.toolStripMenuItem1.Text = "Delete element";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.deleteElementToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(150, 6);
            // 
            // changeToToolStripMenuItem
            // 
            this.changeToToolStripMenuItem.Name = "changeToToolStripMenuItem";
            this.changeToToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.changeToToolStripMenuItem.Text = "=";
            this.changeToToolStripMenuItem.Click += new System.EventHandler(this.changeToToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(153, 22);
            this.toolStripMenuItem6.Text = "<>";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.changeToToolStripMenuItem_Click);
            // 
            // changeToToolStripMenuItem1
            // 
            this.changeToToolStripMenuItem1.Name = "changeToToolStripMenuItem1";
            this.changeToToolStripMenuItem1.Size = new System.Drawing.Size(153, 22);
            this.changeToToolStripMenuItem1.Text = ">";
            this.changeToToolStripMenuItem1.Click += new System.EventHandler(this.changeToToolStripMenuItem_Click);
            // 
            // changeToToolStripMenuItem2
            // 
            this.changeToToolStripMenuItem2.Name = "changeToToolStripMenuItem2";
            this.changeToToolStripMenuItem2.Size = new System.Drawing.Size(153, 22);
            this.changeToToolStripMenuItem2.Text = "<";
            this.changeToToolStripMenuItem2.Click += new System.EventHandler(this.changeToToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(153, 22);
            this.toolStripMenuItem4.Text = ">=";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.changeToToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(153, 22);
            this.toolStripMenuItem5.Text = "<=";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.changeToToolStripMenuItem_Click);
            // 
            // FormulaControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelExMain);
            this.Name = "FormulaControl";
            this.Size = new System.Drawing.Size(331, 83);
            this.contextMenuStrip_mainPanel.ResumeLayout(false);
            this.panelExMain.ResumeLayout(false);
            this.panelExMain.PerformLayout();
            this.contextMenuStrip_element.ResumeLayout(false);
            this.contextMenuStrip_equal.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX ui_labelX_arrow;
        private DevComponents.DotNetBar.PanelEx ui_panelEx_formula;
        private DevComponents.DotNetBar.PanelEx panelExMain;
        private DevComponents.DotNetBar.LabelX labelXTitle;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_element;
        private System.Windows.Forms.ToolStripMenuItem deleteElementToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_equal;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem changeToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem changeToToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem changeToToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_mainPanel;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemClearAll;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_save;
    }
}
