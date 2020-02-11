using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using System.Data;
using Base.Client;
using System.Windows.Forms;

namespace GetsujiSaikenKeisanSyori
{
    class Program
    {
        static Login_BL loginbl = new Login_BL();
        static GetsujiSaikenKeisanSyori skks_syori = new GetsujiSaikenKeisanSyori();
        static GetsujiSaikenKeisanSyori_BL skks_bl = new GetsujiSaikenKeisanSyori_BL();
       
         static  void Main(string[] args)
        {
            Console.Title = "GetsujiSaikenKeisanSyori";
            DataTable dtMode;
            Base_BL bbl = new Base_BL();


            if (loginbl.ReadConfig()==true)
            {
                dtMode = new DataTable();
                dtMode = bbl.SimpleSelect1("50");
                string Mode = dtMode.Rows[0]["MonthlySummuryKBN"].ToString();

                //プログラムを終了する
                if (dtMode.Rows.Count == 0) return;
                
                skks_syori.SujiSKKS_Syori(Mode);
            }
        }
    }
}
