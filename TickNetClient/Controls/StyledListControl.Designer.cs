namespace TickNetClient.Controls
{
    partial class StyledListControl
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
            this.panelEx_container = new DevComponents.DotNetBar.PanelEx();
            this.SuspendLayout();
            // 
            // panelEx_container
            // 
            this.panelEx_container.AutoScroll = true;
            this.panelEx_container.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx_container.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx_container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx_container.Location = new System.Drawing.Point(0, 0);
            this.panelEx_container.Name = "panelEx_container";
            this.panelEx_container.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.panelEx_container.Size = new System.Drawing.Size(283, 175);
            this.panelEx_container.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx_container.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelEx_container.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx_container.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx_container.Style.GradientAngle = 90;
            this.panelEx_container.TabIndex = 0;
            // 
            // StyledListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.Controls.Add(this.panelEx_container);
            this.Name = "StyledListControl";
            this.Size = new System.Drawing.Size(283, 175);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx_container;
    }
}
