﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;
using DL;
namespace BL
{
   public class TenjikaiJuuChuu_BL : Base_BL
    {
        TenjikaiJuuChuu_DL tdl = new TenjikaiJuuChuu_DL();
        public TenjikaiJuuChuu_BL()
        {

        }
        public DataTable JuuChuuCheck(string JuuChuuBi)
        {
           return tdl.JuuChuuCheck(JuuChuuBi);
           
        }
        public DataTable ShuukaSouko(string SouKoCD, string ChangeDate)
        {
            return tdl.ShuukaSouko(SouKoCD, ChangeDate);
        }
        public DataTable M_TenjiKaiJuuChuu_Select(string xml)
        {
            return tdl.M_TenjiKaiJuuChuu_Select(xml);
        }
    }
}
