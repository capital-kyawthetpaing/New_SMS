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

namespace MainMenu.Haspo
{
    public partial class HapoStore_MainMenu : Form
    {

        DataTable menu;

        M_Staff_Entity mse;
        Menu_BL mbl;
        string Staff_CD = "";
        string btnText = string.Empty;
        public HapoStore_MainMenu(string SCD,M_Staff_Entity mse)
        {

            mbl = new Menu_BL();
            Staff_CD = SCD;
            this.mse = mse;
            InitializeComponent();
            lblOperatorName.Text = mse.StaffName;
            SetDesignerFunction();
        }
        private void SetDesignerFunction()

        {
            this.KeyPreview = true;
            Event_Designer(panel_left);
            Event_Designer(panel_right);
            this.Load += HapoStore_MainMenu_Load;
        }
        private void HapoStore_MainMenu_Load(object sender, EventArgs e)
        {
            Clear_Text(panel_left);
            Clear_Text(panel_right);
            BindButtonName();
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
        private void BindButtonName()
        {
            var dt = menu = mbl.getMenuNo(Staff_CD, Base_DL.iniEntity.StoreType);
            var _result = menu.AsEnumerable().GroupBy(x => x.Field<string>("Char1")).Select(g => g.First()).CopyToDataTable();
            ButtonText(panel_left, _result, 1);
        }
        protected void ButtonText(Panel p, DataTable k0, int Gym)
        {
            IOrderedEnumerable<DataRow> result;
            result = k0.Select().OrderBy(row => row["BusinessSEQ"]);
            var k = result.CopyToDataTable();
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
                            }
                        }
                        else if (Gym == 0 && k.Rows[j]["ProgramID"].ToString() != string.Empty)
                        {
                            if (((CKM_Button)ctrl).Name == "btn_Proj" + Convert.ToInt32(k.Rows[j]["ProgramSEQ"].ToString()))
                            {
                                ((CKM_Button)ctrl).Text = k.Rows[j]["ProgramID"].ToString();
                                ((CKM_Button)ctrl).Enabled = true;
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
            for(int i =0;i< c.Count();i++)
            {
                Control ctrl=c.ElementAt(i) as Control;
                if(ctrl is CKM_Button)
                {
                    if(pnl == panel_left)
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
        private void panelRight_Click(object sender, EventArgs e)
        {
            OpenForm(sender);
        }

        private void RightButton_Text(string Text)
        {
            var getDataa = menu.Select("Char1 = '" + Text + "'").CopyToDataTable();
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
                RightButton_Text(btnText);
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

                (sender as CKM_Button).Tag = System.Diagnostics.Process.Start(filePath + @"\" + exe_name + ".exe", cmdLine + "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("The program cannot locate to the specified file!!!");
            }
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
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            Store_Message sm = new Store_Message(mse);
            sm.ShowDialog(); ;
        }

        private void ckM_Button1_Click(object sender, EventArgs e)
        {
            HaspoStoreMenuLogin hln = new HaspoStoreMenuLogin();
            this.Hide();
            hln.ShowDialog();
            this.Close();
        }
    }
}
