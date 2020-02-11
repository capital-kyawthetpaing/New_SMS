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
using CKM_Controls;
using System.IO;
using ClosedXML.Excel;
using System.Collections;


namespace NyuukaShoukai
{
    public partial class FrmNyuukaShoukai : FrmMainForm
    {
        NyuukaShoukai_BL nkskbl;
        D_ArrivalPlan_Entity dape = new D_ArrivalPlan_Entity();
        D_Arrival_Entity dae = new D_Arrival_Entity();
        D_Purchase_Entity dpe = new D_Purchase_Entity();
        M_Souko_Entity mse = new M_Souko_Entity();
        M_Vendor_Entity mve = new M_Vendor_Entity();


        DataTable dtSearch = new DataTable();

        public FrmNyuukaShoukai()
        {
            InitializeComponent();
        }
        private void FrmNyuukaShoukai_Load(object sender, EventArgs e)
        {
            InProgramID = "NyuukaShoukai";

            //SetFunctionLabel(EProMode.MENTE);
            StartProgram();
            
            nkskbl = new NyuukaShoukai_BL();

            F2Visible = false;
            F3Visible = false;
            F4Visible = false;
            //Btn_F5.Text = "ｷｬﾝｾﾙ(F5)";
            F5Visible = false;
            F7Visible = false;
            F8Visible = false;          
            Btn_F10.Text = "Excel出力(F10)";
            F12Visible = false;

            BindCombo();
            cboWarehouse.Focus();
            SetRequireField();
            
        }
        private void BindCombo()
        {
            cboWarehouse.Bind(string.Empty,"");
            cboWarehouse.SelectedValue = SoukoCD;
            cboSourceWH.Bind(string.Empty,"");
        }

        private void SetRequireField()
        {
            cboWarehouse.Require(true);        
        }

        /// <summary>
        /// override F1 Button click
        /// </summary>
        protected override void EndSec()
        {
            this.Close();
        }

        /// <summary>
        /// Handle F1 to F12 Click
        /// </summary>
        /// <param name="index"> button index+1, eg.if index is 0,it means F1 click </param>
        public override void FunctionProcess(int index)
        {
            switch (index + 1)
            {
                case 2:
                case 3:                 
                case 4:
                    break;
                case 5:                   
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        Clear();
                        cboWarehouse.Focus();
                    }
                    break;
                case 10:
                    F10();
                    Clear();
                    dgvNyuukaShoukai.DataSource = string.Empty;
                    cboWarehouse.Focus();
                    break;
                case 11:
                    F11();
                    break;
                case 12:                
                    break;
            }
        }

        public void Clear()
        {
            Clear(panelcombo1);
            Clear(panelcombo2);
            txtArrivalDay1.Clear();
            txtArrivalDay2.Clear();
            txtDeliveryNote.Clear();
            txtPurchaseDate1.Clear();
            txtPurchaseDate2.Clear();
            txtProductName.Clear();
            txtStockDate1.Clear();
            txtStockDate2.Clear();
            ScSKUCD.Clear();
            ScItem.Clear();
            ScJanCD.Clear();
            ScSupplier.Clear();
            
        }

