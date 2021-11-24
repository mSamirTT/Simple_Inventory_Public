using MediatR;
using StarterApp.Core.Areas.Categories.Entities;
using StarterApp.Core.Common.Mappings;
using StarterApp.Core.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<long>, IMapTo<Category>
    {
		public string Name { get; set; }
		public string Thumbnail { get; set; }
		
    }

    public class CreateCategoryHandler : BaseHandler<Category>, IRequestHandler<CreateCategoryCommand, long>
    {
        public async Task<long> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Category>(request);
            _repository.Insert(entity);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }

}
