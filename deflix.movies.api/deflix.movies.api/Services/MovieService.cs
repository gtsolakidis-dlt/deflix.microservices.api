using deflix.movies.api.DTOs;
using deflix.movies.api.Interfaces;
using deflix.movies.api.Models;

namespace deflix.movies.api.Services
{
    public class MovieService : IMovieService
    {
        private readonly IDatabaseService _databaseService;
        private readonly ISubscriptionsApi _subscriptionsApi;

        #region SqlQueries

        private static string GetAllMoviesQuery => "SELECT * FROM Movies WHERE PlanType = @PlanType";
        private static string GetMovieByIdQuery => "SELECT * FROM Movies WHERE MovieId = @MovieId AND PlanType = @PlanType";

        private static string GetMovieByIdsQuery => "SELECT * FROM Movies WHERE MovieId in @MovieIds AND PlanType = @PlanType";

        private static string GetMoviesByGenreQuery => "SELECT * FROM Movies WHERE GenreId = @GenreId AND PlanType = @PlanType";

        private static string GetAllGenreQuery => "SELECT * FROM Genre";
        private static string GetGenreByIdQuery => "SELECT * FROM Genre WHERE GenreId = @GenreId";

        private static string InsertMovieQuery =>
            @"INSERT INTO Movies (MovieId, Title, Description, Poster, Backdrop, Logo, GenreId, YoutubeKey, CriticsRating, PlanType)
              VALUES (@MovieId, @Title, @Description, @Poster, @Backdrop, @Logo, @GenreId, @YoutubeKey, @CriticsRating, @PlanType)";

        #endregion

        public MovieService(IDatabaseService databaseService, ISubscriptionsApi subscriptionsApi)
        {
            _databaseService = databaseService;
            _subscriptionsApi = subscriptionsApi;
        }

        public IEnumerable<MovieDto> GetAllMovies(Guid userId)
        {
            var userSubscription = _subscriptionsApi.GetUserSubscription(userId).Result;
            var moviesDbList = _databaseService.Query<MovieModel>(GetAllMoviesQuery);
            if (moviesDbList == null) return null;
            var movies = new List<MovieDto>();

            var subscription = _subscriptionsApi.GetAllSubscriptions().Result.FirstOrDefault(s => s.Id == userSubscription.SubscriptionCode);
            var genreList = _databaseService.Query<GenreModel>(GetAllGenreQuery);

            moviesDbList?.ForEach(m =>
            {
                var movie = ToMovieDto(m);
                movie.PlanType = subscription?.Name;
                movie.Genre = genreList.FirstOrDefault(g => g.GenreId == m.GenreId)?.Name;
                if (CanViewMovie(m.PlanType, userSubscription.SubscriptionCode)) movies.Add(movie);
            });

            return movies;
        }

        public MovieDto GetMovieById(Guid userId, Guid movieId)
        {
            var userSubscription = _subscriptionsApi.GetUserSubscription(userId).Result;
            var movieDb = _databaseService.QueryFirst<MovieModel>(GetMovieByIdQuery, new { MovieId = movieId, PlanType = userSubscription.SubscriptionCode });
            if (movieDb == null) return null;

            var subscription = _subscriptionsApi.GetAllSubscriptions().Result.FirstOrDefault(s => s.Id == userSubscription.SubscriptionCode);
            var genre = _databaseService.QueryFirst<GenreModel>(GetGenreByIdQuery, new { movieDb.GenreId });
            var movie = ToMovieDto(movieDb);
            movie.PlanType = subscription?.Name;
            movie.Genre = genre?.Name;

            return movie;
        }

        public IEnumerable<MovieDto> GetMovieByIds(Guid userId, List<Guid> movieIds)
        {
            var userSubscription = _subscriptionsApi.GetUserSubscription(userId).Result;
            var moviesDbList = _databaseService.Query<MovieModel>(GetMovieByIdsQuery, new { MovieIds = movieIds, PlanType = userSubscription.SubscriptionCode });
            if (moviesDbList == null) return null;
            var movies = new List<MovieDto>();

            var subscription = _subscriptionsApi.GetAllSubscriptions().Result.FirstOrDefault(s => s.Id == userSubscription.SubscriptionCode);
            var genreList = _databaseService.Query<GenreModel>(GetAllGenreQuery);
            moviesDbList?.ForEach(m =>
            {
                var movie = ToMovieDto(m);
                movie.PlanType = subscription?.Name;
                movie.Genre = genreList.FirstOrDefault(g => g.GenreId == m.GenreId)?.Name;
                if (CanViewMovie(m.PlanType, userSubscription.SubscriptionCode)) movies.Add(movie);
            });

            return movies;
        }

        public IEnumerable<MovieDto> GetMoviesByGenre(Guid userId, Guid genreId)
        {
            var userSubscription = _subscriptionsApi.GetUserSubscription(userId).Result;
            var moviesDbList = _databaseService.Query<MovieModel>(GetMoviesByGenreQuery, new { GenreId = genreId, PlanType = userSubscription.SubscriptionCode });
            if (moviesDbList == null) return null;

            var subscription = _subscriptionsApi.GetAllSubscriptions().Result.FirstOrDefault(s => s.Id == userSubscription.SubscriptionCode);
            var genre = _databaseService.QueryFirst<GenreModel>(GetGenreByIdQuery, new { GenreId = genreId });

            var movies = new List<MovieDto>();

            moviesDbList?.ForEach(m =>
            {
                var movie = ToMovieDto(m);
                movie.PlanType = subscription?.Name;
                movie.Genre = genre?.Name;
                if (CanViewMovie(m.PlanType, userSubscription.SubscriptionCode)) movies.Add(movie);
            });

            return movies;
        }

        public void AddMovie(AddMovieDto movieDto)
        {
            var param = new MovieModel
            {
                MovieId = Guid.NewGuid(),
                Title = movieDto.Title,
                Description = movieDto.Description,
                Poster = movieDto.Poster,
                Backdrop = movieDto.Backdrop,
                Logo = movieDto.Logo,
                GenreId = movieDto.GenreId,
                YoutubeKey = movieDto.YoutubeKey,
                CriticsRating = movieDto.CriticsRating,
                PlanType = movieDto.PlanType
            };

            _databaseService.Execute(InsertMovieQuery, param);
        }

        private MovieDto ToMovieDto(MovieModel model)
        {
            return new MovieDto
            {
                MovieId = model.MovieId,
                Title = model.Title,
                Description = model.Description,
                Poster = model.Poster,
                Backdrop = model.Backdrop,
                Logo = model.Logo,
                Genre = "",
                GenreId = model.GenreId,
                YoutubeKey = model.YoutubeKey,
                UsersRating = 0,
                UsersComment = null,
                CriticsRating = model.CriticsRating,
                PlanType = model.PlanType.ToString()
            };
        }

        private bool CanViewMovie(int moviePlan, int userPlan)
        {
            return moviePlan <= userPlan;
        }
    }

}
