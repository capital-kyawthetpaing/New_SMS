namespace M_Setting
{
    partial class M_Setting
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(M_Setting));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.sc_Staff = new Search.CKM_SearchControl();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.BT_Display = new CKM_Controls.CKM_Button();
            this.dgvsetting = new CKM_Controls.CKM_GridView();
            this.colAll = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.AdminKBN = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SettingKBN = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DefaultKBN = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.StaffCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StaffName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MenuKBN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpdateDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ckM_CheckBox3 = new CKM_Controls.CKM_CheckBox();
            this.ckM_CheckBox4 = new CKM_Controls.CKM_CheckBox();
            this.ckM_CheckBox5 = new CKM_Controls.CKM_CheckBox();
            this.ckM_CheckBox6 = new CKM_Controls.CKM_CheckBox();
            this.ckM_RadioButton1 = new CKM_Controls.CKM_RadioButton();
            this.ckM_RadioButton2 = new CKM_Controls.CKM_RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckM_CheckBox2 = new CKM_Controls.CKM_CheckBox();
            this.ckM_CheckBox1 = new CKM_Controls.CKM_CheckBox();
            this.chk_adm = new CKM_Controls.CKM_CheckBox();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvsetting)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.panel1);
            this.PanelHeader.Controls.Add(this.ckM_RadioButton2);
            this.PanelHeader.Controls.Add(this.ckM_RadioButton1);
            this.PanelHeader.Controls.Add(this.BT_Display);
            this.PanelHeader.Controls.Add(this.ckM_Label6);
            this.PanelHeader.Controls.Add(this.sc_Staff);
            this.PanelHeader.Size = new System.Drawing.Size(1818, 144);
            this.PanelHeader.Controls.SetChildIndex(this.sc_Staff, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label6, 0);
            this.PanelHeader.Controls.SetChildIndex(this.BT_Display, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_RadioButton1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_RadioButton2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.panel1, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(1284, 0);
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            // 
            // sc_Staff
            // 
            this.sc_Staff.AutoSize = true;
            this.sc_Staff.ChangeDate = "";
            this.sc_Staff.ChangeDateWidth = 100;
            this.sc_Staff.Code = "";
            this.sc_Staff.CodeWidth = 70;
            this.sc_Staff.CodeWidth1 = 70;
            this.sc_Staff.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.sc_Staff.DataCheck = false;
            this.sc_Staff.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.sc_Staff.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.sc_Staff.IsCopy = false;
            this.sc_Staff.LabelText = "";
            this.sc_Staff.LabelVisible = true;
            this.sc_Staff.Location = new System.Drawing.Point(106, 6);
            this.sc_Staff.Margin = new System.Windows.Forms.Padding(0);
            this.sc_Staff.Name = "sc_Staff";
            this.sc_Staff.NameWidth = 250;
            this.sc_Staff.SearchEnable = true;
            this.sc_Staff.Size = new System.Drawing.Size(354, 48);
            this.sc_Staff.Stype = Search.CKM_SearchControl.SearchType.スタッフ;
            this.sc_Staff.TabIndex = 0;
            this.sc_Staff.test = null;
            this.sc_Staff.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.sc_Staff.UseChangeDate = false;
            this.sc_Staff.Value1 = null;
            this.sc_Staff.Value2 = null;
            this.sc_Staff.Value3 = null;
            this.sc_Staff.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.sc_Staff_CodeKeyDownEvent);
            // 
            // ckM_Label6
            // 
            this.ckM_Label6.AutoSize = true;
            this.ckM_Label6.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label6.DefaultlabelSize = true;
            this.ckM_Label6.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label6.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label6.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label6.Location = new System.Drawing.Point(44, 15);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(59, 12);
            this.ckM_Label6.TabIndex = 110;
            this.ckM_Label6.Text = "担当ｽﾀｯﾌ";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BT_Display
            // 
            this.BT_Display.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BT_Display.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BT_Display.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BT_Display.DefaultBtnSize = false;
            this.BT_Display.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BT_Display.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Display.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.BT_Display.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BT_Display.Location = new System.Drawing.Point(1007, 84);
            this.BT_Display.Margin = new System.Windows.Forms.Padding(1);
            this.BT_Display.Name = "BT_Display";
            this.BT_Display.Size = new System.Drawing.Size(100, 25);
            this.BT_Display.TabIndex = 3;
            this.BT_Display.Text = "表示 (F11)";
            this.BT_Display.UseVisualStyleBackColor = false;
            this.BT_Display.Click += new System.EventHandler(this.BT_Display_Click);
            // 
            // dgvsetting
            // 
            this.dgvsetting.AllowUserToAddRows = false;
            this.dgvsetting.AllowUserToDeleteRows = false;
            this.dgvsetting.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.dgvsetting.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvsetting.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvsetting.CheckCol = ((System.Collections.ArrayList)(resources.GetObject("dgvsetting.CheckCol")));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvsetting.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvsetting.ColumnHeadersHeight = 25;
            this.dgvsetting.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAll,
            this.AdminKBN,
            this.SettingKBN,
            this.DefaultKBN,
            this.StaffCD,
            this.StaffName,
            this.MenuKBN,
            this.UpdateDateTime});
            this.dgvsetting.EnableHeadersVisualStyles = false;
            this.dgvsetting.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvsetting.Location = new System.Drawing.Point(107, 232);
            this.dgvsetting.Name = "dgvsetting";
            this.dgvsetting.RowHeight_ = 20;
            this.dgvsetting.RowTemplate.Height = 20;
            this.dgvsetting.Size = new System.Drawing.Size(1025, 455);
            this.dgvsetting.TabIndex = 4;
            this.dgvsetting.UseRowNo = true;
            this.dgvsetting.UseSetting = true;
            this.dgvsetting.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvsetting_CellContentClick);
            this.dgvsetting.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvsetting_DataError);
            // 
            // colAll
            // 
            this.colAll.DataPropertyName = "colAll";
            this.colAll.FalseValue = "False";
            this.colAll.HeaderText = "";
            this.colAll.IndeterminateValue = "False";
            this.colAll.Name = "colAll";
            this.colAll.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAll.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colAll.TrueValue = "True";
            this.colAll.Width = 80;
            // 
            // AdminKBN
            // 
            this.AdminKBN.DataPropertyName = "AdminKBN";
            this.AdminKBN.FalseValue = " False";
            this.AdminKBN.HeaderText = "管理者";
            this.AdminKBN.IndeterminateValue = " False";
            this.AdminKBN.Name = "AdminKBN";
            this.AdminKBN.TrueValue = " True";
            this.AdminKBN.Width = 80;
            // 
            // SettingKBN
            // 
            this.SettingKBN.DataPropertyName = "SettingKBN";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "False";
            this.SettingKBN.DefaultCellStyle = dataGridViewCellStyle3;
            this.SettingKBN.FalseValue = " False";
            this.SettingKBN.HeaderText = "設定";
            this.SettingKBN.IndeterminateValue = " False";
            this.SettingKBN.Name = "SettingKBN";
            this.SettingKBN.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SettingKBN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.SettingKBN.TrueValue = " True";
            this.SettingKBN.Width = 80;
            // 
            // DefaultKBN
            // 
            this.DefaultKBN.DataPropertyName = "DefaultKBN";
            this.DefaultKBN.FalseValue = " False";
            this.DefaultKBN.HeaderText = "デフォルト";
            this.DefaultKBN.IndeterminateValue = " False";
            this.DefaultKBN.Name = "DefaultKBN";
            this.DefaultKBN.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DefaultKBN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.DefaultKBN.TrueValue = " True";
            this.DefaultKBN.Width = 80;
            // 
            // StaffCD
            // 
            this.StaffCD.DataPropertyName = "StaffCD";
            this.StaffCD.HeaderText = "オペレーターCD";
            this.StaffCD.Name = "StaffCD";
            this.StaffCD.ReadOnly = true;
            this.StaffCD.Width = 120;
            // 
            // StaffName
            // 
            this.StaffName.DataPropertyName = "StaffName";
            this.StaffName.HeaderText = "オペレーター名";
            this.StaffName.Name = "StaffName";
            this.StaffName.ReadOnly = true;
            this.StaffName.Width = 300;
            // 
            // MenuKBN
            // 
            this.MenuKBN.DataPropertyName = "MenuKBN";
            this.MenuKBN.HeaderText = "メニュー種類";
            this.MenuKBN.Name = "MenuKBN";
            this.MenuKBN.ReadOnly = true;
            this.MenuKBN.Width = 120;
            // 
            // UpdateDateTime
            // 
            this.UpdateDateTime.DataPropertyName = "UpdateDateTime";
            this.UpdateDateTime.HeaderText = "変更日";
            this.UpdateDateTime.Name = "UpdateDateTime";
            this.UpdateDateTime.ReadOnly = true;
            // 
            // ckM_CheckBox3
            // 
            this.ckM_CheckBox3.AutoSize = true;
            this.ckM_CheckBox3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckM_CheckBox3.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_CheckBox3.Location = new System.Drawing.Point(228, 210);
            this.ckM_CheckBox3.Name = "ckM_CheckBox3";
            this.ckM_CheckBox3.Size = new System.Drawing.Size(63, 16);
            this.ckM_CheckBox3.TabIndex = 1;
            this.ckM_CheckBox3.Text = "管理者";
            this.ckM_CheckBox3.UseVisualStyleBackColor = true;
            this.ckM_CheckBox3.CheckedChanged += new System.EventHandler(this.adminfl_Click);
            // 
            // ckM_CheckBox4
            // 
            this.ckM_CheckBox4.AutoSize = true;
            this.ckM_CheckBox4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckM_CheckBox4.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_CheckBox4.Location = new System.Drawing.Point(308, 211);
            this.ckM_CheckBox4.Name = "ckM_CheckBox4";
            this.ckM_CheckBox4.Size = new System.Drawing.Size(50, 16);
            this.ckM_CheckBox4.TabIndex = 2;
            this.ckM_CheckBox4.Text = "設定";
            this.ckM_CheckBox4.UseVisualStyleBackColor = true;
            this.ckM_CheckBox4.CheckedChanged += new System.EventHandler(this.settingfl_Click);
            // 
            // ckM_CheckBox5
            // 
            this.ckM_CheckBox5.AutoSize = true;
            this.ckM_CheckBox5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckM_CheckBox5.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_CheckBox5.Location = new System.Drawing.Point(388, 211);
            this.ckM_CheckBox5.Name = "ckM_CheckBox5";
            this.ckM_CheckBox5.Size = new System.Drawing.Size(89, 16);
            this.ckM_CheckBox5.TabIndex = 3;
            this.ckM_CheckBox5.Text = "デフォルト";
            this.ckM_CheckBox5.UseVisualStyleBackColor = true;
            this.ckM_CheckBox5.CheckedChanged += new System.EventHandler(this.Defaultfl_Click);
            // 
            // ckM_CheckBox6
            // 
            this.ckM_CheckBox6.AutoSize = true;
            this.ckM_CheckBox6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckM_CheckBox6.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_CheckBox6.Location = new System.Drawing.Point(148, 210);
            this.ckM_CheckBox6.Name = "ckM_CheckBox6";
            this.ckM_CheckBox6.Size = new System.Drawing.Size(50, 16);
            this.ckM_CheckBox6.TabIndex = 0;
            this.ckM_CheckBox6.Text = "全部";
            this.ckM_CheckBox6.UseVisualStyleBackColor = true;
            this.ckM_CheckBox6.CheckedChanged += new System.EventHandler(this.ckM_CheckBox6_CheckedChanged);
            // 
            // ckM_RadioButton1
            // 
            this.ckM_RadioButton1.AutoSize = true;
            this.ckM_RadioButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckM_RadioButton1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_RadioButton1.Location = new System.Drawing.Point(106, 42);
            this.ckM_RadioButton1.Name = "ckM_RadioButton1";
            this.ckM_RadioButton1.Size = new System.Drawing.Size(49, 16);
            this.ckM_RadioButton1.TabIndex = 1;
            this.ckM_RadioButton1.TabStop = true;
            this.ckM_RadioButton1.Text = "全部";
            this.ckM_RadioButton1.UseVisualStyleBackColor = true;
            this.ckM_RadioButton1.CheckedChanged += new System.EventHandler(this.ckM_RadioButton1_CheckedChanged);
            // 
            // ckM_RadioButton2
            // 
            this.ckM_RadioButton2.AutoSize = true;
            this.ckM_RadioButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckM_RadioButton2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_RadioButton2.Location = new System.Drawing.Point(228, 42);
            this.ckM_RadioButton2.Name = "ckM_RadioButton2";
            this.ckM_RadioButton2.Size = new System.Drawing.Size(49, 16);
            this.ckM_RadioButton2.TabIndex = 2;
            this.ckM_RadioButton2.TabStop = true;
            this.ckM_RadioButton2.Text = "条件";
            this.ckM_RadioButton2.UseVisualStyleBackColor = true;
            this.ckM_RadioButton2.CheckedChanged += new System.EventHandler(this.ckM_RadioButton2_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ckM_CheckBox2);
            this.panel1.Controls.Add(this.ckM_CheckBox1);
            this.panel1.Controls.Add(this.chk_adm);
            this.panel1.Location = new System.Drawing.Point(217, 68);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 60);
            this.panel1.TabIndex = 113;
            // 
            // ckM_CheckBox2
            // 
            this.ckM_CheckBox2.AutoSize = true;
            this.ckM_CheckBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckM_CheckBox2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_CheckBox2.Location = new System.Drawing.Point(171, 14);
            this.ckM_CheckBox2.Name = "ckM_CheckBox2";
            this.ckM_CheckBox2.Size = new System.Drawing.Size(89, 16);
            this.ckM_CheckBox2.TabIndex = 2;
            this.ckM_CheckBox2.Text = "デフォルト";
            this.ckM_CheckBox2.UseVisualStyleBackColor = true;
            // 
            // ckM_CheckBox1
            // 
            this.ckM_CheckBox1.AutoSize = true;
            this.ckM_CheckBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckM_CheckBox1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_CheckBox1.Location = new System.Drawing.Point(92, 14);
            this.ckM_CheckBox1.Name = "ckM_CheckBox1";
            this.ckM_CheckBox1.Size = new System.Drawing.Size(50, 16);
            this.ckM_CheckBox1.TabIndex = 1;
            this.ckM_CheckBox1.Text = "設定";
            this.ckM_CheckBox1.UseVisualStyleBackColor = true;
            // 
            // chk_adm
            // 
            this.chk_adm.AutoSize = true;
            this.chk_adm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chk_adm.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chk_adm.Location = new System.Drawing.Point(12, 13);
            this.chk_adm.Name = "chk_adm";
            this.chk_adm.Size = new System.Drawing.Size(63, 16);
            this.chk_adm.TabIndex = 0;
            this.chk_adm.Text = "管理者";
            this.chk_adm.UseVisualStyleBackColor = true;
            // 
            // M_Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1820, 745);
            this.Controls.Add(this.ckM_CheckBox6);
            this.Controls.Add(this.ckM_CheckBox5);
            this.Controls.Add(this.ckM_CheckBox4);
            this.Controls.Add(this.ckM_CheckBox3);
            this.Controls.Add(this.dgvsetting);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "M_Setting";
            this.PanelHeaderHeight = 200;
            this.Text = "Setting";
            this.Load += new System.EventHandler(this.M_Setting_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.M_Setting_KeyUp);
            this.Controls.SetChildIndex(this.dgvsetting, 0);
            this.Controls.SetChildIndex(this.ckM_CheckBox3, 0);
            this.Controls.SetChildIndex(this.ckM_CheckBox4, 0);
            this.Controls.SetChildIndex(this.ckM_CheckBox5, 0);
            this.Controls.SetChildIndex(this.ckM_CheckBox6, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvsetting)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Search.CKM_SearchControl sc_Staff;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_Button BT_Display;
        private CKM_Controls.CKM_GridView dgvsetting;
        private CKM_Controls.CKM_CheckBox ckM_CheckBox3;
        private CKM_Controls.CKM_CheckBox ckM_CheckBox4;
        private CKM_Controls.CKM_CheckBox ckM_CheckBox5;
        private CKM_Controls.CKM_CheckBox ckM_CheckBox6;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colAll;
        private System.Windows.Forms.DataGridViewCheckBoxColumn AdminKBN;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SettingKBN;
        private System.Windows.Forms.DataGridViewCheckBoxColumn DefaultKBN;
        private System.Windows.Forms.DataGridViewTextBoxColumn StaffCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn StaffName;
        private System.Windows.Forms.DataGridViewTextBoxColumn MenuKBN;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpdateDateTime;
        private CKM_Controls.CKM_RadioButton ckM_RadioButton2;
        private CKM_Controls.CKM_RadioButton ckM_RadioButton1;
        private System.Windows.Forms.Panel panel1;
        private CKM_Controls.CKM_CheckBox ckM_CheckBox2;
        private CKM_Controls.CKM_CheckBox ckM_CheckBox1;
        private CKM_Controls.CKM_CheckBox chk_adm;
    }
}

