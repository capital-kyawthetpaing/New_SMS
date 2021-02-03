using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using BL;
using System.Data;
using Entity;

namespace CKM_Controls
{
    public partial class CKM_ComboBox : ComboBox
    {
        private CboType type = 0;
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("tableName")]
        [DisplayName("Type")]
        public CboType Cbo_Type
        {
            get => type;
            set => type = value;
        }
        public enum CboType
        {
            Default,
            /// <summary>
            /// Type=1,StoreKBN IN (2,3)
            /// Type=2,All StoreCD
            /// </summary>
            店舗ストア,
            /// <summary>
            /// StoreKBN IN (1,3)
            /// </summary>
            店舗ストア_見積,
            /// <summary>
            /// StoreKBN IN (1)
            /// </summary>
            店舗ストア_受注,
            /// <summary>
            /// StoreKBN NOT IN (2)
            /// </summary>
            店舗ストア_月次,
            倉庫種別,
            部門,
            メニュー,
            処理権限,
            店舗権限,
            役職,
            データ種別,
            貨幣金種名,
            支払予定月,
            支払金種,
            ///*店舗名*/,
            受注確度,
            /// <summary>	
            /// 汎用マスタから金種区分マスタへ変更	
            /// </summary>
            予定金種,
            年度,
            シーズン,
            予約フラグ,
            特記フラグ,
            送料条件,
            要加工品区分,
            要確認品区分,
            タグ,
            発注,
            予約,      //20200316       
            特記,
            送料,
            発注フラグ,//20200316
            年度フラグ,//20200427
            シーズンフラグ,//20200427

            /// <summary>
            /// SoukoType IN (3,4)
            /// </summary>
            倉庫,
            入荷予定状況,
            識別,
            店舗,
            WarehouseSelectAll, //for tanabannyuuyoku (pnz)
            取込種別,
            銀行口座,
            /// <summary>
            /// All SoukoCD
            /// </summary>
            入荷倉庫,
            /// <summary>
            /// SoukoType=8
            /// </summary>
            返品倉庫,

            /// <summary>
            /// SoukoType IN (1,2,3,4)
            /// </summary>
            SoukoAll,//Added by ETZ for PickingList

            /// <summary>	
            /// SoukoType=1～4	
            /// </summary>	
            出荷指示倉庫,
            棚卸倉庫,
            在庫照会倉庫,
            移動区分,
            移動依頼区分,
            運送会社,
            配送会社,
            マークダウン倉庫,   
            在庫情報,
            出荷倉庫,
            箱サイズ,
            希望時間帯
        }

        private int length = 10;
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Set Max Length")]
        [DisplayName("MaxLength")]
        public int Length
        {
            get { return length; }
            set
            {
                length = value;
                CalculateWidth();
            }
        }

        private Bytes CtrlByte { get; set; }
        public enum Bytes
        {
            半角 = 0,
            半全角 = 1
        }
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Full width or Half width")]
        [DisplayName("Character Type")]
        public Bytes Ctrl_Byte
        {
            get { return CtrlByte; }
            set
            {
                CtrlByte = value;
                CalculateWidth();
            }
        }

        private bool IsRequire { get; set; } = false;

        public bool MoveNext { get; set; } = true;

        public string StoreCD;
        public string StoreAthuorizationCD;
        public string StoreAuthorizationChangeDate;
        public string ProgramID;

