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

namespace ShukkaUriageUpdate
{
    public class ShukkaUriageUpdate
    {
        static ShukkaUriageUpdate_BL sbl = new ShukkaUriageUpdate_BL();

        public  void ExecUpdate(D_Sales_Entity de)
        {
            try {
                sbl.ExecUpdate(de);    
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}

