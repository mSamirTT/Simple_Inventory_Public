using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Dashboard.ViewModels;
using StarterApp.Core.Areas.Issues.Entities;
using StarterApp.Core.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Dashboard.Queries
{
    public class RecentIssuingsQuery : IRequest<List<RecentIssuingSupplyingVm>>
    {
        public RecentIssuingsQuery()
        {
        }

        public class RecentIssuingQueryHandler : BaseHandler<IssueHeader>, IRequestHandler<RecentIssuingsQuery, List<RecentIssuingSupplyingVm>>
        {
            public async Task<List<RecentIssuingSupplyingVm>> Handle(RecentIssuingsQuery request, CancellationToken cancellationToken)
            {
                var resultVm = await _repository.Query
                    .Include(x => x.IssueDetails)
                    .Select(x => new RecentIssuingSupplyingVm
                    {
                        Id = x.Id,
                        Date = x.IssueDate,
                        Notes = x.Notes,
                        TotalQty = x.IssueDetails.Sum(x => x.Quantity)
                    })
                    .OrderByDescending(x => x.Date)
                    .Take(5)
                    .ToListAsync();
                return resultVm;
            }
        }
    }
}
