using FluentValidation;
using WebAPI.Repositories;

namespace WebAPI.Handlers.Document.Validation
{
    public class DocumentValidator : AbstractValidator<Domain.Document>
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentValidator(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;

            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleSet("OnGet", () =>
            {
                RuleFor(x => x.Id)
                    .NotEmpty()
                    .Must(Exists)
                    .WithMessage("Document id does not exist.");
            });

            RuleSet("OnSave", () =>
            {
                RuleFor(x => x.Id)
                    .NotEmpty()
                    .Must(NotExists)
                    .WithMessage("Document id already exists.");

                RuleFor(x => x.Tags).NotEmpty();
                RuleFor(x => x.Data).NotNull();
            });

            RuleSet("OnUpdate", () =>
            {
                RuleFor(x => x.Id)
                    .NotEmpty()
                    .Must(Exists)
                    .WithMessage("Document id does not exist.");
                RuleFor(x => x.Tags).NotEmpty();
                RuleFor(x => x.Data).NotNull();
            });
        }

        public bool Exists(int id)
        {
            return !NotExists(id);
        }

        private bool NotExists(int id)
        {
            var document = _documentRepository.GetDocument(id).Result;
            return document == null;
        }
    }
}
