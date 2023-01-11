namespace StoreApi.Admin.Dtos.CategoryDtos
{
    public class PaginatedListDto<T>
    {
        public PaginatedListDto(List<T> items, int pageIndex, int pageSize, int totalCount)
        {
            Items = items;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalPage = (int)Math.Ceiling(totalCount / (double)pageSize);

        }
    
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }  
        public int  PageSize { get; set; }
        public int TotalPage { get; set; }
        public bool HasPrev => PageIndex > 1;
        public bool HasNext => PageIndex < TotalPage;

    }
}
