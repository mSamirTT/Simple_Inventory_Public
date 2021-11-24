using AutoMapper;
using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Common.Mappings;

namespace StarterApp.Core.Areas.Products.ViewModels
{
    public class ProductVm : IMapFrom<Product>
    {
		public string Name { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public string Thumbnail { get; set; }
		public long CategoryId { get; set; }
		public string CategoryName { get; set; }
		public int Quantity { get; set; }
		public long Id { get; set; }
		public void Mapping(Profile profile) => profile
			.CreateMap<Product, ProductVm>()
			.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
	}
}
