using deflix.recommendations.api.DTOs;
using deflix.recommendations.api.Interfaces;

namespace deflix.recommendations.api.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IMovieApi _movieApi;
        private readonly IUserPreferencesApi _userPreferencesApi;

        public RecommendationService( IMovieApi movieApi, IUserPreferencesApi userPreferencesApi)
        {
            _movieApi = movieApi;
            _userPreferencesApi = userPreferencesApi;
        }

        public IEnumerable<MovieDto> GetRecommendationsForUser(Guid userId)
        {
            var userPreferences = _userPreferencesApi.GetFavoritesForUser(userId)?.Result.ToList();
            var movies = _movieApi.GetAllMovies(userId)?.Result.ToList();
            
            if (!(userPreferences?.Count > 0)) return movies;
            
            var userMovies = userPreferences.Select(u => u.MovieId).ToList();
            var genre = userPreferences.Select(u => u.GenreId).ToList();
            var selectMovies = movies?.Where(m => !userMovies.Contains(m.MovieId) && genre.Contains(m.GenreId)).ToList();

            return selectMovies;
        }
    }

}
