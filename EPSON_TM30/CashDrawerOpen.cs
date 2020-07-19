﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PointOfService;

namespace EPSON_TM30
{
    public class CashDrawerOpen
    {
         public CashDrawer m_Drawer { get; set; } = null;
       /* PosExplorer posExplorer*/
        public CashDrawerOpen()
        {
            //string strLogicalName = "LineDisplay";
            //PosExplorer posExplorer = null;
            //try
            //{
            //    posExplorer = new PosExplorer();
            //}
            //catch (Exception ex)
            //{
            //}
            //deviceInfo = posExplorer.GetDevice(DeviceType.LineDisplay, strLogicalName);
            //m_Display = (LineDisplay)posExplorer.CreateInstance(deviceInfo);
            //m_Display.Open();
            //m_Display.Claim(1000);
            //m_Display.DeviceEnabled = true;
        }

        public void OpenCashDrawer(bool IsWaited = false)
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
            if (IsWaited)
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

        public void RemoveDisplay(bool IsForced=false)
        {
            if (IsForced)
            {
                try
                {
                    ForcedToBeBlank();
                }
                catch { }
            }

            try
            {
               // m_Display.MarqueeType = DisplayMarqueeType.None;  // marquee close
                m_Display.DestroyWindow();                        // instance close we have created
                m_Display.ClearText();
                m_Display.DeviceEnabled = false;
                m_Display.Release();
                m_Display.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    //m_Display.DestroyWindow();                        // instance close we have created
                    //m_Display.ClearText();
                    m_Display.DeviceEnabled = false;
                    m_Display.Release();
                    m_Display.Close();
                }
                catch{

                    try
                    {
                       // m_Display.DeviceEnabled = false;
                        m_Display.Release();
                        m_Display.Close();
                    }
                    catch {
                        try
                        {
                           // m_Display.Release();
                            m_Display.Close();
                        }
                        catch { }
                    }
                }
                // MessageBox.Show(ex.Message);
            }
        }

