using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Areas.Products.ViewModels;
using StarterApp.Core.Common.Extensions;
using StarterApp.Core.Common.Models;
using StarterApp.Core.Common.PageSort;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Products.Queries
{
    public class GetProductListQuery : IRequest<PaginatedList<ProductVm>>
    {
        public readonly PageSort<ProductVm, string> PageSort;
        public GetProductListQuery(PageSort<ProductVm, string> pageSort)
        {
            PageSort = pageSort;
        }

        public class GetProductListQueryHandler : BaseHandler<Product>, IRequestHandler<GetProductListQuery, PaginatedList<ProductVm>>
        {
            public async Task<PaginatedList<ProductVm>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
            {
                var filter = request.PageSort.Filter;
                
                var query = _repository.Query
                    .Include(x => x.Category)
                    .Include(x => x.SupplyDetails)
                    .Include(x => x.IssueDetails)
                    .Select(x => new ProductVm
                    {
                        Id = x.Id,
                        CategoryId = x.Category.Id,
                        CategoryName = x.Category.Name,
                        Description = x.Description,
                        Name = x.Name,
                        Price = x.Price,
                        Thumbnail = x.Thumbnail,
                        Quantity = x.SupplyDetails.Sum(q => q.Quantity) - x.IssueDetails.Sum(q => q.Quantity)
                    });

                request.PageSort.TryAddFilterQuery(ref query, x => string.IsNullOrEmpty(filter) ||
                    x.CategoryName.ToLower().Contains(filter.ToLower()) ||
                    x.Name.ToLower().Contains(filter.ToLower()) ||
                    x.Description.ToLower().Contains(filter.ToLower()));

                var resultVm = await request.PageSort.ApplyAsync(query);
                return resultVm;
            }
        }
    }
}
