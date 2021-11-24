using System;

namespace StarterApp.Core.Areas.Dashboard.ViewModels
{
    public class RecentIssuingSupplyingVm
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public int TotalQty { get; set; }
    }
}
