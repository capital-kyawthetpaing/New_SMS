﻿namespace JANCDHenkou
{
    partial class JANCDHenkou
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JANCDHenkou));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelDetail = new System.Windows.Forms.Panel();
            this.BtnF10Show = new CKM_Controls.CKM_Button();
            this.dgvJANCDHenkou = new CKM_Controls.CKM_GridView();
            this.colGenJanCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBtnJAN = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colBrandCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBrandName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colITEM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenJanCD2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colnewJanCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSKUCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAdminCD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ckM_Label4 = new CKM_Controls.CKM_Label();
            this.ckM_Label3 = new CKM_Controls.CKM_Label();
            this.ckM_Label2 = new CKM_Controls.CKM_Label();
            this.ckM_Label1 = new CKM_Controls.CKM_Label();
            this.panelDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJANCDHenkou)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1711, 14);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(1177, 0);
            // 
            // btnChangeIkkatuHacchuuMode
            // 
            this.btnChangeIkkatuHacchuuMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            // 
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.BtnF10Show);
            this.panelDetail.Controls.Add(this.dgvJANCDHenkou);
            this.panelDetail.Controls.Add(this.ckM_Label4);
            this.panelDetail.Controls.Add(this.ckM_Label3);
            this.panelDetail.Controls.Add(this.ckM_Label2);
            this.panelDetail.Controls.Add(this.ckM_Label1);
            this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetail.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelDetail.Location = new System.Drawing.Point(0, 70);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(1713, 859);
            this.panelDetail.TabIndex = 13;
            // 
            // BtnF10Show
            // 
            this.BtnF10Show.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.BtnF10Show.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.BtnF10Show.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnF10Show.DefaultBtnSize = true;
            this.BtnF10Show.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnF10Show.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnF10Show.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.BtnF10Show.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.BtnF10Show.Location = new System.Drawing.Point(1589, 86);
            this.BtnF10Show.Margin = new System.Windows.Forms.Padding(1);
            this.BtnF10Show.Name = "BtnF10Show";
            this.BtnF10Show.Size = new System.Drawing.Size(118, 28);
            this.BtnF10Show.TabIndex = 5;
            this.BtnF10Show.Text = "取込(F10)";
            this.BtnF10Show.UseVisualStyleBackColor = false;
            this.BtnF10Show.Click += new System.EventHandler(this.BtnF10Show_Click);
            // 
            // dgvJANCDHenkou
            // 
            this.dgvJANCDHenkou.AllowUserToDeleteRows = false;
            this.dgvJANCDHenkou.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(235)))), ((int)(((byte)(247)))));
            this.dgvJANCDHenkou.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvJANCDHenkou.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvJANCDHenkou.CheckCol = ((System.Collections.ArrayList)(resources.GetObject("dgvJANCDHenkou.CheckCol")));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvJANCDHenkou.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvJANCDHenkou.ColumnHeadersHeight = 25;
            this.dgvJANCDHenkou.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGenJanCD,
            this.colBtnJAN,
            this.colBrandCD,
            this.colBrandName,
            this.colITEM,
            this.colSKUName,
            this.colSize,
            this.colColor,
            this.colGenJanCD2,
            this.colnewJanCD,
            this.colSKUCD,
            this.colAdminCD});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("MS Gothic", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvJANCDHenkou.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvJANCDHenkou.EnableHeadersVisualStyles = false;
            this.dgvJANCDHenkou.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.dgvJANCDHenkou.Location = new System.Drawing.Point(18, 119);
            this.dgvJANCDHenkou.Name = "dgvJANCDHenkou";
            this.dgvJANCDHenkou.RowHeight_ = 20;
            this.dgvJANCDHenkou.RowTemplate.Height = 20;
            this.dgvJANCDHenkou.Size = new System.Drawing.Size(1690, 735);
            this.dgvJANCDHenkou.TabIndex = 4;
            this.dgvJANCDHenkou.UseRowNo = true;
            this.dgvJANCDHenkou.UseSetting = true;
            this.dgvJANCDHenkou.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvJANCDHenkou_CellContentClick);
            this.dgvJANCDHenkou.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvJANCDHenkou_CellEndEdit);
            this.dgvJANCDHenkou.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvJANCDHenkou_CellPainting);
            this.dgvJANCDHenkou.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvJANCDHenkou_Paint);
            // 
            // colGenJanCD
            // 
            this.colGenJanCD.DataPropertyName = "GenJanCD";
            this.colGenJanCD.HeaderText = "現JANCD";
            this.colGenJanCD.MaxInputLength = 13;
            this.colGenJanCD.Name = "colGenJanCD";
            // 
            // colBtnJAN
            // 
            this.colBtnJAN.HeaderText = "";
            this.colBtnJAN.Name = "colBtnJAN";
            this.colBtnJAN.Text = "";
            this.colBtnJAN.Width = 30;
            // 
            // colBrandCD
            // 
            this.colBrandCD.DataPropertyName = "BrandCD";
            this.colBrandCD.HeaderText = "ブランド";
            this.colBrandCD.Name = "colBrandCD";
            this.colBrandCD.ReadOnly = true;
            this.colBrandCD.Width = 60;
            // 
            // colBrandName
            // 
            this.colBrandName.DataPropertyName = "BrandName";
            this.colBrandName.HeaderText = "";
            this.colBrandName.Name = "colBrandName";
            this.colBrandName.ReadOnly = true;
            this.colBrandName.Width = 180;
            // 
            // colITEM
            // 
            this.colITEM.DataPropertyName = "ITEMCD";
            this.colITEM.HeaderText = "ITEM";
            this.colITEM.Name = "colITEM";
            this.colITEM.ReadOnly = true;
            this.colITEM.Width = 230;
            // 
            // colSKUName
            // 
            this.colSKUName.DataPropertyName = "SKUName";
            this.colSKUName.HeaderText = "商品名";
            this.colSKUName.Name = "colSKUName";
            this.colSKUName.ReadOnly = true;
            this.colSKUName.Width = 610;
            // 
            // colSize
            // 
            this.colSize.DataPropertyName = "SizeName";
            this.colSize.HeaderText = "サイズ";
            this.colSize.Name = "colSize";
            this.colSize.ReadOnly = true;
            this.colSize.Width = 120;
            // 
            // colColor
            // 
            this.colColor.DataPropertyName = "ColorName";
            this.colColor.HeaderText = "カラー";
            this.colColor.Name = "colColor";
            this.colColor.ReadOnly = true;
            // 
            // colGenJanCD2
            // 
            this.colGenJanCD2.DataPropertyName = "GenJanCD2";
            this.colGenJanCD2.HeaderText = "現JANCD";
            this.colGenJanCD2.Name = "colGenJanCD2";
            this.colGenJanCD2.ReadOnly = true;
            // 
            // colnewJanCD
            // 
            this.colnewJanCD.DataPropertyName = "newJanCD";
            this.colnewJanCD.HeaderText = "新JANCD";
            this.colnewJanCD.Name = "colnewJanCD";
            // 
            // colSKUCD
            // 
            this.colSKUCD.DataPropertyName = "SKUCD";
            this.colSKUCD.HeaderText = "SKUCD";
            this.colSKUCD.Name = "colSKUCD";
            this.colSKUCD.ReadOnly = true;
            this.colSKUCD.Visible = false;
            // 
            // colAdminCD
            // 
            this.colAdminCD.DataPropertyName = "AdminNO";
            this.colAdminCD.HeaderText = "AdminNO";
            this.colAdminCD.Name = "colAdminCD";
            this.colAdminCD.ReadOnly = true;
            this.colAdminCD.Visible = false;
            // 
            // ckM_Label4
            // 
            this.ckM_Label4.AutoSize = true;
            this.ckM_Label4.Back_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.BackColor = System.Drawing.Color.Transparent;
            this.ckM_Label4.DefaultlabelSize = true;
            this.ckM_Label4.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Label4.Font_Size = CKM_Controls.CKM_Label.CKM_FontSize.Normal;
            this.ckM_Label4.ForeColor = System.Drawing.Color.Black;
            this.ckM_Label4.Location = new System.Drawing.Point(27, 89);
            this.ckM_Label4.Name = "ckM_Label4";
            this.ckM_Label4.Size = new System.Drawing.Size(443, 12);
            this.ckM_Label4.TabIndex = 3;
            this.ckM_Label4.Text = "定型フォーマットで用意されたExcelファイルを読み込むことができます。";
            this.ckM_Label4.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label3.Location = new System.Drawing.Point(27, 68);
            this.ckM_Label3.Name = "ckM_Label3";
            this.ckM_Label3.Size = new System.Drawing.Size(859, 12);
            this.ckM_Label3.TabIndex = 2;
            this.ckM_Label3.Text = "この処理はやり直しはできません。（JANCDをＡ⇒Ｂと変更するのを間違えてＡ⇒Ｃと変更した場合は、あらためてＣ⇒Ｂと変更してください。）";
            this.ckM_Label3.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.ckM_Label2.Location = new System.Drawing.Point(27, 47);
            this.ckM_Label2.Name = "ckM_Label2";
            this.ckM_Label2.Size = new System.Drawing.Size(495, 12);
            this.ckM_Label2.TabIndex = 1;
            this.ckM_Label2.Text = "すべての情報を新しいJANCDで洗替するため、処理に時間がかかる場合があります。";
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
            this.ckM_Label1.Location = new System.Drawing.Point(27, 25);
            this.ckM_Label1.Name = "ckM_Label1";
            this.ckM_Label1.Size = new System.Drawing.Size(833, 12);
            this.ckM_Label1.TabIndex = 0;
            this.ckM_Label1.Text = "仮JANCDを含め今、SKUごとに設定されているJANCDのマスター値、実績データ値などシステム内のすべての情報を新しいJANCDで洗替します。";
            this.ckM_Label1.Text_Color = CKM_Controls.CKM_Label.CKM_Color.Default;
            this.ckM_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // JANCDHenkou
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1713, 961);
            this.Controls.Add(this.panelDetail);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "JANCDHenkou";
            this.PanelHeaderHeight = 70;
            this.Text = "JANCDHenkou";
            this.Load += new System.EventHandler(this.JANCDHenkou_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.JANCDHenkou_KeyUp);
            this.Controls.SetChildIndex(this.panelDetail, 0);
            this.panelDetail.ResumeLayout(false);
            this.panelDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJANCDHenkou)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelDetail;
        private CKM_Controls.CKM_GridView dgvJANCDHenkou;
        private CKM_Controls.CKM_Label ckM_Label4;
        private CKM_Controls.CKM_Label ckM_Label3;
        private CKM_Controls.CKM_Label ckM_Label2;
        private CKM_Controls.CKM_Label ckM_Label1;
        private CKM_Controls.CKM_Button BtnF10Show;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenJanCD;
        private System.Windows.Forms.DataGridViewButtonColumn colBtnJAN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBrandCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBrandName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colITEM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenJanCD2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colnewJanCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSKUCD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdminCD;
    }
}

