namespace DataAdmin.Forms
{
    partial class TimeSliceControl
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
            this.panelEx_back = new DevComponents.DotNetBar.PanelEx();
            this.labelXTitle = new DevComponents.DotNetBar.LabelX();
            this.panelEx_time = new DevComponents.DotNetBar.PanelEx();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelEx_back.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx_back
            // 
            this.panelEx_back.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx_back.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx_back.Controls.Add(this.labelXTitle);
            this.panelEx_back.Controls.Add(this.panelEx_time);
            this.panelEx_back.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx_back.Location = new System.Drawing.Point(0, 0);
            this.panelEx_back.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelEx_back.Name = "panelEx_back";
            this.panelEx_back.Padding = new System.Windows.Forms.Padding(1);
            this.panelEx_back.Size = new System.Drawing.Size(207, 65);
            this.panelEx_back.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx_back.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelEx_back.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx_back.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx_back.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx_back.Style.GradientAngle = 90;
            this.panelEx_back.TabIndex = 0;
            this.panelEx_back.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelEx_back_MouseMove);
            // 
            // labelXTitle
            // 
            this.labelXTitle.AutoSize = true;
            // 
            // 
            // 
            this.labelXTitle.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.labelXTitle.BackgroundStyle.BorderTopColor = System.Drawing.Color.SteelBlue;
            this.labelXTitle.BackgroundStyle.BorderTopWidth = 3;
            this.labelXTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelXTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelXTitle.Location = new System.Drawing.Point(15, 1);
            this.labelXTitle.Name = "labelXTitle";
            this.labelXTitle.Size = new System.Drawing.Size(110, 22);
            this.labelXTitle.TabIndex = 2;
            this.labelXTitle.Text = "Login Time Slice";
            // 
            // panelEx_time
            // 
            this.panelEx_time.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEx_time.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx_time.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx_time.Location = new System.Drawing.Point(6, 29);
            this.panelEx_time.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelEx_time.Name = "panelEx_time";
            this.panelEx_time.Size = new System.Drawing.Size(195, 30);
            this.panelEx_time.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx_time.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx_time.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx_time.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx_time.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx_time.Style.GradientAngle = 90;
            this.panelEx_time.TabIndex = 1;
            this.panelEx_time.Paint += new System.Windows.Forms.PaintEventHandler(this.panelEx_time_Paint);
            this.panelEx_time.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelEx_time_MouseMove);
            // 
            // TimeSliceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelEx_back);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(100, 65);
            this.Name = "TimeSliceControl";
            this.Size = new System.Drawing.Size(207, 65);
            this.panelEx_back.ResumeLayout(false);
            this.panelEx_back.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx_back;
        private DevComponents.DotNetBar.PanelEx panelEx_time;
        private System.Windows.Forms.ToolTip toolTip1;
        private DevComponents.DotNetBar.LabelX labelXTitle;
    }
}
