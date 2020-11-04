using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using BL;
using Entity;
using Search;
using System.IO;

namespace ShiireShoukaiDetails
{
    public partial class ShiireShoukaiDetails : FrmMainForm
    {

        D_Purchase_Details_Entity dpd_entity;
        ShiireShoukaiDetails_BL ssdbl ;
        Base_BL bbl ;
        private const string ShiireNyuuryokuFromNyuuka = "ShiireNyuuryokuFromNyuuka.exe";
        private const string ShiireNyuuryoku = "ShiireNyuuryoku.exe";
        private const string HenpinNyuuryoku = "HenpinNyuuryoku.exe";
        public ShiireShoukaiDetails()
        {
            InitializeComponent();
            dpd_entity = new D_Purchase_Details_Entity();
            ssdbl = new ShiireShoukaiDetails_BL();
            bbl = new Base_BL();
            
        }
        private void ShiireShoukaiDetails_Load(object sender, EventArgs e)
        {
            InProgramID = Application.ProductName;
            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            Btn_F10.Text = "出力(F10)";
            Btn_F10.Enabled = false;
            BindCombo();
            RequiredField();
            // this.cboStore.SelectedIndexChanged += CboStore_SelectedIndexChanged;
            cboStore.SelectedValue = StoreCD;
            scItem.CodeWidth = 600;
            scSkuCD.CodeWidth = 600;
            ModeVisible = false;
        }

        /// <summary>
        /// Combo の中であるデータに選択すること
        /// </summary>       
        //private void CboStore_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if(!cboStore.SelectedValue.Equals("-1"))
        //    {
        //        //if (!CboStore_ErrorCheck())
        //        //{
        //        //    ssdbl.ShowMessage("E141");
        //        //    cboStore.Focus();
        //        //}
        //        if (!base.CheckAvailableStores(cboStore.SelectedValue.ToString()))
        //        {
        //            ssdbl.ShowMessage("E141");
        //            cboStore.Focus();
        //        }
        //    }
        //}

