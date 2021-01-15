using Base.Client;
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
using System.IO;
using ExcelDataReader;

namespace MasterTorikomi_SKU
{
    public partial class MasterTorikomi_SKU : FrmMainForm
    {
        Base_BL bbl;
        public MasterTorikomi_SKU()
        {
            InitializeComponent();
            bbl = new Base_BL();
        }

       

        private void MasterTorikomi_SKU_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTorikomi_SKU";
            StartProgram();
            RB_all.Checked = true;

        }

        private void BT_Torikomi_Click(object sender, EventArgs e)
        {
            if (bbl.ShowMessage("Q001") == DialogResult.Yes)
            {

                string filePath = string.Empty;
                string fileExt = string.Empty;
                //if (!System.IO.Directory.Exists("C:\\SMS\\TenzikaiShouhin\\"))
                //{
                //    System.IO.Directory.CreateDirectory("C:\\SMS\\TenzikaiShouhin\\");
                //}
                OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file 
                                                            // file.InitialDirectory = "C:\\SMS\\TenzikaiShouhin\\";
                                                            // file.RestoreDirectory = true;
                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
                {
                    filePath = file.FileName; //get the path of the file  
                    fileExt = Path.GetExtension(filePath); //get the file extension  
                    if (!(fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0))
                    {
                        bbl.ShowMessage("E137");
                        return;
                    }
                    DataTable dtExcel = new DataTable();
                    dtExcel = ExcelToDatatable(filePath);
                    //string[] colname = { "SKUCD", "JANCD", "商品名", "カラーNO", "カラー名", "サイズNO", "サイズ名", "販売予定日(月)", "販売予定日", "仕入単価", "標準売上単価", "ランク１単価", "ランク２単価", "ランク３単価", "ランク４単価", "ランク５単価", "ブランドCD", "セグメントCD", "単位CD", "税率区分", "備考" };
                    //if (ColumnCheck(colname, dtExcel))
                    //{
                        
                    //}
                }
            }


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
            string kibun = dt.Rows[0]["データ区分"].ToString();
            for(int i=0;i < dt.Rows.Count; i++)
            {
                if(String.IsNullOrEmpty(dt.Rows[i]["データ区分"].ToString()))
                {
                    bbl.ShowMessage("E102");
                }

                if (String.IsNullOrEmpty(dt.Rows[i]["AdminNO"].ToString()))
                {
                    bbl.ShowMessage("E102");
                }
                if (String.IsNullOrEmpty(dt.Rows[i]["改定日"].ToString()))
                {
                    bbl.ShowMessage("E102");
                }
                if (String.IsNullOrEmpty(dt.Rows[i]["承認日"].ToString()))
                {
                    bbl.ShowMessage("E102");
                }
                if (String.IsNullOrEmpty(dt.Rows[i]["SKUCD"].ToString()))
                {
                    bbl.ShowMessage("E102");
                }
                if (String.IsNullOrEmpty(dt.Rows[i]["JANCD"].ToString()))
                {
                    bbl.ShowMessage("E102");
                }
                if (String.IsNullOrEmpty(dt.Rows[i]["削除"].ToString()))
                {
                    bbl.ShowMessage("E102");
                }
                if(String.IsNullOrEmpty(dt.Rows[i]["諸口区分"].ToString()))
                {
                    bbl.ShowMessage("E102");
                }
                if (String.IsNullOrEmpty(dt.Rows[i]["商品名"].ToString()))
                {
                    bbl.ShowMessage("E102");
                }
                if (String.IsNullOrEmpty(dt.Rows[i]["カナ名"].ToString()))
                {
                    bbl.ShowMessage("E102");
                }
                if (String.IsNullOrEmpty(dt.Rows[i]["略名"].ToString()))
                {
                    bbl.ShowMessage("E102");
                }
            }
        }
        protected DataTable ExcelToDatatable(string filePath)
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

    }
}
