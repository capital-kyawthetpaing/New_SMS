namespace TempoRegiHanbaiTouroku
{
    partial class FrmSpecialWaribiki
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
            this.lblSKUName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ckM_Button1
            // 
            this.ckM_Button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ckM_Button1.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Pink;
            this.ckM_Button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckM_Button1.DefaultBtnSize = false;
            this.ckM_Button1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.ckM_Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckM_Button1.Font = new System.Drawing.Font("ＭＳ ゴシック", 24F, System.Drawing.FontStyle.Bold);
            this.ckM_Button1.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.XLarge;
            this.ckM_Button1.Location = new System.Drawing.Point(51, 99);
            this.ckM_Button1.Margin = new System.Windows.Forms.Padding(1);
            this.ckM_Button1.Name = "ckM_Button1";
            this.ckM_Button1.Size = new System.Drawing.Size(347, 66);
            this.ckM_Button1.TabIndex = 1;
            this.ckM_Button1.Text = "20％割引";
            this.ckM_Button1.UseVisualStyleBackColor = false;
            this.ckM_Button1.Click += new System.EventHandler(this.ckM_Button1_Click);
            // 
            // ckM_Button2
            // 
            this.ckM_Button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ckM_Button2.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Pink;
            this.ckM_Button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckM_Button2.DefaultBtnSize = false;
            this.ckM_Button2.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.ckM_Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckM_Button2.Font = new System.Drawing.Font("ＭＳ ゴシック", 24F, System.Drawing.FontStyle.Bold);
            this.ckM_Button2.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.XLarge;
            this.ckM_Button2.Location = new System.Drawing.Point(51, 179);
            this.ckM_Button2.Margin = new System.Windows.Forms.Padding(1);
            this.ckM_Button2.Name = "ckM_Button2";
            this.ckM_Button2.Size = new System.Drawing.Size(347, 66);
            this.ckM_Button2.TabIndex = 2;
            this.ckM_Button2.Text = "10％割引";
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
            this.btnClose.Font = new System.Drawing.Font("ＭＳ ゴシック", 24F, System.Drawing.FontStyle.Bold);
            this.btnClose.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.XLarge;
            this.btnClose.Location = new System.Drawing.Point(51, 259);
            this.btnClose.Margin = new System.Windows.Forms.Padding(1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(347, 94);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "OK(通常価格)";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblSKUName
            // 
            this.lblSKUName.BackColor = System.Drawing.Color.Transparent;
            this.lblSKUName.Font = new System.Drawing.Font("ＭＳ ゴシック", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSKUName.Location = new System.Drawing.Point(51, 37);
            this.lblSKUName.Name = "lblSKUName";
            this.lblSKUName.Size = new System.Drawing.Size(347, 48);
            this.lblSKUName.TabIndex = 71;
            this.lblSKUName.Text = "特別割引率選択";
            this.lblSKUName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmSpecialWaribiki
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(444, 374);
            this.ControlBox = false;
            this.Controls.Add(this.lblSKUName);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.ckM_Button2);
            this.Controls.Add(this.ckM_Button1);
            this.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSpecialWaribiki";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "販売登録";
            this.ResumeLayout(false);

        }

        #endregion

        private CKM_Controls.CKM_Button ckM_Button1;
        private CKM_Controls.CKM_Button ckM_Button2;
        private CKM_Controls.CKM_Button btnClose;
        private System.Windows.Forms.Label lblSKUName;
    }
}