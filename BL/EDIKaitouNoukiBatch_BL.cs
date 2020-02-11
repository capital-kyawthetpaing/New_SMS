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
    public class EDIKaitouNoukiBatch_BL : Base_BL
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

        public DataTable D_Order_Select(D_Order_Entity ode)
        {
            D_Hacchu_DL dl = new D_Hacchu_DL();
            return dl.D_Order_Select(ode);

        }
        public bool D_EDI_Insert(D_EDI_Entity de, DataTable dt)
        {
            D_Edi_DL dl = new D_Edi_DL();
            return dl.D_EDI_Insert(de, dt);
        }
    }
}
