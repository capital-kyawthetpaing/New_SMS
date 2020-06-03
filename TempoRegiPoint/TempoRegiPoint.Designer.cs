namespace TempoRegiPoint
{
    partial class TempoRegiPoint
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
            this.BtnSearchCustomer = new CKM_Controls.CKM_Button();
            this.lblLastPoint = new CKM_Controls.CKMShop_Label();
            this.lblTicketUnit = new CKM_Controls.CKMShop_Label();
            this.TxtCustomerCD = new CKM_Controls.CKM_TextBox();
            this.TxtLastPoint = new CKM_Controls.CKM_TextBox();
            this.TxtIssuePoint = new CKM_Controls.CKM_TextBox();
            this.LblCustomerName = new CKM_Controls.CKMShop_Label();
            this.SuspendLayout();
            // 
            // BtnSearchCustomer
            // 
            this.BtnSearchCustomer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BtnSearchCustomer.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BtnSearchCustomer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSearchCustomer.DefaultBtnSize = false;
            this.BtnSearchCustomer.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnSearchCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSearchCustomer.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.BtnSearchCustomer.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Medium;
            this.BtnSearchCustomer.Location = new System.Drawing.Point(9, 95);
            this.BtnSearchCustomer.Margin = new System.Windows.Forms.Padding(1);
            this.BtnSearchCustomer.Name = "BtnSearchCustomer";
            this.BtnSearchCustomer.Size = new System.Drawing.Size(255, 45);
            this.BtnSearchCustomer.TabIndex = 4;
            this.BtnSearchCustomer.Text = "会員番号";
            this.BtnSearchCustomer.UseVisualStyleBackColor = false;
            this.BtnSearchCustomer.Click += new System.EventHandler(this.BtnSearchCustomer_Click);
            // 
            // lblLastPoint
            // 
            this.lblLastPoint.AutoSize = true;
            this.lblLastPoint.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblLastPoint.BackColor = System.Drawing.SystemColors.Window;
            this.lblLastPoint.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.lblLastPoint.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.lblLastPoint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblLastPoint.Location = new System.Drawing.Point(23, 175);
            this.lblLastPoint.Name = "lblLastPoint";
            this.lblLastPoint.Size = new System.Drawing.Size(237, 35);
            this.lblLastPoint.TabIndex = 18;
            this.lblLastPoint.Text = "保持ポイント";
            this.lblLastPoint.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblLastPoint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTicketUnit
            // 
            this.lblTicketUnit.AutoSize = true;
            this.lblTicketUnit.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblTicketUnit.BackColor = System.Drawing.SystemColors.Window;
            this.lblTicketUnit.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.lblTicketUnit.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.lblTicketUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblTicketUnit.Location = new System.Drawing.Point(23, 217);
            this.lblTicketUnit.Name = "lblTicketUnit";
            this.lblTicketUnit.Size = new System.Drawing.Size(237, 35);
            this.lblTicketUnit.TabIndex = 19;
            this.lblTicketUnit.Text = "発行ポイント";
            this.lblTicketUnit.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblTicketUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TxtCustomerCD
            // 
            this.TxtCustomerCD.AllowMinus = true;
            this.TxtCustomerCD.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.Green;
            this.TxtCustomerCD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.TxtCustomerCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtCustomerCD.ClientColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.TxtCustomerCD.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.TxtCustomerCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.TxtCustomerCD.DecimalPlace = 0;
            this.TxtCustomerCD.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.TxtCustomerCD.IntegerPart = 8;
            this.TxtCustomerCD.IsCorrectDate = true;
            this.TxtCustomerCD.isEnterKeyDown = false;
            this.TxtCustomerCD.isMaxLengthErr = false;
            this.TxtCustomerCD.IsNumber = true;
            this.TxtCustomerCD.IsShop = false;
            this.TxtCustomerCD.Length = 13;
            this.TxtCustomerCD.Location = new System.Drawing.Point(263, 95);
            this.TxtCustomerCD.MaxLength = 13;
            this.TxtCustomerCD.MoveNext = true;
            this.TxtCustomerCD.Name = "TxtCustomerCD";
            this.TxtCustomerCD.Size = new System.Drawing.Size(255, 42);
            this.TxtCustomerCD.TabIndex = 20;
            this.TxtCustomerCD.Text = "XXXXXXXX10";
            this.TxtCustomerCD.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            this.TxtCustomerCD.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtCustomerCD_KeyDown);
            // 
            // TxtLastPoint
            // 
            this.TxtLastPoint.AllowMinus = true;
            this.TxtLastPoint.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.Green;
            this.TxtLastPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.TxtLastPoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtLastPoint.ClientColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.TxtLastPoint.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.TxtLastPoint.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Number;
            this.TxtLastPoint.DecimalPlace = 0;
            this.TxtLastPoint.Enabled = false;
            this.TxtLastPoint.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.TxtLastPoint.IntegerPart = 8;
            this.TxtLastPoint.IsCorrectDate = true;
            this.TxtLastPoint.isEnterKeyDown = false;
            this.TxtLastPoint.isMaxLengthErr = false;
            this.TxtLastPoint.IsNumber = true;
            this.TxtLastPoint.IsShop = false;
            this.TxtLastPoint.Length = 20;
            this.TxtLastPoint.Location = new System.Drawing.Point(263, 172);
            this.TxtLastPoint.MaxLength = 20;
            this.TxtLastPoint.MoveNext = true;
            this.TxtLastPoint.Name = "TxtLastPoint";
            this.TxtLastPoint.Size = new System.Drawing.Size(150, 42);
            this.TxtLastPoint.TabIndex = 21;
            this.TxtLastPoint.Text = "99,999";
            this.TxtLastPoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtLastPoint.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            // 
            // TxtIssuePoint
            // 
            this.TxtIssuePoint.AllowMinus = true;
            this.TxtIssuePoint.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.Green;
            this.TxtIssuePoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.TxtIssuePoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtIssuePoint.ClientColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.TxtIssuePoint.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.TxtIssuePoint.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Number;
            this.TxtIssuePoint.DecimalPlace = 0;
            this.TxtIssuePoint.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.TxtIssuePoint.IntegerPart = 8;
            this.TxtIssuePoint.IsCorrectDate = true;
            this.TxtIssuePoint.isEnterKeyDown = false;
            this.TxtIssuePoint.isMaxLengthErr = false;
            this.TxtIssuePoint.IsNumber = true;
            this.TxtIssuePoint.IsShop = false;
            this.TxtIssuePoint.Length = 20;
            this.TxtIssuePoint.Location = new System.Drawing.Point(263, 214);
            this.TxtIssuePoint.MaxLength = 20;
            this.TxtIssuePoint.MoveNext = true;
            this.TxtIssuePoint.Name = "TxtIssuePoint";
            this.TxtIssuePoint.Size = new System.Drawing.Size(150, 42);
            this.TxtIssuePoint.TabIndex = 22;
            this.TxtIssuePoint.Text = "99,999";
            this.TxtIssuePoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtIssuePoint.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            // 
            // LblCustomerName
            // 
            this.LblCustomerName.AutoSize = true;
            this.LblCustomerName.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.LblCustomerName.Font = new System.Drawing.Font("MS Gothic", 16F, System.Drawing.FontStyle.Bold);
            this.LblCustomerName.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.LblCustomerName.Location = new System.Drawing.Point(522, 97);
            this.LblCustomerName.Name = "LblCustomerName";
            this.LblCustomerName.Size = new System.Drawing.Size(357, 22);
            this.LblCustomerName.TabIndex = 24;
            this.LblCustomerName.Text = "ＸＸＸＸＸＸＸＸＸ10ＸＸＸＸ15";
            this.LblCustomerName.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.LblCustomerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TempoRegiPoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 920);
            this.Controls.Add(this.LblCustomerName);
            this.Controls.Add(this.TxtIssuePoint);
            this.Controls.Add(this.TxtLastPoint);
            this.Controls.Add(this.TxtCustomerCD);
            this.Controls.Add(this.lblTicketUnit);
            this.Controls.Add(this.lblLastPoint);
            this.Controls.Add(this.BtnSearchCustomer);
            this.Name = "TempoRegiPoint";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.TempoRegiPoint_Load);
            this.Controls.SetChildIndex(this.BtnSearchCustomer, 0);
            this.Controls.SetChildIndex(this.lblLastPoint, 0);
            this.Controls.SetChildIndex(this.lblTicketUnit, 0);
            this.Controls.SetChildIndex(this.TxtCustomerCD, 0);
            this.Controls.SetChildIndex(this.TxtLastPoint, 0);
            this.Controls.SetChildIndex(this.TxtIssuePoint, 0);
            this.Controls.SetChildIndex(this.LblCustomerName, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CKM_Controls.CKM_Button BtnSearchCustomer;
        private CKM_Controls.CKMShop_Label lblLastPoint;
        private CKM_Controls.CKMShop_Label lblTicketUnit;
        private CKM_Controls.CKM_TextBox TxtCustomerCD;
        private CKM_Controls.CKM_TextBox TxtLastPoint;
        private CKM_Controls.CKM_TextBox TxtIssuePoint;
        private CKM_Controls.CKMShop_Label LblCustomerName;
    }
}

