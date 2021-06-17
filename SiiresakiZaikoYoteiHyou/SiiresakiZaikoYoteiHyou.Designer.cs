namespace SiiresakiZaikoYoteiHyou
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
            this.lblStore = new CKM_Controls.CKM_Label();
            this.txtTargetDateTo = new CKM_Controls.CKM_TextBox();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.txtTargetDateFrom = new CKM_Controls.CKM_TextBox();
            this.lblDate = new CKM_Controls.CKM_Label();
            this.panalDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1711, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(1177, 0);
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            // 
            // panalDetail
            // 
            this.panalDetail.Controls.Add(this.cboStore);
            this.panalDetail.Controls.Add(this.lblStore);
            this.panalDetail.Controls.Add(this.txtTargetDateTo);
            this.panalDetail.Controls.Add(this.ckM_Label2);
            this.panalDetail.Controls.Add(this.txtTargetDateFrom);
            this.panalDetail.Controls.Add(this.lblDate);
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
            this.cboStore.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.店舗ストア;
            this.cboStore.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboStore.Flag = 0;
            this.cboStore.FormattingEnabled = true;
            this.cboStore.Length = 10;
            this.cboStore.Location = new System.Drawing.Point(112, 54);
            this.cboStore.MaxLength = 10;
            this.cboStore.MoveNext = true;
            this.cboStore.Name = "cboStore";
            this.cboStore.Size = new System.Drawing.Size(265, 20);
            this.cboStore.TabIndex = 5;
            // 
            // lblStore
            // 
            this.lblStore.AutoSize = true;
            this.lblStore.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblStore.BackColor = System.Drawing.Color.Transparent;
            this.lblStore.DefaultlabelSize = true;
            this.lblStore.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.lblStore.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.lblStore.ForeColor = System.Drawing.Color.Black;
            this.lblStore.Location = new System.Drawing.Point(58, 61);
            this.lblStore.Name = "lblStore";
            this.lblStore.Size = new System.Drawing.Size(31, 12);
            this.lblStore.TabIndex = 4;
            this.lblStore.Text = "店舗";
            this.lblStore.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblStore.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTargetDateTo
            // 
            this.txtTargetDateTo.AllowMinus = false;
            this.txtTargetDateTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtTargetDateTo.BackColor = System.Drawing.Color.White;
            this.txtTargetDateTo.BorderColor = false;
            this.txtTargetDateTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTargetDateTo.ClientColor = System.Drawing.Color.White;
            this.txtTargetDateTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtTargetDateTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.YearMonth;
            this.txtTargetDateTo.DecimalPlace = 0;
            this.txtTargetDateTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtTargetDateTo.IntegerPart = 0;
            this.txtTargetDateTo.IsCorrectDate = true;
            this.txtTargetDateTo.isEnterKeyDown = false;
            this.txtTargetDateTo.IsFirstTime = true;
            this.txtTargetDateTo.isMaxLengthErr = false;
            this.txtTargetDateTo.IsNumber = true;
            this.txtTargetDateTo.IsShop = false;
            this.txtTargetDateTo.IsTimemmss = false;
            this.txtTargetDateTo.Length = 10;
            this.txtTargetDateTo.Location = new System.Drawing.Point(277, 19);
            this.txtTargetDateTo.MaxLength = 10;
            this.txtTargetDateTo.MoveNext = true;
            this.txtTargetDateTo.Name = "txtTargetDateTo";
            this.txtTargetDateTo.Size = new System.Drawing.Size(100, 19);
            this.txtTargetDateTo.TabIndex = 3;
            this.txtTargetDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTargetDateTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtTargetDateTo.UseColorSizMode = false;
            this.txtTargetDateTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTargetDateTo_KeyDown);
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
            this.ckM_Label2.Location = new System.Drawing.Point(235, 25);
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
            this.txtTargetDateFrom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.txtTargetDateFrom.BorderColor = false;
            this.txtTargetDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTargetDateFrom.ClientColor = System.Drawing.Color.White;
            this.txtTargetDateFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtTargetDateFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.YearMonth;
            this.txtTargetDateFrom.DecimalPlace = 0;
            this.txtTargetDateFrom.Enabled = false;
            this.txtTargetDateFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtTargetDateFrom.IntegerPart = 0;
            this.txtTargetDateFrom.IsCorrectDate = true;
            this.txtTargetDateFrom.isEnterKeyDown = false;
            this.txtTargetDateFrom.IsFirstTime = true;
            this.txtTargetDateFrom.isMaxLengthErr = false;
            this.txtTargetDateFrom.IsNumber = true;
            this.txtTargetDateFrom.IsShop = false;
            this.txtTargetDateFrom.IsTimemmss = false;
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
            this.txtTargetDateFrom.UseColorSizMode = false;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.DefaultlabelSize = true;
            this.lblDate.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.lblDate.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.lblDate.ForeColor = System.Drawing.Color.Black;
            this.lblDate.Location = new System.Drawing.Point(38, 25);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(57, 12);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "対象年月";
            this.lblDate.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
        private CKM_Controls.CKM_Label lblDate;
        private CKM_Controls.CKM_ComboBox cboStore;
        private CKM_Controls.CKM_Label lblStore;
        private CKM_Controls.CKM_TextBox txtTargetDateTo;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_TextBox txtTargetDateFrom;
    }
}

