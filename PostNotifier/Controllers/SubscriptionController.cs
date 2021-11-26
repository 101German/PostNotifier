using Microsoft.AspNetCore.Mvc;
using PostNotifier.Contracts;
using PostNotifier.Models;
using System.Threading.Tasks;

namespace PostNotifier.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionController(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Subscription subscription)
        {
            await _subscriptionRepository.InsertAsync(subscription);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(int authorId,string email)
        {
            await _subscriptionRepository.RemoveAsync(authorId, email);

            return NoContent();
        }
    }
}
