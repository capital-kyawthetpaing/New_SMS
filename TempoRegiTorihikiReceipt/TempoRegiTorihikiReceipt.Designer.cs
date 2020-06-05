namespace TempoRegiTorihikiReceipt
{
    partial class TempoRegiTorihikiReceipt
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblSalseNo = new CKM_Controls.CKMShop_Label();
            this.txtPrintDateFrom = new CKM_Controls.CKM_TextBox();
            this.ckmShop_Label1 = new CKM_Controls.CKMShop_Label();
            this.txtPrintDateTo = new CKM_Controls.CKM_TextBox();
            this.ckmShop_Label3 = new CKM_Controls.CKMShop_Label();
            this.PrintCheckBox = new CKM_Controls.CKMShop_CheckBox();
            this.ckmShop_Label4 = new CKM_Controls.CKMShop_Label();
            this.SuspendLayout();
            // 
            // lblSalseNo
            // 
            this.lblSalseNo.AutoSize = true;
            this.lblSalseNo.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblSalseNo.BackColor = System.Drawing.SystemColors.Window;
            this.lblSalseNo.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F, System.Drawing.FontStyle.Bold);
            this.lblSalseNo.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.lblSalseNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblSalseNo.Location = new System.Drawing.Point(138, 148);
            this.lblSalseNo.Name = "lblSalseNo";
            this.lblSalseNo.Size = new System.Drawing.Size(126, 35);
            this.lblSalseNo.TabIndex = 19;
            this.lblSalseNo.Text = "日　付";
            this.lblSalseNo.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblSalseNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPrintDateFrom
            // 
            this.txtPrintDateFrom.AllowMinus = true;
            this.txtPrintDateFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.Green;
            this.txtPrintDateFrom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtPrintDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPrintDateFrom.ClientColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtPrintDateFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtPrintDateFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtPrintDateFrom.DecimalPlace = 0;
            this.txtPrintDateFrom.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F);
            this.txtPrintDateFrom.IntegerPart = 8;
            this.txtPrintDateFrom.IsCorrectDate = true;
            this.txtPrintDateFrom.isEnterKeyDown = false;
            this.txtPrintDateFrom.isMaxLengthErr = false;
            this.txtPrintDateFrom.IsNumber = true;
            this.txtPrintDateFrom.IsShop = false;
            this.txtPrintDateFrom.Length = 10;
            this.txtPrintDateFrom.Location = new System.Drawing.Point(270, 144);
            this.txtPrintDateFrom.MaxLength = 10;
            this.txtPrintDateFrom.MoveNext = true;
            this.txtPrintDateFrom.Name = "txtPrintDateFrom";
            this.txtPrintDateFrom.Size = new System.Drawing.Size(184, 42);
            this.txtPrintDateFrom.TabIndex = 20;
            this.txtPrintDateFrom.Text = "9999/99/99";
            this.txtPrintDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPrintDateFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            // 
            // ckmShop_Label1
            // 
            this.ckmShop_Label1.AutoSize = true;
            this.ckmShop_Label1.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label1.BackColor = System.Drawing.SystemColors.Window;
            this.ckmShop_Label1.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label1.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label1.Location = new System.Drawing.Point(460, 148);
            this.ckmShop_Label1.Name = "ckmShop_Label1";
            this.ckmShop_Label1.Size = new System.Drawing.Size(52, 35);
            this.ckmShop_Label1.TabIndex = 21;
            this.ckmShop_Label1.Text = "～";
            this.ckmShop_Label1.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPrintDateTo
            // 
            this.txtPrintDateTo.AllowMinus = true;
            this.txtPrintDateTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.Green;
            this.txtPrintDateTo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtPrintDateTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPrintDateTo.ClientColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtPrintDateTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtPrintDateTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtPrintDateTo.DecimalPlace = 0;
            this.txtPrintDateTo.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F);
            this.txtPrintDateTo.IntegerPart = 8;
            this.txtPrintDateTo.IsCorrectDate = true;
            this.txtPrintDateTo.isEnterKeyDown = false;
            this.txtPrintDateTo.isMaxLengthErr = false;
            this.txtPrintDateTo.IsNumber = true;
            this.txtPrintDateTo.IsShop = false;
            this.txtPrintDateTo.Length = 10;
            this.txtPrintDateTo.Location = new System.Drawing.Point(518, 144);
            this.txtPrintDateTo.MaxLength = 10;
            this.txtPrintDateTo.MoveNext = true;
            this.txtPrintDateTo.Name = "txtPrintDateTo";
            this.txtPrintDateTo.Size = new System.Drawing.Size(184, 42);
            this.txtPrintDateTo.TabIndex = 22;
            this.txtPrintDateTo.Text = "9999/99/99";
            this.txtPrintDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPrintDateTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            // 
            // ckmShop_Label3
            // 
            this.ckmShop_Label3.AutoSize = true;
            this.ckmShop_Label3.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label3.BackColor = System.Drawing.SystemColors.Window;
            this.ckmShop_Label3.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label3.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label3.Location = new System.Drawing.Point(101, 192);
            this.ckmShop_Label3.Name = "ckmShop_Label3";
            this.ckmShop_Label3.Size = new System.Drawing.Size(163, 35);
            this.ckmShop_Label3.TabIndex = 24;
            this.ckmShop_Label3.Text = "販売明細";
            this.ckmShop_Label3.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PrintCheckBox
            // 
            this.PrintCheckBox.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F, System.Drawing.FontStyle.Bold);
            this.PrintCheckBox.ForeColor = System.Drawing.Color.Black;
            this.PrintCheckBox.IsattachedCaption = false;
            this.PrintCheckBox.Location = new System.Drawing.Point(270, 192);
            this.PrintCheckBox.Name = "PrintCheckBox";
            this.PrintCheckBox.Size = new System.Drawing.Size(35, 35);
            this.PrintCheckBox.TabIndex = 25;
            this.PrintCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PrintCheckBox.UseVisualStyleBackColor = true;
            // 
            // ckmShop_Label4
            // 
            this.ckmShop_Label4.AutoSize = true;
            this.ckmShop_Label4.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label4.BackColor = System.Drawing.SystemColors.Window;
            this.ckmShop_Label4.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label4.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label4.Location = new System.Drawing.Point(311, 192);
            this.ckmShop_Label4.Name = "ckmShop_Label4";
            this.ckmShop_Label4.Size = new System.Drawing.Size(163, 35);
            this.ckmShop_Label4.TabIndex = 26;
            this.ckmShop_Label4.Text = "印刷する";
            this.ckmShop_Label4.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TempoRegiTorihikiReceipt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 849);
            this.Controls.Add(this.ckmShop_Label4);
            this.Controls.Add(this.PrintCheckBox);
            this.Controls.Add(this.ckmShop_Label3);
            this.Controls.Add(this.txtPrintDateTo);
            this.Controls.Add(this.ckmShop_Label1);
            this.Controls.Add(this.txtPrintDateFrom);
            this.Controls.Add(this.lblSalseNo);
            this.Name = "TempoRegiTorihikiReceipt";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.TempoRegiTorihikiReceipt_Load);
            this.Controls.SetChildIndex(this.lblSalseNo, 0);
            this.Controls.SetChildIndex(this.txtPrintDateFrom, 0);
            this.Controls.SetChildIndex(this.ckmShop_Label1, 0);
            this.Controls.SetChildIndex(this.txtPrintDateTo, 0);
            this.Controls.SetChildIndex(this.ckmShop_Label3, 0);
            this.Controls.SetChildIndex(this.PrintCheckBox, 0);
            this.Controls.SetChildIndex(this.ckmShop_Label4, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CKM_Controls.CKMShop_Label lblSalseNo;
        private CKM_Controls.CKM_TextBox txtPrintDateFrom;
        private CKM_Controls.CKMShop_Label ckmShop_Label1;
        private CKM_Controls.CKM_TextBox txtPrintDateTo;
        private CKM_Controls.CKMShop_Label ckmShop_Label3;
        private CKM_Controls.CKMShop_CheckBox PrintCheckBox;
        private CKM_Controls.CKMShop_Label ckmShop_Label4;
    }
}

