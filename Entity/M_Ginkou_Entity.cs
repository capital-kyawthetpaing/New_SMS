using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_Ginkou_Entity : Base_Entity
    {

        public string ginko_CD { get; set; }

        public string ginko_Name { get; set; }

        public string ginko_kananame { get; set; }

        public string ginko_Changedate { get; set; }

        public string cp_ginkoCD { get; set; }

        public string cp_ginkoChangedate { get; set; }

        public string ginko_insertOperator { get; set; }

        public string ginko_DeleteFlag { get; set; }

        public string ginko_remarks { get; set; }

        public string ginko_useflag { get; set; }

        public string Program { get; set; }

        public string OperateMode { get; set; }

        public string KeyItem { get; set; }

    }
}
