using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using CKM_Controls;
namespace MainMenu.Haspo
{
    public partial class HapoStore_MainMenu : Form
    {
        string Staff_CD = "";
        Menu_BL mbl;
        public HapoStore_MainMenu(string SCD)
        {
            mbl = new Menu_BL();
            Staff_CD = SCD;
            InitializeComponent();
        }
        
       

        private void HapoStore_MainMenu_Load(object sender, EventArgs e)
        {
            BindButtonName();
        }
        private void BindButtonName()
        {
            
            var menu = mbl.getMenuNo(Staff_CD);
            //var _result = menu.AsEnumerable().GroupBy(x => x.Field<string>("Char1")).Select(g => g.First()).CopyToDataTable();
          
            //ButtonText(panel_left, _result, 1);

        }
        //protected void ButtonText(Panel p, DataTable k0, int Gym)
        //{
        //    IOrderedEnumerable<DataRow> result;
        //    result = k0.Select().OrderBy(row => row["ProgramSEQ"]);
        //    var k = result.CopyToDataTable();
        //    System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();

        //    for (int j = 0; j < k.Rows.Count; j++)
        //    {
        //        var c = GetAllControls(p);
        //        for (int i = 0; i < c.Count(); i++)
        //        {
        //            Control ctrl = c.ElementAt(i) as Control;

        //            if (ctrl is CKM_Button)
        //            {

        //                ToolTip1.SetToolTip(((CKM_Button)ctrl), null);
        //                if (Gym == 1 && k.Rows[j]["Char1"].ToString() != string.Empty && k.Rows[j]["BusinessSEQ"].ToString() != string.Empty)
        //                {
        //                    if (((CKM_Button)ctrl).Name == "btnGym" + Convert.ToInt32(k.Rows[j]["BusinessSEQ"].ToString()))
        //                    {
        //                        ((CKM_Button)ctrl).Text = k.Rows[j]["Char1"].ToString();
        //                        ((CKM_Button)ctrl).Enabled = true;
        //                    }
        //                }
        //                else if (Gym == 0 && k.Rows[j]["ProgramID"].ToString() != string.Empty)
        //                {
        //                    if (((CKM_Button)ctrl).Name == "btn_Proj" + Convert.ToInt32(k.Rows[j]["ProgramSEQ"].ToString()))
        //                    {
        //                        ((CKM_Button)ctrl).Text = k.Rows[j]["ProgramID"].ToString();
        //                        ((CKM_Button)ctrl).Enabled = true;
        //                        //  ((CKM_Button)ctrl).Name = mope_data.PROID.ToString();
        //                        // ToolTip1.SetToolTip(((CKM_Button)ctrl),"");
        //                        //ToolTip1.SetToolTip(((CKM_Button)ctrl), ((CKM_Button)ctrl).Text);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        
        public IEnumerable<Control> GetAllControls(Control root)
        {
            foreach (Control control in root.Controls)
            {
                foreach (Control child in GetAllControls(control))
                {
                    yield return child;
                }
            }
            yield return root;
        }

        protected Base_BL bbl = new Base_BL();
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (bbl.ShowMessage("Q003") == DialogResult.Yes)
                this.Close();
            else
            {
                btnClose.Focus();
            }
        }
    }
}
