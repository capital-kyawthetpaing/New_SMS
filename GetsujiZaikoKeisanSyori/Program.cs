using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;

namespace GetsujiZaikoKeisanSyori
{
    class Program
    {
        static Login_BL loginbl = new Login_BL();
        static GetsujiZaikoKeisanSyori zks = new GetsujiZaikoKeisanSyori();
        static GetsujiZaikoKeisanSyori gzks = new GetsujiZaikoKeisanSyori();
        static void Main(string[] args)
        {
            Console.Title = "GetsujiZaikoKeisanSyori";
            Base_BL bbl = new Base_BL();

            if (loginbl.ReadConfig() == true)
            {
                string mode = bbl.SimpleSelect1("50").Rows[0]["MonthlySummuryKBN"].ToString();
                if (string.IsNullOrWhiteSpace(mode))
                    return;

                gzks.SujiZaikoKeisan_Syori(mode);
            }
        }
    }
}
