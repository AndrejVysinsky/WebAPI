using MediatR;
using WebAPI.Models;

namespace WebAPI.Handlers.Document.SaveDocument
{
    public class SaveDocumentRequest : IRequest<Response>
    {
        public Editable<Models.Document> Document { get; set; } = new();
    }
}
