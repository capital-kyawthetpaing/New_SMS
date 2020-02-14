using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using BL;
using System.Collections;

using Microsoft.VisualBasic.FileIO;

namespace GetsujiShiireSyuukeiSyori
{
    public class GetsujiShiireSyuukeiSyori
    {
        static GetsujiShimeShori_BL sbl = new GetsujiShimeShori_BL();

        public  void ExecUpdate(D_MonthlyPurchase_Entity de)
        {
            try {
                sbl.GetsujiShiireKeisanSyori(de);    
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}

