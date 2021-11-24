using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Dashboard.ViewModels;
using StarterApp.Core.Areas.Supplies.Entities;
using StarterApp.Core.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Dashboard.Queries
{
    public class RecentSupplyingsQuery : IRequest<List<RecentIssuingSupplyingVm>>
    {
        public RecentSupplyingsQuery()
        {
        }

        public class RecentSupplyingQueryHandler : BaseHandler<SupplyHeader>, IRequestHandler<RecentSupplyingsQuery, List<RecentIssuingSupplyingVm>>
        {
            public async Task<List<RecentIssuingSupplyingVm>> Handle(RecentSupplyingsQuery request, CancellationToken cancellationToken)
            {
                var resultVm = await _repository.Query
                    .Include(x => x.SupplyDetails)
                    .Select(x => new RecentIssuingSupplyingVm
                    {
                        Id = x.Id,
                        Date = x.SupplyDate,
                        Notes = x.Notes,
                        TotalQty = x.SupplyDetails.Sum(x => x.Quantity)
                    })
                    .OrderByDescending(x => x.Date)
                    .Take(5)
                    .ToListAsync();
                return resultVm;
            }
        }
    }
}
