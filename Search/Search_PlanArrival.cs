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
using BL;
using Entity;

namespace Search
{
    public partial class Search_PlanArrival : FrmSubForm
    {
        M_SKU_Entity msku_Entity;
        D_ArrivalPlan_Entity dap_Entity;
        M_SKUInfo_Entity msInfo_Entity;
        M_SKUTag_Entity msT_Entity;
        D_Stock_Entity ds_Entity;
        Base_BL bbl = new Base_BL();
        Search_PlanArrival_BL pa_bl;
        ZaikoShoukai_BL zaibl;
        string adminNO,skucd,shohinmei,color,size,item,brand,jancd,makercd;
        string[] maindata;
        int Type = 0;
        public Search_PlanArrival(String No, string sku, string shohin, string col, string si, string it, string br, string jan, string maker, string[] data)
        {
            InitializeComponent();
            HeaderTitleText = "商品在庫照会（入荷予定）";
            this.BtnF12Visible = true;
            pa_bl = new Search_PlanArrival_BL();
            zaibl = new ZaikoShoukai_BL();
            adminNO = No;
            skucd = sku;
            shohinmei = shohin;
            color = col;
            size = si;
            item = it;
            brand = br;
            jancd = jan;
            makercd = maker;
            maindata = data;
        }
        private void Search_PlanArrival_Load(object sender, EventArgs e)
        {
            CB_Soko.Bind(String.Empty, "");
            CB_Soko.SelectedValue =maindata[0] ;
            TB_Shohinmei.Text= shohinmei;
            TB_ColorName.Text = color;
            TB_SizeName.Text = size;
            TB_Skucd.Text = skucd;
            TB_Brand.Text = brand;
            TB_item.Text = item;
            TB_Jancd.Text = jancd;
            TB_makerCD.Text = makercd;
            BtnF12Text = "表示(F11)";


        }
        public override void FunctionProcess(int index)
        {
            switch (index)
            {
                case 10:
                case 11:
                    F11();
                    break;
            }
        }
        private void F11()
        {
            msku_Entity = GetDataEntity();
            dap_Entity = GetData();
            msT_Entity = GetTagEntity();
            msInfo_Entity = GetInfoEntity();
            ds_Entity = GetStockEntity();
            string ck = maindata[31];
            string rbItem = maindata[32];
            string rbmaker= maindata[33];
            if (ck == "Checked" && rbItem == "True")
            {
                Type = 1;
            }
            else if(ck == "Checked" && rbmaker == "True")
            {
                Type = 2;
            }
            else
            {
                Type = 3;
            }
            DataTable dt = pa_bl.Search_PlanArrival(dap_Entity, msku_Entity,msT_Entity,msInfo_Entity,ds_Entity,adminNO,Type);
            if (dt.Rows.Count > 0)
            {
                GV_PlanArrival.Refresh();
                GV_PlanArrival.DataSource = dt;
            }
            else
            {
                GV_PlanArrival.DataSource = null;
            }
        }
        private M_SKU_Entity GetDataEntity()
        {
            msku_Entity = new M_SKU_Entity()
            {
                ChangeDate = maindata[17],
                MainVendorCD = maindata[1],
                MakerVendorCD = maindata[4],
                BrandCD = maindata[5],
                SKUName = maindata[6],
                JanCD = maindata[7],
                SKUCD = maindata[8],
                MakerItem = maindata[10],
                ITemCD = maindata[11],
                CommentInStore = maindata[9],
                ReserveCD = maindata[15],
                NoticesCD = maindata[12],
                PostageCD = maindata[14],
                OrderAttentionCD = maindata[13],
                SportsCD = maindata[23],
                InputDateFrom = maindata[27],
                InputDateTo = maindata[28],
                UpdateDateFrom = maindata[26],
                UpdateDateTo = maindata[25],
                ApprovalDateFrom = maindata[29],
                ApprovalDateTo = maindata[30],
            };
            return msku_Entity;
        }
        private D_ArrivalPlan_Entity GetData()
        {
            dap_Entity = new D_ArrivalPlan_Entity()
            {
                SoukoCD = CB_Soko.SelectedValue.ToString(),
            };
            return dap_Entity;
        }
        public M_SKUTag_Entity GetTagEntity()
        {
            msT_Entity = new M_SKUTag_Entity()
            {
                TagName1 = maindata[18],
                TagName2 = maindata[19],
                TagName3 = maindata[20],
                TagName4 = maindata[21],
                TagName5 = maindata[22],
            };
            return msT_Entity;
        }
        public M_SKUInfo_Entity GetInfoEntity()
        {
            msInfo_Entity = new M_SKUInfo_Entity()
            {
                YearTerm = maindata[34],
                Season = maindata[16],
                CatalogNO = maindata[35],
                InstructionsNO = maindata[24],
            };
            return msInfo_Entity;
        }
        public D_Stock_Entity GetStockEntity()
        {
            ds_Entity = new D_Stock_Entity()
            {
                //SoukoCD = CB_Soko.SelectedValue.ToString(),
                RackNOFrom =maindata[2],
                RackNOTo = maindata[3],
            };
            return ds_Entity;
        }
    }
}