        public void SetDisplay(bool IsMarquee, bool IsStartUp,string DefaultMessage = null ,string Upval = null, string Downval = null)
        {
           
            try
            {
                int Wdh = 0;
                var valdown = "";
                if (IsStartUp)
                {
                    try { RemoveDisplay(); }
                    catch
                    { }
                    try
                    {
                        string strLogicalName = "LineDisplay";
                        PosExplorer posExplorer = null;
                        try
                        {
                            posExplorer = new PosExplorer();
                        }
                        catch (Exception ex)
                        {
                        }
                        deviceInfo = posExplorer.GetDevice(DeviceType.LineDisplay, strLogicalName);
                        m_Display = (LineDisplay)posExplorer.CreateInstance(deviceInfo);
                        m_Display.Open();
                        m_Display.Claim(1000);
                        m_Display.DeviceEnabled = true;
                    }
                    catch { }
                    valdown = DefaultMessage;
                    Wdh = Encoding.GetEncoding(932).GetByteCount(valdown);
                    m_Display.CharacterSet = 932;
                    m_Display.CreateWindow(1, 0, 1, 20, 1, Wdh);
                    if (!IsMarquee)
                    {
                        try
                        {
                            m_Display.MarqueeType = DisplayMarqueeType.None;
                        }
                        catch { }
                    }
                    else
                    {
                       
                        m_Display.MarqueeFormat = DisplayMarqueeFormat.Walk;
                        m_Display.MarqueeType = DisplayMarqueeType.Init;
                        m_Display.MarqueeRepeatWait = 1000;
                        m_Display.MarqueeUnitWait = 100;
                        m_Display.DisplayText(valdown, DisplayTextMode.Normal);
                        m_Display.MarqueeType = DisplayMarqueeType.Left;
                    }
                   
                }
                else
                {
                  
                    Wdh = 20;
                    try
                    {
                        try
                        {
                            RemoveDisplay();
                        }
                        catch
                        { }
                        try
                        {
                            string strLogicalName = "LineDisplay";
                            PosExplorer posExplorer = null;
                            try
                            {
                                posExplorer = new PosExplorer();
                            }
                            catch (Exception ex)
                            {
                            }
                            deviceInfo = posExplorer.GetDevice(DeviceType.LineDisplay, strLogicalName);
                            m_Display = (LineDisplay)posExplorer.CreateInstance(deviceInfo);
                            m_Display.Open();
                            m_Display.Claim(1000);
                            m_Display.DeviceEnabled = true;
                        }
                        catch { }

                        //m_Display.CreateWindow(1,0,1,20,1,20);
                        //RemoveDisplay();
                        m_Display.CharacterSet = 932;
                        var i= Encoding.GetEncoding(932).GetByteCount(Downval);
                        var j = Encoding.GetEncoding(932).GetByteCount(Upval);
                        m_Display.DisplayTextAt(0, (m_Display.Columns - j), Upval, DisplayTextMode.Normal);
                        m_Display.DisplayTextAt(1, (m_Display.Columns - i), Downval, DisplayTextMode.Normal);
                        //PutSecond(i,Downval);
                    }
                    catch
                    {
                       // m_Display.DisplayText(Upval, DisplayTextMode.Normal);
                    }
                }
            }
            catch (PosControlException pe)
            {
            }
        //End:
            //{
            //    //  m_Display
            //    //   deviceInfo = 
            //    //
            //}
        }
        private void PutSecond(int i, string Downval)
        {
            try
            {
                m_Display.DestroyWindow();
                m_Display.CreateWindow(1, 0, 1, 20, 1, 20);
                m_Display.DisplayTextAt(1, (m_Display.Columns - i), Downval, DisplayTextMode.Normal);
                
            }
            catch { }
        }
        private static void ForcedToBeBlank()
        {
            //try
            //{
            //    string strLogicalName = "LineDisplay";
            //    PosExplorer posExplorer = null;
            //    try
            //    {
            //        posExplorer = new PosExplorer();
            //        m_Display.ClearText();
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //    deviceInfo = posExplorer.GetDevice(DeviceType.LineDisplay, strLogicalName);
            //    m_Display = (LineDisplay)posExplorer.CreateInstance(deviceInfo);
            //    m_Display.Open();
            //    m_Display.Claim(1000);
            //    m_Display.DeviceEnabled = true;
            //}
            //catch { }

          //  m_Display.CharacterSet = 932;
           // m_Display.CreateWindow(1, 0, 1, 20, 1, 20);
            try
            {
                //   m_Display.CreateWindow(1, 0, 1, 20, 1, 20);
                //    m_Display.DestroyWindow();
                /// m_Display.DisplayText("", DisplayTextMode.Normal);
                /// m_Display.CreateWindow(1, 0, 1, 20, 1, 20);
                //  m_Display.MarqueeType = DisplayMarqueeType.None;
                m_Display.CreateWindow(1, 0, 1, 20, 1, 20);
                m_Display.DestroyWindow();
                m_Display.ClearText();
            }
            catch { }

           // m_Display.ClearText();
           // m_Display.DestroyWindow();

        }
        protected string GetMessages()
        {
            var get = getd();
            var str = "";
            str += get.Rows[0]["Char1"] == null ? "" : get.Rows[0]["Char1"].ToString().Trim() + "     ";
            str += get.Rows[0]["Char2"] == null ? "" : get.Rows[0]["Char2"].ToString().Trim() + "     ";
            str += get.Rows[0]["Char3"] == null ? "" : get.Rows[0]["Char3"].ToString().Trim() + "     ";
            str += get.Rows[0]["Char4"] == null ? "" : get.Rows[0]["Char4"].ToString().Trim() + "     ";
            str += get.Rows[0]["Char5"] == null ? "" : get.Rows[0]["Char5"].ToString().Trim();
            int txt = Encoding.GetEncoding(932).GetByteCount(str);
            if (txt > 200)
            {
                str = str.Substring(0, 200);
            }
            return str;
        }
        protected DataTable getd()
        {
        //    Base_DL bdl = new Base_DL();
            var dt = new DataTable();
          //  var con = bdl.GetConnection();
             SqlConnection conn = new SqlConnection("Data Source=202.223.48.145;Initial Catalog=CapitalSMS;Persist Security Info=True;User ID=sa;Password=admin123456!");
            //SqlConnection conn = con;
            conn.Open();
            SqlCommand command = new SqlCommand("Select Char1, Char2, Char3, Char4,Char5 from [M_Multiporpose] where [Key]='1' and Id='326'", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
            conn.Close();
            return dt;
        }
        public static LineDisplay m_Display  = null;
        public static DeviceInfo deviceInfo  = null;

        public object CDO_Main  = null;
    }
}
