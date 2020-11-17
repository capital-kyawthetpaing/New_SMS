namespace MailHistoryShoukai
{
    partial class MailHistoryShoukai
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MailHistoryShoukai));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label4 = new CKM_Controls.CKM_Label();
            this.label11 = new CKM_Controls.CKM_Label();
            this.label1 = new CKM_Controls.CKM_Label();
            this.label27 = new CKM_Controls.CKM_Label();
            this.ckM_SearchControl3 = new Search.CKM_SearchControl();
            this.BtnSubF12 = new CKM_Controls.CKM_Button();
            this.ckM_SearchControl1 = new Search.CKM_SearchControl();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.GvDetail = new CKM_Controls.CKM_GridView();
            this.coIMailDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMailSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMailCounter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMailKBN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Customer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MailContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblEdiMode = new System.Windows.Forms.Label();
            this.BtnStart = new CKM_Controls.CKM_Button();
            this.BtnStop = new CKM_Controls.CKM_Button();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblSendedDateTime = new System.Windows.Forms.Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.lblTitle = new CKM_Controls.CKM_Label();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox2 = new CKM_Controls.CKM_TextBox();
            this.ckM_TextBox1 = new CKM_Controls.CKM_TextBox();
            this.ScCustomer = new Search.CKM_SearchControl();
            this.ckM_ComboBox1 = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label8 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox3 = new CKM_Controls.CKM_TextBox();
            this.ckM_TextBox4 = new CKM_Controls.CKM_TextBox();
            this.ckM_ComboBox2 = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.ckM_Label7 = new CKM_Controls.CKM_Label();
            this.ckM_Label9 = new CKM_Controls.CKM_Label();
            this.lblMailKBN = new System.Windows.Forms.Label();
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.ckM_Label11 = new CKM_Controls.CKM_Label();
            this.ckM_Label12 = new CKM_Controls.CKM_Label();
            this.ckM_Label14 = new CKM_Controls.CKM_Label();
            this.lblAddress1 = new System.Windows.Forms.Label();
            this.lblJuchuuNo = new System.Windows.Forms.Label();
            this.ckM_Label13 = new CKM_Controls.CKM_Label();
            this.lblAddress2 = new System.Windows.Forms.Label();
            this.lblAddress3 = new System.Windows.Forms.Label();
            this.lblMailSubject = new System.Windows.Forms.Label();
            this.lblMailContent = new System.Windows.Forms.Label();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.BtnStart);
            this.PanelHeader.Controls.Add(this.BtnStop);
            this.PanelHeader.Controls.Add(this.lblEdiMode);
            this.PanelHeader.Controls.Add(this.label4);
            this.PanelHeader.Size = new System.Drawing.Size(1368, 54);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.TabStop = true;
            this.PanelHeader.Controls.SetChildIndex(this.label4, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblEdiMode, 0);
            this.PanelHeader.Controls.SetChildIndex(this.BtnStop, 0);
            this.PanelHeader.Controls.SetChildIndex(this.BtnStart, 0);
            // 
            // PanelSearch
            // 
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
            this.label4.Location = new System.Drawing.Point(9, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 12);
            this.label4.TabIndex = 255;
            this.label4.Text = "【メール送信】";
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
            this.label11.Location = new System.Drawing.Point(10, 126);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(122, 12);
            this.label11.TabIndex = 261;
            this.label11.Text = "【メール送信履歴】";
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
            // BtnSubF12
            // 
            this.BtnSubF12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BtnSubF12.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BtnSubF12.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSubF12.DefaultBtnSize = true;
            this.BtnSubF12.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnSubF12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSubF12.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.BtnSubF12.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BtnSubF12.Location = new System.Drawing.Point(353, 177);
            this.BtnSubF12.Margin = new System.Windows.Forms.Padding(1);
            this.BtnSubF12.Name = "BtnSubF12";
            this.BtnSubF12.Size = new System.Drawing.Size(118, 28);
            this.BtnSubF12.TabIndex = 9;
            this.BtnSubF12.Text = "表示(F12)";
            this.BtnSubF12.UseVisualStyleBackColor = false;
            this.BtnSubF12.Click += new System.EventHandler(this.BtnSubF12_Click);
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
            // ckM_Label1
            // 
            this.ckM_Label1.AutoSize = true;
            this.ckM_Label1.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label1.DefaultlabelSize = true;
            this.ckM_Label1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label1.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label1.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label1.Location = new System.Drawing.Point(41, 149);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label1.TabIndex = 721;
            this.ckM_Label1.Text = "送信日時";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GvDetail
            // 
            this.GvDetail.AllowUserToAddRows = false;
            this.GvDetail.AllowUserToDeleteRows = false;
            this.GvDetail.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.GvDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.GvDetail.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GvDetail.CheckCol = ((System.Collections.ArrayList)(resources.GetObject("GvDetail.CheckCol")));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GvDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.GvDetail.ColumnHeadersHeight = 25;
            this.GvDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.coIMailDateTime,
            this.colMailSubject,
            this.colNumber,
            this.colMailCounter,
            this.colMailKBN,
            this.Customer,
            this.MailContent});
            this.GvDetail.EnableHeadersVisualStyles = false;
            this.GvDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GvDetail.Location = new System.Drawing.Point(99, 278);
            this.GvDetail.Name = "GvDetail";
            this.GvDetail.ReadOnly = true;
            this.GvDetail.RowHeight_ = 20;
            this.GvDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GvDetail.Size = new System.Drawing.Size(508, 427);
            this.GvDetail.TabIndex = 20;
            this.GvDetail.UseRowNo = true;
            this.GvDetail.UseSetting = true;
            this.GvDetail.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvDetail_RowEnter);
            // 
            // coIMailDateTime
            // 
            this.coIMailDateTime.DataPropertyName = "MailDateTime";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.coIMailDateTime.DefaultCellStyle = dataGridViewCellStyle3;
            this.coIMailDateTime.HeaderText = "送信日";
            this.coIMailDateTime.Name = "coIMailDateTime";
            this.coIMailDateTime.ReadOnly = true;
            this.coIMailDateTime.Width = 140;
            // 
            // colMailSubject
            // 
            this.colMailSubject.DataPropertyName = "MailSubject";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.NullValue = null;
            this.colMailSubject.DefaultCellStyle = dataGridViewCellStyle4;
            this.colMailSubject.HeaderText = "メール件名";
            this.colMailSubject.Name = "colMailSubject";
            this.colMailSubject.ReadOnly = true;
            this.colMailSubject.Width = 200;
            // 
            // colNumber
            // 
            this.colNumber.DataPropertyName = "Number";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colNumber.DefaultCellStyle = dataGridViewCellStyle5;
            this.colNumber.HeaderText = "番号";
            this.colNumber.Name = "colNumber";
            this.colNumber.ReadOnly = true;
            this.colNumber.Width = 120;
            // 
            // colMailCounter
            // 
            this.colMailCounter.DataPropertyName = "MailCounter";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            dataGridViewCellStyle6.NullValue = "0";
            this.colMailCounter.DefaultCellStyle = dataGridViewCellStyle6;
            this.colMailCounter.HeaderText = "メールカウンター";
            this.colMailCounter.Name = "colMailCounter";
            this.colMailCounter.ReadOnly = true;
            this.colMailCounter.Visible = false;
            this.colMailCounter.Width = 80;
            // 
            // colMailKBN
            // 
            this.colMailKBN.DataPropertyName = "MailKBN";
            this.colMailKBN.HeaderText = "MailKBN";
            this.colMailKBN.Name = "colMailKBN";
            this.colMailKBN.ReadOnly = true;
            this.colMailKBN.Visible = false;
            // 
            // Customer
            // 
            this.Customer.DataPropertyName = "Customer";
            this.Customer.HeaderText = "Customer";
            this.Customer.Name = "Customer";
            this.Customer.ReadOnly = true;
            this.Customer.Visible = false;
            // 
            // MailContent
            // 
            this.MailContent.DataPropertyName = "MailContent";
            this.MailContent.HeaderText = "MailContent";
            this.MailContent.Name = "MailContent";
            this.MailContent.ReadOnly = true;
            this.MailContent.Visible = false;
            // 
            // lblEdiMode
            // 
            this.lblEdiMode.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.lblEdiMode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEdiMode.Font = new System.Drawing.Font("ＭＳ ゴシック", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblEdiMode.Location = new System.Drawing.Point(35, 20);
            this.lblEdiMode.Name = "lblEdiMode";
            this.lblEdiMode.Size = new System.Drawing.Size(191, 31);
            this.lblEdiMode.TabIndex = 722;
            this.lblEdiMode.Text = "処理実行中";
            this.lblEdiMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnStart
            // 
            this.BtnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BtnStart.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BtnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnStart.DefaultBtnSize = true;
            this.BtnStart.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnStart.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.BtnStart.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BtnStart.Location = new System.Drawing.Point(261, 24);
            this.BtnStart.Margin = new System.Windows.Forms.Padding(1);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(118, 28);
            this.BtnStart.TabIndex = 0;
            this.BtnStart.Text = "開始";
            this.BtnStart.UseVisualStyleBackColor = false;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BtnStop.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BtnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnStop.DefaultBtnSize = true;
            this.BtnStop.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnStop.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.BtnStop.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BtnStop.Location = new System.Drawing.Point(379, 24);
            this.BtnStop.Margin = new System.Windows.Forms.Padding(1);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(118, 28);
            this.BtnStop.TabIndex = 1;
            this.BtnStop.Text = "停止";
            this.BtnStop.UseVisualStyleBackColor = false;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // lblCustomer
            // 
            this.lblCustomer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblCustomer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCustomer.Location = new System.Drawing.Point(1050, 146);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(315, 20);
            this.lblCustomer.TabIndex = 723;
            this.lblCustomer.Text = "XXXXXXXXXX ＸＸＸＸＸＸＸＸＸ10ＸＸＸＸＸＸＸＸＸ20";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSendedDateTime
            // 
            this.lblSendedDateTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblSendedDateTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSendedDateTime.Location = new System.Drawing.Point(621, 146);
            this.lblSendedDateTime.Name = "lblSendedDateTime";
            this.lblSendedDateTime.Size = new System.Drawing.Size(107, 20);
            this.lblSendedDateTime.TabIndex = 722;
            this.lblSendedDateTime.Text = " ";
            this.lblSendedDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.ckM_Label2.Location = new System.Drawing.Point(28, 180);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label2.TabIndex = 726;
            this.ckM_Label2.Text = "メール種別";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.DefaultlabelSize = true;
            this.lblTitle.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(52, 247);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(44, 12);
            this.lblTitle.TabIndex = 728;
            this.lblTitle.Text = "得意先";
            this.lblTitle.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label4.Location = new System.Drawing.Point(26, 211);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label4.TabIndex = 727;
            this.ckM_Label4.Text = "メール分類";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label5
            // 
            this.ckM_Label5.AutoSize = true;
            this.ckM_Label5.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label5.DefaultlabelSize = true;
            this.ckM_Label5.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label5.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label5.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label5.Location = new System.Drawing.Point(26, 279);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label5.TabIndex = 729;
            this.ckM_Label5.Text = "メール履歴";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_TextBox2
            // 
            this.ckM_TextBox2.AllowMinus = false;
            this.ckM_TextBox2.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox2.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox2.BorderColor = false;
            this.ckM_TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox2.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox2.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.ckM_TextBox2.DecimalPlace = 0;
            this.ckM_TextBox2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox2.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox2.IntegerPart = 0;
            this.ckM_TextBox2.IsCorrectDate = true;
            this.ckM_TextBox2.isEnterKeyDown = false;
            this.ckM_TextBox2.IsFirstTime = true;
            this.ckM_TextBox2.isMaxLengthErr = false;
            this.ckM_TextBox2.IsNumber = true;
            this.ckM_TextBox2.IsShop = false;
            this.ckM_TextBox2.Length = 10;
            this.ckM_TextBox2.Location = new System.Drawing.Point(308, 147);
            this.ckM_TextBox2.MaxLength = 10;
            this.ckM_TextBox2.MoveNext = true;
            this.ckM_TextBox2.Name = "ckM_TextBox2";
            this.ckM_TextBox2.Size = new System.Drawing.Size(100, 19);
            this.ckM_TextBox2.TabIndex = 4;
            this.ckM_TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox2.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.ckM_TextBox2.UseColorSizMode = false;
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
            this.ckM_TextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox1.IntegerPart = 0;
            this.ckM_TextBox1.IsCorrectDate = true;
            this.ckM_TextBox1.isEnterKeyDown = false;
            this.ckM_TextBox1.IsFirstTime = true;
            this.ckM_TextBox1.isMaxLengthErr = false;
            this.ckM_TextBox1.IsNumber = true;
            this.ckM_TextBox1.IsShop = false;
            this.ckM_TextBox1.Length = 10;
            this.ckM_TextBox1.Location = new System.Drawing.Point(99, 147);
            this.ckM_TextBox1.MaxLength = 10;
            this.ckM_TextBox1.MoveNext = true;
            this.ckM_TextBox1.Name = "ckM_TextBox1";
            this.ckM_TextBox1.Size = new System.Drawing.Size(100, 19);
            this.ckM_TextBox1.TabIndex = 2;
            this.ckM_TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.ckM_TextBox1.UseColorSizMode = false;
            // 
            // ScCustomer
            // 
            this.ScCustomer.AutoSize = true;
            this.ScCustomer.ChangeDate = "";
            this.ScCustomer.ChangeDateWidth = 100;
            this.ScCustomer.Code = "";
            this.ScCustomer.CodeWidth = 100;
            this.ScCustomer.CodeWidth1 = 100;
            this.ScCustomer.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScCustomer.DataCheck = true;
            this.ScCustomer.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ScCustomer.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ScCustomer.IsCopy = false;
            this.ScCustomer.LabelText = "";
            this.ScCustomer.LabelVisible = true;
            this.ScCustomer.Location = new System.Drawing.Point(99, 239);
            this.ScCustomer.Margin = new System.Windows.Forms.Padding(0);
            this.ScCustomer.Name = "ScCustomer";
            this.ScCustomer.NameWidth = 500;
            this.ScCustomer.SearchEnable = true;
            this.ScCustomer.Size = new System.Drawing.Size(634, 28);
            this.ScCustomer.Stype = Search.CKM_SearchControl.SearchType.得意先;
            this.ScCustomer.TabIndex = 8;
            this.ScCustomer.test = null;
            this.ScCustomer.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScCustomer.UseChangeDate = false;
            this.ScCustomer.Value1 = null;
            this.ScCustomer.Value2 = null;
            this.ScCustomer.Value3 = null;
            // 
            // ckM_ComboBox1
            // 
            this.ckM_ComboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.ckM_ComboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ckM_ComboBox1.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.入荷予定状況;
            this.ckM_ComboBox1.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.ckM_ComboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ckM_ComboBox1.Flag = 0;
            this.ckM_ComboBox1.FormattingEnabled = true;
            this.ckM_ComboBox1.Length = 20;
            this.ckM_ComboBox1.Location = new System.Drawing.Point(99, 177);
            this.ckM_ComboBox1.MaxLength = 10;
            this.ckM_ComboBox1.MoveNext = true;
            this.ckM_ComboBox1.Name = "ckM_ComboBox1";
            this.ckM_ComboBox1.Size = new System.Drawing.Size(140, 20);
            this.ckM_ComboBox1.TabIndex = 6;
            this.ckM_ComboBox1.SelectedIndexChanged += new System.EventHandler(this.ckM_ComboBox1_SelectedIndexChanged);
            // 
            // ckM_Label8
            // 
            this.ckM_Label8.AutoSize = true;
            this.ckM_Label8.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label8.DefaultlabelSize = true;
            this.ckM_Label8.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label8.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label8.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label8.Location = new System.Drawing.Point(284, 150);
            this.ckM_Label8.Name = "ckM_Label8";
            this.ckM_Label8.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label8.TabIndex = 734;
            this.ckM_Label8.Text = "～";
            this.ckM_Label8.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_TextBox3
            // 
            this.ckM_TextBox3.AllowMinus = false;
            this.ckM_TextBox3.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox3.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox3.BorderColor = false;
            this.ckM_TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox3.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox3.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox3.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_TextBox3.DecimalPlace = 2;
            this.ckM_TextBox3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox3.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox3.IntegerPart = 0;
            this.ckM_TextBox3.IsCorrectDate = true;
            this.ckM_TextBox3.isEnterKeyDown = false;
            this.ckM_TextBox3.IsFirstTime = true;
            this.ckM_TextBox3.isMaxLengthErr = false;
            this.ckM_TextBox3.IsNumber = true;
            this.ckM_TextBox3.IsShop = false;
            this.ckM_TextBox3.Length = 5;
            this.ckM_TextBox3.Location = new System.Drawing.Point(199, 147);
            this.ckM_TextBox3.MaxLength = 5;
            this.ckM_TextBox3.MoveNext = true;
            this.ckM_TextBox3.Name = "ckM_TextBox3";
            this.ckM_TextBox3.Size = new System.Drawing.Size(63, 19);
            this.ckM_TextBox3.TabIndex = 3;
            this.ckM_TextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox3.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.ckM_TextBox3.UseColorSizMode = false;
            // 
            // ckM_TextBox4
            // 
            this.ckM_TextBox4.AllowMinus = false;
            this.ckM_TextBox4.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox4.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox4.BorderColor = false;
            this.ckM_TextBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox4.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox4.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox4.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_TextBox4.DecimalPlace = 2;
            this.ckM_TextBox4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox4.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox4.IntegerPart = 0;
            this.ckM_TextBox4.IsCorrectDate = true;
            this.ckM_TextBox4.isEnterKeyDown = false;
            this.ckM_TextBox4.IsFirstTime = true;
            this.ckM_TextBox4.isMaxLengthErr = false;
            this.ckM_TextBox4.IsNumber = true;
            this.ckM_TextBox4.IsShop = false;
            this.ckM_TextBox4.Length = 5;
            this.ckM_TextBox4.Location = new System.Drawing.Point(408, 147);
            this.ckM_TextBox4.MaxLength = 5;
            this.ckM_TextBox4.MoveNext = true;
            this.ckM_TextBox4.Name = "ckM_TextBox4";
            this.ckM_TextBox4.Size = new System.Drawing.Size(63, 19);
            this.ckM_TextBox4.TabIndex = 5;
            this.ckM_TextBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox4.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.ckM_TextBox4.UseColorSizMode = false;
            // 
            // ckM_ComboBox2
            // 
            this.ckM_ComboBox2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.ckM_ComboBox2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ckM_ComboBox2.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.入荷予定状況;
            this.ckM_ComboBox2.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.ckM_ComboBox2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ckM_ComboBox2.Flag = 0;
            this.ckM_ComboBox2.FormattingEnabled = true;
            this.ckM_ComboBox2.Length = 20;
            this.ckM_ComboBox2.Location = new System.Drawing.Point(99, 208);
            this.ckM_ComboBox2.MaxLength = 10;
            this.ckM_ComboBox2.MoveNext = true;
            this.ckM_ComboBox2.Name = "ckM_ComboBox2";
            this.ckM_ComboBox2.Size = new System.Drawing.Size(140, 20);
            this.ckM_ComboBox2.TabIndex = 7;
            // 
            // ckM_Label6
            // 
            this.ckM_Label6.AutoSize = true;
            this.ckM_Label6.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label6.DefaultlabelSize = true;
            this.ckM_Label6.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label6.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label6.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label6.Location = new System.Drawing.Point(732, 150);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label6.TabIndex = 739;
            this.ckM_Label6.Text = "メール分類";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label7
            // 
            this.ckM_Label7.AutoSize = true;
            this.ckM_Label7.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label7.DefaultlabelSize = true;
            this.ckM_Label7.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label7.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label7.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label7.Location = new System.Drawing.Point(561, 150);
            this.ckM_Label7.Name = "ckM_Label7";
            this.ckM_Label7.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label7.TabIndex = 738;
            this.ckM_Label7.Text = "送信日時";
            this.ckM_Label7.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label9
            // 
            this.ckM_Label9.AutoSize = true;
            this.ckM_Label9.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label9.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label9.DefaultlabelSize = true;
            this.ckM_Label9.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label9.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label9.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label9.Location = new System.Drawing.Point(1000, 150);
            this.ckM_Label9.Name = "ckM_Label9";
            this.ckM_Label9.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label9.TabIndex = 740;
            this.ckM_Label9.Text = "得意先";
            this.ckM_Label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMailKBN
            // 
            this.lblMailKBN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblMailKBN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMailKBN.Location = new System.Drawing.Point(808, 146);
            this.lblMailKBN.Name = "lblMailKBN";
            this.lblMailKBN.Size = new System.Drawing.Size(176, 20);
            this.lblMailKBN.TabIndex = 741;
            this.lblMailKBN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.ckM_Label10.Location = new System.Drawing.Point(592, 210);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(26, 12);
            this.ckM_Label10.TabIndex = 745;
            this.ckM_Label10.Text = "bcc";
            this.ckM_Label10.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label11.Location = new System.Drawing.Point(599, 190);
            this.ckM_Label11.Name = "ckM_Label11";
            this.ckM_Label11.Size = new System.Drawing.Size(19, 12);
            this.ckM_Label11.TabIndex = 744;
            this.ckM_Label11.Text = "cc";
            this.ckM_Label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label12.Location = new System.Drawing.Point(574, 170);
            this.ckM_Label12.Name = "ckM_Label12";
            this.ckM_Label12.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label12.TabIndex = 743;
            this.ckM_Label12.Text = "送信先";
            this.ckM_Label12.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label14
            // 
            this.ckM_Label14.AutoSize = true;
            this.ckM_Label14.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label14.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label14.DefaultlabelSize = true;
            this.ckM_Label14.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label14.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label14.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label14.Location = new System.Drawing.Point(548, 230);
            this.ckM_Label14.Name = "ckM_Label14";
            this.ckM_Label14.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label14.TabIndex = 746;
            this.ckM_Label14.Text = "メール件名";
            this.ckM_Label14.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAddress1
            // 
            this.lblAddress1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblAddress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddress1.Location = new System.Drawing.Point(621, 166);
            this.lblAddress1.Name = "lblAddress1";
            this.lblAddress1.Size = new System.Drawing.Size(363, 20);
            this.lblAddress1.TabIndex = 747;
            this.lblAddress1.Text = " ";
            this.lblAddress1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblJuchuuNo
            // 
            this.lblJuchuuNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblJuchuuNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblJuchuuNo.Location = new System.Drawing.Point(1050, 166);
            this.lblJuchuuNo.Name = "lblJuchuuNo";
            this.lblJuchuuNo.Size = new System.Drawing.Size(92, 20);
            this.lblJuchuuNo.TabIndex = 749;
            this.lblJuchuuNo.Text = "入荷予定メール";
            this.lblJuchuuNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ckM_Label13
            // 
            this.ckM_Label13.AutoSize = true;
            this.ckM_Label13.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label13.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label13.DefaultlabelSize = true;
            this.ckM_Label13.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label13.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label13.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label13.Location = new System.Drawing.Point(987, 170);
            this.ckM_Label13.Name = "ckM_Label13";
            this.ckM_Label13.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label13.TabIndex = 748;
            this.ckM_Label13.Text = "伝票番号";
            this.ckM_Label13.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAddress2
            // 
            this.lblAddress2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblAddress2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddress2.Location = new System.Drawing.Point(621, 186);
            this.lblAddress2.Name = "lblAddress2";
            this.lblAddress2.Size = new System.Drawing.Size(363, 20);
            this.lblAddress2.TabIndex = 750;
            this.lblAddress2.Text = " ";
            this.lblAddress2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAddress3
            // 
            this.lblAddress3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblAddress3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAddress3.Location = new System.Drawing.Point(621, 206);
            this.lblAddress3.Name = "lblAddress3";
            this.lblAddress3.Size = new System.Drawing.Size(363, 20);
            this.lblAddress3.TabIndex = 751;
            this.lblAddress3.Text = " ";
            this.lblAddress3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMailSubject
            // 
            this.lblMailSubject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblMailSubject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMailSubject.Location = new System.Drawing.Point(621, 226);
            this.lblMailSubject.Name = "lblMailSubject";
            this.lblMailSubject.Size = new System.Drawing.Size(603, 20);
            this.lblMailSubject.TabIndex = 752;
            this.lblMailSubject.Text = " ";
            this.lblMailSubject.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMailContent
            // 
            this.lblMailContent.BackColor = System.Drawing.Color.White;
            this.lblMailContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMailContent.Location = new System.Drawing.Point(621, 279);
            this.lblMailContent.Name = "lblMailContent";
            this.lblMailContent.Size = new System.Drawing.Size(602, 425);
            this.lblMailContent.TabIndex = 753;
            this.lblMailContent.Text = "(説明)送信したメール文書をここに表示する";
            // 
            // MailHistoryShoukai
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.lblMailContent);
            this.Controls.Add(this.lblMailSubject);
            this.Controls.Add(this.lblAddress3);
            this.Controls.Add(this.lblAddress2);
            this.Controls.Add(this.lblJuchuuNo);
            this.Controls.Add(this.ckM_Label13);
            this.Controls.Add(this.lblAddress1);
            this.Controls.Add(this.ckM_Label14);
            this.Controls.Add(this.ckM_Label10);
            this.Controls.Add(this.ckM_Label11);
            this.Controls.Add(this.ckM_Label12);
            this.Controls.Add(this.lblMailKBN);
            this.Controls.Add(this.ckM_Label9);
            this.Controls.Add(this.ckM_Label6);
            this.Controls.Add(this.ckM_Label7);
            this.Controls.Add(this.ckM_ComboBox2);
            this.Controls.Add(this.ckM_TextBox4);
            this.Controls.Add(this.ckM_TextBox3);
            this.Controls.Add(this.ckM_ComboBox1);
            this.Controls.Add(this.ckM_Label8);
            this.Controls.Add(this.ScCustomer);
            this.Controls.Add(this.ckM_TextBox2);
            this.Controls.Add(this.ckM_TextBox1);
            this.Controls.Add(this.ckM_Label5);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.ckM_Label4);
            this.Controls.Add(this.ckM_Label2);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.lblSendedDateTime);
            this.Controls.Add(this.BtnSubF12);
            this.Controls.Add(this.ckM_Label1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.GvDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "MailHistoryShoukai";
            this.PanelHeaderHeight = 110;
            this.Text = "MailHstoryShoukai";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.GvDetail, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.Controls.SetChildIndex(this.BtnSubF12, 0);
            this.Controls.SetChildIndex(this.lblSendedDateTime, 0);
            this.Controls.SetChildIndex(this.lblCustomer, 0);
            this.Controls.SetChildIndex(this.ckM_Label2, 0);
            this.Controls.SetChildIndex(this.ckM_Label4, 0);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.ckM_Label5, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox1, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox2, 0);
            this.Controls.SetChildIndex(this.ScCustomer, 0);
            this.Controls.SetChildIndex(this.ckM_Label8, 0);
            this.Controls.SetChildIndex(this.ckM_ComboBox1, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox3, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox4, 0);
            this.Controls.SetChildIndex(this.ckM_ComboBox2, 0);
            this.Controls.SetChildIndex(this.ckM_Label7, 0);
            this.Controls.SetChildIndex(this.ckM_Label6, 0);
            this.Controls.SetChildIndex(this.ckM_Label9, 0);
            this.Controls.SetChildIndex(this.lblMailKBN, 0);
            this.Controls.SetChildIndex(this.ckM_Label12, 0);
            this.Controls.SetChildIndex(this.ckM_Label11, 0);
            this.Controls.SetChildIndex(this.ckM_Label10, 0);
            this.Controls.SetChildIndex(this.ckM_Label14, 0);
            this.Controls.SetChildIndex(this.lblAddress1, 0);
            this.Controls.SetChildIndex(this.ckM_Label13, 0);
            this.Controls.SetChildIndex(this.lblJuchuuNo, 0);
            this.Controls.SetChildIndex(this.lblAddress2, 0);
            this.Controls.SetChildIndex(this.lblAddress3, 0);
            this.Controls.SetChildIndex(this.lblMailSubject, 0);
            this.Controls.SetChildIndex(this.lblMailContent, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
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
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_GridView GvDetail;
        private CKM_Controls.CKM_Button BtnStart;
        private CKM_Controls.CKM_Button BtnStop;
        protected System.Windows.Forms.Label lblEdiMode;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblSendedDateTime;
        private CKM_Controls.CKM_Button BtnSubF12;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Label lblTitle;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_TextBox ckM_TextBox2;
        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private Search.CKM_SearchControl ScCustomer;
        private CKM_Controls.CKM_ComboBox ckM_ComboBox1;
        private CKM_Controls.CKM_Label ckM_Label8;
        private CKM_Controls.CKM_TextBox ckM_TextBox3;
        private CKM_Controls.CKM_TextBox ckM_TextBox4;
        private CKM_Controls.CKM_ComboBox ckM_ComboBox2;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_Label ckM_Label7;
        private CKM_Controls.CKM_Label ckM_Label9;
        private System.Windows.Forms.Label lblMailKBN;
        private CKM_Controls.CKM_Label ckM_Label10;
        private CKM_Controls.CKM_Label ckM_Label11;
        private CKM_Controls.CKM_Label ckM_Label12;
        private CKM_Controls.CKM_Label ckM_Label14;
        private System.Windows.Forms.Label lblAddress1;
        private System.Windows.Forms.Label lblJuchuuNo;
        private CKM_Controls.CKM_Label ckM_Label13;
        private System.Windows.Forms.Label lblAddress2;
        private System.Windows.Forms.Label lblAddress3;
        private System.Windows.Forms.Label lblMailSubject;
        private System.Windows.Forms.Label lblMailContent;
        private System.Windows.Forms.DataGridViewTextBoxColumn coIMailDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMailSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMailCounter;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMailKBN;
        private System.Windows.Forms.DataGridViewTextBoxColumn Customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn MailContent;
    }
}

