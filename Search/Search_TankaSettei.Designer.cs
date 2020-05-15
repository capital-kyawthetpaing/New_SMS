namespace Search
{
    partial class Search_TankaSettei
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvDetail = new CKM_Controls.CKM_GridView();
            this.colTankaCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTankaName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GeneralRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MemberRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClientRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SaleRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WebRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RoundKBN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColChangeDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ckM_TextBox2 = new CKM_Controls.CKM_TextBox();
            this.ckM_TextBox1 = new CKM_Controls.CKM_TextBox();
            this.label4 = new CKM_Controls.CKM_Label();
            this.label5 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox3 = new CKM_Controls.CKM_TextBox();
            this.label9 = new CKM_Controls.CKM_Label();
            this.label12 = new CKM_Controls.CKM_Label();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSubF11 = new CKM_Controls.CKM_Button();
            this.lblChangeDate = new System.Windows.Forms.Label();
            this.radioButton2 = new CKM_Controls.CKM_RadioButton();
            this.radioButton1 = new CKM_Controls.CKM_RadioButton();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.radioButton2);
            this.PanelHeader.Controls.Add(this.radioButton1);
            this.PanelHeader.Controls.Add(this.lblChangeDate);
            this.PanelHeader.Controls.Add(this.btnSubF11);
            this.PanelHeader.Controls.Add(this.ckM_TextBox3);
            this.PanelHeader.Controls.Add(this.ckM_TextBox2);
            this.PanelHeader.Controls.Add(this.label4);
            this.PanelHeader.Controls.Add(this.ckM_TextBox1);
            this.PanelHeader.Controls.Add(this.label12);
            this.PanelHeader.Controls.Add(this.label5);
            this.PanelHeader.Controls.Add(this.label9);
            this.PanelHeader.Controls.Add(this.label11);
            this.PanelHeader.Size = new System.Drawing.Size(1011, 128);
            this.PanelHeader.Controls.SetChildIndex(this.label11, 0);
            this.PanelHeader.Controls.SetChildIndex(this.label9, 0);
            this.PanelHeader.Controls.SetChildIndex(this.label5, 0);
            this.PanelHeader.Controls.SetChildIndex(this.label12, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.label4, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox3, 0);
            this.PanelHeader.Controls.SetChildIndex(this.btnSubF11, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblChangeDate, 0);
            this.PanelHeader.Controls.SetChildIndex(this.radioButton1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.radioButton2, 0);
            // 
            // dgvDetail
            // 
            this.dgvDetail.AllowUserToAddRows = false;
            this.dgvDetail.AllowUserToDeleteRows = false;
            this.dgvDetail.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.dgvDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvDetail.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvDetail.ColumnHeadersHeight = 25;
            this.dgvDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTankaCD,
            this.colTankaName,
            this.GeneralRate,
            this.MemberRate,
            this.colClientRate,
            this.SaleRate,
            this.WebRate,
            this.RoundKBN,
            this.ColChangeDate,
            this.Remarks});
            this.dgvDetail.Enabled = false;
            this.dgvDetail.EnableHeadersVisualStyles = false;
            this.dgvDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvDetail.Location = new System.Drawing.Point(11, 182);
            this.dgvDetail.MultiSelect = false;
            this.dgvDetail.Name = "dgvDetail";
            this.dgvDetail.ReadOnly = true;
            this.dgvDetail.Size = new System.Drawing.Size(988, 211);
            this.dgvDetail.TabIndex = 9;
            this.dgvDetail.UseRowNo = true;
            this.dgvDetail.UseSetting = true;
            this.dgvDetail.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DgvDetail_CellPainting);
            this.dgvDetail.DoubleClick += new System.EventHandler(this.DgvDetail_DoubleClick);
            this.dgvDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DgvDetail_KeyDown);
            // 
            // colTankaCD
            // 
            this.colTankaCD.DataPropertyName = "TankaCD";
            this.colTankaCD.HeaderText = "単価設定CD";
            this.colTankaCD.Name = "colTankaCD";
            this.colTankaCD.ReadOnly = true;
            // 
            // colTankaName
            // 
            this.colTankaName.DataPropertyName = "TankaName";
            this.colTankaName.HeaderText = "単価設定名";
            this.colTankaName.Name = "colTankaName";
            this.colTankaName.ReadOnly = true;
            this.colTankaName.Width = 170;
            // 
            // GeneralRate
            // 
            this.GeneralRate.DataPropertyName = "GeneralRate";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N2";
            dataGridViewCellStyle7.NullValue = null;
            this.GeneralRate.DefaultCellStyle = dataGridViewCellStyle7;
            this.GeneralRate.HeaderText = "一般掛率";
            this.GeneralRate.Name = "GeneralRate";
            this.GeneralRate.ReadOnly = true;
            this.GeneralRate.Width = 80;
            // 
            // MemberRate
            // 
            this.MemberRate.DataPropertyName = "MemberRate";
            this.MemberRate.HeaderText = "会員掛率";
            this.MemberRate.Name = "MemberRate";
            this.MemberRate.ReadOnly = true;
            this.MemberRate.Width = 80;
            // 
            // colClientRate
            // 
            this.colClientRate.DataPropertyName = "ClientRate";
            this.colClientRate.HeaderText = "外商掛率";
            this.colClientRate.Name = "colClientRate";
            this.colClientRate.ReadOnly = true;
            this.colClientRate.Width = 80;
            // 
            // SaleRate
            // 
            this.SaleRate.DataPropertyName = "SaleRate";
            this.SaleRate.HeaderText = "Sale掛率";
            this.SaleRate.Name = "SaleRate";
            this.SaleRate.ReadOnly = true;
            this.SaleRate.Width = 80;
            // 
            // WebRate
            // 
            this.WebRate.DataPropertyName = "WebRate";
            this.WebRate.HeaderText = "Web掛率";
            this.WebRate.Name = "WebRate";
            this.WebRate.ReadOnly = true;
            this.WebRate.Width = 80;
            // 
            // RoundKBN
            // 
            this.RoundKBN.DataPropertyName = "RoundKBN";
            this.RoundKBN.HeaderText = "端数処理";
            this.RoundKBN.Name = "RoundKBN";
            this.RoundKBN.ReadOnly = true;
            this.RoundKBN.Width = 80;
            // 
            // ColChangeDate
            // 
            this.ColChangeDate.DataPropertyName = "ChangeDate";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColChangeDate.DefaultCellStyle = dataGridViewCellStyle8;
            this.ColChangeDate.HeaderText = "改定日";
            this.ColChangeDate.Name = "ColChangeDate";
            this.ColChangeDate.ReadOnly = true;
            this.ColChangeDate.Width = 80;
            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.HeaderText = "備考";
            this.Remarks.Name = "Remarks";
            this.Remarks.ReadOnly = true;
            this.Remarks.Width = 200;
            // 
            // ckM_TextBox2
            // 
            this.ckM_TextBox2.AllowMinus = false;
            this.ckM_TextBox2.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox2.ClientColor = System.Drawing.SystemColors.Window;
            this.ckM_TextBox2.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_TextBox2.DecimalPlace = 0;
            this.ckM_TextBox2.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ckM_TextBox2.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.ckM_TextBox2.IntegerPart = 0;
            this.ckM_TextBox2.IsCorrectDate = true;
            this.ckM_TextBox2.isEnterKeyDown = false;
            this.ckM_TextBox2.isMaxLengthErr = false;
            this.ckM_TextBox2.IsNumber = true;
            this.ckM_TextBox2.IsShop = false;
            this.ckM_TextBox2.Length = 13;
            this.ckM_TextBox2.Location = new System.Drawing.Point(252, 75);
            this.ckM_TextBox2.MaxLength = 13;
            this.ckM_TextBox2.MoveNext = true;
            this.ckM_TextBox2.Name = "ckM_TextBox2";
            this.ckM_TextBox2.Size = new System.Drawing.Size(110, 19);
            this.ckM_TextBox2.TabIndex = 3;
            this.ckM_TextBox2.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_TextBox1
            // 
            this.ckM_TextBox1.AllowMinus = false;
            this.ckM_TextBox1.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox1.ClientColor = System.Drawing.SystemColors.Window;
            this.ckM_TextBox1.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_TextBox1.DecimalPlace = 0;
            this.ckM_TextBox1.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ckM_TextBox1.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.ckM_TextBox1.IntegerPart = 0;
            this.ckM_TextBox1.IsCorrectDate = true;
            this.ckM_TextBox1.isEnterKeyDown = false;
            this.ckM_TextBox1.isMaxLengthErr = false;
            this.ckM_TextBox1.IsNumber = true;
            this.ckM_TextBox1.IsShop = false;
            this.ckM_TextBox1.Length = 13;
            this.ckM_TextBox1.Location = new System.Drawing.Point(91, 75);
            this.ckM_TextBox1.MaxLength = 13;
            this.ckM_TextBox1.MoveNext = true;
            this.ckM_TextBox1.Name = "ckM_TextBox1";
            this.ckM_TextBox1.Size = new System.Drawing.Size(110, 19);
            this.ckM_TextBox1.TabIndex = 3;
            this.ckM_TextBox1.Text = "XXXXXXXXXXXXXXXX";
            this.ckM_TextBox1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.DefaultlabelSize = true;
            this.label4.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.label4.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label4.Location = new System.Drawing.Point(17, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 296;
            this.label4.Text = "単価設定CD";
            this.label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.DefaultlabelSize = true;
            this.label5.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.label5.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label5.Location = new System.Drawing.Point(41, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 12);
            this.label5.TabIndex = 297;
            this.label5.Text = "基準日";
            this.label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_TextBox3
            // 
            this.ckM_TextBox3.AllowMinus = false;
            this.ckM_TextBox3.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox3.ClientColor = System.Drawing.SystemColors.Window;
            this.ckM_TextBox3.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox3.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_TextBox3.DecimalPlace = 0;
            this.ckM_TextBox3.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ckM_TextBox3.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.ckM_TextBox3.IntegerPart = 0;
            this.ckM_TextBox3.IsCorrectDate = true;
            this.ckM_TextBox3.isEnterKeyDown = false;
            this.ckM_TextBox3.isMaxLengthErr = false;
            this.ckM_TextBox3.IsNumber = true;
            this.ckM_TextBox3.IsShop = false;
            this.ckM_TextBox3.Length = 20;
            this.ckM_TextBox3.Location = new System.Drawing.Point(91, 104);
            this.ckM_TextBox3.MaxLength = 20;
            this.ckM_TextBox3.MoveNext = true;
            this.ckM_TextBox3.Name = "ckM_TextBox3";
            this.ckM_TextBox3.Size = new System.Drawing.Size(167, 19);
            this.ckM_TextBox3.TabIndex = 5;
            this.ckM_TextBox3.Text = "ああああああああああ";
            this.ckM_TextBox3.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.DefaultlabelSize = true;
            this.label9.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.label9.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label9.Location = new System.Drawing.Point(17, 107);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 12);
            this.label9.TabIndex = 299;
            this.label9.Text = "単価設定名";
            this.label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.DefaultlabelSize = true;
            this.label12.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.label12.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label12.Location = new System.Drawing.Point(29, 49);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 12);
            this.label12.TabIndex = 303;
            this.label12.Text = "表示対象";
            this.label12.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(216, 78);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 12);
            this.label11.TabIndex = 304;
            this.label11.Text = "～";
            // 
            // btnSubF11
            // 
            this.btnSubF11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnSubF11.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnSubF11.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubF11.DefaultBtnSize = true;
            this.btnSubF11.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSubF11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubF11.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnSubF11.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnSubF11.Location = new System.Drawing.Point(881, 95);
            this.btnSubF11.Margin = new System.Windows.Forms.Padding(1);
            this.btnSubF11.Name = "btnSubF11";
            this.btnSubF11.Size = new System.Drawing.Size(118, 28);
            this.btnSubF11.TabIndex = 6;
            this.btnSubF11.Text = "表示(F11)";
            this.btnSubF11.UseVisualStyleBackColor = false;
            this.btnSubF11.Click += new System.EventHandler(this.BtnSubF11_Click);
            // 
            // lblChangeDate
            // 
            this.lblChangeDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblChangeDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblChangeDate.Location = new System.Drawing.Point(91, 16);
            this.lblChangeDate.Name = "lblChangeDate";
            this.lblChangeDate.Size = new System.Drawing.Size(84, 20);
            this.lblChangeDate.TabIndex = 0;
            this.lblChangeDate.Text = "YYYY/MM/DD";
            this.lblChangeDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.radioButton2.Location = new System.Drawing.Point(159, 48);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(49, 16);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "履歴";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.radioButton1.Location = new System.Drawing.Point(91, 49);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(62, 16);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "基準日";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // Search_TankaSettei
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1011, 444);
            this.Controls.Add(this.dgvDetail);
            this.F11Visible = true;
            this.F9Visible = true;
            this.Name = "Search_TankaSettei";
            this.PanelHeaderHeight = 170;
            this.Text = "単価設定検索";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.dgvDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        
        private CKM_Controls.CKM_GridView dgvDetail;
        private CKM_Controls.CKM_TextBox ckM_TextBox2;
        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private CKM_Controls.CKM_Label label4;
        private CKM_Controls.CKM_Label label5;
        private CKM_Controls.CKM_TextBox ckM_TextBox3;
        private CKM_Controls.CKM_Label label9;
        private CKM_Controls.CKM_Label label12;
        private System.Windows.Forms.Label label11;
        private CKM_Controls.CKM_Button btnSubF11;
        private System.Windows.Forms.Label lblChangeDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTankaCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTankaName;
        private System.Windows.Forms.DataGridViewTextBoxColumn GeneralRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn MemberRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClientRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn SaleRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn WebRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn RoundKBN;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColChangeDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private CKM_Controls.CKM_RadioButton radioButton2;
        private CKM_Controls.CKM_RadioButton radioButton1;
    }
}

