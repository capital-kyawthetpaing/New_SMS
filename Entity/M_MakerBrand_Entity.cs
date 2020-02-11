using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_MakerBrand_Entity : Base_Entity
    {
        public string BrandCD { get; set; }
        public string MakerCD { get; set; }
        public string IrregularKBN { get; set; }
        public string DataSourseMakerCD { get; set; }
        public string PatternCD { get; set; }

        //検索用Entity
        public string DisplayKbn { get; set; }
        public string BrandName { get; set; }

    }
}
