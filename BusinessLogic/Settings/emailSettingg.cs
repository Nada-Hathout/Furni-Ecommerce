using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
namespace BusinessLogic.Settings
{
    public  static class  emailSettingg
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("fatma.2002nagy@gmail.com", "zwwgjgtpbhjtyrey");
            client.Send("fatma.2002nagy@gmail.com", email.To, email.Subject, email.Body);
            var message = new MailMessage("fatma.2002nagy@gmail.com", email.To)
            {
                Subject = email.Subject,
                Body = email.Body,
                IsBodyHtml = true
            };

            client.Send(message);

        }
    }
}
