﻿namespace NyuukinKesikomiItiranHyou
{
    partial class NyuukinKesikomiItiranHyou
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
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.panelDetail = new System.Windows.Forms.Panel();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.txtCollectDateF = new CKM_Controls.CKM_TextBox();
            this.txtCollectDateT = new CKM_Controls.CKM_TextBox();
            this.InputDateF = new CKM_Controls.CKM_TextBox();
            this.InputDateT = new CKM_Controls.CKM_TextBox();
            this.cboWebCollectType = new CKM_Controls.CKM_ComboBox();
            this.ScCollectCustomerCD = new Search.CKM_SearchControl();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.cboStoreAuthorizations = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label7 = new CKM_Controls.CKM_Label();
            this.panelDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1711, 0);
            // 
            // ckM_Label1
            // 
            this.ckM_Label1.AutoSize = true;
            this.ckM_Label1.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label1.DefaultlabelSize = true;
            this.ckM_Label1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label1.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label1.Location = new System.Drawing.Point(103, 18);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label1.TabIndex = 2;
            this.ckM_Label1.Text = "入金日";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.cboStoreAuthorizations);
            this.panelDetail.Controls.Add(this.ckM_Label7);
            this.panelDetail.Controls.Add(this.ckM_Label6);
            this.panelDetail.Controls.Add(this.ckM_Label5);
            this.panelDetail.Controls.Add(this.ScCollectCustomerCD);
            this.panelDetail.Controls.Add(this.cboWebCollectType);
            this.panelDetail.Controls.Add(this.InputDateT);
            this.panelDetail.Controls.Add(this.InputDateF);
            this.panelDetail.Controls.Add(this.txtCollectDateT);
            this.panelDetail.Controls.Add(this.txtCollectDateF);
            this.panelDetail.Controls.Add(this.ckM_Label4);
            this.panelDetail.Controls.Add(this.ckM_Label3);
            this.panelDetail.Controls.Add(this.ckM_Label2);
            this.panelDetail.Controls.Add(this.ckM_Label1);
            this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetail.Location = new System.Drawing.Point(0, 50);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(1713, 879);
            this.panelDetail.TabIndex = 13;
            // 
            // ckM_Label2
            // 
            this.ckM_Label2.AutoSize = true;
            this.ckM_Label2.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label2.DefaultlabelSize = true;
            this.ckM_Label2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label2.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label2.Location = new System.Drawing.Point(77, 45);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label2.TabIndex = 3;
            this.ckM_Label2.Text = "入金入力日";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label3
            // 
            this.ckM_Label3.AutoSize = true;
            this.ckM_Label3.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label3.DefaultlabelSize = true;
            this.ckM_Label3.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label3.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label3.Location = new System.Drawing.Point(90, 74);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label3.TabIndex = 4;
            this.ckM_Label3.Text = "取込種別";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label4
            // 
            this.ckM_Label4.AutoSize = true;
            this.ckM_Label4.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label4.DefaultlabelSize = true;
            this.ckM_Label4.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label4.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label4.Location = new System.Drawing.Point(103, 101);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label4.TabIndex = 5;
            this.ckM_Label4.Text = "顧　客";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCollectDateF
            // 
            this.txtCollectDateF.AllowMinus = false;
            this.txtCollectDateF.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtCollectDateF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCollectDateF.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtCollectDateF.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtCollectDateF.DecimalPlace = 0;
            this.txtCollectDateF.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.txtCollectDateF.IntegerPart = 0;
            this.txtCollectDateF.IsCorrectDate = true;
            this.txtCollectDateF.isEnterKeyDown = false;
            this.txtCollectDateF.IsNumber = true;
            this.txtCollectDateF.IsShop = false;
            this.txtCollectDateF.Length = 10;
            this.txtCollectDateF.Location = new System.Drawing.Point(150, 14);
            this.txtCollectDateF.MaxLength = 10;
            this.txtCollectDateF.MoveNext = true;
            this.txtCollectDateF.Name = "txtCollectDateF";
            this.txtCollectDateF.Size = new System.Drawing.Size(100, 19);
            this.txtCollectDateF.TabIndex = 6;
            this.txtCollectDateF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCollectDateF.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // txtCollectDateT
            // 
            this.txtCollectDateT.AllowMinus = false;
            this.txtCollectDateT.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtCollectDateT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCollectDateT.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtCollectDateT.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtCollectDateT.DecimalPlace = 0;
            this.txtCollectDateT.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtCollectDateT.IntegerPart = 0;
            this.txtCollectDateT.IsCorrectDate = true;
            this.txtCollectDateT.isEnterKeyDown = false;
            this.txtCollectDateT.IsNumber = true;
            this.txtCollectDateT.IsShop = false;
            this.txtCollectDateT.Length = 10;
            this.txtCollectDateT.Location = new System.Drawing.Point(286, 14);
            this.txtCollectDateT.MaxLength = 10;
            this.txtCollectDateT.MoveNext = true;
            this.txtCollectDateT.Name = "txtCollectDateT";
            this.txtCollectDateT.Size = new System.Drawing.Size(100, 19);
            this.txtCollectDateT.TabIndex = 7;
            this.txtCollectDateT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCollectDateT.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // InputDateF
            // 
            this.InputDateF.AllowMinus = false;
            this.InputDateF.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.InputDateF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InputDateF.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.InputDateF.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.InputDateF.DecimalPlace = 0;
            this.InputDateF.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.InputDateF.IntegerPart = 0;
            this.InputDateF.IsCorrectDate = true;
            this.InputDateF.isEnterKeyDown = false;
            this.InputDateF.IsNumber = true;
            this.InputDateF.IsShop = false;
            this.InputDateF.Length = 10;
            this.InputDateF.Location = new System.Drawing.Point(150, 42);
            this.InputDateF.MaxLength = 10;
            this.InputDateF.MoveNext = true;
            this.InputDateF.Name = "InputDateF";
            this.InputDateF.Size = new System.Drawing.Size(100, 19);
            this.InputDateF.TabIndex = 8;
            this.InputDateF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.InputDateF.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // InputDateT
            // 
            this.InputDateT.AllowMinus = false;
            this.InputDateT.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.InputDateT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InputDateT.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.InputDateT.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.InputDateT.DecimalPlace = 0;
            this.InputDateT.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.InputDateT.IntegerPart = 0;
            this.InputDateT.IsCorrectDate = true;
            this.InputDateT.isEnterKeyDown = false;
            this.InputDateT.IsNumber = true;
            this.InputDateT.IsShop = false;
            this.InputDateT.Length = 10;
            this.InputDateT.Location = new System.Drawing.Point(285, 42);
            this.InputDateT.MaxLength = 10;
            this.InputDateT.MoveNext = true;
            this.InputDateT.Name = "InputDateT";
            this.InputDateT.Size = new System.Drawing.Size(100, 19);
            this.InputDateT.TabIndex = 9;
            this.InputDateT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.InputDateT.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // cboWebCollectType
            // 
            this.cboWebCollectType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboWebCollectType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboWebCollectType.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.取込種別;
            this.cboWebCollectType.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboWebCollectType.FormattingEnabled = true;
            this.cboWebCollectType.Length = 10;
            this.cboWebCollectType.Location = new System.Drawing.Point(150, 70);
            this.cboWebCollectType.MoveNext = true;
            this.cboWebCollectType.Name = "cboWebCollectType";
            this.cboWebCollectType.Size = new System.Drawing.Size(121, 20);
            this.cboWebCollectType.TabIndex = 10;
            // 
            // ScCollectCustomerCD
            // 
            this.ScCollectCustomerCD.AutoSize = true;
            this.ScCollectCustomerCD.ChangeDate = "";
            this.ScCollectCustomerCD.ChangeDateWidth = 100;
            this.ScCollectCustomerCD.Code = "";
            this.ScCollectCustomerCD.CodeWidth = 100;
            this.ScCollectCustomerCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScCollectCustomerCD.DataCheck = false;
            this.ScCollectCustomerCD.IsCopy = false;
            this.ScCollectCustomerCD.LabelText = "";
            this.ScCollectCustomerCD.LabelVisible = true;
            this.ScCollectCustomerCD.Location = new System.Drawing.Point(150, 93);
            this.ScCollectCustomerCD.Margin = new System.Windows.Forms.Padding(0);
            this.ScCollectCustomerCD.Name = "ScCollectCustomerCD";
            this.ScCollectCustomerCD.SearchEnable = true;
            this.ScCollectCustomerCD.Size = new System.Drawing.Size(415, 30);
            this.ScCollectCustomerCD.Stype = Search.CKM_SearchControl.SearchType.Default;
            this.ScCollectCustomerCD.TabIndex = 11;
            this.ScCollectCustomerCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScCollectCustomerCD.UseChangeDate = false;
            this.ScCollectCustomerCD.Value1 = null;
            this.ScCollectCustomerCD.Value2 = null;
            this.ScCollectCustomerCD.Value3 = null;
            // 
            // ckM_Label5
            // 
            this.ckM_Label5.AutoSize = true;
            this.ckM_Label5.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label5.DefaultlabelSize = true;
            this.ckM_Label5.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label5.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label5.Location = new System.Drawing.Point(259, 17);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label5.TabIndex = 12;
            this.ckM_Label5.Text = "～";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label6
            // 
            this.ckM_Label6.AutoSize = true;
            this.ckM_Label6.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label6.DefaultlabelSize = true;
            this.ckM_Label6.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label6.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label6.Location = new System.Drawing.Point(259, 46);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label6.TabIndex = 13;
            this.ckM_Label6.Text = "～";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboStoreAuthorizations
            // 
            this.cboStoreAuthorizations.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboStoreAuthorizations.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStoreAuthorizations.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.店舗ストア;
            this.cboStoreAuthorizations.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboStoreAuthorizations.FormattingEnabled = true;
            this.cboStoreAuthorizations.Length = 10;
            this.cboStoreAuthorizations.Location = new System.Drawing.Point(1503, 18);
            this.cboStoreAuthorizations.MaxLength = 10;
            this.cboStoreAuthorizations.MoveNext = true;
            this.cboStoreAuthorizations.Name = "cboStoreAuthorizations";
            this.cboStoreAuthorizations.Size = new System.Drawing.Size(121, 20);
            this.cboStoreAuthorizations.TabIndex = 15;
            // 
            // ckM_Label7
            // 
            this.ckM_Label7.AutoSize = true;
            this.ckM_Label7.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label7.DefaultlabelSize = true;
            this.ckM_Label7.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label7.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label7.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label7.Location = new System.Drawing.Point(1468, 23);
            this.ckM_Label7.Name = "ckM_Label7";
            this.ckM_Label7.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label7.TabIndex = 16;
            this.ckM_Label7.Text = "店舗";
            this.ckM_Label7.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NyuukinKesikomiItiranHyou
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.panelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "NyuukinKesikomiItiranHyou";
            this.PanelHeaderHeight = 50;
            this.Text = "NyuukinKesikomiItiranHyou";
            this.Load += new System.EventHandler(this.NyuukinKesikomiItiranHyou_Load);
            this.Controls.SetChildIndex(this.panelDetail, 0);
            this.panelDetail.ResumeLayout(false);
            this.panelDetail.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CKM_Controls.CKM_Label ckM_Label1;
        private System.Windows.Forms.Panel panelDetail;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_Label ckM_Label5;
        private Search.CKM_SearchControl ScCollectCustomerCD;
        private CKM_Controls.CKM_ComboBox cboWebCollectType;
        private CKM_Controls.CKM_TextBox InputDateT;
        private CKM_Controls.CKM_TextBox InputDateF;
        private CKM_Controls.CKM_TextBox txtCollectDateT;
        private CKM_Controls.CKM_TextBox txtCollectDateF;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_ComboBox cboStoreAuthorizations;
        private CKM_Controls.CKM_Label ckM_Label7;
    }
}

