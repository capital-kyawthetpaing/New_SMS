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
using Entity;
using BL;
using System.IO;

namespace ShiireShoukaiShiiresaki
{
   
    public partial class ShiireShoukaiShiiresaki : FrmMainForm
    {
        private const string ProNm = "仕入照会(仕入先)";
        string paid = "";
        string unpaid = "";
        string payeeflg = "";
        string StoreAuthen_CD = "";
        string StoreAuthen_ChangeDate = "";
        bool cb_focus=false;
        D_Purchase_Entity dpde;
        ShiireShoukaiShiiresaki_BL dpurchase_bl;
        Base_BL bbl = new Base_BL();
        public ShiireShoukaiShiiresaki()
        {
            InitializeComponent();
            dpurchase_bl = new ShiireShoukaiShiiresaki_BL();
        }
        private void frmShiireShoukaiShiiresaki_Load(object sender, EventArgs e)
        {
            InProgramID = "ShiireShoukaiShiiresaki";
            SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            SetRequireField();
            BindCombo();
            chkPaid.Checked = true;
            chkUnpaid.Checked = true;
            base.InProgramNM = ProNm;
            Btn_F10.Enabled = false;
            txtPurchaseDateFrom.Focus();
        }
        private void SetRequireField()
        {
            ComboStore.Require(true);
            this.Btn_F10.Text = "出力(F10)";
            F2Visible = false;
            F3Visible = false;
            F4Visible = false;
            F5Visible = false;
            F9Visible = false;
            F12Visible = false;
        }
        public void BindCombo()
        {
            ComboStore.Bind(string.Empty, "2");
            ComboStore.SelectedValue = StoreCD;
        }
        /// <summary>
        /// エラーチェック処理
        /// </summary>
        private D_Purchase_Entity GetSearchInfo()
        {
            dpde = new D_Purchase_Entity
            {

                VendorCD = scSupplier.TxtCode.Text,
                StaffCD = scStaff.TxtCode.Text,
                ArrivalDateFrom = txtPurchaseDateFrom.Text,
                ArrivalDateTo = txtPurchaseDateTo.Text,
                PurchaseDateFrom = txtPurchaseDateFrom.Text,
                PurchaseDateTo = txtPurchaseDateTo.Text,
                PaymentDueDateFrom = txtPaymentDueDateFrom.Text,
                PaymentDueDateTo = txtPaymentDueDateTo.Text,
                DeliveryNo = txtDeliveryNoteNo.Text,
                PayeeFLg = payeeflg,
                CheckValue=CheckValue()
                //Paid = paid,
                //UnPaid=unpaid,
                
               
            };

            return dpde;
        }

        public string CheckValue()
        {
            string chk = string.Empty;

            if (chkUnpaid.Checked && chkPaid.Checked)
            {
                return string.Empty;
            }
            else if (!chkUnpaid.Checked && !chkPaid.Checked)
            {
                return string.Empty;
            }
            else
            {
                chk = chkUnpaid.Checked ? "0" : "1";
                return chk;
            }
        }

        /// <summary>
        /// 表示ボタンを押下時GridViewにデータを表示する処理
        /// </summary>

        private void F11()
        {
            dgvPurchaseSearch.DataSource = null;
            if (ErrorCheck())
            {
                if (!string.IsNullOrWhiteSpace(scSupplier.TxtCode.Text))
                {
                    M_Vendor_Entity mve = new M_Vendor_Entity()
                    {
                        VendorCD = scSupplier.TxtCode.Text,
                        ChangeDate=scSupplier.ChangeDate.ToString()
                    };
                    string aa = scSupplier.TxtCode.Text;
                     payeeflg = dpurchase_bl.SelectPayeeFlg(mve);
                }
                dpde = GetSearchInfo();

                DataTable dtPurchase = dpurchase_bl.D_Purchase_Search(dpde);
               
                if (dtPurchase.Rows.Count > 0)
                {
                    dgvPurchaseSearch.Refresh();
                    dgvPurchaseSearch.DataSource = dtPurchase;
                    dgvPurchaseSearch.Enabled = true;
                    dgvPurchaseSearch.Focus();
                    Btn_F10.Enabled = true;
                }
                else
                {
                    dpurchase_bl.ShowMessage("E128");
                    dgvPurchaseSearch.DataSource = null;
                }
            }
        }

