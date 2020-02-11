namespace Search
{
    partial class FrmSearch_Carrier
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
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.lblChangeDate = new System.Windows.Forms.Label();
            this.ckM_Label11 = new CKM_Controls.CKM_Label();
            this.ckM_Label12 = new CKM_Controls.CKM_Label();
            this.ckM_Label14 = new CKM_Controls.CKM_Label();
            this.radioButton2 = new CKM_Controls.CKM_RadioButton();
            this.txtShippingName = new CKM_Controls.CKM_TextBox();
            this.ckM_Label8 = new CKM_Controls.CKM_Label();
            this.txtShippingFrom = new CKM_Controls.CKM_TextBox();
            this.txtShippingTo = new CKM_Controls.CKM_TextBox();
            this.radioButton1 = new CKM_Controls.CKM_RadioButton();
            this.F11Show = new CKM_Controls.CKM_Button();
            this.gvShipping = new CKM_Controls.CKM_GridView();
            this.colCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvShipping)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.F11Show);
            this.PanelHeader.Controls.Add(this.ckM_Label10);
            this.PanelHeader.Controls.Add(this.lblChangeDate);
            this.PanelHeader.Controls.Add(this.ckM_Label11);
            this.PanelHeader.Controls.Add(this.ckM_Label12);
            this.PanelHeader.Controls.Add(this.ckM_Label14);
            this.PanelHeader.Controls.Add(this.radioButton2);
            this.PanelHeader.Controls.Add(this.txtShippingName);
            this.PanelHeader.Controls.Add(this.ckM_Label8);
            this.PanelHeader.Controls.Add(this.txtShippingFrom);
            this.PanelHeader.Controls.Add(this.txtShippingTo);
            this.PanelHeader.Controls.Add(this.radioButton1);
            this.PanelHeader.Size = new System.Drawing.Size(838, 138);
            this.PanelHeader.Controls.SetChildIndex(this.radioButton1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtShippingTo, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtShippingFrom, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label8, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtShippingName, 0);
            this.PanelHeader.Controls.SetChildIndex(this.radioButton2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label14, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label12, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label11, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblChangeDate, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label10, 0);
            this.PanelHeader.Controls.SetChildIndex(this.F11Show, 0);
            // 
            // ckM_Label10
            // 
            this.ckM_Label10.AutoSize = true;
            this.ckM_Label10.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label10.DefaultlabelSize = true;
            this.ckM_Label10.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label10.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label10.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label10.Location = new System.Drawing.Point(54, 38);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label10.TabIndex = 92;
            this.ckM_Label10.Text = "表示対象";
            this.ckM_Label10.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblChangeDate
            // 
            this.lblChangeDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblChangeDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblChangeDate.Location = new System.Drawing.Point(116, 9);
            this.lblChangeDate.Name = "lblChangeDate";
            this.lblChangeDate.Size = new System.Drawing.Size(84, 20);
            this.lblChangeDate.TabIndex = 84;
            this.lblChangeDate.Text = "YYYY/MM/DD";
            this.lblChangeDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ckM_Label11
            // 
            this.ckM_Label11.AutoSize = true;
            this.ckM_Label11.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label11.DefaultlabelSize = true;
            this.ckM_Label11.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label11.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label11.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label11.Location = new System.Drawing.Point(70, 13);
            this.ckM_Label11.Name = "ckM_Label11";
            this.ckM_Label11.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label11.TabIndex = 91;
            this.ckM_Label11.Text = "基準日";
            this.ckM_Label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label12
            // 
            this.ckM_Label12.AutoSize = true;
            this.ckM_Label12.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label12.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label12.DefaultlabelSize = true;
            this.ckM_Label12.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label12.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label12.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label12.Location = new System.Drawing.Point(41, 92);
            this.ckM_Label12.Name = "ckM_Label12";
            this.ckM_Label12.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label12.TabIndex = 94;
            this.ckM_Label12.Text = "運送会社名";
            this.ckM_Label12.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label14
            // 
            this.ckM_Label14.AutoSize = true;
            this.ckM_Label14.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label14.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label14.DefaultlabelSize = true;
            this.ckM_Label14.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label14.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label14.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label14.Location = new System.Drawing.Point(41, 64);
            this.ckM_Label14.Name = "ckM_Label14";
            this.ckM_Label14.Size = new System.Drawing.Size(71, 12);
            this.ckM_Label14.TabIndex = 93;
            this.ckM_Label14.Text = "運送会社CD";
            this.ckM_Label14.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.Location = new System.Drawing.Point(181, 36);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(49, 16);
            this.radioButton2.TabIndex = 86;
            this.radioButton2.Text = "履歴";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // txtShippingName
            // 
            this.txtShippingName.AllowMinus = false;
            this.txtShippingName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtShippingName.BackColor = System.Drawing.Color.White;
            this.txtShippingName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShippingName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtShippingName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtShippingName.DecimalPlace = 0;
            this.txtShippingName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtShippingName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtShippingName.IntegerPart = 0;
            this.txtShippingName.IsCorrectDate = true;
            this.txtShippingName.isEnterKeyDown = false;
            this.txtShippingName.IsNumber = true;
            this.txtShippingName.IsShop = false;
            this.txtShippingName.Length = 25;
            this.txtShippingName.Location = new System.Drawing.Point(114, 88);
            this.txtShippingName.MaxLength = 25;
            this.txtShippingName.MoveNext = true;
            this.txtShippingName.Name = "txtShippingName";
            this.txtShippingName.Size = new System.Drawing.Size(250, 19);
            this.txtShippingName.TabIndex = 89;
            this.txtShippingName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_Label8.Location = new System.Drawing.Point(217, 64);
            this.ckM_Label8.Name = "ckM_Label8";
            this.ckM_Label8.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label8.TabIndex = 90;
            this.ckM_Label8.Text = "～";
            this.ckM_Label8.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtShippingFrom
            // 
            this.txtShippingFrom.AllowMinus = false;
            this.txtShippingFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtShippingFrom.BackColor = System.Drawing.Color.White;
            this.txtShippingFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShippingFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtShippingFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtShippingFrom.DecimalPlace = 0;
            this.txtShippingFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtShippingFrom.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtShippingFrom.IntegerPart = 0;
            this.txtShippingFrom.IsCorrectDate = true;
            this.txtShippingFrom.isEnterKeyDown = false;
            this.txtShippingFrom.IsNumber = true;
            this.txtShippingFrom.IsShop = false;
            this.txtShippingFrom.Length = 10;
            this.txtShippingFrom.Location = new System.Drawing.Point(114, 61);
            this.txtShippingFrom.MaxLength = 10;
            this.txtShippingFrom.MoveNext = true;
            this.txtShippingFrom.Name = "txtShippingFrom";
            this.txtShippingFrom.Size = new System.Drawing.Size(100, 19);
            this.txtShippingFrom.TabIndex = 87;
            this.txtShippingFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // txtShippingTo
            // 
            this.txtShippingTo.AllowMinus = false;
            this.txtShippingTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtShippingTo.BackColor = System.Drawing.Color.White;
            this.txtShippingTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShippingTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtShippingTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtShippingTo.DecimalPlace = 0;
            this.txtShippingTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtShippingTo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtShippingTo.IntegerPart = 0;
            this.txtShippingTo.IsCorrectDate = true;
            this.txtShippingTo.isEnterKeyDown = false;
            this.txtShippingTo.IsNumber = true;
            this.txtShippingTo.IsShop = false;
            this.txtShippingTo.Length = 10;
            this.txtShippingTo.Location = new System.Drawing.Point(238, 61);
            this.txtShippingTo.MaxLength = 10;
            this.txtShippingTo.MoveNext = true;
            this.txtShippingTo.Name = "txtShippingTo";
            this.txtShippingTo.Size = new System.Drawing.Size(100, 19);
            this.txtShippingTo.TabIndex = 88;
            this.txtShippingTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtShippingTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtShippingTo_KeyDown);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.Location = new System.Drawing.Point(114, 36);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(62, 16);
            this.radioButton1.TabIndex = 85;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "基準日";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // F11Show
            // 
            this.F11Show.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.F11Show.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.F11Show.Cursor = System.Windows.Forms.Cursors.Hand;
            this.F11Show.DefaultBtnSize = false;
            this.F11Show.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.F11Show.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.F11Show.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.F11Show.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.F11Show.Location = new System.Drawing.Point(713, 102);
            this.F11Show.Margin = new System.Windows.Forms.Padding(1);
            this.F11Show.Name = "F11Show";
            this.F11Show.Size = new System.Drawing.Size(115, 28);
            this.F11Show.TabIndex = 95;
            this.F11Show.Text = "表示(F11)";
            this.F11Show.UseVisualStyleBackColor = false;
            this.F11Show.Click += new System.EventHandler(this.F11Show_Click);
            // 
            // gvShipping
            // 
            this.gvShipping.AllowUserToAddRows = false;
            this.gvShipping.AllowUserToDeleteRows = false;
            this.gvShipping.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.gvShipping.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gvShipping.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvShipping.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gvShipping.ColumnHeadersHeight = 25;
            this.gvShipping.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCD,
            this.colName,
            this.colDate});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("MS Gothic", 9F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvShipping.DefaultCellStyle = dataGridViewCellStyle4;
            this.gvShipping.EnableHeadersVisualStyles = false;
            this.gvShipping.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.gvShipping.Location = new System.Drawing.Point(45, 197);
            this.gvShipping.Name = "gvShipping";
            this.gvShipping.Size = new System.Drawing.Size(581, 408);
            this.gvShipping.TabIndex = 6;
            this.gvShipping.UseRowNo = true;
            this.gvShipping.UseSetting = true;
            this.gvShipping.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvShipping_CellDoubleClick);
            // 
            // colCD
            // 
            this.colCD.DataPropertyName = "CarrierCD";
            this.colCD.HeaderText = "CD";
            this.colCD.Name = "colCD";
            // 
            // colName
            // 
            this.colName.DataPropertyName = "CarrierName";
            this.colName.HeaderText = "運送会社名";
            this.colName.Name = "colName";
            this.colName.Width = 300;
            // 
            // colDate
            // 
            this.colDate.DataPropertyName = "ChangeDate";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.colDate.HeaderText = "改定日";
            this.colDate.Name = "colDate";
            // 
            // FrmSearch_Carrier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 682);
            this.Controls.Add(this.gvShipping);
            this.F11Visible = true;
            this.F9Visible = true;
            this.Name = "FrmSearch_Carrier";
            this.PanelHeaderHeight = 180;
            this.Text = "Search_Carrier";
            this.Load += new System.EventHandler(this.Search_Carrier_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Search_Carrier_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmSearch_Carrier_KeyUp);
            this.Controls.SetChildIndex(this.gvShipping, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvShipping)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CKM_Controls.CKM_Label ckM_Label10;
        private System.Windows.Forms.Label lblChangeDate;
        private CKM_Controls.CKM_Label ckM_Label11;
        private CKM_Controls.CKM_Label ckM_Label12;
        private CKM_Controls.CKM_Label ckM_Label14;
        private CKM_Controls.CKM_RadioButton radioButton2;
        private CKM_Controls.CKM_TextBox txtShippingName;
        private CKM_Controls.CKM_Label ckM_Label8;
        private CKM_Controls.CKM_TextBox txtShippingFrom;
        private CKM_Controls.CKM_TextBox txtShippingTo;
        private CKM_Controls.CKM_RadioButton radioButton1;
        private CKM_Controls.CKM_Button F11Show;
        private CKM_Controls.CKM_GridView gvShipping;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
    }
}