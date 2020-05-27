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
using Entity;
using Base.Client;
using CKM_Controls;

namespace TempoRejiPointSettei
{
    public partial class FrmTempoRegiPointSettei : ShopBaseForm 
    {
        TempoRejiPointSettei_BL pointSettei_bl;
        M_StorePoint_Entity mspe;

        string type="1";
        public FrmTempoRegiPointSettei()
        {
            InitializeComponent();
        }
        private void FrmTempoRegiPointSettei_Load(object sender, EventArgs e)
        {
            InProgramID = "TempoRejiPointSettei";            
            StartProgram();
            this.Text = "ポイント設定";

            SetRequireField();

            BindData();
        }
        private void SetRequireField()
        {
            txtChangeDate.Require(true);
            txtPointRate.Require(true);
            txtServiceDayRate.Require(true);
            txtExperationDate.Require(true);
            txtMaxPoint.Require(true);
            txtTicketUnit.Require(true);
            txtSize1.Require(true,txtPrint1);
            txtSize2.Require(true, txtPrint2);
            txtSize3.Require(true, txtPrint3);
            txtSize4.Require(true, txtPrint4);
            txtSize5.Require(true, txtPrint5);
            txtSize6.Require(true, txtPrint6);
            txtSize7.Require(true, txtPrint7);
            txtSize8.Require(true, txtPrint8);
            txtSize9.Require(true, txtPrint9);
            txtSize10.Require(true, txtPrint10);
            txtSize11.Require(true, txtPrint11);
            txtSize12.Require(true, txtPrint12);
        }
        public void BindData()
        {
            txtChangeDate.Text = ChangeDate;
            txtChangeDate.Focus();

            DisplayData(StoreCD, type="1");
        }
        public override void FunctionProcess(int index)
        {
            switch (index + 1)
            {
                case 2:
                    Save(type="2");
                    break;
            }
        }
        private void Save(string type)
        {
            
            if (ErrorCheck(type))
            {
                pointSettei_bl = new TempoRejiPointSettei_BL();
                if (pointSettei_bl.ShowMessage("Q101") == DialogResult.Yes)
                {
                    mspe = GetStorePointEntity();
                    if (pointSettei_bl.M_StorePoint_Insert_Update(mspe))
                    {
                        pointSettei_bl.ShowMessage("I101");
                        BindData();

                    }
                    else
                    {
                        pointSettei_bl.ShowMessage("S001");
                    }
                }
            }
        }
        public M_StorePoint_Entity GetStorePointEntity()
        {
            mspe = new M_StorePoint_Entity
            {
                StoreCD=StoreCD,
                ChangeDate = txtChangeDate.Text.Replace("/", "-"),
                PointRate = txtPointRate.Text,
                ServicedayRate = txtServiceDayRate.Text,
                ExpirationDate = txtExperationDate.Text,
                MaxPoint = txtMaxPoint.Text,
                TicketUnit = txtTicketUnit.Text,

                Print1 = txtPrint1.Text,
                Size1 = string.IsNullOrWhiteSpace(txtSize1.Text)? "0" : txtSize1.Text,
                Bold1 = chk1.Checked ? "1" : "0",

                Print2 = txtPrint2.Text,
                Size2 = string.IsNullOrWhiteSpace(txtSize2.Text) ? "0" : txtSize2.Text,
                Bold2 = chk2.Checked ? "1" : "0",

                Print3 = txtPrint3.Text,
                Size3 = string.IsNullOrWhiteSpace(txtSize3.Text) ? "0" : txtSize3.Text,
                Bold3 = chk3.Checked ? "1" : "0",

                Print4 = txtPrint4.Text,
                Size4 = string.IsNullOrWhiteSpace(txtSize4.Text) ? "0" : txtSize4.Text,
                Bold4 = chk4.Checked ? "1" : "0",

                Print5 = txtPrint5.Text,
                Size5 = string.IsNullOrWhiteSpace(txtSize5.Text) ? "0" : txtSize5.Text,
                Bold5 = chk5.Checked ? "1" : "0",

                Print6 = txtPrint6.Text,
                Size6 = string.IsNullOrWhiteSpace(txtSize6.Text) ? "0" : txtSize6.Text,
                Bold6 = chk6.Checked ? "1" : "0",

                Print7 = txtPrint7.Text,
                Size7 = string.IsNullOrWhiteSpace(txtSize7.Text) ? "0" : txtSize7.Text,
                Bold7 = chk7.Checked ? "1" : "0",

                Print8 = txtPrint8.Text,
                Size8 = string.IsNullOrWhiteSpace(txtSize8.Text) ? "0" : txtSize8.Text,
                Bold8 = chk8.Checked ? "1" : "0",

                Print9 = txtPrint9.Text,
                Size9 = string.IsNullOrWhiteSpace(txtSize9.Text) ? "0" : txtSize9.Text,
                Bold9 = chk9.Checked ? "1" : "0",

                Print10 = txtPrint10.Text,
                Size10 = string.IsNullOrWhiteSpace(txtSize10.Text) ? "0" : txtSize10.Text,
                Bold10 = chk10.Checked ? "1" : "0",

                Print11 = txtPrint11.Text,
                Size11 = string.IsNullOrWhiteSpace(txtSize11.Text) ? "0" : txtSize11.Text,
                Bold11 = chk11.Checked ? "1" : "0",

                Print12 = txtPrint12.Text,
                Size12 = string.IsNullOrWhiteSpace(txtSize12.Text) ? "0" : txtSize12.Text,
                Bold12 = chk12.Checked ? "1" : "0",
                DeleteFlg = "0",
                ProcessMode = "登録",
                InsertOperator = InOperatorCD,
                ProgramID = InProgramID,
                Key = StoreCD.ToString()+" "+ ChangeDate.ToString(),
                PC = InPcID,
            };
            return mspe;
        }
        private void DisplayData(string StoreCD,string type)
        {
            pointSettei_bl = new TempoRejiPointSettei_BL(); 
            if (ErrorCheck(type))
            {
                mspe = new M_StorePoint_Entity
                {
                    StoreCD = StoreCD,
                    ChangeDate = txtChangeDate.Text
                };
                mspe = pointSettei_bl.M_StorePoint_Select(mspe);
                if (mspe != null)
                {
                    txtChangeDate.Text = mspe.ChangeDate;
                    txtPointRate.Text = mspe.PointRate;
                    txtServiceDayRate.Text = mspe.ServicedayRate;
                    txtExperationDate.Text = mspe.ExpirationDate;
                    txtMaxPoint.Text = string.IsNullOrWhiteSpace(mspe.MaxPoint)? "0" : mspe.MaxPoint;
                    txtTicketUnit.Text = string.IsNullOrWhiteSpace(mspe.TicketUnit)? "0" : mspe.TicketUnit;

                    txtPrint1.Text = mspe.Print1;
                    txtSize1.Text = mspe.Size1=="0" ? string.Empty: mspe.Size1;
                    chk1.Checked = mspe.Bold1 == "1" ? true : false;

                    txtPrint2.Text = mspe.Print2;
                    txtSize2.Text = mspe.Size2 == "0" ? string.Empty : mspe.Size2;
                    chk2.Checked = mspe.Bold2 == "1" ? true : false;

                    txtPrint3.Text = mspe.Print3;
                    txtSize3.Text = mspe.Size3 == "0" ? string.Empty : mspe.Size3;
                    chk3.Checked = mspe.Bold3 == "1" ? true : false;

                    txtPrint4.Text = mspe.Print4;
                    txtSize4.Text = mspe.Size4 == "0" ? string.Empty : mspe.Size4;
                    chk4.Checked = mspe.Bold4 == "1" ? true : false;

                    txtPrint5.Text = mspe.Print5;
                    txtSize5.Text = mspe.Size5 == "0" ? string.Empty : mspe.Size5;
                    chk5.Checked = mspe.Bold5 == "1" ? true : false;

                    txtPrint6.Text = mspe.Print6;
                    txtSize6.Text = mspe.Size6 == "0" ? string.Empty : mspe.Size6;
                    chk6.Checked = mspe.Bold6 == "1" ? true : false;

                    txtPrint7.Text = mspe.Print7;
                    txtSize7.Text = mspe.Size7 == "0" ? string.Empty : mspe.Size7;
                    chk7.Checked = mspe.Bold7 == "1" ? true : false;

                    txtPrint8.Text = mspe.Print8;
                    txtSize8.Text = mspe.Size8 == "0" ? string.Empty : mspe.Size8;
                    chk8.Checked = mspe.Bold8 == "1" ? true : false;

                    txtPrint9.Text = mspe.Print9;
                    txtSize9.Text = mspe.Size9 == "0" ? string.Empty : mspe.Size9;
                    chk9.Checked = mspe.Bold9 == "1" ? true : false;

                    txtPrint10.Text = mspe.Print10;
                    txtSize10.Text = mspe.Size10 == "0" ? string.Empty : mspe.Size10;
                    chk10.Checked = mspe.Bold10 == "1" ? true : false;

                    txtPrint11.Text = mspe.Print11;
                    txtSize11.Text = mspe.Size11 == "0" ? string.Empty : mspe.Size11;
                    chk11.Checked = mspe.Bold11 == "1" ? true : false;

                    txtPrint12.Text = mspe.Print12;
                    txtSize12.Text = mspe.Size12 == "0" ? string.Empty : mspe.Size12;
                    chk12.Checked = mspe.Bold12 == "1" ? true : false;



                }
            }
        }

