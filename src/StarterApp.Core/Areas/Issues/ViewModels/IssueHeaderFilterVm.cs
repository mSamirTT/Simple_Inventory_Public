using System;

namespace StarterApp.Core.Areas.Issues.ViewModels
{
	public class IssueHeaderFilterVm
	{
		public string SearchText { get; set; }
		public DateTime? IssueDateTo { get; set; }
		public DateTime? IssueDateFrom { get; set; }
	}
}
