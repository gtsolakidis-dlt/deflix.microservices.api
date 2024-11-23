namespace deflix.subscriptions.api.Models;

public class SubscriptionModel
{
    public Guid SubscriptionId { get; set; }
    public int SubscriptionCode { get; set; }
    public int DurationInDays { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}