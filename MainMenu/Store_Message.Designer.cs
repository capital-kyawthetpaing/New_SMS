namespace MainMenu
{
    partial class Store_Message
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Store_Message));
            this.txt_message = new CKM_Controls.CKM_MultiLineTextBox();
            this.btn_Proj4 = new CKM_Controls.CKM_Button();
            this.SuspendLayout();
            // 
            // txt_message
            // 
            this.txt_message.Back_Color = CKM_Controls.CKM_MultiLineTextBox.CKM_Color.White;
            this.txt_message.BackColor = System.Drawing.Color.White;
            this.txt_message.Ctrl_Byte = CKM_Controls.CKM_MultiLineTextBox.Bytes.半全角;
            this.txt_message.F_focus = false;
            this.txt_message.Font = new System.Drawing.Font("MS Gothic", 20F);
            this.txt_message.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txt_message.Length = 500;
            this.txt_message.Location = new System.Drawing.Point(12, 24);
            this.txt_message.MaxLength = 500;
            this.txt_message.Mdea = false;
            this.txt_message.Mfocus = false;
            this.txt_message.MoveNext = false;
            this.txt_message.Multiline = true;
            this.txt_message.Name = "txt_message";
            this.txt_message.RowCount = 10;
            this.txt_message.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_message.Size = new System.Drawing.Size(1163, 543);
            this.txt_message.TabIndex = 0;
            this.txt_message.TextSize = CKM_Controls.CKM_MultiLineTextBox.FontSize.Large;
            this.txt_message.TextChanged += new System.EventHandler(this.txt_message_TextChanged);
            this.txt_message.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txt_message_MouseMove);
            // 
            // btn_Proj4
            // 
            this.btn_Proj4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(242)))), ((int)(((byte)(204)))));
            this.btn_Proj4.BackgroundColor = CKM_Controls.CKM_Button.CKM_Color.Yellow;
            this.btn_Proj4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Proj4.DefaultBtnSize = false;
            this.btn_Proj4.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_Proj4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Proj4.Font = new System.Drawing.Font("MS Gothic", 28F, System.Drawing.FontStyle.Bold);
            this.btn_Proj4.Font_Size = CKM_Controls.CKM_Button.CKM_FontSize.Large;
            this.btn_Proj4.Location = new System.Drawing.Point(12, 586);
            this.btn_Proj4.Margin = new System.Windows.Forms.Padding(1);
            this.btn_Proj4.Name = "btn_Proj4";
            this.btn_Proj4.Size = new System.Drawing.Size(381, 82);
            this.btn_Proj4.TabIndex = 13;
            this.btn_Proj4.Text = "終 了";
            this.btn_Proj4.UseVisualStyleBackColor = false;
            this.btn_Proj4.Click += new System.EventHandler(this.btn_Proj4_Click);
            // 
            // Store_Message
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(224)))), ((int)(((byte)(180)))));
            this.ClientSize = new System.Drawing.Size(1201, 677);
            this.Controls.Add(this.btn_Proj4);
            this.Controls.Add(this.txt_message);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Store_Message";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Store_Message";
            this.Load += new System.EventHandler(this.Store_Message_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CKM_Controls.CKM_MultiLineTextBox txt_message;
        private CKM_Controls.CKM_Button btn_Proj4;
    }
}