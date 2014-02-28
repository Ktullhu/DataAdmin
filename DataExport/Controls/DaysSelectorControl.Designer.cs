namespace DataExport.Controls
{
    partial class DaysSelectorControl
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelTop = new System.Windows.Forms.Label();
            this.labelMiddle = new DevComponents.DotNetBar.LabelX();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.checkBoxX7 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.checkBoxX6 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.checkBoxX5 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.checkBoxX4 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.checkBoxX3 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.checkBoxX2 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.checkBoxX1 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.panelTop.SuspendLayout();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.CadetBlue;
            this.panelTop.Controls.Add(this.labelTop);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(2, 2);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(180, 23);
            this.panelTop.TabIndex = 0;
            // 
            // labelTop
            // 
            this.labelTop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTop.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTop.ForeColor = System.Drawing.SystemColors.Control;
            this.labelTop.Location = new System.Drawing.Point(3, 2);
            this.labelTop.Name = "labelTop";
            this.labelTop.Size = new System.Drawing.Size(174, 23);
            this.labelTop.TabIndex = 0;
            this.labelTop.Text = "select days";
            this.labelTop.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // labelMiddle
            // 
            this.labelMiddle.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelMiddle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelMiddle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelMiddle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelMiddle.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelMiddle.Location = new System.Drawing.Point(2, 25);
            this.labelMiddle.Name = "labelMiddle";
            this.labelMiddle.PaddingLeft = 5;
            this.labelMiddle.Size = new System.Drawing.Size(180, 23);
            this.labelMiddle.TabIndex = 1;
            this.labelMiddle.Text = "Days of week";
            // 
            // panelEx1
            // 
            this.panelEx1.AutoScroll = true;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.checkBoxX7);
            this.panelEx1.Controls.Add(this.checkBoxX6);
            this.panelEx1.Controls.Add(this.checkBoxX5);
            this.panelEx1.Controls.Add(this.checkBoxX4);
            this.panelEx1.Controls.Add(this.checkBoxX3);
            this.panelEx1.Controls.Add(this.checkBoxX2);
            this.panelEx1.Controls.Add(this.checkBoxX1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(2, 48);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Padding = new System.Windows.Forms.Padding(10, 1, 5, 5);
            this.panelEx1.Size = new System.Drawing.Size(180, 190);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.BorderSide = DevComponents.DotNetBar.eBorderSide.Top;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 2;
            // 
            // checkBoxX7
            // 
            // 
            // 
            // 
            this.checkBoxX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxX7.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxX7.Location = new System.Drawing.Point(10, 163);
            this.checkBoxX7.Name = "checkBoxX7";
            this.checkBoxX7.Size = new System.Drawing.Size(165, 27);
            this.checkBoxX7.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBoxX7.TabIndex = 6;
            this.checkBoxX7.Text = "Saturday";
            this.checkBoxX7.CheckedChanged += new System.EventHandler(this.checkBoxX1_CheckedChanged);
            // 
            // checkBoxX6
            // 
            // 
            // 
            // 
            this.checkBoxX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxX6.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxX6.Location = new System.Drawing.Point(10, 136);
            this.checkBoxX6.Name = "checkBoxX6";
            this.checkBoxX6.Size = new System.Drawing.Size(165, 27);
            this.checkBoxX6.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBoxX6.TabIndex = 5;
            this.checkBoxX6.Text = "Friday";
            this.checkBoxX6.CheckedChanged += new System.EventHandler(this.checkBoxX1_CheckedChanged);
            // 
            // checkBoxX5
            // 
            // 
            // 
            // 
            this.checkBoxX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxX5.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxX5.Location = new System.Drawing.Point(10, 109);
            this.checkBoxX5.Name = "checkBoxX5";
            this.checkBoxX5.Size = new System.Drawing.Size(165, 27);
            this.checkBoxX5.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBoxX5.TabIndex = 4;
            this.checkBoxX5.Text = "Thursday";
            this.checkBoxX5.CheckedChanged += new System.EventHandler(this.checkBoxX1_CheckedChanged);
            // 
            // checkBoxX4
            // 
            // 
            // 
            // 
            this.checkBoxX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxX4.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxX4.Location = new System.Drawing.Point(10, 82);
            this.checkBoxX4.Name = "checkBoxX4";
            this.checkBoxX4.Size = new System.Drawing.Size(165, 27);
            this.checkBoxX4.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBoxX4.TabIndex = 3;
            this.checkBoxX4.Text = "Wednesday";
            this.checkBoxX4.CheckedChanged += new System.EventHandler(this.checkBoxX1_CheckedChanged);
            // 
            // checkBoxX3
            // 
            // 
            // 
            // 
            this.checkBoxX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxX3.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxX3.Location = new System.Drawing.Point(10, 55);
            this.checkBoxX3.Name = "checkBoxX3";
            this.checkBoxX3.Size = new System.Drawing.Size(165, 27);
            this.checkBoxX3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBoxX3.TabIndex = 2;
            this.checkBoxX3.Text = "Tusday";
            this.checkBoxX3.CheckedChanged += new System.EventHandler(this.checkBoxX1_CheckedChanged);
            // 
            // checkBoxX2
            // 
            // 
            // 
            // 
            this.checkBoxX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxX2.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxX2.Location = new System.Drawing.Point(10, 28);
            this.checkBoxX2.Name = "checkBoxX2";
            this.checkBoxX2.Size = new System.Drawing.Size(165, 27);
            this.checkBoxX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBoxX2.TabIndex = 1;
            this.checkBoxX2.Text = "Monday";
            this.checkBoxX2.CheckedChanged += new System.EventHandler(this.checkBoxX1_CheckedChanged);
            // 
            // checkBoxX1
            // 
            // 
            // 
            // 
            this.checkBoxX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxX1.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxX1.Location = new System.Drawing.Point(10, 1);
            this.checkBoxX1.Name = "checkBoxX1";
            this.checkBoxX1.Size = new System.Drawing.Size(165, 27);
            this.checkBoxX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBoxX1.TabIndex = 0;
            this.checkBoxX1.Text = "Sunday";
            this.checkBoxX1.CheckedChanged += new System.EventHandler(this.checkBoxX1_CheckedChanged);
            // 
            // DaysSelectorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.labelMiddle);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "DaysSelectorControl";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(184, 240);
            this.Load += new System.EventHandler(this.DaysSelectorControl_Load);
            this.panelTop.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelTop;
        private DevComponents.DotNetBar.LabelX labelMiddle;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX1;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX7;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX6;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX5;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX4;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX3;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX2;
    }
}
