namespace Search
{
    partial class frmSearch_KeihiNO
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearch_KeihiNO));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.txtRecordDateFrom = new CKM_Controls.CKM_TextBox();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.txtRecordDateTo = new CKM_Controls.CKM_TextBox();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.txtEntryDateFrom = new CKM_Controls.CKM_TextBox();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.txtEntryDateTo = new CKM_Controls.CKM_TextBox();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.ckM_Label7 = new CKM_Controls.CKM_Label();
            this.ckM_Label8 = new CKM_Controls.CKM_Label();
            this.ckM_Label9 = new CKM_Controls.CKM_Label();
            this.chkPaid = new CKM_Controls.CKM_CheckBox();
            this.chkUnpaid = new CKM_Controls.CKM_CheckBox();
            this.txtPaymentDateTo = new CKM_Controls.CKM_TextBox();
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.txtPaymentDateFrom = new CKM_Controls.CKM_TextBox();
            this.ckM_Label11 = new CKM_Controls.CKM_Label();
            this.btnSubF11 = new CKM_Controls.CKM_Button();
            this.dgvCostSearch = new CKM_Controls.CKM_GridView();
            this.txtPaymentDueDateFrom = new CKM_Controls.CKM_TextBox();
            this.txtPaymentDueDateTo = new CKM_Controls.CKM_TextBox();
            this.chkTeiki = new CKM_Controls.CKM_CheckBox();
            this.PaymentCD = new Search.CKM_SearchControl();
            this.scStaffCD = new Search.CKM_SearchControl();
            this.ExpenseNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecordDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExpenseEntryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Regular = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentDestination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Staff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentDueDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxIncludePayment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCostSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.chkTeiki);
            this.PanelHeader.Controls.Add(this.txtPaymentDueDateTo);
            this.PanelHeader.Controls.Add(this.txtPaymentDueDateFrom);
            this.PanelHeader.Controls.Add(this.btnSubF11);
            this.PanelHeader.Controls.Add(this.txtPaymentDateTo);
            this.PanelHeader.Controls.Add(this.ckM_Label10);
            this.PanelHeader.Controls.Add(this.txtPaymentDateFrom);
            this.PanelHeader.Controls.Add(this.ckM_Label11);
            this.PanelHeader.Controls.Add(this.chkUnpaid);
            this.PanelHeader.Controls.Add(this.chkPaid);
            this.PanelHeader.Controls.Add(this.ckM_Label9);
            this.PanelHeader.Controls.Add(this.PaymentCD);
            this.PanelHeader.Controls.Add(this.ckM_Label8);
            this.PanelHeader.Controls.Add(this.ckM_Label7);
            this.PanelHeader.Controls.Add(this.ckM_Label6);
            this.PanelHeader.Controls.Add(this.scStaffCD);
            this.PanelHeader.Controls.Add(this.ckM_Label5);
            this.PanelHeader.Controls.Add(this.txtEntryDateTo);
            this.PanelHeader.Controls.Add(this.ckM_Label4);
            this.PanelHeader.Controls.Add(this.txtEntryDateFrom);
            this.PanelHeader.Controls.Add(this.ckM_Label3);
            this.PanelHeader.Controls.Add(this.txtRecordDateTo);
            this.PanelHeader.Controls.Add(this.ckM_Label2);
            this.PanelHeader.Controls.Add(this.txtRecordDateFrom);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Size = new System.Drawing.Size(1255, 218);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtRecordDateFrom, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtRecordDateTo, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label3, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtEntryDateFrom, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label4, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtEntryDateTo, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label5, 0);
            this.PanelHeader.Controls.SetChildIndex(this.scStaffCD, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label6, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label7, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label8, 0);
            this.PanelHeader.Controls.SetChildIndex(this.PaymentCD, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label9, 0);
            this.PanelHeader.Controls.SetChildIndex(this.chkPaid, 0);
            this.PanelHeader.Controls.SetChildIndex(this.chkUnpaid, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label11, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtPaymentDateFrom, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label10, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtPaymentDateTo, 0);
            this.PanelHeader.Controls.SetChildIndex(this.btnSubF11, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtPaymentDueDateFrom, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtPaymentDueDateTo, 0);
            this.PanelHeader.Controls.SetChildIndex(this.chkTeiki, 0);
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
            this.ckM_Label1.Location = new System.Drawing.Point(51, 16);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label1.TabIndex = 1;
            this.ckM_Label1.Text = "計上日";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRecordDateFrom
            // 
            this.txtRecordDateFrom.AllowMinus = false;
            this.txtRecordDateFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtRecordDateFrom.BackColor = System.Drawing.Color.White;
            this.txtRecordDateFrom.BorderColor = false;
            this.txtRecordDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRecordDateFrom.ClientColor = System.Drawing.Color.White;
            this.txtRecordDateFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtRecordDateFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtRecordDateFrom.DecimalPlace = 0;
            this.txtRecordDateFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtRecordDateFrom.IntegerPart = 8;
            this.txtRecordDateFrom.IsCorrectDate = true;
            this.txtRecordDateFrom.isEnterKeyDown = false;
            this.txtRecordDateFrom.IsFirstTime = true;
            this.txtRecordDateFrom.isMaxLengthErr = false;
            this.txtRecordDateFrom.IsNumber = true;
            this.txtRecordDateFrom.IsShop = false;
            this.txtRecordDateFrom.Length = 10;
            this.txtRecordDateFrom.Location = new System.Drawing.Point(98, 13);
            this.txtRecordDateFrom.MaxLength = 10;
            this.txtRecordDateFrom.MoveNext = true;
            this.txtRecordDateFrom.Name = "txtRecordDateFrom";
            this.txtRecordDateFrom.Size = new System.Drawing.Size(100, 19);
            this.txtRecordDateFrom.TabIndex = 2;
            this.txtRecordDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRecordDateFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtRecordDateFrom.UseColorSizMode = false;
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
            this.ckM_Label2.Location = new System.Drawing.Point(216, 15);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label2.TabIndex = 3;
            this.ckM_Label2.Text = "～";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRecordDateTo
            // 
            this.txtRecordDateTo.AllowMinus = false;
            this.txtRecordDateTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtRecordDateTo.BackColor = System.Drawing.Color.White;
            this.txtRecordDateTo.BorderColor = false;
            this.txtRecordDateTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRecordDateTo.ClientColor = System.Drawing.Color.White;
            this.txtRecordDateTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtRecordDateTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtRecordDateTo.DecimalPlace = 0;
            this.txtRecordDateTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtRecordDateTo.IntegerPart = 8;
            this.txtRecordDateTo.IsCorrectDate = true;
            this.txtRecordDateTo.isEnterKeyDown = false;
            this.txtRecordDateTo.IsFirstTime = true;
            this.txtRecordDateTo.isMaxLengthErr = false;
            this.txtRecordDateTo.IsNumber = true;
            this.txtRecordDateTo.IsShop = false;
            this.txtRecordDateTo.Length = 10;
            this.txtRecordDateTo.Location = new System.Drawing.Point(251, 13);
            this.txtRecordDateTo.MaxLength = 10;
            this.txtRecordDateTo.MoveNext = true;
            this.txtRecordDateTo.Name = "txtRecordDateTo";
            this.txtRecordDateTo.Size = new System.Drawing.Size(100, 19);
            this.txtRecordDateTo.TabIndex = 4;
            this.txtRecordDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRecordDateTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtRecordDateTo.UseColorSizMode = false;
            this.txtRecordDateTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtRecordDateTo_KeyDown);
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
            this.ckM_Label3.Location = new System.Drawing.Point(25, 48);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label3.TabIndex = 7;
            this.ckM_Label3.Text = "経費入力日";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEntryDateFrom
            // 
            this.txtEntryDateFrom.AllowMinus = false;
            this.txtEntryDateFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtEntryDateFrom.BackColor = System.Drawing.Color.White;
            this.txtEntryDateFrom.BorderColor = false;
            this.txtEntryDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEntryDateFrom.ClientColor = System.Drawing.Color.White;
            this.txtEntryDateFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtEntryDateFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtEntryDateFrom.DecimalPlace = 0;
            this.txtEntryDateFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtEntryDateFrom.IntegerPart = 8;
            this.txtEntryDateFrom.IsCorrectDate = true;
            this.txtEntryDateFrom.isEnterKeyDown = false;
            this.txtEntryDateFrom.IsFirstTime = true;
            this.txtEntryDateFrom.isMaxLengthErr = false;
            this.txtEntryDateFrom.IsNumber = true;
            this.txtEntryDateFrom.IsShop = false;
            this.txtEntryDateFrom.Length = 10;
            this.txtEntryDateFrom.Location = new System.Drawing.Point(98, 45);
            this.txtEntryDateFrom.MaxLength = 10;
            this.txtEntryDateFrom.MoveNext = true;
            this.txtEntryDateFrom.Name = "txtEntryDateFrom";
            this.txtEntryDateFrom.Size = new System.Drawing.Size(100, 19);
            this.txtEntryDateFrom.TabIndex = 8;
            this.txtEntryDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtEntryDateFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtEntryDateFrom.UseColorSizMode = false;
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
            this.ckM_Label4.Location = new System.Drawing.Point(216, 47);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label4.TabIndex = 9;
            this.ckM_Label4.Text = "～";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEntryDateTo
            // 
            this.txtEntryDateTo.AllowMinus = false;
            this.txtEntryDateTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtEntryDateTo.BackColor = System.Drawing.Color.White;
            this.txtEntryDateTo.BorderColor = false;
            this.txtEntryDateTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEntryDateTo.ClientColor = System.Drawing.Color.White;
            this.txtEntryDateTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtEntryDateTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtEntryDateTo.DecimalPlace = 0;
            this.txtEntryDateTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtEntryDateTo.IntegerPart = 8;
            this.txtEntryDateTo.IsCorrectDate = true;
            this.txtEntryDateTo.isEnterKeyDown = false;
            this.txtEntryDateTo.IsFirstTime = true;
            this.txtEntryDateTo.isMaxLengthErr = false;
            this.txtEntryDateTo.IsNumber = true;
            this.txtEntryDateTo.IsShop = false;
            this.txtEntryDateTo.Length = 10;
            this.txtEntryDateTo.Location = new System.Drawing.Point(251, 45);
            this.txtEntryDateTo.MaxLength = 10;
            this.txtEntryDateTo.MoveNext = true;
            this.txtEntryDateTo.Name = "txtEntryDateTo";
            this.txtEntryDateTo.Size = new System.Drawing.Size(100, 19);
            this.txtEntryDateTo.TabIndex = 10;
            this.txtEntryDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtEntryDateTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtEntryDateTo.UseColorSizMode = false;
            this.txtEntryDateTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEntryDateTo_KeyDown);
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
            this.ckM_Label5.Location = new System.Drawing.Point(424, 17);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label5.TabIndex = 5;
            this.ckM_Label5.Text = "担当スタッフ";
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
            this.ckM_Label6.Location = new System.Drawing.Point(25, 82);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label6.TabIndex = 13;
            this.ckM_Label6.Text = "支払予定日";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label7.Location = new System.Drawing.Point(216, 81);
            this.ckM_Label7.Name = "ckM_Label7";
            this.ckM_Label7.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label7.TabIndex = 15;
            this.ckM_Label7.Text = "～";
            this.ckM_Label7.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label8.Location = new System.Drawing.Point(51, 114);
            this.ckM_Label8.Name = "ckM_Label8";
            this.ckM_Label8.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label8.TabIndex = 17;
            this.ckM_Label8.Text = "支払先";
            this.ckM_Label8.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label9
            // 
            this.ckM_Label9.AutoSize = true;
            this.ckM_Label9.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label9.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label9.DefaultlabelSize = true;
            this.ckM_Label9.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label9.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label9.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label9.Location = new System.Drawing.Point(37, 150);
            this.ckM_Label9.Name = "ckM_Label9";
            this.ckM_Label9.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label9.TabIndex = 19;
            this.ckM_Label9.Text = "支払状況";
            this.ckM_Label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkPaid
            // 
            this.chkPaid.AutoSize = true;
            this.chkPaid.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkPaid.Location = new System.Drawing.Point(98, 148);
            this.chkPaid.Name = "chkPaid";
            this.chkPaid.Size = new System.Drawing.Size(63, 16);
            this.chkPaid.TabIndex = 20;
            this.chkPaid.Text = "支払済";
            this.chkPaid.UseVisualStyleBackColor = true;
            // 
            // chkUnpaid
            // 
            this.chkUnpaid.AutoSize = true;
            this.chkUnpaid.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkUnpaid.Location = new System.Drawing.Point(170, 148);
            this.chkUnpaid.Name = "chkUnpaid";
            this.chkUnpaid.Size = new System.Drawing.Size(63, 16);
            this.chkUnpaid.TabIndex = 21;
            this.chkUnpaid.Text = "未支払";
            this.chkUnpaid.UseVisualStyleBackColor = true;
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
            this.txtPaymentDateTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtPaymentDateTo.IntegerPart = 8;
            this.txtPaymentDateTo.IsCorrectDate = true;
            this.txtPaymentDateTo.isEnterKeyDown = false;
            this.txtPaymentDateTo.IsFirstTime = true;
            this.txtPaymentDateTo.isMaxLengthErr = false;
            this.txtPaymentDateTo.IsNumber = true;
            this.txtPaymentDateTo.IsShop = false;
            this.txtPaymentDateTo.Length = 10;
            this.txtPaymentDateTo.Location = new System.Drawing.Point(251, 174);
            this.txtPaymentDateTo.MaxLength = 10;
            this.txtPaymentDateTo.MoveNext = true;
            this.txtPaymentDateTo.Name = "txtPaymentDateTo";
            this.txtPaymentDateTo.Size = new System.Drawing.Size(100, 19);
            this.txtPaymentDateTo.TabIndex = 25;
            this.txtPaymentDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPaymentDateTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtPaymentDateTo.UseColorSizMode = false;
            this.txtPaymentDateTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPaymentDateTo_KeyDown);
            // 
            // ckM_Label10
            // 
            this.ckM_Label10.AutoSize = true;
            this.ckM_Label10.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label10.DefaultlabelSize = true;
            this.ckM_Label10.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label10.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label10.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label10.Location = new System.Drawing.Point(216, 176);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label10.TabIndex = 24;
            this.ckM_Label10.Text = "～";
            this.ckM_Label10.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.txtPaymentDateFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtPaymentDateFrom.IntegerPart = 8;
            this.txtPaymentDateFrom.IsCorrectDate = true;
            this.txtPaymentDateFrom.isEnterKeyDown = false;
            this.txtPaymentDateFrom.IsFirstTime = true;
            this.txtPaymentDateFrom.isMaxLengthErr = false;
            this.txtPaymentDateFrom.IsNumber = true;
            this.txtPaymentDateFrom.IsShop = false;
            this.txtPaymentDateFrom.Length = 10;
            this.txtPaymentDateFrom.Location = new System.Drawing.Point(98, 174);
            this.txtPaymentDateFrom.MaxLength = 10;
            this.txtPaymentDateFrom.MoveNext = true;
            this.txtPaymentDateFrom.Name = "txtPaymentDateFrom";
            this.txtPaymentDateFrom.Size = new System.Drawing.Size(100, 19);
            this.txtPaymentDateFrom.TabIndex = 23;
            this.txtPaymentDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPaymentDateFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtPaymentDateFrom.UseColorSizMode = false;
            // 
            // ckM_Label11
            // 
            this.ckM_Label11.AutoSize = true;
            this.ckM_Label11.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label11.DefaultlabelSize = true;
            this.ckM_Label11.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label11.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label11.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label11.Location = new System.Drawing.Point(51, 178);
            this.ckM_Label11.Name = "ckM_Label11";
            this.ckM_Label11.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label11.TabIndex = 22;
            this.ckM_Label11.Text = "支払日";
            this.ckM_Label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.btnSubF11.Location = new System.Drawing.Point(1131, 178);
            this.btnSubF11.Margin = new System.Windows.Forms.Padding(1);
            this.btnSubF11.Name = "btnSubF11";
            this.btnSubF11.Size = new System.Drawing.Size(118, 28);
            this.btnSubF11.TabIndex = 26;
            this.btnSubF11.Text = "表示(F11)";
            this.btnSubF11.UseVisualStyleBackColor = false;
            this.btnSubF11.Click += new System.EventHandler(this.btnSubF11_Click);
            // 
            // dgvCostSearch
            // 
            this.dgvCostSearch.AllowUserToAddRows = false;
            this.dgvCostSearch.AllowUserToDeleteRows = false;
            this.dgvCostSearch.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.dgvCostSearch.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCostSearch.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvCostSearch.CheckCol = ((System.Collections.ArrayList)(resources.GetObject("dgvCostSearch.CheckCol")));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCostSearch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCostSearch.ColumnHeadersHeight = 25;
            this.dgvCostSearch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ExpenseNo,
            this.RecordDate,
            this.ExpenseEntryDate,
            this.Regular,
            this.PaymentDestination,
            this.Column1,
            this.Staff,
            this.Column2,
            this.PaymentDueDate,
            this.PaymentDate,
            this.TaxIncludePayment});
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("MS Gothic", 9F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCostSearch.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgvCostSearch.Enabled = false;
            this.dgvCostSearch.EnableHeadersVisualStyles = false;
            this.dgvCostSearch.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvCostSearch.Location = new System.Drawing.Point(12, 275);
            this.dgvCostSearch.MultiSelect = false;
            this.dgvCostSearch.Name = "dgvCostSearch";
            this.dgvCostSearch.ReadOnly = true;
            this.dgvCostSearch.RowHeight_ = 20;
            this.dgvCostSearch.RowTemplate.Height = 20;
            this.dgvCostSearch.Size = new System.Drawing.Size(1218, 490);
            this.dgvCostSearch.TabIndex = 1;
            this.dgvCostSearch.UseRowNo = true;
            this.dgvCostSearch.UseSetting = false;
            this.dgvCostSearch.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvCostSearch_CellPainting);
            this.dgvCostSearch.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvCostSearch_Paint);
            this.dgvCostSearch.DoubleClick += new System.EventHandler(this.dgvCostSearch_DoubleClick);
            // 
            // txtPaymentDueDateFrom
            // 
            this.txtPaymentDueDateFrom.AllowMinus = false;
            this.txtPaymentDueDateFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPaymentDueDateFrom.BackColor = System.Drawing.Color.White;
            this.txtPaymentDueDateFrom.BorderColor = false;
            this.txtPaymentDueDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPaymentDueDateFrom.ClientColor = System.Drawing.Color.White;
            this.txtPaymentDueDateFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtPaymentDueDateFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtPaymentDueDateFrom.DecimalPlace = 0;
            this.txtPaymentDueDateFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtPaymentDueDateFrom.IntegerPart = 8;
            this.txtPaymentDueDateFrom.IsCorrectDate = true;
            this.txtPaymentDueDateFrom.isEnterKeyDown = false;
            this.txtPaymentDueDateFrom.IsFirstTime = true;
            this.txtPaymentDueDateFrom.isMaxLengthErr = false;
            this.txtPaymentDueDateFrom.IsNumber = true;
            this.txtPaymentDueDateFrom.IsShop = false;
            this.txtPaymentDueDateFrom.Length = 10;
            this.txtPaymentDueDateFrom.Location = new System.Drawing.Point(98, 79);
            this.txtPaymentDueDateFrom.MaxLength = 10;
            this.txtPaymentDueDateFrom.MoveNext = true;
            this.txtPaymentDueDateFrom.Name = "txtPaymentDueDateFrom";
            this.txtPaymentDueDateFrom.Size = new System.Drawing.Size(100, 19);
            this.txtPaymentDueDateFrom.TabIndex = 14;
            this.txtPaymentDueDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPaymentDueDateFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtPaymentDueDateFrom.UseColorSizMode = false;
            // 
            // txtPaymentDueDateTo
            // 
            this.txtPaymentDueDateTo.AllowMinus = false;
            this.txtPaymentDueDateTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPaymentDueDateTo.BackColor = System.Drawing.Color.White;
            this.txtPaymentDueDateTo.BorderColor = false;
            this.txtPaymentDueDateTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPaymentDueDateTo.ClientColor = System.Drawing.Color.White;
            this.txtPaymentDueDateTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtPaymentDueDateTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtPaymentDueDateTo.DecimalPlace = 0;
            this.txtPaymentDueDateTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtPaymentDueDateTo.IntegerPart = 8;
            this.txtPaymentDueDateTo.IsCorrectDate = true;
            this.txtPaymentDueDateTo.isEnterKeyDown = false;
            this.txtPaymentDueDateTo.IsFirstTime = true;
            this.txtPaymentDueDateTo.isMaxLengthErr = false;
            this.txtPaymentDueDateTo.IsNumber = true;
            this.txtPaymentDueDateTo.IsShop = false;
            this.txtPaymentDueDateTo.Length = 10;
            this.txtPaymentDueDateTo.Location = new System.Drawing.Point(248, 79);
            this.txtPaymentDueDateTo.MaxLength = 10;
            this.txtPaymentDueDateTo.MoveNext = true;
            this.txtPaymentDueDateTo.Name = "txtPaymentDueDateTo";
            this.txtPaymentDueDateTo.Size = new System.Drawing.Size(103, 19);
            this.txtPaymentDueDateTo.TabIndex = 16;
            this.txtPaymentDueDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPaymentDueDateTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtPaymentDueDateTo.UseColorSizMode = false;
            this.txtPaymentDueDateTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPaymentDueDateTo_KeyDown);
            // 
            // chkTeiki
            // 
            this.chkTeiki.AutoSize = true;
            this.chkTeiki.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkTeiki.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkTeiki.Location = new System.Drawing.Point(476, 47);
            this.chkTeiki.Name = "chkTeiki";
            this.chkTeiki.Size = new System.Drawing.Size(50, 16);
            this.chkTeiki.TabIndex = 11;
            this.chkTeiki.Text = "定期";
            this.chkTeiki.UseVisualStyleBackColor = true;
            // 
            // PaymentCD
            // 
            this.PaymentCD.AutoSize = true;
            this.PaymentCD.ChangeDate = "";
            this.PaymentCD.ChangeDateWidth = 100;
            this.PaymentCD.Code = "";
            this.PaymentCD.CodeWidth = 100;
            this.PaymentCD.CodeWidth1 = 100;
            this.PaymentCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.PaymentCD.DataCheck = false;
            this.PaymentCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.PaymentCD.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.PaymentCD.IsCopy = false;
            this.PaymentCD.LabelText = "";
            this.PaymentCD.LabelVisible = true;
            this.PaymentCD.Location = new System.Drawing.Point(98, 106);
            this.PaymentCD.Margin = new System.Windows.Forms.Padding(0);
            this.PaymentCD.Name = "PaymentCD";
            this.PaymentCD.NameWidth = 310;
            this.PaymentCD.SearchEnable = true;
            this.PaymentCD.Size = new System.Drawing.Size(444, 27);
            this.PaymentCD.Stype = Search.CKM_SearchControl.SearchType.仕入先PayeeFlg;
            this.PaymentCD.TabIndex = 18;
            this.PaymentCD.test = null;
            this.PaymentCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.PaymentCD.UseChangeDate = false;
            this.PaymentCD.Value1 = null;
            this.PaymentCD.Value2 = null;
            this.PaymentCD.Value3 = null;
            this.PaymentCD.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.PaymentCD_CodeKeyDownEvent);
            this.PaymentCD.Enter += new System.EventHandler(this.PaymentCD_Enter);
            // 
            // scStaffCD
            // 
            this.scStaffCD.AutoSize = true;
            this.scStaffCD.ChangeDate = "";
            this.scStaffCD.ChangeDateWidth = 100;
            this.scStaffCD.Code = "";
            this.scStaffCD.CodeWidth = 70;
            this.scStaffCD.CodeWidth1 = 70;
            this.scStaffCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.scStaffCD.DataCheck = false;
            this.scStaffCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.scStaffCD.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.scStaffCD.IsCopy = false;
            this.scStaffCD.LabelText = "";
            this.scStaffCD.LabelVisible = true;
            this.scStaffCD.Location = new System.Drawing.Point(510, 7);
            this.scStaffCD.Margin = new System.Windows.Forms.Padding(0);
            this.scStaffCD.Name = "scStaffCD";
            this.scStaffCD.NameWidth = 250;
            this.scStaffCD.SearchEnable = true;
            this.scStaffCD.Size = new System.Drawing.Size(354, 27);
            this.scStaffCD.Stype = Search.CKM_SearchControl.SearchType.スタッフ;
            this.scStaffCD.TabIndex = 6;
            this.scStaffCD.test = null;
            this.scStaffCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.scStaffCD.UseChangeDate = false;
            this.scStaffCD.Value1 = null;
            this.scStaffCD.Value2 = null;
            this.scStaffCD.Value3 = null;
            this.scStaffCD.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.scStaffCD_CodeKeyDownEvent);
            // 
            // ExpenseNo
            // 
            this.ExpenseNo.DataPropertyName = "CostNo";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ExpenseNo.DefaultCellStyle = dataGridViewCellStyle3;
            this.ExpenseNo.HeaderText = "経費番号";
            this.ExpenseNo.Name = "ExpenseNo";
            this.ExpenseNo.ReadOnly = true;
            // 
            // RecordDate
            // 
            this.RecordDate.DataPropertyName = "RecordedDate";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.RecordDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.RecordDate.HeaderText = " 計上日";
            this.RecordDate.Name = "RecordDate";
            this.RecordDate.ReadOnly = true;
            // 
            // ExpenseEntryDate
            // 
            this.ExpenseEntryDate.DataPropertyName = "ExpenseEntryDate";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ExpenseEntryDate.DefaultCellStyle = dataGridViewCellStyle5;
            this.ExpenseEntryDate.HeaderText = " 経費入力日";
            this.ExpenseEntryDate.Name = "ExpenseEntryDate";
            this.ExpenseEntryDate.ReadOnly = true;
            // 
            // Regular
            // 
            this.Regular.DataPropertyName = "RegularlyFLG";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Regular.DefaultCellStyle = dataGridViewCellStyle6;
            this.Regular.HeaderText = "  定期";
            this.Regular.Name = "Regular";
            this.Regular.ReadOnly = true;
            this.Regular.Width = 70;
            // 
            // PaymentDestination
            // 
            this.PaymentDestination.DataPropertyName = "VendorCD";
            this.PaymentDestination.HeaderText = "支払先";
            this.PaymentDestination.Name = "PaymentDestination";
            this.PaymentDestination.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "VendorName";
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 340;
            // 
            // Staff
            // 
            this.Staff.DataPropertyName = "StaffCD";
            this.Staff.HeaderText = "担当スタッフ";
            this.Staff.Name = "Staff";
            this.Staff.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "StaffName";
            this.Column2.HeaderText = "";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 150;
            // 
            // PaymentDueDate
            // 
            this.PaymentDueDate.DataPropertyName = "PayPlanDate";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.PaymentDueDate.DefaultCellStyle = dataGridViewCellStyle7;
            this.PaymentDueDate.HeaderText = " 支払予定日";
            this.PaymentDueDate.Name = "PaymentDueDate";
            this.PaymentDueDate.ReadOnly = true;
            // 
            // PaymentDate
            // 
            this.PaymentDate.DataPropertyName = "PayDate";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.PaymentDate.DefaultCellStyle = dataGridViewCellStyle8;
            this.PaymentDate.HeaderText = "支払日";
            this.PaymentDate.Name = "PaymentDate";
            this.PaymentDate.ReadOnly = true;
            // 
            // TaxIncludePayment
            // 
            this.TaxIncludePayment.DataPropertyName = "CostGaku";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.TaxIncludePayment.DefaultCellStyle = dataGridViewCellStyle9;
            this.TaxIncludePayment.HeaderText = "   税込支払額";
            this.TaxIncludePayment.Name = "TaxIncludePayment";
            this.TaxIncludePayment.ReadOnly = true;
            // 
            // frmSearch_KeihiNO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1255, 861);
            this.Controls.Add(this.dgvCostSearch);
            this.F11Visible = true;
            this.F12Visible = true;
            this.F9Visible = true;
            this.Name = "frmSearch_KeihiNO";
            this.PanelHeaderHeight = 260;
            this.ProgramName = "経費番号検索";
            this.Text = "Search_KeihiNO";
            this.Load += new System.EventHandler(this.frmSearch_KeihiNO_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeihiNO_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeihiNO_KeyUp);
            this.Controls.SetChildIndex(this.dgvCostSearch, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCostSearch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_TextBox txtRecordDateFrom;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_TextBox txtEntryDateFrom;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_TextBox txtRecordDateTo;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_SearchControl scStaffCD;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_TextBox txtEntryDateTo;
        private CKM_Controls.CKM_Label ckM_Label7;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_Button btnSubF11;
        private CKM_Controls.CKM_TextBox txtPaymentDateTo;
        private CKM_Controls.CKM_Label ckM_Label10;
        private CKM_Controls.CKM_TextBox txtPaymentDateFrom;
        private CKM_Controls.CKM_Label ckM_Label11;
        private CKM_Controls.CKM_CheckBox chkUnpaid;
        private CKM_Controls.CKM_CheckBox chkPaid;
        private CKM_Controls.CKM_Label ckM_Label9;
        private CKM_SearchControl PaymentCD;
        private CKM_Controls.CKM_Label ckM_Label8;
        private CKM_Controls.CKM_GridView dgvCostSearch;
        private CKM_Controls.CKM_TextBox txtPaymentDueDateTo;
        private CKM_Controls.CKM_TextBox txtPaymentDueDateFrom;
        private CKM_Controls.CKM_CheckBox chkTeiki;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExpenseNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecordDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExpenseEntryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Regular;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentDestination;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Staff;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentDueDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxIncludePayment;
    }
}