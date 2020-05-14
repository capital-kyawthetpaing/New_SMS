namespace TempoRegiFurikomiYoushi
{
    partial class Form1
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
            this.ckmShop_RadioButton1 = new CKM_Controls.CKMShop_RadioButton();
            this.SuspendLayout();
            // 
            // ckmShop_RadioButton1
            // 
            this.ckmShop_RadioButton1.DisplayText = "ckmShop_RadioButton1";
            this.ckmShop_RadioButton1.Font = new System.Drawing.Font("MS Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.ckmShop_RadioButton1.ForeColor = System.Drawing.Color.Black;
            this.ckmShop_RadioButton1.Location = new System.Drawing.Point(507, 247);
            this.ckmShop_RadioButton1.Name = "ckmShop_RadioButton1";
            this.ckmShop_RadioButton1.Size = new System.Drawing.Size(236, 41);
            this.ckmShop_RadioButton1.TabIndex = 0;
            this.ckmShop_RadioButton1.TabStop = true;
            this.ckmShop_RadioButton1.Text = "ckmShop_RadioButton1";
            this.ckmShop_RadioButton1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ckmShop_RadioButton1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1638, 642);
            this.Controls.Add(this.ckmShop_RadioButton1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CKM_Controls.CKMShop_RadioButton ckmShop_RadioButton1;
    }
}