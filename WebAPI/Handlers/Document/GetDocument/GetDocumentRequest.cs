using MediatR;

namespace WebAPI.Handlers.Document.GetDocument
{
    public class GetDocumentRequest : IRequest<GetDocumentResponse>
    {
        public int Id { get; init; }
    }
}
