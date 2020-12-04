namespace KeihiNyuuryoku
{
    partial class frmKeihiNyuuryoku
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKeihiNyuuryoku));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelDetail = new System.Windows.Forms.Panel();
            this.lblTotalGaku = new CKM_Controls.CKM_Label();
            this.ckM_Label8 = new CKM_Controls.CKM_Label();
            this.chkRegularFlg = new CKM_Controls.CKM_CheckBox();
            this.dgvKehiNyuuryoku = new CKM_Controls.CKM_GridView();
            this.ckM_Label7 = new CKM_Controls.CKM_Label();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.ScStaff = new Search.CKM_SearchControl();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.txtShihraiYoteiDate = new CKM_Controls.CKM_TextBox();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.txtKeijouDate = new CKM_Controls.CKM_TextBox();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ScVendor = new Search.CKM_SearchControl();
            this.PanelNormal = new System.Windows.Forms.Panel();
            this.ScCost = new Search.CKM_SearchControl();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.PanelCopy = new System.Windows.Forms.Panel();
            this.ScCost_Copy = new Search.CKM_SearchControl();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.colCostCD = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colSummary = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDepartment = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colCostGaku = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PanelHeader.SuspendLayout();
            this.panelDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKehiNyuuryoku)).BeginInit();
            this.PanelNormal.SuspendLayout();
            this.PanelCopy.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.PanelCopy);
            this.PanelHeader.Controls.Add(this.PanelNormal);
            this.PanelHeader.Size = new System.Drawing.Size(1774, 91);
            this.PanelHeader.Controls.SetChildIndex(this.PanelNormal, 0);
            this.PanelHeader.Controls.SetChildIndex(this.PanelCopy, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(1240, 0);
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            // 
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.lblTotalGaku);
            this.panelDetail.Controls.Add(this.ckM_Label8);
            this.panelDetail.Controls.Add(this.chkRegularFlg);
            this.panelDetail.Controls.Add(this.dgvKehiNyuuryoku);
            this.panelDetail.Controls.Add(this.ckM_Label7);
            this.panelDetail.Controls.Add(this.ckM_Label6);
            this.panelDetail.Controls.Add(this.ScStaff);
            this.panelDetail.Controls.Add(this.ckM_Label5);
            this.panelDetail.Controls.Add(this.txtShihraiYoteiDate);
            this.panelDetail.Controls.Add(this.ckM_Label4);
            this.panelDetail.Controls.Add(this.txtKeijouDate);
            this.panelDetail.Controls.Add(this.ckM_Label3);
            this.panelDetail.Controls.Add(this.ScVendor);
            this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetail.Location = new System.Drawing.Point(0, 147);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(1776, 782);
            this.panelDetail.TabIndex = 9;
            // 
            // lblTotalGaku
            // 
            this.lblTotalGaku.AutoSize = true;
            this.lblTotalGaku.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Green;
            this.lblTotalGaku.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblTotalGaku.DefaultlabelSize = true;
            this.lblTotalGaku.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalGaku.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.lblTotalGaku.ForeColor = System.Drawing.Color.Black;
            this.lblTotalGaku.Location = new System.Drawing.Point(1268, 746);
            this.lblTotalGaku.Name = "lblTotalGaku";
            this.lblTotalGaku.Size = new System.Drawing.Size(89, 12);
            this.lblTotalGaku.TabIndex = 15;
            this.lblTotalGaku.Text = "-999,999,999";
            this.lblTotalGaku.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.lblTotalGaku.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label8.Location = new System.Drawing.Point(1224, 747);
            this.ckM_Label8.Name = "ckM_Label8";
            this.ckM_Label8.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label8.TabIndex = 14;
            this.ckM_Label8.Text = "合計額";
            this.ckM_Label8.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkRegularFlg
            // 
            this.chkRegularFlg.AutoSize = true;
            this.chkRegularFlg.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkRegularFlg.Checked = true;
            this.chkRegularFlg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRegularFlg.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkRegularFlg.Location = new System.Drawing.Point(96, 40);
            this.chkRegularFlg.Name = "chkRegularFlg";
            this.chkRegularFlg.Size = new System.Drawing.Size(50, 16);
            this.chkRegularFlg.TabIndex = 13;
            this.chkRegularFlg.Text = "定期";
            this.chkRegularFlg.UseVisualStyleBackColor = true;
            // 
            // dgvKehiNyuuryoku
            // 
            this.dgvKehiNyuuryoku.AllowUserToAddRows = false;
            this.dgvKehiNyuuryoku.AllowUserToDeleteRows = false;
            this.dgvKehiNyuuryoku.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.dgvKehiNyuuryoku.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvKehiNyuuryoku.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvKehiNyuuryoku.CheckCol = ((System.Collections.ArrayList)(resources.GetObject("dgvKehiNyuuryoku.CheckCol")));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvKehiNyuuryoku.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvKehiNyuuryoku.ColumnHeadersHeight = 25;
            this.dgvKehiNyuuryoku.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCostCD,
            this.colSummary,
            this.colDepartment,
            this.colCostGaku});
            this.dgvKehiNyuuryoku.EnableHeadersVisualStyles = false;
            this.dgvKehiNyuuryoku.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvKehiNyuuryoku.Location = new System.Drawing.Point(76, 78);
            this.dgvKehiNyuuryoku.Name = "dgvKehiNyuuryoku";
            this.dgvKehiNyuuryoku.RowHeight_ = 20;
            this.dgvKehiNyuuryoku.RowTemplate.Height = 20;
            this.dgvKehiNyuuryoku.Size = new System.Drawing.Size(1300, 667);
            this.dgvKehiNyuuryoku.TabIndex = 12;
            this.dgvKehiNyuuryoku.UseRowNo = true;
            this.dgvKehiNyuuryoku.UseSetting = true;
            this.dgvKehiNyuuryoku.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKehiNyuuryoku_CellEndEdit);
            this.dgvKehiNyuuryoku.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvKehiNyuuryoku_CellValidating);
            this.dgvKehiNyuuryoku.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvKehiNyuuryoku_DataBindingComplete);
            this.dgvKehiNyuuryoku.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvKehiNyuuryoku_DataError);
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
            this.ckM_Label7.Location = new System.Drawing.Point(63, 63);
            this.ckM_Label7.Name = "ckM_Label7";
            this.ckM_Label7.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label7.TabIndex = 11;
            this.ckM_Label7.Text = "【経費明細】";
            this.ckM_Label7.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label6.Location = new System.Drawing.Point(1249, 18);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label6.TabIndex = 9;
            this.ckM_Label6.Text = "担当スタッフ";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScStaff
            // 
            this.ScStaff.AutoSize = true;
            this.ScStaff.ChangeDate = "";
            this.ScStaff.ChangeDateWidth = 100;
            this.ScStaff.Code = "";
            this.ScStaff.CodeWidth = 70;
            this.ScStaff.CodeWidth1 = 70;
            this.ScStaff.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScStaff.DataCheck = false;
            this.ScStaff.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScStaff.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScStaff.IsCopy = false;
            this.ScStaff.LabelText = "";
            this.ScStaff.LabelVisible = true;
            this.ScStaff.Location = new System.Drawing.Point(1335, 8);
            this.ScStaff.Margin = new System.Windows.Forms.Padding(0);
            this.ScStaff.Name = "ScStaff";
            this.ScStaff.NameWidth = 250;
            this.ScStaff.SearchEnable = true;
            this.ScStaff.Size = new System.Drawing.Size(354, 27);
            this.ScStaff.Stype = Search.CKM_SearchControl.SearchType.スタッフ;
            this.ScStaff.TabIndex = 8;
            this.ScStaff.test = null;
            this.ScStaff.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScStaff.UseChangeDate = false;
            this.ScStaff.Value1 = null;
            this.ScStaff.Value2 = null;
            this.ScStaff.Value3 = null;
            this.ScStaff.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScStaff_CodeKeyDownEvent);
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
            this.ckM_Label5.Location = new System.Drawing.Point(874, 19);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label5.TabIndex = 7;
            this.ckM_Label5.Text = "支払予定日";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtShihraiYoteiDate
            // 
            this.txtShihraiYoteiDate.AllowMinus = false;
            this.txtShihraiYoteiDate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtShihraiYoteiDate.BackColor = System.Drawing.Color.White;
            this.txtShihraiYoteiDate.BorderColor = false;
            this.txtShihraiYoteiDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShihraiYoteiDate.ClientColor = System.Drawing.Color.White;
            this.txtShihraiYoteiDate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtShihraiYoteiDate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtShihraiYoteiDate.DecimalPlace = 0;
            this.txtShihraiYoteiDate.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtShihraiYoteiDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtShihraiYoteiDate.IntegerPart = 0;
            this.txtShihraiYoteiDate.IsCorrectDate = true;
            this.txtShihraiYoteiDate.isEnterKeyDown = false;
            this.txtShihraiYoteiDate.IsFirstTime = true;
            this.txtShihraiYoteiDate.isMaxLengthErr = false;
            this.txtShihraiYoteiDate.IsNumber = true;
            this.txtShihraiYoteiDate.IsShop = false;
            this.txtShihraiYoteiDate.Length = 10;
            this.txtShihraiYoteiDate.Location = new System.Drawing.Point(948, 15);
            this.txtShihraiYoteiDate.MaxLength = 10;
            this.txtShihraiYoteiDate.MoveNext = true;
            this.txtShihraiYoteiDate.Name = "txtShihraiYoteiDate";
            this.txtShihraiYoteiDate.Size = new System.Drawing.Size(100, 19);
            this.txtShihraiYoteiDate.TabIndex = 6;
            this.txtShihraiYoteiDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtShihraiYoteiDate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtShihraiYoteiDate.UseColorSizMode = false;
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
            this.ckM_Label4.Location = new System.Drawing.Point(654, 18);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label4.TabIndex = 5;
            this.ckM_Label4.Text = "計上日";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtKeijouDate
            // 
            this.txtKeijouDate.AllowMinus = false;
            this.txtKeijouDate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtKeijouDate.BackColor = System.Drawing.Color.White;
            this.txtKeijouDate.BorderColor = false;
            this.txtKeijouDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKeijouDate.ClientColor = System.Drawing.Color.White;
            this.txtKeijouDate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtKeijouDate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtKeijouDate.DecimalPlace = 0;
            this.txtKeijouDate.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtKeijouDate.IntegerPart = 0;
            this.txtKeijouDate.IsCorrectDate = true;
            this.txtKeijouDate.isEnterKeyDown = false;
            this.txtKeijouDate.IsFirstTime = true;
            this.txtKeijouDate.isMaxLengthErr = false;
            this.txtKeijouDate.IsNumber = true;
            this.txtKeijouDate.IsShop = false;
            this.txtKeijouDate.Length = 10;
            this.txtKeijouDate.Location = new System.Drawing.Point(702, 14);
            this.txtKeijouDate.MaxLength = 10;
            this.txtKeijouDate.MoveNext = true;
            this.txtKeijouDate.Name = "txtKeijouDate";
            this.txtKeijouDate.Size = new System.Drawing.Size(100, 19);
            this.txtKeijouDate.TabIndex = 4;
            this.txtKeijouDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtKeijouDate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtKeijouDate.UseColorSizMode = false;
            this.txtKeijouDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKeijouDate_KeyDown);
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
            this.ckM_Label3.Location = new System.Drawing.Point(68, 18);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label3.TabIndex = 3;
            this.ckM_Label3.Text = "支払先";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScVendor
            // 
            this.ScVendor.AutoSize = true;
            this.ScVendor.ChangeDate = "";
            this.ScVendor.ChangeDateWidth = 100;
            this.ScVendor.Code = "";
            this.ScVendor.CodeWidth = 100;
            this.ScVendor.CodeWidth1 = 100;
            this.ScVendor.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScVendor.DataCheck = false;
            this.ScVendor.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScVendor.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScVendor.IsCopy = false;
            this.ScVendor.LabelText = "";
            this.ScVendor.LabelVisible = true;
            this.ScVendor.Location = new System.Drawing.Point(115, 8);
            this.ScVendor.Margin = new System.Windows.Forms.Padding(0);
            this.ScVendor.Name = "ScVendor";
            this.ScVendor.NameWidth = 310;
            this.ScVendor.SearchEnable = true;
            this.ScVendor.Size = new System.Drawing.Size(444, 27);
            this.ScVendor.Stype = Search.CKM_SearchControl.SearchType.仕入先;
            this.ScVendor.TabIndex = 2;
            this.ScVendor.test = null;
            this.ScVendor.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScVendor.UseChangeDate = false;
            this.ScVendor.Value1 = null;
            this.ScVendor.Value2 = null;
            this.ScVendor.Value3 = null;
            this.ScVendor.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScVendor_CodeKeyDownEvent);
            this.ScVendor.Enter += new System.EventHandler(this.ScVendor_Enter);
            // 
            // PanelNormal
            // 
            this.PanelNormal.Controls.Add(this.ScCost);
            this.PanelNormal.Controls.Add(this.ckM_Label1);
            this.PanelNormal.Location = new System.Drawing.Point(47, 3);
            this.PanelNormal.Name = "PanelNormal";
            this.PanelNormal.Size = new System.Drawing.Size(240, 50);
            this.PanelNormal.TabIndex = 2;
            this.PanelNormal.Enter += new System.EventHandler(this.PanelNormal_Enter);
            // 
            // ScCost
            // 
            this.ScCost.AutoSize = true;
            this.ScCost.ChangeDate = "";
            this.ScCost.ChangeDateWidth = 100;
            this.ScCost.Code = "";
            this.ScCost.CodeWidth = 100;
            this.ScCost.CodeWidth1 = 100;
            this.ScCost.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScCost.DataCheck = false;
            this.ScCost.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScCost.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScCost.IsCopy = false;
            this.ScCost.LabelText = "";
            this.ScCost.LabelVisible = false;
            this.ScCost.Location = new System.Drawing.Point(70, 10);
            this.ScCost.Margin = new System.Windows.Forms.Padding(0);
            this.ScCost.Name = "ScCost";
            this.ScCost.NameWidth = 280;
            this.ScCost.SearchEnable = true;
            this.ScCost.Size = new System.Drawing.Size(133, 27);
            this.ScCost.Stype = Search.CKM_SearchControl.SearchType.経費番号;
            this.ScCost.TabIndex = 14;
            this.ScCost.test = null;
            this.ScCost.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScCost.UseChangeDate = false;
            this.ScCost.Value1 = null;
            this.ScCost.Value2 = null;
            this.ScCost.Value3 = null;
            this.ScCost.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScCost_CodeKeyDownEvent);
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
            this.ckM_Label1.Location = new System.Drawing.Point(7, 20);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label1.TabIndex = 1;
            this.ckM_Label1.Text = "経費番号";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PanelCopy
            // 
            this.PanelCopy.Controls.Add(this.ScCost_Copy);
            this.PanelCopy.Controls.Add(this.ckM_Label2);
            this.PanelCopy.Location = new System.Drawing.Point(298, 3);
            this.PanelCopy.Name = "PanelCopy";
            this.PanelCopy.Size = new System.Drawing.Size(270, 50);
            this.PanelCopy.TabIndex = 3;
            this.PanelCopy.Enter += new System.EventHandler(this.PanelCopy_Enter);
            // 
            // ScCost_Copy
            // 
            this.ScCost_Copy.AutoSize = true;
            this.ScCost_Copy.ChangeDate = "";
            this.ScCost_Copy.ChangeDateWidth = 100;
            this.ScCost_Copy.Code = "";
            this.ScCost_Copy.CodeWidth = 100;
            this.ScCost_Copy.CodeWidth1 = 100;
            this.ScCost_Copy.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScCost_Copy.DataCheck = false;
            this.ScCost_Copy.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScCost_Copy.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScCost_Copy.IsCopy = false;
            this.ScCost_Copy.LabelText = "";
            this.ScCost_Copy.LabelVisible = false;
            this.ScCost_Copy.Location = new System.Drawing.Point(109, 10);
            this.ScCost_Copy.Margin = new System.Windows.Forms.Padding(0);
            this.ScCost_Copy.Name = "ScCost_Copy";
            this.ScCost_Copy.NameWidth = 280;
            this.ScCost_Copy.SearchEnable = true;
            this.ScCost_Copy.Size = new System.Drawing.Size(133, 27);
            this.ScCost_Copy.Stype = Search.CKM_SearchControl.SearchType.経費番号;
            this.ScCost_Copy.TabIndex = 14;
            this.ScCost_Copy.test = null;
            this.ScCost_Copy.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScCost_Copy.UseChangeDate = false;
            this.ScCost_Copy.Value1 = null;
            this.ScCost_Copy.Value2 = null;
            this.ScCost_Copy.Value3 = null;
            this.ScCost_Copy.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScCost_Copy_CodeKeyDownEvent);
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
            this.ckM_Label2.Location = new System.Drawing.Point(20, 20);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label2.TabIndex = 3;
            this.ckM_Label2.Text = "複写経費番号";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // colCostCD
            // 
            this.colCostCD.DataPropertyName = "CostCD";
            this.colCostCD.HeaderText = "経費CD";
            this.colCostCD.MinimumWidth = 170;
            this.colCostCD.Name = "colCostCD";
            this.colCostCD.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCostCD.Width = 170;
            // 
            // colSummary
            // 
            this.colSummary.DataPropertyName = "Summary";
            this.colSummary.HeaderText = "摘要";
            this.colSummary.MaxInputLength = 100;
            this.colSummary.Name = "colSummary";
            this.colSummary.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSummary.Width = 800;
            // 
            // colDepartment
            // 
            this.colDepartment.HeaderText = "部門";
            this.colDepartment.Name = "colDepartment";
            this.colDepartment.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDepartment.Width = 180;
            // 
            // colCostGaku
            // 
            this.colCostGaku.DataPropertyName = "CostGaku";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "C0";
            dataGridViewCellStyle3.NullValue = "0";
            this.colCostGaku.DefaultCellStyle = dataGridViewCellStyle3;
            this.colCostGaku.HeaderText = "税込支払額";
            this.colCostGaku.MaxInputLength = 12;
            this.colCostGaku.Name = "colCostGaku";
            this.colCostGaku.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCostGaku.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCostGaku.Width = 90;
            // 
            // frmKeihiNyuuryoku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1776, 961);
            this.Controls.Add(this.panelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "frmKeihiNyuuryoku";
            this.Text = "KeihiNyuuryoku";
            this.Load += new System.EventHandler(this.FormLoadEvent);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmKeihiNyuuryoku_KeyUp);
            this.Controls.SetChildIndex(this.panelDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.panelDetail.ResumeLayout(false);
            this.panelDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKehiNyuuryoku)).EndInit();
            this.PanelNormal.ResumeLayout(false);
            this.PanelNormal.PerformLayout();
            this.PanelCopy.ResumeLayout(false);
            this.PanelCopy.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PanelCopy;
        private System.Windows.Forms.Panel PanelNormal;
        private System.Windows.Forms.Panel panelDetail;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_GridView dgvKehiNyuuryoku;
        private CKM_Controls.CKM_Label ckM_Label7;
        private CKM_Controls.CKM_Label ckM_Label6;
        private Search.CKM_SearchControl ScStaff;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_TextBox txtShihraiYoteiDate;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_TextBox txtKeijouDate;
        private CKM_Controls.CKM_Label ckM_Label3;
        private Search.CKM_SearchControl ScVendor;
        private CKM_Controls.CKM_CheckBox chkRegularFlg;
        private Search.CKM_SearchControl ScCost_Copy;
        private Search.CKM_SearchControl ScCost;
        private CKM_Controls.CKM_Label lblTotalGaku;
        private CKM_Controls.CKM_Label ckM_Label8;
        private System.Windows.Forms.DataGridViewComboBoxColumn colCostCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSummary;
        private System.Windows.Forms.DataGridViewComboBoxColumn colDepartment;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCostGaku;
    }
}

