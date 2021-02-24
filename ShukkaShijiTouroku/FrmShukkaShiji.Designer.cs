namespace ShukkaShijiTouroku
{
    partial class FrmShukkaShiji
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmShukkaShiji));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.GvDetail = new CKM_Controls.CKM_GridView();
            this.colJuchuNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJANCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSizeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJuchuuSuu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArrivePlanDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDirectFLG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblJyuchuNo = new System.Windows.Forms.Label();
            this.lblDeliveryName = new System.Windows.Forms.Label();
            this.lblDeliveryAddress1 = new System.Windows.Forms.Label();
            this.lblDecidedDeliveryDate = new System.Windows.Forms.Label();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.lblDecidedDeliveryDate);
            this.PanelHeader.Controls.Add(this.lblDeliveryAddress1);
            this.PanelHeader.Controls.Add(this.lblDeliveryName);
            this.PanelHeader.Controls.Add(this.lblJyuchuNo);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Controls.Add(this.ckM_Label3);
            this.PanelHeader.Size = new System.Drawing.Size(1152, 98);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label3, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblJyuchuNo, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblDeliveryName, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblDeliveryAddress1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblDecidedDeliveryDate, 0);
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
            this.ckM_Label3.Location = new System.Drawing.Point(18, 69);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label3.TabIndex = 51;
            this.ckM_Label3.Text = "着指定日";
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
            this.GvDetail.CheckCol = ((System.Collections.ArrayList)(resources.GetObject("GvDetail.CheckCol")));
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
            this.colJuchuNO,
            this.colSKUCD,
            this.colJANCD,
            this.colSKUName,
            this.colColorName,
            this.colSizeName,
            this.colJuchuuSuu,
            this.colArrivePlanDate,
            this.colDirectFLG});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.GvDetail.DefaultCellStyle = dataGridViewCellStyle8;
            this.GvDetail.EnableHeadersVisualStyles = false;
            this.GvDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GvDetail.Location = new System.Drawing.Point(8, 147);
            this.GvDetail.Name = "GvDetail";
            this.GvDetail.ReadOnly = true;
            this.GvDetail.RowHeight_ = 20;
            this.GvDetail.RowTemplate.Height = 20;
            this.GvDetail.Size = new System.Drawing.Size(1133, 322);
            this.GvDetail.TabIndex = 20;
            this.GvDetail.UseRowNo = true;
            this.GvDetail.UseSetting = true;
            this.GvDetail.DoubleClick += new System.EventHandler(this.GvDetail_DoubleClick);
            this.GvDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DgvDetail_KeyDown);
            // 
            // colJuchuNO
            // 
            this.colJuchuNO.DataPropertyName = "DM_Number";
            this.colJuchuNO.HeaderText = "受注番号";
            this.colJuchuNO.Name = "colJuchuNO";
            this.colJuchuNO.ReadOnly = true;
            this.colJuchuNO.Width = 80;
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
            // colJuchuuSuu
            // 
            this.colJuchuuSuu.DataPropertyName = "JuchuuSuu";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N0";
            dataGridViewCellStyle5.NullValue = null;
            this.colJuchuuSuu.DefaultCellStyle = dataGridViewCellStyle5;
            this.colJuchuuSuu.HeaderText = "受注数";
            this.colJuchuuSuu.Name = "colJuchuuSuu";
            this.colJuchuuSuu.ReadOnly = true;
            this.colJuchuuSuu.Width = 80;
            // 
            // colArrivePlanDate
            // 
            this.colArrivePlanDate.DataPropertyName = "RackNO";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colArrivePlanDate.DefaultCellStyle = dataGridViewCellStyle6;
            this.colArrivePlanDate.HeaderText = "棚番";
            this.colArrivePlanDate.Name = "colArrivePlanDate";
            this.colArrivePlanDate.ReadOnly = true;
            // 
            // colDirectFLG
            // 
            this.colDirectFLG.DataPropertyName = "ShippingPossibleSu";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N0";
            dataGridViewCellStyle7.NullValue = null;
            this.colDirectFLG.DefaultCellStyle = dataGridViewCellStyle7;
            this.colDirectFLG.HeaderText = "可能数";
            this.colDirectFLG.Name = "colDirectFLG";
            this.colDirectFLG.ReadOnly = true;
            this.colDirectFLG.Width = 80;
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
            // lblDeliveryName
            // 
            this.lblDeliveryName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblDeliveryName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDeliveryName.Location = new System.Drawing.Point(81, 38);
            this.lblDeliveryName.Name = "lblDeliveryName";
            this.lblDeliveryName.Size = new System.Drawing.Size(270, 20);
            this.lblDeliveryName.TabIndex = 53;
            this.lblDeliveryName.Text = "出荷先名ＸＸＸＸＸ10ＸＸＸＸＸＸＸＸＸ20";
            this.lblDeliveryName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDeliveryAddress1
            // 
            this.lblDeliveryAddress1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblDeliveryAddress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDeliveryAddress1.Location = new System.Drawing.Point(357, 38);
            this.lblDeliveryAddress1.Name = "lblDeliveryAddress1";
            this.lblDeliveryAddress1.Size = new System.Drawing.Size(517, 20);
            this.lblDeliveryAddress1.TabIndex = 54;
            this.lblDeliveryAddress1.Text = "出荷先住所ＸＸＸＸ10ＸＸＸＸＸＸＸＸＸ20ＸＸＸＸＸＸＸＸＸ30ＸＸＸＸＸＸＸＸＸ40";
            this.lblDeliveryAddress1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDecidedDeliveryDate
            // 
            this.lblDecidedDeliveryDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblDecidedDeliveryDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDecidedDeliveryDate.Location = new System.Drawing.Point(81, 65);
            this.lblDecidedDeliveryDate.Name = "lblDecidedDeliveryDate";
            this.lblDecidedDeliveryDate.Size = new System.Drawing.Size(100, 20);
            this.lblDecidedDeliveryDate.TabIndex = 55;
            this.lblDecidedDeliveryDate.Text = "9999/99/99 99時";
            this.lblDecidedDeliveryDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrmShukkaShiji
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1152, 517);
            this.Controls.Add(this.GvDetail);
            this.F11Visible = true;
            this.F12Visible = true;
            this.F9Visible = true;
            this.Name = "FrmShukkaShiji";
            this.PanelHeaderHeight = 140;
            this.ProgramName = "出荷指示登録";
            this.Text = "ShukkaShijiTouroku";
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
        private System.Windows.Forms.Label lblDeliveryName;
        private System.Windows.Forms.Label lblJyuchuNo;
        private System.Windows.Forms.Label lblDecidedDeliveryDate;
        private System.Windows.Forms.Label lblDeliveryAddress1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJuchuNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJANCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSizeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJuchuuSuu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArrivePlanDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDirectFLG;
    }
}

