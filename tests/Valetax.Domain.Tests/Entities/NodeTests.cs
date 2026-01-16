using FluentAssertions;
using Valetax.Domain.Entities;
using Xunit;
using Valetax.Domain.ValueObjects;

namespace Valetax.Domain.Tests.Entities;

public class NodeTests
{
    private readonly DateTime _now = new(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Create_WithValidParameters_ShouldCreateNode()
    {
        var nodeName = NodeName.Create("TestNode");

        var node = Node.Create(nodeName, 1, null, _now);

        node.Name.Should().Be("TestNode");
        node.TreeId.Should().Be(1);
        node.ParentId.Should().BeNull();
        node.CreatedAt.Should().Be(_now);
        node.IsRoot.Should().BeTrue();
    }

    [Fact]
    public void Create_WithParentId_ShouldNotBeRoot()
    {
        var nodeName = NodeName.Create("ChildNode");

        var node = Node.Create(nodeName, 1, 5, _now);

        node.ParentId.Should().Be(5);
        node.IsRoot.Should().BeFalse();
    }

    [Fact]
    public void Rename_WithValidName_ShouldUpdateName()
    {
        var nodeName = NodeName.Create("OldName");
        var node = Node.Create(nodeName, 1, null, _now);
        var newName = NodeName.Create("NewName");
        var modifiedAt = _now.AddMinutes(5);

        node.Rename(newName, modifiedAt);

        node.Name.Should().Be("NewName");
        node.ModifiedAt.Should().Be(modifiedAt);
    }

    [Fact]
    public void Create_WithZeroTreeId_ShouldThrow()
    {
        var nodeName = NodeName.Create("TestNode");

        var act = () => Node.Create(nodeName, 0, null, _now);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithNegativeTreeId_ShouldThrow()
    {
        var nodeName = NodeName.Create("TestNode");

        var act = () => Node.Create(nodeName, -1, null, _now);

        act.Should().Throw<ArgumentException>();
    }
}
