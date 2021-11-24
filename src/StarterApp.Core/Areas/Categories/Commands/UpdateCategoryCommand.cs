using MediatR;
using StarterApp.Core.Areas.Categories.Entities;
using StarterApp.Core.Common.Exceptions;
using StarterApp.Core.Common.Mappings;
using StarterApp.Core.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Categories.Commands
{
    public class UpdateCategoryCommand : IRequest<bool>, IMapTo<Category>
    {
		public string Name { get; set; }
		public string Thumbnail { get; set; }
		public long Id { get; set; }
		
    }

    public class UpdateCategoryHandler : BaseHandler<Category>, IRequestHandler<UpdateCategoryCommand, bool>
    {
        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(request.Id) ?? throw new NotFoundException(nameof(Category), request.Id);
            _mapper.Map(request, entity);
            var result = await _repository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0;
            return result;
        }
    }
}
