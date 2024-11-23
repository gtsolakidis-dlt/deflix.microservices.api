using deflix.movies.api.DTOs;

namespace deflix.movies.api.Interfaces
{
    public interface IMovieService
    {
        IEnumerable<MovieDto> GetAllMovies(Guid userId);
        MovieDto GetMovieById(Guid userId, Guid movieId);
        IEnumerable<MovieDto> GetMovieByIds(Guid userId, List<Guid> movieIds);
        void AddMovie(AddMovieDto movieDto);
        IEnumerable<MovieDto> GetMoviesByGenre(Guid userId, Guid genreId);
        
    }

}
