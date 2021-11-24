using StarterApp.Core.Areas.Issues.Entities;
using StarterApp.Core.Common.Mappings;
using System;
using System.Collections.Generic;

namespace StarterApp.Core.Areas.Issues.ViewModels
{
    public class IssueHeaderVm : IMapFrom<IssueHeader>
    {
		public int TransactionNumber { get; set; }
		public string Notes { get; set; }
		public DateTime IssueDate { get; set; }
		public long Id { get; set; }
		public ICollection<IssueDetailVm> IssueDetails { get; set; }
	}
}
