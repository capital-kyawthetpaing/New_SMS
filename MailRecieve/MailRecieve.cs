using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using OpenPop.Pop3;
using OpenPop.Mime;
using S22.Imap;
using System.IO;
using System.Net.Mail;
using Limilabs.Mail;
using Limilabs;
using Limilabs.Mail.MIME;
using Limilabs.Client.POP3;
using BL;
using Limilabs.Client.SMTP;
using Limilabs.Mail.Headers;

namespace MailRecieve
{
    public class MailRecieve
    {

        static MailSend_BL msbl = new MailSend_BL();
        static Base_BL bl = new Base_BL();
        string[] filename;
        string realFilepath;
        string tempFilepath;
        string tempfilename;
        string dateTime;
        

        public void MailRead(DataTable dtMailAddress, string filepath)
        {

            
            for (int i = 0; i < dtMailAddress.Rows.Count; i++)
             {
                int readCount = 0; int startIndex = 1;
                ///Read mail
                using (Pop3 pop3 = new Pop3())
                  {
                    pop3.Connect(dtMailAddress.Rows[i]["ReceiveServer"].ToString(), 110, false);   // or ConnectSSL
                    pop3.UseBestLogin(dtMailAddress.Rows[i]["Account"].ToString(), dtMailAddress.Rows[i]["Password"].ToString());

                    foreach (string uid in pop3.GetAll())
                    {

                        IMail email = new MailBuilder().CreateFromEml(pop3.GetMessageByUID(uid));
                        Console.WriteLine(email.Subject);
                        string AddressFrom = email.From[0].Address.Replace("\"", "");

                        ///select VendorCD
                        DataTable dtVendorCD = msbl.ReceiveOfVendorMail(AddressFrom);
                        if (dtVendorCD.Rows.Count > 0)
                        {
                            /// save all attachments to disk
                            int ct = 0;
                            foreach (MimeData mime in email.Attachments)
                            {
                                if (mime.SafeFileName.Contains(".csv"))
                                {

                                    filename = new string[email.Attachments.Count];
                                    if (!Directory.Exists(filepath + "\\" + dtVendorCD.Rows[0]["VendorCD"].ToString()))
                                    {
                                        //Directory.CreateDirectory(filepath + "\\" + dtVendorCD.Rows[0]["VendorCD"].ToString());
                                        Directory.CreateDirectory(filepath + "\\" + dtVendorCD.Rows[0]["VendorCD"].ToString() + "\\" + "tempfolder");
                                    }

                                    dateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                                    tempfilename = mime.SafeFileName;
                                    tempFilepath = filepath + "\\" + dtVendorCD.Rows[0]["VendorCD"].ToString() + "\\" + "tempfolder" + "\\";
                                    realFilepath = filepath + "\\" + dtVendorCD.Rows[0]["VendorCD"].ToString() + "\\";

                                    mime.Save(tempFilepath + tempfilename);
                                    filename[ct] = tempfilename;
                                    ct++;

                                    readCount++;

                                }
                            }


                            #region Send Mail

                            MailMessage mm = new MailMessage();
                            SmtpClient smtpServer = new SmtpClient(dtMailAddress.Rows[i]["ReceiveServer"].ToString());
                            mm.From = new System.Net.Mail.MailAddress(dtMailAddress.Rows[i]["Account"].ToString());
                            mm.Subject = "Forward Mail";
                            mm.Body = email.Text;
                            mm.To.Add(dtMailAddress.Rows[i]["BackUpAddress"].ToString());

                            if (filename != null && filename.Length > 0)
                                for (int fnc = 0; fnc < filename.Length; fnc++)
                                {
                                    if (File.Exists(tempFilepath + "\\" + filename[fnc].ToString()))
                                    {
                                        mm.Attachments.Add(new Attachment(tempFilepath + "\\" + filename[fnc].ToString()));
                                    }
                                }

                            smtpServer.Port = 587;
                            smtpServer.Credentials = new System.Net.NetworkCredential(mm.From.Address, dtMailAddress.Rows[i]["Password"].ToString());
                            smtpServer.EnableSsl = false;
                            try
                            {
                                smtpServer.Send(mm);
                                Console.WriteLine("メールのご送信が完了致しました。");

                            }
                            catch (Exception ex)
                            {
                                var er = ex.Message;
                            }
                            mm.Dispose();

                            #endregion

                            ///Move file
                            if (filename != null && filename.Length > 0)
                            {
                                for (int fnc = 0; fnc < filename.Length; fnc++)
                                {
                                    File.Move(tempFilepath + filename[fnc], realFilepath + dateTime + filename[fnc]);
                                }
                                if (Directory.Exists(filepath + "\\" + dtVendorCD.Rows[0]["VendorCD"].ToString() + "\\" + "tempfolder"))
                                    Directory.Delete(filepath + "\\" + dtVendorCD.Rows[0]["VendorCD"].ToString() + "\\" + "tempfolder");

                            }
                                


                            #region no use test for forward
                            //byte[] eml = pop3.GetMessageByUID(uid);

                            //MailBuilder builder = new MailBuilder();
                            //builder.From.Add(new MailBox(AddressFrom));
                            //builder.To.Add(new MailBox(dtMailAddress.Rows[0]["BackUpAddress"].ToString()));
                            //builder.Subject = "Forwarded email is attached";

                            //// attach themle message
                            //MimeRfc822 rfc822 = new MimeFactory().CreateMimeRfc822();
                            //rfc822.Data = eml;

                            //builder.AddAttachment(rfc822);

                            //IMail forward = builder.Create();

                            //using (Smtp smtp = new Smtp())
                            //{
                            //    smtp.Connect(dtMailAddress.Rows[i]["ReceiveServer"].ToString()); // or ConnectSSL if you want to use SSL
                            //    smtp.UseBestLogin(dtMailAddress.Rows[i]["Account"].ToString(), dtMailAddress.Rows[i]["Password"].ToString());
                            //    smtp.SendMessage(forward);
                            //    Console.WriteLine("Forward mail");
                            //    smtp.Close();
                            //}

                            #endregion


                            ///Delete Mail
                            ///
                            //DateTime datesent = email.Date.Value.ToString("");

                            //pop3.DeleteMessageByUID(uid);

                            //AccountStats stat = pop3.GetAccountStat();

                            //for(int mailId = 1 ; mailId <= stat.MessageCount; mailId++)
                            // {
                            //  IMail email = new MailBuilder().CreateFromEml(pop3.GetMessageByUID(uid));
                            //  Console.WriteLine(email.Subject);
                            //  string AddressFrom = email.From[0].Address.Replace("\"", "");

                            //  //string messageContent = pop3.GetMessageByNumber(mailId);
                            //  //do Something with the Mail
                            //  pop3.DeleteMessageByNumber(mailId);
                            //  }

                            //pop3.DeleteMessageByUID(uid);
                            //pop3.DeleteMessageByNumber(1);

                            //pop3.Dispose();
                        }
                    }

                    pop3.Close();

                  }


                ///Delete Mail
                using (Pop3Client pop3Client = new Pop3Client())
                {
                    pop3Client.Connect(dtMailAddress.Rows[i]["ReceiveServer"].ToString(), 110, false);
                    pop3Client.Authenticate(dtMailAddress.Rows[i]["Account"].ToString(), dtMailAddress.Rows[i]["Password"].ToString());

                    int count = pop3Client.GetMessageCount();
                    Message message = pop3Client.GetMessage(count);

                    if(readCount>0)
                    {
                        while (startIndex <= readCount)//delete count
                        {
                            try
                            {
                                message = pop3Client.GetMessage(startIndex);
                                DateTime date1 = message.Headers.DateSent;
                                //mark as delete
                                pop3Client.DeleteMessage(startIndex);
                             
                                startIndex++;
                            }
                            catch (Exception errMessage)
                            {
                                throw errMessage;
                            }
                        }
                        //commit delete
                        pop3Client.Disconnect();
                    }
                    
                }
                   
               

            }

            
        }




