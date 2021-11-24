using AutoMapper;
using StarterApp.Core.Areas.Issues.Entities;
using StarterApp.Core.Common.Mappings;

namespace StarterApp.Core.Areas.Issues.ViewModels
{
    public class IssueDetailVm : IMapFrom<IssueDetail>, IMapTo<IssueDetail>
    {
		public long ProductId { get; set; }
		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public long Id { get; set; }

        void IMapFrom<IssueDetail>.Mapping(Profile profile) => profile
            .CreateMap<IssueDetail, IssueDetailVm>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
    }
}
