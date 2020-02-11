namespace SeikyuuSho
{
    partial class SeikyuuSho
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.label4 = new CKM_Controls.CKM_Label();
            this.label1 = new CKM_Controls.CKM_Label();
            this.label27 = new CKM_Controls.CKM_Label();
            this.CboStoreCD = new CKM_Controls.CKM_ComboBox();
            this.ckM_SearchControl2 = new Search.CKM_SearchControl();
            this.ckM_SearchControl3 = new Search.CKM_SearchControl();
            this.ckM_CustomerName = new CKM_Controls.CKM_TextBox();
            this.ckM_RadioButton3 = new CKM_Controls.CKM_RadioButton();
            this.ScStaff = new Search.CKM_SearchControl();
            this.label9 = new CKM_Controls.CKM_Label();
            this.lblSkuCD = new System.Windows.Forms.Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_RadioButton1 = new CKM_Controls.CKM_RadioButton();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox1 = new CKM_Controls.CKM_TextBox();
            this.ScCustomer = new Search.CKM_SearchControl();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1368, 34);
            this.PanelHeader.TabIndex = 0;
            // 
            // PanelSearch
            // 
            this.PanelSearch.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(1023, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 12);
            this.label4.TabIndex = 255;
            this.label4.Text = "店舗";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.DarkGray;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(28, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(492, 18);
            this.label1.TabIndex = 261;
            this.label1.Text = "商品名";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.BackColor = System.Drawing.Color.Transparent;
            this.label27.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label27.Location = new System.Drawing.Point(257, 13);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(83, 12);
            this.label27.TabIndex = 341;
            this.label27.Text = "複写見積番号";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CboStoreCD
            // 
            this.CboStoreCD.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.CboStoreCD.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CboStoreCD.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.店舗ストア_見積;
            this.CboStoreCD.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.CboStoreCD.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CboStoreCD.FormattingEnabled = true;
            this.CboStoreCD.Length = 40;
            this.CboStoreCD.Location = new System.Drawing.Point(1060, 111);
            this.CboStoreCD.MaxLength = 20;
            this.CboStoreCD.MoveNext = true;
            this.CboStoreCD.Name = "CboStoreCD";
            this.CboStoreCD.Size = new System.Drawing.Size(280, 20);
            this.CboStoreCD.TabIndex = 0;
            // 
            // ckM_SearchControl2
            // 
            this.ckM_SearchControl2.AutoSize = true;
            this.ckM_SearchControl2.ChangeDate = "";
            this.ckM_SearchControl2.Code = "";
            this.ckM_SearchControl2.DataCheck = false;
            this.ckM_SearchControl2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ckM_SearchControl2.IsCopy = false;
            this.ckM_SearchControl2.LabelText = "";
            this.ckM_SearchControl2.LabelVisible = false;
            this.ckM_SearchControl2.Location = new System.Drawing.Point(343, 5);
            this.ckM_SearchControl2.Name = "ckM_SearchControl2";
            this.ckM_SearchControl2.SearchEnable = true;
            this.ckM_SearchControl2.Size = new System.Drawing.Size(133, 28);
            this.ckM_SearchControl2.Stype = Search.CKM_SearchControl.SearchType.見積番号;
            this.ckM_SearchControl2.TabIndex = 275;
            this.ckM_SearchControl2.UseChangeDate = false;
            this.ckM_SearchControl2.Value1 = null;
            this.ckM_SearchControl2.Value2 = null;
            this.ckM_SearchControl2.Value3 = null;
            // 
            // ckM_SearchControl3
            // 
            this.ckM_SearchControl3.AutoSize = true;
            this.ckM_SearchControl3.ChangeDate = "";
            this.ckM_SearchControl3.Code = "";
            this.ckM_SearchControl3.DataCheck = false;
            this.ckM_SearchControl3.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ckM_SearchControl3.IsCopy = false;
            this.ckM_SearchControl3.LabelText = "";
            this.ckM_SearchControl3.LabelVisible = false;
            this.ckM_SearchControl3.Location = new System.Drawing.Point(343, 4);
            this.ckM_SearchControl3.Name = "ckM_SearchControl3";
            this.ckM_SearchControl3.SearchEnable = true;
            this.ckM_SearchControl3.Size = new System.Drawing.Size(133, 28);
            this.ckM_SearchControl3.Stype = Search.CKM_SearchControl.SearchType.見積番号;
            this.ckM_SearchControl3.TabIndex = 344;
            this.ckM_SearchControl3.UseChangeDate = false;
            this.ckM_SearchControl3.Value1 = null;
            this.ckM_SearchControl3.Value2 = null;
            this.ckM_SearchControl3.Value3 = null;
            // 
            // ckM_CustomerName
            // 
            this.ckM_CustomerName.AllowMinus = false;
            this.ckM_CustomerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_CustomerName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.ckM_CustomerName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_CustomerName.DecimalPlace = 0;
            this.ckM_CustomerName.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_CustomerName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.ckM_CustomerName.IntegerPart = 0;
            this.ckM_CustomerName.IsCorrectDate = true;
            this.ckM_CustomerName.Length = 80;
            this.ckM_CustomerName.Location = new System.Drawing.Point(233, 148);
            this.ckM_CustomerName.MaxLength = 40;
            this.ckM_CustomerName.MoveNext = true;
            this.ckM_CustomerName.Name = "ckM_CustomerName";
            this.ckM_CustomerName.Size = new System.Drawing.Size(520, 19);
            this.ckM_CustomerName.TabIndex = 2;
            this.ckM_CustomerName.Text = "ＸＸＸＸＸＸＸＸＸ10ＸＸＸＸＸＸＸＸＸ20ＸＸＸＸＸＸＸＸＸ30ＸＸＸＸＸＸＸＸＸ40";
            this.ckM_CustomerName.Visible = false;
            // 
            // ckM_RadioButton3
            // 
            this.ckM_RadioButton3.AutoSize = true;
            this.ckM_RadioButton3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckM_RadioButton3.Location = new System.Drawing.Point(103, 230);
            this.ckM_RadioButton3.Name = "ckM_RadioButton3";
            this.ckM_RadioButton3.Size = new System.Drawing.Size(75, 16);
            this.ckM_RadioButton3.TabIndex = 4;
            this.ckM_RadioButton3.Text = "初回発行";
            this.ckM_RadioButton3.UseVisualStyleBackColor = true;
            // 
            // ScStaff
            // 
            this.ScStaff.AutoSize = true;
            this.ScStaff.ChangeDate = "";
            this.ScStaff.Code = "";
            this.ScStaff.DataCheck = true;
            this.ScStaff.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ScStaff.IsCopy = false;
            this.ScStaff.LabelText = "";
            this.ScStaff.LabelVisible = true;
            this.ScStaff.Location = new System.Drawing.Point(102, 181);
            this.ScStaff.Name = "ScStaff";
            this.ScStaff.SearchEnable = true;
            this.ScStaff.Size = new System.Drawing.Size(344, 28);
            this.ScStaff.Stype = Search.CKM_SearchControl.SearchType.スタッフ;
            this.ScStaff.TabIndex = 3;
            this.ScStaff.UseChangeDate = false;
            this.ScStaff.Value1 = null;
            this.ScStaff.Value2 = null;
            this.ScStaff.Value3 = null;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.Location = new System.Drawing.Point(14, 189);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 12);
            this.label9.TabIndex = 705;
            this.label9.Text = "担当スタッフ";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSkuCD
            // 
            this.lblSkuCD.AutoSize = true;
            this.lblSkuCD.BackColor = System.Drawing.Color.Transparent;
            this.lblSkuCD.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSkuCD.Location = new System.Drawing.Point(66, 151);
            this.lblSkuCD.Name = "lblSkuCD";
            this.lblSkuCD.Size = new System.Drawing.Size(31, 12);
            this.lblSkuCD.TabIndex = 704;
            this.lblSkuCD.Text = "顧客";
            this.lblSkuCD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label2
            // 
            this.ckM_Label2.AutoSize = true;
            this.ckM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label2.Location = new System.Drawing.Point(40, 114);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label2.TabIndex = 690;
            this.ckM_Label2.Text = "請求締日";
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_RadioButton1
            // 
            this.ckM_RadioButton1.AutoSize = true;
            this.ckM_RadioButton1.Checked = true;
            this.ckM_RadioButton1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckM_RadioButton1.Location = new System.Drawing.Point(211, 230);
            this.ckM_RadioButton1.Name = "ckM_RadioButton1";
            this.ckM_RadioButton1.Size = new System.Drawing.Size(49, 16);
            this.ckM_RadioButton1.TabIndex = 5;
            this.ckM_RadioButton1.TabStop = true;
            this.ckM_RadioButton1.Text = "全て";
            this.ckM_RadioButton1.UseVisualStyleBackColor = true;
            // 
            // ckM_Label4
            // 
            this.ckM_Label4.AutoSize = true;
            this.ckM_Label4.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label4.Location = new System.Drawing.Point(40, 232);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label4.TabIndex = 701;
            this.ckM_Label4.Text = "印刷対象";
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_TextBox1
            // 
            this.ckM_TextBox1.AllowMinus = false;
            this.ckM_TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox1.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.ckM_TextBox1.DecimalPlace = 0;
            this.ckM_TextBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox1.ForeColor = System.Drawing.Color.Red;
            this.ckM_TextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox1.IntegerPart = 0;
            this.ckM_TextBox1.IsCorrectDate = true;
            this.ckM_TextBox1.Length = 10;
            this.ckM_TextBox1.Location = new System.Drawing.Point(102, 111);
            this.ckM_TextBox1.MaxLength = 10;
            this.ckM_TextBox1.MoveNext = true;
            this.ckM_TextBox1.Name = "ckM_TextBox1";
            this.ckM_TextBox1.Size = new System.Drawing.Size(100, 19);
            this.ckM_TextBox1.TabIndex = 1;
            this.ckM_TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ScCustomer
            // 
            this.ScCustomer.AutoSize = true;
            this.ScCustomer.ChangeDate = "";
            this.ScCustomer.Code = "";
            this.ScCustomer.DataCheck = true;
            this.ScCustomer.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ScCustomer.IsCopy = false;
            this.ScCustomer.LabelText = "";
            this.ScCustomer.LabelVisible = true;
            this.ScCustomer.Location = new System.Drawing.Point(102, 143);
            this.ScCustomer.Name = "ScCustomer";
            this.ScCustomer.SearchEnable = true;
            this.ScCustomer.Size = new System.Drawing.Size(634, 28);
            this.ScCustomer.Stype = Search.CKM_SearchControl.SearchType.得意先;
            this.ScCustomer.TabIndex = 2;
            this.ScCustomer.UseChangeDate = false;
            this.ScCustomer.Value1 = null;
            this.ScCustomer.Value2 = null;
            this.ScCustomer.Value3 = null;
            // 
            // SeikyuuSho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.CboStoreCD);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ScStaff);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.ckM_Label2);
            this.Controls.Add(this.ckM_RadioButton1);
            this.Controls.Add(this.ckM_Label4);
            this.Controls.Add(this.ckM_TextBox1);
            this.Controls.Add(this.ScCustomer);
            this.Controls.Add(this.lblSkuCD);
            this.Controls.Add(this.ckM_RadioButton3);
            this.Controls.Add(this.ckM_CustomerName);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "SeikyuuSho";
            this.PanelHeaderHeight = 90;
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.ckM_CustomerName, 0);
            this.Controls.SetChildIndex(this.ckM_RadioButton3, 0);
            this.Controls.SetChildIndex(this.lblSkuCD, 0);
            this.Controls.SetChildIndex(this.ScCustomer, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox1, 0);
            this.Controls.SetChildIndex(this.ckM_Label4, 0);
            this.Controls.SetChildIndex(this.ckM_RadioButton1, 0);
            this.Controls.SetChildIndex(this.ckM_Label2, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.ScStaff, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.CboStoreCD, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CKM_Controls.CKM_Label label4;
        private CKM_Controls.CKM_Label label27;
        private CKM_Controls.CKM_Label label1;
        private CKM_Controls.CKM_ComboBox CboStoreCD;
        private Search.CKM_SearchControl ckM_SearchControl2;
        private Search.CKM_SearchControl ckM_SearchControl3;
        private CKM_Controls.CKM_TextBox ckM_CustomerName;
        private CKM_Controls.CKM_RadioButton ckM_RadioButton3;
        private Search.CKM_SearchControl ScStaff;
        private CKM_Controls.CKM_Label label9;
        private System.Windows.Forms.Label lblSkuCD;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_RadioButton ckM_RadioButton1;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private Search.CKM_SearchControl ScCustomer;
    }
}

