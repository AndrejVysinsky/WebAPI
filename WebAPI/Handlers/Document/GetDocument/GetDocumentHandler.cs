using MediatR;
using WebAPI.Repositories;
using WebAPI.Serializers;

namespace WebAPI.Handlers.Document.GetDocument
{
    public class GetDocumentHandler : IRequestHandler<GetDocumentRequest, string>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IDictionary<string, ISerializer> _serializers;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetDocumentHandler(IDocumentRepository documentRepository, IEnumerable<ISerializer> serializers, IHttpContextAccessor httpContextAccessor)
        {
            _documentRepository = documentRepository;
            _serializers = serializers.ToDictionary(serializer => serializer.ContentType);
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> Handle(GetDocumentRequest request, CancellationToken cancellationToken)
        {
            var result = await _documentRepository.GetDocument(request.Id);

            var httpContext = _httpContextAccessor.HttpContext;
            var acceptHeader = httpContext?.Request.Headers.Accept;
            var contentType = string.Empty;
            if (acceptHeader.HasValue)
            {
                contentType = acceptHeader.Value.FirstOrDefault() ?? string.Empty;                
            }

            _serializers.TryGetValue(contentType, out var serializer);
            serializer ??= _serializers.First().Value;

            return serializer.Serialize(result);
        }
    }
}
