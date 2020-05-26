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
using Base.Client;
using System.IO;
using ClosedXML.Excel;
using Entity;

namespace ShukkaShoukai
{
    public partial class FrmShukkaShoukai :FrmMainForm
    {
        ShukkaShoukai_BL skskbl = new ShukkaShoukai_BL();
        M_Souko_Entity mse = new M_Souko_Entity();
        D_Juchuu_Entity dje = new D_Juchuu_Entity();
        M_StoreAuthorizations_Entity msae = new M_StoreAuthorizations_Entity();
        D_ShippingDetails_Entity dsde = new D_ShippingDetails_Entity();
        D_Shipping_Entity mshe = new D_Shipping_Entity();
        D_Instruction_Entity die = new D_Instruction_Entity();

        DataTable dtSearch = new DataTable();
        public FrmShukkaShoukai()
        {
            InitializeComponent();
        }

        private void FrmShukkaShoukai_Load(object sender, EventArgs e)
        {
            InProgramID = "ShukkaShoukai";

            StartProgram();

            F2Visible = false;
            F3Visible = false;
            F4Visible = false;
            F5Visible = false;
            F7Visible = false;
            F8Visible = false;
            Btn_F10.Text = "出力(F10)";
            F12Visible = false;
            BindCombo();
            cboWarehouse.Focus();
            SetRequiredField();

            SC_Order.Value1 = InOperatorCD;
            SC_Order.Value2 = StoreCD;
           
        }