        public bool ErrorCheck(string type)
        {
            if (type == "2")
            {
                if (!RequireCheck(new Control[] { txtChangeDate, txtPointRate, txtServiceDayRate, txtExperationDate, txtMaxPoint, txtTicketUnit }))
                    return false;

                if (!RequireCheck(new Control[] { txtSize1 }, txtPrint1))
                    return false;
                if (!RequireCheck(new Control[] { txtSize2 }, txtPrint2))
                    return false;
                if (!RequireCheck(new Control[] { txtSize3 }, txtPrint3))
                    return false;
                if (!RequireCheck(new Control[] { txtSize4 }, txtPrint4))
                    return false;
                if (!RequireCheck(new Control[] { txtSize5 }, txtPrint5))
                    return false;
                if (!RequireCheck(new Control[] { txtSize6 }, txtPrint6))
                    return false;
                if (!RequireCheck(new Control[] { txtSize7 }, txtPrint7))
                    return false;
                if (!RequireCheck(new Control[] { txtSize8 }, txtPrint8))
                    return false;
                if (!RequireCheck(new Control[] { txtSize9 }, txtPrint9))
                    return false;
                if (!RequireCheck(new Control[] { txtSize10 }, txtPrint10))
                    return false;
                if (!RequireCheck(new Control[] { txtSize11 }, txtPrint11))
                    return false;
                if (!RequireCheck(new Control[] { txtSize12 }, txtPrint12))
                    return false;
            }

            return true;
        }
        /// <summary>
        /// override F1 Button click
        /// </summary>
        protected override void EndSec()
        {
            this.Close();
        }

        private void FrmTempoRegiPointSettei_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        //private void txtPointRate_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if(e.KeyCode==Keys.Enter)
        //    {
        //        if (txtPointRate.Text == "99.9")
        //            txtPointRate.Text = "100.0";
               
        //    }
        //}

        //private void txtServiceDayRate_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        if (txtServiceDayRate.Text == "9.9")
        //            txtServiceDayRate.Text = "10.0";

        //    }
        //}
    }
}
