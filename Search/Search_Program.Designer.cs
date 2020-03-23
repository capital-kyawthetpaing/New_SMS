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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.txtProgram_ID = new CKM_Controls.CKM_TextBox();
            this.txtProgramName = new CKM_Controls.CKM_TextBox();
            this.btnSearch = new CKM_Controls.CKM_Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvSearchProgram = new CKM_Controls.CKM_GridView();
            this.colProgramID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProgramName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PanelHeader.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchProgram)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.btnSearch);
            this.PanelHeader.Controls.Add(this.txtProgramName);
            this.PanelHeader.Controls.Add(this.txtProgram_ID);
            this.PanelHeader.Controls.Add(this.ckM_Label2);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Size = new System.Drawing.Size(800, 88);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtProgram_ID, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtProgramName, 0);
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
            this.ckM_Label1.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label1.Location = new System.Drawing.Point(16, 10);
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
            this.ckM_Label2.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label2.Location = new System.Drawing.Point(18, 35);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label2.TabIndex = 3;
            this.ckM_Label2.Text = "プログラム名";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProgram_ID
            // 
            this.txtProgram_ID.AllowMinus = false;
            this.txtProgram_ID.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtProgram_ID.BackColor = System.Drawing.Color.White;
            this.txtProgram_ID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProgram_ID.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtProgram_ID.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtProgram_ID.DecimalPlace = 0;
            this.txtProgram_ID.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtProgram_ID.IntegerPart = 0;
            this.txtProgram_ID.IsCorrectDate = true;
            this.txtProgram_ID.isEnterKeyDown = false;
            this.txtProgram_ID.IsNumber = true;
            this.txtProgram_ID.IsShop = false;
            this.txtProgram_ID.Length = 100;
            this.txtProgram_ID.Location = new System.Drawing.Point(103, 6);
            this.txtProgram_ID.MaxLength = 100;
            this.txtProgram_ID.MoveNext = true;
            this.txtProgram_ID.Name = "txtProgram_ID";
            this.txtProgram_ID.Size = new System.Drawing.Size(570, 19);
            this.txtProgram_ID.TabIndex = 4;
            this.txtProgram_ID.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // txtProgramName
            // 
            this.txtProgramName.AllowMinus = false;
            this.txtProgramName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.txtProgramName.BackColor = System.Drawing.Color.White;
            this.txtProgramName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProgramName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtProgramName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.txtProgramName.DecimalPlace = 0;
            this.txtProgramName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtProgramName.IntegerPart = 0;
            this.txtProgramName.IsCorrectDate = true;
            this.txtProgramName.isEnterKeyDown = false;
            this.txtProgramName.IsNumber = true;
            this.txtProgramName.IsShop = false;
            this.txtProgramName.Length = 10;
            this.txtProgramName.Location = new System.Drawing.Point(104, 31);
            this.txtProgramName.MaxLength = 10;
            this.txtProgramName.MoveNext = true;
            this.txtProgramName.Name = "txtProgramName";
            this.txtProgramName.Size = new System.Drawing.Size(570, 19);
            this.txtProgramName.TabIndex = 5;
            this.txtProgramName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
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
            this.btnSearch.Location = new System.Drawing.Point(672, 55);
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
            this.panel1.Controls.Add(this.dgvSearchProgram);
            this.panel1.Location = new System.Drawing.Point(5, 135);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(790, 435);
            this.panel1.TabIndex = 5;
            // 
            // dgvSearchProgram
            // 
            this.dgvSearchProgram.AllowUserToAddRows = false;
            this.dgvSearchProgram.AllowUserToDeleteRows = false;
            this.dgvSearchProgram.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.dgvSearchProgram.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSearchProgram.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSearchProgram.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSearchProgram.ColumnHeadersHeight = 25;
            this.dgvSearchProgram.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProgramID,
            this.ProgramName});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("MS Gothic", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSearchProgram.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSearchProgram.EnableHeadersVisualStyles = false;
            this.dgvSearchProgram.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvSearchProgram.Location = new System.Drawing.Point(38, 22);
            this.dgvSearchProgram.Name = "dgvSearchProgram";
            this.dgvSearchProgram.Size = new System.Drawing.Size(720, 380);
            this.dgvSearchProgram.TabIndex = 0;
            this.dgvSearchProgram.UseRowNo = true;
            this.dgvSearchProgram.UseSetting = false;
            this.dgvSearchProgram.DoubleClick += new System.EventHandler(this.dgvSearchProgram_DoubleClick);
            // 
            // colProgramID
            // 
            this.colProgramID.DataPropertyName = "ProgramID";
            this.colProgramID.HeaderText = "プログラムID";
            this.colProgramID.Name = "colProgramID";
            this.colProgramID.Visible = false;
            // 
            // ProgramName
            // 
            this.ProgramName.DataPropertyName = "ProgramName";
            this.ProgramName.HeaderText = "プログラム名";
            this.ProgramName.Name = "ProgramName";
            this.ProgramName.Width = 670;
            // 
            // Search_Program
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 611);
            this.Controls.Add(this.panel1);
            this.F11Visible = true;
            this.F9Visible = true;
            this.Name = "Search_Program";
            this.PanelHeaderHeight = 130;
            this.Text = "Search_Program";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Search_Program_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Search_Program_KeyUp);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchProgram)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CKM_Controls.CKM_Button btnSearch;
        private CKM_Controls.CKM_TextBox txtProgramName;
        private CKM_Controls.CKM_TextBox txtProgram_ID;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Label ckM_Label1;
        private System.Windows.Forms.Panel panel1;
        private CKM_Controls.CKM_GridView dgvSearchProgram;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProgramID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProgramName;
    }
}