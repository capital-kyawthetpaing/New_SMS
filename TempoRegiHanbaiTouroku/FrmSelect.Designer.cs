namespace TempoRegiHanbaiTouroku
{
    partial class FrmSelect
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
            this.ckM_Button1 = new CKM_Controls.CKM_Button();
            this.ckM_Button2 = new CKM_Controls.CKM_Button();
            this.btnClose = new CKM_Controls.CKM_Button();
            this.txtSalesDate = new CKM_Controls.CKM_TextBox();
            this.ckmShop_Label13 = new CKM_Controls.CKMShop_Label();
            this.SuspendLayout();
            // 
            // ckM_Button1
            // 
            this.ckM_Button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(242)))), ((int)(((byte)(204)))));
            this.ckM_Button1.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Yellow;
            this.ckM_Button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckM_Button1.DefaultBtnSize = false;
            this.ckM_Button1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.ckM_Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckM_Button1.Font = new System.Drawing.Font("ＭＳ ゴシック", 30F, System.Drawing.FontStyle.Bold);
            this.ckM_Button1.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.XLarge;
            this.ckM_Button1.Location = new System.Drawing.Point(185, 113);
            this.ckM_Button1.Margin = new System.Windows.Forms.Padding(1);
            this.ckM_Button1.Name = "ckM_Button1";
            this.ckM_Button1.Size = new System.Drawing.Size(347, 66);
            this.ckM_Button1.TabIndex = 1;
            this.ckM_Button1.Text = "訂　正";
            this.ckM_Button1.UseVisualStyleBackColor = false;
            this.ckM_Button1.Click += new System.EventHandler(this.ckM_Button1_Click);
            // 
            // ckM_Button2
            // 
            this.ckM_Button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(242)))), ((int)(((byte)(204)))));
            this.ckM_Button2.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Yellow;
            this.ckM_Button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckM_Button2.DefaultBtnSize = false;
            this.ckM_Button2.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.ckM_Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckM_Button2.Font = new System.Drawing.Font("ＭＳ ゴシック", 30F, System.Drawing.FontStyle.Bold);
            this.ckM_Button2.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.XLarge;
            this.ckM_Button2.Location = new System.Drawing.Point(532, 113);
            this.ckM_Button2.Margin = new System.Windows.Forms.Padding(1);
            this.ckM_Button2.Name = "ckM_Button2";
            this.ckM_Button2.Size = new System.Drawing.Size(347, 66);
            this.ckM_Button2.TabIndex = 2;
            this.ckM_Button2.Text = "取　消";
            this.ckM_Button2.UseVisualStyleBackColor = false;
            this.ckM_Button2.Click += new System.EventHandler(this.ckM_Button2_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(242)))), ((int)(((byte)(204)))));
            this.btnClose.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Yellow;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.DefaultBtnSize = false;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("ＭＳ ゴシック", 30F, System.Drawing.FontStyle.Bold);
            this.btnClose.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.XLarge;
            this.btnClose.Location = new System.Drawing.Point(2, 213);
            this.btnClose.Margin = new System.Windows.Forms.Padding(1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(234, 66);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "戻　る";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Visible = false;
            // 
            // txtSalesDate
            // 
            this.txtSalesDate.AllowMinus = false;
            this.txtSalesDate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.Green;
            this.txtSalesDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtSalesDate.BorderColor = false;
            this.txtSalesDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSalesDate.ClientColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtSalesDate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtSalesDate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtSalesDate.DecimalPlace = 0;
            this.txtSalesDate.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F);
            this.txtSalesDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSalesDate.IntegerPart = 0;
            this.txtSalesDate.IsCorrectDate = true;
            this.txtSalesDate.isEnterKeyDown = false;
            this.txtSalesDate.IsFirstTime = true;
            this.txtSalesDate.isMaxLengthErr = false;
            this.txtSalesDate.IsNumber = false;
            this.txtSalesDate.IsShop = false;
            this.txtSalesDate.Length = 10;
            this.txtSalesDate.Location = new System.Drawing.Point(185, 44);
            this.txtSalesDate.MaxLength = 10;
            this.txtSalesDate.MoveNext = true;
            this.txtSalesDate.Name = "txtSalesDate";
            this.txtSalesDate.Size = new System.Drawing.Size(200, 42);
            this.txtSalesDate.TabIndex = 0;
            this.txtSalesDate.Text = "9999/99/99";
            this.txtSalesDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSalesDate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            this.txtSalesDate.UseColorSizMode = false;
            this.txtSalesDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSalesDate_KeyDown);
            // 
            // ckmShop_Label13
            // 
            this.ckmShop_Label13.AutoSize = true;
            this.ckmShop_Label13.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label13.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label13.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label13.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label13.FontBold = true;
            this.ckmShop_Label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label13.Location = new System.Drawing.Point(16, 46);
            this.ckmShop_Label13.Name = "ckmShop_Label13";
            this.ckmShop_Label13.Size = new System.Drawing.Size(163, 35);
            this.ckmShop_Label13.TabIndex = 41;
            this.ckmShop_Label13.Text = "計上日付";
            this.ckmShop_Label13.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(942, 279);
            this.ControlBox = false;
            this.Controls.Add(this.txtSalesDate);
            this.Controls.Add(this.ckmShop_Label13);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.ckM_Button2);
            this.Controls.Add(this.ckM_Button1);
            this.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "FrmSelect";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "販売登録";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CKM_Controls.CKM_Button ckM_Button1;
        private CKM_Controls.CKM_Button ckM_Button2;
        private CKM_Controls.CKM_Button btnClose;
        private CKM_Controls.CKM_TextBox txtSalesDate;
        private CKM_Controls.CKMShop_Label ckmShop_Label13;
    }
}