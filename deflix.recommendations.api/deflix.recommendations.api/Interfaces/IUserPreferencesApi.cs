using deflix.recommendations.api.DTOs;
using Refit;

namespace deflix.recommendations.api.Interfaces
{
    public interface IUserPreferencesApi
    {
        [Get("/api/userPreferences/favorites/user/{userId}")]
        Task<IEnumerable<MovieDto>> GetFavoritesForUser(Guid userId);

        [Get("/api/userPreferences/watchlists/user/{userId}")]
        Task<IEnumerable<MovieDto>> GetWatchlistForUser(Guid userId);
    }
}
