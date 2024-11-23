using deflix.users.api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace deflix.users.api.Interfaces
{
    public interface ISubscriptionsApi
    {
        [Get("/api/subscriptions/user/{userId}")]
        Task<UserSubscriptionDto> GetUserSubscription(Guid userId);

        [Get("/api/subscriptions/list")]
        Task<List<SubscriptionDto>> GetAllSubscriptions();

        [HttpPut("/api/subscriptions/user/{userId}/subscribe/{subscriptionCode}")]
        Task<object> SubscribeUser(Guid userId, int subscriptionCode);
    }

}
