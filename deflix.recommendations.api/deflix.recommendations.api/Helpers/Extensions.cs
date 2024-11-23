namespace deflix.recommendations.api.Helpers;

public static class Extensions
{
    public static string ToSubscriptionType(this int code)
    {
        switch (code)
        {
            case 2: return "Basic";
            case 3: return "Premium";
            default: return "Free";
        }
    }
}