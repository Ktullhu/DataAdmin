using System.Windows.Forms;

namespace DataExport.Forms
{
    partial class CustomFormulaControl
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.ui_buttonX_cancel = new DevComponents.DotNetBar.ButtonX();
            this.ui_buttonX_save = new DevComponents.DotNetBar.ButtonX();
            this.buttonXDelete = new DevComponents.DotNetBar.ButtonX();
            this.buttonXAdd = new DevComponents.DotNetBar.ButtonX();
            this.changeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelExAddNew = new DevComponents.DotNetBar.PanelEx();
            this.buttonX_truncate = new DevComponents.DotNetBar.ButtonX();
            this.buttonX10 = new DevComponents.DotNetBar.ButtonX();
            this.buttonX9 = new DevComponents.DotNetBar.ButtonX();
            this.buttonX_sqrt = new DevComponents.DotNetBar.ButtonX();
            this.buttonX_round = new DevComponents.DotNetBar.ButtonX();
            this.buttonX_pow = new DevComponents.DotNetBar.ButtonX();
            this.buttonX_min = new DevComponents.DotNetBar.ButtonX();
            this.buttonX_Abs = new DevComponents.DotNetBar.ButtonX();
            this.buttonX_max = new DevComponents.DotNetBar.ButtonX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.buttonX2 = new DevComponents.DotNetBar.ButtonX();
            this.ui_buttonX_operation_add = new DevComponents.DotNetBar.ButtonX();
            this.ui_buttonX_operation_closeBraket = new DevComponents.DotNetBar.ButtonX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.ui_buttonX_constant_add = new DevComponents.DotNetBar.ButtonX();
            this.ui_buttonX_replaceColumn = new DevComponents.DotNetBar.ButtonX();
            this.ui_textBoxX_constant = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.ui_buttonX_replaceConstant = new DevComponents.DotNetBar.ButtonX();
            this.ui_buttonX_operation_minus = new DevComponents.DotNetBar.ButtonX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.ui_buttonX_operation_multi = new DevComponents.DotNetBar.ButtonX();
            this.ui_comboBoxEx_useColumn = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem5 = new DevComponents.Editors.ComboItem();
            this.comboItem6 = new DevComponents.Editors.ComboItem();
            this.comboItem7 = new DevComponents.Editors.ComboItem();
            this.comboItem8 = new DevComponents.Editors.ComboItem();
            this.comboItem9 = new DevComponents.Editors.ComboItem();
            this.ui_buttonX_operation_div = new DevComponents.DotNetBar.ButtonX();
            this.ui_buttonX_useColumn = new DevComponents.DotNetBar.ButtonX();
            this.ui_buttonX_ifAdd = new DevComponents.DotNetBar.ButtonX();
            this.ui_buttonX_operation_openBraket = new DevComponents.DotNetBar.ButtonX();
            this.textBoxX_formulaName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.comboItem4 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.buttonX3 = new DevComponents.DotNetBar.ButtonX();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.formulaControl1 = new DataExport.Controls.FormulaControl();
            this.elementContainerControl1 = new DataExport.Controls.ElementContainerControl();
            this.panelExAddNew.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelX1.ForeColor = System.Drawing.Color.CadetBlue;
            this.labelX1.Location = new System.Drawing.Point(70, 9);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(173, 36);
            this.labelX1.TabIndex = 7;
            this.labelX1.Text = "Custom formula";
            // 
            // ui_buttonX_cancel
            // 
            this.ui_buttonX_cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_buttonX_cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_cancel.Location = new System.Drawing.Point(706, 385);
            this.ui_buttonX_cancel.Name = "ui_buttonX_cancel";
            this.ui_buttonX_cancel.Size = new System.Drawing.Size(104, 40);
            this.ui_buttonX_cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_cancel.TabIndex = 9;
            this.ui_buttonX_cancel.Text = "Close";
            // 
            // ui_buttonX_save
            // 
            this.ui_buttonX_save.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_buttonX_save.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.ui_buttonX_save.Location = new System.Drawing.Point(601, 385);
            this.ui_buttonX_save.Name = "ui_buttonX_save";
            this.ui_buttonX_save.Size = new System.Drawing.Size(97, 40);
            this.ui_buttonX_save.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_save.Symbol = "";
            this.ui_buttonX_save.TabIndex = 26;
            this.ui_buttonX_save.Text = "SAVE";
            this.ui_buttonX_save.Click += new System.EventHandler(this.ui_buttonX_save_Click);
            // 
            // buttonXDelete
            // 
            this.buttonXDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonXDelete.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXDelete.Location = new System.Drawing.Point(372, 155);
            this.buttonXDelete.Name = "buttonXDelete";
            this.buttonXDelete.Size = new System.Drawing.Size(57, 23);
            this.buttonXDelete.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXDelete.TabIndex = 28;
            this.buttonXDelete.Text = "DELETE";
            this.buttonXDelete.Click += new System.EventHandler(this.buttonXDelete_Click);
            // 
            // buttonXAdd
            // 
            this.buttonXAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonXAdd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXAdd.Location = new System.Drawing.Point(316, 155);
            this.buttonXAdd.Name = "buttonXAdd";
            this.buttonXAdd.Size = new System.Drawing.Size(50, 23);
            this.buttonXAdd.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXAdd.TabIndex = 29;
            this.buttonXAdd.Text = "ADD";
            this.buttonXAdd.Click += new System.EventHandler(this.buttonXAdd_Click);
            // 
            // changeToolStripMenuItem
            // 
            this.changeToolStripMenuItem.Name = "changeToolStripMenuItem";
            this.changeToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.changeToolStripMenuItem.Text = "Change";
            // 
            // panelExAddNew
            // 
            this.panelExAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelExAddNew.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelExAddNew.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelExAddNew.Controls.Add(this.buttonX_truncate);
            this.panelExAddNew.Controls.Add(this.buttonX10);
            this.panelExAddNew.Controls.Add(this.buttonX9);
            this.panelExAddNew.Controls.Add(this.buttonX_sqrt);
            this.panelExAddNew.Controls.Add(this.buttonX_round);
            this.panelExAddNew.Controls.Add(this.buttonX_pow);
            this.panelExAddNew.Controls.Add(this.buttonX_min);
            this.panelExAddNew.Controls.Add(this.buttonX_Abs);
            this.panelExAddNew.Controls.Add(this.buttonX_max);
            this.panelExAddNew.Controls.Add(this.buttonX1);
            this.panelExAddNew.Controls.Add(this.buttonX2);
            this.panelExAddNew.Controls.Add(this.ui_buttonX_operation_add);
            this.panelExAddNew.Controls.Add(this.ui_buttonX_operation_closeBraket);
            this.panelExAddNew.Controls.Add(this.labelX8);
            this.panelExAddNew.Controls.Add(this.ui_buttonX_constant_add);
            this.panelExAddNew.Controls.Add(this.ui_buttonX_replaceColumn);
            this.panelExAddNew.Controls.Add(this.ui_textBoxX_constant);
            this.panelExAddNew.Controls.Add(this.ui_buttonX_replaceConstant);
            this.panelExAddNew.Controls.Add(this.ui_buttonX_operation_minus);
            this.panelExAddNew.Controls.Add(this.labelX9);
            this.panelExAddNew.Controls.Add(this.ui_buttonX_operation_multi);
            this.panelExAddNew.Controls.Add(this.ui_comboBoxEx_useColumn);
            this.panelExAddNew.Controls.Add(this.ui_buttonX_operation_div);
            this.panelExAddNew.Controls.Add(this.ui_buttonX_useColumn);
            this.panelExAddNew.Controls.Add(this.ui_buttonX_ifAdd);
            this.panelExAddNew.Controls.Add(this.ui_buttonX_operation_openBraket);
            this.panelExAddNew.Enabled = false;
            this.panelExAddNew.Location = new System.Drawing.Point(438, 180);
            this.panelExAddNew.Name = "panelExAddNew";
            this.panelExAddNew.Size = new System.Drawing.Size(370, 199);
            this.panelExAddNew.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelExAddNew.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelExAddNew.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelExAddNew.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelExAddNew.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelExAddNew.Style.GradientAngle = 90;
            this.panelExAddNew.TabIndex = 59;
            // 
            // buttonX_truncate
            // 
            this.buttonX_truncate.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX_truncate.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX_truncate.Location = new System.Drawing.Point(89, 132);
            this.buttonX_truncate.Name = "buttonX_truncate";
            this.buttonX_truncate.Size = new System.Drawing.Size(74, 26);
            this.buttonX_truncate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX_truncate.TabIndex = 68;
            this.buttonX_truncate.Text = "Truncate";
            this.buttonX_truncate.Click += new System.EventHandler(this.buttonX_max_Click);
            // 
            // buttonX10
            // 
            this.buttonX10.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX10.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX10.Location = new System.Drawing.Point(308, 164);
            this.buttonX10.Name = "buttonX10";
            this.buttonX10.Size = new System.Drawing.Size(34, 26);
            this.buttonX10.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX10.TabIndex = 67;
            this.buttonX10.Text = "%";
            this.buttonX10.Click += new System.EventHandler(this.ui_buttonX_operation_Click);
            // 
            // buttonX9
            // 
            this.buttonX9.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX9.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX9.Location = new System.Drawing.Point(268, 164);
            this.buttonX9.Name = "buttonX9";
            this.buttonX9.Size = new System.Drawing.Size(34, 26);
            this.buttonX9.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX9.TabIndex = 66;
            this.buttonX9.Text = "NOT";
            this.buttonX9.Click += new System.EventHandler(this.ui_buttonX_operation_Click);
            // 
            // buttonX_sqrt
            // 
            this.buttonX_sqrt.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX_sqrt.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX_sqrt.Location = new System.Drawing.Point(49, 164);
            this.buttonX_sqrt.Name = "buttonX_sqrt";
            this.buttonX_sqrt.Size = new System.Drawing.Size(34, 26);
            this.buttonX_sqrt.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX_sqrt.TabIndex = 65;
            this.buttonX_sqrt.Text = "Sqrt";
            this.buttonX_sqrt.Click += new System.EventHandler(this.buttonX_max_Click);
            // 
            // buttonX_round
            // 
            this.buttonX_round.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX_round.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX_round.Location = new System.Drawing.Point(89, 164);
            this.buttonX_round.Name = "buttonX_round";
            this.buttonX_round.Size = new System.Drawing.Size(74, 26);
            this.buttonX_round.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX_round.TabIndex = 64;
            this.buttonX_round.Text = "Round";
            this.buttonX_round.Click += new System.EventHandler(this.buttonX_max_Click);
            // 
            // buttonX_pow
            // 
            this.buttonX_pow.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX_pow.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX_pow.Location = new System.Drawing.Point(9, 164);
            this.buttonX_pow.Name = "buttonX_pow";
            this.buttonX_pow.Size = new System.Drawing.Size(34, 26);
            this.buttonX_pow.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX_pow.TabIndex = 63;
            this.buttonX_pow.Text = "Pow";
            this.buttonX_pow.Click += new System.EventHandler(this.buttonX_max_Click);
            // 
            // buttonX_min
            // 
            this.buttonX_min.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX_min.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX_min.Location = new System.Drawing.Point(49, 132);
            this.buttonX_min.Name = "buttonX_min";
            this.buttonX_min.Size = new System.Drawing.Size(34, 26);
            this.buttonX_min.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX_min.TabIndex = 62;
            this.buttonX_min.Text = "Min";
            this.buttonX_min.Click += new System.EventHandler(this.buttonX_max_Click);
            // 
            // buttonX_Abs
            // 
            this.buttonX_Abs.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX_Abs.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX_Abs.Location = new System.Drawing.Point(167, 164);
            this.buttonX_Abs.Name = "buttonX_Abs";
            this.buttonX_Abs.Size = new System.Drawing.Size(34, 26);
            this.buttonX_Abs.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX_Abs.TabIndex = 60;
            this.buttonX_Abs.Text = "Abs";
            this.buttonX_Abs.Click += new System.EventHandler(this.buttonX_Abs_Click);
            // 
            // buttonX_max
            // 
            this.buttonX_max.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX_max.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX_max.Location = new System.Drawing.Point(9, 132);
            this.buttonX_max.Name = "buttonX_max";
            this.buttonX_max.Size = new System.Drawing.Size(34, 26);
            this.buttonX_max.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX_max.TabIndex = 61;
            this.buttonX_max.Text = "Max";
            this.buttonX_max.Click += new System.EventHandler(this.buttonX_max_Click);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(268, 132);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(34, 26);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 58;
            this.buttonX1.Text = "OR";
            this.buttonX1.Click += new System.EventHandler(this.ui_buttonX_operation_Click);
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX2.Location = new System.Drawing.Point(308, 132);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(34, 26);
            this.buttonX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX2.TabIndex = 59;
            this.buttonX2.Text = "AND";
            this.buttonX2.Click += new System.EventHandler(this.ui_buttonX_operation_Click);
            // 
            // ui_buttonX_operation_add
            // 
            this.ui_buttonX_operation_add.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_operation_add.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_operation_add.Location = new System.Drawing.Point(268, 35);
            this.ui_buttonX_operation_add.Name = "ui_buttonX_operation_add";
            this.ui_buttonX_operation_add.Size = new System.Drawing.Size(34, 26);
            this.ui_buttonX_operation_add.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_operation_add.TabIndex = 21;
            this.ui_buttonX_operation_add.Text = "+";
            this.ui_buttonX_operation_add.Click += new System.EventHandler(this.ui_buttonX_operation_Click);
            // 
            // ui_buttonX_operation_closeBraket
            // 
            this.ui_buttonX_operation_closeBraket.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_operation_closeBraket.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_operation_closeBraket.Location = new System.Drawing.Point(308, 100);
            this.ui_buttonX_operation_closeBraket.Name = "ui_buttonX_operation_closeBraket";
            this.ui_buttonX_operation_closeBraket.Size = new System.Drawing.Size(34, 26);
            this.ui_buttonX_operation_closeBraket.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_operation_closeBraket.TabIndex = 57;
            this.ui_buttonX_operation_closeBraket.Text = ")";
            this.ui_buttonX_operation_closeBraket.Click += new System.EventHandler(this.ui_buttonX_operation_closeBraket_Click);
            // 
            // labelX8
            // 
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX8.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX8.ForeColor = System.Drawing.Color.DimGray;
            this.labelX8.Location = new System.Drawing.Point(146, 6);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(106, 23);
            this.labelX8.TabIndex = 33;
            this.labelX8.Text = "use constant";
            // 
            // ui_buttonX_constant_add
            // 
            this.ui_buttonX_constant_add.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_constant_add.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_constant_add.Location = new System.Drawing.Point(146, 67);
            this.ui_buttonX_constant_add.Name = "ui_buttonX_constant_add";
            this.ui_buttonX_constant_add.Size = new System.Drawing.Size(106, 26);
            this.ui_buttonX_constant_add.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_constant_add.TabIndex = 35;
            this.ui_buttonX_constant_add.Text = "Add constant";
            this.ui_buttonX_constant_add.Click += new System.EventHandler(this.ui_buttonX_constant_add_Click);
            // 
            // ui_buttonX_replaceColumn
            // 
            this.ui_buttonX_replaceColumn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_replaceColumn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_replaceColumn.Location = new System.Drawing.Point(9, 100);
            this.ui_buttonX_replaceColumn.Name = "ui_buttonX_replaceColumn";
            this.ui_buttonX_replaceColumn.Size = new System.Drawing.Size(105, 26);
            this.ui_buttonX_replaceColumn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_replaceColumn.TabIndex = 55;
            this.ui_buttonX_replaceColumn.Text = "Replace column";
            this.ui_buttonX_replaceColumn.Click += new System.EventHandler(this.ui_buttonX_replaceColumn_Click);
            // 
            // ui_textBoxX_constant
            // 
            this.ui_textBoxX_constant.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ui_textBoxX_constant.Border.Class = "TextBoxBorder";
            this.ui_textBoxX_constant.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ui_textBoxX_constant.ForeColor = System.Drawing.Color.Black;
            this.ui_textBoxX_constant.Location = new System.Drawing.Point(146, 35);
            this.ui_textBoxX_constant.Name = "ui_textBoxX_constant";
            this.ui_textBoxX_constant.Size = new System.Drawing.Size(106, 22);
            this.ui_textBoxX_constant.TabIndex = 36;
            this.ui_textBoxX_constant.Text = "0";
            this.ui_textBoxX_constant.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ui_textBoxX_constant_KeyPress);
            // 
            // ui_buttonX_replaceConstant
            // 
            this.ui_buttonX_replaceConstant.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_replaceConstant.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_replaceConstant.Location = new System.Drawing.Point(146, 100);
            this.ui_buttonX_replaceConstant.Name = "ui_buttonX_replaceConstant";
            this.ui_buttonX_replaceConstant.Size = new System.Drawing.Size(106, 26);
            this.ui_buttonX_replaceConstant.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_replaceConstant.TabIndex = 54;
            this.ui_buttonX_replaceConstant.Text = "Replace constant";
            this.ui_buttonX_replaceConstant.Click += new System.EventHandler(this.ui_buttonX_replaceConstant_Click);
            // 
            // ui_buttonX_operation_minus
            // 
            this.ui_buttonX_operation_minus.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_operation_minus.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_operation_minus.Location = new System.Drawing.Point(308, 35);
            this.ui_buttonX_operation_minus.Name = "ui_buttonX_operation_minus";
            this.ui_buttonX_operation_minus.Size = new System.Drawing.Size(34, 26);
            this.ui_buttonX_operation_minus.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_operation_minus.TabIndex = 37;
            this.ui_buttonX_operation_minus.Text = "-";
            this.ui_buttonX_operation_minus.Click += new System.EventHandler(this.ui_buttonX_operation_Click);
            // 
            // labelX9
            // 
            // 
            // 
            // 
            this.labelX9.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX9.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX9.ForeColor = System.Drawing.Color.DimGray;
            this.labelX9.Location = new System.Drawing.Point(9, 6);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(105, 23);
            this.labelX9.TabIndex = 51;
            this.labelX9.Text = "use column";
            // 
            // ui_buttonX_operation_multi
            // 
            this.ui_buttonX_operation_multi.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_operation_multi.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_operation_multi.Location = new System.Drawing.Point(268, 67);
            this.ui_buttonX_operation_multi.Name = "ui_buttonX_operation_multi";
            this.ui_buttonX_operation_multi.Size = new System.Drawing.Size(34, 26);
            this.ui_buttonX_operation_multi.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_operation_multi.TabIndex = 38;
            this.ui_buttonX_operation_multi.Text = "*";
            this.ui_buttonX_operation_multi.Click += new System.EventHandler(this.ui_buttonX_operation_Click);
            // 
            // ui_comboBoxEx_useColumn
            // 
            this.ui_comboBoxEx_useColumn.DisplayMember = "Text";
            this.ui_comboBoxEx_useColumn.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ui_comboBoxEx_useColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_comboBoxEx_useColumn.FormattingEnabled = true;
            this.ui_comboBoxEx_useColumn.ItemHeight = 16;
            this.ui_comboBoxEx_useColumn.Items.AddRange(new object[] {
            this.comboItem5,
            this.comboItem6,
            this.comboItem7,
            this.comboItem8,
            this.comboItem9});
            this.ui_comboBoxEx_useColumn.Location = new System.Drawing.Point(9, 35);
            this.ui_comboBoxEx_useColumn.Name = "ui_comboBoxEx_useColumn";
            this.ui_comboBoxEx_useColumn.Size = new System.Drawing.Size(105, 22);
            this.ui_comboBoxEx_useColumn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_comboBoxEx_useColumn.TabIndex = 46;
            // 
            // comboItem5
            // 
            this.comboItem5.Text = "Open";
            // 
            // comboItem6
            // 
            this.comboItem6.Text = "High";
            // 
            // comboItem7
            // 
            this.comboItem7.Text = "Low";
            // 
            // comboItem8
            // 
            this.comboItem8.Text = "Close";
            // 
            // comboItem9
            // 
            this.comboItem9.Text = "Time";
            // 
            // ui_buttonX_operation_div
            // 
            this.ui_buttonX_operation_div.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_operation_div.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_operation_div.Location = new System.Drawing.Point(308, 68);
            this.ui_buttonX_operation_div.Name = "ui_buttonX_operation_div";
            this.ui_buttonX_operation_div.Size = new System.Drawing.Size(34, 26);
            this.ui_buttonX_operation_div.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_operation_div.TabIndex = 39;
            this.ui_buttonX_operation_div.Text = "/";
            this.ui_buttonX_operation_div.Click += new System.EventHandler(this.ui_buttonX_operation_Click);
            // 
            // ui_buttonX_useColumn
            // 
            this.ui_buttonX_useColumn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_useColumn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_useColumn.Location = new System.Drawing.Point(9, 67);
            this.ui_buttonX_useColumn.Name = "ui_buttonX_useColumn";
            this.ui_buttonX_useColumn.Size = new System.Drawing.Size(105, 26);
            this.ui_buttonX_useColumn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_useColumn.TabIndex = 45;
            this.ui_buttonX_useColumn.Text = "Add column";
            this.ui_buttonX_useColumn.Click += new System.EventHandler(this.ui_buttonX_useColumn_Click);
            // 
            // ui_buttonX_ifAdd
            // 
            this.ui_buttonX_ifAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_ifAdd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_ifAdd.Location = new System.Drawing.Point(169, 132);
            this.ui_buttonX_ifAdd.Name = "ui_buttonX_ifAdd";
            this.ui_buttonX_ifAdd.Size = new System.Drawing.Size(74, 26);
            this.ui_buttonX_ifAdd.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_ifAdd.TabIndex = 40;
            this.ui_buttonX_ifAdd.Text = "if (,,)";
            this.ui_buttonX_ifAdd.Click += new System.EventHandler(this.ui_buttonX_ifAdd_Click);
            // 
            // ui_buttonX_operation_openBraket
            // 
            this.ui_buttonX_operation_openBraket.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ui_buttonX_operation_openBraket.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ui_buttonX_operation_openBraket.Location = new System.Drawing.Point(268, 100);
            this.ui_buttonX_operation_openBraket.Name = "ui_buttonX_operation_openBraket";
            this.ui_buttonX_operation_openBraket.Size = new System.Drawing.Size(34, 26);
            this.ui_buttonX_operation_openBraket.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ui_buttonX_operation_openBraket.TabIndex = 43;
            this.ui_buttonX_operation_openBraket.Text = "(";
            this.ui_buttonX_operation_openBraket.Click += new System.EventHandler(this.ui_buttonX_operation_openBraket_Click);
            // 
            // textBoxX_formulaName
            // 
            this.textBoxX_formulaName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxX_formulaName.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textBoxX_formulaName.Border.Class = "TextBoxBorder";
            this.textBoxX_formulaName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX_formulaName.ButtonCustom.Shortcut = DevComponents.DotNetBar.eShortcut.CtrlS;
            this.textBoxX_formulaName.ButtonCustom.Text = "Save";
            this.textBoxX_formulaName.ForeColor = System.Drawing.Color.Black;
            this.textBoxX_formulaName.Location = new System.Drawing.Point(633, 152);
            this.textBoxX_formulaName.Name = "textBoxX_formulaName";
            this.textBoxX_formulaName.ReadOnly = true;
            this.textBoxX_formulaName.Size = new System.Drawing.Size(175, 22);
            this.textBoxX_formulaName.TabIndex = 14;
            this.textBoxX_formulaName.ButtonCustomClick += new System.EventHandler(this.textBoxX_formulaName_ButtonCustomClick);
            this.textBoxX_formulaName.TextChanged += new System.EventHandler(this.textBoxX_formulaName_TextChanged);
            // 
            // labelX2
            // 
            this.labelX2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX2.ForeColor = System.Drawing.Color.DimGray;
            this.labelX2.Location = new System.Drawing.Point(438, 151);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(189, 23);
            this.labelX2.TabIndex = 15;
            this.labelX2.Text = "current formula name:";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // comboItem4
            // 
            this.comboItem4.Text = "Close";
            // 
            // comboItem3
            // 
            this.comboItem3.Text = "Low";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "High";
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "Open";
            // 
            // buttonX3
            // 
            this.buttonX3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonX3.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX3.Location = new System.Drawing.Point(177, 155);
            this.buttonX3.Name = "buttonX3";
            this.buttonX3.Size = new System.Drawing.Size(133, 23);
            this.buttonX3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX3.TabIndex = 62;
            this.buttonX3.Text = "ADD FROM EXISTING";
            this.buttonX3.Click += new System.EventHandler(this.buttonXAddExisting_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DataExport.Properties.Resources.backbutton1;
            this.pictureBox1.Location = new System.Drawing.Point(20, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(44, 44);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // formulaControl1
            // 
            this.formulaControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formulaControl1.AutoScroll = true;
            this.formulaControl1.Location = new System.Drawing.Point(31, 63);
            this.formulaControl1.Name = "formulaControl1";
            this.formulaControl1.Size = new System.Drawing.Size(777, 79);
            this.formulaControl1.TabIndex = 61;
            this.formulaControl1.FormulaChanged += new DataExport.Controls.FormulaControl.ChangeHandler(this.formulaControl1_FormulaChanged);
            // 
            // elementContainerControl1
            // 
            this.elementContainerControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.elementContainerControl1.AutoScroll = true;
            this.elementContainerControl1.BackColor = System.Drawing.Color.White;
            this.elementContainerControl1.ElementHeight = 32;
            this.elementContainerControl1.ElementsColor = System.Drawing.Color.CadetBlue;
            this.elementContainerControl1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.elementContainerControl1.Location = new System.Drawing.Point(31, 152);
            this.elementContainerControl1.MinimumSize = new System.Drawing.Size(170, 2);
            this.elementContainerControl1.Name = "elementContainerControl1";
            this.elementContainerControl1.Size = new System.Drawing.Size(401, 227);
            this.elementContainerControl1.TabIndex = 60;
            this.elementContainerControl1.Title = "Existing Formulas";
            this.elementContainerControl1.SelectedIndexChanged += new DataExport.Controls.ElementContainerControl.SelectedIndexChangedHandler(this.elementContainerControl1_SelectedIndexChanged);
            // 
            // CustomFormulaControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.buttonX3);
            this.Controls.Add(this.formulaControl1);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.textBoxX_formulaName);
            this.Controls.Add(this.panelExAddNew);
            this.Controls.Add(this.buttonXAdd);
            this.Controls.Add(this.buttonXDelete);
            this.Controls.Add(this.ui_buttonX_save);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ui_buttonX_cancel);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.elementContainerControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "CustomFormulaControl";
            this.Size = new System.Drawing.Size(850, 470);
            this.SlideOutButtonVisible = false;
            this.Load += new System.EventHandler(this.StartControlLoad);
            this.panelExAddNew.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_cancel;
        internal System.Windows.Forms.PictureBox pictureBox1;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_save;
        private DevComponents.DotNetBar.ButtonX buttonXDelete;
        private DevComponents.DotNetBar.ButtonX buttonXAdd;
        private System.Windows.Forms.ToolStripMenuItem changeToolStripMenuItem;
        private DevComponents.DotNetBar.PanelEx panelExAddNew;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX_formulaName;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_operation_add;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_operation_closeBraket;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_constant_add;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_replaceColumn;
        private DevComponents.DotNetBar.Controls.TextBoxX ui_textBoxX_constant;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_replaceConstant;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_operation_minus;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_operation_multi;
        private DevComponents.DotNetBar.Controls.ComboBoxEx ui_comboBoxEx_useColumn;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_operation_div;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_useColumn;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_ifAdd;
        private DevComponents.DotNetBar.ButtonX ui_buttonX_operation_openBraket;
        private DevComponents.Editors.ComboItem comboItem4;
        private DevComponents.Editors.ComboItem comboItem3;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem1;
        private Controls.ElementContainerControl elementContainerControl1;
        private DevComponents.Editors.ComboItem comboItem5;
        private DevComponents.Editors.ComboItem comboItem6;
        private DevComponents.Editors.ComboItem comboItem7;
        private DevComponents.Editors.ComboItem comboItem8;
        private DevComponents.Editors.ComboItem comboItem9;
        private Controls.FormulaControl formulaControl1;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.ButtonX buttonX2;
        private DevComponents.DotNetBar.ButtonX buttonX_Abs;
        private DevComponents.DotNetBar.ButtonX buttonX_max;
        private DevComponents.DotNetBar.ButtonX buttonX_sqrt;
        private DevComponents.DotNetBar.ButtonX buttonX_round;
        private DevComponents.DotNetBar.ButtonX buttonX_pow;
        private DevComponents.DotNetBar.ButtonX buttonX_min;
        private DevComponents.DotNetBar.ButtonX buttonX9;
        private DevComponents.DotNetBar.ButtonX buttonX10;
        private DevComponents.DotNetBar.ButtonX buttonX_truncate;
        private DevComponents.DotNetBar.ButtonX buttonX3;
    }
}
