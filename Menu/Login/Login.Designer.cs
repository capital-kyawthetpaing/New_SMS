namespace Menu.Login
{
    partial class frmLogin
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
            this.btnLogin = new CKM_Controls.CKM_Button();
            this.txtPassword = new CKM_Controls.CKM_TextBox();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.txtOperatorCD = new CKM_Controls.CKM_TextBox();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.txtCompanyCD = new CKM_Controls.CKM_TextBox();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnLogin.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.DefaultBtnSize = false;
            this.btnLogin.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnLogin.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnLogin.Location = new System.Drawing.Point(143, 126);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(1);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(105, 23);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "ログイン";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.AllowMinus = false;
            this.txtPassword.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半全角;
            this.txtPassword.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtPassword.DecimalPlace = 0;
            this.txtPassword.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtPassword.IntegerPart = 0;
            this.txtPassword.IsCorrectDate = true;
            this.txtPassword.isEnterKeyDown = false;
            this.txtPassword.IsNumber = true;
            this.txtPassword.IsShop = false;
            this.txtPassword.Length = 10;
            this.txtPassword.Location = new System.Drawing.Point(132, 87);
            this.txtPassword.MaxLength = 10;
            this.txtPassword.MoveNext = true;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Size = new System.Drawing.Size(140, 19);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_Label3
            // 
            this.ckM_Label3.AutoSize = true;
            this.ckM_Label3.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label3.DefaultlabelSize = true;
            this.ckM_Label3.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label3.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label3.Location = new System.Drawing.Point(59, 91);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(70, 12);
            this.ckM_Label3.TabIndex = 8;
            this.ckM_Label3.Text = "パスワード";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOperatorCD
            // 
            this.txtOperatorCD.AllowMinus = false;
            this.txtOperatorCD.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtOperatorCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOperatorCD.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtOperatorCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtOperatorCD.DecimalPlace = 0;
            this.txtOperatorCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtOperatorCD.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtOperatorCD.IntegerPart = 0;
            this.txtOperatorCD.IsCorrectDate = true;
            this.txtOperatorCD.isEnterKeyDown = false;
            this.txtOperatorCD.IsNumber = true;
            this.txtOperatorCD.IsShop = false;
            this.txtOperatorCD.Length = 10;
            this.txtOperatorCD.Location = new System.Drawing.Point(132, 51);
            this.txtOperatorCD.MaxLength = 10;
            this.txtOperatorCD.MoveNext = true;
            this.txtOperatorCD.Name = "txtOperatorCD";
            this.txtOperatorCD.Size = new System.Drawing.Size(100, 19);
            this.txtOperatorCD.TabIndex = 1;
            this.txtOperatorCD.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_Label2
            // 
            this.ckM_Label2.AutoSize = true;
            this.ckM_Label2.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label2.DefaultlabelSize = true;
            this.ckM_Label2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label2.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label2.Location = new System.Drawing.Point(46, 55);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label2.TabIndex = 6;
            this.ckM_Label2.Text = "オペレーター";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCompanyCD
            // 
            this.txtCompanyCD.AllowMinus = false;
            this.txtCompanyCD.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtCompanyCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCompanyCD.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtCompanyCD.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Number;
            this.txtCompanyCD.DecimalPlace = 0;
            this.txtCompanyCD.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtCompanyCD.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtCompanyCD.IntegerPart = 0;
            this.txtCompanyCD.IsCorrectDate = true;
            this.txtCompanyCD.isEnterKeyDown = false;
            this.txtCompanyCD.IsNumber = true;
            this.txtCompanyCD.IsShop = false;
            this.txtCompanyCD.Length = 3;
            this.txtCompanyCD.Location = new System.Drawing.Point(132, 17);
            this.txtCompanyCD.MaxLength = 3;
            this.txtCompanyCD.MoveNext = true;
            this.txtCompanyCD.Name = "txtCompanyCD";
            this.txtCompanyCD.Size = new System.Drawing.Size(30, 19);
            this.txtCompanyCD.TabIndex = 0;
            this.txtCompanyCD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCompanyCD.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_Label1
            // 
            this.ckM_Label1.AutoSize = true;
            this.ckM_Label1.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label1.DefaultlabelSize = true;
            this.ckM_Label1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label1.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label1.Location = new System.Drawing.Point(84, 21);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(45, 12);
            this.ckM_Label1.TabIndex = 4;
            this.ckM_Label1.Text = "会社CD";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.ClientSize = new System.Drawing.Size(336, 167);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.ckM_Label3);
            this.Controls.Add(this.txtOperatorCD);
            this.Controls.Add(this.ckM_Label2);
            this.Controls.Add(this.txtCompanyCD);
            this.Controls.Add(this.ckM_Label1);
            this.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ログイン";
            this.Load += new System.EventHandler(this.FrmLogin_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmLogin_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_TextBox txtCompanyCD;
        private CKM_Controls.CKM_TextBox txtOperatorCD;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_TextBox txtPassword;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Button btnLogin;
    }
}