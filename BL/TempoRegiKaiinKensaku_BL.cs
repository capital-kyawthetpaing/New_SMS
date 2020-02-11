using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using Entity;
using System.Data;

namespace BL
{
    public class TempoRegiKaiinKensaku_BL : Base_BL
    {
        M_Customer_DL CustomerDL;
        public TempoRegiKaiinKensaku_BL()
        {
            CustomerDL = new M_Customer_DL();
        }

        public DataTable M_Customer_Display(M_Customer_Entity Customer)
        {
            return CustomerDL.M_Customer_Display(Customer);
        }
    }
}
