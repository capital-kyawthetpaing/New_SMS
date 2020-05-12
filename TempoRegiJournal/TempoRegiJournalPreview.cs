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

            crystalReportViewer.ReportSource = report;
        }
    }
}
