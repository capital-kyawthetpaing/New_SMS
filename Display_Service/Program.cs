using Microsoft.PointOfService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display_Service
{
    class Program
    {
        public static void Main(string[] args)
        {
            //[0]  01_0=> OpenCashDrawer
            //[0] 02_1 => SetDisplay Main
            //[0] 02_2 => SetDisplay Child 
            //[1]  => DefaultMessage
            //[2]  => UpVal
            //[3]  => DownVal
            if (args[0].ToString().Split('_').First() == "01")  // OpenCash
            {

            }
            else if (args[0].ToString().Split('_').First() == "02")  // Set Display
            {
                if (args[0].ToString().Split('_').Last() == "1") // Main
                {
                    SetDisplay(true, true, args[1].ToString());
                }
                else
                {
                    SetDisplay(false, false);
                }
            }
        }
        public static CashDrawer m_Drawer { get; set; } = null;

        //public ()
        //{


        //}

        public static void OpenCashDrawer()
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

        public static void CloseCashDrawer()
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

        public static void RemoveDisplay()
        {

            try
            {
                m_Display.MarqueeType = DisplayMarqueeType.None;  // marquee close
                m_Display.DestroyWindow();                        // instance close we have created
                m_Display.ClearText();
                m_Display.DeviceEnabled = false;
                m_Display.Release();
                m_Display.Close();
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
        }

        public static void SetDisplay(bool IsMarquee, bool IsStartUp, string DefaultMessage = null, string Upval = null, string Downval = null)
        {
            //m_Display = null;
            //deviceInfo = null;
            // m_Display.ClearText();
            //try { RemoveDisplay();

            //}
            //catch { }

            //if (m_Display != null)
            //{
            //    m_Display.ClearText();
            //    return;
            //}
            string strLogicalName = "LineDisplay";
            PosExplorer posExplorer = null;
            try
            {
                posExplorer = new PosExplorer();
            }
            catch (Exception ex)
            {
                goto End;
            }
            if (m_Display == null)
            {
                if (deviceInfo == null)
                    deviceInfo = posExplorer.GetDevice(DeviceType.LineDisplay, strLogicalName);
                m_Display = (LineDisplay)posExplorer.CreateInstance(deviceInfo);
                m_Display.Open();
                m_Display.Claim(1000);
                m_Display.DeviceEnabled = true;
            }

            try
            {
                int Wdh = 0;
                var valdown = "";
                if (IsStartUp)
                {
                    valdown =DefaultMessage;
                    Wdh = Encoding.GetEncoding(932).GetByteCount(valdown);
                    m_Display.CharacterSet = 932;
                    m_Display.CreateWindow(1, 0, 1, 20, 1, Wdh);
                    m_Display.DisplayText(valdown, DisplayTextMode.Normal);


                    if (!IsMarquee)
                    {
                        m_Display.MarqueeType = DisplayMarqueeType.None;
                    }
                    else

                    {
                        m_Display.MarqueeFormat = DisplayMarqueeFormat.Walk;
                        m_Display.MarqueeType = DisplayMarqueeType.Init;
                        m_Display.MarqueeRepeatWait = 1000;
                        m_Display.MarqueeUnitWait = 100;
                        m_Display.MarqueeType = DisplayMarqueeType.Left;
                    }
                }
                else
                {
                    Wdh = 20;
                    // m_Display.CreateWindow(1, 0, 1, 20, 1, Wdh);
                    m_Display.DisplayTextAt(1, (m_Display.Columns - Downval.Length), Downval, DisplayTextMode.Normal);
                }
            }
            catch (PosControlException pe)
            {
            }
        End:
            {
                //  m_Display
                //   deviceInfo = 
                //
            }
        }
    
        public static LineDisplay m_Display { get; set; } = null;
        public static DeviceInfo deviceInfo { get; set; } = null;

       

    //    public static CDO_Main { get; set; } = null;
    }
}
