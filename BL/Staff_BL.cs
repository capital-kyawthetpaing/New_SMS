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
    public class Staff_BL : Base_BL
    {
        M_Staff_DL msdl;

        /// <summary>
        /// constructor
        /// </summary>
        public Staff_BL()
        {
            msdl = new M_Staff_DL();
        }

        public bool M_Staff_Select(M_Staff_Entity mse)
        {
            DataTable dt = msdl.M_Staff_Select(mse);
            if (dt.Rows.Count > 0)
            {
                mse.StaffName = dt.Rows[0]["StaffName"].ToString();
                mse.StoreCD = dt.Rows[0]["StoreCD"].ToString();
                mse.StaffKana = dt.Rows[0]["StaffKana"].ToString();
                mse.BMNCD = dt.Rows[0]["BMNCD"].ToString();
                mse.MenuCD = dt.Rows[0]["MenuCD"].ToString();
                mse.KengenCD = dt.Rows[0]["AuthorizationsCD"].ToString();
                mse.StoreAuthorizationsCD = dt.Rows[0]["StoreAuthorizationsCD"].ToString();
                mse.PositionCD = dt.Rows[0]["PositionCD"].ToString();
                mse.JoinDate = dt.Rows[0]["JoinDate"].ToString();
                mse.LeaveDate = dt.Rows[0]["LeaveDate"].ToString();
                mse.Password = dt.Rows[0]["Password"].ToString();
                mse.Remarks = dt.Rows[0]["Remarks"].ToString();
                mse.DeleteFlg = dt.Rows[0]["DeleteFlg"].ToString();
                mse.UsedFlg = dt.Rows[0]["UsedFlg"].ToString();

                return true;
            }
            else
                return false;
        }
        
        public DataTable M_Staff_SelectAll(M_Staff_Entity mse)
        {
            return msdl.M_Staff_SelectAll(mse);
        }
        public DataTable BindBMN()
        {
            return SimpleSelect1("21", string.Empty, "209");
        }
        public DataTable BindMenu()
        {
            return SimpleSelect1("22");
        }
        public DataTable BindAuthorization()
        {
            return SimpleSelect1("23");
        }
        public DataTable BindStoreAuthorization()
        {
            return SimpleSelect1("24");
        }
        public DataTable BindPosition()
        {
            return SimpleSelect1("21",string.Empty,"214");
        }
    }
}
