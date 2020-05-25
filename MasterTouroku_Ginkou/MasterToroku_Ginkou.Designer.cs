namespace MasterTouroku_Ginkou
{
    partial class frmMasterTouroku_Ginkou
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
            this.ginKou_CD = new Search.CKM_SearchControl();
            this.copy_ginKou_CD = new Search.CKM_SearchControl();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.銀行名 = new CKM_Controls.CKM_Label();
            this.ginko_name = new CKM_Controls.CKM_TextBox();
            this.ckM_Label5 = new CKM_Controls.CKM_Label();
            this.ginko_kananame = new CKM_Controls.CKM_TextBox();
            this.備考 = new CKM_Controls.CKM_Label();
            this.ginko_remarks = new CKM_Controls.CKM_MultiLineTextBox();
            this.ChkDeleteFlg = new CKM_Controls.CKM_CheckBox();
            this.BtnF11Show = new CKM_Controls.CKM_Button();
            this.PanelDetail = new System.Windows.Forms.Panel();
            this.ginko_useflg = new CKM_Controls.CKM_Label();
            this.panelNormal = new System.Windows.Forms.Panel();
            this.panelCopy = new System.Windows.Forms.Panel();
            this.PanelHeader.SuspendLayout();
            this.PanelSearch.SuspendLayout();
            this.PanelDetail.SuspendLayout();
            this.panelNormal.SuspendLayout();
            this.panelCopy.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.panelCopy);
            this.PanelHeader.Controls.Add(this.panelNormal);
            this.PanelHeader.Size = new System.Drawing.Size(1774, 91);
            this.PanelHeader.TabIndex = 1;
            this.PanelHeader.Controls.SetChildIndex(this.panelNormal, 0);
            this.PanelHeader.Controls.SetChildIndex(this.panelCopy, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Controls.Add(this.BtnF11Show);
            this.PanelSearch.Location = new System.Drawing.Point(1240, 0);
            this.PanelSearch.TabIndex = 0;
            // 
            // ginKou_CD
            // 
            this.ginKou_CD.AutoSize = true;
            this.ginKou_CD.ChangeDate = "";
            this.ginKou_CD.ChangeDateWidth = 100;
            this.ginKou_CD.Code = "";
            this.ginKou_CD.CodeWidth = 40;
            this.ginKou_CD.CodeWidth1 = 40;
            this.ginKou_CD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ginKou_CD.DataCheck = false;
            this.ginKou_CD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ginKou_CD.IsCopy = false;
            this.ginKou_CD.LabelText = "";
            this.ginKou_CD.LabelVisible = false;
            this.ginKou_CD.Location = new System.Drawing.Point(61, 3);
            this.ginKou_CD.Margin = new System.Windows.Forms.Padding(0);
            this.ginKou_CD.Name = "ginKou_CD";
            this.ginKou_CD.NameWidth = 350;
            this.ginKou_CD.SearchEnable = true;
            this.ginKou_CD.Size = new System.Drawing.Size(103, 50);
            this.ginKou_CD.Stype = Search.CKM_SearchControl.SearchType.銀行;
            this.ginKou_CD.TabIndex = 1;
            this.ginKou_CD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ginKou_CD.UseChangeDate = true;
            this.ginKou_CD.Value1 = null;
            this.ginKou_CD.Value2 = null;
            this.ginKou_CD.Value3 = null;
            this.ginKou_CD.ChangeDateKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ginKou_CD_ChangeDateKeyDownEvent);
            // 
            // copy_ginKou_CD
            // 
            this.copy_ginKou_CD.AutoSize = true;
            this.copy_ginKou_CD.ChangeDate = "";
            this.copy_ginKou_CD.ChangeDateWidth = 100;
            this.copy_ginKou_CD.Code = "";
            this.copy_ginKou_CD.CodeWidth = 40;
            this.copy_ginKou_CD.CodeWidth1 = 40;
            this.copy_ginKou_CD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.copy_ginKou_CD.DataCheck = false;
            this.copy_ginKou_CD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.copy_ginKou_CD.IsCopy = false;
            this.copy_ginKou_CD.LabelText = "";
            this.copy_ginKou_CD.LabelVisible = false;
            this.copy_ginKou_CD.Location = new System.Drawing.Point(83, 3);
            this.copy_ginKou_CD.Margin = new System.Windows.Forms.Padding(0);
            this.copy_ginKou_CD.Name = "copy_ginKou_CD";
            this.copy_ginKou_CD.NameWidth = 350;
            this.copy_ginKou_CD.SearchEnable = true;
            this.copy_ginKou_CD.Size = new System.Drawing.Size(103, 50);
            this.copy_ginKou_CD.Stype = Search.CKM_SearchControl.SearchType.銀行;
            this.copy_ginKou_CD.TabIndex = 2;
            this.copy_ginKou_CD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.copy_ginKou_CD.UseChangeDate = true;
            this.copy_ginKou_CD.Value1 = null;
            this.copy_ginKou_CD.Value2 = null;
            this.copy_ginKou_CD.Value3 = null;
            this.copy_ginKou_CD.ChangeDateKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.copy_ginKou_CD_ChangeDateKeyDownEvent);
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
            this.ckM_Label1.Location = new System.Drawing.Point(13, 11);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(45, 12);
            this.ckM_Label1.TabIndex = 3;
            this.ckM_Label1.Text = "銀行CD";
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
            this.ckM_Label2.Location = new System.Drawing.Point(14, 35);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label2.TabIndex = 4;
            this.ckM_Label2.Text = "改定日";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label3.Location = new System.Drawing.Point(9, 12);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(71, 12);
            this.ckM_Label3.TabIndex = 5;
            this.ckM_Label3.Text = "複写銀行CD";
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
            this.ckM_Label4.Location = new System.Drawing.Point(36, 35);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label4.TabIndex = 6;
            this.ckM_Label4.Text = "改定日";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // 銀行名
            // 
            this.銀行名.AutoSize = true;
            this.銀行名.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.銀行名.BackColor = System.Drawing.Color.Transparent;
            this.銀行名.DefaultlabelSize = true;
            this.銀行名.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.銀行名.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.銀行名.ForeColor = System.Drawing.Color.Black;
            this.銀行名.Location = new System.Drawing.Point(77, 11);
            this.銀行名.Name = "銀行名";
            this.銀行名.Size = new System.Drawing.Size(44, 12);
            this.銀行名.TabIndex = 9;
            this.銀行名.Text = "銀行名";
            this.銀行名.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.銀行名.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ginko_name
            // 
            this.ginko_name.AllowMinus = false;
            this.ginko_name.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ginko_name.BackColor = System.Drawing.Color.White;
            this.ginko_name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ginko_name.ClientColor = System.Drawing.Color.White;
            this.ginko_name.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.ginko_name.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ginko_name.DecimalPlace = 0;
            this.ginko_name.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ginko_name.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.ginko_name.IntegerPart = 0;
            this.ginko_name.IsCorrectDate = true;
            this.ginko_name.isEnterKeyDown = false;
            this.ginko_name.isMaxLengthErr = false;
            this.ginko_name.IsNumber = true;
            this.ginko_name.IsShop = false;
            this.ginko_name.Length = 30;
            this.ginko_name.Location = new System.Drawing.Point(123, 7);
            this.ginko_name.MaxLength = 30;
            this.ginko_name.MoveNext = true;
            this.ginko_name.Name = "ginko_name";
            this.ginko_name.Size = new System.Drawing.Size(186, 19);
            this.ginko_name.TabIndex = 0;
            this.ginko_name.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_Label5.Location = new System.Drawing.Point(76, 41);
            this.ckM_Label5.Name = "ckM_Label5";
            this.ckM_Label5.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label5.TabIndex = 11;
            this.ckM_Label5.Text = "カナ名";
            this.ckM_Label5.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ginko_kananame
            // 
            this.ginko_kananame.AllowMinus = false;
            this.ginko_kananame.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ginko_kananame.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ginko_kananame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ginko_kananame.ClientColor = System.Drawing.Color.White;
            this.ginko_kananame.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ginko_kananame.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ginko_kananame.DecimalPlace = 0;
            this.ginko_kananame.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ginko_kananame.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ginko_kananame.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.ginko_kananame.IntegerPart = 0;
            this.ginko_kananame.IsCorrectDate = true;
            this.ginko_kananame.isEnterKeyDown = false;
            this.ginko_kananame.isMaxLengthErr = false;
            this.ginko_kananame.IsNumber = true;
            this.ginko_kananame.IsShop = false;
            this.ginko_kananame.Length = 30;
            this.ginko_kananame.Location = new System.Drawing.Point(124, 38);
            this.ginko_kananame.MaxLength = 30;
            this.ginko_kananame.MoveNext = true;
            this.ginko_kananame.Name = "ginko_kananame";
            this.ginko_kananame.Size = new System.Drawing.Size(183, 19);
            this.ginko_kananame.TabIndex = 1;
            this.ginko_kananame.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // 備考
            // 
            this.備考.AutoSize = true;
            this.備考.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.備考.BackColor = System.Drawing.Color.Transparent;
            this.備考.DefaultlabelSize = true;
            this.備考.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.備考.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.備考.ForeColor = System.Drawing.Color.Black;
            this.備考.Location = new System.Drawing.Point(89, 72);
            this.備考.Name = "備考";
            this.備考.Size = new System.Drawing.Size(31, 12);
            this.備考.TabIndex = 13;
            this.備考.Text = "備考";
            this.備考.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.備考.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ginko_remarks
            // 
            this.ginko_remarks.Back_Color = CKM_Controls.CKM_MultiLineTextBox.CKM_Color.White;
            this.ginko_remarks.BackColor = System.Drawing.SystemColors.Window;
            this.ginko_remarks.Ctrl_Byte = CKM_Controls.CKM_MultiLineTextBox.Bytes.半全角;
            this.ginko_remarks.F_focus = false;
            this.ginko_remarks.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ginko_remarks.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.ginko_remarks.Length = 500;
            this.ginko_remarks.Location = new System.Drawing.Point(123, 69);
            this.ginko_remarks.MaxLength = 500;
            this.ginko_remarks.Mdea = false;
            this.ginko_remarks.Mfocus = false;
            this.ginko_remarks.MoveNext = true;
            this.ginko_remarks.Multiline = true;
            this.ginko_remarks.Name = "ginko_remarks";
            this.ginko_remarks.RowCount = 5;
            this.ginko_remarks.Size = new System.Drawing.Size(620, 95);
            this.ginko_remarks.TabIndex = 2;
            this.ginko_remarks.TextSize = CKM_Controls.CKM_MultiLineTextBox.FontSize.Normal;
            // 
            // ChkDeleteFlg
            // 
            this.ChkDeleteFlg.AutoSize = true;
            this.ChkDeleteFlg.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ChkDeleteFlg.Location = new System.Drawing.Point(780, 11);
            this.ChkDeleteFlg.Name = "ChkDeleteFlg";
            this.ChkDeleteFlg.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ChkDeleteFlg.Size = new System.Drawing.Size(50, 16);
            this.ChkDeleteFlg.TabIndex = 3;
            this.ChkDeleteFlg.Text = "削除";
            this.ChkDeleteFlg.UseVisualStyleBackColor = true;
            // 
            // BtnF11Show
            // 
            this.BtnF11Show.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BtnF11Show.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BtnF11Show.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnF11Show.DefaultBtnSize = true;
            this.BtnF11Show.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnF11Show.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnF11Show.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.BtnF11Show.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BtnF11Show.Location = new System.Drawing.Point(410, 2);
            this.BtnF11Show.Margin = new System.Windows.Forms.Padding(1);
            this.BtnF11Show.Name = "BtnF11Show";
            this.BtnF11Show.Size = new System.Drawing.Size(118, 28);
            this.BtnF11Show.TabIndex = 0;
            this.BtnF11Show.Text = "表示(F11)";
            this.BtnF11Show.UseVisualStyleBackColor = false;
            this.BtnF11Show.Click += new System.EventHandler(this.BtnF11Show_Click);
            // 
            // PanelDetail
            // 
            this.PanelDetail.Controls.Add(this.ginko_remarks);
            this.PanelDetail.Controls.Add(this.ginko_useflg);
            this.PanelDetail.Controls.Add(this.備考);
            this.PanelDetail.Controls.Add(this.ChkDeleteFlg);
            this.PanelDetail.Controls.Add(this.ginko_name);
            this.PanelDetail.Controls.Add(this.銀行名);
            this.PanelDetail.Controls.Add(this.ginko_kananame);
            this.PanelDetail.Controls.Add(this.ckM_Label5);
            this.PanelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelDetail.Location = new System.Drawing.Point(0, 147);
            this.PanelDetail.Name = "PanelDetail";
            this.PanelDetail.Size = new System.Drawing.Size(1776, 708);
            this.PanelDetail.TabIndex = 2;
            this.PanelDetail.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelDetail_Paint);
            // 
            // ginko_useflg
            // 
            this.ginko_useflg.AutoSize = true;
            this.ginko_useflg.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ginko_useflg.BackColor = System.Drawing.Color.Transparent;
            this.ginko_useflg.DefaultlabelSize = true;
            this.ginko_useflg.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ginko_useflg.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ginko_useflg.ForeColor = System.Drawing.Color.Black;
            this.ginko_useflg.Location = new System.Drawing.Point(1666, 40);
            this.ginko_useflg.Name = "ginko_useflg";
            this.ginko_useflg.Size = new System.Drawing.Size(65, 12);
            this.ginko_useflg.TabIndex = 10;
            this.ginko_useflg.Text = "使用済FLG";
            this.ginko_useflg.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ginko_useflg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ginko_useflg.Visible = false;
            // 
            // panelNormal
            // 
            this.panelNormal.Controls.Add(this.ginKou_CD);
            this.panelNormal.Controls.Add(this.ckM_Label1);
            this.panelNormal.Controls.Add(this.ckM_Label2);
            this.panelNormal.Location = new System.Drawing.Point(63, 3);
            this.panelNormal.Name = "panelNormal";
            this.panelNormal.Size = new System.Drawing.Size(200, 60);
            this.panelNormal.TabIndex = 7;
            // 
            // panelCopy
            // 
            this.panelCopy.Controls.Add(this.copy_ginKou_CD);
            this.panelCopy.Controls.Add(this.ckM_Label3);
            this.panelCopy.Controls.Add(this.ckM_Label4);
            this.panelCopy.Location = new System.Drawing.Point(539, 3);
            this.panelCopy.Name = "panelCopy";
            this.panelCopy.Size = new System.Drawing.Size(200, 60);
            this.panelCopy.TabIndex = 8;
            // 
            // frmMasterTouroku_Ginkou
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1776, 887);
            this.Controls.Add(this.PanelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "frmMasterTouroku_Ginkou";
            this.Text = "MasterTouroku_Ginkou";
            this.Load += new System.EventHandler(this.frmMasterTouroku_Ginkou_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmMasterTouroku_Ginkou_KeyUp);
            this.Controls.SetChildIndex(this.PanelDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelSearch.ResumeLayout(false);
            this.PanelDetail.ResumeLayout(false);
            this.PanelDetail.PerformLayout();
            this.panelNormal.ResumeLayout(false);
            this.panelNormal.PerformLayout();
            this.panelCopy.ResumeLayout(false);
            this.panelCopy.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Label ckM_Label1;
        private Search.CKM_SearchControl copy_ginKou_CD;
        private Search.CKM_SearchControl ginKou_CD;
        private CKM_Controls.CKM_Label 銀行名;
        private CKM_Controls.CKM_TextBox ginko_name;
        private CKM_Controls.CKM_Label ckM_Label5;
        private CKM_Controls.CKM_TextBox ginko_kananame;
        private CKM_Controls.CKM_Label 備考;
        private CKM_Controls.CKM_MultiLineTextBox ginko_remarks;
        private CKM_Controls.CKM_Button BtnF11Show;
        private CKM_Controls.CKM_CheckBox ChkDeleteFlg;
        private System.Windows.Forms.Panel PanelDetail;
        private CKM_Controls.CKM_Label ginko_useflg;
        private System.Windows.Forms.Panel panelCopy;
        private System.Windows.Forms.Panel panelNormal;
    }
}

