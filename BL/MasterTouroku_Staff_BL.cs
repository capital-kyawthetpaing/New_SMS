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
    public class MasterTouroku_Staff_BL : Base_BL
    {
        M_Staff_DL StaffDL;
        M_MultiPorpose_DL MmpDL;
        M_Authorizations_DL MauthoDL;
        M_Store_DL storeDL;
      
        public MasterTouroku_Staff_BL()
        {
            StaffDL = new M_Staff_DL();
            MmpDL = new M_MultiPorpose_DL();
            MauthoDL = new M_Authorizations_DL();
            storeDL = new M_Store_DL();
        }
        
        public DataTable StaffDisplay(M_Staff_Entity staffdata)
        {
            return StaffDL.M_StaffDisplay(staffdata);
        }

        public DataTable StoreCheck(M_Store_Entity storedata)
        {
            return SimpleSelect1("17",storedata.ChangeDate,storedata.StoreCD);
        }

        public bool M_Staff_Insert_Update(M_Staff_Entity staffdata, int mode)
        {
            return StaffDL.M_Staff_Insert_Update(staffdata, mode);
        }

        public bool M_Staff_Delete(M_Staff_Entity staffdata)
        {
            return StaffDL.M_Staff_Delete(staffdata);
        }
    }
}
