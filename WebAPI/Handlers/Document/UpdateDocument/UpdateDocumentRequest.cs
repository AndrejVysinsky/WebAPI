using MediatR;

namespace WebAPI.Handlers.Document.UpdateDocument
{
    public class UpdateDocumentRequest : IRequest<int>
    {
        public int DocumentId { get; set; }
        public List<string> Tags { get; set; } = [];
        public string Data { get; set; } = string.Empty;
    }
}
