namespace MiNyuukinKakuninShoukai
{
    partial class FrmDetail
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvDetail = new CKM_Controls.CKM_GridView();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.lblJanCD = new System.Windows.Forms.Label();
            this.lblSKUName = new System.Windows.Forms.Label();
            this.lblSoukoName = new System.Windows.Forms.Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DenominationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CollectDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CollectAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).BeginInit();
            this.SuspendLayout();
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
            this.DenominationName,
            this.CollectDate,
            this.CollectAmount});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDetail.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDetail.Enabled = false;
            this.dgvDetail.EnableHeadersVisualStyles = false;
            this.dgvDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvDetail.Location = new System.Drawing.Point(27, 12);
            this.dgvDetail.MultiSelect = false;
            this.dgvDetail.Name = "dgvDetail";
            this.dgvDetail.ReadOnly = true;
            this.dgvDetail.Size = new System.Drawing.Size(393, 206);
            this.dgvDetail.TabIndex = 10;
            this.dgvDetail.UseRowNo = true;
            this.dgvDetail.UseSetting = false;
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
            this.lblJanCD.Size = new System.Drawing.Size(100, 20);
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
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(318, 218);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 18);
            this.label1.TabIndex = 685;
            this.label1.Text = " ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DenominationName
            // 
            this.DenominationName.DataPropertyName = "DenominationName";
            this.DenominationName.HeaderText = "入金方法";
            this.DenominationName.Name = "DenominationName";
            this.DenominationName.ReadOnly = true;
            this.DenominationName.Width = 150;
            // 
            // CollectDate
            // 
            this.CollectDate.DataPropertyName = "CollectDate";
            this.CollectDate.HeaderText = "入金日";
            this.CollectDate.Name = "CollectDate";
            this.CollectDate.ReadOnly = true;
            // 
            // CollectAmount
            // 
            this.CollectAmount.DataPropertyName = "CollectAmount";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = null;
            this.CollectAmount.DefaultCellStyle = dataGridViewCellStyle3;
            this.CollectAmount.HeaderText = "入金額";
            this.CollectAmount.Name = "CollectAmount";
            this.CollectAmount.ReadOnly = true;
            // 
            // FrmDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.ClientSize = new System.Drawing.Size(453, 245);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvDetail);
            this.Name = "FrmDetail";
            this.Text = "MiNyuukinKakuninShoukai";
            this.Load += new System.EventHandler(this.Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CKM_Controls.CKM_GridView dgvDetail;
        private CKM_Controls.CKM_Label ckM_Label1;
        private System.Windows.Forms.Label lblJanCD;
        private CKM_Controls.CKM_Label ckM_Label2;
        private System.Windows.Forms.Label lblSKUName;
        private System.Windows.Forms.Label lblSoukoName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DenominationName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CollectDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn CollectAmount;
    }
}