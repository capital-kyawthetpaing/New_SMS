namespace MasterTouroku_TenzikaiHanbaiTankaKakeritu
{
    partial class FrmMasterTouroku_TenzikaiHanbaiTankaKakeritu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMasterTouroku_TenzikaiHanbaiTankaKakeritu));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TB_Rate = new CKM_Controls.CKM_TextBox();
            this.BT_Apply = new CKM_Controls.CKM_Button();
            this.BT_DeseletAll = new CKM_Controls.CKM_Button();
            this.BT_SelectAll = new CKM_Controls.CKM_Button();
            this.BT_Display = new CKM_Controls.CKM_Button();
            this.LB_Rate = new CKM_Controls.CKM_Label();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.CB_Season = new CKM_Controls.CKM_ComboBox();
            this.CB_Year = new CKM_Controls.CKM_ComboBox();
            this.TB_PriceOutTaxT = new CKM_Controls.CKM_TextBox();
            this.TB_PriceOutTaxF = new CKM_Controls.CKM_TextBox();
            this.Sc_Segment = new Search.CKM_SearchControl();
            this.SC_Brand = new Search.CKM_SearchControl();
            this.SC_Tanka = new Search.CKM_SearchControl();
            this.LB_Season = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.LB_segment = new CKM_Controls.CKM_Label();
            this.Lb_brand = new CKM_Controls.CKM_Label();
            this.LB_tanka = new CKM_Controls.CKM_Label();
            this.GV_Tenzaishohin = new CKM_Controls.CKM_GridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.colNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.BrandCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BrandName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SegmentCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SegmentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Year = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Season = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GV_Tenzaishohin)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1701, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(1167, 0);
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.TB_Rate);
            this.panel1.Controls.Add(this.BT_Apply);
            this.panel1.Controls.Add(this.BT_DeseletAll);
            this.panel1.Controls.Add(this.BT_SelectAll);
            this.panel1.Controls.Add(this.BT_Display);
            this.panel1.Controls.Add(this.LB_Rate);
            this.panel1.Controls.Add(this.ckM_Label1);
            this.panel1.Controls.Add(this.CB_Season);
            this.panel1.Controls.Add(this.CB_Year);
            this.panel1.Controls.Add(this.TB_PriceOutTaxT);
            this.panel1.Controls.Add(this.TB_PriceOutTaxF);
            this.panel1.Controls.Add(this.Sc_Segment);
            this.panel1.Controls.Add(this.SC_Brand);
            this.panel1.Controls.Add(this.SC_Tanka);
            this.panel1.Controls.Add(this.LB_Season);
            this.panel1.Controls.Add(this.ckM_Label3);
            this.panel1.Controls.Add(this.ckM_Label10);
            this.panel1.Controls.Add(this.LB_segment);
            this.panel1.Controls.Add(this.Lb_brand);
            this.panel1.Controls.Add(this.LB_tanka);
            this.panel1.Location = new System.Drawing.Point(2, 53);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1690, 160);
            this.panel1.TabIndex = 100;
            // 
            // TB_Rate
            // 
            this.TB_Rate.AllowMinus = false;
            this.TB_Rate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.TB_Rate.BackColor = System.Drawing.Color.White;
            this.TB_Rate.BorderColor = false;
            this.TB_Rate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_Rate.ClientColor = System.Drawing.SystemColors.Window;
            this.TB_Rate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.TB_Rate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Price;
            this.TB_Rate.DecimalPlace = 2;
            this.TB_Rate.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.TB_Rate.IntegerPart = 3;
            this.TB_Rate.IsCorrectDate = true;
            this.TB_Rate.isEnterKeyDown = false;
            this.TB_Rate.IsFirstTime = true;
            this.TB_Rate.isMaxLengthErr = false;
            this.TB_Rate.IsNumber = true;
            this.TB_Rate.IsShop = false;
            this.TB_Rate.Length = 6;
            this.TB_Rate.Location = new System.Drawing.Point(831, 127);
            this.TB_Rate.MaxLength = 6;
            this.TB_Rate.MoveNext = true;
            this.TB_Rate.Name = "TB_Rate";
            this.TB_Rate.Size = new System.Drawing.Size(78, 19);
            this.TB_Rate.TabIndex = 11;
            this.TB_Rate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TB_Rate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.TB_Rate.UseColorSizMode = false;
            // 
            // BT_Apply
            // 
            this.BT_Apply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BT_Apply.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BT_Apply.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BT_Apply.DefaultBtnSize = false;
            this.BT_Apply.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BT_Apply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Apply.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.BT_Apply.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BT_Apply.Location = new System.Drawing.Point(934, 126);
            this.BT_Apply.Margin = new System.Windows.Forms.Padding(1);
            this.BT_Apply.Name = "BT_Apply";
            this.BT_Apply.Size = new System.Drawing.Size(90, 23);
            this.BT_Apply.TabIndex = 12;
            this.BT_Apply.Text = "適用";
            this.BT_Apply.UseVisualStyleBackColor = false;
            this.BT_Apply.Click += new System.EventHandler(this.BT_Apply_Click);
            // 
            // BT_DeseletAll
            // 
            this.BT_DeseletAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BT_DeseletAll.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BT_DeseletAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BT_DeseletAll.DefaultBtnSize = false;
            this.BT_DeseletAll.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BT_DeseletAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_DeseletAll.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.BT_DeseletAll.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BT_DeseletAll.Location = new System.Drawing.Point(591, 123);
            this.BT_DeseletAll.Margin = new System.Windows.Forms.Padding(1);
            this.BT_DeseletAll.Name = "BT_DeseletAll";
            this.BT_DeseletAll.Size = new System.Drawing.Size(90, 23);
            this.BT_DeseletAll.TabIndex = 10;
            this.BT_DeseletAll.Text = "全解除";
            this.BT_DeseletAll.UseVisualStyleBackColor = false;
            this.BT_DeseletAll.Click += new System.EventHandler(this.BT_DeseletAll_Click);
            // 
            // BT_SelectAll
            // 
            this.BT_SelectAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BT_SelectAll.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BT_SelectAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BT_SelectAll.DefaultBtnSize = false;
            this.BT_SelectAll.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BT_SelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_SelectAll.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.BT_SelectAll.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BT_SelectAll.Location = new System.Drawing.Point(479, 123);
            this.BT_SelectAll.Margin = new System.Windows.Forms.Padding(1);
            this.BT_SelectAll.Name = "BT_SelectAll";
            this.BT_SelectAll.Size = new System.Drawing.Size(90, 23);
            this.BT_SelectAll.TabIndex = 9;
            this.BT_SelectAll.Text = "全選択";
            this.BT_SelectAll.UseVisualStyleBackColor = false;
            this.BT_SelectAll.Click += new System.EventHandler(this.BT_SelectAll_Click);
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
            this.BT_Display.Location = new System.Drawing.Point(365, 123);
            this.BT_Display.Margin = new System.Windows.Forms.Padding(1);
            this.BT_Display.Name = "BT_Display";
            this.BT_Display.Size = new System.Drawing.Size(90, 23);
            this.BT_Display.TabIndex = 8;
            this.BT_Display.Text = "表示";
            this.BT_Display.UseVisualStyleBackColor = false;
            this.BT_Display.Click += new System.EventHandler(this.BT_Display_Click);
            // 
            // LB_Rate
            // 
            this.LB_Rate.AutoSize = true;
            this.LB_Rate.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.LB_Rate.BackColor = System.Drawing.Color.Transparent;
            this.LB_Rate.DefaultlabelSize = true;
            this.LB_Rate.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.LB_Rate.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.LB_Rate.ForeColor = System.Drawing.Color.Black;
            this.LB_Rate.Location = new System.Drawing.Point(784, 131);
            this.LB_Rate.Name = "LB_Rate";
            this.LB_Rate.Size = new System.Drawing.Size(44, 12);
            this.LB_Rate.TabIndex = 72;
            this.LB_Rate.Text = "掛　率";
            this.LB_Rate.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.LB_Rate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label1
            // 
            this.ckM_Label1.AllowDrop = true;
            this.ckM_Label1.AutoSize = true;
            this.ckM_Label1.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label1.DefaultlabelSize = true;
            this.ckM_Label1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label1.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label1.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label1.Location = new System.Drawing.Point(194, 126);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label1.TabIndex = 66;
            this.ckM_Label1.Text = "～";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.CB_Season.Location = new System.Drawing.Point(280, 96);
            this.CB_Season.MaxLength = 10;
            this.CB_Season.MoveNext = true;
            this.CB_Season.Name = "CB_Season";
            this.CB_Season.Size = new System.Drawing.Size(90, 20);
            this.CB_Season.TabIndex = 5;
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
            this.CB_Year.Location = new System.Drawing.Point(87, 96);
            this.CB_Year.MaxLength = 10;
            this.CB_Year.MoveNext = true;
            this.CB_Year.Name = "CB_Year";
            this.CB_Year.Size = new System.Drawing.Size(90, 20);
            this.CB_Year.TabIndex = 4;
            // 
            // TB_PriceOutTaxT
            // 
            this.TB_PriceOutTaxT.AllowMinus = false;
            this.TB_PriceOutTaxT.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.TB_PriceOutTaxT.BackColor = System.Drawing.Color.White;
            this.TB_PriceOutTaxT.BorderColor = false;
            this.TB_PriceOutTaxT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_PriceOutTaxT.ClientColor = System.Drawing.SystemColors.Window;
            this.TB_PriceOutTaxT.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.TB_PriceOutTaxT.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Price;
            this.TB_PriceOutTaxT.DecimalPlace = 0;
            this.TB_PriceOutTaxT.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.TB_PriceOutTaxT.IntegerPart = 10;
            this.TB_PriceOutTaxT.IsCorrectDate = true;
            this.TB_PriceOutTaxT.isEnterKeyDown = false;
            this.TB_PriceOutTaxT.IsFirstTime = true;
            this.TB_PriceOutTaxT.isMaxLengthErr = false;
            this.TB_PriceOutTaxT.IsNumber = true;
            this.TB_PriceOutTaxT.IsShop = false;
            this.TB_PriceOutTaxT.Length = 11;
            this.TB_PriceOutTaxT.Location = new System.Drawing.Point(218, 123);
            this.TB_PriceOutTaxT.MaxLength = 11;
            this.TB_PriceOutTaxT.MoveNext = true;
            this.TB_PriceOutTaxT.Name = "TB_PriceOutTaxT";
            this.TB_PriceOutTaxT.Size = new System.Drawing.Size(100, 19);
            this.TB_PriceOutTaxT.TabIndex = 7;
            this.TB_PriceOutTaxT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TB_PriceOutTaxT.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.TB_PriceOutTaxT.UseColorSizMode = false;
            this.TB_PriceOutTaxT.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_PriceOutTaxT_KeyDown);
            // 
            // TB_PriceOutTaxF
            // 
            this.TB_PriceOutTaxF.AllowMinus = false;
            this.TB_PriceOutTaxF.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.TB_PriceOutTaxF.BackColor = System.Drawing.Color.White;
            this.TB_PriceOutTaxF.BorderColor = false;
            this.TB_PriceOutTaxF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_PriceOutTaxF.ClientColor = System.Drawing.SystemColors.Window;
            this.TB_PriceOutTaxF.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.TB_PriceOutTaxF.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Price;
            this.TB_PriceOutTaxF.DecimalPlace = 0;
            this.TB_PriceOutTaxF.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.TB_PriceOutTaxF.IntegerPart = 10;
            this.TB_PriceOutTaxF.IsCorrectDate = true;
            this.TB_PriceOutTaxF.isEnterKeyDown = false;
            this.TB_PriceOutTaxF.IsFirstTime = true;
            this.TB_PriceOutTaxF.isMaxLengthErr = false;
            this.TB_PriceOutTaxF.IsNumber = true;
            this.TB_PriceOutTaxF.IsShop = false;
            this.TB_PriceOutTaxF.Length = 11;
            this.TB_PriceOutTaxF.Location = new System.Drawing.Point(87, 123);
            this.TB_PriceOutTaxF.MaxLength = 11;
            this.TB_PriceOutTaxF.MoveNext = true;
            this.TB_PriceOutTaxF.Name = "TB_PriceOutTaxF";
            this.TB_PriceOutTaxF.Size = new System.Drawing.Size(100, 19);
            this.TB_PriceOutTaxF.TabIndex = 6;
            this.TB_PriceOutTaxF.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TB_PriceOutTaxF.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.TB_PriceOutTaxF.UseColorSizMode = false;
            // 
            // Sc_Segment
            // 
            this.Sc_Segment.AutoSize = true;
            this.Sc_Segment.ChangeDate = "";
            this.Sc_Segment.ChangeDateWidth = 100;
            this.Sc_Segment.Code = "";
            this.Sc_Segment.CodeWidth = 60;
            this.Sc_Segment.CodeWidth1 = 60;
            this.Sc_Segment.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.Sc_Segment.DataCheck = false;
            this.Sc_Segment.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.Sc_Segment.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.Sc_Segment.IsCopy = false;
            this.Sc_Segment.LabelText = "";
            this.Sc_Segment.LabelVisible = true;
            this.Sc_Segment.Location = new System.Drawing.Point(87, 64);
            this.Sc_Segment.Margin = new System.Windows.Forms.Padding(0);
            this.Sc_Segment.Name = "Sc_Segment";
            this.Sc_Segment.NameWidth = 250;
            this.Sc_Segment.SearchEnable = true;
            this.Sc_Segment.Size = new System.Drawing.Size(344, 27);
            this.Sc_Segment.Stype = Search.CKM_SearchControl.SearchType.商品分類;
            this.Sc_Segment.TabIndex = 3;
            this.Sc_Segment.test = null;
            this.Sc_Segment.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.Sc_Segment.UseChangeDate = false;
            this.Sc_Segment.Value1 = null;
            this.Sc_Segment.Value2 = null;
            this.Sc_Segment.Value3 = null;
            this.Sc_Segment.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.Sc_Segment_CodeKeyDownEvent);
            this.Sc_Segment.Enter += new System.EventHandler(this.Sc_Segment_Enter);
            // 
            // SC_Brand
            // 
            this.SC_Brand.AutoSize = true;
            this.SC_Brand.ChangeDate = "";
            this.SC_Brand.ChangeDateWidth = 100;
            this.SC_Brand.Code = "";
            this.SC_Brand.CodeWidth = 100;
            this.SC_Brand.CodeWidth1 = 100;
            this.SC_Brand.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.SC_Brand.DataCheck = false;
            this.SC_Brand.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.SC_Brand.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.SC_Brand.IsCopy = false;
            this.SC_Brand.LabelText = "";
            this.SC_Brand.LabelVisible = true;
            this.SC_Brand.Location = new System.Drawing.Point(87, 39);
            this.SC_Brand.Margin = new System.Windows.Forms.Padding(0);
            this.SC_Brand.Name = "SC_Brand";
            this.SC_Brand.NameWidth = 280;
            this.SC_Brand.SearchEnable = true;
            this.SC_Brand.Size = new System.Drawing.Size(414, 27);
            this.SC_Brand.Stype = Search.CKM_SearchControl.SearchType.ブランド;
            this.SC_Brand.TabIndex = 2;
            this.SC_Brand.test = null;
            this.SC_Brand.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.SC_Brand.UseChangeDate = false;
            this.SC_Brand.Value1 = null;
            this.SC_Brand.Value2 = null;
            this.SC_Brand.Value3 = null;
            this.SC_Brand.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.SC_Brand_CodeKeyDownEvent);
            this.SC_Brand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SC_Tanka_CodeKeyDownEvent);
            // 
            // SC_Tanka
            // 
            this.SC_Tanka.AutoSize = true;
            this.SC_Tanka.ChangeDate = "";
            this.SC_Tanka.ChangeDateWidth = 100;
            this.SC_Tanka.Code = "";
            this.SC_Tanka.CodeWidth = 100;
            this.SC_Tanka.CodeWidth1 = 100;
            this.SC_Tanka.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.SC_Tanka.DataCheck = false;
            this.SC_Tanka.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.SC_Tanka.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.SC_Tanka.IsCopy = false;
            this.SC_Tanka.LabelText = "";
            this.SC_Tanka.LabelVisible = true;
            this.SC_Tanka.Location = new System.Drawing.Point(87, 14);
            this.SC_Tanka.Margin = new System.Windows.Forms.Padding(0);
            this.SC_Tanka.Name = "SC_Tanka";
            this.SC_Tanka.NameWidth = 140;
            this.SC_Tanka.SearchEnable = true;
            this.SC_Tanka.Size = new System.Drawing.Size(274, 27);
            this.SC_Tanka.Stype = Search.CKM_SearchControl.SearchType.単価設定;
            this.SC_Tanka.TabIndex = 1;
            this.SC_Tanka.test = null;
            this.SC_Tanka.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.SC_Tanka.UseChangeDate = false;
            this.SC_Tanka.Value1 = null;
            this.SC_Tanka.Value2 = null;
            this.SC_Tanka.Value3 = null;
            this.SC_Tanka.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.SC_Tanka_CodeKeyDownEvent);
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
            this.LB_Season.Location = new System.Drawing.Point(220, 100);
            this.LB_Season.Name = "LB_Season";
            this.LB_Season.Size = new System.Drawing.Size(57, 12);
            this.LB_Season.TabIndex = 65;
            this.LB_Season.Text = "シーズン";
            this.LB_Season.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.LB_Season.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label3.Location = new System.Drawing.Point(26, 126);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label3.TabIndex = 64;
            this.ckM_Label3.Text = "上代単価";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label10.Location = new System.Drawing.Point(40, 99);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label10.TabIndex = 62;
            this.ckM_Label10.Text = "年　度";
            this.ckM_Label10.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LB_segment
            // 
            this.LB_segment.AutoSize = true;
            this.LB_segment.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.LB_segment.BackColor = System.Drawing.Color.Transparent;
            this.LB_segment.DefaultlabelSize = true;
            this.LB_segment.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.LB_segment.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.LB_segment.ForeColor = System.Drawing.Color.Black;
            this.LB_segment.Location = new System.Drawing.Point(14, 72);
            this.LB_segment.Name = "LB_segment";
            this.LB_segment.Size = new System.Drawing.Size(70, 12);
            this.LB_segment.TabIndex = 60;
            this.LB_segment.Text = "セグメント";
            this.LB_segment.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.LB_segment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.Lb_brand.Location = new System.Drawing.Point(13, 48);
            this.Lb_brand.Name = "Lb_brand";
            this.Lb_brand.Size = new System.Drawing.Size(71, 12);
            this.Lb_brand.TabIndex = 58;
            this.Lb_brand.Text = "ブランドCD";
            this.Lb_brand.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.Lb_brand.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LB_tanka
            // 
            this.LB_tanka.AutoSize = true;
            this.LB_tanka.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.LB_tanka.BackColor = System.Drawing.Color.Transparent;
            this.LB_tanka.DefaultlabelSize = true;
            this.LB_tanka.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.LB_tanka.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.LB_tanka.ForeColor = System.Drawing.Color.Black;
            this.LB_tanka.Location = new System.Drawing.Point(40, 22);
            this.LB_tanka.Name = "LB_tanka";
            this.LB_tanka.Size = new System.Drawing.Size(44, 12);
            this.LB_tanka.TabIndex = 56;
            this.LB_tanka.Text = "ランク";
            this.LB_tanka.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.LB_tanka.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GV_Tenzaishohin
            // 
            this.GV_Tenzaishohin.AllowUserToAddRows = false;
            this.GV_Tenzaishohin.AllowUserToDeleteRows = false;
            this.GV_Tenzaishohin.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.GV_Tenzaishohin.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.GV_Tenzaishohin.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GV_Tenzaishohin.CheckCol = ((System.Collections.ArrayList)(resources.GetObject("GV_Tenzaishohin.CheckCol")));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GV_Tenzaishohin.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.GV_Tenzaishohin.ColumnHeadersHeight = 25;
            this.GV_Tenzaishohin.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNo,
            this.CheckBox,
            this.BrandCD,
            this.BrandName,
            this.SegmentCD,
            this.SegmentName,
            this.Year,
            this.Season,
            this.Rate});
            this.GV_Tenzaishohin.EnableHeadersVisualStyles = false;
            this.GV_Tenzaishohin.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GV_Tenzaishohin.Location = new System.Drawing.Point(14, 7);
            this.GV_Tenzaishohin.Name = "GV_Tenzaishohin";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GV_Tenzaishohin.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.GV_Tenzaishohin.RowHeadersVisible = false;
            this.GV_Tenzaishohin.RowHeight_ = 20;
            this.GV_Tenzaishohin.RowTemplate.Height = 20;
            this.GV_Tenzaishohin.Size = new System.Drawing.Size(1010, 500);
            this.GV_Tenzaishohin.TabIndex = 77;
            this.GV_Tenzaishohin.UseRowNo = false;
            this.GV_Tenzaishohin.UseSetting = false;
            this.GV_Tenzaishohin.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.GV_Tenzaishohin_CellValidating);
            this.GV_Tenzaishohin.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.GV_Tenzaishohin_DataError);
            this.GV_Tenzaishohin.Paint += new System.Windows.Forms.PaintEventHandler(this.GV_Tenzaishohin_Paint);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.GV_Tenzaishohin);
            this.panel2.Location = new System.Drawing.Point(1, 212);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1690, 550);
            this.panel2.TabIndex = 101;
            // 
            // colNo
            // 
            this.colNo.DataPropertyName = "colNo";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = null;
            this.colNo.DefaultCellStyle = dataGridViewCellStyle3;
            this.colNo.HeaderText = "No";
            this.colNo.Name = "colNo";
            this.colNo.ReadOnly = true;
            this.colNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colNo.Width = 40;
            // 
            // CheckBox
            // 
            this.CheckBox.FalseValue = "0";
            this.CheckBox.HeaderText = "";
            this.CheckBox.Name = "CheckBox";
            this.CheckBox.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CheckBox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.CheckBox.TrueValue = "1";
            this.CheckBox.Width = 30;
            // 
            // BrandCD
            // 
            this.BrandCD.DataPropertyName = "BrandCD";
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            this.BrandCD.DefaultCellStyle = dataGridViewCellStyle4;
            this.BrandCD.HeaderText = "ブランド";
            this.BrandCD.Name = "BrandCD";
            this.BrandCD.ReadOnly = true;
            this.BrandCD.Width = 80;
            // 
            // BrandName
            // 
            this.BrandName.DataPropertyName = "BrandName";
            this.BrandName.HeaderText = "";
            this.BrandName.Name = "BrandName";
            this.BrandName.ReadOnly = true;
            this.BrandName.Width = 250;
            // 
            // SegmentCD
            // 
            this.SegmentCD.DataPropertyName = "SegmentCD";
            this.SegmentCD.HeaderText = "せグメト";
            this.SegmentCD.Name = "SegmentCD";
            this.SegmentCD.ReadOnly = true;
            this.SegmentCD.Width = 80;
            // 
            // SegmentName
            // 
            this.SegmentName.DataPropertyName = "SegmentName";
            this.SegmentName.HeaderText = "";
            this.SegmentName.Name = "SegmentName";
            this.SegmentName.ReadOnly = true;
            this.SegmentName.Width = 250;
            // 
            // Year
            // 
            this.Year.DataPropertyName = "LastYearTerm";
            this.Year.HeaderText = "年度";
            this.Year.Name = "Year";
            this.Year.ReadOnly = true;
            this.Year.Width = 70;
            // 
            // Season
            // 
            this.Season.DataPropertyName = "LastSeason";
            this.Season.HeaderText = "シーズン";
            this.Season.Name = "Season";
            this.Season.ReadOnly = true;
            this.Season.Width = 80;
            // 
            // Rate
            // 
            this.Rate.DataPropertyName = "Rate";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = "0";
            this.Rate.DefaultCellStyle = dataGridViewCellStyle5;
            this.Rate.HeaderText = "          掛率";
            this.Rate.MaxInputLength = 6;
            this.Rate.Name = "Rate";
            // 
            // FrmMasterTouroku_TenzikaiHanbaiTankaKakeritu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1703, 804);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.F10Visible = false;
            this.F2Visible = false;
            this.F4Visible = false;
            this.F5Visible = false;
            this.F7Visible = false;
            this.F8Visible = false;
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "FrmMasterTouroku_TenzikaiHanbaiTankaKakeritu";
            this.PanelHeaderHeight = 50;
            this.Text = "MasterTouroku_TenzikaiHanbaiTankaKakeritu";
            this.Load += new System.EventHandler(this.FrmMasterTouroku_TenzikaiHanbaiTankaKakeritu_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmMasterTouroku_TenzikaiHanbaiTankaKakeritu_KeyUp);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GV_Tenzaishohin)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_ComboBox CB_Season;
        private CKM_Controls.CKM_ComboBox CB_Year;
        private CKM_Controls.CKM_TextBox TB_PriceOutTaxT;
        private CKM_Controls.CKM_TextBox TB_PriceOutTaxF;
        private Search.CKM_SearchControl Sc_Segment;
        private Search.CKM_SearchControl SC_Brand;
        private Search.CKM_SearchControl SC_Tanka;
        private CKM_Controls.CKM_Label LB_Season;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Label ckM_Label10;
        private CKM_Controls.CKM_Label LB_segment;
        private CKM_Controls.CKM_Label Lb_brand;
        private CKM_Controls.CKM_Label LB_tanka;
        private CKM_Controls.CKM_Button BT_Apply;
        private CKM_Controls.CKM_Button BT_DeseletAll;
        private CKM_Controls.CKM_Button BT_SelectAll;
        private CKM_Controls.CKM_Button BT_Display;
        private CKM_Controls.CKM_Label LB_Rate;
        private CKM_Controls.CKM_GridView GV_Tenzaishohin;
        private System.Windows.Forms.Panel panel2;
        private CKM_Controls.CKM_TextBox TB_Rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn BrandCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn BrandName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SegmentCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn SegmentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Year;
        private System.Windows.Forms.DataGridViewTextBoxColumn Season;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rate;
    }
}