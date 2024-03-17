using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Handlers.Document.GetDocument;
using WebAPI.Handlers.Document.SaveDocument;
using WebAPI.Handlers.Document.UpdateDocument;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ApiController
    {
        private readonly IMediator _mediator;

        public DocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> SaveDocument(SaveDocumentRequest request)
        {
            var result = await _mediator.Send(request);
            return result.Match(documentId => Ok(documentId), Problem);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDocument(UpdateDocumentRequest request)
        {
            var result = await _mediator.Send(request);
            return result.Match(documentId => Ok(documentId), Problem);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocument(int id)
        {
            var result = await _mediator.Send(new GetDocumentRequest { Id = id });
            return result.Match(Ok, Problem);
        }
    }
}
