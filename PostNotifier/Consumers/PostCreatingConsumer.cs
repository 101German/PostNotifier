using MassTransit;
using PoemPost.Data.DTO;
using PostNotifier.Contracts;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PostNotifier.Consumers
{
    public class PostCreatingConsumer : IConsumer<PostDTO>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public PostCreatingConsumer(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task Consume(ConsumeContext<PostDTO> context)
        {
            var subscriptions = await _subscriptionRepository.FindAsync(context.Message.AuthorId);
            var password = Environment.GetEnvironmentVariable("EmailPassword", EnvironmentVariableTarget.User);
            var fromEmailAdress = Environment.GetEnvironmentVariable("EmailAdress", EnvironmentVariableTarget.User);

            using MailMessage mailMessage = new();
            mailMessage.From = new MailAddress(fromEmailAdress, "PoemPost");

            foreach (var emailAdress in subscriptions.Select(s => s.Email))
            {
                mailMessage.To.Add(emailAdress);
            }
            mailMessage.Subject = "New Post!";
            mailMessage.Body = $"{context.Message.AuthorName} has a new post!";

            using SmtpClient smtp = new("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(fromEmailAdress, password);
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(mailMessage);
        }
    }
}
