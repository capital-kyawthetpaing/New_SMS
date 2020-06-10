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
    public class KeihiNyuuryoku_BL : Base_BL
    {
        D_Cost_DL costDL;
        M_Control_DL controlDL;
        //D_PayPlan_DL payplanDL;
        M_Staff_DL staffDL;
        M_Calendar_DL calendarDL;

        public KeihiNyuuryoku_BL()
        {
            costDL = new D_Cost_DL();
            controlDL = new M_Control_DL();
            //payplanDL = new D_PayPlan_DL();
            staffDL = new M_Staff_DL();
            calendarDL = new M_Calendar_DL();
        }

        public DataTable CostNo_Check(string ScCostNo)
        {
            return SimpleSelect1("10", string.Empty, ScCostNo);
        }

        public DataTable M_Control_RecordCheck(string recordDate)
        {
            return controlDL.M_Control_RecordCheck(recordDate);
        }
        
        public DataTable D_Cost_Display(D_Cost_Entity cost)
        {
            return costDL.D_Cost_Display(cost);
        }

        public DataTable D_Cost_Copy_Display(D_Cost_Entity cost)
        {
            return costDL.D_Cost_Copy_Display(cost);
        }

        public DataTable M_Staff_Exist(M_Staff_Entity staff)
        {
            return SimpleSelect1("41", staff.ChangeDate,staff.StaffCD);
        }

        public bool KehiNyuuryoku_Insert_Update(D_Cost_Entity cost, int mode)
        {
            return costDL.KehiNyuuryoku_Insert_Update(cost, mode);
        }

        public bool KehiNyuuryoku_Delete(D_Cost_Entity cost)
        {
            return costDL.KehiNyuuryoku_Delete(cost);
        }

        public string GetYoteibi(string KaisyuShiharaiKbn,string CustomerCD, string ChangeDate, string TyohaKbn)
        {
            DataTable dtShihraiYotei = new DataTable();
            string Yoteibi = string.Empty;
            dtShihraiYotei = calendarDL.GetYoteibi(KaisyuShiharaiKbn, CustomerCD, ChangeDate, TyohaKbn);
            if (dtShihraiYotei.Rows.Count > 0)
                Yoteibi = dtShihraiYotei.Rows[0]["Yoteibi"].ToString();
            //if (dtShihraiYotei.Rows[0]["Yoteibi"].ToString() != "0")
            //{
            //    Yoteibi = dtShihraiYotei.Rows[0]["Yoteibi"].ToString();
            //}
            return Yoteibi;
        }
    }
}