        public override void FunctionProcess(int index)
        {
            if (index + 1 == 11)
            {
                F11();
            }
            if (index + 1 == 10)
            {
                    ExportCSV();
                    //dgvPurchaseSearch.CurrentCell = dgvPurchaseSearch.Rows[0].Cells[1];
            }
            if(index + 1 == 6)
            {
                if (dpurchase_bl.ShowMessage("Q004") == DialogResult.Yes)
                {
                    Clear(PanelHeader);
                    txtPurchaseDateFrom.Focus();
                    dgvPurchaseSearch.DataSource = null;
                }
            }
        }

        private void btnSubF11_Click(object sender, EventArgs e)
        {
            F11();
        }
        /// <summary>
        /// 済チェックボックスをクリックする時”1”をセットする
        /// </summary>
        
        /// <summary>
        /// 未チェックボックスをクリックする時”0”をセットする
        /// </summary>
        
        /// <summary>
        /// Export Data Csv File 
        /// </summary>
        /// <remarks>CSVEXELフィールに出す事</remarks>
        private void ExportCSV()
        {
            if(!ErrorCheck())
            {
                return;
            }
            if (bbl.ShowMessage("Q203") == DialogResult.No)
            {
                return;
            }
            string filePath = "";
            if (!ShowSaveFileDialog(InProgramNM, out filePath))
            {
                return;
            }
            if (dgvPurchaseSearch.DataSource != null)
            {
                //Build the CSV file data as a Comma separated string.
                string csv = string.Empty;

                //LoacalDirectory
                string folderPath = "C:\\CSV\\";
                FileInfo logFileInfo = new FileInfo(folderPath);
                DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);

                if (!logDirInfo.Exists) logDirInfo.Create();

                //Add the Header row for CSV file.
                foreach (DataGridViewColumn column in dgvPurchaseSearch.Columns)
                {
                    //if(column.HeaderText!="")
                    csv += column.HeaderText + ',';
                }
                //Add new line.
                csv += "\r\n";

                //Adding the Rows
                foreach (DataGridViewRow row in dgvPurchaseSearch.Rows)
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

                //Exporting to CSV.            
                File.WriteAllText(folderPath + "仕入照会(仕入先) " + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "csv", csv, Encoding.GetEncoding(932));
                dpurchase_bl.ShowMessage("I203");
                //MessageBox.Show("CSV 出力が完了します。", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                dpurchase_bl.ShowMessage("E138");
               // MessageBox.Show("There is no data for CSV Export", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        protected override void EndSec()
        {
            this.Close();
        }
        private void scSupplier_CodeKeyDownEvent(object sender, KeyEventArgs e)

        {
            if (e.KeyCode == Keys.Enter)
            {
                scSupplier.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scSupplier.TxtCode.Text))
                {
                    if (scSupplier.SelectData())
                    {
                        scSupplier.Value1 = scSupplier.TxtCode.Text;
                        scSupplier.Value2 = scSupplier.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        scSupplier.SetFocus(1);
                    }
                }

            }

        }

