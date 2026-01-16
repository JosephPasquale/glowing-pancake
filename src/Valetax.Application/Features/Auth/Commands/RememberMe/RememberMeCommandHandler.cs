using MediatR;
using Valetax.Application.Common.Interfaces;
using Valetax.Domain.Entities;
using Valetax.Domain.Repositories;

namespace Valetax.Application.Features.Auth.Commands.RememberMe;

public sealed class RememberMeCommandHandler : IRequestHandler<RememberMeCommand, TokenDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RememberMeCommandHandler(
        IUnitOfWork unitOfWork,
        IJwtTokenGenerator jwtTokenGenerator,
        IDateTimeProvider dateTimeProvider)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<TokenDto> Handle(RememberMeCommand request, CancellationToken cancellationToken)
    {
        var now = _dateTimeProvider.UtcNow;
        var user = await _unitOfWork.Users.GetByUniqueCodeAsync(request.Code, cancellationToken);

        if (user is null)
        {
            user = User.Create(request.Code, now);
            await _unitOfWork.Users.AddAsync(user, cancellationToken);
        }

        user.RecordLogin(now);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new TokenDto(token);
    }
}
