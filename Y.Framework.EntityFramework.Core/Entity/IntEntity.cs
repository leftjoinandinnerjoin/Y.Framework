namespace Y.Framework.EntityFramework.Core.Entity;

public class IntEntity : BaseEntity, IBaseEntity<int>
{
    public int Id { get; set; }
}