using Menufy.Domain.Common;

namespace Menufy.Domain.Entities;

public class SubscriptionType : BaseEntity
{
    public List<Feature> Features { get; set; }
    public List<Subscription> Subscriptions { get; set; }
    public decimal Discount { get; set; }
    public decimal YearlyDiscount { get; set; }
}