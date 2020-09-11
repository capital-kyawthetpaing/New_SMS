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
using Entity;
using BL;
using Search;
//using TempoJuchuuNyuuryoku;

namespace TenzikaiJuchuuTourou
{
   internal partial class TenzikaiJuchuuTourou : FrmMainForm
    {
        private const string ProID = "TenzikaiJuchuuTourou";
        private const string ProNm = "展示会受注登録";
        private const short mc_L_END = 3; // ロック用
        private const string TempoNouhinsyo = "TenzikaiJuchuuTourou.exe";
        private string C_dt = "";

        private Control[] keyControls;
        private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        CKM_SearchControl mOldCustomerCD;
        private enum Eindex : int
        {
           SCTenjiKai,
           SCShiiresaki,
           Nendo,
           ShiSon,
           JuuChuuBi,
           Uriageyotei,
           ShuukaSouko,
           SCTentouStaffu,
           SCKokyakuu,
           KJuShou1,
           btnkokyaku,
           KJuShou2,
           pnlKokyakuu,
           KDenwa1,
           KDenwa2,
           KDenwa3,
           SCHaiSoSaki,
           HJuShou1,
           btnHaisou,
           HJuShou2,
           pnlHaisou,
           HDenwa1,
           HDenwa2,
           HDenwa3,
           YoteiKinShuu,
           Count
        }
        FrmAddress addInfo;
        private Tenjikai_Entity tje;
        public TenzikaiJuchuuTourou()
        {
            InitializeComponent();
            C_dt = DateTime.Now.ToString("yyyy-MM-dd");
        }
        private void InitialControlArray()
        {

            detailControls = new Control[] { sc_Tenji.TxtCode, sc_shiiresaki.TxtCode, cbo_nendo,cbo_season,txt_JuchuuBi, txt_UriageYoteiBi, cbo_Shuuka,sc_TentouStaff.TxtCode,
                sc_kokyakuu.TxtCode, txt_kokyaJuusho1, btn_Customer, txt_KokyaJuusho2,  pnl_kokyakuu, txt_KDenwa1, txt_KDenwa2, txt_KDenwa3,
                sc_haisosaki.TxtCode, txt_HaisoJuusho1, btn_Shipping,  txt_HaisoJuusho2, pnl_haisou, txt_HDenwa1, txt_HDenwa2,txt_HDenwa3,
                cbo_yotei };
            detailLabels = new Control[] { };
            searchButtons = new Control[] { sc_Tenji.BtnSearch, sc_shiiresaki.BtnSearch, sc_TentouStaff.BtnSearch, sc_kokyakuu.BtnSearch, sc_haisosaki.BtnSearch };
            detailControls[(int)(Eindex.KJuShou1)].Enabled = false;
            detailControls[(int)(Eindex.KJuShou2)].Enabled = false;
            detailControls[(int)(Eindex.HJuShou1)].Enabled = false;
            detailControls[(int)(Eindex.HJuShou2)].Enabled = false;

            (detailControls[(int)(Eindex.JuuChuuBi)] as CKM_TextBox).Require(false); 

            foreach (var c in detailControls)
            {
                c.KeyDown += C_KeyDown;
                c.Enter += C_Enter;
            }
            kr_1.KeyDown += Kr_2_KeyDown;
            kr_2.KeyDown += Kr_2_KeyDown;
            hr_3.KeyDown += Hr_3_KeyDown;
            hr_4.KeyDown += Hr_3_KeyDown;
          //  sc_Tenji.KeyDown += TenzikaiJuchuuTourou_KeyDown;
        }

        private void TenzikaiJuchuuTourou_KeyDown(object sender, KeyEventArgs e)
        {
            if (ActiveControl is CKM_SearchControl cs && cs.Name == "sc_Tenji")
            {
                if (e.KeyCode == Keys.Tab)
                {
                    detailControls[(int)(Eindex.SCShiiresaki)].Focus();
                }
            }
        }

