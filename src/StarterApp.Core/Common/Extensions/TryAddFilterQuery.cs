using StarterApp.Core.Common.PageSort;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace StarterApp.Core.Common.Extensions
{
    public static class QueryExtensions
    {
        public static bool TryAddFilterQuery<TListVm, TFilterVm>(this PageSort<TListVm, TFilterVm> pageSort, ref IQueryable<TListVm> query, Expression<Func<TListVm, bool>> expression) where TListVm : class where TFilterVm : class
        {
            if (pageSort.Filter == null)
                return false;
            query = query.Where(expression);
            return true;
        }
    }

}
