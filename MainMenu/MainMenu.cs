using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CKM_Controls;

namespace MainMenu
{
    public partial class frmMainMenu : Form
    {
        CKM_Button btnleftcurrent;
        CKM_Button btnrightcurrent;
        string btnText = string.Empty;
        private void setDesignerFunction()
        {
            this.KeyPreview = true;
            btnGym1.Click += panelLeft_Click;
            btnGym2.Click += panelLeft_Click;
            btnGym3.Click += panelLeft_Click;
            btnGym4.Click += panelLeft_Click;
            btnGym5.Click += panelLeft_Click;
            btnGym6.Click += panelLeft_Click;
            btnGym7.Click += panelLeft_Click;
            btnGym8.Click += panelLeft_Click;
            btnGym9.Click += panelLeft_Click;
            btnGym10.Click += panelLeft_Click;
            btnGym11.Click += panelLeft_Click;
            btnGym12.Click += panelLeft_Click;
            btnGym13.Click += panelLeft_Click;
            btnGym14.Click += panelLeft_Click;
            btnGym15.Click += panelLeft_Click;
            btnGym16.Click += panelLeft_Click;
            btnGym17.Click += panelLeft_Click;
            btnGym18.Click += panelLeft_Click;
            btnGym19.Click += panelLeft_Click;
            btnGym20.Click += panelLeft_Click;

            btnGym1.MouseEnter += panelLeft_MouseEnter;
            btnGym2.MouseEnter += panelLeft_MouseEnter;
            btnGym3.MouseEnter += panelLeft_MouseEnter;
            btnGym4.MouseEnter += panelLeft_MouseEnter;
            btnGym5.MouseEnter += panelLeft_MouseEnter;
            btnGym6.MouseEnter += panelLeft_MouseEnter;
            btnGym7.MouseEnter += panelLeft_MouseEnter;
            btnGym8.MouseEnter += panelLeft_MouseEnter;
            btnGym9.MouseEnter += panelLeft_MouseEnter;
            btnGym10.MouseEnter += panelLeft_MouseEnter;
            btnGym11.MouseEnter += panelLeft_MouseEnter;
            btnGym12.MouseEnter += panelLeft_MouseEnter;
            btnGym13.MouseEnter += panelLeft_MouseEnter;
            btnGym14.MouseEnter += panelLeft_MouseEnter;
            btnGym15.MouseEnter += panelLeft_MouseEnter;
            btnGym16.MouseEnter += panelLeft_MouseEnter;
            btnGym17.MouseEnter += panelLeft_MouseEnter;
            btnGym18.MouseEnter += panelLeft_MouseEnter;
            btnGym19.MouseEnter += panelLeft_MouseEnter;
            btnGym20.MouseEnter += panelLeft_MouseEnter;

            btnGym1.MouseLeave += panelLeft_MouseLeave;
            btnGym2.MouseLeave += panelLeft_MouseLeave;
            btnGym3.MouseLeave += panelLeft_MouseLeave;
            btnGym4.MouseLeave += panelLeft_MouseLeave;
            btnGym5.MouseLeave += panelLeft_MouseLeave;
            btnGym6.MouseLeave += panelLeft_MouseLeave;
            btnGym7.MouseLeave += panelLeft_MouseLeave;
            btnGym8.MouseLeave += panelLeft_MouseLeave;
            btnGym9.MouseLeave += panelLeft_MouseLeave;
            btnGym10.MouseLeave += panelLeft_MouseLeave;
            btnGym11.MouseLeave += panelLeft_MouseLeave;
            btnGym12.MouseLeave += panelLeft_MouseLeave;
            btnGym13.MouseLeave += panelLeft_MouseLeave;
            btnGym14.MouseLeave += panelLeft_MouseLeave;
            btnGym15.MouseLeave += panelLeft_MouseLeave;
            btnGym16.MouseLeave += panelLeft_MouseLeave;
            btnGym17.MouseLeave += panelLeft_MouseLeave;
            btnGym18.MouseLeave += panelLeft_MouseLeave;
            btnGym19.MouseLeave += panelLeft_MouseLeave;
            btnGym20.MouseLeave += panelLeft_MouseLeave;

            btnProj1.MouseEnter += panelRight_MouseEnter;
            btnProj2.MouseEnter += panelRight_MouseEnter;
            btnProj3.MouseEnter += panelRight_MouseEnter;
            btnProj4.MouseEnter += panelRight_MouseEnter;
            btnProj5.MouseEnter += panelRight_MouseEnter;
            btnProj6.MouseEnter += panelRight_MouseEnter;
            btnProj7.MouseEnter += panelRight_MouseEnter;
            btnProj8.MouseEnter += panelRight_MouseEnter;
            btnProj9.MouseEnter += panelRight_MouseEnter;
            btnProj10.MouseEnter += panelRight_MouseEnter;
            btnProj11.MouseEnter += panelRight_MouseEnter;
            btnProj12.MouseEnter += panelRight_MouseEnter;
            btnProj13.MouseEnter += panelRight_MouseEnter;
            btnProj14.MouseEnter += panelRight_MouseEnter;
            btnProj15.MouseEnter += panelRight_MouseEnter;
            btnProj16.MouseEnter += panelRight_MouseEnter;
            btnProj17.MouseEnter += panelRight_MouseEnter;
            btnProj18.MouseEnter += panelRight_MouseEnter;
            btnProj19.MouseEnter += panelRight_MouseEnter;
            btnProj20.MouseEnter += panelRight_MouseEnter;
            btnProj21.MouseEnter += panelRight_MouseEnter;
            btnProj22.MouseEnter += panelRight_MouseEnter;
            btnProj23.MouseEnter += panelRight_MouseEnter;
            btnProj24.MouseEnter += panelRight_MouseEnter;
            btnProj25.MouseEnter += panelRight_MouseEnter;
            btnProj26.MouseEnter += panelRight_MouseEnter;
            btnProj27.MouseEnter += panelRight_MouseEnter;
            btnProj28.MouseEnter += panelRight_MouseEnter;
            btnProj29.MouseEnter += panelRight_MouseEnter;
            btnProj30.MouseEnter += panelRight_MouseEnter;

            btnProj1.MouseLeave += panelRight_MouseLeave;
            btnProj2.MouseLeave += panelRight_MouseLeave;
            btnProj3.MouseLeave += panelRight_MouseLeave;
            btnProj4.MouseLeave += panelRight_MouseLeave;
            btnProj5.MouseLeave += panelRight_MouseLeave;
            btnProj6.MouseLeave += panelRight_MouseLeave;
            btnProj7.MouseLeave += panelRight_MouseLeave;
            btnProj8.MouseLeave += panelRight_MouseLeave;
            btnProj9.MouseLeave += panelRight_MouseLeave;
            btnProj10.MouseLeave += panelRight_MouseLeave;
            btnProj11.MouseLeave += panelRight_MouseLeave;
            btnProj12.MouseLeave += panelRight_MouseLeave;
            btnProj13.MouseLeave += panelRight_MouseLeave;
            btnProj14.MouseLeave += panelRight_MouseLeave;
            btnProj15.MouseLeave += panelRight_MouseLeave;
            btnProj16.MouseLeave += panelRight_MouseLeave;
            btnProj17.MouseLeave += panelRight_MouseLeave;
            btnProj18.MouseLeave += panelRight_MouseLeave;
            btnProj19.MouseLeave += panelRight_MouseLeave;
            btnProj20.MouseLeave += panelRight_MouseLeave;
            btnProj21.MouseLeave += panelRight_MouseLeave;
            btnProj22.MouseLeave += panelRight_MouseLeave;
            btnProj23.MouseLeave += panelRight_MouseLeave;
            btnProj24.MouseLeave += panelRight_MouseLeave;
            btnProj25.MouseLeave += panelRight_MouseLeave;
            btnProj26.MouseLeave += panelRight_MouseLeave;
            btnProj27.MouseLeave += panelRight_MouseLeave;
            btnProj28.MouseLeave += panelRight_MouseLeave;
            btnProj29.MouseLeave += panelRight_MouseLeave;
            btnProj30.MouseLeave += panelRight_MouseLeave;

            btnProj1.Click += panelRight_Click;
            btnProj2.Click += panelRight_Click;
            btnProj3.Click += panelRight_Click;
            btnProj4.Click += panelRight_Click;
            btnProj5.Click += panelRight_Click;
            btnProj6.Click += panelRight_Click;
            btnProj7.Click += panelRight_Click;
            btnProj8.Click += panelRight_Click;
            btnProj9.Click += panelRight_Click;
            btnProj10.Click += panelRight_Click;
            btnProj11.Click += panelRight_Click;
            btnProj12.Click += panelRight_Click;
            btnProj13.Click += panelRight_Click;
            btnProj14.Click += panelRight_Click;
            btnProj15.Click += panelRight_Click;
            btnProj16.Click += panelRight_Click;
            btnProj17.Click += panelRight_Click;
            btnProj18.Click += panelRight_Click;
            btnProj19.Click += panelRight_Click;
            btnProj20.Click += panelRight_Click;
            btnProj21.Click += panelRight_Click;
            btnProj22.Click += panelRight_Click;
            btnProj23.Click += panelRight_Click;
            btnProj24.Click += panelRight_Click;
            btnProj25.Click += panelRight_Click;
            btnProj26.Click += panelRight_Click;
            btnProj27.Click += panelRight_Click;
            btnProj28.Click += panelRight_Click;
            btnProj29.Click += panelRight_Click;
            btnProj30.Click += panelRight_Click;

            btnGym1.Text = "マスタ";
            btnGym2.Text = "店舗";
            btnGym3.Text = "";
            btnGym4.Text = "";
            btnGym5.Text = "";
            btnGym6.Text = "";
            btnGym7.Text = "";
            btnGym8.Text = "";
            btnGym9.Text = "";
            btnGym10.Text = "";
            btnGym11.Text = "";
            btnGym12.Text = "";
            btnGym13.Text = "";
            btnGym14.Text = "";
            btnGym15.Text = "";
            btnGym16.Text = "";
            btnGym17.Text = "";
            btnGym18.Text = "";
            btnGym19.Text = "";
            btnGym20.Text = "";
            
        }

