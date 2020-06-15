namespace NyuukaShoukai
{
    partial class FrmNyuukaShoukai
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
            this.cboWarehouse = new CKM_Controls.CKM_ComboBox();
            this.statusChk1 = new CKM_Controls.CKM_CheckBox();
            this.statusChk2 = new CKM_Controls.CKM_CheckBox();
            this.txtDeliveryNote = new CKM_Controls.CKM_TextBox();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.ckM_Label7 = new CKM_Controls.CKM_Label();
            this.ckM_Label8 = new CKM_Controls.CKM_Label();
            this.ckM_Label9 = new CKM_Controls.CKM_Label();
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.ckM_Label11 = new CKM_Controls.CKM_Label();
            this.ckM_Label12 = new CKM_Controls.CKM_Label();
            this.ckM_Label13 = new CKM_Controls.CKM_Label();
            this.ckM_Label14 = new CKM_Controls.CKM_Label();
            this.chkDelivery = new CKM_Controls.CKM_CheckBox();
            this.ChkArrival = new CKM_Controls.CKM_CheckBox();
            this.ckM_Label15 = new CKM_Controls.CKM_Label();
            this.ckM_Label16 = new CKM_Controls.CKM_Label();
            this.ScItem = new Search.CKM_SearchControl();
            this.ScSKUCD = new Search.CKM_SearchControl();
            this.ScJanCD = new Search.CKM_SearchControl();
            this.txtProductName = new CKM_Controls.CKM_TextBox();
            this.ScSupplier = new Search.CKM_SearchControl();
            this.cboSourceWH = new CKM_Controls.CKM_ComboBox();
            this.dgvNyuukaShoukai = new CKM_Controls.CKM_GridView();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCalcuArrivalPlanDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchaseDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGoods = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJanCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSizeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArrivalPlanSu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArrivalSu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVendorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoukoName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDirectDelivery = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReserveNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArrivalNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchaseNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVendor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtArrivalDay1 = new CKM_Controls.CKM_TextBox();
            this.txtArrivalDay2 = new CKM_Controls.CKM_TextBox();
            this.txtStockDate1 = new CKM_Controls.CKM_TextBox();
            this.txtStockDate2 = new CKM_Controls.CKM_TextBox();
            this.txtPurchaseDate1 = new CKM_Controls.CKM_TextBox();
            this.txtPurchaseDate2 = new CKM_Controls.CKM_TextBox();
            this.btnDisplay = new CKM_Controls.CKM_Button();
            this.panelDetail = new System.Windows.Forms.Panel();
            this.panelcombo2 = new System.Windows.Forms.Panel();
            this.panelcombo1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNyuukaShoukai)).BeginInit();
            this.panelDetail.SuspendLayout();
            this.panelcombo2.SuspendLayout();
            this.panelcombo1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1782, 0);
            this.PanelHeader.TabIndex = 0;
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(1248, 0);
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
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
            this.cboWarehouse.Location = new System.Drawing.Point(60, 10);
            this.cboWarehouse.MaxLength = 10;
            this.cboWarehouse.MoveNext = true;
            this.cboWarehouse.Name = "cboWarehouse";
            this.cboWarehouse.Size = new System.Drawing.Size(121, 20);
            this.cboWarehouse.TabIndex = 0;
            this.cboWarehouse.SelectedIndexChanged += new System.EventHandler(this.cboWarehouse_SelectedIndexChanged);
            this.cboWarehouse.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboWarehouse_KeyDown);
            // 
            // statusChk1
            // 
            this.statusChk1.AutoSize = true;
            this.statusChk1.Checked = true;
            this.statusChk1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statusChk1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.statusChk1.Location = new System.Drawing.Point(142, 140);
            this.statusChk1.Name = "statusChk1";
            this.statusChk1.Size = new System.Drawing.Size(44, 16);
            this.statusChk1.TabIndex = 5;
            this.statusChk1.Text = " 済";
            this.statusChk1.UseVisualStyleBackColor = true;
            // 
            // statusChk2
            // 
            this.statusChk2.AutoSize = true;
            this.statusChk2.Checked = true;
            this.statusChk2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statusChk2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.statusChk2.Location = new System.Drawing.Point(205, 139);
            this.statusChk2.Name = "statusChk2";
            this.statusChk2.Size = new System.Drawing.Size(37, 16);
            this.statusChk2.TabIndex = 6;
            this.statusChk2.Text = "未";
            this.statusChk2.UseVisualStyleBackColor = true;
            // 
            // txtDeliveryNote
            // 
            this.txtDeliveryNote.AllowMinus = false;
            this.txtDeliveryNote.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtDeliveryNote.BackColor = System.Drawing.Color.White;
            this.txtDeliveryNote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDeliveryNote.ClientColor = System.Drawing.Color.White;
            this.txtDeliveryNote.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtDeliveryNote.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtDeliveryNote.DecimalPlace = 0;
            this.txtDeliveryNote.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtDeliveryNote.IntegerPart = 0;
            this.txtDeliveryNote.IsCorrectDate = true;
            this.txtDeliveryNote.isEnterKeyDown = false;
            this.txtDeliveryNote.isMaxLengthErr = false;
            this.txtDeliveryNote.IsNumber = true;
            this.txtDeliveryNote.IsShop = false;
            this.txtDeliveryNote.Length = 15;
            this.txtDeliveryNote.Location = new System.Drawing.Point(142, 215);
            this.txtDeliveryNote.MaxLength = 15;
            this.txtDeliveryNote.MoveNext = true;
            this.txtDeliveryNote.Name = "txtDeliveryNote";
            this.txtDeliveryNote.Size = new System.Drawing.Size(150, 19);
            this.txtDeliveryNote.TabIndex = 9;
            this.txtDeliveryNote.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_Label1.Location = new System.Drawing.Point(24, 14);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label1.TabIndex = 19;
            this.ckM_Label1.Text = "倉庫";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label2.Location = new System.Drawing.Point(95, 61);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label2.TabIndex = 20;
            this.ckM_Label2.Text = "入荷日";
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
            this.ckM_Label3.Location = new System.Drawing.Point(69, 103);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label3.TabIndex = 21;
            this.ckM_Label3.Text = "入荷予定日";
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
            this.ckM_Label4.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label4.Location = new System.Drawing.Point(82, 142);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label4.TabIndex = 22;
            this.ckM_Label4.Text = "仕入状況";
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
            this.ckM_Label5.Location = new System.Drawing.Point(95, 179);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label5.TabIndex = 23;
            this.ckM_Label5.Text = "仕入日";
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
            this.ckM_Label6.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label6.Location = new System.Drawing.Point(69, 218);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label6.TabIndex = 24;
            this.ckM_Label6.Text = "納品書番号";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label7.Location = new System.Drawing.Point(253, 62);
            this.ckM_Label7.Name = "ckM_Label7";
            this.ckM_Label7.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label7.TabIndex = 25;
            this.ckM_Label7.Text = "～";
            this.ckM_Label7.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label8.Location = new System.Drawing.Point(253, 102);
            this.ckM_Label8.Name = "ckM_Label8";
            this.ckM_Label8.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label8.TabIndex = 26;
            this.ckM_Label8.Text = "～";
            this.ckM_Label8.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label9.Location = new System.Drawing.Point(253, 178);
            this.ckM_Label9.Name = "ckM_Label9";
            this.ckM_Label9.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label9.TabIndex = 27;
            this.ckM_Label9.Text = "～";
            this.ckM_Label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label10.Location = new System.Drawing.Point(873, 22);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label10.TabIndex = 28;
            this.ckM_Label10.Text = "表示対象";
            this.ckM_Label10.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label11.Location = new System.Drawing.Point(897, 108);
            this.ckM_Label11.Name = "ckM_Label11";
            this.ckM_Label11.Size = new System.Drawing.Size(33, 12);
            this.ckM_Label11.TabIndex = 29;
            this.ckM_Label11.Text = "ITEM";
            this.ckM_Label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label12.Location = new System.Drawing.Point(890, 142);
            this.ckM_Label12.Name = "ckM_Label12";
            this.ckM_Label12.Size = new System.Drawing.Size(40, 12);
            this.ckM_Label12.TabIndex = 30;
            this.ckM_Label12.Text = "SKUCD";
            this.ckM_Label12.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label13
            // 
            this.ckM_Label13.AutoSize = true;
            this.ckM_Label13.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label13.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label13.DefaultlabelSize = true;
            this.ckM_Label13.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label13.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label13.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label13.Location = new System.Drawing.Point(889, 178);
            this.ckM_Label13.Name = "ckM_Label13";
            this.ckM_Label13.Size = new System.Drawing.Size(40, 12);
            this.ckM_Label13.TabIndex = 31;
            this.ckM_Label13.Text = "JANCD";
            this.ckM_Label13.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label14
            // 
            this.ckM_Label14.AutoSize = true;
            this.ckM_Label14.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label14.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label14.DefaultlabelSize = true;
            this.ckM_Label14.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label14.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label14.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label14.Location = new System.Drawing.Point(885, 221);
            this.ckM_Label14.Name = "ckM_Label14";
            this.ckM_Label14.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label14.TabIndex = 32;
            this.ckM_Label14.Text = "商品名";
            this.ckM_Label14.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkDelivery
            // 
            this.chkDelivery.AutoSize = true;
            this.chkDelivery.Checked = true;
            this.chkDelivery.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDelivery.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkDelivery.Location = new System.Drawing.Point(933, 20);
            this.chkDelivery.Name = "chkDelivery";
            this.chkDelivery.Size = new System.Drawing.Size(128, 16);
            this.chkDelivery.TabIndex = 10;
            this.chkDelivery.Text = "仕入先からの入荷";
            this.chkDelivery.UseVisualStyleBackColor = true;
            this.chkDelivery.CheckedChanged += new System.EventHandler(this.chkDelivery_CheckedChanged);
            // 
            // ChkArrival
            // 
            this.ChkArrival.AutoSize = true;
            this.ChkArrival.Checked = true;
            this.ChkArrival.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkArrival.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ChkArrival.Location = new System.Drawing.Point(934, 58);
            this.ChkArrival.Name = "ChkArrival";
            this.ChkArrival.Size = new System.Drawing.Size(128, 16);
            this.ChkArrival.TabIndex = 11;
            this.ChkArrival.Text = "店舗間移動の入荷";
            this.ChkArrival.UseVisualStyleBackColor = true;
            this.ChkArrival.CheckedChanged += new System.EventHandler(this.ChkArrival_CheckedChanged);
            // 
            // ckM_Label15
            // 
            this.ckM_Label15.AutoSize = true;
            this.ckM_Label15.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label15.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label15.DefaultlabelSize = true;
            this.ckM_Label15.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label15.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label15.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label15.Location = new System.Drawing.Point(1276, 22);
            this.ckM_Label15.Name = "ckM_Label15";
            this.ckM_Label15.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label15.TabIndex = 33;
            this.ckM_Label15.Text = "仕入先";
            this.ckM_Label15.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label16
            // 
            this.ckM_Label16.AutoSize = true;
            this.ckM_Label16.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label16.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label16.DefaultlabelSize = true;
            this.ckM_Label16.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label16.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label16.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label16.Location = new System.Drawing.Point(13, 11);
            this.ckM_Label16.Name = "ckM_Label16";
            this.ckM_Label16.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label16.TabIndex = 34;
            this.ckM_Label16.Text = "移動元倉庫";
            this.ckM_Label16.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScItem
            // 
            this.ScItem.AutoSize = true;
            this.ScItem.ChangeDate = "";
            this.ScItem.ChangeDateWidth = 100;
            this.ScItem.Code = "";
            this.ScItem.CodeWidth = 600;
            this.ScItem.CodeWidth1 = 600;
            this.ScItem.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScItem.DataCheck = false;
            this.ScItem.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScItem.IsCopy = false;
            this.ScItem.LabelText = "";
            this.ScItem.LabelVisible = false;
            this.ScItem.Location = new System.Drawing.Point(934, 99);
            this.ScItem.Margin = new System.Windows.Forms.Padding(0);
            this.ScItem.Name = "ScItem";
            this.ScItem.NameWidth = 280;
            this.ScItem.SearchEnable = true;
            this.ScItem.Size = new System.Drawing.Size(633, 27);
            this.ScItem.Stype = Search.CKM_SearchControl.SearchType.ItemMulti;
            this.ScItem.TabIndex = 14;
            this.ScItem.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScItem.UseChangeDate = false;
            this.ScItem.Value1 = null;
            this.ScItem.Value2 = null;
            this.ScItem.Value3 = null;
            // 
            // ScSKUCD
            // 
            this.ScSKUCD.AutoSize = true;
            this.ScSKUCD.ChangeDate = "";
            this.ScSKUCD.ChangeDateWidth = 100;
            this.ScSKUCD.Code = "";
            this.ScSKUCD.CodeWidth = 600;
            this.ScSKUCD.CodeWidth1 = 600;
            this.ScSKUCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScSKUCD.DataCheck = false;
            this.ScSKUCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScSKUCD.IsCopy = false;
            this.ScSKUCD.LabelText = "";
            this.ScSKUCD.LabelVisible = false;
            this.ScSKUCD.Location = new System.Drawing.Point(934, 134);
            this.ScSKUCD.Margin = new System.Windows.Forms.Padding(0);
            this.ScSKUCD.Name = "ScSKUCD";
            this.ScSKUCD.NameWidth = 280;
            this.ScSKUCD.SearchEnable = true;
            this.ScSKUCD.Size = new System.Drawing.Size(633, 27);
            this.ScSKUCD.Stype = Search.CKM_SearchControl.SearchType.SKUMulti;
            this.ScSKUCD.TabIndex = 15;
            this.ScSKUCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScSKUCD.UseChangeDate = false;
            this.ScSKUCD.Value1 = null;
            this.ScSKUCD.Value2 = null;
            this.ScSKUCD.Value3 = null;
            // 
            // ScJanCD
            // 
            this.ScJanCD.AutoSize = true;
            this.ScJanCD.ChangeDate = "";
            this.ScJanCD.ChangeDateWidth = 100;
            this.ScJanCD.Code = "";
            this.ScJanCD.CodeWidth = 600;
            this.ScJanCD.CodeWidth1 = 600;
            this.ScJanCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScJanCD.DataCheck = false;
            this.ScJanCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScJanCD.IsCopy = false;
            this.ScJanCD.LabelText = "";
            this.ScJanCD.LabelVisible = false;
            this.ScJanCD.Location = new System.Drawing.Point(933, 171);
            this.ScJanCD.Margin = new System.Windows.Forms.Padding(0);
            this.ScJanCD.Name = "ScJanCD";
            this.ScJanCD.NameWidth = 280;
            this.ScJanCD.SearchEnable = true;
            this.ScJanCD.Size = new System.Drawing.Size(633, 27);
            this.ScJanCD.Stype = Search.CKM_SearchControl.SearchType.JANMulti;
            this.ScJanCD.TabIndex = 16;
            this.ScJanCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScJanCD.UseChangeDate = false;
            this.ScJanCD.Value1 = null;
            this.ScJanCD.Value2 = null;
            this.ScJanCD.Value3 = null;
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
            this.txtProductName.Location = new System.Drawing.Point(933, 218);
            this.txtProductName.MaxLength = 80;
            this.txtProductName.MoveNext = true;
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(500, 19);
            this.txtProductName.TabIndex = 17;
            this.txtProductName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ScSupplier
            // 
            this.ScSupplier.AutoSize = true;
            this.ScSupplier.ChangeDate = "";
            this.ScSupplier.ChangeDateWidth = 100;
            this.ScSupplier.Code = "";
            this.ScSupplier.CodeWidth = 100;
            this.ScSupplier.CodeWidth1 = 100;
            this.ScSupplier.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScSupplier.DataCheck = false;
            this.ScSupplier.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScSupplier.IsCopy = false;
            this.ScSupplier.LabelText = "";
            this.ScSupplier.LabelVisible = true;
            this.ScSupplier.Location = new System.Drawing.Point(1323, 12);
            this.ScSupplier.Margin = new System.Windows.Forms.Padding(0);
            this.ScSupplier.Name = "ScSupplier";
            this.ScSupplier.NameWidth = 310;
            this.ScSupplier.SearchEnable = true;
            this.ScSupplier.Size = new System.Drawing.Size(444, 27);
            this.ScSupplier.Stype = Search.CKM_SearchControl.SearchType.仕入先;
            this.ScSupplier.TabIndex = 12;
            this.ScSupplier.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScSupplier.UseChangeDate = false;
            this.ScSupplier.Value1 = null;
            this.ScSupplier.Value2 = null;
            this.ScSupplier.Value3 = null;
            this.ScSupplier.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScSupplier_CodeKeyDownEvent);
            // 
            // cboSourceWH
            // 
            this.cboSourceWH.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboSourceWH.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSourceWH.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.倉庫;
            this.cboSourceWH.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboSourceWH.Flag = 0;
            this.cboSourceWH.FormattingEnabled = true;
            this.cboSourceWH.Length = 10;
            this.cboSourceWH.Location = new System.Drawing.Point(86, 6);
            this.cboSourceWH.MaxLength = 10;
            this.cboSourceWH.MoveNext = true;
            this.cboSourceWH.Name = "cboSourceWH";
            this.cboSourceWH.Size = new System.Drawing.Size(121, 20);
            this.cboSourceWH.TabIndex = 0;
            // 
            // dgvNyuukaShoukai
            // 
            this.dgvNyuukaShoukai.AllowUserToAddRows = false;
            this.dgvNyuukaShoukai.AllowUserToDeleteRows = false;
            this.dgvNyuukaShoukai.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.dgvNyuukaShoukai.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvNyuukaShoukai.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvNyuukaShoukai.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvNyuukaShoukai.ColumnHeadersHeight = 25;
            this.dgvNyuukaShoukai.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDate,
            this.colCalcuArrivalPlanDate,
            this.colPurchaseDate,
            this.colGoods,
            this.colSKUCD,
            this.colJanCD,
            this.colSKUName,
            this.colColorName,
            this.colSizeName,
            this.colArrivalPlanSu,
            this.colArrivalSu,
            this.colVendorName,
            this.colSoukoName,
            this.colDirectDelivery,
            this.colReserveNum,
            this.colNumber,
            this.colArrivalNO,
            this.colPurchaseNO,
            this.colVendor});
            this.dgvNyuukaShoukai.EnableHeadersVisualStyles = false;
            this.dgvNyuukaShoukai.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvNyuukaShoukai.Location = new System.Drawing.Point(10, 267);
            this.dgvNyuukaShoukai.Name = "dgvNyuukaShoukai";
            this.dgvNyuukaShoukai.RowHeight_ = 20;
            this.dgvNyuukaShoukai.Size = new System.Drawing.Size(1760, 500);
            this.dgvNyuukaShoukai.TabIndex = 35;
            this.dgvNyuukaShoukai.UseRowNo = true;
            this.dgvNyuukaShoukai.UseSetting = true;
            // 
            // colDate
            // 
            this.colDate.DataPropertyName = "ArrivalDate";
            this.colDate.HeaderText = "入荷日";
            this.colDate.MaxInputLength = 10;
            this.colDate.Name = "colDate";
            // 
            // colCalcuArrivalPlanDate
            // 
            this.colCalcuArrivalPlanDate.DataPropertyName = "CalcuArrivalPlanDate";
            this.colCalcuArrivalPlanDate.HeaderText = "入荷予定日";
            this.colCalcuArrivalPlanDate.MaxInputLength = 10;
            this.colCalcuArrivalPlanDate.Name = "colCalcuArrivalPlanDate";
            // 
            // colPurchaseDate
            // 
            this.colPurchaseDate.DataPropertyName = "PurchaseDate";
            this.colPurchaseDate.HeaderText = "仕入日";
            this.colPurchaseDate.MaxInputLength = 10;
            this.colPurchaseDate.Name = "colPurchaseDate";
            // 
            // colGoods
            // 
            this.colGoods.DataPropertyName = "Goods";
            this.colGoods.HeaderText = "入庫区分";
            this.colGoods.MaxInputLength = 8;
            this.colGoods.Name = "colGoods";
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
            this.colJanCD.Width = 150;
            // 
            // colSKUName
            // 
            this.colSKUName.DataPropertyName = "SKUName";
            this.colSKUName.HeaderText = "商品名";
            this.colSKUName.MaxInputLength = 80;
            this.colSKUName.Name = "colSKUName";
            this.colSKUName.Width = 500;
            // 
            // colColorName
            // 
            this.colColorName.DataPropertyName = "ColorName";
            this.colColorName.HeaderText = "カラー";
            this.colColorName.MaxInputLength = 20;
            this.colColorName.Name = "colColorName";
            this.colColorName.Width = 200;
            // 
            // colSizeName
            // 
            this.colSizeName.DataPropertyName = "SizeName";
            this.colSizeName.HeaderText = "サイズ";
            this.colSizeName.MaxInputLength = 20;
            this.colSizeName.Name = "colSizeName";
            this.colSizeName.Width = 200;
            // 
            // colArrivalPlanSu
            // 
            this.colArrivalPlanSu.DataPropertyName = "ArrivalPlanSu";
            this.colArrivalPlanSu.HeaderText = "予定数";
            this.colArrivalPlanSu.MaxInputLength = 6;
            this.colArrivalPlanSu.Name = "colArrivalPlanSu";
            this.colArrivalPlanSu.Width = 60;
            // 
            // colArrivalSu
            // 
            this.colArrivalSu.DataPropertyName = "ArrivalSu";
            this.colArrivalSu.HeaderText = "入荷数";
            this.colArrivalSu.MaxInputLength = 6;
            this.colArrivalSu.Name = "colArrivalSu";
            this.colArrivalSu.Width = 60;
            // 
            // colVendorName
            // 
            this.colVendorName.DataPropertyName = "VendorName";
            this.colVendorName.HeaderText = "仕入先";
            this.colVendorName.MaxInputLength = 51;
            this.colVendorName.Name = "colVendorName";
            this.colVendorName.Width = 400;
            // 
            // colSoukoName
            // 
            this.colSoukoName.DataPropertyName = "SoukoName";
            this.colSoukoName.HeaderText = "移動元倉庫";
            this.colSoukoName.MaxInputLength = 40;
            this.colSoukoName.Name = "colSoukoName";
            this.colSoukoName.Width = 300;
            // 
            // colDirectDelivery
            // 
            this.colDirectDelivery.DataPropertyName = "Directdelivery";
            this.colDirectDelivery.HeaderText = "直送";
            this.colDirectDelivery.MaxInputLength = 2;
            this.colDirectDelivery.Name = "colDirectDelivery";
            this.colDirectDelivery.Width = 50;
            // 
            // colReserveNum
            // 
            this.colReserveNum.DataPropertyName = "ReserveNumber";
            this.colReserveNum.HeaderText = "受注番号";
            this.colReserveNum.MaxInputLength = 11;
            this.colReserveNum.Name = "colReserveNum";
            this.colReserveNum.Width = 110;
            // 
            // colNumber
            // 
            this.colNumber.DataPropertyName = "Number";
            this.colNumber.HeaderText = "発注番号";
            this.colNumber.MaxInputLength = 11;
            this.colNumber.Name = "colNumber";
            this.colNumber.Width = 110;
            // 
            // colArrivalNO
            // 
            this.colArrivalNO.DataPropertyName = "ArrivalNO";
            this.colArrivalNO.HeaderText = "入荷番号";
            this.colArrivalNO.MaxInputLength = 11;
            this.colArrivalNO.Name = "colArrivalNO";
            this.colArrivalNO.Width = 110;
            // 
            // colPurchaseNO
            // 
            this.colPurchaseNO.DataPropertyName = "PurchaseNO";
            this.colPurchaseNO.HeaderText = "仕入番号";
            this.colPurchaseNO.MaxInputLength = 11;
            this.colPurchaseNO.Name = "colPurchaseNO";
            this.colPurchaseNO.Width = 110;
            // 
            // colVendor
            // 
            this.colVendor.DataPropertyName = "VendorDeliveryNo";
            this.colVendor.HeaderText = "納品書番号";
            this.colVendor.MaxInputLength = 15;
            this.colVendor.Name = "colVendor";
            this.colVendor.Width = 150;
            // 
            // txtArrivalDay1
            // 
            this.txtArrivalDay1.AllowMinus = false;
            this.txtArrivalDay1.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtArrivalDay1.BackColor = System.Drawing.Color.White;
            this.txtArrivalDay1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtArrivalDay1.ClientColor = System.Drawing.Color.White;
            this.txtArrivalDay1.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtArrivalDay1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtArrivalDay1.DecimalPlace = 0;
            this.txtArrivalDay1.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtArrivalDay1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtArrivalDay1.IntegerPart = 0;
            this.txtArrivalDay1.IsCorrectDate = true;
            this.txtArrivalDay1.isEnterKeyDown = false;
            this.txtArrivalDay1.isMaxLengthErr = false;
            this.txtArrivalDay1.IsNumber = true;
            this.txtArrivalDay1.IsShop = false;
            this.txtArrivalDay1.Length = 8;
            this.txtArrivalDay1.Location = new System.Drawing.Point(142, 58);
            this.txtArrivalDay1.MaxLength = 8;
            this.txtArrivalDay1.MoveNext = true;
            this.txtArrivalDay1.Name = "txtArrivalDay1";
            this.txtArrivalDay1.Size = new System.Drawing.Size(100, 19);
            this.txtArrivalDay1.TabIndex = 1;
            this.txtArrivalDay1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtArrivalDay1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // txtArrivalDay2
            // 
            this.txtArrivalDay2.AllowMinus = false;
            this.txtArrivalDay2.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtArrivalDay2.BackColor = System.Drawing.Color.White;
            this.txtArrivalDay2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtArrivalDay2.ClientColor = System.Drawing.Color.White;
            this.txtArrivalDay2.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtArrivalDay2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtArrivalDay2.DecimalPlace = 0;
            this.txtArrivalDay2.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtArrivalDay2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtArrivalDay2.IntegerPart = 0;
            this.txtArrivalDay2.IsCorrectDate = true;
            this.txtArrivalDay2.isEnterKeyDown = false;
            this.txtArrivalDay2.isMaxLengthErr = false;
            this.txtArrivalDay2.IsNumber = true;
            this.txtArrivalDay2.IsShop = false;
            this.txtArrivalDay2.Length = 8;
            this.txtArrivalDay2.Location = new System.Drawing.Point(281, 58);
            this.txtArrivalDay2.MaxLength = 8;
            this.txtArrivalDay2.MoveNext = true;
            this.txtArrivalDay2.Name = "txtArrivalDay2";
            this.txtArrivalDay2.Size = new System.Drawing.Size(100, 19);
            this.txtArrivalDay2.TabIndex = 2;
            this.txtArrivalDay2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtArrivalDay2.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtArrivalDay2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtArrivalDay2_KeyDown);
            // 
            // txtStockDate1
            // 
            this.txtStockDate1.AllowMinus = false;
            this.txtStockDate1.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtStockDate1.BackColor = System.Drawing.Color.White;
            this.txtStockDate1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStockDate1.ClientColor = System.Drawing.Color.White;
            this.txtStockDate1.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtStockDate1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtStockDate1.DecimalPlace = 0;
            this.txtStockDate1.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtStockDate1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtStockDate1.IntegerPart = 0;
            this.txtStockDate1.IsCorrectDate = true;
            this.txtStockDate1.isEnterKeyDown = false;
            this.txtStockDate1.isMaxLengthErr = false;
            this.txtStockDate1.IsNumber = true;
            this.txtStockDate1.IsShop = false;
            this.txtStockDate1.Length = 8;
            this.txtStockDate1.Location = new System.Drawing.Point(142, 100);
            this.txtStockDate1.MaxLength = 8;
            this.txtStockDate1.MoveNext = true;
            this.txtStockDate1.Name = "txtStockDate1";
            this.txtStockDate1.Size = new System.Drawing.Size(100, 19);
            this.txtStockDate1.TabIndex = 3;
            this.txtStockDate1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtStockDate1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // txtStockDate2
            // 
            this.txtStockDate2.AllowMinus = false;
            this.txtStockDate2.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtStockDate2.BackColor = System.Drawing.Color.White;
            this.txtStockDate2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStockDate2.ClientColor = System.Drawing.Color.White;
            this.txtStockDate2.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtStockDate2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtStockDate2.DecimalPlace = 0;
            this.txtStockDate2.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtStockDate2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtStockDate2.IntegerPart = 0;
            this.txtStockDate2.IsCorrectDate = true;
            this.txtStockDate2.isEnterKeyDown = false;
            this.txtStockDate2.isMaxLengthErr = false;
            this.txtStockDate2.IsNumber = true;
            this.txtStockDate2.IsShop = false;
            this.txtStockDate2.Length = 8;
            this.txtStockDate2.Location = new System.Drawing.Point(281, 100);
            this.txtStockDate2.MaxLength = 8;
            this.txtStockDate2.MoveNext = true;
            this.txtStockDate2.Name = "txtStockDate2";
            this.txtStockDate2.Size = new System.Drawing.Size(100, 19);
            this.txtStockDate2.TabIndex = 4;
            this.txtStockDate2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtStockDate2.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtStockDate2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtStockDate2_KeyDown);
            // 
            // txtPurchaseDate1
            // 
            this.txtPurchaseDate1.AllowMinus = false;
            this.txtPurchaseDate1.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPurchaseDate1.BackColor = System.Drawing.Color.White;
            this.txtPurchaseDate1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPurchaseDate1.ClientColor = System.Drawing.Color.White;
            this.txtPurchaseDate1.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtPurchaseDate1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtPurchaseDate1.DecimalPlace = 0;
            this.txtPurchaseDate1.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtPurchaseDate1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtPurchaseDate1.IntegerPart = 0;
            this.txtPurchaseDate1.IsCorrectDate = true;
            this.txtPurchaseDate1.isEnterKeyDown = false;
            this.txtPurchaseDate1.isMaxLengthErr = false;
            this.txtPurchaseDate1.IsNumber = true;
            this.txtPurchaseDate1.IsShop = false;
            this.txtPurchaseDate1.Length = 8;
            this.txtPurchaseDate1.Location = new System.Drawing.Point(142, 176);
            this.txtPurchaseDate1.MaxLength = 8;
            this.txtPurchaseDate1.MoveNext = true;
            this.txtPurchaseDate1.Name = "txtPurchaseDate1";
            this.txtPurchaseDate1.Size = new System.Drawing.Size(100, 19);
            this.txtPurchaseDate1.TabIndex = 7;
            this.txtPurchaseDate1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPurchaseDate1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // txtPurchaseDate2
            // 
            this.txtPurchaseDate2.AllowMinus = false;
            this.txtPurchaseDate2.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPurchaseDate2.BackColor = System.Drawing.Color.White;
            this.txtPurchaseDate2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPurchaseDate2.ClientColor = System.Drawing.Color.White;
            this.txtPurchaseDate2.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtPurchaseDate2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtPurchaseDate2.DecimalPlace = 0;
            this.txtPurchaseDate2.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtPurchaseDate2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtPurchaseDate2.IntegerPart = 0;
            this.txtPurchaseDate2.IsCorrectDate = true;
            this.txtPurchaseDate2.isEnterKeyDown = false;
            this.txtPurchaseDate2.isMaxLengthErr = false;
            this.txtPurchaseDate2.IsNumber = true;
            this.txtPurchaseDate2.IsShop = false;
            this.txtPurchaseDate2.Length = 8;
            this.txtPurchaseDate2.Location = new System.Drawing.Point(281, 176);
            this.txtPurchaseDate2.MaxLength = 8;
            this.txtPurchaseDate2.MoveNext = true;
            this.txtPurchaseDate2.Name = "txtPurchaseDate2";
            this.txtPurchaseDate2.Size = new System.Drawing.Size(100, 19);
            this.txtPurchaseDate2.TabIndex = 8;
            this.txtPurchaseDate2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPurchaseDate2.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtPurchaseDate2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPurchaseDate2_KeyDown);
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
            this.btnDisplay.Location = new System.Drawing.Point(1649, 209);
            this.btnDisplay.Margin = new System.Windows.Forms.Padding(1);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(118, 28);
            this.btnDisplay.TabIndex = 18;
            this.btnDisplay.Text = "表示(F11)";
            this.btnDisplay.UseVisualStyleBackColor = false;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.panelcombo2);
            this.panelDetail.Controls.Add(this.panelcombo1);
            this.panelDetail.Controls.Add(this.btnDisplay);
            this.panelDetail.Controls.Add(this.dgvNyuukaShoukai);
            this.panelDetail.Controls.Add(this.txtPurchaseDate2);
            this.panelDetail.Controls.Add(this.ScItem);
            this.panelDetail.Controls.Add(this.txtPurchaseDate1);
            this.panelDetail.Controls.Add(this.txtStockDate2);
            this.panelDetail.Controls.Add(this.statusChk1);
            this.panelDetail.Controls.Add(this.txtStockDate1);
            this.panelDetail.Controls.Add(this.statusChk2);
            this.panelDetail.Controls.Add(this.txtArrivalDay2);
            this.panelDetail.Controls.Add(this.txtDeliveryNote);
            this.panelDetail.Controls.Add(this.txtArrivalDay1);
            this.panelDetail.Controls.Add(this.ckM_Label2);
            this.panelDetail.Controls.Add(this.ScSupplier);
            this.panelDetail.Controls.Add(this.ckM_Label3);
            this.panelDetail.Controls.Add(this.txtProductName);
            this.panelDetail.Controls.Add(this.ckM_Label4);
            this.panelDetail.Controls.Add(this.ScJanCD);
            this.panelDetail.Controls.Add(this.ckM_Label5);
            this.panelDetail.Controls.Add(this.ScSKUCD);
            this.panelDetail.Controls.Add(this.ckM_Label6);
            this.panelDetail.Controls.Add(this.ckM_Label7);
            this.panelDetail.Controls.Add(this.ckM_Label8);
            this.panelDetail.Controls.Add(this.ckM_Label15);
            this.panelDetail.Controls.Add(this.ckM_Label9);
            this.panelDetail.Controls.Add(this.ChkArrival);
            this.panelDetail.Controls.Add(this.ckM_Label10);
            this.panelDetail.Controls.Add(this.chkDelivery);
            this.panelDetail.Controls.Add(this.ckM_Label11);
            this.panelDetail.Controls.Add(this.ckM_Label14);
            this.panelDetail.Controls.Add(this.ckM_Label12);
            this.panelDetail.Controls.Add(this.ckM_Label13);
            this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetail.Location = new System.Drawing.Point(0, 50);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(1784, 879);
            this.panelDetail.TabIndex = 0;
            // 
            // panelcombo2
            // 
            this.panelcombo2.Controls.Add(this.ckM_Label16);
            this.panelcombo2.Controls.Add(this.cboSourceWH);
            this.panelcombo2.Location = new System.Drawing.Point(1237, 56);
            this.panelcombo2.Name = "panelcombo2";
            this.panelcombo2.Size = new System.Drawing.Size(300, 40);
            this.panelcombo2.TabIndex = 13;
            // 
            // panelcombo1
            // 
            this.panelcombo1.Controls.Add(this.ckM_Label1);
            this.panelcombo1.Controls.Add(this.cboWarehouse);
            this.panelcombo1.Location = new System.Drawing.Point(84, 8);
            this.panelcombo1.Name = "panelcombo1";
            this.panelcombo1.Size = new System.Drawing.Size(300, 40);
            this.panelcombo1.TabIndex = 0;
            // 
            // FrmNyuukaShoukai
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1784, 961);
            this.Controls.Add(this.panelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "FrmNyuukaShoukai";
            this.PanelHeaderHeight = 50;
            this.Text = "NyuukaShoukai";
            this.Load += new System.EventHandler(this.FrmNyuukaShoukai_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmNyuukaShoukai_KeyUp);
            this.Controls.SetChildIndex(this.panelDetail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNyuukaShoukai)).EndInit();
            this.panelDetail.ResumeLayout(false);
            this.panelDetail.PerformLayout();
            this.panelcombo2.ResumeLayout(false);
            this.panelcombo2.PerformLayout();
            this.panelcombo1.ResumeLayout(false);
            this.panelcombo1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CKM_Controls.CKM_ComboBox cboSourceWH;
        private Search.CKM_SearchControl ScSupplier;
        private CKM_Controls.CKM_TextBox txtProductName;
        private Search.CKM_SearchControl ScJanCD;
        private Search.CKM_SearchControl ScSKUCD;
        private Search.CKM_SearchControl ScItem;
        private CKM_Controls.CKM_Label ckM_Label16;
        private CKM_Controls.CKM_Label ckM_Label15;
        private CKM_Controls.CKM_CheckBox ChkArrival;
        private CKM_Controls.CKM_CheckBox chkDelivery;
        private CKM_Controls.CKM_Label ckM_Label14;
        private CKM_Controls.CKM_Label ckM_Label13;
        private CKM_Controls.CKM_Label ckM_Label12;
        private CKM_Controls.CKM_Label ckM_Label11;
        private CKM_Controls.CKM_Label ckM_Label10;
        private CKM_Controls.CKM_Label ckM_Label9;
        private CKM_Controls.CKM_Label ckM_Label8;
        private CKM_Controls.CKM_Label ckM_Label7;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_TextBox txtDeliveryNote;
        private CKM_Controls.CKM_CheckBox statusChk2;
        private CKM_Controls.CKM_CheckBox statusChk1;
        private CKM_Controls.CKM_ComboBox cboWarehouse;
        private CKM_Controls.CKM_GridView dgvNyuukaShoukai;
        private CKM_Controls.CKM_TextBox txtPurchaseDate2;
        private CKM_Controls.CKM_TextBox txtPurchaseDate1;
        private CKM_Controls.CKM_TextBox txtStockDate2;
        private CKM_Controls.CKM_TextBox txtStockDate1;
        private CKM_Controls.CKM_TextBox txtArrivalDay2;
        private CKM_Controls.CKM_TextBox txtArrivalDay1;
        private CKM_Controls.CKM_Button btnDisplay;
        private System.Windows.Forms.Panel panelDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCalcuArrivalPlanDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGoods;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJanCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSizeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArrivalPlanSu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArrivalSu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVendorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoukoName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDirectDelivery;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReserveNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArrivalNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVendor;
        private System.Windows.Forms.Panel panelcombo2;
        private System.Windows.Forms.Panel panelcombo1;
    }
}