namespace TickNetClient.Forms
{
    partial class AddListControl
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
            this.saveButton = new DevComponents.DotNetBar.ButtonX();
            this.cancelButton = new DevComponents.DotNetBar.ButtonX();
            this.labelXTitle = new DevComponents.DotNetBar.LabelX();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.cmbContinuationType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbHistoricalPeriod = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem_tick = new DevComponents.Editors.ComboItem();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.comboItem4 = new DevComponents.Editors.ComboItem();
            this.comboItem5 = new DevComponents.Editors.ComboItem();
            this.comboItem6 = new DevComponents.Editors.ComboItem();
            this.comboItem7 = new DevComponents.Editors.ComboItem();
            this.comboItem8 = new DevComponents.Editors.ComboItem();
            this.comboItem9 = new DevComponents.Editors.ComboItem();
            this.comboItem10 = new DevComponents.Editors.ComboItem();
            this.comboItem11 = new DevComponents.Editors.ComboItem();
            this.comboItem12 = new DevComponents.Editors.ComboItem();
            this.comboItem13 = new DevComponents.Editors.ComboItem();
            this.comboItem14 = new DevComponents.Editors.ComboItem();
            this.comboItem15 = new DevComponents.Editors.ComboItem();
            this.textBoxXListName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX_back = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.saveButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.saveButton.Location = new System.Drawing.Point(366, 397);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(92, 31);
            this.saveButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.saveButton.TabIndex = 70;
            this.saveButton.Text = "Save";
            // 
            // cancelButton
            // 
            this.cancelButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cancelButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.cancelButton.Location = new System.Drawing.Point(468, 397);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(92, 31);
            this.cancelButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cancelButton.TabIndex = 21;
            this.cancelButton.Text = "Cancel";
            this.toolTip1.SetToolTip(this.cancelButton, "Return without saving");
            // 
            // labelXTitle
            // 
            // 
            // 
            // 
            this.labelXTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelXTitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelXTitle.Location = new System.Drawing.Point(101, 4);
            this.labelXTitle.Name = "labelXTitle";
            this.labelXTitle.Size = new System.Drawing.Size(239, 34);
            this.labelXTitle.TabIndex = 19;
            this.labelXTitle.Text = "ADD SYMBOLS LIST";
            // 
            // cmbContinuationType
            // 
            this.cmbContinuationType.DisplayMember = "Text";
            this.cmbContinuationType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbContinuationType.FormattingEnabled = true;
            this.cmbContinuationType.ItemHeight = 17;
            this.cmbContinuationType.Location = new System.Drawing.Point(366, 268);
            this.cmbContinuationType.Name = "cmbContinuationType";
            this.cmbContinuationType.Size = new System.Drawing.Size(209, 23);
            this.cmbContinuationType.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbContinuationType.TabIndex = 82;
            this.cmbContinuationType.Visible = false;
            // 
            // cmbHistoricalPeriod
            // 
            this.cmbHistoricalPeriod.DisplayMember = "Text";
            this.cmbHistoricalPeriod.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbHistoricalPeriod.FormattingEnabled = true;
            this.cmbHistoricalPeriod.ItemHeight = 17;
            this.cmbHistoricalPeriod.Items.AddRange(new object[] {
            this.comboItem_tick,
            this.comboItem1,
            this.comboItem2,
            this.comboItem3,
            this.comboItem4,
            this.comboItem5,
            this.comboItem6,
            this.comboItem7,
            this.comboItem8,
            this.comboItem9,
            this.comboItem10,
            this.comboItem11,
            this.comboItem12,
            this.comboItem13,
            this.comboItem14,
            this.comboItem15});
            this.cmbHistoricalPeriod.Location = new System.Drawing.Point(366, 239);
            this.cmbHistoricalPeriod.Name = "cmbHistoricalPeriod";
            this.cmbHistoricalPeriod.Size = new System.Drawing.Size(209, 23);
            this.cmbHistoricalPeriod.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbHistoricalPeriod.TabIndex = 81;
            this.cmbHistoricalPeriod.Visible = false;
            // 
            // comboItem_tick
            // 
            this.comboItem_tick.Text = "tick";
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "1 minute";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "2 minutes";
            // 
            // comboItem3
            // 
            this.comboItem3.Text = "3 minutes";
            // 
            // comboItem4
            // 
            this.comboItem4.Text = "5 minutes";
            // 
            // comboItem5
            // 
            this.comboItem5.Text = "10 minutes";
            // 
            // comboItem6
            // 
            this.comboItem6.Text = "15 minutes";
            // 
            // comboItem7
            // 
            this.comboItem7.Text = "30 minutes";
            // 
            // comboItem8
            // 
            this.comboItem8.Text = "60 minutes";
            // 
            // comboItem9
            // 
            this.comboItem9.Text = "240 minutes";
            // 
            // comboItem10
            // 
            this.comboItem10.Text = "Daily";
            // 
            // comboItem11
            // 
            this.comboItem11.Text = "Weekly";
            // 
            // comboItem12
            // 
            this.comboItem12.Text = "Monthly";
            // 
            // comboItem13
            // 
            this.comboItem13.Text = "Quarterly";
            // 
            // comboItem14
            // 
            this.comboItem14.Text = "Semiannual";
            // 
            // comboItem15
            // 
            this.comboItem15.Text = "Yearly";
            // 
            // textBoxXListName
            // 
            this.textBoxXListName.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textBoxXListName.Border.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.textBoxXListName.Border.BorderLeftColor = System.Drawing.Color.Green;
            this.textBoxXListName.Border.BorderLeftWidth = 3;
            this.textBoxXListName.Border.Class = "TextBoxBorder";
            this.textBoxXListName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxXListName.ForeColor = System.Drawing.Color.Black;
            this.textBoxXListName.Location = new System.Drawing.Point(366, 210);
            this.textBoxXListName.Name = "textBoxXListName";
            this.textBoxXListName.Size = new System.Drawing.Size(209, 23);
            this.textBoxXListName.TabIndex = 77;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelX1.Location = new System.Drawing.Point(285, 210);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 21);
            this.labelX1.TabIndex = 78;
            this.labelX1.Text = "List Name:";
            this.labelX1.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelX2.Location = new System.Drawing.Point(285, 239);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 21);
            this.labelX2.TabIndex = 79;
            this.labelX2.Text = "Timeframe:";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Far;
            this.labelX2.Visible = false;
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelX3.Location = new System.Drawing.Point(226, 266);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(134, 21);
            this.labelX3.TabIndex = 80;
            this.labelX3.Text = "Continuation Types:";
            this.labelX3.TextAlignment = System.Drawing.StringAlignment.Far;
            this.labelX3.Visible = false;
            // 
            // labelX_back
            // 
            // 
            // 
            // 
            this.labelX_back.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX_back.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX_back.ForeColor = System.Drawing.Color.Black;
            this.labelX_back.Location = new System.Drawing.Point(3, 4);
            this.labelX_back.Name = "labelX_back";
            this.labelX_back.PaddingLeft = 6;
            this.labelX_back.Size = new System.Drawing.Size(68, 64);
            this.labelX_back.Symbol = "";
            this.labelX_back.SymbolColor = System.Drawing.Color.Green;
            this.labelX_back.SymbolSize = 50F;
            this.labelX_back.TabIndex = 105;
            this.labelX_back.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // AddListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.labelX_back);
            this.Controls.Add(this.cmbContinuationType);
            this.Controls.Add(this.cmbHistoricalPeriod);
            this.Controls.Add(this.textBoxXListName);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelXTitle);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "AddListControl";
            this.Size = new System.Drawing.Size(800, 500);
            this.Load += new System.EventHandler(this.EditListControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        internal DevComponents.DotNetBar.ButtonX cancelButton;
        internal DevComponents.DotNetBar.LabelX labelXTitle;
        internal DevComponents.DotNetBar.ButtonX saveButton;
        private System.Windows.Forms.ToolTip toolTip1;
        internal DevComponents.DotNetBar.Controls.ComboBoxEx cmbContinuationType;
        internal DevComponents.DotNetBar.Controls.ComboBoxEx cmbHistoricalPeriod;
        private DevComponents.Editors.ComboItem comboItem_tick;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem3;
        private DevComponents.Editors.ComboItem comboItem4;
        private DevComponents.Editors.ComboItem comboItem5;
        private DevComponents.Editors.ComboItem comboItem6;
        private DevComponents.Editors.ComboItem comboItem7;
        private DevComponents.Editors.ComboItem comboItem8;
        private DevComponents.Editors.ComboItem comboItem9;
        private DevComponents.Editors.ComboItem comboItem10;
        private DevComponents.Editors.ComboItem comboItem11;
        private DevComponents.Editors.ComboItem comboItem12;
        private DevComponents.Editors.ComboItem comboItem13;
        private DevComponents.Editors.ComboItem comboItem14;
        private DevComponents.Editors.ComboItem comboItem15;
        internal DevComponents.DotNetBar.Controls.TextBoxX textBoxXListName;
        internal DevComponents.DotNetBar.LabelX labelX1;
        internal DevComponents.DotNetBar.LabelX labelX2;
        internal DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX_back;

    }
}
