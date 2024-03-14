namespace WebAPI.Data
{
    public class Document
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public string? Tags { get; set; }
        public string? Data { get; set; }
    }
}
