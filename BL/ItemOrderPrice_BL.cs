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
    public class ItemOrderPrice_BL : Base_BL
    {
        M_ItemOrderPrice_DL mdl;
        public ItemOrderPrice_BL()
        {
            mdl = new M_ItemOrderPrice_DL();
        }
        public bool M_ItemOrderPrice_Select(M_ItemOrderPrice_Entity mie)
        {
            DataTable dt = mdl.M_ItemOrderPrice_Select(mie);

            if (dt.Rows.Count > 0)
            {
                mie.Rate = dt.Rows[0]["Rate"].ToString();
                mie.PriceWithoutTax = dt.Rows[0]["PriceWithoutTax"].ToString();
                //mie.Remarks = dt.Rows[0]["Remarks"].ToString();
                mie.DeleteFlg = dt.Rows[0]["DeleteFlg"].ToString();
                mie.UsedFlg = dt.Rows[0]["UsedFlg"].ToString();

                return true;
            }
            else
                return false;
        }

        

    }
}
