namespace TempoRegiRyousyuusyo
{
    partial class TempoRegiRyousyuusyo
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
            this.chkReissue = new CKM_Controls.CKMShop_CheckBox();
            this.lblReissue = new CKM_Controls.CKMShop_Label();
            this.txtPrintDate = new CKM_Controls.CKM_TextBox();
            this.chkReceipt = new CKM_Controls.CKMShop_CheckBox();
            this.lblReceipt = new CKM_Controls.CKMShop_Label();
            this.chkRyousyuusho = new CKM_Controls.CKMShop_CheckBox();
            this.lblRyousyuusho = new CKM_Controls.CKMShop_Label();
            this.txtSalesNO = new CKM_Controls.CKM_TextBox();
            this.lblPrintDate = new CKM_Controls.CKMShop_Label();
            this.lblSalseNo = new CKM_Controls.CKMShop_Label();
            this.panelDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.chkReissue);
            this.panelDetail.Controls.Add(this.lblReissue);
            this.panelDetail.Controls.Add(this.txtPrintDate);
            this.panelDetail.Controls.Add(this.chkReceipt);
            this.panelDetail.Controls.Add(this.lblReceipt);
            this.panelDetail.Controls.Add(this.chkRyousyuusho);
            this.panelDetail.Controls.Add(this.lblRyousyuusho);
            this.panelDetail.Controls.Add(this.txtSalesNO);
            this.panelDetail.Controls.Add(this.lblPrintDate);
            this.panelDetail.Controls.Add(this.lblSalseNo);
            this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetail.Location = new System.Drawing.Point(0, 69);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(1713, 731);
            this.panelDetail.TabIndex = 14;
            // 
            // chkReissue
            // 
            this.chkReissue.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.chkReissue.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Bold);
            this.chkReissue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.chkReissue.IsattachedCaption = false;
            this.chkReissue.Location = new System.Drawing.Point(322, 323);
            this.chkReissue.Name = "chkReissue";
            this.chkReissue.Size = new System.Drawing.Size(35, 35);
            this.chkReissue.TabIndex = 5;
            this.chkReissue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkReissue.UseVisualStyleBackColor = true;
            this.chkReissue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chkReissue_KeyDown);
            // 
            // lblReissue
            // 
            this.lblReissue.AutoSize = true;
            this.lblReissue.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblReissue.BackColor = System.Drawing.Color.Transparent;
            this.lblReissue.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F, System.Drawing.FontStyle.Bold);
            this.lblReissue.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.lblReissue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblReissue.Location = new System.Drawing.Point(190, 323);
            this.lblReissue.Name = "lblReissue";
            this.lblReissue.Size = new System.Drawing.Size(126, 35);
            this.lblReissue.TabIndex = 25;
            this.lblReissue.Text = "再発行";
            this.lblReissue.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblReissue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPrintDate
            // 
            this.txtPrintDate.AllowMinus = true;
            this.txtPrintDate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.Green;
            this.txtPrintDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtPrintDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPrintDate.ClientColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtPrintDate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtPrintDate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtPrintDate.DecimalPlace = 0;
            this.txtPrintDate.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F);
            this.txtPrintDate.IntegerPart = 8;
            this.txtPrintDate.IsCorrectDate = true;
            this.txtPrintDate.isEnterKeyDown = false;
            this.txtPrintDate.isMaxLengthErr = false;
            this.txtPrintDate.IsNumber = true;
            this.txtPrintDate.IsShop = false;
            this.txtPrintDate.Length = 10;
            this.txtPrintDate.Location = new System.Drawing.Point(322, 275);
            this.txtPrintDate.MaxLength = 10;
            this.txtPrintDate.MoveNext = true;
            this.txtPrintDate.Name = "txtPrintDate";
            this.txtPrintDate.Size = new System.Drawing.Size(184, 42);
            this.txtPrintDate.TabIndex = 4;
            this.txtPrintDate.Text = "9999/99/99";
            this.txtPrintDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPrintDate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            this.txtPrintDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPrintDate_KeyDown);
            // 
            // chkReceipt
            // 
            this.chkReceipt.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.chkReceipt.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Bold);
            this.chkReceipt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.chkReceipt.IsattachedCaption = false;
            this.chkReceipt.Location = new System.Drawing.Point(322, 234);
            this.chkReceipt.Name = "chkReceipt";
            this.chkReceipt.Size = new System.Drawing.Size(35, 35);
            this.chkReceipt.TabIndex = 3;
            this.chkReceipt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkReceipt.UseVisualStyleBackColor = true;
            this.chkReceipt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chkReceipt_KeyDown);
            // 
            // lblReceipt
            // 
            this.lblReceipt.AutoSize = true;
            this.lblReceipt.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblReceipt.BackColor = System.Drawing.Color.Transparent;
            this.lblReceipt.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F, System.Drawing.FontStyle.Bold);
            this.lblReceipt.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.lblReceipt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblReceipt.Location = new System.Drawing.Point(153, 234);
            this.lblReceipt.Name = "lblReceipt";
            this.lblReceipt.Size = new System.Drawing.Size(163, 35);
            this.lblReceipt.TabIndex = 22;
            this.lblReceipt.Text = "レシート";
            this.lblReceipt.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblReceipt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkRyousyuusho
            // 
            this.chkRyousyuusho.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.chkRyousyuusho.Checked = true;
            this.chkRyousyuusho.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRyousyuusho.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Bold);
            this.chkRyousyuusho.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.chkRyousyuusho.IsattachedCaption = false;
            this.chkRyousyuusho.Location = new System.Drawing.Point(322, 193);
            this.chkRyousyuusho.Name = "chkRyousyuusho";
            this.chkRyousyuusho.Size = new System.Drawing.Size(35, 35);
            this.chkRyousyuusho.TabIndex = 2;
            this.chkRyousyuusho.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkRyousyuusho.UseVisualStyleBackColor = true;
            this.chkRyousyuusho.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chkRyousyuusho_KeyDown);
            // 
            // lblRyousyuusho
            // 
            this.lblRyousyuusho.AutoSize = true;
            this.lblRyousyuusho.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblRyousyuusho.BackColor = System.Drawing.Color.Transparent;
            this.lblRyousyuusho.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F, System.Drawing.FontStyle.Bold);
            this.lblRyousyuusho.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.lblRyousyuusho.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblRyousyuusho.Location = new System.Drawing.Point(190, 193);
            this.lblRyousyuusho.Name = "lblRyousyuusho";
            this.lblRyousyuusho.Size = new System.Drawing.Size(126, 35);
            this.lblRyousyuusho.TabIndex = 20;
            this.lblRyousyuusho.Text = "領収書";
            this.lblRyousyuusho.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblRyousyuusho.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSalesNO
            // 
            this.txtSalesNO.AllowMinus = true;
            this.txtSalesNO.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.Green;
            this.txtSalesNO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtSalesNO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSalesNO.ClientColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtSalesNO.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtSalesNO.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtSalesNO.DecimalPlace = 0;
            this.txtSalesNO.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F);
            this.txtSalesNO.IntegerPart = 8;
            this.txtSalesNO.IsCorrectDate = true;
            this.txtSalesNO.isEnterKeyDown = false;
            this.txtSalesNO.isMaxLengthErr = false;
            this.txtSalesNO.IsNumber = true;
            this.txtSalesNO.IsShop = false;
            this.txtSalesNO.Length = 11;
            this.txtSalesNO.Location = new System.Drawing.Point(322, 143);
            this.txtSalesNO.MaxLength = 11;
            this.txtSalesNO.MoveNext = true;
            this.txtSalesNO.Name = "txtSalesNO";
            this.txtSalesNO.Size = new System.Drawing.Size(202, 42);
            this.txtSalesNO.TabIndex = 1;
            this.txtSalesNO.Text = "99999999999";
            this.txtSalesNO.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            this.txtSalesNO.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSalesNO_KeyDown);
            // 
            // lblPrintDate
            // 
            this.lblPrintDate.AutoSize = true;
            this.lblPrintDate.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblPrintDate.BackColor = System.Drawing.Color.Transparent;
            this.lblPrintDate.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F, System.Drawing.FontStyle.Bold);
            this.lblPrintDate.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.lblPrintDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblPrintDate.Location = new System.Drawing.Point(42, 277);
            this.lblPrintDate.Name = "lblPrintDate";
            this.lblPrintDate.Size = new System.Drawing.Size(274, 35);
            this.lblPrintDate.TabIndex = 18;
            this.lblPrintDate.Text = "領収書印字日付";
            this.lblPrintDate.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblPrintDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSalseNo
            // 
            this.lblSalseNo.AutoSize = true;
            this.lblSalseNo.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblSalseNo.BackColor = System.Drawing.SystemColors.Window;
            this.lblSalseNo.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F, System.Drawing.FontStyle.Bold);
            this.lblSalseNo.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.lblSalseNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblSalseNo.Location = new System.Drawing.Point(116, 145);
            this.lblSalseNo.Name = "lblSalseNo";
            this.lblSalseNo.Size = new System.Drawing.Size(200, 35);
            this.lblSalseNo.TabIndex = 17;
            this.lblSalseNo.Text = "お買上番号";
            this.lblSalseNo.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblSalseNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TempoRegiRyousyuusyo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 849);
            this.Controls.Add(this.panelDetail);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "TempoRegiRyousyuusyo";
            this.Text = "店舗領収書印刷";
            this.Load += new System.EventHandler(this.TempoRegiRyousyuusyo_Load);
            this.Controls.SetChildIndex(this.panelDetail, 0);
            this.panelDetail.ResumeLayout(false);
            this.panelDetail.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelDetail;
        private CKM_Controls.CKM_TextBox txtSalesNO;
        private CKM_Controls.CKMShop_Label lblPrintDate;
        private CKM_Controls.CKMShop_Label lblSalseNo;
        private CKM_Controls.CKMShop_CheckBox chkRyousyuusho;
        private CKM_Controls.CKMShop_Label lblRyousyuusho;
        private CKM_Controls.CKMShop_CheckBox chkReissue;
        private CKM_Controls.CKMShop_Label lblReissue;
        private CKM_Controls.CKM_TextBox txtPrintDate;
        private CKM_Controls.CKMShop_CheckBox chkReceipt;
        private CKM_Controls.CKMShop_Label lblReceipt;
    }
}

