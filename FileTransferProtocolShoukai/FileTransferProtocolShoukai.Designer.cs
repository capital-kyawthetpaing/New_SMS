namespace FileTransferProtocolShoukai
{
    partial class FileTransferProtocolShoukai
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.lblFTPMode1 = new System.Windows.Forms.Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.btnStartType1 = new CKM_Controls.CKM_Button();
            this.btnStopType1 = new CKM_Controls.CKM_Button();
            this.lblRireki1 = new CKM_Controls.CKM_Label();
            this.gdvFTPType1 = new CKM_Controls.CKM_GridView();
            this.colFTPDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVendor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFTPFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gdvFTPType2 = new CKM_Controls.CKM_GridView();
            this.colFTPDateTime2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVendor2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFTPFile2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblRireki2 = new CKM_Controls.CKM_Label();
            this.btnStartType2 = new CKM_Controls.CKM_Button();
            this.btnStopType2 = new CKM_Controls.CKM_Button();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.lblFTPMode2 = new System.Windows.Forms.Label();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.BtnSubF12 = new CKM_Controls.CKM_Button();
            ((System.ComponentModel.ISupportInitialize)(this.gdvFTPType1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvFTPType2)).BeginInit();
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
            this.ckM_Label1.Location = new System.Drawing.Point(34, 67);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(143, 12);
            this.ckM_Label1.TabIndex = 15;
            this.ckM_Label1.Text = "【FTP処理結果・発信】";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFTPMode1
            // 
            this.lblFTPMode1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(240)))));
            this.lblFTPMode1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFTPMode1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblFTPMode1.Font = new System.Drawing.Font("MS Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFTPMode1.Location = new System.Drawing.Point(58, 90);
            this.lblFTPMode1.Name = "lblFTPMode1";
            this.lblFTPMode1.Size = new System.Drawing.Size(200, 30);
            this.lblFTPMode1.TabIndex = 26;
            this.lblFTPMode1.Text = "処理実行中";
            this.lblFTPMode1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.ckM_Label3.Location = new System.Drawing.Point(34, 137);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label3.TabIndex = 27;
            this.ckM_Label3.Text = "【発信履歴】";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnStartType1
            // 
            this.btnStartType1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnStartType1.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnStartType1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartType1.DefaultBtnSize = false;
            this.btnStartType1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnStartType1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartType1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnStartType1.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnStartType1.Location = new System.Drawing.Point(320, 92);
            this.btnStartType1.Margin = new System.Windows.Forms.Padding(1);
            this.btnStartType1.Name = "btnStartType1";
            this.btnStartType1.Size = new System.Drawing.Size(95, 23);
            this.btnStartType1.TabIndex = 28;
            this.btnStartType1.Text = "開始";
            this.btnStartType1.UseVisualStyleBackColor = false;
            this.btnStartType1.Click += new System.EventHandler(this.btnStartType1_Click);
            // 
            // btnStopType1
            // 
            this.btnStopType1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnStopType1.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnStopType1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStopType1.DefaultBtnSize = false;
            this.btnStopType1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnStopType1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopType1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnStopType1.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnStopType1.Location = new System.Drawing.Point(415, 92);
            this.btnStopType1.Margin = new System.Windows.Forms.Padding(1);
            this.btnStopType1.Name = "btnStopType1";
            this.btnStopType1.Size = new System.Drawing.Size(95, 23);
            this.btnStopType1.TabIndex = 29;
            this.btnStopType1.Text = "停止";
            this.btnStopType1.UseVisualStyleBackColor = false;
            this.btnStopType1.Click += new System.EventHandler(this.btnStopType1_Click);
            // 
            // lblRireki1
            // 
            this.lblRireki1.AutoSize = true;
            this.lblRireki1.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblRireki1.BackColor = System.Drawing.Color.Transparent;
            this.lblRireki1.DefaultlabelSize = true;
            this.lblRireki1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.lblRireki1.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.lblRireki1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblRireki1.Location = new System.Drawing.Point(123, 137);
            this.lblRireki1.Name = "lblRireki1";
            this.lblRireki1.Size = new System.Drawing.Size(188, 12);
            this.lblRireki1.TabIndex = 30;
            this.lblRireki1.Text = "14日間の履歴を保持しています";
            this.lblRireki1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Green;
            this.lblRireki1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gdvFTPType1
            // 
            this.gdvFTPType1.AllowUserToAddRows = false;
            this.gdvFTPType1.AllowUserToDeleteRows = false;
            this.gdvFTPType1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.gdvFTPType1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gdvFTPType1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gdvFTPType1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gdvFTPType1.ColumnHeadersHeight = 25;
            this.gdvFTPType1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFTPDateTime,
            this.colVendor,
            this.colFTPFile});
            this.gdvFTPType1.EnableHeadersVisualStyles = false;
            this.gdvFTPType1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.gdvFTPType1.Location = new System.Drawing.Point(58, 154);
            this.gdvFTPType1.Name = "gdvFTPType1";
            this.gdvFTPType1.RowHeight_ = 20;
            this.gdvFTPType1.RowTemplate.Height = 20;
            this.gdvFTPType1.Size = new System.Drawing.Size(800, 300);
            this.gdvFTPType1.TabIndex = 31;
            this.gdvFTPType1.UseRowNo = true;
            this.gdvFTPType1.UseSetting = true;
            // 
            // colFTPDateTime
            // 
            this.colFTPDateTime.DataPropertyName = "FTPDateTime";
            this.colFTPDateTime.HeaderText = "発信日時";
            this.colFTPDateTime.Name = "colFTPDateTime";
            this.colFTPDateTime.ReadOnly = true;
            this.colFTPDateTime.Width = 120;
            // 
            // colVendor
            // 
            this.colVendor.DataPropertyName = "Vendor";
            this.colVendor.HeaderText = "仕入先";
            this.colVendor.Name = "colVendor";
            this.colVendor.ReadOnly = true;
            this.colVendor.Width = 350;
            // 
            // colFTPFile
            // 
            this.colFTPFile.DataPropertyName = "FTPFile";
            this.colFTPFile.HeaderText = "取込ファイル";
            this.colFTPFile.Name = "colFTPFile";
            this.colFTPFile.ReadOnly = true;
            this.colFTPFile.Width = 280;
            // 
            // gdvFTPType2
            // 
            this.gdvFTPType2.AllowUserToAddRows = false;
            this.gdvFTPType2.AllowUserToDeleteRows = false;
            this.gdvFTPType2.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.gdvFTPType2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.gdvFTPType2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gdvFTPType2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gdvFTPType2.ColumnHeadersHeight = 25;
            this.gdvFTPType2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFTPDateTime2,
            this.colVendor2,
            this.colFTPFile2});
            this.gdvFTPType2.EnableHeadersVisualStyles = false;
            this.gdvFTPType2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.gdvFTPType2.Location = new System.Drawing.Point(58, 566);
            this.gdvFTPType2.Name = "gdvFTPType2";
            this.gdvFTPType2.RowHeight_ = 20;
            this.gdvFTPType2.RowTemplate.Height = 20;
            this.gdvFTPType2.Size = new System.Drawing.Size(800, 300);
            this.gdvFTPType2.TabIndex = 38;
            this.gdvFTPType2.UseRowNo = true;
            this.gdvFTPType2.UseSetting = true;
            // 
            // colFTPDateTime2
            // 
            this.colFTPDateTime2.DataPropertyName = "FTPDateTime";
            this.colFTPDateTime2.HeaderText = "受信日時";
            this.colFTPDateTime2.Name = "colFTPDateTime2";
            this.colFTPDateTime2.ReadOnly = true;
            this.colFTPDateTime2.Width = 120;
            // 
            // colVendor2
            // 
            this.colVendor2.DataPropertyName = "Vendor";
            this.colVendor2.HeaderText = "仕入先";
            this.colVendor2.Name = "colVendor2";
            this.colVendor2.ReadOnly = true;
            this.colVendor2.Width = 350;
            // 
            // colFTPFile2
            // 
            this.colFTPFile2.DataPropertyName = "FTPFile";
            this.colFTPFile2.HeaderText = "取込ファイル";
            this.colFTPFile2.Name = "colFTPFile2";
            this.colFTPFile2.ReadOnly = true;
            this.colFTPFile2.Width = 280;
            // 
            // lblRireki2
            // 
            this.lblRireki2.AutoSize = true;
            this.lblRireki2.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblRireki2.BackColor = System.Drawing.Color.Transparent;
            this.lblRireki2.DefaultlabelSize = true;
            this.lblRireki2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.lblRireki2.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.lblRireki2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblRireki2.Location = new System.Drawing.Point(123, 549);
            this.lblRireki2.Name = "lblRireki2";
            this.lblRireki2.Size = new System.Drawing.Size(188, 12);
            this.lblRireki2.TabIndex = 37;
            this.lblRireki2.Text = "14日間の履歴を保持しています";
            this.lblRireki2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Green;
            this.lblRireki2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnStartType2
            // 
            this.btnStartType2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnStartType2.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnStartType2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartType2.DefaultBtnSize = false;
            this.btnStartType2.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnStartType2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartType2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnStartType2.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnStartType2.Location = new System.Drawing.Point(310, 504);
            this.btnStartType2.Margin = new System.Windows.Forms.Padding(1);
            this.btnStartType2.Name = "btnStartType2";
            this.btnStartType2.Size = new System.Drawing.Size(95, 23);
            this.btnStartType2.TabIndex = 35;
            this.btnStartType2.Text = "開始";
            this.btnStartType2.UseVisualStyleBackColor = false;
            this.btnStartType2.Click += new System.EventHandler(this.btnStartType2_Click);
            // 
            // btnStopType2
            // 
            this.btnStopType2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnStopType2.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnStopType2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStopType2.DefaultBtnSize = false;
            this.btnStopType2.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnStopType2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopType2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnStopType2.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnStopType2.Location = new System.Drawing.Point(405, 504);
            this.btnStopType2.Margin = new System.Windows.Forms.Padding(1);
            this.btnStopType2.Name = "btnStopType2";
            this.btnStopType2.Size = new System.Drawing.Size(95, 23);
            this.btnStopType2.TabIndex = 36;
            this.btnStopType2.Text = "停止";
            this.btnStopType2.UseVisualStyleBackColor = false;
            this.btnStopType2.Click += new System.EventHandler(this.btnStopType2_Click);
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
            this.ckM_Label4.Location = new System.Drawing.Point(34, 549);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label4.TabIndex = 34;
            this.ckM_Label4.Text = "【発信履歴】";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFTPMode2
            // 
            this.lblFTPMode2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(240)))));
            this.lblFTPMode2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFTPMode2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblFTPMode2.Font = new System.Drawing.Font("MS Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFTPMode2.Location = new System.Drawing.Point(58, 504);
            this.lblFTPMode2.Name = "lblFTPMode2";
            this.lblFTPMode2.Size = new System.Drawing.Size(200, 30);
            this.lblFTPMode2.TabIndex = 33;
            this.lblFTPMode2.Text = "処理実行中";
            this.lblFTPMode2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.ckM_Label5.Location = new System.Drawing.Point(34, 479);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(143, 12);
            this.ckM_Label5.TabIndex = 32;
            this.ckM_Label5.Text = "【FTP処理結果・発信】";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.BtnSubF12.Location = new System.Drawing.Point(1608, 126);
            this.BtnSubF12.Margin = new System.Windows.Forms.Padding(1);
            this.BtnSubF12.Name = "BtnSubF12";
            this.BtnSubF12.Size = new System.Drawing.Size(95, 23);
            this.BtnSubF12.TabIndex = 39;
            this.BtnSubF12.Text = "最新化(F12)";
            this.BtnSubF12.UseVisualStyleBackColor = false;
            this.BtnSubF12.Click += new System.EventHandler(this.BtnSubF12_Click);
            // 
            // FileTransferProtocolShoukai
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.BtnSubF12);
            this.Controls.Add(this.gdvFTPType2);
            this.Controls.Add(this.lblRireki2);
            this.Controls.Add(this.btnStartType2);
            this.Controls.Add(this.btnStopType2);
            this.Controls.Add(this.ckM_Label4);
            this.Controls.Add(this.lblFTPMode2);
            this.Controls.Add(this.ckM_Label5);
            this.Controls.Add(this.gdvFTPType1);
            this.Controls.Add(this.lblRireki1);
            this.Controls.Add(this.btnStartType1);
            this.Controls.Add(this.btnStopType1);
            this.Controls.Add(this.ckM_Label3);
            this.Controls.Add(this.lblFTPMode1);
            this.Controls.Add(this.ckM_Label1);
            this.F10Visible = false;
            this.F11Visible = false;
            this.F2Visible = false;
            this.F3Visible = false;
            this.F4Visible = false;
            this.F5Visible = false;
            this.F6Visible = false;
            this.F7Visible = false;
            this.F8Visible = false;
            this.F9Visible = false;
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "FileTransferProtocolShoukai";
            this.PanelHeaderHeight = 50;
            this.Text = "FileTransferProtocolShoukai";
            this.Load += new System.EventHandler(this.FileTransferProtocolShoukai_Load);
            this.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.Controls.SetChildIndex(this.lblFTPMode1, 0);
            this.Controls.SetChildIndex(this.ckM_Label3, 0);
            this.Controls.SetChildIndex(this.btnStopType1, 0);
            this.Controls.SetChildIndex(this.btnStartType1, 0);
            this.Controls.SetChildIndex(this.lblRireki1, 0);
            this.Controls.SetChildIndex(this.gdvFTPType1, 0);
            this.Controls.SetChildIndex(this.ckM_Label5, 0);
            this.Controls.SetChildIndex(this.lblFTPMode2, 0);
            this.Controls.SetChildIndex(this.ckM_Label4, 0);
            this.Controls.SetChildIndex(this.btnStopType2, 0);
            this.Controls.SetChildIndex(this.btnStartType2, 0);
            this.Controls.SetChildIndex(this.lblRireki2, 0);
            this.Controls.SetChildIndex(this.gdvFTPType2, 0);
            this.Controls.SetChildIndex(this.BtnSubF12, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gdvFTPType1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdvFTPType2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CKM_Controls.CKM_Label ckM_Label1;
        private System.Windows.Forms.Label lblFTPMode1;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Button btnStartType1;
        private CKM_Controls.CKM_Button btnStopType1;
        private CKM_Controls.CKM_Label lblRireki1;
        private CKM_Controls.CKM_GridView gdvFTPType1;
        private CKM_Controls.CKM_GridView gdvFTPType2;
        private CKM_Controls.CKM_Label lblRireki2;
        private CKM_Controls.CKM_Button btnStartType2;
        private CKM_Controls.CKM_Button btnStopType2;
        private CKM_Controls.CKM_Label ckM_Label4;
        private System.Windows.Forms.Label lblFTPMode2;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_Button BtnSubF12;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFTPDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVendor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFTPFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFTPDateTime2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVendor2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFTPFile2;
    }
}

