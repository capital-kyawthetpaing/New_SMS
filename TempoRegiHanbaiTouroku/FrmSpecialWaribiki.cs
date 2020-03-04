using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;

namespace TempoRegiHanbaiTouroku
{
    public partial class FrmSpecialWaribiki : Form
    {
        Base_BL bbl = new Base_BL();
        public int btnSelect = 0;
        public string tanka = "0";

        public FrmSpecialWaribiki()
        {
            InitializeComponent();
        }

        private void ckM_Button1_Click(object sender, EventArgs e)
        {
            Select(1);
        }

        private void ckM_Button2_Click(object sender, EventArgs e)
        {
            Select(2);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Select(3);
        }
        void Select(int button)
        {
            btnSelect = button;

            int ritsu = 0;
            if(btnSelect.Equals(1))
            {
                ritsu = 20;
            }
            else if(btnSelect.Equals(2))
            {
                ritsu = 10;
            }

            ////単価×((100－割引率)　÷100)	←１円未満は四捨五入
            //tanka = bbl.Z_SetStr(bbl.Z_Set(tanka) * (100 - ritsu) / 100);
            this.Close();
        }
    }
}
