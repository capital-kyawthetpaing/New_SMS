namespace SaikenKanriHyou
{
    partial class SaikenKanriHyou
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
            this.panelHealder = new System.Windows.Forms.Panel();
            this.cbo_Store = new CKM_Controls.CKM_ComboBox();
            this.lblStoreName = new CKM_Controls.CKM_Label();
            this.chk_Check = new CKM_Controls.CKM_CheckBox();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.sc_Customer = new Search.CKM_SearchControl();
            this.lblClient = new CKM_Controls.CKM_Label();
            this.rdo_Sale = new CKM_Controls.CKM_RadioButton();
            this.rdo_BillAddress = new CKM_Controls.CKM_RadioButton();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.txtTargetdate = new CKM_Controls.CKM_TextBox();
            this.lbltargetdate = new CKM_Controls.CKM_Label();
            this.panelDetail = new System.Windows.Forms.Panel();
            this.PanelHeader.SuspendLayout();
            this.panelDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.panelHealder);
            this.PanelHeader.Size = new System.Drawing.Size(1711, 0);
            this.PanelHeader.Controls.SetChildIndex(this.panelHealder, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(1177, 0);
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            // 
            // panelHealder
            // 
            this.panelHealder.Location = new System.Drawing.Point(5, 2);
            this.panelHealder.Name = "panelHealder";
            this.panelHealder.Size = new System.Drawing.Size(1700, 130);
            this.panelHealder.TabIndex = 0;
            // 
            // cbo_Store
            // 
            this.cbo_Store.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cbo_Store.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbo_Store.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.店舗ストア;
            this.cbo_Store.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cbo_Store.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbo_Store.Flag = 0;
            this.cbo_Store.FormattingEnabled = true;
            this.cbo_Store.Length = 10;
            this.cbo_Store.Location = new System.Drawing.Point(1419, 28);
            this.cbo_Store.MaxLength = 10;
            this.cbo_Store.MoveNext = false;
            this.cbo_Store.Name = "cbo_Store";
            this.cbo_Store.Size = new System.Drawing.Size(280, 20);
            this.cbo_Store.TabIndex = 1;
            // 
            // lblStoreName
            // 
            this.lblStoreName.AutoSize = true;
            this.lblStoreName.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblStoreName.BackColor = System.Drawing.Color.Transparent;
            this.lblStoreName.DefaultlabelSize = true;
            this.lblStoreName.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.lblStoreName.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.lblStoreName.ForeColor = System.Drawing.Color.Black;
            this.lblStoreName.Location = new System.Drawing.Point(1385, 31);
            this.lblStoreName.Name = "lblStoreName";
            this.lblStoreName.Size = new System.Drawing.Size(31, 12);
            this.lblStoreName.TabIndex = 9;
            this.lblStoreName.Text = "店舗";
            this.lblStoreName.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblStoreName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chk_Check
            // 
            this.chk_Check.AutoSize = true;
            this.chk_Check.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chk_Check.Location = new System.Drawing.Point(133, 122);
            this.chk_Check.Name = "chk_Check";
            this.chk_Check.Size = new System.Drawing.Size(50, 16);
            this.chk_Check.TabIndex = 5;
            this.chk_Check.Text = "する";
            this.chk_Check.UseVisualStyleBackColor = true;
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
            this.ckM_Label2.Location = new System.Drawing.Point(45, 123);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label2.TabIndex = 7;
            this.ckM_Label2.Text = "残高０円印刷";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.sc_Customer.Location = new System.Drawing.Point(133, 84);
            this.sc_Customer.Margin = new System.Windows.Forms.Padding(0);
            this.sc_Customer.Name = "sc_Customer";
            this.sc_Customer.NameWidth = 500;
            this.sc_Customer.SearchEnable = true;
            this.sc_Customer.Size = new System.Drawing.Size(634, 28);
            this.sc_Customer.Stype = Search.CKM_SearchControl.SearchType.得意先;
            this.sc_Customer.TabIndex = 4;
            this.sc_Customer.test = null;
            this.sc_Customer.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.sc_Customer.UseChangeDate = false;
            this.sc_Customer.Value1 = null;
            this.sc_Customer.Value2 = null;
            this.sc_Customer.Value3 = null;
            this.sc_Customer.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.sc_Customer_CodeKeyDownEvent);
            this.sc_Customer.Enter += new System.EventHandler(this.sc_Customer_Enter);
            // 
            // lblClient
            // 
            this.lblClient.AutoSize = true;
            this.lblClient.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblClient.BackColor = System.Drawing.Color.Transparent;
            this.lblClient.DefaultlabelSize = true;
            this.lblClient.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.lblClient.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.lblClient.ForeColor = System.Drawing.Color.Black;
            this.lblClient.Location = new System.Drawing.Point(85, 92);
            this.lblClient.Name = "lblClient";
            this.lblClient.Size = new System.Drawing.Size(45, 12);
            this.lblClient.TabIndex = 5;
            this.lblClient.Text = "顧  客";
            this.lblClient.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblClient.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rdo_Sale
            // 
            this.rdo_Sale.AutoSize = true;
            this.rdo_Sale.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.rdo_Sale.Location = new System.Drawing.Point(232, 62);
            this.rdo_Sale.Name = "rdo_Sale";
            this.rdo_Sale.Size = new System.Drawing.Size(62, 16);
            this.rdo_Sale.TabIndex = 3;
            this.rdo_Sale.TabStop = true;
            this.rdo_Sale.Text = "販売先";
            this.rdo_Sale.UseVisualStyleBackColor = true;
            // 
            // rdo_BillAddress
            // 
            this.rdo_BillAddress.AutoSize = true;
            this.rdo_BillAddress.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.rdo_BillAddress.Location = new System.Drawing.Point(133, 62);
            this.rdo_BillAddress.Name = "rdo_BillAddress";
            this.rdo_BillAddress.Size = new System.Drawing.Size(62, 16);
            this.rdo_BillAddress.TabIndex = 2;
            this.rdo_BillAddress.TabStop = true;
            this.rdo_BillAddress.Text = "請求先";
            this.rdo_BillAddress.UseVisualStyleBackColor = true;
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
            this.ckM_Label1.Location = new System.Drawing.Point(71, 64);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label1.TabIndex = 2;
            this.ckM_Label1.Text = "印刷対象";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTargetdate
            // 
            this.txtTargetdate.AllowMinus = false;
            this.txtTargetdate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtTargetdate.BackColor = System.Drawing.Color.White;
            this.txtTargetdate.BorderColor = false;
            this.txtTargetdate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTargetdate.ClientColor = System.Drawing.Color.White;
            this.txtTargetdate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtTargetdate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.YearMonth;
            this.txtTargetdate.DecimalPlace = 0;
            this.txtTargetdate.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtTargetdate.IntegerPart = 0;
            this.txtTargetdate.IsCorrectDate = true;
            this.txtTargetdate.isEnterKeyDown = false;
            this.txtTargetdate.IsFirstTime = true;
            this.txtTargetdate.isMaxLengthErr = false;
            this.txtTargetdate.IsNumber = true;
            this.txtTargetdate.IsShop = false;
            this.txtTargetdate.IsTimemmss = false;
            this.txtTargetdate.Length = 10;
            this.txtTargetdate.Location = new System.Drawing.Point(133, 29);
            this.txtTargetdate.MaxLength = 10;
            this.txtTargetdate.MoveNext = true;
            this.txtTargetdate.Name = "txtTargetdate";
            this.txtTargetdate.Size = new System.Drawing.Size(100, 19);
            this.txtTargetdate.TabIndex = 0;
            this.txtTargetdate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTargetdate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtTargetdate.UseColorSizMode = false;
            // 
            // lbltargetdate
            // 
            this.lbltargetdate.AutoSize = true;
            this.lbltargetdate.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lbltargetdate.BackColor = System.Drawing.Color.Transparent;
            this.lbltargetdate.DefaultlabelSize = true;
            this.lbltargetdate.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.lbltargetdate.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.lbltargetdate.ForeColor = System.Drawing.Color.Black;
            this.lbltargetdate.Location = new System.Drawing.Point(73, 33);
            this.lbltargetdate.Name = "lbltargetdate";
            this.lbltargetdate.Size = new System.Drawing.Size(57, 12);
            this.lbltargetdate.TabIndex = 0;
            this.lbltargetdate.Text = "対象年月";
            this.lbltargetdate.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lbltargetdate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.cbo_Store);
            this.panelDetail.Controls.Add(this.lblStoreName);
            this.panelDetail.Controls.Add(this.txtTargetdate);
            this.panelDetail.Controls.Add(this.lbltargetdate);
            this.panelDetail.Controls.Add(this.chk_Check);
            this.panelDetail.Controls.Add(this.ckM_Label1);
            this.panelDetail.Controls.Add(this.ckM_Label2);
            this.panelDetail.Controls.Add(this.rdo_BillAddress);
            this.panelDetail.Controls.Add(this.sc_Customer);
            this.panelDetail.Controls.Add(this.rdo_Sale);
            this.panelDetail.Controls.Add(this.lblClient);
            this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetail.Location = new System.Drawing.Point(0, 40);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(1713, 889);
            this.panelDetail.TabIndex = 100;
            // 
            // SaikenKanriHyou
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.panelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "SaikenKanriHyou";
            this.PanelHeaderHeight = 40;
            this.Text = "SaikenKanriHyou";
            this.Load += new System.EventHandler(this.SaikenKanriHyou_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SaikenKanriHyou_KeyUp);
            this.Controls.SetChildIndex(this.panelDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.panelDetail.ResumeLayout(false);
            this.panelDetail.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelHealder;
        private CKM_Controls.CKM_TextBox txtTargetdate;
        private CKM_Controls.CKM_Label lbltargetdate;
        private CKM_Controls.CKM_CheckBox chk_Check;
        private CKM_Controls.CKM_Label ckM_Label2;
        private Search.CKM_SearchControl sc_Customer;
        private CKM_Controls.CKM_Label lblClient;
        private CKM_Controls.CKM_RadioButton rdo_Sale;
        private CKM_Controls.CKM_RadioButton rdo_BillAddress;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_ComboBox cbo_Store;
        private CKM_Controls.CKM_Label lblStoreName;
        private System.Windows.Forms.Panel panelDetail;
    }
}


