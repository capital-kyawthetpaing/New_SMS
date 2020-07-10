namespace Search
{
    partial class Search_Location
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
            this.btnSubF11 = new CKM_Controls.CKM_Button();
            this.GvDetail = new CKM_Controls.CKM_GridView();
            this.colTanaCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CboFromSoukoCD = new CKM_Controls.CKM_ComboBox();
            this.lblSoukoCD = new System.Windows.Forms.Label();
            this.ckM_Label1 = new System.Windows.Forms.Label();
            this.label5 = new CKM_Controls.CKM_Label();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Controls.Add(this.label5);
            this.PanelHeader.Controls.Add(this.CboFromSoukoCD);
            this.PanelHeader.Controls.Add(this.lblSoukoCD);
            this.PanelHeader.Controls.Add(this.btnSubF11);
            this.PanelHeader.Size = new System.Drawing.Size(534, 88);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.Controls.SetChildIndex(this.btnSubF11, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblSoukoCD, 0);
            this.PanelHeader.Controls.SetChildIndex(this.CboFromSoukoCD, 0);
            this.PanelHeader.Controls.SetChildIndex(this.label5, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
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
            this.btnSubF11.Location = new System.Drawing.Point(402, 45);
            this.btnSubF11.Margin = new System.Windows.Forms.Padding(1);
            this.btnSubF11.Name = "btnSubF11";
            this.btnSubF11.Size = new System.Drawing.Size(115, 28);
            this.btnSubF11.TabIndex = 15;
            this.btnSubF11.Text = "表示(F11)";
            this.btnSubF11.UseVisualStyleBackColor = false;
            this.btnSubF11.Click += new System.EventHandler(this.BtnF11_Click);
            // 
            // GvDetail
            // 
            this.GvDetail.AllowUserToAddRows = false;
            this.GvDetail.AllowUserToDeleteRows = false;
            this.GvDetail.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.GvDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.GvDetail.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GvDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.GvDetail.ColumnHeadersHeight = 25;
            this.GvDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTanaCD});
            this.GvDetail.EnableHeadersVisualStyles = false;
            this.GvDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GvDetail.Location = new System.Drawing.Point(106, 140);
            this.GvDetail.Name = "GvDetail";
            this.GvDetail.ReadOnly = true;
            this.GvDetail.RowHeight_ = 20;
            this.GvDetail.RowTemplate.Height = 20;
            this.GvDetail.Size = new System.Drawing.Size(143, 350);
            this.GvDetail.TabIndex = 20;
            this.GvDetail.UseRowNo = true;
            this.GvDetail.UseSetting = true;
            this.GvDetail.DoubleClick += new System.EventHandler(this.GvDetail_DoubleClick);
            this.GvDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DgvDetail_KeyDown);
            // 
            // colTanaCD
            // 
            this.colTanaCD.DataPropertyName = "TanaCD";
            this.colTanaCD.HeaderText = "棚番号";
            this.colTanaCD.Name = "colTanaCD";
            this.colTanaCD.ReadOnly = true;
            this.colTanaCD.Width = 80;
            // 
            // CboFromSoukoCD
            // 
            this.CboFromSoukoCD.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.CboFromSoukoCD.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CboFromSoukoCD.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.入荷倉庫;
            this.CboFromSoukoCD.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.CboFromSoukoCD.Flag = 0;
            this.CboFromSoukoCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.CboFromSoukoCD.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.CboFromSoukoCD.Length = 30;
            this.CboFromSoukoCD.Location = new System.Drawing.Point(106, 52);
            this.CboFromSoukoCD.MaxLength = 15;
            this.CboFromSoukoCD.MoveNext = true;
            this.CboFromSoukoCD.Name = "CboFromSoukoCD";
            this.CboFromSoukoCD.Size = new System.Drawing.Size(265, 20);
            this.CboFromSoukoCD.TabIndex = 3;
            // 
            // lblSoukoCD
            // 
            this.lblSoukoCD.AutoSize = true;
            this.lblSoukoCD.BackColor = System.Drawing.Color.Transparent;
            this.lblSoukoCD.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSoukoCD.Location = new System.Drawing.Point(68, 56);
            this.lblSoukoCD.Name = "lblSoukoCD";
            this.lblSoukoCD.Size = new System.Drawing.Size(31, 12);
            this.lblSoukoCD.TabIndex = 743;
            this.lblSoukoCD.Text = "倉庫";
            this.lblSoukoCD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label1
            // 
            this.ckM_Label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.ckM_Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_Label1.Location = new System.Drawing.Point(106, 18);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(84, 20);
            this.ckM_Label1.TabIndex = 745;
            this.ckM_Label1.Text = "YYYY/MM/DD";
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.DefaultlabelSize = true;
            this.label5.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.label5.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(55, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 12);
            this.label5.TabIndex = 744;
            this.label5.Text = "基準日";
            this.label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Search_Location
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 531);
            this.Controls.Add(this.GvDetail);
            this.F11Visible = true;
            this.F12Visible = true;
            this.F9Visible = true;
            this.Name = "Search_Location";
            this.PanelHeaderHeight = 130;
            this.ProgramName = "棚検索";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search_Location";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.GvDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private CKM_Controls.CKM_Button btnSubF11;
        private CKM_Controls.CKM_GridView GvDetail;
        private CKM_Controls.CKM_ComboBox CboFromSoukoCD;
        private System.Windows.Forms.Label lblSoukoCD;
        private System.Windows.Forms.Label ckM_Label1;
        private CKM_Controls.CKM_Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTanaCD;
    }
}

