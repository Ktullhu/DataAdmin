namespace RozkladCommon.Controls
{
    partial class StyledLessonsListControl
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
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.labelX_day = new DevComponents.DotNetBar.LabelX();
            this.panelEx3 = new DevComponents.DotNetBar.PanelEx();
            this.panelEx_container = new DevComponents.DotNetBar.PanelEx();
            this.oneLessonControl4 = new StyledOneLessonControl();
            this.oneLessonControl3 = new StyledOneLessonControl();
            this.oneLessonControl2 = new StyledOneLessonControl();
            this.oneLessonControl1 = new StyledOneLessonControl();
            this.panelEx1.SuspendLayout();
            this.panelEx_container.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.labelX_day);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelEx1.Location = new System.Drawing.Point(0, 5);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(25, 83);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 2;
            // 
            // labelX_day
            // 
            // 
            // 
            // 
            this.labelX_day.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX_day.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelX_day.Location = new System.Drawing.Point(0, 0);
            this.labelX_day.Name = "labelX_day";
            this.labelX_day.Size = new System.Drawing.Size(25, 83);
            this.labelX_day.TabIndex = 0;
            this.labelX_day.Text = "Пн";
            this.labelX_day.TextAlignment = System.Drawing.StringAlignment.Center;
            this.labelX_day.TextOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            // 
            // panelEx3
            // 
            this.panelEx3.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelEx3.Location = new System.Drawing.Point(25, 5);
            this.panelEx3.Name = "panelEx3";
            this.panelEx3.Size = new System.Drawing.Size(5, 83);
            this.panelEx3.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx3.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(243)))));
            this.panelEx3.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx3.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx3.Style.GradientAngle = 90;
            this.panelEx3.TabIndex = 4;
            // 
            // panelEx_container
            // 
            this.panelEx_container.AutoSize = true;
            this.panelEx_container.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx_container.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx_container.Controls.Add(this.oneLessonControl4);
            this.panelEx_container.Controls.Add(this.oneLessonControl3);
            this.panelEx_container.Controls.Add(this.oneLessonControl2);
            this.panelEx_container.Controls.Add(this.oneLessonControl1);
            this.panelEx_container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx_container.Location = new System.Drawing.Point(30, 5);
            this.panelEx_container.Name = "panelEx_container";
            this.panelEx_container.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panelEx_container.Size = new System.Drawing.Size(383, 83);
            this.panelEx_container.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx_container.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.panelEx_container.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx_container.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx_container.Style.GradientAngle = 90;
            this.panelEx_container.TabIndex = 5;
            // 
            // oneLessonControl4
            // 
            this.oneLessonControl4.Auditoria = "Ауд";
            this.oneLessonControl4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.oneLessonControl4.Dock = System.Windows.Forms.DockStyle.Top;
            this.oneLessonControl4.Location = new System.Drawing.Point(0, 60);
            this.oneLessonControl4.Name = "oneLessonControl4";
            this.oneLessonControl4.NoTitle = "4";
            this.oneLessonControl4.Predmet = "Назва предмета";
            this.oneLessonControl4.Prepod = "Препод";
            this.oneLessonControl4.Size = new System.Drawing.Size(383, 20);
            this.oneLessonControl4.TabIndex = 3;
            // 
            // oneLessonControl3
            // 
            this.oneLessonControl3.Auditoria = "Ауд";
            this.oneLessonControl3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.oneLessonControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.oneLessonControl3.Location = new System.Drawing.Point(0, 40);
            this.oneLessonControl3.Name = "oneLessonControl3";
            this.oneLessonControl3.NoTitle = "3";
            this.oneLessonControl3.Predmet = "Назва предмета";
            this.oneLessonControl3.Prepod = "Препод";
            this.oneLessonControl3.Size = new System.Drawing.Size(383, 20);
            this.oneLessonControl3.TabIndex = 2;
            // 
            // oneLessonControl2
            // 
            this.oneLessonControl2.Auditoria = "Ауд";
            this.oneLessonControl2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.oneLessonControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.oneLessonControl2.Location = new System.Drawing.Point(0, 20);
            this.oneLessonControl2.Name = "oneLessonControl2";
            this.oneLessonControl2.NoTitle = "2";
            this.oneLessonControl2.Predmet = "Назва предмета";
            this.oneLessonControl2.Prepod = "Препод";
            this.oneLessonControl2.Size = new System.Drawing.Size(383, 20);
            this.oneLessonControl2.TabIndex = 1;
            // 
            // oneLessonControl1
            // 
            this.oneLessonControl1.Auditoria = "Ауд";
            this.oneLessonControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.oneLessonControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.oneLessonControl1.Location = new System.Drawing.Point(0, 0);
            this.oneLessonControl1.Name = "oneLessonControl1";
            this.oneLessonControl1.NoTitle = "1";
            this.oneLessonControl1.Predmet = "Назва предмета";
            this.oneLessonControl1.Prepod = "Препод";
            this.oneLessonControl1.Size = new System.Drawing.Size(383, 20);
            this.oneLessonControl1.TabIndex = 0;            
            // 
            // LessonsListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.Controls.Add(this.panelEx_container);
            this.Controls.Add(this.panelEx3);
            this.Controls.Add(this.panelEx1);
            this.Name = "LessonsListControl";
            this.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.Size = new System.Drawing.Size(413, 93);
            this.panelEx1.ResumeLayout(false);
            this.panelEx_container.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.LabelX labelX_day;
        private DevComponents.DotNetBar.PanelEx panelEx3;
        private DevComponents.DotNetBar.PanelEx panelEx_container;
        private StyledOneLessonControl oneLessonControl3;
        private StyledOneLessonControl oneLessonControl2;
        private StyledOneLessonControl oneLessonControl1;
        private StyledOneLessonControl oneLessonControl4;


    }
}
