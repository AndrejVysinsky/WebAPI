namespace WebAPI.Handlers.Document.GetDocument
{
    public class GetDocumentResponse : Response
    {
        public Domain.Document Document { get; set; } = new();
    }
}
