using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CKM_Controls;
using BL;
using Entity;
using System.Diagnostics;
using DL;
using System.Runtime.InteropServices;
using Tulpep.NotificationWindow;
using System.IO;
using static CKM_Controls.CKM_Button;

namespace MainMenu
{
    public partial class Main_Menu : Form
    {
        DataTable menu;
        CKM_Button btnleftcurrent;
        CKM_Button btnrightcurrent;
        private string fbase = "";// AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\", "") + @"MainMenu\Logos\";
        private DataTable Dflg = null;
        M_Staff_Entity mse;
        Menu_BL mbl;
        string Staff_CD = "";
        string btnText = string.Empty;
        private int Mstate = 0;
        private int Hstate = 0;
        private bool Defg = false;
        public Main_Menu(String SCD, M_Staff_Entity mse)
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
               txt_Mesaage.Text= dt.Rows[0]["Message"].ToString();
            }
            lblOperatorName.Text = mse.StaffName;
        }
        private void SetDesignerFunction()
        {
            this.KeyPreview = true;
            Event_Designer(panelLeft);
            Event_Designer(panel_right);
            this.Load += Main_Menu_Load;
        }

        private void Main_Menu_Load(object sender, EventArgs e)
        {
            this.ResizeRedraw = false;
            BindButtonName();
            ChangeFont(CKM_FontSize.XSmall);
            
            
            if (!Login_BL.Islocalized)
            {
                Login_BL loginbl = new Login_BL();
                var df =loginbl.CheckDefault("1", Staff_CD);// MenuFlg 1 for MainMenuCapital
                if (df.Rows.Count > 0)
                {
                    Dflg = df;
                    if (df.Rows[0]["SettingKBN"].ToString() == "1")
                        pictureBox2.Visible = true;
                    if (df.Rows[0]["DefaultKBN"].ToString() == "0")
                        Setting(df);
                    else
                        Defg = true;

                }
                
            }
            ButtonState();
            ParentID = "";
        }
        private void Setting(DataTable df)
        {
            //menuLogo
            //var L_pth = Login_BL.FtpPath.Replace("Sync", "Setting") + df.Rows[0]["M_LogoName"].ToString();
            //var Bitmap = FTPData.GetImage(L_pth);
            //if (Bitmap == null)
            //{
            //    MessageBox.Show("Server error");
            //    return;
            //}
            pictureBox1.Image =  Getbm((df.Rows[0]["M_LogoCD"] as byte[]));
            //Icon
            //var I_Pth = Login_BL.FtpPath.Replace("Sync", "Setting") + df.Rows[0]["IconName"].ToString();
            //var FStream = FTPData.GetImageStream(I_Pth);
            try
            {
                this.Icon = new System.Drawing.Icon(new MemoryStream(df.Rows[0]["IconCD"] as byte[]), 32, 32);
            }
            catch
            { }
            //Theme
            var cname = df.Rows[0]["ThemeKBN"].ToString();
            try
            {
                panelRight.BackColor = panel3.BackColor = panel1.BackColor = System.Drawing.ColorTranslator.FromHtml(cname); ;
            }
            catch
            {
                panelRight.BackColor = panel3.BackColor = panel1.BackColor = System.Drawing.ColorTranslator.FromHtml("#"+cname); ;
            }
            // Font 
            var val = Convert.ToInt32(df.Rows[0]["FSKBN"].ToString());
            var fstyle = Convert.ToInt32(df.Rows[0]["FWKBN"].ToString());
            if (val == 1)
            {
                ChangeFont((CKM_FontSize.Normal), fstyle);
            }
            else if (val == 2)
            {
                ChangeFont((CKM_FontSize.XSmall), fstyle);
            }
            else if (val == 3)
            {
                ChangeFont((CKM_FontSize.Small), fstyle);
            }
            else if (val == 4)
            {
                ChangeFont((CKM_FontSize.Medium), fstyle);
            }
            else if (val == 5)
            {
                ChangeFont((CKM_FontSize.Large), fstyle);
            }
            else if (val == 6)
            {
                ChangeFont((CKM_FontSize.XLarge), fstyle);
            }

            Mstate = Convert.ToInt32(df.Rows[0]["M_NormalKBN"].ToString());
            Hstate = Convert.ToInt32(df.Rows[0]["M_HoverKBN"].ToString());
            //
        }
        private Bitmap Getbm(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }
        public static string StringToColor(string colorStr)
        {
            string name = colorStr;
            //foreach (KnownColor kc in Enum.GetValues(typeof(KnownColor)))
            //{
            //    Color known = Color.FromKnownColor(kc);
            //    if (colorToCheck.ToArgb() == known.ToArgb())
            //    {
            //        name = known.Name;
            //    }
            //}
            return name;
        }
        protected void ButtonState()
        {
            int state = Mstate;
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
                            (ctrl as CKM_Button).BackgroundImage = Properties.Resources.bn_9;
                            (ctrl as CKM_Button).ForeColor = Color.Black; ;
                            (ctrl as CKM_Button).FlatAppearance.BorderColor = Color.White;

                        }
                        else
                        {
                            if (state == 0 || state == 1)
                            {
                                (ctrl as CKM_Button).BackgroundImage = Properties.Resources.bm_3;
                            }
                            else if (state == 2)
                            {
                                (ctrl as CKM_Button).BackgroundImage = Properties.Resources.MTennic;// Image.FromFile(fbase + "MTennic.png");
                            }
                            else if (state == 3)
                            {
                                (ctrl as CKM_Button).BackgroundImage = Properties.Resources.MHaspo; //Image.FromFile(fbase + "MHaspo.png");
                                (ctrl as CKM_Button).ForeColor = Color.Black; ;
                            }
                            else if (state == 4)
                            {
                                (ctrl as CKM_Button).BackgroundImage = Properties.Resources.MOther; //Image.FromFile(fbase + "MOther.png");
                            }

                        }
                        (ctrl as CKM_Button).BackgroundImageLayout = ImageLayout.Stretch;
                    }
                }
            }
            if (!Defg)
            {
              //  ckM_Label1.Font = lblOperatorName.Font = lblLoginDate.Font = d;
                // btnLogin.Font = new Font("MS Gothic", 22, FontStyle.Bold);
            }
            else
            {
                btnLogin.Font = new Font("MS Gothic", 22, FontStyle.Bold);
            }
        }
        private Font d = null;
        protected void ChangeFont(CKM_FontSize fs, int fstyle = 0)
        {
           d = this.Font;
            //  var c = GetAllControls(this);
            var c = GetAllControls(this);
            for (int i = 0; i < c.Count(); i++)
            {
                Control ctrl = c.ElementAt(i) as Control;

                if (ctrl is CKM_Button ctrb)
                {
                    ctrb.Font_Size = fs;
                    if (fstyle != 0)
                    {
                        //1 regular
                        //2 bold
                        //3 italic
                        if (fstyle == 1)
                        {
                            d= ctrb.Font = new System.Drawing.Font("MS Gothic", ctrb.Font.Size, System.Drawing.FontStyle.Regular);
                        }
                        else if (fstyle == 2)
                        {
                            d = ctrb.Font = new System.Drawing.Font("MS Gothic", ctrb.Font.Size, System.Drawing.FontStyle.Bold);
                        }
                        else if (fstyle == 3)
                        {
                            d = ctrb.Font = new System.Drawing.Font("MS Gothic", ctrb.Font.Size, System.Drawing.FontStyle.Italic);
                        }

                    }
                }
                else if (ctrl is CKM_Label ctl)
                {
                    ///   ctl.Font_Size = CKM_Label.CKM_FontSize.Small;
                }
            }
            if (fstyle != 0)
            {
                if (!Defg)
                btnLogin.Font = d;
            }
           
        }
        protected void BindButtonName()
        {
            var dt = menu = mbl.getMenuNo(Staff_CD, Base_DL.iniEntity.StoreType);
            if (dt.Rows.Count > 0)
            {
                var _result = dt.AsEnumerable().GroupBy(x => x.Field<string>("Char1")).Select(g => g.First()).CopyToDataTable();
                //////Changed by PTk bcox of Gtone  HOMESTAYED TIME COVID_19
             var  dt1=   dt.AsEnumerable()
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
                                ((CKM_Button)ctrl).TabIndex =Convert.ToInt32 ( k.Rows[j]["BusinessSEQ"].ToString());

                            }
                        }
                        else if (Gym == 0 && k.Rows[j]["ProgramID"].ToString() != string.Empty)
                        {
                            if (((CKM_Button)ctrl).Name == "btnProj" + Convert.ToInt32(k.Rows[j]["ProgramSEQ"].ToString()))
                            {
                                ((CKM_Button)ctrl).Text = k.Rows[j]["ProgramID"].ToString();
                                ((CKM_Button)ctrl).Enabled = true;
                                ((CKM_Button)ctrl).TabIndex = Convert.ToInt32(k.Rows[j]["ProgramSEQ"].ToString());
                               /// ((CKM_Button)ctrl).Tag = Convert.ToInt32(k.Rows[j]["ProgramID_ID"].ToString());
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
        private void panelRight_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.Khaki;
            OpenForm(sender);
        }
        public string ParentID { get; set; }
        private void OpenForm(object sender)
        {
            string filePath = ""; string cmdLine = ""; string exe_name = "";
            try
            {
              if ((sender as CKM_Button).Text == "権限設定")
                {
                    Login_BL loginbl = new Login_BL();
                    var df = loginbl.CheckDefault("1", Staff_CD);
                    if (df.Rows[0]["AdminKBN"].ToString() == "0")
                    {
                        MessageBox.Show("You have no permission to this access.");
                        return;
                    }
                }
                //var programID = (sender as CKM_Button).Text;
                // exe_name = menu.Select("ProgramID = '"+programID+"'").CopyToDataTable().Rows[0]["ProgramID_ID"].ToString();
                var programID = (sender as CKM_Button).Text;
                //var Condition = "ProgramID = '" + programID + "'" + "";
                var Condition = "BusinessSEQ = '" + ParentID + "'" + " And " + "ProgramSEQ = '" + (sender as CKM_Button).Name.Split('j').Last() + "'";
                 exe_name = menu.Select(Condition).CopyToDataTable().Rows[0]["ProgramID_ID"].ToString();
                //System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                //string filePath = System.IO.Path.GetDirectoryName(u.LocalPath);


                if (Debugger.IsAttached || Login_BL.Islocalized)
                {
                    System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                    filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
              //   filePath = @"C:\SMS\AppData";
                }
                else
                {
                    filePath = @"C:\SMS\AppData";
                }
                cmdLine = " " + "001" + " " + mse.StaffCD + " " + Login_BL.GetHostName();

                Process[] localByName = Process.GetProcessesByName(exe_name);
                if (localByName.Count() > 0)
                {

                   
                    IntPtr handle = localByName[0].MainWindowHandle;
                    ShowWindow(handle, SW_SHOWMAXIMIZED);
                    SetForegroundWindow(handle);


                    return;
                }
                System.Diagnostics.Process.Start(filePath + @"\\" + exe_name + ".exe", cmdLine + "");
               //  MessageBox.Show(  Environment.NewLine + cmdLine + Environment.NewLine + filePath + Environment.NewLine + exe_name+ "Upper" + Base_DL.iniEntity.DatabaseServer);
                // (sender as CKM_Button).Tag = System.Diagnostics.Process.Start(filePath + @"\" + exe_name + ".exe", cmdLine + "");
                //  MessageBox.Show(Environment.NewLine + cmdLine + Environment.NewLine + filePath + Environment.NewLine + exe_name + "Lower");

            }
            catch (Exception ex)
            {
                MessageBox.Show("The program cannot locate to the specified file!!!" + ex.ToString() +Environment.NewLine +cmdLine +Environment.NewLine + filePath+Environment.NewLine+ exe_name);
            }
        }
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        var cp = base.CreateParams;
        //        cp.ExStyle |= 8;  // Turn on WS_EX_TOPMOST
        //        return cp;
        //    }
        //}
        private const int SW_SHOWMAXIMIZED = 3;

        [DllImport("user32.dll")]

        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        
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
                ParentID = btn.Name.Split('m').Last();
                RightButton_Text(btnText, btn.TabIndex);
            }
        }
        private void RightButton_Text(string Text,int tabindex)
        {
            var getData = menu.Select("Char1 = '" + Text + "' and BusinessSEQ ='"+tabindex.ToString()+"'").CopyToDataTable();
            if (getData != null)
            {
                ButtonText(panel_right, getData, 0);
            }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SetForegroundWindow(IntPtr hwnd);
        private void panelRight_MouseEnter(object sender, EventArgs e)
        {
            if (Hstate == 0 || Hstate == 1)
            {
                (sender as CKM_Button).BackgroundImage = Properties.Resources.bmback_3;
                (sender as CKM_Button).ForeColor = Color.Black;
            }
            else if (Hstate == 2)
            {

                (sender as CKM_Button).BackgroundImage = Properties.Resources.HTennic; //Image.FromFile(fbase + "HTennic.png");
                (sender as CKM_Button).ForeColor = Color.Black;
            }
            else if (Hstate == 3)
            {
                (sender as CKM_Button).BackgroundImage = Properties.Resources.HHaspo;// Image.FromFile(fbase + "HHaspo.png");
                (sender as CKM_Button).ForeColor = Color.White;
            }
            else if (Hstate == 4)
            {
                (sender as CKM_Button).BackgroundImage = Properties.Resources.HOther;// Image.FromFile(fbase + "HOther.png");
                (sender as CKM_Button).ForeColor = Color.Black;
            }
        }

        private void panelRight_MouseLeave(object sender, EventArgs e)
        {
            if ((sender) as CKM_Button != btnleftcurrent)
            {
                (sender as CKM_Button).BackgroundImage = Properties.Resources.bn_9;
                (sender as CKM_Button).ForeColor = Color.Black;
            }
        }
        private void panelLeft_MouseEnter(object sender, EventArgs e)
        {
            if (Hstate == 1 || Hstate == 0)
            {
                (sender as CKM_Button).BackgroundImage = Properties.Resources.bmback_3;
                (sender as CKM_Button).ForeColor = Color.Black;
            }
            else if (Hstate == 2)
            {

                (sender as CKM_Button).BackgroundImage = Properties.Resources.HTennic;// HOther  Image.FromFile(fbase+"HTennic.png");
                (sender as CKM_Button).ForeColor = Color.Black;
            }
            else if (Hstate == 3)
            {
                (sender as CKM_Button).BackgroundImage = Properties.Resources.HHaspo;// Image.FromFile(fbase + "HHaspo.png");
                (sender as CKM_Button).ForeColor = Color.White;
            }
            else if (Hstate == 4)
            {
                (sender as CKM_Button).BackgroundImage = Properties.Resources.HOther;// Image.FromFile(fbase + "HOther.png");
                (sender as CKM_Button).ForeColor = Color.Black;
            }
           // (sender as CKM_Button).Font = new System.Drawing.Font((sender as CKM_Button).Font.FontFamily, (sender as CKM_Button).Font.SizeInPoints , System.Drawing.FontStyle.Bold);
            
        }

        private void panelLeft_MouseLeave(object sender, EventArgs e)
        {
            if ((sender) as CKM_Button != btnleftcurrent)
            {
                (sender as CKM_Button).BackgroundImage = Properties.Resources.bm_3;
                (sender as CKM_Button).ForeColor = Color.White;

            }
            if (Mstate == 2)
            {
                (sender as CKM_Button).BackgroundImage = Properties.Resources.MTennic;// Image.FromFile(fbase + "MTennic.png");
                (sender as CKM_Button).ForeColor = Color.White;
            }
            else if (Mstate == 3)
            {
                (sender as CKM_Button).BackgroundImage = Properties.Resources.MHaspo;// Image.FromFile(fbase + "MHaspo.png");
                (sender as CKM_Button).ForeColor = Color.Black;
            }
            else if (Mstate == 4)
            {
                (sender as CKM_Button).BackgroundImage = Properties.Resources.MOther;// Image.FromFile(fbase + "MOther.png");
                (sender as CKM_Button).ForeColor = Color.White;
            }
        }
        public void Main_Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CheckOpenForm())
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
        }
        private bool CheckOpenForm()
        {
            return (Application.OpenForms["Main_Menu"].Visible == true);
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
        private void Main_Menu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F1)
            {
                this.Close();
            }
            else if (e.KeyData == Keys.F12)
            {
                ForceToClose();
                  MainmenuLogin hln = new MainmenuLogin();
                this.Hide();
                hln.ShowDialog();
                this.Close();

            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ForceToClose();
               MainmenuLogin hln = new MainmenuLogin(true);
            this.Hide();
            hln.ShowDialog();
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_MouseHover(object sender, EventArgs e)
        {
          
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
            // 
            if (!Defg)
            (sender as CKM_Button).Font = new Font("MS Gothic", 14, FontStyle.Bold);
            else
                (sender as CKM_Button).Font = new Font("MS Gothic", 22, FontStyle.Bold);
            (sender as CKM_Button).ForeColor = Color.White;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Setting st = new Setting();
            st.Width = (sender as PictureBox).Location.X+10;
            var y =   this.Location.Y +30;
            var x = this.Location.X;
            st.Location = new Point(x,y);
            st.LC = new Point(x, y);
            var m = "";
            if (this.Name.Contains("Main"))
            {
                m = "MainMenu";
            }
            else
            {
                m = "StoreMenu";
            }
            Login_BL loginbl = new Login_BL();
            var df = loginbl.CheckDefault("1", Staff_CD);// MenuFlg 1 for MainMenuCapital
            st.Dflg = df;
            st.MenuID = m;
            st.OperatorName = lblOperatorName.Text;
            st.StaffCD = Staff_CD;
            st.MCD = "1";
            st.ShowDialog();
        }
        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            (sender as PictureBox).Image = Properties.Resources.img_380355;
        }
        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            (sender as PictureBox).Image = Properties.Resources.HoverSetting;
        }
    }
}
