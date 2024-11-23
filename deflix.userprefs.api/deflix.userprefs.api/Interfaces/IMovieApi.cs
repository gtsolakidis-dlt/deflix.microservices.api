using deflix.userprefs.api.DTOs;
using Refit;

namespace deflix.userprefs.api.Interfaces
{
    public interface IMovieApi
    {
        [Get("/api/movies/user/{userId}/list")]
        Task<IEnumerable<MovieDto>> GetAllMovies(Guid userId);

        [Post("/api/movies/user/{userId}/list")]
        Task<IEnumerable<MovieDto>> GetMovieByIds(Guid userId, List<Guid> movieIds);
    }

}
