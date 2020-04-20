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
    public class Brand_BL : Base_BL
    {
        M_Brand_DL mmdl;

        /// <summary>
        /// constructor
        /// </summary>
        public Brand_BL()
        {
            mmdl = new M_Brand_DL();
        }

        public bool M_Brand_Select(M_Brand_Entity me)
        {
            DataTable dt = mmdl.M_Brand_Select(me);
            if (dt.Rows.Count > 0)
            {
                me.BrandCD = dt.Rows[0]["BrandCD"].ToString();
                me.BrandName = dt.Rows[0]["BrandName"].ToString();
                me.BrandKana = dt.Rows[0]["BrandKana"].ToString();
                me.UsedFlg = dt.Rows[0]["UsedFlg"].ToString();

                return true;
            }
            else
                return false;
        }
        
        public DataTable M_Brand_SelectAll(M_Brand_Entity me)
        {
            return mmdl.M_Brand_SelectAll(me);
        }
    }
}
