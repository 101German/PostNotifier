using AutoMapper;
using MassTransit;
using Models;
using PostNotifier.Contracts;
using PostNotifier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostNotifier.Consumers
{
    public class SubscriptionConsumer : IConsumer<UserForSubscription>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMapper _mapper;

        public SubscriptionConsumer(ISubscriptionRepository subscriptionRepository, IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<UserForSubscription> context)
        {
            var subscriptiopn = _mapper.Map<Subscription>(context.Message);
            var existSubscription = await _subscriptionRepository.GetAsync(subscriptiopn.AuthorId, subscriptiopn.Email);

            if (existSubscription == null)
            {
                await _subscriptionRepository.InsertAsync(subscriptiopn);
            }
        }
    }
}
