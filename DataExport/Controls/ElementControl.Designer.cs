namespace DataExport.Controls
{
    partial class ElementControl
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
            this.panelRightBorder = new System.Windows.Forms.Panel();
            this.labelXRight = new DevComponents.DotNetBar.LabelX();
            this.panelLeftBorder = new System.Windows.Forms.Panel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.labelElementText = new System.Windows.Forms.Label();
            this.panelRightBorder.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRightBorder
            // 
            this.panelRightBorder.BackColor = System.Drawing.Color.SteelBlue;
            this.panelRightBorder.Controls.Add(this.labelXRight);
            this.panelRightBorder.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRightBorder.Location = new System.Drawing.Point(150, 0);
            this.panelRightBorder.Name = "panelRightBorder";
            this.panelRightBorder.Size = new System.Drawing.Size(45, 38);
            this.panelRightBorder.TabIndex = 3;
            this.panelRightBorder.MouseLeave += new System.EventHandler(this.ElementControl_MouseLeave);
            this.panelRightBorder.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ElementControl_MouseMove);
            // 
            // labelXRight
            // 
            this.labelXRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.labelXRight.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelXRight.Location = new System.Drawing.Point(6, 0);
            this.labelXRight.Name = "labelXRight";
            this.labelXRight.PaddingLeft = 10;
            this.labelXRight.Size = new System.Drawing.Size(39, 38);
            this.labelXRight.Symbol = "";
            this.labelXRight.SymbolColor = System.Drawing.Color.White;
            this.labelXRight.TabIndex = 0;
            this.labelXRight.Click += new System.EventHandler(this.buttonClick);
            this.labelXRight.MouseLeave += new System.EventHandler(this.ElementControl_MouseLeave);
            this.labelXRight.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ElementControl_MouseMove);
            // 
            // panelLeftBorder
            // 
            this.panelLeftBorder.BackColor = System.Drawing.Color.Gray;
            this.panelLeftBorder.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeftBorder.Location = new System.Drawing.Point(0, 0);
            this.panelLeftBorder.Name = "panelLeftBorder";
            this.panelLeftBorder.Size = new System.Drawing.Size(5, 38);
            this.panelLeftBorder.TabIndex = 5;
            this.panelLeftBorder.MouseLeave += new System.EventHandler(this.ElementControl_MouseLeave);
            this.panelLeftBorder.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ElementControl_MouseMove);
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.Gray;
            this.panelMain.Controls.Add(this.labelElementText);
            this.panelMain.Controls.Add(this.panelRightBorder);
            this.panelMain.Controls.Add(this.panelLeftBorder);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 1);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(195, 38);
            this.panelMain.TabIndex = 7;
            // 
            // labelElementText
            // 
            this.labelElementText.BackColor = System.Drawing.Color.Gray;
            this.labelElementText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelElementText.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelElementText.ForeColor = System.Drawing.SystemColors.Control;
            this.labelElementText.Location = new System.Drawing.Point(5, 0);
            this.labelElementText.Name = "labelElementText";
            this.labelElementText.Padding = new System.Windows.Forms.Padding(5);
            this.labelElementText.Size = new System.Drawing.Size(145, 38);
            this.labelElementText.TabIndex = 7;
            this.labelElementText.Text = "element name";
            this.labelElementText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelElementText.UseCompatibleTextRendering = true;
            this.labelElementText.Click += new System.EventHandler(this.buttonClick);
            this.labelElementText.MouseLeave += new System.EventHandler(this.ElementControl_MouseLeave);
            this.labelElementText.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ElementControl_MouseMove);
            // 
            // ElementControl
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelMain);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(170, 30);
            this.Name = "ElementControl";
            this.Padding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.Size = new System.Drawing.Size(195, 40);
            this.MouseLeave += new System.EventHandler(this.ElementControl_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ElementControl_MouseMove);
            this.panelRightBorder.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelRightBorder;
        private DevComponents.DotNetBar.LabelX labelXRight;
        private System.Windows.Forms.Panel panelLeftBorder;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label labelElementText;
    }
}