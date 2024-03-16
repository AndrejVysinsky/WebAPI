namespace WebAPI.Handlers.Document.GetDocument
{
    public class GetDocumentResponse
    {
        public int DocumentId { get; init; }
        public List<string> Tags { get; init; } = [];
        public string Data { get; init; } = string.Empty;
    }
}
