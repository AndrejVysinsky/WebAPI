using FluentValidation;

namespace WebAPI.Handlers.Document.GetDocument
{
    public class GetDocumentRequestValidator : AbstractValidator<GetDocumentRequest>
    {
        public GetDocumentRequestValidator(IValidator<Models.Document> validator)
        {
            RuleFor(x => new Models.Document() { Id = x.Id }).SetValidator(validator, "OnGet");
        }
    }
}