        private void panelLeft_MouseEnter(object sender, EventArgs e)
        {
            // when we hoover the button we get this
            (sender as CKM_Button).BackColor = Color.LightGreen;
        }

        private void panelLeft_MouseLeave(object sender, EventArgs e)
        {
            // when we again leave the button we get back original color
            if ((sender) as CKM_Button != btnleftcurrent)
                (sender as CKM_Button).BackColor = Color.FromArgb(192, 255, 192);
        }

        private void panelRight_MouseEnter(object sender, EventArgs e)
        {
            // when we hoover the button we get this
            (sender as CKM_Button).BackColor = Color.Khaki;
        }

        private void panelRight_MouseLeave(object sender, EventArgs e)
        {
            // when we again leave the button we get back original color
            if ((sender) as CKM_Button != btnrightcurrent)
                (sender as CKM_Button).BackColor = Color.FromArgb(255, 224, 192);
        }

        private void panelLeft_Click(object sender, EventArgs e)
        {
            CKM_Button btn = sender as CKM_Button;
            btnText = btn.Text;
            if (!string.IsNullOrWhiteSpace(btnText))
            {
                RightButton_Text(btnText);
            }
        }

        private void panelRight_Click(object sender, EventArgs e)
        {
            CKM_Button btn = sender as CKM_Button;
            btnText = btn.Text;
            if (!string.IsNullOrWhiteSpace(btnText))
            {
                ButtonClick(btnText);
            }
        }
        public frmMainMenu()
        {
            InitializeComponent();
            setDesignerFunction();
        }

