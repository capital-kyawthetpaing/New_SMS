namespace ZaikoYoteiHyou
{
    partial class FrmZaikoYoteiHyou
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
            this.panelDetail = new System.Windows.Forms.Panel();
            this.txtTargetDateFrom = new CKM_Controls.CKM_TextBox();
            this.cboWareHouse = new CKM_Controls.CKM_ComboBox();
            this.cboStore = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.txtTargetDateTo = new CKM_Controls.CKM_TextBox();
            this.label = new CKM_Controls.CKM_Label();
            this.panelDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1711, 4);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(1177, 0);
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            // 
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.txtTargetDateFrom);
            this.panelDetail.Controls.Add(this.cboWareHouse);
            this.panelDetail.Controls.Add(this.cboStore);
            this.panelDetail.Controls.Add(this.ckM_Label5);
            this.panelDetail.Controls.Add(this.ckM_Label4);
            this.panelDetail.Controls.Add(this.ckM_Label3);
            this.panelDetail.Controls.Add(this.txtTargetDateTo);
            this.panelDetail.Controls.Add(this.label);
            this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetail.Location = new System.Drawing.Point(0, 60);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(1713, 869);
            this.panelDetail.TabIndex = 0;
            // 
            // txtTargetDateFrom
            // 
            this.txtTargetDateFrom.AllowMinus = false;
            this.txtTargetDateFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtTargetDateFrom.BackColor = System.Drawing.Color.White;
            this.txtTargetDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTargetDateFrom.ClientColor = System.Drawing.Color.White;
            this.txtTargetDateFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtTargetDateFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.YearMonth;
            this.txtTargetDateFrom.DecimalPlace = 0;
            this.txtTargetDateFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtTargetDateFrom.IntegerPart = 0;
            this.txtTargetDateFrom.IsCorrectDate = true;
            this.txtTargetDateFrom.isEnterKeyDown = false;
            this.txtTargetDateFrom.isMaxLengthErr = false;
            this.txtTargetDateFrom.IsNumber = true;
            this.txtTargetDateFrom.IsShop = false;
            this.txtTargetDateFrom.Length = 6;
            this.txtTargetDateFrom.Location = new System.Drawing.Point(110, 21);
            this.txtTargetDateFrom.MaxLength = 6;
            this.txtTargetDateFrom.MoveNext = true;
            this.txtTargetDateFrom.Name = "txtTargetDateFrom";
            this.txtTargetDateFrom.ReadOnly = true;
            this.txtTargetDateFrom.Size = new System.Drawing.Size(100, 19);
            this.txtTargetDateFrom.TabIndex = 0;
            this.txtTargetDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTargetDateFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // cboWareHouse
            // 
            this.cboWareHouse.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboWareHouse.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboWareHouse.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.倉庫;
            this.cboWareHouse.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboWareHouse.Flag = 0;
            this.cboWareHouse.FormattingEnabled = true;
            this.cboWareHouse.Length = 10;
            this.cboWareHouse.Location = new System.Drawing.Point(110, 96);
            this.cboWareHouse.MaxLength = 10;
            this.cboWareHouse.MoveNext = true;
            this.cboWareHouse.Name = "cboWareHouse";
            this.cboWareHouse.Size = new System.Drawing.Size(265, 20);
            this.cboWareHouse.TabIndex = 3;
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
            this.cboStore.Location = new System.Drawing.Point(110, 58);
            this.cboStore.MaxLength = 10;
            this.cboStore.MoveNext = true;
            this.cboStore.Name = "cboStore";
            this.cboStore.Size = new System.Drawing.Size(265, 20);
            this.cboStore.TabIndex = 2;
            // 
            // ckM_Label5
            // 
            this.ckM_Label5.AutoSize = true;
            this.ckM_Label5.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label5.DefaultlabelSize = true;
            this.ckM_Label5.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label5.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label5.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label5.Location = new System.Drawing.Point(71, 99);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label5.TabIndex = 5;
            this.ckM_Label5.Text = "倉庫";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label4
            // 
            this.ckM_Label4.AutoSize = true;
            this.ckM_Label4.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label4.DefaultlabelSize = true;
            this.ckM_Label4.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label4.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label4.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label4.Location = new System.Drawing.Point(71, 61);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label4.TabIndex = 5;
            this.ckM_Label4.Text = "店舗";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label3.Location = new System.Drawing.Point(233, 24);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label3.TabIndex = 6;
            this.ckM_Label3.Text = "～";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTargetDateTo
            // 
            this.txtTargetDateTo.AllowMinus = false;
            this.txtTargetDateTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtTargetDateTo.BackColor = System.Drawing.Color.White;
            this.txtTargetDateTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTargetDateTo.ClientColor = System.Drawing.Color.White;
            this.txtTargetDateTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtTargetDateTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.YearMonth;
            this.txtTargetDateTo.DecimalPlace = 0;
            this.txtTargetDateTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtTargetDateTo.IntegerPart = 0;
            this.txtTargetDateTo.IsCorrectDate = true;
            this.txtTargetDateTo.isEnterKeyDown = false;
            this.txtTargetDateTo.isMaxLengthErr = false;
            this.txtTargetDateTo.IsNumber = true;
            this.txtTargetDateTo.IsShop = false;
            this.txtTargetDateTo.Length = 6;
            this.txtTargetDateTo.Location = new System.Drawing.Point(274, 21);
            this.txtTargetDateTo.MaxLength = 6;
            this.txtTargetDateTo.MoveNext = true;
            this.txtTargetDateTo.Name = "txtTargetDateTo";
            this.txtTargetDateTo.Size = new System.Drawing.Size(100, 19);
            this.txtTargetDateTo.TabIndex = 1;
            this.txtTargetDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTargetDateTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtTargetDateTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTargetDateTo_KeyDown);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label.BackColor = System.Drawing.Color.Transparent;
            this.label.DefaultlabelSize = true;
            this.label.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.label.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label.ForeColor = System.Drawing.Color.Black;
            this.label.Location = new System.Drawing.Point(45, 24);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(57, 12);
            this.label.TabIndex = 4;
            this.label.Text = "対象年月";
            this.label.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmZaikoYoteiHyou
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.panelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "FrmZaikoYoteiHyou";
            this.PanelHeaderHeight = 60;
            this.Text = "ZaikoYoteiHyou";
            this.Load += new System.EventHandler(this.FrmZaikoYoteiHyou_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmZaikoYoteiHyou_KeyUp);
            this.Controls.SetChildIndex(this.panelDetail, 0);
            this.panelDetail.ResumeLayout(false);
            this.panelDetail.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelDetail;
        private CKM_Controls.CKM_ComboBox cboWareHouse;
        private CKM_Controls.CKM_ComboBox cboStore;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_TextBox txtTargetDateTo;
        private CKM_Controls.CKM_Label label;
        private CKM_Controls.CKM_TextBox txtTargetDateFrom;
    }
}