using DL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace BL
{
   public class TempoRegiMasterNyuuryoku_BL : Base_BL
    {
        M_Customer_DL mcusto_dl;
        M_SKU_DL msku_dl;
        M_StoreBottunGroup_DL msbtngroup_dl;
        M_StoreButtonDetails_DL msbd_dl;
        
        public TempoRegiMasterNyuuryoku_BL()
        {
            msku_dl = new M_SKU_DL();
            mcusto_dl = new M_Customer_DL();
            msbtngroup_dl = new M_StoreBottunGroup_DL();
            msbd_dl = new M_StoreButtonDetails_DL();
        }
        public DataTable Select_M_SKU_Data(string janCD)
        {
            return msku_dl.Select_M_SKU_Data(janCD);
        }
        public DataTable Select_M_Customer_Data(string janCD)
        {
            return mcusto_dl.Select_M_Customer_CustomerName(janCD);
        }
        public DataTable TempoRegiMasterNyuuryoku_Grid_Select(int mode)
        {
            return msbtngroup_dl.TempoRegiMasterNyuuryoku_Grid_Select(mode);
        }
        public DataTable TempoRegiMasterNyuuryoku_Grid_SelectAll(string StoreCD)
        {
            return msbtngroup_dl.M_StoreBottunGroup_Select(StoreCD);
        }
        public bool Button_Details_Insert_Update(M_StoreBottunDetails_Entity mre)
        {                
            mre.GroupXML = DataTableToXml(mre.dtGroup);
            mre.GroupDetailXML = DataTableToXml(mre.dtGpDetails);
            return msbd_dl.Button_Details_Insert_Update(mre);
        }        
        //public DataTable TempoRegiMasterNyuuryoku_cell_Select(string btnname,string groupno)
        //{
        //    return msbtngroup_dl.TempoRegiMasterNyuuryoku_cell_Select(btnname, groupno);
        //}
        //public String TempoRegiMasterNyuuryoku_cell_Select(string btnname, string groupno)
        //{

        //    DataTable dt = SimpleSelect1("20", string.Empty, groupno, btnname);
        //    if (dt.Rows.Count > 0)
        //    {
        //        return dt.Rows[0]["MasterKBN"].ToString();
        //    }
        //    return string.Empty;
        //}
        //public String TempoRegiMasterNyuuryoku_cell2_Select(string btnname, string groupno)
        //{
        //    DataTable dt = SimpleSelect1("20",string.Empty,groupno,btnname);
        //    if (dt.Rows.Count > 0)
        //    {
        //        return dt.Rows[0]["Botton"].ToString();
        //    }
        //    return string.Empty;
        //}
    }
}
