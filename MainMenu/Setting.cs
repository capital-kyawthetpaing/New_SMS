using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using DL;
using Entity;
namespace MainMenu
{
    public partial class Setting : Form
    {
        bool IsLoad = false;
        public string OperatorName = "";
        public string MenuID = "";
        public string StaffCD = "";
        public M_Setting Msetting;
        public string MCD = "";
        Menu_BL mbl;
        FTPData fd = new FTPData();
        string PC = "";
        public Setting()
        {
            InitializeComponent();
            IsLoad = true;
        }
        public DataTable Dflg = null;
        public Point LC;
        private void Setting_Load(object sender, EventArgs e)
        {
            lbl_ID.Text = MenuID;
            lblOperator.Text = OperatorName;
            if (MenuID.Contains("Main"))
            {
                txtHeader.Enabled = false;
            }
            BindCombo();
            Msetting = new M_Setting();
            mbl = new Menu_BL();
            Msetting.StaffCD = StaffCD;
            Msetting.MenuKBN = MCD;// 1 =Capitalmain, 2 = CapitalStore,3=haspoMain, 4 =haspoStore, 5= TennicMain
            PC = System.Environment.MachineName;
            lblVer.Text = Login_BL.Ver;
            lblVer.ForeColor = Color.Red;
            Setting_();
            button2.Enabled = false;

        }
        public void Setting_()
        { 
            if (Dflg.Rows.Count > 0 && Dflg.Rows[0]["DefaultKBN"].ToString() == "0")
            {
                Msetting.LoginLogo = Dflg.Rows[0]["L_LogoCD"] as byte[];
                pb_Login.Image = Getbm(Msetting.LoginLogo);//

                Msetting.MenuLogo = Dflg.Rows[0]["M_LogoCD"] as byte[];
                pb_menu.Image = Getbm(Msetting.MenuLogo);//

                Msetting.Icon = Dflg.Rows[0]["IconCD"] as byte[];//
                pb_ico.Image = Getbm(Msetting.Icon);//

                Msetting.Theme = Dflg.Rows[0]["ThemeKBN"].ToString();//

                try//
                {
                    pb_theme.BackColor = System.Drawing.ColorTranslator.FromHtml("#" + Msetting.Theme); ;
                }
                catch
                {
                    pb_theme.BackColor = System.Drawing.ColorTranslator.FromHtml(Msetting.Theme); ;//
                }
                cbo_Size.SelectedValue = Dflg.Rows[0]["FSKBN"];//
                Msetting.FSKBN = cbo_Size.SelectedValue.ToString();
                cbo_Style.SelectedValue = Dflg.Rows[0]["FWKBN"];//
                Msetting.FWKBN = cbo_Style.SelectedValue.ToString();

                Msetting.HKBN = Dflg.Rows[0]["M_HoverKBN"].ToString();
                (this.Controls.Find("h_" + Msetting.HKBN, true)[0] as RadioButton).Checked = true;

                Msetting.MKBN = Dflg.Rows[0]["M_NormalKBN"].ToString();
                (this.Controls.Find("m_" + Msetting.MKBN, true)[0] as RadioButton).Checked = true;
            }
            else
                DefaultSetting();
        }
        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        public void DefaultSetting()
        {
            pb_Login.Image = MainMenu.Properties.Resources.nc2;
            Msetting.LoginLogo = imageToByteArray(pb_Login.Image);

            pb_menu.Image = MainMenu.Properties.Resources.logo;   // wait current MenuLogo is bluringgg, asked to fukuda  
            Msetting.MenuLogo = imageToByteArray(pb_menu.Image);
          //  var fbase = AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\", "") + @"MainMenu\Logos\logo_mark_l5k_icon.ico";

            pb_ico.Image = Properties.Resources.logo_mark_l5k_icon; // Image.FromFile(fbase);
            Msetting.Icon = ImageToByte(Properties.Resources.logo_mark_l5k_icon) ;
            
            //Font
            cbo_Size.SelectedValue= Msetting.FSKBN = "3";
            cbo_Style.SelectedValue = Msetting.FWKBN = "1";
            h_1.Checked = true;
            Msetting.HKBN = "1";
            m_1.Checked = true;
            Msetting.MKBN = "1";
            pb_theme.BackColor = Color.LightGray ;
            Msetting.Theme = "LightGray";
            //

            // Theme
        }
        
