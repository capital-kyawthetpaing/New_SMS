namespace Search
{
    partial class Search_EDIHacchuuNO
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.CboStoreCD = new CKM_Controls.CKM_ComboBox();
            this.GvDetail = new CKM_Controls.CKM_GridView();
            this.colEDIOrderNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVendorCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ckM_TextBox1 = new CKM_Controls.CKM_TextBox();
            this.ckM_Label8 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox2 = new CKM_Controls.CKM_TextBox();
            this.btnSubF11 = new CKM_Controls.CKM_Button();
            this.ScVendor = new Search.CKM_SearchControl();
            this.label3 = new System.Windows.Forms.Label();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.ScVendor);
            this.PanelHeader.Controls.Add(this.label3);
            this.PanelHeader.Controls.Add(this.btnSubF11);
            this.PanelHeader.Controls.Add(this.ckM_Label8);
            this.PanelHeader.Controls.Add(this.ckM_TextBox2);
            this.PanelHeader.Controls.Add(this.CboStoreCD);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Controls.Add(this.ckM_Label6);
            this.PanelHeader.Controls.Add(this.ckM_TextBox1);
            this.PanelHeader.Size = new System.Drawing.Size(1062, 128);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label6, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.CboStoreCD, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label8, 0);
            this.PanelHeader.Controls.SetChildIndex(this.btnSubF11, 0);
            this.PanelHeader.Controls.SetChildIndex(this.label3, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ScVendor, 0);
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
            this.ckM_Label1.Location = new System.Drawing.Point(37, 15);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(65, 12);
            this.ckM_Label1.TabIndex = 2;
            this.ckM_Label1.Text = "EDI処理日";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label6.Location = new System.Drawing.Point(747, 15);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label6.TabIndex = 57;
            this.ckM_Label6.Text = "店舗";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CboStoreCD
            // 
            this.CboStoreCD.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.CboStoreCD.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CboStoreCD.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.店舗ストア_受注;
            this.CboStoreCD.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.CboStoreCD.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CboStoreCD.FormattingEnabled = true;
            this.CboStoreCD.Length = 30;
            this.CboStoreCD.Location = new System.Drawing.Point(782, 11);
            this.CboStoreCD.MaxLength = 15;
            this.CboStoreCD.MoveNext = true;
            this.CboStoreCD.Name = "CboStoreCD";
            this.CboStoreCD.Size = new System.Drawing.Size(210, 20);
            this.CboStoreCD.TabIndex = 2;
            // 
            // GvDetail
            // 
            this.GvDetail.AllowUserToAddRows = false;
            this.GvDetail.AllowUserToDeleteRows = false;
            this.GvDetail.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.GvDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.GvDetail.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GvDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.GvDetail.ColumnHeadersHeight = 25;
            this.GvDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colEDIOrderNO,
            this.colOrderDate,
            this.colVendorCD,
            this.colCustomerName,
            this.colSKUName});
            this.GvDetail.EnableHeadersVisualStyles = false;
            this.GvDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GvDetail.Location = new System.Drawing.Point(9, 179);
            this.GvDetail.Name = "GvDetail";
            this.GvDetail.ReadOnly = true;
            this.GvDetail.Size = new System.Drawing.Size(992, 369);
            this.GvDetail.TabIndex = 2;
            this.GvDetail.UseRowNo = true;
            this.GvDetail.UseSetting = true;
            this.GvDetail.DoubleClick += new System.EventHandler(this.GvDetail_DoubleClick);
            this.GvDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DgvDetail_KeyDown);
            // 
            // colEDIOrderNO
            // 
            this.colEDIOrderNO.DataPropertyName = "EDIOrderNO";
            this.colEDIOrderNO.HeaderText = "EDI処理番号";
            this.colEDIOrderNO.Name = "colEDIOrderNO";
            this.colEDIOrderNO.ReadOnly = true;
            // 
            // colOrderDate
            // 
            this.colOrderDate.DataPropertyName = "OrderDate";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colOrderDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.colOrderDate.HeaderText = "EDI処理日";
            this.colOrderDate.Name = "colOrderDate";
            this.colOrderDate.ReadOnly = true;
            // 
            // colVendorCD
            // 
            this.colVendorCD.DataPropertyName = "VendorCD";
            this.colVendorCD.HeaderText = "仕入先";
            this.colVendorCD.Name = "colVendorCD";
            this.colVendorCD.ReadOnly = true;
            // 
            // colCustomerName
            // 
            this.colCustomerName.DataPropertyName = "VendorName";
            this.colCustomerName.HeaderText = "　";
            this.colCustomerName.Name = "colCustomerName";
            this.colCustomerName.ReadOnly = true;
            this.colCustomerName.Width = 300;
            // 
            // colSKUName
            // 
            this.colSKUName.DataPropertyName = "SKUName";
            this.colSKUName.HeaderText = "商品";
            this.colSKUName.Name = "colSKUName";
            this.colSKUName.ReadOnly = true;
            this.colSKUName.Width = 350;
            // 
            // ckM_TextBox1
            // 
            this.ckM_TextBox1.AllowMinus = false;
            this.ckM_TextBox1.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox1.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox1.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.ckM_TextBox1.DecimalPlace = 0;
            this.ckM_TextBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox1.IntegerPart = 0;
            this.ckM_TextBox1.IsCorrectDate = true;
            this.ckM_TextBox1.isEnterKeyDown = false;
            this.ckM_TextBox1.IsNumber = true;
            this.ckM_TextBox1.IsShop = false;
            this.ckM_TextBox1.Length = 10;
            this.ckM_TextBox1.Location = new System.Drawing.Point(106, 12);
            this.ckM_TextBox1.MaxLength = 10;
            this.ckM_TextBox1.MoveNext = true;
            this.ckM_TextBox1.Name = "ckM_TextBox1";
            this.ckM_TextBox1.Size = new System.Drawing.Size(100, 19);
            this.ckM_TextBox1.TabIndex = 0;
            this.ckM_TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_Label8.Location = new System.Drawing.Point(212, 15);
            this.ckM_Label8.Name = "ckM_Label8";
            this.ckM_Label8.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label8.TabIndex = 61;
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
            this.ckM_TextBox2.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.ckM_TextBox2.DecimalPlace = 0;
            this.ckM_TextBox2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox2.IntegerPart = 0;
            this.ckM_TextBox2.IsCorrectDate = true;
            this.ckM_TextBox2.isEnterKeyDown = false;
            this.ckM_TextBox2.IsNumber = true;
            this.ckM_TextBox2.IsShop = false;
            this.ckM_TextBox2.Length = 10;
            this.ckM_TextBox2.Location = new System.Drawing.Point(237, 12);
            this.ckM_TextBox2.MaxLength = 10;
            this.ckM_TextBox2.MoveNext = true;
            this.ckM_TextBox2.Name = "ckM_TextBox2";
            this.ckM_TextBox2.Size = new System.Drawing.Size(100, 19);
            this.ckM_TextBox2.TabIndex = 1;
            this.ckM_TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox2.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.btnSubF11.Location = new System.Drawing.Point(925, 81);
            this.btnSubF11.Margin = new System.Windows.Forms.Padding(1);
            this.btnSubF11.Name = "btnSubF11";
            this.btnSubF11.Size = new System.Drawing.Size(115, 28);
            this.btnSubF11.TabIndex = 6;
            this.btnSubF11.Text = "表示(F11)";
            this.btnSubF11.UseVisualStyleBackColor = false;
            this.btnSubF11.Click += new System.EventHandler(this.BtnF11_Click);
            // 
            // ScVendor
            // 
            this.ScVendor.AutoSize = true;
            this.ScVendor.ChangeDate = "";
            this.ScVendor.ChangeDateWidth = 100;
            this.ScVendor.Code = "";
            this.ScVendor.CodeWidth = 130;
            this.ScVendor.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScVendor.DataCheck = true;
            this.ScVendor.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ScVendor.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ScVendor.IsCopy = false;
            this.ScVendor.LabelText = "";
            this.ScVendor.LabelVisible = true;
            this.ScVendor.Location = new System.Drawing.Point(106, 47);
            this.ScVendor.Name = "ScVendor";
            this.ScVendor.SearchEnable = true;
            this.ScVendor.Size = new System.Drawing.Size(444, 28);
            this.ScVendor.Stype = Search.CKM_SearchControl.SearchType.仕入先;
            this.ScVendor.TabIndex = 749;
            this.ScVendor.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScVendor.UseChangeDate = false;
            this.ScVendor.Value1 = null;
            this.ScVendor.Value2 = null;
            this.ScVendor.Value3 = null;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(56, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 12);
            this.label3.TabIndex = 750;
            this.label3.Text = "仕入先";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Search_EDIHacchuuNO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 591);
            this.Controls.Add(this.GvDetail);
            this.F11Visible = true;
            this.F9Visible = true;
            this.Name = "Search_EDIHacchuuNO";
            this.PanelHeaderHeight = 170;
            this.ProgramName = "EDI処理番号検索";
            this.Text = "Search_EDIHacchuuNO";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.GvDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_ComboBox CboStoreCD;
        private CKM_Controls.CKM_GridView GvDetail;
        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private CKM_Controls.CKM_Label ckM_Label8;
        private CKM_Controls.CKM_TextBox ckM_TextBox2;
        private CKM_Controls.CKM_Button btnSubF11;
        private CKM_SearchControl ScVendor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEDIOrderNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVendorCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUName;
    }
}

