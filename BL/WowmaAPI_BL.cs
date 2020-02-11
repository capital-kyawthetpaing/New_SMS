using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using Entity;
using System.Data;

namespace BL
{
    
   public class WowmaAPI_BL :Base_BL
    {
        M_MultiPorpose_DL MmultiporposeDataDL = new M_MultiPorpose_DL();
        D_WowmaJuchuu_DL wowmadl = new D_WowmaJuchuu_DL();
        D_WowmaRequest_Entity wowma_entity = new D_WowmaRequest_Entity();

        string FieldsName = string.Empty;
        string TableName = string.Empty;
        string Condition = string.Empty;
        public DataTable M_MultiPorpose_SelectID(M_MultiPorpose_Entity MmultiporposeData)
        {
            return MmultiporposeDataDL.M_MultiPorpose_Select(MmultiporposeData);
        }
        public DataTable D_APIControl_Select(D_APIControl_Entity DApiControl_entity)
        {
            return SimpleSelect1("12",string.Empty,DApiControl_entity.APIKey);
        }         
        public DataTable InsertSelect_OrderData(D_WowmaRequest_Entity wowma_e)
        {
            wowma_e.OrderListXml = DataTableToXml(wowma_e.dtOrderList);
            return wowmadl.InsertSelect_OrderData(wowma_e);
        }
        public DataTable M_API_Select(D_APIControl_Entity DApiControl_entity)
        {
            return SimpleSelect1("13", string.Empty, DApiControl_entity.APIKey);
        }
        public bool InsertSelect_OrderDetail(Base_Entity be)
        {
            be.xml1 = DataTableToXml(be.dt1);
            be.xml2 = DataTableToXml(be.dt2);

            return wowmadl.Insert_OrderDetailData(be);
        }
       
    }
}
