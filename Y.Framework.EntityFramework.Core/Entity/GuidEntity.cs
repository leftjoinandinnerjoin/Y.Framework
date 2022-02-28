namespace Y.Framework.EntityFramework.Core.Entity;

public class GuidEntity : BaseEntity, IBaseEntity<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();
}