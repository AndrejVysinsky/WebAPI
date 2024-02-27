namespace WebAPI.Repositories
{
    public class JsonDocumentSettings
    {
        public const string Key = "JsonDocument";

        public required string FileName { get; set; }
        public required string StoragePath { get; set; }
    }
}
