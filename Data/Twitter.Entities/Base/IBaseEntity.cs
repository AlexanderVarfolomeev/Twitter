namespace Twitter.Entities.Base;

public interface IBaseEntity
{
    public Guid Id { get; set; }
    public bool IsNew { get; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
    public void Init();
}