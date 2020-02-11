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
    public class MasterTouroku_YuubinBangou_BL : Base_BL
    {
        M_ZipCode_DL ZipcodeDL;
        public MasterTouroku_YuubinBangou_BL()
        {
            ZipcodeDL = new M_ZipCode_DL();
        }

        public DataTable M_ZipCode_YuubinBangou_Select(M_ZipCode_Entity Zipcode, string Zip1To, string Zip2To)
        {
            return ZipcodeDL.M_ZipCode_YuubinBangou_Select(Zipcode, Zip1To, Zip2To);
        }

        public bool M_ZipCode_Update(M_ZipCode_Entity ZipCode, string Zip1To, string Zip2To, string Xml)
        {
            return ZipcodeDL.M_ZipCode_Update(ZipCode, Zip1To,Zip2To, Xml);
        }
    }
}
