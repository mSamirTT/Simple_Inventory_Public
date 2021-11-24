using StarterApp.Core.Areas.Supplies.Entities;
using StarterApp.Core.Common.Mappings;
using System;
using System.Collections.Generic;

namespace StarterApp.Core.Areas.Supplies.ViewModels
{
    public class SupplyHeaderVm : IMapFrom<SupplyHeader>
    {
		public int TransactionNumber { get; set; }
		public string Notes { get; set; }
		public DateTime SupplyDate { get; set; }
		public long Id { get; set; }
		public ICollection<SupplyDetailVm> SupplyDetails { get; set; }
	}
}
