using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using BL;
using ExcelDataReader;
using Entity;

namespace MasterTorikomi_M_CustomerSKUPrice
{
    public partial class MasterTorikomi_M_CustomerSKUPrice : FrmMainForm
    {
        ITEM_BL ibl;
        Base_BL bbl;
        MasterTorikomi_SKU_BL mtbl;
        M_SKUInitial_BL msIbl;
        SKU_BL skubl;
        DataTable dtBrand = new DataTable();
        DataTable dtMultiP = new DataTable();
        DataTable dtVendor = new DataTable();
        DataTable dtskuintial = new DataTable();
        DataTable dtMessage = new DataTable();
        DataTable dtSKU = new DataTable();
        DataTable dtCustomer = new DataTable();
        M_Customer_Entity ce;
        Customer_BL cbl;
        public MasterTorikomi_M_CustomerSKUPrice()
        {
            InitializeComponent();
        }
        //得意先別商品別単価マスタ取込
        private void MasterTorikomi_M_CustomerSKUPrice_Load(object sender, EventArgs e)
        {
            ibl = new ITEM_BL();
            bbl = new Base_BL();
            mtbl = new MasterTorikomi_SKU_BL();
            msIbl = new M_SKUInitial_BL();
            skubl = new SKU_BL();
            InProgramID = "MasterTorikomi_M_CustomerSKUPrice";
            StartProgram();
            FalseKey();

            dtBrand = mtbl.M_Brand_SelectAll_NoPara();
            dtMultiP = mtbl.M_Multipurpose_SelectAll();
            dtVendor = mtbl.M_Vendor_SelectAll();
            dtskuintial = msIbl.M_SKUInitial_SelectAll();
            dtMessage = msIbl.M_MessageSelectAll();
         
              ce = new M_Customer_Entity() { ChangeDate = bbl.GetDate(),   } ;
              cbl      = new Customer_BL();
            dtCustomer = cbl.M_Customer_Select_byCustomerSKUprice(ce);
            this.ModeVisible = false;
            this.Text = "得意先別商品別単価マスタ取込";
            Btn_F12.Text = "取込(F12)";
        }
        private void FalseKey()
        {
            F2Visible = F3Visible = F4Visible = F5Visible = F7Visible = F8Visible = F9Visible = F10Visible = F11Visible = false;
        }
        private void MasterTorikomi_M_CustomerSKUPrice_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        private DataTable ExcelToDatatable(string filePath)
        {
            try
            {
                FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

                string ext = Path.GetExtension(filePath);
                IExcelDataReader excelReader;
                if (ext.Equals(".xls"))
                    //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                else if (ext.Equals(".xlsx"))
                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                else
                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx) 
                    excelReader = ExcelReaderFactory.CreateCsvReader(stream, null);

                //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                bool useHeaderRow = true;

                DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = useHeaderRow,
                    }
                });
                excelReader.Close();
                return result.Tables[0];
            }
            catch
            {
                bbl.ShowMessage("E137");
                return null;
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            //if (bbl.ShowMessage("Q001") == DialogResult.Yes)
            //{
            gvItem.DataSource = null;
            string filePath = string.Empty;
            string fileExt = string.Empty;
            if (!System.IO.Directory.Exists("C:\\SMS\\MasterShutsuryoku_CustomerSKUPrice"))
            {
                System.IO.Directory.CreateDirectory("C:\\SMS\\MasterShutsuryoku_CustomerSKUPrice");
            }
            OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file 
                                                        // file.InitialDirectory = "C:\\SMS\\TenzikaiShouhin\\";
            file.InitialDirectory = "C:\\SMS\\MasterShutsuryoku_CustomerSKUPrice";                             // file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
            {

                Cursor = Cursors.WaitCursor;
                try
                {
                    filePath = file.FileName; ; //get the path of the file  
                    fileExt = Path.GetExtension(filePath); //get the file extension  
                    if (!(fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0))
                    {
                        bbl.ShowMessage("E137");
                        return;
                    }
                    inputPath.Text = filePath;
                    DataTable dt = new DataTable();
                    dt = ExcelToDatatable(filePath);
                    if (dt != null)
                    {
                        if (ErrorCheck(dt))
                        {
                            ExcelErrorCheck(dt);
                            if (dt != null)
                            {
                                gvItem.DataSource = null;
                                gvItem.DataSource = dt;
                            }

                        }
                        else
                        {
                            inputPath.Focus();
                        }
                        Cursor = Cursors.Default; ;
                    }
                    else
                    {
                        MessageBox.Show("No row data was found or import excel is opening in different location");
                        Cursor = Cursors.Default;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                Cursor = Cursors.Default;
            }
            //}

        }
        private bool ErrorCheck(DataTable dt)
        {
            if (String.IsNullOrEmpty(inputPath.Text))
            {
                MessageBox.Show("E121");
                return false;
            }
            if (dt.Columns.Count != 12)
            {
                bbl.ShowMessage("E137");
                return false;
            }

            if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
            {
                if (dt.Rows[1]["データ区分"].ToString() != "1")
                {
                    bbl.ShowMessage("E137");
                    return false;
                }
            }
            return true;
        }
        public override void FunctionProcess(int index)
        {
            switch (index + 1)
            {
                case 0: // F1:終了
                    {
                        break;
                    }
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        Cancel();
                    }
                    break;
                case 11:
                    break;
                case 12:
                    F12();
                    break;
            }
        }
        protected override void EndSec()
        {
            this.Close();
        }
        private bool CheckPartial(DataTable dt)
        {
            var query = "Error <> ''";
            if (dt.Select(query).Count() > 0)
                return false;
            return true;
        }
        private void F12()
        {
            try
            {
                ibl = new ITEM_BL();
                if (bbl.ShowMessage("Q101") == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    if (String.IsNullOrEmpty(inputPath.Text))
                    {
                        bbl.ShowMessage("E121");
                        inputPath.Focus();
                        Cursor = Cursors.Default;
                        return;
                    }
                    var dt = gvItem.DataSource as DataTable;
                    if (dt == null)
                    {
                        MessageBox.Show("Please import first");
                        Cursor = Cursors.Default;
                        return;
                    }
                    //if (ErrorCheck(dt))
                    //{
                    if (CheckPartial(dt))
                    {
                        dt.Columns["データ区分"].ColumnName = "KBN";
                        dt.Columns["顧客CD"].ColumnName = "CustomerCD";
                        dt.Columns["顧客名称"].ColumnName = "CustomerName";
                        dt.Columns["適用開始日"].ColumnName = "StartDate";
                        dt.Columns["適用終了日"].ColumnName = "EndDate";
                        dt.Columns["税抜単価"].ColumnName = "SalePOT";
                        dt.Columns["備考"].ColumnName = "Remark";
                        dt.Columns["削除FLG"].ColumnName = "DeleteFlg";
                        dt.Columns["商品名"].ColumnName = "ShouhinName";

                        dt.Columns.Add("PC"); dt.Rows[0]["PC"] = Environment.MachineName;
                        dt.Columns.Add("Operator"); dt.Rows[0]["Operator"] = InOperatorCD;
                        dt.Columns.Add("ProgramID"); dt.Rows[0]["ProgramID"] = "MasterTorikomi_M_CustomerSKUPrice";
                        dt.Columns.Add("Key"); dt.Rows[0]["Key"] = inputPath.Text;
                        var xml = bbl.DataTableToXml(dt);
                        var res = ibl.ImportCustomerItem(xml);
                        if (res)
                        {
                            bbl.ShowMessage("I101");
                            Cancel();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Import");   // Changed please
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please fix the error of the imported file shown on screen and then try to import. . .");
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
            Cursor = Cursors.Default;
        }
        private void Cancel()
        {
            Cursor = Cursors.WaitCursor;

            try
            {

                inputPath.Clear();
                gvItem.DataSource = null;
                gvItem.Refresh();
                dtBrand = mtbl.M_Brand_SelectAll_NoPara();
                dtMultiP = mtbl.M_Multipurpose_SelectAll();
                dtVendor = mtbl.M_Vendor_SelectAll();
                dtskuintial = msIbl.M_SKUInitial_SelectAll();
                dtMessage = msIbl.M_MessageSelectAll();
                dtCustomer = cbl.M_Customer_Select_byCustomerSKUprice(ce);

                button1.Focus();
                            }
            catch
            {

            }
            Cursor = Cursors.Default; ;
        }
        private void BT_Torikomi_Click(object sender, EventArgs e)
        {
            F12();
        }
        private async Task UpdateText(Control c, string t)
        {
            await Task.Run(() =>
            {
                if (c.InvokeRequired)
                {
                    UpdateControl(c, t);
                }
            });
        }
        private bool ControlInvokeRequired(Control c, Action a)
        {
            if (c.InvokeRequired)
            {
                c.Invoke(new MethodInvoker(delegate { a(); }));
            }
            else
            {
                return false;
            }
            return true;
        }
        private void UpdateControl(Control c, string s)
        {
            try
            {
                if (ControlInvokeRequired(c, () => UpdateControl(c, s)))
                {
                    c.Text = s;
                    return;
                }
                c.Text = s;
                c.Update();
            }
            catch { }
        }
        private async void ExcelErrorCheck(DataTable dt)
        {
            dt.Columns.Add("EItem");
            dt.Columns.Add("Error");
            dt.Columns.Add("Cusotmer");
            //dt.Columns.Add("SKUCD");
            //dt.Columns.Add("JANCD");
            dt.Columns.Add("AppDate");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Color");
            dt.Columns.Add("Size");
            dt.Columns.Add("ItemMakerCD");
            dt.Columns.Add("ItemDate");
            string[] Cols = "データ区分,顧客CD,顧客名称,適用開始日,適用終了日,AdminNO,JANCD,SKUCD,商品名,税抜単価,備考,削除FLG".Split(',');
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                await UpdateText(label2, i.ToString());
                try
                {
                    if (!Is102(dt.Rows[i]["データ区分"].ToString()))
                    {
                        dt.Rows[i]["EItem"] = "データ区分";
                        dt.Rows[i]["Error"] = "E102";
                        goto SkippedLine;
                    }
                    if (!Is190(dt.Rows[i]["データ区分"].ToString(), true))
                    {
                        dt.Rows[i]["EItem"] = "データ区分";
                        dt.Rows[i]["Error"] = "E190";
                        goto SkippedLine;
                    }

                }
                catch { }
                try
                {
                    if (!Is102(dt.Rows[i]["顧客CD"].ToString()))
                    {
                        dt.Rows[i]["EItem"] = "顧客CD";
                        dt.Rows[i]["Error"] = "E102";
                        goto SkippedLine;
                    }
                    if (!Is101("M_Customer", dt.Rows[i]["顧客CD"].ToString()))
                    {
                        dt.Rows[i]["EItem"] = "顧客CD";
                        dt.Rows[i]["Error"] = "E101";
                        goto SkippedLine;
                    }

                }
                catch { }
                try
                {
                    if (!Is102(dt.Rows[i]["適用開始日"].ToString()))
                    {
                        dt.Rows[i]["EItem"] = "適用開始日";
                        dt.Rows[i]["Error"] = "E102";
                        goto SkippedLine;
                    }
                    if (!Is103(dt.Rows[i]["適用開始日"].ToString()))
                    {
                        dt.Rows[i]["EItem"] = "適用開始日";
                        dt.Rows[i]["Error"] = "E103";
                        goto SkippedLine;
                    }

                }
                catch { }
                try
                {
                    if (!Is103(dt.Rows[i]["適用開始日"].ToString()))
                    {
                        dt.Rows[i]["EItem"] = "適用開始日";
                        dt.Rows[i]["Error"] = "E103";
                        goto SkippedLine;
                    }
                }
                catch { }
                try
                {
                    if (!Is102(dt.Rows[i]["AdminNo"].ToString()))
                    {
                        dt.Rows[i]["EItem"] = "AdminNo";
                        dt.Rows[i]["Error"] = "E102";
                        goto SkippedLine;
                    }
                    if (!Is101("M_SKU", dt.Rows[i]["AdminNo"].ToString(), dt.Rows[i]["SKUCD"].ToString()))
                    {
                        dt.Rows[i]["EItem"] = "AdminNo";
                        dt.Rows[i]["Error"] = "E101";
                        goto SkippedLine;
                    }
                }
                catch { }
                try
                {
                    if (!Is102(dt.Rows[i]["JANCD"].ToString()))
                    {
                        dt.Rows[i]["EItem"] = "JANCD";
                        dt.Rows[i]["Error"] = "E102";
                        goto SkippedLine;
                    }
                }
                catch { }
                try
                {
                    if (!Is102(dt.Rows[i]["SKUCD"].ToString()))
                    {
                        dt.Rows[i]["EItem"] = "SKUCD";
                        dt.Rows[i]["Error"] = "E102";
                        goto SkippedLine;
                    }
                    if (!Is101("M_SKU", dt.Rows[i]["AdminNo"].ToString(), dt.Rows[i]["SKUCD"].ToString()))
                    {
                        dt.Rows[i]["EItem"] = "SKUCD";
                        dt.Rows[i]["Error"] = "E101";
                        goto SkippedLine;
                    }
                }
                catch { }
                try
                {
                    IsNoB(dt, i, "税抜単価", "0");
                }
                catch { }
                try
                {
                    IsNoB(dt, i, "削除FLG", "0");
                }
                catch { }
            SkippedLine:
                string col = "";
                string siz = "";
                string val = CS(dt.Rows[i]["AdminNo"].ToString());
                if (val != "0")
                {
                    col = val.Split(' ').First();
                    siz = val.Split(' ').Last();
                }

                dt.Rows[i]["Cusotmer"] = dt.Rows[i]["顧客名称"].ToString();
                dt.Rows[i]["ItemName"] = dt.Rows[i]["商品名"].ToString();
                dt.Rows[i]["AppDate"] = dt.Rows[i]["適用開始日"].ToString();
                dt.Rows[i]["Color"] = col;
                dt.Rows[i]["Size"] = siz;
                if (dt != null)
                {
                    gvItem.DataSource = dt;
                }
                try
                {
                    label2.Visible = false;
                }
                catch { }
            }
        }
        public string CS(string Admino)
        {
            
            var dt= skubl.M_SKU_CS_Select(Admino);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["ColorNo"].ToString() + " "+dt.Rows[0]["SizeNo"].ToString();
            }
            return "0";
        }
        private bool Is102(string value)
        {
            if (!string.IsNullOrEmpty(value.Trim()))
            {
                return true;
            }
            return false;
        }
        private void IsNoB(DataTable dt, int i, string col, string Value = null)
        {
            if (string.IsNullOrEmpty(dt.Rows[i][col].ToString()))
            {
                dt.Rows[i][col] = (Value == null) ? "0" : Value;
            }
        }
        private bool Is103(string date)  // date
        {
            if (String.IsNullOrEmpty(date.Trim()))
                return true;
            return bbl.CheckDate(bbl.FormatDate(date.Contains(" ") ? date.Split(' ').First() : date));
        }
        private bool Is101(string tableName, string param, string paramID = null)  // Master
        {
            var data = new DataTable();

            if (paramID == null)
            {
                if (string.IsNullOrEmpty(param.Trim()) || param.Trim() == "")
                {
                    return true;
                }
                if (tableName == "M_Vendor")
                {
                    string query = " VendorCD = '" + param + "'";
                    var result = dtVendor.Select(query);
                    return (result.Count() > 0);
                    //data = bbl.SimpleSelect1("75", bbl.GetDate(), param);
                }
                if (tableName == "M_Brand")
                {
                    string query = " BrandCD = '" + param + "'";
                    var result = dtBrand.Select(query);
                    return (result.Count() > 0);

                    //data = bbl.SimpleSelect1("56", bbl.GetDate(), param);
                }

                if (tableName == "M_Customer")
                {
                    string query = " CustomerCD = '" + param.Trim() + "'";
                    return (dtCustomer.Select(query).Count() > 0);
                }

            }
            else if (paramID != null)
            {
                if (string.IsNullOrEmpty(param.Trim()) || param.Trim() == "")
                {
                    return true;
                }
                if (tableName == "M_MultiPorpose")
                {
                    string str = " [Key] ='" + param + "'" +
                           "and ID='" + paramID + "'";
                    string str_sec = " [Char1] ='" + param + "'" +
                           "and ID='" + paramID + "'";
                    var result = dtMultiP.Select(str);
                    if (result.Count() > 0)
                    {

                        return (result.Count() > 0);
                    }
                    else
                    {
                        var res = dtMultiP.Select(str_sec);
                        return (res.Count() > 0);

                    }

                    //data = bbl.SimpleSelect1("42", bbl.GetDate(), paramID, param);
                }
                else if (tableName == "M_SKU")
                {
                    string str = " [AdminNo] ='" + param + "'" +
                        " and [SKUCD]='" + paramID + "'";
                   // var result = dtSKU.Select(str);
                        
                    dtSKU = skubl.M_SKU_Select_byCustomerSKUPrice(new M_SKU_Entity() { AdminNO= param, SKUCD = paramID, ChangeDate = bbl.GetDate() });
                    return (dtSKU.Rows.Count > 0);
                }
                // return (data.Rows.Count > 0);
            }

            return false;
        }
        List<string> Liststr = new List<string>();
        private bool Is190(string value, bool IsDataFlag = false)
        {
            if (!IsDataFlag)
            {
                if (value.Trim().ToString() == "0" || value.Trim().ToString() == "1" || string.IsNullOrEmpty(value))
                {
                    return true;
                }
            }
            else
            {
                var val = "1";
                if (value.Trim() == val)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
