

namespace EventBus.Messages.Events;

public class BasketCheckoutEventV2: BaseIntergrationEvent
{
    public string? UserName { get; set; }

    public decimal? TotalPrice { get; set; }
}
