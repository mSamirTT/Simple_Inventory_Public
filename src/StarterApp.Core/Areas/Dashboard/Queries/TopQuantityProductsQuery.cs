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
            var totalCount = await _repository.Query
                .Include(x => x.IssueDetails)
                .Include(x => x.SupplyDetails)
                .SumAsync(x => x.SupplyDetails.Sum(x => x.Quantity) - x.IssueDetails.Sum(x => x.Quantity));


            var items = await _repository.Query
                .Include(x => x.IssueDetails)
                .Include(x => x.SupplyDetails)
                .Select(x => new TopQtyProductItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Qty = x.SupplyDetails.Sum(x => x.Quantity) - x.IssueDetails.Sum(x => x.Quantity)
                })
                .Where(x => x.Qty > 0)
                .OrderByDescending(x => x.Qty)
                .Take(4)
                .ToListAsync();

            var fetchedItemsQty = items.Sum(x => x.Qty);

            items.Add(new TopQtyProductItem
            {
                Id = 0,
                Name = "Other",
                Qty = totalCount - fetchedItemsQty
            });

            return new TopQtyProduct { TotalQty = totalCount, Items = items };
        }
    }
}
