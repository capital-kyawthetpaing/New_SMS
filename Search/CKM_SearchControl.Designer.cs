namespace Search
{
    partial class CKM_SearchControl
    {

        #region Component Designer generated code
        private void InitializeComponent()
        {
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.txtChangeDate = new CKM_Controls.CKM_TextBox();
            this.txtCode = new CKM_Controls.CKM_TextBox();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSearch.Location = new System.Drawing.Point(99, 4);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(33, 23);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.TabStop = false;
            this.btnSearch.Text = "･･･";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblName.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(131, 5);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(281, 19);
            this.lblName.TabIndex = 46;
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtChangeDate
            // 
            this.txtChangeDate.AllowMinus = false;
            this.txtChangeDate.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtChangeDate.BackColor = System.Drawing.Color.White;
            this.txtChangeDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtChangeDate.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtChangeDate.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Date;
            this.txtChangeDate.DecimalPlace = 0;
            this.txtChangeDate.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtChangeDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtChangeDate.IntegerPart = 0;
            this.txtChangeDate.IsCorrectDate = true;
            this.txtChangeDate.isEnterKeyDown = false;
            this.txtChangeDate.IsNumber = true;
            this.txtChangeDate.IsShop = false;
            this.txtChangeDate.Length = 10;
            this.txtChangeDate.Location = new System.Drawing.Point(0, 30);
            this.txtChangeDate.MaxLength = 10;
            this.txtChangeDate.MoveNext = true;
            this.txtChangeDate.Name = "txtChangeDate";
            this.txtChangeDate.Size = new System.Drawing.Size(100, 19);
            this.txtChangeDate.TabIndex = 1;
            this.txtChangeDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtChangeDate.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtChangeDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtChangeDate_KeyDown);
            // 
            // txtCode
            // 
            this.txtCode.AllowMinus = false;
            this.txtCode.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCode.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtCode.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtCode.DecimalPlace = 0;
            this.txtCode.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtCode.IntegerPart = 0;
            this.txtCode.IsCorrectDate = true;
            this.txtCode.isEnterKeyDown = false;
            this.txtCode.IsNumber = true;
            this.txtCode.IsShop = false;
            this.txtCode.Length = 10;
            this.txtCode.Location = new System.Drawing.Point(0, 5);
            this.txtCode.MaxLength = 10;
            this.txtCode.MoveNext = true;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(100, 19);
            this.txtCode.TabIndex = 0;
            this.txtCode.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.txtCode.Enter += new System.EventHandler(this.TxtCode_Enter);
            this.txtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtCode_KeyDown);
            this.txtCode.Leave += new System.EventHandler(this.TxtCode_Leave);
            // 
            // CKM_SearchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtChangeDate);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnSearch);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CKM_SearchControl";
            this.Size = new System.Drawing.Size(417, 50);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblName;
        private CKM_Controls.CKM_TextBox txtCode;
        private CKM_Controls.CKM_TextBox txtChangeDate;
    }
}
