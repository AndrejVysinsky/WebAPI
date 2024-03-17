using ErrorOr;
using MediatR;

namespace WebAPI.Handlers.Document.UpdateDocument
{
    public class UpdateDocumentRequest : IRequest<ErrorOr<int>>
    {
        public int DocumentId { get; set; }
        public List<string> Tags { get; set; } = [];
        public string Data { get; set; } = string.Empty;
    }
}
