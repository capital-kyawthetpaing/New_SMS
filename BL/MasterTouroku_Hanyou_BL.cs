using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;

namespace BL
{
   public class MasterTouroku_Hanyou_BL : Base_BL
    {
        M_Hanyou_DL mhdl;

        public MasterTouroku_Hanyou_BL()
        {
            mhdl = new M_Hanyou_DL();
        }

        public M_Hanyou_Entity M_Hanyou_Select(M_Hanyou_Entity mhe)
        {
            DataTable dtHanyou = new DataTable();
            dtHanyou = mhdl.M_Hanyou_KeySelect(mhe);
            if (dtHanyou.Rows.Count > 0)
            {
                mhe.ID = dtHanyou.Rows[0]["ID"].ToString();
                mhe.Key = dtHanyou.Rows[0]["Key"].ToString();
                mhe.Text1 = dtHanyou.Rows[0]["Char1"].ToString();
                mhe.Text2 = dtHanyou.Rows[0]["Char2"].ToString();
                mhe.Text3 = dtHanyou.Rows[0]["Char3"].ToString();
                mhe.Text4 = dtHanyou.Rows[0]["Char4"].ToString();
                mhe.Text5 = dtHanyou.Rows[0]["Char5"].ToString();
                mhe.Digital1 = dtHanyou.Rows[0]["Num1"].ToString();
                mhe.Digital2 = dtHanyou.Rows[0]["Num2"].ToString();
                mhe.Digital3 = dtHanyou.Rows[0]["Num3"].ToString();
                mhe.Digital4 = dtHanyou.Rows[0]["Num4"].ToString();
                mhe.Digital5 = dtHanyou.Rows[0]["Num5"].ToString();
                mhe.Day1 = dtHanyou.Rows[0]["Date1"].ToString();
                mhe.Day2 = dtHanyou.Rows[0]["Date2"].ToString();
                mhe.Day3 = dtHanyou.Rows[0]["Date3"].ToString();

                return mhe;
            }
            return null;
        }

        public DataTable Hanyou_IDSelect(M_Hanyou_Entity mhe)
        {
            return mhdl.M_Hanyou_IDSelect(mhe);       
        }

        public DataTable Hanyou_KeySelect(M_Hanyou_Entity mhe)
        {
            return mhdl.M_Hanyou_KeySelect(mhe);
        }

        public bool M_Hanyou_Insert_Update(M_Hanyou_Entity mhe,int mode)
        {
            return mhdl.M_Hanyou_Insert_Update(mhe, mode);
        }

        public bool M_Hanyou_Delete(M_Hanyou_Entity mhe)
        {
            return mhdl.M_Hanyou_Delete(mhe);
        }
    }
}
