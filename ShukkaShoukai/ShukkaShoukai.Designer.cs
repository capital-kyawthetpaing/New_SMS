namespace ShukkaShoukai
{
    partial class FrmShukkaShoukai
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.cboWarehouse = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.txtShippingStartDate = new CKM_Controls.CKM_TextBox();
            this.txtShippingEndDate = new CKM_Controls.CKM_TextBox();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.chkShipmentOfSale = new CKM_Controls.CKM_CheckBox();
            this.chkShipmentOfTransfer = new CKM_Controls.CKM_CheckBox();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.chkAlready = new CKM_Controls.CKM_CheckBox();
            this.chkNot = new CKM_Controls.CKM_CheckBox();
            this.ckM_Label7 = new CKM_Controls.CKM_Label();
            this.txtProductName = new CKM_Controls.CKM_TextBox();
            this.ckM_Label12 = new CKM_Controls.CKM_Label();
            this.ckM_Label11 = new CKM_Controls.CKM_Label();
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.ckM_Label9 = new CKM_Controls.CKM_Label();
            this.cboDestinationWarehouse = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label8 = new CKM_Controls.CKM_Label();
            this.btnDisplay = new CKM_Controls.CKM_Button();
            this.dgvShukkaShoukai = new CKM_Controls.CKM_GridView();
            this.colShippingN0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMovement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShippingDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeliveryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJanCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColorSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCommentOutStore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShippingSu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrderNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCarrierName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSalesDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStaffName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelDetail = new System.Windows.Forms.Panel();
            this.cboShipping = new CKM_Controls.CKM_ComboBox();
            this.SC_Order = new Search.CKM_SearchControl();
            this.SC_JanCD = new Search.CKM_SearchControl();
            this.Sc_SKUCD = new Search.CKM_SearchControl();
            this.Sc_Item = new Search.CKM_SearchControl();
            this.PanelHeader.SuspendLayout();
            this.PanelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShukkaShoukai)).BeginInit();
            this.panelDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.cboShipping);
            this.PanelHeader.Controls.Add(this.txtProductName);
            this.PanelHeader.Controls.Add(this.ckM_Label12);
            this.PanelHeader.Controls.Add(this.SC_Order);
            this.PanelHeader.Controls.Add(this.chkAlready);
            this.PanelHeader.Controls.Add(this.SC_JanCD);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Controls.Add(this.ckM_Label11);
            this.PanelHeader.Controls.Add(this.cboWarehouse);
            this.PanelHeader.Controls.Add(this.chkNot);
            this.PanelHeader.Controls.Add(this.Sc_SKUCD);
            this.PanelHeader.Controls.Add(this.ckM_Label6);
            this.PanelHeader.Controls.Add(this.ckM_Label10);
            this.PanelHeader.Controls.Add(this.ckM_Label3);
            this.PanelHeader.Controls.Add(this.ckM_Label7);
            this.PanelHeader.Controls.Add(this.Sc_Item);
            this.PanelHeader.Controls.Add(this.chkShipmentOfTransfer);
            this.PanelHeader.Controls.Add(this.txtShippingStartDate);
            this.PanelHeader.Controls.Add(this.ckM_Label9);
            this.PanelHeader.Controls.Add(this.chkShipmentOfSale);
            this.PanelHeader.Controls.Add(this.ckM_Label4);
            this.PanelHeader.Controls.Add(this.ckM_Label8);
            this.PanelHeader.Controls.Add(this.ckM_Label2);
            this.PanelHeader.Controls.Add(this.ckM_Label5);
            this.PanelHeader.Controls.Add(this.txtShippingEndDate);
            this.PanelHeader.Controls.Add(this.cboDestinationWarehouse);
            this.PanelHeader.Size = new System.Drawing.Size(1711, 194);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.Controls.SetChildIndex(this.cboDestinationWarehouse, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtShippingEndDate, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label5, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label8, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label4, 0);
            this.PanelHeader.Controls.SetChildIndex(this.chkShipmentOfSale, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label9, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtShippingStartDate, 0);
            this.PanelHeader.Controls.SetChildIndex(this.chkShipmentOfTransfer, 0);
            this.PanelHeader.Controls.SetChildIndex(this.Sc_Item, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label7, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label3, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label10, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label6, 0);
            this.PanelHeader.Controls.SetChildIndex(this.Sc_SKUCD, 0);
            this.PanelHeader.Controls.SetChildIndex(this.chkNot, 0);
            this.PanelHeader.Controls.SetChildIndex(this.cboWarehouse, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label11, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.SC_JanCD, 0);
            this.PanelHeader.Controls.SetChildIndex(this.chkAlready, 0);
            this.PanelHeader.Controls.SetChildIndex(this.SC_Order, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label12, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtProductName, 0);
            this.PanelHeader.Controls.SetChildIndex(this.cboShipping, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Controls.Add(this.btnDisplay);
            this.PanelSearch.Location = new System.Drawing.Point(1177, 0);
            this.PanelSearch.TabIndex = 0;
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
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
            this.ckM_Label1.Location = new System.Drawing.Point(68, 14);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label1.TabIndex = 23;
            this.ckM_Label1.Text = "倉庫";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboWarehouse
            // 
            this.cboWarehouse.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboWarehouse.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboWarehouse.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.倉庫;
            this.cboWarehouse.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboWarehouse.Flag = 0;
            this.cboWarehouse.FormattingEnabled = true;
            this.cboWarehouse.Length = 10;
            this.cboWarehouse.Location = new System.Drawing.Point(102, 10);
            this.cboWarehouse.MaxLength = 10;
            this.cboWarehouse.MoveNext = true;
            this.cboWarehouse.Name = "cboWarehouse";
            this.cboWarehouse.Size = new System.Drawing.Size(265, 20);
            this.cboWarehouse.TabIndex = 1;
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
            this.ckM_Label2.Location = new System.Drawing.Point(995, 16);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label2.TabIndex = 27;
            this.ckM_Label2.Text = "運送会社";
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
            this.ckM_Label3.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label3.Location = new System.Drawing.Point(55, 43);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label3.TabIndex = 27;
            this.ckM_Label3.Text = "出荷日";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtShippingStartDate
            // 
            this.txtShippingStartDate.AllowMinus = false;
            this.txtShippingStartDate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtShippingStartDate.BackColor = System.Drawing.Color.White;
            this.txtShippingStartDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShippingStartDate.ClientColor = System.Drawing.Color.White;
            this.txtShippingStartDate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtShippingStartDate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtShippingStartDate.DecimalPlace = 0;
            this.txtShippingStartDate.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtShippingStartDate.IntegerPart = 0;
            this.txtShippingStartDate.IsCorrectDate = true;
            this.txtShippingStartDate.isEnterKeyDown = false;
            this.txtShippingStartDate.isMaxLengthErr = false;
            this.txtShippingStartDate.IsNumber = true;
            this.txtShippingStartDate.IsShop = false;
            this.txtShippingStartDate.Length = 8;
            this.txtShippingStartDate.Location = new System.Drawing.Point(102, 40);
            this.txtShippingStartDate.MaxLength = 8;
            this.txtShippingStartDate.MoveNext = true;
            this.txtShippingStartDate.Name = "txtShippingStartDate";
            this.txtShippingStartDate.Size = new System.Drawing.Size(100, 19);
            this.txtShippingStartDate.TabIndex = 2;
            this.txtShippingStartDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtShippingStartDate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // txtShippingEndDate
            // 
            this.txtShippingEndDate.AllowMinus = false;
            this.txtShippingEndDate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtShippingEndDate.BackColor = System.Drawing.Color.White;
            this.txtShippingEndDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShippingEndDate.ClientColor = System.Drawing.Color.White;
            this.txtShippingEndDate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtShippingEndDate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtShippingEndDate.DecimalPlace = 0;
            this.txtShippingEndDate.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtShippingEndDate.IntegerPart = 0;
            this.txtShippingEndDate.IsCorrectDate = true;
            this.txtShippingEndDate.isEnterKeyDown = false;
            this.txtShippingEndDate.isMaxLengthErr = false;
            this.txtShippingEndDate.IsNumber = true;
            this.txtShippingEndDate.IsShop = false;
            this.txtShippingEndDate.Length = 8;
            this.txtShippingEndDate.Location = new System.Drawing.Point(266, 40);
            this.txtShippingEndDate.MaxLength = 8;
            this.txtShippingEndDate.MoveNext = true;
            this.txtShippingEndDate.Name = "txtShippingEndDate";
            this.txtShippingEndDate.Size = new System.Drawing.Size(100, 19);
            this.txtShippingEndDate.TabIndex = 3;
            this.txtShippingEndDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtShippingEndDate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtShippingEndDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtShippingEndDate_KeyDown);
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
            this.ckM_Label4.Location = new System.Drawing.Point(226, 43);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label4.TabIndex = 23;
            this.ckM_Label4.Text = "～";
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
            this.ckM_Label5.Location = new System.Drawing.Point(42, 67);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label5.TabIndex = 22;
            this.ckM_Label5.Text = "表示対象";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkShipmentOfSale
            // 
            this.chkShipmentOfSale.AutoSize = true;
            this.chkShipmentOfSale.Checked = true;
            this.chkShipmentOfSale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShipmentOfSale.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkShipmentOfSale.Location = new System.Drawing.Point(103, 67);
            this.chkShipmentOfSale.Name = "chkShipmentOfSale";
            this.chkShipmentOfSale.Size = new System.Drawing.Size(102, 16);
            this.chkShipmentOfSale.TabIndex = 4;
            this.chkShipmentOfSale.Text = "販売分の出荷";
            this.chkShipmentOfSale.UseVisualStyleBackColor = true;
            // 
            // chkShipmentOfTransfer
            // 
            this.chkShipmentOfTransfer.AutoSize = true;
            this.chkShipmentOfTransfer.Checked = true;
            this.chkShipmentOfTransfer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShipmentOfTransfer.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkShipmentOfTransfer.Location = new System.Drawing.Point(103, 89);
            this.chkShipmentOfTransfer.Name = "chkShipmentOfTransfer";
            this.chkShipmentOfTransfer.Size = new System.Drawing.Size(102, 16);
            this.chkShipmentOfTransfer.TabIndex = 5;
            this.chkShipmentOfTransfer.Text = "移動分の出荷";
            this.chkShipmentOfTransfer.UseVisualStyleBackColor = true;
            this.chkShipmentOfTransfer.CheckedChanged += new System.EventHandler(this.chkShipmentOfTransfer_CheckedChanged);
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
            this.ckM_Label6.Location = new System.Drawing.Point(64, 122);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label6.TabIndex = 26;
            this.ckM_Label6.Text = "売上";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkAlready
            // 
            this.chkAlready.AutoSize = true;
            this.chkAlready.Checked = true;
            this.chkAlready.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAlready.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkAlready.Location = new System.Drawing.Point(103, 119);
            this.chkAlready.Name = "chkAlready";
            this.chkAlready.Size = new System.Drawing.Size(37, 16);
            this.chkAlready.TabIndex = 8;
            this.chkAlready.Text = "済";
            this.chkAlready.UseVisualStyleBackColor = true;
            // 
            // chkNot
            // 
            this.chkNot.AutoSize = true;
            this.chkNot.Checked = true;
            this.chkNot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNot.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkNot.Location = new System.Drawing.Point(145, 119);
            this.chkNot.Name = "chkNot";
            this.chkNot.Size = new System.Drawing.Size(37, 16);
            this.chkNot.TabIndex = 9;
            this.chkNot.Text = "未";
            this.chkNot.UseVisualStyleBackColor = true;
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
            this.ckM_Label7.Location = new System.Drawing.Point(372, 69);
            this.ckM_Label7.Name = "ckM_Label7";
            this.ckM_Label7.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label7.TabIndex = 24;
            this.ckM_Label7.Text = "受注番号";
            this.ckM_Label7.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProductName
            // 
            this.txtProductName.AllowMinus = false;
            this.txtProductName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtProductName.BackColor = System.Drawing.Color.White;
            this.txtProductName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProductName.ClientColor = System.Drawing.Color.White;
            this.txtProductName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtProductName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtProductName.DecimalPlace = 0;
            this.txtProductName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtProductName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtProductName.IntegerPart = 0;
            this.txtProductName.IsCorrectDate = true;
            this.txtProductName.isEnterKeyDown = false;
            this.txtProductName.isMaxLengthErr = false;
            this.txtProductName.IsNumber = true;
            this.txtProductName.IsShop = false;
            this.txtProductName.Length = 80;
            this.txtProductName.Location = new System.Drawing.Point(1056, 121);
            this.txtProductName.MaxLength = 80;
            this.txtProductName.MoveNext = true;
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(500, 19);
            this.txtProductName.TabIndex = 14;
            this.txtProductName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_Label12.Location = new System.Drawing.Point(1009, 125);
            this.ckM_Label12.Name = "ckM_Label12";
            this.ckM_Label12.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label12.TabIndex = 22;
            this.ckM_Label12.Text = "商品名";
            this.ckM_Label12.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label11.Location = new System.Drawing.Point(1013, 97);
            this.ckM_Label11.Name = "ckM_Label11";
            this.ckM_Label11.Size = new System.Drawing.Size(40, 12);
            this.ckM_Label11.TabIndex = 21;
            this.ckM_Label11.Text = "JANCD";
            this.ckM_Label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label10.Location = new System.Drawing.Point(1013, 67);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(40, 12);
            this.ckM_Label10.TabIndex = 20;
            this.ckM_Label10.Text = "SKUCD";
            this.ckM_Label10.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label9.Location = new System.Drawing.Point(1019, 41);
            this.ckM_Label9.Name = "ckM_Label9";
            this.ckM_Label9.Size = new System.Drawing.Size(33, 12);
            this.ckM_Label9.TabIndex = 19;
            this.ckM_Label9.Text = "ITEM";
            this.ckM_Label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboDestinationWarehouse
            // 
            this.cboDestinationWarehouse.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboDestinationWarehouse.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDestinationWarehouse.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.倉庫;
            this.cboDestinationWarehouse.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboDestinationWarehouse.Flag = 0;
            this.cboDestinationWarehouse.FormattingEnabled = true;
            this.cboDestinationWarehouse.Length = 10;
            this.cboDestinationWarehouse.Location = new System.Drawing.Point(430, 92);
            this.cboDestinationWarehouse.MaxLength = 10;
            this.cboDestinationWarehouse.MoveNext = true;
            this.cboDestinationWarehouse.Name = "cboDestinationWarehouse";
            this.cboDestinationWarehouse.Size = new System.Drawing.Size(265, 20);
            this.cboDestinationWarehouse.TabIndex = 7;
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
            this.ckM_Label8.Location = new System.Drawing.Point(357, 95);
            this.ckM_Label8.Name = "ckM_Label8";
            this.ckM_Label8.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label8.TabIndex = 25;
            this.ckM_Label8.Text = "移動先倉庫";
            this.ckM_Label8.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.btnDisplay.Location = new System.Drawing.Point(394, 3);
            this.btnDisplay.Margin = new System.Windows.Forms.Padding(1);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(118, 28);
            this.btnDisplay.TabIndex = 15;
            this.btnDisplay.Text = "表示(F11)";
            this.btnDisplay.UseVisualStyleBackColor = false;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // dgvShukkaShoukai
            // 
            this.dgvShukkaShoukai.AllowUserToAddRows = false;
            this.dgvShukkaShoukai.AllowUserToDeleteRows = false;
            this.dgvShukkaShoukai.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.dgvShukkaShoukai.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvShukkaShoukai.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvShukkaShoukai.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvShukkaShoukai.ColumnHeadersHeight = 25;
            this.dgvShukkaShoukai.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colShippingN0,
            this.colMovement,
            this.colShippingDate,
            this.colDeliveryName,
            this.colSKUCD,
            this.colJanCD,
            this.colSKUName,
            this.colColorSize,
            this.colCommentOutStore,
            this.colShippingSu,
            this.colOrderNumber,
            this.colCarrierName,
            this.colSalesDate,
            this.colStaffName});
            this.dgvShukkaShoukai.EnableHeadersVisualStyles = false;
            this.dgvShukkaShoukai.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvShukkaShoukai.Location = new System.Drawing.Point(29, 23);
            this.dgvShukkaShoukai.Name = "dgvShukkaShoukai";
            this.dgvShukkaShoukai.RowHeight_ = 20;
            this.dgvShukkaShoukai.RowTemplate.Height = 20;
            this.dgvShukkaShoukai.Size = new System.Drawing.Size(1600, 500);
            this.dgvShukkaShoukai.TabIndex = 13;
            this.dgvShukkaShoukai.UseRowNo = true;
            this.dgvShukkaShoukai.UseSetting = true;
            // 
            // colShippingN0
            // 
            this.colShippingN0.DataPropertyName = "ShippingNO";
            this.colShippingN0.HeaderText = "出荷番号";
            this.colShippingN0.MaxInputLength = 11;
            this.colShippingN0.Name = "colShippingN0";
            this.colShippingN0.Width = 110;
            // 
            // colMovement
            // 
            this.colMovement.DataPropertyName = "Movement";
            this.colMovement.HeaderText = "移動区分";
            this.colMovement.MaxInputLength = 10;
            this.colMovement.Name = "colMovement";
            // 
            // colShippingDate
            // 
            this.colShippingDate.DataPropertyName = "ShippingDate";
            this.colShippingDate.HeaderText = "出荷日";
            this.colShippingDate.MaxInputLength = 8;
            this.colShippingDate.Name = "colShippingDate";
            this.colShippingDate.Width = 80;
            // 
            // colDeliveryName
            // 
            this.colDeliveryName.DataPropertyName = "DeliveryName";
            this.colDeliveryName.HeaderText = "出荷先";
            this.colDeliveryName.MaxInputLength = 40;
            this.colDeliveryName.Name = "colDeliveryName";
            this.colDeliveryName.Width = 400;
            // 
            // colSKUCD
            // 
            this.colSKUCD.DataPropertyName = "SKUCD";
            this.colSKUCD.HeaderText = "SKUCD";
            this.colSKUCD.MaxInputLength = 30;
            this.colSKUCD.Name = "colSKUCD";
            this.colSKUCD.Width = 300;
            // 
            // colJanCD
            // 
            this.colJanCD.DataPropertyName = "JanCD";
            this.colJanCD.HeaderText = "JANCD";
            this.colJanCD.MaxInputLength = 13;
            this.colJanCD.Name = "colJanCD";
            this.colJanCD.Width = 130;
            // 
            // colSKUName
            // 
            this.colSKUName.DataPropertyName = "SKUName";
            this.colSKUName.HeaderText = "商品名";
            this.colSKUName.MaxInputLength = 80;
            this.colSKUName.Name = "colSKUName";
            this.colSKUName.Width = 800;
            // 
            // colColorSize
            // 
            this.colColorSize.DataPropertyName = "ColorSize";
            this.colColorSize.HeaderText = "カラー.サイズ";
            this.colColorSize.MaxInputLength = 40;
            this.colColorSize.Name = "colColorSize";
            this.colColorSize.Width = 400;
            // 
            // colCommentOutStore
            // 
            this.colCommentOutStore.DataPropertyName = "CommentOutStore";
            this.colCommentOutStore.HeaderText = "備考";
            this.colCommentOutStore.MaxInputLength = 60;
            this.colCommentOutStore.Name = "colCommentOutStore";
            this.colCommentOutStore.Width = 600;
            // 
            // colShippingSu
            // 
            this.colShippingSu.DataPropertyName = "ShippingSu";
            this.colShippingSu.HeaderText = "出荷数";
            this.colShippingSu.MaxInputLength = 6;
            this.colShippingSu.Name = "colShippingSu";
            this.colShippingSu.Width = 60;
            // 
            // colOrderNumber
            // 
            this.colOrderNumber.DataPropertyName = "OrderNumber";
            this.colOrderNumber.HeaderText = "受注番号";
            this.colOrderNumber.MaxInputLength = 11;
            this.colOrderNumber.Name = "colOrderNumber";
            this.colOrderNumber.Width = 110;
            // 
            // colCarrierName
            // 
            this.colCarrierName.DataPropertyName = "CarrierName";
            this.colCarrierName.HeaderText = "運送会社";
            this.colCarrierName.MaxInputLength = 20;
            this.colCarrierName.Name = "colCarrierName";
            this.colCarrierName.Width = 200;
            // 
            // colSalesDate
            // 
            this.colSalesDate.DataPropertyName = "SalesDate";
            this.colSalesDate.HeaderText = "売上日";
            this.colSalesDate.MaxInputLength = 8;
            this.colSalesDate.Name = "colSalesDate";
            this.colSalesDate.Width = 80;
            // 
            // colStaffName
            // 
            this.colStaffName.DataPropertyName = "StaffName";
            this.colStaffName.HeaderText = "入力スタッフ";
            this.colStaffName.MaxInputLength = 20;
            this.colStaffName.Name = "colStaffName";
            this.colStaffName.Width = 200;
            // 
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.dgvShukkaShoukai);
            this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetail.Location = new System.Drawing.Point(0, 250);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(1713, 679);
            this.panelDetail.TabIndex = 13;
            // 
            // cboShipping
            // 
            this.cboShipping.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboShipping.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboShipping.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.配送会社;
            this.cboShipping.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboShipping.Flag = 0;
            this.cboShipping.FormattingEnabled = true;
            this.cboShipping.Length = 10;
            this.cboShipping.Location = new System.Drawing.Point(1056, 10);
            this.cboShipping.MaxLength = 10;
            this.cboShipping.MoveNext = true;
            this.cboShipping.Name = "cboShipping";
            this.cboShipping.Size = new System.Drawing.Size(121, 20);
            this.cboShipping.TabIndex = 10;
            // 
            // SC_Order
            // 
            this.SC_Order.AutoSize = true;
            this.SC_Order.ChangeDate = "";
            this.SC_Order.ChangeDateWidth = 100;
            this.SC_Order.Code = "";
            this.SC_Order.CodeWidth = 100;
            this.SC_Order.CodeWidth1 = 100;
            this.SC_Order.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.SC_Order.DataCheck = false;
            this.SC_Order.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.SC_Order.IsCopy = false;
            this.SC_Order.LabelText = "";
            this.SC_Order.LabelVisible = false;
            this.SC_Order.Location = new System.Drawing.Point(430, 60);
            this.SC_Order.Margin = new System.Windows.Forms.Padding(0);
            this.SC_Order.Name = "SC_Order";
            this.SC_Order.NameWidth = 600;
            this.SC_Order.SearchEnable = true;
            this.SC_Order.Size = new System.Drawing.Size(133, 28);
            this.SC_Order.Stype = Search.CKM_SearchControl.SearchType.受注番号;
            this.SC_Order.TabIndex = 6;
            this.SC_Order.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.SC_Order.UseChangeDate = false;
            this.SC_Order.Value1 = null;
            this.SC_Order.Value2 = null;
            this.SC_Order.Value3 = null;
            this.SC_Order.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.SC_Order_CodeKeyDownEvent);
            // 
            // SC_JanCD
            // 
            this.SC_JanCD.AutoSize = true;
            this.SC_JanCD.ChangeDate = "";
            this.SC_JanCD.ChangeDateWidth = 100;
            this.SC_JanCD.Code = "";
            this.SC_JanCD.CodeWidth = 600;
            this.SC_JanCD.CodeWidth1 = 600;
            this.SC_JanCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.SC_JanCD.DataCheck = false;
            this.SC_JanCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.SC_JanCD.IsCopy = false;
            this.SC_JanCD.LabelText = "";
            this.SC_JanCD.LabelVisible = false;
            this.SC_JanCD.Location = new System.Drawing.Point(1056, 88);
            this.SC_JanCD.Margin = new System.Windows.Forms.Padding(0);
            this.SC_JanCD.Name = "SC_JanCD";
            this.SC_JanCD.NameWidth = 280;
            this.SC_JanCD.SearchEnable = true;
            this.SC_JanCD.Size = new System.Drawing.Size(633, 27);
            this.SC_JanCD.Stype = Search.CKM_SearchControl.SearchType.JANMulti;
            this.SC_JanCD.TabIndex = 13;
            this.SC_JanCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.SC_JanCD.UseChangeDate = false;
            this.SC_JanCD.Value1 = null;
            this.SC_JanCD.Value2 = null;
            this.SC_JanCD.Value3 = null;
            // 
            // Sc_SKUCD
            // 
            this.Sc_SKUCD.AutoSize = true;
            this.Sc_SKUCD.ChangeDate = "";
            this.Sc_SKUCD.ChangeDateWidth = 100;
            this.Sc_SKUCD.Code = "";
            this.Sc_SKUCD.CodeWidth = 600;
            this.Sc_SKUCD.CodeWidth1 = 600;
            this.Sc_SKUCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.Sc_SKUCD.DataCheck = false;
            this.Sc_SKUCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.Sc_SKUCD.IsCopy = false;
            this.Sc_SKUCD.LabelText = "";
            this.Sc_SKUCD.LabelVisible = false;
            this.Sc_SKUCD.Location = new System.Drawing.Point(1056, 59);
            this.Sc_SKUCD.Margin = new System.Windows.Forms.Padding(0);
            this.Sc_SKUCD.Name = "Sc_SKUCD";
            this.Sc_SKUCD.NameWidth = 350;
            this.Sc_SKUCD.SearchEnable = true;
            this.Sc_SKUCD.Size = new System.Drawing.Size(633, 27);
            this.Sc_SKUCD.Stype = Search.CKM_SearchControl.SearchType.SKUCD;
            this.Sc_SKUCD.TabIndex = 12;
            this.Sc_SKUCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.Sc_SKUCD.UseChangeDate = false;
            this.Sc_SKUCD.Value1 = null;
            this.Sc_SKUCD.Value2 = null;
            this.Sc_SKUCD.Value3 = null;
            // 
            // Sc_Item
            // 
            this.Sc_Item.AutoSize = true;
            this.Sc_Item.ChangeDate = "";
            this.Sc_Item.ChangeDateWidth = 100;
            this.Sc_Item.Code = "";
            this.Sc_Item.CodeWidth = 600;
            this.Sc_Item.CodeWidth1 = 600;
            this.Sc_Item.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.Sc_Item.DataCheck = false;
            this.Sc_Item.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.Sc_Item.IsCopy = false;
            this.Sc_Item.LabelText = "";
            this.Sc_Item.LabelVisible = false;
            this.Sc_Item.Location = new System.Drawing.Point(1055, 33);
            this.Sc_Item.Margin = new System.Windows.Forms.Padding(0);
            this.Sc_Item.Name = "Sc_Item";
            this.Sc_Item.NameWidth = 350;
            this.Sc_Item.SearchEnable = true;
            this.Sc_Item.Size = new System.Drawing.Size(633, 27);
            this.Sc_Item.Stype = Search.CKM_SearchControl.SearchType.SKU_ITEM_CD;
            this.Sc_Item.TabIndex = 11;
            this.Sc_Item.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.Sc_Item.UseChangeDate = false;
            this.Sc_Item.Value1 = null;
            this.Sc_Item.Value2 = null;
            this.Sc_Item.Value3 = null;
            // 
            // FrmShukkaShoukai
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.panelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "FrmShukkaShoukai";
            this.PanelHeaderHeight = 250;
            this.Text = "ShukkaShoukai";
            this.Load += new System.EventHandler(this.FrmShukkaShoukai_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmShukkaShoukai_KeyUp);
            this.Controls.SetChildIndex(this.panelDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            this.PanelSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvShukkaShoukai)).EndInit();
            this.panelDetail.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_ComboBox cboWarehouse;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_TextBox txtShippingEndDate;
        private CKM_Controls.CKM_TextBox txtShippingStartDate;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_CheckBox chkNot;
        private CKM_Controls.CKM_CheckBox chkAlready;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_CheckBox chkShipmentOfTransfer;
        private CKM_Controls.CKM_CheckBox chkShipmentOfSale;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Label ckM_Label7;
        private Search.CKM_SearchControl SC_Order;
        private CKM_Controls.CKM_ComboBox cboDestinationWarehouse;
        private CKM_Controls.CKM_Label ckM_Label8;
        private Search.CKM_SearchControl Sc_SKUCD;
        private CKM_Controls.CKM_Label ckM_Label10;
        private Search.CKM_SearchControl Sc_Item;
        private CKM_Controls.CKM_Label ckM_Label9;
        private Search.CKM_SearchControl SC_JanCD;
        private CKM_Controls.CKM_Label ckM_Label11;
        private CKM_Controls.CKM_Label ckM_Label12;
        private CKM_Controls.CKM_TextBox txtProductName;
        private CKM_Controls.CKM_Button btnDisplay;
        private CKM_Controls.CKM_GridView dgvShukkaShoukai;
        private System.Windows.Forms.Panel panelDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtShipping;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtMovement;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtShippingDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtDeliveryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtSKUCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtJanCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtSKUName;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtColorSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtComment;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtShippingSu;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtOrderNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtCarrierName;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtSaleDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtStaffName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShippingN0;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMovement;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShippingDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeliveryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJanCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColorSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCommentOutStore;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShippingSu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCarrierName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStaffName;
        private CKM_Controls.CKM_ComboBox cboShipping;
    }
}

