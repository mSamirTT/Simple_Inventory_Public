using AutoMapper;
using StarterApp.Core.Areas.Supplies.Entities;
using StarterApp.Core.Common.Mappings;

namespace StarterApp.Core.Areas.Supplies.ViewModels
{
    public class SupplyDetailVm : IMapFrom<SupplyDetail>, IMapTo<SupplyDetail>
    {
		public long ProductId { get; set; }
		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public long Id { get; set; }

        void IMapFrom<SupplyDetail>.Mapping(Profile profile) => profile
            .CreateMap<SupplyDetail, SupplyDetailVm>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
    }
}
