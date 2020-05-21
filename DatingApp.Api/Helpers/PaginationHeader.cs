namespace DatingApp.Api.Helpers
{
    public class PaginationHeader
    {
        public int CurrentPage { get; set; }
        public int ItemPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public PaginationHeader(int currentpage, int itemPerPage, int totalItems,int totalPages)
        {
            this.CurrentPage = currentpage;
            this.ItemPerPage = itemPerPage;
            this.TotalItems = totalItems;
            this.TotalPages = totalPages;
        }
    }
}