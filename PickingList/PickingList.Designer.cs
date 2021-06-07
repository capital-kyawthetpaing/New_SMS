namespace PickingList
{
    partial class FrmPickingList
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
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.chkUnissued1 = new CKM_Controls.CKM_CheckBox();
            this.chkReissued1 = new CKM_Controls.CKM_CheckBox();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.txtDateTo1 = new CKM_Controls.CKM_TextBox();
            this.txtDateFrom1 = new CKM_Controls.CKM_TextBox();
            this.txtShipmentDate = new CKM_Controls.CKM_TextBox();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.ScPickingNo1 = new Search.CKM_SearchControl();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.ScPickingNo2 = new Search.CKM_SearchControl();
            this.ckM_Label7 = new CKM_Controls.CKM_Label();
            this.txtDateTo2 = new CKM_Controls.CKM_TextBox();
            this.txtDateFrom2 = new CKM_Controls.CKM_TextBox();
            this.ckM_Label8 = new CKM_Controls.CKM_Label();
            this.chkReissued2 = new CKM_Controls.CKM_CheckBox();
            this.chkUnissued2 = new CKM_Controls.CKM_CheckBox();
            this.ckM_Label9 = new CKM_Controls.CKM_Label();
            this.cboSouko = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.panelDetail = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelDetail.SuspendLayout();
            this.panel2.SuspendLayout();
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
            // ckM_Label2
            // 
            this.ckM_Label2.AutoSize = true;
            this.ckM_Label2.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label2.DefaultlabelSize = true;
            this.ckM_Label2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label2.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label2.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label2.Location = new System.Drawing.Point(38, 53);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label2.TabIndex = 13;
            this.ckM_Label2.Text = "印刷対象";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkUnissued1
            // 
            this.chkUnissued1.AutoSize = true;
            this.chkUnissued1.Checked = true;
            this.chkUnissued1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUnissued1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkUnissued1.Location = new System.Drawing.Point(98, 52);
            this.chkUnissued1.Name = "chkUnissued1";
            this.chkUnissued1.Size = new System.Drawing.Size(76, 16);
            this.chkUnissued1.TabIndex = 0;
            this.chkUnissued1.Text = "未発行分";
            this.chkUnissued1.UseVisualStyleBackColor = true;
            this.chkUnissued1.CheckedChanged += new System.EventHandler(this.chkUnissued1_CheckedChanged);
            // 
            // chkReissued1
            // 
            this.chkReissued1.AutoSize = true;
            this.chkReissued1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkReissued1.Location = new System.Drawing.Point(201, 53);
            this.chkReissued1.Name = "chkReissued1";
            this.chkReissued1.Size = new System.Drawing.Size(76, 16);
            this.chkReissued1.TabIndex = 1;
            this.chkReissued1.Text = "再発行分";
            this.chkReissued1.UseVisualStyleBackColor = true;
            this.chkReissued1.CheckedChanged += new System.EventHandler(this.chkReissued1_CheckedChanged);
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
            this.ckM_Label1.Location = new System.Drawing.Point(94, 84);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label1.TabIndex = 16;
            this.ckM_Label1.Text = "出荷予定日";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label3.Location = new System.Drawing.Point(94, 115);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label3.TabIndex = 17;
            this.ckM_Label3.Text = "出荷予定日";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label4.Location = new System.Drawing.Point(286, 86);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label4.TabIndex = 20;
            this.ckM_Label4.Text = "～";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDateTo1
            // 
            this.txtDateTo1.AllowMinus = false;
            this.txtDateTo1.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtDateTo1.BackColor = System.Drawing.Color.White;
            this.txtDateTo1.BorderColor = false;
            this.txtDateTo1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDateTo1.ClientColor = System.Drawing.Color.White;
            this.txtDateTo1.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtDateTo1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtDateTo1.DecimalPlace = 0;
            this.txtDateTo1.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtDateTo1.IntegerPart = 0;
            this.txtDateTo1.IsCorrectDate = true;
            this.txtDateTo1.isEnterKeyDown = false;
            this.txtDateTo1.IsFirstTime = true;
            this.txtDateTo1.isMaxLengthErr = false;
            this.txtDateTo1.IsNumber = true;
            this.txtDateTo1.IsShop = false;
            this.txtDateTo1.IsTimemmss = false;
            this.txtDateTo1.Length = 10;
            this.txtDateTo1.Location = new System.Drawing.Point(320, 81);
            this.txtDateTo1.MaxLength = 10;
            this.txtDateTo1.MoveNext = true;
            this.txtDateTo1.Name = "txtDateTo1";
            this.txtDateTo1.Size = new System.Drawing.Size(100, 19);
            this.txtDateTo1.TabIndex = 5;
            this.txtDateTo1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDateTo1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtDateTo1.UseColorSizMode = false;
            this.txtDateTo1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDateTo1_KeyDown);
            // 
            // txtDateFrom1
            // 
            this.txtDateFrom1.AllowMinus = false;
            this.txtDateFrom1.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtDateFrom1.BackColor = System.Drawing.Color.White;
            this.txtDateFrom1.BorderColor = false;
            this.txtDateFrom1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDateFrom1.ClientColor = System.Drawing.Color.White;
            this.txtDateFrom1.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtDateFrom1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtDateFrom1.DecimalPlace = 0;
            this.txtDateFrom1.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtDateFrom1.IntegerPart = 0;
            this.txtDateFrom1.IsCorrectDate = true;
            this.txtDateFrom1.isEnterKeyDown = false;
            this.txtDateFrom1.IsFirstTime = true;
            this.txtDateFrom1.isMaxLengthErr = false;
            this.txtDateFrom1.IsNumber = true;
            this.txtDateFrom1.IsShop = false;
            this.txtDateFrom1.IsTimemmss = false;
            this.txtDateFrom1.Length = 10;
            this.txtDateFrom1.Location = new System.Drawing.Point(167, 81);
            this.txtDateFrom1.MaxLength = 10;
            this.txtDateFrom1.MoveNext = true;
            this.txtDateFrom1.Name = "txtDateFrom1";
            this.txtDateFrom1.Size = new System.Drawing.Size(100, 19);
            this.txtDateFrom1.TabIndex = 4;
            this.txtDateFrom1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDateFrom1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtDateFrom1.UseColorSizMode = false;
            // 
            // txtShipmentDate
            // 
            this.txtShipmentDate.AllowMinus = false;
            this.txtShipmentDate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtShipmentDate.BackColor = System.Drawing.Color.White;
            this.txtShipmentDate.BorderColor = false;
            this.txtShipmentDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShipmentDate.ClientColor = System.Drawing.Color.White;
            this.txtShipmentDate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtShipmentDate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtShipmentDate.DecimalPlace = 0;
            this.txtShipmentDate.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtShipmentDate.IntegerPart = 0;
            this.txtShipmentDate.IsCorrectDate = true;
            this.txtShipmentDate.isEnterKeyDown = false;
            this.txtShipmentDate.IsFirstTime = true;
            this.txtShipmentDate.isMaxLengthErr = false;
            this.txtShipmentDate.IsNumber = true;
            this.txtShipmentDate.IsShop = false;
            this.txtShipmentDate.IsTimemmss = false;
            this.txtShipmentDate.Length = 10;
            this.txtShipmentDate.Location = new System.Drawing.Point(167, 112);
            this.txtShipmentDate.MaxLength = 10;
            this.txtShipmentDate.MoveNext = true;
            this.txtShipmentDate.Name = "txtShipmentDate";
            this.txtShipmentDate.Size = new System.Drawing.Size(100, 19);
            this.txtShipmentDate.TabIndex = 6;
            this.txtShipmentDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtShipmentDate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtShipmentDate.UseColorSizMode = false;
            this.txtShipmentDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtShipmentDate_KeyDown);
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
            this.ckM_Label5.Location = new System.Drawing.Point(271, 114);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(291, 12);
            this.ckM_Label5.TabIndex = 6;
            this.ckM_Label5.Text = "に出荷できる商品だけ（受注単位で揃っている）";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScPickingNo1
            // 
            this.ScPickingNo1.AutoSize = true;
            this.ScPickingNo1.ChangeDate = "";
            this.ScPickingNo1.ChangeDateWidth = 100;
            this.ScPickingNo1.Code = "";
            this.ScPickingNo1.CodeWidth = 100;
            this.ScPickingNo1.CodeWidth1 = 100;
            this.ScPickingNo1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScPickingNo1.DataCheck = true;
            this.ScPickingNo1.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScPickingNo1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScPickingNo1.IsCopy = false;
            this.ScPickingNo1.LabelText = "";
            this.ScPickingNo1.LabelVisible = false;
            this.ScPickingNo1.Location = new System.Drawing.Point(7, 8);
            this.ScPickingNo1.Margin = new System.Windows.Forms.Padding(0);
            this.ScPickingNo1.Name = "ScPickingNo1";
            this.ScPickingNo1.NameWidth = 600;
            this.ScPickingNo1.SearchEnable = true;
            this.ScPickingNo1.Size = new System.Drawing.Size(133, 28);
            this.ScPickingNo1.Stype = Search.CKM_SearchControl.SearchType.ピッキング番号;
            this.ScPickingNo1.TabIndex = 0;
            this.ScPickingNo1.test = null;
            this.ScPickingNo1.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScPickingNo1.UseChangeDate = false;
            this.ScPickingNo1.Value1 = null;
            this.ScPickingNo1.Value2 = null;
            this.ScPickingNo1.Value3 = null;
            this.ScPickingNo1.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScPickingNo1_CodeKeyDownEvent);
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
            this.ckM_Label6.Location = new System.Drawing.Point(38, 192);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(545, 12);
            this.ckM_Label6.TabIndex = 24;
            this.ckM_Label6.Text = "未出荷分 （ピッキング完了の登録はあるが、キャンセル等で出荷が登録されていないもの）";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScPickingNo2
            // 
            this.ScPickingNo2.AutoSize = true;
            this.ScPickingNo2.ChangeDate = "";
            this.ScPickingNo2.ChangeDateWidth = 100;
            this.ScPickingNo2.Code = "";
            this.ScPickingNo2.CodeWidth = 100;
            this.ScPickingNo2.CodeWidth1 = 100;
            this.ScPickingNo2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScPickingNo2.DataCheck = true;
            this.ScPickingNo2.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScPickingNo2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScPickingNo2.IsCopy = false;
            this.ScPickingNo2.LabelText = "";
            this.ScPickingNo2.LabelVisible = false;
            this.ScPickingNo2.Location = new System.Drawing.Point(12, 6);
            this.ScPickingNo2.Margin = new System.Windows.Forms.Padding(0);
            this.ScPickingNo2.Name = "ScPickingNo2";
            this.ScPickingNo2.NameWidth = 600;
            this.ScPickingNo2.SearchEnable = true;
            this.ScPickingNo2.Size = new System.Drawing.Size(133, 27);
            this.ScPickingNo2.Stype = Search.CKM_SearchControl.SearchType.ピッキング番号;
            this.ScPickingNo2.TabIndex = 0;
            this.ScPickingNo2.test = null;
            this.ScPickingNo2.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScPickingNo2.UseChangeDate = false;
            this.ScPickingNo2.Value1 = null;
            this.ScPickingNo2.Value2 = null;
            this.ScPickingNo2.Value3 = null;
            this.ScPickingNo2.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScPickingNo2_CodeKeyDownEvent);
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
            this.ckM_Label7.Location = new System.Drawing.Point(286, 256);
            this.ckM_Label7.Name = "ckM_Label7";
            this.ckM_Label7.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label7.TabIndex = 31;
            this.ckM_Label7.Text = "～";
            this.ckM_Label7.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDateTo2
            // 
            this.txtDateTo2.AllowMinus = false;
            this.txtDateTo2.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtDateTo2.BackColor = System.Drawing.Color.White;
            this.txtDateTo2.BorderColor = false;
            this.txtDateTo2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDateTo2.ClientColor = System.Drawing.Color.White;
            this.txtDateTo2.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtDateTo2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtDateTo2.DecimalPlace = 0;
            this.txtDateTo2.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtDateTo2.IntegerPart = 0;
            this.txtDateTo2.IsCorrectDate = true;
            this.txtDateTo2.isEnterKeyDown = false;
            this.txtDateTo2.IsFirstTime = true;
            this.txtDateTo2.isMaxLengthErr = false;
            this.txtDateTo2.IsNumber = true;
            this.txtDateTo2.IsShop = false;
            this.txtDateTo2.IsTimemmss = false;
            this.txtDateTo2.Length = 10;
            this.txtDateTo2.Location = new System.Drawing.Point(319, 252);
            this.txtDateTo2.MaxLength = 10;
            this.txtDateTo2.MoveNext = true;
            this.txtDateTo2.Name = "txtDateTo2";
            this.txtDateTo2.Size = new System.Drawing.Size(100, 19);
            this.txtDateTo2.TabIndex = 11;
            this.txtDateTo2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDateTo2.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtDateTo2.UseColorSizMode = false;
            this.txtDateTo2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDateTo2_KeyDown);
            // 
            // txtDateFrom2
            // 
            this.txtDateFrom2.AllowMinus = false;
            this.txtDateFrom2.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtDateFrom2.BackColor = System.Drawing.Color.White;
            this.txtDateFrom2.BorderColor = false;
            this.txtDateFrom2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDateFrom2.ClientColor = System.Drawing.Color.White;
            this.txtDateFrom2.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtDateFrom2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtDateFrom2.DecimalPlace = 0;
            this.txtDateFrom2.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtDateFrom2.IntegerPart = 0;
            this.txtDateFrom2.IsCorrectDate = true;
            this.txtDateFrom2.isEnterKeyDown = false;
            this.txtDateFrom2.IsFirstTime = true;
            this.txtDateFrom2.isMaxLengthErr = false;
            this.txtDateFrom2.IsNumber = true;
            this.txtDateFrom2.IsShop = false;
            this.txtDateFrom2.IsTimemmss = false;
            this.txtDateFrom2.Length = 10;
            this.txtDateFrom2.Location = new System.Drawing.Point(167, 253);
            this.txtDateFrom2.MaxLength = 10;
            this.txtDateFrom2.MoveNext = true;
            this.txtDateFrom2.Name = "txtDateFrom2";
            this.txtDateFrom2.Size = new System.Drawing.Size(100, 19);
            this.txtDateFrom2.TabIndex = 10;
            this.txtDateFrom2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDateFrom2.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtDateFrom2.UseColorSizMode = false;
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
            this.ckM_Label8.Location = new System.Drawing.Point(55, 256);
            this.ckM_Label8.Name = "ckM_Label8";
            this.ckM_Label8.Size = new System.Drawing.Size(109, 12);
            this.ckM_Label8.TabIndex = 28;
            this.ckM_Label8.Text = "当初の出荷予定日";
            this.ckM_Label8.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkReissued2
            // 
            this.chkReissued2.AutoSize = true;
            this.chkReissued2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkReissued2.Location = new System.Drawing.Point(201, 217);
            this.chkReissued2.Name = "chkReissued2";
            this.chkReissued2.Size = new System.Drawing.Size(76, 16);
            this.chkReissued2.TabIndex = 8;
            this.chkReissued2.Text = "再発行分";
            this.chkReissued2.UseVisualStyleBackColor = true;
            this.chkReissued2.CheckedChanged += new System.EventHandler(this.chkReissued2_CheckedChanged);
            // 
            // chkUnissued2
            // 
            this.chkUnissued2.AutoSize = true;
            this.chkUnissued2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkUnissued2.Location = new System.Drawing.Point(98, 216);
            this.chkUnissued2.Name = "chkUnissued2";
            this.chkUnissued2.Size = new System.Drawing.Size(76, 16);
            this.chkUnissued2.TabIndex = 7;
            this.chkUnissued2.Text = "未発行分";
            this.chkUnissued2.UseVisualStyleBackColor = true;
            this.chkUnissued2.CheckedChanged += new System.EventHandler(this.chkUnissued2_CheckedChanged);
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
            this.ckM_Label9.Location = new System.Drawing.Point(38, 217);
            this.ckM_Label9.Name = "ckM_Label9";
            this.ckM_Label9.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label9.TabIndex = 25;
            this.ckM_Label9.Text = "印刷対象";
            this.ckM_Label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.cboSouko.Location = new System.Drawing.Point(1101, 52);
            this.cboSouko.MaxLength = 10;
            this.cboSouko.MoveNext = true;
            this.cboSouko.Name = "cboSouko";
            this.cboSouko.Size = new System.Drawing.Size(265, 20);
            this.cboSouko.TabIndex = 3;
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
            this.ckM_Label10.Location = new System.Drawing.Point(1067, 56);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label10.TabIndex = 34;
            this.ckM_Label10.Text = "倉庫";
            this.ckM_Label10.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.panel2);
            this.panelDetail.Controls.Add(this.panel1);
            this.panelDetail.Controls.Add(this.chkReissued1);
            this.panelDetail.Controls.Add(this.cboSouko);
            this.panelDetail.Controls.Add(this.ckM_Label10);
            this.panelDetail.Controls.Add(this.ckM_Label2);
            this.panelDetail.Controls.Add(this.chkUnissued1);
            this.panelDetail.Controls.Add(this.ckM_Label1);
            this.panelDetail.Controls.Add(this.ckM_Label7);
            this.panelDetail.Controls.Add(this.ckM_Label3);
            this.panelDetail.Controls.Add(this.txtDateTo2);
            this.panelDetail.Controls.Add(this.txtDateFrom1);
            this.panelDetail.Controls.Add(this.txtDateFrom2);
            this.panelDetail.Controls.Add(this.txtDateTo1);
            this.panelDetail.Controls.Add(this.ckM_Label8);
            this.panelDetail.Controls.Add(this.ckM_Label4);
            this.panelDetail.Controls.Add(this.chkReissued2);
            this.panelDetail.Controls.Add(this.txtShipmentDate);
            this.panelDetail.Controls.Add(this.chkUnissued2);
            this.panelDetail.Controls.Add(this.ckM_Label5);
            this.panelDetail.Controls.Add(this.ckM_Label9);
            this.panelDetail.Controls.Add(this.ckM_Label6);
            this.panelDetail.Location = new System.Drawing.Point(0, 55);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(1710, 875);
            this.panelDetail.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ScPickingNo2);
            this.panel2.Location = new System.Drawing.Point(308, 207);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(150, 40);
            this.panel2.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ScPickingNo1);
            this.panel1.Location = new System.Drawing.Point(313, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(150, 40);
            this.panel1.TabIndex = 2;
            this.panel1.TabStop = true;
            // 
            // FrmPickingList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.panelDetail);
            this.F10Visible = false;
            this.F11Visible = false;
            this.F2Visible = false;
            this.F3Visible = false;
            this.F4Visible = false;
            this.F5Visible = false;
            this.F7Visible = false;
            this.F8Visible = false;
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "FrmPickingList";
            this.PanelHeaderHeight = 50;
            this.Text = "PickingList";
            this.Load += new System.EventHandler(this.FrmPickingList_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmPickingList_KeyUp);
            this.Controls.SetChildIndex(this.panelDetail, 0);
            this.panelDetail.ResumeLayout(false);
            this.panelDetail.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_CheckBox chkUnissued1;
        private CKM_Controls.CKM_CheckBox chkReissued1;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_TextBox txtDateTo1;
        private CKM_Controls.CKM_TextBox txtDateFrom1;
        private CKM_Controls.CKM_TextBox txtShipmentDate;
        private CKM_Controls.CKM_Label ckM_Label5;
        private Search.CKM_SearchControl ScPickingNo1;
        private CKM_Controls.CKM_Label ckM_Label6;
        private Search.CKM_SearchControl ScPickingNo2;
        private CKM_Controls.CKM_Label ckM_Label7;
        private CKM_Controls.CKM_TextBox txtDateTo2;
        private CKM_Controls.CKM_TextBox txtDateFrom2;
        private CKM_Controls.CKM_Label ckM_Label8;
        private CKM_Controls.CKM_CheckBox chkReissued2;
        private CKM_Controls.CKM_CheckBox chkUnissued2;
        private CKM_Controls.CKM_Label ckM_Label9;
        private CKM_Controls.CKM_ComboBox cboSouko;
        private CKM_Controls.CKM_Label ckM_Label10;
        private System.Windows.Forms.Panel panelDetail;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}

