namespace Shiharai_ShimeShori
{
    partial class Shiharai_ShimeShori
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Shiharai_ShimeShori));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvPaymentClose = new CKM_Controls.CKM_GridView();
            this.panelNormal = new System.Windows.Forms.Panel();
            this.Shiiresaki = new Search.CKM_SearchControl();
            this.ckM_LB_Shiiresaki = new CKM_Controls.CKM_Label();
            this.txtPayCloseDate = new CKM_Controls.CKM_TextBox();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.cboProcessType = new CKM_Controls.CKM_ComboBox();
            this.lblProcessing = new CKM_Controls.CKM_Label();
            this.btnDisplay = new CKM_Controls.CKM_Button();
            this.colDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProcess = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PanelHeader.SuspendLayout();
            this.PanelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaymentClose)).BeginInit();
            this.panelNormal.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.panelNormal);
            this.PanelHeader.Size = new System.Drawing.Size(1432, 144);
            this.PanelHeader.Controls.SetChildIndex(this.panelNormal, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Controls.Add(this.btnDisplay);
            this.PanelSearch.Location = new System.Drawing.Point(898, 0);
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            // 
            // dgvPaymentClose
            // 
            this.dgvPaymentClose.AllowUserToAddRows = false;
            this.dgvPaymentClose.AllowUserToDeleteRows = false;
            this.dgvPaymentClose.AllowUserToResizeRows = false;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.dgvPaymentClose.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvPaymentClose.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvPaymentClose.CheckCol = ((System.Collections.ArrayList)(resources.GetObject("dgvPaymentClose.CheckCol")));
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPaymentClose.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvPaymentClose.ColumnHeadersHeight = 25;
            this.dgvPaymentClose.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDateTime,
            this.colDate,
            this.PaymentCD,
            this.PaymentName,
            this.colProcess});
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("MS Gothic", 9F);
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPaymentClose.DefaultCellStyle = dataGridViewCellStyle14;
            this.dgvPaymentClose.EnableHeadersVisualStyles = false;
            this.dgvPaymentClose.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvPaymentClose.Location = new System.Drawing.Point(87, 221);
            this.dgvPaymentClose.Name = "dgvPaymentClose";
            this.dgvPaymentClose.RowHeight_ = 20;
            this.dgvPaymentClose.RowTemplate.Height = 20;
            this.dgvPaymentClose.Size = new System.Drawing.Size(1150, 600);
            this.dgvPaymentClose.TabIndex = 9;
            this.dgvPaymentClose.UseRowNo = true;
            this.dgvPaymentClose.UseSetting = true;
            this.dgvPaymentClose.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvPaymentClose_Paint);
            // 
            // panelNormal
            // 
            this.panelNormal.Controls.Add(this.Shiiresaki);
            this.panelNormal.Controls.Add(this.ckM_LB_Shiiresaki);
            this.panelNormal.Controls.Add(this.txtPayCloseDate);
            this.panelNormal.Controls.Add(this.ckM_Label1);
            this.panelNormal.Controls.Add(this.cboProcessType);
            this.panelNormal.Controls.Add(this.lblProcessing);
            this.panelNormal.Location = new System.Drawing.Point(58, 5);
            this.panelNormal.Name = "panelNormal";
            this.panelNormal.Size = new System.Drawing.Size(600, 100);
            this.panelNormal.TabIndex = 2;
            // 
            // Shiiresaki
            // 
            this.Shiiresaki.AutoSize = true;
            this.Shiiresaki.ChangeDate = "";
            this.Shiiresaki.ChangeDateWidth = 100;
            this.Shiiresaki.Code = "";
            this.Shiiresaki.CodeWidth = 100;
            this.Shiiresaki.CodeWidth1 = 100;
            this.Shiiresaki.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.Shiiresaki.DataCheck = false;
            this.Shiiresaki.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.Shiiresaki.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.Shiiresaki.IsCopy = false;
            this.Shiiresaki.LabelText = "";
            this.Shiiresaki.LabelVisible = true;
            this.Shiiresaki.Location = new System.Drawing.Point(72, 58);
            this.Shiiresaki.Margin = new System.Windows.Forms.Padding(0);
            this.Shiiresaki.Name = "Shiiresaki";
            this.Shiiresaki.NameWidth = 310;
            this.Shiiresaki.SearchEnable = true;
            this.Shiiresaki.Size = new System.Drawing.Size(444, 27);
            this.Shiiresaki.Stype = Search.CKM_SearchControl.SearchType.仕入先;
            this.Shiiresaki.TabIndex = 12;
            this.Shiiresaki.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.Shiiresaki.UseChangeDate = false;
            this.Shiiresaki.Value1 = null;
            this.Shiiresaki.Value2 = null;
            this.Shiiresaki.Value3 = null;
            this.Shiiresaki.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.Shiiresaki_CodeKeyDownEvent);
            this.Shiiresaki.Enter += new System.EventHandler(this.Shiiresaki_Enter);
            // 
            // ckM_LB_Shiiresaki
            // 
            this.ckM_LB_Shiiresaki.AutoSize = true;
            this.ckM_LB_Shiiresaki.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_LB_Shiiresaki.BackColor = System.Drawing.Color.Transparent;
            this.ckM_LB_Shiiresaki.DefaultlabelSize = true;
            this.ckM_LB_Shiiresaki.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_LB_Shiiresaki.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_LB_Shiiresaki.ForeColor = System.Drawing.Color.Black;
            this.ckM_LB_Shiiresaki.Location = new System.Drawing.Point(26, 67);
            this.ckM_LB_Shiiresaki.Name = "ckM_LB_Shiiresaki";
            this.ckM_LB_Shiiresaki.Size = new System.Drawing.Size(44, 12);
            this.ckM_LB_Shiiresaki.TabIndex = 13;
            this.ckM_LB_Shiiresaki.Text = "支払先";
            this.ckM_LB_Shiiresaki.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_LB_Shiiresaki.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPayCloseDate
            // 
            this.txtPayCloseDate.AllowMinus = false;
            this.txtPayCloseDate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPayCloseDate.BackColor = System.Drawing.Color.White;
            this.txtPayCloseDate.BorderColor = false;
            this.txtPayCloseDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPayCloseDate.ClientColor = System.Drawing.Color.White;
            this.txtPayCloseDate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtPayCloseDate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtPayCloseDate.DecimalPlace = 0;
            this.txtPayCloseDate.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtPayCloseDate.IntegerPart = 0;
            this.txtPayCloseDate.IsCorrectDate = true;
            this.txtPayCloseDate.isEnterKeyDown = false;
            this.txtPayCloseDate.IsFirstTime = true;
            this.txtPayCloseDate.isMaxLengthErr = false;
            this.txtPayCloseDate.IsNumber = true;
            this.txtPayCloseDate.IsShop = false;
            this.txtPayCloseDate.Length = 10;
            this.txtPayCloseDate.Location = new System.Drawing.Point(72, 38);
            this.txtPayCloseDate.MaxLength = 10;
            this.txtPayCloseDate.MoveNext = true;
            this.txtPayCloseDate.Name = "txtPayCloseDate";
            this.txtPayCloseDate.Size = new System.Drawing.Size(100, 19);
            this.txtPayCloseDate.TabIndex = 11;
            this.txtPayCloseDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPayCloseDate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtPayCloseDate.UseColorSizMode = false;
            this.txtPayCloseDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPayCloseDate_KeyDown);
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
            this.ckM_Label1.Location = new System.Drawing.Point(13, 42);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label1.TabIndex = 10;
            this.ckM_Label1.Text = "支払締日";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboProcessType
            // 
            this.cboProcessType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboProcessType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProcessType.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.Default;
            this.cboProcessType.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboProcessType.Flag = 0;
            this.cboProcessType.FormattingEnabled = true;
            this.cboProcessType.Length = 10;
            this.cboProcessType.Location = new System.Drawing.Point(71, 12);
            this.cboProcessType.MaxLength = 10;
            this.cboProcessType.MoveNext = true;
            this.cboProcessType.Name = "cboProcessType";
            this.cboProcessType.Size = new System.Drawing.Size(130, 20);
            this.cboProcessType.TabIndex = 9;
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblProcessing.BackColor = System.Drawing.Color.Transparent;
            this.lblProcessing.DefaultlabelSize = true;
            this.lblProcessing.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.lblProcessing.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.lblProcessing.ForeColor = System.Drawing.Color.Black;
            this.lblProcessing.Location = new System.Drawing.Point(37, 16);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(31, 12);
            this.lblProcessing.TabIndex = 8;
            this.lblProcessing.Text = "処理";
            this.lblProcessing.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblProcessing.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnDisplay
            // 
            this.btnDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnDisplay.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnDisplay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDisplay.DefaultBtnSize = false;
            this.btnDisplay.Enabled = false;
            this.btnDisplay.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnDisplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisplay.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnDisplay.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnDisplay.Location = new System.Drawing.Point(412, 1);
            this.btnDisplay.Margin = new System.Windows.Forms.Padding(1);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(118, 28);
            this.btnDisplay.TabIndex = 1;
            this.btnDisplay.Text = "表示(F11)";
            this.btnDisplay.UseVisualStyleBackColor = false;
            this.btnDisplay.Visible = false;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // colDateTime
            // 
            this.colDateTime.DataPropertyName = "PayCloseProcessingDateTime";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colDateTime.DefaultCellStyle = dataGridViewCellStyle10;
            this.colDateTime.HeaderText = "処理日時";
            this.colDateTime.Name = "colDateTime";
            this.colDateTime.Width = 200;
            // 
            // colDate
            // 
            this.colDate.DataPropertyName = "PayCloseDate";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.colDate.DefaultCellStyle = dataGridViewCellStyle11;
            this.colDate.HeaderText = "  締年月日";
            this.colDate.Name = "colDate";
            this.colDate.Width = 200;
            // 
            // PaymentCD
            // 
            this.PaymentCD.DataPropertyName = "PayeeCD";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.PaymentCD.DefaultCellStyle = dataGridViewCellStyle12;
            this.PaymentCD.HeaderText = "支払先";
            this.PaymentCD.Name = "PaymentCD";
            this.PaymentCD.Width = 150;
            // 
            // PaymentName
            // 
            this.PaymentName.DataPropertyName = "VendorName";
            this.PaymentName.HeaderText = "";
            this.PaymentName.Name = "PaymentName";
            this.PaymentName.Width = 350;
            // 
            // colProcess
            // 
            this.colProcess.DataPropertyName = "ProcessingKBN";
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.colProcess.DefaultCellStyle = dataGridViewCellStyle13;
            this.colProcess.HeaderText = "処理";
            this.colProcess.Name = "colProcess";
            this.colProcess.Width = 200;
            // 
            // Shiharai_ShimeShori
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1434, 961);
            this.Controls.Add(this.dgvPaymentClose);
            this.F10Visible = false;
            this.F7Visible = false;
            this.F8Visible = false;
            this.F9Visible = false;
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "Shiharai_ShimeShori";
            this.PanelHeaderHeight = 200;
            this.Text = "Shiharai_ShimeShori";
            this.Load += new System.EventHandler(this.ShiharaiShimeShori_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ShiharaiShimeShori_KeyUp);
            this.Controls.SetChildIndex(this.dgvPaymentClose, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPaymentClose)).EndInit();
            this.panelNormal.ResumeLayout(false);
            this.panelNormal.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CKM_Controls.CKM_GridView dgvPaymentClose;
        private System.Windows.Forms.Panel panelNormal;
        private CKM_Controls.CKM_TextBox txtPayCloseDate;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_ComboBox cboProcessType;
        private CKM_Controls.CKM_Label lblProcessing;
        private CKM_Controls.CKM_Button btnDisplay;
        private Search.CKM_SearchControl Shiiresaki;
        private CKM_Controls.CKM_Label ckM_LB_Shiiresaki;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProcess;
    }
}

