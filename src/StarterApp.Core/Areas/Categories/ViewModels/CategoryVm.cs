using StarterApp.Core.Areas.Categories.Entities;
using StarterApp.Core.Common.Mappings;

namespace StarterApp.Core.Areas.Categories.ViewModels
{
    public class CategoryVm : IMapFrom<Category>
    {
		public string Name { get; set; }
		public string Thumbnail { get; set; }
		public long Id { get; set; }
		
    }
}
