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

namespace MainMenu
{
    public class FTPData
    {
        public static string ErrorStatus = "";
        string[] downloadFiles;
        StringBuilder result = new StringBuilder();
        WebResponse response = null;
        StreamReader reader = null;

        public FTPData()
        {
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
                ErrorStatus += "GetFileListLine No 58 Catch " +Environment.NewLine +ex.StackTrace.ToString();
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

        public void UpdateSyncData(string Path)
        {
            //var Id = "";
            //var pass = "";
            //var path = "";
            //Login_BL lbl = new Login_BL();
            //IniFile_DL idl = new IniFile_DL(@"‪C:\SMS\AppData\CKM.ini");
            //if (lbl.ReadConfig())
            //{
            //    byte[] buffer = new byte[2048];

            //    GetPrivateProfileSection("ServerAuthen", buffer, 2048, @"‪C:\SMS\AppData\CKM.ini");
            //    String[] tmp = Encoding.ASCII.GetString(buffer).Trim('\0').Split('\0');
            //    Id = tmp[1].Replace("\"", "").Split('=').Last();
            //    pass = tmp[2].Replace("\"", "").Split('=').Last();
            //    path = tmp[3].Replace("\"", "").Split('=').Last();
            //    Login_BL.SyncPath = path;
            //}

        

            var GetList = FTPData.GetFileList(Path, Login_BL.ID, Login_BL.Password, @"C:\SMS\AppData\");   /// Add Network Credentials
            //var lblini = " of "+ GetList.Count().ToString();
           // ErrorStatus += GetList.Count();
            //var lbb = (GetParentLbl().Controls.Find("lblProgress", true)[0] as System.Windows.Forms.Label);
            //lbb.Visible = true;
            //lbb.Text = lblini;
            //if (lblini != null)
            //return;
            if (GetList != null)
            {
             //   if ()
                foreach (string file in GetList)
                {
                    FTPData.Download(file, Path, Login_BL.ID, Login_BL.Password, @"C:\SMS\AppData\");
                }
            }
        }
        public System.Windows.Forms.Form GetParentLbl()
        {
            var formOpen = System.Windows.Forms.Application.OpenForms.Cast<System.Windows.Forms.Form>().Where(form => form.Name == "HaspoStoreMenuLogin").FirstOrDefault();
            if (formOpen != null)
            {
                return formOpen;
            }
            return null;
        }
        public static void Download(string file, string ftpuri, string UID, string PWD, string path)
        {
            
            try
            {
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

            catch
            {
                //MessageBox.Show(ex.Message, "Download Error");
            }

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
