using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
    public class D_StoreCalculation_DL:Base_DL
    {
        D_StoreCalculation_Entity dsce = new D_StoreCalculation_Entity();
        public bool D_StoreCalculation_Insert_Update(D_StoreCalculation_Entity dsce)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dsce.StoreCD } },
                { "@Change", new ValuePair { value1 = SqlDbType.Int, value2 = dsce.Change } },
                { "@10000yen", new ValuePair { value1 = SqlDbType.Int, value2 = dsce.Yen10000 } },
                { "@5000yen", new ValuePair { value1 = SqlDbType.Int, value2 = dsce.Yen5000 } },
                { "@2000yen", new ValuePair { value1 = SqlDbType.Int, value2 = dsce.Yen2000 } },
                { "@1000yen", new ValuePair { value1 = SqlDbType.Int, value2 = dsce.Yen1000 } },
                { "@500yen", new ValuePair { value1 = SqlDbType.Int, value2 = dsce.Yen500 } },
                { "@100yen", new ValuePair { value1 = SqlDbType.Int, value2 = dsce.Yen100 } },
                { "@50yen", new ValuePair { value1 = SqlDbType.Int, value2 = dsce.Yen50 } },
                { "@10yen", new ValuePair { value1 = SqlDbType.Int, value2 = dsce.Yen10 } },
                { "@5yen", new ValuePair { value1 = SqlDbType.Int, value2 = dsce.Yen5 } },
                { "@1yen", new ValuePair { value1 = SqlDbType.Int, value2 = dsce.Yen1 } },
                { "@etcyen", new ValuePair { value1 = SqlDbType.Int, value2 = dsce.EtcYen } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = dsce.InsertOperator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = dsce.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = dsce.PC } },               
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic,"D_StoreCalculation_Insert_Update");
        }

        public DataTable D_StoreCalculation_Select(D_StoreCalculation_Entity  dsce)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dsce.StoreCD } },
                { "@Date", new ValuePair { value1 = SqlDbType.Date, value2 = dsce.CalculationDate } },
            };
            UseTransaction = true;
            return SelectData(dic, "D_Store_CalculationSelect");
        }

        public DataTable D_Store_Calculation_SelectForSeiSan(D_StoreCalculation_Entity dsce)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dsce.StoreCD } },
                { "@Date", new ValuePair { value1 = SqlDbType.Date, value2 = dsce.CalculationDate } },
            };
            UseTransaction = true;
            return SelectData(dic, "D_Store_Calculation_SelectForSeiSan");
        }

        public bool D_StoreCalculation_Delete(D_StoreCalculation_Entity dsce)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dsce.StoreCD } },
                { "@Date", new ValuePair { value1 = SqlDbType.Date, value2 = dsce.CalculationDate } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = dsce.InsertOperator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = dsce.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = dsce.PC } },
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "D_Store_CalculationDelete");
        }

    }
}
