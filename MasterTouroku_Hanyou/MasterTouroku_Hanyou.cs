using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using CKM_Controls;
using Search;
using BL;
using Entity;

namespace MasterTouroku_Hanyou
{
    public partial class FrmMasterTouroku_Hanyou : FrmMainForm
    {
        MasterTouroku_Hanyou_BL mthbl;
        M_Hanyou_Entity mhe;
        int type = 0; //1 = ID & Key, 2 = ID & CopyKey (for f11)
        public FrmMasterTouroku_Hanyou()
        {
            InitializeComponent();
        }

        private void FrmMasterTouroku_Hanyou_Load(object sender, EventArgs e)
        {
            //InOperatorCD = "0002";
            InProgramID = "MasterTouroku_Hanyou";

            SetFunctionLabel(EProMode.MENTE);
            StartProgram();

            mthbl = new MasterTouroku_Hanyou_BL();

            ScID.Focus();            
        }

        /// <summary>
        /// override F1 Button click
        /// </summary>
        protected override void EndSec()
        {
            this.Close();
        }

        /// <summary>
        /// Handle F1 to F12 Click
        /// </summary>
        /// <param name="index"> button index+1, eg.if index is 0,it means F1 click </param>
        public override void FunctionProcess(int index)
        {          
            switch (index + 1)
            {
                case 2:
                    ChangeMode(EOperationMode.INSERT);
                    break;
                case 3:
                    ChangeMode(EOperationMode.UPDATE);
                    break;
                case 4:
                    ChangeMode(EOperationMode.DELETE);
                    break;
                case 5:
                    ChangeMode(EOperationMode.SHOW);
                    break;
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        ChangeMode(OperationMode);
                        ScID.SetFocus(1);
                    }
                    else
                        PreviousCtrl.Focus();
                    break;
                case 11:                   
                    F11();
                    break;
                case 12:
                    //ErrorCheck(12);
                    //if (mthbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                        F12();
                    break;
            }
        }

        /// <summary>
        /// Disable/Enable,Clear Handle on ModeChange
        /// </summary>
        private void ChangeMode(EOperationMode OperationMode)
        {
            base.OperationMode = OperationMode;
            switch (OperationMode)
            {
                case EOperationMode.INSERT:
                    Clear(panel3 );
                    Clear(panelDetail);
                    EnablePanel(panel3);
                    DisablePanel(panelDetail);
                    ScKey.SearchEnable = false;
                    ScCopyKey.SearchEnable = false;
                    F9Visible = false;
                    F12Enable = true;
                    BtnF11Show.Enabled = F11Enable = true;
                    break;
                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    Clear(panel3);
                    Clear(panelDetail);
                    EnablePanel(panel3);
                    DisablePanel(panelDetail);
                    ScKey.SearchEnable = true;
                    ScCopyKey.Enabled = false;
                    F12Enable = false;
                    BtnF11Show.Enabled = F11Enable = true;
                    break;
            }
            ScID.SetFocus(1);
        }

        #region DisplayData
        /// <summary>
        /// Display event
        /// </summary>
        /// <param name="type"></param>
        /// 
        private void F11()
        {
            if(ErrorCheck(11))
            {
                switch (OperationMode )
                {
                    case EOperationMode.INSERT:
                        if (type == 1)
                        {                         
                            BtnF11Show.Enabled = true;
                            F11Enable = true;
                            DisablePanel(panelDetail);                           
                            ScCopyKey.SetFocus(1);
                        }                           
                        else
                        {
                            mhe.ID = ScID.Code;
                            mhe.Key = ScCopyKey.Code;
                            DisplayData();
                            DisablePanel(panel3);
                            EnablePanel(panelDetail);
                            BtnF11Show.Enabled = false;
                            F11Enable = false;
                            txtText1.Focus();

                            //if(string.IsNullOrWhiteSpace(ScKey.ChangeDate) || (DisplayData(ScCopyKey)))
                            //{
                            //    DisablePanel(panel3);
                            //    EnablePanel(panelDetail);
                            //    BtnF11Show.Enabled = F11Enable = false;
                            //    SelectNextControl(PanelDetail, true, true, true, true);
                            //}

                        }
                        break;
                    case EOperationMode.UPDATE:
                        mhe.ID = ScID.Code;
                        mhe.Key = ScKey.Code;
                        DisplayData();                        
                        DisablePanel(panel3);
                        BtnF11Show.Enabled = false;
                        F11Enable = false;
                        EnablePanel(panelDetail);
                        F12Enable = true;
                        ScKey.SearchEnable = true;
                        txtText1.Focus();                       
                        break;                  
                    case EOperationMode.DELETE:
                        mhe.ID = ScID.Code;
                        mhe.Key = ScKey.Code;
                        DisplayData();                       
                        DisablePanel(panel3);
                        BtnF11Show.Enabled = false;
                        F11Enable = false;
                        DisablePanel(panelDetail);
                        txtText1.Focus();
                        F12Enable = true;                     
                        break;                 
                    case EOperationMode.SHOW:
                        mhe.ID = ScID.Code;
                        mhe.Key = ScKey.Code;
                        DisplayData();
                        DisablePanel(panel3);
                        BtnF11Show.Enabled = false;
                        F11Enable = false;
                        DisablePanel(panelDetail);
                        txtText1.Focus();
                        F12Enable = false;                       
                        break;
                }
            }
        }

