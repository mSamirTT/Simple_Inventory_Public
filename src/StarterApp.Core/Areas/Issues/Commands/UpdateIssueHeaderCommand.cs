using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Application.Areas.SupplyArea.Queries;
using StarterApp.Core.Areas.Issues.Entities;
using StarterApp.Core.Areas.Issues.ViewModels;
using StarterApp.Core.Common.Exceptions;
using StarterApp.Core.Common.Mappings;
using StarterApp.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Issues.Commands
{
    public class UpdateIssueHeaderCommand : IRequest<bool>, IMapTo<IssueHeader>
    {

        public int TransactionNumber { get; set; }
        public string Notes { get; set; }
        public DateTime IssueDate { get; set; }
        public long Id { get; set; }
        public ICollection<IssueDetailVm> IssueDetails { get; set; }

        // Ignore Collection when mapping
        public void Mapping(Profile profile) => profile.CreateMap<UpdateIssueHeaderCommand, IssueHeader>()
            .ForMember(x => x.IssueDetails, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }

    public class UpdateIssueHeaderHandler : BaseHandler<IssueHeader>, IRequestHandler<UpdateIssueHeaderCommand, bool>
    {
        public async Task<bool> Handle(UpdateIssueHeaderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Entity
                .Include(x => x.IssueDetails)
                .FirstOrDefaultAsync(x => x.Id == request.Id) ?? throw new NotFoundException(nameof(UpdateIssueHeaderCommand), request.Id);

            // Map Header Values
            _mapper.Map(request, entity);

            // Update Collection values
            var newCollection = _mapper.Map<ICollection<IssueDetail>>(request.IssueDetails);
            entity.UpdateChildCollection(newCollection);

            var result = await _repository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0;
            return result;
        }
    }
}
