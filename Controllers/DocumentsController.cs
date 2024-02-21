using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Handlers;
using WebAPI.Handlers.Document.GetDocument;
using WebAPI.Handlers.Document.SaveDocument;

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
        public Task<Response> SaveDocument(SaveDocumentRequest request)
        {
            request.Document.OperationType = Models.OperationType.Add;
            return _mediator.Send(request);
        }

        [HttpPut]
        public Task<Response> UpdateDocument(SaveDocumentRequest request)
        {
            request.Document.OperationType = Models.OperationType.Update;
            return _mediator.Send(request);
        }

        [HttpGet("{id}")]
        public Task<string> GetDocument(int id)
        {
            return _mediator.Send(new GetDocumentRequest { Id = id });
        }
    }
}
