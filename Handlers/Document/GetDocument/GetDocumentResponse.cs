namespace WebAPI.Handlers.Document.GetDocument
{
    public class GetDocumentResponse : Response
    {
        public Models.Document Document { get; set; } = new();
    }
}
