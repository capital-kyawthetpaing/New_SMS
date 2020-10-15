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
    public class IkkatuHacchuuNyuuryoku_BL : Base_BL
    {
        IkkatuHacchuuNyuuryoku_DL ikkatuHacchuuDL = new IkkatuHacchuuNyuuryoku_DL();
        
        /// <summary>
        /// 
        /// </summary>
        public IkkatuHacchuuNyuuryoku_BL()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hacchuuDate"></param>
        /// <param name="vendorCD"></param>
        /// <param name="storeCD"></param>
        /// <returns></returns>
        public DataTable PRC_IkkatuHacchuuNyuuryoku_SelectData(string ikkatuHacchuuMode, string hacchuuDate, string vendorCD, string juchuuStaffCD, string storeCD, string isSaiHacchuu)
        {
            return ikkatuHacchuuDL.PRC_IkkatuHacchuuNyuuryoku_SelectData(ikkatuHacchuuMode, hacchuuDate, vendorCD, juchuuStaffCD,storeCD, isSaiHacchuu);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hacchuuDate"></param>
        /// <param name="vendorCD"></param>
        /// <param name="storeCD"></param>
        /// <returns></returns>
        public DataTable PRC_IkkatuHacchuuNyuuryoku_SelectByOrderNO(string orderNO,string orderProcessNO)
        {
            return ikkatuHacchuuDL.PRC_IkkatuHacchuuNyuuryoku_SelectByOrderNO(orderNO, orderProcessNO);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationMode"></param>
        /// <param name="operatorID"></param>
        /// <param name="pc"></param>
        /// <param name="storeCD"></param>
        /// <param name="staffCD"></param>
        /// <param name="hacchuuDate"></param>
        /// <param name="dtTIkkatuHacchuuNyuuryoku"></param>
        /// <returns></returns>
        public bool PRC_IkkatuHacchuuNyuuryoku_Register(int operationMode, string operatorID, string pc, string storeCD, string staffCD, string hacchuuDate, string orderNO, string orderProcessNO, string ikkatuHacchuuMode, DataTable dtTIkkatuHacchuuNyuuryoku)
        {
            return ikkatuHacchuuDL.PRC_IkkatuHacchuuNyuuryoku_Register(operationMode, operatorID, pc, storeCD, staffCD, hacchuuDate, orderNO, orderProcessNO, ikkatuHacchuuMode, dtTIkkatuHacchuuNyuuryoku);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationMode"></param>
        /// <param name="operatorID"></param>
        /// <param name="pc"></param>
        /// <param name="storeCD"></param>
        /// <param name="staffCD"></param>
        /// <param name="hacchuuDate"></param>
        /// <param name="dtTIkkatuHacchuuNyuuryoku"></param>
        /// <returns></returns>
        public DataTable D_Order_SelectData_SeachHacchuuShoriNO(string storeCD, string dateFrom, string dateTo, string staffCD)
        {
            return ikkatuHacchuuDL.D_Order_SelectData_SeachHacchuuShoriNO(storeCD,dateFrom,dateTo,staffCD);
        }
    }
}
