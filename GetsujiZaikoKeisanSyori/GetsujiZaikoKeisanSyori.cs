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

namespace GetsujiZaikoKeisanSyori
{
    public class GetsujiZaikoKeisanSyori
    {
        static GetsujiShimeShori_BL sbl = new GetsujiShimeShori_BL();

        public  void ExecUpdate(D_MonthlyStock_Entity de)
        {
            try {
                sbl.GetsujiZaikoKeisanSyori(de);    
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}

