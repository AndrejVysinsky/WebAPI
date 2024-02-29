using FluentValidation.Results;
using FluentValidation;
using MediatR;
using WebAPI.Handlers;

namespace WebAPI.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Response
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
                    var response = Activator.CreateInstance<TResponse>();
                    response.Faults = failures;
                    return response;
                }
            }

            return await next();
        }
    }
}
