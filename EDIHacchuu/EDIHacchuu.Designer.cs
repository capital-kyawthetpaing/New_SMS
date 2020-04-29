namespace EDIHacchuu
{
    partial class EDIHacchuu
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
            this.label4 = new CKM_Controls.CKM_Label();
            this.label1 = new CKM_Controls.CKM_Label();
            this.label27 = new CKM_Controls.CKM_Label();
            this.CboStoreCD = new CKM_Controls.CKM_ComboBox();
            this.ckM_SearchControl2 = new Search.CKM_SearchControl();
            this.ckM_SearchControl3 = new Search.CKM_SearchControl();
            this.ckM_Label9 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox2 = new CKM_Controls.CKM_TextBox();
            this.ckM_TextBox1 = new CKM_Controls.CKM_TextBox();
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.ChkMisyounin = new CKM_Controls.CKM_CheckBox();
            this.lblSkuCD = new System.Windows.Forms.Label();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ScOrderNO = new Search.CKM_SearchControl();
            this.ScStaff = new Search.CKM_SearchControl();
            this.label9 = new CKM_Controls.CKM_Label();
            this.ScVendor = new Search.CKM_SearchControl();
            this.RdoReOutput = new CKM_Controls.CKM_RadioButton();
            this.RdoNotOutput = new CKM_Controls.CKM_RadioButton();
            this.ScEDIOrderNO = new Search.CKM_SearchControl();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1368, 34);
            this.PanelHeader.TabIndex = 0;
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(834, 0);
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
            this.label4.Location = new System.Drawing.Point(67, 115);
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
            this.CboStoreCD.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.店舗ストア_見積;
            this.CboStoreCD.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.CboStoreCD.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CboStoreCD.FormattingEnabled = true;
            this.CboStoreCD.Length = 40;
            this.CboStoreCD.Location = new System.Drawing.Point(102, 111);
            this.CboStoreCD.MaxLength = 20;
            this.CboStoreCD.MoveNext = true;
            this.CboStoreCD.Name = "CboStoreCD";
            this.CboStoreCD.Size = new System.Drawing.Size(280, 20);
            this.CboStoreCD.TabIndex = 0;
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
            // ckM_Label9
            // 
            this.ckM_Label9.AutoSize = true;
            this.ckM_Label9.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label9.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label9.DefaultlabelSize = true;
            this.ckM_Label9.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label9.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label9.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label9.Location = new System.Drawing.Point(201, 175);
            this.ckM_Label9.Name = "ckM_Label9";
            this.ckM_Label9.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label9.TabIndex = 742;
            this.ckM_Label9.Text = "～";
            this.ckM_Label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_TextBox2.Location = new System.Drawing.Point(230, 172);
            this.ckM_TextBox2.MaxLength = 10;
            this.ckM_TextBox2.MoveNext = true;
            this.ckM_TextBox2.Name = "ckM_TextBox2";
            this.ckM_TextBox2.Size = new System.Drawing.Size(88, 19);
            this.ckM_TextBox2.TabIndex = 5;
            this.ckM_TextBox2.Text = "2019/01/01";
            this.ckM_TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox2.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_TextBox1.Location = new System.Drawing.Point(102, 172);
            this.ckM_TextBox1.MaxLength = 10;
            this.ckM_TextBox1.MoveNext = true;
            this.ckM_TextBox1.Name = "ckM_TextBox1";
            this.ckM_TextBox1.Size = new System.Drawing.Size(88, 19);
            this.ckM_TextBox1.TabIndex = 4;
            this.ckM_TextBox1.Text = "2019/01/01";
            this.ckM_TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_Label10
            // 
            this.ckM_Label10.AutoSize = true;
            this.ckM_Label10.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label10.DefaultlabelSize = true;
            this.ckM_Label10.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label10.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label10.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label10.Location = new System.Drawing.Point(54, 175);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label10.TabIndex = 741;
            this.ckM_Label10.Text = "発注日";
            this.ckM_Label10.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ChkMisyounin
            // 
            this.ChkMisyounin.AutoSize = true;
            this.ChkMisyounin.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ChkMisyounin.Location = new System.Drawing.Point(102, 296);
            this.ChkMisyounin.Name = "ChkMisyounin";
            this.ChkMisyounin.Size = new System.Drawing.Size(141, 16);
            this.ChkMisyounin.TabIndex = 9;
            this.ChkMisyounin.Text = "未承認分も出力する";
            this.ChkMisyounin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ChkMisyounin.UseVisualStyleBackColor = true;
            // 
            // lblSkuCD
            // 
            this.lblSkuCD.AutoSize = true;
            this.lblSkuCD.BackColor = System.Drawing.Color.Transparent;
            this.lblSkuCD.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSkuCD.Location = new System.Drawing.Point(52, 205);
            this.lblSkuCD.Name = "lblSkuCD";
            this.lblSkuCD.Size = new System.Drawing.Size(44, 12);
            this.lblSkuCD.TabIndex = 748;
            this.lblSkuCD.Text = "発注先";
            this.lblSkuCD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label1.Location = new System.Drawing.Point(41, 145);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label1.TabIndex = 749;
            this.ckM_Label1.Text = "出力対象";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label2.Location = new System.Drawing.Point(325, 145);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(78, 12);
            this.ckM_Label2.TabIndex = 750;
            this.ckM_Label2.Text = "EDI処理番号";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label3
            // 
            this.ckM_Label3.AutoSize = true;
            this.ckM_Label3.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label3.DefaultlabelSize = true;
            this.ckM_Label3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label3.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label3.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label3.Location = new System.Drawing.Point(39, 265);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label3.TabIndex = 753;
            this.ckM_Label3.Text = "発注番号";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScOrderNO
            // 
            this.ScOrderNO.AutoSize = true;
            this.ScOrderNO.ChangeDate = "";
            this.ScOrderNO.ChangeDateWidth = 100;
            this.ScOrderNO.Code = "";
            this.ScOrderNO.CodeWidth = 100;
            this.ScOrderNO.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScOrderNO.DataCheck = false;
            this.ScOrderNO.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ScOrderNO.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ScOrderNO.IsCopy = false;
            this.ScOrderNO.LabelText = "";
            this.ScOrderNO.LabelVisible = false;
            this.ScOrderNO.Location = new System.Drawing.Point(102, 257);
            this.ScOrderNO.Name = "ScOrderNO";
            this.ScOrderNO.SearchEnable = true;
            this.ScOrderNO.Size = new System.Drawing.Size(133, 28);
            this.ScOrderNO.Stype = Search.CKM_SearchControl.SearchType.発注番号;
            this.ScOrderNO.TabIndex = 8;
            this.ScOrderNO.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScOrderNO.UseChangeDate = false;
            this.ScOrderNO.Value1 = null;
            this.ScOrderNO.Value2 = null;
            this.ScOrderNO.Value3 = null;
            // 
            // ScStaff
            // 
            this.ScStaff.AutoSize = true;
            this.ScStaff.ChangeDate = "";
            this.ScStaff.ChangeDateWidth = 100;
            this.ScStaff.Code = "";
            this.ScStaff.CodeWidth = 100;
            this.ScStaff.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScStaff.DataCheck = false;
            this.ScStaff.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ScStaff.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ScStaff.IsCopy = false;
            this.ScStaff.LabelText = "";
            this.ScStaff.LabelVisible = true;
            this.ScStaff.Location = new System.Drawing.Point(102, 227);
            this.ScStaff.Name = "ScStaff";
            this.ScStaff.SearchEnable = true;
            this.ScStaff.Size = new System.Drawing.Size(344, 28);
            this.ScStaff.Stype = Search.CKM_SearchControl.SearchType.スタッフ;
            this.ScStaff.TabIndex = 7;
            this.ScStaff.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScStaff.UseChangeDate = false;
            this.ScStaff.Value1 = null;
            this.ScStaff.Value2 = null;
            this.ScStaff.Value3 = null;
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
            this.label9.Location = new System.Drawing.Point(14, 235);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 12);
            this.label9.TabIndex = 759;
            this.label9.Text = "担当スタッフ";
            this.label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ScVendor.Location = new System.Drawing.Point(102, 197);
            this.ScVendor.Name = "ScVendor";
            this.ScVendor.SearchEnable = true;
            this.ScVendor.Size = new System.Drawing.Size(444, 28);
            this.ScVendor.Stype = Search.CKM_SearchControl.SearchType.仕入先;
            this.ScVendor.TabIndex = 6;
            this.ScVendor.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScVendor.UseChangeDate = false;
            this.ScVendor.Value1 = null;
            this.ScVendor.Value2 = null;
            this.ScVendor.Value3 = null;
            // 
            // RdoReOutput
            // 
            this.RdoReOutput.AutoSize = true;
            this.RdoReOutput.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RdoReOutput.Location = new System.Drawing.Point(203, 143);
            this.RdoReOutput.Name = "RdoReOutput";
            this.RdoReOutput.Size = new System.Drawing.Size(75, 16);
            this.RdoReOutput.TabIndex = 2;
            this.RdoReOutput.Text = "再出力分";
            this.RdoReOutput.UseVisualStyleBackColor = true;
            this.RdoReOutput.CheckedChanged += new System.EventHandler(this.RdoReOutput_CheckedChanged);
            // 
            // RdoNotOutput
            // 
            this.RdoNotOutput.AutoSize = true;
            this.RdoNotOutput.Checked = true;
            this.RdoNotOutput.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RdoNotOutput.Location = new System.Drawing.Point(104, 143);
            this.RdoNotOutput.Name = "RdoNotOutput";
            this.RdoNotOutput.Size = new System.Drawing.Size(75, 16);
            this.RdoNotOutput.TabIndex = 1;
            this.RdoNotOutput.TabStop = true;
            this.RdoNotOutput.Text = "未出力分";
            this.RdoNotOutput.UseVisualStyleBackColor = false;
            // 
            // ScEDIOrderNO
            // 
            this.ScEDIOrderNO.AutoSize = true;
            this.ScEDIOrderNO.ChangeDate = "";
            this.ScEDIOrderNO.ChangeDateWidth = 100;
            this.ScEDIOrderNO.Code = "";
            this.ScEDIOrderNO.CodeWidth = 100;
            this.ScEDIOrderNO.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScEDIOrderNO.DataCheck = false;
            this.ScEDIOrderNO.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ScEDIOrderNO.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ScEDIOrderNO.IsCopy = false;
            this.ScEDIOrderNO.LabelText = "";
            this.ScEDIOrderNO.LabelVisible = false;
            this.ScEDIOrderNO.Location = new System.Drawing.Point(409, 136);
            this.ScEDIOrderNO.Name = "ScEDIOrderNO";
            this.ScEDIOrderNO.SearchEnable = true;
            this.ScEDIOrderNO.Size = new System.Drawing.Size(133, 28);
            this.ScEDIOrderNO.Stype = Search.CKM_SearchControl.SearchType.EDI処理番号;
            this.ScEDIOrderNO.TabIndex = 3;
            this.ScEDIOrderNO.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScEDIOrderNO.UseChangeDate = false;
            this.ScEDIOrderNO.Value1 = null;
            this.ScEDIOrderNO.Value2 = null;
            this.ScEDIOrderNO.Value3 = null;
            // 
            // EDIHacchuu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.ScEDIOrderNO);
            this.Controls.Add(this.RdoReOutput);
            this.Controls.Add(this.RdoNotOutput);
            this.Controls.Add(this.ScVendor);
            this.Controls.Add(this.ScStaff);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.ckM_Label3);
            this.Controls.Add(this.ScOrderNO);
            this.Controls.Add(this.ckM_Label2);
            this.Controls.Add(this.ckM_Label1);
            this.Controls.Add(this.ChkMisyounin);
            this.Controls.Add(this.lblSkuCD);
            this.Controls.Add(this.ckM_Label9);
            this.Controls.Add(this.ckM_TextBox2);
            this.Controls.Add(this.ckM_TextBox1);
            this.Controls.Add(this.ckM_Label10);
            this.Controls.Add(this.CboStoreCD);
            this.Controls.Add(this.label4);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "EDIHacchuu";
            this.PanelHeaderHeight = 90;
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.CboStoreCD, 0);
            this.Controls.SetChildIndex(this.ckM_Label10, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox1, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox2, 0);
            this.Controls.SetChildIndex(this.ckM_Label9, 0);
            this.Controls.SetChildIndex(this.lblSkuCD, 0);
            this.Controls.SetChildIndex(this.ChkMisyounin, 0);
            this.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.Controls.SetChildIndex(this.ckM_Label2, 0);
            this.Controls.SetChildIndex(this.ScOrderNO, 0);
            this.Controls.SetChildIndex(this.ckM_Label3, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.ScStaff, 0);
            this.Controls.SetChildIndex(this.ScVendor, 0);
            this.Controls.SetChildIndex(this.RdoNotOutput, 0);
            this.Controls.SetChildIndex(this.RdoReOutput, 0);
            this.Controls.SetChildIndex(this.ScEDIOrderNO, 0);
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
        private CKM_Controls.CKM_Label ckM_Label9;
        private CKM_Controls.CKM_TextBox ckM_TextBox2;
        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private CKM_Controls.CKM_Label ckM_Label10;
        private CKM_Controls.CKM_CheckBox ChkMisyounin;
        private System.Windows.Forms.Label lblSkuCD;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Label ckM_Label3;
        private Search.CKM_SearchControl ScOrderNO;
        private Search.CKM_SearchControl ScStaff;
        private CKM_Controls.CKM_Label label9;
        private Search.CKM_SearchControl ScVendor;
        private CKM_Controls.CKM_RadioButton RdoReOutput;
        private CKM_Controls.CKM_RadioButton RdoNotOutput;
        private Search.CKM_SearchControl ScEDIOrderNO;
    }
}

