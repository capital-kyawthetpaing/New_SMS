namespace ShukkaUriageUpdateShoukai
{
    partial class ShukkaUriageUpdateShoukai
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label4 = new CKM_Controls.CKM_Label();
            this.label11 = new CKM_Controls.CKM_Label();
            this.label1 = new CKM_Controls.CKM_Label();
            this.label27 = new CKM_Controls.CKM_Label();
            this.ckM_SearchControl3 = new Search.CKM_SearchControl();
            this.BtnSubF11 = new CKM_Controls.CKM_Button();
            this.ckM_SearchControl1 = new Search.CKM_SearchControl();
            this.GvDetail = new CKM_Controls.CKM_GridView();
            this.coIInputDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEDIImportNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVendorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShippingSu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShippingNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblRireki = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ckM_TextBox1 = new CKM_Controls.CKM_TextBox();
            this.PanelHeader.SuspendLayout();
            this.PanelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.ckM_TextBox1);
            this.PanelHeader.Controls.Add(this.label2);
            this.PanelHeader.Controls.Add(this.lblRireki);
            this.PanelHeader.Controls.Add(this.label4);
            this.PanelHeader.Size = new System.Drawing.Size(1368, 104);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.TabStop = true;
            this.PanelHeader.Controls.SetChildIndex(this.label4, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblRireki, 0);
            this.PanelHeader.Controls.SetChildIndex(this.label2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox1, 0);
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
            this.label4.Location = new System.Drawing.Point(9, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 12);
            this.label4.TabIndex = 255;
            this.label4.Text = "【出荷売上データ更新】";
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
            this.label11.Location = new System.Drawing.Point(52, 168);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 12);
            this.label11.TabIndex = 261;
            this.label11.Text = "【更新結果】";
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
            this.BtnSubF11.Location = new System.Drawing.Point(212, 0);
            this.BtnSubF11.Margin = new System.Windows.Forms.Padding(1);
            this.BtnSubF11.Name = "BtnSubF11";
            this.BtnSubF11.Size = new System.Drawing.Size(118, 28);
            this.BtnSubF11.TabIndex = 7;
            this.BtnSubF11.Text = "最新化(F11)";
            this.BtnSubF11.UseVisualStyleBackColor = false;
            this.BtnSubF11.Click += new System.EventHandler(this.BtnSubF11_Click);
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
            this.coIInputDateTime,
            this.Column1,
            this.colEDIImportNO,
            this.colVendorName,
            this.colShippingSu,
            this.colSKUName,
            this.colShippingNO});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.GvDetail.DefaultCellStyle = dataGridViewCellStyle6;
            this.GvDetail.EnableHeadersVisualStyles = false;
            this.GvDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GvDetail.Location = new System.Drawing.Point(52, 183);
            this.GvDetail.Name = "GvDetail";
            this.GvDetail.ReadOnly = true;
            this.GvDetail.Size = new System.Drawing.Size(1113, 482);
            this.GvDetail.TabIndex = 20;
            this.GvDetail.UseRowNo = true;
            this.GvDetail.UseSetting = true;
            // 
            // coIInputDateTime
            // 
            this.coIInputDateTime.DataPropertyName = "InputDateTime";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.coIInputDateTime.DefaultCellStyle = dataGridViewCellStyle3;
            this.coIInputDateTime.HeaderText = "出荷日時";
            this.coIInputDateTime.Name = "coIInputDateTime";
            this.coIInputDateTime.ReadOnly = true;
            this.coIInputDateTime.Width = 150;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "SalesDateTime";
            this.Column1.HeaderText = "売上更新日時";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 150;
            // 
            // colEDIImportNO
            // 
            this.colEDIImportNO.DataPropertyName = "ShippingNO";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N0";
            dataGridViewCellStyle4.NullValue = null;
            this.colEDIImportNO.DefaultCellStyle = dataGridViewCellStyle4;
            this.colEDIImportNO.HeaderText = "出荷番号";
            this.colEDIImportNO.Name = "colEDIImportNO";
            this.colEDIImportNO.ReadOnly = true;
            this.colEDIImportNO.Width = 90;
            // 
            // colVendorName
            // 
            this.colVendorName.DataPropertyName = "DeliveryName";
            this.colVendorName.HeaderText = "出荷先";
            this.colVendorName.Name = "colVendorName";
            this.colVendorName.ReadOnly = true;
            this.colVendorName.Width = 300;
            // 
            // colShippingSu
            // 
            this.colShippingSu.DataPropertyName = "ShippingSu";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N0";
            dataGridViewCellStyle5.NullValue = "0";
            this.colShippingSu.DefaultCellStyle = dataGridViewCellStyle5;
            this.colShippingSu.HeaderText = "商品数";
            this.colShippingSu.Name = "colShippingSu";
            this.colShippingSu.ReadOnly = true;
            this.colShippingSu.Width = 80;
            // 
            // colSKUName
            // 
            this.colSKUName.DataPropertyName = "SKUName";
            this.colSKUName.HeaderText = "商品名（１行目）";
            this.colSKUName.Name = "colSKUName";
            this.colSKUName.ReadOnly = true;
            this.colSKUName.Width = 300;
            // 
            // colShippingNO
            // 
            this.colShippingNO.DataPropertyName = "ShippingNO";
            this.colShippingNO.HeaderText = "ShippingNO";
            this.colShippingNO.Name = "colShippingNO";
            this.colShippingNO.ReadOnly = true;
            this.colShippingNO.Visible = false;
            // 
            // lblRireki
            // 
            this.lblRireki.AutoSize = true;
            this.lblRireki.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblRireki.ForeColor = System.Drawing.Color.Black;
            this.lblRireki.Location = new System.Drawing.Point(123, 43);
            this.lblRireki.Name = "lblRireki";
            this.lblRireki.Size = new System.Drawing.Size(265, 12);
            this.lblRireki.TabIndex = 724;
            this.lblRireki.Text = "に出荷データから売上データを作成します。";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(21, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 12);
            this.label2.TabIndex = 725;
            this.label2.Text = "更新処理開始時刻";
            // 
            // ckM_TextBox1
            // 
            this.ckM_TextBox1.AllowMinus = false;
            this.ckM_TextBox1.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox1.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox1.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_TextBox1.DecimalPlace = 2;
            this.ckM_TextBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox1.IntegerPart = 0;
            this.ckM_TextBox1.IsCorrectDate = true;
            this.ckM_TextBox1.isEnterKeyDown = false;
            this.ckM_TextBox1.IsNumber = true;
            this.ckM_TextBox1.IsShop = false;
            this.ckM_TextBox1.Length = 5;
            this.ckM_TextBox1.Location = new System.Drawing.Point(53, 41);
            this.ckM_TextBox1.MaxLength = 5;
            this.ckM_TextBox1.MoveNext = true;
            this.ckM_TextBox1.Name = "ckM_TextBox1";
            this.ckM_TextBox1.Size = new System.Drawing.Size(63, 19);
            this.ckM_TextBox1.TabIndex = 726;
            this.ckM_TextBox1.Text = "24:59";
            this.ckM_TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ShukkaUriageUpdateShoukai
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.GvDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "ShukkaUriageUpdateShoukai";
            this.PanelHeaderHeight = 160;
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.GvDetail, 0);
            this.Controls.SetChildIndex(this.label11, 0);
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
        private Search.CKM_SearchControl ckM_SearchControl3;
        private Search.CKM_SearchControl ckM_SearchControl1;
        private CKM_Controls.CKM_Button BtnSubF11;
        private CKM_Controls.CKM_GridView GvDetail;
        private System.Windows.Forms.Label lblRireki;
        private System.Windows.Forms.Label label2;
        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn coIInputDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEDIImportNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVendorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShippingSu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShippingNO;
    }
}

