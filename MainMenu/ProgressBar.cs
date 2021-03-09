using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace MainMenu
{
    public partial class ProgressBar : Form
    {
        string p = "";
        public ProgressBar(string Path)
        {
            InitializeComponent();
            //CenterToParent();
            //p = Path;
            //progressBar1.Style = ProgressBarStyle.Marquee;
        }
        private  void ProgressBar_Load(object sender, EventArgs e)
        {
            //await Task.Run(() =>
            //{
            //    FTPData ftp = new FTPData();
            //    ftp.UpdateSyncData(p,"");
            //});

            //MessageBox.Show("Now AppData Files are updated!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //Close();
        }
      
    }
}
