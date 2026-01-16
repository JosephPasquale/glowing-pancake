using FluentAssertions;
using Valetax.Domain.ValueObjects;
using Xunit;

namespace Valetax.Domain.Tests.ValueObjects;

public class NodeNameTests
{
    [Fact]
    public void Create_WithValidName_ShouldCreateNodeName()
    {
        var nodeName = NodeName.Create("ValidNode");

        nodeName.Value.Should().Be("ValidNode");
    }

    [Fact]
    public void Create_WithWhitespace_ShouldTrim()
    {
        var nodeName = NodeName.Create("  TrimmedNode  ");

        nodeName.Value.Should().Be("TrimmedNode");
    }

    [Fact]
    public void Create_WithEmptyString_ShouldThrow()
    {
        var act = () => NodeName.Create("");

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithOnlyWhitespace_ShouldThrow()
    {
        var act = () => NodeName.Create("   ");

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithTooLongName_ShouldThrow()
    {
        var longName = new string('a', 101);

        var act = () => NodeName.Create(longName);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Equals_WithSameValue_ShouldBeEqual()
    {
        var name1 = NodeName.Create("Node");
        var name2 = NodeName.Create("Node");

        name1.Should().Be(name2);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        var nodeName = NodeName.Create("TestNode");

        nodeName.ToString().Should().Be("TestNode");
    }
}
