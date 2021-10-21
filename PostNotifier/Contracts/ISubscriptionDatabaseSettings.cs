namespace PostNotifier.Contracts
{
    public interface ISubscriptionDatabaseSettings
    {
        string SubscriptionsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
