namespace Entity
{
    /// <summary>
    /// 店舗レジ領収書印刷Entity
    /// </summary>
    public class D_TempoRegiRyousyuusyo_Entity : Base_Entity
    {
        /// <summary>
        /// 売上番号
        /// </summary>
        public string SalesNO { get; set; }

        //public string InsertDateTime { get; set; }

        /// <summary>
        /// 相手名
        /// </summary>
        public string AiteName { get; set; }

        /// <summary>
        /// 売上金額
        /// </summary>
        public string SalesGaku { get; set; }

        /// <summary>
        /// うち消費税
        /// </summary>
        public string SalesTax { get; set; }

        /// <summary>
        /// 会社名
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 店舗名
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// 住所１
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// 住所２
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// 電話番号
        /// </summary>
        public string TelphoneNO { get; set; }

        /// <summary>
        /// 店舗表記 - スタッフ表記
        /// </summary>
        public string ReceiptPrint { get; set; }

        /// <summary>
        /// 領収書コメント１
        /// </summary>
        public string Char1 { get; set; }

        /// <summary>
        /// 領収書コメント２
        /// </summary>
        public string Char2 { get; set; }
    }
}
