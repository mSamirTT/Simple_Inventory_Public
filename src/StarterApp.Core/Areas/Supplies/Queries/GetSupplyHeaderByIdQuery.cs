using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Supplies.Entities;
using StarterApp.Core.Areas.Supplies.ViewModels;
using StarterApp.Core.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Supplies.Queries
{
    public class GetSupplyHeaderByIdQuery : IRequest<SupplyHeaderVm>
    {
        public int Id { get; set; }
        public GetSupplyHeaderByIdQuery(int Id)
        {
            this.Id = Id;
        }
    }
    public class GetSupplyHeaderByIdQueryHandler : BaseHandler<SupplyHeader>, IRequestHandler<GetSupplyHeaderByIdQuery, SupplyHeaderVm>
    {
        public async Task<SupplyHeaderVm> Handle(GetSupplyHeaderByIdQuery request, CancellationToken cancellationToken)
        {
            var vm = await _repository.Query
                .Include(x => x.SupplyDetails)
                .ThenInclude(x => x.Product)
                .ProjectTo<SupplyHeaderVm>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Id == request.Id);
            return vm;
        }
    }
}
