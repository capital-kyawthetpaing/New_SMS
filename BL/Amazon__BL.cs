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
  public  class Amazon__BL : Base_BL
    {
        Amazon__DL adl = new Amazon__DL();
        public Amazon__BL()
        {

        }

        public bool Allow_Check()
        {
            return Convert.ToInt32(adl.Allow_Check().Rows[0]["Status"].ToString()) == 1 ? true : false;
        }


    }
}
