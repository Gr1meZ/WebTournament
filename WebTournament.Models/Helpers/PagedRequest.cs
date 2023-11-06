namespace WebTournament.Models.Helpers
{
    public class PagedRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public string OrderColumn { get; set; }
        public string OrderDir { get; set; }
        
        public PagedRequest()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public PagedRequest(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}