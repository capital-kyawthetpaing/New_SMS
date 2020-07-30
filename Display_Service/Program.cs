using Microsoft.PointOfService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPSON_TM30;
using System.Threading;
using BL;
using DL;
using Entity;
namespace Display_Service
{
    class Program
    {
        static CashDrawerOpen cdo;
       static string Isset = "0";
        public static void Main(string[] args)
        {
            try
            {
                cdo = new CashDrawerOpen();
                //Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
                //    Isset = args[0];
                var f = "";
                Login_BL lbl = new Login_BL(); ;
                if (lbl.ReadConfig())
                {
                    f = Base_DL.iniEntity.DefaultMessage;
                }
                else
                {
                    f = "No Message at that moment. . . . . . . . . . . . . . . . ";
                }

                // cdo.RemoveDisplay();
               long RD= 0;
                long SD = 0;
                while (true)
                {
                    
                    
                   // Login_BL lbl = new Login_BL();
                    if (lbl.Display_Service_Status() == "0")
                    {
                        if (RD == 0)
                        {
                            cdo.RemoveDisplay();
                        }
                        RD++;
                        SD = 0;
                    }
                    else
                    {
                        if (SD == 0)
                        {
                            cdo.SetDisplay(true, true, f);
                        }
                        SD++;
                        RD = 0;
                    }
                    Thread.Sleep(1000);
                }
            //
           // cdo.SetDisplay(false, false, "1233123","PPPPP","TTTTT");
                //Start();
             //   Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
            }
            catch(Exception ex)
            { Console.WriteLine(ex.StackTrace + ex.Message); }
            Console.ReadLine();
            
                //Start();
        }
        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            try
            {
                cdo.RemoveDisplay();
            }
            catch { }
        }
        private static void Start()
        {
            if (Isset == "0")
            {
                try
                {
                    cdo.RemoveDisplay();
                }
                catch { }
                try
                {
                    cdo.SetDisplay(true, true, Base_DL.iniEntity.DefaultMessage);
                }
                catch { }
            }
            else {
                try
                {
                    cdo.SetDisplay(true, true, Base_DL.iniEntity.DefaultMessage);
                }
                catch { }
                
            }
            Console.ReadLine();
            //while (true)
            //{
            //    Thread.Sleep(3*1000);
            //    Login_BL bbl = new Login_BL();
            //    Base_DL bdl = new Base_DL();
                
            //    if (bbl.Display_Service_Status() == "1")
            //    {
            //       if (Base_DL.iniEntity.IsDM_D30Used)
            //        cdo.SetDisplay(true,true, Base_DL.iniEntity.DefaultMessage);
            //    }
            //    //else {
            //    //    try
            //    //    {
            //    //        cdo.RemoveDisplay();
            //    //    }
            //    //    catch { }

            //    //}
            //}
        }
        
    }
}
