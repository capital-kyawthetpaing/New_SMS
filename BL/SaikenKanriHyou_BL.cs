using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using System.Data;
using Entity;

namespace BL
{
 public    class SaikenKanriHyou_BL : Base_BL
    {
        D_MonthlyClaims_DL mdc_dl;

        public SaikenKanriHyou_BL()
        {
            mdc_dl = new D_MonthlyClaims_DL();
        }

        public DataTable M_StoreCheck_Select(D_MonthlyClaims_Entity dmce,int mode)
        {
            return mdc_dl.M_StoreCheck_Select(dmce,mode);
        }
        public DataTable D_MonthlyClaims_Select (D_MonthlyClaims_Entity dmce)
        {
            return mdc_dl.D_MonthlyClaims_Select(dmce);
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
    }
}
