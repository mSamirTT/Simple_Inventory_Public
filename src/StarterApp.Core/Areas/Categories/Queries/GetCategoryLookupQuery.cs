using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Categories.Entities;
using StarterApp.Core.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Categories.Queries
{
    public class GetCategoryLookupQuery : IRequest<List<Lookup>>
    {
        public GetCategoryLookupQuery()
        {
        }
        public class GetCategoryLookupQueryHandler : BaseHandler<Category>, IRequestHandler<GetCategoryLookupQuery, List<Lookup>>
        {
            public async Task<List<Lookup>> Handle(GetCategoryLookupQuery request, CancellationToken cancellationToken)
            {
                var query = _repository.Query
                    .Select(x => new Lookup
                    {
                        Id = x.Id,
                        Name = x.Name
                    });
                return await query.ToListAsync();
            }
        }
    }
}
