using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Supplies.Entities;
using StarterApp.Core.Areas.Supplies.ViewModels;
using StarterApp.Core.Common.Extensions;
using StarterApp.Core.Common.Models;
using StarterApp.Core.Common.PageSort;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Supplies.Queries
{
    public class GetSupplyHeaderListQuery : IRequest<PaginatedList<SupplyHeaderVm>>
    {
        public readonly PageSort<SupplyHeaderVm, SupplyHeaderFilterVm> PageSort;
        public GetSupplyHeaderListQuery(PageSort<SupplyHeaderVm, SupplyHeaderFilterVm> PageSort)
        {
            this.PageSort = PageSort;
        }

        public class GetSupplyHeaderListQueryHandler : BaseHandler<SupplyHeader>, IRequestHandler<GetSupplyHeaderListQuery, PaginatedList<SupplyHeaderVm>>
        {
            public async Task<PaginatedList<SupplyHeaderVm>> Handle(GetSupplyHeaderListQuery request, CancellationToken cancellationToken)
            {
                var filter = request.PageSort.Filter;
                var query = _repository.Query
                    .Include(x => x.SupplyDetails)
                    .ThenInclude(x => x.Product)
                    .ProjectTo<SupplyHeaderVm>(_mapper.ConfigurationProvider);
                request.PageSort.TryAddFilterQuery(ref query, x => filter.SupplyDateFrom == null || x.SupplyDate >= filter.SupplyDateFrom);
                request.PageSort.TryAddFilterQuery(ref query, x => filter.SupplyDateTo == null || x.SupplyDate <= filter.SupplyDateTo);
                request.PageSort.TryAddFilterQuery(ref query, x => string.IsNullOrEmpty(filter.SearchText) ||
                    x.Notes.ToLower().Contains(filter.SearchText.ToLower()) ||
                    x.TransactionNumber.ToString().Contains(filter.SearchText));

                var resultVm = await request.PageSort.ApplyAsync(query);
                return resultVm;
            }
        }
    }
}
