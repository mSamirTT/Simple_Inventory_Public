using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Supplies.Entities;
using StarterApp.Core.Areas.Supplies.ViewModels;
using StarterApp.Core.Common.Exceptions;
using StarterApp.Core.Common.Mappings;
using StarterApp.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Supplies.Commands
{
    public class UpdateSupplyHeaderCommand : IRequest<bool>, IMapTo<SupplyHeader>
    {

        public int TransactionNumber { get; set; }
        public string Notes { get; set; }
        public DateTime SupplyDate { get; set; }
        public long Id { get; set; }
        public ICollection<SupplyDetailVm> SupplyDetails { get; set; }

        // Ignore Collection when mapping
        public void Mapping(Profile profile) => profile.CreateMap<UpdateSupplyHeaderCommand, SupplyHeader>()
            .ForMember(x => x.SupplyDetails, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }

    public class UpdateSupplyHeaderHandler : BaseHandler<SupplyHeader>, IRequestHandler<UpdateSupplyHeaderCommand, bool>
    {
        public async Task<bool> Handle(UpdateSupplyHeaderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Entity
                .Include(x => x.SupplyDetails)
                .FirstOrDefaultAsync(x => x.Id == request.Id) ?? throw new NotFoundException(nameof(UpdateSupplyHeaderCommand), request.Id);

            // Map Header Values
            _mapper.Map(request, entity);

            // Update Collection values
            var newCollection = _mapper.Map<ICollection<SupplyDetail>>(request.SupplyDetails);
            entity.UpdateChildCollection(entity.SupplyDetails, newCollection);

            var result = await _repository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0;
            return result;
        }
    }
}
