namespace deflix.userprefs.api.Interfaces
{
    using DTOs;
    using System.Collections.Generic;

    public interface IUserPreferencesService
    {
        IEnumerable<MovieDto> GetFavoritesForUser(Guid userId);
        void AddFavorite(Guid userId, Guid movieId);
        void RemoveFavorite(Guid userId, Guid movieId);

        IEnumerable<MovieDto> GetWatchlistForUser(Guid userId);
        void AddToWatchlist(Guid userId, Guid movieId);
        void RemoveFromWatchlist(Guid userId, Guid movieId);
        void AddComment(Guid userId, Guid movieId, UserCommentDTO comment);
    }

}
