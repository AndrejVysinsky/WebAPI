using FluentValidation;
using WebAPI.Common.Validation;
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
                .WithMessage(DocumentErrors.IdCannotBeEmpty)
                .Must(Exists)
                .WithMessage(DocumentErrors.IdNotFound)
                .WithErrorCode(ErrorCodes.NotFound);
        }

        private bool Exists(int id)
        {
            var document = _repository.GetDocument(id).Result;
            return document != null;
        }
    }
}
