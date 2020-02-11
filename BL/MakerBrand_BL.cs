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
    public class MakerBrand_BL : Base_BL
    {
        M_MakerBrand_DL mmdl;

        /// <summary>
        /// constructor
        /// </summary>
        public MakerBrand_BL()
        {
            mmdl = new M_MakerBrand_DL();
        }

        public bool M_MakerBrand_Select(M_MakerBrand_Entity mse)
        {
            DataTable dt = mmdl.M_MakerBrand_Select(mse);
            if (dt.Rows.Count > 0)
            {
                mse.BrandCD = dt.Rows[0]["BrandCD"].ToString();
                mse.MakerCD = dt.Rows[0]["MakerCD"].ToString();
                mse.IrregularKBN = dt.Rows[0]["IrregularKBN"].ToString();
                mse.DataSourseMakerCD = dt.Rows[0]["DataSourseMakerCD"].ToString();
                mse.PatternCD = dt.Rows[0]["PatternCD"].ToString();
                mse.BrandName = dt.Rows[0]["BrandName"].ToString();
                mse.DeleteFlg = dt.Rows[0]["DeleteFlg"].ToString();
                //mse.UsedFlg = dt.Rows[0]["UsedFlg"].ToString();

                return true;
            }
            else
                return false;
        }
        
        public DataTable M_MakerBrand_SelectAll(M_MakerBrand_Entity mse)
        {
            return mmdl.M_MakerBrand_SelectAll(mse);
        }
    }
}
