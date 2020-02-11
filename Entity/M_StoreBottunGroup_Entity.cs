using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_StoreBottunGroup_Entity : Base_Entity
    {
        public string StoreCD { get; set; }
        public string ProgramKBN { get; set; }
        public string GroupNO { get; set; }
        public string BottunName { get; set; }
        public string MasterKBN { get; set; }
        
        public string GroupXML;

        public DataTable dtGroup;
       

    }
}
