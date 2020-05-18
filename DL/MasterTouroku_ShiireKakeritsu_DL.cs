﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
   public class MasterTouroku_ShiireKakeritsu_DL:Base_DL
    {
        public DataTable MasterTouroku_ShiireKakeritsu_Select(M_OrderRate_Entity moe)
        {
            string sp = "M_ShiireKakeritsu";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                {"@VendorCD", new ValuePair { value1 = System.Data.SqlDbType.VarChar, value2 =moe.VendorCD  } },
                //{"@StoreCD",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.StoreCD} },
                //{"@BrandCD",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.BrandCD} },
                //{"@SportsCD",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.SportsCD} },
                //{"@SegmentCD",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.SegmentCD} },
                //{"@LastSeason",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.LastSeason} },
                //{"@ChangeDate",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.ChangeDate} },
                //{"@Rate",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.Rate} }
            };
            return SelectData(dic, sp);
        }
        public bool M_Shiirekakeritsu(M_OrderRate_Entity moe,string Xml)
        {
            string sp = "M_OrderRate_Update";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Xml", new ValuePair { value1 = SqlDbType.VarChar, value2 = Xml } },
                { "@VendorCD", new ValuePair { value1 = System.Data.SqlDbType.VarChar, value2 =moe.VendorCD  } },
                { "@StoreCD",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.StoreCD} },
                { "@BrandCD",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.BrandCD} },
                { "@SportsCD",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.SportsCD} },
                { "@SegmentCD",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.SegmentCD} },
                { "@LastYearTerm",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.LastYearTerm} },
                { "@LastSeason",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.LastSeason} },
                { "@ChangeDate",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.ChangeDate} },
                { "@Rate",new ValuePair{value1=System.Data.SqlDbType.VarChar,value2=moe.Rate} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = moe.Operator }},
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = moe.ProgramID }},
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = moe.PC }},
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = moe.ProcessMode }},
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = moe.Key }}
            };

            return InsertUpdateDeleteData(dic, sp);
        }
    }
}
