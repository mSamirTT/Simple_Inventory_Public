namespace StarterApp.Infrastructure.Common
{
    public interface IJsonSerializer
    {
        public string Serialize(object objectToSerialize);
        public T Deserialize<T>(string objectAsString);
    }
}
