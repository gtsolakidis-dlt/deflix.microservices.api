using deflix.users.api.DTOs;

namespace deflix.users.api.Interfaces
{
    public interface IUserService
    {
        Guid Register(UserRegisterDto userDto);
        UserDto Authenticate(string username, string password);
        UserExtDto GetUserProfile(Guid userId);
        bool UpdateUserProfile(Guid userId, UserProfileUpdateDto profileUpdateDto);
    }

}
