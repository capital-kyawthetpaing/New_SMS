using DL;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BL
{
    public class EDINouhinJouhon_Batch_BL: Base_BL
    {
        M_MultiPorpose_DL MmultiporposeDataDL;
        D_SKENDelivery_DL dskenDelivery_dl;

        public EDINouhinJouhon_Batch_BL()
        {
            MmultiporposeDataDL = new M_MultiPorpose_DL();
            dskenDelivery_dl = new D_SKENDelivery_DL();
        }
        public DataTable M_MultiPorpose_SelectID(M_MultiPorpose_Entity MmultiporposeData)
        {
            return MmultiporposeDataDL.M_MultiPorpose_Select(MmultiporposeData);
        }
        public bool M_MultiPorpose_Update(M_MultiPorpose_Entity mme)
        {
            return dskenDelivery_dl.M_MultiPorpose_Update(mme);
        }

        public DataTable D_SKENDelivery_SelectAll()
        {
            return dskenDelivery_dl.D_SKENDelivery_SelectAll();
        }
        public DataTable D_SKENDeliveryDetails_SelectAll(D_SKENDeliveryDetails_Entity de)
        {
            return dskenDelivery_dl.D_SKENDeliveryDetails_SelectAll(de);
        }

        public bool SKEN_InsertData(D_SKENDelivery_Entity dskend)
        {
            dskend.xml1 = DataTableToXml(dskend.dt1);
            dskend.xml2 = DataTableToXml(dskend.dt2);
            return dskenDelivery_dl.SKEN_InsertData(dskend);
        }
    }
}
