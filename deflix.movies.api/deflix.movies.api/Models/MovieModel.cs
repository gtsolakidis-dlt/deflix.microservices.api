namespace deflix.movies.api.Models;

public class MovieModel
{
    public Guid MovieId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Poster { get; set; }
    public string Backdrop { get; set; }
    public string Logo { get; set; }
    public Guid GenreId { get; set; }
    public string YoutubeKey { get; set; }
    public decimal CriticsRating { get; set; }
    public int PlanType { get; set; }
}