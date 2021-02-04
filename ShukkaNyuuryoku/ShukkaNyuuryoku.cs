using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;
using GridBase;

namespace ShukkaNyuuryoku
{
    /// <summary>
    /// ShukkaNyuuryoku 出荷入力
    /// </summary>
    internal partial class ShukkaNyuuryoku : FrmMainForm
    {
        private const string ProID = "ShukkaNyuuryoku";
        private const string ProNm = "出荷入力";
        private const short mc_L_END = 3; // ロック用

        private enum EIndex : int
        {
            ShippingNO = 0,
            InstructionNO,
            //StoreCD,

            ShippingDate = 0,
            CarrierName ,
            CarrierBoxSize,
            UnitsCount,
            DecidedDeliveryDate,
            CarrierDeliveryTime,

            JANCD　= 0,
            ShippingSu,

            SKUCD = 0,
            SKUName,
            Color,
            Bikou,
            Tani
        }

        /// <summary>
        /// 検索の種類
        /// </summary>
        private enum EsearchKbn : short
        {
            Null,
            Product
        }

        private Control[] keyControls;
        private Control[] keyLabels;
        private Control[] headControls;
        private Control[] headLabels;
        private Control[] headLabels2;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        
        private ShukkaNyuuryoku_BL snbl;
        private D_Shipping_Entity dse;
        private D_Instruction_Entity die;
        private DataTable dtForUpdate;  //排他用

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避
        private string mSoukoCD;                    //出荷元倉庫CD
        private string mInstructionKBN;             //出荷指示種別
        private string mJANCD;
        private string mAdminNO;
        private bool mFlgCancel;

        private string mOldShippingNO = "";         //排他処理のため使用
        private string mOldInstructionNO = "";      //排他処理のため使用
        private string mOldShippingDate = "";
        private string mOldCarrierCd = "";

        public ShukkaNyuuryoku()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = ProID;
                InProgramNM = ProNm;

                this.SetFunctionLabel(EProMode.INPUT);
                this.InitialControlArray();

                //起動時共通処理
                base.StartProgram();
                Btn_F3.Text = "";
                Btn_F7.Text = "";
                Btn_F8.Text = "";
                Btn_F10.Text = "";
                Btn_F11.Text = "";

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                snbl = new ShukkaNyuuryoku_BL();
                CboStoreCD.Bind(ymd);
                CboCarrierName.Bind(ymd);
                CboCarrierBoxSize.Bind(ymd);
                CboCarrierDeliveryTime.Bind(ymd);

                //検索用のパラメータ設定
                string stores = GetAllAvailableStores();
                ScShippingNO.Value1 = InOperatorCD;
                ScShippingNO.Value2 = stores;
                ScShippingNO.Value3 = ymd;

                ChangeOperationMode(EOperationMode.INSERT);                
                
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }

        /// <summary>
        /// 初期表示
        /// </summary>
        private void InitScr()
        {
            if (snbl == null)
                return;

            string ymd = bbl.GetDate();
            headControls[(int)EIndex.ShippingDate].Text = ymd;
            mOldShippingDate = ymd;

            //スタッフマスター(M_Staff)に存在すること
            //[M_Staff]
            M_Staff_Entity mse = new M_Staff_Entity
            {
                StaffCD = InOperatorCD,
                ChangeDate = ymd
            };
            Staff_BL bl = new Staff_BL();
            bool ret = bl.M_Staff_Select(mse);
            if (ret)
            {
                CboStoreCD.SelectedValue = mse.StoreCD;
            }

            //[M_Carrier_Select]
            M_Carrier_Entity me = new M_Carrier_Entity
            {
                ChangeDate = bbl.GetDate(),
            };
            Carrier_BL cbl = new Carrier_BL();
            DataTable sdt = cbl.M_Carrier_Bind(me);
            if (sdt.Rows.Count > 0)
            {
                CboCarrierName.SelectedValue = sdt.Rows[0]["CarrierCD"];
                mOldCarrierCd = sdt.Rows[0]["CarrierCD"].ToString();
            }
            else
            {
                CboCarrierName.SelectedValue = "";
                mOldCarrierCd = "";
            }

            //個口
            headControls[(int)EIndex.UnitsCount].Text = "1";

            //箱サイズ
            M_CarrierBoxSize_Entity mbe = new M_CarrierBoxSize_Entity
            {
                ChangeDate = bbl.GetDate(),
            };
            CarrierBoxSize_BL cbbl = new CarrierBoxSize_BL();
            DataTable sbdt = cbbl.M_CarrierBoxSize_Bind(mbe);
            if (sbdt.Rows.Count > 0)
            {
                CboCarrierBoxSize.SelectedValue = sdt.Rows[0]["BoxSizeCD"];
            }
            else
            {
                CboCarrierBoxSize.SelectedValue = "";
            }

            //希望時間帯
            M_CarrierDeliveryTime_Entity mte = new M_CarrierDeliveryTime_Entity
            {
                ChangeDate = bbl.GetDate(),
            };
            CarrierDeliveryTime_BL cbtl = new CarrierDeliveryTime_BL();
            DataTable stdt = cbtl.M_CarrierDeliveryTime_Bind(mte);
            if (stdt.Rows.Count > 0)
            {
                CboCarrierDeliveryTime.SelectedValue = sdt.Rows[0]["DeliveryTimeCD"];
            }
            else
            {
                CboCarrierDeliveryTime.SelectedValue = "";
            }
        }

