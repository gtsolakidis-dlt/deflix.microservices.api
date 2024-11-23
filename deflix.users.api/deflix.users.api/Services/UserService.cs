using deflix.users.api.DTOs;
using deflix.users.api.Helpers;
using deflix.users.api.Interfaces;
using deflix.users.api.Models;

namespace deflix.users.api.Services
{
    public class UserService : IUserService
    {
        private readonly IDatabaseService _databaseService;
        private readonly ISubscriptionsApi _subscriptionsApi;

        #region SqlQueries
        private static string UserExistsQuery => @"SELECT COUNT(*) FROM [Users] WHERE [Username] = @Username OR [Email] = @Email";

        private static string GetUserQuery => @"SELECT * FROM [Users] WHERE [Username] = @Username AND [Password] = @Password";

        private static string GetUserByIdQuery => @"SELECT * FROM [Users] WHERE [UserId] = @UserId";

        private static string InsertQuery => @"INSERT INTO [Users]
                                                ([UserId],[Username],[Password],[Email])
                                                VALUES
                                                (@UserId,@Username,@Password,@Email)
                                                ";

        private static string UpdateQuery => @"UPDATE [Users]
                                                SET [Password] = @Password
                                                WHERE [UserId] = @UserId
                                                ";

        #endregion

        public UserService(IDatabaseService databaseService, ISubscriptionsApi subscriptionsApi)
        {
            _databaseService = databaseService;
            _subscriptionsApi = subscriptionsApi;
        }

        public Guid Register(UserRegisterDto userDto)
        {
            var existingUserCount = _databaseService.QueryFirst<int>(UserExistsQuery, new { userDto.Username, userDto.Email });

            if (existingUserCount > 0)
            {
                return Guid.Empty;
            }

            var param = new UserModel
            {
                UserId = Guid.NewGuid(),
                Username = userDto.Username,
                Password = userDto.Password,
                Email = userDto.Email
            };

            _databaseService.Execute(InsertQuery, param);

            var res = _subscriptionsApi.SubscribeUser(param.UserId, 1).Result;

            return param.UserId;
        }

        public UserDto Authenticate(string username, string password)
        {
            var user = _databaseService.QueryFirst<UserModel>(GetUserQuery, new { Username = username, Password = password });
            if (user != null)
            {
                return new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email
                };
            }
            return null;
        }

        public UserExtDto GetUserProfile(Guid userId)
        {
            var user = _databaseService.QueryFirst<UserModel>(GetUserByIdQuery, new { userId });
            var userSubscription = _subscriptionsApi.GetUserSubscription(userId).Result;
            return new UserExtDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                SubscriptionType = userSubscription.SubscriptionCode.ToSubscriptionType(),
                ExpirationDate = userSubscription.ExpirationDate,
                //PaymentMethod = userSubscription.PaymentMethod
            };
        }

        public bool UpdateUserProfile(Guid userId, UserProfileUpdateDto profileUpdateDto)
        {
            var res = _databaseService.Execute(UpdateQuery, new { UserId = userId, profileUpdateDto.Password });
            return res > 0;
        }

    }

}
