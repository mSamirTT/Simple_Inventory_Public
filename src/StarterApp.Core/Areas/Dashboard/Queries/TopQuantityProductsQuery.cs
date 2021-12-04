using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Dashboard.ViewModels;
using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Common.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Dashboard.Queries
{
    public class TopQuantityProductsQuery : IRequest<TopQtyProduct>
    {
        public TopQuantityProductsQuery()
        {
        }
    }
    public class TopQuantityProductsQueryHandler : BaseHandler<Product>, IRequestHandler<TopQuantityProductsQuery, TopQtyProduct>
    {
        public async Task<TopQtyProduct> Handle(TopQuantityProductsQuery request, CancellationToken cancellationToken)
        {
            var totalSupplyCount = await _repository.Query
                .Include(x => x.SupplyDetails)
                .SelectMany(x => x.SupplyDetails)
                .SumAsync(x => x.Quantity, cancellationToken);

            var totalIssueCount = await _repository.Query
                .Include(x => x.IssueDetails)
                .SelectMany(x => x.IssueDetails)
                .SumAsync(x => x.Quantity, cancellationToken);

            var totalCount = totalSupplyCount - totalIssueCount;

            var top4Items = await _repository.Query
                .Include(x => x.IssueDetails)
                .Include(x => x.SupplyDetails)
                .Select(x => new TopQtyProductItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Qty = x.SupplyDetails.Sum(s => s.Quantity) - x.IssueDetails.Sum(x => x.Quantity)
                })
                .Where(x => x.Qty > 0)
                .OrderByDescending(x => x.Qty)
                .Take(4)
                .ToListAsync(cancellationToken);

            var fetchedItemsQty = top4Items.Sum(x => x.Qty);

            top4Items.Add(new TopQtyProductItem
            {
                Id = 0,
                Name = "Other",
                Qty = totalCount - fetchedItemsQty
            });

            return new TopQtyProduct { TotalQty = totalCount, Items = top4Items };
        }
    }
}
