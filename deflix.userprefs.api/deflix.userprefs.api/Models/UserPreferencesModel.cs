namespace deflix.userprefs.api.Models;

public class UserPreferencesModel
{
    public Guid UserPreferencesId { get; set; }

    public Guid UserId { get; set; }

    public Guid MovieId { get; set; }

    public decimal UserRating { get; set; }

    public string UserComment { get; set; }

    public bool IsFavorite { get; set; }

    public bool IsWatchList { get; set; }
}