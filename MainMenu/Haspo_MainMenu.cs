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
using CKM_Controls;
using System.Diagnostics;
using DL;
using System.Runtime.InteropServices;

namespace MainMenu
{
    public partial class Haspo_MainMenu : Form
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


        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SetForegroundWindow(IntPtr hwnd);
        public Haspo_MainMenu(String SCD, M_Staff_Entity mse)
        {
            this.mse = mse;
            InitializeComponent();
            SetDesignerFunction();
            Staff_CD = SCD;
            mbl = new Menu_BL();
            menu = new DataTable();
            lblLoginDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            var dt = mbl.D_MenuMessageSelect(SCD);
            if (dt.Rows.Count > 0)
            {
                txt_Mesaage.Text = dt.Rows[0]["Message"].ToString();
            }

            lblOperatorName.Text = mse.StaffName;
        }
        private void Event_Designer(Panel pnl)
        {
            var c = GetAllControls(pnl);
            for (int i = 0; i < c.Count(); i++)
            {
                Control ctrl = c.ElementAt(i) as Control;
                if (ctrl is CKM_Button)
                {
                    ((CKM_Button)ctrl).Text = "";
                    ((CKM_Button)ctrl).Enabled = false;
                    if (pnl == panelLeft)
                    {
                        ((CKM_Button)ctrl).Click += panelLeft_Click;
                        ((CKM_Button)ctrl).MouseEnter += panelLeft_MouseEnter;
                        ((CKM_Button)ctrl).MouseLeave += panelLeft_MouseLeave;
                    }
                    else
                    {
                        ((CKM_Button)ctrl).Click += panelRight_Click;
                        ((CKM_Button)ctrl).MouseEnter += panelRight_MouseEnter;
                        ((CKM_Button)ctrl).MouseLeave += panelRight_MouseLeave;
                    }
                }
            }
        }
        private void SetDesignerFunction()
        {
            this.KeyPreview = true;
            Event_Designer(panelLeft);
            Event_Designer(panel_right);
            this.Load += Haspo_MainMenu_Load;
        }
        private void panelRight_MouseEnter(object sender, EventArgs e)
        {
            (sender as CKM_Button).BackgroundImage = Properties.Resources.bmback_3;
            (sender as CKM_Button).ForeColor = Color.Black;
            // (sender as CKM_Button).Font = new System.Drawing.Font((sender as CKM_Button).Font.FontFamily, (sender as CKM_Button).Font.SizeInPoints, System.Drawing.FontStyle.Bold);

            //  (sender as Button).BackColor = Color.Khaki;
        }

        private void panelRight_MouseLeave(object sender, EventArgs e)
        {
            if ((sender) as CKM_Button != btnleftcurrent)
            // (sender as CKM_Button).BackColor = Color.FromArgb(192, 255, 192);
            {
                (sender as CKM_Button).BackgroundImage = Properties.Resources.bn_9;
                (sender as CKM_Button).ForeColor = Color.Black;
                //   (sender as CKM_Button).Font = new System.Drawing.Font((sender as CKM_Button).Font.FontFamily, (sender as CKM_Button).Font.SizeInPoints, System.Drawing.FontStyle.Regular);

            }
            //if ((sender) as Button != btnrightcurrent)
            //    (sender as Button).BackColor = Color.FromArgb(255, 224, 192);
        }
        private void panelLeft_MouseEnter(object sender, EventArgs e)
        {
            (sender as CKM_Button).BackgroundImage = Properties.Resources.bmback_3;
            (sender as CKM_Button).ForeColor = Color.Black;
            // (sender as CKM_Button).Font = new System.Drawing.Font((sender as CKM_Button).Font.FontFamily, (sender as CKM_Button).Font.SizeInPoints , System.Drawing.FontStyle.Bold);

        }

