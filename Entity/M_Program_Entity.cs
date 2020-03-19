using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_Program_Entity : Base_Entity
    {
        public string ProgramID {get;set;}
        public string ProgramName {get;set;}
        public string Type { get; set; }
        public string ProgramEXE { get; set; }
        public string FileDrive { get; set; }
        public string FilePass { get; set; }
        public string FileName { get; set; }

        //ses--MasterTouroku_Program
        public string Program_ID { get; set; }
    }
}