        private void Hr_3_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    MoveNextControl(e);
                    //kr_1.Focus();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void Kr_2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    MoveNextControl(e);
                        //kr_1.Focus();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }


        private void C_Enter(object sender, EventArgs e)
        {
            if ((sender as Control) is Panel p)
            {

                if (p.Name == "pnl_kokyakuu")
                {
                    kr_1.Focus();
                }
                else if(p.Name ==  "pnl_haisou")
                {
                    hr_3.Focus();

                }
            }
          //  throw new NotImplementedException();
        }

        private void C_KeyDown(object sender, KeyEventArgs e)
        {
            // Processing when the Enter key is pressed
            // Check if the Return key is pressed
            // Alt or Ctrl key is pressed, do the original operation
            try
            {
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int index = Array.IndexOf(detailControls, sender);
                    bool ret = CheckDetail(index);
                    if (ret)
                    {
                        if (index == (int)Eindex.KDenwa3 || index == (int)Eindex.HDenwa3 || index == (int)(Eindex.YoteiKinShuu))
                        //明細の先頭項目へ
                        {
                            MoveNextControl(e);
                            //   mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridJuchuu.ColNO.JanCD);
                        }
                        else if (detailControls.Length - 1 > index)
                        {
                            if (detailControls[index + 1].CanFocus)
                                detailControls[index + 1].Focus();
                            else
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                ProcessTabKey(!e.Shift);
                        }
                    }
                    else
                    {
                        //((Control)sender).Focus();
                    }
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
            //  throw new NotImplementedException();
        }
        private bool CheckDetail(int index, bool set = true)
        {

            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }
            TenjikaiJuuChuu_BL tbl = new TenjikaiJuuChuu_BL();
            switch (index)
            {
                case (int)Eindex.Nendo:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.ShiSon:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.JuuChuuBi:
                     if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }

                    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        detailControls[index].Focus();
                        return false;
                    }
                    //入力できる範囲内の日付であること
                    if(!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        detailControls[index].Focus();
                        return false;
                    }
                        break;
                case (int)Eindex.Uriageyotei:
                   // tbl = new TenjikaiJuuChuu_BL();
                    // TenjikaiJuuChuu_BL tbl = new TenjikaiJuuChuu_BL();
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        break;
                    }
                    if (!bbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        detailControls[index].Focus();
                        return false;
                    }
                    //入力できる範囲内の日付であること
                    if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.ShuukaSouko:
                    
