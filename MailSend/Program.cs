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
                mmpe.ID = "328";
                mmpe.Key = "1";

                dtMulti = msbl.M_MultiPorpose_SelectID(mmpe);
                //if (dtMulti.Rows[0]["Num1"].ToString().Equals("0"))//0なら、 処理終了
                //{
                //    Console.WriteLine("Stop");
                //}
                //else
                //{
                    dtMail = msbl.D_Mail_Select();

                   
                    int k = 0;
                    string mailcounter = string.Empty;

                    if (dtMail.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtMail.Rows.Count; i++)
                        {
                        string SenderServer = "", FromMail = "", ToMail = "", CCMail = "", BCCMail = "", FromPwd = "", AttServer = "", AttFolder = "", AttFileName = "";

                        if (mailcounter != dtMail.Rows[i]["MailCounter"].ToString())
                            {
                                mailcounter = dtMail.Rows[i]["MailCounter"].ToString();
                                DataTable dtTemp = new DataTable();
                                dtTemp = dtMail.Select("MailCounter='" + mailcounter + "'").CopyToDataTable();
                                MailMessage mm = new MailMessage();
                                FromMail = dtTemp.Rows[0]["SenderAddress"].ToString();
                                FromPwd = dtTemp.Rows[0]["Password"].ToString();

                                SenderServer = dtTemp.Rows[0]["SenderServer"].ToString();
                                SmtpClient smtpServer = new SmtpClient(SenderServer);
                                mm.From = new MailAddress(FromMail);

                                mm.Subject = dtTemp.Rows[0]["MailSubject"].ToString();
                                mm.Body = dtTemp.Rows[0]["MailContent"].ToString();
                                for(int ct=0; ct<dtTemp.Rows.Count; ct++)
                                {
                                    if (dtTemp.Rows[ct]["AddressKBN"].ToString().Equals("1"))
                                    {
                                        ToMail += dtTemp.Rows[ct]["Address"].ToString()+",";
                                        
                                      
                                    }
                                    if (dtTemp.Rows[ct]["AddressKBN"].ToString().Equals("2"))
                                    {
                                        CCMail += dtTemp.Rows[ct]["Address"].ToString() + ",";
                                        //mm.CC.Add(CCMail);
                                       
                                    }
                                    if (dtTemp.Rows[ct]["AddressKBN"].ToString().Equals("3"))
                                    {
                                        BCCMail += dtTemp.Rows[ct]["Address"].ToString() + ",";
                                        //mm.Bcc.Add(BCCMail);
                                       
                                    }
                                }
                                if(!string.IsNullOrWhiteSpace(ToMail))
                                    mm.To.Add(ToMail.TrimEnd(','));
                                if (!string.IsNullOrWhiteSpace(CCMail))
                                    mm.CC.Add(CCMail.TrimEnd(','));
                                if (!string.IsNullOrWhiteSpace(BCCMail))
                                    mm.Bcc.Add(BCCMail.TrimEnd(','));


                                AttServer = dtTemp.Rows[0]["CreateServer"].ToString();
                                AttFolder = dtTemp.Rows[0]["CreateFolder"].ToString();
                                AttFileName = dtTemp.Rows[0]["FileName"].ToString();

                                string filepath = AttServer + "\\" + AttFolder + "\\" + AttFileName;
                                if (File.Exists(filepath))
                                {
                                    mm.Attachments.Add(new Attachment(filepath));
                                }
                                smtpServer.Port = 587;
                                smtpServer.Credentials = new System.Net.NetworkCredential(mm.From.Address, FromPwd);
                                smtpServer.EnableSsl = false;
                                try
                                {
                                    smtpServer.Send(mm);
                                    if (msbl.D_MailSend_Update(Convert.ToInt32(mailcounter)))
                                    {
                                        Console.WriteLine("メールのご送信が完了致しました。");
                                      
                                    }
                                }
                                catch (Exception ex)
                                {
                                    var er = ex.Message;
                                }
                            }

                        }
                        

                    }
                //}
            }

        }
    }
}