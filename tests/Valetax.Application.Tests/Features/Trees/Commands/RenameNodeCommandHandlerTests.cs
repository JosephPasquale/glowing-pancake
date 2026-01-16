using FluentAssertions;
using NSubstitute;
using Xunit;
using Valetax.Application.Common.Interfaces;
using Valetax.Application.Features.Trees.Commands.RenameNode;
using Valetax.Domain.Entities;
using Valetax.Domain.Exceptions;
using Valetax.Domain.Repositories;
using Valetax.Domain.ValueObjects;

namespace Valetax.Application.Tests.Features.Trees.Commands;

public class RenameNodeCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly RenameNodeCommandHandler _handler;
    private readonly DateTime _now = new(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);

    public RenameNodeCommandHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _dateTimeProvider.UtcNow.Returns(_now);
        _handler = new RenameNodeCommandHandler(_unitOfWork, _dateTimeProvider);
    }

    [Fact]
    public async Task Handle_WhenNodeExists_ShouldRenameNode()
    {
        var command = new RenameNodeCommand(1, "NewName");
        var existingNode = Node.Create(NodeName.Create("OldName"), 1, null, _now);
        SetNodeId(existingNode, 1);

        _unitOfWork.Nodes.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(existingNode);
        _unitOfWork.Nodes.ExistsSiblingWithNameAsync(
            1, null, "NewName", 1L, Arg.Any<CancellationToken>())
            .Returns(false);

        await _handler.Handle(command, CancellationToken.None);

        existingNode.Name.Should().Be("NewName");
        _unitOfWork.Nodes.Received(1).Update(existingNode);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenNodeNotFound_ShouldThrowException()
    {
        var command = new RenameNodeCommand(999, "NewName");
        _unitOfWork.Nodes.GetByIdAsync(999, Arg.Any<CancellationToken>())
            .Returns((Node?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NodeNotFoundException>();
    }

    [Fact]
    public async Task Handle_WhenDuplicateName_ShouldThrowException()
    {
        var command = new RenameNodeCommand(1, "ExistingName");
        var existingNode = Node.Create(NodeName.Create("OldName"), 1, null, _now);
        SetNodeId(existingNode, 1);

        _unitOfWork.Nodes.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(existingNode);
        _unitOfWork.Nodes.ExistsSiblingWithNameAsync(
            1, null, "ExistingName", 1L, Arg.Any<CancellationToken>())
            .Returns(true);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DuplicateNodeNameException>();
    }

    private static void SetNodeId(Node node, long id)
    {
        var property = typeof(Node).GetProperty("Id")!;
        property.SetValue(node, id);
    }
}
