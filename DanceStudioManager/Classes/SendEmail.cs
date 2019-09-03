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

            // The important part -- configuring the SMTP client
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 465;   // [1] You can try with 465 also, I always used 587 and got success
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // [2] Added this
            smtp.UseDefaultCredentials = false; // [3] Changed this
            smtp.Credentials = new NetworkCredential(mailFrom, "Dance1213");  // [4] Added this. Note, first parameter is NOT string.
            smtp.Host = "smtp.gmail.com";

            //recipient address
            mail.To.Add(new MailAddress(emailTo));

            //Formatted mail body
            mail.IsBodyHtml = true;
            mail.Body = message;
            smtp.Send(mail);
        }
    }
}
