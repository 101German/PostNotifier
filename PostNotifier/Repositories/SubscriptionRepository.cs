using MongoDB.Driver;
using PostNotifier.Contracts;
using PostNotifier.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PostNotifier.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly IMongoCollection<Subscription> _subscriptions;

        public SubscriptionRepository(ISubscriptionDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _subscriptions = database.GetCollection<Subscription>(settings.SubscriptionsCollectionName);
        }

        public async Task<IReadOnlyCollection<Subscription>> FindAsync(int authorId)
        {
            var subs = await _subscriptions
                .FindAsync(s => s.AuthorId == authorId);

            return await subs
                .ToListAsync();
        }

        public async Task<Subscription> GetAsync(int authorId, string email)
        {
            var subscription = await _subscriptions
                .FindAsync(s => s.AuthorId == authorId && s.Email == email);

            return subscription.FirstOrDefault();
        }

        public async Task InsertAsync(Subscription subscription)
        {
            await _subscriptions.InsertOneAsync(subscription);
        }

        public async Task RemoveAsync(int authorId, string email)
        {
            await _subscriptions.DeleteOneAsync(s =>
            s.AuthorId == authorId && s.Email == email);
        }
    }
}
