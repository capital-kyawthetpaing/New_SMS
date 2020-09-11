using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using Entity;
using System.Data;

namespace BL
{
    public class TenzikaiJuchuuNo_BL:Base_BL
    {
        M_Customer_DL mcdl = new M_Customer_DL();
        M_Customer_Entity mce = new M_Customer_Entity();
        public DataTable M_Customer_SelectForTenzikai(M_Customer_Entity mce)
        {
            return mcdl.M_Customer_SelectForTenzikai(mce);
        }
    }
}
