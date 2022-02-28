using System.ComponentModel.DataAnnotations;

namespace Y.Framework.EntityFramework.Core.Model;

public class PageQueryModel
{
    [Range(1, 2147483647)]
    public int PageIndex { get; set; }

    [Range(1, 100)]
    public int PageSize { get; set; }
}