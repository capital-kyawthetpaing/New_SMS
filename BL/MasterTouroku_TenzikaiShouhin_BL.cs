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
   public class MasterTouroku_TenzikaiShouhin_BL:Base_BL
   {
        MasterTouroku_TenzikaiShouhin_DL dl;
        M_Brand_DL mbdl;
        M_MultiPorpose_DL mpdl;
        
        public MasterTouroku_TenzikaiShouhin_BL()
        {
            dl = new MasterTouroku_TenzikaiShouhin_DL();
            mbdl = new M_Brand_DL();
            mpdl = new M_MultiPorpose_DL();
        }
        

        public DataTable Mastertoroku_Tenzikaishouhin_Select(M_TenzikaiShouhin_Entity mt, int mode)
        {
            return dl.Mastertoroku_Tenzikaishouhin_Select(mt, mode);
        }

        public DataTable M_TenzikaiShouhin_SelectByTenziName(M_TenzikaiShouhin_Entity mt)
        {
            return dl.M_TenzikaiShouhin_SelectByTenziName(mt);
        }
        public DataTable M_TenzikaiShouhinName_Select(M_TenzikaiShouhin_Entity mt)
        {
            return dl.M_TenzikaiShouhinName_Select(mt);
        }
        public DataTable M_Tenzikaishouhin_SelectForJancd(M_TenzikaiShouhin_Entity mt)
        {
            return dl.M_Tenzikaishouhin_SelectForJancd(mt);
        }
        public DataTable M_SKU_SelectForSKUCheck(M_SKU_Entity msku)
        {
            return dl.M_SKU_SelectForSKUCheck(msku);
        }

        public bool M_Tenzikaishouhin_DeleteUpdate(M_TenzikaiShouhin_Entity mt)
        {
            return dl.M_Tenzikaishouhin_DeleteUpdate(mt);
        }

        public bool M_Tenzikaishouhin_InsertUpdate(M_TenzikaiShouhin_Entity mt,int type)
        {
            return dl.M_Tenzikaishouhin_InsertUpdate(mt,type);
        }

        public bool M_Tenzikaishouhin_Delete(M_TenzikaiShouhin_Entity mtzke)
        {
            return dl.M_Tenzikaishouhin_Delete(mtzke);
        }

        public DataTable M_TenzikaiShouhin_Check(M_TenzikaiShouhin_Entity mtz)
        {
            return dl.M_TenzikaiShouhin_Check(mtz);
        }


        public DataTable M_Brand_SelectAll_NoPara()
        {
            return mbdl.M_Brand_SelectAll_NoPara();
        }

        public DataTable M_Multipurpose_SelectAll_ByID(int type)
        {
            return mpdl.M_Multipurpose_SelectAll_ByID(type);
        }

    }
}
