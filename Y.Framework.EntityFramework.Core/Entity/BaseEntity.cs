using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Y.Framework.EntityFramework.Core.Entity;

public abstract class BaseEntity
{
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    [DefaultValue("true")]
    public bool IsDeleted { get; set; } = false;

    [Timestamp]
    public byte[]? Timestamp { get; set; }

}
