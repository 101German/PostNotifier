using PostNotifier.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PostNotifier.Contracts
{
    public interface ISubscriptionRepository
    {
        Task InsertAsync(Subscription subscription);
        Task RemoveAsync(int authorId, string email);
        Task<IReadOnlyCollection<Subscription>> FindAsync(int authorId);
        Task<Subscription> GetAsync(int authorId, string email);
    }
}
