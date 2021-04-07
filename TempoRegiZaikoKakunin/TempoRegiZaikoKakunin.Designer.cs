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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnInquery = new CKM_Controls.CKM_Button();
            this.lblProduct = new CKM_Controls.CKMShop_Label();
            this.lblplandate = new CKM_Controls.CKMShop_Label();
            this.lblsou = new CKM_Controls.CKMShop_Label();
            this.lblallowsou = new CKM_Controls.CKMShop_Label();
            this.dgvZaikokakunin = new CKM_Controls.CKMShop_GridView();
            this.colWarehouse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProduct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblZaiko = new CKM_Controls.CKMShop_Label();
            this.ckmShop_Label1 = new CKM_Controls.CKMShop_Label();
            this.ckmShop_Label2 = new CKM_Controls.CKMShop_Label();
            this.ckmShop_Label3 = new CKM_Controls.CKMShop_Label();
            this.lblCustomerNo = new CKM_Controls.CKMShop_Label();
            this.ckmShop_Label4 = new CKM_Controls.CKMShop_Label();
            this.txtJanCD = new CKM_Controls.CKMShop_Label();
            this.chkColorSize = new CKM_Controls.CKMShop_CheckBox();
            this.lblRyousyuusho = new CKM_Controls.CKMShop_Label();
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
            this.lblProduct.Size = new System.Drawing.Size(874, 35);
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
            this.dgvZaikokakunin.AllowUserToDeleteRows = false;
            this.dgvZaikokakunin.AllowUserToResizeRows = false;
            this.dgvZaikokakunin.AlterBackColor = CKM_Controls.CKMShop_GridView.AltBackcolor.Control;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            this.dgvZaikokakunin.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvZaikokakunin.BackgroundColor = System.Drawing.Color.White;
            this.dgvZaikokakunin.BackgroungColor = CKM_Controls.CKMShop_GridView.DBackcolor.White;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle14.Font = new System.Drawing.Font("MS Gothic", 26F);
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvZaikokakunin.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dgvZaikokakunin.ColumnHeadersHeight = 22;
            this.dgvZaikokakunin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvZaikokakunin.ColumnHeadersVisible = false;
            this.dgvZaikokakunin.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWarehouse,
            this.colProduct,
            this.colDate,
            this.colQuantity,
            this.colNo});
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("MS Gothic", 26F);
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvZaikokakunin.DefaultCellStyle = dataGridViewCellStyle18;
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
            this.dgvZaikokakunin.RowHeight_ = 50;
            this.dgvZaikokakunin.RowTemplate.Height = 50;
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
            this.colWarehouse.Width = 330;
            // 
            // colProduct
            // 
            this.colProduct.DataPropertyName = "JanCD";
            this.colProduct.HeaderText = "商　品";
            this.colProduct.Name = "colProduct";
            this.colProduct.ReadOnly = true;
            this.colProduct.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colProduct.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colProduct.Width = 875;
            // 
            // colDate
            // 
            this.colDate.DataPropertyName = "StockDate";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colDate.DefaultCellStyle = dataGridViewCellStyle15;
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
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colQuantity.DefaultCellStyle = dataGridViewCellStyle16;
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
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colNo.DefaultCellStyle = dataGridViewCellStyle17;
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
            this.ckmShop_Label1.Font = new System.Drawing.Font("MS Gothic", 22F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label1.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label1.FontBold = true;
            this.ckmShop_Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label1.Location = new System.Drawing.Point(63, 84);
            this.ckmShop_Label1.Name = "ckmShop_Label1";
            this.ckmShop_Label1.Size = new System.Drawing.Size(106, 30);
            this.ckmShop_Label1.TabIndex = 57;
            this.ckmShop_Label1.Text = "品　番";
            this.ckmShop_Label1.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            // 
            // ckmShop_Label2
            // 
            this.ckmShop_Label2.AutoSize = true;
            this.ckmShop_Label2.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label2.Font = new System.Drawing.Font("MS Gothic", 22F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label2.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label2.FontBold = true;
            this.ckmShop_Label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label2.Location = new System.Drawing.Point(63, 125);
            this.ckmShop_Label2.Name = "ckmShop_Label2";
            this.ckmShop_Label2.Size = new System.Drawing.Size(106, 30);
            this.ckmShop_Label2.TabIndex = 58;
            this.ckmShop_Label2.Text = "商品名";
            this.ckmShop_Label2.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            // 
            // ckmShop_Label3
            // 
            this.ckmShop_Label3.AutoSize = true;
            this.ckmShop_Label3.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label3.Font = new System.Drawing.Font("MS Gothic", 22F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label3.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label3.FontBold = true;
            this.ckmShop_Label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label3.Location = new System.Drawing.Point(76, 163);
            this.ckmShop_Label3.Name = "ckmShop_Label3";
            this.ckmShop_Label3.Size = new System.Drawing.Size(93, 30);
            this.ckmShop_Label3.TabIndex = 59;
            this.ckmShop_Label3.Text = "JANCD";
            this.ckmShop_Label3.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            // 
            // lblCustomerNo
            // 
            this.lblCustomerNo.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblCustomerNo.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomerNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCustomerNo.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.lblCustomerNo.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Medium0;
            this.lblCustomerNo.FontBold = false;
            this.lblCustomerNo.ForeColor = System.Drawing.Color.Black;
            this.lblCustomerNo.Location = new System.Drawing.Point(172, 78);
            this.lblCustomerNo.Name = "lblCustomerNo";
            this.lblCustomerNo.Size = new System.Drawing.Size(650, 42);
            this.lblCustomerNo.TabIndex = 66;
            this.lblCustomerNo.Text = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            this.lblCustomerNo.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Black;
            this.lblCustomerNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ckmShop_Label4
            // 
            this.ckmShop_Label4.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label4.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckmShop_Label4.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.ckmShop_Label4.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Medium0;
            this.ckmShop_Label4.FontBold = false;
            this.ckmShop_Label4.ForeColor = System.Drawing.Color.Black;
            this.ckmShop_Label4.Location = new System.Drawing.Point(172, 119);
            this.ckmShop_Label4.Name = "ckmShop_Label4";
            this.ckmShop_Label4.Size = new System.Drawing.Size(1200, 42);
            this.ckmShop_Label4.TabIndex = 67;
            this.ckmShop_Label4.Text = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            this.ckmShop_Label4.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Black;
            this.ckmShop_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtJanCD
            // 
            this.txtJanCD.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.txtJanCD.BackColor = System.Drawing.Color.Transparent;
            this.txtJanCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJanCD.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.txtJanCD.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Medium0;
            this.txtJanCD.FontBold = false;
            this.txtJanCD.ForeColor = System.Drawing.Color.Black;
            this.txtJanCD.Location = new System.Drawing.Point(172, 160);
            this.txtJanCD.Name = "txtJanCD";
            this.txtJanCD.Size = new System.Drawing.Size(300, 42);
            this.txtJanCD.TabIndex = 68;
            this.txtJanCD.Text = "XXXXXXXXXXXXX";
            this.txtJanCD.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Black;
            this.txtJanCD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // frmTempoRegiZaikoKakunin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1913, 961);
            this.Controls.Add(this.chkColorSize);
            this.Controls.Add(this.lblRyousyuusho);
            this.Controls.Add(this.txtJanCD);
            this.Controls.Add(this.ckmShop_Label4);
            this.Controls.Add(this.lblCustomerNo);
            this.Controls.Add(this.ckmShop_Label3);
            this.Controls.Add(this.ckmShop_Label2);
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
            this.Controls.SetChildIndex(this.ckmShop_Label2, 0);
            this.Controls.SetChildIndex(this.ckmShop_Label3, 0);
            this.Controls.SetChildIndex(this.lblCustomerNo, 0);
            this.Controls.SetChildIndex(this.ckmShop_Label4, 0);
            this.Controls.SetChildIndex(this.txtJanCD, 0);
            this.Controls.SetChildIndex(this.lblRyousyuusho, 0);
            this.Controls.SetChildIndex(this.chkColorSize, 0);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colWarehouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
        private CKM_Controls.CKMShop_Label lblZaiko;
        private CKM_Controls.CKMShop_Label ckmShop_Label1;
        private CKM_Controls.CKMShop_Label ckmShop_Label2;
        private CKM_Controls.CKMShop_Label ckmShop_Label3;
        private CKM_Controls.CKMShop_Label lblCustomerNo;
        private CKM_Controls.CKMShop_Label ckmShop_Label4;
        private CKM_Controls.CKMShop_Label txtJanCD;
        private CKM_Controls.CKMShop_CheckBox chkColorSize;
        private CKM_Controls.CKMShop_Label lblRyousyuusho;
    }
}