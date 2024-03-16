using FluentValidation;
using WebAPI.Repositories;

namespace WebAPI.Handlers.Document.GetDocument
{
    public class GetDocumentRequestValidator : AbstractValidator<GetDocumentRequest>
    {
        private readonly IDocumentRepository _repository;

        public GetDocumentRequestValidator(IDocumentRepository documentRepository)
        {
            _repository = documentRepository;

            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(Exists)
                .WithMessage("Document id does not exist.");
        }

        private bool Exists(int id)
        {
            var document = _repository.GetDocument(id).Result;
            return document != null;
        }
    }
}
