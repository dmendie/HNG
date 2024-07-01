namespace HNG.Abstractions.Models
{
    public class PagedList<T>
    {
        public PagedListMetadata Meta { get; set; } = new PagedListMetadata();
        public List<T> Data { get; set; } = new List<T>();

        public PagedList()
        {
        }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            Meta = new PagedListMetadata
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            Data = items;
        }

        public PagedList(List<T> items, int pageNumber, int pageSize)
        {
            Meta = new PagedListMetadata
            {
                TotalCount = null,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = null
            };

            Data = items;
        }
    }
}
