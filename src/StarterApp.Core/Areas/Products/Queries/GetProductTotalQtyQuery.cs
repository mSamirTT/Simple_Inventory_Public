using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Areas.Products.ViewModels;
using StarterApp.Core.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Application.Areas.SupplyArea.Queries
{
    public class GetProductTotalQtyQuery : IRequest<ProductTotalQtyVm>
    {
        public long ProductId { get; set; }
        public DateTime UntilDate{ get; set; }
        public GetProductTotalQtyQuery(long productId, DateTime? untilDate = null)
        {
            ProductId = productId;
            UntilDate = untilDate ?? DateTime.Now;
        }
    }
    public class GetProductTotalQtyQueryHandler : BaseHandler<Product>, IRequestHandler<GetProductTotalQtyQuery, ProductTotalQtyVm>
    {
        public async Task<ProductTotalQtyVm> Handle(GetProductTotalQtyQuery request, CancellationToken cancellationToken)
        {
            var vm = await _repository.Query
                 .Include(x => x.IssueDetails)
                 .ThenInclude(x => x.IssueHeader)
                 .Include(x => x.SupplyDetails)
                 .ThenInclude(x => x.SupplyHeader)
                 .Where(x => x.Id == request.ProductId)
                 .Select(x => new ProductTotalQtyVm
                 {
                     ProductId = x.Id,
                     TotalQuantity = x.SupplyDetails.Where(x => x.SupplyHeader.SupplyDate <= request.UntilDate).Sum(q => q.Quantity) - 
                     x.IssueDetails.Where(x => x.IssueHeader.IssueDate <= request.UntilDate).Sum(q => q.Quantity)
                 })
                 .FirstOrDefaultAsync();
            return vm;
        }
    }
}
