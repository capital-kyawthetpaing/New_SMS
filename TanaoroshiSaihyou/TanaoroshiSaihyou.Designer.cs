namespace TanaoroshiSaihyou
{
    partial class TanaoroshiSaihyou
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
            this.CboSoukoCD = new CKM_Controls.CKM_ComboBox();
            this.ckM_SearchControl2 = new Search.CKM_SearchControl();
            this.ckM_SearchControl3 = new Search.CKM_SearchControl();
            this.ckM_Label10 = new CKM_Controls.CKM_Label();
            this.label11 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox1 = new CKM_Controls.CKM_TextBox();
            this.ckM_RadioButton1 = new CKM_Controls.CKM_RadioButton();
            this.ckM_RadioButton2 = new CKM_Controls.CKM_RadioButton();
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
            this.label4.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.DefaultlabelSize = true;
            this.label4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label4.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(38, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 12);
            this.label4.TabIndex = 255;
            this.label4.Text = "対象倉庫";
            this.label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label1.BackColor = System.Drawing.Color.DarkGray;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.DefaultlabelSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(28, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(492, 18);
            this.label1.TabIndex = 261;
            this.label1.Text = "商品名";
            this.label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label27.BackColor = System.Drawing.Color.Transparent;
            this.label27.DefaultlabelSize = true;
            this.label27.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label27.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label27.ForeColor = System.Drawing.Color.Black;
            this.label27.Location = new System.Drawing.Point(257, 13);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(83, 12);
            this.label27.TabIndex = 341;
            this.label27.Text = "複写見積番号";
            this.label27.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CboSoukoCD
            // 
            this.CboSoukoCD.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.CboSoukoCD.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CboSoukoCD.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.棚卸倉庫;
            this.CboSoukoCD.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半全角;
            this.CboSoukoCD.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CboSoukoCD.Flag = 0;
            this.CboSoukoCD.FormattingEnabled = true;
            this.CboSoukoCD.Length = 40;
            this.CboSoukoCD.Location = new System.Drawing.Point(101, 124);
            this.CboSoukoCD.MaxLength = 20;
            this.CboSoukoCD.MoveNext = false;
            this.CboSoukoCD.Name = "CboSoukoCD";
            this.CboSoukoCD.Size = new System.Drawing.Size(280, 20);
            this.CboSoukoCD.TabIndex = 0;
            // 
            // ckM_SearchControl2
            // 
            this.ckM_SearchControl2.AutoSize = true;
            this.ckM_SearchControl2.ChangeDate = "";
            this.ckM_SearchControl2.ChangeDateWidth = 100;
            this.ckM_SearchControl2.Code = "";
            this.ckM_SearchControl2.CodeWidth = 100;
            this.ckM_SearchControl2.CodeWidth1 = 100;
            this.ckM_SearchControl2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_SearchControl2.DataCheck = false;
            this.ckM_SearchControl2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_SearchControl2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ckM_SearchControl2.IsCopy = false;
            this.ckM_SearchControl2.LabelText = "";
            this.ckM_SearchControl2.LabelVisible = false;
            this.ckM_SearchControl2.Location = new System.Drawing.Point(343, 5);
            this.ckM_SearchControl2.Margin = new System.Windows.Forms.Padding(0);
            this.ckM_SearchControl2.Name = "ckM_SearchControl2";
            this.ckM_SearchControl2.NameWidth = 600;
            this.ckM_SearchControl2.SearchEnable = true;
            this.ckM_SearchControl2.Size = new System.Drawing.Size(133, 28);
            this.ckM_SearchControl2.Stype = Search.CKM_SearchControl.SearchType.見積番号;
            this.ckM_SearchControl2.TabIndex = 275;
            this.ckM_SearchControl2.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ckM_SearchControl2.UseChangeDate = false;
            this.ckM_SearchControl2.Value1 = null;
            this.ckM_SearchControl2.Value2 = null;
            this.ckM_SearchControl2.Value3 = null;
            // 
            // ckM_SearchControl3
            // 
            this.ckM_SearchControl3.AutoSize = true;
            this.ckM_SearchControl3.ChangeDate = "";
            this.ckM_SearchControl3.ChangeDateWidth = 100;
            this.ckM_SearchControl3.Code = "";
            this.ckM_SearchControl3.CodeWidth = 100;
            this.ckM_SearchControl3.CodeWidth1 = 100;
            this.ckM_SearchControl3.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_SearchControl3.DataCheck = false;
            this.ckM_SearchControl3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_SearchControl3.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ckM_SearchControl3.IsCopy = false;
            this.ckM_SearchControl3.LabelText = "";
            this.ckM_SearchControl3.LabelVisible = false;
            this.ckM_SearchControl3.Location = new System.Drawing.Point(343, 4);
            this.ckM_SearchControl3.Margin = new System.Windows.Forms.Padding(0);
            this.ckM_SearchControl3.Name = "ckM_SearchControl3";
            this.ckM_SearchControl3.NameWidth = 600;
            this.ckM_SearchControl3.SearchEnable = true;
            this.ckM_SearchControl3.Size = new System.Drawing.Size(133, 28);
            this.ckM_SearchControl3.Stype = Search.CKM_SearchControl.SearchType.見積番号;
            this.ckM_SearchControl3.TabIndex = 344;
            this.ckM_SearchControl3.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.ckM_SearchControl3.UseChangeDate = false;
            this.ckM_SearchControl3.Value1 = null;
            this.ckM_SearchControl3.Value2 = null;
            this.ckM_SearchControl3.Value3 = null;
            // 
            // ckM_Label10
            // 
            this.ckM_Label10.AutoSize = true;
            this.ckM_Label10.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label10.DefaultlabelSize = true;
            this.ckM_Label10.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label10.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label10.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label10.Location = new System.Drawing.Point(39, 223);
            this.ckM_Label10.Name = "ckM_Label10";
            this.ckM_Label10.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label10.TabIndex = 741;
            this.ckM_Label10.Text = "印刷対象";
            this.ckM_Label10.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.DefaultlabelSize = true;
            this.label11.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.label11.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(51, 168);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 12);
            this.label11.TabIndex = 737;
            this.label11.Text = "棚卸日";
            this.label11.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_TextBox1
            // 
            this.ckM_TextBox1.AllowMinus = false;
            this.ckM_TextBox1.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox1.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox1.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox1.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.ckM_TextBox1.DecimalPlace = 2;
            this.ckM_TextBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ckM_TextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ckM_TextBox1.IntegerPart = 0;
            this.ckM_TextBox1.IsCorrectDate = true;
            this.ckM_TextBox1.isEnterKeyDown = false;
            this.ckM_TextBox1.isMaxLengthErr = false;
            this.ckM_TextBox1.IsNumber = true;
            this.ckM_TextBox1.IsShop = false;
            this.ckM_TextBox1.Length = 10;
            this.ckM_TextBox1.Location = new System.Drawing.Point(101, 166);
            this.ckM_TextBox1.MaxLength = 10;
            this.ckM_TextBox1.MoveNext = true;
            this.ckM_TextBox1.Name = "ckM_TextBox1";
            this.ckM_TextBox1.Size = new System.Drawing.Size(88, 19);
            this.ckM_TextBox1.TabIndex = 1;
            this.ckM_TextBox1.Text = "2019/01/01";
            this.ckM_TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ckM_TextBox1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_RadioButton1
            // 
            this.ckM_RadioButton1.AutoSize = true;
            this.ckM_RadioButton1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_RadioButton1.Location = new System.Drawing.Point(126, 221);
            this.ckM_RadioButton1.Name = "ckM_RadioButton1";
            this.ckM_RadioButton1.Size = new System.Drawing.Size(114, 16);
            this.ckM_RadioButton1.TabIndex = 2;
            this.ckM_RadioButton1.TabStop = true;
            this.ckM_RadioButton1.Text = "差異発生分のみ";
            this.ckM_RadioButton1.UseVisualStyleBackColor = true;
            // 
            // ckM_RadioButton2
            // 
            this.ckM_RadioButton2.AutoSize = true;
            this.ckM_RadioButton2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_RadioButton2.Location = new System.Drawing.Point(280, 221);
            this.ckM_RadioButton2.Name = "ckM_RadioButton2";
            this.ckM_RadioButton2.Size = new System.Drawing.Size(49, 16);
            this.ckM_RadioButton2.TabIndex = 3;
            this.ckM_RadioButton2.TabStop = true;
            this.ckM_RadioButton2.Text = "全て";
            this.ckM_RadioButton2.UseVisualStyleBackColor = true;
            // 
            // TanaoroshiSaihyou
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.ckM_RadioButton2);
            this.Controls.Add(this.ckM_RadioButton1);
            this.Controls.Add(this.ckM_Label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.ckM_TextBox1);
            this.Controls.Add(this.CboSoukoCD);
            this.Controls.Add(this.label4);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "TanaoroshiSaihyou";
            this.PanelHeaderHeight = 90;
            this.Text = "TanaoroshiSaihyou";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.CboSoukoCD, 0);
            this.Controls.SetChildIndex(this.ckM_TextBox1, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.ckM_Label10, 0);
            this.Controls.SetChildIndex(this.ckM_RadioButton1, 0);
            this.Controls.SetChildIndex(this.ckM_RadioButton2, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CKM_Controls.CKM_Label label4;
        private CKM_Controls.CKM_Label label27;
        private CKM_Controls.CKM_Label label1;
        private CKM_Controls.CKM_ComboBox CboSoukoCD;
        private Search.CKM_SearchControl ckM_SearchControl2;
        private Search.CKM_SearchControl ckM_SearchControl3;
        private CKM_Controls.CKM_Label ckM_Label10;
        private CKM_Controls.CKM_Label label11;
        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private CKM_Controls.CKM_RadioButton ckM_RadioButton1;
        private CKM_Controls.CKM_RadioButton ckM_RadioButton2;
    }
}

