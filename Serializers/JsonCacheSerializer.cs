using Easy.Cache.Abstractions;
using System.Text.Json;

namespace Easy.Cache.Serializers
{
    public class JsonCacheSerializer : ISerializer
    {
        public byte[] Serialize<T>(T obj)
        {
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }

        public T? Deserialize<T>(byte[] data)
        {
            return JsonSerializer.Deserialize<T>(data);
        }
    }
}