        public void BindCombo()
        {
            cboWarehouse.Bind(string.Empty,"");
            //cboWarehouse.SelectedValue = SoukoCD;
            cboShipping.Bind(string.Empty,"");
            cboDestinationWarehouse.Bind(string.Empty,"");
        }
        public void SetRequiredField()
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
                    //Clear();
                    //dgvShukkaShoukai.DataSource = string.Empty;
                    //cboWarehouse.Focus();
                    break;
                case 11:
                    F11();
                    break;
                case 12:
                    break;
            }
        }


        #region Checkbox check or not
        public string CheckValue1()
        {
            string chk = string.Empty;

            if (chkShipmentOfSale.Checked && chkShipmentOfTransfer.Checked)
            {
                return string.Empty;
            }
            else
            {
                chk = chkShipmentOfSale.Checked ? "0" : "1";
                return chk;
            }
        }

        public string CheckValue2()
        {
            string chk1 = string.Empty;

            if(chkAlready.Checked && chkNot .Checked )
            {
                return string.Empty;
            }
            else
            {
                chk1 = chkAlready.Checked ? "0" : "1";
                return chk1;
            }
        }
        #endregion

        /// <summary>
        /// Clear Data 
        /// </summary>
        public void Clear()
        {
            Clear(PanelHeader);
            BindCombo();
            cboWarehouse.Focus();
            dgvShukkaShoukai.DataSource = string.Empty;
            chkAlready.Checked = true;
            chkNot.Checked = true;
            chkShipmentOfSale.Checked = true;
            chkShipmentOfTransfer.Checked = true;
            //Clear(panelDetail);
        }

        #region Display Data
        public bool ErrorCheck()
        {
            if (cboWarehouse.SelectedValue.ToString() == "-1")
            {
                skskbl.ShowMessage("E102");
                cboWarehouse.Focus();
                return false;
            }
            else
            {
                //mse.DeleteFlg = "0";
                //DataTable dtsouko = new DataTable();
                //dtsouko = nkskbl.D_Souko_Select(mse);
                //if (dtsouko.Rows.Count == 0)
                //{
                //    nkskbl.ShowMessage("E128");
                //    cboWarehouse.Focus();
                //    return false;
                //}
            }

            if (!string.IsNullOrWhiteSpace(txtShippingEndDate.Text))
            {
                int result = txtShippingStartDate.Text.CompareTo(txtShippingEndDate.Text);
                if (result > 0)
                {
                    skskbl.ShowMessage("E104");
                    txtShippingEndDate.Focus();
                    return false;
                }
            }

            if (!chkShipmentOfSale.Checked && !chkShipmentOfTransfer.Checked)
            {
                skskbl.ShowMessage("E111");
                chkShipmentOfSale.Focus();
                return false;
            }

            dje.JuchuuNO = SC_Order.TxtCode.Text;
            DataTable dtJuChuu = new DataTable();
            dtJuChuu = skskbl.D_Juchuu_DataSelect_ForShukkaShoukai(dje);
            if (dtJuChuu.Rows.Count == 0)
            {
                skskbl.ShowMessage("E138");
                SC_Order.Focus();
                return false;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(dtJuChuu.Rows[0]["DeleteDateTime"].ToString()))
                {
                    skskbl.ShowMessage("E140");
                    SC_Order.Focus();
                    return false;
                }
                else
                {
                    msae.StoreCD = dtJuChuu.Rows[0]["StoreCD"].ToString();
                    msae.StoreAuthorizationsCD = StoreAuthorizationsCD;
                    msae.ChangeDate = StoreAuthorizationsChangeDate;
                    //msae.ProgramID = "0";
                    DataTable dtAuthorization = new DataTable();
                    dtAuthorization = skskbl.M_StoreAuthorizations_Select(msae);
                    if (dtAuthorization.Rows.Count == 0)
                    {
                        skskbl.ShowMessage("E139");
                        SC_Order.Focus();
                        return false;
                    }
                }
            }

            if (!chkAlready.Checked && !chkNot.Checked)
            {
                skskbl.ShowMessage("E111");
                chkAlready.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(Sc_Item.TxtCode.Text))
            {
                if (Sc_Item.TxtCode.Text.Contains(","))
                {
                    string a = Sc_Item.TxtCode.Text;
                    string[] arr = a.Split(',');
                    if (arr.Length >= 10)
                    {
                        skskbl.ShowMessage("E187");
                        Sc_Item.SetFocus(1);
                        return false;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(Sc_SKUCD.TxtCode.Text))
            {
                if (Sc_SKUCD.TxtCode.Text.Contains(","))
                {
                    string a = Sc_SKUCD.TxtCode.Text;
                    string[] arr = a.Split(',');
                    if (arr.Length >= 10)
                    {
                        skskbl.ShowMessage("E187");
                        Sc_SKUCD.SetFocus(1);
                        return false;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(SC_JanCD.TxtCode.Text))
            {
                if (SC_JanCD.TxtCode.Text.Contains(","))
                {
                    string a = SC_JanCD.TxtCode.Text;
                    string[] arr = a.Split(',');
                    if (arr.Length >= 10)
                    {
                        skskbl.ShowMessage("E187");
                        SC_JanCD.SetFocus(1);
                        return false;
                    }
                }
            }
            return true;
        }
        public void F11()
         {
            if (ErrorCheck())
            {
                mshe = new D_Shipping_Entity
                {
                    SoukoCD = cboWarehouse.SelectedValue.ToString(),
                    ShippingDateFrom = txtShippingStartDate.Text,
                    ShippingDateTo = txtShippingEndDate.Text,
                    CarrierCD = cboShipping.SelectedValue.ToString(),
                    ShippingKBN = CheckValue1(),
                    InvoiceNO = CheckValue2(),
                };

                dsde = new D_ShippingDetails_Entity
                {
                    Number = SC_Order.TxtCode.Text ,
                    SKUCD = Sc_SKUCD .TxtCode.Text ,
                    JanCD = SC_JanCD .TxtCode .Text ,
                    ItemCD = Sc_Item.TxtCode.Text ,
                };

                die = new D_Instruction_Entity
                {
                    DeliverySoukoCD = cboDestinationWarehouse.SelectedValue.ToString(),
                };

                dtSearch = skskbl.D_Shipping_Select(mshe, dsde, die);
                dgvShukkaShoukai.DataSource = dtSearch;
            }
        }
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            F11();
        }
        #endregion

        /// <summary>
        /// to export Excel File 
        /// </summary>
        private void F10()
        {
            if (ErrorCheck())
            {
                if (dtSearch.Rows.Count > 0)
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
                                skskbl.ShowMessage("I203", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);  //Export Successful
                            }
                        }
                        Clear();
                        dgvShukkaShoukai.DataSource = string.Empty;
                        cboWarehouse.Focus();
                    }
                }
            }
        }

        protected DataTable ChangeDataColumnName(DataTable dt)
        {
            dt.Columns["ShippingNO"].ColumnName = "出荷番号";
            dt.Columns["Movement"].ColumnName = "移動区分";
            dt.Columns["ShippingDate"].ColumnName = "出荷日";
            dt.Columns["DeliveryName"].ColumnName = "出荷先";
            dt.Columns["SKUCD"].ColumnName = "SKUCD";
            dt.Columns["JanCD"].ColumnName = "JANCD";
            dt.Columns["SKUName"].ColumnName = "商品名";
            dt.Columns["ColorSize"].ColumnName = "カラー.サイズ";
            dt.Columns["CommentOutStore"].ColumnName = "備考";
            dt.Columns["ShippingSu"].ColumnName = "出荷数";
            dt.Columns["OrderNumber"].ColumnName = "受注番号";
            dt.Columns["CarrierName"].ColumnName = "運送会社";
            dt.Columns["SalesDate"].ColumnName = "売上日";
            dt.Columns["StaffName"].ColumnName = "入力スタッフ";
            dt.Columns.RemoveAt(2);
            return dt;
        }
      
        private void chkShipmentOfTransfer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShipmentOfTransfer.Checked == false)
            {
                cboDestinationWarehouse.Enabled = false;
            }
            else
            {
                cboDestinationWarehouse.Enabled = true;
            }
        }
        
        #region Key Event
        private void FrmShukkaShoukai_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void txtShippingEndDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtShippingEndDate.Text))
                {
                    int result = txtShippingStartDate.Text.CompareTo(txtShippingEndDate.Text);
                    if (result > 0)
                    {
                        skskbl.ShowMessage("E104");
                        txtShippingEndDate.Focus();
                    }
                }
            }
        }

        private void SC_Order_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dje.JuchuuNO = SC_Order.TxtCode.Text;
                DataTable dtJuChuu = new DataTable();
                dtJuChuu = skskbl.D_Juchuu_DataSelect_ForShukkaShoukai(dje);
                if (dtJuChuu.Rows.Count == 0)
                {
                    skskbl.ShowMessage("E138");
                    SC_Order.Focus();
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(dtJuChuu.Rows[0]["DeleteDateTime"].ToString()))
                    {
                        skskbl.ShowMessage("E140");
                        SC_Order.Focus();
                    }
                    else
                    {
                        msae.StoreCD = dtJuChuu.Rows[0]["StoreCD"].ToString();
                        msae.StoreAuthorizationsCD = StoreAuthorizationsCD;
                        msae.ChangeDate = StoreAuthorizationsChangeDate;
                        //msae.ProgramID = "0";
                        DataTable dtAuthorization = new DataTable();
                        dtAuthorization = skskbl.M_StoreAuthorizations_Select(msae);
                        if (dtAuthorization.Rows.Count == 0)
                        {
                            skskbl.ShowMessage("E139");
                            SC_Order.Focus();
                        }
                    }
                }
            }
        }
        #endregion

    }
}