        Base_BL bbl;
        public CKM_ComboBox()
        {
            bbl = new Base_BL();
            AutoCompleteMode = AutoCompleteMode.Append;
            AutoCompleteSource = AutoCompleteSource.ListItems;

            Enter += Cbo_Enter;
            Leave += Cbo_Leave;

            //Required for ownerdraw
            DrawItem += new DrawItemEventHandler(EnableDisplayCombo_DrawItem);
            ////// Required for ownerdraw
            //DrawMode = DrawMode.OwnerDrawFixed;
            // EnabledChanged += new EventHandler(EnableDisplayCombo_EnabledChanged);
        }
        public bool AcceptKey = false;
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (!AcceptKey)
            {
                e.Handled = true;
            }
            base.OnKeyPress(e);
        }
        private void Cbo_Enter(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(255, 242, 204);
        }

        private void Cbo_Leave(object sender, EventArgs e)
        {
            BackColor = SystemColors.Window;
        }

        public void Bind(string changeDate, string type = null)
        {
            M_Store_Entity mse = new M_Store_Entity
            {
                ChangeDate = changeDate.Replace("/", "-"),
                DeleteFlg = "0",
                Type = type,
                StoreCD = type
            };
            switch (Cbo_Type)
            {
                case CboType.店舗ストア:
                    Store_BL sbl = new Store_BL();
                    DataTable dtStore = sbl.BindStore(mse);
                    BindCombo("StoreCD", "StoreName", dtStore);
                    break;
                case CboType.店舗ストア_見積:
                    Store_BL stbl = new Store_BL();
                    M_Store_Entity mse2 = new M_Store_Entity
                    {
                        ChangeDate = changeDate.Replace("/", "-"),
                        DeleteFlg = "0"
                    };
                    //StoreKBN IN (1,3)のStore情報をBind
                    DataTable dtStore2 = stbl.M_Store_Bind_Mitsumori(mse2);
                    BindCombo("StoreCD", "StoreName", dtStore2);
                    break;
                case CboType.店舗ストア_受注:
                    Store_BL sblj = new Store_BL();
                    M_Store_Entity mse3 = new M_Store_Entity
                    {
                        ChangeDate = changeDate.Replace("/", "-"),
                        DeleteFlg = "0"
                    };
                    //StoreKBN IN 1のStore情報をBind
                    DataTable dtStore3 = sblj.M_Store_Bind_Juchu(mse3);
                    BindCombo("StoreCD", "StoreName", dtStore3);
                    break;
                case CboType.店舗ストア_月次:
                    Store_BL sblg = new Store_BL();
                    M_Store_Entity mse4 = new M_Store_Entity
                    {
                        ChangeDate = changeDate.Replace("/", "-"),
                        Operator = type,
                        DeleteFlg = "0"
                    };
                    //StoreKBN NOT IN 2のStore情報をBind（権限のある店舗のみ）
                    DataTable dtStore4 = sblg.M_Store_Bind_Getsuji(mse4);
                    BindCombo("StoreCD", "StoreName", dtStore4);
                    break;
                case CboType.倉庫種別:
                    MultiPorpose_BL mpbl = new MultiPorpose_BL();
                    M_MultiPorpose_Entity mme = new M_MultiPorpose_Entity();
                    mme.ID = mpbl.ID_Store;
                    DataTable dtSoukoType = mpbl.M_MultiPorpose_SoukoTypeSelect(mme);
                    BindCombo("Key", "IDName", dtSoukoType);
                    break;
                case CboType.部門:
                    Staff_BL staffBL1 = new Staff_BL();
                    DataTable dtBMNCD = staffBL1.BindBMN();
                    BindCombo("Key", "Char1", dtBMNCD);
                    break;
                case CboType.メニュー:
                    Staff_BL staffBL2 = new Staff_BL();
                    DataTable dtMenu = staffBL2.BindMenu();
                    BindCombo("MenuID", "MenuName", dtMenu);
                    break;
                case CboType.処理権限:
                    Staff_BL staffBL3 = new Staff_BL();
                    DataTable dtAuthor = staffBL3.BindAuthorization();
                    BindCombo("AuthorizationsCD", "AuthorizationsName", dtAuthor);
                    break;
                case CboType.店舗権限:
                    Staff_BL staffBL4 = new Staff_BL();
                    DataTable dtStoreAuthor = staffBL4.BindStoreAuthorization();
                    BindCombo("StoreAuthorizationsCD", "StoreAuthorizationsName", dtStoreAuthor);
                    break;
                case CboType.役職:
                    Staff_BL staffBL5 = new Staff_BL();
                    DataTable dtPosition = staffBL5.BindPosition();
                    BindCombo("Key", "Char1", dtPosition);
                    break;
                case CboType.データ種別:
                    MultiPorpose_BL mpBL = new MultiPorpose_BL();
                    DataTable dtKBN = mpBL.BindSeqKBN();
                    BindCombo("Column1", "Char1", dtKBN);
                    break;
                case CboType.貨幣金種名:
                    MultiPorpose_BL mppbl = new MultiPorpose_BL();
                    M_MultiPorpose_Entity mppe = new M_MultiPorpose_Entity();
                    mppe.ID = mppbl.ID_Money;
                    DataTable dtDenomination = mppbl.M_MultiPorpose_SelectAll(mppe);
                    BindCombo("Key", "Char1", dtDenomination);
                    break;
                case CboType.支払予定月:
                    MultiPorpose_BL mtpbl = new MultiPorpose_BL();
                    M_MultiPorpose_Entity mpe = new M_MultiPorpose_Entity();
                    mpe.ID = mtpbl.PaymentMonth;
                    DataTable dtSupplier = mtpbl.M_MultiPorpose_SupplierSelect(mpe);
                    BindCombo("Key", "Char1", dtSupplier);
                    break;
                case CboType.支払金種:
                    MultiPorpose_BL mulbl = new MultiPorpose_BL();
                    M_MultiPorpose_Entity mmule = new M_MultiPorpose_Entity();
                    mmule.ID = mulbl.PaymentType;
                    DataTable dtpayment = mulbl.M_MultiPorpose_SupplierSelect(mmule);
                    BindCombo("Key", "Char1", dtpayment);
                    break;
                //case CboType.店舗名:
                //    Store_BL storebl = new Store_BL();
                //    DataTable storetb = storebl.BindData(mse);
                //    BindCombo("StoreCD", "StoreName", storetb);
                //    break;
                
                case CboType.予定金種:
                    M_DenominationKBN_Entity mde = new M_DenominationKBN_Entity();
                    DenominationKBN_BL bl = new DenominationKBN_BL();
                    DataTable dtDen = bl.BindKbn(mde, 0);
                    BindCombo("DenominationCD", "DenominationName", dtDen);
                    break;
                case CboType.受注確度:
                case CboType.年度:
                case CboType.シーズン:
                case CboType.予約フラグ:
                case CboType.特記フラグ:
                case CboType.送料条件:
                case CboType.要加工品区分:
                case CboType.要確認品区分:
                case CboType.発注:
                case CboType.タグ:
                case CboType.入荷予定状況:
                case CboType.識別:
                case CboType.予約:   //20200316
                case CboType.特記:
                case CboType.送料:
                case CboType.発注フラグ:  //20200316
                case CboType.年度フラグ://20200427
                case CboType.シーズンフラグ://20200427


                    MultiPorpose_BL mbl = new MultiPorpose_BL();
                    M_MultiPorpose_Entity me = new M_MultiPorpose_Entity();
                    int kbn = 0;    //Key+Char1のコンボボックスはKbn1とする

                    switch (Cbo_Type)
                    {
                        case CboType.受注確度:
                            me.ID = MultiPorpose_BL.ID_JyuchuChance;
                            kbn = 2;
                            break;
                        //case CboType.予定金種:
                        //    me.ID = MultiPorpose_BL.ID_PaymentMethod;
                        //    break;
                        case CboType.年度:
                            me.ID = MultiPorpose_BL.ID_YearTerm;
                            break;
                        case CboType.シーズン:
                            me.ID = MultiPorpose_BL.ID_Season;
                            break;
                        case CboType.予約フラグ:
                            me.ID = MultiPorpose_BL.ID_ReserveCD;
                            kbn = 1;
                            break;
                        case CboType.特記フラグ:
                            me.ID = MultiPorpose_BL.ID_NoticesCD;
                            kbn = 1;
                            break;
                        case CboType.送料条件:
                            me.ID = MultiPorpose_BL.ID_PostageCD;
                            kbn = 1;
                            break;
                        case CboType.要加工品区分:
                            me.ID = MultiPorpose_BL.ID_ManufactCD; //	
                            kbn = 1;
                            break;
                        case CboType.要確認品区分:
                            me.ID = MultiPorpose_BL.ID_ConfirmCD;//	
                            kbn = 1;
                            break;
                        case CboType.発注:
                            me.ID = MultiPorpose_BL.ID_OrderAttentionCD;
                            kbn = 1;
                            break;
                        case CboType.タグ:
                            me.ID = MultiPorpose_BL.ID_TagName;
                            kbn = 2;
                            break;
                        case CboType.入荷予定状況:
                            me.ID = MultiPorpose_BL.ID_ArrivalPlanCD;
                            kbn = 2;
                            break;
                        case CboType.識別:
                            me.ID = MultiPorpose_BL.ID_Identification;
                            kbn = 1;
                            break;
                                 //20200316
                        case CboType.予約:
                            me.ID = MultiPorpose_BL.ID_ReserveCD;
                            kbn = 2;
                            break;
                        case CboType.特記:
                            me.ID = MultiPorpose_BL.ID_NoticesCD;
                            kbn = 2;
                            break;
                        case CboType.送料:
                            me.ID = MultiPorpose_BL.ID_PostageCD;
                            kbn = 2;
                            break;
                        case CboType.発注フラグ:
                            me.ID = MultiPorpose_BL.ID_OrderAttentionCD;
                            kbn = 2;
                            break;
                        case CboType.年度フラグ:
                            me.ID = MultiPorpose_BL.ID_YearTerm;
                            kbn = 2;
                            break;
                        case CboType.シーズンフラグ:
                            me.ID = MultiPorpose_BL.ID_Season;
                            kbn = 2;
                            break;
                        
                    }
                    if (type != null)
                    {
                        kbn = Convert.ToInt16(type);
                    }
                    if (kbn == 0)
                    {
                        DataTable dt = mbl.M_MultiPorpose_SelectForCombo(me, "Key2");
                        BindCombo("Key", "Key2", dt);
                    }
                    else if (kbn == 1)
                    {
                        DataTable dt = mbl.M_MultiPorpose_SelectForCombo(me, "KeyAndChar1");
                        BindCombo("Key", "KeyAndChar1", dt);
                    }
                    else if (kbn == 2)
                    {
                        DataTable dt = mbl.M_MultiPorpose_SelectForCombo(me, "Char1");
                        BindCombo("Key", "Char1", dt);
                    }
                    break;
                case CboType.倉庫:
                    Search_Souko_BL msbl = new Search_Souko_BL();
                    M_Souko_Entity mske = new M_Souko_Entity();
                    mske.DeleteFlg = "0";
                    //mske.SoukoType = "3"; 3,4の両方へ変更
                    mske.ChangeDate = changeDate;
                    //GetLoginInformations();
                    //mske.StoreCD = StoreCD;
                    DataTable dtSouko = msbl.M_Souko_Bind(mske);
                    BindCombo("SoukoCD", "SoukoName", dtSouko);
                    break;
                case CboType.SoukoAll:
                    Search_Souko_BL ss_bl = new Search_Souko_BL();
                    M_Souko_Entity msse = new M_Souko_Entity();
                    msse.DeleteFlg = "0";
                    msse.ChangeDate = changeDate;
                    GetLoginInformations();
                    msse.StoreCD = StoreCD;
                    DataTable dtSoukoAll = ss_bl.M_Souko_AllBind(msse);
                    BindCombo("SoukoCD", "SoukoName", dtSoukoAll);
                    break;
                case CboType.WarehouseSelectAll:
                    Search_Souko_BL ssbl = new Search_Souko_BL();
                    M_Souko_Entity msentity = new M_Souko_Entity();
                    msentity.ChangeDate = changeDate;
                    GetLoginInformations();
                    msentity.StoreCD = StoreCD;
                    DataTable dtSoukoSelectAll = ssbl.M_Souko_BindAll(msentity);
                    BindFirstData("SoukoCD", "SoukoName", dtSoukoSelectAll);
                    break;

                case CboType.入荷倉庫:
                    NyuukaNyuuryoku_BL nnbl = new NyuukaNyuuryoku_BL();
                    M_Souko_Entity msoe = new M_Souko_Entity();
                    msoe.ChangeDate = changeDate;
                    msoe.DeleteFlg = "0";
                    DataTable dtNSouko = nnbl.M_Souko_BindForNyuuka(msoe);
                    BindCombo("SoukoCD", "SoukoName", dtNSouko);
                    break;
                case CboType.返品倉庫:
                    ZaikoIdouNyuuryoku_BL zibl = new ZaikoIdouNyuuryoku_BL();
                    M_Souko_Entity msoe2 = new M_Souko_Entity();
                    msoe2.ChangeDate = changeDate;
                    msoe2.SoukoType = "8";
                    msoe2.DeleteFlg = "0";
                    DataTable dtNSoukoH = zibl.M_Souko_BindForHenpin(msoe2);
                    BindCombo("SoukoCD", "SoukoName", dtNSoukoH);
                    break;

                case CboType.出荷指示倉庫:
                    ShukkaShijiTouroku_BL sjbl = new ShukkaShijiTouroku_BL();
                    M_Souko_Entity msoe3 = new M_Souko_Entity();
                    msoe3.ChangeDate = changeDate;
                    msoe3.DeleteFlg = "0";
                    DataTable dtSSoukoH = sjbl.M_Souko_BindForShukka(msoe3);
                    BindCombo("SoukoCD", "SoukoName", dtSSoukoH);
                    break;

                case CboType.出荷倉庫:
                    ShuukaSouko_BL Shuuka = new ShuukaSouko_BL();
                    //M_Souko_Entity msoe3 = new M_Souko_Entity();
                    // msoe3.ChangeDate = changeDate;
                    // msoe3.DeleteFlg = "0";
                    DataTable dtShuukka = Shuuka.M_Souko_BindForShukka(changeDate, "0");
                    BindCombo("SoukoCD", "SoukoName", dtShuukka);
                    break;
                case CboType.棚卸倉庫:
                    Tanaoroshi_BL tabl = new Tanaoroshi_BL();
                    M_Souko_Entity msoe4 = new M_Souko_Entity();
                    msoe4.Operator = type; //mse.StoreCD;
                    msoe4.ChangeDate = changeDate;
                    msoe4.DeleteFlg = "0";
                    DataTable dtSSoukoT = tabl.M_Souko_BindForTanaoroshi(msoe4);
                    BindCombo("SoukoCD", "SoukoName", dtSSoukoT);
                    break;

                case CboType.在庫照会倉庫:
                    ZaikoShoukai_BL zaikobl = new ZaikoShoukai_BL();
                    M_Souko_Entity ms = new M_Souko_Entity();
                    ms.StoreCD = mse.StoreCD;

                    DataTable dtzaiko = zaikobl.M_Souko_BindForZaikoshoukai(ms);
                    BindCombo("SoukoCD", "SoukoName", dtzaiko);
                    break;

                case CboType.移動区分:
                    ZaikoIdouNyuuryoku_BL zibl2 = new ZaikoIdouNyuuryoku_BL();
                    M_MovePurpose_Entity mmpe = new M_MovePurpose_Entity();
                    mmpe.MoveFLG = "1";
                    DataTable dtIdo = zibl2.M_MovePurpose_Bind(mmpe);
                    BindCombo("MovePurposeKBN", "MovePurposeName", dtIdo);
                    break;
                case CboType.移動依頼区分:
                    ZaikoIdouNyuuryoku_BL zibl3 = new ZaikoIdouNyuuryoku_BL();
                    M_MovePurpose_Entity mmpe2 = new M_MovePurpose_Entity();
                    mmpe2.MoveRequestFLG = "1";
                    DataTable dtIdo2 = zibl3.M_MovePurpose_Bind(mmpe2);
                    BindCombo("MovePurposeKBN", "MovePurposeName", dtIdo2);
                    break;
                case CboType.取込種別:
                    Settlement_BL sebl = new Settlement_BL();
                    M_Settlement_Entity mese = new M_Settlement_Entity();
                    //mese.DeleteFlg = "0";
                    //mese.ChangeDate = changeDate;
                    
                    DataTable dtSet = sebl.M_Settlement_Bind(mese);
                    BindCombo("PatternCD", "PatternName", dtSet);
                    break;
                case CboType.銀行口座:
                    Kouza_BL kbl = new Kouza_BL();
                    M_Kouza_Entity mke = new M_Kouza_Entity();
                    mke.DeleteFlg = "0";
                    mke.ChangeDate = changeDate;
                    DataTable dtKouza = kbl.M_Kouza_Bind(mke);
                    BindCombo("KouzaCD", "KouzaName", dtKouza);
                    break;
                case CboType.配送会社:
                    Carrier_BL cbl = new Carrier_BL();
                    M_Carrier_Entity mce = new M_Carrier_Entity();
                    mce.DeleteFlg = "0";
                    mce.ChangeDate = changeDate;
                    DataTable dtCar = cbl.M_Carrier_Bind(mce);
                    BindCombo("CarrierCD", "CarrierName", dtCar);
                    break;

                case CboType.マークダウン倉庫:
                    MarkDownNyuuryoku_BL mdbl = new MarkDownNyuuryoku_BL();
                    DataTable dtmd = mdbl.M_Souko_BindForMarkDown(type);
                    BindCombo("SoukoCD", "SoukoName", dtmd);
                    break;

                case CboType.在庫情報:
                    mdbl = new MarkDownNyuuryoku_BL();
                    DataTable dtzi = mdbl.D_StockReplica_Bind();
                    BindCombo("ReplicaNO", "DateTime", dtzi);
                    break;

                case CboType.箱サイズ:
                    CarrierBoxSize_BL cbbl = new CarrierBoxSize_BL();
                    M_CarrierBoxSize_Entity mcbe = new M_CarrierBoxSize_Entity();
                    mcbe.CarrierCD = type;          //CarrierCd
                    mcbe.DeleteFlg = "0";
                    mcbe.ChangeDate = changeDate;
                    DataTable dtSize = cbbl.M_CarrierBoxSize_Bind(mcbe);
                    BindCombo("BoxSize", "BoxSizeName", dtSize);
                    break;

                case CboType.希望時間帯:
                    CarrierDeliveryTime_BL cbtl = new CarrierDeliveryTime_BL();
                    M_CarrierDeliveryTime_Entity mcte = new M_CarrierDeliveryTime_Entity();
                    mcte.CarrierCD = type;          //CarrierCd
                    mcte.DeleteFlg = "0";
                    mcte.ChangeDate = changeDate;
                    DataTable dtTime = cbtl.M_CarrierDeliveryTime_Bind(mcte);
                    BindCombo("DeliveryTimeCD", "DeliveryTime", dtTime);
                    break;
            }
        }
        public void GetLoginInformations()
        {
            string value = string.Empty;

            Control ctrl = this;
            do
            {
                ctrl = ctrl.Parent;
            } while (!(ctrl is Form));

            //Label lblStoreAuthoCD = ctrl.Controls.Find("lblStoreAuthoCD", true).FirstOrDefault() as Label;
            //Label lblStoreChangeDate = ctrl.Controls.Find("lblStoreAuthorizationChangeDate", true).FirstOrDefault() as Label;
            Label lblStoreCD = ctrl.Controls.Find("lblStoreCD", true).FirstOrDefault() as Label;
            //Label lblProgramID = ctrl.Controls.Find("lblProgramID", true).FirstOrDefault() as Label;

            //StoreAthuorizationCD = lblStoreAuthoCD.Text;
            //StoreAuthorizationChangeDate = lblStoreChangeDate.Text;
            if (lblStoreCD != null)
                StoreCD = lblStoreCD.Text;
            //ProgramID = lblProgramID.Text;
        }

        public void BindCombo(string key, string value, DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr[key] = "-1";
            dt.Rows.InsertAt(dr, 0);
            DataSource = dt;
            DisplayMember = value;
            ValueMember = key;
        }

        public void BindFirstData(string key, string value, DataTable dt) // For TanabanNyuuryoku(pnz)
        {
            DataSource = dt;
            DisplayMember = value;
            ValueMember = key;
        }
        // If control is disabled, we switch to DropDownList style, so we can control the appearance of the
        // edit box
        void EnableDisplayCombo_EnabledChanged(object sender, EventArgs e)
        {
            if (Enabled)
                DropDownStyle = ComboBoxStyle.DropDown;
            else
                DropDownStyle = ComboBoxStyle.DropDownList;
        }
        // Ownerdraw routine
        void EnableDisplayCombo_DrawItem(object sender, DrawItemEventArgs e)
        {
            System.Drawing.Graphics g = e.Graphics;
            Rectangle r = e.Bounds;

            if (e.Index >= 0)
            {
                string label = string.Empty; ;
                if (Items[e.Index] is DataRowView drv)
                    label = drv.Row[1].ToString();

                // This is how we draw a disabled control
                if (e.State == (DrawItemState.Disabled | DrawItemState.NoAccelerator | DrawItemState.NoFocusRect | DrawItemState.ComboBoxEdit))
                {
                    e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), r);
                    g.DrawString(label, e.Font, Brushes.Black, r);
                    e.DrawFocusRectangle();
                }
                // This is how we draw the items in an enabled control that aren't in focus
                else if (e.State == (DrawItemState.NoAccelerator | DrawItemState.NoFocusRect))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), r);
                    g.DrawString(label, e.Font, Brushes.Black, r);
                    e.DrawFocusRectangle();
                }
                // This is how we draw the focused items
                else if (e.State == (DrawItemState.NoAccelerator))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), r);
                    g.DrawString(label, e.Font, Brushes.Black, r);
                    e.DrawFocusRectangle();
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 242, 204)), r);
                    g.DrawString(label, e.Font, Brushes.Black, r);
                    e.DrawFocusRectangle();
                }


            }
            g.Dispose();


        }

        /// <summary>
        /// Calculate Textbox width by FullWidth/HalfWidth * MaxLength
        /// </summary>
        private void CalculateWidth()
        {
            int divider = CtrlByte == Bytes.半全角 ? 2 : 1;
            MaxLength = length / divider;

            int l1 = Ctrl_Byte == Bytes.半角 ? 9 : 14;
            Width = (l1 * Length) / divider;
        }

        private void SelectData()
        {

        }

        public void Require(bool value)
        {
            IsRequire = value;
        }

        private void ShowErrorMessage(string messageID)
        {
            bbl.ShowMessage(messageID);
            MoveNext = false;
            this.SelectionStart = 0;
            this.SelectionLength = this.Text.Length;
        }
        public bool IsExists(string CD, string type, string ChangeDate = null, string ProgramID = null, string StoreCD = null)
        {
            DataTable dtResult = new DataTable();
            switch (type)
            {
                case "StoreAuthorization"://noneed-->ssa
                    dtResult = bbl.SimpleSelect1("34", ChangeDate.Replace("/", "-"), CD, StoreCD); break;
                case "Souko":
                    dtResult = bbl.SimpleSelect1("40", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), CD); break;

            }

            if (dtResult.Rows.Count > 0)
                return true;
            else
                return false;
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)  // PTK added
        {
            //if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter || (e.Shift && e.KeyCode == Keys.Tab))
            //{

            //}
            //else
               
                Flag++;
            base.OnPreviewKeyDown(e);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            
            base.OnKeyUp(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            var con = this;
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter || (e.Shift && e.KeyCode == Keys.Tab))
            {
                var go = DataSource;
                var g = Text;
                Flag = 0;
            }
            else
            {
                return;
            }
            Flag = 0;
            if (e.KeyCode == Keys.Enter)
            {
                if (IsRequire && string.IsNullOrWhiteSpace(Text))
                {
                    ShowErrorMessage("E102");
                    MoveNext = false;
                    return;
                }
                else
                    MoveNext = true;
            }
            base.OnKeyDown(e);
        }
        public int Flag { get; set; } = 0;
        
    }
}
