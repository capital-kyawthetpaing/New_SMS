using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_Settlement_Entity : Base_Entity
    {
        public string PatternCD { get; set; }
        public string PatternName { get; set; }
        public string FileKBN { get; set; }
        public string FirstRecordNo { get; set; }
        public string RecordKBN { get; set; }
        public string MatchingKBN { get; set; }

    }
}
