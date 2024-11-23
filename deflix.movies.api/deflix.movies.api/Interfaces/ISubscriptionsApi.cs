using deflix.movies.api.DTOs;
using Refit;

namespace deflix.movies.api.Interfaces
{
    public interface ISubscriptionsApi
    {
        [Get("/api/subscriptions/user/{userId}")]
        Task<UserSubscriptionDto> GetUserSubscription(Guid userId);

        [Get("/api/subscriptions/list")]
        Task<List<SubscriptionDto>> GetAllSubscriptions();
    }

}
