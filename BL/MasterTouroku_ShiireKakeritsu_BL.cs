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
        public DataTable M_ShiireKakeritsu_Select(M_OrderRate_Entity moe)
        {
            return mskdl.MasterTouroku_ShiireKakeritsu_Select(moe);
        }
        public bool M_OrderRate_Update(M_OrderRate_Entity moe, string delData,string insertData)
        {
            return mskdl.M_Shiirekakeritsu(moe, delData, insertData);
        }
      
    }
}
