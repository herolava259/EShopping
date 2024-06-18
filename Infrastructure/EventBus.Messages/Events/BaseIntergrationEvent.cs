

namespace EventBus.Messages.Events;

public class BaseIntergrationEvent
{
    //Co-Relation Id
    public Guid Id { get; private set; }

    public DateTime CreationDate { get; private set; }

    public BaseIntergrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    public BaseIntergrationEvent(Guid id, DateTime creationDate)
    {
        Id = id;
        CreationDate = creationDate;
    }
}
