namespace TenzikaiJuchuuJouhouHurikaeShori
{
    partial class TenzikaiJuchuuJouhouHurikaeShori
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new CKM_Controls.CKM_Label();
            this.label27 = new CKM_Controls.CKM_Label();
            this.ckM_SearchControl2 = new Search.CKM_SearchControl();
            this.ckM_SearchControl3 = new Search.CKM_SearchControl();
            this.label9 = new CKM_Controls.CKM_Label();
            this.lblSkuCD = new System.Windows.Forms.Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox1 = new CKM_Controls.CKM_TextBox();
            this.ScVendor = new Search.CKM_SearchControl();
            this.GvDetail = new CKM_Controls.CKM_GridView();
            this.cboNendo = new CKM_Controls.CKM_ComboBox();
            this.cboSeason = new CKM_Controls.CKM_ComboBox();
            this.RdoCreate = new CKM_Controls.CKM_RadioButton();
            this.RdoDelete = new CKM_Controls.CKM_RadioButton();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.colJuchuNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.RdoCreate);
            this.PanelHeader.Controls.Add(this.RdoDelete);
            this.PanelHeader.Controls.Add(this.cboSeason);
            this.PanelHeader.Controls.Add(this.cboNendo);
            this.PanelHeader.Controls.Add(this.ScVendor);
            this.PanelHeader.Controls.Add(this.ckM_TextBox1);
            this.PanelHeader.Controls.Add(this.ckM_Label4);
            this.PanelHeader.Controls.Add(this.label9);
            this.PanelHeader.Controls.Add(this.ckM_Label2);
            this.PanelHeader.Controls.Add(this.lblSkuCD);
            this.PanelHeader.Size = new System.Drawing.Size(1368, 144);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.Controls.SetChildIndex(this.lblSkuCD, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.label9, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label4, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ScVendor, 0);
            this.PanelHeader.Controls.SetChildIndex(this.cboNendo, 0);
            this.PanelHeader.Controls.SetChildIndex(this.cboSeason, 0);
            this.PanelHeader.Controls.SetChildIndex(this.RdoDelete, 0);
            this.PanelHeader.Controls.SetChildIndex(this.RdoCreate, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.TabIndex = 7;
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
            // ckM_SearchControl2
            // 
            this.ckM_SearchControl2.AutoSize = true;
            this.ckM_SearchControl2.ChangeDate = "";
            this.ckM_SearchControl2.ChangeDateWidth = 100;
            this.ckM_SearchControl2.Code = "";
            this.ckM_SearchControl2.CodeWidth = 100;
            this.ckM_SearchControl2.CodeWidth1 = 100;
            this.ckM_SearchControl2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_SearchControl2.DataCheck = false;
            this.ckM_SearchControl2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_SearchControl2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ckM_SearchControl2.IsCopy = false;
            this.ckM_SearchControl2.LabelText = "";
            this.ckM_SearchControl2.LabelVisible = false;
            this.ckM_SearchControl2.Location = new System.Drawing.Point(343, 5);
            this.ckM_SearchControl2.Margin = new System.Windows.Forms.Padding(0);
            this.ckM_SearchControl2.Name = "ckM_SearchControl2";
            this.ckM_SearchControl2.NameWidth = 600;
            this.ckM_SearchControl2.SearchEnable = true;
            this.ckM_SearchControl2.Size = new System.Drawing.Size(133, 28);
            this.ckM_SearchControl2.Stype = Search.CKM_SearchControl.SearchType.見積番号;
            this.ckM_SearchControl2.TabIndex = 275;
            this.ckM_SearchControl2.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ckM_SearchControl2.UseChangeDate = false;
            this.ckM_SearchControl2.Value1 = null;
            this.ckM_SearchControl2.Value2 = null;
            this.ckM_SearchControl2.Value3 = null;
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
            this.ckM_SearchControl3.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ckM_SearchControl3.UseChangeDate = false;
            this.ckM_SearchControl3.Value1 = null;
            this.ckM_SearchControl3.Value2 = null;
            this.ckM_SearchControl3.Value3 = null;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.DefaultlabelSize = true;
            this.label9.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label9.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(56, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 12);
            this.label9.TabIndex = 705;
            this.label9.Text = "登録日";
            this.label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSkuCD
            // 
            this.lblSkuCD.AutoSize = true;
            this.lblSkuCD.BackColor = System.Drawing.Color.Transparent;
            this.lblSkuCD.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSkuCD.Location = new System.Drawing.Point(56, 74);
            this.lblSkuCD.Name = "lblSkuCD";
            this.lblSkuCD.Size = new System.Drawing.Size(44, 12);
            this.lblSkuCD.TabIndex = 704;
            this.lblSkuCD.Text = "仕入先";
            this.lblSkuCD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label2
            // 
            this.ckM_Label2.AutoSize = true;
            this.ckM_Label2.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label2.DefaultlabelSize = true;
            this.ckM_Label2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label2.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label2.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label2.Location = new System.Drawing.Point(56, 106);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label2.TabIndex = 690;
            this.ckM_Label2.Text = "年　度";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label4.Location = new System.Drawing.Point(252, 106);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label4.TabIndex = 701;
            this.ckM_Label4.Text = "シーズン";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_TextBox1.DecimalPlace = 0;
            this.ckM_TextBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox1.ForeColor = System.Drawing.Color.Black;
            this.ckM_TextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox1.IntegerPart = 0;
            this.ckM_TextBox1.IsCorrectDate = true;
            this.ckM_TextBox1.isEnterKeyDown = false;
            this.ckM_TextBox1.IsFirstTime = true;
            this.ckM_TextBox1.isMaxLengthErr = false;
            this.ckM_TextBox1.IsNumber = true;
            this.ckM_TextBox1.IsShop = false;
            this.ckM_TextBox1.Length = 10;
            this.ckM_TextBox1.Location = new System.Drawing.Point(105, 40);
            this.ckM_TextBox1.MaxLength = 10;
            this.ckM_TextBox1.MoveNext = true;
            this.ckM_TextBox1.Name = "ckM_TextBox1";
            this.ckM_TextBox1.Size = new System.Drawing.Size(100, 19);
            this.ckM_TextBox1.TabIndex = 2;
            this.ckM_TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.ckM_TextBox1.UseColorSizMode = false;
            // 
            // ScVendor
            // 
            this.ScVendor.AutoSize = true;
            this.ScVendor.ChangeDate = "";
            this.ScVendor.ChangeDateWidth = 100;
            this.ScVendor.Code = "";
            this.ScVendor.CodeWidth = 100;
            this.ScVendor.CodeWidth1 = 100;
            this.ScVendor.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScVendor.DataCheck = true;
            this.ScVendor.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ScVendor.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ScVendor.IsCopy = false;
            this.ScVendor.LabelText = "";
            this.ScVendor.LabelVisible = true;
            this.ScVendor.Location = new System.Drawing.Point(105, 66);
            this.ScVendor.Margin = new System.Windows.Forms.Padding(0);
            this.ScVendor.Name = "ScVendor";
            this.ScVendor.NameWidth = 310;
            this.ScVendor.SearchEnable = true;
            this.ScVendor.Size = new System.Drawing.Size(444, 28);
            this.ScVendor.Stype = Search.CKM_SearchControl.SearchType.仕入先;
            this.ScVendor.TabIndex = 3;
            this.ScVendor.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScVendor.UseChangeDate = false;
            this.ScVendor.Value1 = null;
            this.ScVendor.Value2 = null;
            this.ScVendor.Value3 = null;
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
            this.colJuchuNO,
            this.colSKUName});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.GvDetail.DefaultCellStyle = dataGridViewCellStyle3;
            this.GvDetail.EnableHeadersVisualStyles = false;
            this.GvDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GvDetail.Location = new System.Drawing.Point(46, 229);
            this.GvDetail.Name = "GvDetail";
            this.GvDetail.ReadOnly = true;
            this.GvDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GvDetail.Size = new System.Drawing.Size(181, 468);
            this.GvDetail.TabIndex = 2;
            this.GvDetail.UseRowNo = true;
            this.GvDetail.UseSetting = true;
            // 
            // cboNendo
            // 
            this.cboNendo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboNendo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboNendo.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.年度;
            this.cboNendo.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.cboNendo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboNendo.Flag = 0;
            this.cboNendo.ForeColor = System.Drawing.Color.Black;
            this.cboNendo.FormattingEnabled = true;
            this.cboNendo.Length = 40;
            this.cboNendo.Location = new System.Drawing.Point(105, 103);
            this.cboNendo.MaxDropDownItems = 3;
            this.cboNendo.MaxLength = 20;
            this.cboNendo.MoveNext = false;
            this.cboNendo.Name = "cboNendo";
            this.cboNendo.Size = new System.Drawing.Size(96, 20);
            this.cboNendo.TabIndex = 4;
            // 
            // cboSeason
            // 
            this.cboSeason.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboSeason.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSeason.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.シーズン;
            this.cboSeason.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.cboSeason.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSeason.Flag = 0;
            this.cboSeason.ForeColor = System.Drawing.Color.Black;
            this.cboSeason.FormattingEnabled = true;
            this.cboSeason.Length = 40;
            this.cboSeason.Location = new System.Drawing.Point(315, 103);
            this.cboSeason.MaxDropDownItems = 3;
            this.cboSeason.MaxLength = 20;
            this.cboSeason.MoveNext = false;
            this.cboSeason.Name = "cboSeason";
            this.cboSeason.Size = new System.Drawing.Size(96, 20);
            this.cboSeason.TabIndex = 5;
            // 
            // RdoCreate
            // 
            this.RdoCreate.AutoSize = true;
            this.RdoCreate.Checked = true;
            this.RdoCreate.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RdoCreate.Location = new System.Drawing.Point(105, 15);
            this.RdoCreate.Name = "RdoCreate";
            this.RdoCreate.Size = new System.Drawing.Size(49, 16);
            this.RdoCreate.TabIndex = 0;
            this.RdoCreate.TabStop = true;
            this.RdoCreate.Text = "作成";
            this.RdoCreate.UseVisualStyleBackColor = true;
            this.RdoCreate.CheckedChanged += new System.EventHandler(this.RdoCreate_CheckedChanged);
            // 
            // RdoDelete
            // 
            this.RdoDelete.AutoSize = true;
            this.RdoDelete.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RdoDelete.Location = new System.Drawing.Point(217, 15);
            this.RdoDelete.Name = "RdoDelete";
            this.RdoDelete.Size = new System.Drawing.Size(49, 16);
            this.RdoDelete.TabIndex = 1;
            this.RdoDelete.Text = "削除";
            this.RdoDelete.UseVisualStyleBackColor = true;
            // 
            // ckM_Label1
            // 
            this.ckM_Label1.AutoSize = true;
            this.ckM_Label1.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label1.DefaultlabelSize = true;
            this.ckM_Label1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label1.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label1.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label1.Location = new System.Drawing.Point(44, 207);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(325, 12);
            this.ckM_Label1.TabIndex = 706;
            this.ckM_Label1.Text = "【SKUマスタ未登録の商品が含まれる展示会受注伝票】";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // colJuchuNO
            // 
            this.colJuchuNO.DataPropertyName = "TenzikaiJuchuuNO";
            this.colJuchuNO.HeaderText = "展示会受注番号";
            this.colJuchuNO.Name = "colJuchuNO";
            this.colJuchuNO.ReadOnly = true;
            this.colJuchuNO.Width = 120;
            // 
            // colSKUName
            // 
            this.colSKUName.DataPropertyName = "SKUName";
            this.colSKUName.HeaderText = "商品";
            this.colSKUName.Name = "colSKUName";
            this.colSKUName.ReadOnly = true;
            this.colSKUName.Visible = false;
            this.colSKUName.Width = 350;
            // 
            // TenzikaiJuchuuJouhouHurikaeShori
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.ckM_Label1);
            this.Controls.Add(this.GvDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "TenzikaiJuchuuJouhouHurikaeShori";
            this.PanelHeaderHeight = 200;
            this.Text = "TenzikaiJuchuuJouhouHurikaeShori";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.GvDetail, 0);
            this.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CKM_Controls.CKM_Label label27;
        private CKM_Controls.CKM_Label label1;
        private Search.CKM_SearchControl ckM_SearchControl2;
        private Search.CKM_SearchControl ckM_SearchControl3;
        private CKM_Controls.CKM_Label label9;
        private System.Windows.Forms.Label lblSkuCD;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private Search.CKM_SearchControl ScVendor;
        private CKM_Controls.CKM_GridView GvDetail;
        private CKM_Controls.CKM_ComboBox cboNendo;
        private CKM_Controls.CKM_ComboBox cboSeason;
        private CKM_Controls.CKM_RadioButton RdoCreate;
        private CKM_Controls.CKM_RadioButton RdoDelete;
        private CKM_Controls.CKM_Label ckM_Label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJuchuNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUName;
    }
}

