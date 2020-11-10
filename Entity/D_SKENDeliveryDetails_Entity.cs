using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class D_SKENDeliveryDetails_Entity : Base_Entity
    {
        public string    SKENBangouB             { get; set; }
        public string    ImportDateTime          { get; set; }
        public string    StaffCD                 { get; set; }
        public string    VendorCD                { get; set; }
        public string    ImportFile              { get; set; }
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
        public string    SKENNouhinshoNOGyou     { get; set; }
        public string    SKENNouhinshoNORetsu    { get; set; }
        public string    SKENDenpyouKBN          { get; set; }
        public string    SKENJuchuuDate          { get; set; }
        public string    SKENSyukkaDate          { get; set; }
        public string    SKENNouhinDate          { get; set; }
        public string    SKENHacchuu             { get; set; }
        public string    SKENHacchuuKBN          { get; set; }
        public string    SKENHacchuuShouhinCD    { get; set; }
        public string    SKENNouhinHinban        { get; set; }
        public string    SKENMakerKikaku1        { get; set; }
        public string    SKENMakerKikaku2        { get; set; }
        public string    SKENTani                { get; set; }
        public string    SKENTorihikiTanka       { get; set; }
        public string    SKENHyoujyunJyoudai     { get; set; }
        public string    SKENBrandmei            { get; set; }
        public string    SKENSyouhinmei          { get; set; }
        public string    SKENJanCD               { get; set; }
        public string    AdminNO                 { get; set; }
        public string    SKENNouhinSuu           { get; set; }
        public string    SKENMakerDenpyou        { get; set; }
        public string    SKENMotoDenNO           { get; set; }
        public string    SKENYobi1               { get; set; }
        public string    SKENYobi2               { get; set; }
        public string    SKENYobi3               { get; set; }
        public string    SKENYobi4               { get; set; }
        public string    SKENYobi5               { get; set; }
        public string    DeliveryNo              { get; set; }
        public string ProcessedDateTime { get; set; }

        //SKENDeliver
        public string ChkFlg { get; set; }
        public string SKENBangouA { get; set; }


    }
}
