namespace Search
{
    partial class Search_Brand
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Search_Brand));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label8 = new CKM_Controls.CKM_Label();
            this.label12 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox3 = new CKM_Controls.CKM_TextBox();
            this.label9 = new CKM_Controls.CKM_Label();
            this.label5 = new CKM_Controls.CKM_Label();
            this.dgvDetail = new CKM_Controls.CKM_GridView();
            this.colBrandCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBrandName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MakerCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MakerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColChangeDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSubF11 = new CKM_Controls.CKM_Button();
            this.ckM_Label1 = new System.Windows.Forms.Label();
            this.radioButton2 = new CKM_Controls.CKM_RadioButton();
            this.radioButton1 = new CKM_Controls.CKM_RadioButton();
            this.ScMaker = new Search.CKM_SearchControl();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.radioButton2);
            this.PanelHeader.Controls.Add(this.radioButton1);
            this.PanelHeader.Controls.Add(this.ScMaker);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Controls.Add(this.ckM_TextBox3);
            this.PanelHeader.Controls.Add(this.btnSubF11);
            this.PanelHeader.Controls.Add(this.label9);
            this.PanelHeader.Controls.Add(this.label8);
            this.PanelHeader.Controls.Add(this.label5);
            this.PanelHeader.Controls.Add(this.label12);
            this.PanelHeader.Size = new System.Drawing.Size(916, 128);
            this.PanelHeader.Controls.SetChildIndex(this.label12, 0);
            this.PanelHeader.Controls.SetChildIndex(this.label5, 0);
            this.PanelHeader.Controls.SetChildIndex(this.label8, 0);
            this.PanelHeader.Controls.SetChildIndex(this.label9, 0);
            this.PanelHeader.Controls.SetChildIndex(this.btnSubF11, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox3, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ScMaker, 0);
            this.PanelHeader.Controls.SetChildIndex(this.radioButton1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.radioButton2, 0);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.DefaultlabelSize = true;
            this.label8.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.label8.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(52, 89);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 12);
            this.label8.TabIndex = 327;
            this.label8.Text = "仕入先";
            this.label8.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.DefaultlabelSize = true;
            this.label12.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.label12.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(39, 35);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 12);
            this.label12.TabIndex = 324;
            this.label12.Text = "表示対象";
            this.label12.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_TextBox3
            // 
            this.ckM_TextBox3.AllowMinus = false;
            this.ckM_TextBox3.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox3.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox3.BorderColor = false;
            this.ckM_TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox3.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox3.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.ckM_TextBox3.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_TextBox3.DecimalPlace = 0;
            this.ckM_TextBox3.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ckM_TextBox3.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.ckM_TextBox3.IntegerPart = 0;
            this.ckM_TextBox3.IsCorrectDate = true;
            this.ckM_TextBox3.isEnterKeyDown = false;
            this.ckM_TextBox3.IsFirstTime = true;
            this.ckM_TextBox3.isMaxLengthErr = false;
            this.ckM_TextBox3.IsNumber = true;
            this.ckM_TextBox3.IsShop = false;
            this.ckM_TextBox3.IsTimemmss = false;
            this.ckM_TextBox3.Length = 15;
            this.ckM_TextBox3.Location = new System.Drawing.Point(103, 58);
            this.ckM_TextBox3.MaxLength = 15;
            this.ckM_TextBox3.MoveNext = true;
            this.ckM_TextBox3.Name = "ckM_TextBox3";
            this.ckM_TextBox3.Size = new System.Drawing.Size(195, 19);
            this.ckM_TextBox3.TabIndex = 3;
            this.ckM_TextBox3.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.ckM_TextBox3.UseColorSizMode = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.DefaultlabelSize = true;
            this.label9.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.label9.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(26, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 12);
            this.label9.TabIndex = 323;
            this.label9.Text = "ブランド名";
            this.label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.label5.Location = new System.Drawing.Point(52, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 12);
            this.label5.TabIndex = 322;
            this.label5.Text = "基準日";
            this.label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvDetail
            // 
            this.dgvDetail.AllowUserToAddRows = false;
            this.dgvDetail.AllowUserToDeleteRows = false;
            this.dgvDetail.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.dgvDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDetail.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvDetail.CheckCol = ((System.Collections.ArrayList)(resources.GetObject("dgvDetail.CheckCol")));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDetail.ColumnHeadersHeight = 25;
            this.dgvDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBrandCD,
            this.colBrandName,
            this.MakerCD,
            this.MakerName,
            this.ColChangeDate});
            this.dgvDetail.EnableHeadersVisualStyles = false;
            this.dgvDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvDetail.Location = new System.Drawing.Point(18, 175);
            this.dgvDetail.MultiSelect = false;
            this.dgvDetail.Name = "dgvDetail";
            this.dgvDetail.ReadOnly = true;
            this.dgvDetail.RowHeadersWidth = 35;
            this.dgvDetail.RowHeight_ = 20;
            this.dgvDetail.RowTemplate.Height = 20;
            this.dgvDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetail.Size = new System.Drawing.Size(876, 290);
            this.dgvDetail.TabIndex = 1;
            this.dgvDetail.UseRowNo = true;
            this.dgvDetail.UseSetting = true;
            this.dgvDetail.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvDetail_Paint);
            this.dgvDetail.DoubleClick += new System.EventHandler(this.DgvDetail_DoubleClick);
            this.dgvDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DgvDetail_KeyDown);
            // 
            // colBrandCD
            // 
            this.colBrandCD.DataPropertyName = "BrandCD";
            this.colBrandCD.HeaderText = "CD";
            this.colBrandCD.Name = "colBrandCD";
            this.colBrandCD.ReadOnly = true;
            this.colBrandCD.Width = 70;
            // 
            // colBrandName
            // 
            this.colBrandName.DataPropertyName = "BrandName";
            this.colBrandName.HeaderText = "ブランド名";
            this.colBrandName.Name = "colBrandName";
            this.colBrandName.ReadOnly = true;
            this.colBrandName.Width = 250;
            // 
            // MakerCD
            // 
            this.MakerCD.DataPropertyName = "MakerCD";
            this.MakerCD.HeaderText = "メーカー ";
            this.MakerCD.Name = "MakerCD";
            this.MakerCD.ReadOnly = true;
            // 
            // MakerName
            // 
            this.MakerName.DataPropertyName = "MakerName";
            this.MakerName.HeaderText = " ";
            this.MakerName.Name = "MakerName";
            this.MakerName.ReadOnly = true;
            this.MakerName.Width = 300;
            // 
            // ColChangeDate
            // 
            this.ColChangeDate.DataPropertyName = "ChangeDate";
            this.ColChangeDate.HeaderText = "改定日";
            this.ColChangeDate.Name = "ColChangeDate";
            this.ColChangeDate.ReadOnly = true;
            this.ColChangeDate.Width = 90;
            // 
            // btnSubF11
            // 
            this.btnSubF11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnSubF11.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnSubF11.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubF11.DefaultBtnSize = true;
            this.btnSubF11.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSubF11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubF11.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnSubF11.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnSubF11.Location = new System.Drawing.Point(776, 81);
            this.btnSubF11.Margin = new System.Windows.Forms.Padding(1);
            this.btnSubF11.Name = "btnSubF11";
            this.btnSubF11.Size = new System.Drawing.Size(118, 28);
            this.btnSubF11.TabIndex = 5;
            this.btnSubF11.Text = "表示(F11)";
            this.btnSubF11.UseVisualStyleBackColor = false;
            this.btnSubF11.Click += new System.EventHandler(this.BtnSubF11_Click);
            // 
            // ckM_Label1
            // 
            this.ckM_Label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.ckM_Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_Label1.Location = new System.Drawing.Point(103, 3);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(84, 20);
            this.ckM_Label1.TabIndex = 0;
            this.ckM_Label1.Text = "YYYY/MM/DD";
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.radioButton2.Location = new System.Drawing.Point(171, 32);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(49, 16);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "履歴";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.radioButton1.Location = new System.Drawing.Point(103, 33);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(62, 16);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "基準日";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // ScMaker
            // 
            this.ScMaker.AutoSize = true;
            this.ScMaker.ChangeDate = "";
            this.ScMaker.ChangeDateWidth = 100;
            this.ScMaker.Code = "";
            this.ScMaker.CodeWidth = 100;
            this.ScMaker.CodeWidth1 = 100;
            this.ScMaker.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScMaker.DataCheck = false;
            this.ScMaker.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScMaker.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScMaker.IsCopy = false;
            this.ScMaker.LabelText = "";
            this.ScMaker.LabelVisible = true;
            this.ScMaker.Location = new System.Drawing.Point(103, 80);
            this.ScMaker.Margin = new System.Windows.Forms.Padding(0);
            this.ScMaker.Name = "ScMaker";
            this.ScMaker.NameWidth = 310;
            this.ScMaker.SearchEnable = true;
            this.ScMaker.Size = new System.Drawing.Size(444, 27);
            this.ScMaker.Stype = Search.CKM_SearchControl.SearchType.仕入先;
            this.ScMaker.TabIndex = 4;
            this.ScMaker.test = null;
            this.ScMaker.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScMaker.UseChangeDate = false;
            this.ScMaker.Value1 = null;
            this.ScMaker.Value2 = null;
            this.ScMaker.Value3 = null;
            // 
            // Search_Brand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 590);
            this.Controls.Add(this.dgvDetail);
            this.F11Visible = true;
            this.F12Visible = true;
            this.F9Visible = true;
            this.Name = "Search_Brand";
            this.PanelHeaderHeight = 170;
            this.ProgramName = "ブランド検索";
            this.Text = "Search_Staff";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.dgvDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private CKM_Controls.CKM_Label label8;
        private CKM_Controls.CKM_Label label12;
        private CKM_Controls.CKM_TextBox ckM_TextBox3;
        private CKM_Controls.CKM_Label label9;
        private CKM_Controls.CKM_Label label5;
        private CKM_Controls.CKM_GridView dgvDetail;
        private CKM_Controls.CKM_Button btnSubF11;
        private System.Windows.Forms.Label ckM_Label1;
        private CKM_SearchControl ScMaker;
        private CKM_Controls.CKM_RadioButton radioButton2;
        private CKM_Controls.CKM_RadioButton radioButton1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBrandCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBrandName;
        private System.Windows.Forms.DataGridViewTextBoxColumn MakerCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn MakerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColChangeDate;
    }
}