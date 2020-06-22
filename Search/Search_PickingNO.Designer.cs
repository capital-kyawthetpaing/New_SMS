namespace Search
{
    partial class Search_PickingNO
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.CboSoukoCD = new CKM_Controls.CKM_ComboBox();
            this.btnSubF11 = new CKM_Controls.CKM_Button();
            this.GvDetail = new CKM_Controls.CKM_GridView();
            this.colPickingNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.coIPrintDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPickingKBN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeliveryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ckM_TextBox1 = new CKM_Controls.CKM_TextBox();
            this.ckM_Label8 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox2 = new CKM_Controls.CKM_TextBox();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.ckM_Label8);
            this.PanelHeader.Controls.Add(this.ckM_TextBox2);
            this.PanelHeader.Controls.Add(this.btnSubF11);
            this.PanelHeader.Controls.Add(this.CboSoukoCD);
            this.PanelHeader.Controls.Add(this.ckM_Label6);
            this.PanelHeader.Controls.Add(this.ckM_TextBox1);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Size = new System.Drawing.Size(1026, 98);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label6, 0);
            this.PanelHeader.Controls.SetChildIndex(this.CboSoukoCD, 0);
            this.PanelHeader.Controls.SetChildIndex(this.btnSubF11, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label8, 0);
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
            this.ckM_Label1.Location = new System.Drawing.Point(60, 16);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(148, 12);
            this.ckM_Label1.TabIndex = 2;
            this.ckM_Label1.Text = "ピッキングリスト印刷日";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label6.Location = new System.Drawing.Point(717, 15);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label6.TabIndex = 57;
            this.ckM_Label6.Text = "倉庫";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CboSoukoCD
            // 
            this.CboSoukoCD.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.CboSoukoCD.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CboSoukoCD.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.入荷倉庫;
            this.CboSoukoCD.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.CboSoukoCD.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CboSoukoCD.Flag = 0;
            this.CboSoukoCD.FormattingEnabled = true;
            this.CboSoukoCD.Length = 30;
            this.CboSoukoCD.Location = new System.Drawing.Point(751, 11);
            this.CboSoukoCD.MaxLength = 15;
            this.CboSoukoCD.MoveNext = true;
            this.CboSoukoCD.Name = "CboSoukoCD";
            this.CboSoukoCD.Size = new System.Drawing.Size(265, 20);
            this.CboSoukoCD.TabIndex = 2;
            // 
            // btnSubF11
            // 
            this.btnSubF11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnSubF11.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnSubF11.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubF11.DefaultBtnSize = false;
            this.btnSubF11.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSubF11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubF11.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnSubF11.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnSubF11.Location = new System.Drawing.Point(897, 50);
            this.btnSubF11.Margin = new System.Windows.Forms.Padding(1);
            this.btnSubF11.Name = "btnSubF11";
            this.btnSubF11.Size = new System.Drawing.Size(115, 28);
            this.btnSubF11.TabIndex = 11;
            this.btnSubF11.Text = "表示(F11)";
            this.btnSubF11.UseVisualStyleBackColor = false;
            this.btnSubF11.Click += new System.EventHandler(this.BtnF11_Click);
            // 
            // GvDetail
            // 
            this.GvDetail.AllowUserToAddRows = false;
            this.GvDetail.AllowUserToDeleteRows = false;
            this.GvDetail.AllowUserToResizeRows = false;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.GvDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle16;
            this.GvDetail.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GvDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.GvDetail.ColumnHeadersHeight = 25;
            this.GvDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPickingNO,
            this.coIPrintDateTime,
            this.colPickingKBN,
            this.colSKUName,
            this.DeliveryName});
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("MS Gothic", 9F);
            dataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.GvDetail.DefaultCellStyle = dataGridViewCellStyle20;
            this.GvDetail.EnableHeadersVisualStyles = false;
            this.GvDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GvDetail.Location = new System.Drawing.Point(9, 151);
            this.GvDetail.Name = "GvDetail";
            this.GvDetail.ReadOnly = true;
            this.GvDetail.RowHeight_ = 20;
            this.GvDetail.Size = new System.Drawing.Size(1003, 338);
            this.GvDetail.TabIndex = 20;
            this.GvDetail.UseRowNo = true;
            this.GvDetail.UseSetting = false;
            this.GvDetail.DoubleClick += new System.EventHandler(this.GvDetail_DoubleClick);
            this.GvDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DgvDetail_KeyDown);
            // 
            // colPickingNO
            // 
            this.colPickingNO.DataPropertyName = "PickingNO";
            this.colPickingNO.HeaderText = "ピッキング番号";
            this.colPickingNO.Name = "colPickingNO";
            this.colPickingNO.ReadOnly = true;
            // 
            // coIPrintDateTime
            // 
            this.coIPrintDateTime.DataPropertyName = "PrintDateTime";
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.coIPrintDateTime.DefaultCellStyle = dataGridViewCellStyle18;
            this.coIPrintDateTime.HeaderText = "リスト印刷日";
            this.coIPrintDateTime.Name = "coIPrintDateTime";
            this.coIPrintDateTime.ReadOnly = true;
            // 
            // colPickingKBN
            // 
            this.colPickingKBN.DataPropertyName = "PickingKBN";
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colPickingKBN.DefaultCellStyle = dataGridViewCellStyle19;
            this.colPickingKBN.HeaderText = "戻り";
            this.colPickingKBN.Name = "colPickingKBN";
            this.colPickingKBN.ReadOnly = true;
            this.colPickingKBN.Width = 60;
            // 
            // colSKUName
            // 
            this.colSKUName.DataPropertyName = "SKUName";
            this.colSKUName.HeaderText = "商品名（１行目）";
            this.colSKUName.Name = "colSKUName";
            this.colSKUName.ReadOnly = true;
            this.colSKUName.Width = 350;
            // 
            // DeliveryName
            // 
            this.DeliveryName.DataPropertyName = "DeliveryName";
            this.DeliveryName.HeaderText = "出荷先名（１行目）";
            this.DeliveryName.Name = "DeliveryName";
            this.DeliveryName.ReadOnly = true;
            this.DeliveryName.Width = 350;
            // 
            // ckM_TextBox1
            // 
            this.ckM_TextBox1.AllowMinus = false;
            this.ckM_TextBox1.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox1.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox1.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox1.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.ckM_TextBox1.DecimalPlace = 0;
            this.ckM_TextBox1.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ckM_TextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox1.IntegerPart = 0;
            this.ckM_TextBox1.IsCorrectDate = true;
            this.ckM_TextBox1.isEnterKeyDown = false;
            this.ckM_TextBox1.isMaxLengthErr = false;
            this.ckM_TextBox1.IsNumber = true;
            this.ckM_TextBox1.IsShop = false;
            this.ckM_TextBox1.Length = 10;
            this.ckM_TextBox1.Location = new System.Drawing.Point(211, 12);
            this.ckM_TextBox1.MaxLength = 10;
            this.ckM_TextBox1.MoveNext = true;
            this.ckM_TextBox1.Name = "ckM_TextBox1";
            this.ckM_TextBox1.Size = new System.Drawing.Size(100, 19);
            this.ckM_TextBox1.TabIndex = 0;
            this.ckM_TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_Label8.Location = new System.Drawing.Point(318, 15);
            this.ckM_Label8.Name = "ckM_Label8";
            this.ckM_Label8.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label8.TabIndex = 61;
            this.ckM_Label8.Text = "～";
            this.ckM_Label8.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_TextBox2
            // 
            this.ckM_TextBox2.AllowMinus = false;
            this.ckM_TextBox2.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox2.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox2.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox2.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.ckM_TextBox2.DecimalPlace = 0;
            this.ckM_TextBox2.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ckM_TextBox2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox2.IntegerPart = 0;
            this.ckM_TextBox2.IsCorrectDate = true;
            this.ckM_TextBox2.isEnterKeyDown = false;
            this.ckM_TextBox2.isMaxLengthErr = false;
            this.ckM_TextBox2.IsNumber = true;
            this.ckM_TextBox2.IsShop = false;
            this.ckM_TextBox2.Length = 10;
            this.ckM_TextBox2.Location = new System.Drawing.Point(342, 13);
            this.ckM_TextBox2.MaxLength = 10;
            this.ckM_TextBox2.MoveNext = true;
            this.ckM_TextBox2.Name = "ckM_TextBox2";
            this.ckM_TextBox2.Size = new System.Drawing.Size(100, 19);
            this.ckM_TextBox2.TabIndex = 1;
            this.ckM_TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox2.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // Search_PickingNO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 537);
            this.Controls.Add(this.GvDetail);
            this.F11Visible = true;
            this.F9Visible = true;
            this.Name = "Search_PickingNO";
            this.PanelHeaderHeight = 140;
            this.ProgramName = "ピッキング番号検索";
            this.Text = "Search_PickingNO";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.GvDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_ComboBox CboSoukoCD;
        private CKM_Controls.CKM_Button btnSubF11;
        private CKM_Controls.CKM_GridView GvDetail;
        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private CKM_Controls.CKM_Label ckM_Label8;
        private CKM_Controls.CKM_TextBox ckM_TextBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPickingNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn coIPrintDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPickingKBN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeliveryName;
    }
}

