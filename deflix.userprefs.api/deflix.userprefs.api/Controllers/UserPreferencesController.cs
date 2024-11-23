using deflix.userprefs.api.DTOs;
using deflix.userprefs.api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace deflix.userprefs.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserPreferencesController : ApiController
    {
        private readonly IUserPreferencesService _userPreferencesService;

        public UserPreferencesController(IUserPreferencesService userPreferencesService)
        {
            _userPreferencesService = userPreferencesService;
        }

        [HttpGet("favorites/user/{userId}")]
        public IActionResult GetFavoritesForUser(Guid userId)
        {
            var favorites = _userPreferencesService.GetFavoritesForUser(userId);
            if (favorites == null || !favorites.Any())
            {
                return NotFound(new { message = "No favorites found for this user." });
            }

            return Ok(favorites);
        }

        [HttpPost("favorites/user/{userId}/add/{movieId}")]
        public IActionResult AddFavorite(Guid userId, Guid movieId)
        {
            _userPreferencesService.AddFavorite(userId, movieId);
            return Ok(new { message = "Movie added to favorites successfully." });
        }

        [HttpDelete("favorites/user/{userId}/remove/{movieId}")]
        public IActionResult RemoveFavorite(Guid userId, Guid movieId)
        {
            _userPreferencesService.RemoveFavorite(userId, movieId);
            return Ok(new { message = "Movie removed from favorites successfully." });
        }

        [HttpGet("watchlists/user/{userId}")]
        public IActionResult GetWatchlistForUser(Guid userId)
        {
            var watchlist = _userPreferencesService.GetWatchlistForUser(userId);
            if (watchlist == null || !watchlist.Any())
            {
                return NotFound(new { message = "No watchlist found for this user." });
            }

            return Ok(watchlist);
        }

        [HttpPost("watchlists/user/{userId}/add/{movieId}")]
        public IActionResult AddToWatchlist(Guid userId, Guid movieId)
        {
            _userPreferencesService.AddToWatchlist(userId, movieId);
            return Ok(new { message = "Movie added to watchlist successfully." });
        }

        [HttpDelete("watchlists/user/{userId}/remove/{movieId}")]
        public IActionResult RemoveFromWatchlist(Guid userId, Guid movieId)
        {
            _userPreferencesService.RemoveFromWatchlist(userId, movieId);
            return Ok(new { message = "Movie removed from watchlist successfully." });
        }

        [HttpPost("comment/user/{userId}/add/{movieId}")]
        public IActionResult AddComment(Guid userId, Guid movieId, [FromBody] UserCommentDTO comment)
        {
            _userPreferencesService.AddComment(userId, movieId, comment);
            return Ok(new { message = "Movie comment added successfully." });
        }
    }
}
