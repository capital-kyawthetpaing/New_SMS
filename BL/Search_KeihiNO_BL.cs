using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;
using DL;
namespace BL
{
    public class Search_KeihiNO_BL :Base_BL
    {
        D_Cost_DL dcostdl;
        public Search_KeihiNO_BL()
        {
            dcostdl = new D_Cost_DL();
        }

        public DataTable D_Cost_Search(D_Cost_Entity dcoste)
        {
            return dcostdl.D_Cost_Search(dcoste);
        }
    }
}
