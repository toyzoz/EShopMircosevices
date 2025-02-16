using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var validationContext = new ValidationContext<TRequest>(request);
        var validationResults =
            await Task.WhenAll(validators.Select(v => v.ValidateAsync(validationContext, cancellationToken)));

        var validationFailures = validationResults
            .Where(r => r.Errors.Count != 0)
            .SelectMany(r => r.Errors).ToList();

        if (validationFailures.Count != 0)
        {
            throw new ValidationException(validationFailures);
        }

        return await next();
    }
}

 