        public void POP()
        {
            using (Pop3 pop3 = new Pop3())
            {
                pop3.Connect("133.242.249.67", 110, false);   // or ConnectSSL
                pop3.UseBestLogin("tennicedi@act-gr.co.jp", "@Capital13");

                foreach (string uid in pop3.GetAll() )
                {
                    IMail email = new MailBuilder().CreateFromEml(pop3.GetMessageByUID(uid));
                    Console.WriteLine(email.Subject);

                    // save all attachments to disk
                    foreach (MimeData mime in email.Attachments)
                    {                       
                        if(mime.SafeFileName.Contains(".csv"))
                            mime.Save("C:\\Users\\Ei Thinzar Zaw\\Downloads\\dll\\" + mime.SafeFileName);
                    }
                }
                pop3.Close();
            }
        }

        private static bool ReadError(string emailBody)
        {
            string line;
            Boolean start = false;
            String[] str = new String[30];
            int error = 0;
            using (StringReader fileReader = new StringReader(emailBody))
            {
                while ((line = fileReader.ReadLine()) != null)
                {
                    if (line.Contains("Subject"))
                    {
                        str = line.Split('：');
                        string sub = str[0];
                        if (sub.Contains("Want to be my new f#ckbuddy"))
                        {
                            error = 1;
                        }
                    }
                    if (line.Contains("charset"))
                    {
                        str = line.Split('：');
                        string sub = str[0];
                        if (sub.Contains("iso-8859-14"))
                        {
                            error = 1;
                        }
                    }
                }
            }
            if (error != 0)
                return false;
            else
                return true;
        }

    }
}
