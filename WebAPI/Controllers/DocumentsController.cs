using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Handlers.Document.GetDocument;
using WebAPI.Handlers.Document.SaveDocument;
using WebAPI.Handlers.Document.UpdateDocument;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public Task<int> SaveDocument(SaveDocumentRequest request)
        {
            return _mediator.Send(request);
        }

        [HttpPut]
        public Task<int> UpdateDocument(UpdateDocumentRequest request)
        {
            return _mediator.Send(request);
        }

        [HttpGet("{id}")]
        public Task<GetDocumentResponse> GetDocument(int id)
        {
            return _mediator.Send(new GetDocumentRequest { Id = id });
        }
    }
}
