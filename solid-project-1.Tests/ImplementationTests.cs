namespace solid_project_1.Tests;

/// <summary>
/// Tests to verify that implementation classes exist and implement the correct interfaces.
/// These tests help ensure you've created the implementation classes as specified.
/// </summary>
[TestFixture]
public class ImplementationTests
{
    [Test]
    public void TradeDataProvider_Class_Should_Exist()
    {
        var classType = Type.GetType("solid_project_1.TradeDataProvider, solid-project-1");

        Assert.That(classType, Is.Not.Null);
        Assert.That(classType.IsClass, Is.True);
        Assert.That(classType.IsAbstract, Is.False);
    }

    [Test]
    public void TradeDataProvider_Should_Implement_ITradeDataProvider()
    {
        var classType = Type.GetType("solid_project_1.TradeDataProvider, solid-project-1");
        var interfaceType = Type.GetType("solid_project_1.ITradeDataProvider, solid-project-1");

        Assert.That(classType, Is.Not.Null);
        Assert.That(interfaceType, Is.Not.Null);
        Assert.That(interfaceType.IsAssignableFrom(classType), Is.True);
    }

    [Test]
    public void TradeParser_Class_Should_Exist()
    {
        var classType = Type.GetType("solid_project_1.TradeParser, solid-project-1");

        Assert.That(classType, Is.Not.Null);
        Assert.That(classType.IsClass, Is.True);
        Assert.That(classType.IsAbstract, Is.False);
    }

    [Test]
    public void TradeParser_Should_Implement_ITradeParser()
    {
        var classType = Type.GetType("solid_project_1.TradeParser, solid-project-1");
        var interfaceType = Type.GetType("solid_project_1.ITradeParser, solid-project-1");

        Assert.That(classType, Is.Not.Null);
        Assert.That(interfaceType, Is.Not.Null);
        Assert.That(interfaceType.IsAssignableFrom(classType), Is.True);
    }

    [Test]
    public void TradeStorage_Class_Should_Exist()
    {
        var classType = Type.GetType("solid_project_1.TradeStorage, solid-project-1");

        Assert.That(classType, Is.Not.Null);
        Assert.That(classType.IsClass, Is.True);
        Assert.That(classType.IsAbstract, Is.False);
    }

    [Test]
    public void TradeStorage_Should_Implement_ITradeStorage()
    {
        var classType = Type.GetType("solid_project_1.TradeStorage, solid-project-1");
        var interfaceType = Type.GetType("solid_project_1.ITradeStorage, solid-project-1");

        Assert.That(classType, Is.Not.Null);
        Assert.That(interfaceType, Is.Not.Null);
        Assert.That(interfaceType.IsAssignableFrom(classType), Is.True);
    }

    [Test]
    public void TradeProcessor_Should_Not_Be_Static_Class()
    {
        var classType = Type.GetType("solid_project_1.TradeProcessor, solid-project-1");

        Assert.That(classType, Is.Not.Null);
        Assert.That(classType.IsClass, Is.True);

        var isStaticClass = classType is { IsAbstract: true, IsSealed: true };
        Assert.That(isStaticClass, Is.False, "TradeProcessor should not be a static class after refactoring");
    }

    [Test]
    public void TradeProcessor_Should_Have_Constructor_With_Dependencies()
    {
        var classType = Type.GetType("solid_project_1.TradeProcessor, solid-project-1");
        var tradeParserInterface = Type.GetType("solid_project_1.ITradeParser, solid-project-1");
        var tradeStorageInterface = Type.GetType("solid_project_1.ITradeStorage, solid-project-1");
        var tradeDataProviderInterface = Type.GetType("solid_project_1.ITradeDataProvider, solid-project-1");

        var constructor = classType?.GetConstructor([
            tradeParserInterface!,
            tradeStorageInterface!,
            tradeDataProviderInterface!
        ]);

        Assert.That(constructor, Is.Not.Null);
        Assert.That(constructor.GetParameters().Length, Is.EqualTo(3));
    }
}