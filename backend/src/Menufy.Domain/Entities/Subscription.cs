using Menufy.Domain.Common;

namespace Menufy.Domain.Entities;

public class Subscription: BaseEntity
{
    public Restaurant Restaurant { get; set; }
    public SubscriptionType Type { get; set; }
    public DateTime EndDate { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
}