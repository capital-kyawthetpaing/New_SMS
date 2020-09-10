using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;
using Entity;

namespace BL
{
   public class UriageMotochou_BL : Base_BL
    {
        D_MonthlyClaims_DL dmcdl;
        UriageMotochou_Entity ume;
        Base_BL bbl;
        DataTable dtCheck;
        M_StoreClose_DL mscdl;

        public UriageMotochou_BL()
        {
            bbl = new Base_BL();
            dmcdl = new D_MonthlyClaims_DL();
            mscdl = new M_StoreClose_DL();
        }

        //public bool CheckData(int type,string StoreCD,string YYYYMM)
        //{
        //    switch (type)
        //    {
        //        case 1:
        //            dtCheck = bbl.SimpleSelect1("47", "", StoreCD, YYYYMM);
        //            break;
        //        case 2:
        //            dtCheck = bbl.SimpleSelect1("48", "", StoreCD, YYYYMM);
        //            break;
        //    }


        //    if (dtCheck.Rows.Count > 0)
        //        return true;
        //    else
        //        return false;
        //}
        
        public DataTable UriageMotochou_PrintSelect(UriageMotochou_Entity ume)
        {
            return dmcdl.UriageMotochou_PrintSelect(ume);
        }

        public string GetDate(string Date)
        {
            string date = string.Empty;
            date = Date;
            if (date.Contains("/"))
            {
                string[] m = date.Split('/');
                string year = m[0].ToString();
                string month = m[1].ToString();
                string day = DateTime.DaysInMonth(Convert.ToInt16(year), Convert.ToInt16(month)).ToString();
                DateTime dt = Convert.ToDateTime(year + "/" + month + "/" + day);
                date = dt.ToString("yyyy/MM/dd");

            }
            return date;
        }
        public DataTable M_StoreClose_Check(M_StoreClose_Entity msce, string mode)
        {
            return mscdl.M_StoreClose_Check(msce, mode);
        }
    }
}
