namespace Search
{
    partial class Search_Program
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
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_TextBox1 = new CKM_Controls.CKM_TextBox();
            this.ckM_TextBox2 = new CKM_Controls.CKM_TextBox();
            this.btnSearch = new CKM_Controls.CKM_Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckM_GridView1 = new CKM_Controls.CKM_GridView();
            this.ProgramName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PanelHeader.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckM_GridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.btnSearch);
            this.PanelHeader.Controls.Add(this.ckM_TextBox2);
            this.PanelHeader.Controls.Add(this.ckM_TextBox1);
            this.PanelHeader.Controls.Add(this.ckM_Label2);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Size = new System.Drawing.Size(800, 88);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_TextBox2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.btnSearch, 0);
            // 
            // ckM_Label1
            // 
            this.ckM_Label1.AutoSize = true;
            this.ckM_Label1.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label1.DefaultlabelSize = true;
            this.ckM_Label1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label1.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label1.Location = new System.Drawing.Point(37, 9);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(84, 12);
            this.ckM_Label1.TabIndex = 2;
            this.ckM_Label1.Text = "プログラムID";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_Label2
            // 
            this.ckM_Label2.AutoSize = true;
            this.ckM_Label2.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label2.DefaultlabelSize = true;
            this.ckM_Label2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label2.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label2.Location = new System.Drawing.Point(36, 34);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label2.TabIndex = 3;
            this.ckM_Label2.Text = "プログラム名";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckM_TextBox1
            // 
            this.ckM_TextBox1.AllowMinus = false;
            this.ckM_TextBox1.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox1.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_TextBox1.DecimalPlace = 0;
            this.ckM_TextBox1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_TextBox1.IntegerPart = 0;
            this.ckM_TextBox1.IsCorrectDate = true;
            this.ckM_TextBox1.isEnterKeyDown = false;
            this.ckM_TextBox1.IsNumber = true;
            this.ckM_TextBox1.IsShop = false;
            this.ckM_TextBox1.Length = 100;
            this.ckM_TextBox1.Location = new System.Drawing.Point(131, 6);
            this.ckM_TextBox1.MaxLength = 100;
            this.ckM_TextBox1.MoveNext = true;
            this.ckM_TextBox1.Name = "ckM_TextBox1";
            this.ckM_TextBox1.Size = new System.Drawing.Size(500, 19);
            this.ckM_TextBox1.TabIndex = 4;
            this.ckM_TextBox1.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_TextBox2
            // 
            this.ckM_TextBox2.AllowMinus = false;
            this.ckM_TextBox2.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox2.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_TextBox2.DecimalPlace = 0;
            this.ckM_TextBox2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_TextBox2.IntegerPart = 0;
            this.ckM_TextBox2.IsCorrectDate = true;
            this.ckM_TextBox2.isEnterKeyDown = false;
            this.ckM_TextBox2.IsNumber = true;
            this.ckM_TextBox2.IsShop = false;
            this.ckM_TextBox2.Length = 10;
            this.ckM_TextBox2.Location = new System.Drawing.Point(132, 31);
            this.ckM_TextBox2.MoveNext = true;
            this.ckM_TextBox2.Name = "ckM_TextBox2";
            this.ckM_TextBox2.Size = new System.Drawing.Size(500, 19);
            this.ckM_TextBox2.TabIndex = 5;
            this.ckM_TextBox2.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.btnSearch.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.DefaultBtnSize = false;
            this.btnSearch.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.btnSearch.Location = new System.Drawing.Point(678, 54);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 28);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "表示(F11)";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ckM_GridView1);
            this.panel1.Location = new System.Drawing.Point(4, 135);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(790, 270);
            this.panel1.TabIndex = 5;
            // 
            // ckM_GridView1
            // 
            this.ckM_GridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ckM_GridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProgramName});
            this.ckM_GridView1.Location = new System.Drawing.Point(20, 22);
            this.ckM_GridView1.Name = "ckM_GridView1";
            this.ckM_GridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ckM_GridView1.Size = new System.Drawing.Size(750, 230);
            this.ckM_GridView1.TabIndex = 0;
            this.ckM_GridView1.UseRowNo = true;
            this.ckM_GridView1.UseSetting = false;
            // 
            // ProgramName
            // 
            this.ProgramName.HeaderText = "プログラム名";
            this.ProgramName.Name = "ProgramName";
            this.ProgramName.Width = 700;
            // 
            // Search_Program
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.F11Visible = true;
            this.F9Visible = true;
            this.Name = "Search_Program";
            this.PanelHeaderHeight = 130;
            this.Text = "Search_Program";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ckM_GridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CKM_Controls.CKM_Button btnSearch;
        private CKM_Controls.CKM_TextBox ckM_TextBox2;
        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Label ckM_Label1;
        private System.Windows.Forms.Panel panel1;
        private CKM_Controls.CKM_GridView ckM_GridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProgramName;
    }
}