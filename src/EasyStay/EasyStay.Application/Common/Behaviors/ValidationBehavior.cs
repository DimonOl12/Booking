using FluentValidation;
using MediatR;

namespace EasyStay.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(
	IEnumerable<IValidator<TRequest>> validators
) : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest {

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
		var context = new ValidationContext<TRequest>(request);

		var validationTasks = validators.Select(v => v.ValidateAsync(context, cancellationToken));
		var validationResults = await Task.WhenAll(validationTasks);

		var failures = validationResults
			.SelectMany(result => result.Errors)
			.Where(failure => failure is not null)
			.ToList();

		if (failures.Count != 0)
			throw new ValidationException(failures);

		return await next();
	}
}
