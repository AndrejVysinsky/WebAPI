using ErrorOr;
using MediatR;
using WebAPI.Repositories;

namespace WebAPI.Handlers.Document.SaveDocument
{
    public class SaveDocumentHandler : IRequestHandler<SaveDocumentRequest, ErrorOr<int>>
    {
        private readonly IDocumentRepository _documentRepository;

        public SaveDocumentHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<ErrorOr<int>> Handle(SaveDocumentRequest request, CancellationToken cancellationToken)
        {
            var document = new Data.Document()
            {
                DocumentId = request.DocumentId,
                Tags = string.Join(",", request.Tags),
                Data = request.Data.ToString()
            };

            await _documentRepository.SaveDocument(document);

            return document.DocumentId;
        }
    }
}
