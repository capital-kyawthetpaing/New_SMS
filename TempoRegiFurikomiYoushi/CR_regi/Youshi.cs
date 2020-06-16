using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using DL;
using Entity;
    namespace TempoRegiFurikomiYoushi.CR_regi
{
    public partial class Youshi : Form
    {
        TempoRegiFurikomiYoushi_DL tdl;
        DataTable dtreport;
        Base_DL bdl;
        L_Log_Entity lle;
        string[] lst = new string[] { };
        public Youshi(DataTable dt, params string[] loginfo)
        {

            InitializeComponent();
            lle = new L_Log_Entity();
            lst = loginfo;
            tdl = new TempoRegiFurikomiYoushi_DL();
            HideAllLine();
            bdl = new Base_DL();
            dtreport = dt;
            L_Log_DL ldl = new L_Log_DL();

            ldl.L_Log_Insert(lst);
            // AllEvent_PrintLog();
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
          //  ldl.L_Log_Insert_Print(lst);
        }

        private void Youshi_Load(object sender, EventArgs e)
        {
            try
            {
                Load_Report();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Source + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Message);
            }

        }
        public void HideAllLine()
        {
            yreport y = new yreport();
            var f = y.Section3.ReportObjects;
            foreach (var ob in f)
            {
                if (ob is BoxObject)
                {
                    if ((ob as BoxObject).Kind == CrystalDecisions.Shared.ReportObjectKind.BoxObject)
                    {
                        var c = (BoxObject)y.ReportDefinition.ReportObjects[(ob as BoxObject).Name];
                        c.LineStyle = CrystalDecisions.Shared.LineStyle.NoLine;
                        //(ob as BoxObject).LineColor = Color.Transparent;
                        (ob as BoxObject).LineStyle = CrystalDecisions.Shared.LineStyle.NoLine;
                    

                    }
                }
                else if ( ob is LineObject)
                {
                    if ((ob as LineObject).Kind == CrystalDecisions.Shared.ReportObjectKind.LineObject)
                    {
                        var c = (LineObject)y.ReportDefinition.ReportObjects[(ob as LineObject).Name];
                        c.LineStyle = CrystalDecisions.Shared.LineStyle.NoLine;
                        //  (ob as LineObject).LineColor = Color.Transparent;
                        (ob as LineObject).LineStyle =  CrystalDecisions.Shared.LineStyle.NoLine;
                    }
                }
             
            }
            
            y.Refresh();

        }
        string Custom ="" ;
        public void Load_Report()
        {

            Custom = dtreport.Rows[0]["LongName"].ToString();
            yreport yreport = new yreport();
            dtreport.Rows[0]["LongName"] = dtreport.Rows[0]["LongName"] + dtreport.Rows[0]["AliasKBN"].ToString() ;
            dtreport.AcceptChanges();
            yreport.SetDataSource(dtreport);
            GetSource gs = GetPrintData(dtreport);
            yreport.SetParameterValue("pa_1", gs.pa_1);
            yreport.SetParameterValue("pa_5", gs.pa_5);
            yreport.SetParameterValue("pa_2", gs.pa_2);
            yreport.SetParameterValue("pa_3", gs.pa_3);
            yreport.SetParameterValue("pa_4", gs.pa_4);
            yreport.SetParameterValue("pa_6", gs.pa_6);
            yreport.SetParameterValue("pan_1", gs.pan_1);
            yreport.SetParameterValue("pan_2", gs.pan_2);
            yreport.SetParameterValue("pan_3", gs.pan_3);
            yreport.SetParameterValue("pan_4", gs.pan_4);
            yreport.SetParameterValue("pan_5", gs.pan_5);
            yreport.SetParameterValue("pan_6", gs.pan_6);
            yreport.SetParameterValue("pan_7", gs.pan_7);
            yreport.SetParameterValue("sg_1", gs.sg_1);
            yreport.SetParameterValue("sg_2", gs.sg_2);
            yreport.SetParameterValue("sg_3", gs.sg_3);
            yreport.SetParameterValue("sg_4", gs.sg_4);
            yreport.SetParameterValue("sg_5", gs.sg_5);
            yreport.SetParameterValue("sg_6", gs.sg_6);
            yreport.SetParameterValue("sg_7", gs.sg_7);
            yreport.SetParameterValue("Custom",Custom);
            crystalReportViewer1.ReportSource = null;
          //  crystalReportViewer1.RefreshReport();
            crystalReportViewer1.ReportSource = yreport;
           // crystalReportViewer1.RefreshReport();

        }

        public GetSource GetPrintData(DataTable dt)
        {
            GetSource gs= new GetSource();
            if (dt.Rows.Count == 1)
            {
                
                var pa = dt.Rows[0]["P_Account"].ToString().PadLeft(6,' ');
                var pan = dt.Rows[0]["P_AccountNo"].ToString().PadLeft(7, ' ');
                var g= dt.Rows[0]["SalesGaku"].ToString().Split('.').FirstOrDefault().PadLeft(7, ' ');

                var sg = g;

                //gs.pa_1 = pa[0].ToString();
                //gs.pa_2 = pa[1].ToString();
                //gs.pa_3 = pa[2].ToString();
                //gs.pa_4 = pa[3].ToString();
                //gs.pa_5 = pa[4].ToString();
                //gs.pa_6 = pa[5].ToString();
                //gs.pan_1 = pan[0].ToString();
                //gs.pan_2 = pan[1].ToString();
                //gs.pan_3 = pan[2].ToString();
                //gs.pan_4 = pan[3].ToString();
                //gs.pan_5 = pan[4].ToString();
                //gs.pan_6 = pan[5].ToString();
                //gs.pan_7 = pan[6].ToString();
                //gs.sg_1 = sg[0].ToString();
                //gs.sg_2 = sg[1].ToString();
                //gs.sg_3 = sg[2].ToString();
                //gs.sg_4 = sg[3].ToString();
                //gs.sg_5 = sg[4].ToString();
                //gs.sg_6 = sg[5].ToString();
                //gs.sg_7 = sg[6].ToString();
                gs.pa_1 = "9";
                gs.pa_2 = "9";
                gs.pa_3 = "9";
                gs.pa_4 = "9";
                gs.pa_5 = "9";
                gs.pa_6 = "9";
                gs.pan_1 = "9";
                gs.pan_2 = "9";
                gs.pan_3 = "9";
                gs.pan_4 = "9";
                gs.pan_5 = "9";
                gs.pan_6 = "9";
                gs.pan_7 = "9";
                gs.sg_1 = "9";
                gs.sg_2 = "9";
                gs.sg_3 = "9";
                gs.sg_4 = "9";
                gs.sg_5 = "9";
                gs.sg_6 = "9";
                gs.sg_7 = "9";
            }

            return gs;
        }
    }
    public class GetSource
    {
        public string pa_1 { get; set; }
        public string pa_2 { get; set; }
        public string pa_3 { get; set; }
        public string pa_4 { get; set; }
        public string pa_5 { get; set; }
        public string pa_6 { get; set; }
        public string pan_1 { get; set; }
        public string pan_2 { get; set; }
        public string pan_3 { get; set; }
        public string pan_4 { get; set; }
        public string pan_5 { get; set; }
        public string pan_6 { get; set; }
        public string pan_7 { get; set; }
        public string sg_1 { get; set; }
        public string sg_2 { get; set; }
        public string sg_3 { get; set; }
        public string sg_4 { get; set; }
        public string sg_5 { get; set; }
        public string sg_6 { get; set; }
        public string sg_7 { get; set; }
    }

    
}
