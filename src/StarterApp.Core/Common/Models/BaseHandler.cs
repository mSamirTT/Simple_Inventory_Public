using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StarterApp.Core.Common.Interfaces;

namespace StarterApp.Core.Common.Models
{
    public class BaseHandler<T> where T : BaseEntity
    {
        protected readonly IRepository<T> _repository;
        protected readonly IMapper _mapper;
        protected readonly IMediator _mediator;

        public BaseHandler()
        {
            _repository = ServiceProviderFactory.ServiceProvider?.GetService<IRepository<T>>();
            _mapper = ServiceProviderFactory.ServiceProvider?.GetService<IMapper>();
            _mediator = ServiceProviderFactory.ServiceProvider?.GetService<IMediator>();
        }
    }
}