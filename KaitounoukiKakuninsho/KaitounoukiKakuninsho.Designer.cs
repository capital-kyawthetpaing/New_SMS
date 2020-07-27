namespace KaitounoukiKakuninsho
{
    partial class KaitounoukiKakuninsho
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
            this.ckM_TextBox6 = new CKM_Controls.CKM_TextBox();
            this.ckM_TextBox5 = new CKM_Controls.CKM_TextBox();
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.ckM_Label11 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox2 = new CKM_Controls.CKM_TextBox();
            this.ckM_TextBox4 = new CKM_Controls.CKM_TextBox();
            this.label11 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox3 = new CKM_Controls.CKM_TextBox();
            this.ckM_TextBox1 = new CKM_Controls.CKM_TextBox();
            this.ckM_Label12 = new CKM_Controls.CKM_Label();
            this.ckM_Label13 = new CKM_Controls.CKM_Label();
            this.ChkMikakutei = new CKM_Controls.CKM_CheckBox();
            this.ChkFuyo = new CKM_Controls.CKM_CheckBox();
            this.ChkKanbai = new CKM_Controls.CKM_CheckBox();
            this.CboSoukoName = new CKM_Controls.CKM_ComboBox();
            this.ScOrderCD = new Search.CKM_SearchControl();
            this.lblSkuCD = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1368, 34);
            this.PanelHeader.TabIndex = 0;
            // 
            // PanelSearch
            // 
            this.PanelSearch.TabIndex = 7;
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.DefaultlabelSize = true;
            this.label4.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.label4.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(1044, 115);
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
            this.label1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
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
            this.label27.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
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
            this.CboStoreCD.Flag = 0;
            this.CboStoreCD.FormattingEnabled = true;
            this.CboStoreCD.Length = 40;
            this.CboStoreCD.Location = new System.Drawing.Point(1081, 111);
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
            this.ckM_SearchControl2.CodeWidth1 = 100;
            this.ckM_SearchControl2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_SearchControl2.DataCheck = false;
            this.ckM_SearchControl2.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ckM_SearchControl2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ckM_SearchControl2.IsCopy = false;
            this.ckM_SearchControl2.LabelText = "";
            this.ckM_SearchControl2.LabelVisible = false;
            this.ckM_SearchControl2.Location = new System.Drawing.Point(343, 5);
            this.ckM_SearchControl2.Margin = new System.Windows.Forms.Padding(0);
            this.ckM_SearchControl2.Name = "ckM_SearchControl2";
            this.ckM_SearchControl2.NameWidth = 600;
            this.ckM_SearchControl2.SearchEnable = true;
            this.ckM_SearchControl2.Size = new System.Drawing.Size(133, 26);
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
            this.ckM_SearchControl3.CodeWidth1 = 100;
            this.ckM_SearchControl3.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_SearchControl3.DataCheck = false;
            this.ckM_SearchControl3.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ckM_SearchControl3.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ckM_SearchControl3.IsCopy = false;
            this.ckM_SearchControl3.LabelText = "";
            this.ckM_SearchControl3.LabelVisible = false;
            this.ckM_SearchControl3.Location = new System.Drawing.Point(343, 4);
            this.ckM_SearchControl3.Margin = new System.Windows.Forms.Padding(0);
            this.ckM_SearchControl3.Name = "ckM_SearchControl3";
            this.ckM_SearchControl3.NameWidth = 600;
            this.ckM_SearchControl3.SearchEnable = true;
            this.ckM_SearchControl3.Size = new System.Drawing.Size(133, 26);
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
            this.ckM_Label9.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label9.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label9.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label9.Location = new System.Drawing.Point(201, 185);
            this.ckM_Label9.Name = "ckM_Label9";
            this.ckM_Label9.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label9.TabIndex = 742;
            this.ckM_Label9.Text = "～";
            this.ckM_Label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_TextBox6
            // 
            this.ckM_TextBox6.AllowMinus = false;
            this.ckM_TextBox6.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox6.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox6.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox6.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox6.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.ckM_TextBox6.DecimalPlace = 2;
            this.ckM_TextBox6.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ckM_TextBox6.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox6.IntegerPart = 0;
            this.ckM_TextBox6.IsCorrectDate = true;
            this.ckM_TextBox6.isEnterKeyDown = false;
            this.ckM_TextBox6.IsFirstTime = true;
            this.ckM_TextBox6.isMaxLengthErr = false;
            this.ckM_TextBox6.IsNumber = true;
            this.ckM_TextBox6.IsShop = false;
            this.ckM_TextBox6.Length = 10;
            this.ckM_TextBox6.Location = new System.Drawing.Point(230, 182);
            this.ckM_TextBox6.MaxLength = 10;
            this.ckM_TextBox6.MoveNext = true;
            this.ckM_TextBox6.Name = "ckM_TextBox6";
            this.ckM_TextBox6.Size = new System.Drawing.Size(88, 19);
            this.ckM_TextBox6.TabIndex = 6;
            this.ckM_TextBox6.Text = "2019/01/01";
            this.ckM_TextBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox6.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_TextBox5
            // 
            this.ckM_TextBox5.AllowMinus = false;
            this.ckM_TextBox5.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox5.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox5.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox5.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox5.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.ckM_TextBox5.DecimalPlace = 2;
            this.ckM_TextBox5.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ckM_TextBox5.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox5.IntegerPart = 0;
            this.ckM_TextBox5.IsCorrectDate = true;
            this.ckM_TextBox5.isEnterKeyDown = false;
            this.ckM_TextBox5.IsFirstTime = true;
            this.ckM_TextBox5.isMaxLengthErr = false;
            this.ckM_TextBox5.IsNumber = true;
            this.ckM_TextBox5.IsShop = false;
            this.ckM_TextBox5.Length = 10;
            this.ckM_TextBox5.Location = new System.Drawing.Point(102, 182);
            this.ckM_TextBox5.MaxLength = 10;
            this.ckM_TextBox5.MoveNext = true;
            this.ckM_TextBox5.Name = "ckM_TextBox5";
            this.ckM_TextBox5.Size = new System.Drawing.Size(88, 19);
            this.ckM_TextBox5.TabIndex = 5;
            this.ckM_TextBox5.Text = "2019/01/01";
            this.ckM_TextBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox5.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_Label10
            // 
            this.ckM_Label10.AutoSize = true;
            this.ckM_Label10.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label10.DefaultlabelSize = true;
            this.ckM_Label10.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label10.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label10.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label10.Location = new System.Drawing.Point(54, 185);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label10.TabIndex = 741;
            this.ckM_Label10.Text = "発注日";
            this.ckM_Label10.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label11
            // 
            this.ckM_Label11.AutoSize = true;
            this.ckM_Label11.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label11.DefaultlabelSize = true;
            this.ckM_Label11.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label11.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label11.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label11.Location = new System.Drawing.Point(201, 150);
            this.ckM_Label11.Name = "ckM_Label11";
            this.ckM_Label11.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label11.TabIndex = 740;
            this.ckM_Label11.Text = "～";
            this.ckM_Label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_TextBox2.DecimalPlace = 2;
            this.ckM_TextBox2.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ckM_TextBox2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox2.IntegerPart = 0;
            this.ckM_TextBox2.IsCorrectDate = true;
            this.ckM_TextBox2.isEnterKeyDown = false;
            this.ckM_TextBox2.IsFirstTime = true;
            this.ckM_TextBox2.isMaxLengthErr = false;
            this.ckM_TextBox2.IsNumber = true;
            this.ckM_TextBox2.IsShop = false;
            this.ckM_TextBox2.Length = 10;
            this.ckM_TextBox2.Location = new System.Drawing.Point(230, 113);
            this.ckM_TextBox2.MaxLength = 10;
            this.ckM_TextBox2.MoveNext = true;
            this.ckM_TextBox2.Name = "ckM_TextBox2";
            this.ckM_TextBox2.Size = new System.Drawing.Size(88, 19);
            this.ckM_TextBox2.TabIndex = 2;
            this.ckM_TextBox2.Text = "2019/01/01";
            this.ckM_TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox2.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_TextBox4
            // 
            this.ckM_TextBox4.AllowMinus = false;
            this.ckM_TextBox4.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox4.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox4.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox4.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox4.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.YearMonth;
            this.ckM_TextBox4.DecimalPlace = 0;
            this.ckM_TextBox4.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ckM_TextBox4.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox4.IntegerPart = 0;
            this.ckM_TextBox4.IsCorrectDate = true;
            this.ckM_TextBox4.isEnterKeyDown = false;
            this.ckM_TextBox4.IsFirstTime = true;
            this.ckM_TextBox4.isMaxLengthErr = false;
            this.ckM_TextBox4.IsNumber = true;
            this.ckM_TextBox4.IsShop = false;
            this.ckM_TextBox4.Length = 20;
            this.ckM_TextBox4.Location = new System.Drawing.Point(230, 147);
            this.ckM_TextBox4.MaxLength = 20;
            this.ckM_TextBox4.MoveNext = true;
            this.ckM_TextBox4.Name = "ckM_TextBox4";
            this.ckM_TextBox4.Size = new System.Drawing.Size(88, 19);
            this.ckM_TextBox4.TabIndex = 4;
            this.ckM_TextBox4.Text = "2019/01/01";
            this.ckM_TextBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox4.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.DefaultlabelSize = true;
            this.label11.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.label11.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(28, 115);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 12);
            this.label11.TabIndex = 737;
            this.label11.Text = "入荷予定日";
            this.label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_TextBox3
            // 
            this.ckM_TextBox3.AllowMinus = false;
            this.ckM_TextBox3.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox3.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox3.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox3.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox3.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.YearMonth;
            this.ckM_TextBox3.DecimalPlace = 0;
            this.ckM_TextBox3.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ckM_TextBox3.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox3.IntegerPart = 0;
            this.ckM_TextBox3.IsCorrectDate = true;
            this.ckM_TextBox3.isEnterKeyDown = false;
            this.ckM_TextBox3.IsFirstTime = true;
            this.ckM_TextBox3.isMaxLengthErr = false;
            this.ckM_TextBox3.IsNumber = true;
            this.ckM_TextBox3.IsShop = false;
            this.ckM_TextBox3.Length = 20;
            this.ckM_TextBox3.Location = new System.Drawing.Point(102, 147);
            this.ckM_TextBox3.MaxLength = 20;
            this.ckM_TextBox3.MoveNext = true;
            this.ckM_TextBox3.Name = "ckM_TextBox3";
            this.ckM_TextBox3.Size = new System.Drawing.Size(88, 19);
            this.ckM_TextBox3.TabIndex = 3;
            this.ckM_TextBox3.Text = "2019/01/01";
            this.ckM_TextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox3.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_TextBox1.DecimalPlace = 2;
            this.ckM_TextBox1.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ckM_TextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox1.IntegerPart = 0;
            this.ckM_TextBox1.IsCorrectDate = true;
            this.ckM_TextBox1.isEnterKeyDown = false;
            this.ckM_TextBox1.IsFirstTime = true;
            this.ckM_TextBox1.isMaxLengthErr = false;
            this.ckM_TextBox1.IsNumber = true;
            this.ckM_TextBox1.IsShop = false;
            this.ckM_TextBox1.Length = 10;
            this.ckM_TextBox1.Location = new System.Drawing.Point(102, 113);
            this.ckM_TextBox1.MaxLength = 10;
            this.ckM_TextBox1.MoveNext = true;
            this.ckM_TextBox1.Name = "ckM_TextBox1";
            this.ckM_TextBox1.Size = new System.Drawing.Size(88, 19);
            this.ckM_TextBox1.TabIndex = 1;
            this.ckM_TextBox1.Text = "2019/01/01";
            this.ckM_TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_Label12
            // 
            this.ckM_Label12.AutoSize = true;
            this.ckM_Label12.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label12.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label12.DefaultlabelSize = true;
            this.ckM_Label12.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label12.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label12.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label12.Location = new System.Drawing.Point(28, 151);
            this.ckM_Label12.Name = "ckM_Label12";
            this.ckM_Label12.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label12.TabIndex = 739;
            this.ckM_Label12.Text = "入荷予定月";
            this.ckM_Label12.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label13
            // 
            this.ckM_Label13.AutoSize = true;
            this.ckM_Label13.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label13.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label13.DefaultlabelSize = true;
            this.ckM_Label13.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label13.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label13.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label13.Location = new System.Drawing.Point(201, 116);
            this.ckM_Label13.Name = "ckM_Label13";
            this.ckM_Label13.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label13.TabIndex = 738;
            this.ckM_Label13.Text = "～";
            this.ckM_Label13.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ChkMikakutei
            // 
            this.ChkMikakutei.AutoSize = true;
            this.ChkMikakutei.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ChkMikakutei.Location = new System.Drawing.Point(364, 183);
            this.ChkMikakutei.Name = "ChkMikakutei";
            this.ChkMikakutei.Size = new System.Drawing.Size(76, 16);
            this.ChkMikakutei.TabIndex = 7;
            this.ChkMikakutei.Text = "未確定分";
            this.ChkMikakutei.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ChkMikakutei.UseVisualStyleBackColor = true;
            this.ChkMikakutei.CheckedChanged += new System.EventHandler(this.ChkMikakutei_CheckedChanged);
            // 
            // ChkFuyo
            // 
            this.ChkFuyo.AutoSize = true;
            this.ChkFuyo.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ChkFuyo.Location = new System.Drawing.Point(709, 183);
            this.ChkFuyo.Name = "ChkFuyo";
            this.ChkFuyo.Size = new System.Drawing.Size(50, 16);
            this.ChkFuyo.TabIndex = 10;
            this.ChkFuyo.Text = "不要";
            this.ChkFuyo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ChkFuyo.UseVisualStyleBackColor = true;
            // 
            // ChkKanbai
            // 
            this.ChkKanbai.AutoSize = true;
            this.ChkKanbai.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ChkKanbai.Location = new System.Drawing.Point(631, 183);
            this.ChkKanbai.Name = "ChkKanbai";
            this.ChkKanbai.Size = new System.Drawing.Size(50, 16);
            this.ChkKanbai.TabIndex = 9;
            this.ChkKanbai.Text = "完売";
            this.ChkKanbai.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ChkKanbai.UseVisualStyleBackColor = true;
            // 
            // CboSoukoName
            // 
            this.CboSoukoName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.CboSoukoName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CboSoukoName.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.入荷予定状況;
            this.CboSoukoName.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.CboSoukoName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CboSoukoName.Flag = 0;
            this.CboSoukoName.FormattingEnabled = true;
            this.CboSoukoName.Length = 20;
            this.CboSoukoName.Location = new System.Drawing.Point(446, 181);
            this.CboSoukoName.MaxLength = 10;
            this.CboSoukoName.MoveNext = true;
            this.CboSoukoName.Name = "CboSoukoName";
            this.CboSoukoName.Size = new System.Drawing.Size(140, 20);
            this.CboSoukoName.TabIndex = 8;
            // 
            // ScOrderCD
            // 
            this.ScOrderCD.AutoSize = true;
            this.ScOrderCD.ChangeDate = "";
            this.ScOrderCD.ChangeDateWidth = 100;
            this.ScOrderCD.Code = "";
            this.ScOrderCD.CodeWidth = 100;
            this.ScOrderCD.CodeWidth1 = 100;
            this.ScOrderCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScOrderCD.DataCheck = true;
            this.ScOrderCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScOrderCD.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ScOrderCD.IsCopy = false;
            this.ScOrderCD.LabelText = "";
            this.ScOrderCD.LabelVisible = true;
            this.ScOrderCD.Location = new System.Drawing.Point(102, 228);
            this.ScOrderCD.Margin = new System.Windows.Forms.Padding(0);
            this.ScOrderCD.Name = "ScOrderCD";
            this.ScOrderCD.NameWidth = 310;
            this.ScOrderCD.SearchEnable = true;
            this.ScOrderCD.Size = new System.Drawing.Size(444, 27);
            this.ScOrderCD.Stype = Search.CKM_SearchControl.SearchType.仕入先;
            this.ScOrderCD.TabIndex = 11;
            this.ScOrderCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScOrderCD.UseChangeDate = false;
            this.ScOrderCD.Value1 = null;
            this.ScOrderCD.Value2 = null;
            this.ScOrderCD.Value3 = null;
            // 
            // lblSkuCD
            // 
            this.lblSkuCD.AutoSize = true;
            this.lblSkuCD.BackColor = System.Drawing.Color.Transparent;
            this.lblSkuCD.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSkuCD.Location = new System.Drawing.Point(52, 237);
            this.lblSkuCD.Name = "lblSkuCD";
            this.lblSkuCD.Size = new System.Drawing.Size(44, 12);
            this.lblSkuCD.TabIndex = 748;
            this.lblSkuCD.Text = "仕入先";
            this.lblSkuCD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // KaitounoukiKakuninsho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.ChkMikakutei);
            this.Controls.Add(this.ChkFuyo);
            this.Controls.Add(this.ChkKanbai);
            this.Controls.Add(this.CboSoukoName);
            this.Controls.Add(this.ScOrderCD);
            this.Controls.Add(this.lblSkuCD);
            this.Controls.Add(this.ckM_Label9);
            this.Controls.Add(this.ckM_TextBox6);
            this.Controls.Add(this.ckM_TextBox5);
            this.Controls.Add(this.ckM_Label10);
            this.Controls.Add(this.ckM_Label11);
            this.Controls.Add(this.ckM_TextBox2);
            this.Controls.Add(this.ckM_TextBox4);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.ckM_TextBox3);
            this.Controls.Add(this.ckM_TextBox1);
            this.Controls.Add(this.ckM_Label12);
            this.Controls.Add(this.ckM_Label13);
            this.Controls.Add(this.CboStoreCD);
            this.Controls.Add(this.label4);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "KaitounoukiKakuninsho";
            this.PanelHeaderHeight = 90;
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.CboStoreCD, 0);
            this.Controls.SetChildIndex(this.ckM_Label13, 0);
            this.Controls.SetChildIndex(this.ckM_Label12, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox1, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox3, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox4, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox2, 0);
            this.Controls.SetChildIndex(this.ckM_Label11, 0);
            this.Controls.SetChildIndex(this.ckM_Label10, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox5, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox6, 0);
            this.Controls.SetChildIndex(this.ckM_Label9, 0);
            this.Controls.SetChildIndex(this.lblSkuCD, 0);
            this.Controls.SetChildIndex(this.ScOrderCD, 0);
            this.Controls.SetChildIndex(this.CboSoukoName, 0);
            this.Controls.SetChildIndex(this.ChkKanbai, 0);
            this.Controls.SetChildIndex(this.ChkFuyo, 0);
            this.Controls.SetChildIndex(this.ChkMikakutei, 0);
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
        private CKM_Controls.CKM_TextBox ckM_TextBox6;
        private CKM_Controls.CKM_TextBox ckM_TextBox5;
        private CKM_Controls.CKM_Label ckM_Label10;
        private CKM_Controls.CKM_Label ckM_Label11;
        private CKM_Controls.CKM_TextBox ckM_TextBox2;
        private CKM_Controls.CKM_TextBox ckM_TextBox4;
        private CKM_Controls.CKM_Label label11;
        private CKM_Controls.CKM_TextBox ckM_TextBox3;
        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private CKM_Controls.CKM_Label ckM_Label12;
        private CKM_Controls.CKM_Label ckM_Label13;
        private CKM_Controls.CKM_CheckBox ChkMikakutei;
        private CKM_Controls.CKM_CheckBox ChkFuyo;
        private CKM_Controls.CKM_CheckBox ChkKanbai;
        private CKM_Controls.CKM_ComboBox CboSoukoName;
        private Search.CKM_SearchControl ScOrderCD;
        private System.Windows.Forms.Label lblSkuCD;
    }
}

