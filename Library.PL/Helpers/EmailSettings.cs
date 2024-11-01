using Library.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Library.PL.Helpers
{
    public class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("omad10200099@gmail.com", "tyef rgsb xasb vswq");
           MailMessage msg = new MailMessage();
            msg.From = new MailAddress("omad10200099@gmail.com");
            msg.Subject = email.Subject;
            msg.Body = email.Body;
            msg.To.Add(email.Receiver);
            msg.IsBodyHtml = true;
            client.Send(msg);
            //"omad10200099@gmail.com", email.Receiver, email.Subject, email.Body
        }
    }
}
