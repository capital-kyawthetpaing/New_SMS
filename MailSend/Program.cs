using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using BL;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace MailSend
{
    class Program
    {
        static Login_BL lgbl = new Login_BL();
        static M_MultiPorpose_Entity mmpe = new M_MultiPorpose_Entity();
        static MailSend_BL msbl = new MailSend_BL();

        static DataTable dtMulti, dtMail;
        static void Main(string[] args)
        {
            Console.Title = "MailSend";

            if (lgbl.ReadConfig() == true)
            {
                mmpe.ID = "325";
                mmpe.Key = "1";

                dtMulti = msbl.M_MultiPorpose_SelectID(mmpe);
                if (dtMulti.Rows[0]["Num1"].ToString().Equals("0"))//0なら、 処理終了
                {
                    Console.WriteLine("Stop");
                }
                else
                {
                    dtMail = msbl.D_Mail_Select();

                    string FromMail="",ToMail="", CCMail="", BCCMail="", FromPwd="",AttServer="",AttFolder="",AttFileName="";
                    int k = 0;

                    if (dtMail.Rows.Count > 0)
                    {

                        MailMessage mm = new MailMessage();
                         FromMail = dtMail.Rows[0]["SenderAddress"].ToString();
                         FromPwd = dtMail.Rows[0]["Password"].ToString();
                        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                        mm.From = new MailAddress(FromMail);

                         mm.Subject= dtMail.Rows[0]["MailSubject"].ToString();
                         mm.Body = dtMail.Rows[0]["MailContent"].ToString();

                        for (int i = 0; i < dtMail.Rows.Count; i++)
                        {
                            if (dtMail.Rows[i]["AddressKBN"].ToString().Equals("1"))
                            {
                                ToMail = dtMail.Rows[i]["Address"].ToString();
                                mm.To.Add(ToMail);
                                k++;
                            }
                            if (dtMail.Rows[i]["AddressKBN"].ToString().Equals("2"))
                            {
                                CCMail = dtMail.Rows[i]["Address"].ToString();
                                mm.CC.Add(CCMail);
                                k++;
                            }
                            if (dtMail.Rows[i]["AddressKBN"].ToString().Equals("3"))
                            {
                                BCCMail = dtMail.Rows[i]["Address"].ToString();
                                mm.Bcc.Add(BCCMail);
                                k++;
                            }
                        }
                        AttServer = dtMail.Rows[0]["CreateServer"].ToString();
                        AttFolder = dtMail.Rows[0]["CreateFolder"].ToString();
                        AttFileName = dtMail.Rows[0]["FileName"].ToString();
                        
                        string filepath = AttServer + "\\" + AttFolder + "\\" + AttFileName;
                        if(File.Exists(filepath))
                        {
                            mm.Attachments.Add(new Attachment(filepath));
                        }                       
                        smtpServer.Port = 587;
                        smtpServer.Credentials = new System.Net.NetworkCredential(mm.From.Address, FromPwd);
                        smtpServer.EnableSsl = true;
                        try
                        {
                            smtpServer.Send(mm);
                            if(msbl.D_MailSend_Update(k))
                            {
                                Console.WriteLine("メールのご送信が完了致しました。");
                                //Console.Read();
                            }
                        }
                        catch (Exception ex)
                        {
                           var er= ex.Message;
                        }
                        















                       // var fromAddress = new MailAddress(FromMail,"Swe Swe Aung");
                       // //var toAddress = new MailAddress(ToMail,"Swe Swe");
                       // const string fromPassword =FromPwd;
                       // const string subject = "Mail Test";
                       // const string body = "body";

                       //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
                       // {
                       //     Host = "smtp.gmail.com",
                       //     Port = 587,                            
                       //     EnableSsl = true,
                       //     DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                       //     UseDefaultCredentials = false,
                       //     Credentials = new NetworkCredential(fromAddress.Address, FromPwd)
                       // };

                       // using (var message = new MailMessage(fromAddress, toAddress)
                       // {
                       //     Subject = subject,
                       //     Body = body,
                       //     BodyEncoding = UTF8Encoding.UTF8 ,
                            
                       // })
                        {
                            //try
                            //{
                            //    smtp.Send(message);
                            //}
                            //catch (Exception ex)
                            //{

                            //    var f = ex.Message;
                            //    //MessageBox.Show("Message not emailed: " + ex.ToString());
                            //}
                            //MailMessage msg = new MailMessage();

                            //msg.From = new MailAddress("swesweaung.ucsy2018@gmail.com");
                            //msg.To.Add("pinkyangel1996@gmail.com");
                            //msg.CC.Add("capital.swesweaung@gmail.com");
                            //msg.Subject = "即日出荷対応商品在庫切れにつきまして";
                            //msg.Body = "ohayougosaimasu";

                            //using (SmtpClient client = new SmtpClient())
                            //{
                            //    client.EnableSsl = false;
                            //    client.UseDefaultCredentials = false;
                            //    client.Credentials = new System.Net.NetworkCredential("swesweaung", "ssa16496");
                            //    client.Host = "mail.capitalsports.jp";
                            //    client.Port = 587;
                            //    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            //    client.Send(msg);
                            //}
                        }
                    }
                }

            }
        }
    }
}