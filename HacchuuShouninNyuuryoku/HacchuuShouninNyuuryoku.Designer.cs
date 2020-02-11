namespace HacchuuShouninNyuuryoku
{
    partial class HacchuuShouninNyuuryoku
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label4 = new CKM_Controls.CKM_Label();
            this.label11 = new CKM_Controls.CKM_Label();
            this.label1 = new CKM_Controls.CKM_Label();
            this.label27 = new CKM_Controls.CKM_Label();
            this.CboStoreCD = new CKM_Controls.CKM_ComboBox();
            this.ckM_SearchControl3 = new Search.CKM_SearchControl();
            this.ckM_TextBox1 = new CKM_Controls.CKM_TextBox();
            this.BtnSubF11 = new CKM_Controls.CKM_Button();
            this.ckM_SearchControl1 = new Search.CKM_SearchControl();
            this.ckM_CheckBox1 = new CKM_Controls.CKM_CheckBox();
            this.ckM_CheckBox2 = new CKM_Controls.CKM_CheckBox();
            this.label12 = new CKM_Controls.CKM_Label();
            this.ckM_CheckBox3 = new CKM_Controls.CKM_CheckBox();
            this.ckM_TextBox2 = new CKM_Controls.CKM_TextBox();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.GvDetail = new CKM_Controls.CKM_GridView();
            this.BtnSyonin = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colOrderNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.coIOrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colApprovalStageFLG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastApprovalDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastApprovalStaffName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Store = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StaffName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVendorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PanelHeader.SuspendLayout();
            this.PanelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Controls.Add(this.ckM_CheckBox3);
            this.PanelHeader.Controls.Add(this.ckM_CheckBox2);
            this.PanelHeader.Controls.Add(this.ckM_CheckBox1);
            this.PanelHeader.Controls.Add(this.ckM_TextBox2);
            this.PanelHeader.Controls.Add(this.CboStoreCD);
            this.PanelHeader.Controls.Add(this.label4);
            this.PanelHeader.Controls.Add(this.label12);
            this.PanelHeader.Controls.Add(this.ckM_TextBox1);
            this.PanelHeader.Controls.Add(this.label11);
            this.PanelHeader.Size = new System.Drawing.Size(1368, 94);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.TabStop = true;
            this.PanelHeader.Controls.SetChildIndex(this.label11, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.label12, 0);
            this.PanelHeader.Controls.SetChildIndex(this.label4, 0);
            this.PanelHeader.Controls.SetChildIndex(this.CboStoreCD, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_CheckBox1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_CheckBox2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_CheckBox3, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Controls.Add(this.BtnSubF11);
            this.PanelSearch.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.DefaultlabelSize = true;
            this.label4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label4.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(79, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 12);
            this.label4.TabIndex = 255;
            this.label4.Text = "店舗";
            this.label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.DefaultlabelSize = true;
            this.label11.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label11.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(66, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 12);
            this.label11.TabIndex = 261;
            this.label11.Text = "発注日";
            this.label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label1.BackColor = System.Drawing.Color.DarkGray;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.DefaultlabelSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(28, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(492, 18);
            this.label1.TabIndex = 261;
            this.label1.Text = "商品名";
            this.label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label27.BackColor = System.Drawing.Color.Transparent;
            this.label27.DefaultlabelSize = true;
            this.label27.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label27.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label27.ForeColor = System.Drawing.Color.Black;
            this.label27.Location = new System.Drawing.Point(257, 13);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(83, 12);
            this.label27.TabIndex = 341;
            this.label27.Text = "複写見積番号";
            this.label27.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CboStoreCD
            // 
            this.CboStoreCD.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.CboStoreCD.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CboStoreCD.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.店舗ストア_見積;
            this.CboStoreCD.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.CboStoreCD.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CboStoreCD.FormattingEnabled = true;
            this.CboStoreCD.Length = 40;
            this.CboStoreCD.Location = new System.Drawing.Point(115, 6);
            this.CboStoreCD.MaxLength = 40;
            this.CboStoreCD.MoveNext = true;
            this.CboStoreCD.Name = "CboStoreCD";
            this.CboStoreCD.Size = new System.Drawing.Size(280, 20);
            this.CboStoreCD.TabIndex = 0;
            // 
            // ckM_SearchControl3
            // 
            this.ckM_SearchControl3.AutoSize = true;
            this.ckM_SearchControl3.ChangeDate = "";
            this.ckM_SearchControl3.ChangeDateWidth = 100;
            this.ckM_SearchControl3.Code = "";
            this.ckM_SearchControl3.CodeWidth = 100;
            this.ckM_SearchControl3.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_SearchControl3.DataCheck = false;
            this.ckM_SearchControl3.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ckM_SearchControl3.IsCopy = false;
            this.ckM_SearchControl3.LabelText = "";
            this.ckM_SearchControl3.LabelVisible = false;
            this.ckM_SearchControl3.Location = new System.Drawing.Point(343, 4);
            this.ckM_SearchControl3.Name = "ckM_SearchControl3";
            this.ckM_SearchControl3.SearchEnable = true;
            this.ckM_SearchControl3.Size = new System.Drawing.Size(133, 28);
            this.ckM_SearchControl3.Stype = Search.CKM_SearchControl.SearchType.見積番号;
            this.ckM_SearchControl3.TabIndex = 344;
            this.ckM_SearchControl3.UseChangeDate = false;
            this.ckM_SearchControl3.Value1 = null;
            this.ckM_SearchControl3.Value2 = null;
            this.ckM_SearchControl3.Value3 = null;
            // 
            // ckM_TextBox1
            // 
            this.ckM_TextBox1.AllowMinus = false;
            this.ckM_TextBox1.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox1.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox1.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.ckM_TextBox1.DecimalPlace = 2;
            this.ckM_TextBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox1.IntegerPart = 0;
            this.ckM_TextBox1.IsCorrectDate = true;
            this.ckM_TextBox1.isEnterKeyDown = false;
            this.ckM_TextBox1.IsNumber = true;
            this.ckM_TextBox1.IsShop = false;
            this.ckM_TextBox1.Length = 10;
            this.ckM_TextBox1.Location = new System.Drawing.Point(116, 32);
            this.ckM_TextBox1.MaxLength = 10;
            this.ckM_TextBox1.MoveNext = true;
            this.ckM_TextBox1.Name = "ckM_TextBox1";
            this.ckM_TextBox1.Size = new System.Drawing.Size(88, 19);
            this.ckM_TextBox1.TabIndex = 1;
            this.ckM_TextBox1.Text = "2019/01/01";
            this.ckM_TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // BtnSubF11
            // 
            this.BtnSubF11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BtnSubF11.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BtnSubF11.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSubF11.DefaultBtnSize = true;
            this.BtnSubF11.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnSubF11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSubF11.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.BtnSubF11.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BtnSubF11.Location = new System.Drawing.Point(375, 0);
            this.BtnSubF11.Margin = new System.Windows.Forms.Padding(1);
            this.BtnSubF11.Name = "BtnSubF11";
            this.BtnSubF11.Size = new System.Drawing.Size(118, 28);
            this.BtnSubF11.TabIndex = 10;
            this.BtnSubF11.Text = "表示(F11)";
            this.BtnSubF11.UseVisualStyleBackColor = false;
            this.BtnSubF11.Click += new System.EventHandler(this.BtnF11_Click);
            // 
            // ckM_SearchControl1
            // 
            this.ckM_SearchControl1.AutoSize = true;
            this.ckM_SearchControl1.ChangeDate = "";
            this.ckM_SearchControl1.ChangeDateWidth = 100;
            this.ckM_SearchControl1.Code = "";
            this.ckM_SearchControl1.CodeWidth = 100;
            this.ckM_SearchControl1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_SearchControl1.DataCheck = false;
            this.ckM_SearchControl1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ckM_SearchControl1.IsCopy = false;
            this.ckM_SearchControl1.LabelText = "";
            this.ckM_SearchControl1.LabelVisible = false;
            this.ckM_SearchControl1.Location = new System.Drawing.Point(577, 3);
            this.ckM_SearchControl1.Name = "ckM_SearchControl1";
            this.ckM_SearchControl1.SearchEnable = true;
            this.ckM_SearchControl1.Size = new System.Drawing.Size(133, 28);
            this.ckM_SearchControl1.Stype = Search.CKM_SearchControl.SearchType.見積番号;
            this.ckM_SearchControl1.TabIndex = 694;
            this.ckM_SearchControl1.UseChangeDate = false;
            this.ckM_SearchControl1.Value1 = null;
            this.ckM_SearchControl1.Value2 = null;
            this.ckM_SearchControl1.Value3 = null;
            // 
            // ckM_CheckBox1
            // 
            this.ckM_CheckBox1.AutoSize = true;
            this.ckM_CheckBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_CheckBox1.Location = new System.Drawing.Point(129, 66);
            this.ckM_CheckBox1.Name = "ckM_CheckBox1";
            this.ckM_CheckBox1.Size = new System.Drawing.Size(63, 16);
            this.ckM_CheckBox1.TabIndex = 3;
            this.ckM_CheckBox1.Text = "未承認";
            this.ckM_CheckBox1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ckM_CheckBox1.UseVisualStyleBackColor = true;
            // 
            // ckM_CheckBox2
            // 
            this.ckM_CheckBox2.AutoSize = true;
            this.ckM_CheckBox2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_CheckBox2.Location = new System.Drawing.Point(212, 66);
            this.ckM_CheckBox2.Name = "ckM_CheckBox2";
            this.ckM_CheckBox2.Size = new System.Drawing.Size(63, 16);
            this.ckM_CheckBox2.TabIndex = 4;
            this.ckM_CheckBox2.Text = "承認済";
            this.ckM_CheckBox2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ckM_CheckBox2.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.DefaultlabelSize = true;
            this.label12.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label12.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(215, 35);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(18, 12);
            this.label12.TabIndex = 712;
            this.label12.Text = "～";
            this.label12.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_CheckBox3
            // 
            this.ckM_CheckBox3.AutoSize = true;
            this.ckM_CheckBox3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_CheckBox3.Location = new System.Drawing.Point(295, 66);
            this.ckM_CheckBox3.Name = "ckM_CheckBox3";
            this.ckM_CheckBox3.Size = new System.Drawing.Size(50, 16);
            this.ckM_CheckBox3.TabIndex = 5;
            this.ckM_CheckBox3.Text = "却下";
            this.ckM_CheckBox3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ckM_CheckBox3.UseVisualStyleBackColor = true;
            // 
            // ckM_TextBox2
            // 
            this.ckM_TextBox2.AllowMinus = false;
            this.ckM_TextBox2.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox2.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox2.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.ckM_TextBox2.DecimalPlace = 2;
            this.ckM_TextBox2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox2.IntegerPart = 0;
            this.ckM_TextBox2.IsCorrectDate = true;
            this.ckM_TextBox2.isEnterKeyDown = false;
            this.ckM_TextBox2.IsNumber = true;
            this.ckM_TextBox2.IsShop = false;
            this.ckM_TextBox2.Length = 10;
            this.ckM_TextBox2.Location = new System.Drawing.Point(244, 32);
            this.ckM_TextBox2.MaxLength = 10;
            this.ckM_TextBox2.MoveNext = true;
            this.ckM_TextBox2.Name = "ckM_TextBox2";
            this.ckM_TextBox2.Size = new System.Drawing.Size(88, 19);
            this.ckM_TextBox2.TabIndex = 2;
            this.ckM_TextBox2.Text = "2019/01/01";
            this.ckM_TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox2.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_Label1.Location = new System.Drawing.Point(79, 68);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label1.TabIndex = 721;
            this.ckM_Label1.Text = "承認";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.BtnSyonin,
            this.colOrderNO,
            this.coIOrderDate,
            this.colApprovalStageFLG,
            this.colLastApprovalDate,
            this.colLastApprovalStaffName,
            this.Store,
            this.StaffName,
            this.colVendorName,
            this.colSKUName});
            this.GvDetail.EnableHeadersVisualStyles = false;
            this.GvDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GvDetail.Location = new System.Drawing.Point(12, 166);
            this.GvDetail.Name = "GvDetail";
            this.GvDetail.Size = new System.Drawing.Size(1343, 538);
            this.GvDetail.TabIndex = 20;
            this.GvDetail.UseRowNo = true;
            this.GvDetail.UseSetting = true;
            this.GvDetail.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvDetail_CellContentClick);
            this.GvDetail.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.GvDetail_CellFormatting);
            // 
            // BtnSyonin
            // 
            this.BtnSyonin.HeaderText = " ";
            this.BtnSyonin.Name = "BtnSyonin";
            this.BtnSyonin.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.BtnSyonin.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.BtnSyonin.Width = 40;
            // 
            // colOrderNO
            // 
            this.colOrderNO.DataPropertyName = "OrderNO";
            this.colOrderNO.HeaderText = "発注番号";
            this.colOrderNO.Name = "colOrderNO";
            this.colOrderNO.ReadOnly = true;
            // 
            // coIOrderDate
            // 
            this.coIOrderDate.DataPropertyName = "OrderDate";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.coIOrderDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.coIOrderDate.HeaderText = "発注日";
            this.coIOrderDate.Name = "coIOrderDate";
            this.coIOrderDate.Width = 80;
            // 
            // colApprovalStageFLG
            // 
            this.colApprovalStageFLG.DataPropertyName = "ApprovalStageFLG";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colApprovalStageFLG.DefaultCellStyle = dataGridViewCellStyle4;
            this.colApprovalStageFLG.HeaderText = "状況";
            this.colApprovalStageFLG.Name = "colApprovalStageFLG";
            this.colApprovalStageFLG.ReadOnly = true;
            this.colApprovalStageFLG.Width = 80;
            // 
            // colLastApprovalDate
            // 
            this.colLastApprovalDate.DataPropertyName = "LastApprovalDate";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colLastApprovalDate.DefaultCellStyle = dataGridViewCellStyle5;
            this.colLastApprovalDate.HeaderText = "最新承認日";
            this.colLastApprovalDate.Name = "colLastApprovalDate";
            // 
            // colLastApprovalStaffName
            // 
            this.colLastApprovalStaffName.DataPropertyName = "LastApprovalStaffName";
            this.colLastApprovalStaffName.HeaderText = "最新承認者";
            this.colLastApprovalStaffName.Name = "colLastApprovalStaffName";
            // 
            // Store
            // 
            this.Store.DataPropertyName = "StoreName";
            this.Store.HeaderText = "店舗";
            this.Store.Name = "Store";
            // 
            // StaffName
            // 
            this.StaffName.DataPropertyName = "StaffName";
            this.StaffName.HeaderText = "申請者";
            this.StaffName.Name = "StaffName";
            // 
            // colVendorName
            // 
            this.colVendorName.DataPropertyName = "VendorName";
            this.colVendorName.HeaderText = "仕入先";
            this.colVendorName.Name = "colVendorName";
            this.colVendorName.ReadOnly = true;
            this.colVendorName.Width = 300;
            // 
            // colSKUName
            // 
            this.colSKUName.DataPropertyName = "SKUName";
            this.colSKUName.HeaderText = "商品";
            this.colSKUName.Name = "colSKUName";
            this.colSKUName.ReadOnly = true;
            this.colSKUName.Width = 300;
            // 
            // HacchuuShouninNyuuryoku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.GvDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "HacchuuShouninNyuuryoku";
            this.PanelHeaderHeight = 150;
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.GvDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            this.PanelSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private CKM_Controls.CKM_Label label4;
        private CKM_Controls.CKM_Label label11;
        private CKM_Controls.CKM_Label label27;
        private CKM_Controls.CKM_Label label1;
        private CKM_Controls.CKM_ComboBox CboStoreCD;
        private Search.CKM_SearchControl ckM_SearchControl3;
        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private Search.CKM_SearchControl ckM_SearchControl1;
        private CKM_Controls.CKM_Button BtnSubF11;
        private CKM_Controls.CKM_CheckBox ckM_CheckBox1;
        private CKM_Controls.CKM_CheckBox ckM_CheckBox2;
        private CKM_Controls.CKM_Label label12;
        private CKM_Controls.CKM_CheckBox ckM_CheckBox3;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_TextBox ckM_TextBox2;
        private CKM_Controls.CKM_GridView GvDetail;
        private System.Windows.Forms.DataGridViewButtonColumn BtnSyonin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn coIOrderDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colApprovalStageFLG;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastApprovalDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastApprovalStaffName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Store;
        private System.Windows.Forms.DataGridViewTextBoxColumn StaffName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVendorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUName;
    }
}

