using FluentAssertions;
using FluentValidation;
using Xunit;
using FluentValidation.Results;
using MediatR;
using Valetax.Application.Common.Behaviors;
using Valetax.Domain.Exceptions;

namespace Valetax.Application.Tests.Common.Behaviors;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_WithNoValidators_ShouldCallNext()
    {
        var validators = Enumerable.Empty<IValidator<TestRequest>>();
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);
        var request = new TestRequest();
        var expectedResponse = new TestResponse();
        var called = false;
        RequestHandlerDelegate<TestResponse> next = (ct) =>
        {
            called = true;
            return Task.FromResult(expectedResponse);
        };

        var result = await behavior.Handle(request, next, CancellationToken.None);

        result.Should().Be(expectedResponse);
        called.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldCallNext()
    {
        var validator = new TestRequestValidator(isValid: true);
        var validators = new[] { validator };
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);
        var request = new TestRequest();
        var expectedResponse = new TestResponse();
        RequestHandlerDelegate<TestResponse> next = (ct) => Task.FromResult(expectedResponse);

        var result = await behavior.Handle(request, next, CancellationToken.None);

        result.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_ShouldThrowSecureException()
    {
        var validator = new TestRequestValidator(isValid: false);
        var validators = new[] { validator };
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);
        var request = new TestRequest();
        RequestHandlerDelegate<TestResponse> next = (ct) => Task.FromResult(new TestResponse());

        var act = () => behavior.Handle(request, next, CancellationToken.None);

        await act.Should().ThrowAsync<SecureException>()
            .WithMessage("*Validation error*");
    }

    private record TestRequest : IRequest<TestResponse>;
    private record TestResponse;

    private sealed class TestRequestValidator : AbstractValidator<TestRequest>
    {
        public TestRequestValidator(bool isValid)
        {
            if (!isValid)
            {
                RuleFor(x => x).Must(_ => false).WithMessage("Validation error");
            }
        }
    }
}
