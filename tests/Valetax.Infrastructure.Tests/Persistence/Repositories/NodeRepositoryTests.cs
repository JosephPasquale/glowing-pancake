using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Valetax.Domain.Entities;
using Valetax.Domain.ValueObjects;
using Valetax.Infrastructure.Persistence;
using Valetax.Infrastructure.Persistence.Repositories;

namespace Valetax.Infrastructure.Tests.Persistence.Repositories;

public class NodeRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly NodeRepository _repository;
    private readonly DateTime _now = new(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);

    public NodeRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new NodeRepository(_context);
    }

    [Fact]
    public async Task GetByTreeIdAsync_ShouldReturnNodesForTree()
    {
        var tree = await CreateTreeWithNodes();

        var nodes = await _repository.GetByTreeIdAsync(tree.Id);

        nodes.Should().HaveCount(3);
        nodes.Should().OnlyContain(n => n.TreeId == tree.Id);
    }

    [Fact]
    public async Task HasChildrenAsync_WhenNodeHasChildren_ShouldReturnTrue()
    {
        var tree = await CreateTreeWithNodes();
        var parentNode = await _context.Nodes.FirstAsync(n => n.ParentId == null);

        var hasChildren = await _repository.HasChildrenAsync(parentNode.Id);

        hasChildren.Should().BeTrue();
    }

    [Fact]
    public async Task HasChildrenAsync_WhenNodeHasNoChildren_ShouldReturnFalse()
    {
        var tree = await CreateTreeWithNodes();
        var leafNode = await _context.Nodes.FirstAsync(n => n.Name == "Child2");

        var hasChildren = await _repository.HasChildrenAsync(leafNode.Id);

        hasChildren.Should().BeFalse();
    }

    [Fact]
    public async Task ExistsSiblingWithNameAsync_WhenSiblingExists_ShouldReturnTrue()
    {
        var tree = await CreateTreeWithNodes();

        var exists = await _repository.ExistsSiblingWithNameAsync(
            tree.Id, null, "Root");

        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsSiblingWithNameAsync_WhenSiblingDoesNotExist_ShouldReturnFalse()
    {
        var tree = await CreateTreeWithNodes();

        var exists = await _repository.ExistsSiblingWithNameAsync(
            tree.Id, null, "NonExistent");

        exists.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteWithDescendantsAsync_ShouldDeleteNodeAndChildren()
    {
        var tree = await CreateTreeWithNodes();
        var rootNode = await _context.Nodes.FirstAsync(n => n.ParentId == null);

        await _repository.DeleteWithDescendantsAsync(rootNode.Id);
        await _context.SaveChangesAsync();

        var remainingNodes = await _context.Nodes.Where(n => n.TreeId == tree.Id).ToListAsync();
        remainingNodes.Should().BeEmpty();
    }

    private async Task<Tree> CreateTreeWithNodes()
    {
        var tree = Tree.Create(TreeName.Create("TestTree"), _now);
        await _context.Trees.AddAsync(tree);
        await _context.SaveChangesAsync();

        var root = Node.Create(NodeName.Create("Root"), tree.Id, null, _now);
        await _context.Nodes.AddAsync(root);
        await _context.SaveChangesAsync();

        var child1 = Node.Create(NodeName.Create("Child1"), tree.Id, root.Id, _now);
        var child2 = Node.Create(NodeName.Create("Child2"), tree.Id, root.Id, _now);
        await _context.Nodes.AddRangeAsync(child1, child2);
        await _context.SaveChangesAsync();

        return tree;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
