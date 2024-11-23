namespace deflix.userprefs.api.DTOs
{
    public class MovieDto
    {
        public Guid MovieId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Poster { get; set; }
        public string Backdrop { get; set; }
        public string Logo { get; set; }
        public Guid GenreId { get; set; }
        public string Genre { get; set; }
        public string YoutubeKey { get; set; }
        public decimal UsersRating { get; set; }
        public string UsersComment { get; set; }
        public decimal CriticsRating { get; set; }
        public string PlanType { get; set; }
    }
}
