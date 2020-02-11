using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Entity
{
    public class M_DenominationKBN_Entity : Base_Entity
    {
        public string DenominationCD { get; set; }
        public string DenominationName { get; set; }
        public string SystemKBN { get; set; }
        public string CardCompany { get; set; }
        public string CalculationKBN { get; set; }
        public string MainFLG { get; set; }

    }
}
