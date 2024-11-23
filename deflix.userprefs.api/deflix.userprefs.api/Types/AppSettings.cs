namespace deflix.userprefs.api.Types
{
    public class AppSettings
    {
        public ApiSettings UsersApi { get; set; }
        public ApiSettings SubscriptionsApi { get; set; }
        public ApiSettings MoviesApi { get; set; }
        public ApiSettings UserPreferencesApi { get; set; }
        public ApiSettings RecommendationsApi { get; set; }
    }
}
