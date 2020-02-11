namespace MasterTouroku_Renban
{
    partial class FrmMasterTouroku_Renban
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
            this.BtnF11Show = new CKM_Controls.CKM_Button();
            this.PanelDetail = new System.Windows.Forms.Panel();
            this.txtContinuous = new CKM_Controls.CKM_TextBox();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtPrefixValue = new CKM_Controls.CKM_TextBox();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.PanelHeader.SuspendLayout();
            this.PanelSearch.SuspendLayout();
            this.PanelDetail.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.panel1);
            this.PanelHeader.Size = new System.Drawing.Size(1711, 59);
            this.PanelHeader.TabIndex = 0;
            this.PanelHeader.Controls.SetChildIndex(this.panel1, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Controls.Add(this.BtnF11Show);
            this.PanelSearch.TabIndex = 0;
            // 
            // BtnF11Show
            // 
            this.BtnF11Show.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BtnF11Show.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BtnF11Show.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnF11Show.DefaultBtnSize = false;
            this.BtnF11Show.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnF11Show.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnF11Show.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.BtnF11Show.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BtnF11Show.Location = new System.Drawing.Point(386, 0);
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
            this.PanelDetail.Controls.Add(this.txtContinuous);
            this.PanelDetail.Controls.Add(this.ckM_Label2);
            this.PanelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelDetail.Location = new System.Drawing.Point(0, 115);
            this.PanelDetail.Name = "PanelDetail";
            this.PanelDetail.Size = new System.Drawing.Size(1713, 814);
            this.PanelDetail.TabIndex = 1;
            // 
            // txtContinuous
            // 
            this.txtContinuous.AllowMinus = false;
            this.txtContinuous.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtContinuous.BackColor = System.Drawing.Color.White;
            this.txtContinuous.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContinuous.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtContinuous.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Number;
            this.txtContinuous.DecimalPlace = 0;
            this.txtContinuous.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtContinuous.IntegerPart = 0;
            this.txtContinuous.IsCorrectDate = true;
            this.txtContinuous.isEnterKeyDown = false;
            this.txtContinuous.IsNumber = true;
            this.txtContinuous.IsShop = false;
            this.txtContinuous.Length = 10;
            this.txtContinuous.Location = new System.Drawing.Point(94, 8);
            this.txtContinuous.MaxLength = 10;
            this.txtContinuous.MoveNext = true;
            this.txtContinuous.Name = "txtContinuous";
            this.txtContinuous.Size = new System.Drawing.Size(100, 19);
            this.txtContinuous.TabIndex = 1;
            this.txtContinuous.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtContinuous.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtContinuous.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtContinuous_KeyDown);
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
            this.ckM_Label2.Location = new System.Drawing.Point(60, 12);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label2.TabIndex = 0;
            this.ckM_Label2.Text = "連番";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtPrefixValue);
            this.panel1.Controls.Add(this.ckM_Label1);
            this.panel1.Location = new System.Drawing.Point(13, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 40);
            this.panel1.TabIndex = 0;
            // 
            // txtPrefixValue
            // 
            this.txtPrefixValue.AllowMinus = false;
            this.txtPrefixValue.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtPrefixValue.BackColor = System.Drawing.Color.White;
            this.txtPrefixValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPrefixValue.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtPrefixValue.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtPrefixValue.DecimalPlace = 0;
            this.txtPrefixValue.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtPrefixValue.IntegerPart = 0;
            this.txtPrefixValue.IsCorrectDate = true;
            this.txtPrefixValue.isEnterKeyDown = false;
            this.txtPrefixValue.IsNumber = true;
            this.txtPrefixValue.IsShop = false;
            this.txtPrefixValue.Length = 5;
            this.txtPrefixValue.Location = new System.Drawing.Point(80, 14);
            this.txtPrefixValue.MaxLength = 3;
            this.txtPrefixValue.MoveNext = true;
            this.txtPrefixValue.Name = "txtPrefixValue";
            this.txtPrefixValue.Size = new System.Drawing.Size(50, 19);
            this.txtPrefixValue.TabIndex = 4;
            this.txtPrefixValue.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtPrefixValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPrefixValue_KeyDown);
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
            this.ckM_Label1.Location = new System.Drawing.Point(33, 17);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(44, 12);
            this.ckM_Label1.TabIndex = 5;
            this.ckM_Label1.Text = "接頭値";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmMasterTouroku_Renban
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.PanelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "FrmMasterTouroku_Renban";
            this.PanelHeaderHeight = 115;
            this.Text = "MasterTouroku_Renban";
            this.Load += new System.EventHandler(this.FrmMasterTouroku_Renban_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmMasterTouroku_Renban_KeyUp);
            this.Controls.SetChildIndex(this.PanelDetail, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelSearch.ResumeLayout(false);
            this.PanelDetail.ResumeLayout(false);
            this.PanelDetail.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private CKM_Controls.CKM_Button BtnF11Show;
        private System.Windows.Forms.Panel PanelDetail;
        private CKM_Controls.CKM_TextBox txtContinuous;
        private CKM_Controls.CKM_Label ckM_Label2;
        private System.Windows.Forms.Panel panel1;
        private CKM_Controls.CKM_TextBox txtPrefixValue;
        private CKM_Controls.CKM_Label ckM_Label1;
    }
}