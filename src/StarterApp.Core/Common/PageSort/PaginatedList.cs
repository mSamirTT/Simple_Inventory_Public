using System.Collections.Generic;

namespace StarterApp.Core.Common.PageSort
{
    public class PaginatedList<T> where T : class
    {
        public List<T> Payload { get; set; }
        public int Count { get; set; }
    }
}
