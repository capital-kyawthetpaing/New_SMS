namespace MasterTouroku_YuubinBangou
{
    partial class frmMasterTouroku_YuubinBangou
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
            this.btnDisplay = new CKM_Controls.CKM_Button();
            this.PanelDetail = new System.Windows.Forms.Panel();
            this.dgvYuubinBangou = new CKM_Controls.CKM_GridView();
            this.colZipCD1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colZipCD2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAdd1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAdd2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCarrier = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colCarrierLeadDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.txtZip2To = new CKM_Controls.CKM_TextBox();
            this.txtZip1To = new CKM_Controls.CKM_TextBox();
            this.txtZip2From = new CKM_Controls.CKM_TextBox();
            this.txtZip1from = new CKM_Controls.CKM_TextBox();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.PanelHeader.SuspendLayout();
            this.PanelSearch.SuspendLayout();
            this.PanelDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvYuubinBangou)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.ckM_Label3);
            this.PanelHeader.Controls.Add(this.panel1);
            this.PanelHeader.Size = new System.Drawing.Size(1774, 91);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.Controls.SetChildIndex(this.panel1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label3, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Controls.Add(this.btnDisplay);
            this.PanelSearch.Location = new System.Drawing.Point(1240, 0);
            this.PanelSearch.TabIndex = 0;
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
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
            this.btnDisplay.Location = new System.Drawing.Point(410, 3);
            this.btnDisplay.Margin = new System.Windows.Forms.Padding(1);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(118, 28);
            this.btnDisplay.TabIndex = 0;
            this.btnDisplay.Text = "表示(F11)";
            this.btnDisplay.UseVisualStyleBackColor = false;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // PanelDetail
            // 
            this.PanelDetail.Controls.Add(this.dgvYuubinBangou);
            this.PanelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelDetail.Location = new System.Drawing.Point(0, 147);
            this.PanelDetail.Name = "PanelDetail";
            this.PanelDetail.Size = new System.Drawing.Size(1776, 782);
            this.PanelDetail.TabIndex = 0;
            // 
            // dgvYuubinBangou
            // 
            this.dgvYuubinBangou.AllowUserToDeleteRows = false;
            this.dgvYuubinBangou.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.dgvYuubinBangou.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvYuubinBangou.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvYuubinBangou.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvYuubinBangou.ColumnHeadersHeight = 25;
            this.dgvYuubinBangou.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colZipCD1,
            this.colZipCD2,
            this.colAdd1,
            this.colAdd2,
            this.colCarrier,
            this.colCarrierLeadDay});
            this.dgvYuubinBangou.EnableHeadersVisualStyles = false;
            this.dgvYuubinBangou.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvYuubinBangou.Location = new System.Drawing.Point(21, 17);
            this.dgvYuubinBangou.Name = "dgvYuubinBangou";
            this.dgvYuubinBangou.RowHeight_ = 20;
            this.dgvYuubinBangou.RowTemplate.Height = 20;
            this.dgvYuubinBangou.Size = new System.Drawing.Size(1750, 750);
            this.dgvYuubinBangou.TabIndex = 0;
            this.dgvYuubinBangou.UseRowNo = true;
            this.dgvYuubinBangou.UseSetting = true;
            this.dgvYuubinBangou.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvYuubinBangou_CellEndEdit);
            this.dgvYuubinBangou.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvYuubinBangou_CellPainting);
            this.dgvYuubinBangou.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvYuubinBangou_CellValidated);
            this.dgvYuubinBangou.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvYuubinBangou_DataError);
            this.dgvYuubinBangou.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvYuubinBangou_EditingControlShowing);
            this.dgvYuubinBangou.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvYuubinBangou_Paint);
            // 
            // colZipCD1
            // 
            this.colZipCD1.DataPropertyName = "ZipCD1";
            this.colZipCD1.HeaderText = "郵便番号";
            this.colZipCD1.MaxInputLength = 3;
            this.colZipCD1.Name = "colZipCD1";
            this.colZipCD1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colZipCD1.Width = 40;
            // 
            // colZipCD2
            // 
            this.colZipCD2.DataPropertyName = "ZipCD2";
            this.colZipCD2.HeaderText = "";
            this.colZipCD2.MaxInputLength = 4;
            this.colZipCD2.Name = "colZipCD2";
            this.colZipCD2.Width = 50;
            // 
            // colAdd1
            // 
            this.colAdd1.DataPropertyName = "Address1";
            this.colAdd1.HeaderText = "住所1";
            this.colAdd1.MaxInputLength = 80;
            this.colAdd1.Name = "colAdd1";
            this.colAdd1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colAdd1.Width = 700;
            // 
            // colAdd2
            // 
            this.colAdd2.DataPropertyName = "Address2";
            this.colAdd2.HeaderText = "住所2";
            this.colAdd2.MaxInputLength = 80;
            this.colAdd2.Name = "colAdd2";
            this.colAdd2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colAdd2.Width = 700;
            // 
            // colCarrier
            // 
            this.colCarrier.DataPropertyName = "(none)";
            this.colCarrier.HeaderText = "奨励運送会社";
            this.colCarrier.Name = "colCarrier";
            this.colCarrier.ReadOnly = true;
            this.colCarrier.Width = 150;
            // 
            // colCarrierLeadDay
            // 
            this.colCarrierLeadDay.DataPropertyName = "CarrierLeadDay";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = "0";
            this.colCarrierLeadDay.DefaultCellStyle = dataGridViewCellStyle3;
            this.colCarrierLeadDay.HeaderText = "日数";
            this.colCarrierLeadDay.MaxInputLength = 3;
            this.colCarrierLeadDay.Name = "colCarrierLeadDay";
            this.colCarrierLeadDay.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCarrierLeadDay.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCarrierLeadDay.Width = 50;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ckM_Label2);
            this.panel1.Controls.Add(this.txtZip2To);
            this.panel1.Controls.Add(this.txtZip1To);
            this.panel1.Controls.Add(this.txtZip2From);
            this.panel1.Controls.Add(this.txtZip1from);
            this.panel1.Controls.Add(this.ckM_Label1);
            this.panel1.Location = new System.Drawing.Point(61, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 50);
            this.panel1.TabIndex = 0;
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
            this.ckM_Label2.Location = new System.Drawing.Point(150, 18);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label2.TabIndex = 5;
            this.ckM_Label2.Text = "～";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtZip2To
            // 
            this.txtZip2To.AllowMinus = false;
            this.txtZip2To.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtZip2To.BackColor = System.Drawing.Color.White;
            this.txtZip2To.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtZip2To.ClientColor = System.Drawing.Color.White;
            this.txtZip2To.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtZip2To.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtZip2To.DecimalPlace = 0;
            this.txtZip2To.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtZip2To.IntegerPart = 0;
            this.txtZip2To.IsCorrectDate = true;
            this.txtZip2To.isEnterKeyDown = false;
            this.txtZip2To.isMaxLengthErr = false;
            this.txtZip2To.IsNumber = true;
            this.txtZip2To.IsShop = false;
            this.txtZip2To.Length = 4;
            this.txtZip2To.Location = new System.Drawing.Point(210, 14);
            this.txtZip2To.MaxLength = 4;
            this.txtZip2To.MoveNext = true;
            this.txtZip2To.Name = "txtZip2To";
            this.txtZip2To.Size = new System.Drawing.Size(40, 19);
            this.txtZip2To.TabIndex = 3;
            this.txtZip2To.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtZip2To.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtZip2To_KeyDown);
            // 
            // txtZip1To
            // 
            this.txtZip1To.AllowMinus = false;
            this.txtZip1To.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtZip1To.BackColor = System.Drawing.Color.White;
            this.txtZip1To.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtZip1To.ClientColor = System.Drawing.Color.White;
            this.txtZip1To.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtZip1To.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtZip1To.DecimalPlace = 0;
            this.txtZip1To.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtZip1To.IntegerPart = 0;
            this.txtZip1To.IsCorrectDate = true;
            this.txtZip1To.isEnterKeyDown = false;
            this.txtZip1To.isMaxLengthErr = false;
            this.txtZip1To.IsNumber = true;
            this.txtZip1To.IsShop = false;
            this.txtZip1To.Length = 3;
            this.txtZip1To.Location = new System.Drawing.Point(180, 14);
            this.txtZip1To.MaxLength = 3;
            this.txtZip1To.MoveNext = true;
            this.txtZip1To.Name = "txtZip1To";
            this.txtZip1To.Size = new System.Drawing.Size(30, 19);
            this.txtZip1To.TabIndex = 2;
            this.txtZip1To.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // txtZip2From
            // 
            this.txtZip2From.AllowMinus = false;
            this.txtZip2From.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtZip2From.BackColor = System.Drawing.Color.White;
            this.txtZip2From.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtZip2From.ClientColor = System.Drawing.Color.White;
            this.txtZip2From.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtZip2From.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtZip2From.DecimalPlace = 0;
            this.txtZip2From.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtZip2From.IntegerPart = 0;
            this.txtZip2From.IsCorrectDate = true;
            this.txtZip2From.isEnterKeyDown = false;
            this.txtZip2From.isMaxLengthErr = false;
            this.txtZip2From.IsNumber = true;
            this.txtZip2From.IsShop = false;
            this.txtZip2From.Length = 4;
            this.txtZip2From.Location = new System.Drawing.Point(98, 14);
            this.txtZip2From.MaxLength = 4;
            this.txtZip2From.MoveNext = true;
            this.txtZip2From.Name = "txtZip2From";
            this.txtZip2From.Size = new System.Drawing.Size(40, 19);
            this.txtZip2From.TabIndex = 1;
            this.txtZip2From.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtZip2From.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtZip2From_KeyDown);
            // 
            // txtZip1from
            // 
            this.txtZip1from.AllowMinus = false;
            this.txtZip1from.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtZip1from.BackColor = System.Drawing.Color.White;
            this.txtZip1from.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtZip1from.ClientColor = System.Drawing.Color.White;
            this.txtZip1from.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtZip1from.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtZip1from.DecimalPlace = 0;
            this.txtZip1from.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtZip1from.IntegerPart = 0;
            this.txtZip1from.IsCorrectDate = true;
            this.txtZip1from.isEnterKeyDown = false;
            this.txtZip1from.isMaxLengthErr = false;
            this.txtZip1from.IsNumber = true;
            this.txtZip1from.IsShop = false;
            this.txtZip1from.Length = 3;
            this.txtZip1from.Location = new System.Drawing.Point(68, 14);
            this.txtZip1from.MaxLength = 3;
            this.txtZip1from.MoveNext = true;
            this.txtZip1from.Name = "txtZip1from";
            this.txtZip1from.Size = new System.Drawing.Size(30, 19);
            this.txtZip1from.TabIndex = 0;
            this.txtZip1from.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_Label1.Location = new System.Drawing.Point(5, 18);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label1.TabIndex = 0;
            this.ckM_Label1.Text = "郵便番号";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label3
            // 
            this.ckM_Label3.AutoSize = true;
            this.ckM_Label3.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label3.DefaultlabelSize = false;
            this.ckM_Label3.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label3.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label3.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label3.Location = new System.Drawing.Point(367, 20);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(384, 12);
            this.ckM_Label3.TabIndex = 2;
            this.ckM_Label3.Text = "最大1000件までの表示です。適切な範囲指定を行ってください。";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // frmMasterTouroku_YuubinBangou
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1776, 961);
            this.Controls.Add(this.PanelDetail);
            this.F9Visible = false;
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "frmMasterTouroku_YuubinBangou";
            this.Text = "MasterTouroku_YuubinBangou";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmMasterTouroku_YuubinBangou_KeyUp);
            this.Controls.SetChildIndex(this.PanelDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            this.PanelSearch.ResumeLayout(false);
            this.PanelDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvYuubinBangou)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CKM_Controls.CKM_Button btnDisplay;
        private System.Windows.Forms.Panel PanelDetail;
        private System.Windows.Forms.Panel panel1;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_TextBox txtZip2To;
        private CKM_Controls.CKM_TextBox txtZip1To;
        private CKM_Controls.CKM_TextBox txtZip2From;
        private CKM_Controls.CKM_TextBox txtZip1from;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_GridView dgvYuubinBangou;
        private CKM_Controls.CKM_Label ckM_Label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colZipCD1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colZipCD2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdd1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdd2;
        private System.Windows.Forms.DataGridViewComboBoxColumn colCarrier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCarrierLeadDay;
    }
}

