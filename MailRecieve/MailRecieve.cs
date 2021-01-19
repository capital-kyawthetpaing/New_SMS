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

        public void MailRead(DataTable dtMailAddress, string filepath)
        {
            using (Pop3 pop3 = new Pop3())
            {
                for (int i = 0; i < dtMailAddress.Rows.Count; i++)
                {
                    pop3.Connect(dtMailAddress.Rows[i]["ReceiveServer"].ToString(), 110, false);   // or ConnectSSL
                    pop3.UseBestLogin(dtMailAddress.Rows[i]["Account"].ToString(), dtMailAddress.Rows[i]["Password"].ToString());

                    foreach (string uid in pop3.GetAll())
                    {
                        IMail email = new MailBuilder().CreateFromEml(pop3.GetMessageByUID(uid));
                        Console.WriteLine(email.Subject);
                        string AddressFrom = email.From[0].Address.Replace("\"", "");

                        //select VendorCD
                        DataTable dtVendorCD = msbl.ReceiveOfVendorMail(AddressFrom);

                        // save all attachments to disk
                        foreach (MimeData mime in email.Attachments)
                        {

                            if (mime.SafeFileName.Contains(".csv"))
                            {
                                if (!Directory.Exists(filepath + dtVendorCD.Rows[0]["VendorCD"].ToString()))
                                {
                                    Directory.CreateDirectory(filepath + "\\" + dtVendorCD.Rows[0]["VendorCD"].ToString());
                                }


                                string dateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                                string realFilepath = filepath + "\\" + dtVendorCD.Rows[0]["VendorCD"].ToString() + "\\";
                                mime.Save(realFilepath + dateTime + mime.SafeFileName);

                            }
                        }

                        #region test for forward
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

                        pop3.DeleteMessageByUID(uid);
                        

                    }
                  
                }

                pop3.Close();

            }

            #region Testing
            //Pop3Client pop3Client;
            //pop3Client = new Pop3Client();

            //for (int i = 0; i < dtMailAddress.Rows.Count; i++)
            //{
            //    DataTable dtMessages = new DataTable();
            //    dtMessages.Columns.Add("MessageNumber");
            //    dtMessages.Columns.Add("From");
            //    dtMessages.Columns.Add("Subject");
            //    dtMessages.Columns.Add("DateSent");
            //    dtMessages.Columns.Add("MessageBody");
            //    dtMessages.Columns.Add("User_Email");

            //    pop3Client.Connect(dtMailAddress.Rows[i]["ReceiveServer"].ToString(), 110, false);
            //    pop3Client.Authenticate(dtMailAddress.Rows[i]["Account"].ToString(), dtMailAddress.Rows[i]["Password"].ToString());
            //    int count = pop3Client.GetMessageCount();
            //    for (int j = 1; j <= count; j++)
            //    {
            //        byte[] str = pop3Client.GetMessageAsBytes(j);
            //        string strs = Encoding.GetEncoding(50222).GetString(str).TrimEnd();
            //        if (ReadError(strs))
            //        {



            //            #region Testing
            //            //OpenPop.Mime.Message msg = pop3Client.GetMessage(j);

            //            //OpenPop.Mime.MessagePart plainTextPart = msg.FindFirstPlainTextVersion();
            //            //List<MessagePart> attachments = msg.FindAllAttachments();
            //            //foreach (OpenPop.Mime.MessagePart attachment in attachments)
            //            //{
            //            //    if (attachment != null)
            //            //    {
            //            //        string ext = attachment.FileName.Split('.')[1];
            //            //        FileInfo file = new FileInfo("C:\\Attachments" + DateTime.Now.Ticks.ToString() + "." + ext);

            //            //        msg.
            //            //    }
            //            //}
            //            //Message message = pop3Client.GetMessage(j);
            //            //byte[] body = message.MessagePart.Body;
            //            //dtMessages.Rows.Add();
            //            //dtMessages.Rows[dtMessages.Rows.Count - 1]["MessageNumber"] = j;
            //            //dtMessages.Rows[dtMessages.Rows.Count - 1]["Subject"] = message.Headers.Subject;
            //            //dtMessages.Rows[dtMessages.Rows.Count - 1]["DateSent"] = message.Headers.DateSent.AddHours(9);
            //            //dtMessages.Rows[dtMessages.Rows.Count - 1]["From"] = message.Headers.From;

            //            //List<MessagePart> attachment = message.FindAllAttachments();

            //            //for (int ct=0; ct<attachment.Count(); ct++)
            //            //{
            //            //    FileInfo file = new FileInfo("C:\\"+attachment[ct].ToString());

            //            //    //attachment.SaveToFile(file);
            //            //}

            //            //foreach (OpenPop.Mime.MessagePart attachment in attachments)
            //            //{
            //            //    if (attachment != null)
            //            //    {
            //            //        string ext = attachment.FileName.Split('.')[1];
            //            //        FileInfo file = new FileInfo(Server.MapPath("Attachments\\") + DateTime.Now.Ticks.ToString() + "." + ext);

            //            //        attachment.SaveToFile(file);
            //            //    }
            //            //}

            //            //string filename= attachment[0].FileName.Trim();
            //            //message.FileName = attachment[0].FileName.Trim();
            //            //message.Attachment = attachment;
            //            //if (attachment.Count() > 0)
            //            //    if (attachment[0] != null)
            //            //    {
            //            //        byte[] content = attachment[0].Body;

            //            //        //[1] Save file to server path  
            //            //        //File.WriteAllBytes(Path.Combine(HttpRuntime.AppDomainAppPath, "Files/") + message.FileName, attachment[0].Body);  

            //            //        //[2] Download file  
            //            //        string[] stringParts = attachment[0].FileName.Split(new char[] { '.' });
            //            //        string strType = stringParts[1];
            //            //        //attachment[0].Save

            //            //        //Response.Clear();
            //            //        //Response.ClearContent();
            //            //        //Response.ClearHeaders();
            //            //        //Response.AddHeader("content-disposition", "attachment; filename=" + message.FileName);

            //            //        ////Set the content type as file extension type  
            //            //        //Response.ContentType = strType;
            //            //        ////attachment[0].ContentType.MediaType;  

            //            //        ////Write the file content  
            //            //        //Response.BinaryWrite(content);
            //            //        //Response.End();
            //            //    }

            //            #endregion
            //        }
            //    }

            //pop3Client.Connect("133.242.249.67", 110, false);
            // pop3Client.Authenticate("tennicedi@act-gr.co.jp", "@Capital13");
            //pop3Client.Authenticate("order@capitalk-mm.com", "oeui39@efad");

            //pop3Client.Connect("smtp.gmail.com", 587, false);
            //pop3Client.Authenticate("skshomepage@gmail.com", "homepage");

            #endregion
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
