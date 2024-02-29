namespace WebAPI.Serializers
{
    public class JsonSerializer : ISerializer
    {
        public string ContentType => "application/json";

        public string Serialize<T>(T data)
        {
            if (data == null)
                return string.Empty;

            return System.Text.Json.JsonSerializer.Serialize(data);
        }
    }
}
