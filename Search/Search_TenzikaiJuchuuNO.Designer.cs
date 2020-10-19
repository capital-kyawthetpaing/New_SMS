namespace Search
{
    partial class Search_TenzikaiJuchuuNO
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Search_TenzikaiJuchuuNO));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtCustomerName = new CKM_Controls.CKM_TextBox();
            this.dgvTenzikai = new CKM_Controls.CKM_GridView();
            this.colOrderNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSeason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDisplay = new CKM_Controls.CKM_Button();
            this.ckM_Label11 = new CKM_Controls.CKM_Label();
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.ckM_Label9 = new CKM_Controls.CKM_Label();
            this.ckM_Label7 = new CKM_Controls.CKM_Label();
            this.txtKanaName = new CKM_Controls.CKM_TextBox();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.cboSeason = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.cboYear = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label8 = new CKM_Controls.CKM_Label();
            this.txtOrderDateTo = new CKM_Controls.CKM_TextBox();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.txtOrderDateFrom = new CKM_Controls.CKM_TextBox();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.ScSKUCD = new Search.CKM_SearchControl();
            this.ScItem = new Search.CKM_SearchControl();
            this.ScCustomer = new Search.CKM_SearchControl();
            this.scStaff = new Search.CKM_SearchControl();
            this.ScSupplier = new Search.CKM_SearchControl();
            this.ScJanCD = new Search.CKM_SearchControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTenzikai)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(974, 6);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ScJanCD);
            this.panel1.Controls.Add(this.txtCustomerName);
            this.panel1.Controls.Add(this.dgvTenzikai);
            this.panel1.Controls.Add(this.btnDisplay);
            this.panel1.Controls.Add(this.ckM_Label11);
            this.panel1.Controls.Add(this.ScSKUCD);
            this.panel1.Controls.Add(this.ckM_Label10);
            this.panel1.Controls.Add(this.ScItem);
            this.panel1.Controls.Add(this.ckM_Label9);
            this.panel1.Controls.Add(this.ckM_Label7);
            this.panel1.Controls.Add(this.txtKanaName);
            this.panel1.Controls.Add(this.ScCustomer);
            this.panel1.Controls.Add(this.ckM_Label6);
            this.panel1.Controls.Add(this.scStaff);
            this.panel1.Controls.Add(this.ckM_Label5);
            this.panel1.Controls.Add(this.ckM_Label4);
            this.panel1.Controls.Add(this.cboSeason);
            this.panel1.Controls.Add(this.ckM_Label3);
            this.panel1.Controls.Add(this.cboYear);
            this.panel1.Controls.Add(this.ScSupplier);
            this.panel1.Controls.Add(this.ckM_Label8);
            this.panel1.Controls.Add(this.txtOrderDateTo);
            this.panel1.Controls.Add(this.ckM_Label2);
            this.panel1.Controls.Add(this.txtOrderDateFrom);
            this.panel1.Controls.Add(this.ckM_Label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(974, 741);
            this.panel1.TabIndex = 97;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.AllowMinus = false;
            this.txtCustomerName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtCustomerName.BackColor = System.Drawing.Color.White;
            this.txtCustomerName.BorderColor = false;
            this.txtCustomerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCustomerName.ClientColor = System.Drawing.Color.White;
            this.txtCustomerName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtCustomerName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtCustomerName.DecimalPlace = 0;
            this.txtCustomerName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtCustomerName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtCustomerName.IntegerPart = 0;
            this.txtCustomerName.IsCorrectDate = true;
            this.txtCustomerName.isEnterKeyDown = false;
            this.txtCustomerName.IsFirstTime = true;
            this.txtCustomerName.isMaxLengthErr = false;
            this.txtCustomerName.IsNumber = true;
            this.txtCustomerName.IsShop = false;
            this.txtCustomerName.Length = 80;
            this.txtCustomerName.Location = new System.Drawing.Point(249, 122);
            this.txtCustomerName.MaxLength = 80;
            this.txtCustomerName.MoveNext = true;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new System.Drawing.Size(520, 19);
            this.txtCustomerName.TabIndex = 121;
            this.txtCustomerName.Text = "ＸＸＸＸＸＸＸＸＸ10ＸＸＸＸＸＸＸＸＸ20ＸＸＸＸＸＸＸＸＸ30ＸＸＸＸＸＸＸＸＸ40";
            this.txtCustomerName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtCustomerName.UseColorSizMode = false;
            // 
            // dgvTenzikai
            // 
            this.dgvTenzikai.AllowUserToAddRows = false;
            this.dgvTenzikai.AllowUserToDeleteRows = false;
            this.dgvTenzikai.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.dgvTenzikai.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvTenzikai.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvTenzikai.CheckCol = ((System.Collections.ArrayList)(resources.GetObject("dgvTenzikai.CheckCol")));
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTenzikai.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvTenzikai.ColumnHeadersHeight = 25;
            this.dgvTenzikai.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOrderNum,
            this.colOrderDate,
            this.colSupplier,
            this.colYear,
            this.colSeason,
            this.colClient});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("MS Gothic", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTenzikai.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvTenzikai.EnableHeadersVisualStyles = false;
            this.dgvTenzikai.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvTenzikai.Location = new System.Drawing.Point(18, 279);
            this.dgvTenzikai.Name = "dgvTenzikai";
            this.dgvTenzikai.RowHeight_ = 20;
            this.dgvTenzikai.RowTemplate.Height = 20;
            this.dgvTenzikai.Size = new System.Drawing.Size(890, 430);
            this.dgvTenzikai.TabIndex = 120;
            this.dgvTenzikai.UseRowNo = true;
            this.dgvTenzikai.UseSetting = true;
            // 
            // colOrderNum
            // 
            this.colOrderNum.DataPropertyName = "TenzikaiJuchuuNO";
            this.colOrderNum.HeaderText = "展示会受注番号";
            this.colOrderNum.Name = "colOrderNum";
            this.colOrderNum.Width = 120;
            // 
            // colOrderDate
            // 
            this.colOrderDate.DataPropertyName = "JuchuuDate";
            this.colOrderDate.HeaderText = "受注日";
            this.colOrderDate.Name = "colOrderDate";
            // 
            // colSupplier
            // 
            this.colSupplier.DataPropertyName = "VendorName";
            this.colSupplier.HeaderText = "仕入先";
            this.colSupplier.Name = "colSupplier";
            this.colSupplier.Width = 250;
            // 
            // colYear
            // 
            this.colYear.DataPropertyName = "LastYearTerm";
            this.colYear.HeaderText = "年度";
            this.colYear.Name = "colYear";
            this.colYear.Width = 60;
            // 
            // colSeason
            // 
            this.colSeason.DataPropertyName = "LastSeason";
            this.colSeason.HeaderText = "シーズン";
            this.colSeason.Name = "colSeason";
            this.colSeason.Width = 60;
            // 
            // colClient
            // 
            this.colClient.DataPropertyName = "CustomerName";
            this.colClient.HeaderText = "顧客";
            this.colClient.Name = "colClient";
            this.colClient.Width = 240;
            // 
            // btnDisplay
            // 
            this.btnDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnDisplay.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnDisplay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDisplay.DefaultBtnSize = false;
            this.btnDisplay.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnDisplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisplay.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnDisplay.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnDisplay.Location = new System.Drawing.Point(792, 230);
            this.btnDisplay.Margin = new System.Windows.Forms.Padding(1);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(115, 28);
            this.btnDisplay.TabIndex = 12;
            this.btnDisplay.Text = "表示(F11)";
            this.btnDisplay.UseVisualStyleBackColor = false;
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
            this.ckM_Label11.Location = new System.Drawing.Point(75, 241);
            this.ckM_Label11.Name = "ckM_Label11";
            this.ckM_Label11.Size = new System.Drawing.Size(40, 12);
            this.ckM_Label11.TabIndex = 119;
            this.ckM_Label11.Text = "JANCD";
            this.ckM_Label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label10.Location = new System.Drawing.Point(75, 215);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(40, 12);
            this.ckM_Label10.TabIndex = 118;
            this.ckM_Label10.Text = "SKUCD";
            this.ckM_Label10.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label9.Location = new System.Drawing.Point(82, 189);
            this.ckM_Label9.Name = "ckM_Label9";
            this.ckM_Label9.Size = new System.Drawing.Size(33, 12);
            this.ckM_Label9.TabIndex = 117;
            this.ckM_Label9.Text = "ITEM";
            this.ckM_Label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label7.Location = new System.Drawing.Point(71, 164);
            this.ckM_Label7.Name = "ckM_Label7";
            this.ckM_Label7.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label7.TabIndex = 116;
            this.ckM_Label7.Text = "商品名";
            this.ckM_Label7.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtKanaName
            // 
            this.txtKanaName.AllowMinus = false;
            this.txtKanaName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtKanaName.BackColor = System.Drawing.Color.White;
            this.txtKanaName.BorderColor = false;
            this.txtKanaName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKanaName.ClientColor = System.Drawing.Color.White;
            this.txtKanaName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtKanaName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtKanaName.DecimalPlace = 0;
            this.txtKanaName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtKanaName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtKanaName.IntegerPart = 0;
            this.txtKanaName.IsCorrectDate = true;
            this.txtKanaName.isEnterKeyDown = false;
            this.txtKanaName.IsFirstTime = true;
            this.txtKanaName.isMaxLengthErr = false;
            this.txtKanaName.IsNumber = true;
            this.txtKanaName.IsShop = false;
            this.txtKanaName.Length = 50;
            this.txtKanaName.Location = new System.Drawing.Point(118, 160);
            this.txtKanaName.MaxLength = 50;
            this.txtKanaName.MoveNext = true;
            this.txtKanaName.Name = "txtKanaName";
            this.txtKanaName.Size = new System.Drawing.Size(450, 19);
            this.txtKanaName.TabIndex = 8;
            this.txtKanaName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtKanaName.UseColorSizMode = false;
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
            this.ckM_Label6.Location = new System.Drawing.Point(84, 125);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label6.TabIndex = 115;
            this.ckM_Label6.Text = "顧客";
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
            this.ckM_Label5.Location = new System.Drawing.Point(33, 99);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label5.TabIndex = 112;
            this.ckM_Label5.Text = "担当スタッフ";
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
            this.ckM_Label4.Location = new System.Drawing.Point(287, 73);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label4.TabIndex = 114;
            this.ckM_Label4.Text = "シーズン";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.cboSeason.Location = new System.Drawing.Point(347, 69);
            this.cboSeason.MaxLength = 10;
            this.cboSeason.MoveNext = true;
            this.cboSeason.Name = "cboSeason";
            this.cboSeason.Size = new System.Drawing.Size(121, 20);
            this.cboSeason.TabIndex = 5;
            this.cboSeason.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboSeason_KeyDown);
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
            this.ckM_Label3.Location = new System.Drawing.Point(71, 73);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label3.TabIndex = 111;
            this.ckM_Label3.Text = "年　度";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.cboYear.Location = new System.Drawing.Point(118, 69);
            this.cboYear.MaxLength = 10;
            this.cboYear.MoveNext = true;
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(121, 20);
            this.cboYear.TabIndex = 4;
            this.cboYear.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboYear_KeyDown);
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
            this.ckM_Label8.Location = new System.Drawing.Point(71, 47);
            this.ckM_Label8.Name = "ckM_Label8";
            this.ckM_Label8.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label8.TabIndex = 110;
            this.ckM_Label8.Text = "仕入先";
            this.ckM_Label8.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOrderDateTo
            // 
            this.txtOrderDateTo.AllowMinus = false;
            this.txtOrderDateTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtOrderDateTo.BackColor = System.Drawing.Color.White;
            this.txtOrderDateTo.BorderColor = false;
            this.txtOrderDateTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOrderDateTo.ClientColor = System.Drawing.Color.White;
            this.txtOrderDateTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtOrderDateTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtOrderDateTo.DecimalPlace = 0;
            this.txtOrderDateTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtOrderDateTo.IntegerPart = 8;
            this.txtOrderDateTo.IsCorrectDate = true;
            this.txtOrderDateTo.isEnterKeyDown = false;
            this.txtOrderDateTo.IsFirstTime = true;
            this.txtOrderDateTo.isMaxLengthErr = false;
            this.txtOrderDateTo.IsNumber = true;
            this.txtOrderDateTo.IsShop = false;
            this.txtOrderDateTo.Length = 10;
            this.txtOrderDateTo.Location = new System.Drawing.Point(347, 18);
            this.txtOrderDateTo.MaxLength = 10;
            this.txtOrderDateTo.MoveNext = true;
            this.txtOrderDateTo.Name = "txtOrderDateTo";
            this.txtOrderDateTo.Size = new System.Drawing.Size(100, 19);
            this.txtOrderDateTo.TabIndex = 2;
            this.txtOrderDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtOrderDateTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtOrderDateTo.UseColorSizMode = false;
            this.txtOrderDateTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtOrderDateTo_KeyDown);
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
            this.ckM_Label2.Location = new System.Drawing.Point(274, 20);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label2.TabIndex = 113;
            this.ckM_Label2.Text = "～";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOrderDateFrom
            // 
            this.txtOrderDateFrom.AllowMinus = false;
            this.txtOrderDateFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtOrderDateFrom.BackColor = System.Drawing.Color.White;
            this.txtOrderDateFrom.BorderColor = false;
            this.txtOrderDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOrderDateFrom.ClientColor = System.Drawing.Color.White;
            this.txtOrderDateFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtOrderDateFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtOrderDateFrom.DecimalPlace = 0;
            this.txtOrderDateFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtOrderDateFrom.IntegerPart = 8;
            this.txtOrderDateFrom.IsCorrectDate = true;
            this.txtOrderDateFrom.isEnterKeyDown = false;
            this.txtOrderDateFrom.IsFirstTime = true;
            this.txtOrderDateFrom.isMaxLengthErr = false;
            this.txtOrderDateFrom.IsNumber = true;
            this.txtOrderDateFrom.IsShop = false;
            this.txtOrderDateFrom.Length = 10;
            this.txtOrderDateFrom.Location = new System.Drawing.Point(118, 18);
            this.txtOrderDateFrom.MaxLength = 10;
            this.txtOrderDateFrom.MoveNext = true;
            this.txtOrderDateFrom.Name = "txtOrderDateFrom";
            this.txtOrderDateFrom.Size = new System.Drawing.Size(100, 19);
            this.txtOrderDateFrom.TabIndex = 1;
            this.txtOrderDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtOrderDateFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtOrderDateFrom.UseColorSizMode = false;
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
            this.ckM_Label1.Location = new System.Drawing.Point(71, 21);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label1.TabIndex = 109;
            this.ckM_Label1.Text = "受注日";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScSKUCD
            // 
            this.ScSKUCD.AutoSize = true;
            this.ScSKUCD.ChangeDate = "";
            this.ScSKUCD.ChangeDateWidth = 100;
            this.ScSKUCD.Code = "";
            this.ScSKUCD.CodeWidth = 600;
            this.ScSKUCD.CodeWidth1 = 600;
            this.ScSKUCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScSKUCD.DataCheck = false;
            this.ScSKUCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScSKUCD.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScSKUCD.IsCopy = false;
            this.ScSKUCD.LabelText = "";
            this.ScSKUCD.LabelVisible = false;
            this.ScSKUCD.Location = new System.Drawing.Point(118, 207);
            this.ScSKUCD.Margin = new System.Windows.Forms.Padding(0);
            this.ScSKUCD.Name = "ScSKUCD";
            this.ScSKUCD.NameWidth = 280;
            this.ScSKUCD.SearchEnable = true;
            this.ScSKUCD.Size = new System.Drawing.Size(633, 27);
            this.ScSKUCD.Stype = Search.CKM_SearchControl.SearchType.SKUMulti;
            this.ScSKUCD.TabIndex = 10;
            this.ScSKUCD.test = null;
            this.ScSKUCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScSKUCD.UseChangeDate = false;
            this.ScSKUCD.Value1 = null;
            this.ScSKUCD.Value2 = null;
            this.ScSKUCD.Value3 = null;
            // 
            // ScItem
            // 
            this.ScItem.AutoSize = true;
            this.ScItem.ChangeDate = "";
            this.ScItem.ChangeDateWidth = 100;
            this.ScItem.Code = "";
            this.ScItem.CodeWidth = 600;
            this.ScItem.CodeWidth1 = 600;
            this.ScItem.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScItem.DataCheck = false;
            this.ScItem.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScItem.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScItem.IsCopy = false;
            this.ScItem.LabelText = "";
            this.ScItem.LabelVisible = false;
            this.ScItem.Location = new System.Drawing.Point(118, 181);
            this.ScItem.Margin = new System.Windows.Forms.Padding(0);
            this.ScItem.Name = "ScItem";
            this.ScItem.NameWidth = 280;
            this.ScItem.SearchEnable = true;
            this.ScItem.Size = new System.Drawing.Size(633, 27);
            this.ScItem.Stype = Search.CKM_SearchControl.SearchType.ITEMMulti;
            this.ScItem.TabIndex = 9;
            this.ScItem.test = null;
            this.ScItem.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScItem.UseChangeDate = false;
            this.ScItem.Value1 = null;
            this.ScItem.Value2 = null;
            this.ScItem.Value3 = null;
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
            this.ScCustomer.DataCheck = false;
            this.ScCustomer.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScCustomer.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScCustomer.IsCopy = false;
            this.ScCustomer.LabelText = "";
            this.ScCustomer.LabelVisible = false;
            this.ScCustomer.Location = new System.Drawing.Point(118, 117);
            this.ScCustomer.Margin = new System.Windows.Forms.Padding(0);
            this.ScCustomer.Name = "ScCustomer";
            this.ScCustomer.NameWidth = 500;
            this.ScCustomer.SearchEnable = true;
            this.ScCustomer.Size = new System.Drawing.Size(133, 27);
            this.ScCustomer.Stype = Search.CKM_SearchControl.SearchType.得意先;
            this.ScCustomer.TabIndex = 7;
            this.ScCustomer.test = null;
            this.ScCustomer.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScCustomer.UseChangeDate = false;
            this.ScCustomer.Value1 = null;
            this.ScCustomer.Value2 = null;
            this.ScCustomer.Value3 = null;
            this.ScCustomer.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScCustomer_CodeKeyDownEvent);
            // 
            // scStaff
            // 
            this.scStaff.AutoSize = true;
            this.scStaff.ChangeDate = "";
            this.scStaff.ChangeDateWidth = 100;
            this.scStaff.Code = "";
            this.scStaff.CodeWidth = 70;
            this.scStaff.CodeWidth1 = 70;
            this.scStaff.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.scStaff.DataCheck = false;
            this.scStaff.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.scStaff.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.scStaff.IsCopy = false;
            this.scStaff.LabelText = "";
            this.scStaff.LabelVisible = true;
            this.scStaff.Location = new System.Drawing.Point(118, 91);
            this.scStaff.Margin = new System.Windows.Forms.Padding(0);
            this.scStaff.Name = "scStaff";
            this.scStaff.NameWidth = 250;
            this.scStaff.SearchEnable = true;
            this.scStaff.Size = new System.Drawing.Size(354, 27);
            this.scStaff.Stype = Search.CKM_SearchControl.SearchType.スタッフ;
            this.scStaff.TabIndex = 6;
            this.scStaff.test = null;
            this.scStaff.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.scStaff.UseChangeDate = false;
            this.scStaff.Value1 = null;
            this.scStaff.Value2 = null;
            this.scStaff.Value3 = null;
            this.scStaff.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.scStaff_CodeKeyDownEvent);
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
            this.ScSupplier.Location = new System.Drawing.Point(118, 39);
            this.ScSupplier.Margin = new System.Windows.Forms.Padding(0);
            this.ScSupplier.Name = "ScSupplier";
            this.ScSupplier.NameWidth = 310;
            this.ScSupplier.SearchEnable = true;
            this.ScSupplier.Size = new System.Drawing.Size(444, 27);
            this.ScSupplier.Stype = Search.CKM_SearchControl.SearchType.仕入先;
            this.ScSupplier.TabIndex = 3;
            this.ScSupplier.test = null;
            this.ScSupplier.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScSupplier.UseChangeDate = false;
            this.ScSupplier.Value1 = null;
            this.ScSupplier.Value2 = null;
            this.ScSupplier.Value3 = null;
            this.ScSupplier.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScSupplier_CodeKeyDownEvent);
            // 
            // ScJanCD
            // 
            this.ScJanCD.AutoSize = true;
            this.ScJanCD.ChangeDate = "";
            this.ScJanCD.ChangeDateWidth = 100;
            this.ScJanCD.Code = "";
            this.ScJanCD.CodeWidth = 600;
            this.ScJanCD.CodeWidth1 = 600;
            this.ScJanCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScJanCD.DataCheck = false;
            this.ScJanCD.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScJanCD.IsCopy = false;
            this.ScJanCD.LabelText = "";
            this.ScJanCD.LabelVisible = false;
            this.ScJanCD.Location = new System.Drawing.Point(118, 234);
            this.ScJanCD.Margin = new System.Windows.Forms.Padding(0);
            this.ScJanCD.Name = "ScJanCD";
            this.ScJanCD.NameWidth = 280;
            this.ScJanCD.SearchEnable = true;
            this.ScJanCD.Size = new System.Drawing.Size(633, 30);
            this.ScJanCD.Stype = Search.CKM_SearchControl.SearchType.JANMulti;
            this.ScJanCD.TabIndex = 11;
            this.ScJanCD.test = null;
            this.ScJanCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScJanCD.UseChangeDate = false;
            this.ScJanCD.Value1 = null;
            this.ScJanCD.Value2 = null;
            this.ScJanCD.Value3 = null;
            // 
            // Search_TenzikaiJuchuuNO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 823);
            this.Controls.Add(this.panel1);
            this.F11Visible = true;
            this.F12Visible = true;
            this.F9Visible = true;
            this.Name = "Search_TenzikaiJuchuuNO";
            this.PanelHeaderHeight = 48;
            this.ProgramName = "展示会受注番号検索";
            this.Text = "Search_TenzikaiJuchuuNO";
            this.Load += new System.EventHandler(this.Search_TenzikaiJuchuuNO_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Search_TenzikaiJuchuuNO_KeyUp);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTenzikai)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private CKM_Controls.CKM_TextBox txtCustomerName;
        private CKM_Controls.CKM_GridView dgvTenzikai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSeason;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClient;
        private CKM_Controls.CKM_Button btnDisplay;
        private CKM_Controls.CKM_Label ckM_Label11;
        private CKM_SearchControl ScSKUCD;
        private CKM_Controls.CKM_Label ckM_Label10;
        private CKM_SearchControl ScItem;
        private CKM_Controls.CKM_Label ckM_Label9;
        private CKM_Controls.CKM_Label ckM_Label7;
        private CKM_Controls.CKM_TextBox txtKanaName;
        private CKM_SearchControl ScCustomer;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_SearchControl scStaff;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_ComboBox cboSeason;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_ComboBox cboYear;
        private CKM_SearchControl ScSupplier;
        private CKM_Controls.CKM_Label ckM_Label8;
        private CKM_Controls.CKM_TextBox txtOrderDateTo;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_TextBox txtOrderDateFrom;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_SearchControl ScJanCD;
    }
}