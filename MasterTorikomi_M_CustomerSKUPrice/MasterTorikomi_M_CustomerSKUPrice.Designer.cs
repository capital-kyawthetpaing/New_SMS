namespace MasterTorikomi_M_CustomerSKUPrice
{
    partial class MasterTorikomi_M_CustomerSKUPrice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterTorikomi_M_CustomerSKUPrice));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.inputPath = new CKM_Controls.CKM_TextBox();
            this.BT_Torikomi = new CKM_Controls.CKM_Button();
            this.gvItem = new CKM_Controls.CKM_GridView();
            this.colCusotmer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJanCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAppDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Error = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gvItem)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1882, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(1348, 0);
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("MS Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(778, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 16);
            this.label2.TabIndex = 118;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(695, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "▼";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ckM_Label3
            // 
            this.ckM_Label3.AutoSize = true;
            this.ckM_Label3.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label3.DefaultlabelSize = true;
            this.ckM_Label3.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label3.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label3.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label3.Location = new System.Drawing.Point(3, 82);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label3.TabIndex = 116;
            this.ckM_Label3.Text = "取込ファイル";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // inputPath
            // 
            this.inputPath.AllowMinus = false;
            this.inputPath.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.inputPath.BackColor = System.Drawing.Color.White;
            this.inputPath.BorderColor = false;
            this.inputPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputPath.ClientColor = System.Drawing.SystemColors.Window;
            this.inputPath.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.inputPath.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.inputPath.DecimalPlace = 0;
            this.inputPath.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.inputPath.IntegerPart = 0;
            this.inputPath.IsCorrectDate = true;
            this.inputPath.isEnterKeyDown = false;
            this.inputPath.IsFirstTime = true;
            this.inputPath.isMaxLengthErr = false;
            this.inputPath.IsNumber = true;
            this.inputPath.IsShop = false;
            this.inputPath.IsTimemmss = false;
            this.inputPath.Length = 32767;
            this.inputPath.Location = new System.Drawing.Point(86, 78);
            this.inputPath.MoveNext = true;
            this.inputPath.Name = "inputPath";
            this.inputPath.ReadOnly = true;
            this.inputPath.Size = new System.Drawing.Size(610, 19);
            this.inputPath.TabIndex = 0;
            this.inputPath.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.inputPath.UseColorSizMode = false;
            // 
            // BT_Torikomi
            // 
            this.BT_Torikomi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BT_Torikomi.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BT_Torikomi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BT_Torikomi.DefaultBtnSize = false;
            this.BT_Torikomi.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BT_Torikomi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_Torikomi.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.BT_Torikomi.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BT_Torikomi.Location = new System.Drawing.Point(1738, 66);
            this.BT_Torikomi.Margin = new System.Windows.Forms.Padding(1);
            this.BT_Torikomi.Name = "BT_Torikomi";
            this.BT_Torikomi.Size = new System.Drawing.Size(100, 28);
            this.BT_Torikomi.TabIndex = 2;
            this.BT_Torikomi.Text = "取込(F12)";
            this.BT_Torikomi.UseVisualStyleBackColor = false;
            this.BT_Torikomi.Click += new System.EventHandler(this.BT_Torikomi_Click);
            // 
            // gvItem
            // 
            this.gvItem.AllowUserToAddRows = false;
            this.gvItem.AllowUserToDeleteRows = false;
            this.gvItem.AllowUserToResizeColumns = false;
            this.gvItem.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.gvItem.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gvItem.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.gvItem.CheckCol = ((System.Collections.ArrayList)(resources.GetObject("gvItem.CheckCol")));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvItem.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gvItem.ColumnHeadersHeight = 25;
            this.gvItem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCusotmer,
            this.colSKUCD,
            this.colJanCD,
            this.colAppDate,
            this.ItemName,
            this.colColor,
            this.colSize,
            this.EItem,
            this.Error});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("MS Gothic", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvItem.DefaultCellStyle = dataGridViewCellStyle6;
            this.gvItem.EnableHeadersVisualStyles = false;
            this.gvItem.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.gvItem.Location = new System.Drawing.Point(86, 118);
            this.gvItem.Name = "gvItem";
            this.gvItem.RowHeight_ = 20;
            this.gvItem.RowTemplate.Height = 20;
            this.gvItem.Size = new System.Drawing.Size(1786, 755);
            this.gvItem.TabIndex = 3;
            this.gvItem.UseRowNo = true;
            this.gvItem.UseSetting = false;
            this.gvItem.Paint += new System.Windows.Forms.PaintEventHandler(this.gvItem_Paint);
            // 
            // colCusotmer
            // 
            this.colCusotmer.DataPropertyName = "Cusotmer";
            this.colCusotmer.HeaderText = "顧客名";
            this.colCusotmer.MaxInputLength = 80;
            this.colCusotmer.Name = "colCusotmer";
            this.colCusotmer.ReadOnly = true;
            this.colCusotmer.Width = 330;
            // 
            // colSKUCD
            // 
            this.colSKUCD.DataPropertyName = "SKUCD";
            this.colSKUCD.HeaderText = "SKUCD";
            this.colSKUCD.MaxInputLength = 30;
            this.colSKUCD.Name = "colSKUCD";
            this.colSKUCD.ReadOnly = true;
            this.colSKUCD.Width = 140;
            // 
            // colJanCD
            // 
            this.colJanCD.DataPropertyName = "JANCD";
            this.colJanCD.HeaderText = "JanCD";
            this.colJanCD.MaxInputLength = 13;
            this.colJanCD.Name = "colJanCD";
            this.colJanCD.ReadOnly = true;
            this.colJanCD.Width = 120;
            // 
            // colAppDate
            // 
            this.colAppDate.DataPropertyName = "AppDate";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Format = "d";
            dataGridViewCellStyle3.NullValue = null;
            this.colAppDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.colAppDate.HeaderText = "適用日";
            this.colAppDate.MaxInputLength = 10;
            this.colAppDate.Name = "colAppDate";
            this.colAppDate.ReadOnly = true;
            // 
            // ItemName
            // 
            this.ItemName.DataPropertyName = "ItemName";
            this.ItemName.HeaderText = "商品名";
            this.ItemName.MaxInputLength = 80;
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            this.ItemName.Width = 360;
            // 
            // colColor
            // 
            this.colColor.DataPropertyName = "Color";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.NullValue = null;
            this.colColor.DefaultCellStyle = dataGridViewCellStyle4;
            this.colColor.HeaderText = "カラー";
            this.colColor.MaxInputLength = 20;
            this.colColor.Name = "colColor";
            this.colColor.ReadOnly = true;
            // 
            // colSize
            // 
            this.colSize.DataPropertyName = "Size";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colSize.DefaultCellStyle = dataGridViewCellStyle5;
            this.colSize.HeaderText = "サイズ";
            this.colSize.MaxInputLength = 20;
            this.colSize.Name = "colSize";
            this.colSize.ReadOnly = true;
            // 
            // EItem
            // 
            this.EItem.DataPropertyName = "EItem";
            this.EItem.HeaderText = "エラー";
            this.EItem.Name = "EItem";
            this.EItem.ReadOnly = true;
            // 
            // Error
            // 
            this.Error.DataPropertyName = "Error";
            this.Error.HeaderText = "";
            this.Error.Name = "Error";
            this.Error.ReadOnly = true;
            this.Error.Width = 375;
            // 
            // MasterTorikomi_M_CustomerSKUPrice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1884, 931);
            this.Controls.Add(this.gvItem);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ckM_Label3);
            this.Controls.Add(this.inputPath);
            this.Controls.Add(this.BT_Torikomi);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "MasterTorikomi_M_CustomerSKUPrice";
            this.PanelHeaderHeight = 50;
            this.Text = "MasterTorikomi_M_CustomerSKUPrice";
            this.Load += new System.EventHandler(this.MasterTorikomi_M_CustomerSKUPrice_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MasterTorikomi_M_CustomerSKUPrice_KeyUp);
            this.Controls.SetChildIndex(this.BT_Torikomi, 0);
            this.Controls.SetChildIndex(this.inputPath, 0);
            this.Controls.SetChildIndex(this.ckM_Label3, 0);
            this.Controls.SetChildIndex(this.button1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.gvItem, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gvItem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_TextBox inputPath;
        private CKM_Controls.CKM_Button BT_Torikomi;
        private CKM_Controls.CKM_GridView gvItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCusotmer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJanCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAppDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn EItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Error;
    }
}

