using MediatR;
using WebAPI.Domain;

namespace WebAPI.Handlers.Document.SaveDocument
{
    public class SaveDocumentRequest : IRequest<Response>
    {
        public Editable<Domain.Document> Document { get; set; } = new();
    }
}
