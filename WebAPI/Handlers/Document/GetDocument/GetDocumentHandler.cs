using ErrorOr;
using MediatR;
using WebAPI.Repositories;

namespace WebAPI.Handlers.Document.GetDocument
{
    public class GetDocumentHandler : IRequestHandler<GetDocumentRequest, ErrorOr<GetDocumentResponse>>
    {
        private readonly IDocumentRepository _documentRepository;

        public GetDocumentHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<ErrorOr<GetDocumentResponse>> Handle(GetDocumentRequest request, CancellationToken cancellationToken)
        {
            var result = await _documentRepository.GetDocument(request.Id);

            return new GetDocumentResponse()
            {
                DocumentId = result!.DocumentId,
                Tags = result.Tags?.Split(",").ToList() ?? [],
                Data = result.Data ?? string.Empty
            };
        }
    }
}
