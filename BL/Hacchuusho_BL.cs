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
    public class Hacchuusho_BL : Base_BL
    {
        Hacchuusho_DL hacchuushoDL = new Hacchuusho_DL();
        
        /// <summary>
        /// 
        /// </summary>
        public Hacchuusho_BL()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hacchuuDate"></param>
        /// <param name="vendorCD"></param>
        /// <param name="storeCD"></param>
        /// <returns></returns>
        public DataTable PRC_Hacchuusho_D_Order_SelectByKey(string orderNO)
        {
            return hacchuushoDL.PRC_Hacchuusho_D_Order_SelectByKey(orderNO);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hacchuuDate"></param>
        /// <param name="vendorCD"></param>
        /// <param name="storeCD"></param>
        /// <returns></returns>
        public DataTable PRC_Hacchuusho_M_AutorisationCheck(string AuthorizationsCD, string ChangeDate, string ProgramID, string StoreCD)
        {
            return hacchuushoDL.PRC_Hacchuusho_M_AutorisationCheck(AuthorizationsCD, ChangeDate, ProgramID, StoreCD);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="StoreCD"></param>
        /// <param name="InsatuTaishou_Mihakkou"></param>
        /// <param name="InsatuTaishou_Saihakkou"></param>
        /// <param name="HacchuuDateFrom"></param>
        /// <param name="HacchuuDateTo"></param>
        /// <param name="Vendor"></param>
        /// <param name="Staff"></param>
        /// <param name="HacchuuNO"></param>
        /// <param name="IsPrintMisshounin"></param>
        /// <param name="IsPrintEDIHacchuu"></param>
        /// <param name="InsatuShurui_Hacchhusho"></param>
        /// <param name="InsatuShurui_NetHacchuu"></param>
        /// <param name="InsatuShurui_Chokusou"></param>
        /// <param name="InsatuShurui_Cancel"></param>
        /// <returns></returns>
        public DataTable PRC_Hacchuusho_SelectData(string StoreCD
                                                  ,bool? InsatuTaishou_Mihakkou
                                                  ,bool? InsatuTaishou_Saihakkou
                                                  ,string HacchuuDateFrom
                                                  ,string HacchuuDateTo
                                                  ,string Vendor
                                                  ,string Staff
                                                  ,string HacchuuNO
                                                  ,bool? IsPrintMisshounin
                                                  ,bool? IsPrintEDIHacchuu
                                                  ,bool? InsatuShurui_Hacchhusho
                                                  ,bool? InsatuShurui_NetHacchuu
                                                  ,bool? InsatuShurui_Chokusou
                                                  ,bool? InsatuShurui_Cancel)
        {
            return hacchuushoDL.PRC_Hacchuusho_SelectData(StoreCD
                                                        , InsatuTaishou_Mihakkou
                                                        , InsatuTaishou_Saihakkou
                                                        , HacchuuDateFrom
                                                        , HacchuuDateTo
                                                        , Vendor
                                                        , Staff
                                                        , HacchuuNO
                                                        , IsPrintMisshounin
                                                        , IsPrintEDIHacchuu
                                                        , InsatuShurui_Hacchhusho
                                                        , InsatuShurui_NetHacchuu
                                                        , InsatuShurui_Chokusou
                                                        , InsatuShurui_Cancel);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="hacchuuDate"></param>
        /// <param name="vendorCD"></param>
        /// <param name="storeCD"></param>
        /// <returns></returns>
        public DataTable PRC_Hacchuusho_SelectByOrderNO(string orderNO,string orderProcessNO)
        {
            return hacchuushoDL.PRC_Hacchuusho_SelectByOrderNO(orderNO, orderProcessNO);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatorID"></param>
        /// <param name="storeCD"></param>
        /// <param name="orderCD"></param>
        /// <returns></returns>
        public bool PRC_Hacchuusho_Register(string operatorID, string storeCD, string orderCD)
        {
            return hacchuushoDL.PRC_Hacchuusho_Register(operatorID, storeCD,orderCD);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatorID"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool PRC_Hacchuusho_UpdatePrintDate(string operatorID, DataTable dt)
        {
            return hacchuushoDL.PRC_Hacchuusho_UpdatePrintDate(operatorID, dt);
        }
    }
}
