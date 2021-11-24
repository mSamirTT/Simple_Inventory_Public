using MediatR;
using StarterApp.Core.Areas.Categories.Entities;
using StarterApp.Core.Common.Exceptions;
using StarterApp.Core.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Categories.Commands
{
    public class DeleteCategoryCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeleteCategoryHandler : BaseHandler<Category>, IRequestHandler<DeleteCategoryCommand, bool>
    {
        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(request.Id) ?? throw new NotFoundException(nameof(Category), request.Id);
            _repository.Delete(entity);
            var result = await _repository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0;
            return result;
        }
    }
}
