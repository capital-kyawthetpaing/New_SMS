namespace TanaireList
{
    partial class FrmTanaireList
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ScSKUCD = new Search.CKM_SearchControl();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.ckM_Label7 = new CKM_Controls.CKM_Label();
            this.cboSouko = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.chkLocationNashi = new CKM_Controls.CKM_CheckBox();
            this.chkLocationAri = new CKM_Controls.CKM_CheckBox();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.chkRegistered = new CKM_Controls.CKM_CheckBox();
            this.chkUnregistered = new CKM_Controls.CKM_CheckBox();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.txtEndDate = new CKM_Controls.CKM_TextBox();
            this.txtStartDate = new CKM_Controls.CKM_TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1711, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(1177, 0);
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.cboSouko);
            this.panel1.Controls.Add(this.ckM_Label5);
            this.panel1.Controls.Add(this.chkLocationNashi);
            this.panel1.Controls.Add(this.chkLocationAri);
            this.panel1.Controls.Add(this.ckM_Label4);
            this.panel1.Controls.Add(this.chkRegistered);
            this.panel1.Controls.Add(this.chkUnregistered);
            this.panel1.Controls.Add(this.ckM_Label3);
            this.panel1.Controls.Add(this.ckM_Label2);
            this.panel1.Controls.Add(this.ckM_Label1);
            this.panel1.Controls.Add(this.txtEndDate);
            this.panel1.Controls.Add(this.txtStartDate);
            this.panel1.Location = new System.Drawing.Point(0, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1713, 888);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ScSKUCD);
            this.panel2.Controls.Add(this.ckM_Label6);
            this.panel2.Controls.Add(this.ckM_Label7);
            this.panel2.Location = new System.Drawing.Point(92, 163);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(700, 60);
            this.panel2.TabIndex = 7;
            // 
            // ScSKUCD
            // 
            this.ScSKUCD.AutoSize = true;
            this.ScSKUCD.ChangeDate = "";
            this.ScSKUCD.ChangeDateWidth = 100;
            this.ScSKUCD.Code = "";
            this.ScSKUCD.CodeWidth = 190;
            this.ScSKUCD.CodeWidth1 = 190;
            this.ScSKUCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScSKUCD.DataCheck = false;
            this.ScSKUCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScSKUCD.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ScSKUCD.IsCopy = false;
            this.ScSKUCD.LabelText = "";
            this.ScSKUCD.LabelVisible = true;
            this.ScSKUCD.Location = new System.Drawing.Point(68, 19);
            this.ScSKUCD.Margin = new System.Windows.Forms.Padding(0);
            this.ScSKUCD.Name = "ScSKUCD";
            this.ScSKUCD.NameWidth = 350;
            this.ScSKUCD.SearchEnable = true;
            this.ScSKUCD.Size = new System.Drawing.Size(574, 32);
            this.ScSKUCD.Stype = Search.CKM_SearchControl.SearchType.SKUCD;
            this.ScSKUCD.TabIndex = 0;
            this.ScSKUCD.test = null;
            this.ScSKUCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScSKUCD.UseChangeDate = false;
            this.ScSKUCD.Value1 = null;
            this.ScSKUCD.Value2 = null;
            this.ScSKUCD.Value3 = null;
            this.ScSKUCD.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScSKUCD_CodeKeyDownEvent);
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
            this.ckM_Label6.Location = new System.Drawing.Point(8, 9);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label6.TabIndex = 12;
            this.ckM_Label6.Text = "棚番履歴";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label7.Location = new System.Drawing.Point(21, 28);
            this.ckM_Label7.Name = "ckM_Label7";
            this.ckM_Label7.Size = new System.Drawing.Size(40, 12);
            this.ckM_Label7.TabIndex = 13;
            this.ckM_Label7.Text = "SKUCD";
            this.ckM_Label7.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboSouko
            // 
            this.cboSouko.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboSouko.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSouko.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.WarehouseSelectAll;
            this.cboSouko.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboSouko.Flag = 0;
            this.cboSouko.FormattingEnabled = true;
            this.cboSouko.Length = 10;
            this.cboSouko.Location = new System.Drawing.Point(164, 47);
            this.cboSouko.MaxLength = 10;
            this.cboSouko.MoveNext = true;
            this.cboSouko.Name = "cboSouko";
            this.cboSouko.Size = new System.Drawing.Size(265, 20);
            this.cboSouko.TabIndex = 2;
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
            this.ckM_Label5.Location = new System.Drawing.Point(104, 101);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label5.TabIndex = 11;
            this.ckM_Label5.Text = "既存棚番";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkLocationNashi
            // 
            this.chkLocationNashi.AutoSize = true;
            this.chkLocationNashi.Checked = true;
            this.chkLocationNashi.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLocationNashi.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkLocationNashi.Location = new System.Drawing.Point(286, 100);
            this.chkLocationNashi.Name = "chkLocationNashi";
            this.chkLocationNashi.Size = new System.Drawing.Size(50, 16);
            this.chkLocationNashi.TabIndex = 6;
            this.chkLocationNashi.Text = "なし";
            this.chkLocationNashi.UseVisualStyleBackColor = true;
            // 
            // chkLocationAri
            // 
            this.chkLocationAri.AutoSize = true;
            this.chkLocationAri.Checked = true;
            this.chkLocationAri.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLocationAri.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkLocationAri.Location = new System.Drawing.Point(164, 100);
            this.chkLocationAri.Name = "chkLocationAri";
            this.chkLocationAri.Size = new System.Drawing.Size(50, 16);
            this.chkLocationAri.TabIndex = 5;
            this.chkLocationAri.Text = "あり";
            this.chkLocationAri.UseVisualStyleBackColor = true;
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
            this.ckM_Label4.Location = new System.Drawing.Point(130, 79);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label4.TabIndex = 8;
            this.ckM_Label4.Text = "棚番";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkRegistered
            // 
            this.chkRegistered.AutoSize = true;
            this.chkRegistered.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkRegistered.Location = new System.Drawing.Point(286, 78);
            this.chkRegistered.Name = "chkRegistered";
            this.chkRegistered.Size = new System.Drawing.Size(63, 16);
            this.chkRegistered.TabIndex = 4;
            this.chkRegistered.Text = "登録済";
            this.chkRegistered.UseVisualStyleBackColor = true;
            // 
            // chkUnregistered
            // 
            this.chkUnregistered.AutoSize = true;
            this.chkUnregistered.Checked = true;
            this.chkUnregistered.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUnregistered.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.chkUnregistered.Location = new System.Drawing.Point(164, 78);
            this.chkUnregistered.Name = "chkUnregistered";
            this.chkUnregistered.Size = new System.Drawing.Size(63, 16);
            this.chkUnregistered.TabIndex = 3;
            this.chkUnregistered.Text = "未登録";
            this.chkUnregistered.UseVisualStyleBackColor = true;
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
            this.ckM_Label3.Location = new System.Drawing.Point(130, 51);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label3.TabIndex = 4;
            this.ckM_Label3.Text = "倉庫";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label2.Location = new System.Drawing.Point(117, 23);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label2.TabIndex = 3;
            this.ckM_Label2.Text = "入荷日";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label1.Location = new System.Drawing.Point(290, 25);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label1.TabIndex = 2;
            this.ckM_Label1.Text = "～";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEndDate
            // 
            this.txtEndDate.AllowMinus = false;
            this.txtEndDate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtEndDate.BackColor = System.Drawing.Color.White;
            this.txtEndDate.BorderColor = false;
            this.txtEndDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEndDate.ClientColor = System.Drawing.Color.White;
            this.txtEndDate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtEndDate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtEndDate.DecimalPlace = 0;
            this.txtEndDate.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtEndDate.IntegerPart = 0;
            this.txtEndDate.IsCorrectDate = true;
            this.txtEndDate.isEnterKeyDown = false;
            this.txtEndDate.IsFirstTime = true;
            this.txtEndDate.isMaxLengthErr = false;
            this.txtEndDate.IsNumber = true;
            this.txtEndDate.IsShop = false;
            this.txtEndDate.Length = 10;
            this.txtEndDate.Location = new System.Drawing.Point(329, 21);
            this.txtEndDate.MaxLength = 10;
            this.txtEndDate.MoveNext = true;
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Size = new System.Drawing.Size(100, 19);
            this.txtEndDate.TabIndex = 1;
            this.txtEndDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtEndDate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtEndDate.UseColorSizMode = false;
            this.txtEndDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEndDate_KeyDown);
            // 
            // txtStartDate
            // 
            this.txtStartDate.AllowMinus = false;
            this.txtStartDate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtStartDate.BackColor = System.Drawing.Color.White;
            this.txtStartDate.BorderColor = false;
            this.txtStartDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStartDate.ClientColor = System.Drawing.Color.White;
            this.txtStartDate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtStartDate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtStartDate.DecimalPlace = 0;
            this.txtStartDate.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtStartDate.IntegerPart = 0;
            this.txtStartDate.IsCorrectDate = true;
            this.txtStartDate.isEnterKeyDown = false;
            this.txtStartDate.IsFirstTime = true;
            this.txtStartDate.isMaxLengthErr = false;
            this.txtStartDate.IsNumber = true;
            this.txtStartDate.IsShop = false;
            this.txtStartDate.Length = 10;
            this.txtStartDate.Location = new System.Drawing.Point(164, 20);
            this.txtStartDate.MaxLength = 10;
            this.txtStartDate.MoveNext = true;
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Size = new System.Drawing.Size(100, 19);
            this.txtStartDate.TabIndex = 0;
            this.txtStartDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtStartDate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtStartDate.UseColorSizMode = false;
            // 
            // FrmTanaireList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.panel1);
            this.F10Visible = false;
            this.F11Visible = false;
            this.F2Visible = false;
            this.F3Visible = false;
            this.F4Visible = false;
            this.F5Visible = false;
            this.F7Visible = false;
            this.F8Visible = false;
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "FrmTanaireList";
            this.PanelHeaderHeight = 40;
            this.Text = "TanaireList";
            this.Load += new System.EventHandler(this.FrmTanaireList_Load);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_CheckBox chkLocationNashi;
        private CKM_Controls.CKM_CheckBox chkLocationAri;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_CheckBox chkRegistered;
        private CKM_Controls.CKM_CheckBox chkUnregistered;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_TextBox txtEndDate;
        private CKM_Controls.CKM_TextBox txtStartDate;
        private Search.CKM_SearchControl ScSKUCD;
        private CKM_Controls.CKM_Label ckM_Label7;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_ComboBox cboSouko;
        private System.Windows.Forms.Panel panel2;
    }
}

