namespace Search
{
    partial class FrmSearch_GinkouShiten
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
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.lblChangeDate = new System.Windows.Forms.Label();
            this.RdoRireki = new CKM_Controls.CKM_RadioButton();
            this.RdoKijunBi = new CKM_Controls.CKM_RadioButton();
            this.txtBranchName = new CKM_Controls.CKM_TextBox();
            this.ckM_Label7 = new CKM_Controls.CKM_Label();
            this.txtBranchCDTo = new CKM_Controls.CKM_TextBox();
            this.txtBranchCDFrom = new CKM_Controls.CKM_TextBox();
            this.LblBankCD = new System.Windows.Forms.Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.txtKanaName = new CKM_Controls.CKM_TextBox();
            this.BtnDisplay = new CKM_Controls.CKM_Button();
            this.GvShiten = new CKM_Controls.CKM_GridView();
            this.colBranchCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBranchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChangeDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LblBankName = new System.Windows.Forms.Label();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvShiten)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.LblBankName);
            this.PanelHeader.Controls.Add(this.BtnDisplay);
            this.PanelHeader.Controls.Add(this.txtKanaName);
            this.PanelHeader.Controls.Add(this.ckM_Label2);
            this.PanelHeader.Controls.Add(this.LblBankCD);
            this.PanelHeader.Controls.Add(this.ckM_Label7);
            this.PanelHeader.Controls.Add(this.txtBranchCDTo);
            this.PanelHeader.Controls.Add(this.txtBranchCDFrom);
            this.PanelHeader.Controls.Add(this.txtBranchName);
            this.PanelHeader.Controls.Add(this.RdoRireki);
            this.PanelHeader.Controls.Add(this.RdoKijunBi);
            this.PanelHeader.Controls.Add(this.lblChangeDate);
            this.PanelHeader.Controls.Add(this.ckM_Label6);
            this.PanelHeader.Controls.Add(this.ckM_Label5);
            this.PanelHeader.Controls.Add(this.ckM_Label4);
            this.PanelHeader.Controls.Add(this.ckM_Label3);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Size = new System.Drawing.Size(656, 168);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label3, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label4, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label5, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label6, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblChangeDate, 0);
            this.PanelHeader.Controls.SetChildIndex(this.RdoKijunBi, 0);
            this.PanelHeader.Controls.SetChildIndex(this.RdoRireki, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtBranchName, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtBranchCDFrom, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtBranchCDTo, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label7, 0);
            this.PanelHeader.Controls.SetChildIndex(this.LblBankCD, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtKanaName, 0);
            this.PanelHeader.Controls.SetChildIndex(this.BtnDisplay, 0);
            this.PanelHeader.Controls.SetChildIndex(this.LblBankName, 0);
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
            this.ckM_Label1.Location = new System.Drawing.Point(61, 13);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label1.TabIndex = 0;
            this.ckM_Label1.Text = "基準日";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label3.Location = new System.Drawing.Point(47, 39);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label3.TabIndex = 2;
            this.ckM_Label3.Text = "表示対象";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label4.Location = new System.Drawing.Point(73, 60);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label4.TabIndex = 5;
            this.ckM_Label4.Text = "銀行";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label5.Location = new System.Drawing.Point(59, 89);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(45, 12);
            this.ckM_Label5.TabIndex = 6;
            this.ckM_Label5.Text = "支店CD";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label6.Location = new System.Drawing.Point(34, 116);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label6.TabIndex = 10;
            this.ckM_Label6.Text = "銀行支店名";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblChangeDate
            // 
            this.lblChangeDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblChangeDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblChangeDate.Location = new System.Drawing.Point(107, 9);
            this.lblChangeDate.Name = "lblChangeDate";
            this.lblChangeDate.Size = new System.Drawing.Size(84, 20);
            this.lblChangeDate.TabIndex = 1;
            this.lblChangeDate.Text = "YYYY/MM/DD";
            this.lblChangeDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RdoRireki
            // 
            this.RdoRireki.AutoSize = true;
            this.RdoRireki.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.RdoRireki.Location = new System.Drawing.Point(182, 37);
            this.RdoRireki.Name = "RdoRireki";
            this.RdoRireki.Size = new System.Drawing.Size(49, 16);
            this.RdoRireki.TabIndex = 4;
            this.RdoRireki.Text = "履歴";
            this.RdoRireki.UseVisualStyleBackColor = true;
            // 
            // RdoKijunBi
            // 
            this.RdoKijunBi.AutoSize = true;
            this.RdoKijunBi.Checked = true;
            this.RdoKijunBi.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.RdoKijunBi.Location = new System.Drawing.Point(107, 37);
            this.RdoKijunBi.Name = "RdoKijunBi";
            this.RdoKijunBi.Size = new System.Drawing.Size(62, 16);
            this.RdoKijunBi.TabIndex = 3;
            this.RdoKijunBi.TabStop = true;
            this.RdoKijunBi.Text = "基準日";
            this.RdoKijunBi.UseVisualStyleBackColor = true;
            // 
            // txtBranchName
            // 
            this.txtBranchName.AllowMinus = false;
            this.txtBranchName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtBranchName.BackColor = System.Drawing.Color.White;
            this.txtBranchName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBranchName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtBranchName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtBranchName.DecimalPlace = 0;
            this.txtBranchName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtBranchName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtBranchName.IntegerPart = 0;
            this.txtBranchName.IsCorrectDate = true;
            this.txtBranchName.isEnterKeyDown = false;
            this.txtBranchName.IsNumber = true;
            this.txtBranchName.IsShop = false;
            this.txtBranchName.Length = 30;
            this.txtBranchName.Location = new System.Drawing.Point(107, 112);
            this.txtBranchName.MaxLength = 15;
            this.txtBranchName.MoveNext = true;
            this.txtBranchName.Name = "txtBranchName";
            this.txtBranchName.Size = new System.Drawing.Size(195, 19);
            this.txtBranchName.TabIndex = 11;
            this.txtBranchName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_Label7
            // 
            this.ckM_Label7.AutoSize = true;
            this.ckM_Label7.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label7.DefaultlabelSize = true;
            this.ckM_Label7.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label7.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label7.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label7.Location = new System.Drawing.Point(149, 87);
            this.ckM_Label7.Name = "ckM_Label7";
            this.ckM_Label7.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label7.TabIndex = 9;
            this.ckM_Label7.Text = "～";
            this.ckM_Label7.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBranchCDTo
            // 
            this.txtBranchCDTo.AllowMinus = false;
            this.txtBranchCDTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtBranchCDTo.BackColor = System.Drawing.Color.White;
            this.txtBranchCDTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBranchCDTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtBranchCDTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtBranchCDTo.DecimalPlace = 0;
            this.txtBranchCDTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtBranchCDTo.IntegerPart = 0;
            this.txtBranchCDTo.IsCorrectDate = true;
            this.txtBranchCDTo.isEnterKeyDown = false;
            this.txtBranchCDTo.IsNumber = true;
            this.txtBranchCDTo.IsShop = false;
            this.txtBranchCDTo.Length = 4;
            this.txtBranchCDTo.Location = new System.Drawing.Point(177, 85);
            this.txtBranchCDTo.MaxLength = 4;
            this.txtBranchCDTo.MoveNext = true;
            this.txtBranchCDTo.Name = "txtBranchCDTo";
            this.txtBranchCDTo.Size = new System.Drawing.Size(40, 19);
            this.txtBranchCDTo.TabIndex = 8;
            this.txtBranchCDTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtBranchCDTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBranchCDTo_KeyDown);
            // 
            // txtBranchCDFrom
            // 
            this.txtBranchCDFrom.AllowMinus = false;
            this.txtBranchCDFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtBranchCDFrom.BackColor = System.Drawing.Color.White;
            this.txtBranchCDFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBranchCDFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtBranchCDFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtBranchCDFrom.DecimalPlace = 0;
            this.txtBranchCDFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtBranchCDFrom.IntegerPart = 0;
            this.txtBranchCDFrom.IsCorrectDate = true;
            this.txtBranchCDFrom.isEnterKeyDown = false;
            this.txtBranchCDFrom.IsNumber = true;
            this.txtBranchCDFrom.IsShop = false;
            this.txtBranchCDFrom.Length = 4;
            this.txtBranchCDFrom.Location = new System.Drawing.Point(107, 85);
            this.txtBranchCDFrom.MaxLength = 4;
            this.txtBranchCDFrom.MoveNext = true;
            this.txtBranchCDFrom.Name = "txtBranchCDFrom";
            this.txtBranchCDFrom.Size = new System.Drawing.Size(40, 19);
            this.txtBranchCDFrom.TabIndex = 7;
            this.txtBranchCDFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // LblBankCD
            // 
            this.LblBankCD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.LblBankCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LblBankCD.Location = new System.Drawing.Point(107, 56);
            this.LblBankCD.Name = "LblBankCD";
            this.LblBankCD.Size = new System.Drawing.Size(40, 20);
            this.LblBankCD.TabIndex = 56;
            this.LblBankCD.Text = "xxx";
            this.LblBankCD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ckM_Label2
            // 
            this.ckM_Label2.AutoSize = true;
            this.ckM_Label2.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label2.DefaultlabelSize = true;
            this.ckM_Label2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label2.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label2.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label2.Location = new System.Drawing.Point(60, 142);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label2.TabIndex = 12;
            this.ckM_Label2.Text = "カナ名";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtKanaName
            // 
            this.txtKanaName.AllowMinus = false;
            this.txtKanaName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtKanaName.BackColor = System.Drawing.Color.White;
            this.txtKanaName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKanaName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtKanaName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtKanaName.DecimalPlace = 0;
            this.txtKanaName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtKanaName.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.txtKanaName.IntegerPart = 0;
            this.txtKanaName.IsCorrectDate = true;
            this.txtKanaName.isEnterKeyDown = false;
            this.txtKanaName.IsNumber = true;
            this.txtKanaName.IsShop = false;
            this.txtKanaName.Length = 30;
            this.txtKanaName.Location = new System.Drawing.Point(107, 138);
            this.txtKanaName.MaxLength = 30;
            this.txtKanaName.MoveNext = true;
            this.txtKanaName.Name = "txtKanaName";
            this.txtKanaName.Size = new System.Drawing.Size(300, 19);
            this.txtKanaName.TabIndex = 13;
            this.txtKanaName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // BtnDisplay
            // 
            this.BtnDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BtnDisplay.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BtnDisplay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnDisplay.DefaultBtnSize = false;
            this.BtnDisplay.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnDisplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDisplay.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.BtnDisplay.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BtnDisplay.Location = new System.Drawing.Point(539, 133);
            this.BtnDisplay.Margin = new System.Windows.Forms.Padding(1);
            this.BtnDisplay.Name = "BtnDisplay";
            this.BtnDisplay.Size = new System.Drawing.Size(107, 28);
            this.BtnDisplay.TabIndex = 14;
            this.BtnDisplay.Text = "表示(F11)";
            this.BtnDisplay.UseVisualStyleBackColor = false;
            this.BtnDisplay.Click += new System.EventHandler(this.BtnDisplay_Click);
            this.BtnDisplay.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSearch_GinkouShiten_KeyDown);
            // 
            // GvShiten
            // 
            this.GvShiten.AllowUserToAddRows = false;
            this.GvShiten.AllowUserToDeleteRows = false;
            this.GvShiten.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.GvShiten.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.GvShiten.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GvShiten.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.GvShiten.ColumnHeadersHeight = 25;
            this.GvShiten.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBranchCD,
            this.colBranchName,
            this.colChangeDate});
            this.GvShiten.EnableHeadersVisualStyles = false;
            this.GvShiten.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GvShiten.Location = new System.Drawing.Point(50, 226);
            this.GvShiten.Name = "GvShiten";
            this.GvShiten.Size = new System.Drawing.Size(535, 369);
            this.GvShiten.TabIndex = 6;
            this.GvShiten.UseRowNo = true;
            this.GvShiten.UseSetting = true;
            this.GvShiten.DoubleClick += new System.EventHandler(this.GvShiten_DoubleClick);
            // 
            // colBranchCD
            // 
            this.colBranchCD.DataPropertyName = "BranchCD";
            this.colBranchCD.HeaderText = "支店CD";
            this.colBranchCD.MinimumWidth = 4;
            this.colBranchCD.Name = "colBranchCD";
            this.colBranchCD.Width = 70;
            // 
            // colBranchName
            // 
            this.colBranchName.DataPropertyName = "BranchName";
            this.colBranchName.HeaderText = "銀行支店名";
            this.colBranchName.MinimumWidth = 30;
            this.colBranchName.Name = "colBranchName";
            this.colBranchName.Width = 300;
            // 
            // colChangeDate
            // 
            this.colChangeDate.DataPropertyName = "ChangeDate";
            this.colChangeDate.HeaderText = "改定日";
            this.colChangeDate.MinimumWidth = 8;
            this.colChangeDate.Name = "colChangeDate";
            // 
            // LblBankName
            // 
            this.LblBankName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.LblBankName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LblBankName.Location = new System.Drawing.Point(146, 56);
            this.LblBankName.Name = "LblBankName";
            this.LblBankName.Size = new System.Drawing.Size(236, 20);
            this.LblBankName.TabIndex = 60;
            this.LblBankName.Text = "ＸＸＸＸＸＸＸＸＸ10ＸＸＸＸ15";
            this.LblBankName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrmSearch_GinkouShiten
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 661);
            this.Controls.Add(this.GvShiten);
            this.F11Visible = true;
            this.F9Visible = true;
            this.Name = "FrmSearch_GinkouShiten";
            this.PanelHeaderHeight = 210;
            this.ProgramName = "銀行支店検索";
            this.Text = "Search_GinkouShiten";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSearch_GinkouShiten_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmSearch_GinkouShiten_KeyUp);
            this.Controls.SetChildIndex(this.GvShiten, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvShiten)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Label ckM_Label1;
        private System.Windows.Forms.Label lblChangeDate;
        private CKM_Controls.CKM_TextBox txtBranchName;
        private CKM_Controls.CKM_RadioButton RdoRireki;
        private CKM_Controls.CKM_RadioButton RdoKijunBi;
        private System.Windows.Forms.Label LblBankCD;
        private CKM_Controls.CKM_Label ckM_Label7;
        private CKM_Controls.CKM_TextBox txtBranchCDTo;
        private CKM_Controls.CKM_TextBox txtBranchCDFrom;
        private CKM_Controls.CKM_TextBox txtKanaName;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Button BtnDisplay;
        private CKM_Controls.CKM_GridView GvShiten;
        private System.Windows.Forms.Label LblBankName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBranchCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBranchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChangeDate;
    }
}