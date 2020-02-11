namespace TempoRegiTsurisenJyunbi
{
    partial class frmTempoRegiTsurisenJyunbi
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
            this.panel5 = new System.Windows.Forms.Panel();
            this.DenominationCD = new CKM_Controls.CKMShop_ComboBox();
            this.Remark = new CKM_Controls.CKM_MultiLineTextBox();
            this.ckmShop_Label5 = new CKM_Controls.CKMShop_Label();
            this.ckmShop_Label4 = new CKM_Controls.CKMShop_Label();
            this.DepositGaku = new CKM_Controls.CKM_TextBox();
            this.ckmShop_Label1 = new CKM_Controls.CKMShop_Label();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.DenominationCD);
            this.panel5.Controls.Add(this.Remark);
            this.panel5.Controls.Add(this.ckmShop_Label5);
            this.panel5.Controls.Add(this.ckmShop_Label4);
            this.panel5.Controls.Add(this.DepositGaku);
            this.panel5.Controls.Add(this.ckmShop_Label1);
            this.panel5.Location = new System.Drawing.Point(-2, 90);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1713, 818);
            this.panel5.TabIndex = 1;
            // 
            // DenominationCD
            // 
            this.DenominationCD.Alignment = CKM_Controls.CKMShop_ComboBox.Align.right;
            this.DenominationCD.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.DenominationCD.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.DenominationCD.Cbo_Type = CKM_Controls.CKMShop_ComboBox.CboType.貨幣金種名;
            this.DenominationCD.cboalign = CKM_Controls.CKMShop_ComboBox.Align.right;
            this.DenominationCD.ComboAlign = CKM_Controls.CKMShop_ComboBox.Align.right;
            this.DenominationCD.Ctrl_Byte = CKM_Controls.CKMShop_ComboBox.Bytes.半角;
            this.DenominationCD.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.DenominationCD.Font = new System.Drawing.Font("Meiryo UI", 18F);
            this.DenominationCD.FontSize_ = 18F;
            this.DenominationCD.FormattingEnabled = true;
            this.DenominationCD.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.DenominationCD.ItemHeight = 26;
            this.DenominationCD.ItemHeight_ = 26;
            this.DenominationCD.Length = 10;
            this.DenominationCD.Location = new System.Drawing.Point(255, 191);
            this.DenominationCD.MaxItem = 8;
            this.DenominationCD.MaxLength = 10;
            this.DenominationCD.MoveNext = true;
            this.DenominationCD.Name = "DenominationCD";
            this.DenominationCD.Size = new System.Drawing.Size(216, 32);
            this.DenominationCD.TabIndex = 2;
            // 
            // Remark
            // 
            this.Remark.Back_Color = CKM_Controls.CKM_MultiLineTextBox.CKM_Color.White;
            this.Remark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.Remark.Ctrl_Byte = CKM_Controls.CKM_MultiLineTextBox.Bytes.半全角;
            this.Remark.Font = new System.Drawing.Font("MS Gothic", 16F);
            this.Remark.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.Remark.Length = 200;
            this.Remark.Location = new System.Drawing.Point(255, 243);
            this.Remark.MaxLength = 200;
            this.Remark.Mdea = false;
            this.Remark.Mfocus = false;
            this.Remark.MoveNext = true;
            this.Remark.Multiline = true;
            this.Remark.Name = "Remark";
            this.Remark.RowCount = 5;
            this.Remark.Size = new System.Drawing.Size(460, 120);
            this.Remark.TabIndex = 3;
            this.Remark.TextSize = CKM_Controls.CKM_MultiLineTextBox.FontSize.Medium;
            // 
            // ckmShop_Label5
            // 
            this.ckmShop_Label5.AutoSize = true;
            this.ckmShop_Label5.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label5.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label5.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label5.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label5.Location = new System.Drawing.Point(167, 246);
            this.ckmShop_Label5.Name = "ckmShop_Label5";
            this.ckmShop_Label5.Size = new System.Drawing.Size(85, 24);
            this.ckmShop_Label5.TabIndex = 4;
            this.ckmShop_Label5.Text = "備　考";
            this.ckmShop_Label5.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckmShop_Label4
            // 
            this.ckmShop_Label4.AutoSize = true;
            this.ckmShop_Label4.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label4.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label4.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label4.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label4.Location = new System.Drawing.Point(144, 194);
            this.ckmShop_Label4.Name = "ckmShop_Label4";
            this.ckmShop_Label4.Size = new System.Drawing.Size(110, 24);
            this.ckmShop_Label4.TabIndex = 2;
            this.ckmShop_Label4.Text = "貨幣金種";
            this.ckmShop_Label4.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DepositGaku
            // 
            this.DepositGaku.AllowMinus = true;
            this.DepositGaku.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.DepositGaku.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.DepositGaku.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DepositGaku.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.DepositGaku.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Price;
            this.DepositGaku.DecimalPlace = 0;
            this.DepositGaku.Font = new System.Drawing.Font("MS Gothic", 16F);
            this.DepositGaku.IntegerPart = 8;
            this.DepositGaku.IsCorrectDate = true;
            this.DepositGaku.isEnterKeyDown = false;
            this.DepositGaku.IsNumber = true;
            this.DepositGaku.IsShop = false;
            this.DepositGaku.Length = 10;
            this.DepositGaku.Location = new System.Drawing.Point(255, 140);
            this.DepositGaku.MaxLength = 10;
            this.DepositGaku.MoveNext = true;
            this.DepositGaku.Name = "DepositGaku";
            this.DepositGaku.Size = new System.Drawing.Size(188, 29);
            this.DepositGaku.TabIndex = 1;
            this.DepositGaku.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.DepositGaku.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            // 
            // ckmShop_Label1
            // 
            this.ckmShop_Label1.AutoSize = true;
            this.ckmShop_Label1.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label1.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label1.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label1.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label1.Location = new System.Drawing.Point(169, 143);
            this.ckmShop_Label1.Name = "ckmShop_Label1";
            this.ckmShop_Label1.Size = new System.Drawing.Size(85, 24);
            this.ckmShop_Label1.TabIndex = 0;
            this.ckmShop_Label1.Text = "入金額";
            this.ckmShop_Label1.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmTempoRegiTsurisenJyunbi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.panel5);
            this.Name = "frmTempoRegiTsurisenJyunbi";
            this.Text = "店舗レジ 釣銭準備入力";
            this.Load += new System.EventHandler(this.frmTempoRegiTsurisenJyunbi_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmTempoRegiTsurisenJyunbi_KeyUp);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel5;
        private CKM_Controls.CKMShop_Label ckmShop_Label1;
        private CKM_Controls.CKM_TextBox DepositGaku;
        private CKM_Controls.CKMShop_Label ckmShop_Label4;
        private CKM_Controls.CKM_MultiLineTextBox Remark;
        private CKM_Controls.CKMShop_Label ckmShop_Label5;
        private CKM_Controls.CKMShop_ComboBox DenominationCD;
    }
}