        private void DisplayData()
        {
            DataTable dtKey = new DataTable();
            dtKey = mthbl.Hanyou_KeySelect(mhe);
            if(dtKey.Rows.Count > 0)
            {
                txtText1.Text = dtKey.Rows[0]["Char1"].ToString();
                txtText2.Text = dtKey.Rows[0]["Char2"].ToString();
                txtText3.Text = dtKey.Rows[0]["Char3"].ToString();
                txtText4.Text = dtKey.Rows[0]["Char4"].ToString();
                txtText5.Text = dtKey.Rows[0]["Char5"].ToString();
                txtDigital1.Text = dtKey.Rows[0]["Num1"].ToString();
                txtDigital2.Text = dtKey.Rows[0]["Num2"].ToString();
                txtDigital3.Text = dtKey.Rows[0]["Num3"].ToString();
                txtDigital4.Text = dtKey.Rows[0]["Num4"].ToString();
                txtDigital5.Text = dtKey.Rows[0]["Num5"].ToString();

                string day1 = dtKey.Rows[0]["Date1"].ToString();
                if (!string.IsNullOrWhiteSpace(day1))
                {
                    string[] strdate1 = day1.Split(' ');
                    txtDay1.Text = strdate1[0].ToString();
                    txtDay11.Text = strdate1[1].ToString();
                }

                string day2 = dtKey.Rows[0]["Date2"].ToString();
                if (!string.IsNullOrWhiteSpace(day2))
                {
                    string[] strdate2 = day2.Split(' ');
                    txtDay2.Text = strdate2[0].ToString();
                    txtDay21.Text = strdate2[1].ToString();
                }

                string day3 = dtKey.Rows[0]["Date3"].ToString();
                if (!string.IsNullOrWhiteSpace(day3))
                {
                    string[] strdate3 = day3.Split(' ');
                    txtDay3.Text = strdate3[0].ToString();
                    txtDay31.Text = strdate3[1].ToString();
                }
            }
            
        }
        #endregion

