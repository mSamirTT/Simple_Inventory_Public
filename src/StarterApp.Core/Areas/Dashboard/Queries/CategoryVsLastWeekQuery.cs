using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Categories.Entities;
using StarterApp.Core.Areas.Dashboard.ViewModels;
using StarterApp.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Dashboard.Queries
{
    public class CategoryVsLastWeekQuery : IRequest<List<PerformanceVsLastWeekVm>>
    {
        public CategoryVsLastWeekQuery()
        {
        }
    }
    public class CategoryVsLastWeekQueryHandler : BaseHandler<Category>, IRequestHandler<CategoryVsLastWeekQuery, List<PerformanceVsLastWeekVm>>
    {
        public async Task<List<PerformanceVsLastWeekVm>> Handle(CategoryVsLastWeekQuery request, CancellationToken cancellationToken)
        {
            var vm = await _repository.Query
                .Include(x => x.Products)
                .ThenInclude(x => x.IssueDetails)
                .ThenInclude(x => x.IssueHeader)
                .Include(x => x.Products)
                .ThenInclude(x => x.SupplyDetails)
                .ThenInclude(x => x.SupplyHeader)
                .Select(x => new PerformanceVsLastWeekVm
                {
                    Id = x.Id,
                    Name = x.Name,
                    Count = x.Products.SelectMany(x => x.SupplyDetails).Sum(x => x.Quantity) - x.Products.SelectMany(x => x.IssueDetails).Sum(x => x.Quantity),
                    CountLastWeek = x.Products
                    .SelectMany(x => x.SupplyDetails)
                    .Where(x => x.SupplyHeader.SupplyDate < DateTime.Today.AddDays(-7))
                    .Sum(x => x.Quantity)
                    -
                    x.Products
                    .SelectMany(x => x.IssueDetails)
                    .Where(x => x.IssueHeader.IssueDate < DateTime.Today.AddDays(-7))
                    .Sum(x => x.Quantity),

                    LastActionDate = x.Products.SelectMany(x => x.IssueDetails).Max(x => x.IssueHeader.IssueDate) >
                    x.Products.SelectMany(x => x.SupplyDetails).Max(x => x.SupplyHeader.SupplyDate) ?
                    x.Products.SelectMany(x => x.IssueDetails).Max(x => x.IssueHeader.IssueDate) :
                    x.Products.SelectMany(x => x.SupplyDetails).Max(x => x.SupplyHeader.SupplyDate)
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();
            return vm;
        }
    }
}
