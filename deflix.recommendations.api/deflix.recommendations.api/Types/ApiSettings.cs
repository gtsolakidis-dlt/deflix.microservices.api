using System.ComponentModel.DataAnnotations;

namespace deflix.recommendations.api.Types
{
    public sealed class ApiSettings
    {
        [Required, Url]
        public string BaseAddress { get; init; } = string.Empty;

        [Required]
        public int TimeoutInMinutes { get; init; } = 10;
    }
}
