namespace MasterTouroku_UnsouGaisya
{
    partial class FrmMasterTouroku_UnsouGaisya
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
            this.PanelNormal = new System.Windows.Forms.Panel();
            this.ScShippingCD = new Search.CKM_SearchControl();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.運送会社CD = new CKM_Controls.CKM_Label();
            this.PanelCopy = new System.Windows.Forms.Panel();
            this.ScCopyShippingCD = new Search.CKM_SearchControl();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.btnDisplay = new CKM_Controls.CKM_Button();
            this.PanelDetail = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtremark = new CKM_Controls.CKM_MultiLineTextBox();
            this.chkDeleteFlg = new CKM_Controls.CKM_CheckBox();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.txtOtherCD = new CKM_Controls.CKM_TextBox();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.txtPonpareCD = new CKM_Controls.CKM_TextBox();
            this.ckM_Label7 = new CKM_Controls.CKM_Label();
            this.txtWowmaCD = new CKM_Controls.CKM_TextBox();
            this.ckM_Label8 = new CKM_Controls.CKM_Label();
            this.txtAmazonCD = new CKM_Controls.CKM_TextBox();
            this.ckM_Label9 = new CKM_Controls.CKM_Label();
            this.txtRakutenCD = new CKM_Controls.CKM_TextBox();
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.cboIdentity = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label11 = new CKM_Controls.CKM_Label();
            this.txtYahooCD = new CKM_Controls.CKM_TextBox();
            this.ckM_Label12 = new CKM_Controls.CKM_Label();
            this.txtShippingName = new CKM_Controls.CKM_TextBox();
            this.cboNormalType = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label13 = new CKM_Controls.CKM_Label();
            this.PanelHeader.SuspendLayout();
            this.PanelSearch.SuspendLayout();
            this.PanelNormal.SuspendLayout();
            this.PanelCopy.SuspendLayout();
            this.PanelDetail.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.PanelCopy);
            this.PanelHeader.Controls.Add(this.PanelNormal);
            this.PanelHeader.Size = new System.Drawing.Size(1521, 91);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.Controls.SetChildIndex(this.PanelNormal, 0);
            this.PanelHeader.Controls.SetChildIndex(this.PanelCopy, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Controls.Add(this.btnDisplay);
            this.PanelSearch.Location = new System.Drawing.Point(987, 0);
            this.PanelSearch.TabIndex = 1;
            // 
            // PanelNormal
            // 
            this.PanelNormal.Controls.Add(this.ScShippingCD);
            this.PanelNormal.Controls.Add(this.ckM_Label1);
            this.PanelNormal.Controls.Add(this.運送会社CD);
            this.PanelNormal.Location = new System.Drawing.Point(12, 4);
            this.PanelNormal.Name = "PanelNormal";
            this.PanelNormal.Size = new System.Drawing.Size(205, 50);
            this.PanelNormal.TabIndex = 0;
            // 
            // ScShippingCD
            // 
            this.ScShippingCD.AutoSize = true;
            this.ScShippingCD.ChangeDate = "";
            this.ScShippingCD.ChangeDateWidth = 100;
            this.ScShippingCD.Code = "";
            this.ScShippingCD.CodeWidth = 60;
            this.ScShippingCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScShippingCD.DataCheck = false;
            this.ScShippingCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScShippingCD.IsCopy = false;
            this.ScShippingCD.LabelText = "";
            this.ScShippingCD.LabelVisible = false;
            this.ScShippingCD.Location = new System.Drawing.Point(99, -3);
            this.ScShippingCD.Margin = new System.Windows.Forms.Padding(0);
            this.ScShippingCD.Name = "ScShippingCD";
            this.ScShippingCD.SearchEnable = true;
            this.ScShippingCD.Size = new System.Drawing.Size(103, 50);
            this.ScShippingCD.Stype = Search.CKM_SearchControl.SearchType.Carrier;
            this.ScShippingCD.TabIndex = 0;
            this.ScShippingCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScShippingCD.UseChangeDate = true;
            this.ScShippingCD.Value1 = null;
            this.ScShippingCD.Value2 = null;
            this.ScShippingCD.Value3 = null;
            this.ScShippingCD.ChangeDateKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScShippingCD_ChangeDateKeyDownEvent);
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
            this.ckM_Label1.Location = new System.Drawing.Point(52, 29);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label1.TabIndex = 2;
            this.ckM_Label1.Text = "改定日";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // 運送会社CD
            // 
            this.運送会社CD.AutoSize = true;
            this.運送会社CD.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.運送会社CD.BackColor = System.Drawing.Color.Transparent;
            this.運送会社CD.DefaultlabelSize = true;
            this.運送会社CD.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.運送会社CD.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.運送会社CD.ForeColor = System.Drawing.Color.Black;
            this.運送会社CD.Location = new System.Drawing.Point(25, 6);
            this.運送会社CD.Name = "運送会社CD";
            this.運送会社CD.Size = new System.Drawing.Size(71, 12);
            this.運送会社CD.TabIndex = 0;
            this.運送会社CD.Text = "運送会社CD";
            this.運送会社CD.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.運送会社CD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PanelCopy
            // 
            this.PanelCopy.Controls.Add(this.ScCopyShippingCD);
            this.PanelCopy.Controls.Add(this.ckM_Label3);
            this.PanelCopy.Controls.Add(this.ckM_Label2);
            this.PanelCopy.Location = new System.Drawing.Point(573, 3);
            this.PanelCopy.Name = "PanelCopy";
            this.PanelCopy.Size = new System.Drawing.Size(205, 50);
            this.PanelCopy.TabIndex = 1;
            // 
            // ScCopyShippingCD
            // 
            this.ScCopyShippingCD.AutoSize = true;
            this.ScCopyShippingCD.ChangeDate = "";
            this.ScCopyShippingCD.ChangeDateWidth = 100;
            this.ScCopyShippingCD.Code = "";
            this.ScCopyShippingCD.CodeWidth = 60;
            this.ScCopyShippingCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScCopyShippingCD.DataCheck = false;
            this.ScCopyShippingCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScCopyShippingCD.IsCopy = false;
            this.ScCopyShippingCD.LabelText = "";
            this.ScCopyShippingCD.LabelVisible = false;
            this.ScCopyShippingCD.Location = new System.Drawing.Point(100, -5);
            this.ScCopyShippingCD.Margin = new System.Windows.Forms.Padding(0);
            this.ScCopyShippingCD.Name = "ScCopyShippingCD";
            this.ScCopyShippingCD.SearchEnable = true;
            this.ScCopyShippingCD.Size = new System.Drawing.Size(103, 50);
            this.ScCopyShippingCD.Stype = Search.CKM_SearchControl.SearchType.Carrier;
            this.ScCopyShippingCD.TabIndex = 0;
            this.ScCopyShippingCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScCopyShippingCD.UseChangeDate = true;
            this.ScCopyShippingCD.Value1 = null;
            this.ScCopyShippingCD.Value2 = null;
            this.ScCopyShippingCD.Value3 = null;
            this.ScCopyShippingCD.ChangeDateKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScCopyShippingCD_ChangeDateKeyDownEvent);
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
            this.ckM_Label3.Location = new System.Drawing.Point(1, 6);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(97, 12);
            this.ckM_Label3.TabIndex = 0;
            this.ckM_Label3.Text = "複写運送会社CD";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label2.Location = new System.Drawing.Point(55, 28);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label2.TabIndex = 2;
            this.ckM_Label2.Text = "改定日";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnDisplay
            // 
            this.btnDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnDisplay.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnDisplay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDisplay.DefaultBtnSize = false;
            this.btnDisplay.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnDisplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisplay.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnDisplay.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnDisplay.Location = new System.Drawing.Point(410, 3);
            this.btnDisplay.Margin = new System.Windows.Forms.Padding(1);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(118, 28);
            this.btnDisplay.TabIndex = 2;
            this.btnDisplay.Text = "表示(F11)";
            this.btnDisplay.UseVisualStyleBackColor = false;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // PanelDetail
            // 
            this.PanelDetail.Controls.Add(this.panel1);
            this.PanelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelDetail.Location = new System.Drawing.Point(0, 147);
            this.PanelDetail.Name = "PanelDetail";
            this.PanelDetail.Size = new System.Drawing.Size(1523, 782);
            this.PanelDetail.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ckM_Label13);
            this.panel1.Controls.Add(this.cboNormalType);
            this.panel1.Controls.Add(this.txtremark);
            this.panel1.Controls.Add(this.chkDeleteFlg);
            this.panel1.Controls.Add(this.ckM_Label4);
            this.panel1.Controls.Add(this.ckM_Label5);
            this.panel1.Controls.Add(this.txtOtherCD);
            this.panel1.Controls.Add(this.ckM_Label6);
            this.panel1.Controls.Add(this.txtPonpareCD);
            this.panel1.Controls.Add(this.ckM_Label7);
            this.panel1.Controls.Add(this.txtWowmaCD);
            this.panel1.Controls.Add(this.ckM_Label8);
            this.panel1.Controls.Add(this.txtAmazonCD);
            this.panel1.Controls.Add(this.ckM_Label9);
            this.panel1.Controls.Add(this.txtRakutenCD);
            this.panel1.Controls.Add(this.ckM_Label10);
            this.panel1.Controls.Add(this.cboIdentity);
            this.panel1.Controls.Add(this.ckM_Label11);
            this.panel1.Controls.Add(this.txtYahooCD);
            this.panel1.Controls.Add(this.ckM_Label12);
            this.panel1.Controls.Add(this.txtShippingName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1523, 782);
            this.panel1.TabIndex = 0;
            // 
            // txtremark
            // 
            this.txtremark.Back_Color = CKM_Controls.CKM_MultiLineTextBox.CKM_Color.White;
            this.txtremark.BackColor = System.Drawing.Color.White;
            this.txtremark.Ctrl_Byte = CKM_Controls.CKM_MultiLineTextBox.Bytes.半全角;
            this.txtremark.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtremark.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtremark.Length = 50;
            this.txtremark.Location = new System.Drawing.Point(112, 276);
            this.txtremark.MaxLength = 50;
            this.txtremark.Mdea = false;
            this.txtremark.Mfocus = false;
            this.txtremark.MoveNext = true;
            this.txtremark.Multiline = true;
            this.txtremark.Name = "txtremark";
            this.txtremark.RowCount = 5;
            this.txtremark.Size = new System.Drawing.Size(500, 95);
            this.txtremark.TabIndex = 8;
            this.txtremark.TextSize = CKM_Controls.CKM_MultiLineTextBox.FontSize.Normal;
            // 
            // chkDeleteFlg
            // 
            this.chkDeleteFlg.AutoSize = true;
            this.chkDeleteFlg.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkDeleteFlg.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkDeleteFlg.Location = new System.Drawing.Point(1438, 22);
            this.chkDeleteFlg.Name = "chkDeleteFlg";
            this.chkDeleteFlg.Size = new System.Drawing.Size(50, 16);
            this.chkDeleteFlg.TabIndex = 9;
            this.chkDeleteFlg.Text = "削除";
            this.chkDeleteFlg.UseVisualStyleBackColor = true;
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
            this.ckM_Label4.Location = new System.Drawing.Point(39, 22);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label4.TabIndex = 19;
            this.ckM_Label4.Text = "運送会社名";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label5.Location = new System.Drawing.Point(65, 46);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label5.TabIndex = 21;
            this.ckM_Label5.Text = "識　別";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOtherCD
            // 
            this.txtOtherCD.AllowMinus = false;
            this.txtOtherCD.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtOtherCD.BackColor = System.Drawing.Color.White;
            this.txtOtherCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOtherCD.ClientColor = System.Drawing.Color.White;
            this.txtOtherCD.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtOtherCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtOtherCD.DecimalPlace = 0;
            this.txtOtherCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtOtherCD.IntegerPart = 0;
            this.txtOtherCD.IsCorrectDate = true;
            this.txtOtherCD.isEnterKeyDown = false;
            this.txtOtherCD.isMaxLengthErr = false;
            this.txtOtherCD.IsNumber = true;
            this.txtOtherCD.IsShop = false;
            this.txtOtherCD.Length = 20;
            this.txtOtherCD.Location = new System.Drawing.Point(112, 228);
            this.txtOtherCD.MaxLength = 20;
            this.txtOtherCD.MoveNext = true;
            this.txtOtherCD.Name = "txtOtherCD";
            this.txtOtherCD.Size = new System.Drawing.Size(130, 19);
            this.txtOtherCD.TabIndex = 7;
            this.txtOtherCD.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_Label6
            // 
            this.ckM_Label6.AutoSize = true;
            this.ckM_Label6.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label6.DefaultlabelSize = true;
            this.ckM_Label6.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label6.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label6.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label6.Location = new System.Drawing.Point(55, 103);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(54, 12);
            this.ckM_Label6.TabIndex = 15;
            this.ckM_Label6.Text = "YahooCD";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPonpareCD
            // 
            this.txtPonpareCD.AllowMinus = false;
            this.txtPonpareCD.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPonpareCD.BackColor = System.Drawing.Color.White;
            this.txtPonpareCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPonpareCD.ClientColor = System.Drawing.Color.White;
            this.txtPonpareCD.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtPonpareCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtPonpareCD.DecimalPlace = 0;
            this.txtPonpareCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtPonpareCD.IntegerPart = 0;
            this.txtPonpareCD.IsCorrectDate = true;
            this.txtPonpareCD.isEnterKeyDown = false;
            this.txtPonpareCD.isMaxLengthErr = false;
            this.txtPonpareCD.IsNumber = true;
            this.txtPonpareCD.IsShop = false;
            this.txtPonpareCD.Length = 20;
            this.txtPonpareCD.Location = new System.Drawing.Point(112, 203);
            this.txtPonpareCD.MaxLength = 20;
            this.txtPonpareCD.MoveNext = true;
            this.txtPonpareCD.Name = "txtPonpareCD";
            this.txtPonpareCD.Size = new System.Drawing.Size(130, 19);
            this.txtPonpareCD.TabIndex = 6;
            this.txtPonpareCD.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_Label7.Location = new System.Drawing.Point(64, 128);
            this.ckM_Label7.Name = "ckM_Label7";
            this.ckM_Label7.Size = new System.Drawing.Size(45, 12);
            this.ckM_Label7.TabIndex = 16;
            this.ckM_Label7.Text = "楽天CD";
            this.ckM_Label7.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtWowmaCD
            // 
            this.txtWowmaCD.AllowMinus = false;
            this.txtWowmaCD.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtWowmaCD.BackColor = System.Drawing.Color.White;
            this.txtWowmaCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtWowmaCD.ClientColor = System.Drawing.Color.White;
            this.txtWowmaCD.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtWowmaCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtWowmaCD.DecimalPlace = 0;
            this.txtWowmaCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtWowmaCD.IntegerPart = 0;
            this.txtWowmaCD.IsCorrectDate = true;
            this.txtWowmaCD.isEnterKeyDown = false;
            this.txtWowmaCD.isMaxLengthErr = false;
            this.txtWowmaCD.IsNumber = true;
            this.txtWowmaCD.IsShop = false;
            this.txtWowmaCD.Length = 20;
            this.txtWowmaCD.Location = new System.Drawing.Point(112, 175);
            this.txtWowmaCD.MaxLength = 20;
            this.txtWowmaCD.MoveNext = true;
            this.txtWowmaCD.Name = "txtWowmaCD";
            this.txtWowmaCD.Size = new System.Drawing.Size(130, 19);
            this.txtWowmaCD.TabIndex = 5;
            this.txtWowmaCD.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_Label8
            // 
            this.ckM_Label8.AutoSize = true;
            this.ckM_Label8.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label8.DefaultlabelSize = true;
            this.ckM_Label8.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label8.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label8.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label8.Location = new System.Drawing.Point(48, 153);
            this.ckM_Label8.Name = "ckM_Label8";
            this.ckM_Label8.Size = new System.Drawing.Size(61, 12);
            this.ckM_Label8.TabIndex = 17;
            this.ckM_Label8.Text = "AmazonCD";
            this.ckM_Label8.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAmazonCD
            // 
            this.txtAmazonCD.AllowMinus = false;
            this.txtAmazonCD.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtAmazonCD.BackColor = System.Drawing.Color.White;
            this.txtAmazonCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAmazonCD.ClientColor = System.Drawing.Color.White;
            this.txtAmazonCD.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtAmazonCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtAmazonCD.DecimalPlace = 0;
            this.txtAmazonCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtAmazonCD.IntegerPart = 0;
            this.txtAmazonCD.IsCorrectDate = true;
            this.txtAmazonCD.isEnterKeyDown = false;
            this.txtAmazonCD.isMaxLengthErr = false;
            this.txtAmazonCD.IsNumber = true;
            this.txtAmazonCD.IsShop = false;
            this.txtAmazonCD.Length = 20;
            this.txtAmazonCD.Location = new System.Drawing.Point(112, 150);
            this.txtAmazonCD.MaxLength = 20;
            this.txtAmazonCD.MoveNext = true;
            this.txtAmazonCD.Name = "txtAmazonCD";
            this.txtAmazonCD.Size = new System.Drawing.Size(130, 19);
            this.txtAmazonCD.TabIndex = 4;
            this.txtAmazonCD.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_Label9
            // 
            this.ckM_Label9.AutoSize = true;
            this.ckM_Label9.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label9.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label9.DefaultlabelSize = true;
            this.ckM_Label9.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label9.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label9.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label9.Location = new System.Drawing.Point(55, 178);
            this.ckM_Label9.Name = "ckM_Label9";
            this.ckM_Label9.Size = new System.Drawing.Size(54, 12);
            this.ckM_Label9.TabIndex = 18;
            this.ckM_Label9.Text = "WowmaCD";
            this.ckM_Label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRakutenCD
            // 
            this.txtRakutenCD.AllowMinus = false;
            this.txtRakutenCD.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtRakutenCD.BackColor = System.Drawing.Color.White;
            this.txtRakutenCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRakutenCD.ClientColor = System.Drawing.Color.White;
            this.txtRakutenCD.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtRakutenCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtRakutenCD.DecimalPlace = 0;
            this.txtRakutenCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtRakutenCD.IntegerPart = 0;
            this.txtRakutenCD.IsCorrectDate = true;
            this.txtRakutenCD.isEnterKeyDown = false;
            this.txtRakutenCD.isMaxLengthErr = false;
            this.txtRakutenCD.IsNumber = true;
            this.txtRakutenCD.IsShop = false;
            this.txtRakutenCD.Length = 20;
            this.txtRakutenCD.Location = new System.Drawing.Point(112, 125);
            this.txtRakutenCD.MaxLength = 20;
            this.txtRakutenCD.MoveNext = true;
            this.txtRakutenCD.Name = "txtRakutenCD";
            this.txtRakutenCD.Size = new System.Drawing.Size(130, 19);
            this.txtRakutenCD.TabIndex = 3;
            this.txtRakutenCD.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_Label10
            // 
            this.ckM_Label10.AutoSize = true;
            this.ckM_Label10.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label10.DefaultlabelSize = true;
            this.ckM_Label10.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label10.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label10.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label10.Location = new System.Drawing.Point(41, 206);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(68, 12);
            this.ckM_Label10.TabIndex = 20;
            this.ckM_Label10.Text = "PonpareCD";
            this.ckM_Label10.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboIdentity
            // 
            this.cboIdentity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboIdentity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboIdentity.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.識別;
            this.cboIdentity.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboIdentity.Flag = 0;
            this.cboIdentity.FormattingEnabled = true;
            this.cboIdentity.Length = 20;
            this.cboIdentity.Location = new System.Drawing.Point(112, 43);
            this.cboIdentity.MaxLength = 20;
            this.cboIdentity.MoveNext = true;
            this.cboIdentity.Name = "cboIdentity";
            this.cboIdentity.Size = new System.Drawing.Size(180, 20);
            this.cboIdentity.TabIndex = 1;
            // 
            // ckM_Label11
            // 
            this.ckM_Label11.AutoSize = true;
            this.ckM_Label11.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label11.DefaultlabelSize = true;
            this.ckM_Label11.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label11.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label11.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label11.Location = new System.Drawing.Point(51, 232);
            this.ckM_Label11.Name = "ckM_Label11";
            this.ckM_Label11.Size = new System.Drawing.Size(58, 12);
            this.ckM_Label11.TabIndex = 21;
            this.ckM_Label11.Text = "その他CD";
            this.ckM_Label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtYahooCD
            // 
            this.txtYahooCD.AllowMinus = false;
            this.txtYahooCD.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtYahooCD.BackColor = System.Drawing.Color.White;
            this.txtYahooCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYahooCD.ClientColor = System.Drawing.Color.White;
            this.txtYahooCD.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtYahooCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtYahooCD.DecimalPlace = 0;
            this.txtYahooCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtYahooCD.IntegerPart = 0;
            this.txtYahooCD.IsCorrectDate = true;
            this.txtYahooCD.isEnterKeyDown = false;
            this.txtYahooCD.isMaxLengthErr = false;
            this.txtYahooCD.IsNumber = true;
            this.txtYahooCD.IsShop = false;
            this.txtYahooCD.Length = 20;
            this.txtYahooCD.Location = new System.Drawing.Point(112, 100);
            this.txtYahooCD.MaxLength = 20;
            this.txtYahooCD.MoveNext = true;
            this.txtYahooCD.Name = "txtYahooCD";
            this.txtYahooCD.Size = new System.Drawing.Size(130, 19);
            this.txtYahooCD.TabIndex = 2;
            this.txtYahooCD.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_Label12
            // 
            this.ckM_Label12.AutoSize = true;
            this.ckM_Label12.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label12.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label12.DefaultlabelSize = true;
            this.ckM_Label12.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label12.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label12.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label12.Location = new System.Drawing.Point(65, 279);
            this.ckM_Label12.Name = "ckM_Label12";
            this.ckM_Label12.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label12.TabIndex = 22;
            this.ckM_Label12.Text = "備　考";
            this.ckM_Label12.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtShippingName
            // 
            this.txtShippingName.AllowMinus = false;
            this.txtShippingName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtShippingName.BackColor = System.Drawing.Color.White;
            this.txtShippingName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShippingName.ClientColor = System.Drawing.Color.White;
            this.txtShippingName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtShippingName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtShippingName.DecimalPlace = 0;
            this.txtShippingName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtShippingName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtShippingName.IntegerPart = 0;
            this.txtShippingName.IsCorrectDate = true;
            this.txtShippingName.isEnterKeyDown = false;
            this.txtShippingName.isMaxLengthErr = false;
            this.txtShippingName.IsNumber = true;
            this.txtShippingName.IsShop = false;
            this.txtShippingName.Length = 20;
            this.txtShippingName.Location = new System.Drawing.Point(112, 17);
            this.txtShippingName.MaxLength = 20;
            this.txtShippingName.MoveNext = true;
            this.txtShippingName.Name = "txtShippingName";
            this.txtShippingName.Size = new System.Drawing.Size(200, 19);
            this.txtShippingName.TabIndex = 0;
            this.txtShippingName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // cboNormalType
            // 
            this.cboNormalType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboNormalType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboNormalType.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.Default;
            this.cboNormalType.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboNormalType.Flag = 0;
            this.cboNormalType.FormattingEnabled = true;
            this.cboNormalType.Length = 10;
            this.cboNormalType.Location = new System.Drawing.Point(112, 69);
            this.cboNormalType.MoveNext = true;
            this.cboNormalType.Name = "cboNormalType";
            this.cboNormalType.Size = new System.Drawing.Size(121, 20);
            this.cboNormalType.TabIndex = 23;
            // 
            // ckM_Label13
            // 
            this.ckM_Label13.AutoSize = true;
            this.ckM_Label13.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label13.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label13.DefaultlabelSize = true;
            this.ckM_Label13.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label13.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label13.Location = new System.Drawing.Point(65, 73);
            this.ckM_Label13.Name = "ckM_Label13";
            this.ckM_Label13.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label13.TabIndex = 24;
            this.ckM_Label13.Text = "種　別";
            this.ckM_Label13.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmMasterTouroku_UnsouGaisya
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1523, 961);
            this.Controls.Add(this.PanelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "FrmMasterTouroku_UnsouGaisya";
            this.Text = "MasterTouroku_UnsouGaisya";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmMasterTouroku_UnsouGaisya_KeyUp);
            this.Controls.SetChildIndex(this.PanelDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelSearch.ResumeLayout(false);
            this.PanelNormal.ResumeLayout(false);
            this.PanelNormal.PerformLayout();
            this.PanelCopy.ResumeLayout(false);
            this.PanelCopy.PerformLayout();
            this.PanelDetail.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PanelCopy;
        private System.Windows.Forms.Panel PanelNormal;
        private CKM_Controls.CKM_Button btnDisplay;
        private System.Windows.Forms.Panel PanelDetail;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_Label 運送会社CD;
        private CKM_Controls.CKM_CheckBox chkDeleteFlg;
        private CKM_Controls.CKM_TextBox txtOtherCD;
        private CKM_Controls.CKM_TextBox txtPonpareCD;
        private CKM_Controls.CKM_TextBox txtWowmaCD;
        private CKM_Controls.CKM_TextBox txtAmazonCD;
        private CKM_Controls.CKM_TextBox txtRakutenCD;
        private CKM_Controls.CKM_ComboBox cboIdentity;
        private CKM_Controls.CKM_TextBox txtYahooCD;
        private CKM_Controls.CKM_TextBox txtShippingName;
        private CKM_Controls.CKM_Label ckM_Label12;
        private CKM_Controls.CKM_Label ckM_Label11;
        private CKM_Controls.CKM_Label ckM_Label10;
        private CKM_Controls.CKM_Label ckM_Label9;
        private CKM_Controls.CKM_Label ckM_Label8;
        private CKM_Controls.CKM_Label ckM_Label7;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_MultiLineTextBox txtremark;
        private Search.CKM_SearchControl ScShippingCD;
        private Search.CKM_SearchControl ScCopyShippingCD;
        private System.Windows.Forms.Panel panel1;
        private CKM_Controls.CKM_Label ckM_Label13;
        private CKM_Controls.CKM_ComboBox cboNormalType;
    }
}

