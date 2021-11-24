using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Issues.Entities;
using StarterApp.Core.Areas.Issues.ViewModels;
using StarterApp.Core.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Issues.Queries
{
    public class GetIssueHeaderByIdQuery : IRequest<IssueHeaderVm>
    {
        public int Id { get; set; }
        public GetIssueHeaderByIdQuery(int Id)
        {
            this.Id = Id;
        }
    }
    public class GetIssueHeaderByIdQueryHandler : BaseHandler<IssueHeader>, IRequestHandler<GetIssueHeaderByIdQuery, IssueHeaderVm>
    {
        public async Task<IssueHeaderVm> Handle(GetIssueHeaderByIdQuery request, CancellationToken cancellationToken)
        {
            var vm = await _repository.Query
                .Include(x => x.IssueDetails)
                .ThenInclude(x => x.Product)
                .ProjectTo<IssueHeaderVm>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Id == request.Id);
            return vm;
        }
    }
}
