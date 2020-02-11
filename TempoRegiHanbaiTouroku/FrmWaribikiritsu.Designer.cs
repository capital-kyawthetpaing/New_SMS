namespace TempoRegiHanbaiTouroku
{
    partial class FrmWaribikiritsu
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
            this.ckmShop_Label7 = new CKM_Controls.CKMShop_Label();
            this.txtRitsu = new CKM_Controls.CKM_TextBox();
            this.ckmShop_Label9 = new CKM_Controls.CKMShop_Label();
            this.ckmShop_Label12 = new CKM_Controls.CKMShop_Label();
            this.lblTeika = new System.Windows.Forms.Label();
            this.lblTanka = new System.Windows.Forms.Label();
            this.ckmShop_Label17 = new CKM_Controls.CKMShop_Label();
            this.lblSKUName = new System.Windows.Forms.Label();
            this.lblJANCD = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new CKM_Controls.CKM_Button();
            this.btnProcess = new CKM_Controls.CKM_Button();
            this.ckmShop_Label1 = new CKM_Controls.CKMShop_Label();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ckmShop_Label7
            // 
            this.ckmShop_Label7.AutoSize = true;
            this.ckmShop_Label7.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label7.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label7.Font = new System.Drawing.Font("ＭＳ ゴシック", 20F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label7.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Medium;
            this.ckmShop_Label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label7.Location = new System.Drawing.Point(97, 238);
            this.ckmShop_Label7.Name = "ckmShop_Label7";
            this.ckmShop_Label7.Size = new System.Drawing.Size(99, 27);
            this.ckmShop_Label7.TabIndex = 28;
            this.ckmShop_Label7.Text = "割引率";
            this.ckmShop_Label7.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRitsu
            // 
            this.txtRitsu.AllowMinus = false;
            this.txtRitsu.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.Green;
            this.txtRitsu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(239)))), ((int)(((byte)(218)))));
            this.txtRitsu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRitsu.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.txtRitsu.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Price;
            this.txtRitsu.DecimalPlace = 1;
            this.txtRitsu.Font = new System.Drawing.Font("ＭＳ ゴシック", 20F);
            this.txtRitsu.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtRitsu.IntegerPart = 3;
            this.txtRitsu.IsCorrectDate = true;
            this.txtRitsu.isEnterKeyDown = false;
            this.txtRitsu.IsNumber = true;
            this.txtRitsu.IsShop = false;
            this.txtRitsu.Length = 5;
            this.txtRitsu.Location = new System.Drawing.Point(208, 236);
            this.txtRitsu.MaxLength = 5;
            this.txtRitsu.MoveNext = true;
            this.txtRitsu.Name = "txtRitsu";
            this.txtRitsu.Size = new System.Drawing.Size(159, 34);
            this.txtRitsu.TabIndex = 0;
            this.txtRitsu.Text = "99.9";
            this.txtRitsu.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRitsu.TextSize = CKM_Controls.CKM_TextBox.FontSize.Large;
            this.txtRitsu.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtShippingSu_KeyDown);
            // 
            // ckmShop_Label9
            // 
            this.ckmShop_Label9.AutoSize = true;
            this.ckmShop_Label9.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label9.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label9.Font = new System.Drawing.Font("ＭＳ ゴシック", 20F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label9.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Medium;
            this.ckmShop_Label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label9.Location = new System.Drawing.Point(97, 182);
            this.ckmShop_Label9.Name = "ckmShop_Label9";
            this.ckmShop_Label9.Size = new System.Drawing.Size(99, 27);
            this.ckmShop_Label9.TabIndex = 32;
            this.ckmShop_Label9.Text = "定　価";
            this.ckmShop_Label9.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckmShop_Label12
            // 
            this.ckmShop_Label12.AutoSize = true;
            this.ckmShop_Label12.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label12.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label12.Font = new System.Drawing.Font("ＭＳ ゴシック", 20F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label12.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Medium;
            this.ckmShop_Label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label12.Location = new System.Drawing.Point(109, 58);
            this.ckmShop_Label12.Name = "ckmShop_Label12";
            this.ckmShop_Label12.Size = new System.Drawing.Size(87, 27);
            this.ckmShop_Label12.TabIndex = 37;
            this.ckmShop_Label12.Text = "JANCD";
            this.ckmShop_Label12.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTeika
            // 
            this.lblTeika.BackColor = System.Drawing.Color.Transparent;
            this.lblTeika.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTeika.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTeika.Location = new System.Drawing.Point(208, 181);
            this.lblTeika.Name = "lblTeika";
            this.lblTeika.Size = new System.Drawing.Size(159, 28);
            this.lblTeika.TabIndex = 58;
            this.lblTeika.Text = "9,999";
            this.lblTeika.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTanka
            // 
            this.lblTanka.BackColor = System.Drawing.Color.Transparent;
            this.lblTanka.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTanka.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTanka.Location = new System.Drawing.Point(208, 294);
            this.lblTanka.Name = "lblTanka";
            this.lblTanka.Size = new System.Drawing.Size(159, 28);
            this.lblTanka.TabIndex = 62;
            this.lblTanka.Text = "9,999";
            this.lblTanka.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ckmShop_Label17
            // 
            this.ckmShop_Label17.AutoSize = true;
            this.ckmShop_Label17.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label17.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label17.Font = new System.Drawing.Font("ＭＳ ゴシック", 20F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label17.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Medium;
            this.ckmShop_Label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label17.Location = new System.Drawing.Point(97, 295);
            this.ckmShop_Label17.Name = "ckmShop_Label17";
            this.ckmShop_Label17.Size = new System.Drawing.Size(99, 27);
            this.ckmShop_Label17.TabIndex = 60;
            this.ckmShop_Label17.Text = "単　価";
            this.ckmShop_Label17.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSKUName
            // 
            this.lblSKUName.BackColor = System.Drawing.Color.Transparent;
            this.lblSKUName.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSKUName.Location = new System.Drawing.Point(208, 96);
            this.lblSKUName.Name = "lblSKUName";
            this.lblSKUName.Size = new System.Drawing.Size(580, 48);
            this.lblSKUName.TabIndex = 70;
            this.lblSKUName.Text = "商品名ＸＸＸＸＸＸ10ＸＸＸＸＸＸＸＸＸ20ＸＸＸＸＸＸＸＸＸ30ＸＸＸＸＸＸＸＸＸ40ＸＸＸＸＸＸＸＸＸ50";
            this.lblSKUName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblJANCD
            // 
            this.lblJANCD.BackColor = System.Drawing.Color.Transparent;
            this.lblJANCD.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblJANCD.Location = new System.Drawing.Point(208, 58);
            this.lblJANCD.Name = "lblJANCD";
            this.lblJANCD.Size = new System.Drawing.Size(515, 27);
            this.lblJANCD.TabIndex = 71;
            this.lblJANCD.Text = "49XXXXXXXXX13";
            this.lblJANCD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tableLayoutPanel1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 362);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(970, 49);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(970, 49);
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
            this.btnClose.Font = new System.Drawing.Font("ＭＳ ゴシック", 24F, System.Drawing.FontStyle.Bold);
            this.btnClose.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.XLarge;
            this.btnClose.Location = new System.Drawing.Point(1, 1);
            this.btnClose.Margin = new System.Windows.Forms.Padding(1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(289, 47);
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
            this.btnProcess.Font = new System.Drawing.Font("ＭＳ ゴシック", 24F, System.Drawing.FontStyle.Bold);
            this.btnProcess.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.XLarge;
            this.btnProcess.Location = new System.Drawing.Point(292, 1);
            this.btnProcess.Margin = new System.Windows.Forms.Padding(1);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(677, 47);
            this.btnProcess.TabIndex = 1;
            this.btnProcess.Tag = "1";
            this.btnProcess.Text = "決　定";
            this.btnProcess.UseVisualStyleBackColor = false;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // ckmShop_Label1
            // 
            this.ckmShop_Label1.AutoSize = true;
            this.ckmShop_Label1.Back_Color = CKM_Controls.CKMShop_Label.CKM_Color.Default;
            this.ckmShop_Label1.BackColor = System.Drawing.Color.Transparent;
            this.ckmShop_Label1.Font = new System.Drawing.Font("ＭＳ ゴシック", 20F, System.Drawing.FontStyle.Bold);
            this.ckmShop_Label1.Font_Size = CKM_Controls.CKMShop_Label.CKM_FontSize.Medium;
            this.ckmShop_Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(130)))), ((int)(((byte)(53)))));
            this.ckmShop_Label1.Location = new System.Drawing.Point(393, 238);
            this.ckmShop_Label1.Name = "ckmShop_Label1";
            this.ckmShop_Label1.Size = new System.Drawing.Size(348, 27);
            this.ckmShop_Label1.TabIndex = 83;
            this.ckmShop_Label1.Text = "20%割引の場合は20と入力";
            this.ckmShop_Label1.Text_Color = CKM_Controls.CKMShop_Label.CKM_Color.Green;
            this.ckmShop_Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmWaribikiritsu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(970, 411);
            this.Controls.Add(this.ckmShop_Label1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.lblSKUName);
            this.Controls.Add(this.lblTanka);
            this.Controls.Add(this.ckmShop_Label17);
            this.Controls.Add(this.lblTeika);
            this.Controls.Add(this.ckmShop_Label12);
            this.Controls.Add(this.txtRitsu);
            this.Controls.Add(this.ckmShop_Label9);
            this.Controls.Add(this.ckmShop_Label7);
            this.Controls.Add(this.lblJANCD);
            this.Name = "FrmWaribikiritsu";
            this.Text = "TempoRegiHanbaiTouroku";
            this.Load += new System.EventHandler(this.Form_Load);
            this.panel4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CKM_Controls.CKMShop_Label ckmShop_Label7;
        private CKM_Controls.CKM_TextBox txtRitsu;
        private CKM_Controls.CKMShop_Label ckmShop_Label9;
        private CKM_Controls.CKMShop_Label ckmShop_Label12;
        private System.Windows.Forms.Label lblTeika;
        private System.Windows.Forms.Label lblTanka;
        private CKM_Controls.CKMShop_Label ckmShop_Label17;
        private System.Windows.Forms.Label lblSKUName;
        private System.Windows.Forms.Label lblJANCD;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        protected CKM_Controls.CKM_Button btnClose;
        protected CKM_Controls.CKM_Button btnProcess;
        private CKM_Controls.CKMShop_Label ckmShop_Label1;
    }
}

