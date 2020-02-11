namespace SampleTemplate
{
    partial class Form1
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
            this.ScNormal = new Search.CKM_SearchControl();
            this.PanelCopy = new System.Windows.Forms.Panel();
            this.ScCopy = new Search.CKM_SearchControl();
            this.btnDisplay = new CKM_Controls.CKM_Button();
            this.PanelDetail = new System.Windows.Forms.Panel();
            this.ckM_SearchControl1 = new Search.CKM_SearchControl();
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
            this.PanelHeader.Size = new System.Drawing.Size(1774, 91);
            this.PanelHeader.Controls.SetChildIndex(this.PanelNormal, 0);
            this.PanelHeader.Controls.SetChildIndex(this.PanelCopy, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Controls.Add(this.btnDisplay);
            this.PanelSearch.Location = new System.Drawing.Point(1240, 0);
            // 
            // PanelNormal
            // 
            this.PanelNormal.Controls.Add(this.ScNormal);
            this.PanelNormal.Location = new System.Drawing.Point(12, 4);
            this.PanelNormal.Name = "PanelNormal";
            this.PanelNormal.Size = new System.Drawing.Size(200, 50);
            this.PanelNormal.TabIndex = 9;
            // 
            // ScNormal
            // 
            this.ScNormal.AutoSize = true;
            this.ScNormal.ChangeDate = "";
            this.ScNormal.ChangeDateWidth = 100;
            this.ScNormal.Code = "";
            this.ScNormal.CodeWidth = 100;
            this.ScNormal.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScNormal.DataCheck = false;
            this.ScNormal.IsCopy = false;
            this.ScNormal.LabelText = "";
            this.ScNormal.LabelVisible = false;
            this.ScNormal.Location = new System.Drawing.Point(17, -2);
            this.ScNormal.Name = "ScNormal";
            this.ScNormal.SearchEnable = true;
            this.ScNormal.Size = new System.Drawing.Size(133, 52);
            this.ScNormal.Stype = Search.CKM_SearchControl.SearchType.Default;
            this.ScNormal.TabIndex = 11;
            this.ScNormal.UseChangeDate = true;
            this.ScNormal.Value1 = null;
            this.ScNormal.Value2 = null;
            this.ScNormal.Value3 = null;
            this.ScNormal.ChangeDateKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScNormal_ChangeDateKeyDownEvent);
            // 
            // PanelCopy
            // 
            this.PanelCopy.Controls.Add(this.ScCopy);
            this.PanelCopy.Location = new System.Drawing.Point(573, 3);
            this.PanelCopy.Name = "PanelCopy";
            this.PanelCopy.Size = new System.Drawing.Size(200, 50);
            this.PanelCopy.TabIndex = 10;
            // 
            // ScCopy
            // 
            this.ScCopy.AutoSize = true;
            this.ScCopy.ChangeDate = "";
            this.ScCopy.ChangeDateWidth = 100;
            this.ScCopy.Code = "";
            this.ScCopy.CodeWidth = 100;
            this.ScCopy.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ScCopy.DataCheck = false;
            this.ScCopy.IsCopy = false;
            this.ScCopy.LabelText = "";
            this.ScCopy.LabelVisible = false;
            this.ScCopy.Location = new System.Drawing.Point(6, -3);
            this.ScCopy.Name = "ScCopy";
            this.ScCopy.SearchEnable = true;
            this.ScCopy.Size = new System.Drawing.Size(133, 52);
            this.ScCopy.Stype = Search.CKM_SearchControl.SearchType.Default;
            this.ScCopy.TabIndex = 12;
            this.ScCopy.UseChangeDate = true;
            this.ScCopy.Value1 = null;
            this.ScCopy.Value2 = null;
            this.ScCopy.Value3 = null;
            this.ScCopy.CodeKeyDownEvent += new Search.CKM_SearchControl.KeyEventHandler(this.ScCopy_CodeKeyDownEvent);
            // 
            // btnDisplay
            // 
            this.btnDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnDisplay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDisplay.DefaultBtnSize = false;
            this.btnDisplay.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnDisplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisplay.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnDisplay.Location = new System.Drawing.Point(410, 3);
            this.btnDisplay.Margin = new System.Windows.Forms.Padding(1);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(118, 28);
            this.btnDisplay.TabIndex = 1;
            this.btnDisplay.Text = "表示(F11)";
            this.btnDisplay.UseVisualStyleBackColor = false;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // PanelDetail
            // 
            this.PanelDetail.Controls.Add(this.ckM_SearchControl1);
            this.PanelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelDetail.Location = new System.Drawing.Point(0, 147);
            this.PanelDetail.Name = "PanelDetail";
            this.PanelDetail.Size = new System.Drawing.Size(1776, 782);
            this.PanelDetail.TabIndex = 9;
            // 
            // ckM_SearchControl1
            // 
            this.ckM_SearchControl1.AutoSize = true;
            this.ckM_SearchControl1.ChangeDate = "";
            this.ckM_SearchControl1.ChangeDateWidth = 100;
            this.ckM_SearchControl1.Code = "";
            this.ckM_SearchControl1.CodeWidth = 30;
            this.ckM_SearchControl1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_SearchControl1.DataCheck = false;
            this.ckM_SearchControl1.IsCopy = false;
            this.ckM_SearchControl1.LabelText = "";
            this.ckM_SearchControl1.LabelVisible = true;
            this.ckM_SearchControl1.Location = new System.Drawing.Point(25, 17);
            this.ckM_SearchControl1.Name = "ckM_SearchControl1";
            this.ckM_SearchControl1.SearchEnable = true;
            this.ckM_SearchControl1.Size = new System.Drawing.Size(414, 52);
            this.ckM_SearchControl1.Stype = Search.CKM_SearchControl.SearchType.銀行口座;
            this.ckM_SearchControl1.TabIndex = 0;
            this.ckM_SearchControl1.UseChangeDate = false;
            this.ckM_SearchControl1.Value1 = null;
            this.ckM_SearchControl1.Value2 = null;
            this.ckM_SearchControl1.Value3 = null;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1776, 961);
            this.Controls.Add(this.PanelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "Form1";
            this.Text = "Form1";
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

        }

        #endregion

        private System.Windows.Forms.Panel PanelCopy;
        private System.Windows.Forms.Panel PanelNormal;
        private CKM_Controls.CKM_Button btnDisplay;
        private Search.CKM_SearchControl ScNormal;
        private Search.CKM_SearchControl ScCopy;
        private System.Windows.Forms.Panel PanelDetail;
        private Search.CKM_SearchControl ckM_SearchControl1;
    }
}

