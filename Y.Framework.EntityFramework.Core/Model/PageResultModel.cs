namespace Y.Framework.EntityFramework.Core.Model;

public class PageResultModel<T>
{
    public PageResultModel(int pageIndex, int pageSize, int totalCount, List<T> data)
    {
        this.PageIndex = pageIndex;
        this.PageSize = pageSize;
        this.Total = totalCount;
        this.Data = data;
    }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public List<T> Data { get; set; }
}