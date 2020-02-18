namespace TanaoroshiNyuuryoku
{
    partial class TanaoroshiNyuuryoku
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label4 = new CKM_Controls.CKM_Label();
            this.label11 = new CKM_Controls.CKM_Label();
            this.label1 = new CKM_Controls.CKM_Label();
            this.label27 = new CKM_Controls.CKM_Label();
            this.CboSoukoCD = new CKM_Controls.CKM_ComboBox();
            this.ckM_SearchControl3 = new Search.CKM_SearchControl();
            this.ckM_TextBox1 = new CKM_Controls.CKM_TextBox();
            this.BtnSubF11 = new CKM_Controls.CKM_Button();
            this.ckM_SearchControl1 = new Search.CKM_SearchControl();
            this.GvDetail = new CKM_Controls.CKM_GridView();
            this.colOrderNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.coIOrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colApprovalStageFLG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastApprovalDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastApprovalStaffName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSizeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTheoreticalQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActualQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDifferenceQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AdminNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PanelHeader.SuspendLayout();
            this.PanelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.CboSoukoCD);
            this.PanelHeader.Controls.Add(this.label4);
            this.PanelHeader.Controls.Add(this.ckM_TextBox1);
            this.PanelHeader.Controls.Add(this.label11);
            this.PanelHeader.Size = new System.Drawing.Size(1368, 94);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.TabStop = true;
            this.PanelHeader.Controls.SetChildIndex(this.label11, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.label4, 0);
            this.PanelHeader.Controls.SetChildIndex(this.CboSoukoCD, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Controls.Add(this.BtnSubF11);
            this.PanelSearch.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.DefaultlabelSize = true;
            this.label4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label4.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(53, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 12);
            this.label4.TabIndex = 255;
            this.label4.Text = "対象倉庫";
            this.label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.DefaultlabelSize = true;
            this.label11.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label11.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(66, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 12);
            this.label11.TabIndex = 261;
            this.label11.Text = "棚卸日";
            this.label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label1.BackColor = System.Drawing.Color.DarkGray;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.DefaultlabelSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(28, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(492, 18);
            this.label1.TabIndex = 261;
            this.label1.Text = "商品名";
            this.label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label27.BackColor = System.Drawing.Color.Transparent;
            this.label27.DefaultlabelSize = true;
            this.label27.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label27.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label27.ForeColor = System.Drawing.Color.Black;
            this.label27.Location = new System.Drawing.Point(257, 13);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(83, 12);
            this.label27.TabIndex = 341;
            this.label27.Text = "複写見積番号";
            this.label27.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CboSoukoCD
            // 
            this.CboSoukoCD.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.CboSoukoCD.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CboSoukoCD.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.棚卸倉庫;
            this.CboSoukoCD.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.CboSoukoCD.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CboSoukoCD.FormattingEnabled = true;
            this.CboSoukoCD.Length = 40;
            this.CboSoukoCD.Location = new System.Drawing.Point(115, 6);
            this.CboSoukoCD.MaxLength = 40;
            this.CboSoukoCD.MoveNext = true;
            this.CboSoukoCD.Name = "CboSoukoCD";
            this.CboSoukoCD.Size = new System.Drawing.Size(280, 20);
            this.CboSoukoCD.TabIndex = 0;
            // 
            // ckM_SearchControl3
            // 
            this.ckM_SearchControl3.AutoSize = true;
            this.ckM_SearchControl3.ChangeDate = "";
            this.ckM_SearchControl3.ChangeDateWidth = 100;
            this.ckM_SearchControl3.Code = "";
            this.ckM_SearchControl3.CodeWidth = 100;
            this.ckM_SearchControl3.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_SearchControl3.DataCheck = false;
            this.ckM_SearchControl3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_SearchControl3.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ckM_SearchControl3.IsCopy = false;
            this.ckM_SearchControl3.LabelText = "";
            this.ckM_SearchControl3.LabelVisible = false;
            this.ckM_SearchControl3.Location = new System.Drawing.Point(343, 4);
            this.ckM_SearchControl3.Name = "ckM_SearchControl3";
            this.ckM_SearchControl3.SearchEnable = true;
            this.ckM_SearchControl3.Size = new System.Drawing.Size(133, 28);
            this.ckM_SearchControl3.Stype = Search.CKM_SearchControl.SearchType.見積番号;
            this.ckM_SearchControl3.TabIndex = 344;
            this.ckM_SearchControl3.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ckM_SearchControl3.UseChangeDate = false;
            this.ckM_SearchControl3.Value1 = null;
            this.ckM_SearchControl3.Value2 = null;
            this.ckM_SearchControl3.Value3 = null;
            // 
            // ckM_TextBox1
            // 
            this.ckM_TextBox1.AllowMinus = false;
            this.ckM_TextBox1.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox1.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox1.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.ckM_TextBox1.DecimalPlace = 2;
            this.ckM_TextBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox1.IntegerPart = 0;
            this.ckM_TextBox1.IsCorrectDate = true;
            this.ckM_TextBox1.isEnterKeyDown = false;
            this.ckM_TextBox1.IsNumber = true;
            this.ckM_TextBox1.IsShop = false;
            this.ckM_TextBox1.Length = 10;
            this.ckM_TextBox1.Location = new System.Drawing.Point(116, 32);
            this.ckM_TextBox1.MaxLength = 10;
            this.ckM_TextBox1.MoveNext = true;
            this.ckM_TextBox1.Name = "ckM_TextBox1";
            this.ckM_TextBox1.Size = new System.Drawing.Size(88, 19);
            this.ckM_TextBox1.TabIndex = 1;
            this.ckM_TextBox1.Text = "2019/01/01";
            this.ckM_TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // BtnSubF11
            // 
            this.BtnSubF11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BtnSubF11.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BtnSubF11.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSubF11.DefaultBtnSize = true;
            this.BtnSubF11.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnSubF11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSubF11.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.BtnSubF11.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BtnSubF11.Location = new System.Drawing.Point(375, 0);
            this.BtnSubF11.Margin = new System.Windows.Forms.Padding(1);
            this.BtnSubF11.Name = "BtnSubF11";
            this.BtnSubF11.Size = new System.Drawing.Size(118, 28);
            this.BtnSubF11.TabIndex = 10;
            this.BtnSubF11.Text = "表示(F11)";
            this.BtnSubF11.UseVisualStyleBackColor = false;
            this.BtnSubF11.Click += new System.EventHandler(this.BtnF11_Click);
            // 
            // ckM_SearchControl1
            // 
            this.ckM_SearchControl1.AutoSize = true;
            this.ckM_SearchControl1.ChangeDate = "";
            this.ckM_SearchControl1.ChangeDateWidth = 100;
            this.ckM_SearchControl1.Code = "";
            this.ckM_SearchControl1.CodeWidth = 100;
            this.ckM_SearchControl1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_SearchControl1.DataCheck = false;
            this.ckM_SearchControl1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_SearchControl1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ckM_SearchControl1.IsCopy = false;
            this.ckM_SearchControl1.LabelText = "";
            this.ckM_SearchControl1.LabelVisible = false;
            this.ckM_SearchControl1.Location = new System.Drawing.Point(577, 3);
            this.ckM_SearchControl1.Name = "ckM_SearchControl1";
            this.ckM_SearchControl1.SearchEnable = true;
            this.ckM_SearchControl1.Size = new System.Drawing.Size(133, 28);
            this.ckM_SearchControl1.Stype = Search.CKM_SearchControl.SearchType.見積番号;
            this.ckM_SearchControl1.TabIndex = 694;
            this.ckM_SearchControl1.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ckM_SearchControl1.UseChangeDate = false;
            this.ckM_SearchControl1.Value1 = null;
            this.ckM_SearchControl1.Value2 = null;
            this.ckM_SearchControl1.Value3 = null;
            // 
            // GvDetail
            // 
            this.GvDetail.AllowUserToAddRows = false;
            this.GvDetail.AllowUserToDeleteRows = false;
            this.GvDetail.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.GvDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.GvDetail.AutoGenerateColumns = false;
            this.GvDetail.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GvDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.GvDetail.ColumnHeadersHeight = 25;
            this.GvDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOrderNO,
            this.coIOrderDate,
            this.colApprovalStageFLG,
            this.colLastApprovalDate,
            this.colLastApprovalStaffName,
            this.ColSizeName,
            this.colTheoreticalQuantity,
            this.colActualQuantity,
            this.colDifferenceQuantity,
            this.AdminNO});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.GvDetail.DefaultCellStyle = dataGridViewCellStyle9;
            this.GvDetail.EnableHeadersVisualStyles = false;
            this.GvDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GvDetail.Location = new System.Drawing.Point(12, 166);
            this.GvDetail.Name = "GvDetail";
            this.GvDetail.ReadOnly = true;
            this.GvDetail.Size = new System.Drawing.Size(1343, 538);
            this.GvDetail.TabIndex = 20;
            this.GvDetail.UseRowNo = true;
            this.GvDetail.UseSetting = true;
            this.GvDetail.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvDetail_CellEndEdit);
            // 
            // colOrderNO
            // 
            this.colOrderNO.DataPropertyName = "RackNO";
            this.colOrderNO.HeaderText = "棚番";
            this.colOrderNO.Name = "colOrderNO";
            this.colOrderNO.ReadOnly = true;
            this.colOrderNO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colOrderNO.Width = 80;
            // 
            // coIOrderDate
            // 
            this.coIOrderDate.DataPropertyName = "JANCD";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.coIOrderDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.coIOrderDate.HeaderText = "JANCD";
            this.coIOrderDate.Name = "coIOrderDate";
            this.coIOrderDate.ReadOnly = true;
            this.coIOrderDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colApprovalStageFLG
            // 
            this.colApprovalStageFLG.DataPropertyName = "SKUCD";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colApprovalStageFLG.DefaultCellStyle = dataGridViewCellStyle4;
            this.colApprovalStageFLG.HeaderText = "SKUCD";
            this.colApprovalStageFLG.Name = "colApprovalStageFLG";
            this.colApprovalStageFLG.ReadOnly = true;
            this.colApprovalStageFLG.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colApprovalStageFLG.Width = 200;
            // 
            // colLastApprovalDate
            // 
            this.colLastApprovalDate.DataPropertyName = "SKUName";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colLastApprovalDate.DefaultCellStyle = dataGridViewCellStyle5;
            this.colLastApprovalDate.HeaderText = "商品名";
            this.colLastApprovalDate.Name = "colLastApprovalDate";
            this.colLastApprovalDate.ReadOnly = true;
            this.colLastApprovalDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colLastApprovalDate.Width = 300;
            // 
            // colLastApprovalStaffName
            // 
            this.colLastApprovalStaffName.DataPropertyName = "ColorName";
            this.colLastApprovalStaffName.HeaderText = "カラー";
            this.colLastApprovalStaffName.Name = "colLastApprovalStaffName";
            this.colLastApprovalStaffName.ReadOnly = true;
            this.colLastApprovalStaffName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colLastApprovalStaffName.Width = 150;
            // 
            // ColSizeName
            // 
            this.ColSizeName.DataPropertyName = "SizeName";
            this.ColSizeName.HeaderText = "サイズ";
            this.ColSizeName.Name = "ColSizeName";
            this.ColSizeName.ReadOnly = true;
            this.ColSizeName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColSizeName.Width = 150;
            // 
            // colTheoreticalQuantity
            // 
            this.colTheoreticalQuantity.DataPropertyName = "TheoreticalQuantity";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            dataGridViewCellStyle6.NullValue = null;
            this.colTheoreticalQuantity.DefaultCellStyle = dataGridViewCellStyle6;
            this.colTheoreticalQuantity.HeaderText = "理論在庫";
            this.colTheoreticalQuantity.Name = "colTheoreticalQuantity";
            this.colTheoreticalQuantity.ReadOnly = true;
            this.colTheoreticalQuantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colActualQuantity
            // 
            this.colActualQuantity.DataPropertyName = "ActualQuantity";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N0";
            dataGridViewCellStyle7.NullValue = null;
            this.colActualQuantity.DefaultCellStyle = dataGridViewCellStyle7;
            this.colActualQuantity.HeaderText = "実在庫";
            this.colActualQuantity.Name = "colActualQuantity";
            this.colActualQuantity.ReadOnly = true;
            this.colActualQuantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colDifferenceQuantity
            // 
            this.colDifferenceQuantity.DataPropertyName = "DifferenceQuantity";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N0";
            dataGridViewCellStyle8.NullValue = null;
            this.colDifferenceQuantity.DefaultCellStyle = dataGridViewCellStyle8;
            this.colDifferenceQuantity.HeaderText = "差";
            this.colDifferenceQuantity.Name = "colDifferenceQuantity";
            this.colDifferenceQuantity.ReadOnly = true;
            this.colDifferenceQuantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AdminNO
            // 
            this.AdminNO.DataPropertyName = "AdminNO";
            this.AdminNO.HeaderText = "AdminNO";
            this.AdminNO.Name = "AdminNO";
            this.AdminNO.ReadOnly = true;
            this.AdminNO.Visible = false;
            // 
            // TanaoroshiNyuuryoku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.GvDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "TanaoroshiNyuuryoku";
            this.PanelHeaderHeight = 150;
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.GvDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            this.PanelSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private CKM_Controls.CKM_Label label4;
        private CKM_Controls.CKM_Label label11;
        private CKM_Controls.CKM_Label label27;
        private CKM_Controls.CKM_Label label1;
        private CKM_Controls.CKM_ComboBox CboSoukoCD;
        private Search.CKM_SearchControl ckM_SearchControl3;
        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private Search.CKM_SearchControl ckM_SearchControl1;
        private CKM_Controls.CKM_Button BtnSubF11;
        private CKM_Controls.CKM_GridView GvDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn coIOrderDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colApprovalStageFLG;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastApprovalDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastApprovalStaffName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSizeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTheoreticalQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colActualQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDifferenceQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn AdminNO;
    }
}

