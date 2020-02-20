﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using BL;
using Entity;

namespace Search
{

    public partial class TempoRegiKaiinKensaku : ShopBaseForm
    {
        TempoRegiKaiinKensaku_BL trkkkBL = new TempoRegiKaiinKensaku_BL();
        M_Customer_Entity Customer = new M_Customer_Entity();
        DataTable dt;
        public string CustomerCD = "";
        public string CustomerName = "";

        public TempoRegiKaiinKensaku()
        {
            InitializeComponent();
        }
        //    public TempoRegiKaiinKensaku()
        //{
        //    InitializeComponent();
            
        //}

        private void TempoRegiKaiinKensaku_Load(object sender, EventArgs e)
        {
            InProgramID = "TempoRegiKaiinKensaku";

            //StartProgram();

            txtZipCD.Focus();

            dgvKaniiKensaku.RowHeadersVisible = false;
        }

        protected override void EndSec()
        {
            this.Close();
        }

        public override void FunctionProcess(int index)
        {
            switch (index + 1)
            {
                case 2:
                    ExecSec();
                    break;
            }
        }

        private void ShowDetail()
        {
            Customer = GetCustomer();
            {
                dt = new DataTable();
                dt = trkkkBL.M_Customer_Display(Customer);
                if (dt != null)
                {
                    dgvKaniiKensaku.DataSource = dt;
                    txtZipCD.Focus();
                }
            }
        }

        private M_Customer_Entity GetCustomer()
        {
            Customer = new M_Customer_Entity()
            {
                TelephoneNo1 = txtZipCD.Text,
                Birthdate = txtDBO.Text,
                CustomerCD = txtCustomerCD.Text,
                KanaName = txtKanaName.Text,
                CustomerName = txtCustomerName.Text
            };
            return Customer;
        }

        private void dgvKaniiKensaku_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                ExecSec();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
       
        private void ExecSec()
        {
            CustomerCD = dgvKaniiKensaku.CurrentRow.Cells["colCustomerCD"].Value.ToString().Replace(System.Environment.NewLine, string.Empty);
            CustomerName = dgvKaniiKensaku.CurrentRow.Cells["colCustomerName"].Value.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None)[0];
            this.Close();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            ShowDetail();
        }

        private void TempoRegiKaiinKensaku_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
