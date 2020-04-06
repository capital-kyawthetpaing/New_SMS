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
        public MasterTouroku_ShiireKakeritsu_BL()
        {
            mskdl = new MasterTouroku_ShiireKakeritsu_DL();
        }
        //public bool M_ShiireKakeritsu_Select(M_OrderRate_Entity moe)
        //{
        //    DataTable dt = mskdl.MasterTouroku_ShiireKakeritsu_Select(moe);
        //    if (dt.Rows.Count > 0)
        //    {
        //        moe.BrandCD = dt.Rows[0]["BrandCD"].ToString();
        //        moe.SportsCD = dt.Rows[0]["SportsCD"].ToString();
        //        moe.SegmentCD = dt.Rows[0]["SegmentCD"].ToString();
        //        moe.LastSeason = dt.Rows[0]["LastSeason"].ToString();
        //        moe.ChangeDate = dt.Rows[0]["ChangeDate"].ToString();
        //        moe.Rate = dt.Rows[0]["Rate"].ToString();
        //    }
        //    else
        //        return false;
        //}
    }
    
}
