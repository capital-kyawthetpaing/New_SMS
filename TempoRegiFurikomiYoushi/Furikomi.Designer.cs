namespace TempoRegiFurikomiYoushi
{
    partial class Furikomi
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
            this.ckmShop_Label4 = new CKM_Controls.CKMShop_Label();
            this.txtprintprogress = new CKM_Controls.CKM_TextBox();
            this.SuspendLayout();
            // 
            // ckmShop_Label4
            // 
            this.ckmShop_Label4.AutoSize = true;
            this.ckmShop_Label4.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label4.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label4.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label4.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label4.Location = new System.Drawing.Point(33, 130);
            this.ckmShop_Label4.Name = "ckmShop_Label4";
            this.ckmShop_Label4.Size = new System.Drawing.Size(200, 35);
            this.ckmShop_Label4.TabIndex = 124;
            this.ckmShop_Label4.Text = "お買上番号";
            this.ckmShop_Label4.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.DarkGreen;
            this.ckmShop_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtprintprogress
            // 
            this.txtprintprogress.AllowMinus = false;
            this.txtprintprogress.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.Green;
            this.txtprintprogress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtprintprogress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtprintprogress.ClientColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtprintprogress.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtprintprogress.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Number;
            this.txtprintprogress.DecimalPlace = 0;
            this.txtprintprogress.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.txtprintprogress.IntegerPart = 0;
            this.txtprintprogress.IsCorrectDate = true;
            this.txtprintprogress.isEnterKeyDown = false;
            this.txtprintprogress.isMaxLengthErr = false;
            this.txtprintprogress.IsNumber = true;
            this.txtprintprogress.IsShop = false;
            this.txtprintprogress.Length = 11;
            this.txtprintprogress.Location = new System.Drawing.Point(236, 126);
            this.txtprintprogress.MaxLength = 11;
            this.txtprintprogress.MoveNext = true;
            this.txtprintprogress.Name = "txtprintprogress";
            this.txtprintprogress.Size = new System.Drawing.Size(140, 42);
            this.txtprintprogress.TabIndex = 123;
            this.txtprintprogress.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtprintprogress.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            // 
            // Furikomi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.ckmShop_Label4);
            this.Controls.Add(this.txtprintprogress);
            this.Name = "Furikomi";
            this.Text = "Furikomi";
            this.Load += new System.EventHandler(this.TempoRegiFurikomiYoushi_Load);
            this.Controls.SetChildIndex(this.txtprintprogress, 0);
            this.Controls.SetChildIndex(this.ckmShop_Label4, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CKM_Controls.CKMShop_Label ckmShop_Label4;
        private CKM_Controls.CKM_TextBox txtprintprogress;
    }
}