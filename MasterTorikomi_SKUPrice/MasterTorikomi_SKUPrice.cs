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
using ExcelDataReader;

namespace MasterTorikomi_SKUPrice
{
    public partial class MasterTorikomi_SKUPrice : FrmMainForm
    {
        Base_BL bbl;
        DataTable dtSKU = new DataTable();
        DataTable dtTankaCD = new DataTable();
        DataTable dtStoreCD = new DataTable();
        M_SKUPrice_Entity mskup;
        MasterTorikomi_SKUPrice_BL mskupbl; 
        SKU_BL sbl;
        string filePath = string.Empty;

        public MasterTorikomi_SKUPrice()
        {
            InitializeComponent();
            mskup = new M_SKUPrice_Entity();
            mskupbl = new MasterTorikomi_SKUPrice_BL(); 

            sbl = new SKU_BL();
        }

        private void MasterTorikomi_SKUPrice_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTorikomi_SKUPrice";
            StartProgram();
            ModeVisible = false;
            Btn_F12.Text = "取込(F12)";

            dtSKU = sbl.M_SKU_SelectAll_NOPara();
            dtTankaCD = mskupbl.M_TankaCD_SelectAll_NoPara();
            dtStoreCD = mskupbl.M_StoreCD_SelectAll_NoPara();
        }

