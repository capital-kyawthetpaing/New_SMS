using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_Kouza_Entity : Base_Entity
    {
        public string KouzaCD {get;set;}
        public string KouzaName { get; set; }
        public string BankCD { get; set; }
        public string BankName { get; set; }
        public string BranchCD { get; set; }
        public string BranchName { get; set; }
        public string KouzaKBN { get; set; }
        public string KouzaMeigi { get; set; }
        public string KouzaNO { get; set; }
        public string Print1 { get; set; }
        public string Print2 { get; set; }
        public string Print3 { get; set; }
        public string Print4 { get; set; }
        public string Fee11 { get; set; }
        public string Tax11 { get; set; }
        public string Amount1 { get; set; }
        public string Fee12 { get; set; }
        public string Tax12 { get; set; }
        public string Fee21 { get; set; }
        public string Tax21 { get; set; }
        public string Amount2 { get; set; }
        public string Fee22 { get; set; }
        public string Tax22 { get; set; }
        public string Fee31 { get; set; }
        public string Tax31 { get; set; }
        public string Amount3 { get; set; }
        public string Fee32 { get; set; }
        public string Tax32 { get; set; }
        public string CompanyCD { get; set; }
        public string CompanyName { get; set; }
        public string Remarks { get; set; }

        //検索用Entity
        public string DisplayKbn { get; set; }
        public string KouzaCDFrom { get; set; }
        public string KouzaCDTo { get; set; }

        //SiharaiNyuuryoku
        public string Amount { get; set; }
    }
}
