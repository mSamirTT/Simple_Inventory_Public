using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace StarterApp.Core.Common.PageSort
{
    public class PageSort<T> where T : class
    {
        private int _page = 0;
        private int _pageSize = 10;
        private bool Desc => SortDirection == "desc";
        public int Page
        {
            get => _page;
            set
            {
                _page = value < 0 ? 0 : value;
            }
        }
        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = value <= 0 ? 10 : value;
            }
        }
        public string OrderBy { get; set; }
        public string SortDirection { get; set; }
        public async Task<PaginatedList<T>> ApplyAsync(IQueryable<T> query)
        {
            int count;
            var list = await query.ApplySort(OrderBy, Desc)
                .ApplyPagination(Page, PageSize, out count)
                .ToListAsync();
            return new PaginatedList<T>
            {
                Payload = list,
                Count = count
            };
        }
    }

    public class PageSort<T, TFilter> : PageSort<T> where T : class where TFilter : class
    {
        public TFilter Filter { get; set; }
    }
}
