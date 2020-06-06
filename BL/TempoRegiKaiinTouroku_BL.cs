using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;

namespace BL
{
   public class TempoRegiKaiinTouroku_BL : Base_BL
    {
        M_Customer_DL tprg_Kaiin_DL;
        M_ZipCode_DL mzdl;
        Base_BL bbl=new Base_BL();
        public TempoRegiKaiinTouroku_BL()
        {
            tprg_Kaiin_DL = new M_Customer_DL();
            mzdl = new M_ZipCode_DL();
        }
        public M_Customer_Entity M_Customer_Select(M_Customer_Entity m_cust)
        {
            DataTable dtCustomer = tprg_Kaiin_DL.M_Customer_SelectAll(m_cust);
            if(dtCustomer.Rows.Count>0)
            {
                m_cust.DeleteFlg = dtCustomer.Rows[0]["DeleteFlg"].ToString();
                m_cust.StoreKBN = dtCustomer.Rows[0]["StoreKBN"].ToString();
                m_cust.CustomerCD = dtCustomer.Rows[0]["CustomerCD"].ToString();
                m_cust.FirstName = dtCustomer.Rows[0]["FirstName"].ToString();
                m_cust.LastName = dtCustomer.Rows[0]["LastName"].ToString();
                m_cust.GroupName= dtCustomer.Rows[0]["GroupName"].ToString();
                m_cust.KanaName= dtCustomer.Rows[0]["KanaName"].ToString();
                m_cust.Sex= dtCustomer.Rows[0]["Sex"].ToString();
                m_cust.Birthdate= dtCustomer.Rows[0]["BirthDate"].ToString();
                m_cust.TelephoneNo1= dtCustomer.Rows[0]["Tel11"].ToString();
                m_cust.TelephoneNo2 = dtCustomer.Rows[0]["Tel12"].ToString();
                m_cust.TelephoneNo3 = dtCustomer.Rows[0]["Tel13"].ToString();
                m_cust.HomephoneNo1= dtCustomer.Rows[0]["Tel21"].ToString();
                m_cust.HomephoneNo2 = dtCustomer.Rows[0]["Tel22"].ToString();
                m_cust.HomephoneNo3 = dtCustomer.Rows[0]["Tel23"].ToString();
                m_cust.MailAddress= dtCustomer.Rows[0]["MailAddress"].ToString();
                //m_cust.MailAddress2 = dtCustomer.Rows[0]["MailAddress2"].ToString();
                m_cust.DMFlg= dtCustomer.Rows[0]["DMFlg"].ToString();
                m_cust.DeleteFlg= dtCustomer.Rows[0]["DeleteFlg"].ToString();
                m_cust.ZipCD1= dtCustomer.Rows[0]["ZipCD1"].ToString();
                m_cust.ZipCD2 = dtCustomer.Rows[0]["ZipCD2"].ToString();
                m_cust.Address1= dtCustomer.Rows[0]["Address1"].ToString();
                m_cust.Address2 = dtCustomer.Rows[0]["Address2"].ToString();

                return m_cust;
            }
            return null;
        }
        public bool M_Customer_Insert_Update (M_Customer_Entity cust,int mode)
        {
            return tprg_Kaiin_DL.M_Customer_Insert_Update(cust, mode);

        }
        public bool M_ZipCode_Select(M_ZipCode_Entity mze)
        {
            if (mzdl.M_ZipCode_Select(mze).Rows.Count > 0)
                return true;
            return false;
        }
        public DataTable M_ZipCode_AddressSelect(M_ZipCode_Entity mze)
        {
            return mzdl.M_ZipCode_Select(mze);
        }
    }
}