        private void RequiredField()
        {
            cboStore.Require(true);

            //<Remark>入力無くても良い(It is not necessary to input)</Remark>
            //txtPurchaseDate1.Require(true);
            //txtPurchaseDate2.Require(true);
            //txtPlanDate1.Require(true);
            //txtPlanDate2.Require(true);
            //txtOrderDate1.Require(true);
            //txtOrderDate2.Require(true);
        }
        /// <summary>
        /// Combo の中で　店舗ストア　タイプのStoreName を表示される　こと
        /// </summary>
        private void BindCombo()
        {
            cboStore.Bind(string.Empty,"2");
        }
        /// <summary>
        /// 仕入先の履歴を表示されること
        /// </summary>
        private void sc_SupplierName_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                scMakerCD.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scMakerCD.TxtCode.Text))
                {
                    if (scMakerCD.SelectData())
                    {
                        scMakerCD.Value1 = scMakerCD.TxtCode.Text;
                        scMakerCD.Value2 = scMakerCD.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        scMakerCD.SetFocus(1);
                    }
                }

            }
        }
        protected override void EndSec()
        {
            this.Close();
        }
        private void ShiireShoukaiDetails_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        /// <summary>
        /// スタッフの履歴を表示されること
        /// </summary>        
        private void scStaffCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                scStaffCD.ChangeDate = bbl.GetDate();
                if(!string.IsNullOrEmpty(scStaffCD.TxtCode.Text))
                {
                    if(scStaffCD.SelectData())
                    {
                        scStaffCD.Value1 = scStaffCD.TxtCode.Text;
                        scStaffCD.Value2 = scStaffCD.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        scStaffCD.SetFocus(1);
                    }
                }
            }
        }
        public void Clear()
        {
            Clear(panel1);
            txtPurchaseDate1.Focus();
            cboStore.SelectedValue = StoreCD;
        }
        public override void FunctionProcess(int index)
        {
            CKM_SearchControl sc = new CKM_SearchControl();
           
            switch (index + 1)
            {
                case 2:
                    ChangeMode(EOperationMode.INSERT);
                    break;
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        ChangeMode(OperationMode);
                        Clear();
                        Btn_F10.Text = "出力(F10)";
                    }
                    break;
                case 10:
                    if (bbl.ShowMessage("Q203") == DialogResult.Yes)
                    {
                        ExportCSV();
                    }
                    break;
                case 11:                    
                    F11();
                    break;
            }
        }
        private void ChangeMode(EOperationMode OperationMode)
        {
            base.OperationMode = OperationMode;
            switch (OperationMode)
            {
                case EOperationMode.INSERT:
                    cboStore.Text = string.Empty;
                    Clear(PanelHeader);
                    Clear(panel1);
                    //EnablePanel(PanelHeader);
                    //GvTana.Enabled = false;
                    F2Visible = false;
                    F3Visible = false;
                    F4Visible = false;
                    F5Visible = false;
                    F9Visible = false;                   
                    F10Visible = true;
                    //Btn_F10.Text = "Excel出力(F10)";
                    F12Visible = false;
                    Btn_F10.Enabled = false;
                    //Btn_Display.Enabled = F11Enable = true;
                    chkOk.Checked = true;
                    chkNotOK.Checked = true;
                    dgv_PurchaseDetails.DataSource = null;
                    break;
            }            
        }

        private  D_Purchase_Details_Entity  GetDPurchaseDetails()
        {

            dpd_entity = new D_Purchase_Details_Entity
            {
                VendorCD = scMakerCD.TxtCode.Text,
                JanCD = scJanCD.TxtCode.Text,
                SKUCD = scSkuCD.TxtCode.Text,
                StoreCD = cboStore.SelectedValue.ToString(),
                ItemCD = scItem.TxtCode.Text,
                ITemName = txtItemName.Text,
                MakerItemCD = txtMakerItemCD.Text,
                //MakerName = txtMakerItemCD.Text,
                Purchase_SDate = txtPurchaseDate1.Text,
                Purchase_EDate = txtPurchaseDate2.Text,
                Plan_SDate = txtPlanDate1.Text,
                Plan_EDate = txtPlanDate2.Text,
                Order_SDate = txtOrderDate1.Text,
                Order_EDate = txtOrderDate2.Text,
                CheckValue=CheckValue(),
                //ChkSumi = chkOk.Checked ? "1" : "0",
                //ChkMi = chkNotOK.Checked ? "1" : "0",
                StaffCD = scStaffCD.TxtCode.Text,
            };
            return dpd_entity;
        }

        public string CheckValue()
        {
            string chk = string.Empty;
            
            if(chkNotOK.Checked && chkOk.Checked)
            {
                return string.Empty;
            }
            else if (!chkNotOK.Checked && !chkOk.Checked)
            {
                return string.Empty;
            }
            else
            {
                chk = chkNotOK.Checked ? "0" : "1";
                return chk;
            }
        }
        /// <summary>
        /// GirdView の中で あるデータを表示されるボタン
        /// </summary>
        private void Btn_Display_Click(object sender, EventArgs e)
        {
            F11();
        }
        private void F11()
        {
            if(ErrorCheck())
            {
                dpd_entity.ProgramID = "1";
               

                dpd_entity = GetDPurchaseDetails();

                DataTable dtResult = new DataTable();
                dtResult = ssdbl.ShiireShoukaiDetails_Select(dpd_entity);
                if(dtResult.Rows.Count>0)
                {
                    dgv_PurchaseDetails.DataSource = dtResult;
                    Btn_F10.Enabled = true;
                    //dgv_SizeColor.DataSource = dtResult;
                    //dgv_Store.DataSource = dtResult;
                }
                else
                {
                    ssdbl.ShowMessage("E128");
                    dgv_PurchaseDetails.DataSource = null;
                    txtPurchaseDate1.Focus();
                }
            }
        }

        
        /// <summary>
        /// Export Data Csv File 
        /// </summary>
        /// <remarks>CSVEXELフィールに出す事</remarks>
        /// 
        private void ExportCSV()
        {
            if (!ErrorCheck())
            {
                return;
            }

            if (dgv_PurchaseDetails.DataSource != null)
            {
                //Build the CSV file data as a Comma separated string.
                string csv = string.Empty;

                //LoacalDirectory
                string folderPath = "C:\\CSV\\";
                FileInfo logFileInfo = new FileInfo(folderPath);
                DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                if (!logDirInfo.Exists) logDirInfo.Create();
                //Add the Header row for CSV file.
                foreach (DataGridViewColumn column in dgv_PurchaseDetails.Columns)
                {
                    //if(column.HeaderText!="")
                    csv += column.HeaderText + ',';
                }
                //Add new line.
                csv += "\r\n";
                //Adding the Rows
                foreach (DataGridViewRow row in dgv_PurchaseDetails.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        //Add the Data rows.
                        if (cell.Value == null)
                            cell.Value = "";
                        //csv += cell.Value.ToString().Replace(",", ";")+ ',';
                        csv += cell.Value.ToString().Replace(",", "") + ',';
                    }

                    //Add new line.
                    csv += "\r\n";
                }
                dgv_PurchaseDetails.CurrentCell = dgv_PurchaseDetails.Rows[0].Cells[1];
                //Exporting to CSV.            
                File.WriteAllText(folderPath + "仕入照会 (仕入明細) " + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "csv", csv, Encoding.GetEncoding(932));
                ssdbl.ShowMessage("I203");
            }
            else
            {
                ssdbl.ShowMessage("E138");
                //MessageBox.Show("データがありません。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool ErrorCheck()
        {
            //<Remark>入力無くても良い(It is not necessary to input)</Remark>
            //if (!RequireCheck(new Control[] { txtPurchaseDate1, txtPurchaseDate2, txtPlanDate1, txtPlanDate2, txtOrderDate1, txtOrderDate2 }))
            //{
            //    return false;
            //}
            if (!txtPurchaseDate1.DateCheck() || !txtPurchaseDate2.DateCheck())
                return false;
            if (!txtPlanDate1.DateCheck() || !txtPlanDate2.DateCheck())
                return false;
            if (!txtOrderDate1.DateCheck() || !txtOrderDate2.DateCheck())
                return false;
            if (!string.IsNullOrEmpty(txtPurchaseDate1.Text)&& !string.IsNullOrEmpty(txtPurchaseDate2.Text))
            {
                if (string.Compare(txtPurchaseDate1.Text, txtPurchaseDate2.Text) == 1)
                {
                    ssdbl.ShowMessage("E104");
                    txtPurchaseDate2.Focus();
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(txtPlanDate1.Text) && !string.IsNullOrEmpty(txtPlanDate2.Text))
            {
                if (string.Compare(txtPlanDate1.Text, txtPlanDate2.Text) == 1)
                {
                    ssdbl.ShowMessage("E104");
                    txtPlanDate2.Focus();
                    return false;
                }
            }

            if(!string.IsNullOrEmpty(txtOrderDate1.Text) && !string.IsNullOrEmpty(txtOrderDate2.Text))
            {
               if (string.Compare(txtOrderDate1.Text, txtOrderDate2.Text) == 1)
               {
                  ssdbl.ShowMessage("E104");
                  txtOrderDate2.Focus();
                  return false;
               }
                
            }
            //if (!string.IsNullOrEmpty(scMakerCD.TxtCode.Text))
            //{
            //    scMakerCD.ChangeDate = DateTime.Today.ToShortDateString();
            //    if (!scMakerCD.IsExists(2))
            //    {
            //        ssdbl.ShowMessage("E101");
            //        scMakerCD.SetFocus(1);
            //        return false;
            //    }
            //}
            scMakerCD.ChangeDate = bbl.GetDate();
            if (!string.IsNullOrEmpty(scMakerCD.TxtCode.Text))
            {
                if (scMakerCD.SelectData())
                {
                    scMakerCD.Value1 = scMakerCD.TxtCode.Text;
                    scMakerCD.Value2 = scMakerCD.LabelText;
                }
                else
                {
                    bbl.ShowMessage("E101");
                    scMakerCD.SetFocus(1);
                    return false;
                }
            }

            scStaffCD.ChangeDate = bbl.GetDate();
            if (!string.IsNullOrEmpty(scStaffCD.TxtCode.Text))
            {
                if (scStaffCD.SelectData())
                {
                    scStaffCD.Value1 = scStaffCD.TxtCode.Text;
                    scStaffCD.Value2 = scStaffCD.LabelText;
                }
                else
                {
                    bbl.ShowMessage("E101");
                    scStaffCD.SetFocus(1);
                    return false;
                }
            }

            if (!RequireCheck(new Control[] { cboStore }))
            {
                return false;
            }

            if (!base.CheckAvailableStores(cboStore.SelectedValue.ToString()))
            {
                bbl.ShowMessage("E141");
                cboStore.Focus();
                return false;
            }
            return true;
        }

        private void txtPurchaseDate2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtPurchaseDate1.Text) && !string.IsNullOrEmpty(txtPurchaseDate2.Text))
                {
                    if (string.Compare(txtPurchaseDate1.Text, txtPurchaseDate2.Text)==1)
                    {
                        ssdbl.ShowMessage("E104");
                        txtPurchaseDate2.Focus();
                    }
                }
            }
        }

        private void txtPlanDate2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtPlanDate1.Text) && !string.IsNullOrEmpty(txtPlanDate2.Text))
                {
                    if (string.Compare(txtPlanDate1.Text, txtPlanDate2.Text) == 1)
                    {
                        ssdbl.ShowMessage("E104");
                        txtPlanDate2.Focus();
                    }
                }
            }            
        }

        private void txtOrderDate2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtOrderDate1.Text) && !string.IsNullOrEmpty(txtOrderDate2.Text))
                {
                   if (string.Compare(txtOrderDate1.Text, txtOrderDate2.Text) == 1)
                   {
                        ssdbl.ShowMessage("E104");
                        txtOrderDate2.Focus();
                   }
                 
                }
            }
          
        }

        private void scMakerCD_Enter(object sender, EventArgs e)
        {
            scMakerCD.Value1 = "1";//仕入先区分：1
            scMakerCD.ChangeDate = txtPurchaseDate2.Text;
        }

        private void cboStore_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!base.CheckAvailableStores(cboStore.SelectedValue.ToString()))
                {
                    bbl.ShowMessage("E141");
                    cboStore.Focus();
                }
            }
        }

        private void dgv_PurchaseDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            {
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                   e.RowIndex >= 0)
                {
                    DataTable dt = new DataTable();
                    
                    dt = ssdbl.SimpleSelect1("68", null, dgv_PurchaseDetails.Rows[e.RowIndex].Cells["PurchaseNO"].Value.ToString());
                    if (dt.Rows.Count > 0)
                    {
                        string ProcessKBN = dt.Rows[0]["ProcessKBN"].ToString();
                        string PurchaseNO = dt.Rows[0]["PurchaseNO"].ToString();
                        if (dt.Rows[0]["ProcessKBN"].ToString().Equals("1"))
                        {
                            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + ShiireNyuuryokuFromNyuuka;
                            if (System.IO.File.Exists(filePath))
                            {
                                string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " "+ PurchaseNO;
                                System.Diagnostics.Process.Start(filePath, cmdLine);
                            }
                        }
                        else if(dt.Rows[0]["ProcessKBN"].ToString().Equals("2"))
                        {
                            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + ShiireNyuuryoku;
                            if (System.IO.File.Exists(filePath))
                            {
                                string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " "+ PurchaseNO;
                                System.Diagnostics.Process.Start(filePath, cmdLine);
                            }
                        }
                        else if (dt.Rows[0]["ProcessKBN"].ToString().Equals("3"))
                        {
                            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + HenpinNyuuryoku;
                            if (System.IO.File.Exists(filePath))
                            {
                                string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + PurchaseNO;
                                System.Diagnostics.Process.Start(filePath, cmdLine);
                            }
                        }
                    }
                }
            }
        }
    }
}
