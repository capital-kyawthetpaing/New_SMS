namespace Base.Client
{
    partial class FrmSubForm
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
            this.PanelTop = new System.Windows.Forms.Panel();
            this.PanelHeader = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.PanelTitle = new System.Windows.Forms.Panel();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.PanelFooter = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.BtnF9 = new CKM_Controls.CKM_Button();
            this.BtnF12 = new CKM_Controls.CKM_Button();
            this.BtnF11 = new CKM_Controls.CKM_Button();
            this.BtnF1 = new CKM_Controls.CKM_Button();
            this.label2 = new System.Windows.Forms.Label();
            this.PanelTop.SuspendLayout();
            this.PanelHeader.SuspendLayout();
            this.PanelTitle.SuspendLayout();
            this.PanelFooter.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelTop
            // 
            this.PanelTop.Controls.Add(this.PanelHeader);
            this.PanelTop.Controls.Add(this.PanelTitle);
            this.PanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelTop.Location = new System.Drawing.Point(0, 0);
            this.PanelTop.Name = "PanelTop";
            this.PanelTop.Size = new System.Drawing.Size(838, 101);
            this.PanelTop.TabIndex = 0;
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.label1);
            this.PanelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelHeader.Location = new System.Drawing.Point(0, 42);
            this.PanelHeader.Name = "PanelHeader";
            this.PanelHeader.Size = new System.Drawing.Size(838, 59);
            this.PanelHeader.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(0, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(838, 2);
            this.label1.TabIndex = 1;
            // 
            // PanelTitle
            // 
            this.PanelTitle.Controls.Add(this.lblHeaderTitle);
            this.PanelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelTitle.Location = new System.Drawing.Point(0, 0);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(838, 42);
            this.PanelTitle.TabIndex = 1;
            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(173)))), ((int)(((byte)(71)))));
            this.lblHeaderTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblHeaderTitle.Font = new System.Drawing.Font("MS Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderTitle.Location = new System.Drawing.Point(12, 9);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(370, 23);
            this.lblHeaderTitle.TabIndex = 61;
            this.lblHeaderTitle.Text = "ckM_Label2";
            this.lblHeaderTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PanelFooter
            // 
            this.PanelFooter.Controls.Add(this.tableLayoutPanel1);
            this.PanelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelFooter.Location = new System.Drawing.Point(0, 504);
            this.PanelFooter.Name = "PanelFooter";
            this.PanelFooter.Size = new System.Drawing.Size(838, 32);
            this.PanelFooter.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Controls.Add(this.BtnF9, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.BtnF12, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.BtnF11, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.BtnF1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(838, 32);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // BtnF9
            // 
            this.BtnF9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BtnF9.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BtnF9.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnF9.DefaultBtnSize = false;
            this.BtnF9.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnF9.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnF9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnF9.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.BtnF9.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BtnF9.Location = new System.Drawing.Point(279, 1);
            this.BtnF9.Margin = new System.Windows.Forms.Padding(1);
            this.BtnF9.Name = "BtnF9";
            this.BtnF9.Size = new System.Drawing.Size(137, 28);
            this.BtnF9.TabIndex = 3;
            this.BtnF9.Tag = "8";
            this.BtnF9.Text = "検索(F9)";
            this.BtnF9.UseVisualStyleBackColor = false;
            this.BtnF9.Visible = false;
            // 
            // BtnF12
            // 
            this.BtnF12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BtnF12.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BtnF12.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnF12.DefaultBtnSize = false;
            this.BtnF12.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnF12.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnF12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnF12.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.BtnF12.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BtnF12.Location = new System.Drawing.Point(696, 1);
            this.BtnF12.Margin = new System.Windows.Forms.Padding(1);
            this.BtnF12.Name = "BtnF12";
            this.BtnF12.Size = new System.Drawing.Size(141, 28);
            this.BtnF12.TabIndex = 1;
            this.BtnF12.Tag = "11";
            this.BtnF12.Text = "確定(F12)";
            this.BtnF12.UseVisualStyleBackColor = false;
            this.BtnF12.Click += new System.EventHandler(this.Btn_Click);
            // 
            // BtnF11
            // 
            this.BtnF11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BtnF11.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BtnF11.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnF11.DefaultBtnSize = false;
            this.BtnF11.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnF11.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnF11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnF11.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.BtnF11.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BtnF11.Location = new System.Drawing.Point(557, 1);
            this.BtnF11.Margin = new System.Windows.Forms.Padding(1);
            this.BtnF11.Name = "BtnF11";
            this.BtnF11.Size = new System.Drawing.Size(137, 28);
            this.BtnF11.TabIndex = 2;
            this.BtnF11.Tag = "10";
            this.BtnF11.Text = "表示(F11)";
            this.BtnF11.UseVisualStyleBackColor = false;
            this.BtnF11.Visible = false;
            this.BtnF11.Click += new System.EventHandler(this.Btn_Click);
            // 
            // BtnF1
            // 
            this.BtnF1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BtnF1.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BtnF1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnF1.DefaultBtnSize = false;
            this.BtnF1.Dock = System.Windows.Forms.DockStyle.Top;
            this.BtnF1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnF1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnF1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.BtnF1.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BtnF1.Location = new System.Drawing.Point(1, 1);
            this.BtnF1.Margin = new System.Windows.Forms.Padding(1);
            this.BtnF1.Name = "BtnF1";
            this.BtnF1.Size = new System.Drawing.Size(137, 28);
            this.BtnF1.TabIndex = 0;
            this.BtnF1.Tag = "0";
            this.BtnF1.Text = "戻る(F1)";
            this.BtnF1.UseVisualStyleBackColor = false;
            this.BtnF1.Click += new System.EventHandler(this.Btn_Click);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(0, 502);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(838, 2);
            this.label2.TabIndex = 2;
            // 
            // FrmSubForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.ClientSize = new System.Drawing.Size(838, 536);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PanelFooter);
            this.Controls.Add(this.PanelTop);
            this.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSubForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SearchBase";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubForm_KeyDown);
            this.PanelTop.ResumeLayout(false);
            this.PanelHeader.ResumeLayout(false);
            this.PanelTitle.ResumeLayout(false);
            this.PanelFooter.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelTop;
        private System.Windows.Forms.Panel PanelTitle;
        internal System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Panel PanelFooter;
        private CKM_Controls.CKM_Button BtnF1;
        private CKM_Controls.CKM_Button BtnF12;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CKM_Controls.CKM_Button BtnF9;
        private CKM_Controls.CKM_Button BtnF11;
        protected System.Windows.Forms.Panel PanelHeader;
        private System.Windows.Forms.Label label1;
    }
}