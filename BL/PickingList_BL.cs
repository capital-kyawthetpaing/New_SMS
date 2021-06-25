using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using Entity;
using System.Data;

namespace BL
{
   public class PickingList_BL:Base_BL
    {
        D_Picking_DL dpdl;

        public PickingList_BL()
        {
            dpdl = new D_Picking_DL();
        }

        public DataTable Pickinglist_Select(string PickingNO)
        {
            return dpdl.Pickinglist_Select(PickingNO);
        }

        public DataTable PickingList_InsertUpdateSelect_Check1(D_Picking_Entity dpe)
        {
            return dpdl.PickingList_InsertUpdateSelect_Check1(dpe);
        }

        public DataTable PickingList_InsertUpdateSelect_Check2(D_Picking_Entity dpe)
        {
            return dpdl.PickingList_InsertUpdateSelect_Check2(dpe);
        }

        public DataTable PickingList_InsertUpdateSelect_Check3(D_Picking_Entity dpe)
        {
            return dpdl.PickingList_InsertUpdateSelect_Check3(dpe);
        }

        public void D_Picking_Update(string pickingNo,D_Picking_Entity dpe)
        {
            dpdl.D_Picking_Update(pickingNo, dpe);
        }


    }
}
