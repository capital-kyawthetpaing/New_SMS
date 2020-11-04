namespace EDINouhinJouhonTouroku
{
    partial class FrmEDINouhinJouhouTouroku
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEDINouhinJouhouTouroku));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.btnStart = new CKM_Controls.CKM_Button();
            this.btnStop = new CKM_Controls.CKM_Button();
            this.panelDetail = new System.Windows.Forms.Panel();
            this.lblEdiMode = new System.Windows.Forms.Label();
            this.gdvDSKENDeliveryDetail = new CKM_Controls.CKM_GridView();
            this.colSKENNouhinsho = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKENHacchuu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKENNouhinHinban = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKENJanCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSizeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKENNouhinSuu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colErrorText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblVendor = new CKM_Controls.CKM_Label();
            this.lblImportDateTime = new CKM_Controls.CKM_Label();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.chkCorrect = new CKM_Controls.CKM_CheckBox();
            this.chkError = new CKM_Controls.CKM_CheckBox();
            this.BtnSubF11 = new CKM_Controls.CKM_Button();
            this.BtnSubF10 = new CKM_Controls.CKM_Button();
            this.BtnSubF12 = new CKM_Controls.CKM_Button();
            this.gdvDSKENDelivery = new CKM_Controls.CKM_GridView();
            this.colChk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSKENNouhinshoNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colImportDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVendor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colImportDetailsSu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colErrorSu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colImportFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblRireki = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.panelDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdvDSKENDeliveryDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvDSKENDelivery)).BeginInit();
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
            // ckM_Label1
            // 
            this.ckM_Label1.AutoSize = true;
            this.ckM_Label1.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label1.DefaultlabelSize = true;
            this.ckM_Label1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label1.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label1.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label1.Location = new System.Drawing.Point(13, 9);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(156, 12);
            this.ckM_Label1.TabIndex = 9;
            this.ckM_Label1.Text = "【EDI納品情報更新処理】";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnStart.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.DefaultBtnSize = false;
            this.btnStart.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnStart.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnStart.Location = new System.Drawing.Point(275, 34);
            this.btnStart.Margin = new System.Windows.Forms.Padding(1);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(95, 23);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "開始";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnStop.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.DefaultBtnSize = false;
            this.btnStop.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnStop.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnStop.Location = new System.Drawing.Point(370, 34);
            this.btnStop.Margin = new System.Windows.Forms.Padding(1);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(95, 23);
            this.btnStop.TabIndex = 12;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.lblEdiMode);
            this.panelDetail.Controls.Add(this.gdvDSKENDeliveryDetail);
            this.panelDetail.Controls.Add(this.lblVendor);
            this.panelDetail.Controls.Add(this.lblImportDateTime);
            this.panelDetail.Controls.Add(this.ckM_Label5);
            this.panelDetail.Controls.Add(this.chkCorrect);
            this.panelDetail.Controls.Add(this.chkError);
            this.panelDetail.Controls.Add(this.BtnSubF11);
            this.panelDetail.Controls.Add(this.BtnSubF10);
            this.panelDetail.Controls.Add(this.BtnSubF12);
            this.panelDetail.Controls.Add(this.gdvDSKENDelivery);
            this.panelDetail.Controls.Add(this.lblRireki);
            this.panelDetail.Controls.Add(this.ckM_Label3);
            this.panelDetail.Controls.Add(this.btnStart);
            this.panelDetail.Controls.Add(this.btnStop);
            this.panelDetail.Controls.Add(this.ckM_Label1);
            this.panelDetail.Location = new System.Drawing.Point(1, 52);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(1713, 877);
            this.panelDetail.TabIndex = 13;
            // 
            // lblEdiMode
            // 
            this.lblEdiMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(240)))));
            this.lblEdiMode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEdiMode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblEdiMode.Font = new System.Drawing.Font("MS Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEdiMode.Location = new System.Drawing.Point(34, 32);
            this.lblEdiMode.Name = "lblEdiMode";
            this.lblEdiMode.Size = new System.Drawing.Size(200, 30);
            this.lblEdiMode.TabIndex = 25;
            this.lblEdiMode.Text = "処理実行中";
            this.lblEdiMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gdvDSKENDeliveryDetail
            // 
            this.gdvDSKENDeliveryDetail.AllowUserToAddRows = false;
            this.gdvDSKENDeliveryDetail.AllowUserToDeleteRows = false;
            this.gdvDSKENDeliveryDetail.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.gdvDSKENDeliveryDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gdvDSKENDeliveryDetail.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.gdvDSKENDeliveryDetail.CheckCol = ((System.Collections.ArrayList)(resources.GetObject("gdvDSKENDeliveryDetail.CheckCol")));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gdvDSKENDeliveryDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gdvDSKENDeliveryDetail.ColumnHeadersHeight = 25;
            this.gdvDSKENDeliveryDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSKENNouhinsho,
            this.colSKENHacchuu,
            this.colSKENNouhinHinban,
            this.colSKENJanCD,
            this.colSKUName,
            this.colColorName,
            this.colSizeName,
            this.colSKENNouhinSuu,
            this.colErrorText});
            this.gdvDSKENDeliveryDetail.EnableHeadersVisualStyles = false;
            this.gdvDSKENDeliveryDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.gdvDSKENDeliveryDetail.Location = new System.Drawing.Point(36, 377);
            this.gdvDSKENDeliveryDetail.Name = "gdvDSKENDeliveryDetail";
            this.gdvDSKENDeliveryDetail.RowHeight_ = 20;
            this.gdvDSKENDeliveryDetail.RowTemplate.Height = 20;
            this.gdvDSKENDeliveryDetail.Size = new System.Drawing.Size(1500, 450);
            this.gdvDSKENDeliveryDetail.TabIndex = 24;
            this.gdvDSKENDeliveryDetail.UseRowNo = true;
            this.gdvDSKENDeliveryDetail.UseSetting = true;
            // 
            // colSKENNouhinsho
            // 
            this.colSKENNouhinsho.DataPropertyName = "SKENNouhinshoNO";
            this.colSKENNouhinsho.HeaderText = "納品書番号";
            this.colSKENNouhinsho.Name = "colSKENNouhinsho";
            // 
            // colSKENHacchuu
            // 
            this.colSKENHacchuu.DataPropertyName = "SKENHacchuu";
            this.colSKENHacchuu.HeaderText = "発注番号";
            this.colSKENHacchuu.Name = "colSKENHacchuu";
            // 
            // colSKENNouhinHinban
            // 
            this.colSKENNouhinHinban.DataPropertyName = "SKENNouhinHinban";
            this.colSKENNouhinHinban.HeaderText = "メーカー商品CD";
            this.colSKENNouhinHinban.Name = "colSKENNouhinHinban";
            this.colSKENNouhinHinban.Width = 200;
            // 
            // colSKENJanCD
            // 
            this.colSKENJanCD.DataPropertyName = "SKENJanCD";
            this.colSKENJanCD.HeaderText = "JANCD";
            this.colSKENJanCD.Name = "colSKENJanCD";
            // 
            // colSKUName
            // 
            this.colSKUName.DataPropertyName = "SKUName";
            this.colSKUName.HeaderText = "商品名";
            this.colSKUName.Name = "colSKUName";
            this.colSKUName.Width = 400;
            // 
            // colColorName
            // 
            this.colColorName.DataPropertyName = "ColorName";
            this.colColorName.HeaderText = "カラー";
            this.colColorName.Name = "colColorName";
            // 
            // colSizeName
            // 
            this.colSizeName.DataPropertyName = "SizeName";
            this.colSizeName.HeaderText = "サイズ";
            this.colSizeName.Name = "colSizeName";
            // 
            // colSKENNouhinSuu
            // 
            this.colSKENNouhinSuu.DataPropertyName = "SKENNouhinSuu";
            this.colSKENNouhinSuu.HeaderText = "納品数";
            this.colSKENNouhinSuu.Name = "colSKENNouhinSuu";
            // 
            // colErrorText
            // 
            this.colErrorText.DataPropertyName = "ErrorText";
            this.colErrorText.HeaderText = "エラー";
            this.colErrorText.Name = "colErrorText";
            this.colErrorText.Width = 250;
            // 
            // lblVendor
            // 
            this.lblVendor.AutoSize = true;
            this.lblVendor.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Green;
            this.lblVendor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblVendor.DefaultlabelSize = true;
            this.lblVendor.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.lblVendor.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.lblVendor.ForeColor = System.Drawing.Color.Black;
            this.lblVendor.Location = new System.Drawing.Point(255, 361);
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Size = new System.Drawing.Size(0, 12);
            this.lblVendor.TabIndex = 23;
            this.lblVendor.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblVendor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblImportDateTime
            // 
            this.lblImportDateTime.AutoSize = true;
            this.lblImportDateTime.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Green;
            this.lblImportDateTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblImportDateTime.DefaultlabelSize = true;
            this.lblImportDateTime.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.lblImportDateTime.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.lblImportDateTime.ForeColor = System.Drawing.Color.Black;
            this.lblImportDateTime.Location = new System.Drawing.Point(109, 361);
            this.lblImportDateTime.Name = "lblImportDateTime";
            this.lblImportDateTime.Size = new System.Drawing.Size(0, 12);
            this.lblImportDateTime.TabIndex = 22;
            this.lblImportDateTime.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblImportDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.ckM_Label5.Location = new System.Drawing.Point(13, 361);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label5.TabIndex = 21;
            this.ckM_Label5.Text = "【受信明細】";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkCorrect
            // 
            this.chkCorrect.AutoSize = true;
            this.chkCorrect.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkCorrect.Location = new System.Drawing.Point(1059, 303);
            this.chkCorrect.Name = "chkCorrect";
            this.chkCorrect.Size = new System.Drawing.Size(63, 16);
            this.chkCorrect.TabIndex = 20;
            this.chkCorrect.Text = "納品書";
            this.chkCorrect.UseVisualStyleBackColor = true;
            // 
            // chkError
            // 
            this.chkError.AutoSize = true;
            this.chkError.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkError.Location = new System.Drawing.Point(993, 303);
            this.chkError.Name = "chkError";
            this.chkError.Size = new System.Drawing.Size(63, 16);
            this.chkError.TabIndex = 19;
            this.chkError.Text = "エラー";
            this.chkError.UseVisualStyleBackColor = true;
            // 
            // BtnSubF11
            // 
            this.BtnSubF11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BtnSubF11.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BtnSubF11.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSubF11.DefaultBtnSize = false;
            this.BtnSubF11.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnSubF11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSubF11.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.BtnSubF11.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BtnSubF11.Location = new System.Drawing.Point(1087, 322);
            this.BtnSubF11.Margin = new System.Windows.Forms.Padding(1);
            this.BtnSubF11.Name = "BtnSubF11";
            this.BtnSubF11.Size = new System.Drawing.Size(95, 23);
            this.BtnSubF11.TabIndex = 18;
            this.BtnSubF11.Text = "表示(F11)";
            this.BtnSubF11.UseVisualStyleBackColor = false;
            this.BtnSubF11.Click += new System.EventHandler(this.BtnSubF11_Click);
            // 
            // BtnSubF10
            // 
            this.BtnSubF10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BtnSubF10.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BtnSubF10.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSubF10.DefaultBtnSize = false;
            this.BtnSubF10.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnSubF10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSubF10.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.BtnSubF10.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BtnSubF10.Location = new System.Drawing.Point(992, 322);
            this.BtnSubF10.Margin = new System.Windows.Forms.Padding(1);
            this.BtnSubF10.Name = "BtnSubF10";
            this.BtnSubF10.Size = new System.Drawing.Size(95, 23);
            this.BtnSubF10.TabIndex = 17;
            this.BtnSubF10.Text = "印刷(F10)";
            this.BtnSubF10.UseVisualStyleBackColor = false;
            this.BtnSubF10.Click += new System.EventHandler(this.BtnSubF10_Click);
            // 
            // BtnSubF12
            // 
            this.BtnSubF12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BtnSubF12.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BtnSubF12.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSubF12.DefaultBtnSize = false;
            this.BtnSubF12.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnSubF12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSubF12.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.BtnSubF12.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BtnSubF12.Location = new System.Drawing.Point(891, 69);
            this.BtnSubF12.Margin = new System.Windows.Forms.Padding(1);
            this.BtnSubF12.Name = "BtnSubF12";
            this.BtnSubF12.Size = new System.Drawing.Size(95, 23);
            this.BtnSubF12.TabIndex = 16;
            this.BtnSubF12.Text = "最新化(F12)";
            this.BtnSubF12.UseVisualStyleBackColor = false;
            this.BtnSubF12.Click += new System.EventHandler(this.BtnSubF12_Click);
            // 
            // gdvDSKENDelivery
            // 
            this.gdvDSKENDelivery.AllowUserToAddRows = false;
            this.gdvDSKENDelivery.AllowUserToDeleteRows = false;
            this.gdvDSKENDelivery.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.gdvDSKENDelivery.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.gdvDSKENDelivery.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.gdvDSKENDelivery.CheckCol = ((System.Collections.ArrayList)(resources.GetObject("gdvDSKENDelivery.CheckCol")));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gdvDSKENDelivery.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gdvDSKENDelivery.ColumnHeadersHeight = 25;
            this.gdvDSKENDelivery.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colChk,
            this.colSKENNouhinshoNO,
            this.colImportDateTime,
            this.colVendor,
            this.colImportDetailsSu,
            this.colErrorSu,
            this.colImportFile});
            this.gdvDSKENDelivery.EnableHeadersVisualStyles = false;
            this.gdvDSKENDelivery.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.gdvDSKENDelivery.Location = new System.Drawing.Point(36, 95);
            this.gdvDSKENDelivery.Name = "gdvDSKENDelivery";
            this.gdvDSKENDelivery.RowHeight_ = 20;
            this.gdvDSKENDelivery.RowTemplate.Height = 20;
            this.gdvDSKENDelivery.Size = new System.Drawing.Size(950, 250);
            this.gdvDSKENDelivery.TabIndex = 15;
            this.gdvDSKENDelivery.UseRowNo = true;
            this.gdvDSKENDelivery.UseSetting = true;
            this.gdvDSKENDelivery.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gdvDSKENDelivery_CellContentClick);
            // 
            // colChk
            // 
            this.colChk.FalseValue = "";
            this.colChk.HeaderText = "結果";
            this.colChk.IndeterminateValue = "false";
            this.colChk.Name = "colChk";
            this.colChk.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colChk.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colChk.TrueValue = "true";
            this.colChk.Width = 40;
            // 
            // colSKENNouhinshoNO
            // 
            this.colSKENNouhinshoNO.DataPropertyName = "SKENNouhinshoNO";
            this.colSKENNouhinshoNO.HeaderText = "納品書番号";
            this.colSKENNouhinshoNO.Name = "colSKENNouhinshoNO";
            // 
            // colImportDateTime
            // 
            this.colImportDateTime.DataPropertyName = "ImportDateTime";
            this.colImportDateTime.HeaderText = "受信日時";
            this.colImportDateTime.Name = "colImportDateTime";
            this.colImportDateTime.Width = 150;
            // 
            // colVendor
            // 
            this.colVendor.DataPropertyName = "Vendor";
            this.colVendor.HeaderText = "仕入先";
            this.colVendor.Name = "colVendor";
            this.colVendor.Width = 200;
            // 
            // colImportDetailsSu
            // 
            this.colImportDetailsSu.DataPropertyName = "ImportDetailsSu";
            this.colImportDetailsSu.HeaderText = "取込数";
            this.colImportDetailsSu.Name = "colImportDetailsSu";
            // 
            // colErrorSu
            // 
            this.colErrorSu.DataPropertyName = "ErrorSu";
            this.colErrorSu.HeaderText = "エラー数";
            this.colErrorSu.Name = "colErrorSu";
            // 
            // colImportFile
            // 
            this.colImportFile.DataPropertyName = "ImportFile";
            this.colImportFile.HeaderText = "取込ファイル";
            this.colImportFile.Name = "colImportFile";
            this.colImportFile.Width = 200;
            // 
            // lblRireki
            // 
            this.lblRireki.AutoSize = true;
            this.lblRireki.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblRireki.BackColor = System.Drawing.Color.Transparent;
            this.lblRireki.DefaultlabelSize = true;
            this.lblRireki.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.lblRireki.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.lblRireki.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblRireki.Location = new System.Drawing.Point(102, 80);
            this.lblRireki.Name = "lblRireki";
            this.lblRireki.Size = new System.Drawing.Size(188, 12);
            this.lblRireki.TabIndex = 14;
            this.lblRireki.Text = "14日間の履歴を保持しています";
            this.lblRireki.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Green;
            this.lblRireki.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label3.Location = new System.Drawing.Point(13, 80);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label3.TabIndex = 13;
            this.ckM_Label3.Text = "【受信履歴】";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmEDINouhinJouhouTouroku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.panelDetail);
            this.F9Visible = false;
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "FrmEDINouhinJouhouTouroku";
            this.PanelHeaderHeight = 50;
            this.Text = "EDINouhinJouhouTouroku";
            this.Load += new System.EventHandler(this.FrmEDINouhinJouhouTouroku_Load);
            this.Controls.SetChildIndex(this.panelDetail, 0);
            this.panelDetail.ResumeLayout(false);
            this.panelDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdvDSKENDeliveryDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvDSKENDelivery)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_Button btnStart;
        private CKM_Controls.CKM_Button btnStop;
        private System.Windows.Forms.Panel panelDetail;
        private CKM_Controls.CKM_GridView gdvDSKENDelivery;
        private CKM_Controls.CKM_Label lblRireki;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Button BtnSubF12;
        private CKM_Controls.CKM_Button BtnSubF11;
        private CKM_Controls.CKM_Button BtnSubF10;
        private CKM_Controls.CKM_CheckBox chkCorrect;
        private CKM_Controls.CKM_CheckBox chkError;
        private CKM_Controls.CKM_Label lblVendor;
        private CKM_Controls.CKM_Label lblImportDateTime;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_GridView gdvDSKENDeliveryDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKENNouhinsho;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKENHacchuu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKENNouhinHinban;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKENJanCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSizeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKENNouhinSuu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colErrorText;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colChk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKENNouhinshoNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colImportDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVendor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colImportDetailsSu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colErrorSu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colImportFile;
        private System.Windows.Forms.Label lblEdiMode;
    }
}

