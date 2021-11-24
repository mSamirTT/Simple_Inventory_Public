using AutoMapper.QueryableExtensions;
using MediatR;
using StarterApp.Core.Areas.Categories.Entities;
using StarterApp.Core.Areas.Categories.ViewModels;
using StarterApp.Core.Common.Models;
using StarterApp.Core.Common.PageSort;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Categories.Queries
{
    public class GetCategoryListQuery : IRequest<PaginatedList<CategoryVm>>
    {
        public readonly PageSort<CategoryVm> PageSort;
        public GetCategoryListQuery(PageSort<CategoryVm> PageSort)
        {
            this.PageSort = PageSort;
        }

        public class GetCategoryListQueryHandler : BaseHandler<Category>, IRequestHandler<GetCategoryListQuery, PaginatedList<CategoryVm>>
        {
            public async Task<PaginatedList<CategoryVm>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
            {
                var query = _repository.Query
                    .ProjectTo<CategoryVm>(_mapper.ConfigurationProvider);
                var resultVm = await request.PageSort.ApplyAsync(query);
                return resultVm;
            }
        }
    }
}
