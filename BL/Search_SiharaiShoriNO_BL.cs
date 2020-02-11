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
  public class Search_SiharaiShoriNO_BL: Base_BL
    {
        D_Pay_DL dpdl = new D_Pay_DL();
       public DataTable D_Pay_Search(D_Pay_Entity dpe)
        {
            return dpdl.D_Pay_Search(dpe);
        }
    }
}
