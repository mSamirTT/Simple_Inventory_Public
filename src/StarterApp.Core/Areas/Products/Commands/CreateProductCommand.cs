using MediatR;
using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Common.Mappings;
using StarterApp.Core.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Products.Commands
{
    public class CreateProductCommand : IRequest<long>, IMapTo<Product>
    {
		public string Name { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public string Thumbnail { get; set; }
		public long CategoryId { get; set; }
		
    }

    public class CreateProductHandler : BaseHandler<Product>, IRequestHandler<CreateProductCommand, long>
    {
        public async Task<long> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Product>(request);
            _repository.Insert(entity);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }

}
