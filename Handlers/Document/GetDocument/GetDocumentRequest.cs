using MediatR;

namespace WebAPI.Handlers.Document.GetDocument
{
    public class GetDocumentRequest : IRequest<string>
    {
        public int Id { get; set; }
    }
}
