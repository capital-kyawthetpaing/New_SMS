using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TempoRegiPoint
{
    public partial class TempoRegiPointPreview : Form
    {
        public TempoRegiPointPreview()
        {
            InitializeComponent();
        }

        public void SetCouponDataSet(Coupon_DataSet couponDataSet)
        {
            var report = new TempoRegiPoint_Coupon();
            report.SetDataSource(couponDataSet);

            crystalReportViewer.ReportSource = report;
        }
    }
}
