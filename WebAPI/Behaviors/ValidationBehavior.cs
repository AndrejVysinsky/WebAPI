using FluentValidation.Results;
using FluentValidation;
using MediatR;
using ErrorOr;
using WebAPI.Common.Validation;

namespace WebAPI.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            List<ValidationFailure> failures = [];

            if (_validators.Any())
            {
                foreach (var validator in _validators)
                {
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);
                    if (validationResult.IsValid == false)
                    {
                        failures.AddRange(validationResult.Errors);
                    }
                }

                if (failures.Count != 0)
                {
                    var errors = failures.Select(error =>
                    {
                        return error.ErrorCode switch
                        {
                            ErrorCodes.NotFound => Error.NotFound(error.PropertyName, error.ErrorMessage),
                            _ => Error.Validation(error.PropertyName, error.ErrorMessage)
                        };
                    }).ToList();

                    return (dynamic)errors;
                }
            }

            return await next();
        }
    }
}
