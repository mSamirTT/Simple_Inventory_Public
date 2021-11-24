using StarterApp.Core.Common;

namespace StarterApp.Core.Areas.Dashboard.Entities
{
    public class LogDashboard : AuditableEntity
    {
        public LogDashboard()
        {
            // Required by EF
        }
        public LogDashboard(string logDateTimeZone)
        {
            LogDateTimeZone = logDateTimeZone;
        }
        public string LogDateTimeZone { get; set; }
    }
}
