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
    public class MailPattern_BL : Base_BL
    {
        M_MailPattern_DL mmdl;

        /// <summary>
        /// constructor
        /// </summary>
        public MailPattern_BL()
        {
            mmdl = new M_MailPattern_DL();
        }

        public bool M_MailPattern_Select(M_MailPattern_Entity me)
        {
            DataTable dt = mmdl.M_MailPattern_Select(me);
            if (dt.Rows.Count > 0)
            {
                me.MailPatternCD = dt.Rows[0]["MailPatternCD"].ToString();
                me.MailPatternName = dt.Rows[0]["MailPatternName"].ToString();
                me.MailType = dt.Rows[0]["MailType"].ToString();
                me.MailKBN = dt.Rows[0]["MailKBN"].ToString();
                me.MailPriority = dt.Rows[0]["MailPriority"].ToString();
                me.MailSubject = dt.Rows[0]["MailSubject"].ToString();
                me.MailText = dt.Rows[0]["MailText"].ToString();

                return true;
            }
            else
                return false;
        }

        public DataTable M_MailPattern_SelectAll(M_MailPattern_Entity me)
        {
            return mmdl.M_MailPattern_SelectAll(me);
        }
    }
}
