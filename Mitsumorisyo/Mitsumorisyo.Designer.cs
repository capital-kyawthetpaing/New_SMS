namespace Mitsumorisyo
{
    partial class Mitsumorisyo
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
            this.ScMitsumoriNO = new Search.CKM_SearchControl();
            this.CboStoreCD = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.ckM_SearchControl2 = new Search.CKM_SearchControl();
            this.ckM_SearchControl3 = new Search.CKM_SearchControl();
            this.ckM_CustomerName = new CKM_Controls.CKM_TextBox();
            this.ckM_RadioButton3 = new CKM_Controls.CKM_RadioButton();
            this.ckM_TextBox5 = new CKM_Controls.CKM_TextBox();
            this.ScStaff = new Search.CKM_SearchControl();
            this.ScCustomer = new Search.CKM_SearchControl();
            this.label14 = new CKM_Controls.CKM_Label();
            this.label9 = new CKM_Controls.CKM_Label();
            this.lblSkuCD = new System.Windows.Forms.Label();
            this.ckM_Label8 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox2 = new CKM_Controls.CKM_TextBox();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_RadioButton2 = new CKM_Controls.CKM_RadioButton();
            this.ckM_RadioButton1 = new CKM_Controls.CKM_RadioButton();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox4 = new CKM_Controls.CKM_TextBox();
            this.ckM_TextBox1 = new CKM_Controls.CKM_TextBox();
            this.ckM_TextBox3 = new CKM_Controls.CKM_TextBox();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.ckM_Label7 = new CKM_Controls.CKM_Label();
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.DefaultlabelSize = true;
            this.label4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label4.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(1023, 115);
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
            // ScMitsumoriNO
            // 
            this.ScMitsumoriNO.AutoSize = true;
            this.ScMitsumoriNO.ChangeDate = "";
            this.ScMitsumoriNO.ChangeDateWidth = 100;
            this.ScMitsumoriNO.Code = "";
            this.ScMitsumoriNO.CodeWidth = 100;
            this.ScMitsumoriNO.CodeWidth1 = 100;
            this.ScMitsumoriNO.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScMitsumoriNO.DataCheck = false;
            this.ScMitsumoriNO.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ScMitsumoriNO.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ScMitsumoriNO.IsCopy = false;
            this.ScMitsumoriNO.LabelText = "";
            this.ScMitsumoriNO.LabelVisible = true;
            this.ScMitsumoriNO.Location = new System.Drawing.Point(102, 340);
            this.ScMitsumoriNO.Margin = new System.Windows.Forms.Padding(0);
            this.ScMitsumoriNO.Name = "ScMitsumoriNO";
            this.ScMitsumoriNO.NameWidth = 600;
            this.ScMitsumoriNO.SearchEnable = true;
            this.ScMitsumoriNO.Size = new System.Drawing.Size(734, 28);
            this.ScMitsumoriNO.Stype = Search.CKM_SearchControl.SearchType.見積番号;
            this.ScMitsumoriNO.TabIndex = 12;
            this.ScMitsumoriNO.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScMitsumoriNO.UseChangeDate = false;
            this.ScMitsumoriNO.Value1 = null;
            this.ScMitsumoriNO.Value2 = null;
            this.ScMitsumoriNO.Value3 = null;
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
            this.CboStoreCD.Location = new System.Drawing.Point(1060, 111);
            this.CboStoreCD.MaxLength = 20;
            this.CboStoreCD.MoveNext = false;
            this.CboStoreCD.Name = "CboStoreCD";
            this.CboStoreCD.Size = new System.Drawing.Size(280, 20);
            this.CboStoreCD.TabIndex = 2;
            this.CboStoreCD.SelectedIndexChanged += new System.EventHandler(this.CboStoreCD_SelectedIndexChanged);
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
            this.ckM_Label1.Location = new System.Drawing.Point(39, 349);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label1.TabIndex = 274;
            this.ckM_Label1.Text = "見積番号";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_SearchControl2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_SearchControl2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ckM_SearchControl2.IsCopy = false;
            this.ckM_SearchControl2.LabelText = "";
            this.ckM_SearchControl2.LabelVisible = false;
            this.ckM_SearchControl2.Location = new System.Drawing.Point(343, 5);
            this.ckM_SearchControl2.Margin = new System.Windows.Forms.Padding(0);
            this.ckM_SearchControl2.Name = "ckM_SearchControl2";
            this.ckM_SearchControl2.NameWidth = 600;
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
            this.ckM_SearchControl3.CodeWidth1 = 100;
            this.ckM_SearchControl3.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_SearchControl3.DataCheck = false;
            this.ckM_SearchControl3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_SearchControl3.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ckM_SearchControl3.IsCopy = false;
            this.ckM_SearchControl3.LabelText = "";
            this.ckM_SearchControl3.LabelVisible = false;
            this.ckM_SearchControl3.Location = new System.Drawing.Point(343, 4);
            this.ckM_SearchControl3.Margin = new System.Windows.Forms.Padding(0);
            this.ckM_SearchControl3.Name = "ckM_SearchControl3";
            this.ckM_SearchControl3.NameWidth = 600;
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
            this.ckM_CustomerName.ClientColor = System.Drawing.Color.White;
            this.ckM_CustomerName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.ckM_CustomerName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_CustomerName.DecimalPlace = 0;
            this.ckM_CustomerName.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_CustomerName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.ckM_CustomerName.IntegerPart = 0;
            this.ckM_CustomerName.IsCorrectDate = true;
            this.ckM_CustomerName.isEnterKeyDown = false;
            this.ckM_CustomerName.isMaxLengthErr = false;
            this.ckM_CustomerName.IsNumber = true;
            this.ckM_CustomerName.IsShop = false;
            this.ckM_CustomerName.Length = 40;
            this.ckM_CustomerName.Location = new System.Drawing.Point(233, 215);
            this.ckM_CustomerName.MaxLength = 40;
            this.ckM_CustomerName.MoveNext = false;
            this.ckM_CustomerName.Name = "ckM_CustomerName";
            this.ckM_CustomerName.Size = new System.Drawing.Size(520, 19);
            this.ckM_CustomerName.TabIndex = 7;
            this.ckM_CustomerName.Text = "ＸＸＸＸＸＸＸＸＸ10ＸＸＸＸＸＸＸＸＸ20ＸＸＸＸＸＸＸＸＸ30ＸＸＸＸＸＸＸＸＸ40";
            this.ckM_CustomerName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_RadioButton3
            // 
            this.ckM_RadioButton3.AutoSize = true;
            this.ckM_RadioButton3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckM_RadioButton3.Location = new System.Drawing.Point(233, 291);
            this.ckM_RadioButton3.Name = "ckM_RadioButton3";
            this.ckM_RadioButton3.Size = new System.Drawing.Size(62, 16);
            this.ckM_RadioButton3.TabIndex = 11;
            this.ckM_RadioButton3.Text = "未印刷";
            this.ckM_RadioButton3.UseVisualStyleBackColor = true;
            // 
            // ckM_TextBox5
            // 
            this.ckM_TextBox5.AllowMinus = false;
            this.ckM_TextBox5.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox5.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox5.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox5.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.ckM_TextBox5.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_TextBox5.DecimalPlace = 0;
            this.ckM_TextBox5.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox5.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.ckM_TextBox5.IntegerPart = 0;
            this.ckM_TextBox5.IsCorrectDate = true;
            this.ckM_TextBox5.isEnterKeyDown = false;
            this.ckM_TextBox5.isMaxLengthErr = false;
            this.ckM_TextBox5.IsNumber = true;
            this.ckM_TextBox5.IsShop = false;
            this.ckM_TextBox5.Length = 50;
            this.ckM_TextBox5.Location = new System.Drawing.Point(102, 249);
            this.ckM_TextBox5.MaxLength = 50;
            this.ckM_TextBox5.MoveNext = false;
            this.ckM_TextBox5.Name = "ckM_TextBox5";
            this.ckM_TextBox5.Size = new System.Drawing.Size(650, 19);
            this.ckM_TextBox5.TabIndex = 8;
            this.ckM_TextBox5.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ScStaff.DataCheck = true;
            this.ScStaff.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ScStaff.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ScStaff.IsCopy = false;
            this.ScStaff.LabelText = "";
            this.ScStaff.LabelVisible = true;
            this.ScStaff.Location = new System.Drawing.Point(102, 171);
            this.ScStaff.Margin = new System.Windows.Forms.Padding(0);
            this.ScStaff.Name = "ScStaff";
            this.ScStaff.NameWidth = 250;
            this.ScStaff.SearchEnable = true;
            this.ScStaff.Size = new System.Drawing.Size(354, 28);
            this.ScStaff.Stype = Search.CKM_SearchControl.SearchType.スタッフ;
            this.ScStaff.TabIndex = 5;
            this.ScStaff.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScStaff.UseChangeDate = false;
            this.ScStaff.Value1 = null;
            this.ScStaff.Value2 = null;
            this.ScStaff.Value3 = null;
            // 
            // ScCustomer
            // 
            this.ScCustomer.AutoSize = true;
            this.ScCustomer.ChangeDate = "";
            this.ScCustomer.ChangeDateWidth = 100;
            this.ScCustomer.Code = "";
            this.ScCustomer.CodeWidth = 100;
            this.ScCustomer.CodeWidth1 = 100;
            this.ScCustomer.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScCustomer.DataCheck = true;
            this.ScCustomer.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ScCustomer.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ScCustomer.IsCopy = false;
            this.ScCustomer.LabelText = "";
            this.ScCustomer.LabelVisible = false;
            this.ScCustomer.Location = new System.Drawing.Point(102, 210);
            this.ScCustomer.Margin = new System.Windows.Forms.Padding(0);
            this.ScCustomer.Name = "ScCustomer";
            this.ScCustomer.NameWidth = 500;
            this.ScCustomer.SearchEnable = true;
            this.ScCustomer.Size = new System.Drawing.Size(133, 28);
            this.ScCustomer.Stype = Search.CKM_SearchControl.SearchType.得意先;
            this.ScCustomer.TabIndex = 6;
            this.ScCustomer.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScCustomer.UseChangeDate = false;
            this.ScCustomer.Value1 = null;
            this.ScCustomer.Value2 = null;
            this.ScCustomer.Value3 = null;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.DefaultlabelSize = true;
            this.label14.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label14.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(40, 252);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(57, 12);
            this.label14.TabIndex = 706;
            this.label14.Text = "見積件名";
            this.label14.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.label9.Location = new System.Drawing.Point(14, 179);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 12);
            this.label9.TabIndex = 705;
            this.label9.Text = "担当スタッフ";
            this.label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSkuCD
            // 
            this.lblSkuCD.AutoSize = true;
            this.lblSkuCD.BackColor = System.Drawing.Color.Transparent;
            this.lblSkuCD.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSkuCD.Location = new System.Drawing.Point(66, 218);
            this.lblSkuCD.Name = "lblSkuCD";
            this.lblSkuCD.Size = new System.Drawing.Size(31, 12);
            this.lblSkuCD.TabIndex = 704;
            this.lblSkuCD.Text = "顧客";
            this.lblSkuCD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label8.Location = new System.Drawing.Point(208, 114);
            this.ckM_Label8.Name = "ckM_Label8";
            this.ckM_Label8.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label8.TabIndex = 703;
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
            this.ckM_TextBox2.isMaxLengthErr = false;
            this.ckM_TextBox2.IsNumber = true;
            this.ckM_TextBox2.IsShop = false;
            this.ckM_TextBox2.Length = 10;
            this.ckM_TextBox2.Location = new System.Drawing.Point(233, 111);
            this.ckM_TextBox2.MaxLength = 10;
            this.ckM_TextBox2.MoveNext = false;
            this.ckM_TextBox2.Name = "ckM_TextBox2";
            this.ckM_TextBox2.Size = new System.Drawing.Size(100, 19);
            this.ckM_TextBox2.TabIndex = 1;
            this.ckM_TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            this.ckM_Label2.Location = new System.Drawing.Point(53, 114);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label2.TabIndex = 690;
            this.ckM_Label2.Text = "見積日";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_RadioButton2
            // 
            this.ckM_RadioButton2.AutoSize = true;
            this.ckM_RadioButton2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckM_RadioButton2.Location = new System.Drawing.Point(161, 291);
            this.ckM_RadioButton2.Name = "ckM_RadioButton2";
            this.ckM_RadioButton2.Size = new System.Drawing.Size(62, 16);
            this.ckM_RadioButton2.TabIndex = 10;
            this.ckM_RadioButton2.Text = "印刷済";
            this.ckM_RadioButton2.UseVisualStyleBackColor = true;
            // 
            // ckM_RadioButton1
            // 
            this.ckM_RadioButton1.AutoSize = true;
            this.ckM_RadioButton1.Checked = true;
            this.ckM_RadioButton1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckM_RadioButton1.Location = new System.Drawing.Point(102, 291);
            this.ckM_RadioButton1.Name = "ckM_RadioButton1";
            this.ckM_RadioButton1.Size = new System.Drawing.Size(49, 16);
            this.ckM_RadioButton1.TabIndex = 9;
            this.ckM_RadioButton1.TabStop = true;
            this.ckM_RadioButton1.Text = "全て";
            this.ckM_RadioButton1.UseVisualStyleBackColor = true;
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
            this.ckM_Label3.Location = new System.Drawing.Point(27, 144);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label3.TabIndex = 702;
            this.ckM_Label3.Text = "見積入力日";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label4.Location = new System.Drawing.Point(53, 293);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label4.TabIndex = 701;
            this.ckM_Label4.Text = "見積書";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label5
            // 
            this.ckM_Label5.AutoSize = true;
            this.ckM_Label5.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label5.DefaultlabelSize = true;
            this.ckM_Label5.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label5.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label5.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label5.Location = new System.Drawing.Point(208, 143);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label5.TabIndex = 688;
            this.ckM_Label5.Text = "～";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_TextBox4
            // 
            this.ckM_TextBox4.AllowMinus = false;
            this.ckM_TextBox4.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox4.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox4.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox4.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox4.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.ckM_TextBox4.DecimalPlace = 0;
            this.ckM_TextBox4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox4.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox4.IntegerPart = 0;
            this.ckM_TextBox4.IsCorrectDate = true;
            this.ckM_TextBox4.isEnterKeyDown = false;
            this.ckM_TextBox4.isMaxLengthErr = false;
            this.ckM_TextBox4.IsNumber = true;
            this.ckM_TextBox4.IsShop = false;
            this.ckM_TextBox4.Length = 10;
            this.ckM_TextBox4.Location = new System.Drawing.Point(233, 141);
            this.ckM_TextBox4.MaxLength = 10;
            this.ckM_TextBox4.MoveNext = false;
            this.ckM_TextBox4.Name = "ckM_TextBox4";
            this.ckM_TextBox4.Size = new System.Drawing.Size(100, 19);
            this.ckM_TextBox4.TabIndex = 4;
            this.ckM_TextBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox4.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_TextBox1.isMaxLengthErr = false;
            this.ckM_TextBox1.IsNumber = true;
            this.ckM_TextBox1.IsShop = false;
            this.ckM_TextBox1.Length = 10;
            this.ckM_TextBox1.Location = new System.Drawing.Point(102, 111);
            this.ckM_TextBox1.MaxLength = 10;
            this.ckM_TextBox1.MoveNext = false;
            this.ckM_TextBox1.Name = "ckM_TextBox1";
            this.ckM_TextBox1.Size = new System.Drawing.Size(100, 19);
            this.ckM_TextBox1.TabIndex = 0;
            this.ckM_TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_TextBox3
            // 
            this.ckM_TextBox3.AllowMinus = false;
            this.ckM_TextBox3.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox3.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox3.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox3.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox3.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.ckM_TextBox3.DecimalPlace = 0;
            this.ckM_TextBox3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox3.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox3.IntegerPart = 0;
            this.ckM_TextBox3.IsCorrectDate = true;
            this.ckM_TextBox3.isEnterKeyDown = false;
            this.ckM_TextBox3.isMaxLengthErr = false;
            this.ckM_TextBox3.IsNumber = true;
            this.ckM_TextBox3.IsShop = false;
            this.ckM_TextBox3.Length = 10;
            this.ckM_TextBox3.Location = new System.Drawing.Point(102, 141);
            this.ckM_TextBox3.MaxLength = 10;
            this.ckM_TextBox3.MoveNext = false;
            this.ckM_TextBox3.Name = "ckM_TextBox3";
            this.ckM_TextBox3.Size = new System.Drawing.Size(100, 19);
            this.ckM_TextBox3.TabIndex = 3;
            this.ckM_TextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox3.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_Label6.Location = new System.Drawing.Point(14, 319);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label6.TabIndex = 707;
            this.ckM_Label6.Text = "または";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label7
            // 
            this.ckM_Label7.AutoSize = true;
            this.ckM_Label7.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label7.DefaultlabelSize = true;
            this.ckM_Label7.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label7.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label7.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label7.Location = new System.Drawing.Point(100, 371);
            this.ckM_Label7.Name = "ckM_Label7";
            this.ckM_Label7.Size = new System.Drawing.Size(304, 12);
            this.ckM_Label7.TabIndex = 708;
            this.ckM_Label7.Text = "見積番号の指定がある場合、見積番号を優先します";
            this.ckM_Label7.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Mitsumorisyo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.ckM_Label7);
            this.Controls.Add(this.ckM_Label6);
            this.Controls.Add(this.CboStoreCD);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ckM_CustomerName);
            this.Controls.Add(this.ckM_RadioButton3);
            this.Controls.Add(this.ckM_TextBox5);
            this.Controls.Add(this.ScStaff);
            this.Controls.Add(this.ScCustomer);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblSkuCD);
            this.Controls.Add(this.ckM_Label8);
            this.Controls.Add(this.ckM_TextBox2);
            this.Controls.Add(this.ckM_Label2);
            this.Controls.Add(this.ckM_RadioButton2);
            this.Controls.Add(this.ckM_RadioButton1);
            this.Controls.Add(this.ckM_Label3);
            this.Controls.Add(this.ckM_Label4);
            this.Controls.Add(this.ckM_Label5);
            this.Controls.Add(this.ckM_TextBox4);
            this.Controls.Add(this.ckM_TextBox1);
            this.Controls.Add(this.ckM_TextBox3);
            this.Controls.Add(this.ckM_Label1);
            this.Controls.Add(this.ScMitsumoriNO);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "Mitsumorisyo";
            this.PanelHeaderHeight = 90;
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.ScMitsumoriNO, 0);
            this.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox3, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox1, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox4, 0);
            this.Controls.SetChildIndex(this.ckM_Label5, 0);
            this.Controls.SetChildIndex(this.ckM_Label4, 0);
            this.Controls.SetChildIndex(this.ckM_Label3, 0);
            this.Controls.SetChildIndex(this.ckM_RadioButton1, 0);
            this.Controls.SetChildIndex(this.ckM_RadioButton2, 0);
            this.Controls.SetChildIndex(this.ckM_Label2, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox2, 0);
            this.Controls.SetChildIndex(this.ckM_Label8, 0);
            this.Controls.SetChildIndex(this.lblSkuCD, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.label14, 0);
            this.Controls.SetChildIndex(this.ScCustomer, 0);
            this.Controls.SetChildIndex(this.ScStaff, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox5, 0);
            this.Controls.SetChildIndex(this.ckM_RadioButton3, 0);
            this.Controls.SetChildIndex(this.ckM_CustomerName, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.CboStoreCD, 0);
            this.Controls.SetChildIndex(this.ckM_Label6, 0);
            this.Controls.SetChildIndex(this.ckM_Label7, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CKM_Controls.CKM_Label label4;
        private CKM_Controls.CKM_Label label27;
        private CKM_Controls.CKM_Label label1;
        private Search.CKM_SearchControl ScMitsumoriNO;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_ComboBox CboStoreCD;
        private Search.CKM_SearchControl ckM_SearchControl2;
        private Search.CKM_SearchControl ckM_SearchControl3;
        private CKM_Controls.CKM_TextBox ckM_CustomerName;
        private CKM_Controls.CKM_RadioButton ckM_RadioButton3;
        private CKM_Controls.CKM_TextBox ckM_TextBox5;
        private Search.CKM_SearchControl ScStaff;
        private Search.CKM_SearchControl ScCustomer;
        private CKM_Controls.CKM_Label label14;
        private CKM_Controls.CKM_Label label9;
        private System.Windows.Forms.Label lblSkuCD;
        private CKM_Controls.CKM_Label ckM_Label8;
        private CKM_Controls.CKM_TextBox ckM_TextBox2;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_RadioButton ckM_RadioButton2;
        private CKM_Controls.CKM_RadioButton ckM_RadioButton1;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_TextBox ckM_TextBox4;
        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private CKM_Controls.CKM_TextBox ckM_TextBox3;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_Label ckM_Label7;
    }
}

