using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
   public class M_StorePoint_DL:Base_DL
    {
        public DataTable M_StorePoint_Select(M_StorePoint_Entity mspe)
        {
            string sp = "M_StorePoint_Select";
            //KTP 2019-0529 全部のFunctionでをしなくてもいいように共通のFunctionでやり方を更新しました。
            //command = new SqlCommand(sp, GetConnection());
            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandTimeout = 0;

            //command.Parameters.Add("@KouzaCD", SqlDbType.VarChar).Value = mke.KouzaCD;
            //command.Parameters.Add("@ChangeDate", SqlDbType.VarChar).Value = mke.ChangeDate;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.StoreCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.ChangeDate } },
            };

            //return SelectData(sp);
            return SelectData(dic, sp);
        }

        public bool M_StorePoint_Insert_Update(M_StorePoint_Entity mspe)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.StoreCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = mspe.ChangeDate } },
                { "@PointRate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.PointRate } },
                { "@ServicedayRate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.ServicedayRate } },
                { "@ExpirationDate", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.ExpirationDate } },
                { "@MaxPoint", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.MaxPoint } },
                { "@TicketUnit", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.TicketUnit } },

                { "@Print1", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.Print1 } },
                { "@Size1", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Size1 } },
                { "@Bold1", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Bold1 } },

                { "@Print2", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.Print2 } },
                { "@Size2", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Size2 } },
                { "@Bold2", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Bold2 } },

                { "@Print3", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.Print3 } },
                { "@Size3", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Size3 } },
                { "@Bold3", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Bold3 } },

                { "@Print4", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.Print4 } },
                { "@Size4", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Size4 } },
                { "@Bold4", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Bold4 } },

                { "@Print5", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.Print5 } },
                { "@Size5", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Size5 } },
                { "@Bold5", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Bold5 } },

                { "@Print6", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.Print6 } },
                { "@Size6", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Size6 } },
                { "@Bold6", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Bold6 } },

                { "@Print7", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.Print7 } },
                { "@Size7", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Size7 } },
                { "@Bold7", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Bold7 } },

                { "@Print8", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.Print8 } },
                { "@Size8", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Size8 } },
                { "@Bold8", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Bold8 } },

                { "@Print9", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.Print9 } },
                { "@Size9", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Size9 } },
                { "@Bold9", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Bold9 } },

                { "@Print10", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.Print10 } },
                { "@Size10", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Size10 } },
                { "@Bold10", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Bold10 } },

                { "@Print11", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.Print11 } },
                { "@Size11", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Size11 } },
                { "@Bold11", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Bold11 } },

                { "@Print12", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.Print12 } },
                { "@Size12", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Size12 } },
                { "@Bold12", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.Bold12 } },

                { "@DeleteFlg", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mspe.DeleteFlg } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.InsertOperator } },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.ProgramID } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.PC } },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.ProcessMode } },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mspe.Key } },

            };

            UseTransaction = true;
            return InsertUpdateDeleteData(dic, "M_StorePoint_Insert_Update");
        }
    }
}
