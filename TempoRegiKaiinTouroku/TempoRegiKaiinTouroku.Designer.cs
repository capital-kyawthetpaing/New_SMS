namespace TempoRegiKaiinTouroku
{
    partial class TempoRegiKaiinTouroku
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
            this.txtCustomerNo = new CKM_Controls.CKM_TextBox();
            this.ckmShop_Label4 = new CKM_Controls.CKMShop_Label();
            this.btnCustomerSearch = new CKM_Controls.CKM_Button();
            this.SuspendLayout();
            // 
            // txtCustomerNo
            // 
            this.txtCustomerNo.AllowMinus = false;
            this.txtCustomerNo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.Green;
            this.txtCustomerNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtCustomerNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCustomerNo.ClientColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtCustomerNo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtCustomerNo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtCustomerNo.DecimalPlace = 0;
            this.txtCustomerNo.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.txtCustomerNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtCustomerNo.IntegerPart = 0;
            this.txtCustomerNo.IsCorrectDate = true;
            this.txtCustomerNo.isEnterKeyDown = false;
            this.txtCustomerNo.isMaxLengthErr = false;
            this.txtCustomerNo.IsNumber = true;
            this.txtCustomerNo.IsShop = false;
            this.txtCustomerNo.Length = 13;
            this.txtCustomerNo.Location = new System.Drawing.Point(301, 181);
            this.txtCustomerNo.MaxLength = 13;
            this.txtCustomerNo.MoveNext = true;
            this.txtCustomerNo.Name = "txtCustomerNo";
            this.txtCustomerNo.Size = new System.Drawing.Size(240, 42);
            this.txtCustomerNo.TabIndex = 1;
            this.txtCustomerNo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            this.txtCustomerNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Customer_KeyDown);
            // 
            // ckmShop_Label4
            // 
            this.ckmShop_Label4.AutoSize = true;
            this.ckmShop_Label4.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label4.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label4.Font = new System.Drawing.Font("MS Gothic", 28F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label4.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Medium;
            this.ckmShop_Label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label4.Location = new System.Drawing.Point(296, 226);
            this.ckmShop_Label4.Name = "ckmShop_Label4";
            this.ckmShop_Label4.Size = new System.Drawing.Size(1265, 38);
            this.ckmShop_Label4.TabIndex = 5;
            this.ckmShop_Label4.Text = "↑お客様にお渡しした会員カードに記載されている会員番号を入力する";
            this.ckmShop_Label4.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCustomerSearch
            // 
            this.btnCustomerSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(242)))), ((int)(((byte)(204)))));
            this.btnCustomerSearch.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Yellow;
            this.btnCustomerSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCustomerSearch.DefaultBtnSize = false;
            this.btnCustomerSearch.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCustomerSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomerSearch.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.btnCustomerSearch.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Medium;
            this.btnCustomerSearch.Location = new System.Drawing.Point(181, 181);
            this.btnCustomerSearch.Margin = new System.Windows.Forms.Padding(1);
            this.btnCustomerSearch.Name = "btnCustomerSearch";
            this.btnCustomerSearch.Size = new System.Drawing.Size(120, 42);
            this.btnCustomerSearch.TabIndex = 6;
            this.btnCustomerSearch.Text = "会員番号";
            this.btnCustomerSearch.UseVisualStyleBackColor = false;
            this.btnCustomerSearch.Click += new System.EventHandler(this.btnCustomerSearch_Click);
            // 
            // TempoRegiKaiinTouroku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BtnP_text = "内容登録へ";
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.btnCustomerSearch);
            this.Controls.Add(this.ckmShop_Label4);
            this.Controls.Add(this.txtCustomerNo);
            this.Name = "TempoRegiKaiinTouroku";
            this.Text = "会員登録";
            this.Load += new System.EventHandler(this.TempoRegiKaiinTouroku_Load);
            this.Controls.SetChildIndex(this.txtCustomerNo, 0);
            this.Controls.SetChildIndex(this.ckmShop_Label4, 0);
            this.Controls.SetChildIndex(this.btnCustomerSearch, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CKM_Controls.CKM_TextBox txtCustomerNo;
        private CKM_Controls.CKMShop_Label ckmShop_Label4;
        private CKM_Controls.CKM_Button btnCustomerSearch;
    }
}

