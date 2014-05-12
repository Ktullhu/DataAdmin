namespace DataNetClient.Controls
{
    partial class SymbolItem
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
            this.panelEx_main = new DevComponents.DotNetBar.PanelEx();
            this.labelX_symbol = new DevComponents.DotNetBar.LabelX();
            this.labelX_name = new DevComponents.DotNetBar.LabelX();
            this.panelEx_main.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx_main
            // 
            this.panelEx_main.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx_main.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx_main.Controls.Add(this.labelX_name);
            this.panelEx_main.Controls.Add(this.labelX_symbol);
            this.panelEx_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx_main.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panelEx_main.Location = new System.Drawing.Point(0, 0);
            this.panelEx_main.Name = "panelEx_main";
            this.panelEx_main.Size = new System.Drawing.Size(186, 25);
            this.panelEx_main.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx_main.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx_main.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx_main.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx_main.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx_main.Style.GradientAngle = 90;
            this.panelEx_main.TabIndex = 0;
            this.panelEx_main.Click += new System.EventHandler(this.panelEx_main_Click);
            // 
            // labelX_symbol
            // 
            // 
            // 
            // 
            this.labelX_symbol.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX_symbol.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelX_symbol.Location = new System.Drawing.Point(3, 3);
            this.labelX_symbol.Name = "labelX_symbol";
            this.labelX_symbol.Size = new System.Drawing.Size(17, 23);
            this.labelX_symbol.TabIndex = 0;
            this.labelX_symbol.Text = "*";
            this.labelX_symbol.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // labelX_name
            // 
            // 
            // 
            // 
            this.labelX_name.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelX_name.Location = new System.Drawing.Point(39, -1);
            this.labelX_name.Name = "labelX_name";
            this.labelX_name.Size = new System.Drawing.Size(78, 23);
            this.labelX_name.TabIndex = 1;
            this.labelX_name.Text = "dtudfct";
            // 
            // SymbolItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEx_main);
            this.Name = "SymbolItem";
            this.Size = new System.Drawing.Size(186, 25);
            this.panelEx_main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx_main;
        private DevComponents.DotNetBar.LabelX labelX_symbol;
        private DevComponents.DotNetBar.LabelX labelX_name;
    }
}
