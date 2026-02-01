namespace Easy.Cache.Abstractions
{
    /// <summary>
    /// Defines the contract for serializing and deserializing cache objects.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serializes an object to a byte array.
        /// </summary>
        byte[] Serialize<T>(T obj);

        /// <summary>
        /// Deserializes a byte array to an object.
        /// </summary>
        T? Deserialize<T>(byte[] data);
    }
}