using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Categories.Entities;
using StarterApp.Core.Areas.Categories.ViewModels;
using StarterApp.Core.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Categories.Queries
{
    public class GetCategoryByIdQuery : IRequest<CategoryVm>
    {
        public int Id { get; set; }
        public GetCategoryByIdQuery(int Id)
        {
            this.Id = Id;
        }
    }
    public class GetCategoryByIdQueryHandler : BaseHandler<Category>, IRequestHandler<GetCategoryByIdQuery, CategoryVm>
    {
        public async Task<CategoryVm> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var vm = await _repository.Query
                .ProjectTo<CategoryVm>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Id == request.Id);
            return vm;
        }
    }
}
