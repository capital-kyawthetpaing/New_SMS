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
    public class JuchuuTorikomiAPI_BL : Base_BL
    {
        JuchuuTorikomiAPI_DL pcapdl;
        M_MultiPorpose_DL mmpdl;
        public JuchuuTorikomiAPI_BL()
        {
            pcapdl = new JuchuuTorikomiAPI_DL();
            mmpdl = new M_MultiPorpose_DL();
        }
        public String M_Multipurpose_Num1_Select(M_MultiPorpose_Entity mme)
        {
            DataTable dt = mmpdl.M_MultiPorpose_Select(mme);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["Num1"].ToString();
            }
            return string.Empty;
        }
        public bool JuchuuTorikomiAPI_Insert_Update(D_APIRireki_Entity mre)
        {
            return pcapdl.JuchuuTorikomiAPI_Insert_Update(mre);
        }
        public DataTable D_APIControl_Select()
        {
            //return SimpleSelect1("28");
            return pcapdl.D_APIControl_Select();


        }
        public DataTable JuchuuTorikomiAPI_Grid_Select()
        {
            return pcapdl.JuchuuTorikomiAPI_Grid_Select();
        }
        public bool JuchuuTorikomiAPI_D_APIControl_Insert_Update(D_APIRireki_Entity mre)
        {
            return pcapdl.JuchuuTorikomiAPI_D_APIControl_Insert_Update(mre);
        }
    }
}
