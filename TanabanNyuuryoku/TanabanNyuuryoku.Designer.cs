namespace TanabanNyuuryoku
{
    partial class FrmTanabanNyuuryoku
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTanabanNyuuryoku));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelDetail = new System.Windows.Forms.Panel();
            this.ScStorage = new Search.CKM_SearchControl();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.dgvTanaban = new CKM_Controls.CKM_GridView();
            this.colChk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colRackNo1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBtn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colRackNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSizeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJanCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockSu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnReleaseAll = new CKM_Controls.CKM_Button();
            this.btnSelectAll = new CKM_Controls.CKM_Button();
            this.btnApplicable = new CKM_Controls.CKM_Button();
            this.btnDisplay = new CKM_Controls.CKM_Button();
            this.chkRegister = new CKM_Controls.CKM_CheckBox();
            this.chkNotRegister = new CKM_Controls.CKM_CheckBox();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.cboWarehouse = new CKM_Controls.CKM_ComboBox();
            this.txtArrivalDateTo = new CKM_Controls.CKM_TextBox();
            this.txtArrivalDateFrom = new CKM_Controls.CKM_TextBox();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.panelDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTanaban)).BeginInit();
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
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.ScStorage);
            this.panelDetail.Controls.Add(this.ckM_Label5);
            this.panelDetail.Controls.Add(this.dgvTanaban);
            this.panelDetail.Controls.Add(this.btnReleaseAll);
            this.panelDetail.Controls.Add(this.btnSelectAll);
            this.panelDetail.Controls.Add(this.btnApplicable);
            this.panelDetail.Controls.Add(this.btnDisplay);
            this.panelDetail.Controls.Add(this.chkRegister);
            this.panelDetail.Controls.Add(this.chkNotRegister);
            this.panelDetail.Controls.Add(this.ckM_Label4);
            this.panelDetail.Controls.Add(this.cboWarehouse);
            this.panelDetail.Controls.Add(this.txtArrivalDateTo);
            this.panelDetail.Controls.Add(this.txtArrivalDateFrom);
            this.panelDetail.Controls.Add(this.ckM_Label3);
            this.panelDetail.Controls.Add(this.ckM_Label2);
            this.panelDetail.Controls.Add(this.ckM_Label1);
            this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetail.Location = new System.Drawing.Point(0, 54);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(1713, 875);
            this.panelDetail.TabIndex = 0;
            // 
            // ScStorage
            // 
            this.ScStorage.AutoSize = true;
            this.ScStorage.ChangeDate = "";
            this.ScStorage.ChangeDateWidth = 100;
            this.ScStorage.Code = "";
            this.ScStorage.CodeWidth = 100;
            this.ScStorage.CodeWidth1 = 100;
            this.ScStorage.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScStorage.DataCheck = false;
            this.ScStorage.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScStorage.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScStorage.IsCopy = false;
            this.ScStorage.LabelText = "";
            this.ScStorage.LabelVisible = false;
            this.ScStorage.Location = new System.Drawing.Point(943, 83);
            this.ScStorage.Margin = new System.Windows.Forms.Padding(0);
            this.ScStorage.Name = "ScStorage";
            this.ScStorage.NameWidth = 600;
            this.ScStorage.SearchEnable = true;
            this.ScStorage.Size = new System.Drawing.Size(133, 28);
            this.ScStorage.Stype = Search.CKM_SearchControl.SearchType.棚番号;
            this.ScStorage.TabIndex = 6;
            this.ScStorage.test = null;
            this.ScStorage.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScStorage.UseChangeDate = false;
            this.ScStorage.Value1 = null;
            this.ScStorage.Value2 = null;
            this.ScStorage.Value3 = null;
            this.ScStorage.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScStorage_CodeKeyDownEvent);
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
            this.ckM_Label5.Location = new System.Drawing.Point(909, 91);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label5.TabIndex = 9;
            this.ckM_Label5.Text = "棚番";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvTanaban
            // 
            this.dgvTanaban.AllowUserToAddRows = false;
            this.dgvTanaban.AllowUserToDeleteRows = false;
            this.dgvTanaban.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.dgvTanaban.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTanaban.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvTanaban.CheckCol = ((System.Collections.ArrayList)(resources.GetObject("dgvTanaban.CheckCol")));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTanaban.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTanaban.ColumnHeadersHeight = 25;
            this.dgvTanaban.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colChk,
            this.colRackNo1,
            this.colBtn,
            this.colRackNO,
            this.colSKUCD,
            this.colSKUName,
            this.colColorName,
            this.colSizeName,
            this.colJanCD,
            this.colStockSu,
            this.colStockNo});
            this.dgvTanaban.EnableHeadersVisualStyles = false;
            this.dgvTanaban.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvTanaban.Location = new System.Drawing.Point(25, 149);
            this.dgvTanaban.Name = "dgvTanaban";
            this.dgvTanaban.RowHeadersVisible = false;
            this.dgvTanaban.RowHeight_ = 20;
            this.dgvTanaban.RowTemplate.Height = 20;
            this.dgvTanaban.Size = new System.Drawing.Size(1645, 500);
            this.dgvTanaban.TabIndex = 11;
            this.dgvTanaban.UseRowNo = false;
            this.dgvTanaban.UseSetting = true;
            this.dgvTanaban.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTanaban_CellContentClick);
            this.dgvTanaban.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvTanaban_CellPainting);
            this.dgvTanaban.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvTanaban_CellValidating);
            this.dgvTanaban.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvTanaban_DataError);
            this.dgvTanaban.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvTanaban_Paint);
            this.dgvTanaban.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvTanaban_KeyDown);
            // 
            // colChk
            // 
            this.colChk.HeaderText = "";
            this.colChk.Name = "colChk";
            this.colChk.TrueValue = "true";
            this.colChk.Width = 50;
            // 
            // colRackNo1
            // 
            this.colRackNo1.DataPropertyName = "RackNo1";
            this.colRackNo1.HeaderText = "棚番";
            this.colRackNo1.MaxInputLength = 10;
            this.colRackNo1.Name = "colRackNo1";
            // 
            // colBtn
            // 
            this.colBtn.HeaderText = "";
            this.colBtn.Name = "colBtn";
            this.colBtn.Width = 50;
            // 
            // colRackNO
            // 
            this.colRackNO.DataPropertyName = "RackNO";
            this.colRackNO.HeaderText = "既存棚番";
            this.colRackNO.MaxInputLength = 10;
            this.colRackNO.Name = "colRackNO";
            this.colRackNO.ReadOnly = true;
            // 
            // colSKUCD
            // 
            this.colSKUCD.DataPropertyName = "SKUCD";
            this.colSKUCD.HeaderText = "SKUCD";
            this.colSKUCD.MaxInputLength = 30;
            this.colSKUCD.Name = "colSKUCD";
            this.colSKUCD.ReadOnly = true;
            this.colSKUCD.Width = 270;
            // 
            // colSKUName
            // 
            this.colSKUName.DataPropertyName = "SKUName";
            this.colSKUName.HeaderText = "商品名";
            this.colSKUName.MaxInputLength = 80;
            this.colSKUName.Name = "colSKUName";
            this.colSKUName.ReadOnly = true;
            this.colSKUName.Width = 450;
            // 
            // colColorName
            // 
            this.colColorName.DataPropertyName = "ColorName";
            this.colColorName.HeaderText = "カラー";
            this.colColorName.MaxInputLength = 20;
            this.colColorName.Name = "colColorName";
            this.colColorName.ReadOnly = true;
            this.colColorName.Width = 170;
            // 
            // colSizeName
            // 
            this.colSizeName.DataPropertyName = "SizeName";
            this.colSizeName.HeaderText = "サイズ";
            this.colSizeName.MaxInputLength = 20;
            this.colSizeName.Name = "colSizeName";
            this.colSizeName.ReadOnly = true;
            this.colSizeName.Width = 170;
            // 
            // colJanCD
            // 
            this.colJanCD.DataPropertyName = "JanCD";
            this.colJanCD.HeaderText = "JANCD";
            this.colJanCD.MaxInputLength = 13;
            this.colJanCD.Name = "colJanCD";
            this.colJanCD.ReadOnly = true;
            this.colJanCD.Width = 130;
            // 
            // colStockSu
            // 
            this.colStockSu.DataPropertyName = "StockSu";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colStockSu.DefaultCellStyle = dataGridViewCellStyle3;
            this.colStockSu.HeaderText = "在庫数";
            this.colStockSu.MaxInputLength = 5;
            this.colStockSu.Name = "colStockSu";
            this.colStockSu.ReadOnly = true;
            // 
            // colStockNo
            // 
            this.colStockNo.DataPropertyName = "StockNo";
            this.colStockNo.HeaderText = "StockNo";
            this.colStockNo.Name = "colStockNo";
            this.colStockNo.ReadOnly = true;
            this.colStockNo.Visible = false;
            this.colStockNo.Width = 50;
            // 
            // btnReleaseAll
            // 
            this.btnReleaseAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnReleaseAll.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnReleaseAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReleaseAll.DefaultBtnSize = false;
            this.btnReleaseAll.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnReleaseAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReleaseAll.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnReleaseAll.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnReleaseAll.Location = new System.Drawing.Point(1398, 83);
            this.btnReleaseAll.Margin = new System.Windows.Forms.Padding(1);
            this.btnReleaseAll.Name = "btnReleaseAll";
            this.btnReleaseAll.Size = new System.Drawing.Size(118, 28);
            this.btnReleaseAll.TabIndex = 9;
            this.btnReleaseAll.Text = "全解除";
            this.btnReleaseAll.UseVisualStyleBackColor = false;
            this.btnReleaseAll.Click += new System.EventHandler(this.btnReleaseAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnSelectAll.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnSelectAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectAll.DefaultBtnSize = false;
            this.btnSelectAll.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectAll.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnSelectAll.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnSelectAll.Location = new System.Drawing.Point(1281, 83);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(1);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(118, 28);
            this.btnSelectAll.TabIndex = 8;
            this.btnSelectAll.Text = "全選択";
            this.btnSelectAll.UseVisualStyleBackColor = false;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnApplicable
            // 
            this.btnApplicable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnApplicable.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnApplicable.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApplicable.DefaultBtnSize = false;
            this.btnApplicable.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnApplicable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplicable.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnApplicable.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnApplicable.Location = new System.Drawing.Point(1113, 83);
            this.btnApplicable.Margin = new System.Windows.Forms.Padding(1);
            this.btnApplicable.Name = "btnApplicable";
            this.btnApplicable.Size = new System.Drawing.Size(118, 28);
            this.btnApplicable.TabIndex = 7;
            this.btnApplicable.Text = "適用";
            this.btnApplicable.UseVisualStyleBackColor = false;
            this.btnApplicable.Click += new System.EventHandler(this.btnApplicable_Click);
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
            this.btnDisplay.Location = new System.Drawing.Point(1576, 83);
            this.btnDisplay.Margin = new System.Windows.Forms.Padding(1);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(118, 28);
            this.btnDisplay.TabIndex = 10;
            this.btnDisplay.Text = "表示(F11)";
            this.btnDisplay.UseVisualStyleBackColor = false;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // chkRegister
            // 
            this.chkRegister.AutoSize = true;
            this.chkRegister.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkRegister.Location = new System.Drawing.Point(168, 95);
            this.chkRegister.Name = "chkRegister";
            this.chkRegister.Size = new System.Drawing.Size(63, 16);
            this.chkRegister.TabIndex = 5;
            this.chkRegister.Text = "登録済";
            this.chkRegister.UseVisualStyleBackColor = true;
            // 
            // chkNotRegister
            // 
            this.chkNotRegister.AutoSize = true;
            this.chkNotRegister.Checked = true;
            this.chkNotRegister.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNotRegister.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkNotRegister.Location = new System.Drawing.Point(80, 95);
            this.chkNotRegister.Name = "chkNotRegister";
            this.chkNotRegister.Size = new System.Drawing.Size(63, 16);
            this.chkNotRegister.TabIndex = 4;
            this.chkNotRegister.Text = "未登録";
            this.chkNotRegister.UseVisualStyleBackColor = true;
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
            this.ckM_Label4.Location = new System.Drawing.Point(46, 96);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label4.TabIndex = 6;
            this.ckM_Label4.Text = "棚番";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboWarehouse
            // 
            this.cboWarehouse.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboWarehouse.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboWarehouse.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.WarehouseSelectAll;
            this.cboWarehouse.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboWarehouse.Flag = 0;
            this.cboWarehouse.FormattingEnabled = true;
            this.cboWarehouse.Length = 10;
            this.cboWarehouse.Location = new System.Drawing.Point(80, 58);
            this.cboWarehouse.MaxLength = 10;
            this.cboWarehouse.MoveNext = true;
            this.cboWarehouse.Name = "cboWarehouse";
            this.cboWarehouse.Size = new System.Drawing.Size(265, 20);
            this.cboWarehouse.TabIndex = 3;
            this.cboWarehouse.SelectedIndexChanged += new System.EventHandler(this.cboWarehouse_SelectedIndexChanged);
            // 
            // txtArrivalDateTo
            // 
            this.txtArrivalDateTo.AllowMinus = false;
            this.txtArrivalDateTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtArrivalDateTo.BackColor = System.Drawing.Color.White;
            this.txtArrivalDateTo.BorderColor = false;
            this.txtArrivalDateTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtArrivalDateTo.ClientColor = System.Drawing.Color.White;
            this.txtArrivalDateTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtArrivalDateTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtArrivalDateTo.DecimalPlace = 0;
            this.txtArrivalDateTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtArrivalDateTo.IntegerPart = 0;
            this.txtArrivalDateTo.IsCorrectDate = true;
            this.txtArrivalDateTo.isEnterKeyDown = false;
            this.txtArrivalDateTo.IsFirstTime = true;
            this.txtArrivalDateTo.isMaxLengthErr = false;
            this.txtArrivalDateTo.IsNumber = true;
            this.txtArrivalDateTo.IsShop = false;
            this.txtArrivalDateTo.Length = 10;
            this.txtArrivalDateTo.Location = new System.Drawing.Point(245, 19);
            this.txtArrivalDateTo.MaxLength = 10;
            this.txtArrivalDateTo.MoveNext = true;
            this.txtArrivalDateTo.Name = "txtArrivalDateTo";
            this.txtArrivalDateTo.Size = new System.Drawing.Size(100, 19);
            this.txtArrivalDateTo.TabIndex = 2;
            this.txtArrivalDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtArrivalDateTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtArrivalDateTo.UseColorSizMode = false;
            this.txtArrivalDateTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtArrivalDateTo_KeyDown);
            // 
            // txtArrivalDateFrom
            // 
            this.txtArrivalDateFrom.AllowMinus = false;
            this.txtArrivalDateFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtArrivalDateFrom.BackColor = System.Drawing.Color.White;
            this.txtArrivalDateFrom.BorderColor = false;
            this.txtArrivalDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtArrivalDateFrom.ClientColor = System.Drawing.Color.White;
            this.txtArrivalDateFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtArrivalDateFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtArrivalDateFrom.DecimalPlace = 0;
            this.txtArrivalDateFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtArrivalDateFrom.IntegerPart = 0;
            this.txtArrivalDateFrom.IsCorrectDate = true;
            this.txtArrivalDateFrom.isEnterKeyDown = false;
            this.txtArrivalDateFrom.IsFirstTime = true;
            this.txtArrivalDateFrom.isMaxLengthErr = false;
            this.txtArrivalDateFrom.IsNumber = true;
            this.txtArrivalDateFrom.IsShop = false;
            this.txtArrivalDateFrom.Length = 10;
            this.txtArrivalDateFrom.Location = new System.Drawing.Point(80, 19);
            this.txtArrivalDateFrom.MaxLength = 10;
            this.txtArrivalDateFrom.MoveNext = true;
            this.txtArrivalDateFrom.Name = "txtArrivalDateFrom";
            this.txtArrivalDateFrom.Size = new System.Drawing.Size(100, 19);
            this.txtArrivalDateFrom.TabIndex = 1;
            this.txtArrivalDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtArrivalDateFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtArrivalDateFrom.UseColorSizMode = false;
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
            this.ckM_Label3.Location = new System.Drawing.Point(204, 22);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label3.TabIndex = 2;
            this.ckM_Label3.Text = "～";
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
            this.ckM_Label2.Location = new System.Drawing.Point(46, 62);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label2.TabIndex = 1;
            this.ckM_Label2.Text = "倉庫";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label1.Location = new System.Drawing.Point(33, 22);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label1.TabIndex = 0;
            this.ckM_Label1.Text = "入荷日";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmTanabanNyuuryoku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.panelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "FrmTanabanNyuuryoku";
            this.PanelHeaderHeight = 54;
            this.Text = "TanabanNyuuryoku";
            this.Load += new System.EventHandler(this.FrmTanabanNyuuryoku_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmTanabanNyuuryoku_KeyUp);
            this.Controls.SetChildIndex(this.panelDetail, 0);
            this.panelDetail.ResumeLayout(false);
            this.panelDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTanaban)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelDetail;
        private CKM_Controls.CKM_ComboBox cboWarehouse;
        private CKM_Controls.CKM_TextBox txtArrivalDateTo;
        private CKM_Controls.CKM_TextBox txtArrivalDateFrom;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_CheckBox chkRegister;
        private CKM_Controls.CKM_CheckBox chkNotRegister;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Button btnReleaseAll;
        private CKM_Controls.CKM_Button btnSelectAll;
        private CKM_Controls.CKM_Button btnApplicable;
        private CKM_Controls.CKM_Button btnDisplay;
        private CKM_Controls.CKM_GridView dgvTanaban;
        private Search.CKM_SearchControl ScStorage;
        private CKM_Controls.CKM_Label ckM_Label5;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colChk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRackNo1;
        private System.Windows.Forms.DataGridViewButtonColumn colBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRackNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSizeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJanCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockSu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockNo;
    }
}