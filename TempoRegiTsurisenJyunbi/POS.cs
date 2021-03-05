using Microsoft.PointOfService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempoRegiTsurisenJyunbi
{
   public class POS
    {
        CashDrawer myCashDrawer;
        PosExplorer explorer;
        public POS()
        {
            explorer = new PosExplorer();
            DeviceInfo ObjDevicesInfo = explorer.GetDevices("CashDrawer")[3];
            myCashDrawer = (CashDrawer)explorer.CreateInstance(ObjDevicesInfo);
        }
        public void OpenCashDrawer()
        {
            myCashDrawer.Open();
            myCashDrawer.Claim(1000);
            myCashDrawer.DeviceEnabled = true;
            myCashDrawer.OpenDrawer();
            myCashDrawer.DeviceEnabled = false;
            myCashDrawer.Release();
            myCashDrawer.Close();
        }
    }
}
