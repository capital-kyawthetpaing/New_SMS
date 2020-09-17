namespace ZaikoMotochouInsatsu
{
    partial class ZaikoMotochouInsatsu
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
            this.panelDetail = new System.Windows.Forms.Panel();
            this.scJANCD = new Search.CKM_SearchControl();
            this.scSKUCD = new Search.CKM_SearchControl();
            this.scITEM = new Search.CKM_SearchControl();
            this.scMakerShohinCD = new Search.CKM_SearchControl();
            this.rdoMakerShohinCD = new CKM_Controls.CKM_RadioButton();
            this.rdoITEM = new CKM_Controls.CKM_RadioButton();
            this.chkPrintRelated = new CKM_Controls.CKM_CheckBox();
            this.txtSKUName = new CKM_Controls.CKM_TextBox();
            this.cboSouko = new CKM_Controls.CKM_ComboBox();
            this.txtTargetPeriodT = new CKM_Controls.CKM_TextBox();
            this.txtTargetPeriodF = new CKM_Controls.CKM_TextBox();
            this.ckM_Label8 = new CKM_Controls.CKM_Label();
            this.ckM_Label7 = new CKM_Controls.CKM_Label();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.panelDetail.SuspendLayout();
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
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.scJANCD);
            this.panelDetail.Controls.Add(this.scSKUCD);
            this.panelDetail.Controls.Add(this.scITEM);
            this.panelDetail.Controls.Add(this.scMakerShohinCD);
            this.panelDetail.Controls.Add(this.rdoMakerShohinCD);
            this.panelDetail.Controls.Add(this.rdoITEM);
            this.panelDetail.Controls.Add(this.chkPrintRelated);
            this.panelDetail.Controls.Add(this.txtSKUName);
            this.panelDetail.Controls.Add(this.cboSouko);
            this.panelDetail.Controls.Add(this.txtTargetPeriodT);
            this.panelDetail.Controls.Add(this.txtTargetPeriodF);
            this.panelDetail.Controls.Add(this.ckM_Label8);
            this.panelDetail.Controls.Add(this.ckM_Label7);
            this.panelDetail.Controls.Add(this.ckM_Label6);
            this.panelDetail.Controls.Add(this.ckM_Label5);
            this.panelDetail.Controls.Add(this.ckM_Label4);
            this.panelDetail.Controls.Add(this.ckM_Label3);
            this.panelDetail.Controls.Add(this.ckM_Label2);
            this.panelDetail.Controls.Add(this.ckM_Label1);
            this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetail.Location = new System.Drawing.Point(0, 40);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(1713, 889);
            this.panelDetail.TabIndex = 1;
            // 
            // scJANCD
            // 
            this.scJANCD.AutoSize = true;
            this.scJANCD.ChangeDate = "";
            this.scJANCD.ChangeDateWidth = 100;
            this.scJANCD.Code = "";
            this.scJANCD.CodeWidth = 600;
            this.scJANCD.CodeWidth1 = 600;
            this.scJANCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.scJANCD.DataCheck = false;
            this.scJANCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.scJANCD.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.scJANCD.IsCopy = false;
            this.scJANCD.LabelText = "";
            this.scJANCD.LabelVisible = false;
            this.scJANCD.Location = new System.Drawing.Point(150, 167);
            this.scJANCD.Margin = new System.Windows.Forms.Padding(0);
            this.scJANCD.Name = "scJANCD";
            this.scJANCD.NameWidth = 280;
            this.scJANCD.SearchEnable = true;
            this.scJANCD.Size = new System.Drawing.Size(633, 27);
            this.scJANCD.Stype = Search.CKM_SearchControl.SearchType.JANMulti;
            this.scJANCD.TabIndex = 5;
            this.scJANCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.scJANCD.UseChangeDate = false;
            this.scJANCD.Value1 = null;
            this.scJANCD.Value2 = null;
            this.scJANCD.Value3 = null;
            // 
            // scSKUCD
            // 
            this.scSKUCD.AutoSize = true;
            this.scSKUCD.ChangeDate = "";
            this.scSKUCD.ChangeDateWidth = 100;
            this.scSKUCD.Code = "";
            this.scSKUCD.CodeWidth = 190;
            this.scSKUCD.CodeWidth1 = 190;
            this.scSKUCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.scSKUCD.DataCheck = false;
            this.scSKUCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.scSKUCD.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.scSKUCD.IsCopy = false;
            this.scSKUCD.LabelText = "";
            this.scSKUCD.LabelVisible = false;
            this.scSKUCD.Location = new System.Drawing.Point(150, 143);
            this.scSKUCD.Margin = new System.Windows.Forms.Padding(0);
            this.scSKUCD.Name = "scSKUCD";
            this.scSKUCD.NameWidth = 350;
            this.scSKUCD.SearchEnable = true;
            this.scSKUCD.Size = new System.Drawing.Size(223, 27);
            this.scSKUCD.Stype = Search.CKM_SearchControl.SearchType.SKUCD;
            this.scSKUCD.TabIndex = 4;
            this.scSKUCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.scSKUCD.UseChangeDate = false;
            this.scSKUCD.Value1 = null;
            this.scSKUCD.Value2 = null;
            this.scSKUCD.Value3 = null;
            // 
            // scITEM
            // 
            this.scITEM.AutoSize = true;
            this.scITEM.ChangeDate = "";
            this.scITEM.ChangeDateWidth = 100;
            this.scITEM.Code = "";
            this.scITEM.CodeWidth = 190;
            this.scITEM.CodeWidth1 = 190;
            this.scITEM.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.scITEM.DataCheck = false;
            this.scITEM.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.scITEM.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.scITEM.IsCopy = false;
            this.scITEM.LabelText = "";
            this.scITEM.LabelVisible = false;
            this.scITEM.Location = new System.Drawing.Point(150, 119);
            this.scITEM.Margin = new System.Windows.Forms.Padding(0);
            this.scITEM.Name = "scITEM";
            this.scITEM.NameWidth = 350;
            this.scITEM.SearchEnable = true;
            this.scITEM.Size = new System.Drawing.Size(223, 27);
            this.scITEM.Stype = Search.CKM_SearchControl.SearchType.SKU_ITEM_CD;
            this.scITEM.TabIndex = 3;
            this.scITEM.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.scITEM.UseChangeDate = false;
            this.scITEM.Value1 = null;
            this.scITEM.Value2 = null;
            this.scITEM.Value3 = null;
            // 
            // scMakerShohinCD
            // 
            this.scMakerShohinCD.AutoSize = true;
            this.scMakerShohinCD.ChangeDate = "";
            this.scMakerShohinCD.ChangeDateWidth = 100;
            this.scMakerShohinCD.Code = "";
            this.scMakerShohinCD.CodeWidth = 190;
            this.scMakerShohinCD.CodeWidth1 = 190;
            this.scMakerShohinCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.scMakerShohinCD.DataCheck = false;
            this.scMakerShohinCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.scMakerShohinCD.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.scMakerShohinCD.IsCopy = false;
            this.scMakerShohinCD.LabelText = "";
            this.scMakerShohinCD.LabelVisible = false;
            this.scMakerShohinCD.Location = new System.Drawing.Point(150, 95);
            this.scMakerShohinCD.Margin = new System.Windows.Forms.Padding(0);
            this.scMakerShohinCD.Name = "scMakerShohinCD";
            this.scMakerShohinCD.NameWidth = 350;
            this.scMakerShohinCD.SearchEnable = true;
            this.scMakerShohinCD.Size = new System.Drawing.Size(223, 28);
            this.scMakerShohinCD.Stype = Search.CKM_SearchControl.SearchType.MakerItem;
            this.scMakerShohinCD.TabIndex = 2;
            this.scMakerShohinCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.scMakerShohinCD.UseChangeDate = false;
            this.scMakerShohinCD.Value1 = null;
            this.scMakerShohinCD.Value2 = null;
            this.scMakerShohinCD.Value3 = null;
            // 
            // rdoMakerShohinCD
            // 
            this.rdoMakerShohinCD.AutoSize = true;
            this.rdoMakerShohinCD.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.rdoMakerShohinCD.Location = new System.Drawing.Point(349, 238);
            this.rdoMakerShohinCD.Name = "rdoMakerShohinCD";
            this.rdoMakerShohinCD.Size = new System.Drawing.Size(115, 16);
            this.rdoMakerShohinCD.TabIndex = 9;
            this.rdoMakerShohinCD.TabStop = true;
            this.rdoMakerShohinCD.Text = "メーカー商品CD";
            this.rdoMakerShohinCD.UseVisualStyleBackColor = true;
            // 
            // rdoITEM
            // 
            this.rdoITEM.AutoSize = true;
            this.rdoITEM.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.rdoITEM.Location = new System.Drawing.Point(276, 237);
            this.rdoITEM.Name = "rdoITEM";
            this.rdoITEM.Size = new System.Drawing.Size(58, 16);
            this.rdoITEM.TabIndex = 8;
            this.rdoITEM.TabStop = true;
            this.rdoITEM.Text = " ITEM";
            this.rdoITEM.UseVisualStyleBackColor = true;
            // 
            // chkPrintRelated
            // 
            this.chkPrintRelated.AutoSize = true;
            this.chkPrintRelated.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkPrintRelated.Location = new System.Drawing.Point(153, 238);
            this.chkPrintRelated.Name = "chkPrintRelated";
            this.chkPrintRelated.Size = new System.Drawing.Size(109, 16);
            this.chkPrintRelated.TabIndex = 7;
            this.chkPrintRelated.Text = " 関連印字する";
            this.chkPrintRelated.UseVisualStyleBackColor = true;
            this.chkPrintRelated.CheckedChanged += new System.EventHandler(this.chkPrintRelated_CheckedChanged);
            // 
            // txtSKUName
            // 
            this.txtSKUName.AllowMinus = false;
            this.txtSKUName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtSKUName.BackColor = System.Drawing.Color.White;
            this.txtSKUName.BorderColor = false;
            this.txtSKUName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSKUName.ClientColor = System.Drawing.Color.White;
            this.txtSKUName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtSKUName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtSKUName.DecimalPlace = 0;
            this.txtSKUName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtSKUName.IntegerPart = 0;
            this.txtSKUName.IsCorrectDate = true;
            this.txtSKUName.isEnterKeyDown = false;
            this.txtSKUName.IsFirstTime = true;
            this.txtSKUName.isMaxLengthErr = false;
            this.txtSKUName.IsNumber = true;
            this.txtSKUName.IsShop = false;
            this.txtSKUName.Length = 40;
            this.txtSKUName.Location = new System.Drawing.Point(150, 210);
            this.txtSKUName.MaxLength = 40;
            this.txtSKUName.MoveNext = true;
            this.txtSKUName.Name = "txtSKUName";
            this.txtSKUName.Size = new System.Drawing.Size(500, 19);
            this.txtSKUName.TabIndex = 6;
            this.txtSKUName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtSKUName.UseColorSizMode = false;
            // 
            // cboSouko
            // 
            this.cboSouko.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboSouko.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSouko.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.SoukoAll;
            this.cboSouko.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboSouko.Flag = 0;
            this.cboSouko.FormattingEnabled = true;
            this.cboSouko.Length = 10;
            this.cboSouko.Location = new System.Drawing.Point(150, 65);
            this.cboSouko.MaxLength = 10;
            this.cboSouko.MoveNext = true;
            this.cboSouko.Name = "cboSouko";
            this.cboSouko.Size = new System.Drawing.Size(280, 20);
            this.cboSouko.TabIndex = 1;
            this.cboSouko.SelectedIndexChanged += new System.EventHandler(this.cboSouko_SelectedIndexChanged);
            // 
            // txtTargetPeriodT
            // 
            this.txtTargetPeriodT.AllowMinus = false;
            this.txtTargetPeriodT.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtTargetPeriodT.BackColor = System.Drawing.Color.White;
            this.txtTargetPeriodT.BorderColor = false;
            this.txtTargetPeriodT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTargetPeriodT.ClientColor = System.Drawing.Color.White;
            this.txtTargetPeriodT.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtTargetPeriodT.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.YearMonth;
            this.txtTargetPeriodT.DecimalPlace = 0;
            this.txtTargetPeriodT.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtTargetPeriodT.IntegerPart = 0;
            this.txtTargetPeriodT.IsCorrectDate = true;
            this.txtTargetPeriodT.isEnterKeyDown = false;
            this.txtTargetPeriodT.IsFirstTime = true;
            this.txtTargetPeriodT.isMaxLengthErr = false;
            this.txtTargetPeriodT.IsNumber = true;
            this.txtTargetPeriodT.IsShop = false;
            this.txtTargetPeriodT.Length = 7;
            this.txtTargetPeriodT.Location = new System.Drawing.Point(296, 40);
            this.txtTargetPeriodT.MaxLength = 7;
            this.txtTargetPeriodT.MoveNext = true;
            this.txtTargetPeriodT.Name = "txtTargetPeriodT";
            this.txtTargetPeriodT.Size = new System.Drawing.Size(100, 19);
            this.txtTargetPeriodT.TabIndex = 9;
            this.txtTargetPeriodT.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtTargetPeriodT.UseColorSizMode = false;
            this.txtTargetPeriodT.Visible = false;
            // 
            // txtTargetPeriodF
            // 
            this.txtTargetPeriodF.AllowMinus = false;
            this.txtTargetPeriodF.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtTargetPeriodF.BackColor = System.Drawing.Color.White;
            this.txtTargetPeriodF.BorderColor = false;
            this.txtTargetPeriodF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTargetPeriodF.ClientColor = System.Drawing.Color.White;
            this.txtTargetPeriodF.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtTargetPeriodF.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.YearMonth;
            this.txtTargetPeriodF.DecimalPlace = 0;
            this.txtTargetPeriodF.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtTargetPeriodF.IntegerPart = 0;
            this.txtTargetPeriodF.IsCorrectDate = true;
            this.txtTargetPeriodF.isEnterKeyDown = false;
            this.txtTargetPeriodF.IsFirstTime = true;
            this.txtTargetPeriodF.isMaxLengthErr = false;
            this.txtTargetPeriodF.IsNumber = true;
            this.txtTargetPeriodF.IsShop = false;
            this.txtTargetPeriodF.Length = 7;
            this.txtTargetPeriodF.Location = new System.Drawing.Point(150, 41);
            this.txtTargetPeriodF.MaxLength = 7;
            this.txtTargetPeriodF.MoveNext = true;
            this.txtTargetPeriodF.Name = "txtTargetPeriodF";
            this.txtTargetPeriodF.Size = new System.Drawing.Size(100, 19);
            this.txtTargetPeriodF.TabIndex = 0;
            this.txtTargetPeriodF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTargetPeriodF.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtTargetPeriodF.UseColorSizMode = false;
            this.txtTargetPeriodF.Leave += new System.EventHandler(this.txtTargetPeriodF_Leave);
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
            this.ckM_Label8.Location = new System.Drawing.Point(265, 44);
            this.ckM_Label8.Name = "ckM_Label8";
            this.ckM_Label8.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label8.TabIndex = 7;
            this.ckM_Label8.Text = "～";
            this.ckM_Label8.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckM_Label8.Visible = false;
            // 
            // ckM_Label7
            // 
            this.ckM_Label7.AutoSize = true;
            this.ckM_Label7.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label7.DefaultlabelSize = true;
            this.ckM_Label7.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label7.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label7.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label7.Location = new System.Drawing.Point(104, 214);
            this.ckM_Label7.Name = "ckM_Label7";
            this.ckM_Label7.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label7.TabIndex = 6;
            this.ckM_Label7.Text = "商品名";
            this.ckM_Label7.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label6.Location = new System.Drawing.Point(108, 176);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(40, 12);
            this.ckM_Label6.TabIndex = 5;
            this.ckM_Label6.Text = "JANCD";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label5.Location = new System.Drawing.Point(108, 152);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(40, 12);
            this.ckM_Label5.TabIndex = 4;
            this.ckM_Label5.Text = "SKUCD";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label4.Location = new System.Drawing.Point(115, 128);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(33, 12);
            this.ckM_Label4.TabIndex = 3;
            this.ckM_Label4.Text = "ITEM";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label3.Location = new System.Drawing.Point(51, 104);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(97, 12);
            this.ckM_Label3.TabIndex = 2;
            this.ckM_Label3.Text = "メーカー商品CD";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label2.Location = new System.Drawing.Point(117, 69);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label2.TabIndex = 1;
            this.ckM_Label2.Text = "倉庫";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label1.Location = new System.Drawing.Point(91, 45);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label1.TabIndex = 0;
            this.ckM_Label1.Text = "対象期間";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ZaikoMotochouInsatsu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.panelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "ZaikoMotochouInsatsu";
            this.PanelHeaderHeight = 40;
            this.Text = "ZaikoMotochouInsatsu";
            this.Load += new System.EventHandler(this.ZaikoMotochouInsatsu_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ZaikoMotochouInsatsu_KeyUp);
            this.Controls.SetChildIndex(this.panelDetail, 0);
            this.panelDetail.ResumeLayout(false);
            this.panelDetail.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelDetail;
        private CKM_Controls.CKM_Label ckM_Label8;
        private CKM_Controls.CKM_Label ckM_Label7;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_TextBox txtTargetPeriodT;
        private CKM_Controls.CKM_TextBox txtTargetPeriodF;
        private CKM_Controls.CKM_TextBox txtSKUName;
        private CKM_Controls.CKM_ComboBox cboSouko;
        private CKM_Controls.CKM_RadioButton rdoMakerShohinCD;
        private CKM_Controls.CKM_RadioButton rdoITEM;
        private CKM_Controls.CKM_CheckBox chkPrintRelated;
        private Search.CKM_SearchControl scJANCD;
        private Search.CKM_SearchControl scSKUCD;
        private Search.CKM_SearchControl scITEM;
        private Search.CKM_SearchControl scMakerShohinCD;
    }
}

