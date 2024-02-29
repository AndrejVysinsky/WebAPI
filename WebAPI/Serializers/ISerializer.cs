namespace WebAPI.Serializers
{
    public interface ISerializer
    {
        string ContentType { get; }
        string Serialize<T>(T data);
    }
}
