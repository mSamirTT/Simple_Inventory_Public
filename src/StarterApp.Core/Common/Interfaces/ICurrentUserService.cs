namespace StarterApp.Core.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        public string GetClientSessionId();
        public string GetClientCorrelationId();
    }
}
