using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using OpenPop.Pop3;
using S22.Imap;

namespace MailRecieve
{
    public class MailRecieve
    {
        private static string hostName = "mail.capitalk-mm.com";
        private static int port = 110;
        //public void MailRead(DataTable dtMailAddress)
        public void MailRead()
        {


            Pop3Client pop3Client;
            pop3Client = new Pop3Client();
            //pop3Client.Connect("pop.gmail.com", 995, false);
            //pop3Client.Authenticate("skshomepage@gmail.com", "homepage");

            //pop3Client.Connect("smtp.gmail.com", 587, false);
            //pop3Client.Authenticate("skshomepage@gmail.com", "homepage");


            //ImapClient ic = new ImapClient("imap.gmail.com", "skshomepage@gmail.com", "homepage", AuthMethods.Login, 993, true);
            //// Select a mailbox. Case-insensitive
            //ic.SelectMailbox("Inbox");
            //string countmessages = ic.GetMessageCount().ToString();
            //// Get the first *11* messages. 0 is the first message;
            //// and it also includes the 10th message, which is really the eleventh ;)
            //// MailMessage represents, well, a message in your mailbox
            //MailMessage[] mm = ic.GetMessages(0, 10);


            int count = pop3Client.GetMessageCount();
        }

    }
}
