namespace DataExport.Controls
{
    partial class ElementContainerControl
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
            this.labelXTitle = new DevComponents.DotNetBar.LabelX();
            this.panelExElementsContainer = new DevComponents.DotNetBar.PanelEx();
            this.panelExBack = new DevComponents.DotNetBar.PanelEx();
            this.panelExBack.SuspendLayout();
            this.SuspendLayout();
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
            this.labelXTitle.Location = new System.Drawing.Point(21, 0);
            this.labelXTitle.Name = "labelXTitle";
            this.labelXTitle.Size = new System.Drawing.Size(31, 22);
            this.labelXTitle.TabIndex = 1;
            this.labelXTitle.Text = "Title";
            // 
            // panelExElementsContainer
            // 
            this.panelExElementsContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelExElementsContainer.AutoScroll = true;
            this.panelExElementsContainer.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelExElementsContainer.Location = new System.Drawing.Point(3, 31);
            this.panelExElementsContainer.Name = "panelExElementsContainer";
            this.panelExElementsContainer.Size = new System.Drawing.Size(306, 226);
            this.panelExElementsContainer.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelExElementsContainer.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelExElementsContainer.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelExElementsContainer.Style.BorderColor.Color = System.Drawing.Color.White;
            this.panelExElementsContainer.Style.BorderWidth = 0;
            this.panelExElementsContainer.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelExElementsContainer.Style.GradientAngle = 90;
            this.panelExElementsContainer.TabIndex = 2;
            // 
            // panelExBack
            // 
            this.panelExBack.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelExBack.Controls.Add(this.labelXTitle);
            this.panelExBack.Controls.Add(this.panelExElementsContainer);
            this.panelExBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelExBack.Location = new System.Drawing.Point(0, 0);
            this.panelExBack.Name = "panelExBack";
            this.panelExBack.Size = new System.Drawing.Size(312, 260);
            this.panelExBack.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelExBack.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelExBack.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelExBack.Style.BorderColor.Color = System.Drawing.Color.Silver;
            this.panelExBack.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelExBack.Style.GradientAngle = 90;
            this.panelExBack.TabIndex = 3;
            // 
            // ElementContainerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelExBack);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(170, 2);
            this.Name = "ElementContainerControl";
            this.Size = new System.Drawing.Size(312, 260);
            this.panelExBack.ResumeLayout(false);
            this.panelExBack.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelXTitle;
        private DevComponents.DotNetBar.PanelEx panelExElementsContainer;
        private DevComponents.DotNetBar.PanelEx panelExBack;




    }
}