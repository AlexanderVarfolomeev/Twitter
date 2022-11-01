namespace Twitter.Entities.Base;

public abstract class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }

    public bool IsNew
    {
        get => Id == Guid.Empty;
    }

    public void Init()
    {
        Id = Guid.NewGuid();
        CreationTime = ModificationTime = DateTime.UtcNow;
    }
}