namespace Base.Client
{
    partial class ShopBaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShopBaseForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new CKM_Controls.CKM_Button();
            this.btnProcess = new CKM_Controls.CKM_Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.lblShopName = new CKM_Controls.CKMShop_Label();
            this.lblStoreName = new CKM_Controls.CKMShop_Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.lblOperatorName = new CKM_Controls.CKMShop_Label();
            this.panel6 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(252, 75);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1713, 53);
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
            this.btnClose.Size = new System.Drawing.Size(511, 51);
            this.btnClose.TabIndex = 0;
            this.btnClose.Tag = "0";
            this.btnClose.Text = "戻　る";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.Btn_Click);
            this.btnClose.MouseEnter += new System.EventHandler(this.btnClose_MouseEnter);
            this.btnClose.MouseLeave += new System.EventHandler(this.btnClose_MouseLeave);
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
            this.btnProcess.Location = new System.Drawing.Point(514, 1);
            this.btnProcess.Margin = new System.Windows.Forms.Padding(1);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(1198, 51);
            this.btnProcess.TabIndex = 1;
            this.btnProcess.Tag = "1";
            this.btnProcess.Text = "登  録";
            this.btnProcess.UseVisualStyleBackColor = false;
            this.btnProcess.Click += new System.EventHandler(this.Btn_Click);
            this.btnProcess.MouseEnter += new System.EventHandler(this.btnClose_MouseEnter);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tableLayoutPanel1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 908);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1713, 53);
            this.panel4.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel8);
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1713, 75);
            this.panel5.TabIndex = 3;
            // 
            // panel8
            // 
            this.panel8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel8.Controls.Add(this.lblShopName);
            this.panel8.Controls.Add(this.lblStoreName);
            this.panel8.Location = new System.Drawing.Point(574, 3);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(698, 69);
            this.panel8.TabIndex = 2;
            // 
            // lblShopName
            // 
            this.lblShopName.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblShopName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblShopName.Font = new System.Drawing.Font("MS Gothic", 32F, System.Drawing.FontStyle.Bold);
            this.lblShopName.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.XLarge;
            this.lblShopName.FontBold = true;
            this.lblShopName.ForeColor = System.Drawing.Color.Black;
            this.lblShopName.Location = new System.Drawing.Point(14, 13);
            this.lblShopName.Name = "lblShopName";
            this.lblShopName.Size = new System.Drawing.Size(250, 40);
            this.lblShopName.TabIndex = 3;
            this.lblShopName.Text = "XXXX";
            this.lblShopName.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblShopName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblShopName.Visible = false;
            // 
            // lblStoreName
            // 
            this.lblStoreName.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblStoreName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblStoreName.Font = new System.Drawing.Font("MS Gothic", 32F, System.Drawing.FontStyle.Bold);
            this.lblStoreName.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.XLarge;
            this.lblStoreName.FontBold = true;
            this.lblStoreName.ForeColor = System.Drawing.Color.Black;
            this.lblStoreName.Location = new System.Drawing.Point(264, 13);
            this.lblStoreName.Name = "lblStoreName";
            this.lblStoreName.Size = new System.Drawing.Size(200, 40);
            this.lblStoreName.TabIndex = 2;
            this.lblStoreName.Text = "x x x";
            this.lblStoreName.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblStoreName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStoreName.Visible = false;
            // 
            // panel7
            // 
            this.panel7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel7.Controls.Add(this.lblOperatorName);
            this.panel7.Location = new System.Drawing.Point(1278, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(435, 69);
            this.panel7.TabIndex = 1;
            // 
            // lblOperatorName
            // 
            this.lblOperatorName.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblOperatorName.BackColor = System.Drawing.Color.Transparent;
            this.lblOperatorName.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.lblOperatorName.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Normal;
            this.lblOperatorName.FontBold = true;
            this.lblOperatorName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.lblOperatorName.Location = new System.Drawing.Point(98, 13);
            this.lblOperatorName.Name = "lblOperatorName";
            this.lblOperatorName.Size = new System.Drawing.Size(325, 47);
            this.lblOperatorName.TabIndex = 2;
            this.lblOperatorName.Text = "ＸＸＸＸＸ";
            this.lblOperatorName.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblOperatorName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.pictureBox1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(568, 75);
            this.panel6.TabIndex = 0;
            // 
            // ShopBaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.KeyPreview = true;
            this.Name = "ShopBaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShopBaseForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CKM_Controls.CKMShop_Label lblOperatorName;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        protected CKM_Controls.CKM_Button btnClose;
        protected CKM_Controls.CKM_Button btnProcess;
        private CKM_Controls.CKMShop_Label lblShopName;
        private CKM_Controls.CKMShop_Label lblStoreName;
    }
}