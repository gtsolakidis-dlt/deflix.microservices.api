using deflix.recommendations.api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace deflix.recommendations.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationsController : ApiController
    {
        private readonly IRecommendationService _recommendationService;

        public RecommendationsController(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetRecommendationsForUser(Guid userId)
        {
            var recommendations = _recommendationService.GetRecommendationsForUser(userId)?.ToList();
            if (recommendations?.Count > 0)
            {
                return Ok(recommendations);
            }
            return NotFound(new { message = "No recommendations available for this user." });
        }
    }

}
