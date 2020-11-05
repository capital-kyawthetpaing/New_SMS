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

namespace GetsujiShiireKeisanSyori
{
    public class GetsujiShiireKeisanSyori
    {
        static GetsujiShimeShori_BL sbl = new GetsujiShimeShori_BL();

        public  void ExecUpdate(D_MonthlyPurchase_Entity de)
        {
            try
            {
                sbl.GetsujiShiireKeisanSyori(de);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

                //ログ出力
                L_Log_Entity le = new L_Log_Entity();
                le.InsertOperator = de.Operator;
                le.Program = "GetsujiShiireKeisanSyori";
                le.PC = de.PC;
                le.OperateMode = "エラー";
                le.KeyItem = ex.Message.Substring(0, 100);
                sbl.L_Log_Insert(le);
            }
        }
    }
}


