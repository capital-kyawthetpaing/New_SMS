namespace TempoRegiZaikoKakunin
{
    partial class frmTempoRegiZaikoKakunin
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnInquery = new CKM_Controls.CKM_Button();
            this.lblProduct = new CKM_Controls.CKMShop_Label();
            this.lblplandate = new CKM_Controls.CKMShop_Label();
            this.lblsou = new CKM_Controls.CKMShop_Label();
            this.lblallowsou = new CKM_Controls.CKMShop_Label();
            this.dgvZaikokakunin = new CKM_Controls.CKMShop_GridView();
            this.colWarehouse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblZaiko = new CKM_Controls.CKMShop_Label();
            this.ckmShop_Label1 = new CKM_Controls.CKMShop_Label();
            this.ckmShop_Label3 = new CKM_Controls.CKMShop_Label();
            this.chkColorSize = new CKM_Controls.CKMShop_CheckBox();
            this.lblRyousyuusho = new CKM_Controls.CKMShop_Label();
            this.txtbin = new CKM_Controls.CKM_TextBox();
            this.txtProductName = new CKM_Controls.CKM_TextBox();
            this.txtJanCD = new CKM_Controls.CKM_TextBox();
            this.btnProductName = new CKM_Controls.CKM_Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvZaikokakunin)).BeginInit();
            this.SuspendLayout();
            // 
            // btnInquery
            // 
            this.btnInquery.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(242)))), ((int)(((byte)(204)))));
            this.btnInquery.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Yellow;
            this.btnInquery.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInquery.DefaultBtnSize = true;
            this.btnInquery.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnInquery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInquery.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.btnInquery.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Medium;
            this.btnInquery.ForeColor = System.Drawing.Color.Black;
            this.btnInquery.Location = new System.Drawing.Point(1751, 198);
            this.btnInquery.Margin = new System.Windows.Forms.Padding(1);
            this.btnInquery.Name = "btnInquery";
            this.btnInquery.Size = new System.Drawing.Size(150, 45);
            this.btnInquery.TabIndex = 2;
            this.btnInquery.Text = "照会";
            this.btnInquery.UseVisualStyleBackColor = false;
            this.btnInquery.Click += new System.EventHandler(this.btnInquery_Click);
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblProduct.BackColor = System.Drawing.Color.Transparent;
            this.lblProduct.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.lblProduct.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.lblProduct.FontBold = true;
            this.lblProduct.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblProduct.Location = new System.Drawing.Point(436, 256);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(875, 35);
            this.lblProduct.TabIndex = 53;
            this.lblProduct.Text = "品　番／商品名             JANCD／カラーサイズ\t\t";
            this.lblProduct.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblplandate
            // 
            this.lblplandate.AutoSize = true;
            this.lblplandate.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblplandate.BackColor = System.Drawing.Color.Transparent;
            this.lblplandate.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.lblplandate.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.lblplandate.FontBold = true;
            this.lblplandate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblplandate.Location = new System.Drawing.Point(1323, 256);
            this.lblplandate.Name = "lblplandate";
            this.lblplandate.Size = new System.Drawing.Size(200, 35);
            this.lblplandate.TabIndex = 54;
            this.lblplandate.Text = "入荷予定日";
            this.lblplandate.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblplandate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblsou
            // 
            this.lblsou.AutoSize = true;
            this.lblsou.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblsou.BackColor = System.Drawing.Color.Transparent;
            this.lblsou.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.lblsou.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.lblsou.FontBold = true;
            this.lblsou.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblsou.Location = new System.Drawing.Point(1561, 256);
            this.lblsou.Name = "lblsou";
            this.lblsou.Size = new System.Drawing.Size(126, 35);
            this.lblsou.TabIndex = 55;
            this.lblsou.Text = "在庫数";
            this.lblsou.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblsou.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblallowsou
            // 
            this.lblallowsou.AutoSize = true;
            this.lblallowsou.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblallowsou.BackColor = System.Drawing.Color.Transparent;
            this.lblallowsou.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.lblallowsou.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.lblallowsou.FontBold = true;
            this.lblallowsou.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblallowsou.Location = new System.Drawing.Point(1691, 257);
            this.lblallowsou.Name = "lblallowsou";
            this.lblallowsou.Size = new System.Drawing.Size(145, 35);
            this.lblallowsou.TabIndex = 56;
            this.lblallowsou.Text = " 可能数";
            this.lblallowsou.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblallowsou.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvZaikokakunin
            // 
            this.dgvZaikokakunin.AllowUserToAddRows = false;
            this.dgvZaikokakunin.AllowUserToDeleteRows = false;
            this.dgvZaikokakunin.AllowUserToResizeRows = false;
            this.dgvZaikokakunin.AlterBackColor = CKM_Controls.CKMShop_GridView.AltBackcolor.Control;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.dgvZaikokakunin.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvZaikokakunin.BackgroundColor = System.Drawing.Color.White;
            this.dgvZaikokakunin.BackgroungColor = CKM_Controls.CKMShop_GridView.DBackcolor.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS Gothic", 26F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvZaikokakunin.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvZaikokakunin.ColumnHeadersHeight = 22;
            this.dgvZaikokakunin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvZaikokakunin.ColumnHeadersVisible = false;
            this.dgvZaikokakunin.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWarehouse,
            this.colItemCD,
            this.colDate,
            this.colQuantity,
            this.colNo});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("MS Gothic", 26F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvZaikokakunin.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvZaikokakunin.DGVback = CKM_Controls.CKMShop_GridView.DGVBackcolor.White;
            this.dgvZaikokakunin.EnableHeadersVisualStyles = false;
            this.dgvZaikokakunin.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.dgvZaikokakunin.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvZaikokakunin.GVFontstyle = CKM_Controls.CKMShop_GridView.FontStyle_.Regular;
            this.dgvZaikokakunin.HeaderHeight_ = 22;
            this.dgvZaikokakunin.HeaderVisible = false;
            this.dgvZaikokakunin.Height_ = 550;
            this.dgvZaikokakunin.Location = new System.Drawing.Point(34, 295);
            this.dgvZaikokakunin.Name = "dgvZaikokakunin";
            this.dgvZaikokakunin.RowHeadersWidth = 70;
            this.dgvZaikokakunin.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvZaikokakunin.RowHeight_ = 80;
            this.dgvZaikokakunin.RowTemplate.Height = 80;
            this.dgvZaikokakunin.ShopFontSize = CKM_Controls.CKMShop_GridView.Font_.Medium;
            this.dgvZaikokakunin.Size = new System.Drawing.Size(1830, 650);
            this.dgvZaikokakunin.TabIndex = 48;
            this.dgvZaikokakunin.UseRowNo = true;
            this.dgvZaikokakunin.UseSetting = true;
            this.dgvZaikokakunin.Width_ = 1830;
            this.dgvZaikokakunin.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvZaikokakunin_CellDoubleClick);
            // 
            // colWarehouse
            // 
            this.colWarehouse.DataPropertyName = "SoukoName";
            this.colWarehouse.HeaderText = "在庫倉庫";
            this.colWarehouse.Name = "colWarehouse";
            this.colWarehouse.ReadOnly = true;
            this.colWarehouse.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colWarehouse.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colWarehouse.Width = 280;
            // 
            // colItemCD
            // 
            this.colItemCD.DataPropertyName = "ItemCD";
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.colItemCD.DefaultCellStyle = dataGridViewCellStyle3;
            this.colItemCD.HeaderText = "品番\t\t";
            this.colItemCD.Name = "colItemCD";
            this.colItemCD.ReadOnly = true;
            this.colItemCD.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colItemCD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colItemCD.Width = 925;
            // 
            // colDate
            // 
            this.colDate.DataPropertyName = "StockDate";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.colDate.HeaderText = "入荷予定日";
            this.colDate.MaxInputLength = 8;
            this.colDate.Name = "colDate";
            this.colDate.ReadOnly = true;
            this.colDate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDate.Width = 230;
            // 
            // colQuantity
            // 
            this.colQuantity.DataPropertyName = "StockNum";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colQuantity.DefaultCellStyle = dataGridViewCellStyle5;
            this.colQuantity.HeaderText = "在庫数";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.ReadOnly = true;
            this.colQuantity.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colQuantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colQuantity.Width = 150;
            // 
            // colNo
            // 
            this.colNo.DataPropertyName = "KanoSu";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colNo.DefaultCellStyle = dataGridViewCellStyle6;
            this.colNo.HeaderText = "可能数";
            this.colNo.Name = "colNo";
            this.colNo.ReadOnly = true;
            this.colNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colNo.Width = 150;
            // 
            // lblZaiko
            // 
            this.lblZaiko.AutoSize = true;
            this.lblZaiko.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblZaiko.BackColor = System.Drawing.Color.Transparent;
            this.lblZaiko.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.lblZaiko.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.lblZaiko.FontBold = true;
            this.lblZaiko.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblZaiko.Location = new System.Drawing.Point(125, 255);
            this.lblZaiko.Name = "lblZaiko";
            this.lblZaiko.Size = new System.Drawing.Size(163, 35);
            this.lblZaiko.TabIndex = 52;
            this.lblZaiko.Text = "在庫倉庫";
            this.lblZaiko.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblZaiko.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckmShop_Label1
            // 
            this.ckmShop_Label1.AutoSize = true;
            this.ckmShop_Label1.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label1.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label1.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label1.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label1.FontBold = true;
            this.ckmShop_Label1.ForeColor = System.Drawing.Color.Black;
            this.ckmShop_Label1.Location = new System.Drawing.Point(67, 81);
            this.ckmShop_Label1.Name = "ckmShop_Label1";
            this.ckmShop_Label1.Size = new System.Drawing.Size(126, 35);
            this.ckmShop_Label1.TabIndex = 57;
            this.ckmShop_Label1.Text = "品　番";
            this.ckmShop_Label1.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            // 
            // ckmShop_Label3
            // 
            this.ckmShop_Label3.AutoSize = true;
            this.ckmShop_Label3.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label3.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label3.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label3.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label3.FontBold = true;
            this.ckmShop_Label3.ForeColor = System.Drawing.Color.Black;
            this.ckmShop_Label3.Location = new System.Drawing.Point(76, 165);
            this.ckmShop_Label3.Name = "ckmShop_Label3";
            this.ckmShop_Label3.Size = new System.Drawing.Size(110, 35);
            this.ckmShop_Label3.TabIndex = 59;
            this.ckmShop_Label3.Text = "JANCD";
            this.ckmShop_Label3.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            // 
            // chkColorSize
            // 
            this.chkColorSize.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.chkColorSize.Checked = true;
            this.chkColorSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkColorSize.Font = new System.Drawing.Font("MS Gothic", 18F, System.Drawing.FontStyle.Bold);
            this.chkColorSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.chkColorSize.IsattachedCaption = false;
            this.chkColorSize.Location = new System.Drawing.Point(368, 208);
            this.chkColorSize.Name = "chkColorSize";
            this.chkColorSize.Size = new System.Drawing.Size(35, 35);
            this.chkColorSize.TabIndex = 69;
            this.chkColorSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkColorSize.UseVisualStyleBackColor = true;
            // 
            // lblRyousyuusho
            // 
            this.lblRyousyuusho.AutoSize = true;
            this.lblRyousyuusho.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblRyousyuusho.BackColor = System.Drawing.Color.Transparent;
            this.lblRyousyuusho.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.lblRyousyuusho.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.lblRyousyuusho.FontBold = true;
            this.lblRyousyuusho.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblRyousyuusho.Location = new System.Drawing.Point(128, 208);
            this.lblRyousyuusho.Name = "lblRyousyuusho";
            this.lblRyousyuusho.Size = new System.Drawing.Size(237, 35);
            this.lblRyousyuusho.TabIndex = 70;
            this.lblRyousyuusho.Text = "色サイズ違い";
            this.lblRyousyuusho.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblRyousyuusho.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtbin
            // 
            this.txtbin.AllowMinus = true;
            this.txtbin.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.Green;
            this.txtbin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtbin.BorderColor = false;
            this.txtbin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtbin.ClientColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtbin.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtbin.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtbin.DecimalPlace = 0;
            this.txtbin.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.txtbin.IntegerPart = 8;
            this.txtbin.IsCorrectDate = true;
            this.txtbin.isEnterKeyDown = false;
            this.txtbin.IsFirstTime = true;
            this.txtbin.isMaxLengthErr = false;
            this.txtbin.IsNumber = true;
            this.txtbin.IsShop = false;
            this.txtbin.IsTimemmss = false;
            this.txtbin.Length = 30;
            this.txtbin.Location = new System.Drawing.Point(214, 77);
            this.txtbin.MaxLength = 30;
            this.txtbin.MoveNext = true;
            this.txtbin.Name = "txtbin";
            this.txtbin.Size = new System.Drawing.Size(700, 42);
            this.txtbin.TabIndex = 71;
            this.txtbin.Text = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            this.txtbin.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            this.txtbin.UseColorSizMode = false;
            // 
            // txtProductName
            // 
            this.txtProductName.AllowMinus = true;
            this.txtProductName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.Green;
            this.txtProductName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtProductName.BorderColor = false;
            this.txtProductName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProductName.ClientColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtProductName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtProductName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtProductName.DecimalPlace = 0;
            this.txtProductName.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.txtProductName.IntegerPart = 8;
            this.txtProductName.IsCorrectDate = true;
            this.txtProductName.isEnterKeyDown = false;
            this.txtProductName.IsFirstTime = true;
            this.txtProductName.isMaxLengthErr = false;
            this.txtProductName.IsNumber = true;
            this.txtProductName.IsShop = false;
            this.txtProductName.IsTimemmss = false;
            this.txtProductName.Length = 100;
            this.txtProductName.Location = new System.Drawing.Point(214, 118);
            this.txtProductName.MaxLength = 100;
            this.txtProductName.MoveNext = true;
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(1200, 42);
            this.txtProductName.TabIndex = 72;
            this.txtProductName.Text = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t" +
    "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t";
            this.txtProductName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            this.txtProductName.UseColorSizMode = false;
            // 
            // txtJanCD
            // 
            this.txtJanCD.AllowMinus = true;
            this.txtJanCD.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.Green;
            this.txtJanCD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtJanCD.BorderColor = false;
            this.txtJanCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJanCD.ClientColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtJanCD.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtJanCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtJanCD.DecimalPlace = 0;
            this.txtJanCD.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.txtJanCD.IntegerPart = 8;
            this.txtJanCD.IsCorrectDate = true;
            this.txtJanCD.isEnterKeyDown = false;
            this.txtJanCD.IsFirstTime = true;
            this.txtJanCD.isMaxLengthErr = false;
            this.txtJanCD.IsNumber = true;
            this.txtJanCD.IsShop = false;
            this.txtJanCD.IsTimemmss = false;
            this.txtJanCD.Length = 13;
            this.txtJanCD.Location = new System.Drawing.Point(214, 159);
            this.txtJanCD.MaxLength = 13;
            this.txtJanCD.MoveNext = true;
            this.txtJanCD.Name = "txtJanCD";
            this.txtJanCD.Size = new System.Drawing.Size(300, 42);
            this.txtJanCD.TabIndex = 73;
            this.txtJanCD.Text = "XXXXXXXXXXXXX\t\t\t\t\t\t\t\t\t\t";
            this.txtJanCD.TextSize = CKM_Controls.CKM_TextBox.FontSize.Medium;
            this.txtJanCD.UseColorSizMode = false;
            this.txtJanCD.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtJanCD_KeyDown);
            // 
            // btnProductName
            // 
            this.btnProductName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(242)))), ((int)(((byte)(204)))));
            this.btnProductName.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Yellow;
            this.btnProductName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProductName.DefaultBtnSize = true;
            this.btnProductName.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnProductName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProductName.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.btnProductName.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Medium;
            this.btnProductName.ForeColor = System.Drawing.Color.Black;
            this.btnProductName.Location = new System.Drawing.Point(64, 118);
            this.btnProductName.Margin = new System.Windows.Forms.Padding(1);
            this.btnProductName.Name = "btnProductName";
            this.btnProductName.Size = new System.Drawing.Size(150, 45);
            this.btnProductName.TabIndex = 74;
            this.btnProductName.Text = "商品名";
            this.btnProductName.UseVisualStyleBackColor = false;
            this.btnProductName.Click += new System.EventHandler(this.btnProductName_Click);
            // 
            // frmTempoRegiZaikoKakunin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1913, 961);
            this.Controls.Add(this.btnProductName);
            this.Controls.Add(this.txtJanCD);
            this.Controls.Add(this.txtProductName);
            this.Controls.Add(this.txtbin);
            this.Controls.Add(this.chkColorSize);
            this.Controls.Add(this.lblRyousyuusho);
            this.Controls.Add(this.ckmShop_Label3);
            this.Controls.Add(this.ckmShop_Label1);
            this.Controls.Add(this.lblallowsou);
            this.Controls.Add(this.lblsou);
            this.Controls.Add(this.lblplandate);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.lblZaiko);
            this.Controls.Add(this.dgvZaikokakunin);
            this.Controls.Add(this.btnInquery);
            this.Name = "frmTempoRegiZaikoKakunin";
            this.Text = "店舗レジ 在庫確認";
            this.Load += new System.EventHandler(this.frmTempoRegiZaikoKakunin_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmTempoRegiZaikoKakunin_KeyUp);
            this.Controls.SetChildIndex(this.btnInquery, 0);
            this.Controls.SetChildIndex(this.dgvZaikokakunin, 0);
            this.Controls.SetChildIndex(this.lblZaiko, 0);
            this.Controls.SetChildIndex(this.lblProduct, 0);
            this.Controls.SetChildIndex(this.lblplandate, 0);
            this.Controls.SetChildIndex(this.lblsou, 0);
            this.Controls.SetChildIndex(this.lblallowsou, 0);
            this.Controls.SetChildIndex(this.ckmShop_Label1, 0);
            this.Controls.SetChildIndex(this.ckmShop_Label3, 0);
            this.Controls.SetChildIndex(this.lblRyousyuusho, 0);
            this.Controls.SetChildIndex(this.chkColorSize, 0);
            this.Controls.SetChildIndex(this.txtbin, 0);
            this.Controls.SetChildIndex(this.txtProductName, 0);
            this.Controls.SetChildIndex(this.txtJanCD, 0);
            this.Controls.SetChildIndex(this.btnProductName, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvZaikokakunin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CKM_Controls.CKM_Button btnInquery;
        private CKM_Controls.CKMShop_Label lblProduct;
        private CKM_Controls.CKMShop_Label lblplandate;
        private CKM_Controls.CKMShop_Label lblsou;
        private CKM_Controls.CKMShop_Label lblallowsou;
        private CKM_Controls.CKMShop_GridView dgvZaikokakunin;
        private CKM_Controls.CKMShop_Label lblZaiko;
        private CKM_Controls.CKMShop_Label ckmShop_Label1;
        private CKM_Controls.CKMShop_Label ckmShop_Label3;
        private CKM_Controls.CKMShop_CheckBox chkColorSize;
        private CKM_Controls.CKMShop_Label lblRyousyuusho;
        private CKM_Controls.CKM_TextBox txtbin;
        private CKM_Controls.CKM_TextBox txtProductName;
        private CKM_Controls.CKM_TextBox txtJanCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWarehouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
        private CKM_Controls.CKM_Button btnProductName;
    }
}