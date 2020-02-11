using CrystalDecisions.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PCAP0260P
{
    public partial class Viewer : Form
    {
        public bool printFlg = false;
        public Viewer()
        {
            InitializeComponent();
            
        }

        public CrystalReportViewer CrystalReportViewer1
        {
            get { return this.crystalReportViewer1; }
        }
        //public virtual void PrintReport()
        //{
        //    ((PCAP0260P_Report)CrystalReportViewer1.ReportSource).PrintToPrinter(0, false, 0, 0);
        //    printFlg = true;
        //}
    }
}
