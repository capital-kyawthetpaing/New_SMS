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
   public class MasterTouroku_Program_BL : Base_BL
    {
        M_Program_DL mpd;
        public MasterTouroku_Program_BL()
        {
            mpd = new M_Program_DL();
        }
        //public DataTable Program_Select(M_Program_Entity mpe)
        //{

        //}
    }
}
