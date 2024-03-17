using FluentValidation;
using WebAPI.Common.Validation;
using WebAPI.Repositories;

namespace WebAPI.Handlers.Document.SaveDocument
{
    public class SaveDocumentRequestValidator : AbstractValidator<SaveDocumentRequest>
    {
        private readonly IDocumentRepository _repository;

        public SaveDocumentRequestValidator(IDocumentRepository documentRepository)
        {
            _repository = documentRepository;

            RuleFor(x => x.DocumentId)
                .NotEmpty()
                .WithMessage(DocumentErrors.IdCannotBeEmpty)
                .Must(NotExists)
                .WithMessage(DocumentErrors.IdAlreadyExists);

            RuleFor(x => x.Tags)
                .NotEmpty()
                .WithMessage(DocumentErrors.TagsCannotBeEmpty);

            RuleFor(x => x.Data)
                .NotEmpty()
                .WithMessage(DocumentErrors.DataCannotBeEmpty);
        }

        private bool NotExists(int id)
        {
            var document = _repository.GetDocument(id).Result;
            return document == null;
        }
    }
}
