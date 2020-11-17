using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using Entity;
using System.Data;
using DL;
namespace BL
{
    public class Menu_BL : Base_BL
    {
        Menu_DL mdl;
        public Menu_BL()
        {
            mdl = new Menu_DL();
        }

        public DataTable SettingGetAllPermission(string val, string admin ,string setting ,string def)
        {
            return mdl.SettingGetAllPermission(val,admin,setting,def);
        }
        public DataTable D_MenuMessageSelect(string SCD)
        {
            return mdl.D_MenuMessageSelect(SCD);
        }
        //D_MenuMessageSelect
        public DataTable getMenuNo(string SCD,string IsStored)
        {
            return mdl.getMenuNo(SCD,IsStored);
        }
        public bool SettingSave(M_Setting ms)
        {
            return mdl.SettingSave(ms);
        }
        //SettingDefault
        public bool SettingDefault(string stafCD, string MCD)
        {
            return mdl.SettingDefault(stafCD, MCD);
        }
        public bool SettingPermissionUpdate(string StaffCD, string Date, string xml)
        {
            return mdl.SettingPermissionUpdate(StaffCD, Date, xml);
        }

    }
}
