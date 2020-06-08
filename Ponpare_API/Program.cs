using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BL;
namespace Ponpare_API
{
   public class Program
    {
        //static void Main(string[] args)
        //{

        //}
        static Login_BL loginbl = new Login_BL();
        static DataTable dt;
        static string strbuff = string.Empty;
        public static void Main(string[] args)
        {

            //GetOrderList();

            Console.Title = "Ponpare API Console. . . ";
            Console.WriteLine("Started " + "Ponpare__API Started" + " . . . ");
            if (loginbl.ReadConfig() == true)
            {
                CommonAPI api = new CommonAPI();
                _Ponpare_BL pbl = new _Ponpare_BL();
                if (pbl.Allow_Check())
                {
                    api.GetOrder();
                }
                else
                {
                    Environment.Exit(0);
                }
            }



        }
    }
}
