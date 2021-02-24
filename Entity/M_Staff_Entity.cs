using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class M_Staff_Entity : Base_Entity
    {

        public string StaffCD { get; set; }

        //KTP 2019-05-29　BaseEntity ほかの画面が使うこともあるので、BaseEntity に入れました。
        //public string ChangeDate { get; set; }
        public string StaffName { get; set; }
        public string StaffKana { get; set; }
        public string StoreCD { get; set; }
        public string SoukoCD { get; set; }
        public string BMNCD { get; set; }
        public string MenuCD { get; set; }
        public string KengenCD { get; set; }
        public string StoreAuthorizationsCD { get; set; }
        public string PositionCD { get; set; }
        public string JoinDate { get; set; }
        public string LeaveDate { get; set; }
        public string Password { get; set; }
        public string Remarks { get; set; }
        public string ReceiptPrint { get; set; }
        //KTP 2019-05-29　BaseEntity ほかの画面が使うこともあるので、BaseEntity に入れました。
        //public string DeleteFlg { get; set; }
        //public string UsedFlg { get; set; }

        //for Authorization Check
        public string ProgramID { get; set; }

        //for Initialize
        public string SysDate { get; set; }
        public string CompanyCD { get; set; }

        //検索用Entity
        public string DisplayKbn { get; set; }
        public string StaffCDFrom { get; set; }
        public string StaffCDTo { get; set; }
        public string StoreMenuCD { get; set; }
        public string StoreName { get; set; }

    }
}
