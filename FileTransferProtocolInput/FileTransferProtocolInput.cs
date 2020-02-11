using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using System.Data;
using System.Net;
using System.IO;
using Entity;
using System.Security.Cryptography;

namespace FileTransferProtocolInput
{
   public class FileTransferProtocolInput
    {
        FileTransferProtocol_BL ftpbl = new FileTransferProtocol_BL();
        D_FTP_Entity dftpe;
        public void FTPImport()
        {
           
            DataTable dt = ftpbl.M_VendorFTP_ForSelectFile();
            DataRow[] drKBN2 = dt.Select("DataKBN=2");
            DataRow[] drKBN3 = dt.Select("DataKBN=3");

            if(drKBN2!=null && drKBN2.Count()>0)
              ImportFile_FromFTP(drKBN2);

            if(drKBN3!=null && drKBN3.Count() > 0)
                ImportFile_FromFTP(drKBN3);

        }

        public  void ImportFile_FromFTP(DataRow[]  drKBN2)
        {
            foreach(DataRow row in drKBN2)
            {
                string[] files = GetFileList(row);
                if (files != null)
                {
                    foreach (string file in files)
                    {
                        bool flg= DownloadFiles(row,file);
                        if (flg)
                        {
                            dftpe = new D_FTP_Entity()
                            {
                                FTPType = "2",
                                FTPFile = file,
                                VendorCD=row["VendorCD"].ToString(),
                            };
                            bool result=ftpbl.InsertFiles(dftpe);
                        }
                           
                    }
                }
            }
        }

        public  string[] GetFileList(DataRow row)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(row["FTPServer"].ToString() +row["FTPFolder"].ToString() +"/" + row["VendorCD"].ToString() +"/"));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(row["LoginID"].ToString(), row["Password"].ToString());
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

        public bool DownloadFiles(DataRow row,string file)
        {
            try
            {

                string uri = row["FTPServer"].ToString() +row["FTPFolder"].ToString() + "/" + row["VendorCD"].ToString() + "/" + file;
                Uri serverUri = new Uri(uri);
                if (serverUri.Scheme != Uri.UriSchemeFtp)
                {
                    return false;
                }
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                reqFTP.Credentials = new NetworkCredential(row["LoginID"].ToString(), row["Password"].ToString());
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Proxy = null;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream responseStream = response.GetResponseStream();

                string path = row["CreateServer"].ToString() +  row["CreateFolder"].ToString() + @"/" + row["VendorCD"].ToString();
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

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

                return true;
     
            }

            catch
            {
                return false;

            }
        }
    }
}
