namespace TenzikaiHacchuuJouhouShuturyoku
{
    partial class FrmTenzikaiHacchuuJouhouShuturyoku
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckM_Label11 = new CKM_Controls.CKM_Label();
            this.cboYear = new CKM_Controls.CKM_ComboBox();
            this.rdoProduct = new CKM_Controls.CKM_RadioButton();
            this.rdoCustomer = new CKM_Controls.CKM_RadioButton();
            this.ckM_Label9 = new CKM_Controls.CKM_Label();
            this.ScClient2 = new Search.CKM_SearchControl();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.ScSegmentCD = new Search.CKM_SearchControl();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.cboSeason = new CKM_Controls.CKM_ComboBox();
            this.ScClient1 = new Search.CKM_SearchControl();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ScBrandCD = new Search.CKM_SearchControl();
            this.Sc_BrandCD = new CKM_Controls.CKM_Label();
            this.ScSupplier = new Search.CKM_SearchControl();
            this.ckM_Label8 = new CKM_Controls.CKM_Label();
            this.txtExhibition = new CKM_Controls.CKM_TextBox();
            this.btnExhibition = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1711, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(1177, 0);
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtExhibition);
            this.panel1.Controls.Add(this.btnExhibition);
            this.panel1.Controls.Add(this.ckM_Label11);
            this.panel1.Controls.Add(this.cboYear);
            this.panel1.Controls.Add(this.rdoProduct);
            this.panel1.Controls.Add(this.rdoCustomer);
            this.panel1.Controls.Add(this.ckM_Label9);
            this.panel1.Controls.Add(this.ScClient2);
            this.panel1.Controls.Add(this.ckM_Label6);
            this.panel1.Controls.Add(this.ScSegmentCD);
            this.panel1.Controls.Add(this.ckM_Label5);
            this.panel1.Controls.Add(this.ckM_Label3);
            this.panel1.Controls.Add(this.cboSeason);
            this.panel1.Controls.Add(this.ScClient1);
            this.panel1.Controls.Add(this.ckM_Label2);
            this.panel1.Controls.Add(this.ScBrandCD);
            this.panel1.Controls.Add(this.Sc_BrandCD);
            this.panel1.Controls.Add(this.ScSupplier);
            this.panel1.Controls.Add(this.ckM_Label8);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1713, 889);
            this.panel1.TabIndex = 0;
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
            this.ckM_Label11.Location = new System.Drawing.Point(58, 74);
            this.ckM_Label11.Name = "ckM_Label11";
            this.ckM_Label11.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label11.TabIndex = 37;
            this.ckM_Label11.Text = "年　度";
            this.ckM_Label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboYear
            // 
            this.cboYear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboYear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboYear.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.年度;
            this.cboYear.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboYear.Flag = 0;
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Length = 10;
            this.cboYear.Location = new System.Drawing.Point(105, 70);
            this.cboYear.MaxLength = 5;
            this.cboYear.MoveNext = true;
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(90, 20);
            this.cboYear.TabIndex = 2;
            // 
            // rdoProduct
            // 
            this.rdoProduct.AutoSize = true;
            this.rdoProduct.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.rdoProduct.Location = new System.Drawing.Point(186, 206);
            this.rdoProduct.Name = "rdoProduct";
            this.rdoProduct.Size = new System.Drawing.Size(62, 16);
            this.rdoProduct.TabIndex = 9;
            this.rdoProduct.TabStop = true;
            this.rdoProduct.Text = "商品別";
            this.rdoProduct.UseVisualStyleBackColor = true;
            // 
            // rdoCustomer
            // 
            this.rdoCustomer.AutoSize = true;
            this.rdoCustomer.Checked = true;
            this.rdoCustomer.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.rdoCustomer.Location = new System.Drawing.Point(43, 205);
            this.rdoCustomer.Name = "rdoCustomer";
            this.rdoCustomer.Size = new System.Drawing.Size(75, 16);
            this.rdoCustomer.TabIndex = 8;
            this.rdoCustomer.TabStop = true;
            this.rdoCustomer.Text = "得意先別";
            this.rdoCustomer.UseVisualStyleBackColor = true;
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
            this.ckM_Label9.Location = new System.Drawing.Point(257, 180);
            this.ckM_Label9.Name = "ckM_Label9";
            this.ckM_Label9.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label9.TabIndex = 32;
            this.ckM_Label9.Text = "～";
            this.ckM_Label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScClient2
            // 
            this.ScClient2.AutoSize = true;
            this.ScClient2.ChangeDate = "";
            this.ScClient2.ChangeDateWidth = 100;
            this.ScClient2.Code = "";
            this.ScClient2.CodeWidth = 100;
            this.ScClient2.CodeWidth1 = 100;
            this.ScClient2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScClient2.DataCheck = false;
            this.ScClient2.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScClient2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScClient2.IsCopy = false;
            this.ScClient2.LabelText = "";
            this.ScClient2.LabelVisible = false;
            this.ScClient2.Location = new System.Drawing.Point(297, 172);
            this.ScClient2.Margin = new System.Windows.Forms.Padding(0);
            this.ScClient2.Name = "ScClient2";
            this.ScClient2.NameWidth = 500;
            this.ScClient2.SearchEnable = true;
            this.ScClient2.Size = new System.Drawing.Size(133, 27);
            this.ScClient2.Stype = Search.CKM_SearchControl.SearchType.得意先;
            this.ScClient2.TabIndex = 7;
            this.ScClient2.test = null;
            this.ScClient2.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScClient2.UseChangeDate = false;
            this.ScClient2.Value1 = null;
            this.ScClient2.Value2 = null;
            this.ScClient2.Value3 = null;
            this.ScClient2.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScClient2_CodeKeyDownEvent);
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
            this.ckM_Label6.Location = new System.Drawing.Point(45, 20);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label6.TabIndex = 29;
            this.ckM_Label6.Text = "展示会名";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScSegmentCD
            // 
            this.ScSegmentCD.AutoSize = true;
            this.ScSegmentCD.ChangeDate = "";
            this.ScSegmentCD.ChangeDateWidth = 100;
            this.ScSegmentCD.Code = "";
            this.ScSegmentCD.CodeWidth = 60;
            this.ScSegmentCD.CodeWidth1 = 60;
            this.ScSegmentCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScSegmentCD.DataCheck = false;
            this.ScSegmentCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScSegmentCD.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScSegmentCD.IsCopy = false;
            this.ScSegmentCD.LabelText = "";
            this.ScSegmentCD.LabelVisible = true;
            this.ScSegmentCD.Location = new System.Drawing.Point(105, 146);
            this.ScSegmentCD.Margin = new System.Windows.Forms.Padding(0);
            this.ScSegmentCD.Name = "ScSegmentCD";
            this.ScSegmentCD.NameWidth = 250;
            this.ScSegmentCD.SearchEnable = true;
            this.ScSegmentCD.Size = new System.Drawing.Size(344, 27);
            this.ScSegmentCD.Stype = Search.CKM_SearchControl.SearchType.商品分類;
            this.ScSegmentCD.TabIndex = 5;
            this.ScSegmentCD.test = null;
            this.ScSegmentCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScSegmentCD.UseChangeDate = false;
            this.ScSegmentCD.Value1 = null;
            this.ScSegmentCD.Value2 = null;
            this.ScSegmentCD.Value3 = null;
            this.ScSegmentCD.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScSegmentCD_CodeKeyDownEvent);
            this.ScSegmentCD.Enter += new System.EventHandler(this.ScSegmentCD_Enter);
            // 
            // ckM_Label5
            // 
            this.ckM_Label5.AutoSize = true;
            this.ckM_Label5.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label5.DefaultlabelSize = true;
            this.ckM_Label5.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label5.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label5.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label5.Location = new System.Drawing.Point(32, 154);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label5.TabIndex = 27;
            this.ckM_Label5.Text = "セグメント";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label3
            // 
            this.ckM_Label3.AutoSize = true;
            this.ckM_Label3.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label3.DefaultlabelSize = true;
            this.ckM_Label3.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label3.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label3.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label3.Location = new System.Drawing.Point(46, 102);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label3.TabIndex = 25;
            this.ckM_Label3.Text = "シーズン";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboSeason
            // 
            this.cboSeason.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboSeason.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSeason.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.シーズン;
            this.cboSeason.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboSeason.Flag = 0;
            this.cboSeason.FormattingEnabled = true;
            this.cboSeason.Length = 10;
            this.cboSeason.Location = new System.Drawing.Point(106, 98);
            this.cboSeason.MaxLength = 5;
            this.cboSeason.MoveNext = true;
            this.cboSeason.Name = "cboSeason";
            this.cboSeason.Size = new System.Drawing.Size(90, 20);
            this.cboSeason.TabIndex = 3;
            // 
            // ScClient1
            // 
            this.ScClient1.AutoSize = true;
            this.ScClient1.ChangeDate = "";
            this.ScClient1.ChangeDateWidth = 100;
            this.ScClient1.Code = "";
            this.ScClient1.CodeWidth = 100;
            this.ScClient1.CodeWidth1 = 100;
            this.ScClient1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScClient1.DataCheck = false;
            this.ScClient1.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScClient1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScClient1.IsCopy = false;
            this.ScClient1.LabelText = "";
            this.ScClient1.LabelVisible = false;
            this.ScClient1.Location = new System.Drawing.Point(105, 172);
            this.ScClient1.Margin = new System.Windows.Forms.Padding(0);
            this.ScClient1.Name = "ScClient1";
            this.ScClient1.NameWidth = 500;
            this.ScClient1.SearchEnable = true;
            this.ScClient1.Size = new System.Drawing.Size(133, 27);
            this.ScClient1.Stype = Search.CKM_SearchControl.SearchType.得意先;
            this.ScClient1.TabIndex = 6;
            this.ScClient1.test = null;
            this.ScClient1.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScClient1.UseChangeDate = false;
            this.ScClient1.Value1 = null;
            this.ScClient1.Value2 = null;
            this.ScClient1.Value3 = null;
            // 
            // ckM_Label2
            // 
            this.ckM_Label2.AutoSize = true;
            this.ckM_Label2.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label2.DefaultlabelSize = true;
            this.ckM_Label2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label2.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label2.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label2.Location = new System.Drawing.Point(58, 180);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label2.TabIndex = 21;
            this.ckM_Label2.Text = "顧　客";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScBrandCD
            // 
            this.ScBrandCD.AutoSize = true;
            this.ScBrandCD.ChangeDate = "";
            this.ScBrandCD.ChangeDateWidth = 100;
            this.ScBrandCD.Code = "";
            this.ScBrandCD.CodeWidth = 100;
            this.ScBrandCD.CodeWidth1 = 100;
            this.ScBrandCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScBrandCD.DataCheck = false;
            this.ScBrandCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScBrandCD.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScBrandCD.IsCopy = false;
            this.ScBrandCD.LabelText = "";
            this.ScBrandCD.LabelVisible = true;
            this.ScBrandCD.Location = new System.Drawing.Point(105, 120);
            this.ScBrandCD.Margin = new System.Windows.Forms.Padding(0);
            this.ScBrandCD.Name = "ScBrandCD";
            this.ScBrandCD.NameWidth = 280;
            this.ScBrandCD.SearchEnable = true;
            this.ScBrandCD.Size = new System.Drawing.Size(414, 27);
            this.ScBrandCD.Stype = Search.CKM_SearchControl.SearchType.ブランド;
            this.ScBrandCD.TabIndex = 4;
            this.ScBrandCD.test = null;
            this.ScBrandCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScBrandCD.UseChangeDate = false;
            this.ScBrandCD.Value1 = null;
            this.ScBrandCD.Value2 = null;
            this.ScBrandCD.Value3 = null;
            this.ScBrandCD.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScBrandCD_CodeKeyDownEvent);
            // 
            // Sc_BrandCD
            // 
            this.Sc_BrandCD.AutoSize = true;
            this.Sc_BrandCD.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.Sc_BrandCD.BackColor = System.Drawing.Color.Transparent;
            this.Sc_BrandCD.DefaultlabelSize = true;
            this.Sc_BrandCD.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.Sc_BrandCD.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.Sc_BrandCD.ForeColor = System.Drawing.Color.Black;
            this.Sc_BrandCD.Location = new System.Drawing.Point(31, 128);
            this.Sc_BrandCD.Name = "Sc_BrandCD";
            this.Sc_BrandCD.Size = new System.Drawing.Size(71, 12);
            this.Sc_BrandCD.TabIndex = 19;
            this.Sc_BrandCD.Text = "ブランドCD";
            this.Sc_BrandCD.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.Sc_BrandCD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScSupplier
            // 
            this.ScSupplier.AutoSize = true;
            this.ScSupplier.ChangeDate = "";
            this.ScSupplier.ChangeDateWidth = 100;
            this.ScSupplier.Code = "";
            this.ScSupplier.CodeWidth = 100;
            this.ScSupplier.CodeWidth1 = 100;
            this.ScSupplier.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScSupplier.DataCheck = false;
            this.ScSupplier.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScSupplier.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScSupplier.IsCopy = false;
            this.ScSupplier.LabelText = "";
            this.ScSupplier.LabelVisible = true;
            this.ScSupplier.Location = new System.Drawing.Point(106, 39);
            this.ScSupplier.Margin = new System.Windows.Forms.Padding(0);
            this.ScSupplier.Name = "ScSupplier";
            this.ScSupplier.NameWidth = 310;
            this.ScSupplier.SearchEnable = true;
            this.ScSupplier.Size = new System.Drawing.Size(444, 27);
            this.ScSupplier.Stype = Search.CKM_SearchControl.SearchType.仕入先;
            this.ScSupplier.TabIndex = 1;
            this.ScSupplier.test = null;
            this.ScSupplier.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScSupplier.UseChangeDate = false;
            this.ScSupplier.Value1 = null;
            this.ScSupplier.Value2 = null;
            this.ScSupplier.Value3 = null;
            this.ScSupplier.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScSupplier_CodeKeyDownEvent);
            // 
            // ckM_Label8
            // 
            this.ckM_Label8.AutoSize = true;
            this.ckM_Label8.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label8.DefaultlabelSize = true;
            this.ckM_Label8.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label8.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label8.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label8.Location = new System.Drawing.Point(59, 47);
            this.ckM_Label8.Name = "ckM_Label8";
            this.ckM_Label8.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label8.TabIndex = 17;
            this.ckM_Label8.Text = "仕入先";
            this.ckM_Label8.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtExhibition
            // 
            this.txtExhibition.AllowMinus = false;
            this.txtExhibition.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtExhibition.BackColor = System.Drawing.Color.White;
            this.txtExhibition.BorderColor = false;
            this.txtExhibition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExhibition.ClientColor = System.Drawing.Color.White;
            this.txtExhibition.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtExhibition.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtExhibition.DecimalPlace = 0;
            this.txtExhibition.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtExhibition.IntegerPart = 0;
            this.txtExhibition.IsCorrectDate = true;
            this.txtExhibition.isEnterKeyDown = false;
            this.txtExhibition.IsFirstTime = true;
            this.txtExhibition.isMaxLengthErr = false;
            this.txtExhibition.IsNumber = true;
            this.txtExhibition.IsShop = false;
            this.txtExhibition.Length = 80;
            this.txtExhibition.Location = new System.Drawing.Point(107, 17);
            this.txtExhibition.MaxLength = 80;
            this.txtExhibition.MoveNext = true;
            this.txtExhibition.Name = "txtExhibition";
            this.txtExhibition.Size = new System.Drawing.Size(480, 19);
            this.txtExhibition.TabIndex = 0;
            this.txtExhibition.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtExhibition.UseColorSizMode = false;
            this.txtExhibition.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtExhibition_KeyDown);
            // 
            // btnExhibition
            // 
            this.btnExhibition.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnExhibition.Location = new System.Drawing.Point(586, 15);
            this.btnExhibition.Margin = new System.Windows.Forms.Padding(0);
            this.btnExhibition.Name = "btnExhibition";
            this.btnExhibition.Size = new System.Drawing.Size(33, 23);
            this.btnExhibition.TabIndex = 41;
            this.btnExhibition.TabStop = false;
            this.btnExhibition.Text = "･･･";
            this.btnExhibition.UseVisualStyleBackColor = true;
            this.btnExhibition.Click += new System.EventHandler(this.btnExhibition_Click);
            // 
            // FrmTenzikaiHacchuuJouhouShuturyoku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.panel1);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "FrmTenzikaiHacchuuJouhouShuturyoku";
            this.PanelHeaderHeight = 40;
            this.Text = "TenzikaiHacchuuJouhouShuturyoku";
            this.Load += new System.EventHandler(this.FrmTenzikaiHacchuuJouhouShuturyoku_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmTenzikaiHacchuuJouhouShuturyoku_KeyUp);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Search.CKM_SearchControl ScClient1;
        private CKM_Controls.CKM_Label ckM_Label2;
        private Search.CKM_SearchControl ScBrandCD;
        private CKM_Controls.CKM_Label Sc_BrandCD;
        private Search.CKM_SearchControl ScSupplier;
        private CKM_Controls.CKM_Label ckM_Label8;
        private Search.CKM_SearchControl ScClient2;
        private CKM_Controls.CKM_Label ckM_Label6;
        private Search.CKM_SearchControl ScSegmentCD;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_ComboBox cboSeason;
        private CKM_Controls.CKM_Label ckM_Label9;
        private CKM_Controls.CKM_RadioButton rdoProduct;
        private CKM_Controls.CKM_RadioButton rdoCustomer;
        private CKM_Controls.CKM_Label ckM_Label11;
        private CKM_Controls.CKM_ComboBox cboYear;
        private CKM_Controls.CKM_TextBox txtExhibition;
        private System.Windows.Forms.Button btnExhibition;
    }
}