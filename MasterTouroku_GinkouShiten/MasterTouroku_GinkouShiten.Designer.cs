namespace MasterTouroku_GinkouShiten
{
    partial class MasterTouroku_GinkouShiten
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
            this.PanelNormal = new System.Windows.Forms.Panel();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.ScBankCD = new Search.CKM_SearchControl();
            this.ScBranchCD = new Search.CKM_SearchControl();
            this.PanelCopy = new System.Windows.Forms.Panel();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.ScCopyBankCD = new Search.CKM_SearchControl();
            this.ScCopyBranchCD = new Search.CKM_SearchControl();
            this.btnDisplay = new CKM_Controls.CKM_Button();
            this.PanelDetail = new System.Windows.Forms.Panel();
            this.ckM_Label12 = new CKM_Controls.CKM_Label();
            this.ChkDeleteFlg = new CKM_Controls.CKM_CheckBox();
            this.ckM_Label11 = new CKM_Controls.CKM_Label();
            this.TxtRemark = new CKM_Controls.CKM_MultiLineTextBox();
            this.TxtKanaName = new CKM_Controls.CKM_TextBox();
            this.TxtBankBranchName = new CKM_Controls.CKM_TextBox();
            this.ckM_Label7 = new CKM_Controls.CKM_Label();
            this.PanelHeader.SuspendLayout();
            this.PanelSearch.SuspendLayout();
            this.PanelNormal.SuspendLayout();
            this.PanelCopy.SuspendLayout();
            this.PanelDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.PanelCopy);
            this.PanelHeader.Controls.Add(this.PanelNormal);
            this.PanelHeader.Size = new System.Drawing.Size(1774, 114);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.Controls.SetChildIndex(this.PanelNormal, 0);
            this.PanelHeader.Controls.SetChildIndex(this.PanelCopy, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Controls.Add(this.btnDisplay);
            this.PanelSearch.Location = new System.Drawing.Point(1240, 0);
            // 
            // PanelNormal
            // 
            this.PanelNormal.Controls.Add(this.ckM_Label6);
            this.PanelNormal.Controls.Add(this.ckM_Label2);
            this.PanelNormal.Controls.Add(this.ckM_Label1);
            this.PanelNormal.Controls.Add(this.ScBankCD);
            this.PanelNormal.Controls.Add(this.ScBranchCD);
            this.PanelNormal.Location = new System.Drawing.Point(50, 1);
            this.PanelNormal.Name = "PanelNormal";
            this.PanelNormal.Size = new System.Drawing.Size(510, 78);
            this.PanelNormal.TabIndex = 0;
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
            this.ckM_Label6.Location = new System.Drawing.Point(17, 57);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label6.TabIndex = 4;
            this.ckM_Label6.Text = "改定日";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label2.Location = new System.Drawing.Point(16, 34);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(45, 12);
            this.ckM_Label2.TabIndex = 2;
            this.ckM_Label2.Text = "支店CD";
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
            this.ckM_Label1.Location = new System.Drawing.Point(16, 10);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(45, 12);
            this.ckM_Label1.TabIndex = 0;
            this.ckM_Label1.Text = "銀行CD";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScBankCD
            // 
            this.ScBankCD.AutoSize = true;
            this.ScBankCD.ChangeDate = "";
            this.ScBankCD.ChangeDateWidth = 100;
            this.ScBankCD.Code = "";
            this.ScBankCD.CodeWidth = 40;
            this.ScBankCD.CodeWidth1 = 40;
            this.ScBankCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScBankCD.DataCheck = false;
            this.ScBankCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScBankCD.IsCopy = false;
            this.ScBankCD.LabelText = "";
            this.ScBankCD.LabelVisible = true;
            this.ScBankCD.Location = new System.Drawing.Point(63, 1);
            this.ScBankCD.Margin = new System.Windows.Forms.Padding(0);
            this.ScBankCD.Name = "ScBankCD";
            this.ScBankCD.NameWidth = 350;
            this.ScBankCD.SearchEnable = true;
            this.ScBankCD.Size = new System.Drawing.Size(424, 27);
            this.ScBankCD.Stype = Search.CKM_SearchControl.SearchType.銀行;
            this.ScBankCD.TabIndex = 1;
            this.ScBankCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScBankCD.UseChangeDate = false;
            this.ScBankCD.Value1 = null;
            this.ScBankCD.Value2 = null;
            this.ScBankCD.Value3 = null;
            this.ScBankCD.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScBankCD_CodeKeyDownEvent);
            this.ScBankCD.Enter += new System.EventHandler(this.ScBankCD_Enter);
            this.ScBankCD.Leave += new System.EventHandler(this.ScBankCD_Leave);
            // 
            // ScBranchCD
            // 
            this.ScBranchCD.AutoSize = true;
            this.ScBranchCD.ChangeDate = "";
            this.ScBranchCD.ChangeDateWidth = 100;
            this.ScBranchCD.Code = "";
            this.ScBranchCD.CodeWidth = 40;
            this.ScBranchCD.CodeWidth1 = 40;
            this.ScBranchCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScBranchCD.DataCheck = false;
            this.ScBranchCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScBranchCD.IsCopy = false;
            this.ScBranchCD.LabelText = "";
            this.ScBranchCD.LabelVisible = false;
            this.ScBranchCD.Location = new System.Drawing.Point(63, 23);
            this.ScBranchCD.Margin = new System.Windows.Forms.Padding(0);
            this.ScBranchCD.Name = "ScBranchCD";
            this.ScBranchCD.NameWidth = 350;
            this.ScBranchCD.SearchEnable = true;
            this.ScBranchCD.Size = new System.Drawing.Size(103, 50);
            this.ScBranchCD.Stype = Search.CKM_SearchControl.SearchType.銀行支店;
            this.ScBranchCD.TabIndex = 3;
            this.ScBranchCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScBranchCD.UseChangeDate = true;
            this.ScBranchCD.Value1 = null;
            this.ScBranchCD.Value2 = null;
            this.ScBranchCD.Value3 = null;
            this.ScBranchCD.ChangeDateKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScBranchCD_ChangeDateKeyDownEvent);
            this.ScBranchCD.Leave += new System.EventHandler(this.ChangeDate_Leave);
            // 
            // PanelCopy
            // 
            this.PanelCopy.Controls.Add(this.ckM_Label3);
            this.PanelCopy.Controls.Add(this.ckM_Label5);
            this.PanelCopy.Controls.Add(this.ckM_Label4);
            this.PanelCopy.Controls.Add(this.ScCopyBankCD);
            this.PanelCopy.Controls.Add(this.ScCopyBranchCD);
            this.PanelCopy.Location = new System.Drawing.Point(572, 1);
            this.PanelCopy.Name = "PanelCopy";
            this.PanelCopy.Size = new System.Drawing.Size(510, 78);
            this.PanelCopy.TabIndex = 1;
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
            this.ckM_Label3.Location = new System.Drawing.Point(39, 57);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label3.TabIndex = 4;
            this.ckM_Label3.Text = "改定日";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label5.Location = new System.Drawing.Point(12, 34);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(71, 12);
            this.ckM_Label5.TabIndex = 2;
            this.ckM_Label5.Text = "複写支店CD";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label4.Location = new System.Drawing.Point(11, 9);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(71, 12);
            this.ckM_Label4.TabIndex = 0;
            this.ckM_Label4.Text = "複写銀行CD";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScCopyBankCD
            // 
            this.ScCopyBankCD.AutoSize = true;
            this.ScCopyBankCD.ChangeDate = "";
            this.ScCopyBankCD.ChangeDateWidth = 100;
            this.ScCopyBankCD.Code = "";
            this.ScCopyBankCD.CodeWidth = 40;
            this.ScCopyBankCD.CodeWidth1 = 40;
            this.ScCopyBankCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScCopyBankCD.DataCheck = false;
            this.ScCopyBankCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScCopyBankCD.IsCopy = false;
            this.ScCopyBankCD.LabelText = "";
            this.ScCopyBankCD.LabelVisible = true;
            this.ScCopyBankCD.Location = new System.Drawing.Point(83, 0);
            this.ScCopyBankCD.Margin = new System.Windows.Forms.Padding(0);
            this.ScCopyBankCD.Name = "ScCopyBankCD";
            this.ScCopyBankCD.NameWidth = 350;
            this.ScCopyBankCD.SearchEnable = true;
            this.ScCopyBankCD.Size = new System.Drawing.Size(424, 27);
            this.ScCopyBankCD.Stype = Search.CKM_SearchControl.SearchType.銀行;
            this.ScCopyBankCD.TabIndex = 1;
            this.ScCopyBankCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScCopyBankCD.UseChangeDate = false;
            this.ScCopyBankCD.Value1 = null;
            this.ScCopyBankCD.Value2 = null;
            this.ScCopyBankCD.Value3 = null;
            this.ScCopyBankCD.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScCopyBankCD_CodeKeyDownEvent);
            this.ScCopyBankCD.Enter += new System.EventHandler(this.ScCopyBankCD_Enter);
            this.ScCopyBankCD.Leave += new System.EventHandler(this.ScCopyBankCD_Leave);
            // 
            // ScCopyBranchCD
            // 
            this.ScCopyBranchCD.AutoSize = true;
            this.ScCopyBranchCD.ChangeDate = "";
            this.ScCopyBranchCD.ChangeDateWidth = 100;
            this.ScCopyBranchCD.Code = "";
            this.ScCopyBranchCD.CodeWidth = 40;
            this.ScCopyBranchCD.CodeWidth1 = 40;
            this.ScCopyBranchCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScCopyBranchCD.DataCheck = false;
            this.ScCopyBranchCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScCopyBranchCD.IsCopy = false;
            this.ScCopyBranchCD.LabelText = "";
            this.ScCopyBranchCD.LabelVisible = false;
            this.ScCopyBranchCD.Location = new System.Drawing.Point(83, 23);
            this.ScCopyBranchCD.Margin = new System.Windows.Forms.Padding(0);
            this.ScCopyBranchCD.Name = "ScCopyBranchCD";
            this.ScCopyBranchCD.NameWidth = 350;
            this.ScCopyBranchCD.SearchEnable = true;
            this.ScCopyBranchCD.Size = new System.Drawing.Size(103, 50);
            this.ScCopyBranchCD.Stype = Search.CKM_SearchControl.SearchType.銀行支店;
            this.ScCopyBranchCD.TabIndex = 3;
            this.ScCopyBranchCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScCopyBranchCD.UseChangeDate = true;
            this.ScCopyBranchCD.Value1 = null;
            this.ScCopyBranchCD.Value2 = null;
            this.ScCopyBranchCD.Value3 = null;
            this.ScCopyBranchCD.ChangeDateKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScCopyBranchCD_ChangeDateKeyDownEvent);
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
            this.btnDisplay.Location = new System.Drawing.Point(410, 3);
            this.btnDisplay.Margin = new System.Windows.Forms.Padding(1);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(118, 28);
            this.btnDisplay.TabIndex = 1;
            this.btnDisplay.Text = "表示(F11)";
            this.btnDisplay.UseVisualStyleBackColor = false;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // PanelDetail
            // 
            this.PanelDetail.Controls.Add(this.ckM_Label12);
            this.PanelDetail.Controls.Add(this.ChkDeleteFlg);
            this.PanelDetail.Controls.Add(this.ckM_Label11);
            this.PanelDetail.Controls.Add(this.TxtRemark);
            this.PanelDetail.Controls.Add(this.TxtKanaName);
            this.PanelDetail.Controls.Add(this.TxtBankBranchName);
            this.PanelDetail.Controls.Add(this.ckM_Label7);
            this.PanelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelDetail.Location = new System.Drawing.Point(0, 170);
            this.PanelDetail.Name = "PanelDetail";
            this.PanelDetail.Size = new System.Drawing.Size(1776, 759);
            this.PanelDetail.TabIndex = 1;
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
            this.ckM_Label12.Location = new System.Drawing.Point(67, 51);
            this.ckM_Label12.Name = "ckM_Label12";
            this.ckM_Label12.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label12.TabIndex = 6;
            this.ckM_Label12.Text = "カナ名";
            this.ckM_Label12.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ChkDeleteFlg
            // 
            this.ChkDeleteFlg.AutoSize = true;
            this.ChkDeleteFlg.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ChkDeleteFlg.Location = new System.Drawing.Point(781, 19);
            this.ChkDeleteFlg.Name = "ChkDeleteFlg";
            this.ChkDeleteFlg.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ChkDeleteFlg.Size = new System.Drawing.Size(50, 16);
            this.ChkDeleteFlg.TabIndex = 5;
            this.ChkDeleteFlg.Text = "削除";
            this.ChkDeleteFlg.UseVisualStyleBackColor = true;
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
            this.ckM_Label11.Location = new System.Drawing.Point(80, 86);
            this.ckM_Label11.Name = "ckM_Label11";
            this.ckM_Label11.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label11.TabIndex = 4;
            this.ckM_Label11.Text = "備考";
            this.ckM_Label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TxtRemark
            // 
            this.TxtRemark.Back_Color = CKM_Controls.CKM_MultiLineTextBox.CKM_Color.White;
            this.TxtRemark.BackColor = System.Drawing.Color.White;
            this.TxtRemark.Ctrl_Byte = CKM_Controls.CKM_MultiLineTextBox.Bytes.半全角;
            this.TxtRemark.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.TxtRemark.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.TxtRemark.Length = 500;
            this.TxtRemark.Location = new System.Drawing.Point(114, 83);
            this.TxtRemark.MaxLength = 500;
            this.TxtRemark.Mdea = false;
            this.TxtRemark.Mfocus = false;
            this.TxtRemark.MoveNext = true;
            this.TxtRemark.Multiline = true;
            this.TxtRemark.Name = "TxtRemark";
            this.TxtRemark.RowCount = 6;
            this.TxtRemark.Size = new System.Drawing.Size(620, 85);
            this.TxtRemark.TabIndex = 3;
            this.TxtRemark.TextSize = CKM_Controls.CKM_MultiLineTextBox.FontSize.Normal;
            // 
            // TxtKanaName
            // 
            this.TxtKanaName.AllowMinus = false;
            this.TxtKanaName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.TxtKanaName.BackColor = System.Drawing.Color.White;
            this.TxtKanaName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtKanaName.ClientColor = System.Drawing.Color.White;
            this.TxtKanaName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.TxtKanaName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.TxtKanaName.Cursor = System.Windows.Forms.Cursors.Default;
            this.TxtKanaName.DecimalPlace = 0;
            this.TxtKanaName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.TxtKanaName.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.TxtKanaName.IntegerPart = 0;
            this.TxtKanaName.IsCorrectDate = true;
            this.TxtKanaName.isEnterKeyDown = false;
            this.TxtKanaName.isMaxLengthErr = false;
            this.TxtKanaName.IsNumber = true;
            this.TxtKanaName.IsShop = false;
            this.TxtKanaName.Length = 30;
            this.TxtKanaName.Location = new System.Drawing.Point(114, 48);
            this.TxtKanaName.MaxLength = 30;
            this.TxtKanaName.MoveNext = true;
            this.TxtKanaName.Name = "TxtKanaName";
            this.TxtKanaName.Size = new System.Drawing.Size(195, 19);
            this.TxtKanaName.TabIndex = 2;
            this.TxtKanaName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // TxtBankBranchName
            // 
            this.TxtBankBranchName.AllowMinus = false;
            this.TxtBankBranchName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.TxtBankBranchName.BackColor = System.Drawing.Color.White;
            this.TxtBankBranchName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtBankBranchName.ClientColor = System.Drawing.Color.White;
            this.TxtBankBranchName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.TxtBankBranchName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.TxtBankBranchName.DecimalPlace = 0;
            this.TxtBankBranchName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.TxtBankBranchName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.TxtBankBranchName.IntegerPart = 0;
            this.TxtBankBranchName.IsCorrectDate = true;
            this.TxtBankBranchName.isEnterKeyDown = false;
            this.TxtBankBranchName.isMaxLengthErr = false;
            this.TxtBankBranchName.IsNumber = true;
            this.TxtBankBranchName.IsShop = false;
            this.TxtBankBranchName.Length = 30;
            this.TxtBankBranchName.Location = new System.Drawing.Point(114, 15);
            this.TxtBankBranchName.MaxLength = 30;
            this.TxtBankBranchName.MoveNext = true;
            this.TxtBankBranchName.Name = "TxtBankBranchName";
            this.TxtBankBranchName.Size = new System.Drawing.Size(195, 19);
            this.TxtBankBranchName.TabIndex = 1;
            this.TxtBankBranchName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_Label7.Location = new System.Drawing.Point(41, 19);
            this.ckM_Label7.Name = "ckM_Label7";
            this.ckM_Label7.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label7.TabIndex = 0;
            this.ckM_Label7.Text = "銀行支店名";
            this.ckM_Label7.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MasterTouroku_GinkouShiten
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1776, 961);
            this.Controls.Add(this.PanelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "MasterTouroku_GinkouShiten";
            this.PanelHeaderHeight = 170;
            this.Text = "MasterTouroku_GinkouShiten";
            this.Controls.SetChildIndex(this.PanelDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelSearch.ResumeLayout(false);
            this.PanelNormal.ResumeLayout(false);
            this.PanelNormal.PerformLayout();
            this.PanelCopy.ResumeLayout(false);
            this.PanelCopy.PerformLayout();
            this.PanelDetail.ResumeLayout(false);
            this.PanelDetail.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PanelCopy;
        private System.Windows.Forms.Panel PanelNormal;
        private CKM_Controls.CKM_Button btnDisplay;
        private Search.CKM_SearchControl ScBranchCD;
        private Search.CKM_SearchControl ScCopyBranchCD;
        private System.Windows.Forms.Panel PanelDetail;
        private Search.CKM_SearchControl ScBankCD;
        private Search.CKM_SearchControl ScCopyBankCD;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_Label ckM_Label7;
        private CKM_Controls.CKM_TextBox TxtBankBranchName;
        private CKM_Controls.CKM_TextBox TxtKanaName;
        private CKM_Controls.CKM_Label ckM_Label11;
        private CKM_Controls.CKM_CheckBox ChkDeleteFlg;
        private CKM_Controls.CKM_Label ckM_Label12;
        private CKM_Controls.CKM_MultiLineTextBox TxtRemark;
    }
}