        #region DisplayData
        /// <summary>
        /// Display event
        /// </summary>
        /// <param name="type"></param>
        /// 
        private void F11()
        {
            if(ErrorCheck())
            {
                dape = new D_ArrivalPlan_Entity
                {
                    SoukoCD = cboWarehouse.SelectedValue.ToString(),
                    CalcuArrivalPlanDate1 = txtStockDate1.Text,
                    CalcuArrivalPlanDate2 = txtStockDate2.Text,
                    //FrmSoukoCD = cboSourceWH.SelectedValue.ToString(),
                    ITEMCD = ScItem.TxtCode.Text,
                    JanCD = ScJanCD.TxtCode.Text,
                    SKUCD = ScSKUCD.TxtCode.Text,
                    statusFlg = CheckValue1(),
                    DisplayFlg = CheckValue2(),
                };

                if(cboSourceWH.SelectedValue.ToString () == "-1")
                {
                    dape.FrmSoukoCD = string.Empty;
                }

                dae = new D_Arrival_Entity
                {
                    ArrivalDate1 = txtArrivalDay1.Text,
                    ArrivalDate2 = txtArrivalDay2.Text,
                    PurchaseSu = "0",
                    VendorDeliveryNo = txtDeliveryNote.Text,
                };

                dpe = new D_Purchase_Entity
                {
                   PurchaseDateFrom = txtPurchaseDate1.Text,
                   PurchaseDateTo = txtPurchaseDate2.Text,
                   VendorCD = ScSupplier.TxtCode.Text,
                };

               
                dtSearch = nkskbl.D_ArrivalPlan_Select(dape, dae, dpe);
                dgvNyuukaShoukai.DataSource = dtSearch;
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            F11();
        }
        #endregion

        private void F10()
        {
            if(ErrorCheck())
            {
                if(dtSearch.Rows.Count > 0)
                {
                    DataTable dtExport = dtSearch;
                    dtExport = ChangeDataColumnName(dtExport);
                    SaveFileDialog savedialog = new SaveFileDialog();
                    savedialog.Filter = "Excel Files|*.xlsx;";
                    savedialog.Title = "Save";
                    savedialog.FileName = "ExportFile";
                    savedialog.InitialDirectory = @"C:\";
                    savedialog.RestoreDirectory = true;
                    if (savedialog.ShowDialog() == DialogResult.OK)
                    {
                        if (Path.GetExtension(savedialog.FileName).Contains(".xlsx"))
                        {
                            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
                            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
                            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                            worksheet = workbook.ActiveSheet;
                            worksheet.Name = "ExportedFromDatGrid";

                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dtExport, "test");

                                wb.SaveAs(savedialog.FileName);
                                nkskbl.ShowMessage("I203", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);  //Export Successful
                            }
                        }
                    }
                }
            }
        }

        protected DataTable ChangeDataColumnName(DataTable dt)
        {
            dt.Columns["ArrivalDate"].ColumnName = "入荷日";
            dt.Columns["CalcuArrivalPlanDate"].ColumnName = "入荷予定日";
            dt.Columns["PurchaseDate"].ColumnName = "仕入日";
            dt.Columns["Goods"].ColumnName = "入庫区分";
            dt.Columns["SKUCD"].ColumnName = "SKUCD";
            dt.Columns["JanCD"].ColumnName = "JANCD";
            dt.Columns["SKUName"].ColumnName = "商品名";
            dt.Columns["ColorName"].ColumnName = "カラー";
            dt.Columns["SizeName"].ColumnName = "サイズ";
            dt.Columns["ArrivalPlanSu"].ColumnName = "予定数";
            dt.Columns["ArrivalSu"].ColumnName = "入荷数";
            dt.Columns["VendorName"].ColumnName = "仕入先";
            dt.Columns["SoukoName"].ColumnName = "移動元倉庫";
            dt.Columns["Directdelivery"].ColumnName = "直送";
            dt.Columns["ReserveNumber"].ColumnName = "受注番号";
            dt.Columns["Number"].ColumnName = "発注番号";
            dt.Columns["ArrivalNO"].ColumnName = "入荷番号";
            dt.Columns["PurchaseNO"].ColumnName = "仕入番号";
            dt.Columns["VendorDeliveryNo"].ColumnName = "納品書番号";

            dt.Columns.RemoveAt(2);
            return dt;
        }

        public string CheckValue1()
        {
            string chk = string.Empty;

            if (statusChk1.Checked && statusChk2.Checked)
            {
                return string.Empty;
            }
            else
            {
                chk = statusChk1.Checked ? "0" : "1";
                return chk;
            }
        }

        public string CheckValue2()
        {
            string chk1 = string.Empty;

            if (chkDelivery.Checked && ChkArrival.Checked)
            {
                return string.Empty;
            }
            else
            {
                chk1 = chkDelivery.Checked ? "0" : "1";
                return chk1;
            }
        }

        #region ErrorCheck 
        private bool ErrorCheck()
        {
            if(cboWarehouse.SelectedValue.ToString() == "-1")
            {
                nkskbl.ShowMessage("E102");
                cboWarehouse.Focus();
                return false;
            }
            else
            {
                mse.DeleteFlg = "0";
                DataTable dtsouko = new DataTable();
                dtsouko = nkskbl.D_Souko_Select(mse);
                if (dtsouko.Rows.Count == 0)
                {
                    nkskbl.ShowMessage("E128");
                    cboWarehouse.Focus();
                    return false;
                }

            }

            if (!string.IsNullOrWhiteSpace(txtArrivalDay2.Text))
            {
                int result = txtArrivalDay1.Text.CompareTo(txtArrivalDay2.Text);
                if (result >= 0)
                {
                    nkskbl.ShowMessage("E104");
                    txtArrivalDay2.Focus();
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtStockDate2.Text))
            {
                int stock = txtStockDate1.Text.CompareTo(txtStockDate2.Text);
                if (stock >= 0)
                {
                    nkskbl.ShowMessage("E104");
                    txtArrivalDay2.Focus();
                    return false;
                }
            }

            if (!statusChk1.Checked && !statusChk2.Checked)
            {
                nkskbl.ShowMessage("E111");
                statusChk1.Focus();
                return false;
            }
           

            if (!string.IsNullOrWhiteSpace(txtPurchaseDate2.Text))
            {
                int purchase = txtPurchaseDate1.Text.CompareTo(txtPurchaseDate2.Text);
                if (purchase >= 0)
                {
                    nkskbl.ShowMessage("E104");
                    txtPurchaseDate2.Focus();
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(txtArrivalDay1.Text) || (string.IsNullOrWhiteSpace(txtArrivalDay2.Text)))
            {
                if(string.IsNullOrWhiteSpace(txtStockDate1.Text) || (string.IsNullOrWhiteSpace(txtStockDate2.Text)))
                {
                    if (string.IsNullOrWhiteSpace(txtPurchaseDate1.Text) || (string.IsNullOrWhiteSpace(txtPurchaseDate2.Text)))
                    {
                        nkskbl.ShowMessage("E188");
                        txtPurchaseDate2.Focus();
                        return false;
                    }
                }
            }

            if (!chkDelivery.Checked && !ChkArrival.Checked )
            {
                 nkskbl.ShowMessage("E111");
                 chkDelivery.Focus();
                 return false;
            }
            
            if (!string.IsNullOrEmpty(ScSupplier.TxtCode.Text))
            {
                mve.VendorCD = ScSupplier.TxtCode.Text;
                if (string.IsNullOrWhiteSpace(txtArrivalDay2.Text))
                {
                    mve.ChangeDate = DateTime.Today.ToShortDateString();
                }
                else
                {
                    mve.ChangeDate = txtArrivalDay2.Text;
                }
                DataTable dtsupplier = new DataTable();
                dtsupplier = nkskbl.M_Vendor_DataSelect(mve);
                if (dtsupplier.Rows.Count > 0)
                {
                    ScSupplier.LabelText = dtsupplier.Rows[0]["VendorName"].ToString();
                }
                else
                {
                    nkskbl.ShowMessage("E101");
                    ScSupplier.SetFocus(1);
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace (ScItem.TxtCode.Text))
            {
                if(ScItem.TxtCode.Text.Contains(","))
                {
                    string a = ScItem.TxtCode.Text;
                    string[] arr = a.Split(',');
                    for(int i = 1; i <arr.Length; i ++)
                    {
                        if(i >= 7)
                        {
                            nkskbl.ShowMessage("E187");
                            ScItem.SetFocus(1);
                            return false;
                        }

                    }

                }
            }

            if (!string.IsNullOrWhiteSpace(ScSKUCD.TxtCode.Text))
            {
                if (ScSKUCD.TxtCode.Text.Contains(","))
                {
                    string a = ScSKUCD.TxtCode.Text;
                    string[] arr = a.Split(',');
                    for (int i = 1; i < arr.Length; i++)
                    {
                        if (i >= 7)
                        {
                            nkskbl.ShowMessage("E187");
                            ScSKUCD.SetFocus(1);
                            return false;
                        }
                        //else
                        //{
                        //    ScSKUCD.TxtCode.Text = "'";
                        //}
                    }

                }
            }

            if (!string.IsNullOrWhiteSpace(ScJanCD.TxtCode.Text))
            {
                if (ScJanCD.TxtCode.Text.Contains(","))
                {
                    string a = ScJanCD.TxtCode.Text;
                    string[] arr = a.Split(',');
                    for (int i = 1; i < arr.Length; i++)
                    {
                        if (i >= 13)
                        {
                            nkskbl.ShowMessage("E187");
                            ScJanCD.SetFocus(1);
                            return false;
                        }

                    }

                }
            }

            //if(cboSourceWH.SelectedValue.ToString () == "-1")
            //{
            //    cboSourceWH.SelectedValue = string.Empty;
            //}

            return true;
        }
        #endregion
        private void FrmNyuukaShoukai_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void txtArrivalDay2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtArrivalDay2.Text))
                {
                    int result = txtArrivalDay1.Text.CompareTo(txtArrivalDay2.Text);
                    if (result >= 0)
                    {
                        nkskbl.ShowMessage("E104");
                        txtArrivalDay2.Focus();
                    }
                }
                
            }
        }

        private void txtStockDate2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtStockDate2.Text))
                {
                    int result = txtStockDate1.Text.CompareTo(txtStockDate2.Text);
                    if (result >= 0)
                    {
                        nkskbl.ShowMessage("E104");
                        txtStockDate2.Focus();
                    }
                }

            }
        }

        private void txtPurchaseDate2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtPurchaseDate2.Text))
                {
                    int result = txtPurchaseDate1.Text.CompareTo(txtPurchaseDate2.Text);
                    if (result >= 0)
                    {
                        nkskbl.ShowMessage("E104");
                        txtPurchaseDate2.Focus();
                    }
                }

            }
        }
        
        private void ChkArrival_CheckedChanged(object sender, EventArgs e)
        {
            if (!ChkArrival.Checked)
            {
                cboSourceWH.Enabled = false;
            }
            else
            {
                cboSourceWH.Enabled = true;
            }
        }

        private void chkDelivery_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkDelivery.Checked)
            {
                ScSupplier.Enabled = false;            
            }
            else
            {
                ScSupplier.Enabled = true;
                ScSupplier.SearchEnable = true;
            }
        }
    
        private void cboWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cboWarehouse.SelectedValue.ToString()))
            {
                mse.DeleteFlg = "0";
                DataTable dtsouko = new DataTable();
                dtsouko = nkskbl.D_Souko_Select(mse);
                if (dtsouko.Rows.Count == 0)
                {
                    nkskbl.ShowMessage("E128");
                    cboWarehouse.Focus();                  
                }

            }
        }

        private void cboWarehouse_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(cboWarehouse.SelectedValue.ToString()))
                {
                    mse.DeleteFlg = "0";
                    DataTable dtsouko = new DataTable();
                    dtsouko = nkskbl.D_Souko_Select(mse);
                    if (dtsouko.Rows.Count == 0)
                    {
                        nkskbl.ShowMessage("E128");
                        cboWarehouse.Focus();
                    }

                }
            }
        }    

        private void ScSupplier_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                if (!string.IsNullOrEmpty(ScSupplier.TxtCode.Text))
                {
                    mve.VendorCD = ScSupplier.TxtCode.Text;
                    if (string.IsNullOrWhiteSpace(txtArrivalDay2.Text))
                    {
                        mve.ChangeDate = DateTime.Today.ToShortDateString();
                    }
                    else
                    {
                        mve.ChangeDate = txtArrivalDay2.Text;
                    }
                    DataTable dtsupplier = new DataTable();
                    dtsupplier = nkskbl.M_Vendor_DataSelect(mve);
                    if (dtsupplier.Rows.Count > 0)
                    {
                        ScSupplier.LabelText = dtsupplier.Rows[0]["VendorName"].ToString();   
                    }
                    else
                    {
                        nkskbl.ShowMessage("E101");
                        ScSupplier.SetFocus(1);
                    }
                }

            }
        }
    }
}