        private void RightButton_Text(string Text)
        {
            switch (Text)
            {
                case "マスタ":
                    btnProj1.Text = "Renben";
                    btnProj2.Text = "Hanyou";
                    btnProj3.Text = string.Empty;
                    btnProj4.Text = string.Empty;
                    btnProj5.Text = string.Empty;
                    btnProj6.Text = string.Empty;
                    btnProj7.Text = string.Empty;
                    btnProj8.Text = string.Empty;
                    btnProj9.Text = string.Empty;
                    btnProj10.Text = string.Empty;
                    btnProj11.Text = string.Empty;
                    btnProj12.Text = string.Empty;
                    btnProj13.Text = string.Empty;
                    btnProj14.Text = string.Empty;
                    btnProj15.Text = string.Empty;
                    btnProj16.Text = string.Empty;
                    btnProj17.Text = string.Empty;
                    btnProj18.Text = string.Empty;
                    btnProj19.Text = string.Empty;
                    btnProj20.Text = string.Empty;
                    btnProj21.Text = string.Empty;
                    btnProj22.Text = string.Empty;
                    btnProj23.Text = string.Empty;
                    btnProj24.Text = string.Empty;
                    btnProj25.Text = string.Empty;
                    btnProj26.Text = string.Empty;
                    btnProj27.Text = string.Empty;
                    btnProj28.Text = string.Empty;
                    btnProj29.Text = string.Empty;
                    btnProj30.Text = string.Empty;
                    break;
                case "店舗":
                    btnProj1.Text = string.Empty;
                    btnProj2.Text = "Shiharai";
                    btnProj3.Text = string.Empty;
                    btnProj4.Text = string.Empty;
                    btnProj5.Text = string.Empty;
                    btnProj6.Text = string.Empty;
                    btnProj7.Text = string.Empty;
                    btnProj8.Text = string.Empty;
                    btnProj9.Text = string.Empty;
                    btnProj10.Text = string.Empty;
                    btnProj11.Text = string.Empty;
                    btnProj12.Text = string.Empty;
                    btnProj13.Text = string.Empty;
                    btnProj14.Text = string.Empty;
                    btnProj15.Text = string.Empty;
                    btnProj16.Text = string.Empty;
                    btnProj17.Text = string.Empty;
                    btnProj18.Text = string.Empty;
                    btnProj19.Text = string.Empty;
                    btnProj20.Text = string.Empty;
                    btnProj21.Text = string.Empty;
                    btnProj22.Text = string.Empty;
                    btnProj23.Text = string.Empty;
                    btnProj24.Text = string.Empty;
                    btnProj25.Text = string.Empty;
                    btnProj26.Text = string.Empty;
                    btnProj27.Text = string.Empty;
                    btnProj28.Text = string.Empty;
                    btnProj29.Text = string.Empty;
                    btnProj30.Text = string.Empty;
                    break;
            }

        }
        private void ButtonClick(string Text)
        {
            Form frm;
            switch(Text)
            {
                //case "Hanyou":
                //    frm = GetForm("")
                //    break;
            }
        }
        public Form GetForm(string formText)
        {
            FormCollection frms = Application.OpenForms;
            foreach (Form frm in frms)
            {
                if (frm.Text.Equals(formText))
                {
                    return frm;
                }
            }

            return null;
        }
        //public IEnumerable<Control> GetAllControls(Control root)
        //{
        //    foreach (Control control in root.Controls)
        //    {
        //        foreach (Control child in GetAllControls(control))
        //        {
        //            yield return child;
        //        }
        //    }
        //    yield return root;
        //}
    }
}
