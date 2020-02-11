using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using System.Data;
using Entity;

namespace BL
{
  public   class Shiharai_ItiranHyou_BL  : Base_BL
    {
        D_Pay_DL dpdl;

        public Shiharai_ItiranHyou_BL()
        {
            dpdl = new D_Pay_DL();
        }

        public DataTable  ItiranHyou_SelectForPrint(D_Pay_Entity dpe)
        {
            return dpdl.D_Pay_SelectForPrint(dpe);
        }
    }
}
