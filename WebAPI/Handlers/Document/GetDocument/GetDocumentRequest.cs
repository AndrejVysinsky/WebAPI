using ErrorOr;
using MediatR;

namespace WebAPI.Handlers.Document.GetDocument
{
    public class GetDocumentRequest : IRequest<ErrorOr<GetDocumentResponse>>
    {
        public int Id { get; init; }
    }
}
