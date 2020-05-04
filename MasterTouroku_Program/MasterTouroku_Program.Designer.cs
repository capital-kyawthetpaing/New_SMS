namespace MasterTouroku_Program
{
    partial class MasterTouroku_Program
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
            this.PanelDetail = new System.Windows.Forms.Panel();
            this.ckM_Label11 = new CKM_Controls.CKM_Label();
            this.txtFileName = new CKM_Controls.CKM_TextBox();
            this.txtFilePass = new CKM_Controls.CKM_TextBox();
            this.txtFileDrive = new CKM_Controls.CKM_TextBox();
            this.txtExeName = new CKM_Controls.CKM_TextBox();
            this.cboType = new CKM_Controls.CKM_ComboBox();
            this.txtProgramName = new CKM_Controls.CKM_TextBox();
            this.ckM_Label9 = new CKM_Controls.CKM_Label();
            this.ckM_Label8 = new CKM_Controls.CKM_Label();
            this.ckM_Label7 = new CKM_Controls.CKM_Label();
            this.ckM_Label6 = new CKM_Controls.CKM_Label();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.btnDisplay = new CKM_Controls.CKM_Button();
            this.PanelNormal = new System.Windows.Forms.Panel();
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.PanelCopy = new System.Windows.Forms.Panel();
            this.scProgramCopy = new Search.CKM_SearchControl();
            this.scProgramID = new Search.CKM_SearchControl();
            this.ckM_SearchControl1 = new Search.CKM_SearchControl();
            this.PanelHeader.SuspendLayout();
            this.PanelSearch.SuspendLayout();
            this.PanelDetail.SuspendLayout();
            this.PanelNormal.SuspendLayout();
            this.PanelCopy.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.scProgramCopy);
            this.PanelHeader.Controls.Add(this.scProgramID);
            this.PanelHeader.Controls.Add(this.PanelCopy);
            this.PanelHeader.Controls.Add(this.PanelNormal);
            this.PanelHeader.Controls.Add(this.ckM_SearchControl1);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Size = new System.Drawing.Size(1774, 144);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_SearchControl1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.PanelNormal, 0);
            this.PanelHeader.Controls.SetChildIndex(this.PanelCopy, 0);
            this.PanelHeader.Controls.SetChildIndex(this.scProgramID, 0);
            this.PanelHeader.Controls.SetChildIndex(this.scProgramCopy, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Controls.Add(this.btnDisplay);
            this.PanelSearch.Location = new System.Drawing.Point(1240, 0);
            // 
            // PanelDetail
            // 
            this.PanelDetail.Controls.Add(this.ckM_Label11);
            this.PanelDetail.Controls.Add(this.txtFileName);
            this.PanelDetail.Controls.Add(this.txtFilePass);
            this.PanelDetail.Controls.Add(this.txtFileDrive);
            this.PanelDetail.Controls.Add(this.txtExeName);
            this.PanelDetail.Controls.Add(this.cboType);
            this.PanelDetail.Controls.Add(this.txtProgramName);
            this.PanelDetail.Controls.Add(this.ckM_Label9);
            this.PanelDetail.Controls.Add(this.ckM_Label8);
            this.PanelDetail.Controls.Add(this.ckM_Label7);
            this.PanelDetail.Controls.Add(this.ckM_Label6);
            this.PanelDetail.Controls.Add(this.ckM_Label5);
            this.PanelDetail.Controls.Add(this.ckM_Label4);
            this.PanelDetail.Controls.Add(this.ckM_Label3);
            this.PanelDetail.Location = new System.Drawing.Point(4, 200);
            this.PanelDetail.Name = "PanelDetail";
            this.PanelDetail.Size = new System.Drawing.Size(1776, 730);
            this.PanelDetail.TabIndex = 13;
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
            this.ckM_Label11.Location = new System.Drawing.Point(921, 67);
            this.ckM_Label11.Name = "ckM_Label11";
            this.ckM_Label11.Size = new System.Drawing.Size(33, 12);
            this.ckM_Label11.TabIndex = 13;
            this.ckM_Label11.Text = ".exe";
            this.ckM_Label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFileName
            // 
            this.txtFileName.AllowMinus = false;
            this.txtFileName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtFileName.BackColor = System.Drawing.Color.White;
            this.txtFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFileName.ClientColor = System.Drawing.Color.White;
            this.txtFileName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtFileName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtFileName.DecimalPlace = 0;
            this.txtFileName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtFileName.IntegerPart = 0;
            this.txtFileName.IsCorrectDate = true;
            this.txtFileName.isEnterKeyDown = false;
            this.txtFileName.IsNumber = true;
            this.txtFileName.IsShop = false;
            this.txtFileName.Length = 50;
            this.txtFileName.Location = new System.Drawing.Point(166, 158);
            this.txtFileName.MaxLength = 50;
            this.txtFileName.MoveNext = true;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(750, 19);
            this.txtFileName.TabIndex = 15;
            this.txtFileName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // txtFilePass
            // 
            this.txtFilePass.AllowMinus = false;
            this.txtFilePass.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtFilePass.BackColor = System.Drawing.Color.White;
            this.txtFilePass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFilePass.ClientColor = System.Drawing.Color.White;
            this.txtFilePass.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtFilePass.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtFilePass.DecimalPlace = 0;
            this.txtFilePass.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtFilePass.IntegerPart = 0;
            this.txtFilePass.IsCorrectDate = true;
            this.txtFilePass.isEnterKeyDown = false;
            this.txtFilePass.IsNumber = true;
            this.txtFilePass.IsShop = false;
            this.txtFilePass.Length = 100;
            this.txtFilePass.Location = new System.Drawing.Point(166, 136);
            this.txtFilePass.MaxLength = 100;
            this.txtFilePass.MoveNext = true;
            this.txtFilePass.Name = "txtFilePass";
            this.txtFilePass.Size = new System.Drawing.Size(750, 19);
            this.txtFilePass.TabIndex = 13;
            this.txtFilePass.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // txtFileDrive
            // 
            this.txtFileDrive.AllowMinus = false;
            this.txtFileDrive.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtFileDrive.BackColor = System.Drawing.Color.White;
            this.txtFileDrive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFileDrive.ClientColor = System.Drawing.Color.White;
            this.txtFileDrive.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtFileDrive.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtFileDrive.DecimalPlace = 0;
            this.txtFileDrive.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtFileDrive.IntegerPart = 0;
            this.txtFileDrive.IsCorrectDate = true;
            this.txtFileDrive.isEnterKeyDown = false;
            this.txtFileDrive.IsNumber = true;
            this.txtFileDrive.IsShop = false;
            this.txtFileDrive.Length = 1;
            this.txtFileDrive.Location = new System.Drawing.Point(166, 115);
            this.txtFileDrive.MaxLength = 1;
            this.txtFileDrive.MoveNext = true;
            this.txtFileDrive.Name = "txtFileDrive";
            this.txtFileDrive.Size = new System.Drawing.Size(50, 19);
            this.txtFileDrive.TabIndex = 12;
            this.txtFileDrive.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // txtExeName
            // 
            this.txtExeName.AllowMinus = false;
            this.txtExeName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtExeName.BackColor = System.Drawing.Color.White;
            this.txtExeName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExeName.ClientColor = System.Drawing.Color.White;
            this.txtExeName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtExeName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtExeName.DecimalPlace = 0;
            this.txtExeName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtExeName.IntegerPart = 0;
            this.txtExeName.IsCorrectDate = true;
            this.txtExeName.isEnterKeyDown = false;
            this.txtExeName.IsNumber = true;
            this.txtExeName.IsShop = false;
            this.txtExeName.Length = 100;
            this.txtExeName.Location = new System.Drawing.Point(167, 63);
            this.txtExeName.MaxLength = 100;
            this.txtExeName.MoveNext = true;
            this.txtExeName.Name = "txtExeName";
            this.txtExeName.Size = new System.Drawing.Size(750, 19);
            this.txtExeName.TabIndex = 11;
            this.txtExeName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // cboType
            // 
            this.cboType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboType.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.Default;
            this.cboType.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboType.Flag = 0;
            this.cboType.FormattingEnabled = true;
            this.cboType.Length = 10;
            this.cboType.Location = new System.Drawing.Point(166, 36);
            this.cboType.MaxLength = 10;
            this.cboType.MoveNext = true;
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(121, 20);
            this.cboType.TabIndex = 10;
            // 
            // txtProgramName
            // 
            this.txtProgramName.AllowMinus = false;
            this.txtProgramName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtProgramName.BackColor = System.Drawing.Color.White;
            this.txtProgramName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProgramName.ClientColor = System.Drawing.Color.White;
            this.txtProgramName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtProgramName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtProgramName.DecimalPlace = 0;
            this.txtProgramName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtProgramName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtProgramName.IntegerPart = 0;
            this.txtProgramName.IsCorrectDate = true;
            this.txtProgramName.isEnterKeyDown = false;
            this.txtProgramName.IsNumber = true;
            this.txtProgramName.IsShop = false;
            this.txtProgramName.Length = 100;
            this.txtProgramName.Location = new System.Drawing.Point(166, 11);
            this.txtProgramName.MaxLength = 100;
            this.txtProgramName.MoveNext = true;
            this.txtProgramName.Name = "txtProgramName";
            this.txtProgramName.Size = new System.Drawing.Size(750, 19);
            this.txtProgramName.TabIndex = 9;
            this.txtProgramName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_Label9
            // 
            this.ckM_Label9.AutoSize = true;
            this.ckM_Label9.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label9.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label9.DefaultlabelSize = true;
            this.ckM_Label9.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label9.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label9.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label9.Location = new System.Drawing.Point(93, 162);
            this.ckM_Label9.Name = "ckM_Label9";
            this.ckM_Label9.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label9.TabIndex = 6;
            this.ckM_Label9.Text = "ファイル名";
            this.ckM_Label9.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label8.Location = new System.Drawing.Point(106, 139);
            this.ckM_Label8.Name = "ckM_Label8";
            this.ckM_Label8.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label8.TabIndex = 5;
            this.ckM_Label8.Text = "作成パス";
            this.ckM_Label8.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label7.Location = new System.Drawing.Point(106, 119);
            this.ckM_Label7.Name = "ckM_Label7";
            this.ckM_Label7.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label7.TabIndex = 4;
            this.ckM_Label7.Text = "ドライブ";
            this.ckM_Label7.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label6.Location = new System.Drawing.Point(80, 99);
            this.ckM_Label6.Name = "ckM_Label6";
            this.ckM_Label6.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label6.TabIndex = 3;
            this.ckM_Label6.Text = "ファイル作成";
            this.ckM_Label6.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label5.Location = new System.Drawing.Point(124, 67);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(39, 12);
            this.ckM_Label5.TabIndex = 2;
            this.ckM_Label5.Text = "Exe名";
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
            this.ckM_Label4.Location = new System.Drawing.Point(119, 41);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label4.TabIndex = 1;
            this.ckM_Label4.Text = "タイプ";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label3.Location = new System.Drawing.Point(80, 15);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label3.TabIndex = 0;
            this.ckM_Label3.Text = "プログラム名";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label1.Location = new System.Drawing.Point(43, 463);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(84, 12);
            this.ckM_Label1.TabIndex = 2;
            this.ckM_Label1.Text = "プログラムID";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label2.Location = new System.Drawing.Point(3, 13);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(110, 12);
            this.ckM_Label2.TabIndex = 4;
            this.ckM_Label2.Text = "複写プログラムID";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.btnDisplay.Location = new System.Drawing.Point(390, 3);
            this.btnDisplay.Margin = new System.Windows.Forms.Padding(1);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(130, 28);
            this.btnDisplay.TabIndex = 0;
            this.btnDisplay.Text = "表示(F11)";
            this.btnDisplay.UseVisualStyleBackColor = false;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // PanelNormal
            // 
            this.PanelNormal.Controls.Add(this.ckM_Label10);
            this.PanelNormal.Location = new System.Drawing.Point(63, 13);
            this.PanelNormal.Name = "PanelNormal";
            this.PanelNormal.Size = new System.Drawing.Size(930, 35);
            this.PanelNormal.TabIndex = 4;
            this.PanelNormal.Enter += new System.EventHandler(this.PanelNormal_Enter);
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
            this.ckM_Label10.Location = new System.Drawing.Point(15, 10);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(84, 12);
            this.ckM_Label10.TabIndex = 0;
            this.ckM_Label10.Text = "プログラムID";
            this.ckM_Label10.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PanelCopy
            // 
            this.PanelCopy.Controls.Add(this.ckM_Label2);
            this.PanelCopy.Location = new System.Drawing.Point(788, 56);
            this.PanelCopy.Name = "PanelCopy";
            this.PanelCopy.Size = new System.Drawing.Size(930, 35);
            this.PanelCopy.TabIndex = 5;
            this.PanelCopy.Enter += new System.EventHandler(this.PanelCopy_Enter);
            // 
            // scProgramCopy
            // 
            this.scProgramCopy.AutoSize = true;
            this.scProgramCopy.ChangeDate = "";
            this.scProgramCopy.ChangeDateWidth = 100;
            this.scProgramCopy.Code = "";
            this.scProgramCopy.CodeWidth = 750;
            this.scProgramCopy.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.scProgramCopy.DataCheck = false;
            this.scProgramCopy.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.scProgramCopy.IsCopy = false;
            this.scProgramCopy.LabelText = "";
            this.scProgramCopy.LabelVisible = false;
            this.scProgramCopy.Location = new System.Drawing.Point(902, 60);
            this.scProgramCopy.Margin = new System.Windows.Forms.Padding(0);
            this.scProgramCopy.Name = "scProgramCopy";
            this.scProgramCopy.SearchEnable = true;
            this.scProgramCopy.Size = new System.Drawing.Size(783, 27);
            this.scProgramCopy.Stype = Search.CKM_SearchControl.SearchType.プログラムID;
            this.scProgramCopy.TabIndex = 10;
            this.scProgramCopy.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.scProgramCopy.UseChangeDate = false;
            this.scProgramCopy.Value1 = null;
            this.scProgramCopy.Value2 = null;
            this.scProgramCopy.Value3 = null;
            this.scProgramCopy.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.scProgramCopy_CodeKeyDownEvent);
            // 
            // scProgramID
            // 
            this.scProgramID.AutoSize = true;
            this.scProgramID.ChangeDate = "";
            this.scProgramID.ChangeDateWidth = 100;
            this.scProgramID.Code = "";
            this.scProgramID.CodeWidth = 750;
            this.scProgramID.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.scProgramID.DataCheck = false;
            this.scProgramID.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.scProgramID.IsCopy = false;
            this.scProgramID.LabelText = "";
            this.scProgramID.LabelVisible = false;
            this.scProgramID.Location = new System.Drawing.Point(167, 16);
            this.scProgramID.Margin = new System.Windows.Forms.Padding(0);
            this.scProgramID.Name = "scProgramID";
            this.scProgramID.SearchEnable = true;
            this.scProgramID.Size = new System.Drawing.Size(783, 28);
            this.scProgramID.Stype = Search.CKM_SearchControl.SearchType.プログラムID;
            this.scProgramID.TabIndex = 6;
            this.scProgramID.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.scProgramID.UseChangeDate = false;
            this.scProgramID.Value1 = null;
            this.scProgramID.Value2 = null;
            this.scProgramID.Value3 = null;
            this.scProgramID.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.scProgramID_CodeKeyDownEvent);
            // 
            // ckM_SearchControl1
            // 
            this.ckM_SearchControl1.AutoSize = true;
            this.ckM_SearchControl1.ChangeDate = "";
            this.ckM_SearchControl1.ChangeDateWidth = 100;
            this.ckM_SearchControl1.Code = "";
            this.ckM_SearchControl1.CodeWidth = 100;
            this.ckM_SearchControl1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_SearchControl1.DataCheck = false;
            this.ckM_SearchControl1.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ckM_SearchControl1.IsCopy = false;
            this.ckM_SearchControl1.LabelText = "";
            this.ckM_SearchControl1.LabelVisible = false;
            this.ckM_SearchControl1.Location = new System.Drawing.Point(138, 455);
            this.ckM_SearchControl1.Margin = new System.Windows.Forms.Padding(0);
            this.ckM_SearchControl1.Name = "ckM_SearchControl1";
            this.ckM_SearchControl1.SearchEnable = true;
            this.ckM_SearchControl1.Size = new System.Drawing.Size(133, 50);
            this.ckM_SearchControl1.Stype = Search.CKM_SearchControl.SearchType.Default;
            this.ckM_SearchControl1.TabIndex = 3;
            this.ckM_SearchControl1.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ckM_SearchControl1.UseChangeDate = true;
            this.ckM_SearchControl1.Value1 = null;
            this.ckM_SearchControl1.Value2 = null;
            this.ckM_SearchControl1.Value3 = null;
            // 
            // MasterTouroku_Program
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1776, 961);
            this.Controls.Add(this.PanelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "MasterTouroku_Program";
            this.PanelHeaderHeight = 200;
            this.Text = "MasterTouroku_Program";
            this.Load += new System.EventHandler(this.MasterTouroku_Program_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MasterTouroku_Program_KeyUp);
            this.Controls.SetChildIndex(this.PanelDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            this.PanelSearch.ResumeLayout(false);
            this.PanelDetail.ResumeLayout(false);
            this.PanelDetail.PerformLayout();
            this.PanelNormal.ResumeLayout(false);
            this.PanelNormal.PerformLayout();
            this.PanelCopy.ResumeLayout(false);
            this.PanelCopy.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PanelDetail;
        private Search.CKM_SearchControl ckM_SearchControl1;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Button btnDisplay;
        private CKM_Controls.CKM_Label ckM_Label9;
        private CKM_Controls.CKM_Label ckM_Label8;
        private CKM_Controls.CKM_Label ckM_Label7;
        private CKM_Controls.CKM_Label ckM_Label6;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_TextBox txtExeName;
        private CKM_Controls.CKM_ComboBox cboType;
        private CKM_Controls.CKM_TextBox txtProgramName;
        private System.Windows.Forms.Panel PanelNormal;
        private System.Windows.Forms.Panel PanelCopy;
        private Search.CKM_SearchControl scProgramID;
        private CKM_Controls.CKM_Label ckM_Label10;
        private Search.CKM_SearchControl scProgramCopy;
        private CKM_Controls.CKM_TextBox txtFileName;
        private CKM_Controls.CKM_TextBox txtFilePass;
        private CKM_Controls.CKM_TextBox txtFileDrive;
        private CKM_Controls.CKM_Label ckM_Label11;
    }
}

