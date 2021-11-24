using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Areas.Products.ViewModels;
using StarterApp.Core.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Products.Queries
{
    public class GetProductByIdQuery : IRequest<ProductVm>
    {
        public int Id { get; set; }
        public GetProductByIdQuery(int Id)
        {
            this.Id = Id;
        }
    }
    public class GetProductByIdQueryHandler : BaseHandler<Product>, IRequestHandler<GetProductByIdQuery, ProductVm>
    {
        public async Task<ProductVm> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var vm = await _repository.Query
                .ProjectTo<ProductVm>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Id == request.Id);
            return vm;
        }
    }
}
