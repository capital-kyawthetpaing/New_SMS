namespace Base.Client
{
    partial class Search_Base
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
            this.lblSearch_Name = new CKM_Controls.CKMShop_Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ckM_Button2 = new CKM_Controls.CKM_Button();
            this.ckM_Button1 = new CKM_Controls.CKM_Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSearch_Name
            // 
            this.lblSearch_Name.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.lblSearch_Name.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(208)))), ((int)(((byte)(142)))));
            this.lblSearch_Name.Font = new System.Drawing.Font("MS Gothic", 32F, System.Drawing.FontStyle.Bold);
            this.lblSearch_Name.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.XLarge;
            this.lblSearch_Name.FontBold = true;
            this.lblSearch_Name.ForeColor = System.Drawing.Color.Black;
            this.lblSearch_Name.Location = new System.Drawing.Point(12, 9);
            this.lblSearch_Name.Name = "lblSearch_Name";
            this.lblSearch_Name.Size = new System.Drawing.Size(400, 40);
            this.lblSearch_Name.TabIndex = 0;
            this.lblSearch_Name.Text = "Search_Name";
            this.lblSearch_Name.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.lblSearch_Name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.17815F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.82185F));
            this.tableLayoutPanel1.Controls.Add(this.ckM_Button2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.ckM_Button1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 909);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1913, 52);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // ckM_Button2
            // 
            this.ckM_Button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(242)))), ((int)(((byte)(204)))));
            this.ckM_Button2.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Yellow;
            this.ckM_Button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckM_Button2.DefaultBtnSize = false;
            this.ckM_Button2.Dock = System.Windows.Forms.DockStyle.Right;
            this.ckM_Button2.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.ckM_Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckM_Button2.Font = new System.Drawing.Font("MS Gothic", 30F, System.Drawing.FontStyle.Bold);
            this.ckM_Button2.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.XLarge;
            this.ckM_Button2.Location = new System.Drawing.Point(960, 1);
            this.ckM_Button2.Margin = new System.Windows.Forms.Padding(1);
            this.ckM_Button2.Name = "ckM_Button2";
            this.ckM_Button2.Size = new System.Drawing.Size(952, 50);
            this.ckM_Button2.TabIndex = 2;
            this.ckM_Button2.Tag = "1";
            this.ckM_Button2.Text = "決 定";
            this.ckM_Button2.UseVisualStyleBackColor = false;
            this.ckM_Button2.Click += new System.EventHandler(this.Btn_Click);
            // 
            // ckM_Button1
            // 
            this.ckM_Button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(242)))), ((int)(((byte)(204)))));
            this.ckM_Button1.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Yellow;
            this.ckM_Button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckM_Button1.DefaultBtnSize = false;
            this.ckM_Button1.Dock = System.Windows.Forms.DockStyle.Left;
            this.ckM_Button1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.ckM_Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckM_Button1.Font = new System.Drawing.Font("MS Gothic", 30F, System.Drawing.FontStyle.Bold);
            this.ckM_Button1.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.XLarge;
            this.ckM_Button1.Location = new System.Drawing.Point(1, 1);
            this.ckM_Button1.Margin = new System.Windows.Forms.Padding(1);
            this.ckM_Button1.Name = "ckM_Button1";
            this.ckM_Button1.Size = new System.Drawing.Size(957, 50);
            this.ckM_Button1.TabIndex = 0;
            this.ckM_Button1.Tag = "0";
            this.ckM_Button1.Text = "戻 る";
            this.ckM_Button1.UseVisualStyleBackColor = false;
            this.ckM_Button1.Click += new System.EventHandler(this.Btn_Click);
            // 
            // Search_Base
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1913, 961);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lblSearch_Name);
            this.Name = "Search_Base";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search_Base";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CKM_Controls.CKM_Button ckM_Button2;
        private CKM_Controls.CKM_Button ckM_Button1;
        internal CKM_Controls.CKMShop_Label lblSearch_Name;
    }
}