        private void scStaff_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scStaff.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scStaff.TxtCode.Text))
                {
                    if (scStaff.SelectData())
                    {
                        scStaff.Value1 = scStaff.TxtCode.Text;
                        scStaff.Value2 = scStaff.LabelText;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        scStaff.SetFocus(1);
                    }
                }
            }
        }

        private void ShiireShoukaiShiiresaki_KeyUp(object sender, KeyEventArgs e)
        {
            if(cb_focus== false)
            { MoveNextControl(e); }
            else
            {
                ComboStore.Focus();
                cb_focus = false;
            }
        }

        private bool ErrorCheck()
        {
            /// <remarks>仕入日(from)は仕入日(To)より大きいの場合エラーになる</remarks>
            if (!string.IsNullOrWhiteSpace(txtPurchaseDateFrom.Text) && !string.IsNullOrWhiteSpace(txtPurchaseDateTo.Text))
            {
                if (string.Compare(txtPurchaseDateFrom.Text, txtPurchaseDateTo.Text) == 1)
                {
                    dpurchase_bl.ShowMessage("E104");
                    txtPurchaseDateTo.Focus();
                    return false;
                }
            }

            /// <remarks>入荷日(from)は入荷日(To)より大きいの場合エラーになる</remarks>
            if (!string.IsNullOrWhiteSpace(txtArrivalDateFrom.Text) && !string.IsNullOrWhiteSpace(txtArrivalDateTo.Text))
            {
                if (string.Compare(txtArrivalDateFrom.Text, txtArrivalDateTo.Text) == 1)
                {
                    dpurchase_bl.ShowMessage("E104");
                    txtArrivalDateTo.Focus();
                    return false;
                }
               
            }

            /// <remarks>支払予定日(from)は支払予定日(To)より大きいの場合エラーになる</remarks>
            if (!string.IsNullOrWhiteSpace(txtPaymentDueDateFrom.Text) && !string.IsNullOrWhiteSpace(txtPaymentDueDateTo.Text))
            {

                if (string.Compare(txtPaymentDueDateFrom.Text, txtPaymentDueDateTo.Text) == 1)
                {
                    dpurchase_bl.ShowMessage("E104");
                    txtPaymentDueDateTo.Focus();
                    return false;
                }
            }

            /// <remarks>仕入先CDが存在しない場合エラーになる</remarks>
            if (!string.IsNullOrEmpty(scSupplier.TxtCode.Text))
            {
                if (!scSupplier.IsExists(2))
                {
                    dpurchase_bl.ShowMessage("E101");
                    scSupplier.SetFocus(1);
                    return false;
                }
            }

            /// <remarks>スタッフCDが存在しない場合エラーになる</remarks>
            if (!string.IsNullOrEmpty(scStaff.TxtCode.Text))
            {
                if (!scStaff.IsExists(2))
                {
                    dpurchase_bl.ShowMessage("E101");
                    scStaff.SetFocus(1);
                    return false;
                }
            }

            /// <remarks>店舗名を選択した場合、権限があるかとかをチェックする</remarks>
            //if (!CboStore_ErrorCheck())
            //{
            //    dpurchase_bl.ShowMessage("E141");
            //    ComboStore.Focus();
            //    return false;
            //}

            if (!RequireCheck(new Control[] { ComboStore }))   // go that focus
                return false;

            if (!base.CheckAvailableStores(ComboStore.SelectedValue.ToString()))
            {
                dpurchase_bl.ShowMessage("E141");
                ComboStore.Focus();
                return false;
            }

            return true;
        }

        private void txtPurchaseDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtPurchaseDateFrom.Text) && !string.IsNullOrEmpty(txtPurchaseDateTo.Text))
                {
                    if (string.Compare(txtPurchaseDateFrom.Text, txtPurchaseDateTo.Text) == 1)
                    {
                        dpurchase_bl.ShowMessage("E104");
                        txtPurchaseDateTo.Focus();
                    }
                }
            }
        }

        private void txtArrivalDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtArrivalDateFrom.Text) && !string.IsNullOrEmpty(txtArrivalDateTo.Text))
                {
                    if (string.Compare(txtArrivalDateFrom.Text, txtArrivalDateTo.Text) == 1 )
                    {
                        dpurchase_bl.ShowMessage("E104");
                        txtArrivalDateTo.Focus();
                    }
                }
            }
        }

        private void txtPaymentDueDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtPaymentDueDateFrom.Text) && !string.IsNullOrEmpty(txtPaymentDueDateTo.Text))
                {
                    if (!string.IsNullOrEmpty(txtPaymentDueDateFrom.Text) && !string.IsNullOrEmpty(txtPaymentDueDateTo.Text))
                    {
                        if (string.Compare(txtPaymentDueDateFrom.Text, txtPaymentDueDateTo.Text) == 1 )
                        {
                            dpurchase_bl.ShowMessage("E104");
                            txtPaymentDueDateTo.Focus();
                        }
                    }
                }
            }
        }

        private void scSupplier_Enter(object sender, EventArgs e)
        {
            scSupplier.Value1 = "1";//仕入先区分：1
            scSupplier.ChangeDate = txtPurchaseDateTo.Text;
        }

        private void ComboStore_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (!base.CheckAvailableStores(ComboStore.SelectedValue.ToString()))
                {
                    dpurchase_bl.ShowMessage("E141");
                    ComboStore.Focus();
                   cb_focus = true;
                }

            }
        }
    }
}
