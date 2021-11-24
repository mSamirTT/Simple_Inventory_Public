using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Products.Queries
{
    public class GetProductLookupQuery : IRequest<List<Lookup>>
    {
        public GetProductLookupQuery()
        {
        }
        public class GetProductLookupQueryHandler : BaseHandler<Product>, IRequestHandler<GetProductLookupQuery, List<Lookup>>
        {
            public async Task<List<Lookup>> Handle(GetProductLookupQuery request, CancellationToken cancellationToken)
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
