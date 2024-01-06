
namespace WebTournament.Application.SeedPaging
{
    public class PagedResponse<T> 
    {
        public PagedResponse(T items, int totalItemCount, int pageNumber, int pageSize)
        {
            Data = items;
            Metadata = new PagedList(pageNumber, pageSize, totalItemCount);
        }
        
        public PagedList Metadata { get; }
        public T Data { get; }
    }
    
    public class PagedList
    {
        public PagedList(int pageNumber, int pageSize, int totalItemCount)
        {
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            PageCount = (TotalItemCount > 0) ? ((int)Math.Ceiling((double)(TotalItemCount) / PageSize)) : 0;
            PageNumber = pageNumber > (PageCount - 1) ? (PageCount - 1) : pageNumber;
            HasPreviousPage = PageNumber > 0;
            HasNextPage = PageNumber < (PageCount - 1);
            IsFirstPage = PageNumber == 0;
            IsLastPage = PageNumber >= (PageCount - 1);
        }
        
        public int PageCount { get; }
        public int TotalItemCount { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public bool HasPreviousPage { get; }
        public bool HasNextPage { get; }
        public bool IsFirstPage { get; }
        public bool IsLastPage { get; }
    }
}