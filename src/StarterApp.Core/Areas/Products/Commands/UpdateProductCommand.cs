using MediatR;
using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Common.Exceptions;
using StarterApp.Core.Common.Mappings;
using StarterApp.Core.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Products.Commands
{
    public class UpdateProductCommand : IRequest<bool>, IMapTo<Product>
    {
		public string Name { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public string Thumbnail { get; set; }
		public long CategoryId { get; set; }
		public long Id { get; set; }
		
    }

    public class UpdateProductHandler : BaseHandler<Product>, IRequestHandler<UpdateProductCommand, bool>
    {
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(request.Id) ?? throw new NotFoundException(nameof(Product), request.Id);
            _mapper.Map(request, entity);
            var result = await _repository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0;
            return result;
        }
    }
}
