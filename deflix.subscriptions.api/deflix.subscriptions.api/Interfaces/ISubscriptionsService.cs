using deflix.subscriptions.api.DTOs;

namespace deflix.subscriptions.api.Interfaces
{
    public interface ISubscriptionsService
    {
        IEnumerable<SubscriptionDto> GetAllSubscriptions();
        UserSubscriptionDto GetUserSubscription(Guid userId);
        bool SubscribeUser(Guid userId, int subscriptionId);
    }

}
