using MediatR;
using WebAPI.Repositories;

namespace WebAPI.Handlers.Document.GetDocument
{
    public class GetDocumentHandler : IRequestHandler<GetDocumentRequest, GetDocumentResponse>
    {
        private readonly IDocumentRepository _documentRepository;

        public GetDocumentHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<GetDocumentResponse> Handle(GetDocumentRequest request, CancellationToken cancellationToken)
        {
            var result = await _documentRepository.GetDocument(request.Id);

            var response = new GetDocumentResponse()
            {
                Document = result ?? new()
            };
            return response;
        }
    }
}
