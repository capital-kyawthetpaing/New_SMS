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
        
        Base_BL bbl = new Base_BL();
        Search_PlanArrival_BL pa_bl;
        ZaikoShoukai_BL zaibl;
        string adminNO,skucd,shohinmei,color,size,item,brand,jancd,makercd,soukocd,changedate,soukoname,storecd;

        private void PanelHeader_Paint(object sender, PaintEventArgs e)
        {

        }
        
        public Search_PlanArrival(String No, string sku, string shohin, string col, string si
            , string jan, string br, string it, string maker, string date,string soucd,string souname,string store)
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
            changedate = date;
            soukocd = soucd;
            soukoname = souname;
            storecd = store;
        }
        private void Search_PlanArrival_Load(object sender, EventArgs e)
        {
            string ymd = bbl.GetDate();
            CB_Soko.Bind(ymd, storecd);
            CB_Soko.SelectedValue = soukocd;
            TB_Shohinmei.Text= shohinmei;
            TB_ColorName.Text = color;
            TB_SizeName.Text = size;
            TB_Skucd.Text = skucd;
            TB_Brand.Text = brand;
            TB_item.Text = item;
            TB_Jancd.Text = jancd;
            TB_makerCD.Text = makercd;
            BtnF12Text = "表示(F11)";
            F11Visible = false;
            F9Visible = false;
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
            DataTable dt = pa_bl.Search_PlanArrival(dap_Entity, msku_Entity, adminNO);
            if (dt.Rows.Count > 0)
            {
                GV_PlanArrival.Refresh();
                GV_PlanArrival.DataSource = dt;
                GV_PlanArrival.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                GV_PlanArrival.CurrentRow.Selected = true;
                GV_PlanArrival.Enabled = true;
                GV_PlanArrival.Focus();
            }
            else
            {
                GV_PlanArrival.DataSource = null;
                bbl.ShowMessage("E128");
            }
        }
        private M_SKU_Entity GetDataEntity()
        {
            msku_Entity = new M_SKU_Entity()
            {
                ChangeDate = changedate,
               
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
    }
}
