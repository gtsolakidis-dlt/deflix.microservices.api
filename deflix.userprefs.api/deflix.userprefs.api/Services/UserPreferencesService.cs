using deflix.userprefs.api.DTOs;
using deflix.userprefs.api.Interfaces;
using deflix.userprefs.api.Models;

namespace deflix.userprefs.api.Services
{
    public class UserPreferencesService : IUserPreferencesService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IMovieApi _movieApi;

        #region SqlQueries
        private static string InsertQuery => @"INSERT INTO [UserPreferences]
                                                ([UserPreferencesId],[UserId],[MovieId],UserRating,UserComment,[IsFavorite],[IsWatchList])
                                                VALUES
                                                (NEWID(),@UserId,@MovieId,0,'',0,0)
                                                ";

        private static string UpdateFavoriteQuery => @"UPDATE [UserPreferences]
                                                SET [IsFavorite] = @IsFavorite
                                                WHERE [UserId] = @UserId And MovieId=@MovieId
                                                ";

        private static string UpdateWatchlistQuery => @"UPDATE [UserPreferences]
                                                SET [IsWatchList] = @IsWatchList
                                                WHERE [UserId] = @UserId And MovieId=@MovieId
                                                ";

        private static string UpdateCommentQuery => @"UPDATE [UserPreferences]
                                                SET [UserRating] = @UserRating, UserComment = @UserComment
                                                WHERE [UserId] = @UserId And MovieId=@MovieId
                                                ";

        public static string GetUsersPreferencesQuery =>
            @"SELECT [UserPreferencesId],[UserId],[MovieId],UserRating,UserComment,[IsFavorite],[IsWatchList]
            FROM [UserPreferences]
            order by MovieId,UserRating desc
            ";

        public static string GetUserPreferencesQuery =>
            @"SELECT [UserPreferencesId],[UserId],[MovieId],UserRating,UserComment,[IsFavorite],[IsWatchList]
            FROM [UserPreferences]
            WHERE f.UserId = @UserId
            ";

        #endregion

        public UserPreferencesService(IDatabaseService databaseService, IMovieApi movieApi)
        {
            _databaseService = databaseService;
            _movieApi = movieApi;
        }

        public IEnumerable<MovieDto> GetFavoritesForUser(Guid userId)
        {
            var userPreferences = _databaseService.Query<UserPreferencesModel>(GetUserPreferencesQuery, new { userId });
            if (userPreferences == null) return null;

            var movieIds = userPreferences.Where(up => up.IsFavorite).Select(u => u.MovieId).ToList();
            if (movieIds.Count == 0) return null;
            var movies = _movieApi.GetMovieByIds(userId, movieIds).Result;
            return movies;
        }

        public void AddFavorite(Guid userId, Guid movieId)
        {
            var userPreferences = _databaseService.Query<UserPreferencesModel>(GetUserPreferencesQuery, new { userId });
            if (userPreferences == null)
            {
                _databaseService.Execute(InsertQuery, new { UserId = userId, MovieId = movieId });
            }
            _databaseService.Execute(UpdateFavoriteQuery, new { UserId = userId, MovieId = movieId, IsFavorite = 1 });
        }

        public void RemoveFavorite(Guid userId, Guid movieId)
        {
            var userPreferences = _databaseService.Query<UserPreferencesModel>(GetUserPreferencesQuery, new { userId });
            if (userPreferences == null) return;
            _databaseService.Execute(UpdateFavoriteQuery, new { UserId = userId, MovieId = movieId, IsFavorite = 0 });
        }

        public IEnumerable<MovieDto> GetWatchlistForUser(Guid userId)
        {

            var userPreferences = _databaseService.Query<UserPreferencesModel>(GetUserPreferencesQuery, new { userId });
            if (userPreferences == null) return null;
            var movieIds = userPreferences.Where(up => up.IsWatchList).Select(u => u.MovieId).ToList();
            if (movieIds.Count == 0) return null;
            var movies = _movieApi.GetMovieByIds(userId, movieIds).Result;
            return movies;
        }

        public void AddToWatchlist(Guid userId, Guid movieId)
        {
            var userPreferences = _databaseService.Query<UserPreferencesModel>(GetUserPreferencesQuery, new { userId });
            if (userPreferences == null)
            {
                _databaseService.Execute(InsertQuery, new { UserId = userId, MovieId = movieId });
            }
            _databaseService.Execute(UpdateWatchlistQuery, new { UserId = userId, MovieId = movieId, IsWatchList = 1 });
        }

        public void RemoveFromWatchlist(Guid userId, Guid movieId)
        {
            var userPreferences = _databaseService.Query<UserPreferencesModel>(GetUserPreferencesQuery, new { userId });
            if (userPreferences == null) return;
            _databaseService.Execute(UpdateWatchlistQuery, new { UserId = userId, MovieId = movieId, IsWatchList = 0 });
        }

        public void AddComment(Guid userId, Guid movieId, UserCommentDTO comment)
        {
            var userPreferences = _databaseService.Query<UserPreferencesModel>(GetUserPreferencesQuery, new { userId });
            if (userPreferences == null)
            {
                _databaseService.Execute(InsertQuery, new { UserId = userId, MovieId = movieId });
            }
            _databaseService.Execute(UpdateCommentQuery, new { UserId = userId, MovieId = movieId, UserRating = comment.Rating, UserComment = comment.Comment });
        }
    }
}
