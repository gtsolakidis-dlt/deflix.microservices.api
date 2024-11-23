using deflix.recommendations.api.DTOs;

namespace deflix.recommendations.api.Interfaces
{
    public interface IRecommendationService
    {
        IEnumerable<MovieDto> GetRecommendationsForUser(Guid userId);
    }

}
