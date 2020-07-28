namespace Search
{
    partial class Search_HacchuuShoriNO
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.GvDetail = new CKM_Controls.CKM_GridView();
            this.colOrderProcessNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInsertDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStaffName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrderWayKBNName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVendorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label9 = new CKM_Controls.CKM_Label();
            this.ckM_Label8 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox2 = new CKM_Controls.CKM_TextBox();
            this.ckM_TextBox1 = new CKM_Controls.CKM_TextBox();
            this.CboStoreCD = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.btnSubF11 = new CKM_Controls.CKM_Button();
            this.ScStaff = new Search.CKM_SearchControl();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.btnSubF11);
            this.PanelHeader.Controls.Add(this.CboStoreCD);
            this.PanelHeader.Controls.Add(this.ckM_Label6);
            this.PanelHeader.Controls.Add(this.ScStaff);
            this.PanelHeader.Controls.Add(this.ckM_Label8);
            this.PanelHeader.Controls.Add(this.ckM_TextBox2);
            this.PanelHeader.Controls.Add(this.ckM_TextBox1);
            this.PanelHeader.Controls.Add(this.label9);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.PanelHeader.Size = new System.Drawing.Size(1062, 119);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.label9, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label8, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ScStaff, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label6, 0);
            this.PanelHeader.Controls.SetChildIndex(this.CboStoreCD, 0);
            this.PanelHeader.Controls.SetChildIndex(this.btnSubF11, 0);
            // 
            // ckM_Label1
            // 
            this.ckM_Label1.AutoSize = true;
            this.ckM_Label1.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label1.DefaultlabelSize = true;
            this.ckM_Label1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label1.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label1.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label1.Location = new System.Drawing.Point(54, 21);
            this.ckM_Label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label1.TabIndex = 2;
            this.ckM_Label1.Text = "登録日";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GvDetail
            // 
            this.GvDetail.AllowUserToAddRows = false;
            this.GvDetail.AllowUserToDeleteRows = false;
            this.GvDetail.AllowUserToResizeRows = false;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.GvDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            this.GvDetail.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle14.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GvDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.GvDetail.ColumnHeadersHeight = 25;
            this.GvDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOrderProcessNO,
            this.colInsertDateTime,
            this.colStaffName,
            this.colOrderWayKBNName,
            this.colVendorName});
            this.GvDetail.EnableHeadersVisualStyles = false;
            this.GvDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GvDetail.Location = new System.Drawing.Point(1, 170);
            this.GvDetail.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.GvDetail.Name = "GvDetail";
            this.GvDetail.ReadOnly = true;
            this.GvDetail.RowHeight_ = 20;
            this.GvDetail.RowTemplate.Height = 20;
            this.GvDetail.Size = new System.Drawing.Size(1060, 460);
            this.GvDetail.TabIndex = 20;
            this.GvDetail.UseRowNo = true;
            this.GvDetail.UseSetting = true;
            this.GvDetail.DoubleClick += new System.EventHandler(this.GvDetail_DoubleClick);
            this.GvDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DgvDetail_KeyDown);
            // 
            // colOrderProcessNO
            // 
            this.colOrderProcessNO.DataPropertyName = "OrderProcessNO";
            this.colOrderProcessNO.HeaderText = "発注処理番号";
            this.colOrderProcessNO.Name = "colOrderProcessNO";
            this.colOrderProcessNO.ReadOnly = true;
            this.colOrderProcessNO.Width = 120;
            // 
            // colInsertDateTime
            // 
            this.colInsertDateTime.DataPropertyName = "InsertDateTime";
            this.colInsertDateTime.HeaderText = "処理日時";
            this.colInsertDateTime.Name = "colInsertDateTime";
            this.colInsertDateTime.ReadOnly = true;
            this.colInsertDateTime.Width = 150;
            // 
            // colStaffName
            // 
            this.colStaffName.DataPropertyName = "StaffName";
            this.colStaffName.HeaderText = "担当スタッフ";
            this.colStaffName.Name = "colStaffName";
            this.colStaffName.ReadOnly = true;
            this.colStaffName.Width = 225;
            // 
            // colOrderWayKBNName
            // 
            this.colOrderWayKBNName.DataPropertyName = "OrderWayKBNName";
            this.colOrderWayKBNName.HeaderText = "発注";
            this.colOrderWayKBNName.Name = "colOrderWayKBNName";
            this.colOrderWayKBNName.ReadOnly = true;
            this.colOrderWayKBNName.Width = 50;
            // 
            // colVendorName
            // 
            this.colVendorName.DataPropertyName = "VendorName";
            this.colVendorName.HeaderText = "仕入先（代表）";
            this.colVendorName.Name = "colVendorName";
            this.colVendorName.ReadOnly = true;
            this.colVendorName.Width = 300;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.DefaultlabelSize = true;
            this.label9.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label9.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(16, 49);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 12);
            this.label9.TabIndex = 285;
            this.label9.Text = "担当スタッフ";
            this.label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label8
            // 
            this.ckM_Label8.AutoSize = true;
            this.ckM_Label8.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label8.DefaultlabelSize = true;
            this.ckM_Label8.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label8.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label8.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label8.Location = new System.Drawing.Point(220, 22);
            this.ckM_Label8.Name = "ckM_Label8";
            this.ckM_Label8.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label8.TabIndex = 288;
            this.ckM_Label8.Text = "～";
            this.ckM_Label8.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_TextBox2
            // 
            this.ckM_TextBox2.AllowMinus = false;
            this.ckM_TextBox2.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox2.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox2.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox2.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.ckM_TextBox2.DecimalPlace = 0;
            this.ckM_TextBox2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox2.IntegerPart = 0;
            this.ckM_TextBox2.IsCorrectDate = true;
            this.ckM_TextBox2.isEnterKeyDown = false;
            this.ckM_TextBox2.IsFirstTime = true;
            this.ckM_TextBox2.isMaxLengthErr = false;
            this.ckM_TextBox2.IsNumber = true;
            this.ckM_TextBox2.IsShop = false;
            this.ckM_TextBox2.Length = 10;
            this.ckM_TextBox2.Location = new System.Drawing.Point(244, 19);
            this.ckM_TextBox2.MaxLength = 10;
            this.ckM_TextBox2.MoveNext = true;
            this.ckM_TextBox2.Name = "ckM_TextBox2";
            this.ckM_TextBox2.Size = new System.Drawing.Size(100, 19);
            this.ckM_TextBox2.TabIndex = 287;
            this.ckM_TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox2.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_TextBox1
            // 
            this.ckM_TextBox1.AllowMinus = false;
            this.ckM_TextBox1.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox1.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox1.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox1.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.ckM_TextBox1.DecimalPlace = 0;
            this.ckM_TextBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox1.IntegerPart = 0;
            this.ckM_TextBox1.IsCorrectDate = true;
            this.ckM_TextBox1.isEnterKeyDown = false;
            this.ckM_TextBox1.IsFirstTime = true;
            this.ckM_TextBox1.isMaxLengthErr = false;
            this.ckM_TextBox1.IsNumber = true;
            this.ckM_TextBox1.IsShop = false;
            this.ckM_TextBox1.Length = 10;
            this.ckM_TextBox1.Location = new System.Drawing.Point(113, 19);
            this.ckM_TextBox1.MaxLength = 10;
            this.ckM_TextBox1.MoveNext = true;
            this.ckM_TextBox1.Name = "ckM_TextBox1";
            this.ckM_TextBox1.Size = new System.Drawing.Size(100, 19);
            this.ckM_TextBox1.TabIndex = 286;
            this.ckM_TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // CboStoreCD
            // 
            this.CboStoreCD.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.CboStoreCD.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CboStoreCD.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.店舗ストア_見積;
            this.CboStoreCD.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.CboStoreCD.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CboStoreCD.Flag = 0;
            this.CboStoreCD.FormattingEnabled = true;
            this.CboStoreCD.Length = 30;
            this.CboStoreCD.Location = new System.Drawing.Point(843, 14);
            this.CboStoreCD.MaxLength = 15;
            this.CboStoreCD.MoveNext = true;
            this.CboStoreCD.Name = "CboStoreCD";
            this.CboStoreCD.Size = new System.Drawing.Size(210, 20);
            this.CboStoreCD.TabIndex = 290;
            // 
            // ckM_Label6
            // 
            this.ckM_Label6.AutoSize = true;
            this.ckM_Label6.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label6.DefaultlabelSize = true;
            this.ckM_Label6.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label6.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label6.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label6.Location = new System.Drawing.Point(781, 19);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label6.TabIndex = 291;
            this.ckM_Label6.Text = "発注店舗";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSubF11
            // 
            this.btnSubF11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnSubF11.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnSubF11.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubF11.DefaultBtnSize = false;
            this.btnSubF11.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSubF11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubF11.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.btnSubF11.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnSubF11.Location = new System.Drawing.Point(938, 84);
            this.btnSubF11.Margin = new System.Windows.Forms.Padding(1);
            this.btnSubF11.Name = "btnSubF11";
            this.btnSubF11.Size = new System.Drawing.Size(115, 28);
            this.btnSubF11.TabIndex = 292;
            this.btnSubF11.Text = "表示(F11)";
            this.btnSubF11.UseVisualStyleBackColor = false;
            this.btnSubF11.Click += new System.EventHandler(this.BtnF11_Click);
            // 
            // ScStaff
            // 
            this.ScStaff.AutoSize = true;
            this.ScStaff.ChangeDate = "";
            this.ScStaff.ChangeDateWidth = 100;
            this.ScStaff.Code = "";
            this.ScStaff.CodeWidth = 70;
            this.ScStaff.CodeWidth1 = 70;
            this.ScStaff.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScStaff.DataCheck = false;
            this.ScStaff.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ScStaff.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ScStaff.IsCopy = false;
            this.ScStaff.LabelText = "";
            this.ScStaff.LabelVisible = true;
            this.ScStaff.Location = new System.Drawing.Point(113, 40);
            this.ScStaff.Margin = new System.Windows.Forms.Padding(0);
            this.ScStaff.Name = "ScStaff";
            this.ScStaff.NameWidth = 250;
            this.ScStaff.SearchEnable = true;
            this.ScStaff.Size = new System.Drawing.Size(354, 28);
            this.ScStaff.Stype = Search.CKM_SearchControl.SearchType.スタッフ;
            this.ScStaff.TabIndex = 289;
            this.ScStaff.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScStaff.UseChangeDate = false;
            this.ScStaff.Value1 = null;
            this.ScStaff.Value2 = null;
            this.ScStaff.Value3 = null;
            // 
            // Search_HacchuuShoriNO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 682);
            this.Controls.Add(this.GvDetail);
            this.F11Visible = true;
            this.F12Visible = true;
            this.F9Visible = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Search_HacchuuShoriNO";
            this.PanelHeaderHeight = 161;
            this.ProgramName = "発注処理番号検索";
            this.Text = "Search_HacchuuNO";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.GvDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_GridView GvDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderProcessNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInsertDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStaffName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderWayKBNName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVendorName;
        private CKM_Controls.CKM_Label label9;
        private CKM_Controls.CKM_Label ckM_Label8;
        private CKM_Controls.CKM_TextBox ckM_TextBox2;
        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private CKM_SearchControl ScStaff;
        private CKM_Controls.CKM_ComboBox CboStoreCD;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_Button btnSubF11;
    }
}

