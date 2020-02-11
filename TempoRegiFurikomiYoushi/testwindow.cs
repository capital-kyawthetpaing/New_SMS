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
using Entity;
using Base.Client;
using CKM_Controls;
using System.Data.SqlClient;

namespace TempoRegiFurikomiYoushi
{
    public partial class testwindow :    ShopBaseForm
    {
        DataTable dtreporttemp = new DataTable();
        protected Base_BL bbl = new Base_BL();
        TempoRegiFurikomiYoushi_BL tbl;
        public testwindow()
        {
            InitializeComponent();
            tbl = new TempoRegiFurikomiYoushi_BL();

        }

        private void testwindow_Load(object sender, EventArgs e)
        {
            InProgramID = "TempoRegiFurikomiYoushi";
            string data = InOperatorCD;
            StartProgram();
            DataTable dt = new DataTable();
            dt.Columns.Add("1");
            dt.Columns.Add("2");
            dt.Columns.Add("3");
            dt.Columns.Add("4");
            dt.Columns.Add("5");

            dt.Rows.Add(new object[] { "1", "2", "3", "4", "4" });
            dt.Rows.Add(new object[] { "1", "2", "3", "4", "4" });
            dt.Rows.Add(new object[] { "1", "2", "3", "4", "4" });
            dt.Rows.Add(new object[] { "1", "2", "3", "4", "4" });
            dt.Rows.Add(new object[] { "1", "2", "3", "4", "4" });
            dt.Rows.Add(new object[] { "1", "2", "3", "4", "4" });
            ckM_GridView1.DataSource = dt;
           // ckM_GridView1.EnabledColumn("*");
            ckM_GridView1.DisabledColumn("1,2");
        }

        private void ckmShop_GridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ckM_Button1_Click(object sender, EventArgs e)
        {
             if ( ckM_GridView1.Enabled)
            {
                ckM_GridView1.Enabled = false;
                ckM_GridView1.ReadOnly= true;
            }
            else
            {
                ckM_GridView1.ReadOnly = false;
                ckM_GridView1.Enabled = true;
            }
        }
    }
}
