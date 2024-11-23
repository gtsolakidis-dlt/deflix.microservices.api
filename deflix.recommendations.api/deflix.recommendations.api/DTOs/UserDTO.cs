namespace deflix.recommendations.api.DTOs
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class UserExtDto : UserDto
    {
        public string SubscriptionType { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class UserRegisterDto : UserLoginDto
    {
        public string Email { get; set; }
    }

    public class UserLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserProfileUpdateDto
    {
        public string Password { get; set; }
    }

}
