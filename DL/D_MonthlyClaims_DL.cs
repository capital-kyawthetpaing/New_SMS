using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;


namespace DL
{
    public class D_MonthlyClaims_DL : Base_DL
    {
        public DataTable M_StoreCheck_Select(D_MonthlyClaims_Entity dmce, int mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                 {"@StoreCD",new ValuePair{value1=SqlDbType.VarChar,value2=dmce.StoreCD} },
                 {"@YYYYMM",new ValuePair{value1=SqlDbType.VarChar,value2=dmce.YYYYMM} },
                 {"@Mode",new ValuePair{value1=SqlDbType.TinyInt,value2=mode.ToString()} }
            };
            return SelectData(dic, "M_StoreClose_Check");
        }
        public DataTable D_MonthlyClaims_Select(D_MonthlyClaims_Entity dmce)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                {"@TargetDate",new ValuePair{ value1=SqlDbType.Date,value2=dmce.TargetDate} },
                { "@CustomerCD",new ValuePair{value1=SqlDbType.VarChar,value2=dmce.CustomerCD}},
                //{"@RdoBill",new ValuePair{value1=SqlDbType.TinyInt,value2=dmce.BillType} },
                //{"@RdoSale",new ValuePair{value1=SqlDbType.TinyInt,value2=dmce.SaleType} },
                 {"@Chkdo",new ValuePair{value1=SqlDbType.TinyInt,value2=dmce.chk_do} },
                {"@StoreCD",new ValuePair{value1=SqlDbType.VarChar,value2=dmce.StoreCD} },
                {"@PrintType",new ValuePair{value1=SqlDbType.TinyInt,value2=dmce.PrintType} }
            };
            return SelectData(dic, "D_MonthlyClaims_SelectALL");
        }

        /// <summary>
        /// 月次債権計算処理
        /// GetsujiSaikenKeisanSyoriより更新時に使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public bool D_MonthlyClaims_Exec(D_MonthlyClaims_Entity de)
        {
            string sp = "PRC_GetsujiSaikenKeisanSyori";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@FiscalYYYYMM", new ValuePair { value1 = SqlDbType.Int, value2 = de.YYYYMM} },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StoreCD } },
                { "@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.Mode.ToString()} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.Operator} },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.PC} },
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }


        public DataTable UriageMotochou_PrintSelect(UriageMotochou_Entity ume)
        {
            string rpc = "RPC_UriageMotochou_PrintSelect";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                {"@FromYYYYMM",new ValuePair{value1=SqlDbType.Int,value2=ume.YYYYMMFrom}},
                //{"@ToYYYYMM",new ValuePair{value1=SqlDbType.Int,value2=ume.YYYYMMTo} },
                {"@FromDate",new ValuePair{value1=SqlDbType.Date,value2=ume.TargetDateFrom} },
                //{"@ToDate",new ValuePair{value1=SqlDbType.Date,value2=ume.TargetDateTo} },
                {"@CustomerCD",new ValuePair{value1=SqlDbType.VarChar,value2=ume.CustomerCD} },
                {"@StoreCD",new ValuePair{value1=SqlDbType.VarChar,value2=ume.StoreCD } },
                {"@chkValue",new ValuePair{value1=SqlDbType.TinyInt,value2=ume.ChkValue}}
            };
            return SelectData(dic, rpc);
        }

        public DataTable UrikakekinTairyuuHyou_DataToExport(M_StoreClose_Entity msce)
        {
            string rpc = "RPC_UrikakekinTairyuuHyou_PrintSelect";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                {"@FiscalYYYYMM",new ValuePair{value1=SqlDbType.Int,value2=msce.FiscalYYYYMM}},
                {"@StoreCD",new ValuePair{value1=SqlDbType.VarChar,value2=msce.StoreCD } },

            };
            return SelectData(dic, rpc);
        }

    }
}
