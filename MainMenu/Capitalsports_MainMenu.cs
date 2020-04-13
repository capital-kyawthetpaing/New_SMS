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
using System.Diagnostics;
using DL;
using System.Runtime.InteropServices;

namespace MainMenu
{
    public partial class Capitalsports_MainMenu : Form
    {
        DataTable menu;
        CKM_Button btnleftcurrent;
        CKM_Button btnrightcurrent;

        M_Staff_Entity mse;
        Menu_BL mbl;
        string Staff_CD = "";
        string btnText = string.Empty;
        private const int SW_SHOWMAXIMIZED = 3;

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public Capitalsports_MainMenu(String SCD, M_Staff_Entity mse)
        {
            mbl = new Menu_BL();
            Staff_CD = SCD;
            this.mse = mse;
            InitializeComponent();
            lblOperatorName.Text = mse.StaffName;
            SetDesignerFunction();

            lblOperatorName.Text = mse.StaffName;
        }
        private void SetDesignerFunction()
        {
            this.KeyPreview = true;
            Event_Designer(panel_left);
            Event_Designer(panel_right);
            this.Load += Capital_MainMenu_Load;
        }
        private void Capital_MainMenu_Load(object sender, EventArgs e)
        {
            Clear_Text(panel_left);
            Clear_Text(panel_right);
            BindButtonName();
        }
        protected void Clear_Text(Panel pnl)
        {
            var c = GetAllControls(pnl);
            int j = 0;
            for (int i = 0; i < c.Count(); i++)
            {
                Control ctrl = c.ElementAt(i) as Control;
                if (ctrl is CKM_Button)
                {
                    j++;
                    ((CKM_Button)ctrl).Text = "";
                    ((CKM_Button)ctrl).Enabled = false;
                    ToolTip ttp = new ToolTip();
                    ttp.SetToolTip(((CKM_Button)ctrl), null);
                    // ((CKM_Button)ctrl).Name = "btnPro" + (c.Count() - (j));
                }
            }


        }
        protected void BindButtonName()
        {
            var dt = menu = mbl.getMenuNo(Staff_CD, Base_DL.iniEntity.StoreType);
            if (dt.Rows.Count > 0)
            {
                var _result = dt.AsEnumerable().GroupBy(x => x.Field<string>("Char1")).Select(g => g.First()).CopyToDataTable();
                //////Changed by PTk bcox of Gtone  HOMESTAYED TIME COVID_19
                var dt1 = dt.AsEnumerable()
                       .GroupBy(r => new { Col1 = r["BusinessID"], Col2 = r["BusinessSEQ"] })
                       .Select(g =>
                       {
                           var row = dt.NewRow();
                        //r => r["PK"]).First()
                        //row["PK"] = g.Min(r => r.Field<int>("PK"));
                        row["char1"] = g.First().Field<string>("char1");
                           row["BusinessID"] = g.Key.Col1;
                           row["BusinessSEQ"] = g.Key.Col2;

                           return row;

                       })
                       .CopyToDataTable();
                ButtonText(panel_left, dt1, 1);
            }
            //var dt = menu = mbl.getMenuNo(Staff_CD, Base_DL.iniEntity.StoreType);

            ////var _result =(from r1 in dt.AsEnumerable()  group r1 by new { Char1 = r1.Field<string>("Char1"), } into g  select new { Char1 = g.Key.Char1,   BusinessSEQ = g.Max(x => x.Field<int>("BusinessSEQ")) }).ToArray();    //Group By
            //if (dt.Rows.Count > 0)
            //{
            //    var _result = dt.AsEnumerable().GroupBy(x => x.Field<string>("Char1")).Select(g => g.First()).CopyToDataTable();
            //    ButtonText(panel_left, _result, 1);
            //}
        }
        protected void ButtonText(Panel p, DataTable k0, int Gym)
        {
            IOrderedEnumerable<DataRow> result;
            if (Gym == 1)
                result = k0.Select().OrderBy(row => row["BusinessSEQ"]);
            else
                result = k0.Select().OrderBy(row => row["ProgramSEQ"]);
            var k = result.CopyToDataTable();
            // MainMenuLogin
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();

            for (int j = 0; j < k.Rows.Count; j++)
            {
                var c = GetAllControls(p);
                for (int i = 0; i < c.Count(); i++)
                {
                    Control ctrl = c.ElementAt(i) as Control;

                    if (ctrl is CKM_Button)
                    {

                        ToolTip1.SetToolTip(((CKM_Button)ctrl), null);
                        if (Gym == 1 && k.Rows[j]["Char1"].ToString() != string.Empty && k.Rows[j]["BusinessSEQ"].ToString() != string.Empty)
                        {
                            if (((CKM_Button)ctrl).Name == "btnGym" + Convert.ToInt32(k.Rows[j]["BusinessSEQ"].ToString()))
                            {
                                ((CKM_Button)ctrl).Text = k.Rows[j]["Char1"].ToString();
                                ((CKM_Button)ctrl).Enabled = true;
                                ((CKM_Button)ctrl).TabIndex = Convert.ToInt32(k.Rows[j]["BusinessSEQ"].ToString());

                            }
                        }
                        else if (Gym == 0 && k.Rows[j]["ProgramID"].ToString() != string.Empty)
                        {
                            if (((CKM_Button)ctrl).Name == "btn_Proj" + Convert.ToInt32(k.Rows[j]["ProgramSEQ"].ToString()))
                            {
                                ((CKM_Button)ctrl).Text = k.Rows[j]["ProgramID"].ToString();
                                ((CKM_Button)ctrl).Enabled = true;
                                ((CKM_Button)ctrl).TabIndex = Convert.ToInt32(k.Rows[j]["ProgramSEQ"].ToString());
                                //  ((CKM_Button)ctrl).Name = mope_data.PROID.ToString();
                                // ToolTip1.SetToolTip(((CKM_Button)ctrl),"");
                                //ToolTip1.SetToolTip(((CKM_Button)ctrl), ((CKM_Button)ctrl).Text);
                            }
                        }
                    }
                }
            }
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
        private void Event_Designer(Panel pnl)
        {
            var c = GetAllControls(pnl);
            for (int i = 0; i < c.Count(); i++)
            {
                Control ctrl = c.ElementAt(i) as Control;
                if (ctrl is CKM_Button)
                {
                    if (pnl == panel_left)
                    {
                        ((CKM_Button)ctrl).Click += panelLeft_Click;
                    }
                    else
                    {
                        ((CKM_Button)ctrl).MouseLeave += panelRight_MouseLeave;
                        ((CKM_Button)ctrl).MouseEnter += panelRight_MouseEnter;
                        ((CKM_Button)ctrl).Click += panelRight_Click;
                    }
                }
            }
        }
        private void panelRight_MouseEnter(object sender, EventArgs e)
        {
            (sender as CKM_Button).BackgroundColor = CKM_Button.CKM_Color.Orange;
        }

        private void panelRight_MouseLeave(object sender, EventArgs e)
        {
            (sender as CKM_Button).BackgroundColor = CKM_Button.CKM_Color.Yellow;
        }
        private void panelRight_Click(object sender, EventArgs e)
        {
            OpenForm(sender);
        }
        private void ckM_Button4_Click(object sender, EventArgs e)
        {

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
                return;
        }
        private void panelLeft_Click(object sender, EventArgs e)
        {
            var c = GetAllControls(panel_right);
            int j = 0;
            for (int i = 0; i < c.Count(); i++)
            {
                Control ctrl = c.ElementAt(i) as Control;
                if (ctrl is CKM_Button)
                {
                    j++;
                    ((CKM_Button)ctrl).Text = "";
                    ((CKM_Button)ctrl).Enabled = false;
                    ToolTip ttp = new ToolTip();
                    ttp.SetToolTip(((CKM_Button)ctrl), null);
                    ((CKM_Button)ctrl).BackgroundColor = CKM_Button.CKM_Color.Yellow;
                    // ((CKM_Button)ctrl).Name = "btnPro" + (c.Count() - (j));
                }
            }
            CKM_Button btn = sender as CKM_Button;
            btnText = btn.Text;
            if (!string.IsNullOrWhiteSpace(btnText))
            {
                RightButton_Text(btnText, btn.TabIndex);
            }
        }
        private void RightButton_Text(string Text,int TabIndex)
        {
            var getDataa = menu.Select("Char1 = '" + Text + "' and BusinessSEQ ='" + TabIndex.ToString() + "'").CopyToDataTable();

            //var getDataa = menu.Select("Char1 = '" + Text + "'").CopyToDataTable();
            getDataa.DefaultView.Sort = "ProgramSEQ asc";
            getDataa.AcceptChanges();
            IOrderedEnumerable<DataRow> result;
            result = getDataa.Select().OrderBy(row => row["ProgramSEQ"]);
         var getData=   result.CopyToDataTable();
            if (getData.Rows.Count > 14)    // remove after processing
            {
                for (int k = 0; k < (getData.Rows.Count); k++)
                {
                    if (getData.Rows.Count > 14)
                    {
                        getData.Rows.RemoveAt(getData.Rows.Count -1);
                    }
                    else
                    {
                        break;
                    }

                }
                MessageBox.Show("Menu item is over 14 and the extra menus can not be shown!");
            }
            if (getData.Rows.Count < 15)
            {
                if (getData != null)
                {
                    ButtonText(panel_right, getData, 0);
                }
            }
            else {
                MessageBox.Show("Too Much Menu Items Occurred!!!");
            }
        }
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
        private void OpenForm(object sender)
        {
            try
            {
                var programID = (sender as CKM_Button).Text;
                var exe_name = menu.Select("ProgramID = '" + programID + "'").CopyToDataTable().Rows[0]["ProgramID_ID"].ToString();
                //System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                //string filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
                string filePath = "";
                //System.Diagnostics.Debug 
                if (Debugger.IsAttached || Login_BL.Islocalized)
                {
                    System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                    filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
                }
                else
                {
                    filePath = @"C:\\SMS\\AppData";
                }
                string cmdLine = " " + "01" + " " + mse.StaffCD + " " + Login_BL.GetHostName();
                Process[] localByName = Process.GetProcessesByName(exe_name);
                if (localByName.Count() > 0)
                {
                    IntPtr handle = localByName[0].MainWindowHandle;
                    ShowWindow(handle, SW_SHOWMAXIMIZED);
                    return;
                }
                (sender as CKM_Button).Tag = System.Diagnostics.Process.Start(filePath + @"\" + exe_name + ".exe", cmdLine + "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("The program cannot locate to the specified file!!!");
            }
        }

        private void ckM_Button1_Click(object sender, EventArgs e)
        {
           // this.Hide();
            CapitalsportsLogin hln = new CapitalsportsLogin();
            this.Hide();
            hln.ShowDialog();
            this.Close();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            Store_Message sm = new Store_Message( mse);
            sm.ShowDialog(); ;
        }
    }
}
