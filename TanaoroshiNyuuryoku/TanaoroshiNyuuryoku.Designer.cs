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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TanaoroshiNyuuryoku));
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
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.lblSizeName = new System.Windows.Forms.Label();
            this.SC_ITEM_0 = new Search.CKM_SearchControl();
            this.ckM_Label11 = new CKM_Controls.CKM_Label();
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.ckM_Label12 = new CKM_Controls.CKM_Label();
            this.lblSKUName = new System.Windows.Forms.Label();
            this.lblSKUCD = new System.Windows.Forms.Label();
            this.lblColorName = new System.Windows.Forms.Label();
            this.label12 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox8 = new CKM_Controls.CKM_TextBox();
            this.ScFromRackNo = new Search.CKM_SearchControl();
            this.btnAdd = new CKM_Controls.CKM_Button();
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
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
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
            this.CboSoukoCD.Flag = 0;
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
            this.ckM_SearchControl3.CodeWidth1 = 100;
            this.ckM_SearchControl3.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_SearchControl3.DataCheck = false;
            this.ckM_SearchControl3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_SearchControl3.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ckM_SearchControl3.IsCopy = false;
            this.ckM_SearchControl3.LabelText = "";
            this.ckM_SearchControl3.LabelVisible = false;
            this.ckM_SearchControl3.Location = new System.Drawing.Point(343, 4);
            this.ckM_SearchControl3.Margin = new System.Windows.Forms.Padding(0);
            this.ckM_SearchControl3.Name = "ckM_SearchControl3";
            this.ckM_SearchControl3.NameWidth = 600;
            this.ckM_SearchControl3.SearchEnable = true;
            this.ckM_SearchControl3.Size = new System.Drawing.Size(133, 28);
            this.ckM_SearchControl3.Stype = Search.CKM_SearchControl.SearchType.見積番号;
            this.ckM_SearchControl3.TabIndex = 344;
            this.ckM_SearchControl3.test = null;
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
            this.ckM_TextBox1.BorderColor = false;
            this.ckM_TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox1.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox1.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.ckM_TextBox1.DecimalPlace = 2;
            this.ckM_TextBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox1.IntegerPart = 0;
            this.ckM_TextBox1.IsCorrectDate = true;
            this.ckM_TextBox1.isEnterKeyDown = false;
            this.ckM_TextBox1.IsFirstTime = true;
            this.ckM_TextBox1.isMaxLengthErr = false;
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
            this.ckM_TextBox1.UseColorSizMode = false;
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
            this.ckM_SearchControl1.CodeWidth1 = 100;
            this.ckM_SearchControl1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_SearchControl1.DataCheck = false;
            this.ckM_SearchControl1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_SearchControl1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ckM_SearchControl1.IsCopy = false;
            this.ckM_SearchControl1.LabelText = "";
            this.ckM_SearchControl1.LabelVisible = false;
            this.ckM_SearchControl1.Location = new System.Drawing.Point(577, 3);
            this.ckM_SearchControl1.Margin = new System.Windows.Forms.Padding(0);
            this.ckM_SearchControl1.Name = "ckM_SearchControl1";
            this.ckM_SearchControl1.NameWidth = 600;
            this.ckM_SearchControl1.SearchEnable = true;
            this.ckM_SearchControl1.Size = new System.Drawing.Size(133, 28);
            this.ckM_SearchControl1.Stype = Search.CKM_SearchControl.SearchType.見積番号;
            this.ckM_SearchControl1.TabIndex = 694;
            this.ckM_SearchControl1.test = null;
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
            this.GvDetail.CheckCol = ((System.Collections.ArrayList)(resources.GetObject("GvDetail.CheckCol")));
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
            this.GvDetail.RowHeight_ = 20;
            this.GvDetail.RowTemplate.Height = 20;
            this.GvDetail.Size = new System.Drawing.Size(1343, 474);
            this.GvDetail.TabIndex = 20;
            this.GvDetail.UseRowNo = true;
            this.GvDetail.UseSetting = true;
            this.GvDetail.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvDetail_CellEndEdit);
            this.GvDetail.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.GvDetail_DataError);
            this.GvDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GvDetail_KeyDown);
            // 
            // colOrderNO
            // 
            this.colOrderNO.DataPropertyName = "RackNO";
            this.colOrderNO.HeaderText = "棚番";
            this.colOrderNO.Name = "colOrderNO";
            this.colOrderNO.ReadOnly = true;
            this.colOrderNO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colOrderNO.Width = 70;
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
            this.coIOrderDate.Width = 110;
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
            this.colApprovalStageFLG.Width = 160;
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
            this.colLastApprovalDate.Width = 380;
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
            this.colTheoreticalQuantity.Width = 85;
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
            this.colActualQuantity.Width = 85;
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
            this.colDifferenceQuantity.Width = 85;
            // 
            // AdminNO
            // 
            this.AdminNO.DataPropertyName = "AdminNO";
            this.AdminNO.HeaderText = "AdminNO";
            this.AdminNO.Name = "AdminNO";
            this.AdminNO.ReadOnly = true;
            this.AdminNO.Visible = false;
            // 
            // ckM_Label4
            // 
            this.ckM_Label4.AutoSize = true;
            this.ckM_Label4.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label4.DefaultlabelSize = true;
            this.ckM_Label4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label4.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label4.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label4.Location = new System.Drawing.Point(1162, 675);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label4.TabIndex = 754;
            this.ckM_Label4.Text = "サイズ";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSizeName
            // 
            this.lblSizeName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblSizeName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSizeName.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSizeName.Location = new System.Drawing.Point(1210, 672);
            this.lblSizeName.Name = "lblSizeName";
            this.lblSizeName.Size = new System.Drawing.Size(134, 18);
            this.lblSizeName.TabIndex = 753;
            this.lblSizeName.Text = "ＸＸＸＸＸＸＸＸＸ10";
            this.lblSizeName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SC_ITEM_0
            // 
            this.SC_ITEM_0.AutoSize = true;
            this.SC_ITEM_0.ChangeDate = "";
            this.SC_ITEM_0.ChangeDateWidth = 100;
            this.SC_ITEM_0.Code = "";
            this.SC_ITEM_0.CodeWidth = 110;
            this.SC_ITEM_0.CodeWidth1 = 110;
            this.SC_ITEM_0.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Number;
            this.SC_ITEM_0.DataCheck = false;
            this.SC_ITEM_0.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.SC_ITEM_0.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.SC_ITEM_0.IsCopy = false;
            this.SC_ITEM_0.LabelText = "";
            this.SC_ITEM_0.LabelVisible = false;
            this.SC_ITEM_0.Location = new System.Drawing.Point(84, 667);
            this.SC_ITEM_0.Margin = new System.Windows.Forms.Padding(0);
            this.SC_ITEM_0.Name = "SC_ITEM_0";
            this.SC_ITEM_0.NameWidth = 190;
            this.SC_ITEM_0.SearchEnable = true;
            this.SC_ITEM_0.Size = new System.Drawing.Size(143, 28);
            this.SC_ITEM_0.Stype = Search.CKM_SearchControl.SearchType.JANCD_Detail;
            this.SC_ITEM_0.TabIndex = 12;
            this.SC_ITEM_0.Tag = "0";
            this.SC_ITEM_0.test = null;
            this.SC_ITEM_0.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.SC_ITEM_0.UseChangeDate = false;
            this.SC_ITEM_0.Value1 = null;
            this.SC_ITEM_0.Value2 = null;
            this.SC_ITEM_0.Value3 = null;
            // 
            // ckM_Label11
            // 
            this.ckM_Label11.AutoSize = true;
            this.ckM_Label11.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label11.DefaultlabelSize = true;
            this.ckM_Label11.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label11.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label11.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label11.Location = new System.Drawing.Point(52, 651);
            this.ckM_Label11.Name = "ckM_Label11";
            this.ckM_Label11.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label11.TabIndex = 752;
            this.ckM_Label11.Text = "棚番";
            this.ckM_Label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label10
            // 
            this.ckM_Label10.AutoSize = true;
            this.ckM_Label10.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label10.DefaultlabelSize = true;
            this.ckM_Label10.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label10.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label10.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label10.Location = new System.Drawing.Point(935, 675);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label10.TabIndex = 751;
            this.ckM_Label10.Text = "カラー";
            this.ckM_Label10.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label12
            // 
            this.ckM_Label12.AutoSize = true;
            this.ckM_Label12.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label12.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label12.DefaultlabelSize = true;
            this.ckM_Label12.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label12.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label12.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label12.Location = new System.Drawing.Point(39, 697);
            this.ckM_Label12.Name = "ckM_Label12";
            this.ckM_Label12.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label12.TabIndex = 750;
            this.ckM_Label12.Text = "実在庫";
            this.ckM_Label12.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSKUName
            // 
            this.lblSKUName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblSKUName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSKUName.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSKUName.Location = new System.Drawing.Point(416, 672);
            this.lblSKUName.Name = "lblSKUName";
            this.lblSKUName.Size = new System.Drawing.Size(490, 18);
            this.lblSKUName.TabIndex = 743;
            this.lblSKUName.Text = "ＸＸＸＸＸＸＸＸＸ10ＸＸＸＸＸＸＸＸＸ20ＸＸＸＸＸＸＸＸＸ30ＸＸＸＸＸＸＸＸＸ40";
            this.lblSKUName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSKUCD
            // 
            this.lblSKUCD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblSKUCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSKUCD.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSKUCD.Location = new System.Drawing.Point(227, 672);
            this.lblSKUCD.Name = "lblSKUCD";
            this.lblSKUCD.Size = new System.Drawing.Size(189, 18);
            this.lblSKUCD.TabIndex = 748;
            this.lblSKUCD.Text = "XXXXXXXXX1XXXXXXXXX2XXXXXXXXX3";
            this.lblSKUCD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblColorName
            // 
            this.lblColorName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblColorName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblColorName.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColorName.Location = new System.Drawing.Point(983, 672);
            this.lblColorName.Name = "lblColorName";
            this.lblColorName.Size = new System.Drawing.Size(134, 18);
            this.lblColorName.TabIndex = 747;
            this.lblColorName.Text = "ＸＸＸＸＸＸＸＸＸ10";
            this.lblColorName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.DefaultlabelSize = true;
            this.label12.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label12.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(43, 674);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(40, 12);
            this.label12.TabIndex = 749;
            this.label12.Text = "JANCD";
            this.label12.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_TextBox8
            // 
            this.ckM_TextBox8.AllowMinus = true;
            this.ckM_TextBox8.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox8.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox8.BorderColor = false;
            this.ckM_TextBox8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox8.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox8.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox8.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Price;
            this.ckM_TextBox8.DecimalPlace = 0;
            this.ckM_TextBox8.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox8.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox8.IntegerPart = 5;
            this.ckM_TextBox8.IsCorrectDate = true;
            this.ckM_TextBox8.isEnterKeyDown = false;
            this.ckM_TextBox8.IsFirstTime = true;
            this.ckM_TextBox8.isMaxLengthErr = false;
            this.ckM_TextBox8.IsNumber = true;
            this.ckM_TextBox8.IsShop = false;
            this.ckM_TextBox8.Length = 6;
            this.ckM_TextBox8.Location = new System.Drawing.Point(84, 695);
            this.ckM_TextBox8.MaxLength = 6;
            this.ckM_TextBox8.MoveNext = true;
            this.ckM_TextBox8.Name = "ckM_TextBox8";
            this.ckM_TextBox8.Size = new System.Drawing.Size(85, 19);
            this.ckM_TextBox8.TabIndex = 13;
            this.ckM_TextBox8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ckM_TextBox8.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.ckM_TextBox8.UseColorSizMode = false;
            // 
            // ScFromRackNo
            // 
            this.ScFromRackNo.AutoSize = true;
            this.ScFromRackNo.ChangeDate = "";
            this.ScFromRackNo.ChangeDateWidth = 100;
            this.ScFromRackNo.Code = "";
            this.ScFromRackNo.CodeWidth = 100;
            this.ScFromRackNo.CodeWidth1 = 100;
            this.ScFromRackNo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScFromRackNo.DataCheck = true;
            this.ScFromRackNo.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ScFromRackNo.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ScFromRackNo.IsCopy = false;
            this.ScFromRackNo.LabelText = "";
            this.ScFromRackNo.LabelVisible = false;
            this.ScFromRackNo.Location = new System.Drawing.Point(84, 643);
            this.ScFromRackNo.Margin = new System.Windows.Forms.Padding(0);
            this.ScFromRackNo.Name = "ScFromRackNo";
            this.ScFromRackNo.NameWidth = 600;
            this.ScFromRackNo.SearchEnable = true;
            this.ScFromRackNo.Size = new System.Drawing.Size(133, 28);
            this.ScFromRackNo.Stype = Search.CKM_SearchControl.SearchType.棚番号;
            this.ScFromRackNo.TabIndex = 11;
            this.ScFromRackNo.test = null;
            this.ScFromRackNo.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScFromRackNo.UseChangeDate = false;
            this.ScFromRackNo.Value1 = null;
            this.ScFromRackNo.Value2 = null;
            this.ScFromRackNo.Value3 = null;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnAdd.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.DefaultBtnSize = true;
            this.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.btnAdd.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnAdd.Location = new System.Drawing.Point(230, 693);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(1);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(111, 23);
            this.btnAdd.TabIndex = 14;
            this.btnAdd.Text = "追加";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // TanaoroshiNyuuryoku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.ckM_Label4);
            this.Controls.Add(this.lblSizeName);
            this.Controls.Add(this.SC_ITEM_0);
            this.Controls.Add(this.ckM_Label11);
            this.Controls.Add(this.ckM_Label10);
            this.Controls.Add(this.ckM_Label12);
            this.Controls.Add(this.lblSKUName);
            this.Controls.Add(this.lblSKUCD);
            this.Controls.Add(this.lblColorName);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.ckM_TextBox8);
            this.Controls.Add(this.ScFromRackNo);
            this.Controls.Add(this.GvDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "TanaoroshiNyuuryoku";
            this.PanelHeaderHeight = 150;
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.GvDetail, 0);
            this.Controls.SetChildIndex(this.ScFromRackNo, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox8, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.lblColorName, 0);
            this.Controls.SetChildIndex(this.lblSKUCD, 0);
            this.Controls.SetChildIndex(this.lblSKUName, 0);
            this.Controls.SetChildIndex(this.ckM_Label12, 0);
            this.Controls.SetChildIndex(this.ckM_Label10, 0);
            this.Controls.SetChildIndex(this.ckM_Label11, 0);
            this.Controls.SetChildIndex(this.SC_ITEM_0, 0);
            this.Controls.SetChildIndex(this.lblSizeName, 0);
            this.Controls.SetChildIndex(this.ckM_Label4, 0);
            this.Controls.SetChildIndex(this.btnAdd, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            this.PanelSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private CKM_Controls.CKM_Label ckM_Label4;
        private System.Windows.Forms.Label lblSizeName;
        private Search.CKM_SearchControl SC_ITEM_0;
        private CKM_Controls.CKM_Label ckM_Label11;
        private CKM_Controls.CKM_Label ckM_Label10;
        private CKM_Controls.CKM_Label ckM_Label12;
        private System.Windows.Forms.Label lblSKUName;
        private System.Windows.Forms.Label lblSKUCD;
        private System.Windows.Forms.Label lblColorName;
        private CKM_Controls.CKM_Label label12;
        private CKM_Controls.CKM_TextBox ckM_TextBox8;
        private Search.CKM_SearchControl ScFromRackNo;
        private CKM_Controls.CKM_Button btnAdd;
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

