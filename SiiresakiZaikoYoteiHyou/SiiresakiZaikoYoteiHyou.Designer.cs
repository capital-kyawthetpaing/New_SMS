﻿namespace SiiresakiZaikoYoteiHyou
{
    partial class SiiresakiZaikoYoteiHyou
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
            this.panalDetail = new System.Windows.Forms.Panel();
            this.cboStore = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.txtTargetDateTo = new CKM_Controls.CKM_TextBox();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.txtTargetDateFrom = new CKM_Controls.CKM_TextBox();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.panalDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1711, 0);
            // 
            // panalDetail
            // 
            this.panalDetail.Controls.Add(this.cboStore);
            this.panalDetail.Controls.Add(this.ckM_Label3);
            this.panalDetail.Controls.Add(this.txtTargetDateTo);
            this.panalDetail.Controls.Add(this.ckM_Label2);
            this.panalDetail.Controls.Add(this.txtTargetDateFrom);
            this.panalDetail.Controls.Add(this.ckM_Label1);
            this.panalDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panalDetail.Location = new System.Drawing.Point(0, 50);
            this.panalDetail.Name = "panalDetail";
            this.panalDetail.Size = new System.Drawing.Size(1713, 879);
            this.panalDetail.TabIndex = 13;
            // 
            // cboStore
            // 
            this.cboStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStore.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.店舗;
            this.cboStore.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboStore.FormattingEnabled = true;
            this.cboStore.Length = 10;
            this.cboStore.Location = new System.Drawing.Point(112, 54);
            this.cboStore.MaxLength = 10;
            this.cboStore.MoveNext = true;
            this.cboStore.Name = "cboStore";
            this.cboStore.Size = new System.Drawing.Size(121, 20);
            this.cboStore.TabIndex = 5;
            // 
            // ckM_Label3
            // 
            this.ckM_Label3.AutoSize = true;
            this.ckM_Label3.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label3.DefaultlabelSize = true;
            this.ckM_Label3.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label3.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label3.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label3.Location = new System.Drawing.Point(58, 61);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label3.TabIndex = 4;
            this.ckM_Label3.Text = "店舗";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTargetDateTo
            // 
            this.txtTargetDateTo.AllowMinus = false;
            this.txtTargetDateTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtTargetDateTo.BackColor = System.Drawing.Color.White;
            this.txtTargetDateTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTargetDateTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtTargetDateTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtTargetDateTo.DecimalPlace = 0;
            this.txtTargetDateTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtTargetDateTo.IntegerPart = 0;
            this.txtTargetDateTo.IsCorrectDate = true;
            this.txtTargetDateTo.isEnterKeyDown = false;
            this.txtTargetDateTo.IsNumber = true;
            this.txtTargetDateTo.IsShop = false;
            this.txtTargetDateTo.Length = 10;
            this.txtTargetDateTo.Location = new System.Drawing.Point(298, 19);
            this.txtTargetDateTo.MaxLength = 10;
            this.txtTargetDateTo.MoveNext = true;
            this.txtTargetDateTo.Name = "txtTargetDateTo";
            this.txtTargetDateTo.Size = new System.Drawing.Size(100, 19);
            this.txtTargetDateTo.TabIndex = 3;
            this.txtTargetDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTargetDateTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_Label2
            // 
            this.ckM_Label2.AutoSize = true;
            this.ckM_Label2.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label2.DefaultlabelSize = true;
            this.ckM_Label2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label2.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label2.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label2.Location = new System.Drawing.Point(248, 23);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label2.TabIndex = 2;
            this.ckM_Label2.Text = "～";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTargetDateFrom
            // 
            this.txtTargetDateFrom.AllowMinus = false;
            this.txtTargetDateFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtTargetDateFrom.BackColor = System.Drawing.Color.White;
            this.txtTargetDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTargetDateFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtTargetDateFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtTargetDateFrom.DecimalPlace = 0;
            this.txtTargetDateFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtTargetDateFrom.IntegerPart = 0;
            this.txtTargetDateFrom.IsCorrectDate = true;
            this.txtTargetDateFrom.isEnterKeyDown = false;
            this.txtTargetDateFrom.IsNumber = true;
            this.txtTargetDateFrom.IsShop = false;
            this.txtTargetDateFrom.Length = 10;
            this.txtTargetDateFrom.Location = new System.Drawing.Point(113, 20);
            this.txtTargetDateFrom.MaxLength = 10;
            this.txtTargetDateFrom.MoveNext = true;
            this.txtTargetDateFrom.Name = "txtTargetDateFrom";
            this.txtTargetDateFrom.ReadOnly = true;
            this.txtTargetDateFrom.Size = new System.Drawing.Size(100, 19);
            this.txtTargetDateFrom.TabIndex = 1;
            this.txtTargetDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTargetDateFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_Label1
            // 
            this.ckM_Label1.AutoSize = true;
            this.ckM_Label1.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label1.DefaultlabelSize = true;
            this.ckM_Label1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label1.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label1.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label1.Location = new System.Drawing.Point(38, 25);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label1.TabIndex = 0;
            this.ckM_Label1.Text = "対象年月";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SiiresakiZaikoYoteiHyou
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.panalDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "SiiresakiZaikoYoteiHyou";
            this.PanelHeaderHeight = 50;
            this.Text = "SiiresakiZaikoYoteiHyou";
            this.Load += new System.EventHandler(this.SiiresakiZaikoYoteiHyou_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SiiresakiZaikoYoteiHyou_KeyUp);
            this.Controls.SetChildIndex(this.panalDetail, 0);
            this.panalDetail.ResumeLayout(false);
            this.panalDetail.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panalDetail;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_ComboBox cboStore;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_TextBox txtTargetDateTo;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_TextBox txtTargetDateFrom;
    }
}

