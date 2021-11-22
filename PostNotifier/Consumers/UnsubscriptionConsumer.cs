using MassTransit;
using Models;
using PostNotifier.Contracts;
using System.Threading.Tasks;

namespace PostNotifier.Consumers
{
    public class UnsubscriptionConsumer : IConsumer<UserForUnsubscription>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public UnsubscriptionConsumer(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task Consume(ConsumeContext<UserForUnsubscription> context)
        {
            var existSubscription = await _subscriptionRepository.GetAsync(context.Message.AuthorId, context.Message.Email);

            if (existSubscription != null)
            {
              await  _subscriptionRepository.RemoveAsync(existSubscription.AuthorId,existSubscription.Email);
            }
        }
    }
}
