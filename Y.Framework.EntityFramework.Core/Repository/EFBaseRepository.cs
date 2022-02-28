using Microsoft.EntityFrameworkCore;
using Y.Framework.EntityFramework.Core.Entity;

namespace Y.Framework.EntityFramework.Core.Repository;

public class EFBaseRepository<T> : IEFBaseRepository<T> where T : BaseEntity
{
    readonly DbContext _DbContext;

    public EFBaseRepository(DbContext context)
    {
        _DbContext = context;
    }
}