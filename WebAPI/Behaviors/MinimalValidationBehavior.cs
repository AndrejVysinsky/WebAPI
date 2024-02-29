using FluentValidation;
using MediatR;

namespace WebAPI.Behaviors
{
    public class MinimalValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<string>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public MinimalValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                foreach (var validator in _validators)
                {
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);
                    if (validationResult.IsValid == false)
                    {
                        return default;
                    }
                }
            }

            return await next();
        }
    }
}