        private void panelLeft_MouseLeave(object sender, EventArgs e)
        {
            if ((sender) as CKM_Button != btnleftcurrent)
            // (sender as CKM_Button).BackColor = Color.FromArgb(192, 255, 192);
            {
                (sender as CKM_Button).BackgroundImage = Properties.Resources.bm_3;
                (sender as CKM_Button).ForeColor = Color.White;
                //   (sender as CKM_Button).Font = new System.Drawing.Font((sender as CKM_Button).Font.FontFamily, (sender as CKM_Button).Font.SizeInPoints, System.Drawing.FontStyle.Regular);

            }
        }
        private void panelRight_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.Khaki;
            OpenForm(sender);
        }
        private void Haspo_MainMenu_Load(object sender, EventArgs e)
        {
            this.ResizeRedraw = false;
            BindButtonName();
            ChangeFont();

            //  TextRenderer.MeasureText("12345", btnProj1.Font);
            /// btnProj1.Font = new Font(Font, FontStyle.Bold);

            // Font dd;

            // this.Font = new System.Drawing.Font("MS Gothic", 16F, System.Drawing.FontStyle.Bold);
            //  this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            ButtonState();
        }
        protected void ButtonState()
        {
            var c = GetAllControls(this);
            for (int i = 0; i < c.Count(); i++)
            {
                Control ctrl = c.ElementAt(i) as Control;
                if (ctrl is CKM_Button)
                {
                    (ctrl as CKM_Button).FlatStyle = FlatStyle.Flat;
                    (ctrl as CKM_Button).FlatAppearance.BorderSize = 0;

                    //  (ctrl as CKM_Button).FlatAppearance.BorderColor = System.Drawing.ColorTranslator.FromHtml("#05af34") ;

                    if ((ctrl as CKM_Button).Parent is Panel && (((ctrl as CKM_Button).Parent as Panel).Name == "panelLeft" || ((ctrl as CKM_Button).Parent as Panel).Name == "panel_right"))
                    {


                        (ctrl as CKM_Button).ForeColor = Color.White; ;
                        ////(ctrl as CKM_Button).BackgroundImageLayout = ImageLayout.Stretch;
                        ///
                        if (((ctrl as CKM_Button).Parent as Panel).Name == "panel_right")
                        {
                            (ctrl as CKM_Button).BackgroundImage = MainMenu.Properties.Resources.bn_9;
                            (ctrl as CKM_Button).ForeColor = Color.Black; ;
                            (ctrl as CKM_Button).FlatAppearance.BorderColor = Color.White;
                        }
                        else
                        {
                            (ctrl as CKM_Button).BackgroundImage = MainMenu.Properties.Resources.bm_3;
                        }
                        (ctrl as CKM_Button).BackgroundImageLayout = ImageLayout.Stretch;
                    }
                }
            }

            btnLogin.Font = new Font("MS Gothic", 22, FontStyle.Bold);
        }
        protected void ChangeFont()
        {
            //  var c = GetAllControls(this);
            var c = GetAllControls(this);
            for (int i = 0; i < c.Count(); i++)
            {
                Control ctrl = c.ElementAt(i) as Control;

                if (ctrl is CKM_Button ctrb)
                {
                    ctrb.Font_Size = CKM_Button.CKM_FontSize.XSmall;
                }
                else if (ctrl is CKM_Label ctl)
                {
                    ///   ctl.Font_Size = CKM_Label.CKM_FontSize.Small;
                }
            }


        }
        protected void BindButtonName()
        {
            //var dt = menu = mbl.getMenuNo(Staff_CD, Base_DL.iniEntity.StoreType);
            //if (dt.Rows.Count > 0)
            //{
            //    //var _result =(from r1 in dt.AsEnumerable()  group r1 by new { Char1 = r1.Field<string>("Char1"), } into g  select new { Char1 = g.Key.Char1,   BusinessSEQ = g.Max(x => x.Field<int>("BusinessSEQ")) }).ToArray();    //Group By
            //    var _result = dt.AsEnumerable().GroupBy(x => x.Field<string>("Char1")).Select(g => g.First()).CopyToDataTable();
            //    ButtonText(panelLeft, _result, 1);
            //}
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
                ButtonText(panelLeft, dt1, 1);
            }
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
                            if (((CKM_Button)ctrl).Name == "btnProj" + Convert.ToInt32(k.Rows[j]["ProgramSEQ"].ToString()))
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
        //                else if (Gym == 0 && k.Rows[j]["ProgramID"].ToString() != string.Empty )
        //                {
        //                    if (((CKM_Button)ctrl).Name == "btnProj" + Convert.ToInt32(k.Rows[j]["ProgramSEQ"].ToString()))
        //                    {
        //                        ((CKM_Button)ctrl).Text = k.Rows[j]["ProgramID"].ToString();
        //                        ((CKM_Button)ctrl).Enabled = true;
        //                        //  ((CKM_Button)ctrl).Name = mope_data.PROID.ToString();
        //                       // ToolTip1.SetToolTip(((CKM_Button)ctrl),"");
        //                        //ToolTip1.SetToolTip(((CKM_Button)ctrl), ((CKM_Button)ctrl).Text);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
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
                string cmdLine = " " + "001" + " " + mse.StaffCD + " " + Login_BL.GetHostName();
                //Process[] localByName = Process.GetProcessesByName(exe_name);
                //if (localByName.Count() > 0)
                //{
                //    IntPtr handle = localByName[0].MainWindowHandle;
                //    ShowWindow(handle, SW_SHOWMAXIMIZED);
                //    return;
                //}

                Process[] localByName = Process.GetProcessesByName(exe_name);
                if (localByName.Count() > 0)
                {


                    IntPtr handle = localByName[0].MainWindowHandle;
                    ShowWindow(handle, SW_SHOWMAXIMIZED);
                    SetForegroundWindow(handle);


                    return;
                }
                (sender as CKM_Button).Tag = System.Diagnostics.Process.Start(filePath + @"\" + exe_name + ".exe", cmdLine + "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("The program cannot locate to the specified file!!!");
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
     
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void ForceToClose()
        {
            foreach (DataRow dr in menu.Rows)
            {
                var localByName = Process.GetProcessesByName(dr["ProgramID_ID"].ToString());
                if (localByName.Count() > 0)
                {

                    foreach (var process in localByName)
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }
        private void Haspo_MainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            BL.Base_BL bbl = new Base_BL();
            if (bbl.ShowMessage("Q003") == DialogResult.Yes)
            {
                ForceToClose();
                e.Cancel = false;
            }
            else
                e.Cancel = true;
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
        private void RightButton_Text(string Text, int TabIndex)
        {
            var getData = menu.Select("Char1 = '" + Text + "' and BusinessSEQ ='" + TabIndex.ToString() + "'").CopyToDataTable();
            if (getData != null)
            {
                ButtonText(panelRight,getData,0);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ForceToClose();
               HaspoLogin hln = new HaspoLogin();
            this.Hide();
            hln.ShowDialog();
            this.Close();
        }

        private void Haspo_MainMenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F1)
            {
                this.Close();
            }
            else if (e.KeyData == Keys.F12)
            {
                ForceToClose();
                   HaspoLogin hln = new HaspoLogin();
                this.Hide();
                hln.ShowDialog();
                this.Close();

            }

        }
        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            (sender as CKM_Button).BackgroundImage = Properties.Resources.bm_3;
            (sender as CKM_Button).ForeColor = Color.White;
        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            (sender as CKM_Button).BackgroundImage = Properties.Resources.bmback_3;
            (sender as CKM_Button).ForeColor = Color.Black;
        }

        private void btnLogin_MouseLeave(object sender, EventArgs e)
        {
            (sender as CKM_Button).BackgroundImage = Properties.Resources.bn_15;
            (sender as CKM_Button).Font = new Font("MS Gothic", 22, FontStyle.Bold);
            (sender as CKM_Button).ForeColor = Color.White;
        }
    }
}
