using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entity;
using BL;
using CKM_Controls;
namespace MainMenu
{

    public partial class Store_Message : Form
    {
        string SCD = "";
        Menu_BL mbl;
        public Store_Message(M_Staff_Entity mse)
        {
            InitializeComponent();
           
            SCD = mse.StaffCD;
            mbl = new Menu_BL();

            var dt = mbl.D_MenuMessageSelect(SCD);
            if (dt.Rows.Count > 0)
            {
                txt_message.Text = dt.Rows[0]["Message"].ToString() + " ";
           
            }
            else
                txt_message.Text = "";
        }

        private void Store_Message_Load(object sender, EventArgs e)
        {
            //txt_message.SelectionStart = 0;
            //txt_message.Select(txt_message.Text.Length, txt_message.Length);
            //txt_message.Focus(); ;
            SendKeys.Send("{TAB}");
            btn_Proj4.Focus();
        }
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        private void btn_Proj4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_message_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_message_MouseMove(object sender, MouseEventArgs e)
        {
           
        }
    }
}
