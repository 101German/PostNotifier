using MassTransit;
using Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PostNotifier.Consumers
{
    public class PostCreatingConsumer : IConsumer<UsersForNotify>
    {
        public PostCreatingConsumer()
        {
           
        }

        public async Task Consume(ConsumeContext<UsersForNotify> context)
        {
            var password = Environment.GetEnvironmentVariable("EmailPassword", EnvironmentVariableTarget.User);
            var fromEmailAdress = Environment.GetEnvironmentVariable("EmailAdress", EnvironmentVariableTarget.User);

            using MailMessage mailMessage = new();
            mailMessage.From = new MailAddress(fromEmailAdress, "PoemPost");

            foreach (var emailAdress in context.Message.UsersEmails)
            {
                mailMessage.To.Add(emailAdress);
            }
            mailMessage.Subject = "New Post!";
            mailMessage.Body = $"{context.Message.AuthorName} has a new post {context.Message.PoemName}!";

            using SmtpClient smtp = new("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(fromEmailAdress, password);
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(mailMessage);
        }
    }
}
