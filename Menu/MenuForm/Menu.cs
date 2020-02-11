using System;
using System.Windows.Forms;
using Entity;
using BL;

namespace Menu.MenuForm
{
    public partial class FrmMenu : Form
    {
        M_Staff_Entity mse;//Staff Information

        /// <summary>
        /// Constructor
        /// need to save staff's information and send to program
        /// </summary>
        /// <param name="mse"></param>
        public FrmMenu(M_Staff_Entity mse)
        {
            InitializeComponent();
            this.mse = mse;
        }

        /// <summary>
        /// load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMenu_Load(object sender, EventArgs e)
        {
            lblOperatorName.Text = mse.StaffName;
            lblLoginDate.Text = DateTime.Now.Date.ToString("yyyy/MM/dd");
        }

        /// <summary>
        /// handle all program button click
        /// need button name specific(btn + programID)
        /// all program button need to call this function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            mse.ProgramID = btn.Name.Replace("btn", string.Empty);
            OpenForm(btn,mse.ProgramID);
        }

        /// <summary>
        /// to check form is already open
        /// if already open show this form
        /// else show new form
        /// </summary>
        /// <param name="programID"></param>
        private void OpenForm(Button btn,string programID)
        {
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
            string cmdLine = " " + mse.CompanyCD + " " + mse.StaffCD + " " + Login_BL.GetHostName() ;

            btn.Tag = System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");


            //mse.ProgramID = programID;

            //FormCollection frms = Application.OpenForms;
            //Form openForm = null;
            //foreach (Form frm in frms)
            //{
            //    if (frm.Text.Equals(programID))
            //    {
            //        openForm = frm;
            //    }
            //}

            //if (openForm != null)
            //{
            //    openForm.Show();
            //    openForm.WindowState = FormWindowState.Normal;
            //}
            //else
            //{
            //    switch (programID)
            //    {
            //        case "MasterTouroku_Souko": FrmMasterTouroku_Souko frmmts = new FrmMasterTouroku_Souko(mse);
            //            break;
            //    }
            //}         
        }

        /// <summary>
        /// Confirm message on Menu FormClosing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Base_BL bbl = new Base_BL();
                if (bbl.ShowMessage("Q003") == DialogResult.Yes)
                    e.Cancel = false;
                else
                    e.Cancel = true;
            }
        }
        
    }
}
