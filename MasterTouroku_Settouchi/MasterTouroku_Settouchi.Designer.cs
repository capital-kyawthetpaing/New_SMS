namespace MasterTouroku_Settouchi
{
    partial class frmMasterTouroku_Settouchi
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
            this.txtDate = new CKM_Controls.CKM_TextBox();
            this.cboSeqKBN = new CKM_Controls.CKM_ComboBox();
            this.cboStore = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.panelDetail = new System.Windows.Forms.Panel();
            this.txtPrefix = new CKM_Controls.CKM_TextBox();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.BtnF11Show = new CKM_Controls.CKM_Button();
            this.PanelHeader.SuspendLayout();
            this.PanelSearch.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.panel1);
            this.PanelHeader.Size = new System.Drawing.Size(1711, 94);
            this.PanelHeader.Controls.SetChildIndex(this.panel1, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Controls.Add(this.BtnF11Show);
            this.PanelSearch.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtDate);
            this.panel1.Controls.Add(this.cboSeqKBN);
            this.panel1.Controls.Add(this.cboStore);
            this.panel1.Controls.Add(this.ckM_Label3);
            this.panel1.Controls.Add(this.ckM_Label2);
            this.panel1.Controls.Add(this.ckM_Label1);
            this.panel1.Location = new System.Drawing.Point(42, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 80);
            this.panel1.TabIndex = 0;
            // 
            // txtDate
            // 
            this.txtDate.AllowMinus = false;
            this.txtDate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtDate.BackColor = System.Drawing.Color.White;
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtDate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtDate.DecimalPlace = 0;
            this.txtDate.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtDate.IntegerPart = 0;
            this.txtDate.IsCorrectDate = true;
            this.txtDate.isEnterKeyDown = false;
            this.txtDate.IsNumber = true;
            this.txtDate.IsShop = false;
            this.txtDate.Length = 10;
            this.txtDate.Location = new System.Drawing.Point(80, 55);
            this.txtDate.MaxLength = 10;
            this.txtDate.MoveNext = true;
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(100, 19);
            this.txtDate.TabIndex = 2;
            this.txtDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDate_KeyDown);
            // 
            // cboSeqKBN
            // 
            this.cboSeqKBN.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboSeqKBN.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSeqKBN.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.データ種別;
            this.cboSeqKBN.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.cboSeqKBN.FormattingEnabled = true;
            this.cboSeqKBN.Length = 10;
            this.cboSeqKBN.Location = new System.Drawing.Point(80, 29);
            this.cboSeqKBN.MaxLength = 5;
            this.cboSeqKBN.MoveNext = true;
            this.cboSeqKBN.Name = "cboSeqKBN";
            this.cboSeqKBN.Size = new System.Drawing.Size(200, 20);
            this.cboSeqKBN.TabIndex = 1;
            // 
            // cboStore
            // 
            this.cboStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStore.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.店舗ストア;
            this.cboStore.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.cboStore.FormattingEnabled = true;
            this.cboStore.Length = 10;
            this.cboStore.Location = new System.Drawing.Point(80, 3);
            this.cboStore.MaxLength = 5;
            this.cboStore.MoveNext = true;
            this.cboStore.Name = "cboStore";
            this.cboStore.Size = new System.Drawing.Size(200, 20);
            this.cboStore.TabIndex = 0;
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
            this.ckM_Label3.Location = new System.Drawing.Point(34, 58);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label3.TabIndex = 0;
            this.ckM_Label3.Text = "改定日";
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
            this.ckM_Label2.Location = new System.Drawing.Point(8, 33);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label2.TabIndex = 1;
            this.ckM_Label2.Text = "データ種別";
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
            this.ckM_Label1.Location = new System.Drawing.Point(47, 8);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label1.TabIndex = 0;
            this.ckM_Label1.Text = "店舗";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.txtPrefix);
            this.panelDetail.Controls.Add(this.ckM_Label4);
            this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetail.Location = new System.Drawing.Point(0, 150);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(1713, 779);
            this.panelDetail.TabIndex = 1;
            // 
            // txtPrefix
            // 
            this.txtPrefix.AllowMinus = false;
            this.txtPrefix.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPrefix.BackColor = System.Drawing.Color.White;
            this.txtPrefix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPrefix.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtPrefix.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtPrefix.DecimalPlace = 0;
            this.txtPrefix.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtPrefix.IntegerPart = 0;
            this.txtPrefix.IsCorrectDate = true;
            this.txtPrefix.isEnterKeyDown = false;
            this.txtPrefix.IsNumber = true;
            this.txtPrefix.IsShop = false;
            this.txtPrefix.Length = 10;
            this.txtPrefix.Location = new System.Drawing.Point(123, 8);
            this.txtPrefix.MaxLength = 3;
            this.txtPrefix.MoveNext = true;
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(50, 19);
            this.txtPrefix.TabIndex = 1;
            this.txtPrefix.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtPrefix.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPrefix_KeyDown);
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
            this.ckM_Label4.Location = new System.Drawing.Point(77, 12);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label4.TabIndex = 0;
            this.ckM_Label4.Text = "接頭値";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.BtnF11Show.Location = new System.Drawing.Point(407, 2);
            this.BtnF11Show.Margin = new System.Windows.Forms.Padding(1);
            this.BtnF11Show.Name = "BtnF11Show";
            this.BtnF11Show.Size = new System.Drawing.Size(118, 28);
            this.BtnF11Show.TabIndex = 0;
            this.BtnF11Show.Text = "表示(F11)";
            this.BtnF11Show.UseVisualStyleBackColor = false;
            this.BtnF11Show.Click += new System.EventHandler(this.BtnF11Show_Click);
            // 
            // frmMasterTouroku_Settouchi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.panelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "frmMasterTouroku_Settouchi";
            this.PanelHeaderHeight = 150;
            this.Text = "MasterTouroku_Settouchi";
            this.Load += new System.EventHandler(this.MasterTouroku_Settouchi_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmMasterTouroku_Settouchi_KeyUp);
            this.Controls.SetChildIndex(this.panelDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelSearch.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelDetail.ResumeLayout(false);
            this.panelDetail.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelDetail;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_Button BtnF11Show;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_ComboBox cboSeqKBN;
        private CKM_Controls.CKM_ComboBox cboStore;
        private CKM_Controls.CKM_TextBox txtPrefix;
        private CKM_Controls.CKM_TextBox txtDate;
    }
}