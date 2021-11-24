using MediatR;
using StarterApp.Core.Areas.Supplies.Entities;
using StarterApp.Core.Areas.Supplies.ViewModels;
using StarterApp.Core.Common.Mappings;
using StarterApp.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Supplies.Commands
{
    public class CreateSupplyHeaderCommand : IRequest<long>, IMapTo<SupplyHeader>
    {
		public int TransactionNumber { get; set; }
		public string Notes { get; set; }
		public DateTime SupplyDate { get; set; }
        public ICollection<SupplyDetailVm> SupplyDetails { get; set; }
    }

    public class CreateSupplyHeaderHandler : BaseHandler<SupplyHeader>, IRequestHandler<CreateSupplyHeaderCommand, long>
    {
        public async Task<long> Handle(CreateSupplyHeaderCommand request, CancellationToken cancellationToken)
        {
            var details = _mapper.Map<ICollection<SupplyDetail>>(request.SupplyDetails);
            var entity = new SupplyHeader(request.TransactionNumber, request.Notes, request.SupplyDate, details);
            _repository.Insert(entity);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}