        private void InitialControlArray()
        {
            keyControls = new Control[] {  ScShippingNO.TxtCode, ScInstructionNO.TxtCode,  CboStoreCD };
            keyLabels = new Control[] {  };
            headControls = new Control[] { txtShippingDate, CboCarrierName, CboCarrierBoxSize, txtUnitCount , txtDecidedDeliveryDate, CboCarrierDeliveryTime };
            headLabels = new Control[] { lblDelivery};
            headLabels2 = new Control[] { lblChange, lblSameDay, lblDecidedDelivery, lblDeliveryNote, lblDaibiki };
            detailControls = new Control[] { txtJANCD, txtShippingSu };
            detailLabels = new Control[] { lblSKUCD, lblSkuName, lblColor, lblBikou, lblTani };
            searchButtons = new Control[] { BtnJANCD};

            
            //イベント付与
            foreach (Control ctl in keyControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(KeyControl_KeyDown);
                ctl.Enter += new System.EventHandler(KeyControl_Enter);
            }
            foreach (Control ctl in headControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(HeadControl_KeyDown);
                ctl.Enter += new System.EventHandler(HeadControl_Enter);
            }
            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
                ctl.Leave += new System.EventHandler(DetailControl_Leave);
            }
        }
        
        /// <summary>
        /// PrimaryKeyのコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckKey(int index, bool set=true)
        {
            bool ret;

            switch (index)
            {
                case (int)EIndex.InstructionNO:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");                        
                        previousCtrl.Focus();
                        return false;
                    }

                    //排他処理
                    ret = SelectAndInsertExclusive(keyControls[index].Text, Exclusive_BL.DataKbn.SyukkaShiji);
                    if (!ret)
                        return false;

                    return CheckInstructionData(set);

                case (int)EIndex.ShippingNO:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    //排他処理
                    ret = SelectAndInsertExclusive(keyControls[index].Text, Exclusive_BL.DataKbn.Syukka);
                    if (!ret)
                        return false;

                    return CheckShippingData(set);

                    //case (int)EIndex.StoreCD:
                    //    //選択必須(Entry required)
                    //    if (!RequireCheck(new Control[] { keyControls[index] }))
                    //    {
                    //        return false;
                    //    }
                    //    else
                    //    {
                    //        if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                    //        {
                    //            bbl.ShowMessage("E141");
                    //            CboStoreCD.Focus();
                    //            return false;
                    //        }
                    //    }

                    //    break;

            }

            return true;

        }

        private bool SelectAndInsertExclusive(string no, Exclusive_BL.DataKbn dataKbn)
        {
            if (OperationMode == EOperationMode.SHOW)
                return true;
            
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();

            if (OperationMode == EOperationMode.INSERT)
            {
                DeleteExclusive(mOldInstructionNO, dataKbn);
                mOldInstructionNO = "";
            }
            else 
            {
                DeleteExclusive(mOldShippingNO, dataKbn);
                mOldShippingNO = "";                
            }

            if (string.IsNullOrWhiteSpace(no))
                return true;

            //排他Tableに該当番号が存在するとError
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)dataKbn,
                Number = no,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };


            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(), dt.Rows[0]["Operator"].ToString());
                if (dataKbn == Exclusive_BL.DataKbn.Syukka)
                    keyControls[(int)EIndex.ShippingNO].Focus();
                else
                    keyControls[(int)EIndex.InstructionNO].Focus();
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);
                if (dataKbn == Exclusive_BL.DataKbn.Syukka)
                    mOldShippingNO = keyControls[(int)EIndex.ShippingNO].Text;
                else
                    mOldInstructionNO = keyControls[(int)EIndex.InstructionNO].Text;
                    
                return ret;
            }
            
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
       private void DeleteExclusive(string no, Exclusive_BL.DataKbn dataKbn)
        {

            if (no == "" && dtForUpdate == null)
                return;

            Exclusive_BL ebl = new Exclusive_BL();

            if (dtForUpdate != null)
            {
                foreach (DataRow dr in dtForUpdate.Rows)
                {
                    D_Exclusive_Entity de = new D_Exclusive_Entity
                    {
                        DataKBN = Convert.ToInt16(dr["kbn"]),
                        Number = dr["no"].ToString()
                    };

                    ebl.D_Exclusive_Delete(de);
                }
                return;
            }

            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)dataKbn,
                Number = no,
            };

            bool ret = ebl.D_Exclusive_Delete(dee);
            
        }

        /// <summary>
        /// 出荷指示データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckInstructionData(bool set, int index = (int)EIndex.InstructionNO)
        {
            bool ret;

            //[D_Instruction_SelectData]
            die = new D_Instruction_Entity
            {
                InstructionNO = keyControls[index].Text
            };
                        
            DataTable dt = snbl.CheckInstruction(die);

            //出荷指示(D_Instruction)に存在しない場合、Error 「登録されていない出荷指示番号」
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E138", "出荷指示番号");
                Scr_Clr(1);
                ScInstructionNO.TxtCode.SelectAll();
                ScInstructionNO.TxtCode.Focus();
                return false;
            }
            else
            {
                //DeleteDateTime 「削除された出荷指示番号」
                if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                {
                    bbl.ShowMessage("E140", "出荷指示番号");
                    Scr_Clr(1);
                    ScInstructionNO.TxtCode.SelectAll();
                    ScInstructionNO.TxtCode.Focus();
                    return false;
                }

                //出荷済みでないこと
                dse = new D_Shipping_Entity
                {
                    InstructionNO = keyControls[index].Text
                };

                ret = snbl.CheckShukkaData(dse, out string errno);
                if (ret)
                {
                    if (!string.IsNullOrWhiteSpace(errno))
                    {
                        if (errno.Substring(0).Equals("Q"))
                        {
                            if (bbl.ShowMessage(errno) != DialogResult.Yes)
                                return false;
                        }
                        else
                        {
                            //Errorメッセージを表示する
                            bbl.ShowMessage(errno);
                            return false;
                        }
                    }
                }
            }

            //画面セットなしの場合、処理正常終了
            if (set == false)
            {
                return true;
            }

            string changeDate;
            if (!bbl.CheckDate(headControls[(int)EIndex.ShippingDate].Text))
            {
                changeDate = headControls[(int)EIndex.ShippingDate].Text;
            }
            else
            {
                changeDate = bbl.GetDate();
            };
            dt = snbl.D_Instruction_SelectDataForShukka(die, changeDate);

            Scr_Clr(1);   //画面クリア（明細部）

            
            int i = 0;

            //変更あり、納品書の判断
            bool flgChange = false;
            bool flgNouhinsyo = false;
            var lstJuchuuNo = new List<string>();

            foreach (DataRow row in dt.Rows)
            {
                if (bbl.Z_Set(row["UpdateCancelKBN"]) == 1)
                    flgChange = true;
                if (bbl.Z_Set(row["NouhinsyoFLG"]) == 1)
                    flgNouhinsyo = true;

                if (!string.IsNullOrWhiteSpace(row["JuchuuNO"].ToString()) && row["CancelKbn"].ToString() != "1" && lstJuchuuNo.Contains(row["JuchuuNO"].ToString()) == false)
                {
                    lstJuchuuNo.Add(row["JuchuuNO"].ToString());
                }
                    

            }

            //明細にデータをセット
            i = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (i == 0)
                {
                    //CboStoreCD.SelectedValue = row["StoreCD"];

                    //明細にデータをセット
                    //変更あり
                    lblChange.Visible = flgChange;
                    //即日出荷
                    lblSameDay.Visible = Convert.ToBoolean(bbl.Z_Set(row["OntheDayFLG"]));
                    //着指定
                    lblDecidedDelivery.Visible = Convert.ToBoolean(bbl.Z_Set(row["DecidedDeliveryKbn"]));
                    //代引き
                    lblDaibiki.Visible = Convert.ToBoolean(bbl.Z_Set(row["CashOnDelivery"]));
                    //納品書
                    lblDeliveryNote.Visible = flgNouhinsyo;

                    //出荷先
                    lblDelivery.Text = row["DeliveryName"].ToString();
                    //運送会社
                    CboCarrierName.SelectedValue = row["CarrierCD"];
                    //個口
                    headControls[(int)EIndex.UnitsCount].Text = bbl.Z_SetStr(row["UnitsCount"]);

                    //出荷備考
                    lblBikou.Text = row["CommentInStore"].ToString();

                    mSoukoCD = row["SoukoCD"].ToString();
                    mInstructionKBN = row["InstructionKBN"].ToString();

                    //明細行数設定
                    if (flgNouhinsyo)
                    {
                        GvDetail.RowCount = dt.Rows.Count + lstJuchuuNo.Count;
                        

                        foreach (var no in lstJuchuuNo)
                        {
                            for (int cnt = 0; cnt < GvDetail.Rows[i].Cells.Count; cnt++)
                            {
                                GvDetail.Rows[i].Cells[cnt].Value = "";
                            }

                            GvDetail.Rows[i].Cells["colJANCD"].Value = no;
                            GvDetail.Rows[i].Cells["colSKUName"].Value = "納品書";
                            GvDetail.Rows[i].Cells["colColorSizeName"].Value = "";
                            GvDetail.Rows[i].Cells["colInstructionSu"].Value = 1;
                            GvDetail.Rows[i].Cells["colShippingSu"].Value = 0;
                            GvDetail.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);

                            i++;
                        }
                        
                    }
                        
                    else
                        GvDetail.RowCount = dt.Rows.Count;
                }
                                
                GvDetail.Rows[i].Cells["colJANCD"].Value = row["JANCD"].ToString();
                GvDetail.Rows[i].Cells["colSKUName"].Value = row["SKUName"].ToString();
                GvDetail.Rows[i].Cells["colColorSizeName"].Value = row["ColorName"].ToString() + row["SizeName"].ToString() + row["CommentOutStore"].ToString(); 
                GvDetail.Rows[i].Cells["colInstructionSu"].Value = bbl.Z_Set(row["InstructionSu"]);
                GvDetail.Rows[i].Cells["colShippingSu"].Value = bbl.Z_Set(row["ShippingSu"]);
                //隠し項目
                GvDetail.Rows[i].Cells["colTani"].Value = row["TANI"].ToString();
                GvDetail.Rows[i].Cells["colUpdateCancelKBN"].Value = row["UpdateCancelKBN"].ToString();
                GvDetail.Rows[i].Cells["colCancelKbn"].Value = row["CancelKbn"].ToString();
                GvDetail.Rows[i].Cells["colSKUCD"].Value = row["SKUCD"].ToString();
                GvDetail.Rows[i].Cells["colAdminNO"].Value = bbl.Z_Set(row["AdminNO"]);
                GvDetail.Rows[i].Cells["colColorName"].Value = row["ColorName"].ToString();
                GvDetail.Rows[i].Cells["colSizeName"].Value = row["SizeName"].ToString();
                GvDetail.Rows[i].Cells["colCommentOutStore"].Value = row["CommentOutStore"].ToString();
                GvDetail.Rows[i].Cells["colInstructionRows"].Value = bbl.Z_Set(row["InstructionRows"]);
                GvDetail.Rows[i].Cells["colNumber"].Value = row["Number"].ToString();
                GvDetail.Rows[i].Cells["colNumberRows"].Value = bbl.Z_Set(row["NumberRows"]);
                GvDetail.Rows[i].Cells["colReserveNO"].Value = row["ReserveNO"].ToString();
                GvDetail.Rows[i].Cells["colReserveKBN"].Value = bbl.Z_Set(row["ReserveKBN"]);
                GvDetail.Rows[i].Cells["colStockNO"].Value = bbl.Z_Set(row["StockNO"]);
                GvDetail.Rows[i].Cells["colToStoreCD"].Value = row["ToStoreCD"].ToString();
                GvDetail.Rows[i].Cells["colToSoukoCD"].Value = row["ToSoukoCD"].ToString();
                GvDetail.Rows[i].Cells["colToRackNO"].Value = row["ToRackNO"].ToString();
                GvDetail.Rows[i].Cells["colToStockNO"].Value = row["ToStockNO"].ToString();
                GvDetail.Rows[i].Cells["colFromStoreCD"].Value = row["FromStoreCD"].ToString();
                GvDetail.Rows[i].Cells["colFromSoukoCD"].Value = row["FromSoukoCD"].ToString();
                GvDetail.Rows[i].Cells["colFromRackNO"].Value = row["FromRackNO"].ToString();
                GvDetail.Rows[i].Cells["colCustomerCD"].Value = row["CustomerCD"].ToString();

                //背景色
                GvDetail.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                if (bbl.Z_Set(row["UpdateCancelKBN"]) == 9)
                    GvDetail.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 0, 0);
                else if (bbl.Z_Set(row["CancelKbn"]) == 1)
                    GvDetail.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(191, 191, 191);

                i++;

            }

            S_BodySeigyo(1);

            return true;
        }

        /// <summary>
        /// 出荷データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckShippingData(bool set, int index= (int)EIndex.ShippingNO)
        {
            //[D_Shipping_SelectData]
            dse = new D_Shipping_Entity
            {
                ShippingNO = keyControls[(int)EIndex.ShippingNO].Text
            };

            DataTable dt = snbl.CheckShipping(dse);

            //出荷(D_Shipping)に存在しない場合、Error 「登録されていない出荷番号」
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E138", "出荷番号");
                Scr_Clr(1);
                keyControls[(int)EIndex.InstructionNO].Text = "";
                ScShippingNO.TxtCode.SelectAll();
                ScShippingNO.TxtCode.Focus();
                return false;
            }
            else
            {
                //DeleteDateTime 「削除された出荷番号」
                if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                {
                    bbl.ShowMessage("E140", "出荷番号");
                    Scr_Clr(1);
                    keyControls[(int)EIndex.InstructionNO].Text = "";
                    ScShippingNO.TxtCode.SelectAll();
                    ScShippingNO.TxtCode.Focus();
                    return false;
                }

                //取得した出荷データの出荷日が、入力できる範囲内の日付であること
                if (!bbl.CheckInputPossibleDate(dt.Rows[0]["ShippingDate"].ToString()))
                {
                    //Ｅ１１５
                    bbl.ShowMessage("E115");
                    return false;
                }

                //売上済みでないこと
                bool ret = snbl.CheckShukkaData(dse, out string errno);
                if (ret)
                {
                    if (!string.IsNullOrWhiteSpace(errno))
                    {
                        if (errno.Substring(0).Equals("Q"))
                        {
                            if (bbl.ShowMessage(errno) != DialogResult.Yes)
                                return false;
                        }
                        else
                        {
                            //Errorメッセージを表示する
                            bbl.ShowMessage(errno);
                            return false;
                        }
                    }
                    // Errorでない場合、画面転送表01に従ってデータ取得 / 画面表示
                }

                //画面セットなしの場合、処理正常終了
                if (set == false)
                {
                    return true;
                }

                dt = snbl.D_Shipping_SelectData(dse);

                Scr_Clr(1);   //画面クリア（明細部）

                //明細にデータをセット
                int i = 0;

                //変更あり、納品書の判断
                bool flgChange = false;
                bool flgNouhinsyo = false;

                foreach (DataRow row in dt.Rows)
                {
                    if (bbl.Z_Set(row["UpdateCancelKBN"]) == 1)
                        flgChange = true;
                    if (bbl.Z_Set(row["NouhinsyoFLG"]) == 1)
                        flgNouhinsyo = true;
                }

                foreach (DataRow row in dt.Rows)
                {
                    if (i == 0)
                    {

                        //明細にデータをセット
                        //出荷日
                        headControls[(int)EIndex.ShippingDate].Text = row["ShippingDate"].ToString();
                        mOldShippingDate = headControls[(int)EIndex.ShippingDate].Text;
                        //出荷指示番号
                        keyControls[(int)EIndex.InstructionNO].Text = row["InstructionNO"].ToString();

                        //変更あり
                        lblChange.Visible = flgChange;
                        //即日出荷
                        lblSameDay.Visible = Convert.ToBoolean(bbl.Z_Set(row["OntheDayFLG"]));
                        //着指定
                        lblDecidedDelivery.Visible = Convert.ToBoolean(bbl.Z_Set(row["DecidedDeliveryKbn"]));
                        //代引き
                        lblDaibiki.Visible = Convert.ToBoolean(bbl.Z_Set(row["CashOnDelivery"]));
                        //納品書
                        lblDeliveryNote.Visible = flgNouhinsyo;

                        //出荷先
                        lblDelivery.Text = row["DeliveryName"].ToString();
                        //運送会社
                        CboCarrierName.SelectedValue = row["CarrierCD"];
                        //個口
                        headControls[(int)EIndex.UnitsCount].Text = bbl.Z_SetStr(row["UnitsCount"]);

                        //出荷備考
                        lblBikou.Text = row["CommentInStore"].ToString();

                        mSoukoCD = row["SoukoCD"].ToString();
                        mInstructionKBN = row["InstructionKBN"].ToString();

                        GvDetail.RowCount = dt.Rows.Count;
                    }
                    
                    GvDetail.Rows[i].Cells["colJANCD"].Value = row["JANCD"].ToString();
                    GvDetail.Rows[i].Cells["colSKUName"].Value = row["SKUName"].ToString();
                    GvDetail.Rows[i].Cells["colColorSizeName"].Value = row["ColorName"].ToString() + row["SizeName"].ToString() + row["CommentOutStore"].ToString();
                    GvDetail.Rows[i].Cells["colInstructionSu"].Value = bbl.Z_Set(row["InstructionSu"]);
                    GvDetail.Rows[i].Cells["colShippingSu"].Value = bbl.Z_Set(row["ShippingSu"]);
                    //隠し項目
                    GvDetail.Rows[i].Cells["colTani"].Value = row["TANI"].ToString();
                    GvDetail.Rows[i].Cells["colUpdateCancelKBN"].Value = row["UpdateCancelKBN"].ToString();
                    GvDetail.Rows[i].Cells["colCancelKbn"].Value = row["CancelKbn"].ToString();
                    GvDetail.Rows[i].Cells["colSKUCD"].Value = row["SKUCD"].ToString();
                    GvDetail.Rows[i].Cells["colAdminNO"].Value = bbl.Z_Set(row["AdminNO"]);
                    GvDetail.Rows[i].Cells["colColorName"].Value = row["ColorName"].ToString();
                    GvDetail.Rows[i].Cells["colSizeName"].Value = row["SizeName"].ToString();
                    GvDetail.Rows[i].Cells["colCommentOutStore"].Value = row["CommentOutStore"].ToString();
                    GvDetail.Rows[i].Cells["colInstructionRows"].Value = bbl.Z_Set(row["InstructionRows"]);
                    GvDetail.Rows[i].Cells["colNumber"].Value = row["Number"].ToString();
                    GvDetail.Rows[i].Cells["colNumberRows"].Value = bbl.Z_Set(row["NumberRows"]);
                    GvDetail.Rows[i].Cells["colReserveNO"].Value = row["ReserveNO"].ToString();
                    GvDetail.Rows[i].Cells["colReserveKBN"].Value = bbl.Z_Set(row["ReserveKBN"]);
                    GvDetail.Rows[i].Cells["colStockNO"].Value = row["StockNO"].ToString();
                    GvDetail.Rows[i].Cells["colToStoreCD"].Value = row["ToStoreCD"].ToString();
                    GvDetail.Rows[i].Cells["colToSoukoCD"].Value = row["ToSoukoCD"].ToString();
                    GvDetail.Rows[i].Cells["colToRackNO"].Value = row["ToRackNO"].ToString();
                    GvDetail.Rows[i].Cells["colToStockNO"].Value = row["ToStockNO"].ToString();
                    GvDetail.Rows[i].Cells["colFromStoreCD"].Value = row["FromStoreCD"].ToString();
                    GvDetail.Rows[i].Cells["colFromSoukoCD"].Value = row["FromSoukoCD"].ToString();
                    GvDetail.Rows[i].Cells["colFromRackNO"].Value = row["FromRackNO"].ToString();
                    GvDetail.Rows[i].Cells["colCustomerCD"].Value = row["CustomerCD"].ToString();

                    //背景色
                    GvDetail.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                    if (bbl.Z_Set(row["UpdateCancelKBN"]) == 9)
                        GvDetail.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 0, 0);
                    else if (bbl.Z_Set(row["CancelKbn"]) == 1)
                        GvDetail.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(191, 191, 191);

                    i++;
                    
                }
            }

            S_BodySeigyo(2);

            return true;
        }

        /// <summary>
        /// HEAD部のコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckHead(int index, bool set=false)
        {

            switch (index)
            {
                case (int)EIndex.ShippingDate:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(headControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    headControls[index].Text = bbl.FormatDate(headControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(headControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    //入力できる範囲内の日付であること
                    if(!bbl.CheckInputPossibleDate(headControls[index].Text))
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        return false;
                    }

                    //日付が変わったら運送会社、明細の品名等再取得
                    string carrierCd = CboCarrierName.SelectedValue.ToString();
                    if (mOldShippingDate != headControls[index].Text)
                    {
                        CboCarrierName.Bind(headControls[index].Text);
                        CboCarrierName.SelectedValue = carrierCd;
                        
                        ////for (int RW = 0; RW <= GvDetail.RowCount - 1; RW++)
                        ////{
                        ////    M_SKU_Entity mse = new M_SKU_Entity
                        ////    {
                        ////        JanCD = GvDetail.Rows[RW].Cells["colJANCD"].Value.ToString(),
                        ////        SKUCD = null,
                        ////        AdminNO = GvDetail.Rows[RW].Cells["colAdminNO"].Value.ToString(),
                        ////        SetKBN = null,
                        ////        ChangeDate = headControls[(int)EIndex.ShippingDate].Text
                        ////    };
                        ////    SKU_BL mbl = new SKU_BL();
                        ////    DataTable dt = mbl.M_SKU_SelectAll(mse);
                        ////    if (dt.Rows.Count == 1)
                        ////    {
                        ////        DataRow selectRow = dt.Rows[0];
                        ////        GvDetail.Rows[RW].Cells["colSKUName"].Value = selectRow["SKUName"].ToString();
                        ////        GvDetail.Rows[RW].Cells["colColorName"].Value = selectRow["ColorName"].ToString();
                        ////        GvDetail.Rows[RW].Cells["colSizeName"].Value = selectRow["SizeName"].ToString();
                        ////        GvDetail.Rows[RW].Cells["colCommentOutStore"].Value = selectRow["CommentOutStore"].ToString();

                        ////        if (bbl.Z_Set(mAdminNO) == bbl.Z_Set(GvDetail.Rows[RW].Cells["colAdminNO"].Value))
                        ////        {
                        ////            lblSkuName.Text = selectRow["SKUName"].ToString();
                        ////            lblColor.Text = selectRow["ColorName"].ToString() + selectRow["SizeName"].ToString() + selectRow["CommentOutStore"].ToString();
                        ////        }
                        ////    }
                        ////}

                        mOldShippingDate = headControls[index].Text;
                    }
                    

                    ////店舗の締日チェック
                    ////店舗締マスターで判断
                    //M_StoreClose_Entity msce = new M_StoreClose_Entity
                    //{
                    //    StoreCD = CboStoreCD.SelectedValue.ToString(),
                    //    FiscalYYYYMM = headControls[index].Text.Replace("/", "").Substring(0, 6)
                    //};
                    //ret = bbl.CheckStoreClose(msce,false,false,false,false,true);
                    //if (!ret)
                    //{
                    //    return false;
                    //}

                    break;               

                case (int)EIndex.CarrierName:
                    //入力必須(Entry required)
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { headControls[index] }))
                    {
                        return false;
                    }
                    //[M_Carrier_Select]
                    string changeDate;
                    if (!bbl.CheckDate(headControls[(int)EIndex.ShippingDate].Text))
                    {
                        changeDate = headControls[(int)EIndex.ShippingDate].Text;
                    }
                    else
                    {
                        changeDate = bbl.GetDate();
                    };

                    M_Carrier_Entity me = new M_Carrier_Entity
                    {
                        CarrierCD = CboCarrierName.SelectedValue.ToString(),
                        ChangeDate = changeDate                        
                    };

                    DataTable mdt = snbl.M_Carrier_SelectForShukka(me);
                    if (mdt.Rows.Count == 0)
                    {                        
                        bbl.ShowMessage("E128");
                        headControls[index].Focus();
                        return false;
                    }

                    if (lblDaibiki.Visible == true &&  mdt.Rows[0]["NormalFLG"].ToString() != "1")
                    {
                        bbl.ShowMessage("E211");
                        headControls[index].Focus();
                        return false;
                    }

                    // 配送会社が変更されたら、箱サイズ・希望時間帯のコンボボックスを再取得
                    if (mOldCarrierCd != CboCarrierName.SelectedValue.ToString())
                    {
                        CboCarrierBoxSize.Bind(changeDate, me.CarrierCD);
                        CboCarrierDeliveryTime.Bind(changeDate, me.CarrierCD);
                    }


                    break;

                case (int)EIndex.CarrierBoxSize:
                    //入力必須(Entry required)
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { headControls[index] }))
                    {
                        return false;
                    }
                    
                    break;

                case (int)EIndex.UnitsCount:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(headControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    //0はエラー
                    if (bbl.Z_Set(headControls[index].Text) == 0)
                    {
                        //Ｅ１０８
                        bbl.ShowMessage("E108");
                        return false;
                    }                    
                    break;

                case (int)EIndex.DecidedDeliveryDate:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (!string.IsNullOrWhiteSpace(headControls[index].Text))
                    {
                        headControls[index].Text = bbl.FormatDate(headControls[index].Text);

                        //日付として正しいこと(Be on the correct date)Ｅ１０３
                        if (!bbl.CheckDate(headControls[index].Text))
                        {
                            //Ｅ１０３
                            bbl.ShowMessage("E103");
                            return false;
                        }
                        //入力できる範囲内の日付であること
                        if (string.Compare(headControls[index].Text, bbl.GetDate()) < -1)
                        {
                            //Ｅ１１５
                            bbl.ShowMessage("E276");
                            return false;
                        }
                    }
                    
                    break;

                case (int)EIndex.CarrierDeliveryTime:
                    //入力必須(Entry required)
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { headControls[index] }))
                    {
                        return false;
                    }

                    break;
            }

            return true;
        }


        /// <summary>
        /// 明細部のコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckDetail(int index, bool set = false)
        {
            bool ret;
            DataTable dt;

            switch (index)
            {
                case (int)EIndex.JANCD:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    //入力がある場合、SKUマスターに存在すること
                    //[M_SKU]

                    M_SKU_Entity mse = new M_SKU_Entity
                    {
                        JanCD = detailControls[index].Text,
                        SKUCD = null,                        
                        AdminNO = mAdminNO == "" ? null: mAdminNO,
                        SetKBN = null,
                        ChangeDate = headControls[(int)EIndex.ShippingDate].Text
                    };

                    SKU_BL mbl = new SKU_BL();
                    dt = mbl.M_SKU_SelectAll(mse);
                    DataRow selectRow = null;

                    if (dt.Rows.Count == 0)
                    {
                        //受注テーブルに存在すること
                        //[D_Juchuu]
                        D_Juchuu_Entity de = new D_Juchuu_Entity
                        {
                            JuchuuNO = detailControls[index].Text,
                        };

                        snbl = new ShukkaNyuuryoku_BL();
                        dt = snbl.D_Juchuu_SelectData_ForShukka(de);

                        if (dt.Rows.Count == 0)
                        {
                            //Ｅ１０１
                            bbl.ShowMessage("E128");
                            return false;
                        }
                        else
                        { 
                            mAdminNO = "";
                            mJANCD = detailControls[index].Text;
                            lblSKUCD.Text = "";
                            lblSkuName.Text = "納品書";
                            lblColor.Text = "";
                            lblTani.Text = "";

                            detailControls[(int)EIndex.ShippingSu].Text = "1";
                            if (CalcSu(mJANCD, mAdminNO, 1) == false)
                            {
                                return false;
                            }
                        }
                    }
                    else if (dt.Rows.Count == 1)
                    {
                        selectRow = dt.Rows[0];
                    }
                    else
                    {
                        //JANCDでSKUCDが複数存在する場合（If there is more than one）
                        using (Select_SKU frmSKU = new Select_SKU())
                        {
                            frmSKU.parJANCD = dt.Rows[0]["JanCD"].ToString();
                            frmSKU.parChangeDate = headControls[(int)EIndex.ShippingDate].Text;
                            frmSKU.ShowDialog();

                            if (!frmSKU.flgCancel)
                            {
                                selectRow = dt.Select(" AdminNO = " + frmSKU.parAdminNO)[0];
                            }
                        }
                    }

                    if (selectRow != null)
                    {
                        //変更なしの場合は再セットしない
                        //if (mAdminNO.Equals(selectRow["AdminNO"].ToString()) && set == false)
                        //    return true;

                        //JANCDでSKUCDが１つだけ存在する場合（If there is only one）
                        mAdminNO = selectRow["AdminNO"].ToString();
                        mJANCD = selectRow["JANCD"].ToString();
                        lblSKUCD.Text = selectRow["SKUCD"].ToString();
                        lblSkuName.Text = selectRow["SKUName"].ToString();
                        lblColor.Text = selectRow["ColorName"].ToString() + selectRow["SizeName"].ToString() + selectRow["CommentOutStore"].ToString();
                        lblTani.Text = selectRow["TaniName"].ToString();
                        
                        //明細存在チェック
                        bool blExist = false;
                        mFlgCancel = false;
                        for (int RW = 0; RW <= GvDetail.RowCount - 1; RW++)
                        {
                            if (GvDetail.Rows[RW].Cells["colJANCD"].Value.ToString() == selectRow["JanCD"].ToString() && bbl.Z_Set(GvDetail.Rows[RW].Cells["colAdminNO"].Value) == bbl.Z_Set(selectRow["AdminNO"]))
                            {
                                if (GvDetail.Rows[RW].Cells["colUpdateCancelKBN"].Value.ToString() == "9" || GvDetail.Rows[RW].Cells["colCancelKbn"].Value.ToString() == "1")
                                    mFlgCancel = true;
                                blExist = true;
                                break;
                            }
                        }
                        if (blExist == false)
                        {
                            //Ｅ１０１
                            bbl.ShowMessage("E213");
                            return false;
                        }

                        if (mFlgCancel == false)
                        {
                            detailControls[(int)EIndex.ShippingSu].Text = "1";
                            if (CalcSu(selectRow["JanCD"].ToString(), selectRow["AdminNO"].ToString(), 1) == false)
                            {
                                return false;
                            }
                        }
                        
                    }
                    break;

                case (int)EIndex.ShippingSu:
                    if (mFlgCancel == true)
                        return true;

                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    //出荷数≦0の場合、エラー
                    if (bbl.Z_Set(detailControls[index].Text) <= 0)
                    {
                        //Ｅ１４３
                        bbl.ShowMessage("E143", "1", "小さい");
                        return false;
                    }

                    if (CalcSu(detailControls[(int)EIndex.JANCD].Text, mAdminNO, bbl.Z_Set(detailControls[(int)EIndex.ShippingSu].Text)) == false)
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        /// Footer部 出荷数計算処理
        /// </summary>
        private bool CalcSu(string janCd, string adminNO, decimal su)
        {
            decimal shukkaKanouSu = 0;

            for (int RW = 0; RW < GvDetail.RowCount; RW++)
            {
                if (GvDetail.Rows[RW].Cells["colJANCD"].Value.ToString() == janCd && bbl.Z_Set(GvDetail.Rows[RW].Cells["colAdminNO"].Value) == bbl.Z_Set(adminNO))
                {
                    if (GvDetail.Rows[RW].Cells["colCancelKbn"].Value.ToString() != "1" && GvDetail.Rows[RW].Cells["colUpdateCancelKBN"].Value.ToString() != "9")
                    {
                        shukkaKanouSu = (bbl.Z_Set(GvDetail.Rows[RW].Cells["colInstructionSu"].Value) - bbl.Z_Set(GvDetail.Rows[RW].Cells["colShippingSu"].Value));
                        if (shukkaKanouSu > su)
                        {
                            GvDetail.Rows[RW].Cells["colShippingSu"].Value = bbl.Z_Set(GvDetail.Rows[RW].Cells["colShippingSu"].Value) + su;                            
                            su = 0;
                            break;
                        }
                        else
                        {
                            GvDetail.Rows[RW].Cells["colShippingSu"].Value = bbl.Z_Set(GvDetail.Rows[RW].Cells["colShippingSu"].Value) + shukkaKanouSu;
                            //背景色緑に
                            GvDetail.Rows[RW].DefaultCellStyle.BackColor = Color.FromArgb(226, 239, 218);
                            su = su - shukkaKanouSu;
                        }
                    }
                }

            }

            if (su > 0)
            {
                //Ｅ１４３
                bbl.ShowMessage("E143", "指示数", "大きい");
                return false;

            }     

            return true;
        }
      
        /// <summary>
        /// 更新前チェック処理
        /// </summary>
        private bool CheckAll()
        {
            for (int i = 0; i < headControls.Length; i++)
            {
                if (CheckHead(i) == false)
                {
                    headControls[i].Focus();
                    return false;
                }       
            }

            //出荷数チェック処理
            for (int RW = 0; RW <= GvDetail.RowCount - 1; RW++)
            {
                if (GvDetail.Rows[RW].Cells["colCancelKbn"].Value.ToString() != "1" && GvDetail.Rows[RW].Cells["colUpdateCancelKBN"].Value.ToString() != "9")
                {
                    if (bbl.Z_Set(GvDetail.Rows[RW].Cells["colInstructionSu"].Value) != bbl.Z_Set(GvDetail.Rows[RW].Cells["colShippingSu"].Value))
                    {
                        //Ｅ１４３
                        bbl.ShowMessage("E195", "指示数", "出荷数");
                        return false;
                    }
                }
                               
            }

            return true;
        }

        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_Shipping_Entity GetEntity()
        {
            dse = new D_Shipping_Entity
            {
                ShippingNO = keyControls[(int)EIndex.ShippingNO].Text,
                ShippingDate = headControls[(int)EIndex.ShippingDate].Text,
                ShippingKBN = mInstructionKBN,
                InstructionNO = keyControls[(int)EIndex.InstructionNO].Text,                
                SoukoCD = mSoukoCD,
                CarrierCD = CboCarrierName.SelectedValue.ToString(),
                StaffCD = InOperatorCD,
                UnitsCount = headControls[(int)EIndex.UnitsCount].Text,
                
                InsertOperator = InOperatorCD,
                PC = InPcID
            };

            return dse;
        }

        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            int rowNo = 1;

            for (int RW = 0; RW <= GvDetail.RowCount - 1; RW++)
            {

                if (GvDetail.Rows[RW].Cells["colCancelKbn"].Value.ToString() != "1" && GvDetail.Rows[RW].Cells["colUpdateCancelKBN"].Value.ToString() != "9")
                {
                    dt.Rows.Add(
                         rowNo                       
                       , bbl.Z_Set(GvDetail.Rows[RW].Cells["colInstructionRows"].Value)
                       , string.IsNullOrWhiteSpace(GvDetail.Rows[RW].Cells["colNumber"].Value.ToString()) ? null : GvDetail.Rows[RW].Cells["colNumber"].Value.ToString()
                       , bbl.Z_Set(GvDetail.Rows[RW].Cells["colNumberRows"].Value)
                       , GvDetail.Rows[RW].Cells["colReserveNO"].Value.ToString()
                       , bbl.Z_Set(GvDetail.Rows[RW].Cells["colReserveKBN"].Value)
                       , string.IsNullOrWhiteSpace(GvDetail.Rows[RW].Cells["colSKUCD"].Value.ToString()) ? null : GvDetail.Rows[RW].Cells["colSKUCD"].Value.ToString()
                       , bbl.Z_Set(GvDetail.Rows[RW].Cells["colAdminNO"].Value)
                       , GvDetail.Rows[RW].Cells["colJANCD"].Value.ToString()
                       , string.IsNullOrWhiteSpace(GvDetail.Rows[RW].Cells["colSKUName"].Value.ToString()) ? null : GvDetail.Rows[RW].Cells["colSKUName"].Value.ToString()
                       , string.IsNullOrWhiteSpace(GvDetail.Rows[RW].Cells["colColorName"].Value.ToString()) ? null : GvDetail.Rows[RW].Cells["colColorName"].Value.ToString()
                       , string.IsNullOrWhiteSpace(GvDetail.Rows[RW].Cells["colSizeName"].Value.ToString()) ? null : GvDetail.Rows[RW].Cells["colSizeName"].Value.ToString()
                       , bbl.Z_Set(GvDetail.Rows[RW].Cells["colShippingSu"].Value)
                       , string.IsNullOrWhiteSpace(GvDetail.Rows[RW].Cells["colStockNO"].Value.ToString()) ? null : GvDetail.Rows[RW].Cells["colStockNO"].Value.ToString()
                       , GvDetail.Rows[RW].Cells["colToStoreCD"].Value.ToString()
                       , GvDetail.Rows[RW].Cells["colToSoukoCD"].Value.ToString()
                       , string.IsNullOrWhiteSpace(GvDetail.Rows[RW].Cells["colToRackNO"].Value.ToString()) ? null : GvDetail.Rows[RW].Cells["colToRackNO"].Value.ToString()
                       , string.IsNullOrWhiteSpace(GvDetail.Rows[RW].Cells["colToStockNO"].Value.ToString()) ? null : GvDetail.Rows[RW].Cells["colToStockNO"].Value.ToString()
                       , GvDetail.Rows[RW].Cells["colFromStoreCD"].Value.ToString()
                       , GvDetail.Rows[RW].Cells["colFromSoukoCD"].Value.ToString()
                       , GvDetail.Rows[RW].Cells["colFromRackNO"].Value.ToString()
                       , GvDetail.Rows[RW].Cells["colCustomerCD"].Value.ToString()
                       );
                    rowNo++;
                }
                
            }
            
            return dt;
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("ShippingRows", typeof(int));
            dt.Columns.Add("InstructionRows", typeof(int));
            dt.Columns.Add("Number", typeof(string));
            dt.Columns.Add("NumberRows", typeof(int));
            dt.Columns.Add("ReserveNO", typeof(string));
            dt.Columns.Add("ReserveKBN", typeof(int));
            dt.Columns.Add("SKUCD", typeof(string));
            dt.Columns.Add("AdminNO", typeof(int));
            dt.Columns.Add("JanCD", typeof(string));
            dt.Columns.Add("SKUName", typeof(string));
            dt.Columns.Add("ColorName", typeof(string));
            dt.Columns.Add("SizeName", typeof(string));
            dt.Columns.Add("ShippingSu", typeof(int));   
            dt.Columns.Add("StockNO", typeof(string));
            dt.Columns.Add("ToStoreCD", typeof(string));
            dt.Columns.Add("ToSoukoCD", typeof(string));
            dt.Columns.Add("ToRackNO", typeof(string));
            dt.Columns.Add("ToStockNO", typeof(string));
            dt.Columns.Add("FromStoreCD", typeof(string));
            dt.Columns.Add("FromSoukoCD", typeof(string));
            dt.Columns.Add("FromRackNO", typeof(string));
            dt.Columns.Add("CustomerCD", typeof(string));
        }
                
        protected override void ExecSec()
        {
            DataTable dt = GetGridEntity();
            
            //更新処理
            dse = GetEntity();

            bool ret = snbl.Shipping_Exec(dse, dt, (short)OperationMode, CboStoreCD.SelectedValue.ToString());
            if(!ret)
            {
                bbl.ShowMessage("E000");
                return;
            }

            if (OperationMode == EOperationMode.DELETE)
                bbl.ShowMessage("I102");
            else
                bbl.ShowMessage("I101");

            //更新後画面クリア
            ChangeOperationMode(OperationMode);
        }
        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,3:削除,4:照会)

            //排他処理を解除
            DeleteExclusive(mOldInstructionNO, Exclusive_BL.DataKbn.SyukkaShiji);
            DeleteExclusive(mOldShippingNO, Exclusive_BL.DataKbn.Syukka);                

            //画面クリア
            Scr_Clr(0);

            //画面制御
            S_BodySeigyo(0);

            switch (mode)
            {
                case EOperationMode.INSERT:
                    headControls[(int)EIndex.ShippingDate].Enabled = true;
                    keyControls[(int)EIndex.InstructionNO].Focus();
                    break;

                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    keyControls[(int)EIndex.ShippingNO].Focus();
                    break;

            }

        }

        /// <summary>
        /// 画面クリア(0:全項目、1:KEY部以外、2:明細のみ)
        /// </summary>
        /// <param name="Kbn"></param>
        private void Scr_Clr(short Kbn)
        {
            //カスタムコントロールのLeave処理を先に走らせるため
            //IMT_DMY_0.Focus();

            if (Kbn == 0)
            {
                foreach (Control ctl in keyControls)
                {
                    if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                    {
                        ((CheckBox)ctl).Checked = false;
                    }
                    else
                    {
                        ctl.Text = "";
                    }
                }

                foreach (Control ctl in keyLabels)
                {
                    ctl.Text = "";
                }
            }

            if (Kbn != 2)
            {
                foreach (Control ctl in headControls)
                {
                    if (ctl.GetType().Equals(typeof(CKM_Controls.CKMShop_ComboBox)))
                    {
                        ((CKM_Controls.CKMShop_ComboBox)ctl).SelectedIndex = -1;
                    }
                    else
                    {
                        ctl.Text = "";
                    }
                }

                foreach (Control ctl in headLabels)
                {
                    ctl.Text = "";
                }

                foreach (Control ctl in headLabels2)
                {
                    ctl.Visible = false;
                }
            }

            foreach (Control ctl in detailControls)
            {
                if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKMShop_ComboBox)))
                {
                    ((CKM_Controls.CKMShop_ComboBox)ctl).SelectedIndex=-1;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_Button)) || ctl.GetType().Equals(typeof(Button)))
                {

                }
                else
                {
                    ctl.Text = "";
                }
            }

            foreach (Control ctl in detailLabels)
            {
                ctl.Text = "";
            }

            mAdminNO = "";
            mJANCD = "";
            mOldShippingDate = "";
            mSoukoCD = "";
            mInstructionKBN = "";
            mFlgCancel = false;

            GvDetail.DataSource = null;
            GvDetail.RowCount = 0;

            if (Kbn != 0 && OperationMode == EOperationMode.INSERT)
                InitScr();

        }

        /// <summary>
        /// 画面制御
        /// </summary>
        /// <param name="no1"></param>
        /// <param name="no2"></param>
        /// <param name="Kbn"></param>
        private void Scr_Lock(short no1, short no2, short Kbn)
        {
            short i;
            for (i = no1; i <= no2; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            // ｷｰ部（削除）                            
                            keyControls[(int)EIndex.ShippingNO].Enabled = Kbn == 0 ? true : false;
                            ScShippingNO.BtnSearch.Enabled = Kbn == 0 ? true : false;
                            break;
                        }

                    case 1:
                        {
                            // ｷｰ部（新規） 
                            keyControls[(int)EIndex.InstructionNO].Enabled = Kbn == 0 ? true : false;                            
                            ScInstructionNO.BtnSearch.Enabled = Kbn == 0 ? true : false;
                            break;
                        }

                    case 2:
                        {
                            // HEAD部
                            foreach (Control ctl in headControls)
                            {
                                ctl.Enabled = Kbn == 0 ? true : false;
                            }
                            break;
                        }

                    case 3:
                        {
                            // 明細部
                            foreach (Control ctl in detailControls)
                            {
                                ctl.Enabled = Kbn == 0 ? true : false;
                            }
                            for (int index = 0; index < searchButtons.Length; index++)
                                searchButtons[index].Enabled = Kbn == 0 ? true : false;

                            //GvDetail.Enabled = Kbn == 0 ? true : false;
                            BtnSubF12.Enabled = Kbn == 0 ? true : false;
                            break;
                        }
                }
            }
        }

        // ---------------------------------------------
        // ボディ部の状態制御
        // 
        // 引数    pKBN    0...  新規/修正/削除時(モード選択時)
        //                 1...  新規時(画面展開後)
        //                 2...  照会/削除時(画面展開後)明細入力不可、スクロールのみ
        // ---------------------------------------------
        private void S_BodySeigyo(short pKBN)
        {

            switch (pKBN)
            {
                case 0:
                    {
                        
                        //IMT_DMY_0.Focus();

                        if (OperationMode == EOperationMode.INSERT)
                        {                            
                            InitScr();
                            Scr_Lock(0, 0, 1);
                            Scr_Lock(1, 2, 0);
                            Scr_Lock(3, mc_L_END, 1);  
                            SetFuncKeyAll(this, "111111001000");
                        }
                        else
                        {
                            Scr_Lock(0, 0, 0);
                            Scr_Lock(1, mc_L_END, 1);

                            SetFuncKeyAll(this, "111111001000");
                        }
                        break;
                    }

                case 1: //新規時                    
                    {
                        //明細入力可能
                        Scr_Lock(3, mc_L_END, 0);
                        Scr_Lock(1, 1, 1);
                        SetFuncKeyAll(this, "111111001001");
                        break;
                    }

                case 2: //削除時
                    {                        
                        if (OperationMode == EOperationMode.DELETE)
                        {
                            Scr_Lock(2, mc_L_END, 1);
                            SetFuncKeyAll(this, "111111000001");
                            BtnSubF12.Enabled = true;
                            //IMT_DMY_0.Focus();
                            Scr_Lock(0, 0, 0);
                        }
                        else
                        {
                            Scr_Lock(2, mc_L_END, 1);                            
                            SetFuncKeyAll(this, "111111000000");
                            Scr_Lock(0, 0, 0);
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// handle f1 to f12 click event
        /// implement base virtual function
        /// </summary>
        /// <param name="Index"></param>
        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);

            switch (Index)
            {
                case 0: // F1:終了
                    {
                        break;
                    }
                case 1:     //F2:新規
                    {
                        ChangeOperationMode((EOperationMode)Index);

                        break;
                    }
                case 2:     //F3:変更
                case 3:     //F4:削除
                    {
                        ChangeOperationMode((EOperationMode)Index);

                        break;
                    }
                case 4:     //F5:照会 
                    {
                        ChangeOperationMode((EOperationMode)Index);

                        break;
                    }
                case 5: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;

                        ChangeOperationMode(base.OperationMode);

                        break;
                    }
                case 6://F7:行削除
                case 7://F8:行追加
                case 9://F10複写
                    break;

                case 8: //F9:検索
                    if (previousCtrl.Name.Equals(txtJANCD.Name))
                    {
                        //商品検索
                            SearchData(EsearchKbn.Product, previousCtrl);
                    }
                    break;

                case 11:    //F12:登録
                    {
                        if (OperationMode == EOperationMode.DELETE)
                        { //Ｑ１０２		
                            if (bbl.ShowMessage("Q102") != DialogResult.Yes)
                                return;
                        }
                        else
                        {
                            //明細の出荷数と出荷指示数のチェック
                            if (CheckAll() == false)
                            {
                                return;
                            }

                            //Ｑ１０１		
                            if (bbl.ShowMessage("Q101") != DialogResult.Yes)
                                return;
                        }


                        this.ExecSec();
                        break;
                    }
            }   //switch end

        }

        // ==================================================
        // 終了処理
        // ==================================================
        protected override void EndSec()
        {
            try
            {                
                DeleteExclusive(mOldInstructionNO, Exclusive_BL.DataKbn.SyukkaShiji);
                DeleteExclusive(mOldShippingNO, Exclusive_BL.DataKbn.Syukka);
            }
            catch(Exception ex)
            {
                //例外は無視する
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            this.Close();
            //アプリケーションを終了する
            //Application.Exit();
            //System.Environment.Exit(0);
        }

        /// <summary>
        /// 検索フォーム起動処理
        /// </summary>
        /// <param name="kbn"></param>
        /// <param name="setCtl"></param>
        private void SearchData(EsearchKbn kbn, Control setCtl)
        {
            switch (kbn)
            {
                case EsearchKbn.Product:
                    using (Search_Product frmProduct = new Search_Product(headControls[(int)EIndex.ShippingDate].Text))
                    {
                        frmProduct.Mode = "4";
                        frmProduct.ShowDialog();

                        if (!frmProduct.flgCancel)
                        {
                            detailControls[(int)EIndex.JANCD].Text = frmProduct.JANCD;
                            mJANCD = frmProduct.JANCD;                            
                            mAdminNO = frmProduct.AdminNO;

                            setCtl.Focus();

                            CheckDetail((int)EIndex.JANCD, true);
                        }
                    }
                    break;
            }

        }

        #region "内部イベント"
        private void KeyControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int index = Array.IndexOf(keyControls, sender);
                    bool ret = CheckKey(index);

                    if (ret)
                    {
                        if(index == (int)EIndex.InstructionNO)
                                headControls[(int)EIndex.CarrierName].Focus();

                    }
                    else
                    {
                        ((Control)sender).Focus();
                    }
                }
            }

            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }

        }

        private void HeadControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int index = Array.IndexOf(headControls, sender);
                    bool ret = CheckHead(index);
                    if (ret)
                    {
                        if (headControls.Length - 1 > index)
                        {
                            if (headControls[index + 1].CanFocus)
                                headControls[index + 1].Focus();
                            else
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                ProcessTabKey(!e.Shift);
                        }
                        else
                        {
                            if (detailControls[0].CanFocus)
                                detailControls[0].Focus();
                        }
                            
                    }
                    else
                    {
                        ((Control)sender).Focus();
                    }
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        

        private void DetailControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int index = Array.IndexOf(detailControls, sender);
                    if (index == (int)EIndex.JANCD)
                        mAdminNO = "";

                    bool ret = CheckDetail(index);
                    if (ret)
                    {
                        if (index == (int)EIndex.JANCD)
                        {
                            ((CKM_Controls.CKM_TextBox)sender).SelectAll();
                            ((CKM_Controls.CKM_TextBox)sender).Focus();
                        }
                        else if (index == (int)EIndex.ShippingSu)
                        {
                            BtnSubF12.Focus();
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
                        ((CKM_Controls.CKM_TextBox)sender).SelectAll();
                        ((CKM_Controls.CKM_TextBox)sender).Focus();
                    }
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void BtnSubF11_Click(object sender, EventArgs e)
        {
            //表示ボタンClick時   
            try
            {
                base.FunctionProcess(FuncDisp - 1);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }

        }

        private void KeyControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
                //SetFuncKeyAll(this, "111111001011");
            }

            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void HeadControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;

                int index = Array.IndexOf(headControls, sender);
                //if (index == (int)EIndex.JANCD)
                //{
                //    F9Visible = true;
                //}
                //SetFuncKeyAll(this, "111111001011");
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void DetailControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;

                int index = Array.IndexOf(detailControls, sender);
                if(index == (int)EIndex.JANCD)
                {
                    F9Visible = true;
                }
                //SetFuncKeyAll(this, "111111001011");
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void DetailControl_Leave(object sender, EventArgs e)
        {
            try
            {
                F9Visible = false;
                
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {

        }        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnF12_Click(object sender, EventArgs e)
        {
            try
            {
                FunctionProcess(FuncExec - 1);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void BtnJANCD_Click(object sender, EventArgs e)
        {
            try
            {
                SearchData(EsearchKbn.Product, txtJANCD);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        #endregion
   
        private void GvDetail_Click(object sender, EventArgs e)
        {
            //if (detailControls[(int)EIndex.JANCD].Enabled == false)
            //    return;

            if (GvDetail.CurrentRow != null && GvDetail.CurrentRow.Index >= 0)
            {
                detailControls[(int)EIndex.JANCD].Text = GvDetail.CurrentRow.Cells["colJANCD"].Value.ToString();
                detailLabels[(int)EIndex.SKUCD].Text = GvDetail.CurrentRow.Cells["colSKUCD"].Value.ToString();
                detailLabels[(int)EIndex.SKUName].Text = GvDetail.CurrentRow.Cells["colSKUName"].Value.ToString();
                detailLabels[(int)EIndex.Color].Text = GvDetail.CurrentRow.Cells["colColorName"].Value.ToString() + GvDetail.CurrentRow.Cells["colSizeName"].Value.ToString() + GvDetail.CurrentRow.Cells["colCommentOutStore"].Value.ToString();
                detailLabels[(int)EIndex.Tani].Text = GvDetail.CurrentRow.Cells["colTani"].Value.ToString();
                if (OperationMode == EOperationMode.DELETE)
                    detailControls[(int)EIndex.ShippingSu].Text = GvDetail.CurrentRow.Cells["colShippingSu"].Value.ToString();
                else
                    detailControls[(int)EIndex.ShippingSu].Text = "";
                
                mAdminNO = GvDetail.CurrentRow.Cells["colAdminNo"].Value.ToString();
                mJANCD = GvDetail.CurrentRow.Cells["colJANCD"].Value.ToString();

                if (GvDetail.CurrentRow.Cells["colUpdateCancelKBN"].Value.ToString() == "9" || GvDetail.CurrentRow.Cells["colCancelKbn"].Value.ToString() == "1")
                    mFlgCancel = true;
                else
                    mFlgCancel = false;

                detailControls[(int)EIndex.ShippingSu].Focus();
                
            }
        }
    }
}








