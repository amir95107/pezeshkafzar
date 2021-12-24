using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace MyEshop
{
    public class SendEmail
    {
        public static void Send(string To, string cc, string Subject, string Body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
            mail.From = new MailAddress("pezeshkafzarcom@gmail.com", "پشتیبانی پزشک افزار");
            mail.To.Add(To);
            if(cc!=null&&cc!=""&&cc!=" ")
            {
                mail.CC.Add(cc);
            }
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = true;

            //System.Net.Mail.Attachment attachment;
            // attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            // mail.Attachments.Add(attachment);

            //SmtpServer.Credentials = new System.Net.NetworkCredential("support@pezeshkafzar.com", "Q9g9yf@1");
            SmtpServer.Credentials = new System.Net.NetworkCredential("pezeshkafzarcom@gmail.com", "810995107");
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }
    }
}