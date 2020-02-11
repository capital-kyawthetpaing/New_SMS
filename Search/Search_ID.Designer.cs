namespace Search
{
    partial class Search_ID
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.txtID1 = new CKM_Controls.CKM_TextBox();
            this.txtID2 = new CKM_Controls.CKM_TextBox();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.F11Show = new CKM_Controls.CKM_Button();
            this.GvID = new CKM_Controls.CKM_GridView();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIDName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvID)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.F11Show);
            this.PanelHeader.Controls.Add(this.ckM_Label2);
            this.PanelHeader.Controls.Add(this.txtID2);
            this.PanelHeader.Controls.Add(this.txtID1);
            this.PanelHeader.Controls.Add(this.ckM_Label1);
            this.PanelHeader.Size = new System.Drawing.Size(712, 108);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtID1, 0);
            this.PanelHeader.Controls.SetChildIndex(this.txtID2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.ckM_Label2, 0);
            this.PanelHeader.Controls.SetChildIndex(this.F11Show, 0);
            // 
            // ckM_Label1
            // 
            this.ckM_Label1.AutoSize = true;
            this.ckM_Label1.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label1.Location = new System.Drawing.Point(46, 23);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(19, 12);
            this.ckM_Label1.TabIndex = 2;
            this.ckM_Label1.Text = "ID";
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtID1
            // 
            this.txtID1.AllowMinus = false;
            this.txtID1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtID1.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtID1.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Number;
            this.txtID1.DecimalPlace = 0;
            this.txtID1.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtID1.IntegerPart = 0;
            this.txtID1.IsCorrectDate = true;
            this.txtID1.isEnterKeyDown = false;
            this.txtID1.IsNumber = true;
            this.txtID1.Length = 3;
            this.txtID1.Location = new System.Drawing.Point(68, 20);
            this.txtID1.MaxLength = 3;
            this.txtID1.MoveNext = true;
            this.txtID1.Name = "txtID1";
            this.txtID1.Size = new System.Drawing.Size(30, 19);
            this.txtID1.TabIndex = 0;
            this.txtID1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtID2
            // 
            this.txtID2.AllowMinus = false;
            this.txtID2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtID2.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtID2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Number;
            this.txtID2.DecimalPlace = 0;
            this.txtID2.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.txtID2.IntegerPart = 0;
            this.txtID2.IsCorrectDate = true;
            this.txtID2.isEnterKeyDown = false;
            this.txtID2.IsNumber = true;
            this.txtID2.Length = 3;
            this.txtID2.Location = new System.Drawing.Point(124, 20);
            this.txtID2.MaxLength = 3;
            this.txtID2.MoveNext = true;
            this.txtID2.Name = "txtID2";
            this.txtID2.Size = new System.Drawing.Size(30, 19);
            this.txtID2.TabIndex = 1;
            this.txtID2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtID2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtID2_KeyDown);
            // 
            // ckM_Label2
            // 
            this.ckM_Label2.AutoSize = true;
            this.ckM_Label2.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label2.Location = new System.Drawing.Point(102, 23);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(18, 12);
            this.ckM_Label2.TabIndex = 5;
            this.ckM_Label2.Text = "～";
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // F11Show
            // 
            this.F11Show.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.F11Show.Cursor = System.Windows.Forms.Cursors.Hand;
            this.F11Show.DefaultBtnSize = false;
            this.F11Show.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.F11Show.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.F11Show.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.F11Show.Location = new System.Drawing.Point(585, 68);
            this.F11Show.Margin = new System.Windows.Forms.Padding(1);
            this.F11Show.Name = "F11Show";
            this.F11Show.Size = new System.Drawing.Size(115, 28);
            this.F11Show.TabIndex = 2;
            this.F11Show.Text = "表示(F11)";
            this.F11Show.UseVisualStyleBackColor = false;
            this.F11Show.Click += new System.EventHandler(this.F11Show_Click);
            // 
            // GvID
            // 
            this.GvID.AllowUserToAddRows = false;
            this.GvID.AllowUserToDeleteRows = false;
            this.GvID.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.GvID.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.GvID.AutoGenerateColumns = false;
            this.GvID.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GvID.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.GvID.ColumnHeadersHeight = 25;
            this.GvID.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colIDName});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("MS Gothic", 9F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.GvID.DefaultCellStyle = dataGridViewCellStyle8;
            this.GvID.EnableHeadersVisualStyles = false;
            this.GvID.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GvID.Location = new System.Drawing.Point(35, 166);
            this.GvID.Name = "GvID";
            this.GvID.Size = new System.Drawing.Size(656, 471);
            this.GvID.TabIndex = 5;
            this.GvID.UseRowNo = true;
            this.GvID.UseSetting = true;
            this.GvID.DoubleClick += new System.EventHandler(this.GvID_DoubleClick);
            // 
            // colID
            // 
            this.colID.DataPropertyName = "Key";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colID.DefaultCellStyle = dataGridViewCellStyle7;
            this.colID.HeaderText = "ID";
            this.colID.Name = "colID";
            this.colID.Width = 70;
            // 
            // colIDName
            // 
            this.colIDName.DataPropertyName = "Char1";
            this.colIDName.HeaderText = "ID内容";
            this.colIDName.Name = "colIDName";
            this.colIDName.Width = 530;
            // 
            // Search_ID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 708);
            this.Controls.Add(this.GvID);
            this.F11Visible = true;
            this.F9Visible = true;
            this.Name = "Search_ID";
            this.PanelHeaderHeight = 150;
            this.ProgramName = "汎用マスター";
            this.Text = "Search_ID";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Search_ID_KeyUp);
            this.Controls.SetChildIndex(this.GvID, 0);
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CKM_Controls.CKM_Button F11Show;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_TextBox txtID2;
        private CKM_Controls.CKM_TextBox txtID1;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_GridView GvID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIDName;
    }
}