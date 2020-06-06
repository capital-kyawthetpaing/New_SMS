using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MainMenu
{
   public class FTPData
    {
        string[] downloadFiles;
        StringBuilder result = new StringBuilder();
        WebResponse response = null;
        StreamReader reader = null;

        //credentials
        //static string ftpuri = "ftp://202.223.48.145/";
        //static string UID = "Administrator";
        //static string PWD = "c@p!+A1062O";
        //static string path = @"C:\SMS\AppData\";
        public FTPData()
        {

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
            catch
            {
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
        public void UpdateSyncData(string Path)
        {
            var GetList = FTPData.GetFileList(Path, "Administrator", "c@p!+A1062O", @"C:\SMS\AppData\");   /// Add Network Credentials
            // if (GetList.Count() > 0 && GetList != null)
            if (GetList != null)
            {
               // Cursor = Cursors.WaitCursor;


                foreach (string file in GetList)
                {

                    FTPData.Download(file, Path, "Administrator", "c@p!+A1062O", @"C:\SMS\AppData\");
                }

              //  Cursor = Cursors.Default;
            }

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


                //}
                //Delete
                //FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create("ftp://202.223.48.145/" + file);
                //requestFileDelete.Credentials = new NetworkCredential("Administrator", "c@p!+A1062O");
                //requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;

                //FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();
            }

            catch
            {
                //MessageBox.Show(ex.Message, "Download Error");
            }

        }
    }
}
