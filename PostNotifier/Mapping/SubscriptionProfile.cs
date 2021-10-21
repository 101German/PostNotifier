using AutoMapper;
using Models;
using PostNotifier.Models;

namespace PostNotifier.Mapping
{
    public class SubscriptionProfile : Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<UserForSubscription, Subscription>();
        }
    }
}
