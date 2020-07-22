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
            this.ckmShop_Label3 = new CKM_Controls.CKMShop_Label();
            this.txtDate = new CKM_Controls.CKM_TextBox();
            this.ckmShop_Label2 = new CKM_Controls.CKMShop_Label();
            this.Remark = new CKM_Controls.CKM_MultiLineTextBox();
            this.ckmShop_Label5 = new CKM_Controls.CKMShop_Label();
            this.DepositGaku = new CKM_Controls.CKM_TextBox();
            this.ckmShop_Label1 = new CKM_Controls.CKMShop_Label();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.ckmShop_Label3);
            this.panel5.Controls.Add(this.txtDate);
            this.panel5.Controls.Add(this.ckmShop_Label2);
            this.panel5.Controls.Add(this.Remark);
            this.panel5.Controls.Add(this.ckmShop_Label5);
            this.panel5.Controls.Add(this.DepositGaku);
            this.panel5.Controls.Add(this.ckmShop_Label1);
            this.panel5.Location = new System.Drawing.Point(-2, 90);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1713, 818);
            this.panel5.TabIndex = 1;
            // 
            // ckmShop_Label3
            // 
            this.ckmShop_Label3.AutoSize = true;
            this.ckmShop_Label3.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label3.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label3.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label3.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label3.FontBold = true;
            this.ckmShop_Label3.ForeColor = System.Drawing.Color.Black;
            this.ckmShop_Label3.Location = new System.Drawing.Point(473, 83);
            this.ckmShop_Label3.Name = "ckmShop_Label3";
            this.ckmShop_Label3.Size = new System.Drawing.Size(607, 35);
            this.ckmShop_Label3.TabIndex = 7;
            this.ckmShop_Label3.Text = "この日の開店時のレジ内釣銭金です";
            this.ckmShop_Label3.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Black;
            this.ckmShop_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDate
            // 
            this.txtDate.AllowMinus = false;
            this.txtDate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtDate.BackColor = System.Drawing.Color.White;
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.ClientColor = System.Drawing.SystemColors.Window;
            this.txtDate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtDate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtDate.DecimalPlace = 0;
            this.txtDate.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.txtDate.IntegerPart = 0;
            this.txtDate.IsCorrectDate = true;
            this.txtDate.isEnterKeyDown = false;
            this.txtDate.IsFirstTime = true;
            this.txtDate.isMaxLengthErr = false;
            this.txtDate.IsNumber = true;
            this.txtDate.IsShop = false;
            this.txtDate.Length = 10;
            this.txtDate.Location = new System.Drawing.Point(258, 79);
            this.txtDate.MaxLength = 10;
            this.txtDate.MoveNext = true;
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(210, 42);
            this.txtDate.TabIndex = 0;
            this.txtDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            this.txtDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDate_KeyDown);
            // 
            // ckmShop_Label2
            // 
            this.ckmShop_Label2.AutoSize = true;
            this.ckmShop_Label2.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label2.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label2.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label2.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label2.FontBold = true;
            this.ckmShop_Label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label2.Location = new System.Drawing.Point(130, 83);
            this.ckmShop_Label2.Name = "ckmShop_Label2";
            this.ckmShop_Label2.Size = new System.Drawing.Size(125, 35);
            this.ckmShop_Label2.TabIndex = 5;
            this.ckmShop_Label2.Text = "日　付";
            this.ckmShop_Label2.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Remark
            // 
            this.Remark.Back_Color = CKM_Controls.CKM_MultiLineTextBox.CKM_Color.White;
            this.Remark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.Remark.Ctrl_Byte = CKM_Controls.CKM_MultiLineTextBox.Bytes.半全角;
            this.Remark.F_focus = false;
            this.Remark.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.Remark.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.Remark.Length = 200;
            this.Remark.Location = new System.Drawing.Point(258, 201);
            this.Remark.MaxLength = 200;
            this.Remark.Mdea = false;
            this.Remark.Mfocus = false;
            this.Remark.MoveNext = true;
            this.Remark.Multiline = true;
            this.Remark.Name = "Remark";
            this.Remark.RowCount = 5;
            this.Remark.Size = new System.Drawing.Size(750, 200);
            this.Remark.TabIndex = 3;
            this.Remark.TextSize = CKM_Controls.CKM_MultiLineTextBox.FontSize.Medium;
            // 
            // ckmShop_Label5
            // 
            this.ckmShop_Label5.AutoSize = true;
            this.ckmShop_Label5.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label5.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label5.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label5.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label5.FontBold = true;
            this.ckmShop_Label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label5.Location = new System.Drawing.Point(130, 204);
            this.ckmShop_Label5.Name = "ckmShop_Label5";
            this.ckmShop_Label5.Size = new System.Drawing.Size(125, 35);
            this.ckmShop_Label5.TabIndex = 4;
            this.ckmShop_Label5.Text = "備　考";
            this.ckmShop_Label5.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DepositGaku
            // 
            this.DepositGaku.AllowMinus = true;
            this.DepositGaku.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.DepositGaku.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.DepositGaku.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DepositGaku.ClientColor = System.Drawing.Color.White;
            this.DepositGaku.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.DepositGaku.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Price;
            this.DepositGaku.DecimalPlace = 0;
            this.DepositGaku.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.DepositGaku.IntegerPart = 8;
            this.DepositGaku.IsCorrectDate = true;
            this.DepositGaku.isEnterKeyDown = false;
            this.DepositGaku.IsFirstTime = true;
            this.DepositGaku.isMaxLengthErr = false;
            this.DepositGaku.IsNumber = true;
            this.DepositGaku.IsShop = false;
            this.DepositGaku.Length = 10;
            this.DepositGaku.Location = new System.Drawing.Point(258, 140);
            this.DepositGaku.MaxLength = 10;
            this.DepositGaku.MoveNext = true;
            this.DepositGaku.Name = "DepositGaku";
            this.DepositGaku.Size = new System.Drawing.Size(210, 42);
            this.DepositGaku.TabIndex = 1;
            this.DepositGaku.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.DepositGaku.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            // 
            // ckmShop_Label1
            // 
            this.ckmShop_Label1.AutoSize = true;
            this.ckmShop_Label1.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label1.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label1.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label1.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label1.FontBold = true;
            this.ckmShop_Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label1.Location = new System.Drawing.Point(55, 143);
            this.ckmShop_Label1.Name = "ckmShop_Label1";
            this.ckmShop_Label1.Size = new System.Drawing.Size(200, 35);
            this.ckmShop_Label1.TabIndex = 0;
            this.ckmShop_Label1.Text = "釣銭準備額\t\t\t\t\t\t\t";
            this.ckmShop_Label1.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmTempoRegiTsurisenJyunbi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1913, 961);
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
        private CKM_Controls.CKM_MultiLineTextBox Remark;
        private CKM_Controls.CKMShop_Label ckmShop_Label5;
        private CKM_Controls.CKMShop_Label ckmShop_Label2;
        private CKM_Controls.CKM_TextBox txtDate;
        private CKM_Controls.CKMShop_Label ckmShop_Label3;
    }
}

