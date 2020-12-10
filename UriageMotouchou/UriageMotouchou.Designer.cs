namespace UriageMotouchou
{
    partial class UriageMotouchou
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
            this.chkNo = new CKM_Controls.CKM_CheckBox();
            this.chkYes = new CKM_Controls.CKM_CheckBox();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.cboStore = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.sc_Customer = new Search.CKM_SearchControl();
            this.txtTagetFrom = new CKM_Controls.CKM_TextBox();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.Panel_Details = new System.Windows.Forms.Panel();
            this.Panel_Details.SuspendLayout();
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
            // chkNo
            // 
            this.chkNo.AutoSize = true;
            this.chkNo.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkNo.Location = new System.Drawing.Point(191, 112);
            this.chkNo.Name = "chkNo";
            this.chkNo.Size = new System.Drawing.Size(37, 16);
            this.chkNo.TabIndex = 32;
            this.chkNo.Text = "無";
            this.chkNo.UseVisualStyleBackColor = true;
            // 
            // chkYes
            // 
            this.chkYes.AutoSize = true;
            this.chkYes.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkYes.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkYes.Location = new System.Drawing.Point(124, 112);
            this.chkYes.Name = "chkYes";
            this.chkYes.Size = new System.Drawing.Size(37, 16);
            this.chkYes.TabIndex = 31;
            this.chkYes.Text = "有";
            this.chkYes.UseVisualStyleBackColor = true;
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
            this.ckM_Label5.Location = new System.Drawing.Point(77, 113);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label5.TabIndex = 30;
            this.ckM_Label5.Text = "債権残";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.cboStore.Location = new System.Drawing.Point(124, 80);
            this.cboStore.MaxLength = 10;
            this.cboStore.MoveNext = true;
            this.cboStore.Name = "cboStore";
            this.cboStore.Size = new System.Drawing.Size(265, 20);
            this.cboStore.TabIndex = 29;
            this.cboStore.Text = "西川店ＭＭＭＭＭ１０ＭＭＭＭＭＭＭＭ２０";
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
            this.ckM_Label4.Location = new System.Drawing.Point(51, 83);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label4.TabIndex = 28;
            this.ckM_Label4.Text = "店舗ストア";
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
            this.ckM_Label3.Location = new System.Drawing.Point(90, 49);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label3.TabIndex = 27;
            this.ckM_Label3.Text = "顧客";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sc_Customer
            // 
            this.sc_Customer.AutoSize = true;
            this.sc_Customer.ChangeDate = "";
            this.sc_Customer.ChangeDateWidth = 100;
            this.sc_Customer.Code = "";
            this.sc_Customer.CodeWidth = 100;
            this.sc_Customer.CodeWidth1 = 100;
            this.sc_Customer.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.sc_Customer.DataCheck = false;
            this.sc_Customer.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.sc_Customer.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.sc_Customer.IsCopy = false;
            this.sc_Customer.LabelText = "";
            this.sc_Customer.LabelVisible = true;
            this.sc_Customer.Location = new System.Drawing.Point(124, 41);
            this.sc_Customer.Margin = new System.Windows.Forms.Padding(0);
            this.sc_Customer.Name = "sc_Customer";
            this.sc_Customer.NameWidth = 500;
            this.sc_Customer.SearchEnable = true;
            this.sc_Customer.Size = new System.Drawing.Size(634, 27);
            this.sc_Customer.Stype = Search.CKM_SearchControl.SearchType.得意先;
            this.sc_Customer.TabIndex = 26;
            this.sc_Customer.test = null;
            this.sc_Customer.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.sc_Customer.UseChangeDate = false;
            this.sc_Customer.Value1 = null;
            this.sc_Customer.Value2 = null;
            this.sc_Customer.Value3 = null;
            this.sc_Customer.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.sc_Customer_CodeKeyDownEvent);
            this.sc_Customer.Enter += new System.EventHandler(this.sc_Customer_Enter);
            // 
            // txtTagetFrom
            // 
            this.txtTagetFrom.AllowMinus = false;
            this.txtTagetFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtTagetFrom.BackColor = System.Drawing.Color.White;
            this.txtTagetFrom.BorderColor = false;
            this.txtTagetFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTagetFrom.ClientColor = System.Drawing.Color.White;
            this.txtTagetFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtTagetFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.YearMonth;
            this.txtTagetFrom.DecimalPlace = 0;
            this.txtTagetFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtTagetFrom.IntegerPart = 0;
            this.txtTagetFrom.IsCorrectDate = true;
            this.txtTagetFrom.isEnterKeyDown = false;
            this.txtTagetFrom.IsFirstTime = true;
            this.txtTagetFrom.isMaxLengthErr = false;
            this.txtTagetFrom.IsNumber = true;
            this.txtTagetFrom.IsShop = false;
            this.txtTagetFrom.Length = 6;
            this.txtTagetFrom.Location = new System.Drawing.Point(124, 13);
            this.txtTagetFrom.MaxLength = 6;
            this.txtTagetFrom.MoveNext = true;
            this.txtTagetFrom.Name = "txtTagetFrom";
            this.txtTagetFrom.Size = new System.Drawing.Size(100, 19);
            this.txtTagetFrom.TabIndex = 23;
            this.txtTagetFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTagetFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtTagetFrom.UseColorSizMode = false;
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
            this.ckM_Label1.Location = new System.Drawing.Point(64, 16);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label1.TabIndex = 22;
            this.ckM_Label1.Text = "対象期間";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Panel_Details
            // 
            this.Panel_Details.Controls.Add(this.ckM_Label1);
            this.Panel_Details.Controls.Add(this.chkNo);
            this.Panel_Details.Controls.Add(this.cboStore);
            this.Panel_Details.Controls.Add(this.sc_Customer);
            this.Panel_Details.Controls.Add(this.chkYes);
            this.Panel_Details.Controls.Add(this.txtTagetFrom);
            this.Panel_Details.Controls.Add(this.ckM_Label4);
            this.Panel_Details.Controls.Add(this.ckM_Label5);
            this.Panel_Details.Controls.Add(this.ckM_Label3);
            this.Panel_Details.Location = new System.Drawing.Point(2, 41);
            this.Panel_Details.Name = "Panel_Details";
            this.Panel_Details.Size = new System.Drawing.Size(1710, 140);
            this.Panel_Details.TabIndex = 100;
            // 
            // UriageMotouchou
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.Panel_Details);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "UriageMotouchou";
            this.PanelHeaderHeight = 40;
            this.Text = "UriageMotouchou";
            this.Load += new System.EventHandler(this.UriageMotouchou_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UriageMotouchou_KeyUp);
            this.Controls.SetChildIndex(this.Panel_Details, 0);
            this.Panel_Details.ResumeLayout(false);
            this.Panel_Details.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CKM_Controls.CKM_CheckBox chkNo;
        private CKM_Controls.CKM_CheckBox chkYes;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_ComboBox cboStore;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Label ckM_Label3;
        private Search.CKM_SearchControl sc_Customer;
        private CKM_Controls.CKM_TextBox txtTagetFrom;
        private CKM_Controls.CKM_Label ckM_Label1;
        private System.Windows.Forms.Panel Panel_Details;
    }
}

