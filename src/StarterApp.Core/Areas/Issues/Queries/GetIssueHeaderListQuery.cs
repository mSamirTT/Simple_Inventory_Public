using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Issues.Entities;
using StarterApp.Core.Areas.Issues.ViewModels;
using StarterApp.Core.Common.Extensions;
using StarterApp.Core.Common.Models;
using StarterApp.Core.Common.PageSort;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Issues.Queries
{
    public class GetIssueHeaderListQuery : IRequest<PaginatedList<IssueHeaderVm>>
    {
        public readonly PageSort<IssueHeaderVm, IssueHeaderFilterVm> PageSort;
        public GetIssueHeaderListQuery(PageSort<IssueHeaderVm, IssueHeaderFilterVm> PageSort)
        {
            this.PageSort = PageSort;
        }

        public class GetIssueHeaderListQueryHandler : BaseHandler<IssueHeader>, IRequestHandler<GetIssueHeaderListQuery, PaginatedList<IssueHeaderVm>>
        {
            public async Task<PaginatedList<IssueHeaderVm>> Handle(GetIssueHeaderListQuery request, CancellationToken cancellationToken)
            {
                var filter = request.PageSort.Filter;
                var query = _repository.Query
                    .Include(x => x.IssueDetails)
                    .ThenInclude(x => x.Product)
                    .ProjectTo<IssueHeaderVm>(_mapper.ConfigurationProvider);
                request.PageSort.TryAddFilterQuery(ref query, x => filter.IssueDateFrom == null || x.IssueDate >= filter.IssueDateFrom);
                request.PageSort.TryAddFilterQuery(ref query, x => filter.IssueDateTo == null || x.IssueDate <= filter.IssueDateTo);
                request.PageSort.TryAddFilterQuery(ref query, x => string.IsNullOrEmpty(filter.SearchText) ||
                    x.Notes.ToLower().Contains(filter.SearchText.ToLower()) ||
                    x.TransactionNumber.ToString().Contains(filter.SearchText));

                var resultVm = await request.PageSort.ApplyAsync(query);
                return resultVm;
            }
        }
    }
}
