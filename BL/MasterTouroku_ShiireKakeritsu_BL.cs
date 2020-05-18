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

    public class MasterTouroku_ShiireKakeritsu_BL : Base_BL
    {
        MasterTouroku_ShiireKakeritsu_DL mskdl;
        M_Vendor_DL mvdl;
        M_Brand_DL mbdl;
        public MasterTouroku_ShiireKakeritsu_BL()
        {
            mskdl = new MasterTouroku_ShiireKakeritsu_DL();
            mvdl = new M_Vendor_DL();
            mbdl = new M_Brand_DL();
        }
        public DataTable M_ShiireKakeritsu_Select(M_OrderRate_Entity moe)
        {
            return mskdl.MasterTouroku_ShiireKakeritsu_Select(moe);
            //if (dt.Rows.Count > 0)
            //{
            //    moe.BrandCD = dt.Rows[0]["BrandCD"].ToString();
            //    moe.SportsCD = dt.Rows[0]["SportsCD"].ToString();
            //    moe.SegmentCD = dt.Rows[0]["SegmentCD"].ToString();
            //    moe.LastSeason = dt.Rows[0]["LastSeason"].ToString();
            //    moe.ChangeDate = dt.Rows[0]["ChangeDate"].ToString();
            //    moe.Rate = dt.Rows[0]["Rate"].ToString();
              
            //}
            //else
            //    return false;
        }

        public DataTable M_Vendor_Select(M_Vendor_Entity mve)
        {
            return mvdl.M_Vendor_Select(mve);
        }

        public DataTable M_BrandSelect(M_Brand_Entity mbe)
        {
            return mbdl.M_BrandSelect(mbe);
        }
        public bool M_OrderRate_Update(M_OrderRate_Entity moe, string Xml)
        {
            return mskdl.M_Shiirekakeritsu(moe,Xml);
        }
    }
    
}
