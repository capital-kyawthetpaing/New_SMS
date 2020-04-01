namespace Search
{
    partial class FrmJuchuu
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.GvDetail = new CKM_Controls.CKM_GridView();
            this.colSKUCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJANCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSizeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArrivePlanDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDirectFLG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJuchuuSuu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblJyuchuNo = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.lblCustomer);
            this.PanelHeader.Controls.Add(this.lblJyuchuNo);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Controls.Add(this.ckM_Label3);
            this.PanelHeader.Size = new System.Drawing.Size(1062, 78);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label3, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblJyuchuNo, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblCustomer, 0);
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
            this.ckM_Label1.Location = new System.Drawing.Point(18, 15);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label1.TabIndex = 2;
            this.ckM_Label1.Text = "受注番号";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label3.Location = new System.Drawing.Point(31, 45);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label3.TabIndex = 51;
            this.ckM_Label3.Text = "会員名";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GvDetail
            // 
            this.GvDetail.AllowUserToAddRows = false;
            this.GvDetail.AllowUserToDeleteRows = false;
            this.GvDetail.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.GvDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.GvDetail.AutoGenerateColumns = false;
            this.GvDetail.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GvDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.GvDetail.ColumnHeadersHeight = 25;
            this.GvDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSKUCD,
            this.colJANCD,
            this.colSKUName,
            this.colColorName,
            this.colSizeName,
            this.colArrivePlanDate,
            this.colDirectFLG,
            this.colJuchuuSuu});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.GvDetail.DefaultCellStyle = dataGridViewCellStyle7;
            this.GvDetail.EnableHeadersVisualStyles = false;
            this.GvDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GvDetail.Location = new System.Drawing.Point(9, 126);
            this.GvDetail.Name = "GvDetail";
            this.GvDetail.ReadOnly = true;
            this.GvDetail.Size = new System.Drawing.Size(1018, 345);
            this.GvDetail.TabIndex = 20;
            this.GvDetail.UseRowNo = true;
            this.GvDetail.UseSetting = true;
            this.GvDetail.DoubleClick += new System.EventHandler(this.GvDetail_DoubleClick);
            this.GvDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DgvDetail_KeyDown);
            // 
            // colSKUCD
            // 
            this.colSKUCD.DataPropertyName = "SKUCD";
            this.colSKUCD.HeaderText = "SKUCD";
            this.colSKUCD.Name = "colSKUCD";
            this.colSKUCD.ReadOnly = true;
            this.colSKUCD.Width = 120;
            // 
            // colJANCD
            // 
            this.colJANCD.DataPropertyName = "JANCD";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colJANCD.DefaultCellStyle = dataGridViewCellStyle3;
            this.colJANCD.HeaderText = "JANCD";
            this.colJANCD.Name = "colJANCD";
            this.colJANCD.ReadOnly = true;
            this.colJANCD.Width = 90;
            // 
            // colSKUName
            // 
            this.colSKUName.DataPropertyName = "SKUName";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colSKUName.DefaultCellStyle = dataGridViewCellStyle4;
            this.colSKUName.HeaderText = "商品名";
            this.colSKUName.Name = "colSKUName";
            this.colSKUName.ReadOnly = true;
            this.colSKUName.Width = 250;
            // 
            // colColorName
            // 
            this.colColorName.DataPropertyName = "ColorName";
            this.colColorName.HeaderText = "カラー";
            this.colColorName.Name = "colColorName";
            this.colColorName.ReadOnly = true;
            this.colColorName.Width = 150;
            // 
            // colSizeName
            // 
            this.colSizeName.DataPropertyName = "SizeName";
            this.colSizeName.HeaderText = "サイズ";
            this.colSizeName.Name = "colSizeName";
            this.colSizeName.ReadOnly = true;
            this.colSizeName.Width = 150;
            // 
            // colArrivePlanDate
            // 
            this.colArrivePlanDate.DataPropertyName = "ArrivePlanDate";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colArrivePlanDate.DefaultCellStyle = dataGridViewCellStyle5;
            this.colArrivePlanDate.HeaderText = "入荷予定日";
            this.colArrivePlanDate.Name = "colArrivePlanDate";
            this.colArrivePlanDate.ReadOnly = true;
            // 
            // colDirectFLG
            // 
            this.colDirectFLG.DataPropertyName = "DirectFLG";
            this.colDirectFLG.HeaderText = "直送";
            this.colDirectFLG.Name = "colDirectFLG";
            this.colDirectFLG.ReadOnly = true;
            this.colDirectFLG.Width = 45;
            // 
            // colJuchuuSuu
            // 
            this.colJuchuuSuu.DataPropertyName = "JuchuuSuu";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            dataGridViewCellStyle6.NullValue = null;
            this.colJuchuuSuu.DefaultCellStyle = dataGridViewCellStyle6;
            this.colJuchuuSuu.HeaderText = "受注数";
            this.colJuchuuSuu.Name = "colJuchuuSuu";
            this.colJuchuuSuu.ReadOnly = true;
            this.colJuchuuSuu.Width = 70;
            // 
            // lblJyuchuNo
            // 
            this.lblJyuchuNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblJyuchuNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblJyuchuNo.Location = new System.Drawing.Point(81, 11);
            this.lblJyuchuNo.Name = "lblJyuchuNo";
            this.lblJyuchuNo.Size = new System.Drawing.Size(100, 20);
            this.lblJyuchuNo.TabIndex = 52;
            this.lblJyuchuNo.Text = "XXXXXXXXX1XX3";
            this.lblJyuchuNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCustomer
            // 
            this.lblCustomer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblCustomer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCustomer.Location = new System.Drawing.Point(81, 41);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(270, 20);
            this.lblCustomer.TabIndex = 53;
            this.lblCustomer.Text = "ＸＸＸＸＸＸＸＸＸ10ＸＸＸＸＸＸＸＸＸ10";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrmJuchuu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 517);
            this.Controls.Add(this.GvDetail);
            this.F11Visible = true;
            this.F9Visible = true;
            this.Name = "FrmJuchuu";
            this.PanelHeaderHeight = 120;
            this.ProgramName = "受注照会";
            this.Text = "Search_JuchuuNO";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.GvDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_GridView GvDetail;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblJyuchuNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJANCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSizeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArrivePlanDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDirectFLG;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJuchuuSuu;
    }
}

