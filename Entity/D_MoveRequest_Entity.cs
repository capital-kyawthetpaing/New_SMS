using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_MoveRequest_Entity : Base_Entity
    {
        public string RequestNO { get; set; }
        public string StoreCD { get; set; }
        public string RequestDate { get; set; }
        public string MovePurposeKBN { get; set; }
        public string FromStoreCD { get; set; }
        public string FromSoukoCD { get; set; }
        public string ToStoreCD { get; set; }
        public string ToSoukoCD { get; set; }
        public string RequestInputDateTime { get; set; }
        public string StaffCD { get; set; }
        public string AnswerDateTime { get; set; }
        public string AnswerStaffCD { get; set; }

        //照会用Entity
        public string AnswerDateFrom { get; set; }
        public string AnswerDateTo { get; set; }
        public short AnswerKBN { get; set; }

        //移動依頼入力用
    public int RequestRows { get; set; }
        public int OldRequestSu { get; set; }
    }
}
