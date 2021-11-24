using MediatR;
using StarterApp.Core.Areas.Issues.Entities;
using StarterApp.Core.Areas.Issues.ViewModels;
using StarterApp.Core.Common.Mappings;
using StarterApp.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Issues.Commands
{
    public class CreateIssueHeaderCommand : IRequest<long>, IMapTo<IssueHeader>
    {
		public int TransactionNumber { get; set; }
		public string Notes { get; set; }
		public DateTime IssueDate { get; set; }
        public ICollection<IssueDetailVm> IssueDetails { get; set; }
    }

    public class CreateIssueHeaderHandler : BaseHandler<IssueHeader>, IRequestHandler<CreateIssueHeaderCommand, long>
    {
        public async Task<long> Handle(CreateIssueHeaderCommand request, CancellationToken cancellationToken)
        {
            var details = _mapper.Map<ICollection<IssueDetail>>(request.IssueDetails);
            var entity = new IssueHeader(request.TransactionNumber, request.Notes, request.IssueDate, details); 
            _repository.Insert(entity);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}
