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
using DL;

namespace KeihiltiranHyou
{
    
    public partial class keihiPrint : Form
    {
        D_Cost_Entity dce,dce2 ;
        KeihiltiranHyou_BL kbl;
        string[] lst;
        public keihiPrint(D_Cost_Entity dc, params string[] login)
        {
            InitializeComponent();
            lst = login;
            dce = new D_Cost_Entity();
            dce2 = new D_Cost_Entity();
            dce = dc;
            kbl = new KeihiltiranHyou_BL();
            AllEvent_PrintLog();
        }

        private void keihiPrint_Load(object sender, EventArgs e)
        {
            Loadreport();
        }
        //yyyymmdd	mmss	CostNo	StoreName	RecordedDate	PayPlanDate	InputDateTime	CostNo	VendorCD	VendorName	Summary	Char1	CostGaku	TotalGaku

        void Loadreport()
        {
            var  f= kbl.getPrintData(dce); 
           // DataTable dt = null;
            Dataset.keihihanyou rep = new Dataset.keihihanyou();
            rep.SetDataSource(f);
          //  rep.SetParameterValue("txtPageNo","");
            rep.SetParameterValue("txtStoreName",dce.Store);
            rep.SetParameterValue("txtDateTime", "2019/09/09  12:12");
            rep.SetParameterValue("CostGaku", "999,999,999");
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = rep;

        }
        public void AllEvent_PrintLog()
        {

            foreach (ToolStrip ts in crystalReportViewer1.Controls.OfType<ToolStrip>())
            {
                foreach (ToolStripButton tsb in ts.Items.OfType<ToolStripButton>())
                {

                    if (tsb.ToolTipText.ToLower().Contains("print"))
                    {

                        tsb.Click += new EventHandler(printButton_Click);
                    }
                }
            }

        }
        private void printButton_Click(object sender, EventArgs e)
        {

            L_Log_DL ldl = new L_Log_DL();
            //ldl.L_Log_Insert_Print(lst);
        }
    }
}
