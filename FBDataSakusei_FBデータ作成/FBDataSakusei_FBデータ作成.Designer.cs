namespace FBDataSakusei_FBデータ作成
{
    partial class FrmFBDataSakusei_FBデータ作成
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmFBDataSakusei_FBデータ作成));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gvFBDataSakusei = new CKM_Controls.CKM_GridView();
            this.btnDisplay = new CKM_Controls.CKM_Button();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.txtTransferDate = new CKM_Controls.CKM_TextBox();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.txtPaymentDate = new CKM_Controls.CKM_TextBox();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.cboPayment = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.cboProcess = new CKM_Controls.CKM_ComboBox();
            this.colPayment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPayeeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colbank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBankTransfer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colbranch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTransferDestination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKouzaKBN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKouzaNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKouzaMeigi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTransferAcc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTransferAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTransferFee = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFeeKBN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFeeBurden = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvFBDataSakusei)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1579, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(1045, 0);
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gvFBDataSakusei);
            this.panel1.Controls.Add(this.btnDisplay);
            this.panel1.Controls.Add(this.ckM_Label4);
            this.panel1.Controls.Add(this.txtTransferDate);
            this.panel1.Controls.Add(this.ckM_Label2);
            this.panel1.Controls.Add(this.txtPaymentDate);
            this.panel1.Controls.Add(this.ckM_Label1);
            this.panel1.Controls.Add(this.cboPayment);
            this.panel1.Controls.Add(this.ckM_Label3);
            this.panel1.Controls.Add(this.cboProcess);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1581, 829);
            this.panel1.TabIndex = 100;
            // 
            // gvFBDataSakusei
            // 
            this.gvFBDataSakusei.AllowUserToAddRows = false;
            this.gvFBDataSakusei.AllowUserToDeleteRows = false;
            this.gvFBDataSakusei.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.gvFBDataSakusei.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.gvFBDataSakusei.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.gvFBDataSakusei.CheckCol = ((System.Collections.ArrayList)(resources.GetObject("gvFBDataSakusei.CheckCol")));
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvFBDataSakusei.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.gvFBDataSakusei.ColumnHeadersHeight = 25;
            this.gvFBDataSakusei.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPayment,
            this.colPayeeName,
            this.colbank,
            this.colBankTransfer,
            this.colbranch,
            this.colTransferDestination,
            this.colKouzaKBN,
            this.colKouzaNO,
            this.colKouzaMeigi,
            this.colTransferAcc,
            this.colTransferAmount,
            this.colTransferFee,
            this.colFeeKBN,
            this.colFeeBurden});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("MS Gothic", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvFBDataSakusei.DefaultCellStyle = dataGridViewCellStyle6;
            this.gvFBDataSakusei.EnableHeadersVisualStyles = false;
            this.gvFBDataSakusei.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.gvFBDataSakusei.Location = new System.Drawing.Point(43, 142);
            this.gvFBDataSakusei.Name = "gvFBDataSakusei";
            this.gvFBDataSakusei.RowHeight_ = 20;
            this.gvFBDataSakusei.RowTemplate.Height = 20;
            this.gvFBDataSakusei.Size = new System.Drawing.Size(1420, 500);
            this.gvFBDataSakusei.TabIndex = 44;
            this.gvFBDataSakusei.UseRowNo = true;
            this.gvFBDataSakusei.UseSetting = true;
            // 
            // btnDisplay
            // 
            this.btnDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnDisplay.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnDisplay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDisplay.DefaultBtnSize = false;
            this.btnDisplay.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnDisplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisplay.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnDisplay.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnDisplay.Location = new System.Drawing.Point(1344, 105);
            this.btnDisplay.Margin = new System.Windows.Forms.Padding(1);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(118, 28);
            this.btnDisplay.TabIndex = 43;
            this.btnDisplay.Text = "表示(F10)";
            this.btnDisplay.UseVisualStyleBackColor = false;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
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
            this.ckM_Label4.Location = new System.Drawing.Point(1165, 117);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label4.TabIndex = 42;
            this.ckM_Label4.Text = "実振込日";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTransferDate
            // 
            this.txtTransferDate.AllowMinus = false;
            this.txtTransferDate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtTransferDate.BackColor = System.Drawing.Color.White;
            this.txtTransferDate.BorderColor = false;
            this.txtTransferDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTransferDate.ClientColor = System.Drawing.Color.White;
            this.txtTransferDate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtTransferDate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtTransferDate.DecimalPlace = 0;
            this.txtTransferDate.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtTransferDate.IntegerPart = 0;
            this.txtTransferDate.IsCorrectDate = true;
            this.txtTransferDate.isEnterKeyDown = false;
            this.txtTransferDate.IsFirstTime = true;
            this.txtTransferDate.isMaxLengthErr = false;
            this.txtTransferDate.IsNumber = true;
            this.txtTransferDate.IsShop = false;
            this.txtTransferDate.Length = 10;
            this.txtTransferDate.Location = new System.Drawing.Point(1225, 114);
            this.txtTransferDate.MaxLength = 10;
            this.txtTransferDate.MoveNext = true;
            this.txtTransferDate.Name = "txtTransferDate";
            this.txtTransferDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtTransferDate.Size = new System.Drawing.Size(100, 19);
            this.txtTransferDate.TabIndex = 41;
            this.txtTransferDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTransferDate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtTransferDate.UseColorSizMode = false;
            this.txtTransferDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTransferDate_KeyDown);
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
            this.ckM_Label2.Location = new System.Drawing.Point(65, 97);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label2.TabIndex = 40;
            this.ckM_Label2.Text = "支払日";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPaymentDate
            // 
            this.txtPaymentDate.AllowMinus = false;
            this.txtPaymentDate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPaymentDate.BackColor = System.Drawing.Color.White;
            this.txtPaymentDate.BorderColor = false;
            this.txtPaymentDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPaymentDate.ClientColor = System.Drawing.Color.White;
            this.txtPaymentDate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtPaymentDate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtPaymentDate.DecimalPlace = 0;
            this.txtPaymentDate.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtPaymentDate.IntegerPart = 0;
            this.txtPaymentDate.IsCorrectDate = true;
            this.txtPaymentDate.isEnterKeyDown = false;
            this.txtPaymentDate.IsFirstTime = true;
            this.txtPaymentDate.isMaxLengthErr = false;
            this.txtPaymentDate.IsNumber = true;
            this.txtPaymentDate.IsShop = false;
            this.txtPaymentDate.Length = 10;
            this.txtPaymentDate.Location = new System.Drawing.Point(112, 93);
            this.txtPaymentDate.MaxLength = 10;
            this.txtPaymentDate.MoveNext = true;
            this.txtPaymentDate.Name = "txtPaymentDate";
            this.txtPaymentDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtPaymentDate.Size = new System.Drawing.Size(100, 19);
            this.txtPaymentDate.TabIndex = 39;
            this.txtPaymentDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPaymentDate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtPaymentDate.UseColorSizMode = false;
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
            this.ckM_Label1.Location = new System.Drawing.Point(41, 61);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label1.TabIndex = 38;
            this.ckM_Label1.Text = "支払元口座";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboPayment
            // 
            this.cboPayment.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboPayment.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPayment.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.銀行口座;
            this.cboPayment.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboPayment.Flag = 0;
            this.cboPayment.FormattingEnabled = true;
            this.cboPayment.Length = 10;
            this.cboPayment.Location = new System.Drawing.Point(113, 58);
            this.cboPayment.MaxLength = 10;
            this.cboPayment.MoveNext = true;
            this.cboPayment.Name = "cboPayment";
            this.cboPayment.Size = new System.Drawing.Size(150, 20);
            this.cboPayment.TabIndex = 37;
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
            this.ckM_Label3.Location = new System.Drawing.Point(78, 27);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label3.TabIndex = 36;
            this.ckM_Label3.Text = "処理";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboProcess
            // 
            this.cboProcess.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboProcess.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProcess.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.Default;
            this.cboProcess.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboProcess.Flag = 0;
            this.cboProcess.FormattingEnabled = true;
            this.cboProcess.Length = 10;
            this.cboProcess.Location = new System.Drawing.Point(114, 24);
            this.cboProcess.MaxLength = 10;
            this.cboProcess.MoveNext = true;
            this.cboProcess.Name = "cboProcess";
            this.cboProcess.Size = new System.Drawing.Size(200, 20);
            this.cboProcess.TabIndex = 35;
            this.cboProcess.SelectedIndexChanged += new System.EventHandler(this.cboProcess_SelectedIndexChanged);
            this.cboProcess.SelectedValueChanged += new System.EventHandler(this.cboProcess_SelectedValueChanged);
            // 
            // colPayment
            // 
            this.colPayment.DataPropertyName = "PayeeCD";
            this.colPayment.HeaderText = "支払先";
            this.colPayment.MaxInputLength = 11;
            this.colPayment.Name = "colPayment";
            this.colPayment.Width = 110;
            // 
            // colPayeeName
            // 
            this.colPayeeName.DataPropertyName = "VendorName";
            this.colPayeeName.HeaderText = "支払先名";
            this.colPayeeName.MaxInputLength = 100;
            this.colPayeeName.Name = "colPayeeName";
            this.colPayeeName.Width = 300;
            // 
            // colbank
            // 
            this.colbank.DataPropertyName = "BankCD";
            this.colbank.HeaderText = "振込先銀行";
            this.colbank.Name = "colbank";
            this.colbank.Visible = false;
            // 
            // colBankTransfer
            // 
            this.colBankTransfer.DataPropertyName = "BankName";
            this.colBankTransfer.HeaderText = "振込先銀行";
            this.colBankTransfer.MaxInputLength = 60;
            this.colBankTransfer.Name = "colBankTransfer";
            this.colBankTransfer.Width = 150;
            // 
            // colbranch
            // 
            this.colbranch.DataPropertyName = "BranchCD";
            this.colbranch.HeaderText = "振込先支店";
            this.colbranch.Name = "colbranch";
            this.colbranch.Visible = false;
            // 
            // colTransferDestination
            // 
            this.colTransferDestination.DataPropertyName = "BranchName";
            this.colTransferDestination.HeaderText = "振込先支店";
            this.colTransferDestination.MaxInputLength = 60;
            this.colTransferDestination.Name = "colTransferDestination";
            this.colTransferDestination.Width = 150;
            // 
            // colKouzaKBN
            // 
            this.colKouzaKBN.DataPropertyName = "KouzaKBN";
            this.colKouzaKBN.HeaderText = "振込先口座種別";
            this.colKouzaKBN.Name = "colKouzaKBN";
            this.colKouzaKBN.Visible = false;
            // 
            // colKouzaNO
            // 
            this.colKouzaNO.DataPropertyName = "KouzaNO";
            this.colKouzaNO.HeaderText = "振込先口座番号";
            this.colKouzaNO.Name = "colKouzaNO";
            this.colKouzaNO.Visible = false;
            // 
            // colKouzaMeigi
            // 
            this.colKouzaMeigi.DataPropertyName = "KouzaMeigi";
            this.colKouzaMeigi.HeaderText = "振込先口座名義";
            this.colKouzaMeigi.Name = "colKouzaMeigi";
            this.colKouzaMeigi.Visible = false;
            // 
            // colTransferAcc
            // 
            this.colTransferAcc.DataPropertyName = "transferAcc";
            this.colTransferAcc.HeaderText = "振込口座";
            this.colTransferAcc.MaxInputLength = 60;
            this.colTransferAcc.Name = "colTransferAcc";
            this.colTransferAcc.Width = 300;
            // 
            // colTransferAmount
            // 
            this.colTransferAmount.DataPropertyName = "TransferGaku";
            this.colTransferAmount.HeaderText = "振込額";
            this.colTransferAmount.Name = "colTransferAmount";
            // 
            // colTransferFee
            // 
            this.colTransferFee.DataPropertyName = "TransferFeeGaku";
            this.colTransferFee.HeaderText = "振込手数料";
            this.colTransferFee.Name = "colTransferFee";
            // 
            // colFeeKBN
            // 
            this.colFeeKBN.DataPropertyName = "FeeKBN";
            this.colFeeKBN.HeaderText = "手数料負担区分";
            this.colFeeKBN.Name = "colFeeKBN";
            this.colFeeKBN.Visible = false;
            // 
            // colFeeBurden
            // 
            this.colFeeBurden.DataPropertyName = "FeeKBN1";
            this.colFeeBurden.HeaderText = "手数料負担";
            this.colFeeBurden.MaxInputLength = 80;
            this.colFeeBurden.Name = "colFeeBurden";
            this.colFeeBurden.Width = 150;
            // 
            // FrmFBDataSakusei_FBデータ作成
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1581, 911);
            this.Controls.Add(this.panel1);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "FrmFBDataSakusei_FBデータ作成";
            this.PanelHeaderHeight = 50;
            this.Text = "FBDataSakusei_FBデータ作成";
            this.Load += new System.EventHandler(this.FrmFBDataSakusei_FBデータ作成_Load);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvFBDataSakusei)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_ComboBox cboProcess;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_ComboBox cboPayment;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_TextBox txtPaymentDate;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_TextBox txtTransferDate;
        private CKM_Controls.CKM_Button btnDisplay;
        private CKM_Controls.CKM_GridView gvFBDataSakusei;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPayment;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPayeeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colbank;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBankTransfer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colbranch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTransferDestination;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKouzaKBN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKouzaNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKouzaMeigi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTransferAcc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTransferAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTransferFee;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFeeKBN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFeeBurden;
    }
}