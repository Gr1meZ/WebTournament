namespace WebTournament.Application.SeedPaging
{
    public class PagedRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public string OrderColumn { get; set; }
        public string OrderDir { get; set; }
        
        protected PagedRequest()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        protected PagedRequest(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}