namespace MasterTouroku_Brand
{
    partial class FrmMasterTouroku_Brand
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
            this.ブランドCD = new CKM_Controls.CKM_Label();
            this.ScBrandCD = new Search.CKM_SearchControl();
            this.PanelCopy = new System.Windows.Forms.Panel();
            this.複写ブランドCD = new CKM_Controls.CKM_Label();
            this.ScCopyBrand = new Search.CKM_SearchControl();
            this.PanelDetail = new System.Windows.Forms.Panel();
            this.txtKanaName = new CKM_Controls.CKM_TextBox();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.txtBrandName = new CKM_Controls.CKM_TextBox();
            this.btnDisplay = new CKM_Controls.CKM_Button();
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
            this.PanelHeader.Size = new System.Drawing.Size(1774, 74);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.Controls.SetChildIndex(this.PanelNormal, 0);
            this.PanelHeader.Controls.SetChildIndex(this.PanelCopy, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Controls.Add(this.btnDisplay);
            this.PanelSearch.Location = new System.Drawing.Point(1240, 0);
            this.PanelSearch.TabIndex = 0;
            // 
            // PanelNormal
            // 
            this.PanelNormal.Controls.Add(this.ブランドCD);
            this.PanelNormal.Controls.Add(this.ScBrandCD);
            this.PanelNormal.Location = new System.Drawing.Point(25, 3);
            this.PanelNormal.Name = "PanelNormal";
            this.PanelNormal.Size = new System.Drawing.Size(250, 35);
            this.PanelNormal.TabIndex = 0;
            // 
            // ブランドCD
            // 
            this.ブランドCD.AutoSize = true;
            this.ブランドCD.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ブランドCD.BackColor = System.Drawing.Color.Transparent;
            this.ブランドCD.DefaultlabelSize = true;
            this.ブランドCD.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ブランドCD.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ブランドCD.ForeColor = System.Drawing.Color.Black;
            this.ブランドCD.Location = new System.Drawing.Point(32, 9);
            this.ブランドCD.Name = "ブランドCD";
            this.ブランドCD.Size = new System.Drawing.Size(71, 12);
            this.ブランドCD.TabIndex = 0;
            this.ブランドCD.Text = "ブランドCD";
            this.ブランドCD.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ブランドCD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScBrandCD
            // 
            this.ScBrandCD.AutoSize = true;
            this.ScBrandCD.ChangeDate = "";
            this.ScBrandCD.ChangeDateWidth = 100;
            this.ScBrandCD.Code = "";
            this.ScBrandCD.CodeWidth = 100;
            this.ScBrandCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScBrandCD.DataCheck = false;
            this.ScBrandCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScBrandCD.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ScBrandCD.IsCopy = false;
            this.ScBrandCD.LabelText = "";
            this.ScBrandCD.LabelVisible = false;
            this.ScBrandCD.Location = new System.Drawing.Point(106, 1);
            this.ScBrandCD.Margin = new System.Windows.Forms.Padding(0);
            this.ScBrandCD.Name = "ScBrandCD";
            this.ScBrandCD.SearchEnable = true;
            this.ScBrandCD.Size = new System.Drawing.Size(133, 28);
            this.ScBrandCD.Stype = Search.CKM_SearchControl.SearchType.ブランド;
            this.ScBrandCD.TabIndex = 1;
            this.ScBrandCD.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScBrandCD.UseChangeDate = false;
            this.ScBrandCD.Value1 = null;
            this.ScBrandCD.Value2 = null;
            this.ScBrandCD.Value3 = null;
            this.ScBrandCD.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScBrand_CodeKeyDownEvent);
            this.ScBrandCD.Leave += new System.EventHandler(this.ScBrandCD_Leave);
            // 
            // PanelCopy
            // 
            this.PanelCopy.Controls.Add(this.複写ブランドCD);
            this.PanelCopy.Controls.Add(this.ScCopyBrand);
            this.PanelCopy.Location = new System.Drawing.Point(573, 3);
            this.PanelCopy.Name = "PanelCopy";
            this.PanelCopy.Size = new System.Drawing.Size(250, 35);
            this.PanelCopy.TabIndex = 1;
            // 
            // 複写ブランドCD
            // 
            this.複写ブランドCD.AutoSize = true;
            this.複写ブランドCD.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.複写ブランドCD.BackColor = System.Drawing.Color.Transparent;
            this.複写ブランドCD.DefaultlabelSize = true;
            this.複写ブランドCD.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.複写ブランドCD.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.複写ブランドCD.ForeColor = System.Drawing.Color.Black;
            this.複写ブランドCD.Location = new System.Drawing.Point(5, 9);
            this.複写ブランドCD.Name = "複写ブランドCD";
            this.複写ブランドCD.Size = new System.Drawing.Size(97, 12);
            this.複写ブランドCD.TabIndex = 13;
            this.複写ブランドCD.Text = "複写ブランドCD";
            this.複写ブランドCD.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.複写ブランドCD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScCopyBrand
            // 
            this.ScCopyBrand.AutoSize = true;
            this.ScCopyBrand.ChangeDate = "";
            this.ScCopyBrand.ChangeDateWidth = 100;
            this.ScCopyBrand.Code = "";
            this.ScCopyBrand.CodeWidth = 100;
            this.ScCopyBrand.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScCopyBrand.DataCheck = false;
            this.ScCopyBrand.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ScCopyBrand.IsCopy = false;
            this.ScCopyBrand.LabelText = "";
            this.ScCopyBrand.LabelVisible = false;
            this.ScCopyBrand.Location = new System.Drawing.Point(105, 1);
            this.ScCopyBrand.Margin = new System.Windows.Forms.Padding(0);
            this.ScCopyBrand.Name = "ScCopyBrand";
            this.ScCopyBrand.SearchEnable = true;
            this.ScCopyBrand.Size = new System.Drawing.Size(133, 28);
            this.ScCopyBrand.Stype = Search.CKM_SearchControl.SearchType.ブランド;
            this.ScCopyBrand.TabIndex = 1;
            this.ScCopyBrand.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ScCopyBrand.UseChangeDate = false;
            this.ScCopyBrand.Value1 = null;
            this.ScCopyBrand.Value2 = null;
            this.ScCopyBrand.Value3 = null;
            this.ScCopyBrand.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScCopyBrand_CodeKeyDownEvent);
            this.ScCopyBrand.Leave += new System.EventHandler(this.ScCopyBrand_Leave);
            // 
            // PanelDetail
            // 
            this.PanelDetail.Controls.Add(this.txtKanaName);
            this.PanelDetail.Controls.Add(this.ckM_Label2);
            this.PanelDetail.Controls.Add(this.ckM_Label1);
            this.PanelDetail.Controls.Add(this.txtBrandName);
            this.PanelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelDetail.Location = new System.Drawing.Point(0, 130);
            this.PanelDetail.Name = "PanelDetail";
            this.PanelDetail.Size = new System.Drawing.Size(1776, 799);
            this.PanelDetail.TabIndex = 1;
            // 
            // txtKanaName
            // 
            this.txtKanaName.AllowMinus = false;
            this.txtKanaName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtKanaName.BackColor = System.Drawing.Color.White;
            this.txtKanaName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKanaName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtKanaName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtKanaName.DecimalPlace = 0;
            this.txtKanaName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtKanaName.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf;
            this.txtKanaName.IntegerPart = 0;
            this.txtKanaName.IsCorrectDate = true;
            this.txtKanaName.isEnterKeyDown = false;
            this.txtKanaName.IsNumber = true;
            this.txtKanaName.IsShop = false;
            this.txtKanaName.Length = 20;
            this.txtKanaName.Location = new System.Drawing.Point(135, 48);
            this.txtKanaName.MaxLength = 20;
            this.txtKanaName.MoveNext = true;
            this.txtKanaName.Name = "txtKanaName";
            this.txtKanaName.Size = new System.Drawing.Size(130, 19);
            this.txtKanaName.TabIndex = 3;
            this.txtKanaName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_Label2.Location = new System.Drawing.Point(86, 51);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label2.TabIndex = 2;
            this.ckM_Label2.Text = "カナ名";
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
            this.ckM_Label1.Location = new System.Drawing.Point(60, 19);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label1.TabIndex = 1;
            this.ckM_Label1.Text = "ブランド名";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBrandName
            // 
            this.txtBrandName.AllowMinus = false;
            this.txtBrandName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtBrandName.BackColor = System.Drawing.Color.White;
            this.txtBrandName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBrandName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtBrandName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtBrandName.DecimalPlace = 0;
            this.txtBrandName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtBrandName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtBrandName.IntegerPart = 0;
            this.txtBrandName.IsCorrectDate = true;
            this.txtBrandName.isEnterKeyDown = false;
            this.txtBrandName.IsNumber = true;
            this.txtBrandName.IsShop = false;
            this.txtBrandName.Length = 40;
            this.txtBrandName.Location = new System.Drawing.Point(135, 16);
            this.txtBrandName.MaxLength = 40;
            this.txtBrandName.MoveNext = true;
            this.txtBrandName.Name = "txtBrandName";
            this.txtBrandName.Size = new System.Drawing.Size(260, 19);
            this.txtBrandName.TabIndex = 1;
            this.txtBrandName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.btnDisplay.Location = new System.Drawing.Point(406, 3);
            this.btnDisplay.Margin = new System.Windows.Forms.Padding(1);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(118, 28);
            this.btnDisplay.TabIndex = 0;
            this.btnDisplay.Text = "表示(F11)";
            this.btnDisplay.UseVisualStyleBackColor = false;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // FrmMasterTouroku_Brand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1776, 961);
            this.Controls.Add(this.PanelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "FrmMasterTouroku_Brand";
            this.PanelHeaderHeight = 130;
            this.Text = "MasterTouroku_Brand";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmMasterTouroku_Brand_KeyUp);
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
        private System.Windows.Forms.Panel PanelDetail;
        private CKM_Controls.CKM_Button btnDisplay;
        private Search.CKM_SearchControl ScBrandCD;
        private Search.CKM_SearchControl ScCopyBrand;
        private CKM_Controls.CKM_Label 複写ブランドCD;
        private CKM_Controls.CKM_Label ブランドCD;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_TextBox txtBrandName;
        private CKM_Controls.CKM_TextBox txtKanaName;
        private CKM_Controls.CKM_Label ckM_Label2;
    }
}

