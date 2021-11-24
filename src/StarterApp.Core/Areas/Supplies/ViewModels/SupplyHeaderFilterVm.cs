using System;

namespace StarterApp.Core.Areas.Supplies.ViewModels
{
	public class SupplyHeaderFilterVm
	{
		public string SearchText { get; set; }
		public DateTime? SupplyDateTo { get; set; }
		public DateTime? SupplyDateFrom { get; set; }
	}
}
