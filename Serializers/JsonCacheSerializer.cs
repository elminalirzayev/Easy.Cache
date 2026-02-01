using Easy.Cache.Abstractions;
using System.Text.Json;

namespace Easy.Cache.Serializers
{
    /// <summary>
    /// Implementation of ISerializer using System.Text.Json.
    /// </summary>
    public class JsonCacheSerializer : ISerializer
    {
        /// <inheritdoc />
        public byte[] Serialize<T>(T obj)
        {
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }

        /// <inheritdoc />
        public T? Deserialize<T>(byte[] data)
        {
            return JsonSerializer.Deserialize<T>(data);
        }
    }
}