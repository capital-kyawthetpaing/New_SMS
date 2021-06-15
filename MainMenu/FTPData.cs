using BL;
using DL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace MainMenu
{
    public class FTPData
    {
        public static string ErrorStatus = "";
        string[] downloadFiles;
        StringBuilder result = new StringBuilder();
        WebResponse response = null;
        StreamReader reader = null;
        string SenderPath = "";
        string SenderParent = "";
        public FTPData(string Sen = null, string par = null)
        {
            SenderPath = Sen;
            SenderParent = par;
            ErrorStatus = "";
        }
        public static string[] GetFileList(string ftpuri, string UID, string PWD, string path)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpuri));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(UID, PWD);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Proxy = null;
                reqFTP.KeepAlive = false;
                //reqFTP.UsePassive = false;
                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n'
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                ErrorStatus += "GetFileListLine No 58 Catch " + Environment.NewLine + ex.StackTrace.ToString();
                if (reader != null)
                {
                    reader.Close();
                }

                if (response != null)
                {
                    response.Close();
                }

                downloadFiles = null;

                return downloadFiles;


            }
        }
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpszReturnBuffer, int nSize, string lpFileName);
        public async void UpdateSync()
        {
           // Task<int> task = UpdateSyncData();
        }
        public async void UpdateSyncData()
        {
            var GetList = FTPData.GetFileList(SenderPath, Login_BL.ID, Login_BL.Password, @"C:\SMS\AppData\");   /// Add Network Credentials
            //  PTK async 2021-03-04
            bool UseAsync = true;
            var lblini = "";
            try
            {
                MaxCount = GetList.Count();
                lblini = " of " + GetList.Count().ToString() + " Completed!";//
                _Progressbar = (System.Windows.Forms.ProgressBar)GetParentLbl(SenderParent).Controls.Find("progressBar1", true)[0];
                _Progressbar.Visible = true;
                _Progressbar.Enabled = true;
                _Progressbar.Maximum = 100;
                _Progressbar.Minimum = 0;
                _Progressbar.Value = 0;
                _Progressbar.Step = 1;
                //   _Progressbar.Text = "0" + lblini;

                _Progress = (GetParentLbl(SenderParent).Controls.Find("lblProgress", true)[0] as System.Windows.Forms.Label);
                _Progress.Visible = true;
                _Progress.Text = "0" + lblini;

                if (lblini == "")
                    UseAsync = false;
            }
            catch
            {
                UseAsync = false;
            }
            //  PTK async 2021-03-04
            if (GetList != null)
            {
                int cc = 0;
               // _Progressbar.Value = 50;// Convert.ToInt32((Convert.ToInt32(cc) / MaxCount) * 100);
                _Progressbar.PerformStep();
                _Progressbar.Update();
                foreach (string file in GetList)
                {
                    //if (cc == 8)
                    //    break;
                    cc++;
                    if (UseAsync)
                    {
                        await Task.Run(() =>
                        {
                            try
                            {
                                UpdateText(_Progressbar, cc.ToString() + lblini);
                                UpdateText(_Progress, cc.ToString() + lblini);
                            }
                            catch { }
                        });
                    }
                    Download(cc.ToString() + lblini, file, SenderPath, Login_BL.ID, Login_BL.Password, @"C:\SMS\AppData\");
                }
                MessageBox.Show("ダウンロードが終わりました", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _Progressbar.Enabled = false;
                _Progressbar.Visible = false;
                _Progress.Text = "";
                // System.Windows.Forms.Cursor.Current = Cursors.Default;
            }
          
        }
        public async void _UpdateSyncData(string Path, string Parent)
        {
            var GetList = FTPData.GetFileList(Path, Login_BL.ID, Login_BL.Password, @"C:\SMS\AppData\");   /// Add Network Credentials

            //  PTK async 2021-03-04
            bool UseAsync = true;
            var lblini = "";
            try
            {
                lblini = " of " + GetList.Count().ToString();//
                _Progress = (GetParentLbl(Parent).Controls.Find("lblProgress", true)[0] as System.Windows.Forms.Label);
                _Progress.Visible = true;
                _Progress.Text = "0" + lblini;
                if (lblini == "")
                    UseAsync = false;
            }
            catch
            {
                UseAsync = false;
            }
            //  PTK async 2021-03-04
            if (GetList != null)
            {
                int cc = 0;
                foreach (string file in GetList)
                {
                    //if (cc == 10)
                    //    break;
                    cc++;
                    if (UseAsync)
                    {
                        await Task.Run(() =>
                        {
                            try
                            {
                                UpdateText(_Progress, cc.ToString() + lblini);
                            }
                            catch { }
                        });
                    }
                    Download(cc.ToString() + lblini, file, Path, Login_BL.ID, Login_BL.Password, @"C:\SMS\AppData\");
                }
                MessageBox.Show("Now AppData Files are updated!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _Progress.Text = "";
                // System.Windows.Forms.Cursor.Current = Cursors.Default;
            }
        }
        public async Task UpdateText(Control c, string t)
        {
            await Task.Run(() =>
            {
                if (c.InvokeRequired)
                {
                    UpdateControl(c, t);
                }
            }).ConfigureAwait(false);
        }
        public int MaxCount = 0;
        public void UpdateControl(Control c, string s)
        {
            if (c.Name == "progressBar1")
            {
                try
                {
                    if (ControlInvokeRequired(c, () => UpdateControl(c, s)))
                    {
                      //  MessageBox.Show(Convert.ToInt32((Convert.ToInt32(s.Split(' ').First()) *100 / MaxCount) ).ToString());
                        (c as System.Windows.Forms.ProgressBar).Value = Convert.ToInt32((Convert.ToInt32(s.Split(' ').First()) * 100 / MaxCount));
                        return;
                    }
                    (c as System.Windows.Forms.ProgressBar).Value = Convert.ToInt32((Convert.ToInt32(s.Split(' ').First()) * 100 / MaxCount));
                    c.Update();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    //return;
                }
            }
            else
                try
                {
                    if (ControlInvokeRequired(c, () => UpdateControl(c, s)))
                    {
                        c.Text = s;
                        return;
                    }
                    c.Text = s;
                    c.Update();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    //return;
                }
        }
        public bool ControlInvokeRequired(Control c, Action a)
        {
            if (c.InvokeRequired)
            {
                c.Invoke(new System.Windows.Forms.MethodInvoker(delegate { a(); }));
            }
            else
            {
                return false;
            }
            return true;
        }
        public  System.Windows.Forms.ProgressBar _Progressbar = null;
        public System.Windows.Forms.Label _Progress = null;
        public void Sync(string path)
        {

            var GetList = FTPData.GetFileList(path, Login_BL.ID, Login_BL.Password, @"C:\SMS\AppData\");   /// Add Network Credentials
            var lblini = " of " + GetList.Count().ToString();
            // ErrorStatus += GetList.Count();
            var lbb = (GetParentLbl("").Controls.Find("lblProgress", true)[0] as System.Windows.Forms.Label);
            lbb.Visible = true;
            lbb.Text = "0" + lblini;
            if (lblini == null)
                return;
            if (GetList != null)
            {
                //   if ()
                int cc = 0;
                foreach (string file in GetList)
                {
                    cc++;
                    lbb.Text = cc.ToString() + lblini;
                    // FTPData.Download(file, Path, Login_BL.ID, Login_BL.Password, @"C:\SMS\AppData\");
                }
            }
        }
        public System.Windows.Forms.Form GetParentLbl(string ParentName)
        {//"HaspoStoreMenuLogin"
            var formOpen = System.Windows.Forms.Application.OpenForms.Cast<System.Windows.Forms.Form>().Where(form => form.Name == ParentName ).FirstOrDefault();
            if (formOpen != null)
            {
                return formOpen;
            }
            return null;
        }
        public void Download(string cc,string file, string ftpuri, string UID, string PWD, string path)
        {
           // await Task.Run(() =>
           //{
               try
               {
              //  await
                   
                //_Progress.Text = cc;
                string uri = ftpuri + file;
                   Uri serverUri = new Uri(uri);
                   if (serverUri.Scheme != Uri.UriSchemeFtp)
                   {
                       return;
                   }
                   FileInfo localFileInfo = new FileInfo(path + "/" + file);
                   var local_Info = localFileInfo.LastWriteTime;
                   FtpWebRequest reqFTP1;
                   reqFTP1 = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpuri + file));
                   reqFTP1.Credentials = new NetworkCredential(UID, PWD);
                   reqFTP1.KeepAlive = false;
                   //  reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                   reqFTP1.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                   reqFTP1.UseBinary = true;
                   reqFTP1.Proxy = null;
                   FtpWebResponse response1 = (FtpWebResponse)reqFTP1.GetResponse();
                   var server_Info = response1.LastModified;

                   response1.Close();

                   if (!Directory.Exists(path))
                       Directory.CreateDirectory(path);
                   if (File.Exists(path + "/" + file))   // Check Modified Files
                   {
                       if (server_Info > local_Info) // Assumed it was extended
                       {
                           goto Down;
                       }
                       else
                           return;
                   }
               Down:
                   if (File.Exists(path + "/" + file))   // Check Modified Files
                   {
                       File.Delete(path + "/" + file);
                   }
                   FtpWebRequest reqFTP;
                   reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpuri + file));
                   reqFTP.Credentials = new NetworkCredential(UID, PWD);
                   reqFTP.KeepAlive = false;
                   reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                   reqFTP.UseBinary = true;
                   reqFTP.Proxy = null;
                   FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                   Stream responseStream = response.GetResponseStream();

                   FileStream writeStream = new FileStream(path + "/" + file, FileMode.Create);
                   int Length = 2048;
                   Byte[] buffer = new Byte[Length];
                   int bytesRead = responseStream.Read(buffer, 0, Length);
                   while (bytesRead > 0)
                   {
                       writeStream.Write(buffer, 0, bytesRead);
                       bytesRead = responseStream.Read(buffer, 0, Length);
                   }

                   writeStream.Close();
                   response.Close();
                   File.SetLastWriteTime(path + "/" + file, server_Info);
               }

               catch (Exception ex)
               {
                   //System.Windows.Forms.MessageBox.Show(ex.Message, "Download Error");
               }
           //});
        }
        public static Stream GetImageStream(string ftpFilePath)
        {
            if (!IsExistFile(ftpFilePath))
            {
                return null;
            }
            WebClient ftpClient = new WebClient();
            ftpClient.Credentials = new NetworkCredential(Login_BL.ID, Login_BL.Password);

            byte[] imageByte = ftpClient.DownloadData(ftpFilePath);
            MemoryStream mStream = new MemoryStream();
            
            mStream.Write(imageByte, 0, Convert.ToInt32(imageByte.Length));
            var bytes = ((MemoryStream)mStream).ToArray();
            System.IO.Stream inputStream = new MemoryStream(bytes);
            return inputStream;
        }
        public static Bitmap GetImage(string ftpFilePath)
        {
            if (!IsExistFile(ftpFilePath))
            {
                return null;
            }
            WebClient ftpClient = new WebClient();
            ftpClient.Credentials = new NetworkCredential(Login_BL.ID, Login_BL.Password);

            byte[] imageByte = ftpClient.DownloadData(ftpFilePath);
            return ByteToImage(imageByte);
        }

        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }

        private static bool IsExistFile(string pth)
        {
            var request = (FtpWebRequest)WebRequest.Create(pth);
            request.Credentials = new NetworkCredential(Login_BL.ID, Login_BL.Password);
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode ==
                    FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    return false;
                }
            }
            return true;
        }
        public String GetError()
        {
            return ErrorStatus;
        }
        public bool FileUpload(string Path,string ID,out string resName)
        {
            try
            {
                Login_BL lb = new Login_BL();
                var fnm = ID+"_"+DateTime.Now.ToString("yyyyMMdd_HHmmss");

                var fp = Login_BL.FtpPath.Replace("Sync", "Setting") + Base_DL.iniEntity.DatabaseName + "/" + fnm+ System.IO.Path.GetExtension(Path);
                resName = fp;
                if (IsExistFile(fp))
                {
                    DeleteFile(fp);
                }
                using (var client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(Login_BL.ID, Login_BL.Password);
                    client.UploadFile(fp, WebRequestMethods.Ftp.UploadFile, Path);
                }
            }
            catch (Exception ex){
                var mdg = ex.Message;
                resName = "";
                return false;
            }
            return true;
        }
        private void DeleteFile(string fileName)
        {
            try
            {
                var request = (FtpWebRequest)WebRequest.Create(fileName);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(Login_BL.ID, Login_BL.Password);
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    var d= response.StatusDescription;
                }
            }
            catch { }

        }
    }
}
