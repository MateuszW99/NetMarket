using System.Collections.Generic;
using System.Net.Http;

namespace Application.IntegrationTests.Helpers
{
    public interface IObjectDeserializer<T> where T : class
    {
        T Deserialize(HttpContent content);
        List<T> DeserializeToList(HttpContent content);
    }
}