using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
namespace BL
{
  public  class TempoRegiFurikomiYoushi_BL : Base_BL
    {
        TempoRegiFurikomiYoushi_DL tdl;
        public TempoRegiFurikomiYoushi_BL()
        {
            tdl = new TempoRegiFurikomiYoushi_DL();
        }
        public DataTable SelectPrintData(String printno, string lblStore)
        {
            return tdl.SelectPrintData(printno, lblStore);
        }
        public DataTable Furikomi(string SalesNo)
        {
              return SimpleSelect1("16",string.Empty,SalesNo,"1");
        }
    }
}
