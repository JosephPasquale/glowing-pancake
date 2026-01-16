using FluentAssertions;
using Valetax.Domain.Entities;
using Xunit;
using Valetax.Domain.Exceptions;
using Valetax.Domain.ValueObjects;

namespace Valetax.Domain.Tests.Entities;

public class TreeTests
{
    private readonly DateTime _now = new(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Create_WithValidName_ShouldCreateTree()
    {
        var treeName = TreeName.Create("TestTree");

        var tree = Tree.Create(treeName, _now);

        tree.Name.Should().Be("TestTree");
        tree.CreatedAt.Should().Be(_now);
        tree.ModifiedAt.Should().BeNull();
        tree.Nodes.Should().BeEmpty();
    }

    [Fact]
    public void AddNode_WithValidName_ShouldAddNodeToTree()
    {
        var tree = CreateTreeWithId(1);
        var nodeName = NodeName.Create("RootNode");

        var node = tree.AddNode(nodeName, null, _now);

        node.Should().NotBeNull();
        node.Name.Should().Be("RootNode");
        node.ParentId.Should().BeNull();
        tree.Nodes.Should().HaveCount(1);
        tree.ModifiedAt.Should().Be(_now);
    }

    [Fact]
    public void AddNode_WithDuplicateName_ShouldThrowException()
    {
        var tree = CreateTreeWithId(1);
        var nodeName = NodeName.Create("Node1");
        tree.AddNode(nodeName, null, _now);

        var duplicateName = NodeName.Create("Node1");

        var act = () => tree.AddNode(duplicateName, null, _now);

        act.Should().Throw<DuplicateNodeNameException>()
           .WithMessage("*Node1*");
    }

    [Fact]
    public void AddNode_WithSameNameDifferentParent_ShouldSucceed()
    {
        var tree = CreateTreeWithId(1);
        var parent1Name = NodeName.Create("Parent1");
        var parent1 = tree.AddNode(parent1Name, null, _now);
        SetNodeId(parent1, 1);

        var parent2Name = NodeName.Create("Parent2");
        var parent2 = tree.AddNode(parent2Name, null, _now);
        SetNodeId(parent2, 2);

        var childName = NodeName.Create("SameName");
        var child1 = tree.AddNode(childName, 1, _now);
        SetNodeId(child1, 3);

        var child2Name = NodeName.Create("SameName");
        var act = () => tree.AddNode(child2Name, 2, _now);

        act.Should().NotThrow();
        tree.Nodes.Should().HaveCount(4);
    }

    private Tree CreateTreeWithId(long id)
    {
        var treeName = TreeName.Create("TestTree");
        var tree = Tree.Create(treeName, _now);
        SetTreeId(tree, id);
        return tree;
    }

    private static void SetTreeId(Tree tree, long id)
    {
        var property = typeof(Tree).GetProperty("Id")!;
        property.SetValue(tree, id);
    }

    private static void SetNodeId(Node node, long id)
    {
        var property = typeof(Node).GetProperty("Id")!;
        property.SetValue(node, id);
    }
}
