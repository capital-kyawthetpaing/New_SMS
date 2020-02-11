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

namespace TempoRegiDataUpdate
{
    public class TempoRegiDataUpdate
    {
        public  void ExecUpdate( D_Sales_Entity dse, int OperationMode)
        {
            TempoRegiHanbaiTouroku_BL tprg_Hanbai_Bl = new TempoRegiHanbaiTouroku_BL();
            bool ret = tprg_Hanbai_Bl.PRC_TempoRegiDataUpdate(dse, OperationMode);
        }       
    }
}

