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
using System.Threading;
using EPSON_TM30;

namespace MainMenu.Haspo
{
    public partial class HapoStore_MainMenu : Form
    {

        DataTable menu;

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
        public HapoStore_MainMenu(string SCD,M_Staff_Entity mse)
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
            this.Load += HapoStore_MainMenu_Load;
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

                    if ((ctrl as CKM_Button).Parent is Panel && (((ctrl as CKM_Button).Parent as Panel).Name == "panel_left" || ((ctrl as CKM_Button).Parent as Panel).Name == "panel_right"))
                    {
                        (ctrl as CKM_Button).Font_Size = CKM_Button.CKM_FontSize.Large;
                        (ctrl as CKM_Button).BackgroundImageLayout = ImageLayout.Stretch;
                    }
                }
            }
        }
        private void HapoStore_MainMenu_Load(object sender, EventArgs e)
        {
            Clear_Text(panel_left);
            Clear_Text(panel_right);
            BindButtonName();
            ParentID = "";
            M_Staff_Entity mse2 = new M_Staff_Entity
            {
                StaffCD = Staff_CD
            };
            Login_BL loginbl = new Login_BL();
            mse2 = loginbl.M_Store_InitSelect(mse);

             
            lblStoreName.Text = mse2.StoreName;

            
        }


        protected void Clear_Text(Panel p)
        {
            var c = GetAllControls(p);
           int j = 0;
            for(int i=0;i < c.Count(); i++)
            {
                Control ctrl = c.ElementAt(i) as Control;
                if (ctrl is CKM_Button)
                {
                    j++;
                    ((CKM_Button)ctrl).Text = "";
                    ((CKM_Button)ctrl).Enabled = false;
                    ToolTip ttp = new ToolTip();
                    ttp.SetToolTip(((CKM_Button)ctrl), null);
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
            //var _result =(from r1 in dt.AsEnumerable()  group r1 by new { Char1 = r1.Field<string>("Char1"), } into g  select new { Char1 = g.Key.Char1,   BusinessSEQ = g.Max(x => x.Field<int>("BusinessSEQ")) }).ToArray();    //Group By
            //var _result = dt.AsEnumerable().GroupBy(x => x.Field<string>("Char1")).Select(g => g.First()).CopyToDataTable();
            //ButtonText(panelLeft, _result, 1);
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
                                if (!Base_DL.iniEntity.IsDM_D30Used && k.Rows[j]["ProgramID_ID"].ToString() == "CashDrawerOpen")
                                {
                                    ((CKM_Button)ctrl).Text = "";
                                    ((CKM_Button)ctrl).Enabled = false;
                                    ((CKM_Button)ctrl).TabIndex = Convert.ToInt32(k.Rows[j]["ProgramSEQ"].ToString());
                                }
                                else
                                {
                                    ((CKM_Button)ctrl).Text = k.Rows[j]["ProgramID"].ToString();
                                    ((CKM_Button)ctrl).Enabled = true;
                                    ((CKM_Button)ctrl).TabIndex = Convert.ToInt32(k.Rows[j]["ProgramSEQ"].ToString());
                                }
                                //  ((CKM_Button)ctrl).Name = mope_data.PROID.ToString();
                                // ToolTip1.SetToolTip(((CKM_Button)ctrl),"");
                                //ToolTip1.SetToolTip(((CKM_Button)ctrl), ((CKM_Button)ctrl).Text);
                            }
                        }
                    }
                }
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
                        ((CKM_Button)ctrl).MouseLeave += panelLeft_MouseLeave;
                        ((CKM_Button)ctrl).MouseEnter += panelLeft_MouseEnter;
                        ((CKM_Button)ctrl).Click += panelLeft_Click;
                        ((CKM_Button)ctrl).EnabledChanged += OnEnabledChanged;
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
        private void panelLeft_MouseLeave(object sender, EventArgs e)
        {
            (sender as CKM_Button).BackgroundImage = Properties.Resources.bm_3;
            (sender as CKM_Button).ForeColor = Color.White;
        }
        private void panelLeft_MouseEnter(object sender, EventArgs e)
        {
            (sender as CKM_Button).BackgroundImage = Properties.Resources.bmback_3;
            (sender as CKM_Button).ForeColor = Color.Black;
        }
        private void OnEnabledChanged(object sender, EventArgs e)
        {
            //if (!((CKM_Button)sender).Enabled)
            //((CKM_Button)sender).BackgroundImage = MainMenu.Properties.Resources.bm_3;

        }
        private void panelRight_MouseEnter(object sender, EventArgs e)
        {
            (sender as CKM_Button).BackColor = Color.Transparent;
            (sender as CKM_Button).BackgroundImage = Properties.Resources.bmback_3;
            (sender as CKM_Button).ForeColor = Color.Black;
            //  (sender as CKM_Button).BackgroundColor = CKM_Button.CKM_Color.Orange;
        }
        private void panelRight_MouseLeave(object sender, EventArgs e)
        {
            (sender as CKM_Button).BackColor = Color.Transparent;
            (sender as CKM_Button).BackgroundImage = Properties.Resources.bn_9;
            (sender as CKM_Button).ForeColor = Color.Green;
            //   (sender as CKM_Button).BackgroundColor = CKM_Button.CKM_Color.Yellow;
        }
        private void panelRight_Click(object sender, EventArgs e)
        {
            IsClose = false;
            OpenForm(sender);
        }
        bool IsClose = false;
        private void RightButton_Text(string Text, int TabIndex)
        {
            var getDataa = menu.Select("Char1 = '" + Text + "' and BusinessSEQ ='" + TabIndex.ToString() + "'").CopyToDataTable();
            getDataa.DefaultView.Sort = "ProgramSEQ asc";
            getDataa.AcceptChanges();
            IOrderedEnumerable<DataRow> result;
            result = getDataa.Select().OrderBy(row => row["ProgramSEQ"]);
            var getData = result.CopyToDataTable();
            if (getData.Rows.Count > 14)    // remove after processing
            {
                for (int k = 0; k < (getData.Rows.Count); k++)
                {
                    if (getData.Rows.Count > 14)
                    {
                        getData.Rows.RemoveAt(getData.Rows.Count - 1);
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
            else
            {
                MessageBox.Show("Too Much Menu Items Occurred!!!");
            }
        }

        private void panelLeft_Click(object sender, EventArgs e)
        {
            IsClose = false;
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
                ParentID = btn.Name.Split('m').Last();
                RightButton_Text(btnText, btn.TabIndex);
            }
        }
        public string ParentID { get; set; }
        private void OpenForm(object sender)
        {
            try
            {
                var programID = (sender as CKM_Button).Text;
                //var exe_name = menu.Select("ProgramID = '" + programID + "'").CopyToDataTable().Rows[0]["ProgramID_ID"].ToString();

                var Condition = "BusinessSEQ = '" + ParentID + "'" + " And " + "ProgramSEQ = '" + (sender as CKM_Button).Name.Split('j').Last() + "'";
                var exe_name = menu.Select(Condition).CopyToDataTable().Rows[0]["ProgramID_ID"].ToString();
                if (Base_DL.iniEntity.IsDM_D30Used && exe_name == "CashDrawerOpen")
                {
                    string E = "";
                    try
                    {
                        CashDrawerOpen cdo_open = new CashDrawerOpen();
                        
                        cdo_open.OpenCashDrawer(false,false,E);
                    }
                    catch (Exception ex)
                    {
                        var nl = Environment.NewLine;
                        MessageBox.Show(ex.Message +nl +ex.StackTrace +nl+  ex.InnerException +nl+ ex.TargetSite.Name + nl + E   );
                    }
                }
                else
                {
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
                    RejectDisplay(exe_name);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("The program cannot locate to the specified file!!!");
            }
        }
        private void RejectDisplay(string Exename)
        {
            string[] str = new string[] {"TempoRegiHanbaiTouroku" ,"TempoRegiPoint", "TempoRegiRyousyuusyo", "TempoRegiTorihikiReceipt",
                "TempoRegiRyougaeNyuuryoku", "TempoRegiShiharaiNyuuryoku", "TempoRegiTsurisenJyunbi","TempoRegiNyuukinTouroku","TempoRegiNyuukinNyuuryoku" };

            foreach (var f in str)
            {
                if (Exename == f)
                {
                    try
                    {

                        Process[] localByName = Process.GetProcessesByName("Display_Service");
                        Process[] localByName1 = Process.GetProcessesByName("Display_Service.exe");
                        ////////if (Base_DL.iniEntity.IsDM_D30Used && localByName.Count() == 0 && localByName1.Count() == 0)
                        ////////{
                        ////////   // cdo.RemoveDisplay();
                        ////////}
                    }
                    catch
                    {
                        MessageBox.Show("Reclick on HBT");
                    }
                    break;
                }
            }
        }
        protected Base_BL bbl = new Base_BL();
        private void btnClose_Click(object sender, EventArgs e)
        {
            IsClose = true;
         //   if (bbl.ShowMessage("Q003") == DialogResult.Yes)
                this.Close();
            //else
            //{
            //    btnClose.Focus();
            //}
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            IsClose = false;
               Store_Message sm = new Store_Message(mse);
            sm.ShowDialog(); ;
        }

        private void ckM_Button1_Click(object sender, EventArgs e)
        {
            ForceToClose();
            HaspoStoreMenuLogin hln = new HaspoStoreMenuLogin(true);
            this.Hide();
            hln.ShowDialog();
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

        private void HapoStore_MainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsClose)
            {
                if (CheckOpenForm())
                {
                    BL.Base_BL bbl = new Base_BL();
                    if (bbl.ShowMessage("Q003") == DialogResult.Yes)
                    {
                        try
                        {
                            ForceToclose();
                        }
                        catch { }
                        e.Cancel = false;
                    }
                    else
                        e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = true;
            }
        }
        private bool CheckOpenForm()
        {
            return (Application.OpenForms["HapoStore_MainMenu"].Visible == true);
        }
        CashDrawerOpen cdo = new CashDrawerOpen();
        public void ForceToclose()
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
            try
            {
                Login_BL lbl = new Login_BL();
                try
                {
                    lbl.Display_Service_Update(false);
                    Thread.Sleep(3 * 1000);
                    lbl.Display_Service_Enabled(false);
                }
                catch
                {

                }
                cdo.RemoveDisplay();
                // true, true,Base_DL.iniEntity.DefaultMessage
                //  Base_DL.Ini_Entity_CDP.CDO_DISPLAY.RemoveDisplay();
            }
            catch
            {

            }

            //try
            //{
            //    m_Display.MarqueeType = DisplayMarqueeType.None;  // marquee close
            //    m_Display.DestroyWindow();                        // instance close we have created


            //    m_Display.ClearText();
            //    m_Display.DeviceEnabled = false;
            //    m_Display.Release();
            //    m_Display.Close();
            //}
            //catch(Exception ex)
            //{
            //   // MessageBox.Show(ex.Message);
            //}
        }
        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            (sender as CKM_Button).BackgroundImage = Properties.Resources.bmback_3;
            (sender as CKM_Button).ForeColor = Color.Black;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            (sender as CKM_Button).BackgroundImage = Properties.Resources.bn_10;
            (sender as CKM_Button).ForeColor = Color.White;
        }

        private void btnProcess_MouseLeave(object sender, EventArgs e)
        {
            (sender as CKM_Button).BackgroundImage = Properties.Resources.bn_9;
            (sender as CKM_Button).ForeColor = Color.Green;
        }

        private void ckM_Button1_MouseLeave(object sender, EventArgs e)
        {
            (sender as CKM_Button).BackgroundImage = Properties.Resources.bn_12;
            (sender as CKM_Button).ForeColor = Color.White;
        }
    }
}
