namespace Search
{
    partial class FrmSearch_SiharaiNO
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSearch_SiharaiNO));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.txtPaymentDateFrom = new CKM_Controls.CKM_TextBox();
            this.txtPaymentDateTo = new CKM_Controls.CKM_TextBox();
            this.txtPaymentInputDateFrom = new CKM_Controls.CKM_TextBox();
            this.txtPaymentInputDateTo = new CKM_Controls.CKM_TextBox();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.btnShow = new CKM_Controls.CKM_Button();
            this.dgvSiharaiNO = new CKM_Controls.CKM_GridView();
            this.colTransactionNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaymentNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaymentDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaymentEntryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPayeeCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPayeeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmountOfPayment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTransferAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOtherThanTransfer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sc_PaymentDestination = new Search.CKM_SearchControl();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSiharaiNO)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.btnShow);
            this.PanelHeader.Controls.Add(this.Sc_PaymentDestination);
            this.PanelHeader.Controls.Add(this.ckM_Label5);
            this.PanelHeader.Controls.Add(this.ckM_Label4);
            this.PanelHeader.Controls.Add(this.ckM_Label3);
            this.PanelHeader.Controls.Add(this.ckM_Label2);
            this.PanelHeader.Controls.Add(this.txtPaymentInputDateTo);
            this.PanelHeader.Controls.Add(this.txtPaymentInputDateFrom);
            this.PanelHeader.Controls.Add(this.txtPaymentDateTo);
            this.PanelHeader.Controls.Add(this.txtPaymentDateFrom);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Size = new System.Drawing.Size(1004, 138);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtPaymentDateFrom, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtPaymentDateTo, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtPaymentInputDateFrom, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtPaymentInputDateTo, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label3, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label4, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label5, 0);
            this.PanelHeader.Controls.SetChildIndex(this.Sc_PaymentDestination, 0);
            this.PanelHeader.Controls.SetChildIndex(this.btnShow, 0);
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
            this.ckM_Label1.Location = new System.Drawing.Point(55, 22);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label1.TabIndex = 6;
            this.ckM_Label1.Text = "支払日";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPaymentDateFrom
            // 
            this.txtPaymentDateFrom.AllowMinus = false;
            this.txtPaymentDateFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPaymentDateFrom.BackColor = System.Drawing.Color.White;
            this.txtPaymentDateFrom.BorderColor = false;
            this.txtPaymentDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPaymentDateFrom.ClientColor = System.Drawing.Color.White;
            this.txtPaymentDateFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtPaymentDateFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtPaymentDateFrom.DecimalPlace = 0;
            this.txtPaymentDateFrom.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.txtPaymentDateFrom.IntegerPart = 0;
            this.txtPaymentDateFrom.IsCorrectDate = true;
            this.txtPaymentDateFrom.isEnterKeyDown = false;
            this.txtPaymentDateFrom.IsFirstTime = true;
            this.txtPaymentDateFrom.isMaxLengthErr = false;
            this.txtPaymentDateFrom.IsNumber = true;
            this.txtPaymentDateFrom.IsShop = false;
            this.txtPaymentDateFrom.Length = 8;
            this.txtPaymentDateFrom.Location = new System.Drawing.Point(105, 19);
            this.txtPaymentDateFrom.MaxLength = 8;
            this.txtPaymentDateFrom.MoveNext = true;
            this.txtPaymentDateFrom.Name = "txtPaymentDateFrom";
            this.txtPaymentDateFrom.Size = new System.Drawing.Size(100, 19);
            this.txtPaymentDateFrom.TabIndex = 0;
            this.txtPaymentDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPaymentDateFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtPaymentDateFrom.UseColorSizMode = false;
            // 
            // txtPaymentDateTo
            // 
            this.txtPaymentDateTo.AllowMinus = false;
            this.txtPaymentDateTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPaymentDateTo.BackColor = System.Drawing.Color.White;
            this.txtPaymentDateTo.BorderColor = false;
            this.txtPaymentDateTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPaymentDateTo.ClientColor = System.Drawing.Color.White;
            this.txtPaymentDateTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtPaymentDateTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtPaymentDateTo.DecimalPlace = 0;
            this.txtPaymentDateTo.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.txtPaymentDateTo.IntegerPart = 0;
            this.txtPaymentDateTo.IsCorrectDate = true;
            this.txtPaymentDateTo.isEnterKeyDown = false;
            this.txtPaymentDateTo.IsFirstTime = true;
            this.txtPaymentDateTo.isMaxLengthErr = false;
            this.txtPaymentDateTo.IsNumber = true;
            this.txtPaymentDateTo.IsShop = false;
            this.txtPaymentDateTo.Length = 8;
            this.txtPaymentDateTo.Location = new System.Drawing.Point(261, 19);
            this.txtPaymentDateTo.MaxLength = 8;
            this.txtPaymentDateTo.MoveNext = true;
            this.txtPaymentDateTo.Name = "txtPaymentDateTo";
            this.txtPaymentDateTo.Size = new System.Drawing.Size(100, 19);
            this.txtPaymentDateTo.TabIndex = 1;
            this.txtPaymentDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPaymentDateTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtPaymentDateTo.UseColorSizMode = false;
            this.txtPaymentDateTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPaymentDateTo_KeyDown);
            // 
            // txtPaymentInputDateFrom
            // 
            this.txtPaymentInputDateFrom.AllowMinus = false;
            this.txtPaymentInputDateFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPaymentInputDateFrom.BackColor = System.Drawing.Color.White;
            this.txtPaymentInputDateFrom.BorderColor = false;
            this.txtPaymentInputDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPaymentInputDateFrom.ClientColor = System.Drawing.Color.White;
            this.txtPaymentInputDateFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtPaymentInputDateFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtPaymentInputDateFrom.DecimalPlace = 0;
            this.txtPaymentInputDateFrom.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.txtPaymentInputDateFrom.IntegerPart = 0;
            this.txtPaymentInputDateFrom.IsCorrectDate = true;
            this.txtPaymentInputDateFrom.isEnterKeyDown = false;
            this.txtPaymentInputDateFrom.IsFirstTime = true;
            this.txtPaymentInputDateFrom.isMaxLengthErr = false;
            this.txtPaymentInputDateFrom.IsNumber = true;
            this.txtPaymentInputDateFrom.IsShop = false;
            this.txtPaymentInputDateFrom.Length = 8;
            this.txtPaymentInputDateFrom.Location = new System.Drawing.Point(105, 49);
            this.txtPaymentInputDateFrom.MaxLength = 8;
            this.txtPaymentInputDateFrom.MoveNext = true;
            this.txtPaymentInputDateFrom.Name = "txtPaymentInputDateFrom";
            this.txtPaymentInputDateFrom.Size = new System.Drawing.Size(100, 19);
            this.txtPaymentInputDateFrom.TabIndex = 2;
            this.txtPaymentInputDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPaymentInputDateFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtPaymentInputDateFrom.UseColorSizMode = false;
            // 
            // txtPaymentInputDateTo
            // 
            this.txtPaymentInputDateTo.AllowMinus = false;
            this.txtPaymentInputDateTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPaymentInputDateTo.BackColor = System.Drawing.Color.White;
            this.txtPaymentInputDateTo.BorderColor = false;
            this.txtPaymentInputDateTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPaymentInputDateTo.ClientColor = System.Drawing.Color.White;
            this.txtPaymentInputDateTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtPaymentInputDateTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtPaymentInputDateTo.DecimalPlace = 0;
            this.txtPaymentInputDateTo.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.txtPaymentInputDateTo.IntegerPart = 0;
            this.txtPaymentInputDateTo.IsCorrectDate = true;
            this.txtPaymentInputDateTo.isEnterKeyDown = false;
            this.txtPaymentInputDateTo.IsFirstTime = true;
            this.txtPaymentInputDateTo.isMaxLengthErr = false;
            this.txtPaymentInputDateTo.IsNumber = true;
            this.txtPaymentInputDateTo.IsShop = false;
            this.txtPaymentInputDateTo.Length = 8;
            this.txtPaymentInputDateTo.Location = new System.Drawing.Point(261, 49);
            this.txtPaymentInputDateTo.MaxLength = 8;
            this.txtPaymentInputDateTo.MoveNext = true;
            this.txtPaymentInputDateTo.Name = "txtPaymentInputDateTo";
            this.txtPaymentInputDateTo.Size = new System.Drawing.Size(100, 19);
            this.txtPaymentInputDateTo.TabIndex = 3;
            this.txtPaymentInputDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPaymentInputDateTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtPaymentInputDateTo.UseColorSizMode = false;
            this.txtPaymentInputDateTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPaymentInputDateTo_KeyDown);
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
            this.ckM_Label2.Location = new System.Drawing.Point(224, 22);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label2.TabIndex = 7;
            this.ckM_Label2.Text = "～";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label3
            // 
            this.ckM_Label3.AutoSize = true;
            this.ckM_Label3.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label3.DefaultlabelSize = true;
            this.ckM_Label3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label3.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label3.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label3.Location = new System.Drawing.Point(224, 52);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label3.TabIndex = 9;
            this.ckM_Label3.Text = "～";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label4.Location = new System.Drawing.Point(29, 52);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label4.TabIndex = 8;
            this.ckM_Label4.Text = "支払入力日";
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
            this.ckM_Label5.Location = new System.Drawing.Point(55, 84);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label5.TabIndex = 10;
            this.ckM_Label5.Text = "支払先";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnShow
            // 
            this.btnShow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnShow.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnShow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShow.DefaultBtnSize = true;
            this.btnShow.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShow.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.btnShow.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnShow.Location = new System.Drawing.Point(866, 100);
            this.btnShow.Margin = new System.Windows.Forms.Padding(1);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(118, 28);
            this.btnShow.TabIndex = 5;
            this.btnShow.Text = "表示(F11)";
            this.btnShow.UseVisualStyleBackColor = false;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // dgvSiharaiNO
            // 
            this.dgvSiharaiNO.AllowUserToAddRows = false;
            this.dgvSiharaiNO.AllowUserToDeleteRows = false;
            this.dgvSiharaiNO.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.dgvSiharaiNO.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSiharaiNO.AutoGenerateColumns = false;
            this.dgvSiharaiNO.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvSiharaiNO.CheckCol = ((System.Collections.ArrayList)(resources.GetObject("dgvSiharaiNO.CheckCol")));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSiharaiNO.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSiharaiNO.ColumnHeadersHeight = 25;
            this.dgvSiharaiNO.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTransactionNo,
            this.colPaymentNO,
            this.colPaymentDate,
            this.colPaymentEntryDate,
            this.colPayeeCD,
            this.colPayeeName,
            this.colAmountOfPayment,
            this.colTransferAmount,
            this.colOtherThanTransfer});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSiharaiNO.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSiharaiNO.EnableHeadersVisualStyles = false;
            this.dgvSiharaiNO.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvSiharaiNO.Location = new System.Drawing.Point(31, 197);
            this.dgvSiharaiNO.Name = "dgvSiharaiNO";
            this.dgvSiharaiNO.ReadOnly = true;
            this.dgvSiharaiNO.RowHeight_ = 20;
            this.dgvSiharaiNO.RowTemplate.Height = 20;
            this.dgvSiharaiNO.Size = new System.Drawing.Size(953, 400);
            this.dgvSiharaiNO.TabIndex = 20;
            this.dgvSiharaiNO.UseRowNo = true;
            this.dgvSiharaiNO.UseSetting = true;
            this.dgvSiharaiNO.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSiharaiNO_CellDoubleClick);
            this.dgvSiharaiNO.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvSiharaiNO_Paint);
            // 
            // colTransactionNo
            // 
            this.colTransactionNo.DataPropertyName = "LargePayNO";
            this.colTransactionNo.HeaderText = "支払処理番号";
            this.colTransactionNo.MaxInputLength = 11;
            this.colTransactionNo.Name = "colTransactionNo";
            this.colTransactionNo.ReadOnly = true;
            this.colTransactionNo.Width = 110;
            // 
            // colPaymentNO
            // 
            this.colPaymentNO.DataPropertyName = "PayNO";
            this.colPaymentNO.HeaderText = "支払番号";
            this.colPaymentNO.MaxInputLength = 11;
            this.colPaymentNO.Name = "colPaymentNO";
            this.colPaymentNO.ReadOnly = true;
            this.colPaymentNO.Width = 110;
            // 
            // colPaymentDate
            // 
            this.colPaymentDate.DataPropertyName = "PayDate";
            this.colPaymentDate.HeaderText = "支払日";
            this.colPaymentDate.MaxInputLength = 8;
            this.colPaymentDate.Name = "colPaymentDate";
            this.colPaymentDate.ReadOnly = true;
            this.colPaymentDate.Width = 80;
            // 
            // colPaymentEntryDate
            // 
            this.colPaymentEntryDate.DataPropertyName = "InputDateTime";
            this.colPaymentEntryDate.HeaderText = "支払入力日";
            this.colPaymentEntryDate.MaxInputLength = 8;
            this.colPaymentEntryDate.Name = "colPaymentEntryDate";
            this.colPaymentEntryDate.ReadOnly = true;
            // 
            // colPayeeCD
            // 
            this.colPayeeCD.DataPropertyName = "PayeeCD";
            this.colPayeeCD.HeaderText = "支払先CD";
            this.colPayeeCD.MaxInputLength = 13;
            this.colPayeeCD.Name = "colPayeeCD";
            this.colPayeeCD.ReadOnly = true;
            this.colPayeeCD.Width = 130;
            // 
            // colPayeeName
            // 
            this.colPayeeName.DataPropertyName = "VendorName";
            this.colPayeeName.HeaderText = "支払先名";
            this.colPayeeName.MaxInputLength = 80;
            this.colPayeeName.Name = "colPayeeName";
            this.colPayeeName.ReadOnly = true;
            this.colPayeeName.Width = 400;
            // 
            // colAmountOfPayment
            // 
            this.colAmountOfPayment.DataPropertyName = "PayGaku";
            this.colAmountOfPayment.HeaderText = "支払額";
            this.colAmountOfPayment.MaxInputLength = 8;
            this.colAmountOfPayment.Name = "colAmountOfPayment";
            this.colAmountOfPayment.ReadOnly = true;
            this.colAmountOfPayment.Width = 80;
            // 
            // colTransferAmount
            // 
            this.colTransferAmount.DataPropertyName = "TransferGaku";
            this.colTransferAmount.HeaderText = "振込額";
            this.colTransferAmount.MaxInputLength = 8;
            this.colTransferAmount.Name = "colTransferAmount";
            this.colTransferAmount.ReadOnly = true;
            this.colTransferAmount.Width = 80;
            // 
            // colOtherThanTransfer
            // 
            this.colOtherThanTransfer.DataPropertyName = "total";
            this.colOtherThanTransfer.HeaderText = "振込以外";
            this.colOtherThanTransfer.MaxInputLength = 8;
            this.colOtherThanTransfer.Name = "colOtherThanTransfer";
            this.colOtherThanTransfer.ReadOnly = true;
            this.colOtherThanTransfer.Width = 80;
            // 
            // Sc_PaymentDestination
            // 
            this.Sc_PaymentDestination.AutoSize = true;
            this.Sc_PaymentDestination.ChangeDate = "";
            this.Sc_PaymentDestination.ChangeDateWidth = 100;
            this.Sc_PaymentDestination.Code = "";
            this.Sc_PaymentDestination.CodeWidth = 100;
            this.Sc_PaymentDestination.CodeWidth1 = 100;
            this.Sc_PaymentDestination.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.Sc_PaymentDestination.DataCheck = false;
            this.Sc_PaymentDestination.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.Sc_PaymentDestination.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.Sc_PaymentDestination.IsCopy = false;
            this.Sc_PaymentDestination.LabelText = "";
            this.Sc_PaymentDestination.LabelVisible = true;
            this.Sc_PaymentDestination.Location = new System.Drawing.Point(105, 76);
            this.Sc_PaymentDestination.Margin = new System.Windows.Forms.Padding(0);
            this.Sc_PaymentDestination.Name = "Sc_PaymentDestination";
            this.Sc_PaymentDestination.NameWidth = 310;
            this.Sc_PaymentDestination.SearchEnable = true;
            this.Sc_PaymentDestination.Size = new System.Drawing.Size(444, 28);
            this.Sc_PaymentDestination.Stype = Search.CKM_SearchControl.SearchType.仕入先;
            this.Sc_PaymentDestination.TabIndex = 4;
            this.Sc_PaymentDestination.test = null;
            this.Sc_PaymentDestination.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.Sc_PaymentDestination.UseChangeDate = false;
            this.Sc_PaymentDestination.Value1 = null;
            this.Sc_PaymentDestination.Value2 = null;
            this.Sc_PaymentDestination.Value3 = null;
            this.Sc_PaymentDestination.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.Sc_PaymentDestination_CodeKeyDownEvent);
            // 
            // FrmSearch_SiharaiNO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 649);
            this.Controls.Add(this.dgvSiharaiNO);
            this.F11Visible = true;
            this.F12Visible = true;
            this.F9Visible = true;
            this.Name = "FrmSearch_SiharaiNO";
            this.PanelHeaderHeight = 180;
            this.ProgramName = "支払番号検索";
            this.Text = "Search_SiharaiNO";
            this.Load += new System.EventHandler(this.FrmSearch_SiharaiNO_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmSearch_SiharaiNO_KeyUp);
            this.Controls.SetChildIndex(this.dgvSiharaiNO, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSiharaiNO)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private CKM_SearchControl Sc_PaymentDestination;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_TextBox txtPaymentInputDateTo;
        private CKM_Controls.CKM_TextBox txtPaymentInputDateFrom;
        private CKM_Controls.CKM_TextBox txtPaymentDateTo;
        private CKM_Controls.CKM_TextBox txtPaymentDateFrom;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_Button btnShow;
        private CKM_Controls.CKM_GridView dgvSiharaiNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTransactionNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaymentNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaymentDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaymentEntryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPayeeCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPayeeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmountOfPayment;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTransferAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOtherThanTransfer;
    }
}