﻿namespace TempoRegiFurikomiYoushi
{
    partial class TempoRegiFurikomiYoushi
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
            this.ckmShop_Label4.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label4.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label4.FontBold = true;
            this.ckmShop_Label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label4.Location = new System.Drawing.Point(46, 209);
            this.ckmShop_Label4.Name = "ckmShop_Label4";
            this.ckmShop_Label4.Size = new System.Drawing.Size(200, 35);
            this.ckmShop_Label4.TabIndex = 122;
            this.ckmShop_Label4.Text = "お買上番号";
            this.ckmShop_Label4.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.DarkGreen;
            this.ckmShop_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtprintprogress
            // 
            this.txtprintprogress.AllowMinus = false;
            this.txtprintprogress.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.Green;
            this.txtprintprogress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtprintprogress.BorderColor = false;
            this.txtprintprogress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtprintprogress.ClientColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtprintprogress.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtprintprogress.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtprintprogress.DecimalPlace = 0;
            this.txtprintprogress.Font = new System.Drawing.Font("ＭＳ ゴシック", 26F);
            this.txtprintprogress.IntegerPart = 0;
            this.txtprintprogress.IsCorrectDate = true;
            this.txtprintprogress.isEnterKeyDown = false;
            this.txtprintprogress.IsFirstTime = true;
            this.txtprintprogress.isMaxLengthErr = false;
            this.txtprintprogress.IsNumber = true;
            this.txtprintprogress.IsShop = false;
            this.txtprintprogress.IsTimemmss = false;
            this.txtprintprogress.Length = 11;
            this.txtprintprogress.Location = new System.Drawing.Point(250, 206);
            this.txtprintprogress.MaxLength = 11;
            this.txtprintprogress.MoveNext = true;
            this.txtprintprogress.Name = "txtprintprogress";
            this.txtprintprogress.Size = new System.Drawing.Size(210, 42);
            this.txtprintprogress.TabIndex = 121;
            this.txtprintprogress.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            this.txtprintprogress.UseColorSizMode = false;
            this.txtprintprogress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtprintprogress_KeyDown);
            // 
            // TempoRegiFurikomiYoushi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1913, 887);
            this.Controls.Add(this.ckmShop_Label4);
            this.Controls.Add(this.txtprintprogress);
            this.Name = "TempoRegiFurikomiYoushi";
            this.Text = "店舗レジ 振込用紙印刷";
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

