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
    public class JANOrderPrice_BL : Base_BL
    {
        M_JANOrderPrice_DL mdl;
        public JANOrderPrice_BL()
        {
            mdl = new M_JANOrderPrice_DL();
        }
        public bool M_JANOrderPrice_Select(M_JANOrderPrice_Entity mie)
        {
            DataTable dt = mdl.M_JANOrderPrice_Select(mie);

            if (dt.Rows.Count > 0)
            {
                mie.Rate = dt.Rows[0]["Rate"].ToString();
                mie.PriceWithoutTax = dt.Rows[0]["PriceWithoutTax"].ToString();
                mie.Remarks = dt.Rows[0]["Remarks"].ToString();
                mie.DeleteFlg = dt.Rows[0]["DeleteFlg"].ToString();
                mie.UsedFlg = dt.Rows[0]["UsedFlg"].ToString();

                return true;
            }
            else
                return false;
        }

    }
}
