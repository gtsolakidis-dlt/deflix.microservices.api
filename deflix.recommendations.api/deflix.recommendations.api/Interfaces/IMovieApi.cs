using deflix.recommendations.api.DTOs;
using Refit;

namespace deflix.recommendations.api.Interfaces
{
    public interface IMovieApi
    {
        [Get("/api/movies/user/{userId}/list")]
        Task<IEnumerable<MovieDto>> GetAllMovies(Guid userId);
    }

}
