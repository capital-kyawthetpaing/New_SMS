using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;
using System.Collections;

namespace BL
{
    public class DenominationKBN_BL : Base_BL
    {


        M_DenominationKBN_DL mdl;
        public DenominationKBN_BL()
        {
            mdl = new M_DenominationKBN_DL();
        }

        /// <summary>
        /// Select DenominationName										
        /// </summary>
        /// <param name="mme"></param>
        /// <returns></returns>
        public DataTable BindKbn(M_DenominationKBN_Entity me, int kbn)
        {
            DataTable dt = mdl.M_DenominationKBN_SelectAll(me);
            int count = dt.Columns.Count;

            if (kbn == 1)
            {
                //DataRow[] rows = dt.Select("SystemKBN NOT IN (1,2,3,5,8,9)");     2019/12/20 "1:現金,2:カード,3:ポイント,4:商品券,5:振込,6:小切手,7:相殺,8:値引,9:掛,10:Paypay
                 DataRow[] rows = dt.Select("SystemKBN NOT IN (1,2,3,8,9)");

                for (int i = 0; i < rows.Length; i++)
                {
                    if (rows[i].RowState != DataRowState.Deleted)
                    {
                        rows[i].Delete();
                    }
                }
            }

            ArrayList arrlst = new ArrayList();
            arrlst.Add("DenominationCD");
            arrlst.Add("DenominationName");
            //arrlst.Add("Char1");

            for (int i = 0; i < count; i++)
            {
                if (!arrlst.Contains(dt.Columns[i].ColumnName))
                {
                    dt.Columns.RemoveAt(i);
                    i--;
                    count--;
                    dt.AcceptChanges();
                }
            }

            return dt;
        }
        public bool M_DenominationKBN_Select(M_DenominationKBN_Entity mde)
        {
            DataTable dt = mdl.M_DenominationKBN_Select(mde);
            if (dt.Rows.Count > 0)
            {
                //mde.ChangeDate = dt.Rows[0]["ChangeDate"].ToString();
                mde.DenominationName = dt.Rows[0]["DenominationName"].ToString();
                mde.SystemKBN = dt.Rows[0]["SystemKBN"].ToString();
                mde.CardCompany = dt.Rows[0]["CardCompany"].ToString();
                mde.CalculationKBN = dt.Rows[0]["CalculationKBN"].ToString();
                mde.MainFLG = dt.Rows[0]["MainFLG"].ToString();

                return true;
            }
            else
                return false;

        }



    }
}