        #region Insert/Update/Date
        private void F12()
        {
            if (ErrorCheck(12))
            {
                if (mthbl.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
                {
                    mhe = GetHanyouEntity();
                    switch (OperationMode)
                    {
                        case EOperationMode.INSERT:
                            InsertUpdate(1);
                            break;
                        case EOperationMode.UPDATE:
                            InsertUpdate(2);
                            break;
                        case EOperationMode.DELETE:
                            Delete();
                            break;
                    }
                }
                else
                    PreviousCtrl.Focus();
            }
        }

        private void InsertUpdate(int mode)
        {
            if (mthbl.M_Hanyou_Insert_Update(mhe, mode))
            {
                ChangeMode(OperationMode);

                mthbl.ShowMessage("I101");
            }
            else
            {
                mthbl.ShowMessage("S001");
            }
        }

        private void Delete()
        {
            if (mthbl.M_Hanyou_Delete(mhe))
            {
                ChangeMode(OperationMode);

                mthbl.ShowMessage("I102");
            }
            else
            {
                mthbl.ShowMessage("S001");
            }
        }
        #endregion

        private M_Hanyou_Entity GetHanyouEntity()
        {
            mhe = new M_Hanyou_Entity
            {
                ID = ScID.Code,
                Key = ScKey.Code,
                IDName = ScID.LabelText,
                Text1 = txtText1.Text,
                Text2 = txtText2.Text,
                Text3 = txtText3.Text,
                Text4 = txtText4.Text,
                Text5 = txtText5.Text,
                Digital1 = txtDigital1.Text,
                Digital2 = txtDigital2.Text,
                Digital3 = txtDigital3.Text,
                Digital4 = txtDigital4.Text,
                Digital5 = txtDigital5.Text,
                Day1 = txtDay1.Text + " " +txtDay11.Text ,
                Day2 = txtDay2.Text + " " + txtDay21.Text,
                Day3 = txtDay3.Text + " " + txtDay31.Text,
                ProcessMode = ModeText ,
                InsertOperator = InOperatorCD,
                //Key = ScID.Code + " " + ScKey.Code,
                ProgramID = InProgramID,
                PC = InPcID
            };
            return mhe;        
        }


        #region ErrorCheck function
        private bool ErrorCheck(int index)
        {
            if (index == 11)
            {
                //HeaderCheck on F11
                if (OperationMode == EOperationMode.INSERT)
                {
                    if (type == 1)
                    {
                        if (!RequireCheck(new Control[] { ScID.TxtCode }))
                            return false;
                        if (!RequireCheck(new Control[] { ScKey.TxtCode }))
                            return false;
                    }
                    else // Copy
                    {
                        if (!RequireCheck(new Control[] { ScID.TxtCode }))
                            return false;

                        if (!RequireCheck(new Control[] { ScKey.TxtCode }))
                            return false;


                        if (!string.IsNullOrWhiteSpace(ScCopyKey.Code))
                        {
                            DataTable dtKey = new DataTable();
                            mhe = GetHanyouEntity();
                            dtKey = mthbl.Hanyou_KeySelect(mhe);
                            if (dtKey.Rows.Count > 0)
                            {
                                mthbl.ShowMessage("E132");
                                ScKey.SetFocus(1);
                                return false;
                            }
                            else
                            {
                                mhe.ID = ScID.Code;
                                mhe.Key = ScCopyKey.Code;
                                ScCopyKey.SearchEnable = true;
                                DataTable dtcopyKey = new DataTable();
                                dtcopyKey = mthbl.Hanyou_KeySelect(mhe);
                                if (dtcopyKey.Rows.Count == 0)
                                {
                                    mthbl.ShowMessage("E133");
                                    ScCopyKey.SetFocus(1);
                                    return false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    DataTable dtKey = new DataTable();
                    mhe = GetHanyouEntity();
                    dtKey = mthbl.Hanyou_KeySelect(mhe);
                    if (dtKey.Rows.Count == 0)
                    {
                        mthbl.ShowMessage("E133");
                        ScKey.SetFocus(1);
                        return false;
                    }
                }               
            }
            else if (index == 12)
            {
                if (!RequireCheck(new Control[] { ScID.TxtCode }))
                    return false;
                if (!RequireCheck(new Control[] { ScKey.TxtCode }))
                    return false;

                if (OperationMode == EOperationMode.INSERT)
                {
                    DataTable dtKey = new DataTable();
                    mhe = GetHanyouEntity();
                    dtKey = mthbl.Hanyou_KeySelect(mhe);
                    if (dtKey.Rows.Count > 0)
                    {
                        mthbl.ShowMessage("E132");
                        ScKey.SetFocus(1);
                        return false;
                    }
                }
            }
            return true;

        }
        #endregion

        private bool CheckID()
        {
            if (string.IsNullOrWhiteSpace(ScID.Code))
            {
                mthbl.ShowMessage("E102");
                ScID.TxtCode.MoveNext = false;
                return false;
            }
            else
            {
                DataTable dtID = new DataTable();
                mhe = GetHanyouEntity();

                //if (Convert.ToInt32(ScID.Code) != 000)
                if (ScID.Code != "000")
                {
                    dtID = mthbl.Hanyou_IDSelect(mhe);
                    if (dtID.Rows.Count == 0)
                    {
                        mthbl.ShowMessage("E101");
                        ScID.TxtCode.MoveNext = false;
                        return false;
                    }
                    else
                    {
                        ScID.LabelText = dtID.Rows[0]["Char1"].ToString();
                    }
                }
                else
                {
                    dtID = mthbl.Hanyou_IDSelect(mhe);
                    if (dtID.Rows.Count > 0)
                    {
                        ScID.LabelText = dtID.Rows[0]["Char1"].ToString();
                    }
                }
            }
            
            return true;
        }

        private void ScID_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {                
                if(CheckID())
                {
                    if (OperationMode == EOperationMode.INSERT)
                    {
                        ScCopyKey.SearchEnable = true;
                        F9Visible = true;
                        ScKey.TxtCode.MoveNext = false;
                        ScCopyKey.Value1 = ScID.Code;
                        ScCopyKey.Value2 = ScID.LabelText;
                    }
                    else
                    {
                        ScKey.SearchEnable = true;
                        F9Visible = true;
                        ScKey.Value1 = ScID.Code;
                        ScKey.Value2 = ScID.LabelText;
                    }
                }
                
            }
        }

        private void ScKey_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 1;
                F11();
            }

        }

        private void ScCopyKey_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F11)
            {
                type = 2;
                F11();
            }

        }     

        private void BtnF11Show_Click(object sender, EventArgs e)
        {
            FunctionProcess(10);
        }

        private void ScID_Leave(object sender, EventArgs e)
        {
            if (OperationMode == EOperationMode.INSERT)
            {               
                ScCopyKey.Value1 = ScID.Code;
                ScCopyKey.Value2 = ScID.LabelText;
            }
            else
            {             
                ScKey.Value1 = ScID.Code;
                ScKey.Value2 = ScID.LabelText;
            }
        }

        private void ScCopyKey_Leave(object sender, EventArgs e)
        {
            type = 2;
        }

        private void ScKey_Leave(object sender, EventArgs e)
        {
            //if(OperationMode ==  EOperationMode.INSERT )
            //{
            //    if(!string.IsNullOrWhiteSpace(ScID.Code) && !string.IsNullOrWhiteSpace (ScCopyKey.Code))
            //    {
            //        type = 2;
            //    }
            //    else
            //    {
            //        type = 1;
            //    }
            //}
            //else
            //{
            //    type = 1;
            //}           
        }

        private void FrmMasterTouroku_Hanyou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void panelDetail_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
