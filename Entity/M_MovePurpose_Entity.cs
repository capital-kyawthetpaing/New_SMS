using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_MovePurpose_Entity : Base_Entity
    {
        public string MovePurposeKBN { get; set; }
        public string MovePurposeName { get; set; }
        /// <summary>
        /// 表示順
        /// </summary>
        public string DisplayOrder { get; set; }
        /// <summary>
        /// 移動依頼入力対象
        /// </summary>
        public string MovePurposeType { get; set; }
        public string MoveRequestFLG { get; set; }
        /// <summary>
        /// 移動入力対象
        /// </summary>
        public string MoveFLG { get; set; }
        /// <summary>
        /// 移動先倉庫必須 1:必須
        /// </summary>
        public string ToSoukoFLG { get; set; }
        /// <summary>
        /// メール送信 1:必要
        /// </summary>
        public string MailFLG { get; set; }

    }
}
