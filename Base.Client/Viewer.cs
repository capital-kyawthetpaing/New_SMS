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


namespace Base.Client
{
    public partial class Viewer : Form
    {
        public bool printFlg = false;
        public Viewer()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        public CrystalReportViewer CrystalReportViewer1
        {
            
            get { return this.crystalReportViewer1;
            }
            set
            {
                crystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
            }
        }

        private void Viewer_Load(object sender, EventArgs e)
        {

        }
        //public virtual void PrintReport()
        //{
        //    ((TempoNouhinsyo_Report)CrystalReportViewer1.ReportSource).PrintToPrinter(0, false, 0, 0);
        //    printFlg = true;
        //}
    }
}