                    tbl = new TenjikaiJuuChuu_BL();
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        detailControls[index].Focus();
                        return false;
                    }
                   var souko = tbl.ShuukaSouko((detailControls[index] as CKM_ComboBox).SelectedValue.ToString(), bbl.GetDate());

                    if (souko.Rows.Count == 0)
                    {
                        bbl.ShowMessage("E145");
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.SCTentouStaffu:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }

                    if (!CheckDependsOnDate(index))
                        return false;

                    break;
                case (int)Eindex.SCKokyakuu:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        ClearCustomerInfo(0);
                        detailControls[index].Focus();
                        return false;
                    }
                    if (!CheckDependsOnDate(index))
                        return false;


                    break;
                case (int)Eindex.SCHaiSoSaki:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        ClearCustomerInfo(1);
                        detailControls[index].Focus();
                        return false;
                    }
                    if (!CheckDependsOnDate(index))
                        return false;
                    break;
                case (int)Eindex.SCShiiresaki:    // ShiireSaki
                    //Entry required
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        ClearCustomerInfo(2);
                        detailControls[index].Focus();
                        return false;
                    }
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        ChangeDate = bbl.GetDate(),
                        VendorFlg  = "1",
                        VendorCD = detailControls[index].Text,
                        DeleteFlg = "0"
                    };
                    Vendor_BL vbl = new Vendor_BL();
                    var resul = vbl.M_Vendor_Select_Tenji(mve);
                    if (!resul)
                    {
                        bbl.ShowMessage("E101");
                        ClearCustomerInfo(2);
                        return false;
                    }
                    (detailControls[(int)Eindex.SCShiiresaki].Parent as CKM_SearchControl).LabelText = mve.VendorName;
                    if (!CheckDependsOnDate(index))
                        return false;

                    break;

                //case (int)Eindex.JuchuuDate:
                //    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                //    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                //    {
                //        //Ｅ１０２
                //        bbl.ShowMessage("E102");
                //        detailControls[index].Focus();
                //        return false;
                //    }

                //    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                //    //日付として正しいこと(Be on the correct date)Ｅ１０３
                //    if (!bbl.CheckDate(detailControls[index].Text))
                //    {
                //        //Ｅ１０３
                //        bbl.ShowMessage("E103");
                //        detailControls[index].Focus();
                //        return false;
                //    }
                //    //入力できる範囲内の日付であること
                //    if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                //    {
                //        //Ｅ１１５
                //        bbl.ShowMessage("E115");
                //        detailControls[index].Focus();
                //        return false;
                //    }

                //    //受注日が変更された場合のチェック処理
                //    if (mOldJyuchuDate != detailControls[index].Text)
                //    {
                //        for (int i = (int)EIndex.StaffCD; i <= (int)EIndex.DeliveryCD; i++)
                //            if (!string.IsNullOrWhiteSpace(detailControls[i].Text))
                //                if (!CheckDependsOnDate(i, true))
                //                    return false;

                //        //明細部JANCDの再チェック
                //        for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                //        {
                //            if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD))
                //            {
                //                if (CheckGrid((int)ClsGridJuchuu.ColNO.JanCD, RW, true, true) == false)
                //                {
                //                    //Focusセット処理
                //                    ERR_FOCUS_GRID_SUB((int)ClsGridJuchuu.ColNO.JanCD, RW);
                //                    return false;
                //                }
                //            }
                //        }
                //        mOldJyuchuDate = detailControls[index].Text;
                //        ScCustomer.ChangeDate = mOldJyuchuDate;
                //        ScDeliveryCD.ChangeDate = mOldJyuchuDate;
                //    }

                //    break;

                //case (int)EIndex.SoukoName:
                //    //選択必須(Entry required)
                //    if (!RequireCheck(new Control[] { detailControls[index] }))
                //    {
                //        CboSoukoName.MoveNext = false;
                //        return false;
                //    }
                //    if (!CheckDependsOnDate(index))
                //    {
                //        CboSoukoName.MoveNext = false;
                //        return false;
                //    }
                //    break;

                //case (int)EIndex.StaffCD:
                //    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                //    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                //    {
                //        //Ｅ１０２
                //        bbl.ShowMessage("E102");
                //        detailControls[index].Focus();
                //        return false;
                //    }

                //    if (!CheckDependsOnDate(index))
                //        return false;

                //    break;


                case (int)Eindex.KJuShou1:
                case (int)Eindex.HJuShou1:
                    //入力可能の場合 入力必須(Entry required)
                    if (detailControls[index].Enabled)
                    {
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            detailControls[index].Focus();
                            return false;
                        }
                        //顧客名2に入力無い場合、先頭20Byteを顧客名2へセット   delete 4/24
                        if (string.IsNullOrWhiteSpace(detailControls[index + 2].Text))
                        {
                            detailControls[index + 2].Text = bbl.LeftB(detailControls[index].Text, 20);
                        }
                    }
                    break;

                case (int)Eindex.KJuShou2:
                case (int)Eindex.HJuShou2:
                    //入力可能の場合 入力必須(Entry required)
                    if (detailControls[index].Enabled && string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }
                    break;



                case (int)Eindex.YoteiKinShuu:
                    //選択必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }
                    break;

                    //////case (int)EIndex.Point:
                    //////    //入力された場合 ポイント＞	Form.Header.残ポイントの場合、Error				
                    //////    if(bbl.Z_Set(detailControls[index].Text)>0)
                    //////        if(bbl.Z_Set(detailControls[index].Text) > bbl.Z_Set(lblPoint.Text))
                    //////        {
                    //////            //Ｅ１４６
                    //////            bbl.ShowMessage("E146");
                    //////            detailControls[index].Focus();
                    //////            return false;
                    //////        }

                    //////    break;

                    ////case (int)EIndex.SalesDate:
                    ////    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    ////    {
                    ////        return true;
                    ////    }
                    ////    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    ////    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    ////    if (!bbl.CheckDate(detailControls[index].Text))
                    ////    {
                    ////        //Ｅ１０３
                    ////        bbl.ShowMessage("E103");
                    ////        detailControls[index].Focus();
                    ////        return false;
                    ////    }

                    ////    //入力できる範囲内の日付であること
                    ////    if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    ////    {
                    ////        //Ｅ１１５
                    ////        bbl.ShowMessage("E115");
                    ////        detailControls[index].Focus();
                    ////        return false;
                    ////    }

                    ////    //Form.Detail.入荷予定日＞form.売上予定日の場合、
                    ////    bool errFlg = false;
                    ////    for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                    ////    {
                    ////        if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ArrivePlanDate))
                    ////        {
                    ////            int result = detailControls[index].Text.CompareTo(mGrid.g_DArray[RW].ArrivePlanDate);
                    ////            if (result < 0)
                    ////            {
                    ////                errFlg = true;
                    ////                break;
                    ////            }
                    ////        }
                    ////    }
                    ////    if (errFlg)
                    ////    {
                    ////        //「入荷予定日が売上予定日より後になっている商品があります。このまま処理を進めますか？」
                    ////        if (bbl.ShowMessage("Q304") != DialogResult.Yes)
                    ////        {
                    ////            detailControls[index].Focus();
                    ////            //売上予定日にCursorを移動
                    ////            return false;
                    ////        }
                    ////    }
                    ////    break;

                    ////case (int)EIndex.FirstPaypentPlanDate:
                    ////    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    ////    {
                    ////        return true;
                    ////    }

                    ////    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    ////    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    ////    if (!bbl.CheckDate(detailControls[index].Text))
                    ////    {
                    ////        //Ｅ１０３
                    ////        bbl.ShowMessage("E103");
                    ////        detailControls[index].Focus();
                    ////        return false;
                    ////    }

                    ////    break;

            }

            return true;
        }
        private bool CheckDependsOnDate(int index, bool ChangeDate = false)

        {
            string ymd = detailControls[(int)Eindex.JuuChuuBi].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            switch (index)
            {
                //case (int)Eindex.SoukoName:
                //    //[M_Souko_Select]
                //    M_Souko_Entity me = new M_Souko_Entity
                //    {
                //        SoukoCD = CboSoukoName.SelectedValue.ToString(),
                //        ChangeDate = ymd,
                //        DeleteFlg = "0"
                //    };

                //    DataTable mdt = mibl.M_Souko_IsExists(me);
                //    if (mdt.Rows.Count > 0)
                //    {
                //        if (!base.CheckAvailableStores(mdt.Rows[0]["StoreCD"].ToString()))
                //        {
                //            bbl.ShowMessage("E141");
                //            detailControls[index].Focus();
                //            return false;
                //        }
                //    }
                //    else
                //    {
                //        bbl.ShowMessage("E101");
                //        detailControls[index].Focus();
                //        return false;
                //    }

                //    break;

                case (int)Eindex.SCTentouStaffu:
                    //スタッフマスター(M_Staff)に存在すること
                    //[M_Staff]
                    M_Staff_Entity mse = new M_Staff_Entity
                    {
                        StaffCD = detailControls[index].Text,
                        ChangeDate = ymd
                    };
                    Staff_BL bl = new Staff_BL();
                    bool staff = bl.M_Staff_Select(mse);
                    if (staff)
                    {
                        (detailControls[(int)Eindex.SCTentouStaffu].Parent as CKM_SearchControl).LabelText = mse.StaffName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        (detailControls[(int)Eindex.SCTentouStaffu].Parent as CKM_SearchControl).LabelText = "";
                        detailControls[(int)Eindex.SCTentouStaffu].Text = "";
                        detailControls[index].Focus();
                        return false;
                    }
                    break;

                case (int)Eindex.SCShiiresaki:
                    //[Shiiresaki]

                    break;
                case (int)Eindex.SCKokyakuu:
                case (int)Eindex.SCHaiSoSaki:
                    short kbn = 0;
                    if (index.Equals((int)Eindex.SCHaiSoSaki))
                        kbn = 1;

                    //[M_Customer_Select]
                    Entity.M_Customer_Entity mce = new M_Customer_Entity
                    {
                        CustomerCD = detailControls[index].Text,
                        ChangeDate = ymd
                    };
                    BL.Customer_BL sbl = new Customer_BL();
                    //ret = sbl.M_Customer_Select(mce, 1);
                    bool ret = sbl.M_Customer_Select(mce, 1);

                    if (ret)
                    {
                        if (mce.DeleteFlg == "1")
                        {
                            bbl.ShowMessage("E119");
                            //顧客情報ALLクリア
                            ClearCustomerInfo(kbn);
                            detailControls[index].Focus();
                            return false;
                        }
                        if (kbn.Equals(0))
                        {                                //住所情報セット
                            addInfo.ade.VariousFLG = mce.VariousFLG;
                            addInfo.ade.ZipCD1 = mce.ZipCD1;
                            addInfo.ade.ZipCD2 = mce.ZipCD2;
                            addInfo.ade.Address1 = mce.Address1;
                            addInfo.ade.Address2 = mce.Address2;
                            addInfo.ade.Tel11 = mce.Tel11;
                            addInfo.ade.Tel12 = mce.Tel12;
                            addInfo.ade.Tel13 = mce.Tel13;
                            detailControls[(int)(Eindex.KDenwa1)].Text = mce.Tel11;
                            detailControls[(int)(Eindex.KDenwa2)].Text = mce.Tel12;
                            detailControls[(int)(Eindex.KDenwa3)].Text = mce.Tel13;
                            detailControls[(int)(Eindex.KJuShou1)].Text = mce.CustomerName;
                            // detailControls[(int)(Eindex.KJuShou2)].Text = ;
                            //radio
                            if (mce.AliasKBN == "1")
                            {
                                kr_1.Checked = true;
                                kr_2.Checked = false;
                            }
                            else
                            {
                                kr_1.Checked = false;
                                kr_2.Checked = true;
                            }
                            if (mce.VariousFLG == "1")
                            {
                                if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
                                {
                                    detailControls[index + 1].Enabled = true;
                                    detailControls[index + 3].Enabled = true;
                                }
                                detailControls[index + 3].Text = "";

                            }
                            else
                            {
                                detailControls[index + 1].Enabled = false;
                                detailControls[index + 3].Text = "";
                                detailControls[index + 3].Enabled = false;
                            }

                            if (string.IsNullOrWhiteSpace(sc_haisosaki.TxtCode.Text))
                            {
                                detailControls[(int)(Eindex.SCHaiSoSaki)].Text = detailControls[(int)Eindex.SCKokyakuu].Text;
                                CheckDetail((int)Eindex.SCHaiSoSaki);
                                detailControls[(int)Eindex.HJuShou1].Text = detailControls[(int)Eindex.KJuShou1].Text;
                                hr_3.Checked = kr_1.Checked;
                                hr_4.Checked = kr_2.Checked;
                                detailControls[(int)Eindex.HDenwa1].Text = detailControls[(int)Eindex.KDenwa1].Text;
                                detailControls[(int)Eindex.HDenwa2].Text = detailControls[(int)Eindex.KDenwa2].Text;
                                detailControls[(int)Eindex.HDenwa3].Text = detailControls[(int)Eindex.KDenwa3].Text;
                                addInfo.adeD.ZipCD1 = addInfo.ade.ZipCD1;
                                addInfo.adeD.ZipCD2 = addInfo.ade.ZipCD2;
                                addInfo.adeD.Address1 = addInfo.ade.Address1;
                                addInfo.adeD.Address2 = addInfo.ade.Address2;
                            }
                        }

                        else
                        {
                            addInfo.adeD.VariousFLG = mce.VariousFLG;
                            addInfo.adeD.ZipCD1 = mce.ZipCD1;
                            addInfo.adeD.ZipCD2 = mce.ZipCD2;
                            addInfo.adeD.Address1 = mce.Address1;
                            addInfo.adeD.Address2 = mce.Address2;
                            addInfo.adeD.Tel11 = mce.Tel11;
                            addInfo.adeD.Tel12 = mce.Tel12;
                            addInfo.adeD.Tel13 = mce.Tel13;
                            detailControls[(int)Eindex.HDenwa1].Text = mce.Tel11;
                            detailControls[(int)Eindex.HDenwa2].Text = mce.Tel12;
                            detailControls[(int)Eindex.HDenwa3].Text = mce.Tel13;

                            if (mce.VariousFLG == "1")
                            {
                                detailControls[(int)(Eindex.KJuShou1)].Text = mce.CustomerName;
                                if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
                                {
                                    detailControls[index + 1].Enabled = true;
                                    detailControls[index + 3].Enabled = true;
                                }
                                detailControls[index + 3].Text = "";
                            }
                            else
                            {
                                detailControls[index + 1].Text = mce.CustomerName;
                                detailControls[index + 1].Enabled = false;
                                detailControls[index + 3].Text = "";
                                detailControls[index + 3].Enabled = false;
                            }
                            //敬称
                            if (mce.AliasKBN == "1")
                            {
                                hr_3.Checked = true;
                                hr_4.Checked = false;
                            }
                            else
                            {
                                hr_3.Checked = false;
                                hr_4.Checked = true;
                            }
                            //    }
                        }
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ClearCustomerInfo(kbn);
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
            }
            return true;

        }
        private void ClearCustomerInfo(short kbn)
        {
            addInfo.ClearAddressInfo(kbn);  //Clear to Address Screen that still idle

            if (kbn.Equals(0))   // Customer
            {

                detailControls[(int)(Eindex.SCKokyakuu)].Text = "";
                detailControls[(int)(Eindex.KJuShou1)].Text = "";
                detailControls[(int)(Eindex.KJuShou2)].Text = "";
                detailControls[(int)(Eindex.KDenwa1)].Text = "";
                detailControls[(int)(Eindex.KDenwa2)].Text = "";
                detailControls[(int)(Eindex.KDenwa3)].Text = "";
                kr_1.Checked = true;
                kr_2.Checked = false;

                //mOldCustomerCD = "";
                //mTaxFractionKBN = 0;
                //mTaxTiming = 0;

                ////addInfo = new FrmAddress();

                //ScCustomer.LabelText = "";
                //detailControls[(int)Eindex.CustomerName].Text = "";
                //detailControls[(int)EIndex.CustomerName2].Text = "";
                //detailControls[(int)EIndex.CustomerName].Enabled = false;
                //textBox1.Text = "";
                //textBox2.Text = "";
                //lblLastSalesDate.Text = "";
                //lblStoreName.Text = "";
                //lblTankaName.Text = "";
                //lblPoint.Text = "";
            }
            else if (kbn.Equals(1))  // delivery / Shipping
            {
                detailControls[(int)(Eindex.SCHaiSoSaki)].Text = "";
                detailControls[(int)(Eindex.HJuShou1)].Text = "";
                detailControls[(int)(Eindex.HJuShou2)].Text = "";
                detailControls[(int)(Eindex.HDenwa1)].Text = "";
                detailControls[(int)(Eindex.HDenwa2)].Text = "";
                detailControls[(int)(Eindex.HDenwa3)].Text = "";
                hr_3.Checked = true;
                hr_4.Checked = false;
                //mOldDeliveryCD = "";

                //ScDeliveryCD.LabelText = "";
                //detailControls[(int)EIndex.DeliveryName].Text = "";
                //detailControls[(int)EIndex.DeliveryName2].Text = "";
                //detailControls[(int)EIndex.DeliveryName].Enabled = false;
            }
            else if (kbn.Equals(2))  // Shiiresaki
            {
                detailControls[(int)(Eindex.SCShiiresaki)].Text = "";
                sc_shiiresaki.LabelText = "";

            }
        }
        private void TenzikaiJuchuuTourou_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = ProID;
                InProgramNM = ProNm;

                this.SetFunctionLabel(EProMode.INPUT);
                this.InitialControlArray();
                addInfo = new FrmAddress();
                // 明細部初期化
                //this.S_SetInit_Grid();

                //Scr_Clr(0);

                //起動時共通処理
                base.StartProgram();
                BindCombo();
                //コンボボックス初期化
                //  string ymd = bbl.GetDate();
                // tubl = new TempoUriageNyuuryoku_BL();
                //  CboStoreCD.Bind(ymd);

                //検索用のパラメータ設定
                //    ScCustomerCD.Value1 = "1";
                //   ScCustomerCD.Value2 = "";

                Btn_F11.Text="";


               // ChangeOperationMode(EOperationMode.INSERT);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();
            }
        }
        protected void BindCombo()
        {
            cbo_nendo.Bind(C_dt);
            cbo_Shuuka.Bind(C_dt);
            cbo_season.Bind(C_dt);
            cbo_yotei.Bind(C_dt);
        }
        protected void EndSec()
        {
            this.Close();
        }

        private void btn_Customer_Click(object sender, EventArgs e)
        {
            try
            {
                addInfo.ade.CustomerCD = detailControls[(int)(Eindex.SCKokyakuu)].Text;
                addInfo.ade.CustomerName = detailControls[(int)Eindex.KJuShou1].Text;
                addInfo.ade.CustomerName2 = detailControls[(int)Eindex.KJuShou2].Text;
                addInfo.ade.Tel11 = detailControls[(int)Eindex.KDenwa1].Text;
                addInfo.ade.Tel12 = detailControls[(int)Eindex.KDenwa2].Text;
                addInfo.ade.Tel13 = detailControls[(int)Eindex.KDenwa3].Text;
                addInfo.kbn = 0;
                addInfo.ShowDialog();

                detailControls[(int)(Eindex.pnlKokyakuu)].Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
           
        }

        private void btn_Shipping_Click(object sender, EventArgs e)
        {
            try
            {
                addInfo.ade.CustomerCD = detailControls[(int)(Eindex.SCHaiSoSaki)].Text;
                addInfo.ade.CustomerName = detailControls[(int)Eindex.HJuShou1].Text;
                addInfo.ade.CustomerName2 = detailControls[(int)Eindex.HJuShou2].Text;
                addInfo.ade.Tel11 = detailControls[(int)Eindex.HDenwa1].Text;
                addInfo.ade.Tel12 = detailControls[(int)Eindex.HDenwa2].Text;
                addInfo.ade.Tel13 = detailControls[(int)Eindex.HDenwa3].Text;
                addInfo.kbn = 1;
                addInfo.ShowDialog();

                detailControls[(int)(Eindex.pnlHaisou)].Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
