using FluentValidation;
using WebAPI.Models;

namespace WebAPI.Handlers.Document.SaveDocument
{
    public class SaveDocumentRequestValidator : AbstractValidator<SaveDocumentRequest>
    {
        public SaveDocumentRequestValidator(IValidator<Models.Document> validator)
        {
            RuleFor(x => x.Document.Object).SetValidator(validator, "OnSave").When(x => x.Document.OperationType == OperationType.Add);
            RuleFor(x => x.Document.Object).SetValidator(validator, "OnUpdate").When(x => x.Document.OperationType == OperationType.Update);
        }
    }
}
