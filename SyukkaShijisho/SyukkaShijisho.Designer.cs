namespace SyukkaShijisho
{
    partial class SyukkaShijisho
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
            this.CboSouko = new CKM_Controls.CKM_ComboBox();
            this.ckM_SearchControl2 = new Search.CKM_SearchControl();
            this.ckM_SearchControl3 = new Search.CKM_SearchControl();
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.label11 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox1 = new CKM_Controls.CKM_TextBox();
            this.ckM_Label12 = new CKM_Controls.CKM_Label();
            this.ChkMihakko = new CKM_Controls.CKM_CheckBox();
            this.ChkNohinSeikyu = new CKM_Controls.CKM_CheckBox();
            this.ChkSaihakko = new CKM_Controls.CKM_CheckBox();
            this.CboCarrierCD = new CKM_Controls.CKM_ComboBox();
            this.ScInstructionNO = new Search.CKM_SearchControl();
            this.lblSkuCD = new System.Windows.Forms.Label();
            this.ckM_CheckBox5 = new CKM_Controls.CKM_CheckBox();
            this.ckM_CheckBox4 = new CKM_Controls.CKM_CheckBox();
            this.ckM_CheckBox3 = new CKM_Controls.CKM_CheckBox();
            this.ckM_CheckBox2 = new CKM_Controls.CKM_CheckBox();
            this.ckM_CheckBox1 = new CKM_Controls.CKM_CheckBox();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
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
            this.label4.Text = "倉庫";
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
            // CboSouko
            // 
            this.CboSouko.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.CboSouko.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CboSouko.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.出荷指示倉庫;
            this.CboSouko.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.CboSouko.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CboSouko.Flag = 0;
            this.CboSouko.FormattingEnabled = true;
            this.CboSouko.Length = 40;
            this.CboSouko.Location = new System.Drawing.Point(1060, 111);
            this.CboSouko.MaxLength = 20;
            this.CboSouko.MoveNext = false;
            this.CboSouko.Name = "CboSouko";
            this.CboSouko.Size = new System.Drawing.Size(280, 20);
            this.CboSouko.TabIndex = 0;
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
            // ckM_Label10
            // 
            this.ckM_Label10.AutoSize = true;
            this.ckM_Label10.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label10.DefaultlabelSize = true;
            this.ckM_Label10.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label10.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label10.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label10.Location = new System.Drawing.Point(51, 143);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label10.TabIndex = 741;
            this.ckM_Label10.Text = "印刷対象";
            this.ckM_Label10.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.label11.Location = new System.Drawing.Point(114, 178);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 12);
            this.label11.TabIndex = 737;
            this.label11.Text = "出荷予定日";
            this.label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_TextBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox1.IntegerPart = 0;
            this.ckM_TextBox1.IsCorrectDate = true;
            this.ckM_TextBox1.isEnterKeyDown = false;
            this.ckM_TextBox1.isMaxLengthErr = false;
            this.ckM_TextBox1.IsNumber = true;
            this.ckM_TextBox1.IsShop = false;
            this.ckM_TextBox1.Length = 10;
            this.ckM_TextBox1.Location = new System.Drawing.Point(186, 175);
            this.ckM_TextBox1.MaxLength = 10;
            this.ckM_TextBox1.MoveNext = true;
            this.ckM_TextBox1.Name = "ckM_TextBox1";
            this.ckM_TextBox1.Size = new System.Drawing.Size(88, 19);
            this.ckM_TextBox1.TabIndex = 2;
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
            this.ckM_Label12.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label12.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label12.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label12.Location = new System.Drawing.Point(114, 210);
            this.ckM_Label12.Name = "ckM_Label12";
            this.ckM_Label12.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label12.TabIndex = 739;
            this.ckM_Label12.Text = "配送会社";
            this.ckM_Label12.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ChkMihakko
            // 
            this.ChkMihakko.AutoSize = true;
            this.ChkMihakko.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ChkMihakko.Location = new System.Drawing.Point(114, 142);
            this.ChkMihakko.Name = "ChkMihakko";
            this.ChkMihakko.Size = new System.Drawing.Size(76, 16);
            this.ChkMihakko.TabIndex = 1;
            this.ChkMihakko.Text = "未発行分";
            this.ChkMihakko.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ChkMihakko.UseVisualStyleBackColor = true;
            this.ChkMihakko.CheckedChanged += new System.EventHandler(this.ChkMihakko_CheckedChanged);
            // 
            // ChkNohinSeikyu
            // 
            this.ChkNohinSeikyu.AutoSize = true;
            this.ChkNohinSeikyu.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ChkNohinSeikyu.Location = new System.Drawing.Point(114, 278);
            this.ChkNohinSeikyu.Name = "ChkNohinSeikyu";
            this.ChkNohinSeikyu.Size = new System.Drawing.Size(50, 16);
            this.ChkNohinSeikyu.TabIndex = 11;
            this.ChkNohinSeikyu.Text = "発行";
            this.ChkNohinSeikyu.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ChkNohinSeikyu.UseVisualStyleBackColor = true;
            // 
            // ChkSaihakko
            // 
            this.ChkSaihakko.AutoSize = true;
            this.ChkSaihakko.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ChkSaihakko.Location = new System.Drawing.Point(114, 242);
            this.ChkSaihakko.Name = "ChkSaihakko";
            this.ChkSaihakko.Size = new System.Drawing.Size(76, 16);
            this.ChkSaihakko.TabIndex = 9;
            this.ChkSaihakko.Text = "再発行分";
            this.ChkSaihakko.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ChkSaihakko.UseVisualStyleBackColor = true;
            this.ChkSaihakko.CheckedChanged += new System.EventHandler(this.ChkSaihakko_CheckedChanged);
            // 
            // CboCarrierCD
            // 
            this.CboCarrierCD.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.CboCarrierCD.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CboCarrierCD.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.配送会社;
            this.CboCarrierCD.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.CboCarrierCD.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CboCarrierCD.Flag = 0;
            this.CboCarrierCD.FormattingEnabled = true;
            this.CboCarrierCD.Length = 20;
            this.CboCarrierCD.Location = new System.Drawing.Point(186, 206);
            this.CboCarrierCD.MaxLength = 10;
            this.CboCarrierCD.MoveNext = false;
            this.CboCarrierCD.Name = "CboCarrierCD";
            this.CboCarrierCD.Size = new System.Drawing.Size(140, 20);
            this.CboCarrierCD.TabIndex = 8;
            // 
            // ScInstructionNO
            // 
            this.ScInstructionNO.AutoSize = true;
            this.ScInstructionNO.ChangeDate = "";
            this.ScInstructionNO.ChangeDateWidth = 100;
            this.ScInstructionNO.Code = "";
            this.ScInstructionNO.CodeWidth = 100;
            this.ScInstructionNO.CodeWidth1 = 100;
            this.ScInstructionNO.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScInstructionNO.DataCheck = true;
            this.ScInstructionNO.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ScInstructionNO.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ScInstructionNO.IsCopy = false;
            this.ScInstructionNO.LabelText = "";
            this.ScInstructionNO.LabelVisible = false;
            this.ScInstructionNO.Location = new System.Drawing.Point(332, 236);
            this.ScInstructionNO.Margin = new System.Windows.Forms.Padding(0);
            this.ScInstructionNO.Name = "ScInstructionNO";
            this.ScInstructionNO.NameWidth = 600;
            this.ScInstructionNO.SearchEnable = true;
            this.ScInstructionNO.Size = new System.Drawing.Size(133, 28);
            this.ScInstructionNO.Stype = Search.CKM_SearchControl.SearchType.出荷指示番号;
            this.ScInstructionNO.TabIndex = 10;
            this.ScInstructionNO.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScInstructionNO.UseChangeDate = false;
            this.ScInstructionNO.Value1 = null;
            this.ScInstructionNO.Value2 = null;
            this.ScInstructionNO.Value3 = null;
            // 
            // lblSkuCD
            // 
            this.lblSkuCD.AutoSize = true;
            this.lblSkuCD.BackColor = System.Drawing.Color.Transparent;
            this.lblSkuCD.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSkuCD.Location = new System.Drawing.Point(243, 244);
            this.lblSkuCD.Name = "lblSkuCD";
            this.lblSkuCD.Size = new System.Drawing.Size(83, 12);
            this.lblSkuCD.TabIndex = 748;
            this.lblSkuCD.Text = "出荷指示番号";
            this.lblSkuCD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_CheckBox5
            // 
            this.ckM_CheckBox5.AutoSize = true;
            this.ckM_CheckBox5.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_CheckBox5.Location = new System.Drawing.Point(722, 176);
            this.ckM_CheckBox5.Name = "ckM_CheckBox5";
            this.ckM_CheckBox5.Size = new System.Drawing.Size(89, 16);
            this.ckM_CheckBox5.TabIndex = 7;
            this.ckM_CheckBox5.Text = "店舗間移動";
            this.ckM_CheckBox5.UseVisualStyleBackColor = true;
            // 
            // ckM_CheckBox4
            // 
            this.ckM_CheckBox4.AutoSize = true;
            this.ckM_CheckBox4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_CheckBox4.Location = new System.Drawing.Point(630, 176);
            this.ckM_CheckBox4.Name = "ckM_CheckBox4";
            this.ckM_CheckBox4.Size = new System.Drawing.Size(76, 16);
            this.ckM_CheckBox4.TabIndex = 6;
            this.ckM_CheckBox4.Text = "即日出荷";
            this.ckM_CheckBox4.UseVisualStyleBackColor = true;
            // 
            // ckM_CheckBox3
            // 
            this.ckM_CheckBox3.AutoSize = true;
            this.ckM_CheckBox3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_CheckBox3.Location = new System.Drawing.Point(512, 176);
            this.ckM_CheckBox3.Name = "ckM_CheckBox3";
            this.ckM_CheckBox3.Size = new System.Drawing.Size(102, 16);
            this.ckM_CheckBox3.TabIndex = 5;
            this.ckM_CheckBox3.Text = "着指定日あり";
            this.ckM_CheckBox3.UseVisualStyleBackColor = true;
            // 
            // ckM_CheckBox2
            // 
            this.ckM_CheckBox2.AutoSize = true;
            this.ckM_CheckBox2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_CheckBox2.Location = new System.Drawing.Point(439, 176);
            this.ckM_CheckBox2.Name = "ckM_CheckBox2";
            this.ckM_CheckBox2.Size = new System.Drawing.Size(57, 16);
            this.ckM_CheckBox2.TabIndex = 4;
            this.ckM_CheckBox2.Text = " 至急";
            this.ckM_CheckBox2.UseVisualStyleBackColor = true;
            // 
            // ckM_CheckBox1
            // 
            this.ckM_CheckBox1.AutoSize = true;
            this.ckM_CheckBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_CheckBox1.Location = new System.Drawing.Point(333, 170);
            this.ckM_CheckBox1.Name = "ckM_CheckBox1";
            this.ckM_CheckBox1.Size = new System.Drawing.Size(90, 28);
            this.ckM_CheckBox1.TabIndex = 3;
            this.ckM_CheckBox1.Text = " 通常\r\n(右記以外)";
            this.ckM_CheckBox1.UseVisualStyleBackColor = true;
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
            this.ckM_Label1.Location = new System.Drawing.Point(38, 280);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label1.TabIndex = 754;
            this.ckM_Label1.Text = "納品請求書";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SyukkaShijisho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.ckM_Label1);
            this.Controls.Add(this.ckM_CheckBox5);
            this.Controls.Add(this.ckM_CheckBox4);
            this.Controls.Add(this.ckM_CheckBox3);
            this.Controls.Add(this.ckM_CheckBox2);
            this.Controls.Add(this.ckM_CheckBox1);
            this.Controls.Add(this.ChkMihakko);
            this.Controls.Add(this.ChkNohinSeikyu);
            this.Controls.Add(this.ChkSaihakko);
            this.Controls.Add(this.CboCarrierCD);
            this.Controls.Add(this.ScInstructionNO);
            this.Controls.Add(this.lblSkuCD);
            this.Controls.Add(this.ckM_Label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.ckM_TextBox1);
            this.Controls.Add(this.ckM_Label12);
            this.Controls.Add(this.CboSouko);
            this.Controls.Add(this.label4);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "SyukkaShijisho";
            this.PanelHeaderHeight = 90;
            this.Text = "SyukkaShijisho";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.CboSouko, 0);
            this.Controls.SetChildIndex(this.ckM_Label12, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox1, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.ckM_Label10, 0);
            this.Controls.SetChildIndex(this.lblSkuCD, 0);
            this.Controls.SetChildIndex(this.ScInstructionNO, 0);
            this.Controls.SetChildIndex(this.CboCarrierCD, 0);
            this.Controls.SetChildIndex(this.ChkSaihakko, 0);
            this.Controls.SetChildIndex(this.ChkNohinSeikyu, 0);
            this.Controls.SetChildIndex(this.ChkMihakko, 0);
            this.Controls.SetChildIndex(this.ckM_CheckBox1, 0);
            this.Controls.SetChildIndex(this.ckM_CheckBox2, 0);
            this.Controls.SetChildIndex(this.ckM_CheckBox3, 0);
            this.Controls.SetChildIndex(this.ckM_CheckBox4, 0);
            this.Controls.SetChildIndex(this.ckM_CheckBox5, 0);
            this.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CKM_Controls.CKM_Label label4;
        private CKM_Controls.CKM_Label label27;
        private CKM_Controls.CKM_Label label1;
        private CKM_Controls.CKM_ComboBox CboSouko;
        private Search.CKM_SearchControl ckM_SearchControl2;
        private Search.CKM_SearchControl ckM_SearchControl3;
        private CKM_Controls.CKM_Label ckM_Label10;
        private CKM_Controls.CKM_Label label11;
        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private CKM_Controls.CKM_Label ckM_Label12;
        private CKM_Controls.CKM_CheckBox ChkMihakko;
        private CKM_Controls.CKM_CheckBox ChkNohinSeikyu;
        private CKM_Controls.CKM_CheckBox ChkSaihakko;
        private CKM_Controls.CKM_ComboBox CboCarrierCD;
        private Search.CKM_SearchControl ScInstructionNO;
        private System.Windows.Forms.Label lblSkuCD;
        private CKM_Controls.CKM_CheckBox ckM_CheckBox5;
        private CKM_Controls.CKM_CheckBox ckM_CheckBox4;
        private CKM_Controls.CKM_CheckBox ckM_CheckBox3;
        private CKM_Controls.CKM_CheckBox ckM_CheckBox2;
        private CKM_Controls.CKM_CheckBox ckM_CheckBox1;
        private CKM_Controls.CKM_Label ckM_Label1;
    }
}

