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

namespace GetsujiSaimuKeisanSyori
{
    public class GetsujiSaimuKeisanSyori
    {
        static GetsujiShimeShori_BL sbl = new GetsujiShimeShori_BL();

        public  void ExecUpdate(D_MonthlyDebt_Entity de)
        {
            try {
                sbl.GetsujiSaimuKeisanSyori(de);    
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}

