namespace deflix.subscriptions.api.Models;

public class UserSubscriptionModel
{
    public Guid UserId { get; set; }
    public Guid UserSubscriptionId { get; set; }
    public int SubscriptionCode { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string PaymentMethod { get; set; }
}