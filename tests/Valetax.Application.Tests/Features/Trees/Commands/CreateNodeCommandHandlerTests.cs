using FluentAssertions;
using NSubstitute;
using Xunit;
using Valetax.Application.Common.Interfaces;
using Valetax.Application.Features.Trees.Commands.CreateNode;
using Valetax.Domain.Entities;
using Valetax.Domain.Exceptions;
using Valetax.Domain.Repositories;
using Valetax.Domain.ValueObjects;

namespace Valetax.Application.Tests.Features.Trees.Commands;

public class CreateNodeCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly CreateNodeCommandHandler _handler;
    private readonly DateTime _now = new(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);

    public CreateNodeCommandHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _dateTimeProvider.UtcNow.Returns(_now);
        _handler = new CreateNodeCommandHandler(_unitOfWork, _dateTimeProvider);
    }

    [Fact]
    public async Task Handle_WhenTreeDoesNotExist_ShouldCreateTreeAndNode()
    {
        var command = new CreateNodeCommand("NewTree", null, "RootNode");
        _unitOfWork.Trees.GetByNameWithNodesAsync("NewTree", Arg.Any<CancellationToken>())
            .Returns((Tree?)null);
        _unitOfWork.Trees.AddAsync(Arg.Any<Tree>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask)
            .AndDoes(ci => SetTreeId(ci.Arg<Tree>(), 1));
        _unitOfWork.Nodes.ExistsSiblingWithNameAsync(
            Arg.Any<long>(), null, "RootNode", null, Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await _handler.Handle(command, CancellationToken.None);

        await _unitOfWork.Trees.Received(1).AddAsync(Arg.Any<Tree>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Nodes.Received(1).AddAsync(Arg.Any<Node>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received().CommitTransactionAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenParentNodeNotFound_ShouldThrowException()
    {
        var command = new CreateNodeCommand("ExistingTree", 999, "ChildNode");
        var existingTree = Tree.Create(TreeName.Create("ExistingTree"), _now);
        SetTreeId(existingTree, 1);

        _unitOfWork.Trees.GetByNameWithNodesAsync("ExistingTree", Arg.Any<CancellationToken>())
            .Returns(existingTree);
        _unitOfWork.Nodes.GetByIdAsync(999, Arg.Any<CancellationToken>())
            .Returns((Node?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NodeNotFoundException>();
        await _unitOfWork.Received().RollbackTransactionAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenDuplicateNodeName_ShouldThrowException()
    {
        var command = new CreateNodeCommand("ExistingTree", null, "DuplicateNode");
        var existingTree = Tree.Create(TreeName.Create("ExistingTree"), _now);
        SetTreeId(existingTree, 1);

        _unitOfWork.Trees.GetByNameWithNodesAsync("ExistingTree", Arg.Any<CancellationToken>())
            .Returns(existingTree);
        _unitOfWork.Nodes.ExistsSiblingWithNameAsync(
            1, null, "DuplicateNode", null, Arg.Any<CancellationToken>())
            .Returns(true);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DuplicateNodeNameException>();
    }

    private static void SetTreeId(Tree tree, long id)
    {
        var property = typeof(Tree).GetProperty("Id")!;
        property.SetValue(tree, id);
    }
}
