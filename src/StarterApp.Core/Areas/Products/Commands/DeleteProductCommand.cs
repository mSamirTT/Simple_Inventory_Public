using MediatR;
using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Common.Exceptions;
using StarterApp.Core.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Products.Commands
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeleteProductHandler : BaseHandler<Product>, IRequestHandler<DeleteProductCommand, bool>
    {
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(request.Id) ?? throw new NotFoundException(nameof(Product), request.Id);
            _repository.Delete(entity);
            var result = await _repository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0;
            return result;
        }
    }
}