        protected void BindCombo()
        {
            //Normal,
            //XSmall,
            //Small,
            //Medium,
            //Large,
            //XLarge
            DataTable dt = new DataTable();
            dt.Columns.Add("Key");
            dt.Columns.Add("Value");
            dt.Rows.Add("1", "Normal");
            dt.Rows.Add("2", "XSmall");
            dt.Rows.Add("3", "Small");
            dt.Rows.Add("4", "Medium");
            dt.Rows.Add("5", "Large");
            dt.Rows.Add("6", "XLarge");
            cbo_Size.DataSource = dt;
            cbo_Size.DisplayMember = "Value";
            cbo_Size.ValueMember = "Key";
            cbo_Size.SelectedValue = 1;

            DataTable dtWeight = new DataTable();
            dtWeight.Columns.Add("Key");
            dtWeight.Columns.Add("Value");
            dtWeight.Rows.Add("1", "Regular");
            dtWeight.Rows.Add("2", "Bold");
            dtWeight.Rows.Add("3", "Italic");
            cbo_Style.DataSource = dtWeight;
            cbo_Style.DisplayMember = "Value";
            cbo_Style.ValueMember = "Key";
            cbo_Style.SelectedValue = 1;
        }
        private void button5_Click_1(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.AllowFullOpen = false;
            colorDlg.AnyColor = true;
            colorDlg.SolidColorOnly = false;
           
            colorDlg.Color = Color.Red;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                pb_theme.BackColor =colorDlg.Color;
                Msetting.Theme = pb_theme.BackColor.Name;
                button2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.InitialDirectory = "Documents";
            o.Filter = "File_Logo|*.jpg;*.jpeg;*.png";
            if (o.ShowDialog() == DialogResult.OK)
            {
                button2.Enabled = true;
                pb_Login.Image = System.Drawing.Bitmap.FromFile(o.FileName);
                Msetting.LoginLogo = System.IO.File.ReadAllBytes(o.FileName);
                Msetting.Local_LoginLogo = o.FileName;
            }
        }
        protected  void BrowseUpload(string p, string MM, out string FName )
        {
            
            var res = fd.FileUpload(p, MCD+MM, out string resName);
            FName = "";
            if (res)
            {
                FName = resName;
            }
            else
            {
                MessageBox.Show("Error in File reading or server connection. . .");
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.InitialDirectory = "Documents";
            o.Filter = "File_Logo|*.jpg;*.jpeg;*.png";
            if (o.ShowDialog() == DialogResult.OK)
            {
                button2.Enabled = true;
                pb_menu.Image = System.Drawing.Bitmap.FromFile(o.FileName);
                Msetting.MenuLogo = System.IO.File.ReadAllBytes(o.FileName);
                Msetting.Local_MenuLogo = o.FileName;
            }
        }

        private void Setting_LocationChanged(object sender, EventArgs e)
        {
            if (!IsLoad)
                this.Location = LC;
            else
                IsLoad = false;
            return;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.InitialDirectory = "Documents";
            o.Filter = "File_Icon|*.ico;";
            if (o.ShowDialog() == DialogResult.OK)
            {

                var pth = o.FileName;
                var img = System.Drawing.Image.FromFile(pth);
                if (img.Width > 48 || img.Height > 48 || !Path.GetExtension(pth).Contains("ico"))
                {
                    MessageBox.Show("Please browse an ico file size within range of 48x48 or 32x32 or 16x16.");
                    return;
                }
                button2.Enabled = true;
                pb_ico.Image = System.Drawing.Bitmap.FromFile(o.FileName);
                Msetting.Icon = System.IO.File.ReadAllBytes(o.FileName);
                Msetting.Local_Icon = o.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            CheckImages(out bool Ok);
            if (!Ok)
            {
                this.Cursor = Cursors.Default;
                return;
            }
            Msetting.HText = txtHeader.Text;
            Msetting.FSKBN = cbo_Size.SelectedValue.ToString();
            Msetting.FWKBN = cbo_Style.SelectedValue.ToString();
            Msetting.HKBN = CheckChecked(true);
            Msetting.MKBN = CheckChecked(false);
            Msetting.PC = this.PC;
           // Msetting.Test = @"C:\Users\Dell\Pictures\bn_9.png";
            //Msetting.Imge = System.IO.File.ReadAllBytes(@"C:\Users\Dell\Pictures\bmback_3.png");

            UploadImg(Msetting);
            if (mbl.SettingSave(Msetting))
            {
                MessageBox.Show("Saved Setting Successfully!");
            }
            this.Cursor = Cursors.Default;
        }
        public string CheckChecked(bool hover )
        {
            var i = 0;
            if (hover)
                i = h_1.Checked ? 1 : h_2.Checked ? 2 : h_3.Checked ? 3 : 4;
            else
                i = m_1.Checked ? 1 : m_2.Checked ? 2 : m_3.Checked ? 3 : 4;
            return i.ToString();

        }
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        public byte[] ConvertToBitmap(string fileName)
        {
            return new MemoryStream(File.ReadAllBytes(fileName)).ToArray();
            //var ms = new MemoryStream();
            //using (Stream bmpStream = System.IO.File.Open(fileName, System.IO.FileMode.Open))
            //{
            //    bmpStream.CopyTo(ms);
            //}
            //return ms.ToArray();
        }
        private Bitmap Getbm(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(Image.FromStream(mStream));
         ///  bm.Save(mStream);
            mStream.Dispose();
            return bm;
            // Image.FromStream(m)
        }
        private void UploadImg(M_Setting ms)
        {
            //Dont Delete it by PTK
            //FTP Logic 
            //BrowseUpload(ms.Local_LoginLogo,"L", out string FLogin);
            //ms.LoginLogo =Base_DL.iniEntity.DatabaseName+"/"+ FLogin.Split('/').Last();
            //BrowseUpload(ms.Local_MenuLogo, "M", out string FMenu);
            //ms.MenuLogo = Base_DL.iniEntity.DatabaseName + "/" + FMenu.Split('/').Last();
            //BrowseUpload(ms.Local_Icon, "I", out string FIcon);
            //ms.Icon = Base_DL.iniEntity.DatabaseName + "/" +  FIcon.Split('/').Last();
        }
        private void CheckImages(out bool Ok)
        {
            Ok = false;
            if (pb_Login.Image == null )
            {
                MessageBox.Show("Browse a file . . .");
                button1.Focus();
                return;
            }
            if (pb_menu.Image == null)
            {
                MessageBox.Show("Browse a file . . .");
                button4.Focus();
                return;
            }
            if (pb_ico.Image == null)
            {
                MessageBox.Show("Browse a file . . .");
                button6.Focus();
                return;
            }
            if (pb_theme.BackColor.Name == "ffc6e0b4")
            {
                MessageBox.Show("Browse a file . . .");
                button5.Focus();
                return;
            }
            Ok= true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            DefaultSetting();
            if (mbl.SettingDefault(StaffCD , MCD))
            {
                MessageBox.Show("Changed as default Successfully!");
                button2.Enabled = false;
            }


            this.Cursor = Cursors.Default;
        }

        private void txtHeader_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void cbo_Size_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void ckM_RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }
    }

}
