using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Valetax.Domain.Entities;
using Valetax.Domain.ValueObjects;
using Valetax.Infrastructure.Persistence;
using Valetax.Infrastructure.Persistence.Repositories;

namespace Valetax.Infrastructure.Tests.Persistence.Repositories;

public class TreeRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly TreeRepository _repository;
    private readonly DateTime _now = new(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);

    public TreeRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new TreeRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddTree()
    {
        var tree = Tree.Create(TreeName.Create("TestTree"), _now);

        await _repository.AddAsync(tree);
        await _context.SaveChangesAsync();

        var savedTree = await _context.Trees.FirstOrDefaultAsync();
        savedTree.Should().NotBeNull();
        savedTree!.Name.Should().Be("TestTree");
    }

    [Fact]
    public async Task GetByNameAsync_WhenTreeExists_ShouldReturnTree()
    {
        var tree = Tree.Create(TreeName.Create("FindMe"), _now);
        await _context.Trees.AddAsync(tree);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByNameAsync("FindMe");

        result.Should().NotBeNull();
        result!.Name.Should().Be("FindMe");
    }

    [Fact]
    public async Task GetByNameAsync_WhenTreeDoesNotExist_ShouldReturnNull()
    {
        var result = await _repository.GetByNameAsync("NonExistent");

        result.Should().BeNull();
    }

    [Fact]
    public async Task ExistsAsync_WhenTreeExists_ShouldReturnTrue()
    {
        var tree = Tree.Create(TreeName.Create("ExistingTree"), _now);
        await _context.Trees.AddAsync(tree);
        await _context.SaveChangesAsync();

        var exists = await _repository.ExistsAsync("ExistingTree");

        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_WhenTreeDoesNotExist_ShouldReturnFalse()
    {
        var exists = await _repository.ExistsAsync("NonExistent");

        exists.Should().BeFalse();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
