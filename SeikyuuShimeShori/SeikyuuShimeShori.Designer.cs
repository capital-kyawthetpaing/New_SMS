namespace SeikyuuShimeShori
{
    partial class SeikyuuShimeShori
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label4 = new CKM_Controls.CKM_Label();
            this.label1 = new CKM_Controls.CKM_Label();
            this.label27 = new CKM_Controls.CKM_Label();
            this.CboStoreCD = new CKM_Controls.CKM_ComboBox();
            this.ckM_SearchControl2 = new Search.CKM_SearchControl();
            this.ckM_SearchControl3 = new Search.CKM_SearchControl();
            this.ckM_CustomerName = new CKM_Controls.CKM_TextBox();
            this.label9 = new CKM_Controls.CKM_Label();
            this.lblSkuCD = new System.Windows.Forms.Label();
            this.ckM_TextBox2 = new CKM_Controls.CKM_TextBox();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox1 = new CKM_Controls.CKM_TextBox();
            this.ScCustomer = new Search.CKM_SearchControl();
            this.GvDetail = new CKM_Controls.CKM_GridView();
            this.colProcessingDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBillingDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProcessingKBN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cboSyori = new CKM_Controls.CKM_ComboBox();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.cboSyori);
            this.PanelHeader.Controls.Add(this.CboStoreCD);
            this.PanelHeader.Controls.Add(this.label4);
            this.PanelHeader.Controls.Add(this.ScCustomer);
            this.PanelHeader.Controls.Add(this.ckM_CustomerName);
            this.PanelHeader.Controls.Add(this.ckM_TextBox1);
            this.PanelHeader.Controls.Add(this.ckM_Label4);
            this.PanelHeader.Controls.Add(this.label9);
            this.PanelHeader.Controls.Add(this.ckM_Label2);
            this.PanelHeader.Controls.Add(this.lblSkuCD);
            this.PanelHeader.Controls.Add(this.ckM_TextBox2);
            this.PanelHeader.Size = new System.Drawing.Size(1368, 144);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblSkuCD, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.label9, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label4, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_CustomerName, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ScCustomer, 0);
            this.PanelHeader.Controls.SetChildIndex(this.label4, 0);
            this.PanelHeader.Controls.SetChildIndex(this.CboStoreCD, 0);
            this.PanelHeader.Controls.SetChildIndex(this.cboSyori, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.TabIndex = 7;
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
            this.label4.Location = new System.Drawing.Point(1011, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 12);
            this.label4.TabIndex = 255;
            this.label4.Text = "店舗";
            this.label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.CboStoreCD.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.店舗ストア_受注;
            this.CboStoreCD.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.CboStoreCD.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CboStoreCD.FormattingEnabled = true;
            this.CboStoreCD.Length = 40;
            this.CboStoreCD.Location = new System.Drawing.Point(1062, 15);
            this.CboStoreCD.MaxLength = 20;
            this.CboStoreCD.MoveNext = true;
            this.CboStoreCD.Name = "CboStoreCD";
            this.CboStoreCD.Size = new System.Drawing.Size(280, 20);
            this.CboStoreCD.TabIndex = 0;
            this.CboStoreCD.SelectedIndexChanged += new System.EventHandler(this.CboStoreCD_SelectedIndexChanged);
            // 
            // ckM_SearchControl2
            // 
            this.ckM_SearchControl2.AutoSize = true;
            this.ckM_SearchControl2.ChangeDate = "";
            this.ckM_SearchControl2.ChangeDateWidth = 100;
            this.ckM_SearchControl2.Code = "";
            this.ckM_SearchControl2.CodeWidth = 100;
            this.ckM_SearchControl2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_SearchControl2.DataCheck = false;
            this.ckM_SearchControl2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_SearchControl2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ckM_SearchControl2.IsCopy = false;
            this.ckM_SearchControl2.LabelText = "";
            this.ckM_SearchControl2.LabelVisible = false;
            this.ckM_SearchControl2.Location = new System.Drawing.Point(343, 5);
            this.ckM_SearchControl2.Name = "ckM_SearchControl2";
            this.ckM_SearchControl2.SearchEnable = true;
            this.ckM_SearchControl2.Size = new System.Drawing.Size(133, 28);
            this.ckM_SearchControl2.Stype = Search.CKM_SearchControl.SearchType.見積番号;
            this.ckM_SearchControl2.TabIndex = 275;
            this.ckM_SearchControl2.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ckM_SearchControl2.UseChangeDate = false;
            this.ckM_SearchControl2.Value1 = null;
            this.ckM_SearchControl2.Value2 = null;
            this.ckM_SearchControl2.Value3 = null;
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
            this.ckM_SearchControl3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
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
            this.ckM_SearchControl3.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ckM_SearchControl3.UseChangeDate = false;
            this.ckM_SearchControl3.Value1 = null;
            this.ckM_SearchControl3.Value2 = null;
            this.ckM_SearchControl3.Value3 = null;
            // 
            // ckM_CustomerName
            // 
            this.ckM_CustomerName.AllowMinus = false;
            this.ckM_CustomerName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_CustomerName.BackColor = System.Drawing.Color.White;
            this.ckM_CustomerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_CustomerName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.ckM_CustomerName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_CustomerName.DecimalPlace = 0;
            this.ckM_CustomerName.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_CustomerName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.ckM_CustomerName.IntegerPart = 0;
            this.ckM_CustomerName.IsCorrectDate = true;
            this.ckM_CustomerName.isEnterKeyDown = false;
            this.ckM_CustomerName.IsNumber = true;
            this.ckM_CustomerName.IsShop = false;
            this.ckM_CustomerName.Length = 80;
            this.ckM_CustomerName.Location = new System.Drawing.Point(236, 89);
            this.ckM_CustomerName.MaxLength = 40;
            this.ckM_CustomerName.MoveNext = true;
            this.ckM_CustomerName.Name = "ckM_CustomerName";
            this.ckM_CustomerName.Size = new System.Drawing.Size(520, 19);
            this.ckM_CustomerName.TabIndex = 5;
            this.ckM_CustomerName.Text = "ＸＸＸＸＸＸＸＸＸ10ＸＸＸＸＸＸＸＸＸ20ＸＸＸＸＸＸＸＸＸ30ＸＸＸＸＸＸＸＸＸ40";
            this.ckM_CustomerName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.ckM_CustomerName.Visible = false;
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
            this.label9.Location = new System.Drawing.Point(43, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 12);
            this.label9.TabIndex = 705;
            this.label9.Text = "請求締日";
            this.label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSkuCD
            // 
            this.lblSkuCD.AutoSize = true;
            this.lblSkuCD.BackColor = System.Drawing.Color.Transparent;
            this.lblSkuCD.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSkuCD.Location = new System.Drawing.Point(69, 92);
            this.lblSkuCD.Name = "lblSkuCD";
            this.lblSkuCD.Size = new System.Drawing.Size(31, 12);
            this.lblSkuCD.TabIndex = 704;
            this.lblSkuCD.Text = "顧客";
            this.lblSkuCD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_TextBox2
            // 
            this.ckM_TextBox2.AllowMinus = false;
            this.ckM_TextBox2.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox2.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox2.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Number;
            this.ckM_TextBox2.DecimalPlace = 0;
            this.ckM_TextBox2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox2.ForeColor = System.Drawing.Color.Black;
            this.ckM_TextBox2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox2.IntegerPart = 2;
            this.ckM_TextBox2.IsCorrectDate = true;
            this.ckM_TextBox2.isEnterKeyDown = false;
            this.ckM_TextBox2.IsNumber = true;
            this.ckM_TextBox2.IsShop = false;
            this.ckM_TextBox2.Length = 2;
            this.ckM_TextBox2.Location = new System.Drawing.Point(105, 64);
            this.ckM_TextBox2.MaxLength = 2;
            this.ckM_TextBox2.MoveNext = true;
            this.ckM_TextBox2.Name = "ckM_TextBox2";
            this.ckM_TextBox2.Size = new System.Drawing.Size(20, 19);
            this.ckM_TextBox2.TabIndex = 3;
            this.ckM_TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ckM_TextBox2.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_Label2
            // 
            this.ckM_Label2.AutoSize = true;
            this.ckM_Label2.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label2.DefaultlabelSize = true;
            this.ckM_Label2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label2.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label2.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label2.Location = new System.Drawing.Point(69, 18);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label2.TabIndex = 690;
            this.ckM_Label2.Text = "処理";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label4
            // 
            this.ckM_Label4.AutoSize = true;
            this.ckM_Label4.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label4.DefaultlabelSize = true;
            this.ckM_Label4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label4.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label4.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label4.Location = new System.Drawing.Point(43, 66);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label4.TabIndex = 701;
            this.ckM_Label4.Text = "対象締日";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_TextBox1.ForeColor = System.Drawing.Color.Black;
            this.ckM_TextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox1.IntegerPart = 0;
            this.ckM_TextBox1.IsCorrectDate = true;
            this.ckM_TextBox1.isEnterKeyDown = false;
            this.ckM_TextBox1.IsNumber = true;
            this.ckM_TextBox1.IsShop = false;
            this.ckM_TextBox1.Length = 10;
            this.ckM_TextBox1.Location = new System.Drawing.Point(105, 40);
            this.ckM_TextBox1.MaxLength = 10;
            this.ckM_TextBox1.MoveNext = true;
            this.ckM_TextBox1.Name = "ckM_TextBox1";
            this.ckM_TextBox1.Size = new System.Drawing.Size(100, 19);
            this.ckM_TextBox1.TabIndex = 2;
            this.ckM_TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ScCustomer
            // 
            this.ScCustomer.AutoSize = true;
            this.ScCustomer.ChangeDate = "";
            this.ScCustomer.ChangeDateWidth = 100;
            this.ScCustomer.Code = "";
            this.ScCustomer.CodeWidth = 100;
            this.ScCustomer.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScCustomer.DataCheck = true;
            this.ScCustomer.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ScCustomer.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ScCustomer.IsCopy = false;
            this.ScCustomer.LabelText = "";
            this.ScCustomer.LabelVisible = true;
            this.ScCustomer.Location = new System.Drawing.Point(105, 84);
            this.ScCustomer.Name = "ScCustomer";
            this.ScCustomer.SearchEnable = true;
            this.ScCustomer.Size = new System.Drawing.Size(634, 28);
            this.ScCustomer.Stype = Search.CKM_SearchControl.SearchType.得意先;
            this.ScCustomer.TabIndex = 4;
            this.ScCustomer.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScCustomer.UseChangeDate = false;
            this.ScCustomer.Value1 = null;
            this.ScCustomer.Value2 = null;
            this.ScCustomer.Value3 = null;
            // 
            // GvDetail
            // 
            this.GvDetail.AllowUserToAddRows = false;
            this.GvDetail.AllowUserToDeleteRows = false;
            this.GvDetail.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.GvDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.GvDetail.AutoGenerateColumns = false;
            this.GvDetail.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GvDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.GvDetail.ColumnHeadersHeight = 25;
            this.GvDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProcessingDateTime,
            this.colBillingDate,
            this.colCustomer,
            this.colCustomerName,
            this.colProcessingKBN,
            this.colSKUName});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.GvDetail.DefaultCellStyle = dataGridViewCellStyle4;
            this.GvDetail.EnableHeadersVisualStyles = false;
            this.GvDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GvDetail.Location = new System.Drawing.Point(46, 206);
            this.GvDetail.Name = "GvDetail";
            this.GvDetail.ReadOnly = true;
            this.GvDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GvDetail.Size = new System.Drawing.Size(963, 468);
            this.GvDetail.TabIndex = 2;
            this.GvDetail.UseRowNo = true;
            this.GvDetail.UseSetting = true;
            // 
            // colProcessingDateTime
            // 
            this.colProcessingDateTime.DataPropertyName = "ProcessingDateTime";
            this.colProcessingDateTime.HeaderText = "処理日時";
            this.colProcessingDateTime.Name = "colProcessingDateTime";
            this.colProcessingDateTime.ReadOnly = true;
            this.colProcessingDateTime.Width = 120;
            // 
            // colBillingDate
            // 
            this.colBillingDate.DataPropertyName = "BillingDate";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colBillingDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.colBillingDate.HeaderText = "締年月日";
            this.colBillingDate.Name = "colBillingDate";
            this.colBillingDate.ReadOnly = true;
            this.colBillingDate.Width = 80;
            // 
            // colCustomer
            // 
            this.colCustomer.DataPropertyName = "CustomerCD";
            this.colCustomer.HeaderText = "顧客";
            this.colCustomer.Name = "colCustomer";
            this.colCustomer.ReadOnly = true;
            this.colCustomer.Width = 120;
            // 
            // colCustomerName
            // 
            this.colCustomerName.DataPropertyName = "CustomerName";
            this.colCustomerName.HeaderText = "　";
            this.colCustomerName.Name = "colCustomerName";
            this.colCustomerName.ReadOnly = true;
            this.colCustomerName.Width = 300;
            // 
            // colProcessingKBN
            // 
            this.colProcessingKBN.DataPropertyName = "ProcessingKBN";
            this.colProcessingKBN.HeaderText = "処理";
            this.colProcessingKBN.Name = "colProcessingKBN";
            this.colProcessingKBN.ReadOnly = true;
            this.colProcessingKBN.Width = 300;
            // 
            // colSKUName
            // 
            this.colSKUName.DataPropertyName = "SKUName";
            this.colSKUName.HeaderText = "商品";
            this.colSKUName.Name = "colSKUName";
            this.colSKUName.ReadOnly = true;
            this.colSKUName.Visible = false;
            this.colSKUName.Width = 350;
            // 
            // cboSyori
            // 
            this.cboSyori.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboSyori.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSyori.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.Default;
            this.cboSyori.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.cboSyori.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSyori.ForeColor = System.Drawing.Color.Black;
            this.cboSyori.FormattingEnabled = true;
            this.cboSyori.Length = 40;
            this.cboSyori.Location = new System.Drawing.Point(105, 15);
            this.cboSyori.MaxDropDownItems = 3;
            this.cboSyori.MaxLength = 20;
            this.cboSyori.MoveNext = true;
            this.cboSyori.Name = "cboSyori";
            this.cboSyori.Size = new System.Drawing.Size(130, 20);
            this.cboSyori.TabIndex = 1;
            // 
            // SeikyuuShimeShori
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.GvDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "SeikyuuShimeShori";
            this.PanelHeaderHeight = 200;
            this.Text = "SeikyuuShimeShori";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.GvDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CKM_Controls.CKM_Label label4;
        private CKM_Controls.CKM_Label label27;
        private CKM_Controls.CKM_Label label1;
        private CKM_Controls.CKM_ComboBox CboStoreCD;
        private Search.CKM_SearchControl ckM_SearchControl2;
        private Search.CKM_SearchControl ckM_SearchControl3;
        private CKM_Controls.CKM_TextBox ckM_CustomerName;
        private CKM_Controls.CKM_Label label9;
        private System.Windows.Forms.Label lblSkuCD;
        private CKM_Controls.CKM_TextBox ckM_TextBox2;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private Search.CKM_SearchControl ScCustomer;
        private CKM_Controls.CKM_GridView GvDetail;
        private CKM_Controls.CKM_ComboBox cboSyori;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcessingDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBillingDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcessingKBN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUName;
    }
}

