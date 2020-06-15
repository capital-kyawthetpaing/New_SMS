namespace Shiharai_IchiranHyou
{
    partial class SiharaiItiranHyou
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
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.txtPurchaseDateFrom = new CKM_Controls.CKM_TextBox();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.txtPurChaseDateTo = new CKM_Controls.CKM_TextBox();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.SC_Payment = new Search.CKM_SearchControl();
            this.SC_Staff = new Search.CKM_SearchControl();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.PanelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.ckM_Label4);
            this.PanelHeader.Controls.Add(this.SC_Staff);
            this.PanelHeader.Controls.Add(this.SC_Payment);
            this.PanelHeader.Controls.Add(this.ckM_Label3);
            this.PanelHeader.Controls.Add(this.txtPurChaseDateTo);
            this.PanelHeader.Controls.Add(this.ckM_Label2);
            this.PanelHeader.Controls.Add(this.txtPurchaseDateFrom);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Size = new System.Drawing.Size(1355, 124);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtPurchaseDateFrom, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtPurChaseDateTo, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label3, 0);
            this.PanelHeader.Controls.SetChildIndex(this.SC_Payment, 0);
            this.PanelHeader.Controls.SetChildIndex(this.SC_Staff, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label4, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(821, 0);
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
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
            this.ckM_Label1.Location = new System.Drawing.Point(44, 8);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label1.TabIndex = 2;
            this.ckM_Label1.Text = "支払日";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPurchaseDateFrom
            // 
            this.txtPurchaseDateFrom.AllowMinus = false;
            this.txtPurchaseDateFrom.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPurchaseDateFrom.BackColor = System.Drawing.Color.White;
            this.txtPurchaseDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPurchaseDateFrom.ClientColor = System.Drawing.Color.White;
            this.txtPurchaseDateFrom.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtPurchaseDateFrom.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtPurchaseDateFrom.DecimalPlace = 0;
            this.txtPurchaseDateFrom.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtPurchaseDateFrom.IntegerPart = 0;
            this.txtPurchaseDateFrom.IsCorrectDate = true;
            this.txtPurchaseDateFrom.isEnterKeyDown = false;
            this.txtPurchaseDateFrom.isMaxLengthErr = false;
            this.txtPurchaseDateFrom.IsNumber = true;
            this.txtPurchaseDateFrom.IsShop = false;
            this.txtPurchaseDateFrom.Length = 10;
            this.txtPurchaseDateFrom.Location = new System.Drawing.Point(91, 4);
            this.txtPurchaseDateFrom.MaxLength = 10;
            this.txtPurchaseDateFrom.MoveNext = true;
            this.txtPurchaseDateFrom.Name = "txtPurchaseDateFrom";
            this.txtPurchaseDateFrom.Size = new System.Drawing.Size(100, 19);
            this.txtPurchaseDateFrom.TabIndex = 3;
            this.txtPurchaseDateFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPurchaseDateFrom.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.ckM_Label2.Location = new System.Drawing.Point(211, 9);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label2.TabIndex = 4;
            this.ckM_Label2.Text = "～";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPurChaseDateTo
            // 
            this.txtPurChaseDateTo.AllowMinus = false;
            this.txtPurChaseDateTo.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPurChaseDateTo.BackColor = System.Drawing.Color.White;
            this.txtPurChaseDateTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPurChaseDateTo.ClientColor = System.Drawing.Color.White;
            this.txtPurChaseDateTo.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtPurChaseDateTo.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtPurChaseDateTo.DecimalPlace = 0;
            this.txtPurChaseDateTo.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtPurChaseDateTo.IntegerPart = 0;
            this.txtPurChaseDateTo.IsCorrectDate = true;
            this.txtPurChaseDateTo.isEnterKeyDown = false;
            this.txtPurChaseDateTo.isMaxLengthErr = false;
            this.txtPurChaseDateTo.IsNumber = true;
            this.txtPurChaseDateTo.IsShop = false;
            this.txtPurChaseDateTo.Length = 10;
            this.txtPurChaseDateTo.Location = new System.Drawing.Point(246, 3);
            this.txtPurChaseDateTo.MaxLength = 10;
            this.txtPurChaseDateTo.MoveNext = true;
            this.txtPurChaseDateTo.Name = "txtPurChaseDateTo";
            this.txtPurChaseDateTo.Size = new System.Drawing.Size(100, 19);
            this.txtPurChaseDateTo.TabIndex = 5;
            this.txtPurChaseDateTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPurChaseDateTo.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtPurChaseDateTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPurChaseDateTo_KeyDown);
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
            this.ckM_Label3.Location = new System.Drawing.Point(44, 36);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label3.TabIndex = 6;
            this.ckM_Label3.Text = "支払先";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SC_Payment
            // 
            this.SC_Payment.AutoSize = true;
            this.SC_Payment.ChangeDate = "";
            this.SC_Payment.ChangeDateWidth = 100;
            this.SC_Payment.Code = "";
            this.SC_Payment.CodeWidth = 100;
            this.SC_Payment.CodeWidth1 = 100;
            this.SC_Payment.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.SC_Payment.DataCheck = false;
            this.SC_Payment.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.SC_Payment.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.SC_Payment.IsCopy = false;
            this.SC_Payment.LabelText = "";
            this.SC_Payment.LabelVisible = true;
            this.SC_Payment.Location = new System.Drawing.Point(91, 27);
            this.SC_Payment.Margin = new System.Windows.Forms.Padding(0);
            this.SC_Payment.Name = "SC_Payment";
            this.SC_Payment.NameWidth = 310;
            this.SC_Payment.SearchEnable = true;
            this.SC_Payment.Size = new System.Drawing.Size(444, 27);
            this.SC_Payment.Stype = Search.CKM_SearchControl.SearchType.仕入先;
            this.SC_Payment.TabIndex = 7;
            this.SC_Payment.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.SC_Payment.UseChangeDate = false;
            this.SC_Payment.Value1 = null;
            this.SC_Payment.Value2 = null;
            this.SC_Payment.Value3 = null;
            this.SC_Payment.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.SC_Payment_CodeKeyDownEvent);
            this.SC_Payment.Enter += new System.EventHandler(this.SC_Payment_Enter);
            // 
            // SC_Staff
            // 
            this.SC_Staff.AutoSize = true;
            this.SC_Staff.ChangeDate = "";
            this.SC_Staff.ChangeDateWidth = 100;
            this.SC_Staff.Code = "";
            this.SC_Staff.CodeWidth = 70;
            this.SC_Staff.CodeWidth1 = 70;
            this.SC_Staff.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.SC_Staff.DataCheck = false;
            this.SC_Staff.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.SC_Staff.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.SC_Staff.IsCopy = false;
            this.SC_Staff.LabelText = "";
            this.SC_Staff.LabelVisible = true;
            this.SC_Staff.Location = new System.Drawing.Point(91, 54);
            this.SC_Staff.Margin = new System.Windows.Forms.Padding(0);
            this.SC_Staff.Name = "SC_Staff";
            this.SC_Staff.NameWidth = 250;
            this.SC_Staff.SearchEnable = true;
            this.SC_Staff.Size = new System.Drawing.Size(354, 27);
            this.SC_Staff.Stype = Search.CKM_SearchControl.SearchType.スタッフ;
            this.SC_Staff.TabIndex = 8;
            this.SC_Staff.TextSize = Search.CKM_SearchControl.FontSize.Normal;
            this.SC_Staff.UseChangeDate = false;
            this.SC_Staff.Value1 = null;
            this.SC_Staff.Value2 = null;
            this.SC_Staff.Value3 = null;
            this.SC_Staff.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.SC_Staff_CodeKeyDownEvent);
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
            this.ckM_Label4.Location = new System.Drawing.Point(5, 63);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label4.TabIndex = 9;
            this.ckM_Label4.Text = "担当スタッフ";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SiharaiItiranHyou
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1357, 845);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "SiharaiItiranHyou";
            this.PanelHeaderHeight = 180;
            this.Text = "SiharaiItiranHyou";
            this.Load += new System.EventHandler(this.Siharai_ItiranHyou_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Siharai_ItiranHyou_KeyUp);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CKM_Controls.CKM_Label ckM_Label4;
        private Search.CKM_SearchControl SC_Staff;
        private Search.CKM_SearchControl SC_Payment;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_TextBox txtPurChaseDateTo;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_TextBox txtPurchaseDateFrom;
        private CKM_Controls.CKM_Label ckM_Label1;
    }
}

