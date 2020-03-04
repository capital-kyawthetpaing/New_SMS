using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_Brand_Entity : Base_Entity
    {
        public string BrandCD { get; set; }
        public string BrandName { get; set; }
        public string BrandKana { get; set; }

        //検索用Entity
        public string DisplayKbn { get; set; }
        public string MakerCD { get; set; }

    }
}
