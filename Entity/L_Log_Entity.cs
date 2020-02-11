using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class L_Log_Entity : Base_Entity
    {
        public string Program { get; set; }
        public string PC { get; set; }
        public string OperateMode { get; set; }
        public string KeyItem { get; set; }

    }

    /// <summary>
    /// INIファイル情報
    /// </summary>
    /// 
    public class Ini_Entity
    {
        public string FilePath { get; set; }
        public string DatabaseServer { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseLoginID { get; set; }
        public string DatabasePassword { get; set; }
        public string TimeoutValues { get; set; }

        /// <summary>
        /// データベースの日付　YYYY/MM/DD形式
        /// </summary>
        public string DatabaseDate { get; set; }

        public string Login_Type { get; set; }
    }
}
