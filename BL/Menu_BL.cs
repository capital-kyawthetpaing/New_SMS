using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using Entity;
using System.Data;
using DL;
namespace BL
{
    public class Menu_BL : Base_BL
    {
        Menu_DL mdl;
        public Menu_BL()
        {
            mdl = new Menu_DL();
        }

        public DataTable D_MenuMessageSelect(string SCD)
        {
            return mdl.D_MenuMessageSelect(SCD);
        }


        //D_MenuMessageSelect
        public DataTable getMenuNo(string SCD,string IsStored)
        {
            return mdl.getMenuNo(SCD,IsStored);
        }


    }
}
