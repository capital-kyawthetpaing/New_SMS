using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using Base.Client;
using BL;
using Entity;
using Search;
using System.IO;
using ExcelDataReader;

namespace JANCDHenkou
{
    public partial class JANCDHenkou : FrmMainForm
    {
        JANCDHenkou_BL jhbl;
        public bool dup, isExist = false;
        DataTable dtJanCDExist;
        DataTable dtGenJanCD;
        string SKUCD, xml, JANCD;
        L_Log_Entity log_data;

        public JANCDHenkou()
        {
            InitializeComponent();
            jhbl = new JANCDHenkou_BL();
        }

        private void JANCDHenkou_Load(object sender, EventArgs e)
        {
            InProgramID = Application.ProductName;
            StartProgram();
            SetFunctionLabel(EProMode.KehiNyuuryoku);
            Btn_F2.Text = string.Empty;
            Btn_F3.Text = string.Empty;
            Btn_F4.Text = string.Empty;
            Btn_F5.Text = string.Empty;
            Btn_F7.Text = string.Empty;
            Btn_F8.Text = string.Empty;
            Btn_F10.Text = "取込(F10)";
            //Btn_F11.Text = "取込(F11)";
            Btn_F11.Text = string.Empty;
            Btn_F12.Text = "登録(F12)";
            ModeVisible = false;
            dtGenJanCD = CreateDatatable();
        }
        private void JANCDHenkou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        public DataTable CreateDatatable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("GenJanCD" , typeof(string));
            dt.Columns.Add("BrandCD" , typeof(string));
            dt.Columns.Add("BrandName", typeof(string));
            dt.Columns.Add("ITemCD" , typeof(string));
            dt.Columns.Add("SKUName", typeof(string));
            dt.Columns.Add("SizeName", typeof(string));
            dt.Columns.Add("ColorName", typeof(string));
            dt.Columns.Add("GenJanCD2", typeof(string));
            dt.Columns.Add("newJanCD", typeof(string));
            dt.Columns.Add("SKUCD", typeof(string));

