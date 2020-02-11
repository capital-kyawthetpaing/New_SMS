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
    public class SalesTax_BL : Base_BL
    {
        M_SalesTax_DL mmdl;
        public SalesTax_BL()
        {
            mmdl = new M_SalesTax_DL();
        }

        public bool M_SalesTax_Select(M_SalesTax_Entity mse)
        {
            DataTable dt = mmdl.M_SalesTax_Select(mse);
            if (dt.Rows.Count > 0)
            {
                mse.ChangeDate = dt.Rows[0]["ChangeDate"].ToString();
                mse.TaxRate1 = dt.Rows[0]["TaxRate1"].ToString();
                mse.TaxRate2 = dt.Rows[0]["TaxRate2"].ToString();
                mse.FractionKBN = dt.Rows[0]["FractionKBN"].ToString();
            
                return true;
            }
            else
                return false;

        }
        
    }
}
