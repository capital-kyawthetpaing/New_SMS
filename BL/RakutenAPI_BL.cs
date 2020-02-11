using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entity;
using DL;

namespace BL
{
    public class RakutenAPI_BL:Base_BL
    {
        M_MultiPorpose_DL MmultiporposeDataDL = new M_MultiPorpose_DL();
        D_RakutenRequest_DL DRakutenReq_dl = new D_RakutenRequest_DL();
        D_RakutenJuchuu_DL DRakutenJuChuu_dl = new D_RakutenJuchuu_DL();

        string FieldsName = string.Empty;
        string TableName = string.Empty;
        string Condition = string.Empty;

        public DataTable M_MultiPorpose_SelectID(M_MultiPorpose_Entity MmultiporposeData)
        {
            return MmultiporposeDataDL.M_MultiPorpose_Select(MmultiporposeData);
        }

        public DataTable D_APIControl_Select(D_APIControl_Entity DApiControl_entity)
        {
            return SimpleSelect1("12",string.Empty, DApiControl_entity.APIKey);
        }

        public DataTable M_API_Select(D_APIControl_Entity DApiControl_entity)
        {
            return SimpleSelect1("13",string.Empty, DApiControl_entity.APIKey);
        }

        public DataTable InsertSelect_SearchOrderData(D_RakutenRequest_Entity DRakutenReq_entity)
        {
            DRakutenReq_entity.OrderListXml = DataTableToXml(DRakutenReq_entity.dtOrderList);
            return DRakutenReq_dl.InsertSelect_SearchOrderData(DRakutenReq_entity);
        }

        public bool Insert_GetOrderData(Base_Entity base_entity)
        {
            base_entity.xml1= DataTableToXml(base_entity.dt1);
            base_entity.xml2 = DataTableToXml(base_entity.dt2);
            base_entity.xml3 = DataTableToXml(base_entity.dt3);
            base_entity.xml4 = DataTableToXml(base_entity.dt4);
            base_entity.xml5 = DataTableToXml(base_entity.dt5);
            base_entity.xml6 = DataTableToXml(base_entity.dt6);
            return DRakutenJuChuu_dl.Insert_GetOrderData(base_entity);
        }
    }
}