            dt.AcceptChanges();
            return dt;
        }
        public override void FunctionProcess(int index)
        {
            //CKM_SearchControl sc = new CKM_SearchControl();
            base.FunctionProcess(index);
            switch (index + 1)
            {
                case 6: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                        {
                            Clear();
                        }
                    }
                    break;
                case 10:
                    F10();
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

        /// <summary>
        /// Clear data of Panel Detail 
        /// </summary>
        public void Clear()
        {
            Clear(panelDetail);
        }

        /// <summary>
        /// Show select_SKU form on gridview JanCD button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvJANCDHenkou_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            //if (dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value  != null)
            {
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                    e.RowIndex >= 0)
                {
                    Search_Product frmsp = new Search_Product(System.DateTime.Now.ToString("yyyy-MM-dd"));
                    //frmsp.JANCD = dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value.ToString();
                    frmsp.ShowDialog();
                    if (!frmsp.flgCancel)
                    {
                        JANCD = frmsp.JANCD;
                        
                        //if (dtGenJanCD.Rows.Count > 0)
                        //{
                        // DataTable tmp1 = jhbl.SimpleSelect1("61", System.DateTime.Now.ToString("yyyy-MM-dd"), SKUCD, dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value.ToString());
                        dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value = JANCD;
                        dgvJANCDHenkou.CurrentCell = dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"];
                            //dgvJANCDHenkou.Rows[e.RowIndex].Cells["colBrandCD"].Value = tmp1.Rows[0]["BrandCD"].ToString();
                            //dgvJANCDHenkou.Rows[e.RowIndex].Cells["colBrandName"].Value = tmp1.Rows[0]["BrandName"].ToString();
                            //dgvJANCDHenkou.Rows[e.RowIndex].Cells["colITEM"].Value = tmp1.Rows[0]["ITemCD"].ToString();
                            //dgvJANCDHenkou.Rows[e.RowIndex].Cells["colSKUName"].Value = tmp1.Rows[0]["SKUName"].ToString();
                            //dgvJANCDHenkou.Rows[e.RowIndex].Cells["colSize"].Value = tmp1.Rows[0]["SizeName"].ToString();
                            //dgvJANCDHenkou.Rows[e.RowIndex].Cells["colColor"].Value = tmp1.Rows[0]["ColorName"].ToString();
                            //dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD2"].Value = tmp1.Rows[0]["GenJanCD2"].ToString();
                            //dgvJANCDHenkou.Rows[e.RowIndex].Cells["colSKUCD"].Value = tmp1.Rows[0]["SKUCD"].ToString();
                            //dtGenJanCD.AcceptChanges();
                            //dgvJANCDHenkou.DataSource = dtGenJanCD;
                            //}
                            //else
                            //{
                            //    dtGenJanCD = jhbl.SimpleSelect1("61", System.DateTime.Now.ToString("yyyy-MM-dd"), SKUCD);
                            //    dgvJANCDHenkou.DataSource = dtGenJanCD;
                            //}
                    }
                }
            }
        }

        private void F10()
        {
            OpenFileDialog op = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                RestoreDirectory = true
            };

            if(op.ShowDialog() == DialogResult.OK)
            {
                string str = op.FileName;
                string ext = Path.GetExtension(str);
                if(!(ext.Equals(".xls") || ext.Equals(".xlsx")))
                {
                    jhbl.ShowMessage("E137");
                }
                else
                {
                    DataTable dtexcel = ExcelToDatatable(str);
                    string[] colname = { "現JANCD", "新JANCD" };
                    if(CheckColumn(colname,dtexcel))
                    {
                        xml = jhbl.DataTableToXml(dtexcel);
                        dtGenJanCD = jhbl.M_SKU_JanCDHenkou_Select(xml);
                        if (dtGenJanCD.Rows.Count > 0)
                        {
                            dgvJANCDHenkou.DataSource = dtGenJanCD;
                        }
                    }
                    else
                    {
                        jhbl.ShowMessage("E137");
                    }
                }
            }
        }

        private void F12()
        {
            if(ErrorCheck())
            {
                if (jhbl.ShowMessage("Q101") == DialogResult.Yes)
                {
                    xml = jhbl.DataTableToXml(dtGenJanCD);
                    log_data = Get_Log_Data();

                    if (jhbl.JanCDHenkou_Insert(xml, log_data))
                    {
                        Clear();
                    }
                }
            }
        }
        private bool ErrorCheck()
        {
            DataTable dt  = dtGenJanCD;
            foreach (DataGridViewRow row in dgvJANCDHenkou.Rows)
            {
                if (!dgvJANCDHenkou.Rows[row.Index].IsNewRow)
                {
                    //現JANCD
                    if (dgvJANCDHenkou.CurrentRow.Cells["colGenJanCD"].Value == null)
                    {
                        jhbl.ShowMessage("E102");
                        dgvJANCDHenkou.CurrentCell = dgvJANCDHenkou.CurrentRow.Cells["colGenJanCD"];
                        dgvJANCDHenkou.BeginEdit(true);
                        return false;
                    }
                    else
                    {
                        if (jhbl.SimpleSelect1("60", System.DateTime.Now.ToString("yyyy-MM-dd"), row.Cells["colGenJanCD"].Value.ToString()).Rows.Count < 1) //error if not exists in M_SKU tb
                        {
                            jhbl.ShowMessage("E101");
                            dgvJANCDHenkou.CurrentCell = row.Cells["colGenJanCD"];
                            dgvJANCDHenkou.BeginEdit(true);
                            return false;
                        }
                        if (dtGenJanCD.Rows.Count > 1)
                        {
                            DataRow[] dr = dtGenJanCD.Select("GenJanCD = '" + row.Cells["colGenJanCD"].Value + "'");
                            if (dr.Length > 1)
                            {
                                jhbl.ShowMessage("E226");
                                dgvJANCDHenkou.CurrentCell = row.Cells["colGenJanCD"];
                                dgvJANCDHenkou.BeginEdit(true);
                                return false;
                            }
                            //foreach (DataRow r in dtGenJanCD.Rows)
                            //{
                            //    if (r.RowState == DataRowState.Added)
                            //    {
                            //        if (r["GenJanCD"].ToString() == dgvJANCDHenkou.CurrentRow.Cells["colGenJanCD"].Value.ToString())
                            //        {
                            //            jhbl.ShowMessage("E226");
                            //            dgvJANCDHenkou.CurrentCell = row.Cells["colGenJanCD"];
                            //            dgvJANCDHenkou.BeginEdit(true);
                            //            return false;
                            //        }
                            //    }
                            //}
                        }
                    }

                   // 新JANCD
                    if (row.Cells["colNewJANCD"].Value == null)
                    {
                        jhbl.ShowMessage("E102");
                        dgvJANCDHenkou.CurrentCell = row.Cells["colNewJANCD"];
                        dgvJANCDHenkou.BeginEdit(true);
                        return false;
                    }
                    else
                    {
                        if (!row.Cells["colNewJanCD"].Value.ToString().Length.Equals(13) || !IsDigit(row.Cells["colNewJanCD"].Value.ToString()))    //For 13digits and digit only
                        {
                            jhbl.ShowMessage("E220");
                            dgvJANCDHenkou.CurrentCell = row.Cells["colNewJanCD"];

                            dgvJANCDHenkou.BeginEdit(true);
                            return false;
                        }
                        else if (jhbl.SimpleSelect1("60", System.DateTime.Now.ToString("yyyy-MM-dd"), row.Cells["colNewJanCD"].Value.ToString()).Rows.Count > 0)    //error if exists in M_SKU tb
                        {  ///duplicate check is required 
                            DialogResult dr = jhbl.ShowMessage("Q316");
                            if (dr == DialogResult.No)
                            {
                                dgvJANCDHenkou.CurrentCell = row.Cells["colNewJanCD"];
                                dgvJANCDHenkou.BeginEdit(true);
                                return false;
                            }
                            else
                                return true;
                        }
                    }
                }

            }
            return true;
        }
        private void dgvJANCDHenkou_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if ((sender as DataGridView).CurrentCell is DataGridViewTextBoxCell)
            {
                // 現JANCD
                if ((dgvJANCDHenkou.CurrentCell == dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"]) )
                { 
                    if(dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value == null)
                    {
                        jhbl.ShowMessage("E102");
                        dgvJANCDHenkou.CurrentCell = dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"];
                    }
                    else
                    {
                        foreach (DataGridViewRow r in dgvJANCDHenkou.Rows)      //duplicate error
                        {
                            if (r.Index != e.RowIndex & !r.IsNewRow)
                            {
                                if (r.Cells["colGenJanCD"].Value.ToString() == dgvJANCDHenkou.CurrentRow.Cells["colGenJanCD"].Value.ToString())
                                {
                                    jhbl.ShowMessage("E226");
                                    dgvJANCDHenkou.CurrentCell = dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"];
                                    return;
                                }
                            }
                        }

                        dtJanCDExist = jhbl.SimpleSelect1("60", System.DateTime.Now.ToString("yyyy-MM-dd"), dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value.ToString());
                        if (dtJanCDExist.Rows.Count == 0)        // error if not exists in M_SKU tb
                        {
                            jhbl.ShowMessage("E101");
                            dgvJANCDHenkou.CurrentCell = dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"];
                        }
                        else if (dtJanCDExist.Rows.Count == 1)      //BindData
                        {
                            if (dgvJANCDHenkou != null)
                            {
                                if(dtGenJanCD.Rows.Count > 0)
                                //if (!dtJanCDExist.Rows[0]["JanCD"].ToString().Equals(dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value.ToString()))
                                {
                                    DataTable tmp = jhbl.SimpleSelect1("59", System.DateTime.Now.ToString("yyyy-MM-dd"), dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value.ToString());
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value = tmp.Rows[0]["GenJanCD"].ToString();
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colBrandCD"].Value = tmp.Rows[0]["BrandCD"].ToString();
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colBrandName"].Value = tmp.Rows[0]["BrandName"].ToString();
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colITEM"].Value = tmp.Rows[0]["ITemCD"].ToString();
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colSKUName"].Value = tmp.Rows[0]["SKUName"].ToString();
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colSize"].Value = tmp.Rows[0]["SizeName"].ToString();
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colColor"].Value = tmp.Rows[0]["ColorName"].ToString();
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD2"].Value = tmp.Rows[0]["GenJanCD2"].ToString();
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colSKUCD"].Value = tmp.Rows[0]["SKUCD"].ToString();
                                    dtGenJanCD.AcceptChanges();
                                    dgvJANCDHenkou.DataSource = dtGenJanCD;
                                }
                                else
                                {
                                    dtGenJanCD = jhbl.SimpleSelect1("59", System.DateTime.Now.ToString("yyyy-MM-dd"), dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value.ToString());
                                    dgvJANCDHenkou.DataSource = dtGenJanCD;
                                }
                            }
                        }
                        else
                        {
                            Select_SKU frmsku = new Select_SKU();
                            frmsku.parJANCD = dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value.ToString();
                            frmsku.parChangeDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                            frmsku.ShowDialog();
                            if (!frmsku.flgCancel)
                            {
                                SKUCD = frmsku.parSKUCD;
                                if(dtGenJanCD.Rows.Count > 0)
                                //if (!dtJanCDExist.Rows[0]["JanCD"].ToString().Equals(dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value.ToString()))
                                {
                                    //DataTable tmp1 = jhbl.SimpleSelect1("59", System.DateTime.Now.ToString("yyyy-MM-dd"), dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value.ToString());
                                    DataTable tmp1 = jhbl.SimpleSelect1("61", System.DateTime.Now.ToString("yyyy-MM-dd"), SKUCD, dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value.ToString());
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value = tmp1.Rows[0]["GenJanCD"].ToString();
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colBrandCD"].Value = tmp1.Rows[0]["BrandCD"].ToString();
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colBrandName"].Value = tmp1.Rows[0]["BrandName"].ToString();
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colITEM"].Value = tmp1.Rows[0]["ITemCD"].ToString();
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colSKUName"].Value = tmp1.Rows[0]["SKUName"].ToString();
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colSize"].Value = tmp1.Rows[0]["SizeName"].ToString();
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colColor"].Value = tmp1.Rows[0]["ColorName"].ToString();
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD2"].Value = tmp1.Rows[0]["GenJanCD2"].ToString();
                                    dgvJANCDHenkou.Rows[e.RowIndex].Cells["colSKUCD"].Value = tmp1.Rows[0]["SKUCD"].ToString();
                                    dtGenJanCD.AcceptChanges();
                                    dgvJANCDHenkou.DataSource = dtGenJanCD;
                                    // DataRow row = dtGenJanCD.NewRow();
                                    // DataTable tmp1 = jhbl.SimpleSelect1("61", System.DateTime.Now.ToString("yyyy-MM-dd"), SKUCD);
                                    // row["GenJanCD"] = tmp1.Rows[0]["GenJanCD"].ToString();
                                    // row["BrandCD"] = tmp1.Rows[0]["BrandCD"].ToString();
                                    // row["BrandName"] = tmp1.Rows[0]["BrandName"].ToString();
                                    // row["ITemCD"] = tmp1.Rows[0]["ITemCD"].ToString();
                                    // row["SKUName"] = tmp1.Rows[0]["SKUName"].ToString();
                                    // row["SizeName"] = tmp1.Rows[0]["SizeName"].ToString();
                                    // row["ColorName"] = tmp1.Rows[0]["ColorName"].ToString();
                                    // row["GenJanCD2"] = tmp1.Rows[0]["GenJanCD2"].ToString();
                                    //// row["newJanCD"] = tmp1.Rows[0]["newJanCD"];
                                    // row["SKUCD"] = tmp1.Rows[0]["SKUCD"].ToString();
                                    // dtGenJanCD.Rows.Add(row);
                                    // dtGenJanCD.Rows.RemoveAt(dtGenJanCD.Rows.IndexOf(row) + 1);
                                    // dtGenJanCD.AcceptChanges();
                                    // dgvJANCDHenkou.DataSource = dtGenJanCD;
                                    // dgvJANCDHenkou.Rows.RemoveAt(dgvJANCDHenkou.Rows.Count - 2);
                                }
                                else
                                {
                                    dtGenJanCD = jhbl.SimpleSelect1("61", System.DateTime.Now.ToString("yyyy-MM-dd"), SKUCD);
                                    dgvJANCDHenkou.DataSource = dtGenJanCD;
                                }
                            }
                        }
                        
                    }

                }
                // 現JANCD

                // 新JANCD
                else if (dgvJANCDHenkou.CurrentCell == dgvJANCDHenkou.Rows[e.RowIndex].Cells["colNewJanCD"])
                {
                   if (string.IsNullOrWhiteSpace(dgvJANCDHenkou.Rows[e.RowIndex].Cells["colNewJanCD"].Value.ToString()))
                   {
                        jhbl.ShowMessage("E102");
                        dgvJANCDHenkou.CurrentCell = dgvJANCDHenkou.Rows[e.RowIndex].Cells["colNewJanCD"];
                        
                    }
                   else
                   {
                        if (!dgvJANCDHenkou.Rows[e.RowIndex].Cells["colNewJanCD"].Value.ToString().Length.Equals(13) ||
                           !IsDigit(dgvJANCDHenkou.Rows[e.RowIndex].Cells["colNewJanCD"].Value.ToString()))          //For 13digits and digit only
                        {
                            jhbl.ShowMessage("E220");
                            dgvJANCDHenkou.CurrentCell = dgvJANCDHenkou.Rows[e.RowIndex].Cells["colNewJanCD"];
                        }
                        else
                        {
                            dtJanCDExist = jhbl.SimpleSelect1("60", System.DateTime.Now.ToString("yyyy-MM-dd"), dgvJANCDHenkou.Rows[e.RowIndex].Cells["colNewJanCD"].Value.ToString());
                            if (dtJanCDExist.Rows.Count > 0)          //error if exists in M_SKU tb
                            {
                                isExist = true;
                            }

                            foreach (DataGridViewRow r in dgvJANCDHenkou.Rows)      //duplicate error
                            {
                                if (r.Index != e.RowIndex & !r.IsNewRow)
                                {
                                    if (r.Cells["colNewJanCD"].Value.ToString() == dgvJANCDHenkou.CurrentRow.Cells["colNewJanCD"].Value.ToString())
                                    {
                                        dup = true;
                                    }
                                }
                            }

                            if (isExist || dup)
                            {
                                DialogResult dr = jhbl.ShowMessage("Q316");
                                if (dr == DialogResult.No)
                                {
                                    dgvJANCDHenkou.CurrentCell = dgvJANCDHenkou.CurrentRow.Cells["colNewJanCD"];
                                }
                            }
                        }
                   }
                }
                // 新JANCD
            }
        }
        private bool IsDigit(string str)
        {
            if (!String.IsNullOrWhiteSpace(str))
            {
                for (int s = 0; s < str.Length; s++)
                {
                    if (char.IsDigit(str[s]) == false)
                        return false;

                }
            }
            return true;
        }

        protected DataTable ExcelToDatatable(string filePath)
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

        private void dgvJANCDHenkou_Paint(object sender, PaintEventArgs e)
        {
            string[] columns = { "現JANCD", "ブランド", "ITEM", "商品名", "サイズ", "カラー", "現JANCD", "新JANCD" };
            for (int j = 0; j < 4;)
            {
                Rectangle r1 = this.dgvJANCDHenkou.GetCellDisplayRectangle(j, -1, true);
                int w1 = this.dgvJANCDHenkou.GetCellDisplayRectangle(j + 1, -1, true).Width;
                int w2 = this.dgvJANCDHenkou.GetCellDisplayRectangle(j + 2, -1, true).Width;
                r1.X += 2;
                r1.Y += 1;
                r1.Width = r1.Width + w1  - 2;
                r1.Height = r1.Height - 2;
                e.Graphics.FillRectangle(new SolidBrush(this.dgvJANCDHenkou.ColumnHeadersDefaultCellStyle.BackColor), r1);
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(columns[j / 2],
                this.dgvJANCDHenkou.ColumnHeadersDefaultCellStyle.Font,
                new SolidBrush(this.dgvJANCDHenkou.ColumnHeadersDefaultCellStyle.ForeColor),
                r1,
                format);
                j += 2;
            }
        }

        private void BtnF10Show_Click(object sender, EventArgs e)
        {
            F10();
        }

        private void dgvJANCDHenkou_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                Rectangle r2 = e.CellBounds;
                r2.Y += e.CellBounds.Height / 2;
                r2.Height = e.CellBounds.Height / 2;
                e.PaintBackground(r2, true);
                e.PaintContent(r2);
                e.Handled = true;
            }
        }

        protected Boolean CheckColumn(String[] colName, DataTable dt)
        {
            DataColumnCollection col = dt.Columns;
            for (int i = 0; i < colName.Length; i++)
            {
                //if (!col.Contains(colName[i]))
                if (!dt.Columns[i].ColumnName.ToString().Equals(colName[i]))
                    return false;
            }
            return true;

        }

        private L_Log_Entity Get_Log_Data()
        {
            log_data = new L_Log_Entity()
            {
                Program = "JANCDHenkou",
                PC = InPcID,
                OperateMode = string.Empty,
                Operator = InOperatorCD,
                KeyItem = string.Empty
            };
            return log_data;
        }
    }
}
