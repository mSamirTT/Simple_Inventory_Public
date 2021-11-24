using System;

namespace StarterApp.Core.Areas.Dashboard.ViewModels
{
    public class PerformanceVsLastWeekVm
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int? Count { get; set; }
        public int? CountLastWeek { get; set; }
        public DateTime? LastActionDate { get; set; }
    }
}
