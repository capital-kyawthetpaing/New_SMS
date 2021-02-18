namespace KeihiltiranHyou
{
    partial class FrmKeihiltiranHyou
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
            this.expense_timeto = new CKM_Controls.CKM_TextBox();
            this.expense_timefrom = new CKM_Controls.CKM_TextBox();
            this.cboStoreName = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.rdb_all = new CKM_Controls.CKM_RadioButton();
            this.txtExpenseTo = new CKM_Controls.CKM_TextBox();
            this.txtPaymentTo = new CKM_Controls.CKM_TextBox();
            this.txtRecordTo = new CKM_Controls.CKM_TextBox();
            this.ckM_Label9 = new CKM_Controls.CKM_Label();
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.ckM_Label11 = new CKM_Controls.CKM_Label();
            this.txtRecordFrom = new CKM_Controls.CKM_TextBox();
            this.txtPaymentFrom = new CKM_Controls.CKM_TextBox();
            this.txtExpenseFrom = new CKM_Controls.CKM_TextBox();
            this.rdb_paid = new CKM_Controls.CKM_RadioButton();
            this.rdb_unpaid = new CKM_Controls.CKM_RadioButton();
            this.ckM_Label12 = new CKM_Controls.CKM_Label();
            this.ckM_Label13 = new CKM_Controls.CKM_Label();
            this.ckM_Label14 = new CKM_Controls.CKM_Label();
            this.ckM_Label15 = new CKM_Controls.CKM_Label();
            this.panelDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1538, 24);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(1004, 0);
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            // 
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.expense_timeto);
            this.panelDetail.Controls.Add(this.expense_timefrom);
            this.panelDetail.Controls.Add(this.cboStoreName);
            this.panelDetail.Controls.Add(this.ckM_Label1);
            this.panelDetail.Controls.Add(this.rdb_all);
            this.panelDetail.Controls.Add(this.txtExpenseTo);
            this.panelDetail.Controls.Add(this.txtPaymentTo);
            this.panelDetail.Controls.Add(this.txtRecordTo);
            this.panelDetail.Controls.Add(this.ckM_Label9);
            this.panelDetail.Controls.Add(this.ckM_Label10);
            this.panelDetail.Controls.Add(this.ckM_Label11);
            this.panelDetail.Controls.Add(this.txtRecordFrom);
            this.panelDetail.Controls.Add(this.txtPaymentFrom);
            this.panelDetail.Controls.Add(this.txtExpenseFrom);
            this.panelDetail.Controls.Add(this.rdb_paid);
            this.panelDetail.Controls.Add(this.rdb_unpaid);
            this.panelDetail.Controls.Add(this.ckM_Label12);
            this.panelDetail.Controls.Add(this.ckM_Label13);
            this.panelDetail.Controls.Add(this.ckM_Label14);
            this.panelDetail.Controls.Add(this.ckM_Label15);
            this.panelDetail.Location = new System.Drawing.Point(9, 86);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(1700, 650);
            this.panelDetail.TabIndex = 30;
            // 
            // expense_timeto
            // 
            this.expense_timeto.AllowMinus = false;
            this.expense_timeto.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.expense_timeto.BackColor = System.Drawing.Color.White;
            this.expense_timeto.BorderColor = false;
            this.expense_timeto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.expense_timeto.ClientColor = System.Drawing.SystemColors.Window;
            this.expense_timeto.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.expense_timeto.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Time;
            this.expense_timeto.DecimalPlace = 0;
            this.expense_timeto.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.expense_timeto.IntegerPart = 0;
            this.expense_timeto.IsCorrectDate = true;
            this.expense_timeto.isEnterKeyDown = false;
            this.expense_timeto.IsFirstTime = true;
            this.expense_timeto.isMaxLengthErr = false;
            this.expense_timeto.IsNumber = true;
            this.expense_timeto.IsShop = false;
            this.expense_timeto.IsTimemmss = false;
            this.expense_timeto.Length = 8;
            this.expense_timeto.Location = new System.Drawing.Point(421, 73);
            this.expense_timeto.MaxLength = 8;
            this.expense_timeto.MoveNext = true;
            this.expense_timeto.Name = "expense_timeto";
            this.expense_timeto.Size = new System.Drawing.Size(50, 19);
            this.expense_timeto.TabIndex = 7;
            this.expense_timeto.Text = "00:00";
            this.expense_timeto.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.expense_timeto.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.expense_timeto.UseColorSizMode = false;
            this.expense_timeto.KeyDown += new System.Windows.Forms.KeyEventHandler(this.expense_timeto_KeyDown);
            // 
            // expense_timefrom
            // 
            this.expense_timefrom.AllowMinus = false;
            this.expense_timefrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.expense_timefrom.BackColor = System.Drawing.Color.White;
            this.expense_timefrom.BorderColor = false;
            this.expense_timefrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.expense_timefrom.ClientColor = System.Drawing.SystemColors.Window;
            this.expense_timefrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.expense_timefrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Time;
            this.expense_timefrom.DecimalPlace = 0;
            this.expense_timefrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.expense_timefrom.IntegerPart = 0;
            this.expense_timefrom.IsCorrectDate = true;
            this.expense_timefrom.isEnterKeyDown = false;
            this.expense_timefrom.IsFirstTime = true;
            this.expense_timefrom.isMaxLengthErr = false;
            this.expense_timefrom.IsNumber = true;
            this.expense_timefrom.IsShop = false;
            this.expense_timefrom.IsTimemmss = false;
            this.expense_timefrom.Length = 8;
            this.expense_timefrom.Location = new System.Drawing.Point(203, 73);
            this.expense_timefrom.MaxLength = 8;
            this.expense_timefrom.MoveNext = true;
            this.expense_timefrom.Name = "expense_timefrom";
            this.expense_timefrom.Size = new System.Drawing.Size(50, 19);
            this.expense_timefrom.TabIndex = 5;
            this.expense_timefrom.Text = "00:00";
            this.expense_timefrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.expense_timefrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.expense_timefrom.UseColorSizMode = false;
            // 
            // cboStoreName
            // 
            this.cboStoreName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboStoreName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStoreName.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.店舗ストア_受注;
            this.cboStoreName.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboStoreName.Flag = 0;
            this.cboStoreName.FormattingEnabled = true;
            this.cboStoreName.Length = 6;
            this.cboStoreName.Location = new System.Drawing.Point(1542, 11);
            this.cboStoreName.MaxLength = 6;
            this.cboStoreName.MoveNext = true;
            this.cboStoreName.Name = "cboStoreName";
            this.cboStoreName.Size = new System.Drawing.Size(150, 20);
            this.cboStoreName.TabIndex = 9;
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
            this.ckM_Label1.Location = new System.Drawing.Point(1508, 15);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label1.TabIndex = 45;
            this.ckM_Label1.Text = "店舗";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rdb_all
            // 
            this.rdb_all.AutoSize = true;
            this.rdb_all.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.rdb_all.Location = new System.Drawing.Point(257, 99);
            this.rdb_all.Name = "rdb_all";
            this.rdb_all.Size = new System.Drawing.Size(49, 16);
            this.rdb_all.TabIndex = 44;
            this.rdb_all.TabStop = true;
            this.rdb_all.Text = "全て";
            this.rdb_all.UseVisualStyleBackColor = true;
            // 
            // txtExpenseTo
            // 
            this.txtExpenseTo.AllowMinus = false;
            this.txtExpenseTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtExpenseTo.BackColor = System.Drawing.Color.White;
            this.txtExpenseTo.BorderColor = false;
            this.txtExpenseTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExpenseTo.ClientColor = System.Drawing.Color.White;
            this.txtExpenseTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtExpenseTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtExpenseTo.DecimalPlace = 0;
            this.txtExpenseTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtExpenseTo.IntegerPart = 0;
            this.txtExpenseTo.IsCorrectDate = true;
            this.txtExpenseTo.isEnterKeyDown = false;
            this.txtExpenseTo.IsFirstTime = true;
            this.txtExpenseTo.isMaxLengthErr = false;
            this.txtExpenseTo.IsNumber = true;
            this.txtExpenseTo.IsShop = false;
            this.txtExpenseTo.IsTimemmss = false;
            this.txtExpenseTo.Length = 10;
            this.txtExpenseTo.Location = new System.Drawing.Point(321, 73);
            this.txtExpenseTo.MaxLength = 10;
            this.txtExpenseTo.MoveNext = true;
            this.txtExpenseTo.Name = "txtExpenseTo";
            this.txtExpenseTo.Size = new System.Drawing.Size(100, 19);
            this.txtExpenseTo.TabIndex = 6;
            this.txtExpenseTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtExpenseTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtExpenseTo.UseColorSizMode = false;
            this.txtExpenseTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtExpenseTo_KeyDown);
            // 
            // txtPaymentTo
            // 
            this.txtPaymentTo.AllowMinus = false;
            this.txtPaymentTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPaymentTo.BackColor = System.Drawing.Color.White;
            this.txtPaymentTo.BorderColor = false;
            this.txtPaymentTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPaymentTo.ClientColor = System.Drawing.Color.White;
            this.txtPaymentTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtPaymentTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtPaymentTo.DecimalPlace = 0;
            this.txtPaymentTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtPaymentTo.IntegerPart = 0;
            this.txtPaymentTo.IsCorrectDate = true;
            this.txtPaymentTo.isEnterKeyDown = false;
            this.txtPaymentTo.IsFirstTime = true;
            this.txtPaymentTo.isMaxLengthErr = false;
            this.txtPaymentTo.IsNumber = true;
            this.txtPaymentTo.IsShop = false;
            this.txtPaymentTo.IsTimemmss = false;
            this.txtPaymentTo.Length = 10;
            this.txtPaymentTo.Location = new System.Drawing.Point(260, 46);
            this.txtPaymentTo.MaxLength = 10;
            this.txtPaymentTo.MoveNext = true;
            this.txtPaymentTo.Name = "txtPaymentTo";
            this.txtPaymentTo.Size = new System.Drawing.Size(100, 19);
            this.txtPaymentTo.TabIndex = 3;
            this.txtPaymentTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPaymentTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtPaymentTo.UseColorSizMode = false;
            this.txtPaymentTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPaymentTo_KeyDown);
            // 
            // txtRecordTo
            // 
            this.txtRecordTo.AllowMinus = false;
            this.txtRecordTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtRecordTo.BackColor = System.Drawing.Color.White;
            this.txtRecordTo.BorderColor = false;
            this.txtRecordTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRecordTo.ClientColor = System.Drawing.Color.White;
            this.txtRecordTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtRecordTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtRecordTo.DecimalPlace = 0;
            this.txtRecordTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtRecordTo.IntegerPart = 0;
            this.txtRecordTo.IsCorrectDate = true;
            this.txtRecordTo.isEnterKeyDown = false;
            this.txtRecordTo.IsFirstTime = true;
            this.txtRecordTo.isMaxLengthErr = false;
            this.txtRecordTo.IsNumber = true;
            this.txtRecordTo.IsShop = false;
            this.txtRecordTo.IsTimemmss = false;
            this.txtRecordTo.Length = 10;
            this.txtRecordTo.Location = new System.Drawing.Point(261, 16);
            this.txtRecordTo.MaxLength = 10;
            this.txtRecordTo.MoveNext = true;
            this.txtRecordTo.Name = "txtRecordTo";
            this.txtRecordTo.Size = new System.Drawing.Size(100, 19);
            this.txtRecordTo.TabIndex = 1;
            this.txtRecordTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRecordTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtRecordTo.UseColorSizMode = false;
            this.txtRecordTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRecordTo_KeyDown);
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
            this.ckM_Label9.Location = new System.Drawing.Point(293, 77);
            this.ckM_Label9.Name = "ckM_Label9";
            this.ckM_Label9.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label9.TabIndex = 38;
            this.ckM_Label9.Text = "～";
            this.ckM_Label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label10.Location = new System.Drawing.Point(221, 48);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label10.TabIndex = 37;
            this.ckM_Label10.Text = "～";
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
            this.ckM_Label11.Location = new System.Drawing.Point(222, 20);
            this.ckM_Label11.Name = "ckM_Label11";
            this.ckM_Label11.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label11.TabIndex = 36;
            this.ckM_Label11.Text = "～";
            this.ckM_Label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRecordFrom
            // 
            this.txtRecordFrom.AllowMinus = false;
            this.txtRecordFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtRecordFrom.BackColor = System.Drawing.Color.White;
            this.txtRecordFrom.BorderColor = false;
            this.txtRecordFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRecordFrom.ClientColor = System.Drawing.Color.White;
            this.txtRecordFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtRecordFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtRecordFrom.DecimalPlace = 0;
            this.txtRecordFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtRecordFrom.IntegerPart = 0;
            this.txtRecordFrom.IsCorrectDate = true;
            this.txtRecordFrom.isEnterKeyDown = false;
            this.txtRecordFrom.IsFirstTime = true;
            this.txtRecordFrom.isMaxLengthErr = false;
            this.txtRecordFrom.IsNumber = true;
            this.txtRecordFrom.IsShop = false;
            this.txtRecordFrom.IsTimemmss = false;
            this.txtRecordFrom.Length = 10;
            this.txtRecordFrom.Location = new System.Drawing.Point(103, 16);
            this.txtRecordFrom.MaxLength = 10;
            this.txtRecordFrom.MoveNext = true;
            this.txtRecordFrom.Name = "txtRecordFrom";
            this.txtRecordFrom.Size = new System.Drawing.Size(100, 19);
            this.txtRecordFrom.TabIndex = 0;
            this.txtRecordFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRecordFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtRecordFrom.UseColorSizMode = false;
            // 
            // txtPaymentFrom
            // 
            this.txtPaymentFrom.AllowMinus = false;
            this.txtPaymentFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPaymentFrom.BackColor = System.Drawing.Color.White;
            this.txtPaymentFrom.BorderColor = false;
            this.txtPaymentFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPaymentFrom.ClientColor = System.Drawing.Color.White;
            this.txtPaymentFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtPaymentFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtPaymentFrom.DecimalPlace = 0;
            this.txtPaymentFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtPaymentFrom.IntegerPart = 0;
            this.txtPaymentFrom.IsCorrectDate = true;
            this.txtPaymentFrom.isEnterKeyDown = false;
            this.txtPaymentFrom.IsFirstTime = true;
            this.txtPaymentFrom.isMaxLengthErr = false;
            this.txtPaymentFrom.IsNumber = true;
            this.txtPaymentFrom.IsShop = false;
            this.txtPaymentFrom.IsTimemmss = false;
            this.txtPaymentFrom.Length = 10;
            this.txtPaymentFrom.Location = new System.Drawing.Point(103, 45);
            this.txtPaymentFrom.MaxLength = 10;
            this.txtPaymentFrom.MoveNext = true;
            this.txtPaymentFrom.Name = "txtPaymentFrom";
            this.txtPaymentFrom.Size = new System.Drawing.Size(100, 19);
            this.txtPaymentFrom.TabIndex = 2;
            this.txtPaymentFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPaymentFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtPaymentFrom.UseColorSizMode = false;
            // 
            // txtExpenseFrom
            // 
            this.txtExpenseFrom.AllowMinus = false;
            this.txtExpenseFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtExpenseFrom.BackColor = System.Drawing.Color.White;
            this.txtExpenseFrom.BorderColor = false;
            this.txtExpenseFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExpenseFrom.ClientColor = System.Drawing.Color.White;
            this.txtExpenseFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtExpenseFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtExpenseFrom.DecimalPlace = 0;
            this.txtExpenseFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtExpenseFrom.IntegerPart = 0;
            this.txtExpenseFrom.IsCorrectDate = true;
            this.txtExpenseFrom.isEnterKeyDown = false;
            this.txtExpenseFrom.IsFirstTime = true;
            this.txtExpenseFrom.isMaxLengthErr = false;
            this.txtExpenseFrom.IsNumber = true;
            this.txtExpenseFrom.IsShop = false;
            this.txtExpenseFrom.IsTimemmss = false;
            this.txtExpenseFrom.Length = 10;
            this.txtExpenseFrom.Location = new System.Drawing.Point(103, 73);
            this.txtExpenseFrom.MaxLength = 10;
            this.txtExpenseFrom.MoveNext = true;
            this.txtExpenseFrom.Name = "txtExpenseFrom";
            this.txtExpenseFrom.Size = new System.Drawing.Size(100, 19);
            this.txtExpenseFrom.TabIndex = 4;
            this.txtExpenseFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtExpenseFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtExpenseFrom.UseColorSizMode = false;
            // 
            // rdb_paid
            // 
            this.rdb_paid.AutoSize = true;
            this.rdb_paid.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.rdb_paid.Location = new System.Drawing.Point(181, 99);
            this.rdb_paid.Name = "rdb_paid";
            this.rdb_paid.Size = new System.Drawing.Size(62, 16);
            this.rdb_paid.TabIndex = 32;
            this.rdb_paid.TabStop = true;
            this.rdb_paid.Text = "支払済";
            this.rdb_paid.UseVisualStyleBackColor = true;
            // 
            // rdb_unpaid
            // 
            this.rdb_unpaid.AutoSize = true;
            this.rdb_unpaid.Checked = true;
            this.rdb_unpaid.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.rdb_unpaid.Location = new System.Drawing.Point(102, 99);
            this.rdb_unpaid.Name = "rdb_unpaid";
            this.rdb_unpaid.Size = new System.Drawing.Size(62, 16);
            this.rdb_unpaid.TabIndex = 31;
            this.rdb_unpaid.TabStop = true;
            this.rdb_unpaid.Text = "未支払";
            this.rdb_unpaid.UseVisualStyleBackColor = true;
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
            this.ckM_Label12.Location = new System.Drawing.Point(41, 101);
            this.ckM_Label12.Name = "ckM_Label12";
            this.ckM_Label12.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label12.TabIndex = 8;
            this.ckM_Label12.Text = "印刷対象";
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
            this.ckM_Label13.Location = new System.Drawing.Point(17, 76);
            this.ckM_Label13.Name = "ckM_Label13";
            this.ckM_Label13.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label13.TabIndex = 29;
            this.ckM_Label13.Text = "経費入力日時";
            this.ckM_Label13.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label14
            // 
            this.ckM_Label14.AutoSize = true;
            this.ckM_Label14.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label14.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label14.DefaultlabelSize = true;
            this.ckM_Label14.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label14.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label14.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label14.Location = new System.Drawing.Point(30, 48);
            this.ckM_Label14.Name = "ckM_Label14";
            this.ckM_Label14.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label14.TabIndex = 28;
            this.ckM_Label14.Text = "支払予定日";
            this.ckM_Label14.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label15
            // 
            this.ckM_Label15.AutoSize = true;
            this.ckM_Label15.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label15.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label15.DefaultlabelSize = true;
            this.ckM_Label15.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label15.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label15.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label15.Location = new System.Drawing.Point(56, 19);
            this.ckM_Label15.Name = "ckM_Label15";
            this.ckM_Label15.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label15.TabIndex = 27;
            this.ckM_Label15.Text = "計上日";
            this.ckM_Label15.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmKeihiltiranHyou
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1540, 774);
            this.Controls.Add(this.panelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "FrmKeihiltiranHyou";
            this.PanelHeaderHeight = 80;
            this.Text = "KeihiltiranHyou";
            this.Load += new System.EventHandler(this.FrmKeihiltiranHyou_Load);
            this.Controls.SetChildIndex(this.panelDetail, 0);
            this.panelDetail.ResumeLayout(false);
            this.panelDetail.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panelDetail;
        private CKM_Controls.CKM_ComboBox cboStoreName;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_RadioButton rdb_all;
        private CKM_Controls.CKM_TextBox txtExpenseTo;
        private CKM_Controls.CKM_TextBox txtPaymentTo;
        private CKM_Controls.CKM_TextBox txtRecordTo;
        private CKM_Controls.CKM_Label ckM_Label9;
        private CKM_Controls.CKM_Label ckM_Label10;
        private CKM_Controls.CKM_Label ckM_Label11;
        private CKM_Controls.CKM_TextBox txtRecordFrom;
        private CKM_Controls.CKM_TextBox txtPaymentFrom;
        private CKM_Controls.CKM_TextBox txtExpenseFrom;
        private CKM_Controls.CKM_RadioButton rdb_paid;
        private CKM_Controls.CKM_RadioButton rdb_unpaid;
        private CKM_Controls.CKM_Label ckM_Label12;
        private CKM_Controls.CKM_Label ckM_Label13;
        private CKM_Controls.CKM_Label ckM_Label14;
        private CKM_Controls.CKM_Label ckM_Label15;
        private CKM_Controls.CKM_TextBox expense_timefrom;
        private CKM_Controls.CKM_TextBox expense_timeto;
    }
}