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

namespace JANCDHenkou
{
    public partial class JANCDHenkou : FrmMainForm
    {
        JANCDHenkou_BL jhbl;
        public bool dup, isExist = true;
        DataTable dtJanCDExist;
        DataTable dtGenJanCD;
        string SKUCD;

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
            Btn_F10.Text = string.Empty;
            Btn_F11.Text = "取込(F11)";



            //dtGenJanCD = CreateDatatable();
            //dgvJANCDHenkou.DataSource = dtGenJanCD;
        }
        private void JANCDHenkou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        //public DataTable CreateDatatable()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("GenJanCD");
        //    dt.Columns.Add("BrandCD");
        //    dt.Columns.Add("BrandName");
        //    dt.Columns.Add("ITemCD");
        //    dt.Columns.Add("SKUName");
        //    dt.Columns.Add("SizeName");
        //    dt.Columns.Add("ColorName");
        //    dt.Columns.Add("GenJanCD2");
        //    dt.Columns.Add("newJanCD");
        //    dt.Columns.Add("SKUCD");

        //    dt.Rows.Add();
        //    dt.AcceptChanges();
        //    return dt;
        //}
        public override void FunctionProcess(int index)
        {
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
                        break;
                    }
                case 11:
                  //  F11();
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
            //txtTargetPeriodF.Focus();
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
                }
            }
        }

        private bool ErrorCheck()
        {
           
            foreach (DataGridViewRow row in dgvJANCDHenkou.Rows)
            {
                if (!(row.Cells["colGenJanCD"].Value == null))
                {
                    if (jhbl.SimpleSelect1("60", System.DateTime.Now.ToString("yyyy-MM-dd"),row.Cells["colGenJanCD"].Value.ToString()).Rows.Count < 1)
                    {
                        jhbl.ShowMessage("E101");
                        return false;
                    }
                }
           
                if (!(row.Cells["colNewJANCD"].Value == null))
                {
                    if (jhbl.SimpleSelect1("60", System.DateTime.Now.ToString("yyyy-MM-dd"), row.Cells["colNewJanCD"].Value.ToString()).Rows.Count > 0)
                    {
                        DialogResult dr = jhbl.ShowMessage("Q316");
                        if (dr == DialogResult.No)
                        {
                            dgvJANCDHenkou.CurrentCell = row.Cells["colNewJanCD"];
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void F11()
        {
            ErrorCheck();
        }

        private void F12()
        {
            ErrorCheck();
        }

        private void BtnF11Show_Click(object sender, EventArgs e)
        {
            F11();
        }

        private void dgvJANCDHenkou_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if ((sender as DataGridView).CurrentCell is DataGridViewTextBoxCell)
            {
                // 現JANCD
                if ((dgvJANCDHenkou.CurrentCell == dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"]) && !(dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value == null))
                {

                    foreach (DataGridViewRow r in dgvJANCDHenkou.Rows) //duplicate error
                    {
                        if (r.Index != e.RowIndex & !r.IsNewRow)
                        {
                            if (r.Cells["colGenJanCD"].Value.ToString() == dgvJANCDHenkou.CurrentRow.Cells["colGenJanCD"].Value.ToString())
                            {
                                jhbl.ShowMessage("E226");
                                return;
                            }
                        }
                    }

                    dtJanCDExist = jhbl.SimpleSelect1("60", System.DateTime.Now.ToString("yyyy-MM-dd"), dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value.ToString());
                    if (dtJanCDExist.Rows.Count < 1) // If its not exists then error
                    {
                        jhbl.ShowMessage("E101");
                    }
                    else if (dtJanCDExist.Rows.Count == 1) //BindData
                    {
                        if (dtGenJanCD != null)
                        {
                            if (!dtJanCDExist.Rows[0]["JanCD"].ToString().Equals(dtGenJanCD.Rows[0]["GenJanCD"].ToString()))
                            {
                                DataRow row = dtGenJanCD.NewRow();
                                DataTable tmp = jhbl.SimpleSelect1("59", System.DateTime.Now.ToString("yyyy-MM-dd"), dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value.ToString());
                                row["GenJanCD"] = tmp.Rows[0]["GenJanCD"].ToString();
                                row["BrandCD"] = tmp.Rows[0]["BrandCD"].ToString();
                                row["BrandName"] = tmp.Rows[0]["BrandName"].ToString();
                                row["ITemCD"] = tmp.Rows[0]["ITemCD"].ToString();
                                row["SKUName"] = tmp.Rows[0]["SKUName"].ToString();
                                row["SizeName"] = tmp.Rows[0]["SizeName"].ToString();
                                row["ColorName"] = tmp.Rows[0]["ColorName"].ToString();
                                row["GenJanCD2"] = tmp.Rows[0]["GenJanCD2"].ToString();
                                row["newJanCD"] = tmp.Rows[0]["newJanCD"];
                                row["SKUCD"] = tmp.Rows[0]["SKUCD"].ToString();
                                dtGenJanCD.Rows.Add(row);
                                dtGenJanCD.Rows.RemoveAt(dtGenJanCD.Rows.IndexOf(row) + 1);
                                dtGenJanCD.AcceptChanges();
                                dgvJANCDHenkou.DataSource = dtGenJanCD;
                                dgvJANCDHenkou.Rows.RemoveAt(dgvJANCDHenkou.Rows.Count - 2);
                            }
                        }
                        else
                        {
                            dtGenJanCD = jhbl.SimpleSelect1("59", System.DateTime.Now.ToString("yyyy-MM-dd"), dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value.ToString());
                            dgvJANCDHenkou.DataSource = dtGenJanCD;

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
                            if (!dtJanCDExist.Rows[0]["JanCD"].ToString().Equals(dtGenJanCD.Rows[0]["GenJanCD"].ToString()))
                            {
                                DataRow row = dtGenJanCD.NewRow();
                                DataTable tmp1 = jhbl.SimpleSelect1("61", System.DateTime.Now.ToString("yyyy-MM-dd"), SKUCD);
                                row["GenJanCD"] = tmp1.Rows[0]["GenJanCD"].ToString();
                                row["BrandCD"] = tmp1.Rows[0]["BrandCD"].ToString();
                                row["BrandName"] = tmp1.Rows[0]["BrandName"].ToString();
                                row["ITemCD"] = tmp1.Rows[0]["ITemCD"].ToString();
                                row["SKUName"] = tmp1.Rows[0]["SKUName"].ToString();
                                row["SizeName"] = tmp1.Rows[0]["SizeName"].ToString();
                                row["ColorName"] = tmp1.Rows[0]["ColorName"].ToString();
                                row["GenJanCD2"] = tmp1.Rows[0]["GenJanCD2"].ToString();
                                row["newJanCD"] = tmp1.Rows[0]["newJanCD"];
                                row["SKUCD"] = tmp1.Rows[0]["SKUCD"].ToString();
                                dtGenJanCD.Rows.Add(row);
                                dtGenJanCD.Rows.RemoveAt(dtGenJanCD.Rows.IndexOf(row) + 1);
                                dtGenJanCD.AcceptChanges();
                                dgvJANCDHenkou.DataSource = dtGenJanCD;
                                dgvJANCDHenkou.Rows.RemoveAt(dgvJANCDHenkou.Rows.Count - 2);
                            }
                        }
                        else
                        {
                            dtGenJanCD = jhbl.SimpleSelect1("61", System.DateTime.Now.ToString("yyyy-MM-dd"), SKUCD);
                            dgvJANCDHenkou.DataSource = dtGenJanCD;
                        }
                    }
                    }
                }
                // 現JANCD

                // 新JANCD
                if ((dgvJANCDHenkou.CurrentCell == dgvJANCDHenkou.Rows[e.RowIndex].Cells["colNewJanCD"]) && !(dgvJANCDHenkou.Rows[e.RowIndex].Cells["colNewJanCD"].Value == null))
                {
                    dtJanCDExist = jhbl.SimpleSelect1("60", System.DateTime.Now.ToString("yyyy-MM-dd"), dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value.ToString());
                    {
                        isExist = false;
                    }

                    foreach (DataGridViewRow r in dgvJANCDHenkou.Rows) //duplicate error
                    {
                        if (r.Index != e.RowIndex & !r.IsNewRow)
                        {
                            if (r.Cells["colNewJanCD"].Value.ToString() == dgvJANCDHenkou.CurrentRow.Cells["colNewJanCD"].Value.ToString())
                            {
                                dup = false;
                            }
                        }
                    }

                    if (!isExist || !dup)
                    {
                        DialogResult dr = jhbl.ShowMessage("Q316");
                        if (dr == DialogResult.No)
                        {
                            dgvJANCDHenkou.CurrentCell = dgvJANCDHenkou.CurrentRow.Cells["colNewJanCD"];
                        }
                    }
                }
                // 新JANCD
            }
        }
    }
}
