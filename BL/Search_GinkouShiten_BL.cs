using Entity;
using DL;
using System.Data;

namespace BL
{
    public class Search_GinkouShiten_BL : Base_BL
    {
        M_BankShiten_DL mbsdl;
        public Search_GinkouShiten_BL()
        {
            mbsdl = new M_BankShiten_DL();
        }

        public DataTable M_BankShiten_Search(M_BankShiten_Entity mse)
        {
            return mbsdl.M_BankShiten_Search(mse);
        }
    }
}
