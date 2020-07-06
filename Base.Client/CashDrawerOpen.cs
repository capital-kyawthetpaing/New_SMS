using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PointOfService;

namespace Base.Client
{
    public class CashDrawerOpen
    {
        public CashDrawer m_Drawer { get; set; }= null;
        
        public CashDrawerOpen()
        {
          
          
        }

        public void OpenCashDrawer()
        {
            try
            {
                string strLogicalName = "CashDrawer";

                //PosExplorerを生成します。
                PosExplorer posExplorer = new PosExplorer();

                DeviceInfo deviceInfo = null;

                deviceInfo = posExplorer.GetDevice(DeviceType.CashDrawer, strLogicalName);
                m_Drawer = (CashDrawer)posExplorer.CreateInstance(deviceInfo);
                try
                {
                    // m_Drawer.DeviceEnabled = true;
                }
                catch { }
                m_Drawer.Open();

                m_Drawer.Claim(1000);

                //デバイスを使用可能（動作できる状態）にします。

                m_Drawer.DeviceEnabled = true;
            }
            catch (PosControlException)
            {

            }
            m_Drawer.OpenDrawer();

            // ドロワーが開いている間、待ちます。

            while (m_Drawer.DrawerOpened == false)
            {
                System.Threading.Thread.Sleep(100);
            }

            //開いてから10秒間経っても閉じられない場合はビープ音を断続的に鳴らします。

            //このメソッドを実行すると、ドロワーが閉じられるまで処理が戻ってこないので注意してください。

            m_Drawer.WaitForDrawerClose(10000, 2000, 100, 1000);

            try
            {

                CloseCashDrawer();
            }
            catch
            {
                try
                {
                    m_Drawer = null;
                }
                catch { }
            }
        }

        public void CloseCashDrawer()
        {
            //<<<ステップ1>>>--Start
            if (m_Drawer != null)
            {
                try
                {
                    //デバイスを停止します。

                  //  m_Drawer.DeviceEnabled = false;

                    //デバイスの使用権を解除します。

                    m_Drawer.Release();

                }
                catch (PosControlException)
                {
                    
                }
                finally
                {
                    //デバイスの使用を終了します。

                    m_Drawer.Close();
                }
            }
            //<<<ステップ1>>>--End
        }
    }
}
