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
using System.Collections;

namespace FileTransferProtcolOutput
{
    public class FileTransferProtocolOutput
    {
        FileTransferProtocol_BL ftpbl = new FileTransferProtocol_BL();
        D_FTP_Entity dftpe;
        public void FTPOutput()
        {

            DataTable dt = ftpbl.M_VendorFTP_ForSelectFile();
            DataRow[] drKBN = dt.Select("DataKBN=1");
            if (drKBN != null)
                UploadFile_ToFTP(drKBN);


        }
        public void UploadFile_ToFTP(DataRow[] drKBN)
        {
            foreach (DataRow row in drKBN)
            {
                 ArrayList FilePath = getFilePath(row["CreateServer"].ToString() + row["CreateFolder"].ToString() + @"\" + row["VendorCD"].ToString());
                if (FilePath != null)
                {
                    foreach (string file in FilePath)
                    {
                        bool flg = UploadFiles(row, file);
                        if (flg)
                        {
                            dftpe = new D_FTP_Entity()
                            {
                                FTPType = "1",
                                FTPFile = file.Split('\\').Last().ToString(),
                                VendorCD = row["VendorCD"].ToString(),
                            };
                            bool result = ftpbl.InsertFiles(dftpe);
                        }

                    }
                }
            }
        }



        public bool UploadFiles(DataRow row, string file)
        {
            try
            {
                if(DirectoryExists(row))
                {
                    string ftpPath = row["FTPServer"].ToString() + "/" + row["FTPFolder"].ToString() + "/" + row["VendorCD"].ToString();
                    string createPath = row["CreateServer"].ToString() + row["CreateFolder"].ToString() + @"/" + row["VendorCD"].ToString();

                    string[] arrName = file.Split('\\');
                    
                    WebClient wc = new WebClient();
                    wc.Credentials = new NetworkCredential(row["LoginID"].ToString(), row["Password"].ToString());

                    FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath +'/'+ arrName.Last().ToString()));
                    ftp.Credentials = new NetworkCredential(row["LoginID"].ToString(), row["Password"].ToString());
                    ftp.Method = WebRequestMethods.Ftp.UploadFile;
                    ftp.KeepAlive = true;
                    ftp.UseBinary = true;

                    FileStream fs = File.OpenRead(file);
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();

                    Stream ftpstream = ftp.GetRequestStream();
                    ftpstream.Write(buffer, 0, buffer.Length);
                    ftpstream.Close();


                    FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                    response.Close();
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
       private ArrayList getFilePath(string folderPath)
        {
            ArrayList arr = new ArrayList();

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            DirectoryInfo d = new DirectoryInfo(folderPath);
            string[] Files;

            Files = Directory.GetFiles(folderPath, "*.*", System.IO.SearchOption.AllDirectories).Where(s => s.EndsWith(".csv")).ToArray();

            foreach (string file in Files)
            {
                //このとき、フォルダー名＝仕入先CDと見なす
                arr.Add(file);
            }

            return arr;
        }

        private static bool DirectoryExists(DataRow row)
        {
            FtpWebRequest ftpRequest;
            /* Create an FTP Request */
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(row["FTPServer"].ToString() + "/" + row["FTPFolder"].ToString() + "/" + row["VendorCD"].ToString());
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(row["LoginID"].ToString(), row["Password"].ToString());
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            try
            {
                using (FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse())
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally  /* Resource Cleanup */
            {
                ftpRequest = null;
            }
        }

    }
}
