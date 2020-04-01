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
    public class Site_BL : Base_BL
    {
        M_Site_DL mmdl;
        public Site_BL()
        {
            mmdl = new M_Site_DL();
        }

        public bool M_Site_Select(M_Site_Entity mse)
        {
            DataTable dt = mmdl.M_Site_Select(mse);
            if (dt.Rows.Count > 0)
            {
                mse.ItemSKUCD = dt.Rows[0]["ItemSKUCD"].ToString();
                mse.APIKey = dt.Rows[0]["APIKey"].ToString();
                //mse.ItemSKUFLG = dt.Rows[0]["ItemSKUFLG"].ToString();
                mse.SiteURL = dt.Rows[0]["SiteURL"].ToString();
                mse.InsertOperator = dt.Rows[0]["InsertOperator"].ToString();
                mse.InsertDateTime = dt.Rows[0]["InsertDateTime"].ToString();
                mse.UpdateOperator = dt.Rows[0]["UpdateOperator"].ToString();
                mse.UpdateDateTime = dt.Rows[0]["UpdateDateTime"].ToString();

                return true;
            }
            else
                return false;

        }
    }


}
