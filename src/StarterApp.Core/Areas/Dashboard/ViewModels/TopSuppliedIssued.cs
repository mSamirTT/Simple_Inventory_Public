using System.Collections.Generic;

namespace StarterApp.Core.Areas.Dashboard.ViewModels
{
    public class TopQtyProduct
    {
        public int? TotalQty { get; set; }
        public List<TopQtyProductItem> Items { get; set; }
    }

    public class TopQtyProductItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int? Qty { get; set; }
    }
}
