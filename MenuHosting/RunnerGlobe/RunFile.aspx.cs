using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32.TaskScheduler;
using System.Security;
using System.Text;

namespace MenuHosting.RunnerGlobe
{
    public partial class RunFile : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            SetDefaultValue();
            if (!IsPostBack)
            {
                SetLog();
            }
          
        }
        protected void UpdateClick(object sender, EventArgs e)
        {

        }
        protected void Trigger(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPass.Value))
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Script1", "<script>alert('Please fill the secret password.')</script> ");
            }
            else
            {
                if (txtPass.Value == "capital")
                {
                    SetLog(false, false, true);
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Script1", "<script>alert('Please fill the correct secret password.')</script> ");
                }
            }
        }
        protected void ShowAllLog(object sender, EventArgs e)
        {
                SetLog(true);
        }
        protected void FlgOn(bool flg, string txt, System.Drawing.Color col)
        {
            if (System.Drawing.Color.Green == col)
            {
                loader.Visible = false;
            }
            else
            {
                loader.Visible = true;
            }
            status.Visible = flg;
            status.InnerText = txt;
            status.Style.Add(HtmlTextWriterStyle.Color, col.Name);
        }
        protected void RunBatch(string pth)
        {
            //try
            //{
            //    using (TaskService tasksrvc = new TaskService(@"\\" + "163.43.194.87:5812", "Administrator", null, "4X#JMr$CApiTal&", true)) //WIN-6QGPVCKMR3O
            //    {
            //        Task task = tasksrvc.FindTask("SMS_1");
            //        task.Run();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    var mse = ex.Message.ToString().Replace("'", "").Replace("\\", "\\\\");
            //    mse = mse.Replace("path", "remote path");//.Replace("_", " ");
            //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Script1", "<script>alert(' " + mse + "')</script> ");
            //}
            //var p = new Process
            //{
            //    StartInfo =
            //                        {
            //                            UseShellExecute = false,
            //                            FileName = pth,
            //                            RedirectStandardError = true,
            //                            RedirectStandardOutput = true,
            //                            CreateNoWindow = true,
            //                            WindowStyle = ProcessWindowStyle.Hidden,
            //                         ///  Arguments = arguments
            //                        }
            //};
            //p.Start();
            //var d = pth;
            //ProcessStartInfo psi = new ProcessStartInfo();
            //psi.UseShellExecute = false;
            //psi.LoadUserProfile = false; 
            //psi.FileName = HttpContext.Current.Server.MapPath("~");
            //psi.FileName = @d;
            //psi.Arguments = "Myargument1 Myargument2";
            //try
            //{
            //    var securePassword = new SecureString();
            //    string password = "4X#JMr$CApiTal&";
            //    foreach (char c in password)
            //        securePassword.AppendChar(c);
            //    Process.Start(pth, "", "Administrator", securePassword, "CAPITAl");
            //}
            //catch { }

            Process process = new Process();
            process.StartInfo.FileName = pth;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Verb = "runas";
            
            process.Start();

        }
        protected void SetLog(bool IsLogClick = false, bool IsPageLoad = false, bool IsTrigger=false)
        {
            if (CheckPathError())
            {
                try
                {
                    if (IsTrigger)
                    {
                        RunBatch(bthPath.Value);
                        System.Threading.Thread.Sleep(6000);
                    }
                    //if (IsLogClick)
                    //{
                    //    FlgOn(false, "", System.Drawing.Color.Green);
                    //    ShowLog();
                    //}
                    //else
                    //{
                        var pth = stPath.Value;//@"D:\Runner\Flg.log";
                        string[] text = System.IO.File.ReadAllLines(pth);
                        var first = text.First();
                        if (first == "IsRunning=Yes")
                        {
                            FlgOn(true, "Compilation processing, Please wait . . .", System.Drawing.Color.Red);
                            if (!IsPageLoad)
                                compiledLog.InnerText = sqlLog.InnerText = "Waiting . . .";
                        }
                        else
                        {
                            FlgOn(true, "Compilation have successfully done!", System.Drawing.Color.Green);
                            ShowLog();
                        }
                    //}
                }
                catch (Exception ex)
                {
                    var mse = ex.Message.ToString().Replace("'","").Replace("\\","\\\\");
                    mse = mse.Replace("path", "remote path");//.Replace("_", " ");

                    //if (mse.Contains("errors.log"))
                    //{
                    //    //errors.log
                    //    mse = "Still running please wait . . .";
                    //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Script1", "<script>alert(' " + mse + "')</script> ");
                    //    //SetLog();
                    //}
                    //else
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Script1", "<script>alert(' "+mse+ "')</script> ");
                    // Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Script1", "<script>alert(' System Error have occurred!')</script> ");
                    //   Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Script1", "<script>alert('" + ex.ToString() + "')</script> ");
                }
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Script1", "<script>alert('Please fill the correct path of execution destinations.')</script> ");
                // Show Error
            }
        }
        protected void ShowLog()
        {
            // var exe = cmpPath.Value;  //@"D:\Runner\Errors.log";
            var com = cmpPath.Value;
            var prj = "";
            if (sl_Project.Value == "SMS" || sl_Project.Value == "Shinyoh")
            {
                prj = "";
            }
            else if (sl_Project.Value == "Haspo")
            {
                prj = "Haspo";
            }
            else if (sl_Project.Value == "Tennic")
            {
                prj = "Tennic";
            }
                var exe = System.IO.Path.GetDirectoryName(execuPath.Value) + @"\SP_Execution"+prj+"(" + DateTime.Now.ToString("yyyyMMdd") + ").log";// @"D:\Runner\SP_Execution(" + DateTime.Now.ToString("yyyyMMdd") + ").log";
            string comAr = "";
            var ln=System.IO.File.ReadAllLines(com);
            foreach (var l in ln)
                comAr += l + Environment.NewLine;
            if (!string.IsNullOrEmpty(comAr))
                compiledLog.InnerText = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(comAr)); 
            else
                compiledLog.InnerText = "No Error * * *";

            string exeAr = "";
             ln =System.IO.File.ReadAllLines(exe);
            foreach (var l in ln)
                exeAr += l + Environment.NewLine;
            if (!string.IsNullOrEmpty(exeAr))
                sqlLog.InnerText = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(exeAr));
            else
                sqlLog.InnerText = "No Error * * *";
        }
        protected void CheckStatus()
        {
            var pth = @"D:\Runner\Errors.txt";
           // var pth = bthPath.Value;
            string[] text = System.IO.File.ReadAllLines(pth);
            var first = text.First();
            compiledLog.InnerText = first;
            var last = text.Last();
            sqlLog.InnerText = last;
        }
        protected bool CheckPathError()
        {
            if (String.IsNullOrEmpty(bthPath.Value))
            {
                return false;
            }
            if (String.IsNullOrEmpty(cmpPath.Value))
            {
                return false;
            }
            if (String.IsNullOrEmpty(execuPath.Value))
            {
                return false;
            }
            if (String.IsNullOrEmpty(stPath.Value))
            {
                return false;
            }
            return true;
        }
        protected void SetDefaultValue()
        {
            bthPath.Value = @"C:\GetLatestVersion\SMS_GetLatest(GitHub)_JP.bat";     //C:\GetLatestVersion\SMS_GetLatest(GitHub)_JP.bat
            cmpPath.Value = @"C:\GetLatestVersion\errors.log";     //C:\GetLatestVersion\errors.log
            execuPath.Value = @"C:\GetLatestVersion\SP_Execution.log";   //C:\GetLatestVersion\SP_Execution.log
            stPath.Value = @"C:\GetLatestVersion\Flg.log";      //C:\GetLatestVersion\Flg.log
        }
       
//        using (TaskService tasksrvc = new TaskService(@"\\" + servername, username, domain, password, true))
//{       
//    Task task = tasksrvc.FindTask(taskSchedulerName);
//    task.Run();
//}   
        //Trigger
    }
}