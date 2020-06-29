using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;


namespace BL
{
    public class SKSMasterUpdate_BL: Base_BL
    {
        M_ITEM_DL mitem_dl;
        M_SKU_DL msku_dl;
        public SKSMasterUpdate_BL()
        {
            mitem_dl = new M_ITEM_DL();
            msku_dl = new M_SKU_DL();
        }

        public DataTable SKS_Item_Select()
        {
            return mitem_dl.M_Item_SelectForSKSMasterUpdate();

        }

        public DataTable SKS_SKU_Select()
        {
            return msku_dl.M_SKU_SelectForSKSMasterUpdate();

        }

        public bool SKSUpdateFlg_ForItem(DataTable dtMasterItem,DataTable dtMasterSKU)
        {
            string xmlMasterItem = DataTableToXml(dtMasterItem);
            string xmlMasterSKU = DataTableToXml(dtMasterSKU);
            return mitem_dl.M_ITem_SKSUpdateFlg(xmlMasterItem, xmlMasterSKU);
        }
    }
}
