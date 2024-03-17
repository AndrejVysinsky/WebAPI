using FluentValidation;
using WebAPI.Common.Validation;
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
                .WithMessage(DocumentErrors.IdCannotBeEmpty)
                .Must(Exists)
                .WithMessage(DocumentErrors.IdNotFound)
                .WithErrorCode(ErrorCodes.NotFound);

            RuleFor(x => x.Tags)
                .NotEmpty()
                .WithMessage(DocumentErrors.TagsCannotBeEmpty);

            RuleFor(x => x.Data)
                .NotEmpty()
                .WithMessage(DocumentErrors.DataCannotBeEmpty);
        }

        private bool Exists(int id)
        {
            var document = _repository.GetDocument(id).Result;
            return document != null;
        }
    }
}
