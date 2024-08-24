

namespace EventBus.Messages.Events;

public class BaseIntergrationEvent
{
    //Co-Relation Id
    public string CorrelationId { get; set; }

    //public Guid Id { get; private set; }

    public DateTime CreationDate { get; private set; }

    public BaseIntergrationEvent()
    {
        CorrelationId = Guid.NewGuid().ToString();
        CreationDate = DateTime.UtcNow;
    }

    public BaseIntergrationEvent(Guid correlationId, DateTime creationDate)
    {
        CorrelationId = correlationId.ToString();
        CreationDate = creationDate;
    }
}
