namespace TempoRegiShiharaiNyuuryoku
{
    partial class TempoRegiShiharaiNyuuryoku
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
            this.ckmShop_Label1 = new CKM_Controls.CKMShop_Label();
            this.ckmShop_Label4 = new CKM_Controls.CKMShop_Label();
            this.ckmShop_Label5 = new CKM_Controls.CKMShop_Label();
            this.txtPayment = new CKM_Controls.CKM_TextBox();
            this.txtRemarks = new CKM_Controls.CKM_MultiLineTextBox();
            this.cboDenominationName = new CKM_Controls.CKMShop_ComboBox();
            this.SuspendLayout();
            // 
            // ckmShop_Label1
            // 
            this.ckmShop_Label1.AutoSize = true;
            this.ckmShop_Label1.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label1.BackColor = System.Drawing.SystemColors.Window;
            this.ckmShop_Label1.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label1.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label1.Location = new System.Drawing.Point(132, 239);
            this.ckmShop_Label1.Name = "ckmShop_Label1";
            this.ckmShop_Label1.Size = new System.Drawing.Size(126, 35);
            this.ckmShop_Label1.TabIndex = 4;
            this.ckmShop_Label1.Text = "支払額";
            this.ckmShop_Label1.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckmShop_Label4
            // 
            this.ckmShop_Label4.AutoSize = true;
            this.ckmShop_Label4.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label4.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label4.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label4.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label4.Location = new System.Drawing.Point(134, 294);
            this.ckmShop_Label4.Name = "ckmShop_Label4";
            this.ckmShop_Label4.Size = new System.Drawing.Size(125, 35);
            this.ckmShop_Label4.TabIndex = 5;
            this.ckmShop_Label4.Text = "金　種";
            this.ckmShop_Label4.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckmShop_Label5
            // 
            this.ckmShop_Label5.AutoSize = true;
            this.ckmShop_Label5.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label5.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label5.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label5.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label5.Location = new System.Drawing.Point(134, 355);
            this.ckmShop_Label5.Name = "ckmShop_Label5";
            this.ckmShop_Label5.Size = new System.Drawing.Size(125, 35);
            this.ckmShop_Label5.TabIndex = 6;
            this.ckmShop_Label5.Text = "備　考";
            this.ckmShop_Label5.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPayment
            // 
            this.txtPayment.AllowMinus = true;
            this.txtPayment.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.Green;
            this.txtPayment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtPayment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPayment.ClientColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtPayment.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtPayment.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Price;
            this.txtPayment.DecimalPlace = 0;
            this.txtPayment.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.txtPayment.IntegerPart = 8;
            this.txtPayment.IsCorrectDate = true;
            this.txtPayment.isEnterKeyDown = false;
            this.txtPayment.isMaxLengthErr = false;
            this.txtPayment.IsNumber = true;
            this.txtPayment.IsShop = false;
            this.txtPayment.Length = 10;
            this.txtPayment.Location = new System.Drawing.Point(250, 236);
            this.txtPayment.MaxLength = 10;
            this.txtPayment.MoveNext = true;
            this.txtPayment.Name = "txtPayment";
            this.txtPayment.Size = new System.Drawing.Size(200, 42);
            this.txtPayment.TabIndex = 4;
            this.txtPayment.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPayment.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            // 
            // txtRemarks
            // 
            this.txtRemarks.Back_Color = CKM_Controls.CKM_MultiLineTextBox.CKM_Color.Green;
            this.txtRemarks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemarks.Ctrl_Byte = CKM_Controls.CKM_MultiLineTextBox.Bytes.半全角;
            this.txtRemarks.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.txtRemarks.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtRemarks.Length = 200;
            this.txtRemarks.Location = new System.Drawing.Point(250, 352);
            this.txtRemarks.MaxLength = 200;
            this.txtRemarks.Mdea = false;
            this.txtRemarks.Mfocus = false;
            this.txtRemarks.MoveNext = true;
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.RowCount = 5;
            this.txtRemarks.Size = new System.Drawing.Size(750, 200);
            this.txtRemarks.TabIndex = 6;
            this.txtRemarks.TextSize = CKM_Controls.CKM_MultiLineTextBox.FontSize.Medium;
            // 
            // cboDenominationName
            // 
            this.cboDenominationName.Alignment = CKM_Controls.CKMShop_ComboBox.Align.right;
            this.cboDenominationName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboDenominationName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDenominationName.Cbo_Type = CKM_Controls.CKMShop_ComboBox.CboType.金種名;
            this.cboDenominationName.cboalign = CKM_Controls.CKMShop_ComboBox.Align.right;
            this.cboDenominationName.ComboAlign = CKM_Controls.CKMShop_ComboBox.Align.right;
            this.cboDenominationName.Ctrl_Byte = CKM_Controls.CKMShop_ComboBox.Bytes.半全角;
            this.cboDenominationName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboDenominationName.Font = new System.Drawing.Font("Meiryo UI", 26F);
            this.cboDenominationName.FontSize_ = 26F;
            this.cboDenominationName.FormattingEnabled = true;
            this.cboDenominationName.IntegralHeight = false;
            this.cboDenominationName.ItemHeight = 40;
            this.cboDenominationName.ItemHeight_ = 40;
            this.cboDenominationName.Length = 20;
            this.cboDenominationName.Location = new System.Drawing.Point(250, 291);
            this.cboDenominationName.MaxItem = 8;
            this.cboDenominationName.MaxLength = 10;
            this.cboDenominationName.MoveNext = true;
            this.cboDenominationName.Name = "cboDenominationName";
            this.cboDenominationName.Size = new System.Drawing.Size(220, 46);
            this.cboDenominationName.TabIndex = 5;
            // 
            // TempoRegiShiharaiNyuuryoku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.cboDenominationName);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.txtPayment);
            this.Controls.Add(this.ckmShop_Label5);
            this.Controls.Add(this.ckmShop_Label4);
            this.Controls.Add(this.ckmShop_Label1);
            this.Name = "TempoRegiShiharaiNyuuryoku";
            this.Text = "支払入力";
            this.Load += new System.EventHandler(this.TempoRejiShiharaiNyuuryoku_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TempoRegiShiharaiNyuuryoku_KeyUp);
            this.Controls.SetChildIndex(this.ckmShop_Label1, 0);
            this.Controls.SetChildIndex(this.ckmShop_Label4, 0);
            this.Controls.SetChildIndex(this.ckmShop_Label5, 0);
            this.Controls.SetChildIndex(this.txtPayment, 0);
            this.Controls.SetChildIndex(this.txtRemarks, 0);
            this.Controls.SetChildIndex(this.cboDenominationName, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CKM_Controls.CKMShop_Label ckmShop_Label1;
        private CKM_Controls.CKMShop_Label ckmShop_Label4;
        private CKM_Controls.CKMShop_Label ckmShop_Label5;
        private CKM_Controls.CKM_TextBox txtPayment;
        private CKM_Controls.CKM_MultiLineTextBox txtRemarks;
        private CKM_Controls.CKMShop_ComboBox cboDenominationName;
    }
}