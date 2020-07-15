namespace Search
{
    partial class frmSearch_SKU
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
            this.lbl = new CKM_Controls.CKMShop_Label();
            this.lblJanCD = new CKM_Controls.CKMShop_Label();
            this.GvMultiSKU = new CKM_Controls.CKMShop_GridView();
            this.colSKUCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSizeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAdminNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ckmShop_Label1 = new CKM_Controls.CKMShop_Label();
            this.ckmShop_Label2 = new CKM_Controls.CKMShop_Label();
            ((System.ComponentModel.ISupportInitialize)(this.GvMultiSKU)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lbl.BackColor = System.Drawing.Color.Transparent;
            this.lbl.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.lbl.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.lbl.FontBold = true;
            this.lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lbl.Location = new System.Drawing.Point(31, 64);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(110, 35);
            this.lbl.TabIndex = 3;
            this.lbl.Text = "JanCD";
            this.lbl.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.DarkGreen;
            this.lbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblJanCD
            // 
            this.lblJanCD.AutoSize = true;
            this.lblJanCD.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.White;
            this.lblJanCD.BackColor = System.Drawing.Color.White;
            this.lblJanCD.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.lblJanCD.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.lblJanCD.FontBold = true;
            this.lblJanCD.ForeColor = System.Drawing.Color.Black;
            this.lblJanCD.Location = new System.Drawing.Point(146, 64);
            this.lblJanCD.Name = "lblJanCD";
            this.lblJanCD.Size = new System.Drawing.Size(281, 35);
            this.lblJanCD.TabIndex = 4;
            this.lblJanCD.Text = "XXXXXXXXXXXX13";
            this.lblJanCD.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblJanCD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GvMultiSKU
            // 
            this.GvMultiSKU.AllowUserToAddRows = false;
            this.GvMultiSKU.AllowUserToDeleteRows = false;
            this.GvMultiSKU.AllowUserToResizeRows = false;
            this.GvMultiSKU.AlterBackColor = CKM_Controls.CKMShop_GridView.AltBackcolor.White;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.GvMultiSKU.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.GvMultiSKU.BackgroundColor = System.Drawing.Color.White;
            this.GvMultiSKU.BackgroungColor = CKM_Controls.CKMShop_GridView.DBackcolor.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS Gothic", 26F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GvMultiSKU.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.GvMultiSKU.ColumnHeadersHeight = 30;
            this.GvMultiSKU.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.GvMultiSKU.ColumnHeadersVisible = false;
            this.GvMultiSKU.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSKUCD,
            this.colSKUName,
            this.colSizeName,
            this.colAdminNO,
            this.colColorName});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("MS Gothic", 26F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.GvMultiSKU.DefaultCellStyle = dataGridViewCellStyle3;
            this.GvMultiSKU.DGVback = CKM_Controls.CKMShop_GridView.DGVBackcolor.White;
            this.GvMultiSKU.EnableHeadersVisualStyles = false;
            this.GvMultiSKU.Font = new System.Drawing.Font("MS Gothic", 26F);
            this.GvMultiSKU.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GvMultiSKU.GVFontstyle = CKM_Controls.CKMShop_GridView.FontStyle_.Regular;
            this.GvMultiSKU.HeaderHeight_ = 30;
            this.GvMultiSKU.HeaderVisible = false;
            this.GvMultiSKU.Height_ = 200;
            this.GvMultiSKU.Location = new System.Drawing.Point(37, 163);
            this.GvMultiSKU.Name = "GvMultiSKU";
            this.GvMultiSKU.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.GvMultiSKU.RowHeight_ = 42;
            this.GvMultiSKU.RowTemplate.Height = 42;
            this.GvMultiSKU.ShopFontSize = CKM_Controls.CKMShop_GridView.Font_.Medium;
            this.GvMultiSKU.Size = new System.Drawing.Size(1500, 520);
            this.GvMultiSKU.TabIndex = 6;
            this.GvMultiSKU.UseRowNo = true;
            this.GvMultiSKU.UseSetting = true;
            this.GvMultiSKU.Width_ = 1500;
            this.GvMultiSKU.DoubleClick += new System.EventHandler(this.GvMultiSKU_DoubleClick);
            // 
            // colSKUCD
            // 
            this.colSKUCD.DataPropertyName = "SKUCD";
            this.colSKUCD.HeaderText = "SKUCD";
            this.colSKUCD.Name = "colSKUCD";
            this.colSKUCD.ReadOnly = true;
            this.colSKUCD.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colSKUCD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSKUCD.Width = 350;
            // 
            // colSKUName
            // 
            this.colSKUName.DataPropertyName = "SKUName";
            this.colSKUName.HeaderText = "商品名";
            this.colSKUName.Name = "colSKUName";
            this.colSKUName.ReadOnly = true;
            this.colSKUName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colSKUName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSKUName.Width = 1100;
            // 
            // colSizeName
            // 
            this.colSizeName.DataPropertyName = "SizeName";
            this.colSizeName.HeaderText = "SizeName";
            this.colSizeName.Name = "colSizeName";
            this.colSizeName.ReadOnly = true;
            this.colSizeName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colSizeName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSizeName.Visible = false;
            // 
            // colAdminNO
            // 
            this.colAdminNO.DataPropertyName = "AdminNO";
            this.colAdminNO.HeaderText = "AdminNO";
            this.colAdminNO.Name = "colAdminNO";
            this.colAdminNO.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colAdminNO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colAdminNO.Visible = false;
            // 
            // colColorName
            // 
            this.colColorName.DataPropertyName = "ColorName";
            this.colColorName.HeaderText = "ColorName";
            this.colColorName.Name = "colColorName";
            this.colColorName.ReadOnly = true;
            this.colColorName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colColorName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colColorName.Visible = false;
            // 
            // ckmShop_Label1
            // 
            this.ckmShop_Label1.AutoSize = true;
            this.ckmShop_Label1.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label1.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label1.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label1.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label1.FontBold = true;
            this.ckmShop_Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label1.Location = new System.Drawing.Point(71, 125);
            this.ckmShop_Label1.Name = "ckmShop_Label1";
            this.ckmShop_Label1.Size = new System.Drawing.Size(110, 35);
            this.ckmShop_Label1.TabIndex = 7;
            this.ckmShop_Label1.Text = "SKUCD";
            this.ckmShop_Label1.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.DarkGreen;
            this.ckmShop_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckmShop_Label2
            // 
            this.ckmShop_Label2.AutoSize = true;
            this.ckmShop_Label2.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label2.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label2.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label2.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.ckmShop_Label2.FontBold = true;
            this.ckmShop_Label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label2.Location = new System.Drawing.Point(418, 125);
            this.ckmShop_Label2.Name = "ckmShop_Label2";
            this.ckmShop_Label2.Size = new System.Drawing.Size(126, 35);
            this.ckmShop_Label2.TabIndex = 8;
            this.ckmShop_Label2.Text = "商品名";
            this.ckmShop_Label2.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.DarkGreen;
            this.ckmShop_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmSearch_SKU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 761);
            this.Controls.Add(this.ckmShop_Label2);
            this.Controls.Add(this.ckmShop_Label1);
            this.Controls.Add(this.GvMultiSKU);
            this.Controls.Add(this.lblJanCD);
            this.Controls.Add(this.lbl);
            this.Name = "frmSearch_SKU";
            this.Text = "SKU選択";
            this.Controls.SetChildIndex(this.lbl, 0);
            this.Controls.SetChildIndex(this.lblJanCD, 0);
            this.Controls.SetChildIndex(this.GvMultiSKU, 0);
            this.Controls.SetChildIndex(this.ckmShop_Label1, 0);
            this.Controls.SetChildIndex(this.ckmShop_Label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.GvMultiSKU)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CKM_Controls.CKMShop_Label lbl;
        private CKM_Controls.CKMShop_Label lblJanCD;
        private CKM_Controls.CKMShop_GridView GvMultiSKU;
        private CKM_Controls.CKMShop_Label ckmShop_Label1;
        private CKM_Controls.CKMShop_Label ckmShop_Label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSizeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdminNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColorName;
    }
}