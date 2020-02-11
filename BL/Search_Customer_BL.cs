using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DL;
using Entity;

namespace BL
{
 public  class Search_Customer_BL : Base_BL
    {
        M_Customer_DL mcdl;

        public Search_Customer_BL()
        {
            mcdl = new M_Customer_DL();
        }
        public DataTable M_Customer_Search(M_Customer_Entity mce)
        {
            return mcdl.M_Customer_Search(mce);
        }
    }
}
