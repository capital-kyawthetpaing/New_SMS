using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;

namespace BL
{
    public class StoreBottunGroup_BL : Base_BL
    {
        M_StoreBottunGroup_DL storedl;
        public StoreBottunGroup_BL()
        {
            storedl = new M_StoreBottunGroup_DL();
        }

        public DataTable M_StoreButtonGroup_SelectAll(M_StoreBottunGroup_Entity mse)
        {
            return storedl.M_StoreButtonGroup_SelectAll(mse);
        }
    }
}
