﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;

namespace BL
{
   public class MasterTouroku_TenzikaiShouhin_BL:Base_BL
   {
        MasterTouroku_TenzikaiShouhin_DL dl = new MasterTouroku_TenzikaiShouhin_DL();
       public DataTable Mastertoroku_Tenzikaishouhin_Select(M_TenzikaiShouhin_Entity mt,string mode)
       {

            return dl.Mastertoroku_Tenzikaishouhin_Select(mt,mode);
                
       }
   }
}