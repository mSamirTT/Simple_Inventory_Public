using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Supplies.Entities;
using StarterApp.Core.Common.Exceptions;
using StarterApp.Core.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Supplies.Commands
{
    public class DeleteSupplyHeaderCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeleteSupplyHeaderHandler : BaseHandler<SupplyHeader>, IRequestHandler<DeleteSupplyHeaderCommand, bool>
    {
        public async Task<bool> Handle(DeleteSupplyHeaderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Query
                .Include(x => x.SupplyDetails)
                .FirstOrDefaultAsync(x => x.Id == request.Id) ?? throw new NotFoundException(nameof(SupplyHeader), request.Id);
            entity.Delete(); // TODO: Hack(?): Raised domain event
            _repository.Delete(entity);
            var result = await _repository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0;
            return result;
        }
    }
}
