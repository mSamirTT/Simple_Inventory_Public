using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Dashboard.ViewModels;
using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Dashboard.Queries
{
    public class LeastProductVsLastWeekQuery : IRequest<PerformanceVsLastWeekVm>
    {
        public LeastProductVsLastWeekQuery()
        {
        }
    }
    public class LeastProductVsLastWeekQueryHandler : BaseHandler<Product>, IRequestHandler<LeastProductVsLastWeekQuery, PerformanceVsLastWeekVm>
    {
        public async Task<PerformanceVsLastWeekVm> Handle(LeastProductVsLastWeekQuery request, CancellationToken cancellationToken)
        {
            var vm = await _repository.Query
                .Include(x => x.IssueDetails)
                .ThenInclude(x => x.IssueHeader)
                .Include(x => x.SupplyDetails)
                .ThenInclude(x => x.SupplyHeader)
                .Select(x => new PerformanceVsLastWeekVm
                {
                    Id = x.Id,
                    Name = x.Name,
                    Count = x.SupplyDetails.Sum(x => x.Quantity) - x.IssueDetails.Sum(x => x.Quantity),
                    CountLastWeek = x.SupplyDetails
                    .Where(x => x.SupplyHeader.SupplyDate < DateTime.Today.AddDays(-7))
                    .Sum(x => x.Quantity)
                    -
                    x.IssueDetails
                    .Where(x => x.IssueHeader.IssueDate < DateTime.Today.AddDays(-7))
                    .Sum(x => x.Quantity),

                    LastActionDate = x.IssueDetails.Max(x => x.IssueHeader.IssueDate) >
                    x.SupplyDetails.Max(x => x.SupplyHeader.SupplyDate) ?
                    x.IssueDetails.Max(x => x.IssueHeader.IssueDate) :
                    x.SupplyDetails.Max(x => x.SupplyHeader.SupplyDate)
                })
                .Where(x => x.Count > 0)
                .OrderBy(x => x.Count)
                .FirstOrDefaultAsync();
            return vm;
        }
    }
}
