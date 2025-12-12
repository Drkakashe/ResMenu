namespace Menufy.Domain.Entities;

public class Feature
{
    public List<SubscriptionType> SubscriptionTypes { get; set; } = [];
    public decimal Price { get; set; }
    public string Name { get; set; }
    public string Description  { get; set; }
}