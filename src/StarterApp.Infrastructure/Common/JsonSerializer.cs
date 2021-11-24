using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace StarterApp.Infrastructure.Common
{
    [ExcludeFromCodeCoverage]
    public class JsonSerializer : IJsonSerializer
    {
        #region Fields

        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        #endregion

        #region IAmqpMessageBodyJsonSerializer

        public string Serialize(object objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize, _serializerSettings);
        }

        public T Deserialize<T>(string objectAsString)
        {
            return JsonConvert.DeserializeObject<T>(objectAsString, _serializerSettings);
        }

        #endregion
    }
}
