using MediatR;
using WebAPI.Repositories;

namespace WebAPI.Handlers.Document.SaveDocument
{
    public class SaveDocumentHandler : IRequestHandler<SaveDocumentRequest, Response>
    {
        private readonly IDocumentRepository _documentRepository;

        public SaveDocumentHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<Response> Handle(SaveDocumentRequest request, CancellationToken cancellationToken)
        {
            if (request.Document.OperationType == Models.OperationType.Add)
            {
                await _documentRepository.SaveDocument(request.Document.Object);
            }
            else
            {
                await _documentRepository.UpdateDocument(request.Document.Object);
            }

            var response = new Response();
            return response;
        }
    }
}
