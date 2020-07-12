using System.Windows.Forms;
using TempoRegiJournal.Reports;

namespace TempoRegiJournal
{
    /// <summary>
    /// 店舗ジャーナル印刷プレビューウィンドウ
    /// </summary>
    public partial class TempoRegiJournalPreview : Form
    {
        /// <summary>
        /// データセット
        /// </summary>
        public StoreDataSet Store
        {
            get;
            set;
        }

        /// <summary>
        /// 店舗レジで使用するプリンター名
        /// </summary>
        public string StorePrinterName
        {
            get;
            set;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TempoRegiJournalPreview()
        {
            InitializeComponent();
        }
                          
        /// <summary>
        /// データセットをプレビューに設定
        /// </summary>
        public void SetJournalDataSet()
        {
            var report = new TempoRegiJournal_Journal();

            report.SetDataSource(Store);
            report.Refresh();
            report.PrintOptions.PrinterName = StorePrinterName;

            crystalReportViewer.ReportSource = report;
        }
    }
}
