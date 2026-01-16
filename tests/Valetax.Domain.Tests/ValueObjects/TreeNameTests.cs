using FluentAssertions;
using Valetax.Domain.ValueObjects;
using Xunit;

namespace Valetax.Domain.Tests.ValueObjects;

public class TreeNameTests
{
    [Fact]
    public void Create_WithValidName_ShouldCreateTreeName()
    {
        var treeName = TreeName.Create("ValidName");

        treeName.Value.Should().Be("ValidName");
    }

    [Fact]
    public void Create_WithWhitespace_ShouldTrim()
    {
        var treeName = TreeName.Create("  TrimmedName  ");

        treeName.Value.Should().Be("TrimmedName");
    }

    [Fact]
    public void Create_WithEmptyString_ShouldThrow()
    {
        var act = () => TreeName.Create("");

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithNull_ShouldThrow()
    {
        var act = () => TreeName.Create(null!);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithTooLongName_ShouldThrow()
    {
        var longName = new string('a', 101);

        var act = () => TreeName.Create(longName);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithMaxLengthName_ShouldSucceed()
    {
        var maxLengthName = new string('a', 100);

        var treeName = TreeName.Create(maxLengthName);

        treeName.Value.Should().HaveLength(100);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldBeEqual()
    {
        var name1 = TreeName.Create("Test");
        var name2 = TreeName.Create("Test");

        name1.Should().Be(name2);
        (name1 == name2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldNotBeEqual()
    {
        var name1 = TreeName.Create("Test1");
        var name2 = TreeName.Create("Test2");

        name1.Should().NotBe(name2);
        (name1 != name2).Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversion_ShouldReturnValue()
    {
        var treeName = TreeName.Create("Test");

        string value = treeName;

        value.Should().Be("Test");
    }
}
