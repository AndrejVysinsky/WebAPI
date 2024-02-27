using FluentValidation;

namespace WebAPI.Handlers.Document.GetDocument
{
    public class GetDocumentRequestValidator : AbstractValidator<GetDocumentRequest>
    {
        public GetDocumentRequestValidator(IValidator<Domain.Document> validator)
        {
            RuleFor(x => new Domain.Document() { Id = x.Id }).SetValidator(validator, "OnGet");
        }
    }
}
