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

namespace TanaoroshiHyou
{
    public partial class Viewer1 : Form
    {
        public Viewer1()
        {
            InitializeComponent();
        }
        public CrystalReportViewer CrystalReportViewer1
        {

            get
            {
                return this.crystalReportViewer1;
            }
            set
            {
                crystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
            }
        }
    }
}
