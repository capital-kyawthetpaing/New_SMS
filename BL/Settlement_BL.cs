using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using Entity;

namespace BL
{
    public class Settlement_BL : Base_BL
    {
        M_Settlement_DL dl;
        public Settlement_BL()
        {
            dl = new M_Settlement_DL();
        }
        public DataTable M_Settlement_Bind(M_Settlement_Entity mse)
        {
            mse.FileKBN = mse.FileKBN == "1" ? "1" : mse.FileKBN;
                return dl.M_Settlement_Bind(mse);
        }
    }
}
