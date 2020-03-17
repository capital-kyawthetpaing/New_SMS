using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Entity
{
    public class Base_Entity
    {
        public string ID {get;set;}
        public string TableName { get; set; }
        public string FieldsName { get; set; }
        public string ChangeDate { get; set; }
        public string Condition { get; set; }
        public string InsertOperator { get; set; }
        public string InsertDateTime { get; set; } 
        public string UpdateOperator { get; set; }
        public string UpdateDateTime { get; set; }
        public string DeleteFlg { get; set; }
        public string UsedFlg { get; set; }
        public string ProcessMode { get; set; }
        public string Key { get; set; }
        public string ProgramID { get; set; }

        public string Operator { get; set; }
        public string PC { get; set; }
        public string OrderBy { get; set; }
        public DataTable dtTemp1 { get; set; }
        public DataTable dtTemp2 { get; set; }
        public DataTable dtTemp3 { get; set; }

        public DataTable dtTemp4 { get; set; }
        public DataTable dtTemp5 { get; set; }
        public DataTable dtTemp6 { get; set; }
        public string searchType { get; set; }

        public DataTable dt1 { get; set; }
        public DataTable dt2 { get; set; }
        public DataTable dt3 { get; set; }
        public DataTable dt4 { get; set; }
        public DataTable dt5 { get; set; }
        public DataTable dt6 { get; set; }

        public string xml1 { get; set; }
        public string xml2 { get; set; }
        public string xml3 { get; set; }
        public string xml4 { get; set; }
        public string xml5 { get; set; }
        public string xml6 { get; set; }
    }
}
