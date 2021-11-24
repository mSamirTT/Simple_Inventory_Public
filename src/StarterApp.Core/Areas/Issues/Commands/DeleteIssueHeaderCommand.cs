using MediatR;
using StarterApp.Core.Areas.Issues.Entities;
using StarterApp.Core.Common.Exceptions;
using StarterApp.Core.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Issues.Commands
{
    public class DeleteIssueHeaderCommand : IRequest<bool>
    {
        public long Id { get; set; }
    }

    public class DeleteIssueHeaderHandler : BaseHandler<IssueHeader>, IRequestHandler<DeleteIssueHeaderCommand, bool>
    {
        public async Task<bool> Handle(DeleteIssueHeaderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(request.Id) ?? throw new NotFoundException(nameof(IssueHeader), request.Id);
            _repository.Delete(entity);
            var result = await _repository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0;
            return result;
        }
    }
}
