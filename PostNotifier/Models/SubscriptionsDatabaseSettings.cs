using PostNotifier.Contracts;

namespace PostNotifier.Models
{
    public class SubscriptionsDatabaseSettings : ISubscriptionDatabaseSettings
    {
        public string SubscriptionsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
