using System;
using BL;
using Entity;
using System.Data;

using Base.Client;

namespace GetsujiZaikoKeisanSyori
{
    class Program
    {
        static GetsujiZaikoKeisanSyori api = new GetsujiZaikoKeisanSyori();
        static Login_BL loginbl = new Login_BL();

        static GetsujiShimeShori_BL gsbl;
        static int Mode;
        static string InOperatorCD;
        static string InStoreCD;
        static string InProcessMode;
        static string InFiscalYYYYMM;

        static DataTable dtMulti;
        static void Main(string[] args)
        {
            Console.Title = "GetsujiZaikoKeisanSyori";

            if (loginbl.ReadConfig() == true)
            {
                //コマンドライン引数を配列で取得する
                string[] cmds = System.Environment.GetCommandLineArgs();

                if (cmds.Length - 1 >= (int)FrmMainForm.ECmdLine.PcID + 1)
                {
                    InOperatorCD = cmds[(int)FrmMainForm.ECmdLine.OperatorCD];
                    InStoreCD = cmds[(int)FrmMainForm.ECmdLine.PcID + 1];
                    InProcessMode = cmds[(int)FrmMainForm.ECmdLine.PcID + 2];
                    InFiscalYYYYMM = cmds[(int)FrmMainForm.ECmdLine.PcID + 3].Replace("/","");
                }

                gsbl = new GetsujiShimeShori_BL();

                //処理モードを覚える
                M_Control_Entity mce = new M_Control_Entity();
                mce.MainKey = "1";
                Mode = gsbl.GetMode(mce);

                bool execFlg = false;

                M_StoreClose_Entity me = new M_StoreClose_Entity();
                if (Mode.Equals(1))
                {
                    //Mode		＝	1	の場合			(＝ALL店舗）	
                    me.StoreCD = "";

                }
                else if(Mode.Equals(2))
                {
                    //Mode		＝	2	の場合			（＝店舗ごとに計算）		
                    me.StoreCD = InStoreCD;

                }
                me.FiscalYYYYMM = InFiscalYYYYMM;

                bool ret= gsbl.M_StoreClose_SelectAll(me);

                if (!ret)
                {
                    //Insertしたあとに再度Select
                    ret = gsbl.M_StoreClose_SelectAll(me);
                }

                if (ret)
                {
                    //FiscalYYYYMM＜Parameter受取	FiscalYYYYMM
                    //またはFiscalYYYYMM＝Parameter受取	FiscalYYYYMM＆	ClosePosition5＝0なら
                    if (gsbl.Z_Set( me.FiscalYYYYMM) <= gsbl.Z_Set(InFiscalYYYYMM) )
                    {
                        //if (gsbl.Z_Set(me.FiscalYYYYMM) == gsbl.Z_Set(InFiscalYYYYMM))
                        //{
                        //    if(me.ClosePosition5.Equals("0"))
                        //    {
                        //        execFlg = true;
                        //    }
                        //}
                        //else
                        //{
                            execFlg = true;
                        //}
                        InFiscalYYYYMM = me.FiscalYYYYMM;
                    }
                }

                if (execFlg)
                {
                    //【データ更新】
                    D_MonthlyStock_Entity de = new D_MonthlyStock_Entity
                    {
                        PC = Login_BL.GetHostName(),
                        Operator = InOperatorCD,
                        YYYYMM = InFiscalYYYYMM,
                        StoreCD = me.StoreCD,
                        Mode = Mode
                    };

                    api.ExecUpdate(de);
                }
            }
        }
    }
}
