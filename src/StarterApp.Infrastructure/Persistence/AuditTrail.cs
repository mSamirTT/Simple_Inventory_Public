using System;

namespace StarterApp.Infrastructure.Persistence
{
    public class AuditTrail
    {
        public long Id { get; set; }
        public string Table { get; set; }
        public string Column { get; set; }
        public string Action { get; set; }
        public string Value { get; set; }
        public string NewValue { get; set; }
        public DateTime Date { get; set; }
        public string User { get; set; }

    }
}
