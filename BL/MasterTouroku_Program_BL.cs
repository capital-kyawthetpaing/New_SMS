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
        public DataTable M_Program_Select(M_Program_Entity mpe)
        {
            return mpd.M_Program_Select(mpe);
        }
        public bool M_Program_Insert_Update(M_Program_Entity mpe, int mode)
        {
            return mpd.M_Program_Insert_Update(mpe, mode);
        }
        public bool M_Program_Delete(M_Program_Entity mpe)
        {
            return mpd.M_Program_Delete(mpe);
        }
        public DataTable M_ProgramSearch(M_Program_Entity mpe)
        {
            return mpd.M_ProgramSearch(mpe);
        }
    }
}
