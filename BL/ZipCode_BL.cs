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
    public class ZipCode_BL : Base_BL
    {
        M_ZipCode_DL mzdl;
        public ZipCode_BL()
        {
            mzdl = new M_ZipCode_DL();
        }

        public DataTable M_ZipCode_Select(M_ZipCode_Entity mze)
        {
            return mzdl.M_ZipCode_Select(mze);
        }

        public bool M_ZipCode_SelectData(M_ZipCode_Entity mze)
        {
            DataTable dt = mzdl.M_ZipCode_Select(mze);
            if (dt.Rows.Count > 0)
            {
                mze.Address1 = dt.Rows[0]["Address1"].ToString();
                mze.Address2 = dt.Rows[0]["Address2"].ToString();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
