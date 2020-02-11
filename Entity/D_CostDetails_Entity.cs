using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_CostDetails_Entity : Base_Entity
    {
        public string CostNO { get; set; }
        public string CostRows { get; set; }
        public string CostCD { get; set; }
        public string Summary { get; set; }
        public string DepartmentCD { get; set; }
        public string CostGaku { get; set; }
        public string DeleteOperator { get; set; }
        public string DeleteDateTime { get; set; }
    }
}
