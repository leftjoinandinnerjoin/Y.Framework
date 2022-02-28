using System.ComponentModel.DataAnnotations;

namespace Y.Framework.EntityFramework.Core.Entity;

public interface IBaseEntity<TKey> where TKey : new()
{
    [Key]
    public TKey? Id { get; set; }
}