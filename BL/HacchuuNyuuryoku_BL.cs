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
    public class HacchuuNyuuryoku_BL : Base_BL
    {
        D_Hacchu_DL mdl;
        public HacchuuNyuuryoku_BL()
        {
            mdl = new D_Hacchu_DL();
        }

        /// <summary>
        /// 発注番号検索にて使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Order_SelectAll(D_Order_Entity de, M_SKU_Entity mse)
        {
            return mdl.D_Order_SelectAll(de, mse);
        }

        /// <summary>
        /// 発注入力更新処理
        /// HacchuuNyuuryokuより更新時に使用
        /// </summary>
        public bool Order_Exec(D_Order_Entity dme, DataTable dt, short operationMode, string operatorNm, string pc)
        {
            return mdl.D_Order_Exec(dme, dt, operationMode, operatorNm, pc);
        }

        /// <summary>
        /// 発注入力取得処理
        /// HacchuuchuuNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Order_SelectData(D_Order_Entity de, short operationMode)
        {
            DataTable dt = mdl.D_Order_SelectData(de, operationMode);

            return dt;
        }

        /// <summary>
        /// 進捗チェック　
        /// 既に出荷済み,出荷指示済み,ピッキングリスト完了済み,仕入済み,入荷済み警告
        /// </summary>
        /// <param name="hacchuNo"></param>
        /// <param name="errno"></param>
        /// <returns></returns>
        public bool CheckHacchuData(string hacchuNo, out string errno)
        {
            DataTable dt = mdl.CheckHacchuData(hacchuNo);
            
            errno = "";

            if (dt.Rows.Count>0)
            {
                errno = dt.Rows[0]["errno"].ToString();
            }
            
            return true;
        }
        public bool CheckSyonin(string hacchuNo, string InOperatorCD, out string errno)
        {
            DataTable dt = mdl.CheckSyonin(hacchuNo, InOperatorCD);
            
            errno = "";

            if (dt.Rows.Count > 0)
            {
                errno = dt.Rows[0]["errno"].ToString();
            }

            return true;
        }
        public int GetApprovalStageFLG(string operatorNm, string storeCD = "")
        {
            int approvalStageFLG = 0;

            M_Store_DL dl = new M_Store_DL();
            DataTable dt = dl.GetApprovalData(operatorNm, storeCD);

            if (dt.Rows.Count > 0)
            {
                //ApprovalStaffCD11かApprovalStaffCD12で条件合致していたら、値を3とする
                if (dt.Rows[0]["ApprovalStaffCD11"].ToString() == operatorNm
                    || dt.Rows[0]["ApprovalStaffCD12"].ToString() == operatorNm)
                {
                    if (string.IsNullOrWhiteSpace(dt.Rows[0]["ApprovalStaffCD21"].ToString())
                    && string.IsNullOrWhiteSpace(dt.Rows[0]["ApprovalStaffCD22"].ToString())
                    && string.IsNullOrWhiteSpace(dt.Rows[0]["ApprovalStaffCD31"].ToString())
                    && string.IsNullOrWhiteSpace(dt.Rows[0]["ApprovalStaffCD32"].ToString()))
                        approvalStageFLG = 9;
                    else
                        approvalStageFLG = 3;
                }
                //ApprovalStaffCD11	かApprovalStaffCD12で条件合致していたら、値を5とする
                else if (dt.Rows[0]["ApprovalStaffCD21"].ToString() == operatorNm
                   || dt.Rows[0]["ApprovalStaffCD22"].ToString() == operatorNm)
                {
                    if (string.IsNullOrWhiteSpace(dt.Rows[0]["ApprovalStaffCD31"].ToString())
                    && string.IsNullOrWhiteSpace(dt.Rows[0]["ApprovalStaffCD32"].ToString()))
                        approvalStageFLG = 9;
                    else
                        approvalStageFLG = 5;
                }
                else
                {               
                        approvalStageFLG = 9;
                }
            }

            return approvalStageFLG;
        }
        public DataTable M_Souko_Select(M_Souko_Entity mse)
        {
            M_Souko_DL msdl = new M_Souko_DL();
            return msdl.M_Souko_SelectForNyuuka(mse);
        }
    }
}
