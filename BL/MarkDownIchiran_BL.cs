using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;

namespace BL
{
    public class MarkDownIchiran_BL : Base_BL
    {
        D_MarkDown_DL mdl;
        public MarkDownIchiran_BL()
        {
            mdl = new D_MarkDown_DL();
        }

        /// <summary>
        /// マークダウン一覧にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_MarkDown_SelectAll(D_MarkDown_Entity dme)
        {
            return mdl.D_MarkDown_SelectAll(dme);
        }
    }
}
