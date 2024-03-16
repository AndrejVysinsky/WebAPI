using MediatR;
using WebAPI.Repositories;

namespace WebAPI.Handlers.Document.UpdateDocument
{
    public class UpdateDocumentHandler : IRequestHandler<UpdateDocumentRequest, int>
    {
        private readonly IDocumentRepository _documentRepository;

        public UpdateDocumentHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<int> Handle(UpdateDocumentRequest request, CancellationToken cancellationToken)
        {
            var document = await _documentRepository.GetDocument(request.DocumentId);

            document.Tags = string.Join(",", request.Tags);
            document.Data = request.Data;

            await _documentRepository.UpdateDocument(document);

            return document.DocumentId;
        }
    }
}
