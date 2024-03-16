using FluentValidation;
using WebAPI.Repositories;

namespace WebAPI.Handlers.Document.UpdateDocument
{
    public class UpdateDocumentRequestValidator : AbstractValidator<UpdateDocumentRequest>
    {
        private readonly IDocumentRepository _repository;

        public UpdateDocumentRequestValidator(IDocumentRepository documentRepository)
        {
            _repository = documentRepository;

            RuleFor(x => x.DocumentId)
                .NotEmpty()
                .Must(Exists)
                .WithMessage("Document id does not exist.");

            RuleFor(x => x.Tags).NotEmpty();

            RuleFor(x => x.Data).NotEmpty();
        }

        private bool Exists(int id)
        {
            var document = _repository.GetDocument(id).Result;
            return document != null;
        }
    }
}
