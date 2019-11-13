using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;


namespace DanceStudioManager
{
    public class SendEmail : Controller
    {
        public void Send(string emailTo, User user, string message)
        {
            MailMessage mail = new MailMessage();
            string mailFrom = "dancestudiomanager2019@gmail.com";
            mail.From = new MailAddress(mailFrom);

            // Configuring the SMTP client
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587; 
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network; 
            smtp.UseDefaultCredentials = false; 
            smtp.Credentials = new NetworkCredential(mailFrom, "Dance1213");
            smtp.Host = "smtp.gmail.com";

            //recipient address
            mail.To.Add(new MailAddress(emailTo));

            //Formatted mail body
            mail.IsBodyHtml = true;
            mail.Body = message;
            mail.Subject = "Registration For DanceStudioManager!";
            smtp.Send(mail);
        }

        public void SendContacts(string email, string firstName, string lastName, string Subject, string message)
        {
            MailMessage mail = new MailMessage();
            string mailFrom = "dancestudiomanager2019@gmail.com";
            mail.From = new MailAddress(mailFrom);

            // Configuring the SMTP client
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;   
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network; 
            smtp.UseDefaultCredentials = false; 
            smtp.Credentials = new NetworkCredential(mailFrom, "Dance1213"); 
            smtp.Host = "smtp.gmail.com";

            //recipient address
            mail.To.Add(new MailAddress("dancestudiomanager2019@gmail.com"));

            //Formatted mail body
            mail.IsBodyHtml = true;
            mail.Subject = Subject;
            mail.Body = $"From {firstName} {lastName} {email} {message}";
            smtp.Send(mail);
        }
    }
}
