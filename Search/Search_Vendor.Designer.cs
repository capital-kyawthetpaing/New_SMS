namespace Search
{
    partial class Search_Vendor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.txtChangeDate = new CKM_Controls.CKM_TextBox();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.txtVendorName = new CKM_Controls.CKM_TextBox();
            this.txtVendorKana = new CKM_Controls.CKM_TextBox();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.txtNotDisplayNote = new CKM_Controls.CKM_TextBox();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.txtSupplierNoFrom = new CKM_Controls.CKM_TextBox();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.txtSupplierNoTo = new CKM_Controls.CKM_TextBox();
            this.btnSearch = new CKM_Controls.CKM_Button();
            this.dgvSearchVendor = new CKM_Controls.CKM_GridView();
            this.lblVendorKBN = new CKM_Controls.CKM_Label();
            this.colVendorCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVendorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChangeDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchVendor)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.lblVendorKBN);
            this.PanelHeader.Controls.Add(this.btnSearch);
            this.PanelHeader.Controls.Add(this.txtSupplierNoTo);
            this.PanelHeader.Controls.Add(this.ckM_Label6);
            this.PanelHeader.Controls.Add(this.txtSupplierNoFrom);
            this.PanelHeader.Controls.Add(this.ckM_Label5);
            this.PanelHeader.Controls.Add(this.txtNotDisplayNote);
            this.PanelHeader.Controls.Add(this.ckM_Label4);
            this.PanelHeader.Controls.Add(this.ckM_Label3);
            this.PanelHeader.Controls.Add(this.txtVendorKana);
            this.PanelHeader.Controls.Add(this.txtVendorName);
            this.PanelHeader.Controls.Add(this.ckM_Label2);
            this.PanelHeader.Controls.Add(this.txtChangeDate);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Size = new System.Drawing.Size(916, 158);
            this.PanelHeader.TabIndex = 1;
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtChangeDate, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtVendorName, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtVendorKana, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label3, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label4, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtNotDisplayNote, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label5, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtSupplierNoFrom, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label6, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtSupplierNoTo, 0);
            this.PanelHeader.Controls.SetChildIndex(this.btnSearch, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblVendorKBN, 0);
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
            this.ckM_Label1.Location = new System.Drawing.Point(117, 13);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label1.TabIndex = 0;
            this.ckM_Label1.Text = "基準日";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtChangeDate
            // 
            this.txtChangeDate.AllowMinus = false;
            this.txtChangeDate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtChangeDate.BackColor = System.Drawing.Color.White;
            this.txtChangeDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtChangeDate.ClientColor = System.Drawing.Color.White;
            this.txtChangeDate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtChangeDate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtChangeDate.DecimalPlace = 0;
            this.txtChangeDate.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtChangeDate.IntegerPart = 0;
            this.txtChangeDate.IsCorrectDate = true;
            this.txtChangeDate.isEnterKeyDown = false;
            this.txtChangeDate.IsFirstTime = true;
            this.txtChangeDate.isMaxLengthErr = false;
            this.txtChangeDate.IsNumber = true;
            this.txtChangeDate.IsShop = false;
            this.txtChangeDate.Length = 8;
            this.txtChangeDate.Location = new System.Drawing.Point(164, 10);
            this.txtChangeDate.MaxLength = 8;
            this.txtChangeDate.MoveNext = true;
            this.txtChangeDate.Name = "txtChangeDate";
            this.txtChangeDate.Size = new System.Drawing.Size(100, 19);
            this.txtChangeDate.TabIndex = 0;
            this.txtChangeDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtChangeDate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_Label2.Location = new System.Drawing.Point(104, 39);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label2.TabIndex = 9;
            this.ckM_Label2.Text = "仕入先名";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtVendorName
            // 
            this.txtVendorName.AllowMinus = false;
            this.txtVendorName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtVendorName.BackColor = System.Drawing.Color.White;
            this.txtVendorName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVendorName.ClientColor = System.Drawing.Color.White;
            this.txtVendorName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtVendorName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtVendorName.DecimalPlace = 0;
            this.txtVendorName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtVendorName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtVendorName.IntegerPart = 0;
            this.txtVendorName.IsCorrectDate = true;
            this.txtVendorName.isEnterKeyDown = false;
            this.txtVendorName.IsFirstTime = true;
            this.txtVendorName.isMaxLengthErr = false;
            this.txtVendorName.IsNumber = true;
            this.txtVendorName.IsShop = false;
            this.txtVendorName.Length = 80;
            this.txtVendorName.Location = new System.Drawing.Point(164, 35);
            this.txtVendorName.MaxLength = 80;
            this.txtVendorName.MoveNext = true;
            this.txtVendorName.Name = "txtVendorName";
            this.txtVendorName.Size = new System.Drawing.Size(491, 19);
            this.txtVendorName.TabIndex = 1;
            this.txtVendorName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // txtVendorKana
            // 
            this.txtVendorKana.AllowMinus = false;
            this.txtVendorKana.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtVendorKana.BackColor = System.Drawing.Color.White;
            this.txtVendorKana.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVendorKana.ClientColor = System.Drawing.Color.White;
            this.txtVendorKana.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtVendorKana.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtVendorKana.DecimalPlace = 0;
            this.txtVendorKana.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtVendorKana.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.txtVendorKana.IntegerPart = 0;
            this.txtVendorKana.IsCorrectDate = true;
            this.txtVendorKana.isEnterKeyDown = false;
            this.txtVendorKana.IsFirstTime = true;
            this.txtVendorKana.isMaxLengthErr = false;
            this.txtVendorKana.IsNumber = true;
            this.txtVendorKana.IsShop = false;
            this.txtVendorKana.Length = 30;
            this.txtVendorKana.Location = new System.Drawing.Point(164, 59);
            this.txtVendorKana.MaxLength = 30;
            this.txtVendorKana.MoveNext = true;
            this.txtVendorKana.Name = "txtVendorKana";
            this.txtVendorKana.Size = new System.Drawing.Size(201, 19);
            this.txtVendorKana.TabIndex = 2;
            this.txtVendorKana.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_Label3.Location = new System.Drawing.Point(117, 62);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label3.TabIndex = 13;
            this.ckM_Label3.Text = "カナ名";
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
            this.ckM_Label4.Location = new System.Drawing.Point(52, 85);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(109, 12);
            this.ckM_Label4.TabIndex = 14;
            this.ckM_Label4.Text = "備考欄キーワード";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNotDisplayNote
            // 
            this.txtNotDisplayNote.AllowMinus = false;
            this.txtNotDisplayNote.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtNotDisplayNote.BackColor = System.Drawing.Color.White;
            this.txtNotDisplayNote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNotDisplayNote.ClientColor = System.Drawing.Color.White;
            this.txtNotDisplayNote.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtNotDisplayNote.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtNotDisplayNote.DecimalPlace = 0;
            this.txtNotDisplayNote.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtNotDisplayNote.IntegerPart = 0;
            this.txtNotDisplayNote.IsCorrectDate = true;
            this.txtNotDisplayNote.isEnterKeyDown = false;
            this.txtNotDisplayNote.IsFirstTime = true;
            this.txtNotDisplayNote.isMaxLengthErr = false;
            this.txtNotDisplayNote.IsNumber = true;
            this.txtNotDisplayNote.IsShop = false;
            this.txtNotDisplayNote.Length = 80;
            this.txtNotDisplayNote.Location = new System.Drawing.Point(164, 82);
            this.txtNotDisplayNote.MaxLength = 80;
            this.txtNotDisplayNote.MoveNext = true;
            this.txtNotDisplayNote.Name = "txtNotDisplayNote";
            this.txtNotDisplayNote.Size = new System.Drawing.Size(553, 19);
            this.txtNotDisplayNote.TabIndex = 3;
            this.txtNotDisplayNote.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtNotDisplayNote.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNotDisplayNote_KeyDown);
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
            this.ckM_Label5.Location = new System.Drawing.Point(91, 113);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label5.TabIndex = 16;
            this.ckM_Label5.Text = "仕入先番号";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSupplierNoFrom
            // 
            this.txtSupplierNoFrom.AllowMinus = false;
            this.txtSupplierNoFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtSupplierNoFrom.BackColor = System.Drawing.Color.White;
            this.txtSupplierNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSupplierNoFrom.ClientColor = System.Drawing.Color.White;
            this.txtSupplierNoFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtSupplierNoFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtSupplierNoFrom.DecimalPlace = 0;
            this.txtSupplierNoFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtSupplierNoFrom.IntegerPart = 0;
            this.txtSupplierNoFrom.IsCorrectDate = true;
            this.txtSupplierNoFrom.isEnterKeyDown = false;
            this.txtSupplierNoFrom.IsFirstTime = true;
            this.txtSupplierNoFrom.isMaxLengthErr = false;
            this.txtSupplierNoFrom.IsNumber = true;
            this.txtSupplierNoFrom.IsShop = false;
            this.txtSupplierNoFrom.Length = 13;
            this.txtSupplierNoFrom.Location = new System.Drawing.Point(164, 109);
            this.txtSupplierNoFrom.MaxLength = 13;
            this.txtSupplierNoFrom.MoveNext = true;
            this.txtSupplierNoFrom.Name = "txtSupplierNoFrom";
            this.txtSupplierNoFrom.Size = new System.Drawing.Size(100, 19);
            this.txtSupplierNoFrom.TabIndex = 4;
            this.txtSupplierNoFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_Label6.Location = new System.Drawing.Point(279, 113);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label6.TabIndex = 5;
            this.ckM_Label6.Text = "～";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSupplierNoTo
            // 
            this.txtSupplierNoTo.AllowMinus = false;
            this.txtSupplierNoTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtSupplierNoTo.BackColor = System.Drawing.Color.White;
            this.txtSupplierNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSupplierNoTo.ClientColor = System.Drawing.Color.White;
            this.txtSupplierNoTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtSupplierNoTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtSupplierNoTo.DecimalPlace = 0;
            this.txtSupplierNoTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtSupplierNoTo.IntegerPart = 0;
            this.txtSupplierNoTo.IsCorrectDate = true;
            this.txtSupplierNoTo.isEnterKeyDown = false;
            this.txtSupplierNoTo.IsFirstTime = true;
            this.txtSupplierNoTo.isMaxLengthErr = false;
            this.txtSupplierNoTo.IsNumber = true;
            this.txtSupplierNoTo.IsShop = false;
            this.txtSupplierNoTo.Length = 13;
            this.txtSupplierNoTo.Location = new System.Drawing.Point(311, 110);
            this.txtSupplierNoTo.MaxLength = 13;
            this.txtSupplierNoTo.MoveNext = true;
            this.txtSupplierNoTo.Name = "txtSupplierNoTo";
            this.txtSupplierNoTo.Size = new System.Drawing.Size(100, 19);
            this.txtSupplierNoTo.TabIndex = 5;
            this.txtSupplierNoTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtSupplierNoTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSupplierNoTo_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnSearch.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.DefaultBtnSize = false;
            this.btnSearch.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnSearch.Location = new System.Drawing.Point(785, 113);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(118, 28);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "表示(F11)";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvSearchVendor
            // 
            this.dgvSearchVendor.AllowUserToAddRows = false;
            this.dgvSearchVendor.AllowUserToDeleteRows = false;
            this.dgvSearchVendor.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.dgvSearchVendor.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSearchVendor.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSearchVendor.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSearchVendor.ColumnHeadersHeight = 25;
            this.dgvSearchVendor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVendorCD,
            this.colVendorName,
            this.Column3,
            this.Column4,
            this.colChangeDate});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("MS Gothic", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSearchVendor.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSearchVendor.EnableHeadersVisualStyles = false;
            this.dgvSearchVendor.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvSearchVendor.Location = new System.Drawing.Point(25, 214);
            this.dgvSearchVendor.Name = "dgvSearchVendor";
            this.dgvSearchVendor.RowHeight_ = 20;
            this.dgvSearchVendor.RowTemplate.Height = 20;
            this.dgvSearchVendor.Size = new System.Drawing.Size(876, 290);
            this.dgvSearchVendor.TabIndex = 9;
            this.dgvSearchVendor.UseRowNo = true;
            this.dgvSearchVendor.UseSetting = true;
            this.dgvSearchVendor.DoubleClick += new System.EventHandler(this.dgvSearchVendor_DoubleClick);
            // 
            // lblVendorKBN
            // 
            this.lblVendorKBN.AutoSize = true;
            this.lblVendorKBN.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblVendorKBN.BackColor = System.Drawing.Color.Transparent;
            this.lblVendorKBN.DefaultlabelSize = true;
            this.lblVendorKBN.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.lblVendorKBN.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.lblVendorKBN.ForeColor = System.Drawing.Color.Black;
            this.lblVendorKBN.Location = new System.Drawing.Point(959, 10);
            this.lblVendorKBN.Name = "lblVendorKBN";
            this.lblVendorKBN.Size = new System.Drawing.Size(0, 12);
            this.lblVendorKBN.TabIndex = 19;
            this.lblVendorKBN.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblVendorKBN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblVendorKBN.Visible = false;
            // 
            // colVendorCD
            // 
            this.colVendorCD.DataPropertyName = "VendorCD";
            this.colVendorCD.HeaderText = "仕入先番号";
            this.colVendorCD.Name = "colVendorCD";
            this.colVendorCD.ReadOnly = true;
            this.colVendorCD.Width = 90;
            // 
            // colVendorName
            // 
            this.colVendorName.DataPropertyName = "VendorName";
            this.colVendorName.HeaderText = "仕入先名";
            this.colVendorName.Name = "colVendorName";
            this.colVendorName.ReadOnly = true;
            this.colVendorName.Width = 280;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "VendorKana";
            this.Column3.HeaderText = "カナ名";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 90;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "NotDisplyNote";
            this.Column4.HeaderText = "備考";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 350;
            // 
            // colChangeDate
            // 
            this.colChangeDate.DataPropertyName = "ChangeDate";
            this.colChangeDate.HeaderText = "基準日";
            this.colChangeDate.Name = "colChangeDate";
            this.colChangeDate.Visible = false;
            // 
            // Search_Vendor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 590);
            this.Controls.Add(this.dgvSearchVendor);
            this.F11Visible = true;
            this.F12Visible = true;
            this.F9Visible = true;
            this.Name = "Search_Vendor";
            this.PanelHeaderHeight = 200;
            this.ProgramName = "仕入先・支払先検索";
            this.Text = "Search_Vendor";
            this.Load += new System.EventHandler(this.Search_Vendor_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Search_Vendor_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Search_Vendor_KeyUp);
            this.Controls.SetChildIndex(this.dgvSearchVendor, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchVendor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_TextBox txtVendorName;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_TextBox txtChangeDate;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_TextBox txtVendorKana;
        private CKM_Controls.CKM_TextBox txtSupplierNoFrom;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_TextBox txtNotDisplayNote;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_Button btnSearch;
        private CKM_Controls.CKM_TextBox txtSupplierNoTo;
        private CKM_Controls.CKM_GridView dgvSearchVendor;
        private CKM_Controls.CKM_Label lblVendorKBN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVendorCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVendorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChangeDate;
    }
}