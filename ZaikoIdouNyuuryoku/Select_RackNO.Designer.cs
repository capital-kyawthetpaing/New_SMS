namespace ZaikoIdouNyuuryoku
{
    partial class Select_RackNO
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
            this.dgvDetail = new CKM_Controls.CKM_GridView();
            this.colRackNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAllowableSu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAdminNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.lblJanCD = new System.Windows.Forms.Label();
            this.lblSKUName = new System.Windows.Forms.Label();
            this.lblSoukoName = new System.Windows.Forms.Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.ckM_Label2);
            this.PanelHeader.Controls.Add(this.lblSKUName);
            this.PanelHeader.Controls.Add(this.lblSoukoName);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Controls.Add(this.lblJanCD);
            this.PanelHeader.Size = new System.Drawing.Size(593, 98);
            this.PanelHeader.Controls.SetChildIndex(this.lblJanCD, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblSoukoName, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblSKUName, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label2, 0);
            // 
            // dgvDetail
            // 
            this.dgvDetail.AllowUserToAddRows = false;
            this.dgvDetail.AllowUserToDeleteRows = false;
            this.dgvDetail.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.dgvDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDetail.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDetail.ColumnHeadersHeight = 25;
            this.dgvDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRackNO,
            this.colAllowableSu,
            this.colAdminNO});
            this.dgvDetail.Enabled = false;
            this.dgvDetail.EnableHeadersVisualStyles = false;
            this.dgvDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvDetail.Location = new System.Drawing.Point(81, 146);
            this.dgvDetail.MultiSelect = false;
            this.dgvDetail.Name = "dgvDetail";
            this.dgvDetail.ReadOnly = true;
            this.dgvDetail.Size = new System.Drawing.Size(243, 219);
            this.dgvDetail.TabIndex = 10;
            this.dgvDetail.UseRowNo = true;
            this.dgvDetail.UseSetting = false;
            this.dgvDetail.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DgvStore_CellPainting);
            this.dgvDetail.DoubleClick += new System.EventHandler(this.DgvStore_DoubleClick);
            this.dgvDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DgvStore_KeyDown);
            // 
            // colRackNO
            // 
            this.colRackNO.DataPropertyName = "RackNO";
            this.colRackNO.HeaderText = "棚番";
            this.colRackNO.Name = "colRackNO";
            this.colRackNO.ReadOnly = true;
            // 
            // colAllowableSu
            // 
            this.colAllowableSu.DataPropertyName = "AllowableSu";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = null;
            this.colAllowableSu.DefaultCellStyle = dataGridViewCellStyle3;
            this.colAllowableSu.HeaderText = "数量";
            this.colAllowableSu.Name = "colAllowableSu";
            this.colAllowableSu.ReadOnly = true;
            // 
            // colAdminNO
            // 
            this.colAdminNO.DataPropertyName = "AdminNO";
            this.colAdminNO.HeaderText = "AdminNO";
            this.colAdminNO.Name = "colAdminNO";
            this.colAdminNO.ReadOnly = true;
            this.colAdminNO.Visible = false;
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
            this.ckM_Label1.Location = new System.Drawing.Point(35, 13);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(40, 12);
            this.ckM_Label1.TabIndex = 48;
            this.ckM_Label1.Text = "SKUCD";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblJanCD
            // 
            this.lblJanCD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblJanCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblJanCD.Location = new System.Drawing.Point(81, 9);
            this.lblJanCD.Name = "lblJanCD";
            this.lblJanCD.Size = new System.Drawing.Size(239, 20);
            this.lblJanCD.TabIndex = 49;
            this.lblJanCD.Text = "XXXXXXXXX1XX3";
            this.lblJanCD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSKUName
            // 
            this.lblSKUName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblSKUName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSKUName.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSKUName.Location = new System.Drawing.Point(81, 32);
            this.lblSKUName.Name = "lblSKUName";
            this.lblSKUName.Size = new System.Drawing.Size(500, 18);
            this.lblSKUName.TabIndex = 683;
            this.lblSKUName.Text = "ＸＸＸＸＸＸＸＸＸ10ＸＸＸＸＸＸＸＸＸ20ＸＸＸＸＸＸＸＸＸ30ＸＸＸＸＸＸＸＸＸ40";
            this.lblSKUName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSoukoName
            // 
            this.lblSoukoName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblSoukoName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSoukoName.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSoukoName.Location = new System.Drawing.Point(81, 62);
            this.lblSoukoName.Name = "lblSoukoName";
            this.lblSoukoName.Size = new System.Drawing.Size(134, 18);
            this.lblSoukoName.TabIndex = 684;
            this.lblSoukoName.Text = "ＸＸＸＸＸＸＸＸＸ10";
            this.lblSoukoName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.ckM_Label2.Location = new System.Drawing.Point(35, 65);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label2.TabIndex = 685;
            this.ckM_Label2.Text = "倉庫";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Select_RackNO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 413);
            this.Controls.Add(this.dgvDetail);
            this.F11Visible = true;
            this.F12Visible = true;
            this.F9Visible = true;
            this.Name = "Select_RackNO";
            this.PanelHeaderHeight = 140;
            this.ProgramName = "棚番選択";
            this.Text = "Select_RackNO";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.dgvDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CKM_Controls.CKM_GridView dgvDetail;
        private CKM_Controls.CKM_Label ckM_Label1;
        private System.Windows.Forms.Label lblJanCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRackNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAllowableSu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdminNO;
        private CKM_Controls.CKM_Label ckM_Label2;
        private System.Windows.Forms.Label lblSKUName;
        private System.Windows.Forms.Label lblSoukoName;
    }
}