using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_FiscalYear_Entity : Base_Entity
    {
        public string FiscalYear { get; set; }
        public string FiscalStartDate { get; set; }
        public string FiscalEndDate { get; set; }
        public string InputPossibleStartDate { get; set; }
        public string InputPossibleEndDate { get; set; }

    }
}
