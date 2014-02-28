namespace RozkladCommon.Controls
{
    partial class StyledLoginPanelControl
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
            this.panelEx_back = new DevComponents.DotNetBar.PanelEx();
            this.panelEx_loginButton = new DevComponents.DotNetBar.PanelEx();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.panelEx7 = new DevComponents.DotNetBar.PanelEx();
            this.labelX_loginTwitter = new DevComponents.DotNetBar.LabelX();
            this.labelX_loginFacebook = new DevComponents.DotNetBar.LabelX();
            this.panelEx6 = new DevComponents.DotNetBar.PanelEx();
            this.label1 = new System.Windows.Forms.Label();
            this.styledLoadAnimation1 = new RozkladCommon.Controls.StyledLoadAnimation();
            this.styledPasswordTextBox1 = new RozkladCommon.Controls.StyledPasswordTextBox();
            this.styledLoginTextBox1 = new RozkladCommon.Controls.StyledLoginTextBox();
            this.panelEx_back.SuspendLayout();
            this.panelEx_loginButton.SuspendLayout();
            this.panelEx1.SuspendLayout();
            this.panelEx7.SuspendLayout();
            this.panelEx6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx_back
            // 
            this.panelEx_back.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx_back.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx_back.Controls.Add(this.styledLoadAnimation1);
            this.panelEx_back.Controls.Add(this.panelEx_loginButton);
            this.panelEx_back.Controls.Add(this.styledPasswordTextBox1);
            this.panelEx_back.Controls.Add(this.styledLoginTextBox1);
            this.panelEx_back.Controls.Add(this.panelEx7);
            this.panelEx_back.Controls.Add(this.panelEx6);
            this.panelEx_back.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx_back.Location = new System.Drawing.Point(0, 0);
            this.panelEx_back.Name = "panelEx_back";
            this.panelEx_back.Size = new System.Drawing.Size(372, 271);
            this.panelEx_back.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx_back.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.panelEx_back.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx_back.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx_back.Style.GradientAngle = 90;
            this.panelEx_back.TabIndex = 4;
            // 
            // panelEx_loginButton
            // 
            this.panelEx_loginButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEx_loginButton.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx_loginButton.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx_loginButton.Controls.Add(this.panelEx1);
            this.panelEx_loginButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelEx_loginButton.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panelEx_loginButton.Location = new System.Drawing.Point(200, 180);
            this.panelEx_loginButton.Name = "panelEx_loginButton";
            this.panelEx_loginButton.Size = new System.Drawing.Size(140, 29);
            this.panelEx_loginButton.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx_loginButton.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(243)))));
            this.panelEx_loginButton.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx_loginButton.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx_loginButton.Style.GradientAngle = 90;
            this.panelEx_loginButton.StyleMouseDown.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx_loginButton.StyleMouseDown.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.panelEx_loginButton.StyleMouseDown.ForeColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(243)))));
            this.panelEx_loginButton.StyleMouseOver.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx_loginButton.StyleMouseOver.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(178)))), ((int)(((byte)(27)))));
            this.panelEx_loginButton.TabIndex = 2;
            this.panelEx_loginButton.TabStop = true;
            this.panelEx_loginButton.Text = "Ввійти";
            this.panelEx_loginButton.Click += new System.EventHandler(this.panelEx_loginButton_Click);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(36, 29);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 5;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.labelX1.Location = new System.Drawing.Point(8, 0);
            this.labelX1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(31, 29);
            this.labelX1.Symbol = "";
            this.labelX1.SymbolColor = System.Drawing.Color.White;
            this.labelX1.SymbolSize = 12F;
            this.labelX1.TabIndex = 0;
            // 
            // panelEx7
            // 
            this.panelEx7.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx7.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx7.Controls.Add(this.labelX_loginTwitter);
            this.panelEx7.Controls.Add(this.labelX_loginFacebook);
            this.panelEx7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelEx7.Location = new System.Drawing.Point(0, 235);
            this.panelEx7.Name = "panelEx7";
            this.panelEx7.Size = new System.Drawing.Size(372, 36);
            this.panelEx7.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx7.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.panelEx7.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx7.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx7.Style.GradientAngle = 90;
            this.panelEx7.TabIndex = 3;
            // 
            // labelX_loginTwitter
            // 
            this.labelX_loginTwitter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.labelX_loginTwitter.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX_loginTwitter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelX_loginTwitter.Location = new System.Drawing.Point(187, 5);
            this.labelX_loginTwitter.Name = "labelX_loginTwitter";
            this.labelX_loginTwitter.Size = new System.Drawing.Size(153, 23);
            this.labelX_loginTwitter.Symbol = "";
            this.labelX_loginTwitter.SymbolColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(243)))));
            this.labelX_loginTwitter.SymbolSize = 14F;
            this.labelX_loginTwitter.TabIndex = 1;
            this.labelX_loginTwitter.Text = "  Ввійти через Вконтакте";
            this.labelX_loginTwitter.Click += new System.EventHandler(this.labelX_loginVk_Click);
            this.labelX_loginTwitter.MouseLeave += new System.EventHandler(this.labelX_loginTwitter_MouseLeave);
            this.labelX_loginTwitter.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelX_loginTwitter_MouseMove);
            // 
            // labelX_loginFacebook
            // 
            // 
            // 
            // 
            this.labelX_loginFacebook.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX_loginFacebook.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelX_loginFacebook.Location = new System.Drawing.Point(30, 5);
            this.labelX_loginFacebook.Name = "labelX_loginFacebook";
            this.labelX_loginFacebook.Size = new System.Drawing.Size(146, 23);
            this.labelX_loginFacebook.Symbol = "";
            this.labelX_loginFacebook.SymbolColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(243)))));
            this.labelX_loginFacebook.SymbolSize = 14F;
            this.labelX_loginFacebook.TabIndex = 0;
            this.labelX_loginFacebook.Text = "  Ввійти через Facebook";
            this.labelX_loginFacebook.Click += new System.EventHandler(this.labelX_loginFacebook_Click);
            this.labelX_loginFacebook.MouseLeave += new System.EventHandler(this.labelX_loginFacebook_MouseLeave);
            this.labelX_loginFacebook.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelX_loginFacebook_MouseMove);
            // 
            // panelEx6
            // 
            this.panelEx6.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx6.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx6.Controls.Add(this.label1);
            this.panelEx6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx6.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panelEx6.Location = new System.Drawing.Point(0, 0);
            this.panelEx6.Name = "panelEx6";
            this.panelEx6.Size = new System.Drawing.Size(372, 36);
            this.panelEx6.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx6.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(243)))));
            this.panelEx6.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx6.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx6.Style.GradientAngle = 90;
            this.panelEx6.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(372, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "АВТОРИЗАЦІЯ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // styledLoadAnimation1
            // 
            this.styledLoadAnimation1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.styledLoadAnimation1.BackColor1 = System.Drawing.Color.Transparent;
            this.styledLoadAnimation1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(178)))), ((int)(((byte)(27)))));
            this.styledLoadAnimation1.Location = new System.Drawing.Point(0, 36);
            this.styledLoadAnimation1.Name = "styledLoadAnimation1";
            this.styledLoadAnimation1.Size = new System.Drawing.Size(372, 10);
            this.styledLoadAnimation1.TabIndex = 4;
            // 
            // styledPasswordTextBox1
            // 
            this.styledPasswordTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.styledPasswordTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.styledPasswordTextBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.styledPasswordTextBox1.InputText = "";
            this.styledPasswordTextBox1.Location = new System.Drawing.Point(30, 130);
            this.styledPasswordTextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.styledPasswordTextBox1.Name = "styledPasswordTextBox1";
            this.styledPasswordTextBox1.Size = new System.Drawing.Size(310, 36);
            this.styledPasswordTextBox1.TabIndex = 1;
            this.styledPasswordTextBox1.WatermarkText = "Пароль";
            this.styledPasswordTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.styledLoginTextBox1_KeyDown);
            // 
            // styledLoginTextBox1
            // 
            this.styledLoginTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.styledLoginTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.styledLoginTextBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.styledLoginTextBox1.InputText = "";
            this.styledLoginTextBox1.Location = new System.Drawing.Point(30, 65);
            this.styledLoginTextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.styledLoginTextBox1.Name = "styledLoginTextBox1";
            this.styledLoginTextBox1.Size = new System.Drawing.Size(310, 36);
            this.styledLoginTextBox1.TabIndex = 0;
            this.styledLoginTextBox1.WatermarkText = "Логін";
            this.styledLoginTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.styledLoginTextBox1_KeyDown);
            // 
            // StyledLoginPanelControl
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.Controls.Add(this.panelEx_back);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "StyledLoginPanelControl";
            this.Size = new System.Drawing.Size(372, 271);
            this.panelEx_back.ResumeLayout(false);
            this.panelEx_loginButton.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.panelEx7.ResumeLayout(false);
            this.panelEx6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx_back;
        private DevComponents.DotNetBar.PanelEx panelEx7;
        private DevComponents.DotNetBar.PanelEx panelEx6;
        private System.Windows.Forms.Label label1;
        private StyledLoginTextBox styledLoginTextBox1;
        private StyledPasswordTextBox styledPasswordTextBox1;
        private DevComponents.DotNetBar.PanelEx panelEx_loginButton;
        private DevComponents.DotNetBar.LabelX labelX_loginFacebook;
        private DevComponents.DotNetBar.LabelX labelX_loginTwitter;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private StyledLoadAnimation styledLoadAnimation1;
    }
}
