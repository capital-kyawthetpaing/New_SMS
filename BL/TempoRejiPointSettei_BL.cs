using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;

namespace BL
{
    public class TempoRejiPointSettei_BL:Base_BL
    {
        M_StorePoint_DL mspdl;
        public TempoRejiPointSettei_BL()
        {
            mspdl = new M_StorePoint_DL();
        }

        public bool M_StorePoint_Insert_Update(M_StorePoint_Entity mspe)
        {
            return mspdl.M_StorePoint_Insert_Update(mspe);
        }

        public M_StorePoint_Entity M_StorePoint_Select(M_StorePoint_Entity mspe)
        {
            DataTable dtStorePoint = mspdl.M_StorePoint_Select(mspe);
            if (dtStorePoint.Rows.Count > 0)
            {
                mspe.ChangeDate = dtStorePoint.Rows[0]["ChangeDate"].ToString();
                mspe.PointRate = dtStorePoint.Rows[0]["PointRate"].ToString();
                mspe.ServicedayRate = dtStorePoint.Rows[0]["ServicedayRate"].ToString();
                mspe.ExpirationDate = dtStorePoint.Rows[0]["ExpirationDate"].ToString();
                mspe.MaxPoint = dtStorePoint.Rows[0]["MaxPoint"].ToString();
                mspe.TicketUnit = dtStorePoint.Rows[0]["TicketUnit"].ToString();

                mspe.Print1 = dtStorePoint.Rows[0]["Print1"].ToString();
                mspe.Bold1 = dtStorePoint.Rows[0]["Bold1"].ToString();
                mspe.Size1 = dtStorePoint.Rows[0]["Size1"].ToString();

                mspe.Print2 = dtStorePoint.Rows[0]["Print2"].ToString();
                mspe.Bold2 = dtStorePoint.Rows[0]["Bold2"].ToString();
                mspe.Size2 = dtStorePoint.Rows[0]["Size2"].ToString();

                mspe.Print3 = dtStorePoint.Rows[0]["Print3"].ToString();
                mspe.Bold3 = dtStorePoint.Rows[0]["Bold3"].ToString();
                mspe.Size3 = dtStorePoint.Rows[0]["Size3"].ToString();

                mspe.Print4 = dtStorePoint.Rows[0]["Print4"].ToString();
                mspe.Bold4 = dtStorePoint.Rows[0]["Bold4"].ToString();
                mspe.Size4 = dtStorePoint.Rows[0]["Size4"].ToString();

                mspe.Print5 = dtStorePoint.Rows[0]["Print5"].ToString();
                mspe.Bold5 = dtStorePoint.Rows[0]["Bold5"].ToString();
                mspe.Size5 = dtStorePoint.Rows[0]["Size5"].ToString();

                mspe.Print6 = dtStorePoint.Rows[0]["Print6"].ToString();
                mspe.Bold6 = dtStorePoint.Rows[0]["Bold6"].ToString();
                mspe.Size6 = dtStorePoint.Rows[0]["Size6"].ToString();

                mspe.Print7 = dtStorePoint.Rows[0]["Print7"].ToString();
                mspe.Bold7 = dtStorePoint.Rows[0]["Bold7"].ToString();
                mspe.Size7 = dtStorePoint.Rows[0]["Size7"].ToString();

                mspe.Print8 = dtStorePoint.Rows[0]["Print8"].ToString();
                mspe.Bold8 = dtStorePoint.Rows[0]["Bold8"].ToString();
                mspe.Size8 = dtStorePoint.Rows[0]["Size8"].ToString();

                mspe.Print9 = dtStorePoint.Rows[0]["Print9"].ToString();
                mspe.Bold9 = dtStorePoint.Rows[0]["Bold9"].ToString();
                mspe.Size9 = dtStorePoint.Rows[0]["Size9"].ToString();

                mspe.Print10 = dtStorePoint.Rows[0]["Print10"].ToString();
                mspe.Bold10 = dtStorePoint.Rows[0]["Bold10"].ToString();
                mspe.Size10 = dtStorePoint.Rows[0]["Size10"].ToString();

                mspe.Print11 = dtStorePoint.Rows[0]["Print11"].ToString();
                mspe.Bold11 = dtStorePoint.Rows[0]["Bold11"].ToString();
                mspe.Size11 = dtStorePoint.Rows[0]["Size11"].ToString();

                mspe.Print12 = dtStorePoint.Rows[0]["Print12"].ToString();
                mspe.Bold12 = dtStorePoint.Rows[0]["Bold12"].ToString();
                mspe.Size12 = dtStorePoint.Rows[0]["Size12"].ToString();

                mspe.DeleteFlg = dtStorePoint.Rows[0]["DeleteFlg"].ToString();
                return mspe;
            }
            return null;
        }
    }
}
