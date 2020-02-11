namespace Search
{
    partial class FrmSearch_Ginkou
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
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.RdoKijunBi = new CKM_Controls.CKM_RadioButton();
            this.RdoRireki = new CKM_Controls.CKM_RadioButton();
            this.txtBankCDFrom = new CKM_Controls.CKM_TextBox();
            this.txtBankCDTo = new CKM_Controls.CKM_TextBox();
            this.ckM_Label7 = new CKM_Controls.CKM_Label();
            this.txtBankName = new CKM_Controls.CKM_TextBox();
            this.txtBankKana = new CKM_Controls.CKM_TextBox();
            this.btnDisplay = new CKM_Controls.CKM_Button();
            this.GdvGinkou = new CKM_Controls.CKM_GridView();
            this.colNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBankCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBankName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChangeDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblChangeDate = new System.Windows.Forms.Label();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GdvGinkou)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.lblChangeDate);
            this.PanelHeader.Controls.Add(this.btnDisplay);
            this.PanelHeader.Controls.Add(this.txtBankKana);
            this.PanelHeader.Controls.Add(this.txtBankName);
            this.PanelHeader.Controls.Add(this.ckM_Label7);
            this.PanelHeader.Controls.Add(this.txtBankCDTo);
            this.PanelHeader.Controls.Add(this.txtBankCDFrom);
            this.PanelHeader.Controls.Add(this.RdoRireki);
            this.PanelHeader.Controls.Add(this.RdoKijunBi);
            this.PanelHeader.Controls.Add(this.ckM_Label6);
            this.PanelHeader.Controls.Add(this.ckM_Label5);
            this.PanelHeader.Controls.Add(this.ckM_Label4);
            this.PanelHeader.Controls.Add(this.ckM_Label3);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Size = new System.Drawing.Size(659, 143);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label3, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label4, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label5, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label6, 0);
            this.PanelHeader.Controls.SetChildIndex(this.RdoKijunBi, 0);
            this.PanelHeader.Controls.SetChildIndex(this.RdoRireki, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtBankCDFrom, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtBankCDTo, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label7, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtBankName, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtBankKana, 0);
            this.PanelHeader.Controls.SetChildIndex(this.btnDisplay, 0);
            this.PanelHeader.Controls.SetChildIndex(this.lblChangeDate, 0);
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
            this.ckM_Label1.Location = new System.Drawing.Point(48, 15);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label1.TabIndex = 2;
            this.ckM_Label1.Text = "基準日";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label3.Location = new System.Drawing.Point(35, 38);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label3.TabIndex = 4;
            this.ckM_Label3.Text = "表示対象";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label4.Location = new System.Drawing.Point(47, 63);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(45, 12);
            this.ckM_Label4.TabIndex = 5;
            this.ckM_Label4.Text = "銀行CD";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label5.Location = new System.Drawing.Point(48, 90);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label5.TabIndex = 6;
            this.ckM_Label5.Text = "銀行名";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label6.Location = new System.Drawing.Point(48, 116);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label6.TabIndex = 7;
            this.ckM_Label6.Text = "カナ名";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RdoKijunBi
            // 
            this.RdoKijunBi.AutoSize = true;
            this.RdoKijunBi.Checked = true;
            this.RdoKijunBi.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.RdoKijunBi.Location = new System.Drawing.Point(97, 36);
            this.RdoKijunBi.Name = "RdoKijunBi";
            this.RdoKijunBi.Size = new System.Drawing.Size(62, 16);
            this.RdoKijunBi.TabIndex = 1;
            this.RdoKijunBi.TabStop = true;
            this.RdoKijunBi.Text = "基準日";
            this.RdoKijunBi.UseVisualStyleBackColor = true;
            // 
            // RdoRireki
            // 
            this.RdoRireki.AutoSize = true;
            this.RdoRireki.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.RdoRireki.Location = new System.Drawing.Point(172, 36);
            this.RdoRireki.Name = "RdoRireki";
            this.RdoRireki.Size = new System.Drawing.Size(49, 16);
            this.RdoRireki.TabIndex = 2;
            this.RdoRireki.Text = "履歴";
            this.RdoRireki.UseVisualStyleBackColor = true;
            // 
            // txtBankCDFrom
            // 
            this.txtBankCDFrom.AllowMinus = false;
            this.txtBankCDFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtBankCDFrom.BackColor = System.Drawing.Color.White;
            this.txtBankCDFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBankCDFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtBankCDFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtBankCDFrom.DecimalPlace = 0;
            this.txtBankCDFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtBankCDFrom.IntegerPart = 0;
            this.txtBankCDFrom.IsCorrectDate = true;
            this.txtBankCDFrom.isEnterKeyDown = false;
            this.txtBankCDFrom.IsNumber = true;
            this.txtBankCDFrom.IsShop = false;
            this.txtBankCDFrom.Length = 4;
            this.txtBankCDFrom.Location = new System.Drawing.Point(95, 60);
            this.txtBankCDFrom.MaxLength = 4;
            this.txtBankCDFrom.MoveNext = true;
            this.txtBankCDFrom.Name = "txtBankCDFrom";
            this.txtBankCDFrom.Size = new System.Drawing.Size(40, 19);
            this.txtBankCDFrom.TabIndex = 3;
            this.txtBankCDFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // txtBankCDTo
            // 
            this.txtBankCDTo.AllowMinus = false;
            this.txtBankCDTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtBankCDTo.BackColor = System.Drawing.Color.White;
            this.txtBankCDTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBankCDTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtBankCDTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtBankCDTo.DecimalPlace = 0;
            this.txtBankCDTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtBankCDTo.IntegerPart = 0;
            this.txtBankCDTo.IsCorrectDate = true;
            this.txtBankCDTo.isEnterKeyDown = false;
            this.txtBankCDTo.IsNumber = true;
            this.txtBankCDTo.IsShop = false;
            this.txtBankCDTo.Length = 4;
            this.txtBankCDTo.Location = new System.Drawing.Point(179, 60);
            this.txtBankCDTo.MaxLength = 4;
            this.txtBankCDTo.MoveNext = true;
            this.txtBankCDTo.Name = "txtBankCDTo";
            this.txtBankCDTo.Size = new System.Drawing.Size(40, 19);
            this.txtBankCDTo.TabIndex = 4;
            this.txtBankCDTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_Label7.Location = new System.Drawing.Point(150, 63);
            this.ckM_Label7.Name = "ckM_Label7";
            this.ckM_Label7.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label7.TabIndex = 12;
            this.ckM_Label7.Text = "～";
            this.ckM_Label7.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBankName
            // 
            this.txtBankName.AllowMinus = false;
            this.txtBankName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtBankName.BackColor = System.Drawing.Color.White;
            this.txtBankName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBankName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtBankName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtBankName.DecimalPlace = 0;
            this.txtBankName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtBankName.IntegerPart = 0;
            this.txtBankName.IsCorrectDate = true;
            this.txtBankName.isEnterKeyDown = false;
            this.txtBankName.IsNumber = true;
            this.txtBankName.IsShop = false;
            this.txtBankName.Length = 30;
            this.txtBankName.Location = new System.Drawing.Point(95, 87);
            this.txtBankName.MaxLength = 15;
            this.txtBankName.MoveNext = true;
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.Size = new System.Drawing.Size(195, 19);
            this.txtBankName.TabIndex = 5;
            this.txtBankName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // txtBankKana
            // 
            this.txtBankKana.AllowMinus = false;
            this.txtBankKana.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtBankKana.BackColor = System.Drawing.Color.White;
            this.txtBankKana.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBankKana.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtBankKana.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtBankKana.DecimalPlace = 0;
            this.txtBankKana.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtBankKana.IntegerPart = 0;
            this.txtBankKana.IsCorrectDate = true;
            this.txtBankKana.isEnterKeyDown = false;
            this.txtBankKana.IsNumber = true;
            this.txtBankKana.IsShop = false;
            this.txtBankKana.Length = 30;
            this.txtBankKana.Location = new System.Drawing.Point(95, 113);
            this.txtBankKana.MaxLength = 30;
            this.txtBankKana.MoveNext = true;
            this.txtBankKana.Name = "txtBankKana";
            this.txtBankKana.Size = new System.Drawing.Size(300, 19);
            this.txtBankKana.TabIndex = 6;
            this.txtBankKana.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.btnDisplay.Location = new System.Drawing.Point(537, 108);
            this.btnDisplay.Margin = new System.Windows.Forms.Padding(1);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(118, 28);
            this.btnDisplay.TabIndex = 7;
            this.btnDisplay.Text = "表示(F11)";
            this.btnDisplay.UseVisualStyleBackColor = false;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // GdvGinkou
            // 
            this.GdvGinkou.AllowUserToAddRows = false;
            this.GdvGinkou.AllowUserToDeleteRows = false;
            this.GdvGinkou.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.GdvGinkou.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.GdvGinkou.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GdvGinkou.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.GdvGinkou.ColumnHeadersHeight = 25;
            this.GdvGinkou.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNo,
            this.colBankCD,
            this.colBankName,
            this.colChangeDate});
            this.GdvGinkou.EnableHeadersVisualStyles = false;
            this.GdvGinkou.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GdvGinkou.Location = new System.Drawing.Point(94, 191);
            this.GdvGinkou.Name = "GdvGinkou";
            this.GdvGinkou.Size = new System.Drawing.Size(425, 432);
            this.GdvGinkou.TabIndex = 5;
            this.GdvGinkou.UseRowNo = true;
            this.GdvGinkou.UseSetting = true;
            this.GdvGinkou.DoubleClick += new System.EventHandler(this.GdvGinkou_DoubleClick);
            // 
            // colNo
            // 
            this.colNo.DataPropertyName = "No";
            this.colNo.HeaderText = "No";
            this.colNo.MinimumWidth = 2;
            this.colNo.Name = "colNo";
            this.colNo.ReadOnly = true;
            this.colNo.Visible = false;
            this.colNo.Width = 50;
            // 
            // colBankCD
            // 
            this.colBankCD.DataPropertyName = "BankCD";
            this.colBankCD.HeaderText = "銀行CD";
            this.colBankCD.MinimumWidth = 4;
            this.colBankCD.Name = "colBankCD";
            this.colBankCD.ReadOnly = true;
            this.colBankCD.Width = 70;
            // 
            // colBankName
            // 
            this.colBankName.DataPropertyName = "BankName";
            this.colBankName.HeaderText = "銀行名";
            this.colBankName.MinimumWidth = 30;
            this.colBankName.Name = "colBankName";
            this.colBankName.ReadOnly = true;
            this.colBankName.Width = 200;
            // 
            // colChangeDate
            // 
            this.colChangeDate.DataPropertyName = "ChangeDate";
            this.colChangeDate.HeaderText = "改定日";
            this.colChangeDate.MinimumWidth = 8;
            this.colChangeDate.Name = "colChangeDate";
            this.colChangeDate.ReadOnly = true;
            // 
            // lblChangeDate
            // 
            this.lblChangeDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblChangeDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblChangeDate.Location = new System.Drawing.Point(95, 11);
            this.lblChangeDate.Name = "lblChangeDate";
            this.lblChangeDate.Size = new System.Drawing.Size(84, 20);
            this.lblChangeDate.TabIndex = 0;
            this.lblChangeDate.Text = "YYYY/MM/DD";
            this.lblChangeDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmSearch_Ginkou
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 661);
            this.Controls.Add(this.GdvGinkou);
            this.F11Visible = true;
            this.F9Visible = true;
            this.Name = "FrmSearch_Ginkou";
            this.PanelHeaderHeight = 185;
            this.ProgramName = "銀行検索";
            this.Text = "Search_Ginkou";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSearch_Ginkou_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmSearch_Ginkou_KeyUp);
            this.Controls.SetChildIndex(this.GdvGinkou, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GdvGinkou)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_RadioButton RdoRireki;
        private CKM_Controls.CKM_RadioButton RdoKijunBi;
        private CKM_Controls.CKM_TextBox txtBankKana;
        private CKM_Controls.CKM_TextBox txtBankName;
        private CKM_Controls.CKM_Label ckM_Label7;
        private CKM_Controls.CKM_TextBox txtBankCDTo;
        private CKM_Controls.CKM_TextBox txtBankCDFrom;
        private CKM_Controls.CKM_Button btnDisplay;
        private CKM_Controls.CKM_GridView GdvGinkou;
        private System.Windows.Forms.Label lblChangeDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBankCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBankName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChangeDate;
    }
}