﻿namespace MasterTorikomi_SKU
{
    partial class MasterTorikomi_SKU
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.RB_tagInfo = new CKM_Controls.CKM_RadioButton();
            this.RB_SizeURL = new CKM_Controls.CKM_RadioButton();
            this.RB_JanCD = new CKM_Controls.CKM_RadioButton();
            this.RB_Catloginfo = new CKM_Controls.CKM_RadioButton();
            this.RB_priceinfo = new CKM_Controls.CKM_RadioButton();
            this.RB_attributeinfo = new CKM_Controls.CKM_RadioButton();
            this.RB_all = new CKM_Controls.CKM_RadioButton();
            this.RB_BaseInfo = new CKM_Controls.CKM_RadioButton();
            this.BT_Torikomi = new CKM_Controls.CKM_Button();
            this.GV_SKU = new CKM_Controls.CKM_GridView();
            this.colSKU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colskuname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TB_FileName = new CKM_Controls.CKM_TextBox();
            this.BT_FileName = new CKM_Controls.CKM_Button();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GV_SKU)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1682, 0);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(1148, 0);
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
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
            this.ckM_Label2.Location = new System.Drawing.Point(32, 78);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(83, 12);
            this.ckM_Label2.TabIndex = 105;
            this.ckM_Label2.Text = "取込ファイル";
            this.ckM_Label2.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label1.Location = new System.Drawing.Point(32, 54);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(57, 12);
            this.ckM_Label1.TabIndex = 104;
            this.ckM_Label1.Text = "取込対象";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.RB_tagInfo);
            this.panel3.Controls.Add(this.RB_SizeURL);
            this.panel3.Controls.Add(this.RB_JanCD);
            this.panel3.Controls.Add(this.RB_Catloginfo);
            this.panel3.Controls.Add(this.RB_priceinfo);
            this.panel3.Controls.Add(this.RB_attributeinfo);
            this.panel3.Controls.Add(this.RB_all);
            this.panel3.Controls.Add(this.RB_BaseInfo);
            this.panel3.Location = new System.Drawing.Point(117, 49);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(700, 25);
            this.panel3.TabIndex = 0;
            // 
            // RB_tagInfo
            // 
            this.RB_tagInfo.AutoSize = true;
            this.RB_tagInfo.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.RB_tagInfo.Location = new System.Drawing.Point(386, 2);
            this.RB_tagInfo.Name = "RB_tagInfo";
            this.RB_tagInfo.Size = new System.Drawing.Size(75, 16);
            this.RB_tagInfo.TabIndex = 1;
            this.RB_tagInfo.TabStop = true;
            this.RB_tagInfo.Text = "タグ情報";
            this.RB_tagInfo.UseVisualStyleBackColor = true;
            // 
            // RB_SizeURL
            // 
            this.RB_SizeURL.AutoSize = true;
            this.RB_SizeURL.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.RB_SizeURL.Location = new System.Drawing.Point(518, 2);
            this.RB_SizeURL.Name = "RB_SizeURL";
            this.RB_SizeURL.Size = new System.Drawing.Size(83, 16);
            this.RB_SizeURL.TabIndex = 99;
            this.RB_SizeURL.TabStop = true;
            this.RB_SizeURL.Text = "サイトURL";
            this.RB_SizeURL.UseVisualStyleBackColor = true;
            // 
            // RB_JanCD
            // 
            this.RB_JanCD.AutoSize = true;
            this.RB_JanCD.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.RB_JanCD.Location = new System.Drawing.Point(461, 2);
            this.RB_JanCD.Name = "RB_JanCD";
            this.RB_JanCD.Size = new System.Drawing.Size(58, 16);
            this.RB_JanCD.TabIndex = 0;
            this.RB_JanCD.TabStop = true;
            this.RB_JanCD.Text = "JANCD";
            this.RB_JanCD.UseVisualStyleBackColor = true;
            // 
            // RB_Catloginfo
            // 
            this.RB_Catloginfo.AutoSize = true;
            this.RB_Catloginfo.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.RB_Catloginfo.Location = new System.Drawing.Point(286, 3);
            this.RB_Catloginfo.Name = "RB_Catloginfo";
            this.RB_Catloginfo.Size = new System.Drawing.Size(101, 16);
            this.RB_Catloginfo.TabIndex = 6;
            this.RB_Catloginfo.TabStop = true;
            this.RB_Catloginfo.Text = "カタログ情報";
            this.RB_Catloginfo.UseVisualStyleBackColor = true;
            // 
            // RB_priceinfo
            // 
            this.RB_priceinfo.AutoSize = true;
            this.RB_priceinfo.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.RB_priceinfo.Location = new System.Drawing.Point(208, 3);
            this.RB_priceinfo.Name = "RB_priceinfo";
            this.RB_priceinfo.Size = new System.Drawing.Size(75, 16);
            this.RB_priceinfo.TabIndex = 5;
            this.RB_priceinfo.TabStop = true;
            this.RB_priceinfo.Text = "価格情報";
            this.RB_priceinfo.UseVisualStyleBackColor = true;
            // 
            // RB_attributeinfo
            // 
            this.RB_attributeinfo.AutoSize = true;
            this.RB_attributeinfo.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.RB_attributeinfo.Location = new System.Drawing.Point(129, 3);
            this.RB_attributeinfo.Name = "RB_attributeinfo";
            this.RB_attributeinfo.Size = new System.Drawing.Size(75, 16);
            this.RB_attributeinfo.TabIndex = 4;
            this.RB_attributeinfo.TabStop = true;
            this.RB_attributeinfo.Text = "属性情報";
            this.RB_attributeinfo.UseVisualStyleBackColor = true;
            // 
            // RB_all
            // 
            this.RB_all.AutoSize = true;
            this.RB_all.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.RB_all.Location = new System.Drawing.Point(2, 3);
            this.RB_all.Name = "RB_all";
            this.RB_all.Size = new System.Drawing.Size(49, 16);
            this.RB_all.TabIndex = 1;
            this.RB_all.TabStop = true;
            this.RB_all.Text = "全て";
            this.RB_all.UseVisualStyleBackColor = true;
            // 
            // RB_BaseInfo
            // 
            this.RB_BaseInfo.AutoSize = true;
            this.RB_BaseInfo.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.RB_BaseInfo.Location = new System.Drawing.Point(51, 3);
            this.RB_BaseInfo.Name = "RB_BaseInfo";
            this.RB_BaseInfo.Size = new System.Drawing.Size(75, 16);
            this.RB_BaseInfo.TabIndex = 3;
            this.RB_BaseInfo.TabStop = true;
            this.RB_BaseInfo.Text = "基本情報";
            this.RB_BaseInfo.UseVisualStyleBackColor = true;
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
            this.BT_Torikomi.Location = new System.Drawing.Point(1443, 68);
            this.BT_Torikomi.Margin = new System.Windows.Forms.Padding(1);
            this.BT_Torikomi.Name = "BT_Torikomi";
            this.BT_Torikomi.Size = new System.Drawing.Size(100, 28);
            this.BT_Torikomi.TabIndex = 2;
            this.BT_Torikomi.Text = "取込(F12)";
            this.BT_Torikomi.UseVisualStyleBackColor = false;
            this.BT_Torikomi.Click += new System.EventHandler(this.BT_Torikomi_Click);
            // 
            // GV_SKU
            // 
            this.GV_SKU.AllowUserToAddRows = false;
            this.GV_SKU.AllowUserToDeleteRows = false;
            this.GV_SKU.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.GV_SKU.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.GV_SKU.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GV_SKU.CheckCol = null;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GV_SKU.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.GV_SKU.ColumnHeadersHeight = 25;
            this.GV_SKU.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSKU,
            this.colJan,
            this.colskuname,
            this.colColr,
            this.Column1,
            this.Column2,
            this.Column3,
            this.colError});
            this.GV_SKU.EnableHeadersVisualStyles = false;
            this.GV_SKU.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.GV_SKU.Location = new System.Drawing.Point(33, 106);
            this.GV_SKU.Name = "GV_SKU";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GV_SKU.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.GV_SKU.RowHeight_ = 20;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.GV_SKU.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.GV_SKU.RowTemplate.Height = 20;
            this.GV_SKU.Size = new System.Drawing.Size(1510, 650);
            this.GV_SKU.TabIndex = 108;
            this.GV_SKU.UseRowNo = true;
            this.GV_SKU.UseSetting = false;
            this.GV_SKU.Paint += new System.Windows.Forms.PaintEventHandler(this.GV_SKU_Paint);
            // 
            // colSKU
            // 
            this.colSKU.DataPropertyName = "SKUCD";
            this.colSKU.HeaderText = "SKUCD";
            this.colSKU.Name = "colSKU";
            this.colSKU.Width = 220;
            // 
            // colJan
            // 
            this.colJan.DataPropertyName = "JANCD";
            this.colJan.HeaderText = "JANCD";
            this.colJan.MaxInputLength = 13;
            this.colJan.Name = "colJan";
            this.colJan.Width = 120;
            // 
            // colskuname
            // 
            this.colskuname.DataPropertyName = "商品名";
            this.colskuname.HeaderText = "商品名";
            this.colskuname.Name = "colskuname";
            this.colskuname.Width = 430;
            // 
            // colColr
            // 
            this.colColr.DataPropertyName = "カラー";
            this.colColr.HeaderText = "カラー";
            this.colColr.Name = "colColr";
            this.colColr.Width = 130;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "セイズ";
            this.Column1.HeaderText = "セイズ";
            this.Column1.Name = "Column1";
            this.Column1.Width = 130;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "メーカー商品CD";
            this.Column2.HeaderText = "メーカー商品CD";
            this.Column2.Name = "Column2";
            this.Column2.Width = 170;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "EItem";
            this.Column3.HeaderText = "エラー";
            this.Column3.Name = "Column3";
            // 
            // colError
            // 
            this.colError.DataPropertyName = "Error";
            this.colError.HeaderText = "";
            this.colError.Name = "colError";
            this.colError.Width = 150;
            // 
            // TB_FileName
            // 
            this.TB_FileName.AllowMinus = false;
            this.TB_FileName.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.TB_FileName.BackColor = System.Drawing.Color.White;
            this.TB_FileName.BorderColor = false;
            this.TB_FileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_FileName.ClientColor = System.Drawing.SystemColors.Window;
            this.TB_FileName.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.TB_FileName.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.TB_FileName.DecimalPlace = 0;
            this.TB_FileName.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.TB_FileName.IntegerPart = 0;
            this.TB_FileName.IsCorrectDate = true;
            this.TB_FileName.isEnterKeyDown = false;
            this.TB_FileName.IsFirstTime = true;
            this.TB_FileName.isMaxLengthErr = false;
            this.TB_FileName.IsNumber = true;
            this.TB_FileName.IsShop = false;
            this.TB_FileName.IsTimemmss = false;
            this.TB_FileName.Length = 32767;
            this.TB_FileName.Location = new System.Drawing.Point(117, 75);
            this.TB_FileName.MoveNext = true;
            this.TB_FileName.Name = "TB_FileName";
            this.TB_FileName.Size = new System.Drawing.Size(480, 19);
            this.TB_FileName.TabIndex = 1;
            this.TB_FileName.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            this.TB_FileName.UseColorSizMode = false;
            // 
            // BT_FileName
            // 
            this.BT_FileName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BT_FileName.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BT_FileName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BT_FileName.DefaultBtnSize = false;
            this.BT_FileName.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BT_FileName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BT_FileName.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.BT_FileName.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BT_FileName.Location = new System.Drawing.Point(597, 75);
            this.BT_FileName.Margin = new System.Windows.Forms.Padding(1);
            this.BT_FileName.Name = "BT_FileName";
            this.BT_FileName.Size = new System.Drawing.Size(35, 19);
            this.BT_FileName.TabIndex = 110;
            this.BT_FileName.Text = "▼";
            this.BT_FileName.UseVisualStyleBackColor = false;
            this.BT_FileName.Click += new System.EventHandler(this.BT_FileName_Click);
            // 
            // MasterTorikomi_SKU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1684, 798);
            this.Controls.Add(this.BT_FileName);
            this.Controls.Add(this.TB_FileName);
            this.Controls.Add(this.GV_SKU);
            this.Controls.Add(this.BT_Torikomi);
            this.Controls.Add(this.ckM_Label2);
            this.Controls.Add(this.ckM_Label1);
            this.Controls.Add(this.panel3);
            this.F10Visible = false;
            this.F11Visible = false;
            this.F2Visible = false;
            this.F3Visible = false;
            this.F4Visible = false;
            this.F5Visible = false;
            this.F7Visible = false;
            this.F8Visible = false;
            this.F9Visible = false;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "MasterTorikomi_SKU";
            this.PanelHeaderHeight = 45;
            this.Text = "MasterTorikomi_SKU";
            this.Load += new System.EventHandler(this.MasterTorikomi_SKU_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MasterTorikomi_SKU_KeyUp);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.ckM_Label1, 0);
            this.Controls.SetChildIndex(this.ckM_Label2, 0);
            this.Controls.SetChildIndex(this.BT_Torikomi, 0);
            this.Controls.SetChildIndex(this.GV_SKU, 0);
            this.Controls.SetChildIndex(this.TB_FileName, 0);
            this.Controls.SetChildIndex(this.BT_FileName, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GV_SKU)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Label ckM_Label1;
        private System.Windows.Forms.Panel panel3;
        private CKM_Controls.CKM_RadioButton RB_tagInfo;
        private CKM_Controls.CKM_RadioButton RB_SizeURL;
        private CKM_Controls.CKM_RadioButton RB_JanCD;
        private CKM_Controls.CKM_RadioButton RB_Catloginfo;
        private CKM_Controls.CKM_RadioButton RB_priceinfo;
        private CKM_Controls.CKM_RadioButton RB_attributeinfo;
        private CKM_Controls.CKM_RadioButton RB_all;
        private CKM_Controls.CKM_RadioButton RB_BaseInfo;
        private CKM_Controls.CKM_Button BT_Torikomi;
        private CKM_Controls.CKM_GridView GV_SKU;
        private CKM_Controls.CKM_TextBox TB_FileName;
        private CKM_Controls.CKM_Button BT_FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKU;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colskuname;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColr;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colError;
    }
}