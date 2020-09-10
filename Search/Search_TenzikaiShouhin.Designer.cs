namespace Search
{
    partial class Search_TenzikaiShouhin
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.CB_Season = new CKM_Controls.CKM_ComboBox();
            this.CB_Year = new CKM_Controls.CKM_ComboBox();
            this.LB_Season = new CKM_Controls.CKM_Label();
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.Lb_brand = new CKM_Controls.CKM_Label();
            this.SC_Vendor = new Search.CKM_SearchControl();
            this.TB_Shohinmei = new System.Windows.Forms.Label();
            this.TB_tenzikainame = new CKM_Controls.CKM_Label();
            this.TB_SKUname = new System.Windows.Forms.Label();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.LB_ChangeDate = new System.Windows.Forms.Label();
            this.ckM_LB_Kijunbi = new CKM_Controls.CKM_Label();
            this.ckM_LB_Shinkitorokubi = new CKM_Controls.CKM_Label();
            this.TB_UpdateDateTimeF = new CKM_Controls.CKM_TextBox();
            this.TB_UpdateTimeT = new CKM_Controls.CKM_TextBox();
            this.ckM_LＢ_ShinkitorokuF = new CKM_Controls.CKM_Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.TB_InsertDateTimeF = new CKM_Controls.CKM_TextBox();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_SearchControl1 = new Search.CKM_SearchControl();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.SC_segment = new Search.CKM_SearchControl();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.BT_Display = new CKM_Controls.CKM_Button();
            this.TB_InsertDateTimeT = new CKM_Controls.CKM_TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckmShop_GridView1 = new CKM_Controls.CKMShop_GridView();
            this.tenzikaimei = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.skucd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jancd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shouhinmei = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.segment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.brand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vendorcd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.color = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.biko = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.insertdatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updatedatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PanelHeader.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckmShop_GridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.BT_Display);
            this.PanelHeader.Controls.Add(this.SC_segment);
            this.PanelHeader.Controls.Add(this.ckM_Label5);
            this.PanelHeader.Controls.Add(this.ckM_SearchControl1);
            this.PanelHeader.Controls.Add(this.ckM_Label4);
            this.PanelHeader.Controls.Add(this.ckM_Label2);
            this.PanelHeader.Controls.Add(this.TB_InsertDateTimeF);
            this.PanelHeader.Controls.Add(this.TB_InsertDateTimeT);
            this.PanelHeader.Controls.Add(this.ckM_Label3);
            this.PanelHeader.Controls.Add(this.ckM_LB_Shinkitorokubi);
            this.PanelHeader.Controls.Add(this.TB_UpdateDateTimeF);
            this.PanelHeader.Controls.Add(this.TB_UpdateTimeT);
            this.PanelHeader.Controls.Add(this.ckM_LＢ_ShinkitorokuF);
            this.PanelHeader.Controls.Add(this.LB_ChangeDate);
            this.PanelHeader.Controls.Add(this.ckM_LB_Kijunbi);
            this.PanelHeader.Controls.Add(this.TB_SKUname);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Controls.Add(this.TB_Shohinmei);
            this.PanelHeader.Controls.Add(this.TB_tenzikainame);
            this.PanelHeader.Controls.Add(this.CB_Season);
            this.PanelHeader.Controls.Add(this.CB_Year);
            this.PanelHeader.Controls.Add(this.SC_Vendor);
            this.PanelHeader.Controls.Add(this.LB_Season);
            this.PanelHeader.Controls.Add(this.ckM_Label10);
            this.PanelHeader.Controls.Add(this.Lb_brand);
            this.PanelHeader.Size = new System.Drawing.Size(1302, 178);
            this.PanelHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelHeader_Paint);
            this.PanelHeader.Controls.SetChildIndex(this.Lb_brand, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label10, 0);
            this.PanelHeader.Controls.SetChildIndex(this.LB_Season, 0);
            this.PanelHeader.Controls.SetChildIndex(this.SC_Vendor, 0);
            this.PanelHeader.Controls.SetChildIndex(this.CB_Year, 0);
            this.PanelHeader.Controls.SetChildIndex(this.CB_Season, 0);
            this.PanelHeader.Controls.SetChildIndex(this.TB_tenzikainame, 0);
            this.PanelHeader.Controls.SetChildIndex(this.TB_Shohinmei, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.TB_SKUname, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_LB_Kijunbi, 0);
            this.PanelHeader.Controls.SetChildIndex(this.LB_ChangeDate, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_LＢ_ShinkitorokuF, 0);
            this.PanelHeader.Controls.SetChildIndex(this.TB_UpdateTimeT, 0);
            this.PanelHeader.Controls.SetChildIndex(this.TB_UpdateDateTimeF, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_LB_Shinkitorokubi, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label3, 0);
            this.PanelHeader.Controls.SetChildIndex(this.TB_InsertDateTimeT, 0);
            this.PanelHeader.Controls.SetChildIndex(this.TB_InsertDateTimeF, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label4, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_SearchControl1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label5, 0);
            this.PanelHeader.Controls.SetChildIndex(this.SC_segment, 0);
            this.PanelHeader.Controls.SetChildIndex(this.BT_Display, 0);
            // 
            // CB_Season
            // 
            this.CB_Season.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.CB_Season.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Season.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.シーズン;
            this.CB_Season.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.CB_Season.Flag = 0;
            this.CB_Season.FormattingEnabled = true;
            this.CB_Season.Length = 10;
            this.CB_Season.Location = new System.Drawing.Point(272, 99);
            this.CB_Season.MaxLength = 10;
            this.CB_Season.MoveNext = true;
            this.CB_Season.Name = "CB_Season";
            this.CB_Season.Size = new System.Drawing.Size(90, 20);
            this.CB_Season.TabIndex = 71;
            // 
            // CB_Year
            // 
            this.CB_Year.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.CB_Year.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Year.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.年度;
            this.CB_Year.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.CB_Year.Flag = 0;
            this.CB_Year.FormattingEnabled = true;
            this.CB_Year.Length = 10;
            this.CB_Year.Location = new System.Drawing.Point(79, 99);
            this.CB_Year.MaxLength = 10;
            this.CB_Year.MoveNext = true;
            this.CB_Year.Name = "CB_Year";
            this.CB_Year.Size = new System.Drawing.Size(90, 20);
            this.CB_Year.TabIndex = 70;
            // 
            // LB_Season
            // 
            this.LB_Season.AutoSize = true;
            this.LB_Season.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.LB_Season.BackColor = System.Drawing.Color.Transparent;
            this.LB_Season.DefaultlabelSize = true;
            this.LB_Season.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.LB_Season.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.LB_Season.ForeColor = System.Drawing.Color.Black;
            this.LB_Season.Location = new System.Drawing.Point(212, 103);
            this.LB_Season.Name = "LB_Season";
            this.LB_Season.Size = new System.Drawing.Size(57, 12);
            this.LB_Season.TabIndex = 79;
            this.LB_Season.Text = "シーズン";
            this.LB_Season.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.LB_Season.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label10.Location = new System.Drawing.Point(32, 102);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label10.TabIndex = 77;
            this.ckM_Label10.Text = "年　度";
            this.ckM_Label10.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Lb_brand
            // 
            this.Lb_brand.AutoSize = true;
            this.Lb_brand.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.Lb_brand.BackColor = System.Drawing.Color.Transparent;
            this.Lb_brand.DefaultlabelSize = true;
            this.Lb_brand.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.Lb_brand.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.Lb_brand.ForeColor = System.Drawing.Color.Black;
            this.Lb_brand.Location = new System.Drawing.Point(32, 74);
            this.Lb_brand.Name = "Lb_brand";
            this.Lb_brand.Size = new System.Drawing.Size(44, 12);
            this.Lb_brand.TabIndex = 75;
            this.Lb_brand.Text = "仕入先";
            this.Lb_brand.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.Lb_brand.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SC_Vendor
            // 
            this.SC_Vendor.AutoSize = true;
            this.SC_Vendor.ChangeDate = "";
            this.SC_Vendor.ChangeDateWidth = 100;
            this.SC_Vendor.Code = "";
            this.SC_Vendor.CodeWidth = 100;
            this.SC_Vendor.CodeWidth1 = 100;
            this.SC_Vendor.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.SC_Vendor.DataCheck = false;
            this.SC_Vendor.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.SC_Vendor.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.SC_Vendor.IsCopy = false;
            this.SC_Vendor.LabelText = "";
            this.SC_Vendor.LabelVisible = true;
            this.SC_Vendor.Location = new System.Drawing.Point(79, 65);
            this.SC_Vendor.Margin = new System.Windows.Forms.Padding(0);
            this.SC_Vendor.Name = "SC_Vendor";
            this.SC_Vendor.NameWidth = 280;
            this.SC_Vendor.SearchEnable = true;
            this.SC_Vendor.Size = new System.Drawing.Size(414, 27);
            this.SC_Vendor.Stype = Search.CKM_SearchControl.SearchType.ブランド;
            this.SC_Vendor.TabIndex = 68;
            this.SC_Vendor.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.SC_Vendor.UseChangeDate = false;
            this.SC_Vendor.Value1 = null;
            this.SC_Vendor.Value2 = null;
            this.SC_Vendor.Value3 = null;
            // 
            // TB_Shohinmei
            // 
            this.TB_Shohinmei.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.TB_Shohinmei.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_Shohinmei.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TB_Shohinmei.Location = new System.Drawing.Point(79, 42);
            this.TB_Shohinmei.Name = "TB_Shohinmei";
            this.TB_Shohinmei.Size = new System.Drawing.Size(520, 19);
            this.TB_Shohinmei.TabIndex = 772;
            this.TB_Shohinmei.Text = "ＸＸＸＸＸＸＸＸＸ10ＸＸＸＸＸＸＸＸＸ20ＸＸＸＸＸＸＸＸＸ30ＸＸＸＸＸＸＸＸＸ40";
            this.TB_Shohinmei.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TB_tenzikainame
            // 
            this.TB_tenzikainame.AutoSize = true;
            this.TB_tenzikainame.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.TB_tenzikainame.BackColor = System.Drawing.Color.Transparent;
            this.TB_tenzikainame.DefaultlabelSize = true;
            this.TB_tenzikainame.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.TB_tenzikainame.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.TB_tenzikainame.ForeColor = System.Drawing.Color.Black;
            this.TB_tenzikainame.Location = new System.Drawing.Point(19, 45);
            this.TB_tenzikainame.Name = "TB_tenzikainame";
            this.TB_tenzikainame.Size = new System.Drawing.Size(57, 12);
            this.TB_tenzikainame.TabIndex = 771;
            this.TB_tenzikainame.Text = "展示会名";
            this.TB_tenzikainame.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.TB_tenzikainame.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TB_SKUname
            // 
            this.TB_SKUname.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.TB_SKUname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_SKUname.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TB_SKUname.Location = new System.Drawing.Point(79, 131);
            this.TB_SKUname.Name = "TB_SKUname";
            this.TB_SKUname.Size = new System.Drawing.Size(520, 19);
            this.TB_SKUname.TabIndex = 774;
            this.TB_SKUname.Text = "ＸＸＸＸＸＸＸＸＸ10ＸＸＸＸＸＸＸＸＸ20ＸＸＸＸＸＸＸＸＸ30ＸＸＸＸＸＸＸＸＸ40";
            this.TB_SKUname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ckM_Label1
            // 
            this.ckM_Label1.AutoSize = true;
            this.ckM_Label1.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label1.DefaultlabelSize = true;
            this.ckM_Label1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label1.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label1.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label1.Location = new System.Drawing.Point(32, 134);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label1.TabIndex = 773;
            this.ckM_Label1.Text = "商品名";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LB_ChangeDate
            // 
            this.LB_ChangeDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.LB_ChangeDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LB_ChangeDate.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_ChangeDate.Location = new System.Drawing.Point(79, 12);
            this.LB_ChangeDate.Name = "LB_ChangeDate";
            this.LB_ChangeDate.Size = new System.Drawing.Size(80, 19);
            this.LB_ChangeDate.TabIndex = 776;
            this.LB_ChangeDate.Text = "YYYY/MM/DD";
            this.LB_ChangeDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ckM_LB_Kijunbi
            // 
            this.ckM_LB_Kijunbi.AutoSize = true;
            this.ckM_LB_Kijunbi.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_LB_Kijunbi.BackColor = System.Drawing.Color.Transparent;
            this.ckM_LB_Kijunbi.DefaultlabelSize = true;
            this.ckM_LB_Kijunbi.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_LB_Kijunbi.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_LB_Kijunbi.ForeColor = System.Drawing.Color.Black;
            this.ckM_LB_Kijunbi.Location = new System.Drawing.Point(31, 16);
            this.ckM_LB_Kijunbi.Name = "ckM_LB_Kijunbi";
            this.ckM_LB_Kijunbi.Size = new System.Drawing.Size(44, 12);
            this.ckM_LB_Kijunbi.TabIndex = 775;
            this.ckM_LB_Kijunbi.Text = "基準日";
            this.ckM_LB_Kijunbi.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_LB_Kijunbi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_LB_Shinkitorokubi
            // 
            this.ckM_LB_Shinkitorokubi.AutoSize = true;
            this.ckM_LB_Shinkitorokubi.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_LB_Shinkitorokubi.BackColor = System.Drawing.Color.Transparent;
            this.ckM_LB_Shinkitorokubi.DefaultlabelSize = true;
            this.ckM_LB_Shinkitorokubi.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_LB_Shinkitorokubi.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_LB_Shinkitorokubi.ForeColor = System.Drawing.Color.Black;
            this.ckM_LB_Shinkitorokubi.Location = new System.Drawing.Point(627, 138);
            this.ckM_LB_Shinkitorokubi.Name = "ckM_LB_Shinkitorokubi";
            this.ckM_LB_Shinkitorokubi.Size = new System.Drawing.Size(70, 12);
            this.ckM_LB_Shinkitorokubi.TabIndex = 779;
            this.ckM_LB_Shinkitorokubi.Text = "最終変更日";
            this.ckM_LB_Shinkitorokubi.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_LB_Shinkitorokubi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TB_UpdateDateTimeF
            // 
            this.TB_UpdateDateTimeF.AllowMinus = false;
            this.TB_UpdateDateTimeF.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.TB_UpdateDateTimeF.BackColor = System.Drawing.Color.White;
            this.TB_UpdateDateTimeF.BorderColor = false;
            this.TB_UpdateDateTimeF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_UpdateDateTimeF.ClientColor = System.Drawing.Color.White;
            this.TB_UpdateDateTimeF.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.TB_UpdateDateTimeF.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.TB_UpdateDateTimeF.DecimalPlace = 0;
            this.TB_UpdateDateTimeF.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.TB_UpdateDateTimeF.IntegerPart = 0;
            this.TB_UpdateDateTimeF.IsCorrectDate = true;
            this.TB_UpdateDateTimeF.isEnterKeyDown = false;
            this.TB_UpdateDateTimeF.IsFirstTime = true;
            this.TB_UpdateDateTimeF.isMaxLengthErr = false;
            this.TB_UpdateDateTimeF.IsNumber = true;
            this.TB_UpdateDateTimeF.IsShop = false;
            this.TB_UpdateDateTimeF.Length = 10;
            this.TB_UpdateDateTimeF.Location = new System.Drawing.Point(700, 134);
            this.TB_UpdateDateTimeF.MaxLength = 10;
            this.TB_UpdateDateTimeF.MoveNext = true;
            this.TB_UpdateDateTimeF.Name = "TB_UpdateDateTimeF";
            this.TB_UpdateDateTimeF.Size = new System.Drawing.Size(100, 19);
            this.TB_UpdateDateTimeF.TabIndex = 777;
            this.TB_UpdateDateTimeF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_UpdateDateTimeF.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.TB_UpdateDateTimeF.UseColorSizMode = false;
            // 
            // TB_UpdateTimeT
            // 
            this.TB_UpdateTimeT.AllowMinus = false;
            this.TB_UpdateTimeT.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.TB_UpdateTimeT.BackColor = System.Drawing.Color.White;
            this.TB_UpdateTimeT.BorderColor = false;
            this.TB_UpdateTimeT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_UpdateTimeT.ClientColor = System.Drawing.Color.White;
            this.TB_UpdateTimeT.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.TB_UpdateTimeT.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.TB_UpdateTimeT.DecimalPlace = 0;
            this.TB_UpdateTimeT.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.TB_UpdateTimeT.IntegerPart = 0;
            this.TB_UpdateTimeT.IsCorrectDate = true;
            this.TB_UpdateTimeT.isEnterKeyDown = false;
            this.TB_UpdateTimeT.IsFirstTime = true;
            this.TB_UpdateTimeT.isMaxLengthErr = false;
            this.TB_UpdateTimeT.IsNumber = true;
            this.TB_UpdateTimeT.IsShop = false;
            this.TB_UpdateTimeT.Length = 10;
            this.TB_UpdateTimeT.Location = new System.Drawing.Point(824, 132);
            this.TB_UpdateTimeT.MaxLength = 10;
            this.TB_UpdateTimeT.MoveNext = true;
            this.TB_UpdateTimeT.Name = "TB_UpdateTimeT";
            this.TB_UpdateTimeT.Size = new System.Drawing.Size(100, 19);
            this.TB_UpdateTimeT.TabIndex = 778;
            this.TB_UpdateTimeT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_UpdateTimeT.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.TB_UpdateTimeT.UseColorSizMode = false;
            // 
            // ckM_LＢ_ShinkitorokuF
            // 
            this.ckM_LＢ_ShinkitorokuF.AutoSize = true;
            this.ckM_LＢ_ShinkitorokuF.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_LＢ_ShinkitorokuF.BackColor = System.Drawing.Color.Transparent;
            this.ckM_LＢ_ShinkitorokuF.DefaultlabelSize = true;
            this.ckM_LＢ_ShinkitorokuF.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_LＢ_ShinkitorokuF.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_LＢ_ShinkitorokuF.ForeColor = System.Drawing.Color.Black;
            this.ckM_LＢ_ShinkitorokuF.Location = new System.Drawing.Point(803, 138);
            this.ckM_LＢ_ShinkitorokuF.Name = "ckM_LＢ_ShinkitorokuF";
            this.ckM_LＢ_ShinkitorokuF.Size = new System.Drawing.Size(18, 12);
            this.ckM_LＢ_ShinkitorokuF.TabIndex = 780;
            this.ckM_LＢ_ShinkitorokuF.Text = "～";
            this.ckM_LＢ_ShinkitorokuF.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_LＢ_ShinkitorokuF.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label2.Location = new System.Drawing.Point(627, 107);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label2.TabIndex = 783;
            this.ckM_Label2.Text = "新規登録日";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ckM_Label2.Click += new System.EventHandler(this.ckM_Label2_Click);
            // 
            // TB_InsertDateTimeF
            // 
            this.TB_InsertDateTimeF.AllowMinus = false;
            this.TB_InsertDateTimeF.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.TB_InsertDateTimeF.BackColor = System.Drawing.Color.White;
            this.TB_InsertDateTimeF.BorderColor = false;
            this.TB_InsertDateTimeF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_InsertDateTimeF.ClientColor = System.Drawing.Color.White;
            this.TB_InsertDateTimeF.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.TB_InsertDateTimeF.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.TB_InsertDateTimeF.DecimalPlace = 0;
            this.TB_InsertDateTimeF.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.TB_InsertDateTimeF.IntegerPart = 0;
            this.TB_InsertDateTimeF.IsCorrectDate = true;
            this.TB_InsertDateTimeF.isEnterKeyDown = false;
            this.TB_InsertDateTimeF.IsFirstTime = true;
            this.TB_InsertDateTimeF.isMaxLengthErr = false;
            this.TB_InsertDateTimeF.IsNumber = true;
            this.TB_InsertDateTimeF.IsShop = false;
            this.TB_InsertDateTimeF.Length = 10;
            this.TB_InsertDateTimeF.Location = new System.Drawing.Point(700, 103);
            this.TB_InsertDateTimeF.MaxLength = 10;
            this.TB_InsertDateTimeF.MoveNext = true;
            this.TB_InsertDateTimeF.Name = "TB_InsertDateTimeF";
            this.TB_InsertDateTimeF.Size = new System.Drawing.Size(100, 19);
            this.TB_InsertDateTimeF.TabIndex = 781;
            this.TB_InsertDateTimeF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_InsertDateTimeF.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.TB_InsertDateTimeF.UseColorSizMode = false;
            this.TB_InsertDateTimeF.TextChanged += new System.EventHandler(this.TB_InsertDateTimeF_TextChanged);
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
            this.ckM_Label3.Location = new System.Drawing.Point(802, 107);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label3.TabIndex = 784;
            this.ckM_Label3.Text = "～";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ckM_Label3.Click += new System.EventHandler(this.ckM_Label3_Click);
            // 
            // ckM_SearchControl1
            // 
            this.ckM_SearchControl1.AutoSize = true;
            this.ckM_SearchControl1.ChangeDate = "";
            this.ckM_SearchControl1.ChangeDateWidth = 100;
            this.ckM_SearchControl1.Code = "";
            this.ckM_SearchControl1.CodeWidth = 100;
            this.ckM_SearchControl1.CodeWidth1 = 100;
            this.ckM_SearchControl1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_SearchControl1.DataCheck = false;
            this.ckM_SearchControl1.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ckM_SearchControl1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_SearchControl1.IsCopy = false;
            this.ckM_SearchControl1.LabelText = "";
            this.ckM_SearchControl1.LabelVisible = true;
            this.ckM_SearchControl1.Location = new System.Drawing.Point(699, 34);
            this.ckM_SearchControl1.Margin = new System.Windows.Forms.Padding(0);
            this.ckM_SearchControl1.Name = "ckM_SearchControl1";
            this.ckM_SearchControl1.NameWidth = 280;
            this.ckM_SearchControl1.SearchEnable = true;
            this.ckM_SearchControl1.Size = new System.Drawing.Size(414, 27);
            this.ckM_SearchControl1.Stype = Search.CKM_SearchControl.SearchType.ブランド;
            this.ckM_SearchControl1.TabIndex = 785;
            this.ckM_SearchControl1.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ckM_SearchControl1.UseChangeDate = false;
            this.ckM_SearchControl1.Value1 = null;
            this.ckM_SearchControl1.Value2 = null;
            this.ckM_SearchControl1.Value3 = null;
            // 
            // ckM_Label4
            // 
            this.ckM_Label4.AutoSize = true;
            this.ckM_Label4.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label4.DefaultlabelSize = true;
            this.ckM_Label4.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label4.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label4.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label4.Location = new System.Drawing.Point(626, 43);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label4.TabIndex = 786;
            this.ckM_Label4.Text = "ブランド";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SC_segment
            // 
            this.SC_segment.AutoSize = true;
            this.SC_segment.ChangeDate = "";
            this.SC_segment.ChangeDateWidth = 100;
            this.SC_segment.Code = "";
            this.SC_segment.CodeWidth = 100;
            this.SC_segment.CodeWidth1 = 100;
            this.SC_segment.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.SC_segment.DataCheck = false;
            this.SC_segment.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.SC_segment.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.SC_segment.IsCopy = false;
            this.SC_segment.LabelText = "";
            this.SC_segment.LabelVisible = true;
            this.SC_segment.Location = new System.Drawing.Point(699, 65);
            this.SC_segment.Margin = new System.Windows.Forms.Padding(0);
            this.SC_segment.Name = "SC_segment";
            this.SC_segment.NameWidth = 280;
            this.SC_segment.SearchEnable = true;
            this.SC_segment.Size = new System.Drawing.Size(414, 27);
            this.SC_segment.Stype = Search.CKM_SearchControl.SearchType.ブランド;
            this.SC_segment.TabIndex = 787;
            this.SC_segment.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.SC_segment.UseChangeDate = false;
            this.SC_segment.Value1 = null;
            this.SC_segment.Value2 = null;
            this.SC_segment.Value3 = null;
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
            this.ckM_Label5.Location = new System.Drawing.Point(626, 74);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label5.TabIndex = 788;
            this.ckM_Label5.Text = "セグメント";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.BT_Display.Location = new System.Drawing.Point(1176, 118);
            this.BT_Display.Margin = new System.Windows.Forms.Padding(1);
            this.BT_Display.Name = "BT_Display";
            this.BT_Display.Size = new System.Drawing.Size(118, 28);
            this.BT_Display.TabIndex = 789;
            this.BT_Display.Text = "表示(F11)";
            this.BT_Display.UseVisualStyleBackColor = false;
            // 
            // TB_InsertDateTimeT
            // 
            this.TB_InsertDateTimeT.AllowMinus = false;
            this.TB_InsertDateTimeT.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.TB_InsertDateTimeT.BackColor = System.Drawing.Color.White;
            this.TB_InsertDateTimeT.BorderColor = false;
            this.TB_InsertDateTimeT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_InsertDateTimeT.ClientColor = System.Drawing.Color.White;
            this.TB_InsertDateTimeT.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.TB_InsertDateTimeT.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.TB_InsertDateTimeT.DecimalPlace = 0;
            this.TB_InsertDateTimeT.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.TB_InsertDateTimeT.IntegerPart = 0;
            this.TB_InsertDateTimeT.IsCorrectDate = true;
            this.TB_InsertDateTimeT.isEnterKeyDown = false;
            this.TB_InsertDateTimeT.IsFirstTime = true;
            this.TB_InsertDateTimeT.isMaxLengthErr = false;
            this.TB_InsertDateTimeT.IsNumber = true;
            this.TB_InsertDateTimeT.IsShop = false;
            this.TB_InsertDateTimeT.Length = 10;
            this.TB_InsertDateTimeT.Location = new System.Drawing.Point(824, 103);
            this.TB_InsertDateTimeT.MaxLength = 10;
            this.TB_InsertDateTimeT.MoveNext = true;
            this.TB_InsertDateTimeT.Name = "TB_InsertDateTimeT";
            this.TB_InsertDateTimeT.Size = new System.Drawing.Size(100, 19);
            this.TB_InsertDateTimeT.TabIndex = 782;
            this.TB_InsertDateTimeT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_InsertDateTimeT.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.TB_InsertDateTimeT.UseColorSizMode = false;
            this.TB_InsertDateTimeT.TextChanged += new System.EventHandler(this.TB_InsertDateTimeT_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ckmShop_GridView1);
            this.panel1.Location = new System.Drawing.Point(1, 220);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1300, 460);
            this.panel1.TabIndex = 5;
            // 
            // ckmShop_GridView1
            // 
            this.ckmShop_GridView1.AllowUserToAddRows = false;
            this.ckmShop_GridView1.AlterBackColor = CKM_Controls.CKMShop_GridView.AltBackcolor.White;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.ckmShop_GridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ckmShop_GridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.ckmShop_GridView1.BackgroungColor = CKM_Controls.CKMShop_GridView.DBackcolor.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ckmShop_GridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ckmShop_GridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ckmShop_GridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tenzikaimei,
            this.skucd,
            this.jancd,
            this.shouhinmei,
            this.segment,
            this.brand,
            this.vendorcd,
            this.color,
            this.size,
            this.biko,
            this.insertdatetime,
            this.updatedatetime});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ckmShop_GridView1.DefaultCellStyle = dataGridViewCellStyle5;
            this.ckmShop_GridView1.DGVback = CKM_Controls.CKMShop_GridView.DGVBackcolor.White;
            this.ckmShop_GridView1.GVFontstyle = CKM_Controls.CKMShop_GridView.FontStyle_.Regular;
            this.ckmShop_GridView1.HeaderHeight_ = 22;
            this.ckmShop_GridView1.HeaderVisible = true;
            this.ckmShop_GridView1.Height_ = 200;
            this.ckmShop_GridView1.Location = new System.Drawing.Point(20, 6);
            this.ckmShop_GridView1.Name = "ckmShop_GridView1";
            this.ckmShop_GridView1.RowHeight_ = 20;
            this.ckmShop_GridView1.ShopFontSize = CKM_Controls.CKMShop_GridView.Font_.Normal;
            this.ckmShop_GridView1.Size = new System.Drawing.Size(1241, 400);
            this.ckmShop_GridView1.TabIndex = 0;
            this.ckmShop_GridView1.UseRowNo = true;
            this.ckmShop_GridView1.UseSetting = false;
            this.ckmShop_GridView1.Width_ = 200;
            // 
            // tenzikaimei
            // 
            this.tenzikaimei.HeaderText = "展示会名";
            this.tenzikaimei.Name = "tenzikaimei";
            // 
            // skucd
            // 
            this.skucd.HeaderText = "SKUCD";
            this.skucd.Name = "skucd";
            // 
            // jancd
            // 
            this.jancd.HeaderText = "JANCD";
            this.jancd.Name = "jancd";
            // 
            // shouhinmei
            // 
            this.shouhinmei.HeaderText = "商品名";
            this.shouhinmei.Name = "shouhinmei";
            // 
            // segment
            // 
            this.segment.HeaderText = "セグメント";
            this.segment.Name = "segment";
            // 
            // brand
            // 
            this.brand.HeaderText = "ブランド";
            this.brand.Name = "brand";
            // 
            // vendorcd
            // 
            this.vendorcd.HeaderText = "仕入先";
            this.vendorcd.Name = "vendorcd";
            // 
            // color
            // 
            this.color.HeaderText = "カラー";
            this.color.Name = "color";
            // 
            // size
            // 
            this.size.HeaderText = "セイズ";
            this.size.Name = "size";
            // 
            // biko
            // 
            this.biko.HeaderText = "備　考";
            this.biko.Name = "biko";
            // 
            // insertdatetime
            // 
            dataGridViewCellStyle3.Format = "d";
            dataGridViewCellStyle3.NullValue = null;
            this.insertdatetime.DefaultCellStyle = dataGridViewCellStyle3;
            this.insertdatetime.HeaderText = "新規登録日\t\t\t\t";
            this.insertdatetime.Name = "insertdatetime";
            // 
            // updatedatetime
            // 
            dataGridViewCellStyle4.Format = "d";
            dataGridViewCellStyle4.NullValue = null;
            this.updatedatetime.DefaultCellStyle = dataGridViewCellStyle4;
            this.updatedatetime.HeaderText = "最終変更日";
            this.updatedatetime.Name = "updatedatetime";
            // 
            // Search_TenzikaiShouhin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1302, 728);
            this.Controls.Add(this.panel1);
            this.F11Visible = true;
            this.F12Visible = true;
            this.F9Visible = true;
            this.Name = "Search_TenzikaiShouhin";
            this.PanelHeaderHeight = 220;
            this.Text = "展示会商品検索";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ckmShop_GridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private CKM_Controls.CKM_ComboBox CB_Season;
        private CKM_Controls.CKM_ComboBox CB_Year;
        private CKM_Controls.CKM_Label LB_Season;
        private CKM_Controls.CKM_Label ckM_Label10;
        private CKM_SearchControl SC_Vendor;
        private CKM_Controls.CKM_Label Lb_brand;
        private System.Windows.Forms.Label TB_SKUname;
        private CKM_Controls.CKM_Label ckM_Label1;
        private System.Windows.Forms.Label TB_Shohinmei;
        private CKM_Controls.CKM_Label TB_tenzikainame;
        private System.Windows.Forms.Label LB_ChangeDate;
        private CKM_Controls.CKM_Label ckM_LB_Kijunbi;
        private CKM_SearchControl SC_segment;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_SearchControl ckM_SearchControl1;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_TextBox TB_InsertDateTimeF;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Label ckM_LB_Shinkitorokubi;
        private CKM_Controls.CKM_TextBox TB_UpdateDateTimeF;
        private CKM_Controls.CKM_TextBox TB_UpdateTimeT;
        private CKM_Controls.CKM_Label ckM_LＢ_ShinkitorokuF;
        private CKM_Controls.CKM_Button BT_Display;
        private CKM_Controls.CKM_TextBox TB_InsertDateTimeT;
        private System.Windows.Forms.Panel panel1;
        private CKM_Controls.CKMShop_GridView ckmShop_GridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn tenzikaimei;
        private System.Windows.Forms.DataGridViewTextBoxColumn skucd;
        private System.Windows.Forms.DataGridViewTextBoxColumn jancd;
        private System.Windows.Forms.DataGridViewTextBoxColumn shouhinmei;
        private System.Windows.Forms.DataGridViewTextBoxColumn segment;
        private System.Windows.Forms.DataGridViewTextBoxColumn brand;
        private System.Windows.Forms.DataGridViewTextBoxColumn vendorcd;
        private System.Windows.Forms.DataGridViewTextBoxColumn color;
        private System.Windows.Forms.DataGridViewTextBoxColumn size;
        private System.Windows.Forms.DataGridViewTextBoxColumn biko;
        private System.Windows.Forms.DataGridViewTextBoxColumn insertdatetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn updatedatetime;
    }
}