using DL;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   public class MasterTorikomi_SKU_BL:Base_BL
   {
        M_SKUCounter_DL sdl;
        M_JANCounter_DL mdl;
        M_Brand_DL bdl;
        M_MultiPorpose_DL multidl;
        M_Vendor_DL mvdl;
        MasterTorikomi_SKU_DL mtdl;

        public MasterTorikomi_SKU_BL()
        {
            sdl = new M_SKUCounter_DL();
            mdl = new M_JANCounter_DL();
            bdl = new M_Brand_DL();
            multidl = new M_MultiPorpose_DL();
            mvdl = new M_Vendor_DL();
            mtdl = new MasterTorikomi_SKU_DL();
        }


        public DataTable M_SKUCounter_Update()
        {
            return sdl.M_SKUCounter_Update();
        }

        public DataTable M_JANCounter_JanContUpdate()
        {
            return mdl.M_JANCounter_JanContUpdate();
        }

        public DataTable M_Brand_SelectAll_NoPara()
        {
            return bdl.M_Brand_SelectAll_NoPara();
        }

        public DataTable M_Multipurpose_SelectAll_ByID(int type)
        {
            return multidl.M_Multipurpose_SelectAll_ByID(type);
        }

        public DataTable M_Multipurpose_SelectAll()
        {
            return multidl.M_Multipurpose_SelectAll();
        }

        public DataTable M_Vendor_SelectAll()
        {
            return mvdl.M_Vendor_SelectAll();
        }

        public bool MasterTorikomi_SKU_Insert_Update(int type,M_SKU_Entity mE)
        {
            return mtdl.MasterTorikomi_SKU_Insert_Update(type,mE);
        }
    }
}
