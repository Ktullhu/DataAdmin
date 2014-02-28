namespace RozkladCommon.Controls
{
    partial class StyledOneLessonControl
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
            this.labelX_aud = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX_prepod = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX_no = new DevComponents.DotNetBar.LabelX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX_predmet = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // labelX_aud
            // 
            // 
            // 
            // 
            this.labelX_aud.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX_aud.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelX_aud.Location = new System.Drawing.Point(304, 0);
            this.labelX_aud.Name = "labelX_aud";
            this.labelX_aud.PaddingLeft = 5;
            this.labelX_aud.Size = new System.Drawing.Size(60, 20);
            this.labelX_aud.TabIndex = 0;
            this.labelX_aud.Text = "Ауд";
            this.labelX_aud.Click += new System.EventHandler(this.labelX_predmet_Click);
            this.labelX_aud.DoubleClick += new System.EventHandler(this.labelX_predmet_DoubleClick);
            this.labelX_aud.MouseLeave += new System.EventHandler(this.labelX_no_MouseLeave);
            this.labelX_aud.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelX_no_MouseMove);
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelX2.Location = new System.Drawing.Point(297, 0);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(7, 20);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "|";
            // 
            // labelX_prepod
            // 
            // 
            // 
            // 
            this.labelX_prepod.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX_prepod.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelX_prepod.Location = new System.Drawing.Point(147, 0);
            this.labelX_prepod.Name = "labelX_prepod";
            this.labelX_prepod.PaddingLeft = 5;
            this.labelX_prepod.Size = new System.Drawing.Size(150, 20);
            this.labelX_prepod.TabIndex = 2;
            this.labelX_prepod.Text = "Викладач";
            this.labelX_prepod.Click += new System.EventHandler(this.labelX_predmet_Click);
            this.labelX_prepod.DoubleClick += new System.EventHandler(this.labelX_predmet_DoubleClick);
            this.labelX_prepod.MouseLeave += new System.EventHandler(this.labelX_no_MouseLeave);
            this.labelX_prepod.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelX_no_MouseMove);
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelX4.Location = new System.Drawing.Point(140, 0);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(7, 20);
            this.labelX4.TabIndex = 3;
            this.labelX4.Text = "|";
            // 
            // labelX_no
            // 
            // 
            // 
            // 
            this.labelX_no.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX_no.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelX_no.Location = new System.Drawing.Point(0, 0);
            this.labelX_no.Name = "labelX_no";
            this.labelX_no.PaddingLeft = 10;
            this.labelX_no.Size = new System.Drawing.Size(30, 20);
            this.labelX_no.TabIndex = 6;
            this.labelX_no.Text = "1.";
            // 
            // labelX6
            // 
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelX6.Location = new System.Drawing.Point(30, 0);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(7, 20);
            this.labelX6.TabIndex = 7;
            this.labelX6.Text = "|";
            // 
            // labelX_predmet
            // 
            // 
            // 
            // 
            this.labelX_predmet.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX_predmet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelX_predmet.Location = new System.Drawing.Point(37, 0);
            this.labelX_predmet.Name = "labelX_predmet";
            this.labelX_predmet.PaddingLeft = 5;
            this.labelX_predmet.Size = new System.Drawing.Size(103, 20);
            this.labelX_predmet.TabIndex = 9;
            this.labelX_predmet.Text = "Назва предмета";
            this.labelX_predmet.Click += new System.EventHandler(this.labelX_predmet_Click);
            this.labelX_predmet.DoubleClick += new System.EventHandler(this.labelX_predmet_DoubleClick);
            this.labelX_predmet.MouseLeave += new System.EventHandler(this.labelX_no_MouseLeave);
            this.labelX_predmet.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelX_no_MouseMove);
            // 
            // OneLessonControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.Controls.Add(this.labelX_predmet);
            this.Controls.Add(this.labelX6);
            this.Controls.Add(this.labelX_no);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX_prepod);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX_aud);
            this.Name = "OneLessonControl";
            this.Size = new System.Drawing.Size(364, 20);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX_aud;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX_prepod;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX_no;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX_predmet;
    }
}
