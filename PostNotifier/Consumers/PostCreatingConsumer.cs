using MassTransit;
using MimeKit;
using PoemPost.Data.DTO;
using PostNotifier.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
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
            var FromEmailAdress = Environment.GetEnvironmentVariable("EmailAdress", EnvironmentVariableTarget.User);

            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(FromEmailAdress, "PoemPost");

                foreach (var emailAdress in subscriptions.Select(s => s.Email))
                {
                    mailMessage.To.Add(emailAdress);
                }
                mailMessage.Subject = "New Post!";
                mailMessage.Body = $"{context.Message.AuthorName} has a new post!";

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587) )
                {
                    smtp.Credentials = new NetworkCredential(FromEmailAdress, password);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mailMessage);
                }
            }



           
         
 


            //var subscriptions = await _subscriptionRepository.FindAsync(context.Message.AuthorId);

            //var emailMessage = new MimeMessage();

            //emailMessage.From.Add(new MailboxAddress("Администрация сайта", "herman.shvayukou@innowise-group.com"));
            //emailMessage.To.Add(new MailboxAddress("name","geras210e@gmail.com"));
            //emailMessage.Subject = "Hi";
            //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            //{
            //    Text = context.Message.PoemText
            //};

            //using (var client = new SmtpClient())
            //{
            //    await client.ConnectAsync("smtp.gmail.com", 587,false);
            //    await client.AuthenticateAsync("herman.shvayukou@innowise-group.com", "gERA5127716");
            //    await client.SendAsync(emailMessage);

            //    await client.DisconnectAsync(true);
            //}

        }
    }
}
