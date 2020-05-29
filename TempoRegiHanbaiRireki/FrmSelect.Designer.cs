namespace TempoRegiHanbaiRireki
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
            this.ckM_Button1.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.ckM_Button1.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Medium;
            this.ckM_Button1.Location = new System.Drawing.Point(88, 27);
            this.ckM_Button1.Margin = new System.Windows.Forms.Padding(1);
            this.ckM_Button1.Name = "ckM_Button1";
            this.ckM_Button1.Size = new System.Drawing.Size(243, 45);
            this.ckM_Button1.TabIndex = 0;
            this.ckM_Button1.Text = "未売上明細を出荷・売上する";
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
            this.ckM_Button2.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.ckM_Button2.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Medium;
            this.ckM_Button2.Location = new System.Drawing.Point(381, 27);
            this.ckM_Button2.Margin = new System.Windows.Forms.Padding(1);
            this.ckM_Button2.Name = "ckM_Button2";
            this.ckM_Button2.Size = new System.Drawing.Size(243, 45);
            this.ckM_Button2.TabIndex = 1;
            this.ckM_Button2.Text = "既に出荷・売上した明細を見る";
            this.ckM_Button2.UseVisualStyleBackColor = false;
            this.ckM_Button2.Click += new System.EventHandler(this.ckM_Button2_Click);
            // 
            // FrmSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(784, 141);
            this.Controls.Add(this.ckM_Button2);
            this.Controls.Add(this.ckM_Button1);
            this.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "FrmSelect";
            this.Text = "販売履歴";
            this.ResumeLayout(false);

        }

        #endregion

        private CKM_Controls.CKM_Button ckM_Button1;
        private CKM_Controls.CKM_Button ckM_Button2;
    }
}