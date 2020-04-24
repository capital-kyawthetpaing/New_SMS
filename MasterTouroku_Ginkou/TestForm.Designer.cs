namespace MasterTouroku_Ginkou
{
    partial class TestForm
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
            this.ckM_TextBox2 = new CKM_Controls.CKM_TextBox();
            this.ckM_Button1 = new CKM_Controls.CKM_Button();
            this.ckM_Button2 = new CKM_Controls.CKM_Button();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Size = new System.Drawing.Size(1734, 91);
            // 
            // PanelSearch
            // 
            this.PanelSearch.Location = new System.Drawing.Point(1200, 0);
            // 
            // ckM_TextBox2
            // 
            this.ckM_TextBox2.AllowMinus = false;
            this.ckM_TextBox2.Back_Color = CKM_Controls.CKM_TextBox.CKM_Color.White;
            this.ckM_TextBox2.BackColor = System.Drawing.Color.White;
            this.ckM_TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ckM_TextBox2.ClientColor = System.Drawing.Color.White;
            this.ckM_TextBox2.Ctrl_Byte = CKM_Controls.CKM_TextBox.Bytes.半角;
            this.ckM_TextBox2.Ctrl_Type = CKM_Controls.CKM_TextBox.Type.Normal;
            this.ckM_TextBox2.DecimalPlace = 0;
            this.ckM_TextBox2.Font = new System.Drawing.Font("MS Gothic", 9F);
            this.ckM_TextBox2.IntegerPart = 0;
            this.ckM_TextBox2.IsCorrectDate = true;
            this.ckM_TextBox2.isEnterKeyDown = false;
            this.ckM_TextBox2.IsNumber = true;
            this.ckM_TextBox2.IsShop = false;
            this.ckM_TextBox2.Length = 10;
            this.ckM_TextBox2.Location = new System.Drawing.Point(602, 257);
            this.ckM_TextBox2.MaxLength = 10;
            this.ckM_TextBox2.MoveNext = true;
            this.ckM_TextBox2.Name = "ckM_TextBox2";
            this.ckM_TextBox2.Size = new System.Drawing.Size(100, 19);
            this.ckM_TextBox2.TabIndex = 100;
            this.ckM_TextBox2.TextSize = CKM_Controls.CKM_TextBox.FontSize.Normal;
            // 
            // ckM_Button1
            // 
            this.ckM_Button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.ckM_Button1.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.ckM_Button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckM_Button1.DefaultBtnSize = false;
            this.ckM_Button1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.ckM_Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckM_Button1.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Button1.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.ckM_Button1.Location = new System.Drawing.Point(799, 253);
            this.ckM_Button1.Margin = new System.Windows.Forms.Padding(1);
            this.ckM_Button1.Name = "ckM_Button1";
            this.ckM_Button1.Size = new System.Drawing.Size(75, 23);
            this.ckM_Button1.TabIndex = 101;
            this.ckM_Button1.Text = "Enabled";
            this.ckM_Button1.UseVisualStyleBackColor = false;
            this.ckM_Button1.Click += new System.EventHandler(this.ckM_Button1_Click);
            // 
            // ckM_Button2
            // 
            this.ckM_Button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.ckM_Button2.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Default;
            this.ckM_Button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckM_Button2.DefaultBtnSize = false;
            this.ckM_Button2.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.ckM_Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckM_Button2.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.ckM_Button2.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Normal;
            this.ckM_Button2.Location = new System.Drawing.Point(943, 254);
            this.ckM_Button2.Margin = new System.Windows.Forms.Padding(1);
            this.ckM_Button2.Name = "ckM_Button2";
            this.ckM_Button2.Size = new System.Drawing.Size(75, 23);
            this.ckM_Button2.TabIndex = 101;
            this.ckM_Button2.Text = "Disabled";
            this.ckM_Button2.UseVisualStyleBackColor = false;
            this.ckM_Button2.Click += new System.EventHandler(this.ckM_Button2_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1736, 677);
            this.Controls.Add(this.ckM_Button2);
            this.Controls.Add(this.ckM_Button1);
            this.Controls.Add(this.ckM_TextBox2);
            this.Location = new System.Drawing.Point(0, 0);
            this.ModeVisible = true;
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.Controls.SetChildIndex(this.ckM_TextBox2, 0);
            this.Controls.SetChildIndex(this.ckM_Button1, 0);
            this.Controls.SetChildIndex(this.ckM_Button2, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CKM_Controls.CKM_TextBox ckM_TextBox1;
        private CKM_Controls.CKM_TextBox ckM_TextBox2;
        private CKM_Controls.CKM_Button ckM_Button1;
        private CKM_Controls.CKM_Button ckM_Button2;
    }
}