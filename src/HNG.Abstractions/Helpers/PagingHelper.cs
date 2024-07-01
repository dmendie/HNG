using HNG.Abstractions.Models;

namespace HNG.Abstractions.Helpers
{
    public static class PagingHelper
    {
        public static PagedList<T> ToPagedList<T>(IEnumerable<T> query, int pageNumber, int pageSize)
        {
            pageNumber = (pageNumber < 1) ? 1 : pageNumber;
            var count = query.Count();
            var items = query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
