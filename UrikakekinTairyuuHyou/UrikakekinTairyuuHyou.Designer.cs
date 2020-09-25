namespace UrikakekinTairyuuHyou
{
    partial class UrikakekinTairyuuHyou
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
            this.cboStore = new CKM_Controls.CKM_ComboBox();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.txtDate = new CKM_Controls.CKM_TextBox();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1774, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(1240, 0);
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboStore);
            this.panel1.Controls.Add(this.ckM_Label2);
            this.panel1.Controls.Add(this.txtDate);
            this.panel1.Controls.Add(this.ckM_Label1);
            this.panel1.Location = new System.Drawing.Point(6, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1770, 880);
            this.panel1.TabIndex = 0;
            // 
            // cboStore
            // 
            this.cboStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cboStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStore.Cbo_Type = CKM_Controls.CKM_ComboBox.CboType.店舗ストア;
            this.cboStore.Ctrl_Byte = CKM_Controls.CKM_ComboBox.Bytes.半角;
            this.cboStore.Flag = 0;
            this.cboStore.FormattingEnabled = true;
            this.cboStore.Length = 10;
            this.cboStore.Location = new System.Drawing.Point(1400, 37);
            this.cboStore.MaxLength = 10;
            this.cboStore.MoveNext = true;
            this.cboStore.Name = "cboStore";
            this.cboStore.Size = new System.Drawing.Size(265, 20);
            this.cboStore.TabIndex = 1;
            this.cboStore.Text = "西川店ＭＭＭＭＭ１０ＭＭＭＭＭＭＭＭ２０";
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
            this.ckM_Label2.Location = new System.Drawing.Point(1366, 41);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(31, 12);
            this.ckM_Label2.TabIndex = 2;
            this.ckM_Label2.Text = "店舗";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDate
            // 
            this.txtDate.AllowMinus = false;
            this.txtDate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtDate.BackColor = System.Drawing.Color.White;
            this.txtDate.BorderColor = false;
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDate.ClientColor = System.Drawing.Color.White;
            this.txtDate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtDate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.YearMonth;
            this.txtDate.DecimalPlace = 0;
            this.txtDate.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtDate.IntegerPart = 0;
            this.txtDate.IsCorrectDate = true;
            this.txtDate.isEnterKeyDown = false;
            this.txtDate.IsFirstTime = true;
            this.txtDate.isMaxLengthErr = false;
            this.txtDate.IsNumber = true;
            this.txtDate.IsShop = false;
            this.txtDate.Length = 7;
            this.txtDate.Location = new System.Drawing.Point(148, 34);
            this.txtDate.MaxLength = 7;
            this.txtDate.MoveNext = true;
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(80, 19);
            this.txtDate.TabIndex = 0;
            this.txtDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtDate.UseColorSizMode = false;
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
            this.ckM_Label1.Location = new System.Drawing.Point(89, 37);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label1.TabIndex = 0;
            this.ckM_Label1.Text = "対象年月";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // UrikakekinTairyuuHyou
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1776, 961);
            this.Controls.Add(this.panel1);
            this.F10Visible = false;
            this.F2Visible = false;
            this.F3Visible = false;
            this.F4Visible = false;
            this.F5Visible = false;
            this.F7Visible = false;
            this.F8Visible = false;
            this.F9Visible = false;
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "UrikakekinTairyuuHyou";
            this.PanelHeaderHeight = 50;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.UrikakekinTairyuuHyou_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UrikakekinTairyuuHyou_KeyUp);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private CKM_Controls.CKM_TextBox txtDate;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_ComboBox cboStore;
        private CKM_Controls.CKM_Label ckM_Label2;
    }
}