        public override void FunctionProcess(int index)
        {

            switch (index + 1)
            {

                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        CleanData();
                    }
                    break;

                case 12:
                    InputExcel();
                    break;
            }
        }

        private void InputExcel()
        {
            if (String.IsNullOrEmpty(TB_FileName.Text))
            {
                bbl.ShowMessage("E121");
            }
            else
            {
                //if (bbl.ShowMessage("Q001") == DialogResult.Yes)
                if (bbl.ShowMessage("Q001", "取込処理") == DialogResult.Yes)
                {
                    DataTable dt = ExcelToDatatable(filePath);
                    if (dt != null)
                    {
                        if (ErrorCheck(dt))
                        {
                            ExcelErrorCheck(dt);
                            if (CheckPartial(dt))
                            {
                                mskup = GetEntity(dt);
                                if (mskupbl.MasterTorikomi_SKUPrice_Insert_Update(mskup))
                                {
                                    bbl.ShowMessage("I101");
                                }
                            }
                            GV_SKUPrice.DataSource = null;
                            GV_SKUPrice.DataSource = dt;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No row data was found or import excel is opening in different location");
                    }
                }
            }
        }

        private M_SKUPrice_Entity GetEntity(DataTable dtT)
        {
            mskup = new M_SKUPrice_Entity
            {
                dt1 = dtT,
                Operator = InOperatorCD,
                ProgramID = InProgramID,
                Key = filePath,
                PC = InPcID,
            };
            return mskup;
        }

        private bool ErrorCheck(DataTable dt)
        {
            if (dt.Columns.Contains("EItem") && dt.Columns.Contains("Error"))
            {
                dt.Columns.Remove("EItem");
                dt.Columns.Remove("Error");
            }
            string[] colname = {"データ区分", "単価設定CD", "店舗CD", "AdminNO", "SKUCD","商品名", "改定日","適用終了日", "税込定価", "税抜定価","一般掛率", "税込一般単価", "税抜一般単価",
                "会員掛率","税込会員単価","税抜会員単価", "外商掛率", "税込外商単価", "税抜外商単価","Sale掛率", "税込Sale単価", "税抜Sale単価", "Web掛率","税込Web単価","税抜Web単価", "備考", "削除FLG",};
            if (!ColumnCheck(colname, dt))
            {
                bbl.ShowMessage("E137");
                return false;
            }
            return true;
        }

        private bool CheckPartial(DataTable dt)
        {
            var query = "Error <> ''";
            if (dt.Select(query).Count() > 0)
                return false;
            return true;
        }
        private void CleanData()
        {
            GV_SKUPrice.DataSource = null;
            TB_FileName.Text = "";

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
                // bbl.ShowMessage("E137");
                return null;
            }
        }

        private void BT_FileName_Click(object sender, EventArgs e)
        {
            GV_SKUPrice.DataSource = null;
            //filePath = string.Empty;
            string fileExt = string.Empty;
            OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file 
                                                        // file.InitialDirectory = "C:\\SMS\\TenzikaiShouhin\\";
                                                        // file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
            {
                filePath = file.FileName; //get the path of the file  
                TB_FileName.Text = file.FileName;
                fileExt = Path.GetExtension(filePath); //get the file extension  
                if (!(fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0))
                {

                    MessageBox.Show("No row data was found or import excel is opening in different location");
                    return;
                }
                GV_SKUPrice.DataSource = null;
             }
                
            
        }

        protected override void EndSec()
        {
            this.Close();
        }

        protected Boolean ColumnCheck(String[] colName, DataTable dtMain)
        {
            DataColumnCollection cols = dtMain.Columns;
            for (int i = 0; i < colName.Length; i++)
            {
                if (!cols.Contains(colName[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private void ExcelErrorCheck(DataTable dt)
        {
            dt.Columns.Add("EItem");
            dt.Columns.Add("Error");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (String.IsNullOrEmpty(dt.Rows[i]["データ区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "データ区分";
                    dt.Rows[i]["Error"] = "E102";
                }

                if (String.IsNullOrEmpty(dt.Rows[i]["単価設定CD"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "単価設定CD";
                    dt.Rows[i]["Error"] = "E102";
                }
                else
                {
                    String query = " TankaCD = '" + dt.Rows[i]["単価設定CD"].ToString() + "'";

                    var result = dtSKU.Select(query);
                    if (result.Count() == 0)
                    {
                        dt.Rows[i]["EItem"] = "単価設定CD";
                        dt.Rows[i]["Error"] = "E101";
                    }
                }

                if (String.IsNullOrEmpty(dt.Rows[i]["店舗CD"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "店舗CD";
                    dt.Rows[i]["Error"] = "E102";
                }
                else
                {
                    String query = " StoreCD = '" + dt.Rows[i]["店舗CD"].ToString() + "'";

                    var result = dtSKU.Select(query);
                    if (result.Count() == 0)
                    {
                        dt.Rows[i]["EItem"] = "店舗CD";
                        dt.Rows[i]["Error"] = "E101";
                    }
                }

                if (String.IsNullOrEmpty(dt.Rows[i]["AdminNO"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "AdminNO";
                    dt.Rows[i]["Error"] = "E102";
                }
                else
                {
                    String query = "AdminNO = " + dt.Rows[i]["AdminNO"].ToString() + "" +
                        " and SKUCD = '" + dt.Rows[i]["SKUCD"].ToString() + "'";

                    var result = dtSKU.Select(query);
                    if (result.Count() == 0)
                    {
                        dt.Rows[i]["EItem"] = "AdminNO";
                        dt.Rows[i]["Error"] = "E101";
                    }
                }

                if (String.IsNullOrEmpty(dt.Rows[i]["SKUCD"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "SKUCD";
                    dt.Rows[i]["Error"] = "E102";
                }

                if (String.IsNullOrEmpty(dt.Rows[i]["改定日"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "改定日";
                    dt.Rows[i]["Error"] = "E102";
                }

                if (String.IsNullOrEmpty(dt.Rows[i]["改定日"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "改定日";
                    dt.Rows[i]["Error"] = "E102";
                }

                if (!String.IsNullOrEmpty(dt.Rows[i]["データ区分"].ToString()))
                {
                    string d = dt.Rows[i]["データ区分"].ToString();
                    if (dt.Rows[i]["データ区分"].ToString() != "1")
                    {
                        dt.Rows[i]["EItem"] = "データ区分";
                        dt.Rows[i]["Error"] = "E190";
                    }
                }

                if (!String.IsNullOrEmpty(dt.Rows[i]["改定日"].ToString()))
                {
                    string date = bbl.FormatDate(dt.Rows[i]["改定日"].ToString());
                    if (!bbl.CheckDate(date))
                    {
                        dt.Rows[i]["EItem"] = "改定日";
                        dt.Rows[i]["Error"] = "E103";
                    }
                }

                if (!String.IsNullOrEmpty(dt.Rows[i]["適用終了日"].ToString()))
                {
                    string date = bbl.FormatDate(dt.Rows[i]["適用終了日"].ToString());
                    if (!bbl.CheckDate(date))
                    {
                        dt.Rows[i]["EItem"] = "適用終了日";
                        dt.Rows[i]["Error"] = "E103";
                    }
                }

            }
        }

        private void MasterTorikomi_SKU_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void BT_Torikomi_Click(object sender, EventArgs e)
        {
            InputExcel();
        }
    }
}
