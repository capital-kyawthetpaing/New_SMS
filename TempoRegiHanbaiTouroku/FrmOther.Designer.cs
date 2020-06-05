namespace TempoRegiHanbaiTouroku
{
    partial class FrmOther
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new CKM_Controls.CKM_Button();
            this.btnProcess = new CKM_Controls.CKM_Button();
            this.ckmShop_RadioButton1 = new CKM_Controls.CKMShop_RadioButton();
            this.ckmShop_RadioButton2 = new CKM_Controls.CKMShop_RadioButton();
            this.ckmShop_RadioButton3 = new CKM_Controls.CKMShop_RadioButton();
            this.ckmShop_RadioButton4 = new CKM_Controls.CKMShop_RadioButton();
            this.ckmShop_RadioButton5 = new CKM_Controls.CKMShop_RadioButton();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tableLayoutPanel1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 392);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(970, 53);
            this.panel4.TabIndex = 82;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnProcess, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(970, 53);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(242)))), ((int)(((byte)(204)))));
            this.btnClose.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Yellow;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.DefaultBtnSize = false;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("MS Gothic", 30F, System.Drawing.FontStyle.Bold);
            this.btnClose.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.XLarge;
            this.btnClose.Location = new System.Drawing.Point(1, 1);
            this.btnClose.Margin = new System.Windows.Forms.Padding(1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(289, 51);
            this.btnClose.TabIndex = 2;
            this.btnClose.Tag = "0";
            this.btnClose.Text = "戻　る";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(242)))), ((int)(((byte)(204)))));
            this.btnProcess.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Yellow;
            this.btnProcess.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProcess.DefaultBtnSize = false;
            this.btnProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProcess.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnProcess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcess.Font = new System.Drawing.Font("MS Gothic", 30F, System.Drawing.FontStyle.Bold);
            this.btnProcess.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.XLarge;
            this.btnProcess.Location = new System.Drawing.Point(292, 1);
            this.btnProcess.Margin = new System.Windows.Forms.Padding(1);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(677, 51);
            this.btnProcess.TabIndex = 1;
            this.btnProcess.Tag = "1";
            this.btnProcess.Text = "決　定";
            this.btnProcess.UseVisualStyleBackColor = false;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // ckmShop_RadioButton1
            // 
            this.ckmShop_RadioButton1.Font = new System.Drawing.Font("MS Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ckmShop_RadioButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_RadioButton1.Location = new System.Drawing.Point(104, 60);
            this.ckmShop_RadioButton1.Name = "ckmShop_RadioButton1";
            this.ckmShop_RadioButton1.Size = new System.Drawing.Size(125, 34);
            this.ckmShop_RadioButton1.TabIndex = 0;
            this.ckmShop_RadioButton1.TabStop = true;
            this.ckmShop_RadioButton1.Text = "小学生";
            this.ckmShop_RadioButton1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ckmShop_RadioButton1.UseVisualStyleBackColor = true;
            // 
            // ckmShop_RadioButton2
            // 
            this.ckmShop_RadioButton2.Font = new System.Drawing.Font("MS Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ckmShop_RadioButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_RadioButton2.Location = new System.Drawing.Point(104, 114);
            this.ckmShop_RadioButton2.Name = "ckmShop_RadioButton2";
            this.ckmShop_RadioButton2.Size = new System.Drawing.Size(125, 34);
            this.ckmShop_RadioButton2.TabIndex = 1;
            this.ckmShop_RadioButton2.TabStop = true;
            this.ckmShop_RadioButton2.Text = "中学生";
            this.ckmShop_RadioButton2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ckmShop_RadioButton2.UseVisualStyleBackColor = true;
            // 
            // ckmShop_RadioButton3
            // 
            this.ckmShop_RadioButton3.Font = new System.Drawing.Font("MS Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ckmShop_RadioButton3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_RadioButton3.Location = new System.Drawing.Point(104, 168);
            this.ckmShop_RadioButton3.Name = "ckmShop_RadioButton3";
            this.ckmShop_RadioButton3.Size = new System.Drawing.Size(125, 34);
            this.ckmShop_RadioButton3.TabIndex = 2;
            this.ckmShop_RadioButton3.TabStop = true;
            this.ckmShop_RadioButton3.Text = "高校生";
            this.ckmShop_RadioButton3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ckmShop_RadioButton3.UseVisualStyleBackColor = true;
            // 
            // ckmShop_RadioButton4
            // 
            this.ckmShop_RadioButton4.Font = new System.Drawing.Font("MS Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ckmShop_RadioButton4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_RadioButton4.Location = new System.Drawing.Point(104, 222);
            this.ckmShop_RadioButton4.Name = "ckmShop_RadioButton4";
            this.ckmShop_RadioButton4.Size = new System.Drawing.Size(125, 34);
            this.ckmShop_RadioButton4.TabIndex = 3;
            this.ckmShop_RadioButton4.TabStop = true;
            this.ckmShop_RadioButton4.Text = "大学・一般";
            this.ckmShop_RadioButton4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ckmShop_RadioButton4.UseVisualStyleBackColor = true;
            // 
            // ckmShop_RadioButton5
            // 
            this.ckmShop_RadioButton5.Font = new System.Drawing.Font("MS Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ckmShop_RadioButton5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_RadioButton5.Location = new System.Drawing.Point(104, 276);
            this.ckmShop_RadioButton5.Name = "ckmShop_RadioButton5";
            this.ckmShop_RadioButton5.Size = new System.Drawing.Size(125, 34);
            this.ckmShop_RadioButton5.TabIndex = 4;
            this.ckmShop_RadioButton5.TabStop = true;
            this.ckmShop_RadioButton5.Text = "その他";
            this.ckmShop_RadioButton5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ckmShop_RadioButton5.UseVisualStyleBackColor = true;
            // 
            // FrmOther
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(970, 445);
            this.Controls.Add(this.ckmShop_RadioButton5);
            this.Controls.Add(this.ckmShop_RadioButton4);
            this.Controls.Add(this.ckmShop_RadioButton3);
            this.Controls.Add(this.ckmShop_RadioButton2);
            this.Controls.Add(this.ckmShop_RadioButton1);
            this.Controls.Add(this.panel4);
            this.Name = "FrmOther";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TempoRegiHanbaiTouroku";
            this.Load += new System.EventHandler(this.Form_Load);
            this.panel4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        protected CKM_Controls.CKM_Button btnClose;
        protected CKM_Controls.CKM_Button btnProcess;
        private CKM_Controls.CKMShop_RadioButton ckmShop_RadioButton1;
        private CKM_Controls.CKMShop_RadioButton ckmShop_RadioButton2;
        private CKM_Controls.CKMShop_RadioButton ckmShop_RadioButton3;
        private CKM_Controls.CKMShop_RadioButton ckmShop_RadioButton4;
        private CKM_Controls.CKMShop_RadioButton ckmShop_RadioButton5;
    }
}

