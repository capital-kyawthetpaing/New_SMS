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
    public class MasterTouroku_TenzikaiHanbaiTankaKakeritu_BL:Base_BL
    {
        MasterTouroku_TenzikaiHanbaiTankaKakeritu_DL dl = new MasterTouroku_TenzikaiHanbaiTankaKakeritu_DL();
        public DataTable M_TenzikaiShouhin_Select(M_TenzikaiShouhin_Entity mTSE)
        {
            return dl.M_TenzikaiShouhin_Select(mTSE);
        }

        public bool InsertUpdate_TenzikaiHanbaiTankaKakeritu(M_TenzikaiShouhin_Entity mTSE, string xml)
        {
            return dl.InsertUpdate_TenzikaiHanbaiTankaKakeritu(mTSE, xml);
        }
    }
}
