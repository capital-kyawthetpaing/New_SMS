using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_Move_Entity : Base_Entity
    {
        public string MoveNO { get; set; }
        public string StoreCD { get; set; }
        public string MoveDate { get; set; }
        public string RequestNO { get; set; }
        public string MovePurposeKBN { get; set; }
        public string FromSoukoCD { get; set; }
        public string ToStoreCD { get; set; }
        public string ToSoukoCD { get; set; }
        public string MoveInputDateTime { get; set; }
        public string StaffCD { get; set; }

        //移動入力用
        public string MovePurposeType { get; set; }
        public string FromStoreCD { get; set; }
    public int MoveRows { get; set; }
        public int OldMoveSu { get; set; }
        //検索用Entity
        public string MoveDateFrom { get; set; }
        public string MoveDateTo { get; set; }
    }
}
