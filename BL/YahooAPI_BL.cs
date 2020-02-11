using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using Entity;
using System.Data;

namespace BL
{
    public class YahooAPI_BL : Base_BL
    {
        M_MultiPorpose_DL MmultiporposeDataDL = new M_MultiPorpose_DL();
        M_API_DL apiDL = new M_API_DL();
        D_YahooAPI_DL D_API_DL = new D_YahooAPI_DL();
        string FieldsName = string.Empty;
        string TableName = string.Empty;
        string Condition = string.Empty;

        public DataTable M_MultiPorpose_SelectID(M_MultiPorpose_Entity MmultiporposeData)
        {
            return MmultiporposeDataDL.M_MultiPorpose_Select(MmultiporposeData);
        }

        public DataTable D_APIControl_Select(D_APIControl_Entity DApiControl_entity)
        {
            return SimpleSelect1("12",string.Empty, DApiControl_entity.APIKey);
        }

        public DataTable M_API_Select(D_APIControl_Entity DApiControl_entity)
        {
            return SimpleSelect1("13",string.Empty, DApiControl_entity.APIKey);
        }

        public bool InsertRefreshToken(M_API_Entity api)
        {
            return apiDL.M_API_InsertRefreshToken_Yahoo(api);
        }


        public bool D_APIRireki_D_YahooCount_Insert(D_APIRireki_Entity apiRireki, D_YahooCount_Entity yahooCount)
        {
            return D_API_DL.D_APIRireki_D_YahooCount_Insert(apiRireki,yahooCount);
        }


        public bool D_APIDetail_YahooList(D_APIRireki_Entity d_API, DataTable dtorder)
        {
            var dt =  dtorder.DataSet.Tables[2].Copy();
            //foreach (DataRow dr in dtorder.Rows)
            //{
            //    dt.ImportRow(dr);
            //}
            
            return D_API_DL.D_APIDetail_YahooList(d_API, DataTableToXml(dt));
        }

        public bool ImportYahooJuuChuu(DataTable YJ )
        {
            string dtcol = "OrderTime,LastUpdateTime,RoyaltyFixTime,SendConfirmTime,SendPayTime,PrintSlipTime,PrintDeliveryTime,PrintBillTime,PayActionTime,PayDate,PayNoIssueDate,PaymentTerm";

            for (int i = 0; i < YJ.Rows.Count; i++)
            {
                for (int j = 0; j < YJ.Columns.Count; j++)
                {
                    if (dtcol.Split(',').Contains(YJ.Columns[j].ColumnName))
                    {
                        var m = YJ.Rows[i][j].ToString();
                        try
                        {
                            var oDate = Convert.ToDateTime(m).ToString();
                            YJ.Rows[i][j] = oDate;


                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            return D_API_DL.ImportYahooJuuChuu(DataTableToXml(YJ));

        }
        public bool ImportYahooShipping(DataTable YJ)
        {
            string dtcol = "ShipRequestDate,ShipDate,ArrivalDate";

            for (int i = 0; i < YJ.Rows.Count; i++)
            {
                for (int j = 0; j < YJ.Columns.Count; j++)
                {
                    if (dtcol.Split(',').Contains(YJ.Columns[j].ColumnName))
                    {
                        var m = YJ.Rows[i][j].ToString();
                        try
                        {
                            var oDate = Convert.ToDateTime(m).ToString();
                            YJ.Rows[i][j] = oDate;


                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            return D_API_DL.ImportYahooShipping(DataTableToXml(YJ));
        }
        public bool ImportYahooShippingDetail(DataTable YJ)
        {
            string dtcol = "LeadTimeStart,LeadTimeEnd,GetPointFixDate,ReleaseDate"; //  date Cols
            string money_decimal = "AffiliateRatio_Store,AffiliateRatio_Yahoo,UnitPrice,CouponDiscount,OriginalPrice"; // ItemOption01Price    **** Money, decimal, int
            for (int i = 0; i < YJ.Rows.Count; i++)
            {
                for (int j = 0; j < YJ.Columns.Count; j++)
                {
                    if (money_decimal.Split(',').Contains(YJ.Columns[j].ColumnName))
                    {
                        YJ.Rows[i][j] = string.IsNullOrWhiteSpace(YJ.Rows[i][j].ToString()) ? "0" : YJ.Rows[i][j].ToString();
                    }
                }

            }
                    for (int i = 0; i < YJ.Rows.Count; i++)
            {
                for (int j = 0; j < YJ.Columns.Count; j++)
                {
                    if (dtcol.Split(',').Contains(YJ.Columns[j].ColumnName))
                    {
                        var m = YJ.Rows[i][j].ToString();
                        try
                        {
                            var oDate = Convert.ToDateTime(m).ToString();
                            YJ.Rows[i][j] = oDate;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            //param.Value = !string.IsNullOrEmpty(activity.StaffId) ? activity.StaffId : (object)DBNull.Value;
            return D_API_DL.ImportYahooShippingDetail(DataTableToXml(YJ));
        }
        //ImportYahooShippingDetail
    }
}
