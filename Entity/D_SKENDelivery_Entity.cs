using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_SKENDelivery_Entity : Base_Entity
    {
        public string    SKENBangouA              { get; set; }
        public string    ImportDateTime          { get; set; }
        public string    StaffCD                 { get; set; }
        public string    VendorCD                { get; set; }
        public string    ImportFile              { get; set; }
        public int ImportDetailsSu         { get; set; }
        public int ErrorSu                 { get; set; }
        public string    ErrorKBN                { get; set; }
        public string    ErrorText               { get; set; }
        public string    SKENRecordKBN           { get; set; }
        public string    SKENDataKBN             { get; set; }
        public string    SKENTorihikisakiCD      { get; set; }
        public string    SKENTorihikisakiMei     { get; set; }
        public string    SKENNouhinmotoCD        { get; set; }
        public string    SKENNouhinmotoMei       { get; set; }
        public string    SKENHanbaitenCD         { get; set; }
        public string    SKENHanbaitenMei        { get; set; }
        public string    SKENSyukkasakiCD        { get; set; }
        public string    SKENSyukkasakiMei       { get; set; }
        public string    SKENNouhinshoNO         { get; set; }
        public string    SKENDenpyouKBN          { get; set; }
        public string    SKENJuchuuDate          { get; set; }
        public string    SKENSyukkaDate          { get; set; }
        public string    SKENNouhinDate          { get; set; }
        public string    SKENHacchuu             { get; set; }
        public string    SKENHacchuuKBN          { get; set; }
        public string    SKENDenpyoua            { get; set; }
        public string    SKENDenpyoub            { get; set; }
        public string    SKENDenpyouc            { get; set; }
        public string    SKENDenpyoud            { get; set; }
        public string    SKENUnsouHouhou         { get; set; }
        public string    SKENKosuu               { get; set; }
        public string    SKENUnchinKBN           { get; set; }
        public string    SKENSyogakari           { get; set; }
        public string    SKENUnchin              { get; set; }
        public string    SKENShinadai            { get; set; }
        public string    SKENSyouhiZei           { get; set; }
        public string    SKENSougoukei           { get; set; }
        public string    SKENMakerDenpyou        { get; set; }
        public string    SKENMotoDenNO           { get; set; }
        public string    SKENYobi1               { get; set; }
        public string    SKENYobi2               { get; set; }
        public string    SKENYobi3               { get; set; }
        public string    SKENYobi4               { get; set; }
        public string    SKENYobi5               { get; set; }


        public string ProcessedDateTime { get; set; }

        public string ChkFlg { get; set; }


    }
}
