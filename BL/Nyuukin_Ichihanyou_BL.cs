using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using System.Data;
using Entity;
namespace BL 
{
    public class Nyuukin_Ichihanyou_BL :Base_BL
    {
        Nyuukin_Ichihanyou_DL nidl;
        public Nyuukin_Ichihanyou_BL()
        {
            nidl = new Nyuukin_Ichihanyou_DL();
        }

        public bool Check_PaymentStartDate(string paymentstart)
        {
            //return nidl.Check_PaymentStartDate(paymentstart);
            return true;
        }
        public DataTable getPrintData(Nyuukin_Ichihanyou_Entity nie)
        {
            return nidl.getPrintData(nie);
        }
        //
        public bool L_Log_Insert_Print(string[] str, DataTable dt)
        {
            return nidl.L_Log_Insert_Print(str,dt);
        }

    }
}
