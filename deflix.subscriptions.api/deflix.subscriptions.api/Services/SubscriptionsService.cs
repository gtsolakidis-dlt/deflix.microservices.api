using deflix.subscriptions.api.DTOs;
using deflix.subscriptions.api.Interfaces;
using deflix.subscriptions.api.Models;

namespace deflix.subscriptions.api.Services
{
    public class SubscriptionsService : ISubscriptionsService
    {
        private readonly IDatabaseService _databaseService;

        #region SqlQueries

        internal static string GetSubscriptionsQuery => @"SELECT * FROM Subscriptions";

        internal static string GetSubscriptionsByCodeQuery => @"SELECT * FROM Subscriptions where SubscriptionCode = @SubscriptionCode";

        internal static string InsertOrUpdateUserSubscriptionQuery =>
            @"
            MERGE INTO UserSubscriptions AS target
            USING (SELECT @UserId AS UserId, @SubscriptionCode AS SubscriptionCode, @PaymentMethod AS PaymentMethod) AS source
            ON target.UserId = source.UserId
            WHEN MATCHED THEN
                UPDATE SET SubscriptionCode = source.SubscriptionCode, ExpirationDate = @ExpirationDate, PaymentMethod = @PaymentMethod
            WHEN NOT MATCHED THEN
                INSERT (UserId, SubscriptionCode, ExpirationDate, PaymentMethod)
                VALUES (@UserId, @SubscriptionCode, @ExpirationDate, @PaymentMethod);
            ";

        internal static string GetUserSubscriptionQuery =>
            @"
            SELECT SubscriptionCode, PaymentMethod, ExpirationDate
            FROM UserSubscriptions
            WHERE UserId = @UserId;
            ";

        #endregion

        public SubscriptionsService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<SubscriptionDto> GetAllSubscriptions()
        {
            var subscriptions = new List<SubscriptionDto>();
            var subscriptionsDbList = _databaseService.Query<SubscriptionModel>(GetSubscriptionsQuery);
            subscriptionsDbList.ForEach(s=>subscriptions.Add(new SubscriptionDto
            {
                Id = s.SubscriptionCode,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price
            }));

            return subscriptions;
        }

        public UserSubscriptionDto GetUserSubscription(Guid userId)
        {
            var userSubscription = _databaseService.QueryFirst<UserSubscriptionModel>(GetUserSubscriptionQuery, new { UserId = userId });

            if (userSubscription != null)
            {
                var subscription = _databaseService.QueryFirst<SubscriptionModel>(GetSubscriptionsByCodeQuery, new { userSubscription.SubscriptionCode });

                return new UserSubscriptionDto
                {
                    SubscriptionCode = subscription.SubscriptionCode,
                    Name = subscription.Name,
                    Description = subscription.Description,
                    Price = subscription.Price,
                    ExpirationDate = userSubscription.ExpirationDate
                };
            }

            return null;
        }

        public bool SubscribeUser(Guid userId, int subscriptionCode)
        {
            var param = new UserSubscriptionModel
            {
                UserId = userId,
                SubscriptionCode = subscriptionCode,
                ExpirationDate = DateTime.Now.Date.AddMonths(1),
                PaymentMethod = "None"
            };

           var res = _databaseService.Execute(InsertOrUpdateUserSubscriptionQuery, param);
           return res > 0;
        }
    }
}
