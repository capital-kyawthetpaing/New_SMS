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
    public class Search_Ginkou_BL:Base_BL
    {
        M_Bank_DL mbankdl;
        public Search_Ginkou_BL()
        {
            mbankdl = new M_Bank_DL();
        }

        public DataTable M_Bank_Search(M_Bank_Entity mbe)
        {
            return mbankdl.M_Bank_Search(mbe);
        }
    }
}
