using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_Authorizations_Entity : Base_Entity
    {
        public string AuthorizationsCD {get;set;}
        public string AuthorizationsName { get; set; }

        #region to AccessCheck
        public string StaffCD { get; set; }
        public string ProgramID { get; set; }
        #endregion

        #region for L_Log
        public string PC { get; set; }
        #endregion

        #region for Message
        public string MessageID { get; set; }
        #endregion
    }
}
