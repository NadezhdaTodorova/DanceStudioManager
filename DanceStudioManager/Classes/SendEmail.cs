using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace DanceStudioManager
{
    public class SendEmail : Controller
    {
        public void SendGrid(string emailTo, string message)
        {
            var apiKey = System.Environment.GetEnvironmentVariable("sendGridApi");
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("dancestudiomanager2019@gmail.com", "Dance1213"),
                Subject = "Registration For DanceStudioManager!",
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(emailTo));
            var response = client.SendEmailAsync(msg);
        }

        public void SendContactsGrid(string email, string firstName, string lastName, string Subject, string message)
        {
            var apiKey = System.Environment.GetEnvironmentVariable("sendGridApi");
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("dancestudiomanager2019@gmail.com", "Dance1213"),
                Subject = Subject,
                HtmlContent = $"From { firstName } { lastName } { email } { message }"
            };
            msg.AddTo(new EmailAddress("dancestudiomanager2019@gmail.com", "Test User"));
            var response =  client.SendEmailAsync(msg);
        }
    }
}
