namespace Search
{
    partial class Search_Supplier
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
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.lblChangeDate = new System.Windows.Forms.Label();
            this.ckM_Label11 = new CKM_Controls.CKM_Label();
            this.ckM_Label14 = new CKM_Controls.CKM_Label();
            this.txtSupplierName = new CKM_Controls.CKM_TextBox();
            this.txtSupplierFrom = new CKM_Controls.CKM_TextBox();
            this.radioButton1 = new CKM_Controls.CKM_RadioButton();
            this.txtSupplierTo = new CKM_Controls.CKM_TextBox();
            this.ckM_Label8 = new CKM_Controls.CKM_Label();
            this.radioButton2 = new CKM_Controls.CKM_RadioButton();
            this.ckM_Label12 = new CKM_Controls.CKM_Label();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.txtKanaName = new CKM_Controls.CKM_TextBox();
            this.gvSupplier = new CKM_Controls.CKM_GridView();
            this.colCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.F11Show = new CKM_Controls.CKM_Button();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSupplier)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.F11Show);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Controls.Add(this.txtKanaName);
            this.PanelHeader.Controls.Add(this.ckM_Label10);
            this.PanelHeader.Controls.Add(this.lblChangeDate);
            this.PanelHeader.Controls.Add(this.ckM_Label11);
            this.PanelHeader.Controls.Add(this.ckM_Label12);
            this.PanelHeader.Controls.Add(this.ckM_Label14);
            this.PanelHeader.Controls.Add(this.radioButton2);
            this.PanelHeader.Controls.Add(this.txtSupplierName);
            this.PanelHeader.Controls.Add(this.ckM_Label8);
            this.PanelHeader.Controls.Add(this.txtSupplierFrom);
            this.PanelHeader.Controls.Add(this.txtSupplierTo);
            this.PanelHeader.Controls.Add(this.radioButton1);
            this.PanelHeader.Size = new System.Drawing.Size(838, 148);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.Controls.SetChildIndex(this.radioButton1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtSupplierTo, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtSupplierFrom, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label8, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtSupplierName, 0);
            this.PanelHeader.Controls.SetChildIndex(this.radioButton2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label14, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label12, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label11, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblChangeDate, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label10, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtKanaName, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.F11Show, 0);
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
            this.ckM_Label10.Location = new System.Drawing.Point(39, 39);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label10.TabIndex = 81;
            this.ckM_Label10.Text = "表示対象";
            this.ckM_Label10.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblChangeDate
            // 
            this.lblChangeDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblChangeDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblChangeDate.Location = new System.Drawing.Point(101, 10);
            this.lblChangeDate.Name = "lblChangeDate";
            this.lblChangeDate.Size = new System.Drawing.Size(84, 20);
            this.lblChangeDate.TabIndex = 73;
            this.lblChangeDate.Text = "YYYY/MM/DD";
            this.lblChangeDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.ckM_Label11.Location = new System.Drawing.Point(55, 14);
            this.ckM_Label11.Name = "ckM_Label11";
            this.ckM_Label11.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label11.TabIndex = 80;
            this.ckM_Label11.Text = "基準日";
            this.ckM_Label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label14.Location = new System.Drawing.Point(39, 65);
            this.ckM_Label14.Name = "ckM_Label14";
            this.ckM_Label14.Size = new System.Drawing.Size(58, 12);
            this.ckM_Label14.TabIndex = 82;
            this.ckM_Label14.Text = "仕入先CD";
            this.ckM_Label14.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSupplierName
            // 
            this.txtSupplierName.AllowMinus = false;
            this.txtSupplierName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtSupplierName.BackColor = System.Drawing.Color.White;
            this.txtSupplierName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSupplierName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtSupplierName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtSupplierName.DecimalPlace = 0;
            this.txtSupplierName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtSupplierName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtSupplierName.IntegerPart = 0;
            this.txtSupplierName.IsCorrectDate = true;
            this.txtSupplierName.isEnterKeyDown = false;
            this.txtSupplierName.IsNumber = true;
            this.txtSupplierName.IsShop = false;
            this.txtSupplierName.Length = 50;
            this.txtSupplierName.Location = new System.Drawing.Point(99, 89);
            this.txtSupplierName.MaxLength = 50;
            this.txtSupplierName.MoveNext = true;
            this.txtSupplierName.Name = "txtSupplierName";
            this.txtSupplierName.Size = new System.Drawing.Size(250, 19);
            this.txtSupplierName.TabIndex = 4;
            this.txtSupplierName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // txtSupplierFrom
            // 
            this.txtSupplierFrom.AllowMinus = false;
            this.txtSupplierFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtSupplierFrom.BackColor = System.Drawing.Color.White;
            this.txtSupplierFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSupplierFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtSupplierFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtSupplierFrom.DecimalPlace = 0;
            this.txtSupplierFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtSupplierFrom.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSupplierFrom.IntegerPart = 0;
            this.txtSupplierFrom.IsCorrectDate = true;
            this.txtSupplierFrom.isEnterKeyDown = false;
            this.txtSupplierFrom.IsNumber = true;
            this.txtSupplierFrom.IsShop = false;
            this.txtSupplierFrom.Length = 10;
            this.txtSupplierFrom.Location = new System.Drawing.Point(99, 62);
            this.txtSupplierFrom.MaxLength = 10;
            this.txtSupplierFrom.MoveNext = true;
            this.txtSupplierFrom.Name = "txtSupplierFrom";
            this.txtSupplierFrom.Size = new System.Drawing.Size(100, 19);
            this.txtSupplierFrom.TabIndex = 2;
            this.txtSupplierFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.Location = new System.Drawing.Point(99, 37);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(62, 16);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "基準日";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // txtSupplierTo
            // 
            this.txtSupplierTo.AllowMinus = false;
            this.txtSupplierTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtSupplierTo.BackColor = System.Drawing.Color.White;
            this.txtSupplierTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSupplierTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtSupplierTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtSupplierTo.DecimalPlace = 0;
            this.txtSupplierTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtSupplierTo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSupplierTo.IntegerPart = 0;
            this.txtSupplierTo.IsCorrectDate = true;
            this.txtSupplierTo.isEnterKeyDown = false;
            this.txtSupplierTo.IsNumber = true;
            this.txtSupplierTo.IsShop = false;
            this.txtSupplierTo.Length = 10;
            this.txtSupplierTo.Location = new System.Drawing.Point(223, 62);
            this.txtSupplierTo.MaxLength = 10;
            this.txtSupplierTo.MoveNext = true;
            this.txtSupplierTo.Name = "txtSupplierTo";
            this.txtSupplierTo.Size = new System.Drawing.Size(100, 19);
            this.txtSupplierTo.TabIndex = 3;
            this.txtSupplierTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtSupplierTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSupplierTo_KeyDown);
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
            this.ckM_Label8.Location = new System.Drawing.Point(202, 65);
            this.ckM_Label8.Name = "ckM_Label8";
            this.ckM_Label8.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label8.TabIndex = 79;
            this.ckM_Label8.Text = "～";
            this.ckM_Label8.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.Location = new System.Drawing.Point(166, 37);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(49, 16);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "履歴";
            this.radioButton2.UseVisualStyleBackColor = true;
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
            this.ckM_Label12.Location = new System.Drawing.Point(39, 93);
            this.ckM_Label12.Name = "ckM_Label12";
            this.ckM_Label12.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label12.TabIndex = 83;
            this.ckM_Label12.Text = "仕入先名";
            this.ckM_Label12.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label1.Location = new System.Drawing.Point(52, 118);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label1.TabIndex = 85;
            this.ckM_Label1.Text = "カナ名";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtKanaName
            // 
            this.txtKanaName.AllowMinus = false;
            this.txtKanaName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtKanaName.BackColor = System.Drawing.Color.White;
            this.txtKanaName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKanaName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtKanaName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtKanaName.DecimalPlace = 0;
            this.txtKanaName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtKanaName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtKanaName.IntegerPart = 0;
            this.txtKanaName.IsCorrectDate = true;
            this.txtKanaName.isEnterKeyDown = false;
            this.txtKanaName.IsNumber = true;
            this.txtKanaName.IsShop = false;
            this.txtKanaName.Length = 50;
            this.txtKanaName.Location = new System.Drawing.Point(99, 114);
            this.txtKanaName.MaxLength = 50;
            this.txtKanaName.MoveNext = true;
            this.txtKanaName.Name = "txtKanaName";
            this.txtKanaName.Size = new System.Drawing.Size(250, 19);
            this.txtKanaName.TabIndex = 5;
            this.txtKanaName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // gvSupplier
            // 
            this.gvSupplier.AllowUserToAddRows = false;
            this.gvSupplier.AllowUserToDeleteRows = false;
            this.gvSupplier.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.gvSupplier.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gvSupplier.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvSupplier.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gvSupplier.ColumnHeadersHeight = 25;
            this.gvSupplier.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCD,
            this.colName,
            this.colDate});
            this.gvSupplier.EnableHeadersVisualStyles = false;
            this.gvSupplier.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.gvSupplier.Location = new System.Drawing.Point(41, 205);
            this.gvSupplier.Name = "gvSupplier";
            this.gvSupplier.Size = new System.Drawing.Size(615, 282);
            this.gvSupplier.TabIndex = 5;
            this.gvSupplier.UseRowNo = true;
            this.gvSupplier.UseSetting = true;
            this.gvSupplier.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvSupplier_CellDoubleClick);
            // 
            // colCD
            // 
            this.colCD.DataPropertyName = "VendorCD";
            this.colCD.HeaderText = "仕入先CD";
            this.colCD.Name = "colCD";
            this.colCD.Width = 150;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "VendorName";
            this.colName.HeaderText = "仕入先名";
            this.colName.Name = "colName";
            this.colName.Width = 300;
            // 
            // colDate
            // 
            this.colDate.DataPropertyName = "ChangeDate";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.colDate.HeaderText = "改定日";
            this.colDate.Name = "colDate";
            // 
            // F11Show
            // 
            this.F11Show.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.F11Show.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.F11Show.Cursor = System.Windows.Forms.Cursors.Hand;
            this.F11Show.DefaultBtnSize = false;
            this.F11Show.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.F11Show.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.F11Show.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.F11Show.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.F11Show.Location = new System.Drawing.Point(718, 115);
            this.F11Show.Margin = new System.Windows.Forms.Padding(1);
            this.F11Show.Name = "F11Show";
            this.F11Show.Size = new System.Drawing.Size(115, 28);
            this.F11Show.TabIndex = 6;
            this.F11Show.Text = "表示(F11)";
            this.F11Show.UseVisualStyleBackColor = false;
            this.F11Show.Click += new System.EventHandler(this.F11Show_Click);
            // 
            // Search_Supplier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 536);
            this.Controls.Add(this.gvSupplier);
            this.F11Visible = true;
            this.F9Visible = true;
            this.Name = "Search_Supplier";
            this.PanelHeaderHeight = 190;
            this.ProgramName = "仕入先検索画面";
            this.Text = "Search_Supplier";
            this.Load += new System.EventHandler(this.Search_Supplier_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Search_Supplier_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Search_Supplier_KeyUp);
            this.Controls.SetChildIndex(this.gvSupplier, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSupplier)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CKM_Controls.CKM_Label ckM_Label10;
        private System.Windows.Forms.Label lblChangeDate;
        private CKM_Controls.CKM_Label ckM_Label11;
        private CKM_Controls.CKM_Label ckM_Label14;
        private CKM_Controls.CKM_TextBox txtSupplierName;
        private CKM_Controls.CKM_TextBox txtSupplierFrom;
        private CKM_Controls.CKM_RadioButton radioButton1;
        private CKM_Controls.CKM_TextBox txtSupplierTo;
        private CKM_Controls.CKM_Label ckM_Label8;
        private CKM_Controls.CKM_RadioButton radioButton2;
        private CKM_Controls.CKM_Label ckM_Label12;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_TextBox txtKanaName;
        private CKM_Controls.CKM_GridView gvSupplier;
        private CKM_Controls.CKM_Button F11Show;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
    }
}